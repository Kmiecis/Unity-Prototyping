using System.Collections;
using UnityEngine;

namespace Common.Prototyping
{
	[ExecuteInEditMode]
	public class PrototypeMeshDisplay : MonoBehaviour
	{
#pragma warning disable 0649
		[Header("Display")]
		[SerializeField] private MeshFilter m_MeshFilter;

		[Header("Create")]
		[SerializeField] private PrototypeMeshBase m_PrototypeMesh;
		[SerializeField] private Mesh m_Mesh;

		[Header("Save")]
		[SerializeField] private string m_AssetPath = "References/Meshes";
		[SerializeField] private string m_AssetName = "";
#pragma warning restore

		private void Prototype()
		{
			if (m_PrototypeMesh != null)
			{
				var meshData = m_PrototypeMesh.Create();
				m_Mesh = meshData.CreateMesh();
			}

			if (m_MeshFilter != null)
				m_MeshFilter.sharedMesh = m_Mesh;
		}

		public void Save()
		{
			m_Mesh = AssetDatabaseTools.CreateOrReplaceAssetAtPath(m_Mesh, GetPathToAsset());
		}

		private string GetPathToAsset()
		{
			return AssetDatabaseTools.ConstructPathToAsset(m_AssetPath, m_AssetName, AssetDatabaseTools.EAssetType.Other);
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			StartPrototype();
		}

		private void Update()
		{
			if (m_PrototypeMesh != null && m_PrototypeMesh.IsDirty)
			{
				m_PrototypeMesh.IsDirty = false;
				StartPrototype();
			}
		}

		private void StartPrototype()
		{
			if (gameObject.activeInHierarchy)
				StartCoroutine(DelayedPrototype());
		}

		IEnumerator DelayedPrototype()
		{
			yield return null;
			Prototype();
		}
#endif
	}
}
