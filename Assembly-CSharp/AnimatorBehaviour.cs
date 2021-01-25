using System;
using UnityEngine;

// Token: 0x020005CB RID: 1483
public class AnimatorBehaviour : MonoBehaviour
{
	// Token: 0x060024DE RID: 9438 RVA: 0x0011FE74 File Offset: 0x0011E074
	private void GetEffectSettingsComponent(Transform tr)
	{
		Transform parent = tr.parent;
		if (parent != null)
		{
			this.effectSettings = parent.GetComponentInChildren<EffectSettings>();
			if (this.effectSettings == null)
			{
				this.GetEffectSettingsComponent(parent.transform);
			}
		}
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x0011FEC0 File Offset: 0x0011E0C0
	private void Start()
	{
		this.oldSpeed = this.anim.speed;
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.prefabSettings_CollisionEnter);
		}
		this.isInitialized = true;
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x000186DC File Offset: 0x000168DC
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.anim.speed = this.oldSpeed;
		}
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x000186FA File Offset: 0x000168FA
	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.anim.speed = 0f;
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04002CE9 RID: 11497
	public Animator anim;

	// Token: 0x04002CEA RID: 11498
	private EffectSettings effectSettings;

	// Token: 0x04002CEB RID: 11499
	private bool isInitialized;

	// Token: 0x04002CEC RID: 11500
	private float oldSpeed;
}
