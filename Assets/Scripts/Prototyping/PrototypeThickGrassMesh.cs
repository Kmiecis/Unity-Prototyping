using Common.Mathematics;
using Common.Rendering;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Prototyping
{
	public class PrototypeThickGrassMesh : PrototypeMeshBase
	{
		[Header("Properties")]
		public Input input = Input.Default;
		public int seed;

		public override IMeshData Create()
		{
			var randomState = Random.state;
			Random.InitState(seed);
			var result = Create(in input);
			Random.state = randomState;
			return result;
		}

		public static FlatMeshDataUVs Create(in Input input)
		{
			var meshData = new FlatMeshDataUVs();

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
					CreateSingleGrass(meshData, input, innerInput, new Vector3(offsetX, 0.0f, offsetY));
				}
			}

			return meshData;
		}

		private static void CreateSingleGrass(FlatMeshDataUVs meshData, in Input input, in InnerInput innerInput, Vector3 offset)
		{
			int randomIndex = Random.Range(0, HexModel.VCOUNT);
			var randomIndexOffset = Random.Range(0.0f, 1.0f);
			var randomStrength = (randomIndex + randomIndexOffset) / HexModel.VCOUNT;

			var vertex0 = Geometry.Vertex(randomIndex + 0, HexModel.VCOUNT, randomIndexOffset);
			var vertex1 = Geometry.Vertex(randomIndex + 2, HexModel.VCOUNT, randomIndexOffset);

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

			var s1 = Mathx.Unlerp(0.0f, input.height, v2.y);
			var uv2 = new Vector2(0.0f, s1);
			var uv3 = new Vector2(1.0f, s1);

			var s2 = Mathx.Unlerp(0.0f, input.height, v4.y);
			var uv4 = new Vector2(0.0f, s2);
			var uv5 = new Vector2(1.0f, s2);

			var uv6 = new Vector2(0.5f, 1.0f);

			// Bottom front quad
			meshData.AddTriangle(v0, v2, v3);
			meshData.AddTriangle(v0, v3, v1);
			// Bottom back quad
			meshData.AddTriangle(v1, v3, v0);
			meshData.AddTriangle(v3, v2, v0);
			// Middle front quad
			meshData.AddTriangle(v2, v4, v5);
			meshData.AddTriangle(v2, v5, v3);
			// Middle back quad
			meshData.AddTriangle(v3, v5, v2);
			meshData.AddTriangle(v5, v4, v2);
			// Top front triangle
			meshData.AddTriangle(v4, v6, v5);
			// Top back triangle
			meshData.AddTriangle(v5, v6, v4);

			// Bottom front quad
			meshData.AddUVs(uv0, uv2, uv3);
			meshData.AddUVs(uv0, uv3, uv1);
			// Bottom back quad
			meshData.AddUVs(uv1, uv3, uv0);
			meshData.AddUVs(uv3, uv2, uv0);
			// Middle front quad
			meshData.AddUVs(uv2, uv4, uv5);
			meshData.AddUVs(uv2, uv5, uv3);
			// Middle back quad
			meshData.AddUVs(uv3, uv5, uv2);
			meshData.AddUVs(uv5, uv4, uv2);
			// Top front triangle
			meshData.AddUVs(uv4, uv6, uv5);
			// Top back triangle
			meshData.AddUVs(uv5, uv6, uv4);
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
