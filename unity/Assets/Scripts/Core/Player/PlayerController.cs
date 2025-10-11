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
    }
}