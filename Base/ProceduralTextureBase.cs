﻿using UnityEngine;

namespace Common.Prototyping
{
	public abstract class ProceduralTextureBase : MonoBehaviour, IProceduralTexture
	{
		public bool IsDirty { get; set; }

		public abstract ITexture2DBuilder Create();

#if UNITY_EDITOR
		private void OnValidate()
		{
			IsDirty = true;
		}
#endif

		public static readonly Color32 COLOR_WHITE = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		public static readonly Color32 COLOR_BLACK = new Color32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MaxValue);
		public static readonly Color32 COLOR_ALPHA = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MinValue);
	}
}