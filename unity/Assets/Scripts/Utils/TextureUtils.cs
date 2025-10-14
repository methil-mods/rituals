using UnityEngine;

namespace Utils
{
    public static class TextureUtils
    {
        public static Texture2D ResizeTextureBinary(Texture2D source, int width, int height)
        {
            RenderTexture rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(source, rt);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = rt;

            Texture2D result = new Texture2D(width, height, TextureFormat.RGB24, false);
            result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            
            Color[] pixels = result.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                float brightness = pixels[i].grayscale;
                pixels[i] = brightness > 0.5f ? Color.white : Color.black;
            }
            result.SetPixels(pixels);
            result.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(rt);
            return result;
        }
        
        public static Texture2D ResizeTexture(Texture2D source, int width, int height)
        {
            return ResizeTextureBinary(source, width, height);
        }
    }
}