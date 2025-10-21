using Framework.Controller;
using ScriptableObjects.SFX;
using UnityEngine;

namespace Core.SFX
{
    public class SFXController : BaseController<SFXController>
    {
        public AudioSource musicAudioSource;
        
        public AudioSource talkingUiAudioSource;
        public AudioSource bookUiAudioSource;
        public AudioSource otherUiAudioSource;

        protected override void Awake()
        {
            base.Awake();

            // Persiste entre les scènes
            DontDestroyOnLoad(gameObject);

            if (musicAudioSource == null)
            {
                Debug.LogWarning("[SFXController] musicAudioSource n'est pas assigné !");
                return;
            }

            musicAudioSource.loop = true;
            musicAudioSource.playOnAwake = false;
            
            PlayMusic();
        }

        private void PlayMusic()
        {
            musicAudioSource.clip = SFXDatabase.Instance.musicClip;
            musicAudioSource.volume = SFXDatabase.Instance.volume;
            musicAudioSource.Play();
        }

        private void StopMusic()
        {
            if (musicAudioSource != null && musicAudioSource.isPlaying)
                musicAudioSource.Stop();
        }

        public void PlayOpenBookSound()
        {
            bookUiAudioSource.volume = SFXDatabase.Instance.volume;
            bookUiAudioSource.loop = false;
            bookUiAudioSource.PlayOneShot(SFXDatabase.Instance.pageOpenClip);
        }

        public void PlayTalkAudioClip(AudioClip clip)
        {
            talkingUiAudioSource.clip = clip;
            talkingUiAudioSource.volume = SFXDatabase.Instance.volume;
            talkingUiAudioSource.loop = true;
            talkingUiAudioSource.Play();
        }

        public void StopTalkAudioClip()
        {
            talkingUiAudioSource.Stop();
        }
    }
}