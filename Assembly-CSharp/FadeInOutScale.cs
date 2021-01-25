using System;
using UnityEngine;

// Token: 0x020005E1 RID: 1505
public class FadeInOutScale : MonoBehaviour
{
	// Token: 0x0600254C RID: 9548 RVA: 0x00018C95 File Offset: 0x00016E95
	private void Start()
	{
		this.t = base.transform;
		this.oldScale = this.t.localScale;
		this.isInitialized = true;
	}

	// Token: 0x0600254D RID: 9549 RVA: 0x00018CBB File Offset: 0x00016EBB
	public void InitDefaultVariables()
	{
		this.t.localScale = Vector3.zero;
		this.time = 0f;
		this.oldSin = 0f;
		this.canUpdate = true;
		this.updateTime = true;
	}

	// Token: 0x0600254E RID: 9550 RVA: 0x00018CF1 File Offset: 0x00016EF1
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x0600254F RID: 9551 RVA: 0x001224F0 File Offset: 0x001206F0
	private void Update()
	{
		if (!this.canUpdate)
		{
			return;
		}
		if (this.updateTime)
		{
			this.time = Time.time;
			this.updateTime = false;
		}
		float num = Mathf.Sin((Time.time - this.time) / this.Speed);
		float num2;
		if (this.oldSin > num)
		{
			this.canUpdate = false;
			num2 = this.MaxScale;
		}
		else
		{
			num2 = num * this.MaxScale;
		}
		if (this.FadeInOutStatus == FadeInOutStatus.In)
		{
			if (num2 < this.MaxScale)
			{
				this.t.localScale = new Vector3(this.oldScale.x * num2, this.oldScale.y * num2, this.oldScale.z * num2);
			}
			else
			{
				this.t.localScale = new Vector3(this.MaxScale, this.MaxScale, this.MaxScale);
			}
		}
		if (this.FadeInOutStatus == FadeInOutStatus.Out)
		{
			if (num2 > 0f)
			{
				this.t.localScale = new Vector3(this.MaxScale * this.oldScale.x - this.oldScale.x * num2, this.MaxScale * this.oldScale.y - this.oldScale.y * num2, this.MaxScale * this.oldScale.z - this.oldScale.z * num2);
			}
			else
			{
				this.t.localScale = Vector3.zero;
			}
		}
		this.oldSin = num;
	}

	// Token: 0x04002DA3 RID: 11683
	public FadeInOutStatus FadeInOutStatus;

	// Token: 0x04002DA4 RID: 11684
	public float Speed = 1f;

	// Token: 0x04002DA5 RID: 11685
	public float MaxScale = 2f;

	// Token: 0x04002DA6 RID: 11686
	private Vector3 oldScale;

	// Token: 0x04002DA7 RID: 11687
	private float time;

	// Token: 0x04002DA8 RID: 11688
	private float oldSin;

	// Token: 0x04002DA9 RID: 11689
	private bool updateTime = true;

	// Token: 0x04002DAA RID: 11690
	private bool canUpdate = true;

	// Token: 0x04002DAB RID: 11691
	private Transform t;

	// Token: 0x04002DAC RID: 11692
	private bool isInitialized;
}
