using System;
using Core.UserInterface.Quest;
using ScriptableObjects.Quest;
using Action = Framework.Action.Action;

namespace Core.Actions
{
    [Serializable]
    public class AddQuestAction : Action
    {
        public QuestData questData;   
        
        public override void Execute()
        {
            QuestUserInterfaceController.Instance.AddQuest(questData);
        }
    }
}