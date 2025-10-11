using Core.UserInterface;
using Framework.Props;
using ScriptableObjects.Book;
using UnityEngine;

namespace Core.Props
{
    public class Bookshelf : Interactible<Bookshelf>
    {
        public BookData bookData;

        public void OpenBook()
        {
            BookUserInterfaceController.Instance.OpenBook(bookData);
        }

        public new void Interact()
        {
            base.Interact();
            OpenBook();
        }
    }
}