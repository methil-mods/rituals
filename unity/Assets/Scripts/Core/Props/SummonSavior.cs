using Core.UserInterface.Savior;
using Framework.Props;

namespace Core.Props
{
    public class SummonSavior : Interactible<SummonSavior>
    {
        private bool interacted = false;
        
        protected override bool CanInteract()
        {
            if (interacted) return false;
            return true;
        }

        public override void Interact()
        {
            interacted = true;
            base.Interact();
            SummonSaviorInterface.Instance.OpenPanel();
        }
    }
}