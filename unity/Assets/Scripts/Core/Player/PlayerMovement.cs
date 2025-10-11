using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public Tilemap tilemap;

        private Vector3 lastClickTilePos;
        private Vector3 lastWorldClickPos;

        private void Update()
        {
            if (!Mouse.current.leftButton.isPressed) return;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldClickPos = Camera.main.ScreenToWorldPoint(mousePos);
            
            Vector3Int cellPos = tilemap.layoutGrid.WorldToCell(worldClickPos);
            Vector3 tilePos = tilemap.layoutGrid.GetCellCenterWorld(cellPos);
            tilePos.z = 0;

            transform.position = tilePos;

            lastClickTilePos = tilePos;
            lastWorldClickPos = worldClickPos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastClickTilePos, 0.15f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(lastWorldClickPos, 0.15f);
        }
    }
}