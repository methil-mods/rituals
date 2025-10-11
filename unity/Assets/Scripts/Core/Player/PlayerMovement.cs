using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using World;

namespace Player
{
    [Serializable]
    public class PlayerMovement : Updatable<PlayerController>
    {
        public WorldController worldController;

        private Vector3 lastClickTilePos;
        private Vector3 lastWorldClickPos;

        [SerializeField]
        private bool debug = false;

        public override void Update(PlayerController controller)
        {
            if (!Mouse.current.leftButton.isPressed) return;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldClickPos = Camera.main.ScreenToWorldPoint(mousePos);
            
            Vector3 tilePos = worldController.WorldToCellCenter(worldClickPos);

            if (worldController.IsCellMovable(tilePos))
                controller.transform.position = tilePos;

            if (!debug) return;
            lastClickTilePos = tilePos;
            lastWorldClickPos = worldClickPos;
        }

        public override void OnDrawGizmos()
        {
            if (!debug) return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastClickTilePos, 0.15f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(lastWorldClickPos, 0.15f);
        }
    }
}