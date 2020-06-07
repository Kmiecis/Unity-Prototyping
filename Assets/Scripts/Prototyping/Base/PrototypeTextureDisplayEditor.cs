using UnityEditor;
using UnityEngine;

namespace Common.Prototyping
{
	[CustomEditor(typeof(PrototypeTextureDisplay))]
	public class PrototypeTextureDisplayEditor : Editor
	{
		protected PrototypeTextureDisplay script
			=> (PrototypeTextureDisplay)target;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Save Texture as Asset"))
				script.Save();
		}
	}
}
