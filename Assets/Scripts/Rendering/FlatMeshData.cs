using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.Rendering
{
	public class FlatMeshData : IMeshData, IMeshTriangles
	{
		protected List<Vector3> m_Vertices = new List<Vector3>();
		protected List<int> m_Triangles = new List<int>();
		protected List<Vector3> m_Normals = new List<Vector3>();

		public List<Vector3> Vertices
			=> m_Vertices;

		public List<int> Triangles
			=> m_Triangles;

		public List<Vector3> Normals
			=> m_Normals;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AddTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
		{
			var n = Vector3.Cross(v1 - v0, v2 - v1);
			AddVertex(v0, n);
			AddVertex(v1, n);
			AddVertex(v2, n);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddVertex(Vector3 v, Vector3 n)
		{
			m_Vertices.Add(v);
			m_Triangles.Add(m_Triangles.Count);
			m_Normals.Add(n);
		}

		public virtual void PrepareMesh(Mesh mesh)
		{
			mesh.Clear();
			mesh.SetVertices(m_Vertices);
			mesh.SetTriangles(m_Triangles, 0);
			mesh.SetNormals(m_Normals);
		}

		public Mesh CreateMesh()
		{
			var mesh = new Mesh();
			PrepareMesh(mesh);
			return mesh;
		}

		public virtual void Clear()
		{
			m_Vertices.Clear();
			m_Triangles.Clear();
			m_Normals.Clear();
		}
	}
}
