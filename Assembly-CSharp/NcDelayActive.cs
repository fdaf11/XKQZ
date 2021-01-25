using System;

// Token: 0x020003D2 RID: 978
public class NcDelayActive : NcEffectBehaviour
{
	// Token: 0x06001744 RID: 5956 RVA: 0x0000F098 File Offset: 0x0000D298
	public float GetParentDelayTime(bool bCheckStarted)
	{
		return 0f;
	}

	// Token: 0x04001B9C RID: 7068
	public string NotAvailable = "This component is not available.";

	// Token: 0x04001B9D RID: 7069
	public float m_fDelayTime;

	// Token: 0x04001B9E RID: 7070
	public bool m_bActiveRecursively = true;

	// Token: 0x04001B9F RID: 7071
	protected float m_fAliveTime;

	// Token: 0x04001BA0 RID: 7072
	public float m_fParentDelayTime;

	// Token: 0x04001BA1 RID: 7073
	protected bool m_bAddedInvoke;

	// Token: 0x04001BA2 RID: 7074
	protected float m_fStartedTime;
}
