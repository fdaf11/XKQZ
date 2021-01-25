using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005EA RID: 1514
public class RandomRotate02 : MonoBehaviour
{
	// Token: 0x06002587 RID: 9607 RVA: 0x0012378C File Offset: 0x0012198C
	private void Start()
	{
		this.deltaTime = 1f / (float)this.fps;
		this.rangeX = (float)Random.Range(0, 10);
		this.rangeY = (float)Random.Range(0, 10);
		this.rangeZ = (float)Random.Range(0, 10);
	}

	// Token: 0x06002588 RID: 9608 RVA: 0x00018FE1 File Offset: 0x000171E1
	private void OnBecameVisible()
	{
		this.isVisible = true;
		base.StartCoroutine(this.UpdateRotation());
	}

	// Token: 0x06002589 RID: 9609 RVA: 0x00018FF7 File Offset: 0x000171F7
	private void OnBecameInvisible()
	{
		this.isVisible = false;
	}

	// Token: 0x0600258A RID: 9610 RVA: 0x001237DC File Offset: 0x001219DC
	private IEnumerator UpdateRotation()
	{
		while (this.isVisible)
		{
			if (this.isRotate)
			{
				base.transform.Rotate(this.deltaTime * Mathf.Sin(Time.time + this.rangeX) * (float)this.x, this.deltaTime * Mathf.Sin(Time.time + this.rangeY) * (float)this.y, this.deltaTime * Mathf.Sin(Time.time + this.rangeZ) * (float)this.z);
			}
			yield return new WaitForSeconds(this.deltaTime);
		}
		yield break;
	}

	// Token: 0x04002E06 RID: 11782
	public bool isRotate = true;

	// Token: 0x04002E07 RID: 11783
	public int fps = 30;

	// Token: 0x04002E08 RID: 11784
	public int x = 100;

	// Token: 0x04002E09 RID: 11785
	public int y = 200;

	// Token: 0x04002E0A RID: 11786
	public int z = 300;

	// Token: 0x04002E0B RID: 11787
	private float rangeX;

	// Token: 0x04002E0C RID: 11788
	private float rangeY;

	// Token: 0x04002E0D RID: 11789
	private float rangeZ;

	// Token: 0x04002E0E RID: 11790
	private float deltaTime;

	// Token: 0x04002E0F RID: 11791
	private bool isVisible;
}
