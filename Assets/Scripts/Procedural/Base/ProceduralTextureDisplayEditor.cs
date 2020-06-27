using UnityEditor;
using UnityEngine;

namespace Common.Prototyping
{
	[CustomEditor(typeof(ProceduralTextureDisplay))]
	public class ProceduralTextureDisplayEditor : Editor
	{
		protected ProceduralTextureDisplay script
			=> (ProceduralTextureDisplay)target;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Save Texture as Asset"))
				script.Save();
		}
	}
}
