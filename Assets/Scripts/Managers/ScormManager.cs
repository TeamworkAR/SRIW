using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using Scorm;
using Unity.VisualScripting;
using UnityEngine;
using Version = Scorm.Version;

public class ScormManager : SingletonBehaviour<ScormManager>
{
    private const string SuspendPrimaryFieldSeparator = "|";
    private const string SuspendPrimaryKeyValSeparator = ":";
    private const string SuspendValueListSeparator = ",";
    private const int MaxSuspendCharactersScorm2004 = 64000;
    private const int MaxSuspendCharactersScorm12 = 4096;
    private const int InitialSuspendCapacity = 256;

    //TODO: Custom editor to connect this with SCORMPublishSettings scormVersion to avoid mismatch
    [SerializeField] private Version scormVersion;
    [SerializeField] private bool useLargeSuspendDataLimit = false;
    
    private IScormService _scormService;
    private float _startTime;
    private Dictionary<string, string> _suspendDataKeyVal = new Dictionary<string, string>();
    private List<ScormSessionWarning> _sessionWarnings = new List<ScormSessionWarning>();
    private bool _initialized = false;
    
    //SCORM Model elements
    private EntryType _entryType;
    private LessonMode _lessonMode;
    private CreditType _creditType;
    private LessonStatus _status;
    private string _studentId;
    private string _studentName;
    private string _locale;
    private string _bookmark;
    private string _suspendString;
    private int _totalTime;
    private int _sessionTime;
    
    /// <summary>
    /// This action is dispatched when the ScormManager has been initialized and communications have been successfully
    /// established.
    /// </summary>
    public event Action ScormManagerReady;

    /// <summary>
    /// Whether or not the ScormManager has been initialized.
    /// </summary>
    public bool Initialized => _initialized;
    
    protected override void Awake()
    {
        base.Awake();
        
        #if UNITY_EDITOR
            _scormService = new ScormPlayerPrefsService();
        #else
            _scormService = new ScormService();
        #endif
        
        if (!_scormService.Initialize(scormVersion))
        {
            LogSessionWarning("Init Error",
                $"There was an error during scorm initialization ({_scormService.Version}).");
            return;
        }

        if (scormVersion == Version.Scorm_1_2 && useLargeSuspendDataLimit)
        {
            LogSessionWarning("Suspend Data Limit Overridden: ",
                $"Normally, {Version.Scorm_1_2} normally only supports {MaxSuspendCharactersScorm12} in suspend " +
                $"data, however, this limit is currently set to {MaxSuspendCharactersScorm2004}. Some LMS' may support this, " +
                "but if not, suspend data longer than 4096 characters will be truncated, and could lead to unexpected behaviour.");
        }
        
        _initialized = StartCommunicationSession();
        
        if (!_initialized)
        {
            LogSessionWarning("Scorm Error", $"There was an error retrieving scorm data.");
            return;
        }
        
        Debug.Log($"Scorm communication initialized ({_scormService.Version}).");
        
        ScormManagerReady?.Invoke();
    }

