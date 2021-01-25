using System;
using UnityEngine;

// Token: 0x020003DB RID: 987
public class NcRotation : NcEffectBehaviour
{
	// Token: 0x06001795 RID: 6037 RVA: 0x000C2060 File Offset: 0x000C0260
	private void Update()
	{
		base.transform.Rotate(NcEffectBehaviour.GetEngineDeltaTime() * this.m_vRotationValue.x, NcEffectBehaviour.GetEngineDeltaTime() * this.m_vRotationValue.y, NcEffectBehaviour.GetEngineDeltaTime() * this.m_vRotationValue.z, (!this.m_bWorldSpace) ? 1 : 0);
	}

	// Token: 0x06001796 RID: 6038 RVA: 0x0000F5DF File Offset: 0x0000D7DF
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_vRotationValue *= fSpeedRate;
	}

	// Token: 0x04001C26 RID: 7206
	public bool m_bWorldSpace;

	// Token: 0x04001C27 RID: 7207
	public Vector3 m_vRotationValue = new Vector3(0f, 360f, 0f);
}
