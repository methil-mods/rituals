using System;
using Framework.Controller;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for URP access

namespace Core.Camera
{
    public class CameraController : BaseController<CameraController>
    {
        [Tooltip("List of cameras managed by this controller.")]
        public UnityEngine.Camera[] cameras;

        [Tooltip("Renderer index to assign (check your URP Renderer List in the Pipeline Asset).")]
        public int rendererIndex = 1; // 0 = default renderer, 1 = another custom renderer, etc.

        private void Start()
        {
            foreach (var camera in cameras)
            {
                if (camera == null) continue;

                var camData = camera.GetComponent<UniversalAdditionalCameraData>();
                if (camData == null)
                    camData = camera.gameObject.AddComponent<UniversalAdditionalCameraData>();

                camData.SetRenderer(rendererIndex);

                Debug.Log($"[CameraController] Set renderer index {rendererIndex} on {camera.name}");
            }
        }
    }
}