using System;
using DialogueSystem;

namespace NodeEditor.Actions
{
    [Serializable]
    public class StopDialogueAction : Action
    {
        public override void Execute()
        {
            DialogueManager.Instance.EndDialogue();
        }
    }
}