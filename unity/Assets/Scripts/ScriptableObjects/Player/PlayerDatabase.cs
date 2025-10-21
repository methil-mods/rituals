using Framework.ScriptableObjects;
using UnityEngine;

namespace ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "PlayerDatabase", menuName = "Player/PlayerDatabase")]
    public class PlayerDatabase : SingletonScriptableObject<PlayerDatabase>
    {
        public Sprite[] animationIdleTopRight;
        public Sprite[] animationIdleTopLeft;
        public Sprite[] animationIdleBottomRight;
        public Sprite[] animationIdleBottomLeft;
        
        public Sprite[] animationWalkTopRight;
        public Sprite[] animationWalkTopLeft;
        public Sprite[] animationWalkBottomRight;
        public Sprite[] animationWalkBottomLeft;
    }
}