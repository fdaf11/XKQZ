using System;
using UnityEngine;

// Token: 0x020005D9 RID: 1497
public class CollisionActiveBehaviour : MonoBehaviour
{
	// Token: 0x06002526 RID: 9510 RVA: 0x00121CF8 File Offset: 0x0011FEF8
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.IsReverse)
		{
			this.effectSettings.RegistreInactiveElement(base.gameObject, this.TimeDelay);
			base.gameObject.SetActive(false);
		}
		else
		{
			this.effectSettings.RegistreActiveElement(base.gameObject, this.TimeDelay);
		}
		if (this.IsLookAt)
		{
			this.effectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.effectSettings_CollisionEnter);
		}
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x000189D2 File Offset: 0x00016BD2
	private void effectSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		base.transform.LookAt(this.effectSettings.transform.position + e.Hit.normal);
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x00121D80 File Offset: 0x0011FF80
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

	// Token: 0x04002D62 RID: 11618
	public bool IsReverse;

	// Token: 0x04002D63 RID: 11619
	public float TimeDelay;

	// Token: 0x04002D64 RID: 11620
	public bool IsLookAt;

	// Token: 0x04002D65 RID: 11621
	private EffectSettings effectSettings;
}
