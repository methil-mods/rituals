using Core.Dialog;
using UnityEngine;

namespace ScriptableObjects.Entity
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Entity/EntityData")]
    public class EntityData : ScriptableObject
    {
        public string entityName;
        public Sprite entitySprite;
        [TextArea]
        public string entityDescription;
        
        public DialogData[] dialogs;
    }
}