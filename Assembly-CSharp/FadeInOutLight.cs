using System;
using UnityEngine;

// Token: 0x020005DF RID: 1503
public class FadeInOutLight : MonoBehaviour
{
	// Token: 0x0600253D RID: 9533 RVA: 0x00121FBC File Offset: 0x001201BC
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

	// Token: 0x0600253E RID: 9534 RVA: 0x00122008 File Offset: 0x00120208
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.prefabSettings_CollisionEnter);
		}
		this.goLight = base.light;
		this.startIntensity = this.goLight.intensity;
		this.isStartDelay = (this.StartDelay > 0.001f);
		this.isIn = (this.FadeInSpeed > 0.001f);
		this.isOut = (this.FadeOutSpeed > 0.001f);
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	// Token: 0x0600253F RID: 9535 RVA: 0x001220AC File Offset: 0x001202AC
	private void InitDefaultVariables()
	{
		this.fadeInComplited = false;
		this.fadeOutComplited = false;
		this.allComplited = false;
		this.canStartFadeOut = false;
		this.isCollisionEnter = false;
		this.oldIntensity = 0f;
		this.currentIntensity = 0f;
		this.canStart = false;
		this.goLight.intensity = ((!this.isIn) ? this.startIntensity : 0f);
		if (this.isStartDelay)
		{
			base.Invoke("SetupStartDelay", this.StartDelay);
		}
		else
		{
			this.canStart = true;
		}
		if (!this.isIn)
		{
			if (!this.FadeOutAfterCollision)
			{
				base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
			}
			this.oldIntensity = this.startIntensity;
		}
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x00018BE4 File Offset: 0x00016DE4
	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.isCollisionEnter = true;
		if (!this.isIn && this.FadeOutAfterCollision)
		{
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
	}

	// Token: 0x06002541 RID: 9537 RVA: 0x00018C14 File Offset: 0x00016E14
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x06002542 RID: 9538 RVA: 0x00018C27 File Offset: 0x00016E27
	private void SetupStartDelay()
	{
		this.canStart = true;
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x00018C30 File Offset: 0x00016E30
	private void SetupFadeOutDelay()
	{
		this.canStartFadeOut = true;
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x0012217C File Offset: 0x0012037C
	private void Update()
	{
		if (!this.canStart)
		{
			return;
		}
		if (this.effectSettings != null && this.UseHideStatus && this.allComplited && this.effectSettings.IsVisible)
		{
			this.allComplited = false;
			this.fadeInComplited = false;
			this.fadeOutComplited = false;
			this.InitDefaultVariables();
		}
		if (this.isIn && !this.fadeInComplited)
		{
			if (this.effectSettings == null)
			{
				this.FadeIn();
			}
			else if ((this.UseHideStatus && this.effectSettings.IsVisible) || !this.UseHideStatus)
			{
				this.FadeIn();
			}
		}
		if (!this.isOut || this.fadeOutComplited || !this.canStartFadeOut)
		{
			return;
		}
		if (this.effectSettings == null || (!this.UseHideStatus && !this.FadeOutAfterCollision))
		{
			this.FadeOut();
		}
		else if ((this.UseHideStatus && !this.effectSettings.IsVisible) || (this.FadeOutAfterCollision && this.isCollisionEnter))
		{
			this.FadeOut();
		}
	}

	// Token: 0x06002545 RID: 9541 RVA: 0x001222D0 File Offset: 0x001204D0
	private void FadeIn()
	{
		this.currentIntensity = this.oldIntensity + Time.deltaTime / this.FadeInSpeed * this.startIntensity;
		if (this.currentIntensity >= this.startIntensity)
		{
			this.fadeInComplited = true;
			if (!this.isOut)
			{
				this.allComplited = true;
			}
			this.currentIntensity = this.startIntensity;
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
		this.goLight.intensity = this.currentIntensity;
		this.oldIntensity = this.currentIntensity;
	}

	// Token: 0x06002546 RID: 9542 RVA: 0x00122364 File Offset: 0x00120564
	private void FadeOut()
	{
		this.currentIntensity = this.oldIntensity - Time.deltaTime / this.FadeOutSpeed * this.startIntensity;
		if (this.currentIntensity <= 0f)
		{
			this.currentIntensity = 0f;
			this.fadeOutComplited = true;
			this.allComplited = true;
		}
		this.goLight.intensity = this.currentIntensity;
		this.oldIntensity = this.currentIntensity;
	}

	// Token: 0x04002D8B RID: 11659
	public float StartDelay;

	// Token: 0x04002D8C RID: 11660
	public float FadeInSpeed;

	// Token: 0x04002D8D RID: 11661
	public float FadeOutDelay;

	// Token: 0x04002D8E RID: 11662
	public float FadeOutSpeed;

	// Token: 0x04002D8F RID: 11663
	public bool FadeOutAfterCollision;

	// Token: 0x04002D90 RID: 11664
	public bool UseHideStatus;

	// Token: 0x04002D91 RID: 11665
	private Light goLight;

	// Token: 0x04002D92 RID: 11666
	private float oldIntensity;

	// Token: 0x04002D93 RID: 11667
	private float currentIntensity;

	// Token: 0x04002D94 RID: 11668
	private float startIntensity;

	// Token: 0x04002D95 RID: 11669
	private bool canStart;

	// Token: 0x04002D96 RID: 11670
	private bool canStartFadeOut;

	// Token: 0x04002D97 RID: 11671
	private bool fadeInComplited;

	// Token: 0x04002D98 RID: 11672
	private bool fadeOutComplited;

	// Token: 0x04002D99 RID: 11673
	private bool isCollisionEnter;

	// Token: 0x04002D9A RID: 11674
	private bool allComplited;

	// Token: 0x04002D9B RID: 11675
	private bool isStartDelay;

	// Token: 0x04002D9C RID: 11676
	private bool isIn;

	// Token: 0x04002D9D RID: 11677
	private bool isOut;

	// Token: 0x04002D9E RID: 11678
	private EffectSettings effectSettings;

	// Token: 0x04002D9F RID: 11679
	private bool isInitialized;
}
