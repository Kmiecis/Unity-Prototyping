﻿using System;
using UnityEngine;

namespace Common.Prototyping
{
    public class ProceduralGradientTexture : ProceduralTextureBase
    {
        [Header("Properties")]
        public Input input = Input.Default;

        public override Texture2DBuilder Create()
        {
            return Create(in input);
        }

        public static Texture2DBuilder Create(in Input input)
        {
            var textureBuilder = new Texture2DBuilder(input.width, input.height);

            switch (input.axis)
            {
                case Input.Axis.Horizontal:
                    for (int x = 0; x < input.width; ++x)
                        for (int y = 0; y < input.height; ++y)
                            textureBuilder[x, y] = Color32.Lerp(Color.white, Color.clear, x * 1.0f / input.width);
                    break;

                case Input.Axis.Vertical:
                    for (int x = 0; x < input.width; ++x)
                        for (int y = 0; y < input.height; ++y)
                            textureBuilder[x, y] = Color32.Lerp(Color.white, Color.clear, y * 1.0f / input.height);
                    break;
            }

            return textureBuilder;
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
