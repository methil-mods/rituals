using Core.UserInterface;
using Framework.Props;
using Player;
using ScriptableObjects.Book;
using UnityEngine;
using World;

namespace Core.Props
{
    public class BookStand : Interactible<BookStand>
    {
        public BookData bookData;

        public void OpenBook()
        {
            if (!bookData)
            {
                Debug.LogWarning($"No book data found on tile ${gameObject.name}");
                return;
            }
            BookUserInterfaceController.Instance.OpenBook(bookData);
        }

        public override void Interact()
        {
            base.Interact();
            OpenBook();
        }
    }
}