    /// <summary>
    /// Begins communication with the Scorm LMS. 
    /// </summary>
    /// <returns> False if the _scormService cannot be communicated with, true otherwise</returns>
    /// <remarks> This is a simple implementation that <b>does NOT</b> validate feature availability for non scorm-mandatory
    /// features.
    /// TODO: Implement feature compatibility checks when/if this tool is scaled beyond it's initial use-case
    /// </remarks>
    private bool StartCommunicationSession()
    {
        if (_scormService is { IsConnected: false }) return false;
        
        _studentId = _scormService.GetLearnerId();
        _studentName = _scormService.GetLearnerName();
        //_locale = _scormService.GetLanguage(); // NOTE: GetLanguage only returns "en" for the ScormPlayerPrefsService when testing locally, 
        _locale = _scormService.GetStringValue("cmi.student_preference.language");
        _bookmark = _scormService.GetLessonLocation();
        _status = _scormService.GetLessonStatus();
        _lessonMode = _scormService.GetLessonMode();
        _creditType = _scormService.GetCredit();
        _totalTime = _scormService.GetTotalTime();
        _suspendString = _scormService.GetSuspendData();
        _suspendDataKeyVal = UnEncodeKeyValuesFromSuspendString(_suspendString);
        
        _startTime = Time.time;
        
        // SCORM Quirk. When the course is launched for the first time, it will be opened with a status of
        // LessonStatus.NotAttempted. If the course does not explicitly set a different status, the LMS will automatically
        // complete the course.
        // See: https://xml.coverpages.org/SCORM-12-RunTimeEnv.pdf,
        // "cmi.core.lesson_status" -> "LMS Behavior", specifically "Additional Behavior Requirements" page 3-24 / 3-25
        if (_status == LessonStatus.NotAttempted)
        {
            SetCourseCompletion(false);
        }
        
        return true;
    }
    
    /// <summary>
    /// Writes and commits all scorm data tracked within the given session.
    /// </summary>
    /// <returns>False in the event of an error, true otherwise.</returns>
    private bool PersistAllScormData()
    {
        if (_scormService is not { IsConnected: true }) return false;
        Locale = _locale;
        
        var hasError = !_scormService.SetLessonStatus(_status);
        hasError = hasError || !_scormService.SetSessionTime(SessionTime);
        hasError = hasError || !_scormService.SetExitReason(ExitReason.Suspend);
        hasError = hasError || !WriteSuspendKeyVals();
        hasError = hasError || !_scormService.Commit();
        
        return !hasError;
    }

    /// <summary>
    /// The time the student has spent in this learning session.
    /// </summary>
    public int SessionTime => (int)((Time.time - _startTime) * 1000);
    
    /// <summary>
    /// The total time the student has spent in this course, not including the current session.
    /// </summary>
    public int TotalTime => _totalTime;
    
    /// <summary>
    /// A string representing the locale selected by the student.
    /// </summary>
    public string Locale
    {
        get => _locale;
        set
        {
            _locale = value;
            _scormService.SetValue("cmi.student_preference.language", _locale);
            _scormService.Commit();
        }
    }
    
    /// <summary>
    /// A string representing the location the student should resume the course from.
    /// </summary>
    public string Bookmark
    {
        get => _bookmark;
        set
        {
            _bookmark = value;
            _scormService.SetLessonLocation(_bookmark);
            _scormService.Commit();
        }
    }
    
    /// <summary>
    /// Stores a custom key/value (string,string) pair in suspend data.
    /// </summary>
    /// <remarks>
    /// Note that the following characters should be avoided in the key and value:
    /// <list type="bullet">
    /// <item> <see cref="ScormManager.SuspendPrimaryFieldSeparator"/> </item>
    /// <item> <see cref="ScormManager.SuspendPrimaryKeyValSeparator"/> </item>
    /// </list>
    /// </remarks>
    /// <param name="key">The key used to retrieve this custom string from suspend data.</param>
    /// <param name="value">The custom string data to store in suspend data.</param>
    /// <returns>True if the custom key/value data were written and committed to suspend data, otherwise false.</returns>
    public bool StoreCustomData(string key, string value)
    {
        Debug.Log($"Attempting to store: {key}{SuspendPrimaryKeyValSeparator}{value}");
        if (key.Contains(SuspendPrimaryFieldSeparator) || key.Contains(SuspendPrimaryKeyValSeparator) ||
            value.Contains(SuspendPrimaryFieldSeparator) || value.Contains(SuspendPrimaryKeyValSeparator))
        {
            LogSessionWarning("Suspend Data Issue", "Key or Value contains special characters which are used in the suspend encoding process. Cannot store.");
            return false;
        }
        
        Debug.Log(_suspendDataKeyVal.Keys.Contains(key) ? $"Overwriting data for {key}" : $"Adding new data: {key}");
        _suspendDataKeyVal[key] = value;
        return WriteSuspendKeyVals();
    }

