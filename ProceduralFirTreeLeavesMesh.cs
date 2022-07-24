using Common.Extensions;
using Common.Mathematics;
using System;
using UnityEngine;
using Random = System.Random;

namespace Common.Prototyping
{
    public class ProceduralFirTreeLeavesMesh : ProceduralMeshBase
    {
        [Header("Properties")]
        public Input input = Input.Default;

        public override MeshBuilder Create()
        {
            return Create(in input);
        }

        public static FlatMeshBuilder Create(in Input input)
        {
            var random = new Random(input.seed);
            var meshBuilder = new FlatMeshBuilder();

            var height = input.height * input.fill;
            var baseHeight = input.height - height;
            var incrHeightStep = height / input.count;
            var incrSmoothStep = input.smoothing / (input.count - 1);

            var invLeavesHeight = 1.0f / height;
            var baseOffset = baseHeight - incrHeightStep * input.overlaps;

            for (int i = 0; i < input.count; ++i)
            {
                var baseLatitude = baseHeight + i * incrHeightStep - incrHeightStep * input.overlaps;
                var topLatitude = baseHeight + (i + 1) * incrHeightStep;
                var splitLatitude = Mathf.Lerp(topLatitude, baseLatitude, input.split);

                var radius = input.radius * (1.0f - i * incrSmoothStep);
                var splitRadius = input.radius * input.split * (1.0f - i * incrSmoothStep);

                var vb = new Vector3(0.0f, baseLatitude, 0.0f);
                var vs = new Vector3(0.0f, splitLatitude, 0.0f);
                var vt = new Vector3(0.0f, topLatitude, 0.0f);

                var uvdb = Mathf.Clamp((vb.y - baseOffset) * invLeavesHeight, 0.0f, 1.0f);
                var uvds = Mathf.Clamp((vs.y - baseOffset) * invLeavesHeight, 0.0f, 1.0f);
                var uvdt = Mathf.Clamp((vt.y - baseOffset) * invLeavesHeight, 0.0f, 1.0f);

                var uvt = new Vector2(0.5f, uvdt);

                var uv1 = new Vector2(0.0f, uvds);
                var uv2 = new Vector2(1.0f, uvds);

                var uv3 = new Vector2(0.0f, uvdb);
                var uv4 = new Vector2(1.0f, uvdb);

                var uv5 = new Vector2(0.0f, uvdb);
                var uv6 = new Vector2(1.0f, uvdb);

                var uv7 = new Vector2(0.5f, uvds);

                var vertices = Hexagons.Vertices;
                for (int i0 = vertices.Length - 1, i1 = 0; i1 < vertices.Length; i0 = i1++)
                {
                    var vertex1 = vertices[i0].X_Y();
                    var vertex2 = vertices[i1].X_Y();

                    // Split vertices
                    var v1 = vertex1 * splitRadius + vs;
                    var v2 = vertex2 * splitRadius + vs;

                    // Outer bottom vertices
                    var v3 = vb;
                    var v4 = vb;

                    // Inner bottom vertices
                    var v5 = vb;
                    var v6 = vb;

                    // Inner top vertex
                    var v7 = vs;

                    var leaves = (LeavesType)random.Next(0, (int)LeavesType.ALL);

                    if ((leaves & LeavesType.OFFSET_LEFT) == LeavesType.NONE)
                    {
                        v3 += vertex1 * radius;
                        v5 += vertex1 * (radius * (1f - input.thickness));
                    }
                    else
                    {
                        v3 += Vector3.Lerp(vertex1, vertex2, input.offset) * radius;
                        v5 += Vector3.Lerp(vertex1, vertex2, input.offset) * (radius * (1f - input.thickness));
                    }

                    if ((leaves & LeavesType.OFFSET_RIGHT) == LeavesType.NONE)
                    {
                        v4 += vertex2 * radius;
                        v6 += vertex2 * (radius * (1f - input.thickness));
                    }
                    else
                    {
                        v4 += Vector3.Lerp(vertex2, vertex1, input.offset) * radius;
                        v6 += Vector3.Lerp(vertex2, vertex1, input.offset) * (radius * (1f - input.thickness));
                    }

                    if ((leaves & LeavesType.CUT_LEFT) == LeavesType.NONE)
                    {
                        // Add side
                        meshBuilder.AddTriangle(
                            v5, v3, v1,
                            uv5, uv3, uv1
                        );
                    }
                    else
                    {
                        var v40 = Vector3.Lerp(v3, v1, input.thickness);
                        v3 = Vector3.Lerp(v3, v4, input.thickness);
                        var uv40 = new Vector2(0, Mathf.Clamp((v40.y - baseOffset) * invLeavesHeight, 0, 1));

                        meshBuilder.AddTriangle(
                            v40, v3, v1,
                            uv3, uv1, uv40
                        );

                        // Add sides
                        meshBuilder.AddTriangle(
                            v40, v5, v3,
                            uv3, uv40, uv5
                        );
                        meshBuilder.AddTriangle(
                            v40, v1, v5,
                            uv5, uv40, uv1
                        );
                    }

                    if ((leaves & LeavesType.CUT_RIGHT) == LeavesType.NONE)
                    {
                        // Add side
                        meshBuilder.AddTriangle(
                            v2, v4, v6,
                            uv2, uv4, uv6
                        );
                    }
                    else
                    {
                        var v30 = Vector3.Lerp(v4, v2, input.thickness);
                        v4 = Vector3.Lerp(v4, v3, input.thickness);
                        var uv30 = new Vector2(1, Mathf.Clamp((v30.y - baseOffset) * invLeavesHeight, 0, 1));

                        meshBuilder.AddTriangle(
                            v2, v4, v30,
                            uv2, uv4, uv30
                        );

                        // Add sides
                        meshBuilder.AddTriangle(
                            v4, v6, v30,
                            uv4, uv6, uv30
                        );
                        meshBuilder.AddTriangle(
                            v6, v2, v30,
                            uv6, uv2, uv30
                        );
                    }

                    // Add top triangle
                    meshBuilder.AddTriangle(
                        vt, v1, v2,
                        uvt, uv1, uv2
                    );

                    // Add top quad
                    meshBuilder.AddTriangle(
                        v1, v3, v2,
                        uv1, uv3, uv2
                    );
                    meshBuilder.AddTriangle(
                        v2, v3, v4,
                        uv2, uv3, uv4
                    );

                    // Add bottom quad
                    meshBuilder.AddTriangle(
                        v3, v5, v4,
                        uv3, uv5, uv4
                    );
                    meshBuilder.AddTriangle(
                        v4, v5, v6,
                        uv4, uv5, uv6
                    );

                    // Add bottom triangle
                    meshBuilder.AddTriangle(
                        v5, v7, v6,
                        uv5, uv7, uv6
                    );

                    // Add remaining left and right side
                    meshBuilder.AddTriangle(
                        v7, v5, v1,
                        uv7, uv5, uv1
                    );
                    meshBuilder.AddTriangle(
                        v7, v2, v6,
                        uv7, uv2, uv6
                    );
                }
            }

            return meshBuilder;
        }

        [Serializable]
        public struct Input
        {
            [Range(0.0f, 10.0f)] public float height;
            [Range(0, 10)] public int count;
            [Range(0.0f, 1.0f)] public float fill;
            [Range(0.0f, 10.0f)] public float radius;
            [Range(0.0f, 1.0f)] public float overlaps;
            [Range(0.0f, 1.0f)] public float smoothing;
            [Range(0.0f, 1.0f)] public float split;
            [Range(0.0f, 1.0f)] public float thickness;
            [Range(0.0f, 1.0f)] public float offset;
            public int seed;

            public static readonly Input Default = new Input
            {
                height = 2.5f,
                count = 4,
                fill = 0.6f,
                radius = 0.55f,
                overlaps = 0.5f,
                smoothing = 0.55f,
                split = 0.715f,
                thickness = 0.2f,
                offset = 0.15f
            };
        }

        private enum LeavesType
        {
            NONE = 0,
            OFFSET_LEFT = 1 << 0,
            OFFSET_RIGHT = 1 << 1,
            CUT_LEFT = 1 << 2,
            CUT_RIGHT = 1 << 3,
            ALL = 1 << 4
        }
    }
}
