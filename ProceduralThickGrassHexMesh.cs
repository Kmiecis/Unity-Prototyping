using Common.Extensions;
using Common.Mathematics;
using System;
using UnityEngine;
using Random = System.Random;

namespace Common.Prototyping
{
    public class ProceduralThickGrassHexMesh : ProceduralMeshBase
    {
        [Header("Properties")]
        public Input input = Input.Default;

        public override MeshBuilder Create()
        {
            return Create(in input);
        }

        public static FlatMeshBuilder Create(in Input input)
        {
            var meshBuilder = new FlatMeshBuilder();

            var random = new Random(input.seed);

            var unbendHeight = input.height * input.bendThreshold;
            var bendHeight = input.height - unbendHeight;
            var bendHeightHalf = bendHeight * 0.5f;

            var innerInput = new InnerInput()
            {
                unbendHeight = unbendHeight,
                bendHeight = bendHeight,
                bendHeightHalf = bendHeightHalf
            };

            var initialCoordinates = Vector2Int.zero;
            var initialPosition = Hexagons.Convert(initialCoordinates, input.diameter).X_Y() * 2;
            CreateSingleGrass(input, meshBuilder, innerInput, initialPosition, ref random);
            for (int d = 0; d < (int)Hexagons.Direction.Count; ++d)
            {
                for (int x = 0; x < input.depth; ++x)
                {
                    var nextCoordinates = initialCoordinates + Hexagons.Translations[d] * (x + 1);
                    var nextPosition = Hexagons.Convert(nextCoordinates, input.diameter).X_Y() * 2;
                    CreateSingleGrass(input, meshBuilder, innerInput, nextPosition, ref random);

                    for (int y = 0; y < x; ++y)
                    {
                        var offsetCoordinates = nextCoordinates + Hexagons.Translations[Mathx.IncrIndex(d, (int)Hexagons.Direction.Count, 2)] * (y + 1);
                        var offsetPosition = Hexagons.Convert(offsetCoordinates, input.diameter).X_Y() * 2;
                        CreateSingleGrass(input, meshBuilder, innerInput, offsetPosition, ref random);
                    }
                }
            }

            return meshBuilder;
        }

        private static void CreateSingleGrass(Input input, FlatMeshBuilder meshBuilder, InnerInput innerInput, Vector3 offset, ref Random random)
        {
            int index = random.Next(0, Hexagons.VERTEX_COUNT);
            float indexOffset = random.NextFloat(0f, 1f);

            var u0 = (index * 1.0f / Hexagons.VERTEX_COUNT + indexOffset) * Mathf.PI * 2.0f;
            var u01 = ((index + 1) * 1.0f / Hexagons.VERTEX_COUNT + indexOffset) * Mathf.PI * 2.0f;
            var u1 = ((index + 2) * 1.0f / Hexagons.VERTEX_COUNT + indexOffset) * Mathf.PI * 2.0f;
            var vertex0 = Circles.Point(u0).X_Y();
            var vertex01 = Circles.Point(u01).X_Y();
            var vertex1 = Circles.Point(u1).X_Y();
            var randomizationStrength = (index + indexOffset) / Hexagons.VERTEX_COUNT;

            var direction = vertex1 - vertex0;

            var v0 = vertex0 * input.diameter + offset;
            var v01 = vertex01 * input.diameter + offset;
            var v1 = vertex1 * input.diameter + offset;

            var rh0 = new Vector3(0.0f, innerInput.unbendHeight - innerInput.bendHeightHalf * randomizationStrength, 0.0f);

            var v2 = v0 + rh0;
            var v23 = v01 + rh0;
            var v3 = v1 + rh0;

            var h1 = new Vector3(0.0f, innerInput.bendHeightHalf, 0.0f);
            var angle = input.curveStrength * 180.0f;
            var rh1 = Quaternion.AngleAxis(angle, direction) * h1;
            var rh2 = Quaternion.AngleAxis(angle * 2.0f, direction) * h1;

            var v4 = v2 + rh1;
            var v45 = v23 + rh1;
            var v5 = v3 + rh1;

            var v6 = v45 + rh2;

            var s1 = Mathx.Unlerp(0.0f, input.height, v23.y);
            var s2 = Mathx.Unlerp(0.0f, input.height, v45.y);

            var uv0 = new Vector2(0.0f, 0.0f);
            var uv01 = new Vector2(0.5f, 0.0f);
            var uv1 = new Vector2(1.0f, 0.0f);

            var uv2 = new Vector2(0.0f, s1);
            var uv23 = new Vector2(0.5f, s1);
            var uv3 = new Vector2(1.0f, s1);

            var uv4 = new Vector2(0.0f, s2);
            var uv45 = new Vector2(0.5f, s2);
            var uv5 = new Vector2(1.0f, s2);

            var uv6 = new Vector2(0.5f, 1.0f);

            // Bottom front
            meshBuilder.AddTriangle(v01, v0, v2, uv01, uv0, uv2);
            meshBuilder.AddTriangle(v01, v2, v23, uv01, uv2, uv23);
            meshBuilder.AddTriangle(v01, v23, v3, uv01, uv23, uv3);
            meshBuilder.AddTriangle(v01, v3, v1, uv01, uv3, uv1);
            // Bottom back
            meshBuilder.AddTriangle(v01, v1, v3, uv01, uv1, uv3);
            meshBuilder.AddTriangle(v01, v3, v23, uv01, uv3, uv23);
            meshBuilder.AddTriangle(v01, v23, v2, uv01, uv23, uv2);
            meshBuilder.AddTriangle(v01, v2, v0, uv01, uv2, uv0);

            // Middle front
            meshBuilder.AddTriangle(v23, v2, v4, uv23, uv2, uv4);
            meshBuilder.AddTriangle(v23, v4, v45, uv23, uv4, uv45);
            meshBuilder.AddTriangle(v23, v45, v5, uv23, uv45, uv5);
            meshBuilder.AddTriangle(v23, v5, v3, uv23, uv5, uv3);
            // Middle back
            meshBuilder.AddTriangle(v23, v3, v5, uv23, uv3, uv5);
            meshBuilder.AddTriangle(v23, v5, v45, uv23, uv5, uv45);
            meshBuilder.AddTriangle(v23, v45, v4, uv23, uv45, uv4);
            meshBuilder.AddTriangle(v23, v4, v2, uv23, uv4, uv2);

            // Top front
            meshBuilder.AddTriangle(v45, v4, v6, uv45, uv4, uv6);
            meshBuilder.AddTriangle(v45, v6, v5, uv45, uv6, uv5);
            // Top back
            meshBuilder.AddTriangle(v45, v5, v6, uv45, uv5, uv6);
            meshBuilder.AddTriangle(v45, v6, v4, uv45, uv6, uv4);
        }

        [Serializable]
        public struct Input
        {
            public int depth;
            [Range(0.0f, 5.0f)] public float height;
            [Range(0.0f, 1.0f)] public float diameter;
            [Range(0.0f, 1.0f)] public float bendThreshold;
            [Range(0.0f, 1.0f)] public float curveStrength;
            public int seed;

            public static readonly Input Default = new Input
            {
                depth = 2,
                height = 1.25f,
                diameter = 0.125f,
                bendThreshold = 0.7f,
                curveStrength = 0.2f
            };
        }

        private struct InnerInput
        {
            public float unbendHeight;
            public float bendHeight;
            public float bendHeightHalf;
        }
    }
}
