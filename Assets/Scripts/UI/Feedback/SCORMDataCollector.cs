using System.Text;
using AeLa.EasyFeedback;

namespace UI.Feedback
{
    public class SCORMDataCollector : FormElement
    {
        protected override void FormClosed()
        {
        }

        protected override void FormOpened()
        {
        }
    
        protected override void FormSubmitted()
        {
            // attach SCORM Debug String
            var bytes = Encoding.ASCII.GetBytes(ScormManager.Instance.GetDebugString(false));
            Form.CurrentReport.AttachFile("scormDebug.txt", bytes);
        }
    }
}
