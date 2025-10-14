using System;
using System.Collections.Generic;
using ScriptableObjects.Quest;
using UnityEngine;

namespace ScriptableObjects.Book
{
    [CreateAssetMenu(fileName = "BookData", menuName = "Books/BookData")]
    public class BookData : ScriptableObject
    {
        public string bookName;
        public List<BookPage> bookContent;
    }

    [Serializable]
    public class BookPage
    {
        [TextArea]
        public string contentLeft;
        public Sprite contentLeftImage;
        [TextArea]
        public string contentRight;
        public Sprite contentRightImage;
        
        // ! This can be nullable
        public QuestData unlockableQuestData;
    }
}