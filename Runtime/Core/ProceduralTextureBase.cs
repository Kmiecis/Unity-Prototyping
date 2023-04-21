using Common.Extensions;
using System.Collections;
using UnityEngine;

namespace Common.Prototyping
{
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class ProceduralTextureBase : MonoBehaviour, IProceduralTexture
    {
        public abstract Texture2DBuilder Create();

#if UNITY_EDITOR
        private void OnValidate()
        {
            StartCoroutine(Validate());
        }

        IEnumerator Validate()
        {
            yield return null;

            if (TryGetComponent(out MeshRenderer meshRenderer))
            {
                var materials = meshRenderer.sharedMaterials;

                for (int m = 0; materials != null && m < materials.Length; m++)
                {
                    var material = materials[m];

                    var texturePropertyIds = material.GetTexturePropertyNameIDs();

                    for (int t = 0; texturePropertyIds != null && t < texturePropertyIds.Length; t++)
                    {
                        var texturePropertyId = texturePropertyIds[t];

                        if (material.TryGetTexture(texturePropertyId, out var texture) &&
                            texture is Texture2D texture2D)
                        {
                            var textureBuilder = Create();
                            textureBuilder.Overwrite(texture2D);
                        }
                        else
                        {
                            Debug.LogError($"No Texture2D attached to material in MeshRenderer component on {name} at {texturePropertyId} property id");
                        }
                    }
                }
            }
            else
            {
                Debug.LogError($"No MeshRenderer component on {name}");
            }
        }
#endif
    }
}
