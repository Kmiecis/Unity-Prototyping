using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.Mathematics
{
	public class Polygon2
	{
		public Vector2[] vs;

		public Polygon2(int vertices = 5)
		{
			vs = new Vector2[vertices];
		}

		public float Distance(Vector2 p)
		{
			float min = float.MaxValue;
			float max = float.MinValue;

			for (int i0 = 0; i0 < vs.Length; ++i0)
			{
				int i1 = (i0 + 1) % vs.Length;
				var v0 = vs[i0];
				var v1 = vs[i1];

				var n = new Vector2(v1.y - v0.y, v0.x - v1.x).normalized;

				float A = Vector2.Dot(n, p);

				float minB = float.MaxValue;
				float maxB = float.MinValue;

				for (int i = 0; i < vs.Length; ++i)
				{
					float B = Vector2.Dot(n, vs[i]);

					minB = Mathf.Min(minB, B);
					maxB = Mathf.Max(maxB, B);
				}

				min = Mathf.Min(min, A - maxB);
				max = Mathf.Max(max, minB - A);
			}

			return Mathx.AbsMin(min, max);
		}

		public Vector2 this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return vs[index]; }
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set { vs[index] = value; }
		}
	}
}
