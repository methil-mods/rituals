using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Utils
{
    public static class PathFindingUtils
    {
        public static List<Vector3> CalculateSimplePath(Vector3 start, Vector3 end, Tilemap groundTilemap, Tilemap collisionTilemap, int maxSteps = 100)
        {
            List<Vector3> path = new List<Vector3>();
            Vector3Int startCell = groundTilemap.WorldToCell(start);
            Vector3Int endCell = groundTilemap.WorldToCell(end);

            Vector3Int current = startCell;
            int steps = 0;

            while (current != endCell && steps < maxSteps)
            {
                Vector3Int next = current;
                
                if (current.x < endCell.x)
                    next.x++;
                else if (current.x > endCell.x)
                    next.x--;
                else if (current.y < endCell.y)
                    next.y++;
                else if (current.y > endCell.y)
                    next.y--;

                if (IsTileWalkable(next, groundTilemap, collisionTilemap))
                {
                    current = next;
                    path.Add(groundTilemap.GetCellCenterWorld(current));
                }
                else
                {
                    break;
                }

                steps++;
            }

            return path;
        }

        public static List<Vector3> CalculatePathAStar(Vector3 start, Vector3 end, Tilemap groundTilemap, Tilemap collisionTilemap, int maxIterations = 500)
        {
            List<Vector3> path = new List<Vector3>();
            Vector3Int startCell = groundTilemap.WorldToCell(start);
            Vector3Int endCell = groundTilemap.WorldToCell(end);

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
                    return ReconstructPath(cameFrom, current, groundTilemap);
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
                    if (!IsTileWalkable(neighbor, groundTilemap, collisionTilemap)) continue;

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

        private static List<Vector3> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current, Tilemap groundTilemap)
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
                worldPath.Add(groundTilemap.GetCellCenterWorld(cell));
            }

            return worldPath;
        }

        public static bool IsTileWalkable(Vector3Int tilePos, Tilemap groundTilemap, Tilemap collisionTilemap)
        {
            bool hasGround = groundTilemap.HasTile(tilePos);
            bool hasCollision = collisionTilemap != null && collisionTilemap.HasTile(tilePos);
            return hasGround && !hasCollision;
        }

        private static float ManhattanDistance(Vector3Int a, Vector3Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
    }
}