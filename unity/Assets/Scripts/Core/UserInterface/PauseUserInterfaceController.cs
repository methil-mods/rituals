using Core.SFX;
using Framework.Controller;
using ScriptableObjects.SFX;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UserInterface
{
    public class PauseUserInterfaceController : InterfaceController<PauseUserInterfaceController>
    {
        [SerializeField]
        private Slider musicVolumeSlider;
        [SerializeField]
        private Slider walkVolumeSlider;
        [SerializeField]
        private Slider uiVolumeSlider;
        [SerializeField]
        private Slider otherVolumeSlider;

        new void Start()
        {
            base.Start();
            musicVolumeSlider.value = SFXDatabase.Instance.musicVolume;
            walkVolumeSlider.value = SFXDatabase.Instance.walkVolume;
            uiVolumeSlider.value = SFXDatabase.Instance.pageOpenVolume;
            otherVolumeSlider.value = SFXDatabase.Instance.otherVolume;
        }

        private void UpdateVolume()
        {
            SFXController.Instance.UpdateVolume();
            // Might call an event maybe
        }
        
        // Convert slider value [0,1] to perceived-linear volume [0,1]
        private float SliderToVolume(float sliderValue)
        {
            // Avoid log(0)
            if (sliderValue <= 0f)
                return 0f;

            // Use a logarithmic scale: typical audio volume curve
            return Mathf.Pow(sliderValue, 2.0f); // simple power curve
        }

        public void OnMusicChange()
        {
            SFXDatabase.Instance.musicVolume = SliderToVolume(musicVolumeSlider.value);
            UpdateVolume();
        }

        public void OnWalkChange()
        {
            SFXDatabase.Instance.walkVolume = SliderToVolume(walkVolumeSlider.value);
            UpdateVolume();
        }

        public void OnUIChange()
        {
            SFXDatabase.Instance.pageOpenVolume = SliderToVolume(uiVolumeSlider.value);
            UpdateVolume();
        }

        public void OnOtherChange()
        {
            SFXDatabase.Instance.otherVolume = SliderToVolume(otherVolumeSlider.value);
            UpdateVolume();
        }
    }
}