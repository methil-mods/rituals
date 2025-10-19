using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Core.CNN;
using TMPro;
using System.Collections.Generic;
using Core.PostProcess;
using Utils;
using Utils.Extensions;

namespace Core.UserInterface.Ritual
{
    public class RitualDraw : MonoBehaviour
    {
        [Header("Canvas & Drawing")]
        public RawImage drawImage;
        public int brushSize = 8;
        [Header("Controls")]
        [SerializeField] private Button finishRitualButton;
        [SerializeField] private Button cleanButton;
        [SerializeField] private TextMeshProUGUI resultText;
        private Texture2D canvasTexture;
        
        // Pour l'interpolation
        private Vector2? lastDrawPosition = null;
        private bool wasDrawing = false;

        public void Start()
        {
            RectTransform rt = drawImage.rectTransform;
            int width = Mathf.RoundToInt(rt.rect.width);
            int height = Mathf.RoundToInt(rt.rect.height);
            canvasTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            ClearCanvas();
            drawImage.texture = canvasTexture;
            finishRitualButton.onClick.AddListener(FinishRitual);
            cleanButton.onClick.AddListener(ClearCanvas);
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
        }
        
        private void DrawLine(Vector2 from, Vector2 to, int radius, Color color)
        {
            float distance = Vector2.Distance(from, to);
            int steps = Mathf.CeilToInt(distance / (radius * 0.5f)); // Dessine tous les demi-rayons pour éviter les trous
            
            for (int i = 0; i <= steps; i++)
            {
                float t = steps > 0 ? i / (float)steps : 0;
                Vector2 pos = Vector2.Lerp(from, to, t);
                DrawCircle((int)pos.x, (int)pos.y, radius, color);
            }
        }

        private void ClearCanvas()
        {
            if (canvasTexture == null) return;

            Color[] fill = new Color[canvasTexture.width * canvasTexture.height];
            for (int i = 0; i < fill.Length; i++)
                fill[i] = Color.black;
            canvasTexture.SetPixels(fill);
            canvasTexture.Apply();
            
            // Reset la position précédente
            lastDrawPosition = null;
            wasDrawing = false;
        }

        private void FinishRitual()
        {
            var results = CNNController.Instance.RunInference(canvasTexture);
            results.SortByBestMatch();
            var bestMatch = results.GetBestMatch();
            resultText.text = bestMatch.RitualData.ritualName;
            if (bestMatch.RitualData.ritualName == "Not") return;
            RitualUserInterfaceController.Instance.LaunchRitualAnimation(bestMatch.RitualData.entityData);
        }

        private void Update()
        {
            bool isDrawing = Mouse.current.leftButton.isPressed;
            
            if (isDrawing)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                RectTransform rt = drawImage.rectTransform;
                Canvas canvas = drawImage.canvas;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, mousePos,
                        canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out Vector2 localPos))
                {
                    float x = localPos.x + rt.rect.width * rt.pivot.x;
                    float y = localPos.y + rt.rect.height * rt.pivot.y;

                    if (x >= 0 && x < canvasTexture.width && y >= 0 && y < canvasTexture.height)
                    {
                        Vector2 currentPos = new Vector2(x, y);
                        
                        // Si on était déjà en train de dessiner, interpoler entre l'ancienne et la nouvelle position
                        if (wasDrawing && lastDrawPosition.HasValue)
                        {
                            DrawLine(lastDrawPosition.Value, currentPos, brushSize, Color.white);
                        }
                        else
                        {
                            // Premier point, juste dessiner un cercle
                            DrawCircle((int)x, (int)y, brushSize, Color.white);
                        }
                        
                        lastDrawPosition = currentPos;
                        canvasTexture.Apply();
                    }
                }
            }
            else
            {
                // Reset quand on relâche le bouton
                lastDrawPosition = null;
            }
            
            wasDrawing = isDrawing;
        }
    }
}