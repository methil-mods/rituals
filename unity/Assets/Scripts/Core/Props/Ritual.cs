using Core.UserInterface.Ritual;
using Framework.Props;
using Player;
using ScriptableObjects.Ritual;
using UnityEngine;
using World;

namespace Core.Props
{
    public class Ritual : Interactible<Ritual>
    {
        private Vector3Int cellPos;

        [SerializeField] private Vector3Int detectionTileOffset = Vector3Int.zero;

        void Start()
        {
            this.cellPos = WorldController.Instance.WorldToCell(transform.position);
        }

        public void StartRitual()
        {
            RitualUserInterfaceController.Instance.OpenRitualPanel();
        }

        public new void Interact()
        {
            base.Interact();
            StartRitual();
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