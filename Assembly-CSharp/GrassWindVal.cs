using System;
using UnityEngine;

// Token: 0x0200066F RID: 1647
public class GrassWindVal : MonoBehaviour
{
	// Token: 0x0600284C RID: 10316 RVA: 0x0001A889 File Offset: 0x00018A89
	private void Start()
	{
		base.transform.GetComponentInChildren<SetupAdvancedFoliageShader>().WindMultiplierForGrassshader = this.WindGrassLen;
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x0400328F RID: 12943
	public float WindGrassLen = 0.3f;

	// Token: 0x04003290 RID: 12944
	private GameObject SAFS;
}
