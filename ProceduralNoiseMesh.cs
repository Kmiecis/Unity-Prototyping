using Common.Extensions;
using Common.Mathematics;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Common.Prototyping
{
    public class ProceduralNoiseMesh : ProceduralMeshBase
    {
        [Header("Properties")]
        public Input input = Input.Default;
#if UNITY_EDITOR
        public bool showGizmos;
        private Mesh m_Mesh;
#endif

        public override MeshBuilder Create()
        {
#if UNITY_EDITOR
            var result = Create(in input);
            m_Mesh = result.Build();
            return result;
#else
            return Create(in input);
#endif
        }

        public static FlatMeshBuilder Create(in Input input)
        {
            var meshBuilder = new FlatMeshBuilder();

            var extents = new Vector2Int(input.width, input.height);
            var scale = new Vector2(input.scale, input.scale);
            var noiseMap = new float[extents.x, extents.y];
            Noisex.GetNoiseMap(
                noiseMap,
                input.offset.x, input.offset.y,
                input.octaves, input.persistance, input.lacunarity,
                scale.x, scale.y, input.seed
            );

            float nMaxX = (input.width - 1) * 1.0f / input.width;
            float nMaxY = (input.height - 1) * 1.0f / input.height;

            var corners = new Vector2[]
            {
                new Vector2(0.0f, 0.0f),
                new Vector2(nMaxX, 0.0f),
                new Vector2(nMaxX, nMaxY),
                new Vector2(0.0f, nMaxY)
            };
            
            var revExtents = new Vector2(1.0f / input.width, 1.0f / input.height);

            var meshPoints = new List<Vector2>();
            meshPoints.AddRange(corners);

            for (int y = input.resolution; y < input.height - input.resolution; ++y)
            {
                for (int x = input.resolution; x < input.width - input.resolution; ++x)
                {
                    var noise = noiseMap[x, y];

                    bool isMin = true;
                    bool isMax = true;
                    for (int iy = y - input.resolution; (isMin || isMax) && iy <= y + input.resolution; ++iy)
                    {
                        for (int ix = x - input.resolution; (isMin || isMax) && ix <= x + input.resolution; ++ix)
                        {
                            if (ix == x && iy == y)
                                continue;

                            var inoise = noiseMap[ix, iy];

                            if (inoise >= noise)
                                isMax = false;
                            if (inoise <= noise)
                                isMin = false;
                        }
                    }

                    if (isMin || isMax)
                    {
                        meshPoints.Add(new Vector2(x * revExtents.x, y * revExtents.y));
                    }
                }
            }
            
            var ts = Triangulate.Simple(meshPoints);
            for (int i = 0; i < ts.Count; i += 3)
            {
                var t0 = ts[i + 0];
                var t1 = ts[i + 1];
                var t2 = ts[i + 2];

                var v0 = meshPoints[t0];
                var v1 = meshPoints[t1];
                var v2 = meshPoints[t2];

                var h0 = noiseMap[Mathf.RoundToInt(v0.x * input.width), Mathf.RoundToInt(v0.y * input.height)] * input.multiplier;
                var h1 = noiseMap[Mathf.RoundToInt(v1.x * input.width), Mathf.RoundToInt(v1.y * input.height)] * input.multiplier;
                var h2 = noiseMap[Mathf.RoundToInt(v2.x * input.width), Mathf.RoundToInt(v2.y * input.height)] * input.multiplier;

                meshBuilder.AddTriangle(
                    new Vector3(v0.x, h0, v0.y),
                    new Vector3(v1.x, h1, v1.y),
                    new Vector3(v2.x, h2, v2.y)
                );
            }
            
            return meshBuilder;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (showGizmos && m_Mesh != null)
            {
                var vertices = m_Mesh.vertices;
                var triangles = m_Mesh.triangles;

                var n = Vector3.up;
                for (int t = 0; t < triangles.Length; t += 3)
                {
                    var v0 = vertices[t + 0];
                    var v1 = vertices[t + 1];
                    var v2 = vertices[t + 2];

                    if (Circles.TryCreate(v0.XZ(), v1.XZ(), v2.XZ(), out Vector2 c, out float r))
                    {
                        Handles.DrawWireDisc(c.X_Y() + this.transform.position, n, r);
                    }
                }

                for (int v = 0; v < vertices.Length; ++v)
                {
                    Gizmos.DrawCube(
                        vertices[v] + this.transform.position,
                        Vector3.one * 0.009f
                    );
                }
            }
        }
#endif

        [Serializable]
        public struct Input
        {
            [Range(1, 512)] public int width;
            [Range(1, 512)] public int height;
            public Vector2Int offset;
            [Range(0, 5)] public int octaves;
            [Range(0.0f, 1.0f)] public float persistance;
            [Range(1.0f, 10.0f)] public float lacunarity;
            [Range(0.1f, 100.0f)] public float scale;
            [Range(1, 8)] public int resolution;
            [Range(0.0f, 1.0f)] public float multiplier;
            public int seed;
            
            public static readonly Input Default = new Input
            {
                width = 256,
                height = 256,
                octaves = 3,
                persistance = 0.5f,
                lacunarity = 2.0f,
                scale = 0.5f,
                resolution = 5,
                multiplier = 0.15f
            };
        }
    }
}
