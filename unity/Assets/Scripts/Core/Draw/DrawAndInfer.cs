using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Core.CNN;
using TMPro;
using System.Collections.Generic;
using Utils;

namespace Core.Draw
{
    public class DrawAndInferUI : MonoBehaviour
    {
        public RawImage drawImage;
        public int brushSize = 8;
        public RectTransform debugPanel; // Optional debug UI parent
        public TextMeshProUGUI textPrefab; // Assign a TMP prefab
        private List<TextMeshProUGUI> debugTexts = new List<TextMeshProUGUI>();

        private Texture2D canvasTexture;
        private bool isDrawing;

        private void Start()
        {
            RectTransform rt = drawImage.rectTransform;
            canvasTexture = new Texture2D((int)rt.rect.width, (int)rt.rect.height, TextureFormat.RGBA32, false);
            ClearCanvas();
            drawImage.texture = canvasTexture;
        }

        private void Update()
        {
            if (!Mouse.current.leftButton.isPressed) return;

            Vector2 mousePos = Mouse.current.position.ReadValue();
            RectTransform rt = drawImage.rectTransform;
            Canvas canvas = drawImage.canvas;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, mousePos, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out Vector2 localPos))
            {
                float x = localPos.x + rt.rect.width * rt.pivot.x;
                float y = localPos.y + rt.rect.height * rt.pivot.y;

                DrawCircle((int)x, (int)y, brushSize, Color.white);
                SendToInference();
            }
        }

        private void DrawCircle(int centerX, int centerY, int radius, Color color)
        {
            int sqrRadius = radius * radius;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (x * x + y * y <= sqrRadius)
                    {
                        int px = Mathf.Clamp(centerX + x, 0, canvasTexture.width - 1);
                        int py = Mathf.Clamp(centerY + y, 0, canvasTexture.height - 1);
                        canvasTexture.SetPixel(px, py, color);
                    }
                }
            }
            canvasTexture.Apply();
        }

        private void ClearCanvas()
        {
            Color[] fill = new Color[canvasTexture.width * canvasTexture.height];
            for (int i = 0; i < fill.Length; i++) fill[i] = Color.black;
            canvasTexture.SetPixels(fill);
            canvasTexture.Apply();
        }

        private void SendToInference()
        {
            var results = CNNController.Instance.RunInference(canvasTexture);
            UpdateDebugTexts(results);
        }

        private void UpdateDebugTexts(RitualInferenceResult[] results)
        {
            if (debugPanel == null || textPrefab == null) return;

            // Ensure enough text elements
            while (debugTexts.Count < results.Length)
            {
                var textInstance = Instantiate(textPrefab, debugPanel);
                debugTexts.Add(textInstance);
            }

            // Update or disable extras
            for (int i = 0; i < debugTexts.Count; i++)
            {
                if (i < results.Length)
                {
                    debugTexts[i].gameObject.SetActive(true);
                    debugTexts[i].text = $"{results[i].RitualData.name}: {results[i].similarity:F3}";
                }
                else
                {
                    debugTexts[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
