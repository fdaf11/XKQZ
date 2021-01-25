using System;
using UnityEngine;

// Token: 0x020005F4 RID: 1524
public class AnimationSpeedDebuff : MonoBehaviour
{
	// Token: 0x060025BF RID: 9663 RVA: 0x001248EC File Offset: 0x00122AEC
	private void GetAnimatorOnParent(Transform t)
	{
		Animator component = t.parent.GetComponent<Animator>();
		if (component == null)
		{
			if (this.root == t.parent)
			{
				return;
			}
			this.GetAnimatorOnParent(t.parent);
		}
		else
		{
			this.myAnimator = component;
		}
	}

	// Token: 0x060025C0 RID: 9664 RVA: 0x00124940 File Offset: 0x00122B40
	private void Start()
	{
		this.root = base.transform.root;
		this.GetAnimatorOnParent(base.transform);
		if (this.myAnimator == null)
		{
			return;
		}
		this.oldSpeed = this.myAnimator.speed;
	}

	// Token: 0x060025C1 RID: 9665 RVA: 0x00124990 File Offset: 0x00122B90
	private void Update()
	{
		if (this.myAnimator == null || this.AnimationSpeenOnTime.length == 0)
		{
			return;
		}
		this.time += Time.deltaTime;
		this.myAnimator.speed = this.AnimationSpeenOnTime.Evaluate(this.time / this.MaxTime) * this.oldSpeed;
	}

	// Token: 0x04002E48 RID: 11848
	public AnimationCurve AnimationSpeenOnTime;

	// Token: 0x04002E49 RID: 11849
	public float MaxTime = 1f;

	// Token: 0x04002E4A RID: 11850
	private Animator myAnimator;

	// Token: 0x04002E4B RID: 11851
	private Transform root;

	// Token: 0x04002E4C RID: 11852
	private float oldSpeed;

	// Token: 0x04002E4D RID: 11853
	private float time;
}
