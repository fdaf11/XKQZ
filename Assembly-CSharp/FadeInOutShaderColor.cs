using System;
using UnityEngine;

// Token: 0x020005E2 RID: 1506
public class FadeInOutShaderColor : MonoBehaviour
{
	// Token: 0x06002551 RID: 9553 RVA: 0x00122680 File Offset: 0x00120880
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

	// Token: 0x06002552 RID: 9554 RVA: 0x00018D17 File Offset: 0x00016F17
	public void UpdateMaterial(Material instanceMaterial)
	{
		this.mat = instanceMaterial;
		this.InitMaterial();
	}

	// Token: 0x06002553 RID: 9555 RVA: 0x00018D26 File Offset: 0x00016F26
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings != null)
		{
			this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.prefabSettings_CollisionEnter);
		}
		this.InitMaterial();
	}

	// Token: 0x06002554 RID: 9556 RVA: 0x001226CC File Offset: 0x001208CC
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
		this.oldColor = this.mat.GetColor(this.ShaderColorName);
		this.isStartDelay = (this.StartDelay > 0.001f);
		this.isIn = (this.FadeInSpeed > 0.001f);
		this.isOut = (this.FadeOutSpeed > 0.001f);
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x00122808 File Offset: 0x00120A08
	private void InitDefaultVariables()
	{
		this.fadeInComplited = false;
		this.fadeOutComplited = false;
		this.canStartFadeOut = false;
		this.isCollisionEnter = false;
		this.oldAlpha = 0f;
		this.alpha = 0f;
		this.canStart = false;
		this.currentColor = this.oldColor;
		if (this.isIn)
		{
			this.currentColor.a = 0f;
		}
		this.mat.SetColor(this.ShaderColorName, this.currentColor);
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
			this.oldAlpha = this.oldColor.a;
		}
	}

	// Token: 0x06002556 RID: 9558 RVA: 0x00018D62 File Offset: 0x00016F62
	private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		this.isCollisionEnter = true;
		if (!this.isIn && this.FadeOutAfterCollision)
		{
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
	}

	// Token: 0x06002557 RID: 9559 RVA: 0x00018D92 File Offset: 0x00016F92
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x06002558 RID: 9560 RVA: 0x00018DA5 File Offset: 0x00016FA5
	private void SetupStartDelay()
	{
		this.canStart = true;
	}

	// Token: 0x06002559 RID: 9561 RVA: 0x00018DAE File Offset: 0x00016FAE
	private void SetupFadeOutDelay()
	{
		this.canStartFadeOut = true;
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x001228EC File Offset: 0x00120AEC
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

	// Token: 0x0600255B RID: 9563 RVA: 0x00122A90 File Offset: 0x00120C90
	private void FadeIn()
	{
		this.alpha = this.oldAlpha + Time.deltaTime / this.FadeInSpeed;
		if (this.alpha >= this.oldColor.a)
		{
			this.fadeInComplited = true;
			this.alpha = this.oldColor.a;
			base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
		}
		this.currentColor.a = this.alpha;
		this.mat.SetColor(this.ShaderColorName, this.currentColor);
		this.oldAlpha = this.alpha;
	}

	// Token: 0x0600255C RID: 9564 RVA: 0x00122B2C File Offset: 0x00120D2C
	private void FadeOut()
	{
		this.alpha = this.oldAlpha - Time.deltaTime / this.FadeOutSpeed;
		if (this.alpha <= 0f)
		{
			this.alpha = 0f;
			this.fadeOutComplited = true;
		}
		this.currentColor.a = this.alpha;
		this.mat.SetColor(this.ShaderColorName, this.currentColor);
		this.oldAlpha = this.alpha;
	}

	// Token: 0x04002DAD RID: 11693
	public string ShaderColorName = "_Color";

	// Token: 0x04002DAE RID: 11694
	public float StartDelay;

	// Token: 0x04002DAF RID: 11695
	public float FadeInSpeed;

	// Token: 0x04002DB0 RID: 11696
	public float FadeOutDelay;

	// Token: 0x04002DB1 RID: 11697
	public float FadeOutSpeed;

	// Token: 0x04002DB2 RID: 11698
	public bool UseSharedMaterial;

	// Token: 0x04002DB3 RID: 11699
	public bool FadeOutAfterCollision;

	// Token: 0x04002DB4 RID: 11700
	public bool UseHideStatus;

	// Token: 0x04002DB5 RID: 11701
	private Material mat;

	// Token: 0x04002DB6 RID: 11702
	private Color oldColor;

	// Token: 0x04002DB7 RID: 11703
	private Color currentColor;

	// Token: 0x04002DB8 RID: 11704
	private float oldAlpha;

	// Token: 0x04002DB9 RID: 11705
	private float alpha;

	// Token: 0x04002DBA RID: 11706
	private bool canStart;

	// Token: 0x04002DBB RID: 11707
	private bool canStartFadeOut;

	// Token: 0x04002DBC RID: 11708
	private bool fadeInComplited;

	// Token: 0x04002DBD RID: 11709
	private bool fadeOutComplited;

	// Token: 0x04002DBE RID: 11710
	private bool isCollisionEnter;

	// Token: 0x04002DBF RID: 11711
	private bool isStartDelay;

	// Token: 0x04002DC0 RID: 11712
	private bool isIn;

	// Token: 0x04002DC1 RID: 11713
	private bool isOut;

	// Token: 0x04002DC2 RID: 11714
	private EffectSettings effectSettings;

	// Token: 0x04002DC3 RID: 11715
	private bool isInitialized;
}
