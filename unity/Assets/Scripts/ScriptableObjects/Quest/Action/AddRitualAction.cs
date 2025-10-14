using System;
using Core.UserInterface.Ritual;
using ScriptableObjects.Ritual;
using UnityEngine;

namespace ScriptableObjects.Quest.Action
{
    [Serializable]
    public class AddRitualAction : QuestAction
    {
        [SerializeField] private RitualData ritual;

        public override void Execute()
        {
            RitualUserInterfaceController.Instance.AddRitual(ritual);
        }
    }
}