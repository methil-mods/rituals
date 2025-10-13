using System;
using ScriptableObjects.Entity;
using UnityEngine;

namespace Core.Dialog
{
    [CreateAssetMenu(fileName = "DialogData", menuName = "Dialog/DialogData")]
    public class DialogData : ScriptableObject
    {
        [SerializeField]
        public DialogContent[] contents;
    }

    [Serializable]
    public class DialogContent
    {
        public EntityData entity;
        [TextArea(3, 10)]
        public string content;
        public float typingSpeed = 0.05f;
    }
}