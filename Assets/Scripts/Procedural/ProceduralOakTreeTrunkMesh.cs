using Common.Mathematics;
using Common.Rendering;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Prototyping
{
	// TODO
	public class ProceduralOakTreeTrunkMesh : ProceduralMeshBase
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

		public static FlatMeshData Create(in Input input)
		{
			var meshData = new FlatMeshData();

			var trunkInput = new BranchInput
			{
				height = input.height * 0.5f,
				radius = input.radius,
				offset = Vector3.zero,
				direction = Vector3.up,
				splits = input.splits
			};

			CreateBranch(trunkInput, meshData);

			return meshData;
		}

		private static void CreateBranch(BranchInput input, FlatMeshData meshData)
		{
			var dh = input.direction * input.height;
			var rotation = Quaternion.FromToRotation(Vector3.up, input.direction);

			for (int i0 = 0; i0 < HexModel.VCOUNT; ++i0)
			{
				int i1 = Mathx.Next(i0, HexModel.VCOUNT);

				var vertex0 = HexModel.V3[i0];
				var vertex1 = HexModel.V3[i1];
				
				var v0r = rotation * vertex0;
				var v1r = rotation * vertex1;

				var v0 = v0r * input.radius + input.offset;
				var v1 = v1r * input.radius + input.offset;

				var v2 = v0 + dh;
				var v3 = v1 + dh;

				meshData.AddTriangle(v0, v1, v3);
				meshData.AddTriangle(v0, v3, v2);
			}

			var splits = input.splits - 1;
			if (splits < 0)
				return;

			var radiusScale = Random.Range(0.3f, 0.7f);
			var leftRadius = input.radius * radiusScale;
			var rightRadius = input.radius - leftRadius;

			var leftDirection = Mathx.Direction(Random.Range(0.0f, 0.5f), Random.Range(0.0f, 1.0f));
			var rightDirection = Mathx.Direction(Random.Range(0.0f, 0.5f), Random.Range(0.0f, 1.0f));

			var nextHeight = input.height * 0.5f;

			var leftBranchInput = new BranchInput
			{
				height = nextHeight,
				radius = leftRadius,
				offset = input.offset + dh,
				direction = leftDirection,
				splits = splits
			};

			var rightBranchInput = new BranchInput
			{
				height = nextHeight,
				radius = rightRadius,
				offset = input.offset + dh,
				direction = rightDirection,
				splits = splits
			};

			CreateBranch(leftBranchInput, meshData);
			CreateBranch(rightBranchInput, meshData);
		}

		private struct BranchInput
		{
			public float height;
			public float radius;
			public Vector3 offset;
			public Vector3 direction;
			public int splits;
		}

		[Serializable]
		public struct Input
		{
			[Range(0f, 10f)] public float height;
			[Range(0, 10)] public int splits;
			[Range(0f, 2f)] public float radius;
			[Range(0f, 90f)] public float angle;

			public static readonly Input Default = new Input
			{
				height = 2.5f,
				splits = 5,
				radius = 0.2f
			};
		}
	}
}