    public bool StoreCustomData(string key, in List<string> value)
    {
        var valueString = string.Join(SuspendValueListSeparator, value);
        return StoreCustomData(key, valueString);
    }
    
    /// <summary>
    /// Attempts to retrieve a string value for a given key stored within suspend data
    /// </summary>
    /// <param name="key">The string representing which suspend data entry to retrieve</param>
    /// <returns>The retrieved string if found, or an empty string in the case of failure to find a value for the key.</returns>
    public string GetCustomString(string key)
    {
        var success = _suspendDataKeyVal.TryGetValue(key, out var value);
        if (success) return value;
        LogSessionWarning("Cannot get custom string",$"Error retrieving custom string for key {key}");
        return string.Empty;

    }

    /// <summary>
    /// Attempts to retrieve a List&lt;String&gt; for a given key stored within suspend data
    /// </summary>
    /// <param name="key">The string representing which suspend data entry to retrieve</param>
    /// <returns>The retrieved List&lt;String&gt; if found, or an empty string in the case of failure to find a value for the key.</returns>
    public List<string> GetCustomList(string key)
    {
        var valueString = GetCustomString(key);
        return new List<string>(valueString.Split(SuspendValueListSeparator));
    }
    /// <summary>
    /// A readonly List of strings representing the keys stored within suspend data.
    /// </summary>
    public IReadOnlyList<string> CustomDataKeys => (IReadOnlyList<string>)_suspendDataKeyVal.Keys.AsReadOnlyList();
    
    /// <summary>
    /// Sets the key/value pairs stored with <see cref="StoreCustomData"/> into suspend data and commits
    /// the suspend string.
    /// </summary>
    /// <returns>True if encoding, writing, and commiting are successful, false otherwise</returns>
    private bool WriteSuspendKeyVals()
    {
        var hasError = false;
        var previousSuspend = _suspendString;
        var tmpSuspend = GetEncodedSuspendDataString();
        hasError = tmpSuspend == string.Empty;
        hasError = hasError || !_scormService.SetSuspendData(tmpSuspend);
        hasError = hasError || !_scormService.Commit();

        if (hasError)
        {
            _suspendString = previousSuspend;
            _suspendDataKeyVal = UnEncodeKeyValuesFromSuspendString(previousSuspend);
            LogSessionWarning("Suspend Data Error", "Error encoding, writing, or committing to suspend data.");
            return false;
        }
        
        _suspendString = tmpSuspend;
        return true;
    }
    
    /// <summary>
    /// Returns key/value pairs as Dictionary&lt;string,string&gt; from a previously encoded suspend string.
    /// <seealso cref="GetEncodedSuspendDataString"/>
    /// <seealso cref="SuspendPrimaryFieldSeparator"/>
    /// <seealso cref="SuspendPrimaryKeyValSeparator"/>
    /// </summary>
    /// <param name="suspendString">The previously encoded suspend string.</param>
    /// <returns>A Dictionary&lt;string,string&gt; representing key/value pairs stored within the encoded suspend string</returns>
    private Dictionary<string,string> UnEncodeKeyValuesFromSuspendString(string suspendString)
    {
        Dictionary<string, string> rtn = new();
        foreach (var suspendField in suspendString.Split(SuspendPrimaryFieldSeparator))
        {
            var data = suspendField.Split(SuspendPrimaryKeyValSeparator);
            if (data.Length > 1)
            {
                rtn.Add(data[0], data[1]);
            }
        }

        return rtn;
    }
    
