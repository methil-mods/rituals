using System.Collections.Generic;
using Framework.Controller;
using UnityEngine;
using ScriptableObjects.Inventory;

namespace Core.Inventory
{
    public class InventoryController : BaseController<InventoryController>
    {
        [SerializeField] private List<ItemData> itemsDatas;

        private List<ItemDataHolder> items;

        private new void Awake()
        {
            base.Awake();
            
            items = new List<ItemDataHolder>();

            foreach (ItemData data in itemsDatas)
            {
                items.Add(new ItemDataHolder(data));
            }
        }

        public void AddItem(ItemData itemData)
        {
            items.Add(new ItemDataHolder(itemData));
        }
        
    }
}