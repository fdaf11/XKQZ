using System;
using UnityEngine;

// Token: 0x020005F9 RID: 1529
public class ResetPositionOnDiactivated : MonoBehaviour
{
	// Token: 0x060025D2 RID: 9682 RVA: 0x000192D1 File Offset: 0x000174D1
	private void Start()
	{
		this.EffectSettings.EffectDeactivated += new EventHandler(this.EffectSettings_EffectDeactivated);
	}

	// Token: 0x060025D3 RID: 9683 RVA: 0x000192EA File Offset: 0x000174EA
	private void EffectSettings_EffectDeactivated(object sender, EventArgs e)
	{
		base.transform.localPosition = Vector3.zero;
	}

	// Token: 0x04002E5D RID: 11869
	public EffectSettings EffectSettings;
}
