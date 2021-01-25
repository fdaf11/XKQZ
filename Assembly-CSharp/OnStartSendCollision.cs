using System;
using UnityEngine;

// Token: 0x020005E6 RID: 1510
public class OnStartSendCollision : MonoBehaviour
{
	// Token: 0x06002574 RID: 9588 RVA: 0x00123300 File Offset: 0x00121500
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

	// Token: 0x06002575 RID: 9589 RVA: 0x00018EB4 File Offset: 0x000170B4
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		this.effectSettings.OnCollisionHandler(new CollisionInfo());
		this.isInitialized = true;
	}

	// Token: 0x06002576 RID: 9590 RVA: 0x00018ED9 File Offset: 0x000170D9
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.effectSettings.OnCollisionHandler(new CollisionInfo());
		}
	}

	// Token: 0x04002DEC RID: 11756
	private EffectSettings effectSettings;

	// Token: 0x04002DED RID: 11757
	private bool isInitialized;
}
