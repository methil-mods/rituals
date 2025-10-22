using System;
using System.Collections.Generic;
using Coffee.UIEffects;
using Core.PostProcess;
using Framework.Controller;
using ScriptableObjects.Entity;
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
        public RitualAnimation ritualAnimation;
        [Header("Controls")]
        public Button closeButton;

        public new void Start()
        {
            base.Start();
            
            closeButton.onClick.AddListener(ClosePanel);
            
            PauseUserInterfaceController.Instance.OnPanelOpen += ClosePanel;
        }

        private void OnDisable()
        {
            PauseUserInterfaceController.Instance.OnPanelOpen -= ClosePanel;
        }

        public void LaunchRitualAnimation(EntityData entityData)
        {
            PostProcessingController.Instance.userInterfacePostProcessing.LaunchRitualAnimation(
                (() =>
                {
                    DialogUserInterfaceController.Instance.LaunchDialogWithEntity(entityData);
                    
                }));
            LeanTween.delayedCall(0.6f, (() =>
            {
                AnimateEffects(0f, 1f, () => {panel.SetActive(false);});
                ritualAnimation.LaunchAnimation();
            }));
        }

        public void AddRitual(RitualData ritual)
        {
            Debug.Log("Adding ritual of name: " + ritual.ritualName);
            if(unlockedRituals.Contains(ritual)) return;
            unlockedRituals.Add(ritual);
        }

        public void RemoveRitual(RitualData ritual)
        {
            if(unlockedRituals.Contains(ritual)) unlockedRituals.Remove(ritual);
        }
    }
}