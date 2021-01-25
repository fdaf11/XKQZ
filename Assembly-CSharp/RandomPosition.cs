using System;
using UnityEngine;

// Token: 0x020005E9 RID: 1513
public class RandomPosition : MonoBehaviour
{
	// Token: 0x06002584 RID: 9604 RVA: 0x001236AC File Offset: 0x001218AC
	private void Start()
	{
		this.rangeX = Random.Range(0f, this.x);
		this.rangeY = Random.Range(0f, this.y);
		this.rangeZ = Random.Range(0f, this.z);
	}

	// Token: 0x06002585 RID: 9605 RVA: 0x001236FC File Offset: 0x001218FC
	private void Update()
	{
		Vector3 vector;
		vector..ctor(Time.deltaTime * Mathf.Sin(Time.time + this.rangeX) * this.x, Time.deltaTime * Mathf.Sin(Time.time + this.rangeY) * this.y, Time.deltaTime * Mathf.Sin(Time.time + this.rangeZ) * this.z);
		base.transform.position += vector * Time.deltaTime;
	}

	// Token: 0x04002DFF RID: 11775
	public int fps = 30;

	// Token: 0x04002E00 RID: 11776
	public float x = 1f;

	// Token: 0x04002E01 RID: 11777
	public float y = 2f;

	// Token: 0x04002E02 RID: 11778
	public float z = 3f;

	// Token: 0x04002E03 RID: 11779
	private float rangeX;

	// Token: 0x04002E04 RID: 11780
	private float rangeY;

	// Token: 0x04002E05 RID: 11781
	private float rangeZ;
}
