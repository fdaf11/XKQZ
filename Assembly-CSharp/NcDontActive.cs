using System;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class NcDontActive : NcEffectBehaviour
{
	// Token: 0x060015A4 RID: 5540 RVA: 0x0000DBD4 File Offset: 0x0000BDD4
	private void Awake()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x0000264F File Offset: 0x0000084F
	private void OnEnable()
	{
	}
}
