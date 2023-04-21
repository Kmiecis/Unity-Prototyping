using Common.Extensions;
using Common.Mathematics;
using System;
using UnityEngine;
using Random = System.Random;

namespace Common.Prototyping
{
    public class ProceduralThickGrassMesh : ProceduralMeshBase
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

            for (int y = 0; y < input.count.y; y++)
            {
                float offsetY = input.size.y *(y * 1.0f / input.count.y - 0.5f) + input.size.y * 0.5f / input.count.y;
                for (int x = 0; x < input.count.x; x++)
                {
                    float offsetX = input.size.x * (x * 1.0f / input.count.x - 0.5f) + input.size.x * 0.5f / input.count.x;
                    CreateSingleGrass(meshBuilder, input, innerInput, new Vector3(offsetX, 0.0f, offsetY), ref random);
                }
            }

            return meshBuilder;
        }

        private static void CreateSingleGrass(FlatMeshBuilder meshBuilder, in Input input, in InnerInput innerInput, Vector3 offset, ref Random random)
        {
            int randomIndex = random.Next(0, Hexagons.VERTEX_COUNT);
            var randomIndexOffset = random.NextFloat(0.0f, 1.0f);
            var randomStrength = (randomIndex + randomIndexOffset) / Hexagons.VERTEX_COUNT;

            var u0 = ((randomIndex + 0) * 1.0f / Hexagons.VERTEX_COUNT + randomIndexOffset) * Mathf.PI * 2.0f;
            var u1 = ((randomIndex + 2) * 1.0f / Hexagons.VERTEX_COUNT + randomIndexOffset) * Mathf.PI * 2.0f;
            var vertex0 = Circles.Point(u0).X_Y();
            var vertex1 = Circles.Point(u1).X_Y();

            var h0 = new Vector3(0.0f, innerInput.unbendHeight + innerInput.bendHeightHalf * randomStrength, 0.0f);
            var h1 = new Vector3(0.0f, innerInput.bendHeightHalf, 0.0f);

            var v0 = vertex0 * input.radius + offset;
            var v1 = vertex1 * input.radius + offset;

            var v2 = v0 + h0;
            var v3 = v1 + h0;

            var grassDirection = vertex1 - vertex0;
            var grassAngle = input.curveStrength * 180.0f;
            var rh1 = Quaternion.AngleAxis(grassAngle, grassDirection) * h1;
            var rh2 = Quaternion.AngleAxis(grassAngle * 2.0f, grassDirection) * h1;

            var v4 = v2 + rh1;
            var v5 = v3 + rh1;

            var v6 = (v4 + v5) * 0.5f + rh2;

            var uv0 = new Vector2(0.0f, 0.0f);
            var uv1 = new Vector2(1.0f, 0.0f);

            var s1 = Mathx.InverseLerp(0.0f, input.height, v2.y);
            var uv2 = new Vector2(0.0f, s1);
            var uv3 = new Vector2(1.0f, s1);

            var s2 = Mathx.InverseLerp(0.0f, input.height, v4.y);
            var uv4 = new Vector2(0.0f, s2);
            var uv5 = new Vector2(1.0f, s2);

            var uv6 = new Vector2(0.5f, 1.0f);

            // Bottom front quad
            meshBuilder.AddTriangle(
                v0, v2, v3,
                uv0, uv2, uv3
            );
            meshBuilder.AddTriangle(
                v0, v3, v1,
                uv0, uv3, uv1
            );
            // Bottom back quad
            meshBuilder.AddTriangle(
                v1, v3, v0,
                uv1, uv3, uv0
            );
            meshBuilder.AddTriangle(
                v3, v2, v0,
                uv3, uv2, uv0
            );
            // Middle front quad
            meshBuilder.AddTriangle(
                v2, v4, v5,
                uv2, uv4, uv5
            );
            meshBuilder.AddTriangle(
                v2, v5, v3,
                uv2, uv5, uv3
            );
            // Middle back quad
            meshBuilder.AddTriangle(
                v3, v5, v2,
                uv3, uv5, uv2
            );
            meshBuilder.AddTriangle(
                v5, v4, v2,
                uv5, uv4, uv2
            );
            // Top front triangle
            meshBuilder.AddTriangle(
                v4, v6, v5,
                uv4, uv6, uv5
            );
            // Top back triangle
            meshBuilder.AddTriangle(
                v5, v6, v4,
                uv5, uv6, uv4
            );
        }

        [Serializable]
        public struct Input
        {
            public Vector2Int count;
            public Vector2 size;
            [Range(0.0f, 2.0f)] public float height;
            [Range(0.0f, 0.5f)] public float radius;
            [Range(0.0f, 1.0f)] public float bendThreshold;
            [Range(0.0f, 1.0f)] public float curveStrength;
            public int seed;

            public static readonly Input Default = new Input
            {
                count = new Vector2Int(4, 4),
                size = new Vector2(1.0f, 1.0f),
                height = 1.25f,
                radius = 0.125f,
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
