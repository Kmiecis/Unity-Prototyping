using Common.Mathematics;
using Common.Rendering;
using System;
using UnityEngine;

namespace Common.Prototyping
{
	public class ProceduralLogMesh : ProceduralMeshBase
	{
		[Header("Parameters")]
		public Input input = Input.Default;

		public override IMeshData Create()
		{
			return Create(in input);
		}

		public static FlatMeshDataUVs Create(in Input input)
		{
			var meshData = new FlatMeshDataUVs();

			var height = new Vector3(0.0f, input.height, 0.0f);
			var rotation = Quaternion.AngleAxis(input.angle, Vector3.up);

			var v0 = new Vector3(0.0f, input.height * input.depth, 0.0f);
			var v5 = new Vector3(0.0f, input.height * (1.0f - input.depth), 0.0f);

			var uv0 = new Vector2(0.5f, 0.0f);
			var uv5 = new Vector2(0.5f, 1.0f);
			var uvStep = 1.0f / input.vertices;

			for (int i0 = 0; i0 < input.vertices; ++i0)
			{
				int i1 = Mathx.Next(i0, input.vertices);

				var vertex0 = Geometry.Vertex(i0, input.vertices);
				var vertex1 = Geometry.Vertex(i1, input.vertices);

				var v1 = vertex0 * input.radius;
				var v2 = vertex1 * input.radius;

				var v3 = rotation * (v1 + height);
				var v4 = rotation * (v2 + height);

				meshData.AddTriangle(v0, v1, v2);

				meshData.AddTriangle(v1, v3, v4);
				meshData.AddTriangle(v1, v4, v2);

				meshData.AddTriangle(v5, v4, v3);

				var uv1 = new Vector2(i0 * uvStep, 0.25f);
				var uv2 = new Vector2(i1 * uvStep, 0.25f);

				var uv3 = new Vector2(uv1.x, 0.75f);
				var uv4 = new Vector2(uv2.x, 0.75f);

				meshData.AddUVs(uv0, uv1, uv2);

				meshData.AddUVs(uv1, uv3, uv4);
				meshData.AddUVs(uv1, uv4, uv2);

				meshData.AddUVs(uv5, uv4, uv3);
			}

			return meshData;
		}

		[Serializable]
		public struct Input
		{
			[Range(3, 8)] public int vertices;
			[Range(0.0f, 2.0f)] public float radius;
			[Range(0.0f, 2.0f)] public float height;
			[Range(0.0f, 0.5f)] public float depth;
			[Range(0.0f, 90.0f)] public float angle;

			public static readonly Input Default = new Input
			{
				vertices = 6,
				radius = 0.15f,
				height = 0.9f,
				depth = 0.1f,
				angle = 30.0f
			};
		}
	}
}
