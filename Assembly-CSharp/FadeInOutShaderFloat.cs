using System;
using UnityEngine;

// Token: 0x020005E3 RID: 1507
public class FadeInOutShaderFloat : MonoBehaviour
{
	// Token: 0x0600255E RID: 9566 RVA: 0x00122BA8 File Offset: 0x00120DA8
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

	// Token: 0x0600255F RID: 9567 RVA: 0x00018DD5 File Offset: 0x00016FD5
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.prefabSettings_CollisionEnter);
		}
		this.InitMaterial();
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x00018E11 File Offset: 0x00017011
	public void UpdateMaterial(Material instanceMaterial)
	{
		this.mat = instanceMaterial;
		this.InitMaterial();
	}

	// Token: 0x06002561 RID: 9569 RVA: 0x00122BF4 File Offset: 0x00120DF4
	private void InitMaterial()
	{
		if (this.isInitialized)
		{
			return;
		}
		if (base.renderer != null)
		{
			this.mat = base.renderer.material;
		}
		else
		{
			LineRenderer component = base.GetComponent<LineRenderer>();
			if (component != null)
			{
				this.mat = component.material;
			}
			else
			{
				Projector component2 = base.GetComponent<Projector>();
				if (component2 != null)
				{
					if (!component2.material.name.EndsWith("(Instance)"))
					{
						component2.material = new Material(component2.material)
						{
							name = component2.material.name + " (Instance)"
						};
					}
					this.mat = component2.material;
				}
			}
		}
		if (this.mat == null)
		{
			return;
		}
		this.isStartDelay = (this.StartDelay > 0.001f);
		this.isIn = (this.FadeInSpeed > 0.001f);
		this.isOut = (this.FadeOutSpeed > 0.001f);
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	// Token: 0x06002562 RID: 9570 RVA: 0x00122D18 File Offset: 0x00120F18
	private void InitDefaultVariables()
	{
		this.fadeInComplited = false;
		this.fadeOutComplited = false;
		this.canStartFadeOut = false;
		this.canStart = false;
		this.isCollisionEnter = false;
		this.oldFloat = 0f;
		this.currentFloat = this.MaxFloat;
		if (this.isIn)
		{
			this.currentFloat = 0f;
		}
		this.mat.SetFloat(this.PropertyName, this.currentFloat);
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
			this.oldFloat = this.MaxFloat;
		}
	}

	// Token: 0x06002563 RID: 9571 RVA: 0x00018E20 File Offset: 0x00017020
	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.isCollisionEnter = true;
		if (!this.isIn && this.FadeOutAfterCollision)
		{
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
	}

	// Token: 0x06002564 RID: 9572 RVA: 0x00018E50 File Offset: 0x00017050
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x06002565 RID: 9573 RVA: 0x00018E63 File Offset: 0x00017063
	private void SetupStartDelay()
	{
		this.canStart = true;
	}

	// Token: 0x06002566 RID: 9574 RVA: 0x00018E6C File Offset: 0x0001706C
	private void SetupFadeOutDelay()
	{
		this.canStartFadeOut = true;
	}

	// Token: 0x06002567 RID: 9575 RVA: 0x00122DE8 File Offset: 0x00120FE8
	private void Update()
	{
		if (!this.canStart)
		{
			return;
		}
		if (this.effectSettings != null && this.UseHideStatus)
		{
			if (!this.effectSettings.IsVisible && this.fadeInComplited)
			{
				this.fadeInComplited = false;
			}
			if (this.effectSettings.IsVisible && this.fadeOutComplited)
			{
				this.fadeOutComplited = false;
			}
		}
		if (this.UseHideStatus)
		{
			if (this.isIn && this.effectSettings != null && this.effectSettings.IsVisible && !this.fadeInComplited)
			{
				this.FadeIn();
			}
			if (this.isOut && this.effectSettings != null && !this.effectSettings.IsVisible && !this.fadeOutComplited)
			{
				this.FadeOut();
			}
		}
		else if (!this.FadeOutAfterCollision)
		{
			if (this.isIn && !this.fadeInComplited)
			{
				this.FadeIn();
			}
			if (this.isOut && this.canStartFadeOut && !this.fadeOutComplited)
			{
				this.FadeOut();
			}
		}
		else
		{
			if (this.isIn && !this.fadeInComplited)
			{
				this.FadeIn();
			}
			if (this.isOut && this.isCollisionEnter && this.canStartFadeOut && !this.fadeOutComplited)
			{
				this.FadeOut();
			}
		}
	}

	// Token: 0x06002568 RID: 9576 RVA: 0x00122F8C File Offset: 0x0012118C
	private void FadeIn()
	{
		this.currentFloat = this.oldFloat + Time.deltaTime / this.FadeInSpeed * this.MaxFloat;
		if (this.currentFloat >= this.MaxFloat)
		{
			this.fadeInComplited = true;
			this.currentFloat = this.MaxFloat;
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
		this.mat.SetFloat(this.PropertyName, this.currentFloat);
		this.oldFloat = this.currentFloat;
	}

	// Token: 0x06002569 RID: 9577 RVA: 0x00123014 File Offset: 0x00121214
	private void FadeOut()
	{
		this.currentFloat = this.oldFloat - Time.deltaTime / this.FadeOutSpeed * this.MaxFloat;
		if (this.currentFloat <= 0f)
		{
			this.currentFloat = 0f;
			this.fadeOutComplited = true;
		}
		this.mat.SetFloat(this.PropertyName, this.currentFloat);
		this.oldFloat = this.currentFloat;
	}

	// Token: 0x04002DC4 RID: 11716
	public string PropertyName = "_CutOut";

	// Token: 0x04002DC5 RID: 11717
	public float MaxFloat = 1f;

	// Token: 0x04002DC6 RID: 11718
	public float StartDelay;

	// Token: 0x04002DC7 RID: 11719
	public float FadeInSpeed;

	// Token: 0x04002DC8 RID: 11720
	public float FadeOutDelay;

	// Token: 0x04002DC9 RID: 11721
	public float FadeOutSpeed;

	// Token: 0x04002DCA RID: 11722
	public bool FadeOutAfterCollision;

	// Token: 0x04002DCB RID: 11723
	public bool UseHideStatus;

	// Token: 0x04002DCC RID: 11724
	private Material OwnMaterial;

	// Token: 0x04002DCD RID: 11725
	private Material mat;

	// Token: 0x04002DCE RID: 11726
	private float oldFloat;

	// Token: 0x04002DCF RID: 11727
	private float currentFloat;

	// Token: 0x04002DD0 RID: 11728
	private bool canStart;

	// Token: 0x04002DD1 RID: 11729
	private bool canStartFadeOut;

	// Token: 0x04002DD2 RID: 11730
	private bool fadeInComplited;

	// Token: 0x04002DD3 RID: 11731
	private bool fadeOutComplited;

	// Token: 0x04002DD4 RID: 11732
	private bool previousFrameVisibleStatus;

	// Token: 0x04002DD5 RID: 11733
	private bool isCollisionEnter;

	// Token: 0x04002DD6 RID: 11734
	private bool isStartDelay;

	// Token: 0x04002DD7 RID: 11735
	private bool isIn;

	// Token: 0x04002DD8 RID: 11736
	private bool isOut;

	// Token: 0x04002DD9 RID: 11737
	private EffectSettings effectSettings;

	// Token: 0x04002DDA RID: 11738
	private bool isInitialized;
}
