using System;
using Core.UserInterface;
using ScriptableObjects.Book;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Core.PostProcess
{
    [Serializable]
    public class WorldPostProcessing
    {
        private GameObject gameObject;

        public VolumeProfile worldVolume;

        public float resetDuration = 0.5f;
        public float hideDuration = 0.5f;

        public float chromaticAberrationReset = 0f;
        public float chromaticAberrationHide = 1f;

        public float vignetteReset = 0f;
        public float vignetteHide = 0.4f;

        private ChromaticAberration chromaticAberration;
        private Vignette vignette;

        public void Start(PostProcessingController controller)
        {
            gameObject = controller.gameObject;

            if (worldVolume != null)
            {
                worldVolume.TryGet(out chromaticAberration);
                worldVolume.TryGet(out vignette);
            }
        }

        public void ResetPostProcessingEffect()
        {
            SetValueEffect(v => chromaticAberration.intensity.Override(v), chromaticAberrationHide, chromaticAberrationReset, resetDuration);
            SetValueEffect(v => vignette.intensity.Override(v), vignetteHide, vignetteReset, resetDuration);
        }

        public void SetHidePostProcessingEffect()
        {
            SetValueEffect(v => chromaticAberration.intensity.Override(v), chromaticAberrationReset, chromaticAberrationHide, hideDuration);
            SetValueEffect(v => vignette.intensity.Override(v), vignetteReset, vignetteHide, hideDuration);
        }

        private void SetValueEffect(Action<float> callback, float from, float to, float time)
        {
            LeanTween.value(gameObject, callback, from, to, time).setEaseOutCirc();
        }
    }
}