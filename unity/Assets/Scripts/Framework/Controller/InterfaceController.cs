using System;
using System.Collections.Generic;
using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Controller
{
    public class InterfaceController<T> : BaseController<T> where T : InterfaceController<T>
    {
        [SerializeField] protected GameObject panel;
        [SerializeField] protected List<UIEffect> otherEffects = new();

        public UnityAction OnPanelOpen;
        public UnityAction OnPanelClose;

        protected UIEffect _panelEffect;
        protected const float TransitionDuration = 1.1f;

        protected void Start()
        {
            if (panel != null)
            {
                _panelEffect = panel.GetComponent<UIEffect>();
                panel.SetActive(false);
            }
        }

        public virtual bool CanOpen() => true;

        public void OpenPanel()
        {
            if (!CanOpen() || panel == null) return;
            panel.SetActive(true);
            OnPanelOpen?.Invoke();
            AnimateEffects(1f, 0f, null);
        }

        public void ClosePanel()
        {
            if (panel == null)
            {
                OnPanelClose?.Invoke();
                return;
            }

            OnPanelClose?.Invoke();
            AnimateEffects(0f, 1f, () => panel.SetActive(false));
        }

        protected void AnimateEffects(float from, float to, System.Action onComplete)
        {
            if (_panelEffect == null && otherEffects.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            LeanTween.cancel(panel);
            LeanTween.value(panel, from, to, TransitionDuration)
                .setEaseOutCirc()
                .setOnUpdate((float value) =>
                {
                    if (_panelEffect != null)
                        _panelEffect.transitionRate = value;

                    foreach (var effect in otherEffects)
                        if (effect != null)
                            effect.transitionRate = value;
                })
                .setOnComplete(() => onComplete?.Invoke());
        }
    }
}
