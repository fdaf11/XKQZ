using System;
using UnityEngine;

// Token: 0x020005F8 RID: 1528
public class OnRigidbodySendCollision : MonoBehaviour
{
	// Token: 0x060025CE RID: 9678 RVA: 0x00124C10 File Offset: 0x00122E10
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

	// Token: 0x060025CF RID: 9679 RVA: 0x000192B1 File Offset: 0x000174B1
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
	}

	// Token: 0x060025D0 RID: 9680 RVA: 0x000192BF File Offset: 0x000174BF
	private void OnCollisionEnter(Collision collision)
	{
		this.effectSettings.OnCollisionHandler(new CollisionInfo());
	}

	// Token: 0x04002E5C RID: 11868
	private EffectSettings effectSettings;
}
