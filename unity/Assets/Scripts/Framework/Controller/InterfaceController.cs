using System;
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
                panel.gameObject.SetActive(true);
                OnPanelOpen?.Invoke();
            }
        }

        public void ClosePanel()
        {
            panel.gameObject.SetActive(false);
            OnPanelClose?.Invoke();
        }
    }
}