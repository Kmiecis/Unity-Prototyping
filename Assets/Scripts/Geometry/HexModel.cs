using UnityEngine;

namespace Common.Mathematics
{
	public static class HexModel
	{
		public const int VCOUNT = 6;
		public const float CENTER_TO_SIDE = 1.0f;
		public const float CENTER_TO_VERTEX = CENTER_TO_SIDE * (2.0f / Mathx.ROOT_3);

		public static readonly Vector2[] V2 = new Vector2[]
		{
			new Vector2(0.0f, +CENTER_TO_VERTEX),
			new Vector2(+CENTER_TO_SIDE, +CENTER_TO_VERTEX * 0.5f),
			new Vector2(+CENTER_TO_SIDE, -CENTER_TO_VERTEX * 0.5f),
			new Vector2(0.0f, -CENTER_TO_VERTEX),
			new Vector2(-CENTER_TO_SIDE, -CENTER_TO_VERTEX * 0.5f),
			new Vector2(-CENTER_TO_SIDE, +CENTER_TO_VERTEX * 0.5f),
		};

		public static readonly Vector3[] V3 = new Vector3[]
		{
			new Vector3(0.0f, 0.0f, +CENTER_TO_VERTEX),
			new Vector3(+CENTER_TO_SIDE, 0.0f, +CENTER_TO_VERTEX * 0.5f),
			new Vector3(+CENTER_TO_SIDE, 0.0f, -CENTER_TO_VERTEX * 0.5f),
			new Vector3(0.0f, 0.0f, -CENTER_TO_VERTEX),
			new Vector3(-CENTER_TO_SIDE, 0.0f, -CENTER_TO_VERTEX * 0.5f),
			new Vector3(-CENTER_TO_SIDE, 0.0f, +CENTER_TO_VERTEX * 0.5f),
		};

		public static readonly Vector2Int[] T2 = new Vector2Int[]
		{
			new Vector2Int() { x = +0, y = +1 },
			new Vector2Int() { x = +1, y = +0 },
			new Vector2Int() { x = +0, y = -1 },
			new Vector2Int() { x = -1, y = -1 },
			new Vector2Int() { x = -1, y = +0 },
			new Vector2Int() { x = -1, y = +1 }
		};

		/// <summary>
		/// Convers position defined in hex coordinates to world position
		/// </summary>
		public static Vector3 Convert(Vector2Int v)
		{
			float x = (v.x + Mathf.Abs(v.y % 2) * 0.5f) * CENTER_TO_SIDE * 2.0f;
			float z = v.y * CENTER_TO_VERTEX * 1.5f;
			return new Vector3(x, 0.0f, z);
		}

		/// <summary>
		/// Converts position defined as world position to hex coordinates
		/// </summary>
		public static Vector2Int Convert(Vector3 v)
		{
			float x = v.x / (CENTER_TO_SIDE * 2.0f);
			float y = -x;

			float offset = v.z / (CENTER_TO_VERTEX * 3.0f);
			x -= offset;
			y -= offset;

			int ix = Mathf.RoundToInt(x);
			int iy = Mathf.RoundToInt(y);
			int iz = Mathf.RoundToInt(-x - y);

			if (ix + iy + iz != 0)
			{
				float dx = Mathf.Abs(x - ix);
				float dy = Mathf.Abs(y - iy);
				float dz = Mathf.Abs(-x - y - iz);

				if (dx > dy && dx > dz)
					ix = -iy - iz;
				else if (dz > dy)
					iz = -ix - iy;
			}

			return new Vector2Int(ix, iz);
		}

		public enum Direction
		{
			NE, E, SE, SW, W, NW, Count
		}
	}
}
