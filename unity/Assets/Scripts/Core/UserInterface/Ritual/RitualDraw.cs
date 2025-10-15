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
            
            canvasTexture.Apply();
        }

        private void ClearCanvas()
        {
            if (canvasTexture == null) return;

            Color[] fill = new Color[canvasTexture.width * canvasTexture.height];
            for (int i = 0; i < fill.Length; i++)
                fill[i] = Color.black;
            canvasTexture.SetPixels(fill);
            canvasTexture.Apply();
        }

        private void FinishRitual()
        {
            var results = CNNController.Instance.RunInference(canvasTexture);
            results.SortByBestMatch();
            var bestMatch = results.GetBestMatch();
            resultText.text = bestMatch.RitualData.ritualName;
            PostProcessingController.Instance.userInterfacePostProcessing.LaunchRitualAnimation();
            /*
            if (bestMatch.RitualData.ritualName == "Not") return;
            DialogUserInterfaceController.Instance.LaunchDialogWithEntity(bestMatch.RitualData.entityData);
            */
        }
        
        private void Update()
        {
            if (!Mouse.current.leftButton.isPressed) return;

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
                    DrawCircle((int)x, (int)y, brushSize, Color.white);
                }
            }
        }
    }
}