using System;
using UnityEngine;

// Token: 0x02000782 RID: 1922
public class EffectOverlay
{
	// Token: 0x06002DF5 RID: 11765 RVA: 0x0001D6BD File Offset: 0x0001B8BD
	public EffectOverlay(Vector3 p, string m, _OverlayType oType, float time = 0f)
	{
		this.pos = p;
		this.msg = m;
		this.overType = oType;
		this.fTime = time;
		if (EffectOverlay.onEffectOverlayE != null)
		{
			EffectOverlay.onEffectOverlayE(this);
		}
	}

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06002DF6 RID: 11766 RVA: 0x0001D6F4 File Offset: 0x0001B8F4
	// (remove) Token: 0x06002DF7 RID: 11767 RVA: 0x0001D70B File Offset: 0x0001B90B
	public static event EffectOverlay.EffectOverlayHandler onEffectOverlayE;

	// Token: 0x04003A44 RID: 14916
	public Vector3 pos;

	// Token: 0x04003A45 RID: 14917
	public string msg;

	// Token: 0x04003A46 RID: 14918
	public _OverlayType overType;

	// Token: 0x04003A47 RID: 14919
	public float fTime;

	// Token: 0x02000783 RID: 1923
	// (Invoke) Token: 0x06002DF9 RID: 11769
	public delegate void EffectOverlayHandler(EffectOverlay eff);
}
