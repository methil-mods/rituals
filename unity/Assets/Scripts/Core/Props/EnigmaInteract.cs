using Core.Enigma;
using Framework.Props;
using ScriptableObjects.Enigma;

namespace Core.Props
{
    public class EnigmaInteract : Interactible<EnigmaInteract>
    {
        public EnigmaData enigmaData;
        
        protected override bool CanInteract()
        {
            return true;
        }

        public override void Interact()
        {
            base.Interact();
            EnigmaController.Instance.OpenEnigmaPanel(enigmaData);
        }
    }
}