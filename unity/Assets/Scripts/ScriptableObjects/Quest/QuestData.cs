using Framework.Action;
using UnityEngine;

namespace ScriptableObjects.Quest
{
    public abstract class QuestData : ScriptableObject
    {
        public string questName;
        [TextArea]
        public string questDescription;

        [SerializeReference, SubclassSelector]
        public Action[] onQuestStartActions;
        
        [SerializeReference, SubclassSelector]
        public Action[] onQuestFinishActions;
    }
}