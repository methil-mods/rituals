using Framework.ScriptableObjects;
using UnityEngine;

namespace ScriptableObjects.SFX
{
    [CreateAssetMenu(fileName = "SFXDatabase", menuName = "SFX/SFXDatabase")]
    public class SFXDatabase : SingletonScriptableObject<SFXDatabase>
    {
        public float volume = 1f;
        public AudioClip musicClip;
    }
}