using UnityEngine;

namespace Utils
{
    public static class CameraUtils
    {
        public static Vector3 ScreenToWorld(Vector2 screenPos)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
            worldPos.z = 0;
            return worldPos;
        }
    }
}