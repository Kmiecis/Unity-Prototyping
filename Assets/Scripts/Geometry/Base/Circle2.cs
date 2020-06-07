using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common.Mathematics
{
	public struct Circle2
	{
		public Vector2 position;
		public float radius;

		public float Diameter
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return radius * 2.0f; }
		}

		public float Area
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return Mathf.PI * radius * radius; }
		}

		public float Circumference
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return 2 * Mathf.PI * radius; }
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Distance(Vector2 v)
		{
			return Vector2.Distance(position, v) - radius;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Contains(Vector2 v)
		{
			var dx = position.x - v.x;
			var dy = position.y - v.y;
			return dx * dx + dy * dy < radius * radius;
		}

		public static Circle2 Create(Vector2 v0, Vector2 v1, Vector2 v2)
		{
			var m1c = new Vector4(v0.x * v0.x + v0.y * v0.y, v1.x * v1.x + v1.y * v1.y, v2.x * v2.x + v2.y * v2.y, 0.0f);
			var m2c = new Vector4(v0.x, v1.x, v2.x, 0.0f);
			var m3c = new Vector4(v0.y, v1.y, v2.y, 0.0f);
			var m4c = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
			var m5c = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
			
			var M11 = new Matrix4x4(m2c, m3c, m4c, m5c);
			var M12 = new Matrix4x4(m1c, m3c, m4c, m5c);
			var M13 = new Matrix4x4(m1c, m2c, m4c, m5c);
			var M14 = new Matrix4x4(m1c, m2c, m3c, m5c);

			var detM11 = M11.determinant;
			var detM12 = M12.determinant;
			var detM13 = M13.determinant;
			var detM14 = M14.determinant;

			if (detM11 == 0)
				return new Circle2();

			var x = 0.5f * detM12 / detM11;
			var y = -0.5f * detM13 / detM11;
			var r = Mathf.Sqrt(x * x + y * y + detM14 / detM11);

			return new Circle2 { position = new Vector2(x, y), radius = r };
		}

		public bool Equals(Circle2 v)
		{
			return radius.Equals(v.radius) && position.Equals(v.position);
		}

		public override bool Equals(object obj)
		{
			return obj is Circle2 && Equals((Circle2)obj);
		}

		public override string ToString()
		{
			return string.Format("[position: {0}, radius: {1}]", position, radius);
		}

		public override int GetHashCode()
		{
			return position.GetHashCode() ^ radius.GetHashCode();
		}
	}
}
