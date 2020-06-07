using UnityEngine;

namespace Common
{
	public class TextureProperty : MonoBehaviour
	{
		private Texture2D m_PreviousTarget;
		public Texture2D target;

		[Header("Properties")]
		public bool alphaIsTransparency;
		public TextureWrapMode wrapMode;
		public FilterMode filterMode;

#if UNITY_EDITOR
		public void OnValidate()
		{
			bool isTargetJustSet = m_PreviousTarget == null && target != null;
			bool hasTargetChanged = m_PreviousTarget != null && m_PreviousTarget != target;

			if (isTargetJustSet || hasTargetChanged)
			{
				m_PreviousTarget = target;

				this.alphaIsTransparency = target.alphaIsTransparency;
				this.wrapMode = target.wrapMode;
				this.filterMode = target.filterMode;
			}

			if (target != null)
			{
				target.alphaIsTransparency = this.alphaIsTransparency;
				target.wrapMode = this.wrapMode;
				target.filterMode = this.filterMode;
			}
		}
#endif
	}
}
