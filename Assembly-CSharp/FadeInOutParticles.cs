using System;
using UnityEngine;

// Token: 0x020005E0 RID: 1504
public class FadeInOutParticles : MonoBehaviour
{
	// Token: 0x06002548 RID: 9544 RVA: 0x001223D8 File Offset: 0x001205D8
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

	// Token: 0x06002549 RID: 9545 RVA: 0x00018C39 File Offset: 0x00016E39
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		this.particles = this.effectSettings.GetComponentsInChildren<ParticleSystem>();
		this.oldVisibleStat = this.effectSettings.IsVisible;
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x00122424 File Offset: 0x00120624
	private void Update()
	{
		if (this.effectSettings.IsVisible != this.oldVisibleStat)
		{
			if (this.effectSettings.IsVisible)
			{
				foreach (ParticleSystem particleSystem in this.particles)
				{
					if (this.effectSettings.IsVisible)
					{
						particleSystem.Play();
						particleSystem.enableEmission = true;
					}
				}
			}
			else
			{
				foreach (ParticleSystem particleSystem2 in this.particles)
				{
					if (!this.effectSettings.IsVisible)
					{
						particleSystem2.Stop();
						particleSystem2.enableEmission = false;
					}
				}
			}
		}
		this.oldVisibleStat = this.effectSettings.IsVisible;
	}

	// Token: 0x04002DA0 RID: 11680
	private EffectSettings effectSettings;

	// Token: 0x04002DA1 RID: 11681
	private ParticleSystem[] particles;

	// Token: 0x04002DA2 RID: 11682
	private bool oldVisibleStat;
}
