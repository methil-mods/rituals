using Core.UserInterface.Book;
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
        
        public void Start()
        {
            worldPostProcessing.Start(this);
            userInterfacePostProcessing.Start(this);
            
            DialogUserInterfaceController.Instance.OnPanelOpen += SetHidePostProcessingEffect;
            DialogUserInterfaceController.Instance.OnPanelClose += ResetPostProcessingEffect;
            
            RitualUserInterfaceController.Instance.OnPanelOpen += SetHidePostProcessingEffect;
            RitualUserInterfaceController.Instance.OnPanelClose += ResetPostProcessingEffect;

            BookUserInterfaceController.Instance.OnPanelOpen += SetHidePostProcessingEffect;
            BookUserInterfaceController.Instance.OnPanelClose += ResetPostProcessingEffect;

            ResetPostProcessingEffect();
        }

        public void OnDisable()
        {
            DialogUserInterfaceController.Instance.OnPanelOpen -= SetHidePostProcessingEffect;
            DialogUserInterfaceController.Instance.OnPanelClose -= ResetPostProcessingEffect;

            BookUserInterfaceController.Instance.OnPanelOpen -= SetHidePostProcessingEffect;
            BookUserInterfaceController.Instance.OnPanelClose -= ResetPostProcessingEffect;
            
            RitualUserInterfaceController.Instance.OnPanelOpen -= SetHidePostProcessingEffect;
            RitualUserInterfaceController.Instance.OnPanelClose -= ResetPostProcessingEffect;

        }
        
        public void ResetPostProcessingEffect()
        {
            worldPostProcessing.ResetPostProcessingEffect();
        }

        public void SetHidePostProcessingEffect()
        {
            worldPostProcessing.SetHidePostProcessingEffect();
        }

        public void SetEndGamePostProcessingEffect()
        {
            worldPostProcessing.SetEndWorldPostProcessingEffect();
        }
    }
}
