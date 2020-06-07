using System.Collections;
using UnityEngine;

namespace Common.Prototyping
{
	[ExecuteInEditMode]
	public class PrototypeTextureDisplay : MonoBehaviour
	{
#pragma warning disable 0649
		[Header("Display")]
		[SerializeField] private MeshRenderer m_MeshRenderer;

		[Header("Create")]
		[SerializeField] private PrototypeTextureBase m_PrototypeTexture;
		[SerializeField] private Texture2D m_Texture;

		[Header("Properties")]
		[SerializeField] private bool m_AlphaIsTransparency;

		[Header("Save")]
		[SerializeField] private string m_AssetPath = "References/Textures";
		[SerializeField] private string m_AssetName = "";
#pragma warning restore

		public void Prototype()
		{
			if (m_PrototypeTexture != null)
			{
				var textureData = m_PrototypeTexture.Create();
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
			StartPrototype();
		}

		private void Update()
		{
			if (m_PrototypeTexture != null && m_PrototypeTexture.IsDirty)
			{
				m_PrototypeTexture.IsDirty = false;
				StartPrototype();
			}
		}

		void StartPrototype()
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
