using System;
using Core.UserInterface;
using Framework.Controller;
using ScriptableObjects.Book;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Core.PostProcess
{
    public class PostProcessingController : BaseController<PostProcessingController>
    {
        public VolumeProfile globalVolume;

        private ChromaticAberration chromaticAberration;
        private Vignette vignette;

        private UnityAction<BookData> onBookOpenedHandler;
        private UnityAction<BookData> onBookClosedHandler;

        private void Start()
        {
            if (globalVolume != null)
            {
                globalVolume.TryGet(out chromaticAberration);
                globalVolume.TryGet(out vignette);
            }

            DialogUserInterfaceController.Instance.OnDialogStart += SetHidePostProcessingEffect;
            DialogUserInterfaceController.Instance.OnDialogEnd += ResetPostProcessingEffect;

            onBookOpenedHandler = (a) => SetHidePostProcessingEffect();
            onBookClosedHandler = (a) => ResetPostProcessingEffect();

            BookUserInterfaceController.Instance.OnBookOpened += onBookOpenedHandler;
            BookUserInterfaceController.Instance.OnBookClosed += onBookClosedHandler;

            ResetPostProcessingEffect();
        }

        private void OnDisable()
        {
            DialogUserInterfaceController.Instance.OnDialogStart -= SetHidePostProcessingEffect;
            DialogUserInterfaceController.Instance.OnDialogEnd -= ResetPostProcessingEffect;

            BookUserInterfaceController.Instance.OnBookOpened -= onBookOpenedHandler;
            BookUserInterfaceController.Instance.OnBookClosed -= onBookClosedHandler;
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
