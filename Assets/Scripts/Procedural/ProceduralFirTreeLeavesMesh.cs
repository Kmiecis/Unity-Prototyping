using Common.Mathematics;
using Common.Rendering;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Prototyping
{
	public class ProceduralFirTreeLeavesMesh : ProceduralMeshBase
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

				for (int i0 = 0; i0 < HexModel.VCOUNT; ++i0)
				{
					int i1 = Mathx.Next(i0, HexModel.VCOUNT);

					var vertex1 = HexModel.V3[i0];
					var vertex2 = HexModel.V3[i1];

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

					var leaves = (LeavesType)Random.Range(0, (int)LeavesType.ALL);

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
						meshData.AddTriangle(v5, v3, v1);
						meshData.AddUVs(uv5, uv3, uv1);
					}
					else
					{
						var v40 = Vector3.Lerp(v3, v1, input.thickness);
						v3 = Vector3.Lerp(v3, v4, input.thickness);
						var uv40 = new Vector2(0, Mathf.Clamp((v40.y - baseOffset) * invLeavesHeight, 0, 1));

						meshData.AddTriangle(v40, v3, v1);
						meshData.AddUVs(uv3, uv1, uv40);

						// Add sides
						meshData.AddTriangle(v40, v5, v3);
						meshData.AddTriangle(v40, v1, v5);
						meshData.AddUVs(uv3, uv40, uv5);
						meshData.AddUVs(uv5, uv40, uv1);
					}

					if ((leaves & LeavesType.CUT_RIGHT) == LeavesType.NONE)
					{
						// Add side
						meshData.AddTriangle(v2, v4, v6);
						meshData.AddUVs(uv2, uv4, uv6);
					}
					else
					{
						var v30 = Vector3.Lerp(v4, v2, input.thickness);
						v4 = Vector3.Lerp(v4, v3, input.thickness);
						var uv30 = new Vector2(1, Mathf.Clamp((v30.y - baseOffset) * invLeavesHeight, 0, 1));

						meshData.AddTriangle(v2, v4, v30);
						meshData.AddUVs(uv2, uv4, uv30);

						// Add sides
						meshData.AddTriangle(v4, v6, v30);
						meshData.AddTriangle(v6, v2, v30);
						meshData.AddUVs(uv4, uv6, uv30);
						meshData.AddUVs(uv6, uv2, uv30);
					}

					// Add top triangle
					meshData.AddTriangle(vt, v1, v2);
					meshData.AddUVs(uvt, uv1, uv2);

					// Add top quad
					meshData.AddTriangle(v1, v3, v2);
					meshData.AddTriangle(v2, v3, v4);
					meshData.AddUVs(uv1, uv3, uv2);
					meshData.AddUVs(uv2, uv3, uv4);

					// Add bottom quad
					meshData.AddTriangle(v3, v5, v4);
					meshData.AddTriangle(v4, v5, v6);
					meshData.AddUVs(uv3, uv5, uv4);
					meshData.AddUVs(uv4, uv5, uv6);

					// Add bottom triangle
					meshData.AddTriangle(v5, v7, v6);
					meshData.AddUVs(uv5, uv7, uv6);

					// Add remaining left and right side
					meshData.AddTriangle(v7, v5, v1);
					meshData.AddTriangle(v7, v2, v6);
					meshData.AddUVs(uv7, uv5, uv1);
					meshData.AddUVs(uv7, uv2, uv6);
				}
			}

			return meshData;
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
