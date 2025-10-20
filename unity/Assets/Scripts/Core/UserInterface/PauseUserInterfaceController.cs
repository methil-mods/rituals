using Framework.Controller;
using UnityEngine;

namespace Core.UserInterface
{
    public class PauseUserInterfaceController : BaseController<PauseUserInterfaceController>
    {
        public void SetPauseMenuVisibility(bool visibility)
        {
            gameObject.SetActive(visibility);
        }
    }
}