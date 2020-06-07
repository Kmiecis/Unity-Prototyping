using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.Rendering
{
	public class FlatMeshDataUVs : FlatMeshData, IMeshUVs
	{
		protected List<Vector2> m_UVs = new List<Vector2>();

		public List<Vector2> UVs
			=> m_UVs;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddUVs(Vector2 uv0, Vector2 uv1, Vector2 uv2)
		{
			AddUV(uv0);
			AddUV(uv1);
			AddUV(uv2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddUV(Vector2 uv)
		{
			m_UVs.Add(uv);
		}

		public override void PrepareMesh(Mesh mesh)
		{
			base.PrepareMesh(mesh);
			mesh.SetUVs(0, m_UVs);
		}

		public override void Clear()
		{
			base.Clear();
			m_UVs.Clear();
		}
	}
}
