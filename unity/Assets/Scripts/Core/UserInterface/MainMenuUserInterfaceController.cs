using Coffee.UIEffects;
using Framework.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UserInterface
{
    public class MainMenuUserInterfaceController : BaseController<MainMenuUserInterfaceController>
    {
        public UIEffect mainMenuEffect;
        public Image mainMenuLogo;

        public void Start()
        {
            mainMenuEffect.transitionRate = 0f;
            // Transition for main menu ui effect
            LeanTween.value(this.gameObject, f => mainMenuEffect.transitionRate = f, 0f, 0.345f, 1f)
                .setEase(LeanTweenType.easeOutExpo);
            
            mainMenuLogo.color = new Color(mainMenuLogo.color.r, mainMenuLogo.color.g, mainMenuLogo.color.b, 0f);
            // Transition for the main menu logo
            LeanTween.value(this.gameObject, f =>
                {
                    mainMenuLogo.color = new Color(mainMenuLogo.color.r, mainMenuLogo.color.g, mainMenuLogo.color.b, f);
                }, 0f, 1f, 1f)
                .setEase(LeanTweenType.easeOutExpo);
        }
    }
}