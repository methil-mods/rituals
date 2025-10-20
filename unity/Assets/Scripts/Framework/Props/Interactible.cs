using Core.Game;
using Player;
using ScriptableObjects.Input;
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
        
        void OnEnable()
        {
            InputDatabase.Instance.interactAction.action.Enable();
        }

        void OnDisable()
        {
            InputDatabase.Instance.interactAction.action.Disable();
        }

        protected void Start()
        {
            this.cellPos = WorldController.Instance.worldGrid.WorldToCell(transform.position);
            InputDatabase.Instance.interactAction.action.performed += CallbackInteraction;
        }
        
        void Update()
        {
            if (!IsPlayerDetected() || !CanInteract())
            {
                SetInteractionIndicator(false);
                return;
            }
            SetInteractionIndicator(true);
        }

        private void CallbackInteraction(InputAction.CallbackContext context)
        {
            if (!PauseController.Instance.isPaused && IsPlayerDetected() && CanInteract())
            {
                this.Interact();
            }
        }

        private bool IsPlayerDetected()
        {
            Vector3 playerPos = PlayerController.Instance.transform.position;
            Vector3Int playerCellPos = WorldController.Instance.worldGrid.WorldToCell(playerPos);
            bool isOnDetectionCell = (this.cellPos + this.detectionTileOffset) - playerCellPos == Vector3Int.zero;
            return isOnDetectionCell;
        }

        protected abstract bool CanInteract();
        
        public virtual void Interact()
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