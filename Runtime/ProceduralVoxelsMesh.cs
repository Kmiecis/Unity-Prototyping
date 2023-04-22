using Common.Extensions;
using Common.Mathematics;
using System;
using UnityEngine;

namespace Common.Prototyping
{
    public class ProceduralVoxelsMesh : ProceduralMeshBase
    {
        [Header("Properties")]
        public Input input = Input.Defailt;

        public override MeshBuilder Create()
        {
            return Create(in input);
        }

        public static FlatMeshBuilder Create(in Input input)
        {
            var builder = new FlatMeshBuilder();

            var hs = new Vector2(input.size * 0.5f, input.size * 0.5f);

            var vs = Rects.Vertices;
            var ts = Rects.Triangles;
            for (int y = 0; y < input.size; ++y)
            {
                for (int x = 0; x < input.size; ++x)
                {
                    var o = new Vector2(x, y);
                    var uvo = o / input.size;

                    // Flat surface quads
                    for (int i = 0; i < ts.Length; i += 3)
                    {
                        var t0 = ts[i + 0];
                        var t1 = ts[i + 1];
                        var t2 = ts[i + 2];

                        var v0 = (vs[t0] + o - hs).X_Y() * input.scale;
                        var v1 = (vs[t1] + o - hs).X_Y() * input.scale;
                        var v2 = (vs[t2] + o - hs).X_Y() * input.scale;

                        var uv0 = uvo;
                        var uv1 = uvo;
                        var uv2 = uvo;

                        builder.AddTriangle(
                            v0, v1, v2,
                            uv0, uv1, uv2
                        );
                    }

                    // Joints between quads
                    for (int i0 = ts.Length - 1, i1 = 0; i1 < ts.Length; i0 = i1++)
                    {
                        var t0 = ts[i0];
                        var t1 = ts[i1];

                        var vs0 = vs[t0];
                        var vs1 = vs[t1];

                        // Joint on x axis
                        if (Math.Sign(vs0.x) > 0 &&
                            Math.Sign(vs1.x) > 0 &&
                            (x + 1) < input.size)
                        {
                            var v0 = (vs0 + o - hs).X_Y() * input.scale;
                            var v1 = (vs1 + o - hs).X_Y() * input.scale;

                            var uv0 = uvo;
                            var uv1 = uvo;
                            uv1.x += 1.0f / input.size;

                            builder.AddQuad(
                                v1, v0, v0, v1,
                                Vector3.up, Vector3.up, Vector3.up, Vector3.up,
                                uv0, uv0, uv1, uv1
                            );
                        }

                        // Joint on y axis
                        if (Math.Sign(vs0.y) > 0 &&
                            Math.Sign(vs1.y) > 0 &&
                            (y + 1) < input.size)
                        {
                            var v0 = (vs0 + o - hs).X_Y() * input.scale;
                            var v1 = (vs1 + o - hs).X_Y() * input.scale;

                            var uv0 = uvo;
                            var uv1 = uvo;
                            uv1.y += 1.0f / input.size;

                            builder.AddQuad(
                                v1, v0, v0, v1,
                                Vector3.up, Vector3.up, Vector3.up, Vector3.up,
                                uv0, uv0, uv1, uv1
                            );
                        }
                    }
                }
            }

            return builder;
        }

        [Serializable]
        public struct Input
        {
            [Range(2, 128)] public int size;
            [Range(0.0f, 8.0f)] public float scale;

            public static readonly Input Defailt = new Input
            {
                size = 16,
                scale = 1.0f
            };
        }
    }
}
