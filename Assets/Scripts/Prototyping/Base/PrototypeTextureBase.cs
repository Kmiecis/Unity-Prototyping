using Common.Rendering;
using UnityEngine;

namespace Common.Prototyping
{
	public abstract class PrototypeTextureBase : MonoBehaviour, IPrototypeTexture
	{
		public bool IsDirty { get; set; }

		public abstract ITextureData Create();

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
