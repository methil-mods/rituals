using ScriptableObjects.Quest;
using System;
using Core.UserInterface.Quest;
using Action = Framework.Action.Action;

namespace Core.Actions
{
    [Serializable]
    public class EndQuestAction : Action
    {
        public QuestData questToEnd;
        
        public override void Execute()
        {
            QuestUserInterfaceController.Instance.FinishQuest(questToEnd);
        }
    }
}