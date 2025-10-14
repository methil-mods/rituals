using System;
using Core.Inventory;
using ScriptableObjects.Inventory;
using Action = Framework.Action.Action;

namespace Core.Actions
{
    [Serializable]
    public class AddItemAction : Action
    {
        public ItemData itemData;   
        
        public override void Execute()
        {
            InventoryController.Instance.AddItem(itemData);
        }
    }
}