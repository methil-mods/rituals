using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Controller;
using Framework.ExtendedGrid;
using ScriptableObjects.Materials;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World
{
    public class WorldController: BaseController<WorldController>
    {
        [SerializeField]
        private List<MapPiece> mapPieces;
        public Grid worldGrid;
        [SerializeField]
        private WorldTileHover worldTileHover;
        
        public void Start()
        {
            mapPieces.ForEach((piece => piece.Setup()));
        }

        public void Update()
        {
            worldTileHover.Update(this);
        }

        public void HideAllMap()
        {
            foreach (var mapPiece in mapPieces)
            {
                mapPiece.HideMap();
            }
        }

        public Tilemap[] GetGroundMap(bool locked = false)
        {
            List<Tilemap> groundMap = new List<Tilemap>();
            foreach (MapPiece piece in mapPieces)
            {
                if (locked != piece.isLocked) continue;
                groundMap.Add(piece.groundTilemap);
            }
            return groundMap.ToArray();
        }

        public Tilemap[] GetCollisionMap(bool locked = false)
        {
            List<Tilemap> collisions = new List<Tilemap>();
            foreach (MapPiece piece in mapPieces)
            {
                if (locked != piece.isLocked) continue;
                foreach (var collisionMap in piece.collisionMap) collisions.Add(collisionMap);
            }
            return collisions.ToArray();
        }

        public bool IsCellMovable(Vector3 worldPos)
        {
            Vector3Int cellPos = worldGrid.WorldToCell(worldPos);
            Tilemap[] ground = GetGroundMap();
            bool hasGround = false;
            foreach (Tilemap groundTilemap in ground)
            {
                if (groundTilemap != null && groundTilemap.HasTile(cellPos))
                {
                    hasGround = true;
                    break;
                }
            }
            return hasGround;
        }

        public void UnlockPiece(string pieceIdentifier)
        {
            bool found = false;
            mapPieces.ForEach((piece =>
            {
                if (piece.pieceIdentifier == pieceIdentifier)
                {
                    piece.Unlock();
                    found = true;
                }
            }));
    
            if (!found)
            {
                Debug.LogWarning($"Piece with identifier '{pieceIdentifier}' not found in mapPieces");
            }
        }
    }

    [Serializable]
    public class MapPiece
    {
        public string pieceIdentifier;
        [SerializeField]
        public Tilemap groundTilemap;
        [SerializeField]
        public Tilemap[] collisionMap;
        [SerializeField]
        public Tilemap[] decorationMap;
        [SerializeField]
        public Transform interactibleParent;
        public bool isLocked;

        public void Setup()
        {
            LaunchActionOnEveryMap(SetMap);
            SetShaderOnInteractible();
        }

        public void Unlock()
        {
            if (this.isLocked == false) return;
            this.isLocked = false;
            LaunchActionOnEveryMap(UnlockMap);
            SetShaderOnInteractible();
        }

        public void HideMap()
        {
            LaunchActionOnEveryMap(LockMap);
        }

        private void LaunchActionOnEveryMap(Action<Tilemap> action)
        {
            foreach (var map in collisionMap) action(map);
            foreach (var map in decorationMap) action(map);
            action(groundTilemap);
        }

        private void SetMap(Tilemap tilemap)
        {
            Material tileMapMaterial = new Material(MaterialDatabase.Instance.tileMapMaterial);
            tilemap.GetComponent<TilemapRenderer>().material = tileMapMaterial;
            tileMapMaterial.name = "TileMap Renderer";
            tileMapMaterial.SetFloat("_Alpha", isLocked ? 0f : 1f);
        }

        private void SetShaderOnInteractible()
        {
            foreach (Transform interactibleChild in interactibleParent)
            {
                Material interactibleMaterial = new Material(MaterialDatabase.Instance.tileMapMaterial);
                interactibleMaterial.name = "Sprite Object Interactible Renderer";
                interactibleMaterial.SetFloat("_Alpha", isLocked ? 0f : 1f);

                var spriteRendererParent = interactibleChild.GetComponent<SpriteRenderer>();
                if(spriteRendererParent != null)
                {
                    Debug.Log(interactibleChild.name);
                    spriteRendererParent.material = interactibleMaterial;
                }
                
                var spriteRendererChildren = interactibleChild.GetComponentInChildren<SpriteRenderer>();
                if(spriteRendererChildren != null)
                {
                    spriteRendererChildren.material = interactibleMaterial;
                }
            }
        }
        
        private void LockMap(Tilemap tilemap)
        {
            LeanTween.value(WorldController.Instance.gameObject, f =>
            {
                tilemap.GetComponent<TilemapRenderer>().material.SetFloat("_Alpha", f);
            }, 1f, 0f, 1f);
        }
        
        private void UnlockMap(Tilemap tilemap)
        {
            LeanTween.value(WorldController.Instance.gameObject, f =>
            {
                tilemap.GetComponent<TilemapRenderer>().material.SetFloat("_Alpha", f);
            }, 0f, 1f, 1f);
        }
    }
}