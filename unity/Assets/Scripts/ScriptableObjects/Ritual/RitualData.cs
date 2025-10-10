using System.Collections.Generic;
using ScriptableObjects.Ritual;
using Unity.Barracuda;
using UnityEngine;

namespace ScriptableObjects.Ritual
{
    [CreateAssetMenu(fileName = "RitualData", menuName = "Rituals/RitualData")]
    public class RitualData : ScriptableObject
    {
        public string ritualName;
    }
}