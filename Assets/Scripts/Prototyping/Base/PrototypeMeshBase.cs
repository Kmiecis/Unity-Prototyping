using Common.Rendering;
using UnityEngine;

namespace Common.Prototyping
{
	public abstract class PrototypeMeshBase : MonoBehaviour, IPrototypeMesh
	{
		public bool IsDirty { get; set; }

		public abstract IMeshData Create();

#if UNITY_EDITOR
		private void OnValidate()
		{
			IsDirty = true;
		}
#endif
	}
}
