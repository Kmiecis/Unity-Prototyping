using System.Collections;
using UnityEngine;

namespace Common.Prototyping
{
	[ExecuteInEditMode]
	public class ProceduralMeshDisplay : MonoBehaviour
	{
#pragma warning disable 0649
		[Header("Display")]
		[SerializeField] private MeshFilter m_MeshFilter;

		[Header("Create")]
		[SerializeField] private ProceduralMeshBase m_ProceduralMesh;
		[SerializeField] private Mesh m_Mesh;

		[Header("Save")]
		[SerializeField] private string m_AssetPath = "References/Meshes";
		[SerializeField] private string m_AssetName = "";
#pragma warning restore

		private void Procedural()
		{
			if (m_ProceduralMesh != null)
			{
				var meshData = m_ProceduralMesh.Create();
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
			StartProcedural();
		}

		private void Update()
		{
			if (m_ProceduralMesh != null && m_ProceduralMesh.IsDirty)
			{
				m_ProceduralMesh.IsDirty = false;
				StartProcedural();
			}
		}

		private void StartProcedural()
		{
			if (gameObject.activeInHierarchy)
				StartCoroutine(DelayedProcedural());
		}

		IEnumerator DelayedProcedural()
		{
			yield return null;
			Procedural();
		}
#endif
	}
}
