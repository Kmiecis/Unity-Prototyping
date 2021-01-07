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
				var meshBuilder = m_ProceduralMesh.Create();
				m_Mesh = meshBuilder.Build();
			}

			if (m_MeshFilter != null)
				m_MeshFilter.sharedMesh = m_Mesh;
		}

		public void Save()
		{
			m_Mesh = AssetDatabaseUtility.CreateOrReplaceAssetAtPath(m_Mesh, GetPathToAsset());
		}

		private string GetPathToAsset()
		{
			return AssetDatabaseUtility.ConstructPathToAsset(m_AssetPath, m_AssetName, AssetDatabaseUtility.EAssetType.Other);
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
