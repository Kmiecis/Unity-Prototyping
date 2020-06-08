using UnityEngine;

namespace Common.Mathematics
{
	public static class SquareModel
	{
		public const int VCOUNT = 4;
		public const float CENTER_TO_VERTEX = Mathx.ROOT_2 * 0.5f;

		public static readonly Vector2[] V2 = new Vector2[]
		{
			new Vector2(-0.5f, -0.5f),
			new Vector2(-0.5f, +0.5f),
			new Vector2(+0.5f, +0.5f),
			new Vector2(+0.5f, -0.5f)
		};

		public static readonly Vector3[] V3 = new Vector3[]
		{
			new Vector3(-0.5f, 0.0f, -0.5f),
			new Vector3(-0.5f, 0.0f, +0.5f),
			new Vector3(+0.5f, 0.0f, +0.5f),
			new Vector3(+0.5f, 0.0f, -0.5f)
		};

		public static readonly Vector2Int[] T2 = new Vector2Int[]
		{
			new Vector2Int(-1, 0),
			new Vector2Int(0, +1),
			new Vector2Int(+1, 0),
			new Vector2Int(0, -1)
		};
	}
}
