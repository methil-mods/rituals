using UnityEngine;

namespace Utils
{
    public class FloatingCanvas : MonoBehaviour
    {
        public float amplitude = 0.2f;   // height of the bob
        public float frequency = 1.5f;   // speed of the bob

        private Vector3 startPos;

        void Start()
        {
            startPos = transform.localPosition;
            StartFloating();
        }

        void StartFloating()
        {
            // Animate up
            LeanTween.moveLocalY(gameObject, startPos.y + amplitude, frequency / 2)
                .setEase(LeanTweenType.easeInOutSine)
                .setLoopPingPong();
        }
    }
}