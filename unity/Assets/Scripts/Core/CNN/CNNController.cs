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
        private IWorker _worker;
        private CNNData _cnnData;

        void OnEnable()
        {
            _cnnData = CNNDatabase.Instance.ritualsCNN;
            var runtimeModel = ModelLoader.Load(_cnnData.model);
            _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
        }

        void OnDisable()
        {
            _worker?.Dispose();
        }

        public RitualInferenceResult[] RunInference(Texture2D texture) =>
             InferenceUtils.RunRitualInference(_cnnData, _worker, texture, _cnnData.InputName, _cnnData.OutputName);
    }
}
