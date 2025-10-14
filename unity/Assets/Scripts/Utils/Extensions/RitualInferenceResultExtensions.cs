using System.Linq;
using ScriptableObjects.CNN;

namespace Utils.Extensions
{
    public static class RitualInferenceResultExtensions
    {
        public static RitualInferenceResult GetBestMatch(this RitualInferenceResult[] results)
        {
            if (results == null || results.Length == 0)
                return null;

            return results.OrderByDescending(r => r.similarity).First();
        }

        public static void SortByBestMatch(this RitualInferenceResult[] results)
        {
            if (results == null || results.Length == 0)
                return;

            var sorted = results.OrderByDescending(r => r.similarity).ToArray();
            for (int i = 0; i < results.Length; i++)
                results[i] = sorted[i];
        }
    }
}