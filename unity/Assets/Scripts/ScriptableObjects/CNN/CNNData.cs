using System;
using System.Collections.Generic;
using ScriptableObjects.Ritual;
using Unity.Barracuda;
using UnityEngine;

namespace ScriptableObjects.CNN
{
    [CreateAssetMenu(fileName = "CNNData", menuName = "CNN/CNNData")]
    public class CNNData : ScriptableObject
    {
        public readonly string InputName = "input";
        public readonly string OutputName = "dense_1";
        
        public string modelName;
        public NNModel model;
        public List<RitualData> rituals;
    }
}