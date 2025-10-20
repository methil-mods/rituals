using System;
using Core.UserInterface;
using Framework.Controller;
using UnityEngine.Events;

namespace Core.Game
{
    [Serializable]
    public class PauseController : BaseController<PauseController>
    {
        public UnityAction OnPauseEvent;
        public UnityAction OnResumeEvent;
        public bool isPaused = false;

        private bool _isPauseAllowed = true;

        public bool PerformPause(bool displayPauseUI)
        {
            if (!_isPauseAllowed) return false;
            isPaused = true;
            OnPauseEvent?.Invoke();
            PauseUserInterfaceController.Instance?.SetPauseMenuVisibility(displayPauseUI);
            return true;
        }

        public bool PerformResume()
        {
            if (!isPaused) return false;
            isPaused = false;
            OnResumeEvent?.Invoke();
            PauseUserInterfaceController.Instance?.SetPauseMenuVisibility(false);
            return true;
        }
    }
}