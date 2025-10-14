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
        public RitualData ritualData;

        public void StartRitual()
        {
            if (!ritualData)
            {
                Debug.LogWarning($"No book data found on tile ${gameObject.name}");
                return;
            }
            // TODO: Ethan ton taff de plugger le CNN ici
            Debug.Log("Start ritual here" + ritualData.ritualName);
        }

        public new void Interact()
        {
            base.Interact();
            StartRitual();
        }
    }
}