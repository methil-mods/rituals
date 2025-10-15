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

        private ChromaticAberration chromaticAberration;
        private Vignette vignette;


        public void Start(PostProcessingController controller)
        {
            gameObject = controller.gameObject;
            
            if (uiVolume != null)
            {
                uiVolume.TryGet(out chromaticAberration);
                uiVolume.TryGet(out vignette);
            }
        }
    }
}