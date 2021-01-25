using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class NpcPositionDirect : MonoBehaviour
{
	// Token: 0x0600032F RID: 815 RVA: 0x0002BC04 File Offset: 0x00029E04
	public void SetNpcPositionDirect(Transform tTemp)
	{
		this.tNpc = tTemp;
		if (this.labelX != null)
		{
			this.labelX.text = this.tNpc.position.x.ToString("0.0");
		}
		if (this.labelY != null)
		{
			this.labelY.text = this.tNpc.position.y.ToString("0.0");
		}
		if (this.labelZ != null)
		{
			this.labelZ.text = this.tNpc.position.z.ToString("0.0");
		}
		if (this.labelDir != null)
		{
			this.labelDir.text = this.tNpc.localEulerAngles.y.ToString("0.0");
		}
	}

	// Token: 0x06000330 RID: 816 RVA: 0x0002BCFC File Offset: 0x00029EFC
	public void UpdateNpcPostionDirect()
	{
		if (this.tNpc != null)
		{
			float num = 0f;
			if (this.labelX != null && !float.TryParse(this.labelX.text, ref num))
			{
				num = 0f;
			}
			float num2 = 0f;
			if (this.labelY != null && !float.TryParse(this.labelY.text, ref num2))
			{
				num2 = 0f;
			}
			float num3 = 0f;
			if (this.labelZ != null && !float.TryParse(this.labelZ.text, ref num3))
			{
				num3 = 0f;
			}
			float num4 = 0f;
			if (this.labelDir != null && !float.TryParse(this.labelDir.text, ref num4))
			{
				num4 = 0f;
			}
			this.tNpc.position = new Vector3(num, num2, num3);
			this.tNpc.rotation = Quaternion.identity;
			this.tNpc.Rotate(0f, num4, 0f);
		}
	}

	// Token: 0x04000253 RID: 595
	public UILabel labelX;

	// Token: 0x04000254 RID: 596
	public UILabel labelY;

	// Token: 0x04000255 RID: 597
	public UILabel labelZ;

	// Token: 0x04000256 RID: 598
	public UILabel labelDir;

	// Token: 0x04000257 RID: 599
	private Transform tNpc;
}
