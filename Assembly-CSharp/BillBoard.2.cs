using System;
using UnityEngine;

// Token: 0x0200064E RID: 1614
public class BillBoard : MonoBehaviour
{
	// Token: 0x060027D1 RID: 10193 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060027D2 RID: 10194 RVA: 0x0001A405 File Offset: 0x00018605
	private void Update()
	{
		base.transform.rotation = Camera.main.transform.rotation;
	}
}
