using System;
using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.Events;

namespace Framework.Controller
{
    public class InterfaceController<T> : BaseController<T> where T : InterfaceController<T>
    {
        public GameObject panel;
        
        public UnityAction OnPanelOpen;
        public UnityAction OnPanelClose;

        public void Start()
        {
            panel.SetActive(false);
        }

        public virtual bool CanOpen()
        {
            return true;
        }
        
        public void OpenPanel()
        {
            if (CanOpen())
            {
                UIEffect panelEffect = panel.GetComponent<UIEffect>();
                if (panelEffect != null)
                {
                    LeanTween.cancelAll(panel);
                    panelEffect.transitionRate = 1f;
                    LeanTween.value(panel, f => panelEffect.transitionRate = f, 1f, 0f, 1.1f).setEaseOutCirc();
                }
                panel.gameObject.SetActive(true);
                OnPanelOpen?.Invoke();
            }
        }

        public void ClosePanel()
        {
            UIEffect panelEffect = panel.GetComponent<UIEffect>();
            if (panelEffect != null)
            {
                LeanTween.cancelAll(panel);
                OnPanelClose?.Invoke();
                panelEffect.transitionRate = 1f;
                LeanTween.value(panel, f => panelEffect.transitionRate = f, 0f, 1f, 1.1f).setEaseOutCirc();
                LeanTween.delayedCall(1.1f, () =>
                {
                    panel.gameObject.SetActive(false);
                });
            }
            else
            {
                panel.gameObject.SetActive(false);
                OnPanelClose?.Invoke();
            }
        }
    }
}