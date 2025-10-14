using Player;
using UnityEngine;
using UnityEngine.Events;
using World;
using UnityEngine.InputSystem;

namespace Framework.Props
{
    public abstract class Interactible<T> : MonoBehaviour where T : Interactible<T>
    {
        public UnityAction<T> OnInteract;
        
        [SerializeField] private Vector3Int detectionTileOffset = Vector3Int.zero;
        [SerializeField] private GameObject interactionIndicator;
        
        private Vector3Int cellPos;
        private InputSystem_Actions inputActions;

        void Awake()
        {
            inputActions = new InputSystem_Actions();
        }

        void OnEnable()
        {
            inputActions.Player.Enable();
        }

        void OnDisable()
        {
            inputActions.Player.Disable();
        }

        void Start()
        {
            this.cellPos = WorldController.Instance.WorldToCell(transform.position);
        }
        
        void Update()
        {
            Vector3 playerPos = PlayerController.Instance.transform.position;
            Vector3Int playerCellPos = WorldController.Instance.WorldToCell(playerPos);
            bool isOnDetectionCell = (this.cellPos + this.detectionTileOffset) - playerCellPos == Vector3Int.zero;
            if (!isOnDetectionCell)
            {
                SetInteractionIndicator(false);
                return;
            }
            SetInteractionIndicator(true);
            if (inputActions.Player.Interact.WasPerformedThisFrame())
                this.Interact();
        }
        
        public void Interact()
        {
            SetInteractionIndicator(false);
            OnInteract?.Invoke((T)this);
        }

        void SetInteractionIndicator(bool state)
        {
            interactionIndicator.SetActive(state);
        }
        
        void OnDrawGizmos()
        {
            if (WorldController.Instance == null) return;

            Vector3 worldPos = WorldController.Instance.worldGrid.CellToWorld(this.cellPos + detectionTileOffset);
            worldPos += new Vector3(0, 0.3f, 0);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(worldPos, 0.25f);
        }
    }
}