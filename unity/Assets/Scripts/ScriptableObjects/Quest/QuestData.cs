using ScriptableObjects.Quest.Action;
using UnityEngine;

namespace ScriptableObjects.Quest
{
    public abstract class QuestData : ScriptableObject
    {
        public string questName;
        [TextArea]
        public string questDescription;

        [SerializeReference, SubclassSelector]
        public QuestAction[] onQuestStartActions;
    }
}