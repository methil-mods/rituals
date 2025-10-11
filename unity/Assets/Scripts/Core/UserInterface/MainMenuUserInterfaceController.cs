using Coffee.UIEffects;
using Core.Scene;
using Framework.Controller;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Core.UserInterface
{
    public class MainMenuUserInterfaceController : BaseController<MainMenuUserInterfaceController>
    {
        public UIEffect mainMenuEffect;
        public Image mainMenuLogo;
        public Button mainMenuButton;
        [SerializeField]
        public int sceneToLoadIndex;

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

            mainMenuButton.onClick.AddListener(LaunchScene);
        }

        public void LaunchScene()
        {
            // Animate fire
            LeanTween.value(this.gameObject, f => mainMenuEffect.transitionRate = f, 0.345f, 1f, 1.2f)
                .setEase(LeanTweenType.easeInOutCirc);
            // Hide text logo
            LeanTween.value(this.gameObject, f =>
                { mainMenuLogo.color = new Color(mainMenuLogo.color.r, mainMenuLogo.color.g, mainMenuLogo.color.b, f); }, 1f, 0f, 1f)
                .setEase(LeanTweenType.easeInOutCirc);

            LeanTween.delayedCall(1f, () =>
            {
                SceneTransitor.Instance.LoadScene(sceneToLoadIndex);
            });
        }
    }
}