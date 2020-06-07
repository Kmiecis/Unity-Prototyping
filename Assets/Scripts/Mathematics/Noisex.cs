using UnityEngine;

namespace Common.Mathematics
{
	public static class Noisex
	{
		/// <summary> Noise Map generator </summary>
		public static float[,] Map(
			Vector2Int extents, Vector2Int offset = default,
			int octaves = 1, float persistance = 0.5f, float lacunarity = 2.0f,
			Vector2 scale = default, int seed = 0
		)
		{
			var map = new float[extents.x, extents.y];
			Random.InitState(seed);

			Vector2 randomOffset = new Vector2(Random.Range(-99999.0f, +99999.0f), Random.Range(-99999.0f, +99999.0f));
			Vector2 middleOffset = new Vector2(extents.x * 0.5f, extents.y * 0.5f);
			Vector2 revertedExtents = new Vector2(1.0f / extents.x, 1.0f / extents.y);
			Vector2 revertedScale = new Vector2(1.0f / Mathf.Max(scale.x, 1e-5f), 1.0f / Mathf.Max(scale.y, 1e-5f));

			for (int y = 0; y < extents.y; ++y)
			{
				for (int x = 0; x < extents.x; ++x)
				{
					float amplitude = 1.0f;
					float frequency = 1.0f;
					float value = 0.0f;

					for (int i = 0; i < octaves; ++i)
					{
						float sampleX = ((x - middleOffset.x + offset.x) * revertedScale.x * revertedExtents.x) * frequency + randomOffset.x;
						float sampleY = ((y - middleOffset.y + offset.y) * revertedScale.y * revertedExtents.y) * frequency + randomOffset.y;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
						value += perlinValue * amplitude;

						amplitude *= persistance;
						frequency *= lacunarity;
					}

					map[x, y] = value;
				}
			}

			return map;
		}

		/// <summary> Noise Map generator smoothed to [0 .. 1] values </summary>
		public static float[,] SmoothMap(
			Vector2Int extents, Vector2Int offset = default,
			int octaves = 1, float persistance = 0.5f, float lacunarity = 2.0f,
			Vector2 scale = default, int seed = 0
		)
		{
			var map = new float[extents.x, extents.y];
			Random.InitState(seed);

			Vector2 noiseHeightMinMax = new Vector2(0.0f, 1.0f);
			for (int i = 1; i < octaves; ++i)
			{
				float persistanceAffection = Mathf.Pow(persistance, i);
				noiseHeightMinMax.x += persistanceAffection * 0.25f;
				noiseHeightMinMax.y += persistanceAffection * 0.75f;
			}

			Vector2 randomOffset = new Vector2(Random.Range(-99999.0f, +99999.0f), Random.Range(-99999.0f, +99999.0f));
			Vector2 middleOffset = new Vector2(extents.x * 0.5f, extents.y * 0.5f);
			Vector2 revertedExtents = new Vector2(1.0f / extents.x, 1.0f / extents.y);
			Vector2 revertedScale = new Vector2(1.0f / Mathf.Max(scale.x, 1e-4f), 1.0f / Mathf.Max(scale.y, 1e-4f));

			for (int y = 0; y < extents.y; ++y)
			{
				for (int x = 0; x < extents.x; ++x)
				{
					float amplitude = 1.0f;
					float frequency = 1.0f;
					float value = 0.0f;

					for (int i = 0; i < octaves; ++i)
					{
						float sampleX = ((x - middleOffset.x + offset.x) * revertedScale.x * revertedExtents.x) * frequency + randomOffset.x;
						float sampleY = ((y - middleOffset.y + offset.y) * revertedScale.y * revertedExtents.y) * frequency + randomOffset.y;

						float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
						value += perlinValue * amplitude;

						amplitude *= persistance;
						frequency *= lacunarity;
					}

					map[x, y] = Mathf.SmoothStep(noiseHeightMinMax.x, noiseHeightMinMax.y, value);
				}
			}

			return map;
		}

		/// <summary> Noise Map generator smoothed to [0 .. 1] values </summary>
		public static int[,] RandomMap(Vector2Int extents, float fill = 0.5f, int smooths = 5, int seed = 0)
		{
			var map = new int[extents.x, extents.y];
			Random.InitState(seed);

			for (int y = 0; y < extents.y; ++y)
			{
				for (int x = 0; x < extents.x; ++x)
				{
					var randomValue = Random.Range(0.0f, 1.0f);
					var value = randomValue < fill ? 1 : 0;
					map[x, y] = value;
				}
			}

			for (int i = 0; i < smooths; ++i)
			{
				for (int y = 0; y < extents.y; ++y)
				{
					for (int x = 0; x < extents.x; ++x)
					{
						int counter = 0;

						int dyMin = Mathf.Max(y - 1, 0);
						int dyMax = Mathf.Min(y + 1, extents.y - 1);
						for (int dy = dyMin; dy <= dyMax; ++dy)
						{
							int dxMin = Mathf.Max(x - 1, 0);
							int dxMax = Mathf.Min(x + 1, extents.x - 1);
							for (int dx = dxMin; dx <= dxMax; ++dx)
							{
								if (dx == x && dy == y)
								{
									continue;
								}

								counter += map[dx, dy];
							}
						}

						if (counter > 4)
						{
							map[x, y] = 1;
						}
						else if (counter < 4)
						{
							map[x, y] = 0;
						}
					}
				}
			}

			return map;
		}
	}
}
