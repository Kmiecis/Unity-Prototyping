using Common.Extensions;
using Common.Mathematics;
using System;
using UnityEngine;
using Random = System.Random;

namespace Common.Prototyping
{
    public class ProceduralPineTreeLeavesMesh : ProceduralMeshBase
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

            var height = input.height * input.fill;
            var baseHeight = input.height - height;
            var incrHeightStep = height / input.count;
            var incrSmoothStep = input.smoothing / (input.count - 1);

            var invLeavesHeight = 1.0f / height;
            var baseOffset = baseHeight - incrHeightStep * input.overlaps;

            for (int l = 0; l < input.count; ++l)
            {
                var baseLatitude = baseHeight + l * incrHeightStep - incrHeightStep * input.overlaps;
                var topLatitude = baseHeight + (l + 1) * incrHeightStep;
                var radius = input.radius * (1.0f - l * incrSmoothStep);

                var vb = new Vector3(0.0f, baseLatitude, 0.0f);
                var vt = new Vector3(0.0f, topLatitude, 0.0f);

                var uvdb = Mathf.Clamp((vb.y - baseOffset) * invLeavesHeight, 0.0f, 1.0f);
                var uvdt = Mathf.Clamp((vt.y - baseOffset) * invLeavesHeight, 0.0f, 1.0f);

                var uv0 = new Vector2(0.0f, uvdb);
                var uv1 = new Vector2(1.0f, uvdb);
                var uvt = new Vector2(0.5f, uvdt);

                var angleOffset = random.NextFloat(0f, input.maxAngleOffset);

                var vertices = Hexagons.Vertices;
                for (int i1 = 0, i0 = vertices.Length - 1; i1 < vertices.Length; i0 = i1++)
                {
                    var u0 = (i1 * 1.0f / vertices.Length + angleOffset) * Mathf.PI * 2.0f;
                    var u1 = (i0 * 1.0f / vertices.Length + angleOffset) * Mathf.PI * 2.0f;

                    var vertex0 = Circles.Point(u0).X_Y();
                    var vertex1 = Circles.Point(u1).X_Y();

                    var v0 = vertex0 * radius + vb;
                    var v1 = vertex1 * radius + vb;

                    meshBuilder.AddTriangle(
                        v0, v1, vt,
                        uv0, uv1, uvt
                    );
                }
            }

            return meshBuilder;
        }

        [Serializable]
        public struct Input
        {
            [Range(0f, 10f)] public float height;
            [Range(0, 10)] public int count;
            [Range(0f, 1f)] public float fill;
            [Range(0f, 10f)] public float radius;
            [Range(0f, 1f)] public float overlaps;
            [Range(0f, 1f)] public float smoothing;
            [Range(0f, 1f)] public float maxAngleOffset;
            public int seed;

            public static readonly Input Default = new Input
            {
                height = 2.5f,
                count = 5,
                fill = 0.7f,
                radius = 0.5f,
                overlaps = 0.7f,
                smoothing = 0.55f,
                seed = 5
            };
        }
    }
}
