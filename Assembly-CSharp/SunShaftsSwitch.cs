using System;
using UnityEngine;

// Token: 0x02000694 RID: 1684
public class SunShaftsSwitch : MonoBehaviour
{
	// Token: 0x060028E3 RID: 10467 RVA: 0x00144CDC File Offset: 0x00142EDC
	private void Start()
	{
		if (this.CameraObj.GetComponent<SunShafts>().enabled && this.OnePlace)
		{
			this.CameraObj.GetComponent<SunShafts>().enabled = false;
			this.CanDo = true;
		}
		if (!this.OnePlace)
		{
			this.CanDo = true;
		}
	}

	// Token: 0x060028E4 RID: 10468 RVA: 0x0001AEA3 File Offset: 0x000190A3
	private void OnTriggerEnter(Collider playerOne)
	{
		if (playerOne.CompareTag("Player") && this.CanDo)
		{
			this.CameraObj.GetComponent<SunShafts>().enabled = true;
		}
	}

	// Token: 0x060028E5 RID: 10469 RVA: 0x0001AED1 File Offset: 0x000190D1
	private void OnTriggerExit(Collider playerOne)
	{
		if (playerOne.CompareTag("Player") && this.CanDo)
		{
			this.CameraObj.GetComponent<SunShafts>().enabled = false;
		}
	}

	// Token: 0x040033BD RID: 13245
	public GameObject CameraObj;

	// Token: 0x040033BE RID: 13246
	private bool CanDo;

	// Token: 0x040033BF RID: 13247
	public bool OnePlace = true;
}
