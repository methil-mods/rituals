using System;
using Core.Game;
using Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace World
{
    [Serializable]
    public class WorldTileHover: Updatable<WorldController>
    {
        [SerializeField]
        private Tilemap effectedTilemap;
        [SerializeField]
        private TileBase tile;

        private Vector3Int oldCellPos = Vector3Int.zero;

        public override void Start(WorldController controller)
        {
            // Silent should be golden
        }

        public override void Update(WorldController controller)
        {
            // Avoid setting null to null
            if (effectedTilemap.GetTile(oldCellPos) != null)
                effectedTilemap.SetTile(oldCellPos, null);
            
            if (PauseController.Instance.isPaused || (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
                return; 
            
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int cellPos = effectedTilemap.WorldToCell(worldPos);
            
            if (!controller.IsCellMovable(worldPos)) return;
            
            effectedTilemap.SetTile(cellPos, tile);
            oldCellPos = cellPos;
        }
    }
}