using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class HexIndicatorProjectorControl : MonoBehaviour
{
	// Token: 0x060005BE RID: 1470 RVA: 0x00042120 File Offset: 0x00040320
	private void Start()
	{
		this.tMaterial1 = new Material(this.project1.material);
		this.tMaterial2 = new Material(this.project2.material);
		this.tMaterial1.name = "TempMaterial 1";
		this.project1.material = this.tMaterial1;
		this.tMaterial2.name = "TempMaterial 2";
		this.project2.material = this.tMaterial2;
		if (this.Mode == 0)
		{
			this.colorS1 = new Color(0f, 0f, 0f);
			this.colorE1 = new Color(0.5f, 0.5f, 0f);
			this.colorS2 = new Color(0.5f, 0.5f, 0f);
			this.colorE2 = new Color(0f, 0f, 0f);
			this.vSize1 = new Vector2(0.9f, 1.2f);
			this.vSize2 = new Vector2(1.2f, 0.9f);
			this.fPlus = 0f;
		}
		else if (this.Mode == 1)
		{
			this.colorS1 = new Color(0f, 0f, 0f);
			this.colorE1 = new Color(0f, 0.5f, 0f);
			this.colorS2 = new Color(0f, 0f, 0f);
			this.colorE2 = new Color(0f, 0.5f, 0f);
			this.vSize1 = new Vector2(0.9f, 0.9f);
			this.vSize2 = new Vector2(1.2f, 1.2f);
		}
		else if (this.Mode == 2)
		{
			this.colorS1 = new Color(0f, 0f, 0f);
			this.colorE1 = new Color(0.5f, 0f, 0f);
			this.colorS2 = new Color(0f, 0f, 0f);
			this.colorE2 = new Color(0.5f, 0f, 0f);
			this.vSize1 = new Vector2(1.2f, 1.2f);
			this.vSize2 = new Vector2(0.9f, 0.9f);
		}
		this.fPos = 0f;
		this.fColorPos = 0f;
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x000055CB File Offset: 0x000037CB
	public void SetTileColor(int iType)
	{
		base.StartCoroutine(this._SetTileColor(iType));
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x000423A0 File Offset: 0x000405A0
	private IEnumerator _SetTileColor(int iType)
	{
		yield return new WaitForSeconds(0.1f);
		Color cTemp;
		switch (iType)
		{
		case 0:
			cTemp = new Color(0f, 0f, 0f);
			break;
		case 1:
			cTemp = new Color(0.5f, 0f, 0f);
			break;
		case 2:
			cTemp = new Color(0f, 0.5f, 0f);
			break;
		case 3:
			cTemp = new Color(0f, 0.25f, 0.75f);
			break;
		default:
			cTemp = new Color(0.5f, 0.5f, 0.5f);
			break;
		}
		switch (this.Mode)
		{
		case 0:
			this.colorE1 = cTemp;
			this.colorS2 = cTemp;
			goto IL_16B;
		}
		this.colorE1 = cTemp;
		this.colorE2 = cTemp;
		IL_16B:
		yield break;
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x000423CC File Offset: 0x000405CC
	private void Update()
	{
		this.fPos += Time.deltaTime * this.fSizeRate;
		float num = this.fSizeRate;
		this.fPos = this.MathPos(this.fPos, ref num, this.bSizePingpon);
		this.fSizeRate = num;
		Vector2 vector = Vector2.Lerp(this.vSize1, this.vSize2, this.fPos);
		this.project1.orthographicSize = vector.x;
		float num2 = this.fPos + this.fPlus * this.fSizeRate;
		num2 = this.MathPos(num2, ref num, this.bSizePingpon);
		vector = Vector2.Lerp(this.vSize1, this.vSize2, num2);
		this.project2.orthographicSize = vector.y;
		this.fColorPos += Time.deltaTime * this.fColorRate;
		num = this.fColorRate;
		this.fColorPos = this.MathPos(this.fColorPos, ref num, this.bColorPingpon);
		this.fColorRate = num;
		this.tMaterial1.color = Color.Lerp(this.colorS1, this.colorE1, this.fColorPos);
		float num3 = this.fColorPos + this.fColorPlus * this.fColorRate;
		num3 = this.MathPos(num3, ref num, this.bColorPingpon);
		this.tMaterial2.color = Color.Lerp(this.colorS2, this.colorE2, num3);
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x00042534 File Offset: 0x00040734
	private float MathPos(float fNow, ref float fRate, bool pingpon)
	{
		if (pingpon)
		{
			if (fNow > 1f)
			{
				fNow = 1f - (fNow - Mathf.Floor(fNow));
				fRate = -fRate;
			}
			else if (fNow < 0f)
			{
				fNow = -fNow;
				fNow -= Mathf.Floor(fNow);
				fRate = -fRate;
			}
		}
		else if (fNow > 1f)
		{
			fNow -= Mathf.Floor(fNow);
		}
		return fNow;
	}

	// Token: 0x0400062E RID: 1582
	public Projector project1;

	// Token: 0x0400062F RID: 1583
	public Projector project2;

	// Token: 0x04000630 RID: 1584
	public int Mode;

	// Token: 0x04000631 RID: 1585
	private Material tMaterial1;

	// Token: 0x04000632 RID: 1586
	private Material tMaterial2;

	// Token: 0x04000633 RID: 1587
	private Color colorS1;

	// Token: 0x04000634 RID: 1588
	private Color colorS2;

	// Token: 0x04000635 RID: 1589
	private Color colorE1;

	// Token: 0x04000636 RID: 1590
	private Color colorE2;

	// Token: 0x04000637 RID: 1591
	private Vector2 vSize1;

	// Token: 0x04000638 RID: 1592
	private Vector2 vSize2;

	// Token: 0x04000639 RID: 1593
	private float fPos;

	// Token: 0x0400063A RID: 1594
	private float fColorPos;

	// Token: 0x0400063B RID: 1595
	public float fPlus;

	// Token: 0x0400063C RID: 1596
	public float fColorPlus;

	// Token: 0x0400063D RID: 1597
	public bool bColorPingpon;

	// Token: 0x0400063E RID: 1598
	public bool bSizePingpon;

	// Token: 0x0400063F RID: 1599
	public float fSizeRate;

	// Token: 0x04000640 RID: 1600
	public float fColorRate;
}
