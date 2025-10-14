using ScriptableObjects.Quest;
using System;
using Core.UserInterface;

namespace Core.Dialog.Action
{
    [Serializable]
    public class EndQuestAction : DialogAction
    {
        public QuestData questToEnd;
        
        public override void Execute()
        {
            QuestUserInterfaceController.Instance.FinishQuest(questToEnd);
        }
    }
}