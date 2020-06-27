using Common.Mathematics;
using Common.Rendering;
using System;
using UnityEngine;

namespace Common.Prototyping
{
	public class ProceduralWoodenDoorFrameMesh : ProceduralMeshBase
	{
		[Header("Properties")]
		public Input input = Input.Default;

		public override IMeshData Create()
		{
			return Create(in input);
		}

		public static FlatMeshData Create(in Input input)
		{
			var meshData = new FlatMeshData();
			
			const float windowBarAngleY = 45.0f;

			var frameOffset = new Vector3(input.width * 0.5f, 0.0f, 0.0f);
			var frameHeight = new Vector3(0.0f, input.height, 0.0f);
			var frameSpike = new Vector3(0.0f, input.frameBarSpike, 0.0f);
			var frameLeftBarInput = new BarInput
			{
				angleY = -input.frameBarAngleY,
				radius = input.frameBarRadius,
				offset = -frameOffset,
				height = frameHeight,
				spike = frameSpike
			};
			var frameRightBarInput = new BarInput
			{
				angleY = input.frameBarAngleY,
				radius = input.frameBarRadius,
				offset = frameOffset,
				height = frameHeight,
				spike = frameSpike
			};

			AddBar(meshData, frameLeftBarInput);
			AddBar(meshData, frameRightBarInput);

			var doorOffset = frameOffset * input.doorWidthFill;
			var doorHeight = frameHeight * input.doorHeightFill;
			var doorLeftBarInput = new BarInput
			{
				angleY = -windowBarAngleY,
				radius = input.doorBarRadius,
				offset = -doorOffset,
				height = doorHeight,
				spike = frameSpike
			};
			var doorRightBarInput = new BarInput
			{
				angleY = windowBarAngleY,
				radius = input.doorBarRadius,
				offset = doorOffset,
				height = doorHeight,
				spike = frameSpike
			};

			AddBar(meshData, doorLeftBarInput);
			AddBar(meshData, doorRightBarInput);

			var topBoardsExtents = new Vector3(input.width, 0.0f, input.boardThickness * 0.5f);
			var topBoardsHeight = frameHeight - doorHeight;
			var topBoardsInput = new BoardsInput
			{
				extents = topBoardsExtents,
				top = frameHeight,
				height = topBoardsHeight,
				count = input.topBoardsCount
			};

			AddBoards(meshData, topBoardsInput);

			var sideBoardsOffset = doorOffset + (frameOffset - doorOffset) * 0.5f;
			var sideBoardsExtents = (topBoardsExtents - doorOffset * 2.0f) * 0.5f;
			var leftSideBoardsInput = new BoardsInput
			{
				offset = -sideBoardsOffset,
				extents = sideBoardsExtents,
				top = doorHeight,
				height = doorHeight,
				count = input.sideBoardsCount
			};
			var rightSideBoardsInput = new BoardsInput
			{
				offset = sideBoardsOffset,
				extents = sideBoardsExtents,
				top = doorHeight,
				height = doorHeight,
				count = input.sideBoardsCount
			};

			AddBoards(meshData, leftSideBoardsInput);
			AddBoards(meshData, rightSideBoardsInput);

			return meshData;
		}

		private static void AddBar(IMeshTriangles meshData, BarInput input)
		{
			var barRotationY = Quaternion.AngleAxis(input.angleY, Vector3.up);

			var v0 = -input.spike + input.offset;
			var v5 = input.height + input.spike + input.offset;

			for (int i0 = 0; i0 < SquareModel.VCOUNT; ++i0)
			{
				int i1 = Mathx.Next(i0, SquareModel.VCOUNT);

				var vertex0 = SquareModel.V3[i0];
				var vertex1 = SquareModel.V3[i1];

				var v1 = barRotationY * vertex0 * input.radius + input.offset;
				var v2 = barRotationY * vertex1 * input.radius + input.offset;
				var v3 = v1 + input.height;
				var v4 = v2 + input.height;

				meshData.AddTriangle(v0, v2, v1);
				meshData.AddTriangle(v1, v2, v4);
				meshData.AddTriangle(v1, v4, v3);
				meshData.AddTriangle(v3, v4, v5);
			}
		}

		private static void AddBoards(IMeshTriangles meshData, BoardsInput input)
		{
			var boardVertices = new Vector3[SquareModel.VCOUNT];
			for (int i = 0; i < boardVertices.Length; ++i)
				boardVertices[i] = Mathx.Mul(SquareModel.V3[i], input.extents) + input.offset;

			var boardHeight = input.height / input.count;
			for (int i = 0; i < input.count; ++i)
			{
				var boardTop = input.top - boardHeight * i;

				var v0 = boardVertices[0] + boardTop;
				var v1 = boardVertices[1] + boardTop;
				var v2 = boardVertices[2] + boardTop;
				var v3 = boardVertices[3] + boardTop;

				var v4 = (v0 + v1) * 0.5f - boardHeight;
				var v5 = (v2 + v3) * 0.5f - boardHeight;

				meshData.AddTriangle(v0, v1, v2);
				meshData.AddTriangle(v0, v2, v3);

				meshData.AddTriangle(v1, v4, v5);
				meshData.AddTriangle(v1, v5, v2);

				meshData.AddTriangle(v4, v0, v3);
				meshData.AddTriangle(v4, v3, v5);
			}
		}

		const int VERTEX_COUNT = 4;

		[Serializable]
		public struct Input
		{
			[Range(0.0f, 2.5f)] public float width;
			[Range(0.0f, 2.5f)] public float height;
			[Range(0.0f, 0.5f)] public float frameBarRadius;
			[Range(0.0f, 0.5f)] public float frameBarSpike;
			[Range(-45.0f, 45.0f)] public float frameBarAngleY;
			[Range(0.0f, 0.5f)] public float doorBarRadius;
			[Range(0.0f, 1.0f)] public float doorHeightFill;
			[Range(0.0f, 1.0f)] public float doorWidthFill;
			[Range(0, 6)] public int topBoardsCount;
			[Range(0, 6)] public int sideBoardsCount;
			[Range(0.0f, 0.1f)] public float boardThickness;

			public static Input Default
			{
				get
				{
					return new Input
					{
						width = 2.0f,
						height = 2.0f,
						frameBarRadius = 0.1f,
						frameBarSpike = 0.1f,
						frameBarAngleY = 30.0f,
						doorBarRadius = 0.05f,
						doorHeightFill = 0.8f,
						doorWidthFill = 0.45f,
						topBoardsCount = 1,
						sideBoardsCount = 4,
						boardThickness = 0.05f
					};
				}
			}
		}

		private struct BarInput
		{
			public Vector3 offset;
			public Vector3 height;
			public Vector3 spike;
			public float angleY;
			public float radius;
		}

		private struct BoardsInput
		{
			public Vector3 offset;
			public Vector3 extents;
			public Vector3 top;
			public Vector3 height;
			public int count;
		}
	}
}
