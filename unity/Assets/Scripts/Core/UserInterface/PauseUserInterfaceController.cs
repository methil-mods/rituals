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

        public void OnMusicChange()
        {
            SFXDatabase.Instance.musicVolume = musicVolumeSlider.value;
            UpdateVolume();
        }

        public void OnWalkChange()
        {
            SFXDatabase.Instance.walkVolume = walkVolumeSlider.value;
            UpdateVolume();
        }

        public void OnUIChange()
        {
            SFXDatabase.Instance.pageOpenVolume = uiVolumeSlider.value;
            UpdateVolume();
        }

        public void OnOtherChange()
        {
            SFXDatabase.Instance.otherVolume = otherVolumeSlider.value;
            UpdateVolume();
        }
    }
}