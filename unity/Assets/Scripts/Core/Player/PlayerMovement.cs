using System;
using Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Player
{
    [Serializable]
    public class PlayerMovement : Updatable<PlayerController>
    {
        public Tilemap tilemap;

        private Vector3 lastClickTilePos;
        private Vector3 lastWorldClickPos;

        public override void Update(PlayerController controller)
        {
            if (!Mouse.current.leftButton.isPressed) return;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldClickPos = Camera.main.ScreenToWorldPoint(mousePos);
            
            Vector3Int cellPos = tilemap.layoutGrid.WorldToCell(worldClickPos);
            Vector3 tilePos = tilemap.layoutGrid.GetCellCenterWorld(cellPos);
            tilePos.z = 0;

            controller.transform.position = tilePos;

            lastClickTilePos = tilePos;
            lastWorldClickPos = worldClickPos;
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastClickTilePos, 0.15f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(lastWorldClickPos, 0.15f);
        }
    }
}