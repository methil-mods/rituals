using ScriptableObjects.Entity;
using UnityEngine;

namespace ScriptableObjects.Ritual
{
    [CreateAssetMenu(fileName = "RitualData", menuName = "Rituals/RitualData")]
    public class RitualData : ScriptableObject
    {
        public string ritualName;
        public Sprite ritualImage;
        public EntityData entityData;
    }
}