using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000016 RID: 22
	public class AIAnimationState : MonoBehaviour
	{
		// Token: 0x04000041 RID: 65
		public string name = "Untitled";

		// Token: 0x04000042 RID: 66
		public int startFrame;

		// Token: 0x04000043 RID: 67
		public int endFrame;

		// Token: 0x04000044 RID: 68
		public float speed = 1f;

		// Token: 0x04000045 RID: 69
		public WrapMode animationWrapMode = 2;

		// Token: 0x04000046 RID: 70
		public bool crossFadeIn;

		// Token: 0x04000047 RID: 71
		public bool crossFadeOut;

		// Token: 0x04000048 RID: 72
		public float transitionTime = 0.2f;

		// Token: 0x04000049 RID: 73
		public bool foldoutOpen;
	}
}
