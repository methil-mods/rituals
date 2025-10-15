using System;
using System.Collections.Generic;
using ScriptableObjects.Quest;
using UnityEngine;
using Action = Framework.Action.Action;

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
        [TextArea(3, 20)]
        public string contentLeft;
        public Sprite contentLeftImage;
        [TextArea(3, 20)]
        public string contentRight;
        public Sprite contentRightImage;

        [SubclassSelector, SerializeReference] public Action[] onPageOpen;
    }
}