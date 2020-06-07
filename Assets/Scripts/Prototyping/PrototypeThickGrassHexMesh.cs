using Common.Mathematics;
using Common.Rendering;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Prototyping
{
	public class PrototypeThickGrassHexMesh : PrototypeMeshBase
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

			var initialCoordinates = Vector2Int.zero;
			var initialPosition = HexModel.Convert(initialCoordinates) * input.radius * 2;
			CreateSingleGrass(input, meshData, innerInput, initialPosition);
			for (int d = 0; d < (int)HexModel.Direction.Count; ++d)
			{
				for (int x = 0; x < input.depth; ++x)
				{
					var nextCoordinates = initialCoordinates + HexModel.T2[d] * (x + 1);
					var nextPosition = HexModel.Convert(nextCoordinates) * input.radius * 2;
					CreateSingleGrass(input, meshData, innerInput, nextPosition);

					for (int y = 0; y < x; ++y)
					{
						var offsetCoordinates = nextCoordinates + HexModel.T2[Mathx.Next(d, (int)HexModel.Direction.Count, 2)] * (y + 1);
						var offsetPosition = HexModel.Convert(offsetCoordinates) * input.radius * 2;
						CreateSingleGrass(input, meshData, innerInput, offsetPosition);
					}
				}
			}

			return meshData;
		}

		private static void CreateSingleGrass(Input input, FlatMeshDataUVs meshData, InnerInput innerInput, Vector3 offset)
		{
			int index = Random.Range(0, HexModel.VCOUNT);
			float indexOffset = Random.Range(0f, 1f);

			var vertex0 = Geometry.Vertex(index, HexModel.VCOUNT, indexOffset);
			var vertex01 = Geometry.Vertex(index + 1, HexModel.VCOUNT, indexOffset);
			var vertex1 = Geometry.Vertex(index + 2, HexModel.VCOUNT, indexOffset);
			var randomizationStrength = (index + indexOffset) / HexModel.VCOUNT;

			var direction = vertex1 - vertex0;

			var v0 = vertex0 * input.radius + offset;
			var v01 = vertex01 * input.radius + offset;
			var v1 = vertex1 * input.radius + offset;

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

			// Bottom front
			meshData.AddTriangle(v01, v0, v2);
			meshData.AddTriangle(v01, v2, v23);
			meshData.AddTriangle(v01, v23, v3);
			meshData.AddTriangle(v01, v3, v1);
			// Bottom back
			meshData.AddTriangle(v01, v1, v3);
			meshData.AddTriangle(v01, v3, v23);
			meshData.AddTriangle(v01, v23, v2);
			meshData.AddTriangle(v01, v2, v0);

			// Middle front
			meshData.AddTriangle(v23, v2, v4);
			meshData.AddTriangle(v23, v4, v45);
			meshData.AddTriangle(v23, v45, v5);
			meshData.AddTriangle(v23, v5, v3);
			// Middle back
			meshData.AddTriangle(v23, v3, v5);
			meshData.AddTriangle(v23, v5, v45);
			meshData.AddTriangle(v23, v45, v4);
			meshData.AddTriangle(v23, v4, v2);

			// Top front
			meshData.AddTriangle(v45, v4, v6);
			meshData.AddTriangle(v45, v6, v5);
			// Top back
			meshData.AddTriangle(v45, v5, v6);
			meshData.AddTriangle(v45, v6, v4);

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
			meshData.AddUVs(uv01, uv0, uv2);
			meshData.AddUVs(uv01, uv2, uv23);
			meshData.AddUVs(uv01, uv23, uv3);
			meshData.AddUVs(uv01, uv3, uv1);
			// Bottom back
			meshData.AddUVs(uv01, uv1, uv3);
			meshData.AddUVs(uv01, uv3, uv23);
			meshData.AddUVs(uv01, uv23, uv2);
			meshData.AddUVs(uv01, uv2, uv0);

			// Middle front
			meshData.AddUVs(uv23, uv2, uv4);
			meshData.AddUVs(uv23, uv4, uv45);
			meshData.AddUVs(uv23, uv45, uv5);
			meshData.AddUVs(uv23, uv5, uv3);
			// Middle back
			meshData.AddUVs(uv23, uv3, uv5);
			meshData.AddUVs(uv23, uv5, uv45);
			meshData.AddUVs(uv23, uv45, uv4);
			meshData.AddUVs(uv23, uv4, uv2);

			// Top front
			meshData.AddUVs(uv45, uv4, uv6);
			meshData.AddUVs(uv45, uv6, uv5);
			// Top back
			meshData.AddUVs(uv45, uv5, uv6);
			meshData.AddUVs(uv45, uv6, uv4);
		}

		[Serializable]
		public struct Input
		{
			public int depth;
			[Range(0.0f, 5.0f)] public float height;
			[Range(0.0f, 1.0f)] public float radius;
			[Range(0.0f, 1.0f)] public float bendThreshold;
			[Range(0.0f, 1.0f)] public float curveStrength;

			public static readonly Input Default = new Input
			{
				depth = 2,
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
