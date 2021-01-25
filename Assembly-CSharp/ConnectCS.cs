using System;
using UnityEngine;

// Token: 0x020006D6 RID: 1750
public class ConnectCS : MonoBehaviour
{
	// Token: 0x06002A2C RID: 10796 RVA: 0x0014B9FC File Offset: 0x00149BFC
	public void EnableMeleeWeaponTrail()
	{
		MeleeWeaponTrail[] componentsInChildren = base.GetComponentsInChildren<MeleeWeaponTrail>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = true;
		}
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x0014BA30 File Offset: 0x00149C30
	public void DisableMeleeWeaponTrail()
	{
		MeleeWeaponTrail[] componentsInChildren = base.GetComponentsInChildren<MeleeWeaponTrail>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Emit = false;
		}
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x0014BA64 File Offset: 0x00149C64
	public void DestroyMeleeWeaponTrail()
	{
		MeleeWeaponTrail[] componentsInChildren = base.GetComponentsInChildren<MeleeWeaponTrail>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.Destroy(componentsInChildren[i]);
		}
	}
}
