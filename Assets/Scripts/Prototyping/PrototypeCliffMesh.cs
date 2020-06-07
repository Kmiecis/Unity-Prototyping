using Common.Mathematics;
using Common.Rendering;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Prototyping
{
	// TODO
	public class PrototypeCliffMesh : PrototypeMeshBase
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

			var vz = new Vector3(0.0f, 0.0f, 0.0f);
			var vh = new Vector3(0.0f, input.height, 0.0f);

			for (int i0 = 0; i0 < HexModel.VCOUNT; ++i0)
			{
				int i1 = Mathx.Next(i0, HexModel.VCOUNT);

				var vertex0 = HexModel.V3[i0];
				var vertex1 = HexModel.V3[i1];

				var v0 = vertex0 * input.radius;
				var v1 = vertex1 * input.radius;
				var v01 = (v0 + v1) * 0.5f;

				var v2 = v0 + vh;
				var v3 = v1 + vh;
				var v23 = (v2 + v3) * 0.5f;

				bool hasBaseCrack = Random.Range(0, 2) == 0;
				if (hasBaseCrack)
				{
					/*
                    var crackSize = Random.Range(0, (int)CrackSize.Count);
                    var crackLengthBegin = input.crackLengthMin;
                    for (int i = 0; i < crackSize; ++i)
                    {
                        var crackLengthEnd = crackLengthBegin + input.crackLengthStep;
                        var randomCrackLength = Random.Range(crackLengthBegin, crackLengthEnd);
                        var randomCrackWidth = Random.Range(input.crackMinOffset, 1f - input.crackMinOffset);
                        var randomCrackDepth = Random.Range(input.crackMinOffset, 1f - input.crackMinOffset);

                        var randomWidthPointDistance = Random.Range(input.crackMinOffset, 1f - randomCrackWidth - input.crackMinOffset);

                        var vc0 = Vector3.Lerp(v0, v1, randomWidthPointDistance);
                        var vc1 = Vector3.Lerp(v0, v1, randomWidthPointDistance + randomCrackWidth);

                        crackLengthBegin += input.crackLengthStep;
                    }
					*/
					var crackSizeMultiplier = 1f;
					var randomCrackLength = Mathf.Max(Random.Range(0f, 1f) * crackSizeMultiplier, input.crackLengthMin);
					var randomCrackWidth = Mathf.Max(Random.Range(0f, 1f) * crackSizeMultiplier, input.crackLengthMin);
					var randomCrackDepth = Mathf.Max(Random.Range(0f, 1f) * crackSizeMultiplier, input.crackLengthMin);

					var randomWidthPointDistance = Mathf.Max(Random.Range(0f, 1f - randomCrackWidth), input.crackMinOffset);
					randomCrackWidth = Mathf.Min(randomCrackWidth, 1f - randomWidthPointDistance - input.crackMinOffset);

					var vc0 = Vector3.Lerp(v0, v1, randomWidthPointDistance);
					var vc1 = Vector3.Lerp(v0, v1, randomWidthPointDistance + randomCrackWidth);

					var vch = Vector3.Lerp(v01, v23, randomCrackLength);
					var vcz = Vector3.Lerp(v01, vz, randomCrackDepth);

					// Right quad
					meshData.AddTriangle(v0, vc0, vch);
					meshData.AddTriangle(v0, vch, v2);

					// Top triangle
					meshData.AddTriangle(v2, vch, v3);

					// Left quad
					meshData.AddTriangle(vc1, v1, v3);
					meshData.AddTriangle(vc1, v3, vch);

					// Depth triangles
					meshData.AddTriangle(vcz, vch, vc0);
					meshData.AddTriangle(vcz, vc1, vch);
				}
				else
				{
					meshData.AddTriangle(v0, v1, v3);
					meshData.AddTriangle(v0, v3, v2);
				}
				// if Base then crack starts between v0 & v1
				// if Edge then crack starts between v2 & v3

				meshData.AddTriangle(v2, v3, vh);
			}

			return meshData;
		}

		private enum CrackSize
		{
			Small,
			Medium,
			Large,
			Count
		}

		[Serializable]
		public struct Input
		{
			[Range(0.0f, 4.0f)] public float height;
			[Range(0.0f, 2.0f)] public float radius;
			[Range(0.0f, 1.0f)] public float crackLengthMin;
			[Range(0.0f, 1.0f)] public float crackLengthStep;
			[Range(0.0f, 0.5f)] public float crackMinOffset;

			public static readonly Input Default = new Input
			{
				height = 2.0f,
				radius = 1.0f,
				crackLengthMin = 0.1f,
				crackLengthStep = 0.25f,
				crackMinOffset = 0.1f
			};
		}
	}
}
