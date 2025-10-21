using System;
using Core.UserInterface;
using Framework.Controller;
using ScriptableObjects.Input;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Core.Game
{
    [Serializable]
    public class PauseController : BaseController<PauseController>
    {
        public UnityAction OnPauseEvent;
        public UnityAction OnResumeEvent;
        public bool isPaused = false;

        private bool _isPauseAllowed = true;
        private bool _havePanelOpened = false;

        private void Start()
        {
            InputDatabase.Instance.pauseAction.action.performed += CallbackPauseBtn;
        }
        
        private void CallbackPauseBtn(InputAction.CallbackContext context)
        {
            if (isPaused)
            {
                PerformResume();
                return;
            }
            PerformPause(true);
        }

        public bool PerformPause(bool displayPauseUI)
        {
            if (!_isPauseAllowed) return false;
            isPaused = true;
            OnPauseEvent?.Invoke();
            if (displayPauseUI)
            {
                PauseUserInterfaceController.Instance?.OpenPanel();
                _havePanelOpened = true;
            }
            return true;
        }

        public bool PerformResume()
        {
            if (!isPaused) return false;
            isPaused = false;
            OnResumeEvent?.Invoke();
            if (_havePanelOpened)
            {
                PauseUserInterfaceController.Instance?.ClosePanel();
                _havePanelOpened = false;
            }
            return true;
        }
    }
}