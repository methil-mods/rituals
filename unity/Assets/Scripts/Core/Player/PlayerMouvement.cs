using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Player
{
    public class PlayerMouvement : MonoBehaviour
    {
        public Tilemap tilemap;
        private void Update()
        {
            if (!Mouse.current.leftButton.isPressed) return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector2 worldClickPos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3 tilePos = tilemap.WorldToCell(worldClickPos);
            Debug.Log(tilePos.ToString());
            tilePos.z = 0;
            transform.position = tilePos;
        }

    }
}
