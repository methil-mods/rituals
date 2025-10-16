using System;
using Framework.Controller;
using ScriptableObjects.Enigma;
using UnityEngine;
using Action = Framework.Action.Action;

namespace Core.Enigma
{
    public class EnigmaController : BaseController<EnigmaController>
    {
        public GameObject enigmaPanel;
        private EnigmaData _enigmaData;
        [NonSerialized]
        public string ActualCode = "";
        
        public void Start()
        {
            
        }

        public void Update()
        {
            
        }

        public void OpenEnigmaPanel(EnigmaData enigmaData)
        {
            ActualCode = "";
            _enigmaData = enigmaData;
            enigmaPanel.SetActive(true);
        }

        public void CloseEnigmaPanel()
        {
            enigmaPanel.SetActive(false);
        }

        public bool ResolveEnigma()
        {
            bool resolved = _enigmaData.TryResolve(ActualCode);
            if (resolved)
                foreach (Action resolveAction in _enigmaData.OnResolveActions)
                { resolveAction.Execute(); }
            return resolved;
        }
    }
}