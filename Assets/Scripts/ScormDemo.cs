using Scorm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScormDemo : MonoBehaviour
{
    private IScormService scormService;

    public delegate void LogHandler(string text);
    public event LogHandler OnMessageLogged;

    [SerializeField]
    TMP_InputField inputField;


    void Start()
    {


        
    }

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitScorm()
    {
#if UNITY_EDITOR
        scormService = new ScormPlayerPrefsService();
#else
            scormService = new ScormService();
#endif

        Version version = Version.Scorm_1_2;
        bool result = scormService.Initialize(version);

        if (result)
            Log("Communication initialized (Scorm " + (version == Version.Scorm_1_2 ? "1.2" : "2004") + ").");
        else
            Log("There was an error during initialization (Scorm " + (version == Version.Scorm_1_2 ? "1.2" : "2004") + ").");
    }

    public void GetLearnerData()
    {
        string learnerID = scormService.GetLearnerId();
        string learnerName = scormService.GetLearnerName();

        Log("Learner ID = " + learnerID + ", Learner Name: " + learnerName);
    }
    public void GetSuspendData()
    {
        string value = scormService.GetSuspendData();
        Log("SuspendData: " + value);
    }
    public void SetSuspendData()
    {
        string data = inputField.text;

        Log("Setting suspended data: " + data);

        scormService.SetSuspendData(data);
        scormService.Commit();

        inputField.text = "";
    }
    public void FinishCourse()
    {
        if (scormService.Finish())
            Log("Communication terminated");
    }




        public void Log(string text)
    {
        string message = string.Format("[Scorm] - {0}", text);

        Debug.Log(message);

        OnMessageLogged?.Invoke(message);
    }

}
