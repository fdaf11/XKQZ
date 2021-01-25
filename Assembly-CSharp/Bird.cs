using System;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class Bird : MonoBehaviour
{
	// Token: 0x06002253 RID: 8787 RVA: 0x0010CD18 File Offset: 0x0010AF18
	private void Start()
	{
		this.anim = base.GetComponent<Animator>();
		this.angleX = (float)Random.Range(0, 360);
		this.angleY = (float)Random.Range(0, 360);
		this.angleZ = (float)Random.Range(0, 360);
		this.lastPosition = this.GetNewPos();
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x0010CD74 File Offset: 0x0010AF74
	private void OnAnimatorMove()
	{
		if (this.anim.GetCurrentAnimatorStateInfo(0).IsTag("NewAnim"))
		{
			if (this.canChangeAnim)
			{
				this.anim.SetInteger("AnimNum", Random.Range(0, this.animCount + 1));
				this.canChangeAnim = false;
			}
		}
		else
		{
			this.canChangeAnim = true;
		}
		Vector3 newPos = this.GetNewPos();
		base.transform.position += newPos - this.lastPosition;
		this.lastPosition = newPos;
		this.angleX = Mathf.MoveTowardsAngle(this.angleX, this.angleX + this.speedX * Time.deltaTime, this.speedX * Time.deltaTime);
		this.angleY = Mathf.MoveTowardsAngle(this.angleY, this.angleY + this.speedY * Time.deltaTime, this.speedY * Time.deltaTime);
		this.angleZ = Mathf.MoveTowardsAngle(this.angleZ, this.angleZ + this.speedZ * Time.deltaTime, this.speedZ * Time.deltaTime);
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x0010CE9C File Offset: 0x0010B09C
	private Vector3 GetNewPos()
	{
		Vector3 result;
		result.x = Mathf.Sin(this.angleX * 0.017453292f) * this.amplitudeX;
		result.y = Mathf.Sin(this.angleY * 0.017453292f) * this.amplitudeY;
		result.z = Mathf.Sin(this.angleZ * 0.017453292f) * this.amplitudeZ;
		return result;
	}

	// Token: 0x0400287F RID: 10367
	public int animCount = 2;

	// Token: 0x04002880 RID: 10368
	public float speedX;

	// Token: 0x04002881 RID: 10369
	public float speedY;

	// Token: 0x04002882 RID: 10370
	public float speedZ;

	// Token: 0x04002883 RID: 10371
	public float amplitudeX;

	// Token: 0x04002884 RID: 10372
	public float amplitudeY;

	// Token: 0x04002885 RID: 10373
	public float amplitudeZ;

	// Token: 0x04002886 RID: 10374
	private Animator anim;

	// Token: 0x04002887 RID: 10375
	private bool canChangeAnim;

	// Token: 0x04002888 RID: 10376
	private float angleX;

	// Token: 0x04002889 RID: 10377
	private float angleY;

	// Token: 0x0400288A RID: 10378
	private float angleZ;

	// Token: 0x0400288B RID: 10379
	private Vector3 lastPosition;
}
