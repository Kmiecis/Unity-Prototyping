﻿using System;
using UnityEngine;

namespace Common.Prototyping
{
	public class ProceduralWoodenFloorHexMesh : ProceduralMeshBase
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
			
			var uvOffset = new Vector3(HexagonUtility.CENTER_TO_VERTEX, 0.0f, HexagonUtility.CENTER_TO_SIDE);
			var uvMultiplier = new Vector3(0.5f / HexagonUtility.CENTER_TO_VERTEX, 0.0f, 0.5f / HexagonUtility.CENTER_TO_SIDE);

			var vz = new Vector3(0.0f, 0.0f, 0.0f);
			for (int i0 = 0; i0 < HexagonUtility.VCOUNT; i0 += 2)
			{
				int i1 = Mathx.NextIndex(i0, HexagonUtility.VCOUNT);
				int i2 = Mathx.NextIndex(i1, HexagonUtility.VCOUNT);

				var vertex0 = HexagonUtility.V3[i0];
				var vertex1 = HexagonUtility.V3[i1];
				var vertex2 = HexagonUtility.V3[i2];

				var v0 = vertex0;
				var v1 = vertex1;
				var v2 = vertex2;

				var v0vz = (vz - v0) * 1.0f / input.boards;

				var v0v1 = v1 - v0;
				var v1v2 = v2 - v1;

				var rotationOffset = v0v1 * 0.5f;
				var rotationAxis = v1v2.normalized;
				var positiveRotation = Quaternion.AngleAxis(input.angle, rotationAxis);
				var negativeRotation = Quaternion.AngleAxis(input.angle, rotationAxis);

				var shrinkFactor = input.shrink / (input.boards + 1) / (3.0f / 2.0f);
				var v0v1ShrinkStep = v0v1 * shrinkFactor;
				var v1v2ShrinkStep = v1v2 * shrinkFactor;

				for (int i = 0; i < input.boards; ++i)
				{
					var vb0 = v0 + v0vz * i + v0v1ShrinkStep + v1v2ShrinkStep;
					var vb1 = v1 + v0vz * i - v0v1ShrinkStep + v1v2ShrinkStep;

					var isEven = i % 2 == 0;
					var currentRotation = isEven ? positiveRotation : negativeRotation;
					vb0 = currentRotation * (vb0 - rotationOffset) + rotationOffset;
					vb1 = currentRotation * (vb1 - rotationOffset) + rotationOffset;

					var vb2 = vb0 + v0vz - 2 * v1v2ShrinkStep;
					var vb3 = vb1 + v0vz - 2 * v1v2ShrinkStep;

					meshBuilder.AddTriangle(vb0 * input.radius, vb1 * input.radius, vb3 * input.radius);
					meshBuilder.AddTriangle(vb0 * input.radius, vb3 * input.radius, vb2 * input.radius);
					
					var uv0 = Mathx.Multiply(uvMultiplier, vb0 + uvOffset).XZ();
					var uv1 = Mathx.Multiply(uvMultiplier, vb1 + uvOffset).XZ();
					var uv2 = Mathx.Multiply(uvMultiplier, vb2 + uvOffset).XZ();
					var uv3 = Mathx.Multiply(uvMultiplier, vb3 + uvOffset).XZ();

					meshBuilder.AddUVs(uv0, uv1, uv3);
					meshBuilder.AddUVs(uv0, uv3, uv2);
				}
			}

			return meshBuilder;
		}

		[Serializable]
		public struct Input
		{
			[Range(1, 5)] public int boards;
			[Range(0.0f, 2.0f)] public float radius;
			[Range(0.0f, 1.0f)] public float shrink;
			[Range(-10.0f, 10.0f)] public float angle;

			public static Input Default
			{
				get
				{
					return new Input
					{
						boards = 3,
						radius = 1.0f,
						shrink = 0.1f,
						angle = 2.0f
					};
				}
			}
		}
	}
}
