using UnityEditor;
using UnityEngine;

namespace Common.Prototyping
{
	[CustomEditor(typeof(ProceduralMeshDisplay))]
	public class ProceduralMeshDisplayEditor : Editor
	{
		protected ProceduralMeshDisplay script
			=> (ProceduralMeshDisplay)target;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Save Mesh as Asset"))
				script.Save();
		}
	}
}
