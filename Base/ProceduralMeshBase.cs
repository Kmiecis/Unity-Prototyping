using System.Collections;
using UnityEngine;

namespace Common.Prototyping
{
    [RequireComponent(typeof(MeshFilter))]
    public abstract class ProceduralMeshBase : MonoBehaviour, IProceduralMesh
    {
        public abstract IMeshBuilder Create();

#if UNITY_EDITOR
        private void OnValidate()
        {
            StartCoroutine(Validate());
        }

        IEnumerator Validate()
        {
            yield return null;

            if (TryGetComponent(out MeshFilter meshFilter))
            {
                var mesh = meshFilter.sharedMesh;

                if (mesh == null)
                    mesh = meshFilter.sharedMesh = new Mesh();

                var meshBuilder = Create();
                meshBuilder.Overwrite(mesh);
            }
            else
            {
                Debug.LogError($"No MeshFilter component on {name}");
            }
        }
#endif
    }
}
