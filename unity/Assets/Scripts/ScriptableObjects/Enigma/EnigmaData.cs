using Framework.Action;
using UnityEngine;

namespace ScriptableObjects.Enigma
{
    [CreateAssetMenu(fileName = "EnigmaData", menuName = "Enigma/EnigmaData")]
    public class EnigmaData :  ScriptableObject
    {
        public string enigmaName;
        public string enigmaCode;

        public Action[] OnResolveActions;

        public virtual bool TryResolve(string testCode)
        {
            return enigmaCode == testCode;
        }
    }
}