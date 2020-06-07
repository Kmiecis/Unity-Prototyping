using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.Mathematics
{
	public static class Geometry
	{
		/// <summary> Calculates vertex at 'index' of regular polygon with 'count' vertices </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Vertex(int index, int count)
		{
			var angleStep = 2 * Mathf.PI / count;
			var angle = index * angleStep;
			return new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
		}

		/// <summary> Calculates offset vertex at 'index' of regular polygon with 'count' vertices </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Vertex(int index, int count, float offset)
		{
			var angleStep = 2 * Mathf.PI / count;
			var angleOffset = offset * angleStep;
			var angle = index * angleStep + angleOffset;
			return new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
		}

		/// <summary> Projects point 'v' to plane defined by two axes 'ax' and 'ay' </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Project(Vector3 v, Vector3 ax, Vector3 ay)
		{
			float x = Vector3.Dot(ax, v);
			float y = Vector3.Dot(ay, v);
			return new Vector2(x, y);
		}

		/// <summary> Projects point 'v' to plane y </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ProjectY(Vector3 v)
		{
			return Project(v, Vector3.right, Vector3.forward);
		}

		/// <summary> Projects point 'v' to plane z </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ProjectZ(Vector3 v)
		{
			return Project(v, Vector3.right, Vector3.up);
		}

		/// <summary> Projects point 'v' to plane x </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ProjectX(Vector3 v)
		{
			return Project(v, Vector3.back, Vector3.up);
		}
	}
}
