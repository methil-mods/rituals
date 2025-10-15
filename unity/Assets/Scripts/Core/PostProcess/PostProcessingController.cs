using System;
using Core.UserInterface;
using Core.UserInterface.Ritual;
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
        public WorldPostProcessing worldPostProcessing;
        public UserInterfacePostProcessing userInterfacePostProcessing;

        private UnityAction<BookData> _onBookOpenedHandler;
        private UnityAction<BookData> _onBookClosedHandler;
        
        public void Start()
        {
            worldPostProcessing.Start(this);
            userInterfacePostProcessing.Start(this);
            
            DialogUserInterfaceController.Instance.OnDialogStart += SetHidePostProcessingEffect;
            DialogUserInterfaceController.Instance.OnDialogEnd += ResetPostProcessingEffect;
            
            RitualUserInterfaceController.Instance.OnOpenRitualPanel += SetHidePostProcessingEffect;
            RitualUserInterfaceController.Instance.OnCloseRitualPanel += ResetPostProcessingEffect;

            _onBookOpenedHandler = (a) => SetHidePostProcessingEffect();
            _onBookClosedHandler = (a) => ResetPostProcessingEffect();

            BookUserInterfaceController.Instance.OnBookOpened += _onBookOpenedHandler;
            BookUserInterfaceController.Instance.OnBookClosed += _onBookClosedHandler;

            ResetPostProcessingEffect();
        }

        public void OnDisable()
        {
            DialogUserInterfaceController.Instance.OnDialogStart -= SetHidePostProcessingEffect;
            DialogUserInterfaceController.Instance.OnDialogEnd -= ResetPostProcessingEffect;

            BookUserInterfaceController.Instance.OnBookOpened -= _onBookOpenedHandler;
            BookUserInterfaceController.Instance.OnBookClosed -= _onBookClosedHandler;
            
            RitualUserInterfaceController.Instance.OnOpenRitualPanel -= SetHidePostProcessingEffect;
            RitualUserInterfaceController.Instance.OnCloseRitualPanel -= ResetPostProcessingEffect;

        }
        
        public void ResetPostProcessingEffect()
        {
            worldPostProcessing.ResetPostProcessingEffect();
        }

        public void SetHidePostProcessingEffect()
        {
            worldPostProcessing.SetHidePostProcessingEffect();
        }
    }
}
