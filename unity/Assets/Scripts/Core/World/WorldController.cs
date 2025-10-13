using System.Collections.Generic;
using Framework.Controller;
using Framework.ExtendedGrid;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World
{
    public class WorldController: BaseController<WorldController>
    {
        [SerializeField]
        private List<Tilemap> tilemaps;
        [SerializeField]
        private Grid worldGrid;
        [SerializeField]
        private WorldTileHover worldTileHover;

        public void Update()
        {
            worldTileHover.Update(this);
        }

        private Tilemap GetTilemapFromTag(string tagName)
        {
            return  tilemaps.Find(t => t.CompareTag(tagName));
        }

        public Vector3Int WorldToCell(Vector3 worldPosition)
        {
            return worldGrid.WorldToCell(worldPosition);
        }

        public Vector3 WorldToCellCenter(Vector3 worldPos)
        {
            Vector3Int cellPos = worldGrid.WorldToCell(worldPos);
            Vector3 tilePos = worldGrid.GetCellCenterWorld(cellPos);
            // Make sure the z pos of the tile is set to 0 to avoid layer messing
            tilePos.z = 0;
            return tilePos;
        }

        public bool IsCellMovable(Vector3 worldPos)
        {
            Vector3Int cellPos = worldGrid.WorldToCell(worldPos);
            Tilemap ground = GetTilemapFromTag("Ground");
            bool hasGround = ground.HasTile(cellPos);
            return hasGround;
        }
    }
}