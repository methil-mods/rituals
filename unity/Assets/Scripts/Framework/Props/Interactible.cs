using UnityEngine;
using UnityEngine.Events;

namespace Framework.Props
{
    public abstract class Interactible<T> : MonoBehaviour where T : Interactible<T>
    {
        public UnityAction<T> OnInteract;
        
        public void Interact()
        {
            OnInteract?.Invoke((T)this);
        }
    }
}