    /// <summary>
    /// Encodes the suspend data key/value pairs in preparation for saving.
    /// </summary>
    /// <returns>An encoded string that is ready to be committed in suspend_data, or an empty string in the case of failure</returns>
    private string GetEncodedSuspendDataString()
    {
        var sb = new StringBuilder(InitialSuspendCapacity,
            useLargeSuspendDataLimit ? MaxSuspendCharactersScorm2004 : MaxSuspendCharactersScorm12);
        
        foreach(var (key, value) in _suspendDataKeyVal)
        {
            try
            {
                sb.Append($"{key}{SuspendPrimaryKeyValSeparator}{value}{SuspendPrimaryFieldSeparator}");
            }
            catch (OutOfMemoryException)
            {
                LogSessionWarning("Suspend Data Warning", "Value not written - suspend data maximum length would be exceeded when writing the value for {key}.  - your data will not be suspended.");
                return string.Empty;
            }
        }
        return sb.ToString();
    }
    
    /// <summary>
    /// Sets the <see cref="LessonStatus"/> for the course.
    /// Takes <see cref="LessonMode"/> and <see cref="CreditType"/> into consideration.
    /// </summary>
    /// <param name="isComplete">Whether the course should be considered complete or not.</param>
    /// <returns>True if data was set successfully, false if not</returns>
    /// <remarks> This is a basic implementation that:
    /// <list type="bullet">
    /// <item>
    /// <b>Does</b> follow behavioral expectations set forward re <see cref="CreditType"/> and <see cref="LessonMode"/>
    /// in https://xml.coverpages.org/SCORM-12-RunTimeEnv.pdf, "cmi.core.lesson_status" -> "Usage 3)" page 3-24
    /// </item>
    /// <item> <b>Does Not</b> prevent a user from overwriting a completion</item>
    /// <item> <b>Does Not</b> handle Passed/Fail/Unknown status at present</item>
    /// </list>
    /// </remarks>
    public bool SetCourseCompletion(bool isComplete)
    {
        var status = _scormService.GetLessonStatus();
        
        if (_creditType == CreditType.Credit)
        {
            status = _lessonMode switch
            {
                LessonMode.Browse => LessonStatus.Browsed,
                LessonMode.Normal => isComplete ? LessonStatus.Completed : LessonStatus.Incomplete,
                _ => status
            };
        }
        else
        {
            status = LessonStatus.Browsed;
        }
        
        _status = status;
        return _scormService.SetLessonStatus(_status);
    }

    /// <summary>
    /// Terminates the communication session with the LMS.
    /// Does not set course status, which can be set instead using <see cref="ScormManager.SetCourseCompletion"/>
    /// </summary>
    /// <param name="preventCloseOnError">Default: <b>true</b><br/>When true, will prevent completion of the close action. If an error is found
    /// when committing data, we will not finish communication with the LMS or close the window. If an error is found when
    /// closing the connection to the LMS, we will not close the window.</param>
    /// <param name="closeWithoutSaving">Default: <b>false</b><br/>When true, closes the course window without committing scorm data.
    /// Note: at present, many calls persist scorm data at the time they are made, so, the amount of data left uncommitted
    /// at point of closure will likely be minimal.</param>
    /// <returns>True if data is committed and scorm communication is successfully terminated, false otherwise</returns>
    public bool CloseCourse(bool preventCloseOnError = true, bool closeWithoutSaving = false)
    {
        if (_scormService.IsConnected)
        {
            if (!closeWithoutSaving)
            {
                if (!PersistAllScormData())
                {
                    LogSessionWarning("Close Error","Error committing scorm data");
                    if (preventCloseOnError) return false;
                }
            }

            if (!_scormService.Finish())
            {
                LogSessionWarning("Close Error","Error terminating communication");
                if (preventCloseOnError) return false;
            }
            Debug.Log("Communication terminated");
        }

        Application.Quit();
        
        return true;
    }
    
    /// <summary>
    /// Completes the course, saves, and then ends the communication session with the LMS.
    /// </summary>
    /// <remarks>
    /// TODO: It may be prudent to rename this method to avoid confusion with SCORM's "LMSFinish" which only ends communication and does not set completion for the course
    /// </remarks>
    public void FinishCourse()
    {
        SetCourseCompletion(true);
        CloseCourse();
    }

