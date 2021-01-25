using System;
using UnityEngine;

// Token: 0x020003A3 RID: 931
public class NcDetachObject : NcEffectBehaviour
{
	// Token: 0x0600159F RID: 5535 RVA: 0x000B9054 File Offset: 0x000B7254
	public static NcDetachObject Create(GameObject parentObj, GameObject linkObject)
	{
		NcDetachObject ncDetachObject = parentObj.AddComponent<NcDetachObject>();
		ncDetachObject.m_LinkGameObject = linkObject;
		return ncDetachObject;
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x0000DBA1 File Offset: 0x0000BDA1
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		if (bRuntime)
		{
			NsEffectManager.AdjustSpeedRuntime(this.m_LinkGameObject, fSpeedRate);
		}
	}

	// Token: 0x060015A1 RID: 5537 RVA: 0x0000DBB5 File Offset: 0x0000BDB5
	public override void OnSetActiveRecursively(bool bActive)
	{
		if (this.m_LinkGameObject != null)
		{
			NsEffectManager.SetActiveRecursively(this.m_LinkGameObject, bActive);
		}
	}

	// Token: 0x04001A40 RID: 6720
	public GameObject m_LinkGameObject;
}
