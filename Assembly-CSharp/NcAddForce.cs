using System;
using UnityEngine;

// Token: 0x020003BE RID: 958
public class NcAddForce : NcEffectBehaviour
{
	// Token: 0x060016C9 RID: 5833 RVA: 0x0000EA01 File Offset: 0x0000CC01
	private void Start()
	{
		if (!base.enabled)
		{
			return;
		}
		this.AddForce();
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x000BCD08 File Offset: 0x000BAF08
	private void AddForce()
	{
		if (base.rigidbody != null)
		{
			Vector3 vector;
			vector..ctor(Random.Range(-this.m_RandomRange.x, this.m_RandomRange.x) + this.m_AddForce.x, Random.Range(-this.m_RandomRange.y, this.m_RandomRange.y) + this.m_AddForce.y, Random.Range(-this.m_RandomRange.z, this.m_RandomRange.z) + this.m_AddForce.z);
			base.rigidbody.AddForce(vector, this.m_ForceMode);
		}
	}

	// Token: 0x04001AFA RID: 6906
	public Vector3 m_AddForce = new Vector3(0f, 300f, 0f);

	// Token: 0x04001AFB RID: 6907
	public Vector3 m_RandomRange = new Vector3(100f, 100f, 100f);

	// Token: 0x04001AFC RID: 6908
	public ForceMode m_ForceMode;
}
