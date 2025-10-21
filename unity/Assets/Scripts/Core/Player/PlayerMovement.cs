using System;
using Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Core.Game;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Utils;
using World;
using ScriptableObjects.Player;

namespace Player
{
    [Serializable]
    public class PlayerMovement : Updatable<PlayerController>
    {
        private List<Vector3> currentPath;
        private int currentWaypointIndex = 0;
        
        private PlayerController controller;
        
        public SpriteRenderer spriteRenderer;
        
        private Sprite[] currentIdleAnimation;
        private Sprite[] currentWalkAnimation;
        private int currentAnimationFrame = 0;
        private float animationTimer = 0f;
        private float animationFrameRate = 0.1f;
        private bool isMoving = false;
        
        public override void Start(PlayerController controller)
        {
            currentPath = new List<Vector3>();
            this.controller = controller;
            currentIdleAnimation = PlayerDatabase.Instance.animationIdleBottomRight;
        }
        
        public override void Update(PlayerController controller)
        {
            if (PauseController.Instance.isPaused || (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
                return;
            
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector3 mouseWorld = CameraUtils.ScreenToWorld(Mouse.current.position.ReadValue());
                Vector3Int clickedTile = WorldController.Instance.worldGrid.WorldToCell(mouseWorld);
                
                if (!PathFindingUtils.IsTileWalkable(clickedTile,
                         WorldController.Instance.GetGroundMap(),
                         WorldController.Instance.GetCollisionMap()
                        )) return;
                
                Vector3 targetWorld = WorldController.Instance.worldGrid.GetCellCenterWorld(clickedTile);
                MoveToTile(targetWorld);
            }
            
            UpdateAnimation();
        }
        
        private void UpdateAnimation()
        {
            animationTimer += Time.deltaTime;
            
            if (animationTimer >= animationFrameRate)
            {
                animationTimer = 0f;
                
                Sprite[] animationToUse = isMoving ? currentWalkAnimation : currentIdleAnimation;
                
                if (animationToUse != null && animationToUse.Length > 0)
                {
                    currentAnimationFrame = (currentAnimationFrame + 1) % animationToUse.Length;
                    spriteRenderer.sprite = animationToUse[currentAnimationFrame];
                }
            }
        }
        
        private void UpdateAnimationDirection(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            if (angle >= -45f && angle < 45f)
            {
                currentIdleAnimation = PlayerDatabase.Instance.animationIdleBottomRight;
                currentWalkAnimation = PlayerDatabase.Instance.animationWalkBottomRight;
            }
            else if (angle >= 45f && angle < 135f)
            {
                currentIdleAnimation = PlayerDatabase.Instance.animationIdleTopRight;
                currentWalkAnimation = PlayerDatabase.Instance.animationWalkTopRight;
            }
            else if (angle >= -135f && angle < -45f)
            {
                currentIdleAnimation = PlayerDatabase.Instance.animationIdleBottomLeft;
                currentWalkAnimation = PlayerDatabase.Instance.animationWalkBottomLeft;
            }
            else
            {
                currentIdleAnimation = PlayerDatabase.Instance.animationIdleTopLeft;
                currentWalkAnimation = PlayerDatabase.Instance.animationWalkTopLeft;
            }
        }
        
        public void MoveToTile(Vector3 targetWorld)
        {
            currentPath = PathFindingUtils.CalculatePathAStar(controller.transform.position,
                 targetWorld,
                 WorldController.Instance.GetGroundMap(),
                 WorldController.Instance.GetCollisionMap()
                );
            
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
                currentAnimationFrame = 0;
                return;
            }
            
            Vector3 targetPos = currentPath[currentWaypointIndex];
            Vector3 direction = (targetPos - controller.transform.position).normalized;
            UpdateAnimationDirection(direction);
            
            float distance = Vector3.Distance(controller.transform.position, targetPos);
            float duration = distance / controller.playerData.moveSpeed;
            
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