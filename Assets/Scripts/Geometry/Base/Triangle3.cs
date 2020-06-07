using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.Mathematics
{
	public struct Triangle3
	{
		public Vector3 v0, v1, v2;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector3 Weights(Vector3 v)
		{
			var va = v0 - v;
			var vb = v1 - v;
			var vc = v2 - v;

			var fsq = Vector3.Cross(v1 - v0, v2 - v1).sqrMagnitude;
			var fsqa = Vector3.Cross(vb, vc).sqrMagnitude;
			var fsqb = Vector3.Cross(vc, va).sqrMagnitude;
			var fsqc = Vector3.Cross(va, vb).sqrMagnitude;

			return new Vector3(fsqa, fsqb, fsqc);
		}

		public bool Equals(Triangle3 v)
		{
			return v0.Equals(v.v0) && v1.Equals(v.v1) && v2.Equals(v.v2);
		}

		public override bool Equals(object obj)
		{
			return obj is Triangle3 && Equals((Triangle3)obj);
		}

		public override string ToString()
		{
			return string.Format("[v0: {0}, v1: {1}, v2: {2}]", v0, v1, v2);
		}

		public override int GetHashCode()
		{
			return v0.GetHashCode() ^ v1.GetHashCode() ^ v2.GetHashCode();
		}
	}
}
