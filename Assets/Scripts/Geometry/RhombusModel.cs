using UnityEngine;

namespace Common.Mathematics
{
	public static class RhombusModel
	{
		public const int VCOUNT = 4;
		public const float CENTER_TO_VERTEX = Mathx.ROOT_2 * 0.5f;

		public static readonly Vector2[] V2 = new Vector2[]
		{
			new Vector2(-CENTER_TO_VERTEX, 0.0f),
			new Vector2(0.0f, +CENTER_TO_VERTEX),
			new Vector2(+CENTER_TO_VERTEX, 0.0f),
			new Vector2(0.0f, -CENTER_TO_VERTEX)
		};

		public static readonly Vector3[] V3 = new Vector3[]
		{
			new Vector3(-CENTER_TO_VERTEX, 0.0f, 0.0f),
			new Vector3(0.0f, 0.0f, +CENTER_TO_VERTEX),
			new Vector3(+CENTER_TO_VERTEX, 0.0f, 0.0f),
			new Vector3(0.0f, 0.0f, -CENTER_TO_VERTEX),
		};
	}
}
