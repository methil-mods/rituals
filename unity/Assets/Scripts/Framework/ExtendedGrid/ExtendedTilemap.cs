using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Framework.ExtendedGrid
{
    [ExecuteAlways]
    public class ExtendedTilemap : MonoBehaviour
    {
        [SerializeField] private GridTile tilePrefab;
        private readonly Dictionary<Vector2Int, GridTile> tiles = new();
        public Grid layoutGrid;

        private void Awake()
        {
            layoutGrid = GetComponentInParent<Grid>();
            if (!layoutGrid)
                Debug.LogError("ExtendedTilemap must be a child of a Grid!");
        }

        public void Clear()
        {
            foreach (var tile in tiles.Values)
            {
                if (tile)
#if UNITY_EDITOR
                    DestroyImmediate(tile.gameObject);
#else
                    Destroy(tile.gameObject);
#endif
            }
            tiles.Clear();
        }

        public GridTile GetTile(Vector2Int cell)
        {
            tiles.TryGetValue(cell, out var tile);
            return tile;
        }

        public GridTile PlaceTile(Vector2Int cell)
        {
            if (tiles.ContainsKey(cell))
                return tiles[cell];

            Vector3 worldPos = layoutGrid.CellToWorld(new Vector3Int(cell.x, cell.y, 0));
            var tile = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
            tile.name = $"Tile_{cell.x}_{cell.y}";
            tiles[cell] = tile;
            return tile;
        }

        public void RemoveTile(Vector2Int cell)
        {
            if (!tiles.TryGetValue(cell, out var tile))
                return;
#if UNITY_EDITOR
            DestroyImmediate(tile.gameObject);
#else
            Destroy(tile.gameObject);
#endif
            tiles.Remove(cell);
        }
    }
}