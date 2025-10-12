using UnityEngine;
using UnityEngine.Events;

namespace Framework.ExtendedGrid
{
    public class GridTile : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer tileSprite;
        public bool IsWalkable;

        public Vector3 hoverRaise = new Vector3(0, 0.1f, 0);
        
        private Vector2Int gridPosition;
        
        public UnityAction OnHoverEnter;
        public UnityAction OnHoverLeave;

        void Awake()
        {
            OnHoverEnter += RaiseTile;
            OnHoverLeave += LowerTile;
        }

        void RaiseTile()
        {
            transform.position += hoverRaise;
        }
        
        void LowerTile()
        {
            transform.position -= hoverRaise;
        }
    }
}