using UnityEditor;
using UnityEngine;

namespace Common.Prototyping
{
	[CustomEditor(typeof(PrototypeMeshDisplay))]
	public class PrototypeMeshDisplayEditor : Editor
	{
		protected PrototypeMeshDisplay script
			=> (PrototypeMeshDisplay)target;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Save Mesh as Asset"))
				script.Save();
		}
	}
}
