using System.Collections.Generic;
using ScriptableObjects.Ritual;
using Unity.Barracuda;
using UnityEngine;

namespace ScriptableObjects.CNN
{
    [CreateAssetMenu(fileName = "CNNData", menuName = "CNN/CNNData")]
    public class CNNData : ScriptableObject
    {
        public string modelName;
        public NNModel model;
        public List<RitualData> rituals;
    }
}