using Core.UserInterface.Ritual;
using Framework.Props;
using Player;
using ScriptableObjects.Ritual;
using UnityEngine;
using World;

namespace Core.Props
{
    public class Ritual : Interactible<Ritual>
    {
        public void StartRitual()
        {
            RitualUserInterfaceController.Instance.OpenRitualPanel();
        }

        public new void Interact()
        {
            base.Interact();
            StartRitual();
        }
    }
}