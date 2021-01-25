using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x0200001A RID: 26
	public class MecanimAnimation : MonoBehaviour
	{
		// Token: 0x0600007F RID: 127 RVA: 0x000029B8 File Offset: 0x00000BB8
		private void Awake()
		{
			this.animator = base.GetComponent<Animator>();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00022F5C File Offset: 0x0002115C
		public void OnAnimationState(AIAnimationState animData)
		{
			string name = animData.name;
			if (animData != this.prevAnim)
			{
				if (animData.crossFadeIn || (this.prevAnim != null && this.prevAnim.crossFadeOut))
				{
					this.animator.CrossFade(name, animData.transitionTime);
				}
				this.prevAnim = animData;
			}
		}

		// Token: 0x04000055 RID: 85
		private Animator animator;

		// Token: 0x04000056 RID: 86
		private AIAnimationState prevAnim;
	}
}
