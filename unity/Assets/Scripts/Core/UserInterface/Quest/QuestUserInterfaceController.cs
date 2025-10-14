using System.Collections.Generic;
using Framework.Action;
using Framework.Controller;
using ScriptableObjects.Book;
using ScriptableObjects.Quest;
using UnityEngine;

namespace Core.UserInterface.Quest
{
    public class QuestUserInterfaceController : BaseController<QuestUserInterfaceController>
    {
        public List<QuestData> currentQuests = new List<QuestData>();
        public List<QuestData> finishedQuests = new List<QuestData>();
        public GameObject questPanel;
        public GameObject questPrefab;

        void Start()
        {
            UpdateQuestUserInterface();
        }

        private void UpdateQuestUserInterface()
        {
            if (currentQuests.Count == 0)
            {
                questPanel.SetActive(false);
                return;
            }

            questPanel.SetActive(true);

            foreach (Transform child in questPanel.transform)
                Destroy(child.gameObject);

            foreach (QuestData quest in currentQuests)
            {
                GameObject questGO = Instantiate(questPrefab, questPanel.transform);
                QuestComponent uiItem = questGO.GetComponent<QuestComponent>();
                if (uiItem != null)
                    uiItem.SetQuest(quest);
            }
        }

        public bool AddQuest(QuestData quest)
        {
            if (quest == null) return false;
            if (finishedQuests.Contains(quest)) return false;
            if (currentQuests.Contains(quest)) return false;

            currentQuests.Add(quest);
            foreach (Action action in quest.onQuestStartActions) action.Execute();
            UpdateQuestUserInterface();
            return true;
        }

        public void FinishQuest(QuestData quest)
        {
            if (currentQuests.Remove(quest))
            {
                foreach (Action action in quest.onQuestFinishActions) action.Execute();
                finishedQuests.Add(quest);
                UpdateQuestUserInterface();
            }
        }
        
        [ContextMenu("Reset Quests")]
        private void ResetQuests()
        {
            currentQuests.Clear();
            finishedQuests.Clear();
        }
    }
}