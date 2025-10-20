using Framework.Controller;
using ScriptableObjects.SFX;
using UnityEngine;

namespace Core.SFX
{
    public class SFXController : BaseController<SFXController>
    {
        [Header("Assign manually in Inspector")]
        public AudioSource MusicAudioSource;

        protected override void Awake()
        {
            base.Awake();

            // Persiste entre les scènes
            DontDestroyOnLoad(gameObject);

            if (MusicAudioSource == null)
            {
                Debug.LogWarning("[SFXController] MusicAudioSource n'est pas assigné !");
                return;
            }

            MusicAudioSource.loop = true;
            MusicAudioSource.playOnAwake = false;
            
            PlayMusic();
        }

        public void PlayMusic()
        {
            MusicAudioSource.clip = SFXDatabase.Instance.musicClip;
            MusicAudioSource.volume = SFXDatabase.Instance.volume;
            MusicAudioSource.Play();
        }

        public void StopMusic()
        {
            if (MusicAudioSource != null && MusicAudioSource.isPlaying)
                MusicAudioSource.Stop();
        }
    }
}