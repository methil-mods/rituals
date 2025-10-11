using Framework.ScriptableObjects;
using Unity.Barracuda;
using UnityEngine;

namespace ScriptableObjects.CNN
{
    [CreateAssetMenu(fileName = "CNNDatabase", menuName = "CNN/CNNDatabase")]
    public class CNNDatabase : SingletonScriptableObject<CNNDatabase>
    {
        public CNNData ritualsCNN;
        
        private void OnEnable()
        {
            ValidateData();
        }

        private void ValidateData()
        {
            if (ritualsCNN == null)
            {
                Debug.LogWarning($"[CNNDatabase] ritualsCNN is not assigned in {name}. Please assign a CNNData asset.", this);
            }
        }
    }
}