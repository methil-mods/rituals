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
using Core.SFX;

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
        
        private float lastClickTime = 0f;
        [SerializeField]
        private float clickCooldown = 0.25f; // seconds (adjust as needed)

        
        public override void Start(PlayerController controller)
        {
            currentPath = new List<Vector3>();
            this.controller = controller;
            currentIdleAnimation = PlayerDatabase.Instance.animationIdleBottomRight;
        }
        
        public override void Update(PlayerController controller)
        {
            UpdateAnimation();
            
            if (PauseController.Instance.isPaused || (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
                return;
            
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Time.time - lastClickTime < clickCooldown)
                    return; // still in cooldown

                lastClickTime = Time.time;

                Vector3 mouseWorld = CameraUtils.ScreenToWorld(Mouse.current.position.ReadValue());
                Vector3Int clickedTile = WorldController.Instance.worldGrid.WorldToCell(mouseWorld);

                if (!PathFindingUtils.IsTileWalkable(clickedTile,
                        WorldController.Instance.GetGroundMap(),
                        WorldController.Instance.GetCollisionMap()))
                    return;

                Vector3 targetWorld = WorldController.Instance.worldGrid.GetCellCenterWorld(clickedTile);
                MoveToTile(targetWorld);
            }
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
            
            if (angle >= -90f && angle < 0f)
            {
                currentIdleAnimation = PlayerDatabase.Instance.animationIdleBottomRight;
                currentWalkAnimation = PlayerDatabase.Instance.animationWalkBottomRight;
            }
            else if (angle >= 0f && angle < 90f)
            {
                currentIdleAnimation = PlayerDatabase.Instance.animationIdleTopRight;
                currentWalkAnimation = PlayerDatabase.Instance.animationWalkTopRight;
            }
            else if (angle >= 90f && angle <= 180f)
            {
                currentIdleAnimation = PlayerDatabase.Instance.animationIdleTopLeft;
                currentWalkAnimation = PlayerDatabase.Instance.animationWalkTopLeft;
            }
            else
            {
                currentIdleAnimation = PlayerDatabase.Instance.animationIdleBottomLeft;
                currentWalkAnimation = PlayerDatabase.Instance.animationWalkBottomLeft;
            }
        }
        
        public void MoveToTile(Vector3 targetWorld)
        {
            Vector3 startPosition = controller.transform.position;
            
            currentPath = PathFindingUtils.CalculatePathAStar(startPosition,
                 targetWorld,
                 WorldController.Instance.GetGroundMap(),
                 WorldController.Instance.GetCollisionMap()
                );
            
            if (currentPath.Count > 0)
            {
                LeanTween.cancel(controller.gameObject);
                controller.transform.position = startPosition;
                currentWaypointIndex = 0;
                isMoving = true;
                SFXController.Instance.PlayWalkingAudioClip();
                MoveToNextWaypoint();
            }
        }
        
        private void MoveToNextWaypoint()
        {
            if (currentWaypointIndex >= currentPath.Count)
            {
                isMoving = false;
                SFXController.Instance.StopWalkingAudioClip();
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
            SFXController.Instance.StopWalkingAudioClip();
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