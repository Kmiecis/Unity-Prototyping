using UnityEngine;

namespace Common.Rendering
{
	public class TextureData : ITextureData
	{
		private int m_Width;
		private int m_Height;
		private Color32[] m_Pixels;

		public Color32[] Pixels
			=> m_Pixels;

		public Color32 this[int i]
		{
			get { return m_Pixels[i]; }
			set { m_Pixels[i] = value; }
		}

		public Color32 this[int x, int y]
		{
			get { return m_Pixels[x + y * m_Width]; }
			set { m_Pixels[x + y * m_Width] = value; }
		}

		public TextureData(Vector2Int extents)
			: this(extents.x, extents.y)
		{
		}

		public TextureData(int width, int height)
		{
			m_Width = width;
			m_Height = height;
			m_Pixels = new Color32[width * height];
		}

		public Texture2D CreateTexture()
		{
			var texture = new Texture2D(m_Width, m_Height);
			texture.SetPixels32(m_Pixels);
			texture.Apply();
			return texture;
		}
	}
}
