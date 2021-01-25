using System;
using UnityEngine;

// Token: 0x020005AD RID: 1453
public class rotationHelper : MonoBehaviour
{
	// Token: 0x06002446 RID: 9286 RVA: 0x0011BE00 File Offset: 0x0011A000
	private void Update()
	{
		if (this.rotateX)
		{
			base.transform.Rotate(Vector3.left * this.speedX * Time.deltaTime);
		}
		if (this.rotateY)
		{
			base.transform.Rotate(Vector3.up * this.speedY * Time.deltaTime);
		}
		if (this.rotateZ)
		{
			base.transform.Rotate(Vector3.forward * this.speedZ * Time.deltaTime);
		}
	}

	// Token: 0x04002C1A RID: 11290
	public bool rotateX;

	// Token: 0x04002C1B RID: 11291
	public bool rotateY;

	// Token: 0x04002C1C RID: 11292
	public bool rotateZ;

	// Token: 0x04002C1D RID: 11293
	public float speedX;

	// Token: 0x04002C1E RID: 11294
	public float speedY;

	// Token: 0x04002C1F RID: 11295
	public float speedZ;
}
