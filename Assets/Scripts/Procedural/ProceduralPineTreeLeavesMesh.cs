using Common.Mathematics;
using Common.Rendering;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Prototyping
{
	public class ProceduralPineTreeLeavesMesh : ProceduralMeshBase
	{
		[Header("Properties")]
		public Input input = Input.Default;
		public int seed = 5;

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

				var angleOffset = Random.Range(0f, input.maxAngleOffset);
				for (int i0 = 0; i0 < HexModel.VCOUNT; ++i0)
				{
					int i1 = Mathx.Next(i0, HexModel.VCOUNT);

					var vertex0 = Geometry.Vertex(i1, HexModel.VCOUNT, angleOffset);
					var vertex1 = Geometry.Vertex(i0, HexModel.VCOUNT, angleOffset);

					var v0 = vertex0 * radius + vb;
					var v1 = vertex1 * radius + vb;

					meshData.AddTriangle(v0, v1, vt);

					meshData.AddUVs(uv0, uv1, uvt);

					vertex0 = vertex1;
				}
			}

			return meshData;
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

			public static readonly Input Default = new Input
			{
				height = 2.5f,
				count = 5,
				fill = 0.7f,
				radius = 0.5f,
				overlaps = 0.7f,
				smoothing = 0.55f,
			};
		}
	}
}
