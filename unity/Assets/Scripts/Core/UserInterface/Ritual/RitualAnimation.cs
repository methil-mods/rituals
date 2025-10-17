using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UserInterface.Ritual
{
    public class RitualAnimation : MonoBehaviour
    {
        [SerializeField] private FrameAnimation[] frameAnimations;
        [SerializeField] private Image ritualImage;

        private Coroutine _animationCoroutine;

        private void Start()
        {
            ritualImage.gameObject.SetActive(false);
        }

        [ContextMenu("Play Ritual Animation")]
        public void LaunchAnimation()
        {
            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine = StartCoroutine(AnimateRitual());
        }

        public IEnumerator AnimateRitual()
        {
            if (frameAnimations == null || frameAnimations.Length == 0)
                yield break;

            ritualImage.gameObject.SetActive(true);

            foreach (var frameAnim in frameAnimations)
            {
                if (frameAnim.frame != null)
                    ritualImage.sprite = frameAnim.frame;

                yield return new WaitForSeconds(frameAnim.time);
            }

            ritualImage.gameObject.SetActive(false);

            _animationCoroutine = null;
        }
    }

    [Serializable]
    public class FrameAnimation
    {
        public Sprite frame;
        public float time = 0.1f;
    }
}