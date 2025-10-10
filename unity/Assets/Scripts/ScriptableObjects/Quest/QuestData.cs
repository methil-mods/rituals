using UnityEngine;

namespace ScriptableObjects.Quest
{
    [CreateAssetMenu(fileName = "QuestData", menuName = "Quests/QuestData")]
    public class QuestData : ScriptableObject
    {
        public string questName;
        [TextArea]
        public string questDescription;
    }
}