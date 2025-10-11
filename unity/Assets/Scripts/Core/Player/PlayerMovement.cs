using System;
using Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using World;

namespace Player
{
    [Serializable]
    public class PlayerMovement : Updatable<PlayerController>
    {
        public NavMeshAgent agent;
        public WorldController worldController;

        private Vector3 lastClickTilePos;
        private Vector3 lastWorldClickPos;

        [SerializeField]
        private bool debug = false;

        public override void Update(PlayerController controller)
        {
            if (!Mouse.current.leftButton.isPressed) return;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldClickPos = Camera.main.ScreenToWorldPoint(mousePos);
            
            Vector3 tileWorldPos = worldController.WorldToCellCenter(worldClickPos);

            if (worldController.IsCellMovable(tileWorldPos))
                agent.SetDestination(tileWorldPos);

            if (!debug) return;
            lastClickTilePos = tileWorldPos;
            lastWorldClickPos = worldClickPos;
        }

        public override void OnDrawGizmos()
        {
            if (!debug) return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastClickTilePos, 0.15f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(lastWorldClickPos, 0.15f);
        }
    }
}