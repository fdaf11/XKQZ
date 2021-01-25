using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class ChangPath : MonoBehaviour
{
	// Token: 0x06000174 RID: 372 RVA: 0x00003348 File Offset: 0x00001548
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && this.GoThisPath)
		{
			this.cameraControll.GetComponentInChildren<CPEPlayerPathFollow>().path = this.GoThisPath;
		}
	}

	// Token: 0x0400012A RID: 298
	public CameraPath GoThisPath;

	// Token: 0x0400012B RID: 299
	public GameObject cameraControll;
}
