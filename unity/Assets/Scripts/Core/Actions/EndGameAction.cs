using System;
using Core.Inventory;
using Core.UserInterface.End;
using ScriptableObjects.Inventory;
using Action = Framework.Action.Action;

namespace Core.Actions
{
    [Serializable]
    public class EndGameAction : Action
    {
        public override void Execute()
        {
            EndController.Instance.EndGame();
        }
    }
}