using Core.UserInterface;
using ScriptableObjects.Book;
using UnityEngine;

namespace Core.Props
{
    public class Bookshelf : MonoBehaviour
    {
        public BookData bookData;

        public void OpenBook()
        {
            BookUserInterfaceController.Instance.OpenBook(bookData);
        }
    }
}