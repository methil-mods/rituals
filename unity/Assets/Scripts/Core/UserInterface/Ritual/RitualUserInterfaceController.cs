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
        public List<RitualData> unlockedRitual = new List<RitualData>();
        public GameObject ritualPanel;
        public GameObject makeRitualPanel;
        public GameObject ritualPrefab;
        
        public Button closeButton;

        public void Start()
        {
            makeRitualPanel.SetActive(false);
            closeButton.onClick.AddListener(CloseRitualPanel);
        }

        private void UpdateRitualUserInterface()
        {
            if (unlockedRitual.Count == 0)
            {
                ritualPanel.SetActive(false);
                return;
            }

            ritualPanel.SetActive(true);

            foreach (Transform child in ritualPanel.transform)
                Destroy(child.gameObject);

            foreach (RitualData ritual in unlockedRitual)
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