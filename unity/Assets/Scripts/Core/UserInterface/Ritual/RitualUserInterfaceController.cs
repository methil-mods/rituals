using System;
using System.Collections.Generic;
using Framework.Controller;
using ScriptableObjects.Ritual;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UserInterface.Ritual
{
    public class RitualUserInterfaceController : BaseController<RitualUserInterfaceController>
    {
        public List<RitualData> unlockedRituals = new List<RitualData>();
        public GameObject ritualPanel;
        public GameObject makeRitualPanel;
        public GameObject ritualPrefab;
        
        public Button closeButton;

        public void Start()
        {
            makeRitualPanel.SetActive(false);
            closeButton.onClick.AddListener(CloseRitualPanel);
            UpdateRitualUserInterface();
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

            foreach (RitualData ritual in unlockedRituals)
            {
                GameObject ritualGo = Instantiate(ritualPrefab, ritualPanel.transform);
                RitualComponent uiItem = ritualGo.GetComponent<RitualComponent>();
                if (uiItem != null)
                    uiItem.SetRitual(ritual);
            }
        }
        
        public void OpenRitualPanel()
        {
            makeRitualPanel.SetActive(true);
        }

        public void CloseRitualPanel()
        {
            makeRitualPanel.SetActive(false);
        }
    }
}