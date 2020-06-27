using System.Collections;
using UnityEngine;

namespace Common.Prototyping
{
	[ExecuteInEditMode]
	public class ProceduralTextureDisplay : MonoBehaviour
	{
#pragma warning disable 0649
		[Header("Display")]
		[SerializeField] private MeshRenderer m_MeshRenderer;

		[Header("Create")]
		[SerializeField] private ProceduralTextureBase m_ProceduralTexture;
		[SerializeField] private Texture2D m_Texture;

		[Header("Properties")]
		[SerializeField] private bool m_AlphaIsTransparency;

		[Header("Save")]
		[SerializeField] private string m_AssetPath = "References/Textures";
		[SerializeField] private string m_AssetName = "";
#pragma warning restore

		public void Procedural()
		{
			if (m_ProceduralTexture != null)
			{
				var textureData = m_ProceduralTexture.Create();
				m_Texture = textureData.CreateTexture();

				m_Texture.alphaIsTransparency = m_AlphaIsTransparency;
			}

			if (m_MeshRenderer != null)
				m_MeshRenderer.sharedMaterial.SetTexture("_BaseMap", m_Texture);
		}

		public void Save()
		{
			m_Texture = AssetDatabaseTools.CreateOrReplaceAssetAtPath(m_Texture, GetPathToAsset());
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
			if (m_ProceduralTexture != null && m_ProceduralTexture.IsDirty)
			{
				m_ProceduralTexture.IsDirty = false;
				StartProcedural();
			}
		}

		void StartProcedural()
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
