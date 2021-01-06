using Common.Mathematics;
using System;
using UnityEngine;
using Random = System.Random;

namespace Common.Prototyping
{
	// TODO
	public class ProceduralOakTreeTrunkMesh : ProceduralMeshBase
	{
		[Header("Properties")]
		public Input input = Input.Default;

		public override IMeshBuilder Create()
		{
			return Create(in input);
		}

		public static FlatMeshBuilder Create(in Input input)
		{
			var meshBuilder = new FlatMeshBuilder();

			var random = new Random(input.seed);
			var trunkInput = new BranchInput
			{
				height = input.height * 0.5f,
				radius = input.radius,
				offset = Vector3.zero,
				direction = Vector3.up,
				splits = input.splits
			};

			CreateBranch(trunkInput, meshBuilder, ref random);

			return meshBuilder;
		}

		private static void CreateBranch(BranchInput input, FlatMeshBuilder meshBuilder, ref Random random)
		{
			var dh = input.direction * input.height;
			var rotation = Quaternion.FromToRotation(Vector3.up, input.direction);

			for (int i0 = 0; i0 < HexagonUtility.VCOUNT; ++i0)
			{
				int i1 = Mathx.NextIndex(i0, HexagonUtility.VCOUNT);

				var vertex0 = HexagonUtility.V3[i0];
				var vertex1 = HexagonUtility.V3[i1];
				
				var v0r = rotation * vertex0;
				var v1r = rotation * vertex1;

				var v0 = v0r * input.radius + input.offset;
				var v1 = v1r * input.radius + input.offset;

				var v2 = v0 + dh;
				var v3 = v1 + dh;

				meshBuilder.AddTriangle(v0, v1, v3);
				meshBuilder.AddTriangle(v0, v3, v2);
			}

			var splits = input.splits - 1;
			if (splits < 0)
				return;

			var radiusScale = random.NextFloat(0.3f, 0.7f);
			var leftRadius = input.radius * radiusScale;
			var rightRadius = input.radius - leftRadius;

			var leftDirection = Mathx.Direction(random.NextFloat(0.0f, 0.5f), random.NextFloat(0.0f, 1.0f));
			var rightDirection = Mathx.Direction(random.NextFloat(0.0f, 0.5f), random.NextFloat(0.0f, 1.0f));

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

			CreateBranch(leftBranchInput, meshBuilder, ref random);
			CreateBranch(rightBranchInput, meshBuilder, ref random);
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
			public int seed;

			public static readonly Input Default = new Input
			{
				height = 2.5f,
				splits = 5,
				radius = 0.2f
			};
		}
	}
}
