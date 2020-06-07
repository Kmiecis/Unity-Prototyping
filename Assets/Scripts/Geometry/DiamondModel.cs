using UnityEngine;

namespace Common.Mathematics
{
	public static class DiamondModel
	{
		public const int VCOUNT = 6;
		public const float CENTER_TO_VERTEX = 2 * Mathx.ROOT_2;

		public static readonly Vector3[] V3 = new Vector3[]
		{
			new Vector3(0.0f, -CENTER_TO_VERTEX, 0.0f),
			new Vector3(-CENTER_TO_VERTEX, 0.0f, 0.0f),
			new Vector3(0.0f, 0.0f, +CENTER_TO_VERTEX),
			new Vector3(+CENTER_TO_VERTEX, 0.0f, 0.0f),
			new Vector3(0.0f, 0.0f, -CENTER_TO_VERTEX),
			new Vector3(0.0f, +CENTER_TO_VERTEX, 0.0f)
		};
	}
}
