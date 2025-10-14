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
        public string inputName = "input";
        public string outputName = "dense_3";
        
        public string modelName;
        public NNModel model;
        public List<RitualData> rituals;
    }
}