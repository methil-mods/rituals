using System.Collections.Generic;
using ScriptableObjects.CNN;
using ScriptableObjects.Ritual;
using UnityEngine;
using Unity.Barracuda;
using System.Linq;

namespace Utils
{
    public static class InferenceUtils
    {
        public static RitualInferenceResult[] RunRitualInference(
            CNNData cnnData, 
            IWorker worker, 
            Texture2D inputTexture, 
            string inputName, 
            string outputName)
        {
            Texture2D resized = TextureUtils.ResizeTexture(inputTexture, 128, 128);
            Color[] pixels = resized.GetPixels();
            float[] grayscale = new float[pixels.Length];

            for (int i = 0; i < pixels.Length; i++)
                grayscale[i] = pixels[i].grayscale;

            using var inputTensor = new Tensor(1, 128, 128, 1, grayscale);
            worker.Execute(new Dictionary<string, Tensor> { { inputName, inputTensor } });
            using Tensor output = worker.PeekOutput(outputName);

            float[] result = output.ToReadOnlyArray();
            MonoBehaviour.Destroy(resized);

            List<RitualData> rituals = cnnData.rituals;
            int minCount = Mathf.Min(result.Length, rituals.Count);

            RitualInferenceResult[] inferenceResults = new RitualInferenceResult[minCount];
            for (int i = 0; i < minCount; i++)
            {
                inferenceResults[i] = new RitualInferenceResult
                {
                    RitualData = rituals[i],
                    similarity = result[i]
                };
            }

            return inferenceResults.OrderByDescending(r => r.similarity).ToArray();
        }
    }

    public class RitualInferenceResult
    {
        public RitualData RitualData;
        public float similarity;
    }
}