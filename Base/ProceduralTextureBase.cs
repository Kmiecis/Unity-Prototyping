using System.Collections;
using UnityEngine;

namespace Common.Prototyping
{
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class ProceduralTextureBase : MonoBehaviour, IProceduralTexture
    {
        public abstract ITexture2DBuilder Create();

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

                        var texture = material.GetTexture(texturePropertyId);
                        if (Utility.TryCast(texture, out Texture2D texture2D))
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
