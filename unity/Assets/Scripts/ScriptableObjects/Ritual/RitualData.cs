using System.Collections.Generic;
using ScriptableObjects.Ritual;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects.Ritual
{
    [CreateAssetMenu(fileName = "RitualData", menuName = "Rituals/RitualData")]
    public class RitualData : ScriptableObject
    {
        public string ritualName;
        public Sprite ritualImage;
    }
}