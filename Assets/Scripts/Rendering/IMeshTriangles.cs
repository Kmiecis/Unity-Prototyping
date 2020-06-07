using UnityEngine;

namespace Common.Rendering
{
	public interface IMeshTriangles
	{
		void AddTriangle(Vector3 v0, Vector3 v1, Vector3 v2);
	}
}
