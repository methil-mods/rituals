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
            SetValueEffect(v => chromaticAberration.intensity.Override(v), 1f, 0f, 0.5f);
            SetValueEffect(v => vignette.intensity.Override(v), 0.4f, 0f, 0.5f);
        }

        public void SetHidePostProcessingEffect()
        {
            SetValueEffect(v => chromaticAberration.intensity.Override(v), 0f, 1f, 0.5f);
            SetValueEffect(v => vignette.intensity.Override(v), 0f, 0.4f, 0.5f);
        }

        private void SetValueEffect(Action<float> callback, float from, float to, float time)
        {
            LeanTween.value(gameObject, callback, from, to, time).setEaseOutCirc();
        }
    }
}