using System;
using UnityEngine;

// Token: 0x020005FB RID: 1531
public class FadeInOutSound : MonoBehaviour
{
	// Token: 0x060025D8 RID: 9688 RVA: 0x00124D20 File Offset: 0x00122F20
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

	// Token: 0x060025D9 RID: 9689 RVA: 0x00019334 File Offset: 0x00017534
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.prefabSettings_CollisionEnter);
		}
		this.InitSource();
	}

	// Token: 0x060025DA RID: 9690 RVA: 0x00124D6C File Offset: 0x00122F6C
	private void InitSource()
	{
		if (this.isInitialized)
		{
			return;
		}
		this.audioSource = base.GetComponent<AudioSource>();
		if (this.audioSource == null)
		{
			return;
		}
		this.isStartDelay = (this.StartDelay > 0.001f);
		this.isIn = (this.FadeInSpeed > 0.001f);
		this.isOut = (this.FadeOutSpeed > 0.001f);
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	// Token: 0x060025DB RID: 9691 RVA: 0x00124DEC File Offset: 0x00122FEC
	private void InitDefaultVariables()
	{
		this.fadeInComplited = false;
		this.fadeOutComplited = false;
		this.allComplited = false;
		this.canStartFadeOut = false;
		this.isCollisionEnter = false;
		this.oldVolume = 0f;
		this.currentVolume = this.MaxVolume;
		if (this.isIn)
		{
			this.currentVolume = 0f;
		}
		this.audioSource.volume = this.currentVolume;
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
			this.oldVolume = this.MaxVolume;
		}
	}

	// Token: 0x060025DC RID: 9692 RVA: 0x00019370 File Offset: 0x00017570
	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.isCollisionEnter = true;
		if (!this.isIn && this.FadeOutAfterCollision)
		{
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
	}

	// Token: 0x060025DD RID: 9693 RVA: 0x000193A0 File Offset: 0x000175A0
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x060025DE RID: 9694 RVA: 0x000193B3 File Offset: 0x000175B3
	private void SetupStartDelay()
	{
		this.canStart = true;
	}

	// Token: 0x060025DF RID: 9695 RVA: 0x000193BC File Offset: 0x000175BC
	private void SetupFadeOutDelay()
	{
		this.canStartFadeOut = true;
	}

	// Token: 0x060025E0 RID: 9696 RVA: 0x00124EB8 File Offset: 0x001230B8
	private void Update()
	{
		if (!this.canStart || this.audioSource == null)
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
		else if ((this.UseHideStatus && !this.effectSettings.IsVisible) || this.isCollisionEnter)
		{
			this.FadeOut();
		}
	}

	// Token: 0x060025E1 RID: 9697 RVA: 0x00125014 File Offset: 0x00123214
	private void FadeIn()
	{
		this.currentVolume = this.oldVolume + Time.deltaTime / this.FadeInSpeed * this.MaxVolume;
		if (this.currentVolume >= this.MaxVolume)
		{
			this.fadeInComplited = true;
			if (!this.isOut)
			{
				this.allComplited = true;
			}
			this.currentVolume = this.MaxVolume;
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
		this.audioSource.volume = this.currentVolume;
		this.oldVolume = this.currentVolume;
	}

	// Token: 0x060025E2 RID: 9698 RVA: 0x001250A8 File Offset: 0x001232A8
	private void FadeOut()
	{
		this.currentVolume = this.oldVolume - Time.deltaTime / this.FadeOutSpeed * this.MaxVolume;
		if (this.currentVolume <= 0f)
		{
			this.currentVolume = 0f;
			this.fadeOutComplited = true;
			this.allComplited = true;
		}
		this.audioSource.volume = this.currentVolume;
		this.oldVolume = this.currentVolume;
	}

	// Token: 0x04002E63 RID: 11875
	public float MaxVolume = 1f;

	// Token: 0x04002E64 RID: 11876
	public float StartDelay;

	// Token: 0x04002E65 RID: 11877
	public float FadeInSpeed;

	// Token: 0x04002E66 RID: 11878
	public float FadeOutDelay;

	// Token: 0x04002E67 RID: 11879
	public float FadeOutSpeed;

	// Token: 0x04002E68 RID: 11880
	public bool FadeOutAfterCollision;

	// Token: 0x04002E69 RID: 11881
	public bool UseHideStatus;

	// Token: 0x04002E6A RID: 11882
	private AudioSource audioSource;

	// Token: 0x04002E6B RID: 11883
	private float oldVolume;

	// Token: 0x04002E6C RID: 11884
	private float currentVolume;

	// Token: 0x04002E6D RID: 11885
	private bool canStart;

	// Token: 0x04002E6E RID: 11886
	private bool canStartFadeOut;

	// Token: 0x04002E6F RID: 11887
	private bool fadeInComplited;

	// Token: 0x04002E70 RID: 11888
	private bool fadeOutComplited;

	// Token: 0x04002E71 RID: 11889
	private bool isCollisionEnter;

	// Token: 0x04002E72 RID: 11890
	private bool allComplited;

	// Token: 0x04002E73 RID: 11891
	private bool isStartDelay;

	// Token: 0x04002E74 RID: 11892
	private bool isIn;

	// Token: 0x04002E75 RID: 11893
	private bool isOut;

	// Token: 0x04002E76 RID: 11894
	private EffectSettings effectSettings;

	// Token: 0x04002E77 RID: 11895
	private bool isInitialized;
}
