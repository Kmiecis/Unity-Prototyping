using Common.Rendering;
using System;
using UnityEngine;

namespace Common.Prototyping
{
	public class PrototypeGradientTexture : PrototypeTextureBase
	{
		[Header("Properties")]
		public Input input = Input.Default;

		public override ITextureData Create()
		{
			return Create(in input);
		}

		public static TextureData Create(in Input input)
		{
			var textureData = new TextureData(new Vector2Int(input.width, input.height));

			switch (input.axis)
			{
				case Input.Axis.Horizontal:
					for (int x = 0; x < input.width; ++x)
						for (int y = 0; y < input.height; ++y)
							textureData[x, y] = Color32.Lerp(COLOR_WHITE, COLOR_ALPHA, x * 1.0f / input.width);
					break;

				case Input.Axis.Vertical:
					for (int x = 0; x < input.width; ++x)
						for (int y = 0; y < input.height; ++y)
							textureData[x, y] = Color32.Lerp(COLOR_WHITE, COLOR_ALPHA, y * 1.0f / input.height);
					break;
			}

			return textureData;
		}

		[Serializable]
		public struct Input
		{
			[Range(0, 512)] public int width;
			[Range(0, 512)] public int height;
			public Axis axis;

			public static readonly Input Default = new Input
			{
				width = 512,
				height = 512,
				axis = Axis.Horizontal
			};

			public enum Axis { Horizontal, Vertical }
		}
	}
}
