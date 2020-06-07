using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.Mathematics
{
	public static class Mathx
	{
		public const float ROOT_2 = 1.41421356237f;
		public const float ROOT_3 = 1.73205080757f;
		public const float FLOAT_EQUALITY_EPS = 1e-5f;


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Equals(float a, float b, float e = FLOAT_EQUALITY_EPS)
		{
			return a - e < b && a + e > b;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float AbsMin(float a, float b)
		{
			return Mathf.Abs(a) < Mathf.Abs(b) ? a : b;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float AbsMax(float a, float b)
		{
			return Mathf.Abs(a) > Mathf.Abs(b) ? a : b;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Next(int v, int count, int offset = 1)
		{
			return (v + offset) % count;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Prev(int v, int count, int offset = 1)
		{
			return (v - offset + count) % count;
		}


		/// <summary> Calculates circle direction from single value of range [0, 1] </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Direction(float a)
		{
			var angle = 2 * a * Mathf.PI;
			var x = Mathf.Cos(angle);
			var y = Mathf.Sin(angle);
			return new Vector2(x, y);
		}

		/// <summary> Calculates sphere direction from two values of range [0, 1] </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Direction(float u, float v)
		{
			var omega = 2 * Mathf.PI * u;
			var theta = Mathf.Acos(2 * v - 1);
			var x = Mathf.Sin(theta) * Mathf.Cos(omega);
			var y = Mathf.Sin(theta) * Mathf.Sin(omega);
			var z = Mathf.Cos(theta);
			return new Vector3(x, y, z);
		}


		/// <summary>Returns the result of normalizing a floating point value x to a range [a, b]. The opposite of lerp. Equivalent to (x - a) / (b - a).</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Unlerp(float a, float b, float v)
		{
			return (v - a) / (b - a);
		}

		/// <summary>Returns the componentwise result of normalizing a floating point value x to a range [a, b]. The opposite of lerp. Equivalent to (x - a) / (b - a).</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Unlerp(Vector2 a, Vector2 b, Vector2 v)
		{
			return new Vector2(
				Unlerp(a.x, b.x, v.x),
				Unlerp(a.y, b.y, v.y)
			);
		}

		/// <summary>Returns the componentwise result of normalizing a floating point value x to a range [a, b]. The opposite of lerp. Equivalent to (x - a) / (b - a).</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Unlerp(Vector3 a, Vector3 b, Vector3 v)
		{
			return new Vector3(
				Unlerp(a.x, b.x, v.x),
				Unlerp(a.y, b.y, v.y),
				Unlerp(a.z, b.z, v.z)
			);
		}

		/// <summary>Returns the componentwise result of normalizing a floating point value x to a range [a, b]. The opposite of lerp. Equivalent to (x - a) / (b - a).</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector4 Unlerp(Vector4 a, Vector4 b, Vector4 v)
		{
			return new Vector4(
				Unlerp(a.x, b.x, v.x),
				Unlerp(a.y, b.y, v.y),
				Unlerp(a.z, b.z, v.z),
				Unlerp(a.w, b.w, v.w)
			);
		}
	}
}