    private void LogSessionWarning(string title, string description)
    {
        Debug.LogWarning($"<color='orange'>{title}</color>: {description}");
        _sessionWarnings.Add(new ScormSessionWarning(title, description));
    }
    
    /// <summary>
    /// Outputs a string which is useful for debugging purposes that contains all pertinent scorm data, warnings, etc.
    /// </summary>
    /// <param name="useRichText">Whether or not to insert rich text formatting into the debug string. Useful in the
    /// context of the Unity Editor.</param>
    /// <param name="printRawSuspend">Whether or not to include the raw suspendData value in the debug string.</param>
    /// <returns>The debug string</returns>
    public string GetDebugString(bool useRichText = false, bool printRawSuspend = false)
    {
        string titleStart = "";
        string titleEnd = "";
        string valueStart = "";
        string valueEnd = "";
        string warnStart = "";
        string warnEnd = "";
        
        var newline = Environment.NewLine;
        var sb = new StringBuilder();
        var warningsBuilder = new StringBuilder();
        var warnString = "";
        var debugHint = Application.isEditor? " (Click to Expand)" : "";
        if (useRichText)
        {
            titleStart = "<b><color=green>";
            titleEnd = "</color></b>";
            valueStart = "<color=white>";
            warnStart = "<color=orange>";
            valueEnd = warnEnd = "</color>";
        }
        
        sb.Append(newline);
        sb.Append($"ℹ------{titleStart}SCORM Session Data{debugHint}{titleEnd}------ℹ");
        sb.Append(newline);
        sb.Append($"| Entry: {valueStart}{_entryType}{valueEnd}  ");
        sb.Append($"LessonMode: {valueStart}{_lessonMode}{valueEnd}  ");
        sb.Append($"Credit {valueStart}{_creditType}{valueEnd}");
        sb.Append(newline);
        sb.Append($"| Locale {valueStart}{_locale}{valueEnd}");
        sb.Append(newline);
        sb.Append($"| Status: {valueStart}{_status}{valueEnd}  ");
        sb.Append($"Total Time: {valueStart}{_totalTime}{valueEnd}  ");
        sb.Append($"Current Session Time: {valueStart}{SessionTime}{valueEnd}  ");
        sb.Append($"Expected Total Time Next Launch: {valueStart}{_totalTime + SessionTime}{valueEnd}");
        sb.Append(newline);
        sb.Append($"| Bookmark: {valueStart}{_bookmark}{valueEnd}");
        sb.Append(newline);
        sb.Append($"| Suspend Data: {newline}");
        foreach (var key in CustomDataKeys)
        {
            sb.Append($"|  {titleStart}{key}{titleEnd}: {valueStart}{GetCustomString(key)}{valueEnd}{newline}");
        }
        if(printRawSuspend) sb.Append($"| Raw Suspend String: {newline}{valueStart}{_suspendString}{valueEnd}");
        
        //Session Warnings
        foreach (var warning in _sessionWarnings)
        {
            warningsBuilder.Append($"| {warnStart}*{warnEnd} {warning.Time:T} {warning.Title} {warning.Description}");
        }
        
        warnString = warningsBuilder.ToString();
        
        if (warnString.Length > 0)
        {
            sb.Append($"| {warnStart}⚠ Warnings{warnEnd}:{newline}");
            sb.Append(warningsBuilder.ToString());
        }
        else
        {
            sb.Append($"| {valueStart}✔ No Warnings{valueEnd}{newline}");
        }
        
        sb.Append($"{newline}------------------------------");
        sb.Append(newline);
        
        return sb.ToString();
    }
}

public struct ScormSessionWarning
{
    public DateTime Time
    {
        get;
        private set;
    }
    
    public string Title
    {
        get;
        private set;
    }

    public string Description
    {
        get;
        private set;
    }
    public ScormSessionWarning(string title, string description)
    {
        Title = title;
        Description = description;
        Time = DateTime.Now;
    }
}