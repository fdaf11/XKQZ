using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class AnimationSpeed : MonoBehaviour
{
	// Token: 0x060006E5 RID: 1765 RVA: 0x00047BEC File Offset: 0x00045DEC
	private void Start()
	{
		if (base.animation == null)
		{
			return;
		}
		base.animation.wrapMode = 2;
		if (this.anim == null)
		{
			this.anim = base.animation.clip;
		}
		if (this.anim != null)
		{
			base.animation[this.anim.name].speed = this.speedAnim;
		}
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x0400074F RID: 1871
	public AnimationClip anim;

	// Token: 0x04000750 RID: 1872
	public float speedAnim;
}
