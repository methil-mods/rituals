using Framework.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerController : BaseController<PlayerController>
    {
        [SerializeField]
        private PlayerMovement playerMovement;
        [SerializeField]
        private PlayerInteraction playerInteraction;
        
        public void Start()
        {
            playerMovement.Start(this);
            playerInteraction.Start(this);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("OnTriggerEnter2D");
        }

        public void Update()
        {
            playerMovement.Update(this);
            playerInteraction.Update(this);
        }

        public void OnDrawGizmos()
        {
            playerMovement.OnDrawGizmos();
            playerInteraction.OnDrawGizmos();
        }

        public void OnDestroy()
        {
            playerMovement.OnDestroy();
            playerInteraction.OnDestroy();
        }
    }
}