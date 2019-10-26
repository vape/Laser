using UnityEngine;

namespace Laser.Editor.Utilities
{
    public static class EditorUtilities
    {
        public static void ApplyMaterialRecursively(GameObject obj, Material material)
        {
            var meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material = material;
            }

            for (int i = 0; i < obj.transform.childCount; ++i)
            {
                var child = obj.transform.GetChild(i);
                ApplyMaterialRecursively(child.gameObject, material);
            }
        }
    }
}
