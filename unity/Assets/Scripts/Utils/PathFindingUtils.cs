using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Utils
{
    public static class PathFindingUtils
    {
        public static List<Vector3> CalculatePathAStar(Vector3 start, Vector3 end, Tilemap[] groundTilemaps, Tilemap[] collisionTilemaps, int maxIterations = 500)
        {
            List<Vector3> path = new List<Vector3>();
            
            Tilemap referenceTilemap = groundTilemaps[0];
            Vector3Int startCell = referenceTilemap.WorldToCell(start);
            Vector3Int endCell = referenceTilemap.WorldToCell(end);

            List<Vector3Int> openSet = new List<Vector3Int> { startCell };
            HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
            Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
            Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
            Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>();

            gScore[startCell] = 0;
            fScore[startCell] = ManhattanDistance(startCell, endCell);

            int iterations = 0;

            while (openSet.Count > 0 && iterations < maxIterations)
            {
                iterations++;

                Vector3Int current = openSet[0];
                float lowestF = fScore[current];
                foreach (var node in openSet)
                {
                    if (fScore[node] < lowestF)
                    {
                        current = node;
                        lowestF = fScore[node];
                    }
                }

                if (current == endCell)
                {
                    return ReconstructPath(cameFrom, current, referenceTilemap);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                Vector3Int[] neighbors = new Vector3Int[]
                {
                    current + Vector3Int.up,
                    current + Vector3Int.down,
                    current + Vector3Int.left,
                    current + Vector3Int.right
                };

                foreach (Vector3Int neighbor in neighbors)
                {
                    if (closedSet.Contains(neighbor)) continue;
                    if (!IsTileWalkable(neighbor, groundTilemaps, collisionTilemaps)) continue;

                    float tentativeGScore = gScore[current] + 1;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (gScore.ContainsKey(neighbor) && tentativeGScore >= gScore[neighbor])
                    {
                        continue;
                    }

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + ManhattanDistance(neighbor, endCell);
                }
            }

            return path;
        }

        private static List<Vector3> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current, Tilemap referenceTilemap)
        {
            List<Vector3Int> cellPath = new List<Vector3Int> { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                cellPath.Add(current);
            }

            cellPath.Reverse();

            List<Vector3> worldPath = new List<Vector3>();
            foreach (Vector3Int cell in cellPath)
            {
                worldPath.Add(referenceTilemap.GetCellCenterWorld(cell));
            }

            return worldPath;
        }

        public static bool IsTileWalkable(Vector3Int tilePos, Tilemap[] groundTilemaps, Tilemap[] collisionTilemaps)
        {
            bool hasGround = false;
            foreach (Tilemap groundTilemap in groundTilemaps)
            {
                if (groundTilemap != null && groundTilemap.HasTile(tilePos))
                {
                    hasGround = true;
                    break;
                }
            }

            bool hasCollision = false;
            if (collisionTilemaps != null)
            {
                foreach (Tilemap collisionTilemap in collisionTilemaps)
                {
                    if (collisionTilemap != null && collisionTilemap.HasTile(tilePos))
                    {
                        hasCollision = true;
                        break;
                    }
                }
            }

            return hasGround && !hasCollision;
        }

        private static float ManhattanDistance(Vector3Int a, Vector3Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
    }
}