using System;
using Core.Inventory;
using ScriptableObjects.Inventory;
using World;
using Action = Framework.Action.Action;

namespace Core.Actions
{
    [Serializable]
    public class UnlockPieceAction : Action
    {
        public string pieceIdentifier;   
        
        public override void Execute()
        {
            WorldController.Instance.UnlockPiece(pieceIdentifier);
        }
    }
}