using System.Collections.Generic;
using Framework.Controller;
using ScriptableObjects.Book;
using ScriptableObjects.Quest;
using UnityEngine;

namespace Core.UserInterface
{
    public class QuestUserInterfaceController : BaseController<QuestUserInterfaceController>
    {
        public List<QuestData> currentQuests = new List<QuestData>();
        public List<QuestData> finishedQuests = new List<QuestData>();

        void Start()
        {
            BookUserInterfaceController.Instance.OnPageChanged += CheckPageBookForAQuest;
        }

        private void CheckPageBookForAQuest(BookPage pageBefore, BookPage newPage, BookData bookData)
        {
            if (newPage.unlockableQuestData == null) return;
            AddQuest(quest: newPage.unlockableQuestData);
        }

        public bool AddQuest(QuestData quest)
        {
            if (quest == null) return false;
            if (finishedQuests.Contains(quest)) return false;
            if (currentQuests.Contains(quest)) return false;

            currentQuests.Add(quest);
            return true;
        }

        public void FinishQuest(QuestData quest)
        {
            if (currentQuests.Remove(quest))
                finishedQuests.Add(quest);
        }
        
        [ContextMenu("Reset Quests")]
        private void ResetQuests()
        {
            currentQuests.Clear();
            finishedQuests.Clear();
        }
    }
}