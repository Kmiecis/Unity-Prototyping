using Common.Mathematics;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Rendering
{
	public class Triangulation
	{
		/// <summary> Simple implementation of 2D triangulation by checking minimal circles not containing any point </summary>
		public static List<int> Simple(List<Vector2> points)
		{
			var ts = new List<int>();

			var normal = Vector3.up;
			for (int i = 0; i < points.Count; ++i)
			{
				var iPoint = points[i];

				for (int j = i + 1; j < points.Count; ++j)
				{
					var jPoint = points[j];

					for (int k = j + 1; k < points.Count; ++k)
					{
						var kPoint = points[k];

						var circle = Circle2.Create(iPoint, jPoint, kPoint);

						bool contains = false;
						for (int l = 0; !contains && l < points.Count; ++l)
						{
							if (l == i || l == j || l == k)
							{
								continue;
							}

							contains = circle.Contains(points[l]);
						}

						if (!contains)
						{
							var dij = jPoint - iPoint;
							var djk = kPoint - jPoint;

							var triangleNormal = Vector3.Cross(new Vector3(dij.x, 0.0f, dij.y), new Vector3(djk.x, 0.0f, djk.y));
							if (Vector3.Dot(triangleNormal, normal) > 0)
							{
								ts.Add(i);
								ts.Add(j);
								ts.Add(k);
							}
							else
							{
								ts.Add(k);
								ts.Add(j);
								ts.Add(i);
							}
						}
					}
				}
			}

			return ts;
		}
	}
}
