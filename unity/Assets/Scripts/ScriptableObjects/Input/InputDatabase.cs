using Framework.ScriptableObjects;
using UnityEngine.InputSystem;
using UnityEngine;

namespace ScriptableObjects.Input
{
    [CreateAssetMenu(fileName = "InputDatabase", menuName = "Input/InputDatabase")]
    public class InputDatabase: SingletonScriptableObject<InputDatabase>
    {
        public InputActionReference interactAction;
    }
}