using System;
using System.Collections.Generic;
using Coffee.UIEffects;
using Core.PostProcess;
using Framework.Controller;
using ScriptableObjects.Ritual;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.UserInterface.Ritual
{
    public class RitualUserInterfaceController : InterfaceController<RitualUserInterfaceController>
    {
        public List<RitualData> unlockedRituals = new List<RitualData>();
        [Header("Rituals panel")]
        public GameObject ritualPrefab;
        public GameObject ritualPanel;
        public RitualAnimation ritualAnimation;
        [SerializeField] private GameObject ritualListInPanel;
        [Header("Controls")]
        public Button closeButton;

        public void Start()
        {
            base.Start();
            
            closeButton.onClick.AddListener(ClosePanel);
            DialogUserInterfaceController.Instance.OnDialogStart += ClosePanel;
            
            UpdateRitualUserInterface();
        }

        public void LaunchRitualAnimation()
        {
            PostProcessingController.Instance.userInterfacePostProcessing.LaunchRitualAnimation();
            LeanTween.delayedCall(0.6f, (() =>
            {
                AnimateEffects(0f, 1f, () => {});
                ritualAnimation.LaunchAnimation();
            }));
        }

        public void AddRitual(RitualData ritual)
        {
            if(unlockedRituals.Contains(ritual)) return;
            unlockedRituals.Add(ritual);
            UpdateRitualUserInterface();
        }

        public void RemoveRitual(RitualData ritual)
        {
            if(unlockedRituals.Contains(ritual)) unlockedRituals.Remove(ritual);
            UpdateRitualUserInterface();
        }

        private void UpdateRitualUserInterface()
        {
            if (unlockedRituals.Count == 0)
            {
                ritualPanel.SetActive(false);
                return;
            }

            ritualPanel.SetActive(true);

            foreach (Transform child in ritualPanel.transform)
                Destroy(child.gameObject);
            foreach (Transform child in ritualListInPanel.transform)
                Destroy(child.gameObject);

            foreach (RitualData ritual in unlockedRituals)
            {
                GameObject ritualGo = Instantiate(ritualPrefab, ritualPanel.transform);
                RitualComponent uiItem = ritualGo.GetComponent<RitualComponent>();
                if (uiItem != null) uiItem.SetRitual(ritual);
            }

            foreach (RitualData ritual in unlockedRituals)
            {
                GameObject ritualGo = Instantiate(ritualPrefab, ritualListInPanel.transform);
                RitualComponent uiItem = ritualGo.GetComponent<RitualComponent>();
                uiItem.GetComponent<UIEffectReplica>().target = panel.GetComponent<UIEffect>();
                this.otherEffects.Add(uiItem.GetComponentInChildren<UIEffect>());
                if (uiItem != null) uiItem.SetRitual(ritual);
            }
        }
    }
}