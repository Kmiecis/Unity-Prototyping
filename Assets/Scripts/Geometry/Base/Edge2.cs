using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.Mathematics
{
	public struct Edge2
	{
		public Vector2 v0, v1;

		public float Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return Vector2.Distance(v1, v0); }
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Distance(Vector2 v)
		{
			var v0v1 = v1 - v0;
			var v0p = v - v0;
			var pv1 = v1 - v;
			var r = Vector2.Dot(v0v1, v0p) / v0v1.magnitude;
			if (r < 0)
				return v0p.magnitude;
			else if (r > 0)
				return pv1.magnitude;
			var lv0p = v0p.magnitude;
			var lv0v1 = v0v1.magnitude;
			return Mathf.Sqrt(lv0p * lv0p - r * lv0v1 * lv0v1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(Vector2 v)
		{
			return Mathx.Equals(Length, Vector2.Distance(v1, v) + Vector2.Distance(v, v0));
		}

		public bool Equals(Edge2 v)
		{
			return v0.Equals(v.v0) && v1.Equals(v.v1);
		}

		public override bool Equals(object obj)
		{
			return obj is Edge2 && Equals((Edge2)obj);
		}

		public override int GetHashCode()
		{
			return v0.GetHashCode() ^ v1.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[v0: {0}, v1: {1}]", v0, v1);
		}
	}
}
