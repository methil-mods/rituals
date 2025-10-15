using Framework.ScriptableObjects;
using UnityEngine;

namespace ScriptableObjects.Materials
{
    [CreateAssetMenu(fileName = "MaterialDatabase", menuName = "Material/MaterialDatabase")]
    public class MaterialDatabase : SingletonScriptableObject<MaterialDatabase>
    {
        public Material tileMapMaterial;
    }
}