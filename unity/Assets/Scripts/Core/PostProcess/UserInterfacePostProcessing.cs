using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

namespace Core.PostProcess
{
    [Serializable]
    public class UserInterfacePostProcessing
    {
        private GameObject gameObject;

        public VolumeProfile uiVolume;

        public float animationDuration = 1f;
        public float endAnimationDuration = 0.5f;

        public float chromaticAberrationTarget = 1f;
        public float vignetteTarget = 0.3f;
        public float lensDistortionIntensityTarget = 0.5f;
        public float lensDistortionScaleTarget = 0.7f;
        private float initialBloomIntensity;
        public float targetBloomIntensity = 3f;

        private ChromaticAberration chromaticAberration;
        private Vignette vignette;
        private LensDistortion lensDistortion;
        private Bloom bloom;

        public void Start(PostProcessingController controller)
        {
            gameObject = controller.gameObject;

            if (uiVolume != null)
            {
                uiVolume.TryGet(out chromaticAberration);
                uiVolume.TryGet(out vignette);
                uiVolume.TryGet(out lensDistortion);
                uiVolume.TryGet(out bloom);
                initialBloomIntensity = bloom.intensity.value;
            }
        }

        public void LaunchRitualAnimation(Action onEnd)
        {
            SetValueEffect(f => chromaticAberration.intensity.value = f, 0f, chromaticAberrationTarget, animationDuration);
            SetValueEffect(f => vignette.intensity.value = f, 0f, vignetteTarget, animationDuration);
            SetValueEffect(f => lensDistortion.intensity.value = f, 0f, lensDistortionIntensityTarget, animationDuration);
            SetValueEffect(f => lensDistortion.scale.value = f, 1f, lensDistortionScaleTarget, animationDuration);
            SetValueEffect(f => bloom.intensity.value = f, initialBloomIntensity, targetBloomIntensity, animationDuration);

            LeanTween.delayedCall(animationDuration, () =>
            {
                ResetValueEffect();
                LeanTween.delayedCall(0.3f, () =>
                {
                    onEnd?.Invoke();
                });
            });
        }

        public void ResetValueEffect()
        {
            SetValueEffect(f => chromaticAberration.intensity.value = f, chromaticAberrationTarget, 0f, endAnimationDuration);
            SetValueEffect(f => vignette.intensity.value = f, vignetteTarget, 0f, endAnimationDuration);
            SetValueEffect(f => lensDistortion.intensity.value = f, lensDistortionIntensityTarget, 0f, endAnimationDuration);
            SetValueEffect(f => lensDistortion.scale.value = f, lensDistortionScaleTarget, 1f, endAnimationDuration);
            SetValueEffect(f => bloom.intensity.value = f, targetBloomIntensity, initialBloomIntensity, 0.2f);
        }

        private void SetValueEffect(Action<float> callback, float from, float to, float time)
        {
            LeanTween.value(gameObject, callback, from, to, time).setEaseOutCirc();
        }
    }
}
