using System;
using UnityEngine;

// Token: 0x02000767 RID: 1895
public class Rotate : MonoBehaviour
{
	// Token: 0x06002D16 RID: 11542 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06002D17 RID: 11543 RVA: 0x0015C330 File Offset: 0x0015A530
	private void Update()
	{
		base.transform.Rotate(Vector3.up * this.rotateSpeed);
		if (this.randomSpeed)
		{
			this.rotateSpeed += Time.deltaTime * Random.Range(-4f, 4f);
			this.rotateSpeed = Mathf.Clamp(this.rotateSpeed, -5f, 5f);
		}
	}

	// Token: 0x0400397A RID: 14714
	public bool randomSpeed = true;

	// Token: 0x0400397B RID: 14715
	public float rotateSpeed = 5f;
}
