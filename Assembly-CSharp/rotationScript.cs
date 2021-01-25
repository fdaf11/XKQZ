using System;
using UnityEngine;

// Token: 0x020005AA RID: 1450
public class rotationScript : MonoBehaviour
{
	// Token: 0x0600243F RID: 9279 RVA: 0x00018101 File Offset: 0x00016301
	private void Start()
	{
		this.tempVector = base.transform.position;
	}

	// Token: 0x06002440 RID: 9280 RVA: 0x0011BB6C File Offset: 0x00119D6C
	private void Update()
	{
		if (this.wiggleX)
		{
			float num;
			if (this.xCos)
			{
				num = this.tempVector.x + Mathf.Cos(this.animationProgres) * this.amountX;
			}
			else
			{
				num = this.tempVector.x + Mathf.Sin(this.animationProgres) * this.amountX;
			}
			base.transform.position = new Vector3(num, base.transform.position.y, base.transform.position.z);
		}
		if (this.wiggleY)
		{
			float num2;
			if (this.yCos)
			{
				num2 = this.tempVector.y + Mathf.Cos(this.animationProgres) * this.amountY;
			}
			else
			{
				num2 = this.tempVector.y + Mathf.Sin(this.animationProgres) * this.amountY;
			}
			base.transform.position = new Vector3(base.transform.position.x, num2, base.transform.position.z);
		}
		if (this.wiggleZ)
		{
			float num3;
			if (this.zCos)
			{
				num3 = this.tempVector.z + Mathf.Cos(this.animationProgres) * this.amountZ;
			}
			else
			{
				num3 = this.tempVector.z + Mathf.Sin(this.animationProgres) * this.amountZ;
			}
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, num3);
		}
		this.animationProgres += this.animationSpeed * Time.deltaTime;
	}

	// Token: 0x04002C0D RID: 11277
	public bool wiggleX;

	// Token: 0x04002C0E RID: 11278
	public bool wiggleY;

	// Token: 0x04002C0F RID: 11279
	public bool wiggleZ;

	// Token: 0x04002C10 RID: 11280
	public bool xCos;

	// Token: 0x04002C11 RID: 11281
	public bool yCos;

	// Token: 0x04002C12 RID: 11282
	public bool zCos;

	// Token: 0x04002C13 RID: 11283
	public float amountX;

	// Token: 0x04002C14 RID: 11284
	public float amountY;

	// Token: 0x04002C15 RID: 11285
	public float amountZ;

	// Token: 0x04002C16 RID: 11286
	public float animationSpeed;

	// Token: 0x04002C17 RID: 11287
	private float animationProgres;

	// Token: 0x04002C18 RID: 11288
	private Vector3 tempVector;
}
