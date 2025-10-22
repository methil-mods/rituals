using System;
using Coffee.UIEffects;
using Core.UserInterface;
using Framework.Controller;
using ScriptableObjects.Enigma;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Action = Framework.Action.Action;

namespace Core.UserInterfae.Enigma
{
    public class EnigmaController : InterfaceController<EnigmaController>
    {
        public RectTransform numericPanel;
        public TextMeshProUGUI actualResultText;
        public Button buttonReset;
        public Button buttonSubmit;
        public Button buttonClose;
        public Sprite[] numericSprites;

        private EnigmaData _enigmaData;
        [NonSerialized] public string ActualCode = "";

        private new void Start()
        {
            base.Start();
            
            foreach (Transform child in numericPanel)
                Destroy(child.gameObject);

            for (int i = 0; i < numericSprites.Length; i++)
            {
                int number = i;
                GameObject buttonObj = new GameObject("Button_" + number+1, typeof(Image), typeof(Button));
                buttonObj.transform.SetParent(numericPanel, false);

                Image image = buttonObj.GetComponent<Image>();
                image.sprite = numericSprites[number];
                image.preserveAspect = true;

                Button btn = buttonObj.GetComponent<Button>();
                btn.onClick.AddListener(() => OnNumberClicked(number+1));

                RectTransform rect = buttonObj.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(100, 100);
                buttonObj.AddComponent<UIEffectReplica>().target = panel.GetComponent<UIEffect>();
            }

            buttonReset.onClick.AddListener(ResetCode);
            buttonClose.onClick.AddListener(CloseEnigmaPanel);
            buttonSubmit.onClick.AddListener(TryResolveEnigma);
            
            PauseUserInterfaceController.Instance.OnPanelOpen += ClosePanel;
        }

        private void OnDisable()
        {
            PauseUserInterfaceController.Instance.OnPanelOpen -= ClosePanel;
        }

        private void OnNumberClicked(int number)
        {
            ActualCode += number.ToString();
            actualResultText.text = ActualCode;
        }

        private void ResetCode()
        {
            ActualCode = "";
            actualResultText.text = "";
        }

        public void OpenEnigmaPanel(EnigmaData enigmaData)
        {
            ActualCode = "";
            _enigmaData = enigmaData;
            actualResultText.text = "";
            this.OpenPanel();
        }

        public void CloseEnigmaPanel()
        {
            ResetCode();
            this.ClosePanel();
        }

        public void TryResolveEnigma()
        {
            bool resolved = ResolveEnigma();
            if(resolved) CloseEnigmaPanel();
            else ResetCode();
        }

        public bool ResolveEnigma()
        {
            bool resolved = _enigmaData.TryResolve(ActualCode);
            if (resolved)
            {
                foreach (Action resolveAction in _enigmaData.OnResolveActions)
                {
                    resolveAction.Execute();
                }
            }
            return resolved;
        }
    }
}
