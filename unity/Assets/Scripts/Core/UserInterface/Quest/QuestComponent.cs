using ScriptableObjects.Quest;
using TMPro;
using UnityEngine;

namespace Core.UserInterface
{
    public class QuestComponent : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;

        public void SetQuest(QuestData questData)
        {
            title.text = questData.questName;
            description.text = questData.questDescription;
        }
    }
}