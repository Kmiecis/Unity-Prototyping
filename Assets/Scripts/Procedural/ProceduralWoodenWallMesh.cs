﻿using Common.Mathematics;
using System;
using UnityEngine;

namespace Common.Prototyping
{
	public class ProceduralWoodenWallMesh : ProceduralMeshBase
	{
		[Header("Properties")]
		public Input input = Input.Default;

		public override IMeshBuilder Create()
		{
			return Create(ref input);
		}

		public static FlatMeshBuilder Create(ref Input input)
		{
			var meshBuilder = new FlatMeshBuilder();

			var frameOffset = new Vector3(input.width * 0.5f, 0.0f, 0.0f);
			var frameHeight = new Vector3(0.0f, input.height, 0.0f);
			var frameSpike = new Vector3(0.0f, input.barSpike, 0.0f);
			var frameLeftBarInput = new BarInput
			{
				angleY = input.frameAngleY,
				radius = input.barRadius,
				offset = -frameOffset,
				height = frameHeight,
				spike = frameSpike
			};
			var frameRightBarInput = new BarInput
			{
				angleY = -input.frameAngleY,
				radius = input.barRadius,
				offset = frameOffset,
				height = frameHeight,
				spike = frameSpike
			};

			AddBar(meshBuilder, frameLeftBarInput);
			AddBar(meshBuilder, frameRightBarInput);

			var topBoardsExtents = new Vector3(input.width, 0.0f, input.boardThickness * 0.5f);
			var topBoardsInput = new BoardsInput
			{
				extents = topBoardsExtents,
				top = frameHeight,
				height = frameHeight,
				count = input.boardsCount
			};

			AddBoards(meshBuilder, topBoardsInput);

			return meshBuilder;
		}

		private static void AddBar(FlatMeshBuilder meshBuilder, BarInput input)
		{
			var barRotationY = Quaternion.AngleAxis(input.angleY, Vector3.up);

			var v0 = -input.spike + input.offset;
			var v5 = input.height + input.spike + input.offset;

			for (int i0 = 0; i0 < SquareUtility.VCOUNT; ++i0)
			{
				int i1 = Mathx.NextIndex(i0, SquareUtility.VCOUNT);

				var vertex0 = SquareUtility.V3[i0];
				var vertex1 = SquareUtility.V3[i1];

				var v1 = barRotationY * vertex0 * input.radius + input.offset;
				var v2 = barRotationY * vertex1 * input.radius + input.offset;
				var v3 = v1 + input.height;
				var v4 = v2 + input.height;

				meshBuilder.AddTriangle(v0, v2, v1);
				meshBuilder.AddTriangle(v1, v2, v4);
				meshBuilder.AddTriangle(v1, v4, v3);
				meshBuilder.AddTriangle(v3, v4, v5);
			}
		}

		private static void AddBoards(FlatMeshBuilder meshBuilder, BoardsInput input)
		{
			var boardVertices = new Vector3[SquareUtility.VCOUNT];
			for (int i = 0; i < boardVertices.Length; ++i)
				boardVertices[i] = Mathx.Multiply(SquareUtility.V3[i], input.extents);

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

				meshBuilder.AddTriangle(v0, v1, v2);
				meshBuilder.AddTriangle(v0, v2, v3);

				meshBuilder.AddTriangle(v1, v4, v5);
				meshBuilder.AddTriangle(v1, v5, v2);

				meshBuilder.AddTriangle(v4, v0, v3);
				meshBuilder.AddTriangle(v4, v3, v5);
			}
		}

		[Serializable]
		public struct Input
		{
			[Range(0.0f, 2.5f)] public float width;
			[Range(0.0f, 2.5f)] public float height;
			[Range(0.0f, 0.5f)] public float barRadius;
			[Range(0.0f, 0.5f)] public float barSpike;
			[Range(-45.0f, 45.0f)] public float frameAngleY;
			[Range(0, 6)] public int boardsCount;
			[Range(0.0f, 0.5f)] public float boardHeight;
			[Range(0.0f, 0.1f)] public float boardThickness;

			public static Input Default
			{
				get
				{
					return new Input
					{
						width = 2.0f,
						height = 2.0f,
						barRadius = 0.1f,
						barSpike = 0.1f,
						frameAngleY = -45.0f,
						boardsCount = 5,
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
			public Vector3 extents;
			public Vector3 top;
			public Vector3 height;
			public int count;
		}
	}
}
