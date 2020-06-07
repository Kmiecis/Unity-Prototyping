using UnityEngine;

namespace Common.Rendering
{
	public interface ITextureData
	{
		Color32[] Pixels { get; }
		Texture2D CreateTexture();
	}
}
