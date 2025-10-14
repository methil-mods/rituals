using ScriptableObjects.Inventory;
using UnityEngine;

namespace Core.Inventory
{
    [System.Serializable]
    public class ItemDataHolder
    {
        private ItemData baseData;

        public string itemName;
        [TextArea]
        public string description;
        protected Sprite icon;
        
        public ItemDataHolder(ItemData baseData)
        {
            this.baseData = baseData;
            ResetToBase();
        }

        public void ResetToBase()
        {
            itemName = baseData.itemName;
            description = baseData.description;
            icon = baseData.icon;
        }
    }
}