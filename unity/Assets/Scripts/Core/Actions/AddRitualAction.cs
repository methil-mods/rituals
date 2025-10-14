using System;
using Core.UserInterface.Ritual;
using ScriptableObjects.Ritual;
using UnityEngine;
using Action = Framework.Action.Action;

namespace Core.Actions
{
    [Serializable]
    public class AddRitualAction : Action
    {
        [SerializeField] private RitualData ritual;

        public override void Execute()
        {
            RitualUserInterfaceController.Instance.AddRitual(ritual);
        }
    }
}