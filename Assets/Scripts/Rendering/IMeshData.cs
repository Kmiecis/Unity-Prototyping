using UnityEngine;

namespace Common.Rendering
{
	public interface IMeshData
	{
		void PrepareMesh(Mesh mesh);
		Mesh CreateMesh();
	}
}
