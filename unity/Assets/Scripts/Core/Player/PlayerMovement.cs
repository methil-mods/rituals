using System;
using Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Utils;

namespace Player
{
    [Serializable]
    public class PlayerMovement : Updatable<PlayerController>
    {
        [Header("References")]
        public Tilemap groundTilemap;
        public Tilemap collisionTilemap;

        [Header("Settings")]
        public float moveSpeed = 3f;

        private bool isMoving = false;
        private List<Vector3> currentPath;
        private int currentWaypointIndex = 0;
        
        private PlayerController controller;

        public override void Start(PlayerController controller)
        {
            currentPath = new List<Vector3>();
            this.controller = controller;
        }
        
        public override void Update(PlayerController controller)
        {
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (isMoving) return;
                
                Vector3 mouseWorld = CameraUtils.ScreenToWorld(Mouse.current.position.ReadValue());
                Vector3Int clickedTile = groundTilemap.WorldToCell(mouseWorld);

                if (!PathFindingUtils.IsTileWalkable(clickedTile, groundTilemap, collisionTilemap)) return;
                    
                Vector3 targetWorld = groundTilemap.GetCellCenterWorld(clickedTile);
                MoveToTile(targetWorld);
            }
        }

        public void MoveToTile(Vector3 targetWorld)
        {
            currentPath = PathFindingUtils.CalculatePathAStar(controller.transform.position, targetWorld, groundTilemap, collisionTilemap);
            
            if (currentPath.Count > 0)
            {
                LeanTween.cancel(controller.gameObject);
                currentWaypointIndex = 0;
                isMoving = true;
                MoveToNextWaypoint();
            }
        }

        private void MoveToNextWaypoint()
        {
            if (currentWaypointIndex >= currentPath.Count)
            {
                isMoving = false;
                return;
            }

            Vector3 targetPos = currentPath[currentWaypointIndex];
            float distance = Vector3.Distance(controller.transform.position, targetPos);
            float duration = distance / moveSpeed;

            LeanTween.move(controller.gameObject, targetPos, duration).setEase(LeanTweenType.linear)
                .setOnComplete(() =>
                {
                    currentWaypointIndex++;
                    MoveToNextWaypoint();
                });
        }

        public override void OnDestroy()
        {
            LeanTween.cancel(controller.gameObject);
        }

        public override void OnDrawGizmos()
        {
            if (currentPath == null || currentPath.Count == 0) return;

            Gizmos.color = Color.cyan;
            for (int i = 0; i < currentPath.Count; i++)
            {
                Gizmos.DrawSphere(currentPath[i], 0.15f);
                if (i < currentPath.Count - 1)
                    Gizmos.DrawLine(currentPath[i], currentPath[i + 1]);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(controller.transform.position, 0.2f);
        }
    }
}