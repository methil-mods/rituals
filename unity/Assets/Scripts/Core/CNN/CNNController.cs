using System.Collections.Generic;
using Framework.Controller;
using ScriptableObjects.CNN;
using Unity.Barracuda;
using UnityEngine;
using Utils;

namespace Core.CNN
{
    public class CNNController : BaseController<CNNController>
    {
        private const string inputName = "input";
        private const string outputName = "dense_1";
        private IWorker worker;
        private CNNData _cnnData;

        void OnEnable()
        {
            _cnnData = CNNDatabase.Instance.ritualsCNN;
            var runtimeModel = ModelLoader.Load(_cnnData.model);
            worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
        }

        void OnDisable()
        {
            worker?.Dispose();
        }

        public RitualInferenceResult[] RunInference(Texture2D texture)
        {
            return InferenceUtils.RunRitualInference(_cnnData, worker, texture, inputName, outputName);
        }
    }
}
