using UnityEngine;

namespace ScriptableObjects.Inventory
{
    public abstract class ItemData : ScriptableObject
    {
        public string itemName;
        [TextArea]
        public string description;
        public Sprite icon;
    }
}