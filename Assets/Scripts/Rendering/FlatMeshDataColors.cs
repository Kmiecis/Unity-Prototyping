using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.Rendering
{
	public class FlatMeshDataColors : FlatMeshData, IMeshColors
	{
		protected List<Color32> m_Colors = new List<Color32>();

		public List<Color32> Colors
			=> m_Colors;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddColors(Color32 c0, Color32 c1, Color32 c2)
		{
			AddColor(c0);
			AddColor(c1);
			AddColor(c2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddColor(Color32 c)
		{
			m_Colors.Add(c);
		}

		public override void PrepareMesh(Mesh mesh)
		{
			base.PrepareMesh(mesh);
			mesh.SetColors(m_Colors);
		}

		public override void Clear()
		{
			base.Clear();
			m_Colors.Clear();
		}
	}
}
