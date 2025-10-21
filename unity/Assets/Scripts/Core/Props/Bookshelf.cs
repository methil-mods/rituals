using Core.UserInterface.Book;
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

        protected override bool CanInteract()
        {
            return true;
        }

        public override void Interact()
        {
            base.Interact();
            OpenBook();
        }
    }
}