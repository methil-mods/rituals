using UnityEngine;

namespace Core.Scene
{
    public class SceneTransitionButton : MonoBehaviour
    {
        public void ReturnOnSceneInterface()
        {
            SceneTransitor.Instance.LoadScene(0);
        }
    }
}