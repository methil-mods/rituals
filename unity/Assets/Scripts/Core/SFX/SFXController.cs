using Framework.Controller;
using ScriptableObjects.SFX;
using UnityEngine;

namespace Core.SFX
{

    public enum AudioSources
    {
        Talking,
        BookUI,
        Walking,
        Music,
        Others
    }
    public class SFXController : BaseController<SFXController>
    {
        public AudioSource musicAudioSource;
        
        public AudioSource talkingUiAudioSource;
        public AudioSource bookUiAudioSource;
        public AudioSource walkingUiAudioSource;
        public AudioSource saviorAudioSource;
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

        public void UpdateVolume()
        {
            talkingUiAudioSource.volume = SFXDatabase.Instance.talkVolume;
            bookUiAudioSource.volume = SFXDatabase.Instance.pageOpenVolume;
            walkingUiAudioSource.volume = SFXDatabase.Instance.walkVolume;
            otherUiAudioSource.volume = SFXDatabase.Instance.otherVolume;
            musicAudioSource.volume = SFXDatabase.Instance.musicVolume;
        }

        private void PlayMusic()
        {
            musicAudioSource.clip = SFXDatabase.Instance.musicClip;
            musicAudioSource.volume = SFXDatabase.Instance.musicVolume;
            musicAudioSource.Play();
        }

        private void StopMusic()
        {
            if (musicAudioSource != null && musicAudioSource.isPlaying)
                musicAudioSource.Stop();
        }

        public void PlaySaviorSound()
        {
            saviorAudioSource.volume = SFXDatabase.Instance.otherVolume;
            saviorAudioSource.loop = false;
            saviorAudioSource.PlayOneShot(SFXDatabase.Instance.saviorClip);
        }

        public void PlayOpenBookSound()
        {
            bookUiAudioSource.volume = SFXDatabase.Instance.pageOpenVolume;
            bookUiAudioSource.loop = false;
            bookUiAudioSource.PlayOneShot(SFXDatabase.Instance.pageOpenClip);
        }

        public void PlayWalkingAudioClip()
        {
            walkingUiAudioSource.clip = SFXDatabase.Instance.walkClip;
            walkingUiAudioSource.volume = SFXDatabase.Instance.walkVolume;
            walkingUiAudioSource.loop = true;
            walkingUiAudioSource.Play();
        }

        public void StopWalkingAudioClip()
        {
            walkingUiAudioSource.Stop();
        }

        public void PlayTalkAudioClip(AudioClip clip)
        {
            talkingUiAudioSource.clip = clip;
            talkingUiAudioSource.volume = SFXDatabase.Instance.talkVolume;
            talkingUiAudioSource.loop = true;
            talkingUiAudioSource.Play();
        }

        public void StopTalkAudioClip()
        {
            talkingUiAudioSource.Stop();
        }
    }
}