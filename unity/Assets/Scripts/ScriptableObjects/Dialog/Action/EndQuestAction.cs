using ScriptableObjects.Quest;
using System;

namespace Core.Dialog.Action
{
    [Serializable]
    public class EndQuestAction : DialogAction
    {
        public QuestData questToEnd;
        
        public override void Execute()
        {
            // TODO : Implémenter la con de logique
        }
    }
}