using System.Collections;
using System.Collections.Generic;
using Core.PostProcess;
using Core.Savior;
using Framework.Controller;
using UnityEngine;
using World;

namespace Core.UserInterface.End
{
    public class EndController : InterfaceController<EndController>
    {
        [SerializeField]
        private SaviorBehaviour savior;

        public void LaunchEndGameAnimation()
        {
            StartCoroutine(LaunchEndGameCoroutine());
        }

        private IEnumerator LaunchEndGameCoroutine()
        {
            savior.animator.SetTrigger("animation");
            PostProcessingController.Instance.SetEndGamePostProcessingEffect();
            WorldController.Instance.HideAllMap();
            yield return new WaitForEndOfFrame();
            // TODO : VERIFY THAT WE CAN'T PAUSE GAME IN THIS ANIMATION
            yield return new WaitForSecondsRealtime(2f);
            EndGame();
        }
        
        public void EndGame()
        {
            this.OpenPanel();
        }
    }
}