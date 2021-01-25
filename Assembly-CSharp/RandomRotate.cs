using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000765 RID: 1893
public class RandomRotate : MonoBehaviour
{
	// Token: 0x06002D0C RID: 11532 RVA: 0x0001D09F File Offset: 0x0001B29F
	private void Start()
	{
		this.thisT = base.transform;
		base.StartCoroutine(this.RotateRoutine());
	}

	// Token: 0x06002D0D RID: 11533 RVA: 0x0015C0B8 File Offset: 0x0015A2B8
	private IEnumerator RotateRoutine()
	{
		yield return new WaitForSeconds(Random.Range(1f, 5f));
		for (;;)
		{
			while (this.unit != null && this.unit.InAction())
			{
				yield return new WaitForSeconds(Random.Range(1f, 3f));
			}
			this.rotateSpeed = (float)Random.Range(3, 6);
			float val = Random.Range(this.min, this.max);
			if (this.rotateAxis == _Axis.X)
			{
				this.targetRot = Quaternion.Euler(val, 0f, 0f);
			}
			else if (this.rotateAxis == _Axis.Y)
			{
				this.targetRot = Quaternion.Euler(0f, val, 0f);
			}
			else if (this.rotateAxis == _Axis.Z)
			{
				this.targetRot = Quaternion.Euler(0f, 0f, val);
			}
			yield return new WaitForSeconds(Random.Range(3f, 6f));
		}
		yield break;
	}

	// Token: 0x06002D0E RID: 11534 RVA: 0x0015C0D4 File Offset: 0x0015A2D4
	private void Update()
	{
		if (this.unit == null)
		{
			this.thisT.localRotation = Quaternion.Slerp(this.thisT.localRotation, this.targetRot, Time.deltaTime * this.rotateSpeed);
		}
		else if (!this.unit.InAction())
		{
			this.thisT.localRotation = Quaternion.Slerp(this.thisT.localRotation, this.targetRot, Time.deltaTime * this.rotateSpeed);
		}
		else
		{
			this.targetRot = this.thisT.localRotation;
		}
	}

	// Token: 0x0400396F RID: 14703
	public _Axis rotateAxis;

	// Token: 0x04003970 RID: 14704
	public float min = -30f;

	// Token: 0x04003971 RID: 14705
	public float max = 30f;

	// Token: 0x04003972 RID: 14706
	public UnitTB unit;

	// Token: 0x04003973 RID: 14707
	private Quaternion targetRot;

	// Token: 0x04003974 RID: 14708
	private float rotateSpeed;

	// Token: 0x04003975 RID: 14709
	private Transform thisT;
}
