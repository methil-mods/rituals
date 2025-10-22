using Framework.ScriptableObjects;
using UnityEngine;

namespace ScriptableObjects.SFX
{
    [CreateAssetMenu(fileName = "SFXDatabase", menuName = "SFX/SFXDatabase")]
    public class SFXDatabase : SingletonScriptableObject<SFXDatabase>
    {
        [Header("Music")]
        public float musicVolume = 1f;
        public AudioClip musicClip;

        [Header("Player")]
        public float walkVolume = 1f;
        public AudioClip walkClip;
        
        [Header("Talk")]
        public float talkVolume = 1f;
        
        [Header("Monster")]
        public AudioClip saviorClip;
        
        [Header("User Interface")]
        public float pageOpenVolume = 1f;
        public AudioClip pageOpenClip;
        
        public float ritualOpenVolume = 1f;
        public AudioClip ritualOpenClip;
        
        public float ritualStartVolume = 1f;
        public AudioClip ritualStartClip;

        public float hoverButtonVolume = 1f;
        public AudioClip hoverButtonClip;
        
        public float clickButtonVolume = 1f;
        public AudioClip clickButtonClip;
        
        [Header("Others")]
        public float otherVolume = 1f;
    }
}