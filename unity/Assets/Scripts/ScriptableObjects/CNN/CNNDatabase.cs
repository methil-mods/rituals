using Framework.ScriptableObjects;
using Unity.Barracuda;
using UnityEngine;

namespace ScriptableObjects.CNN
{
    [CreateAssetMenu(fileName = "CNNDatabase", menuName = "CNN/CNNDatabase")]
    public class CNNDatabase : SingletonScriptableObject<CNNDatabase>
    {
        public CNNData ritualsCNN;
    }
}