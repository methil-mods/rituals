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

        private Vector3Int cellPos;

        [SerializeField] private Vector3Int detectionTileOffset = Vector3Int.zero;

        void Start()
        {
            this.cellPos = WorldController.Instance.WorldToCell(transform.position);
        }

        public void OpenBook()
        {
            BookUserInterfaceController.Instance.OpenBook(bookData);
        }

        public new void Interact()
        {
            base.Interact();
            OpenBook();
        }

        void Update()
        {
            Vector3 playerPos = PlayerController.Instance.transform.position;
            Vector3Int playerCellPos = WorldController.Instance.WorldToCell(playerPos);
            bool isOnDetectionCell = (this.cellPos + this.detectionTileOffset) - playerCellPos == Vector3Int.zero;
            if (!isOnDetectionCell) return;
            this.Interact();
        }
    }
}