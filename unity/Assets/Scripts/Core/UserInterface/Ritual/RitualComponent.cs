using ScriptableObjects.Ritual;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UserInterface.Ritual
{
    public class RitualComponent : MonoBehaviour
    {
        public Image ritualImage;
        
        public void SetRitual(RitualData ritual)
        {
            ritualImage.sprite = ritual.ritualImage;    
        }
    }
}