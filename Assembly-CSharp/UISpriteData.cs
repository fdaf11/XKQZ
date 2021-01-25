using System;

// Token: 0x02000509 RID: 1289
[Serializable]
public class UISpriteData
{
	// Token: 0x17000350 RID: 848
	// (get) Token: 0x06002128 RID: 8488 RVA: 0x0001630E File Offset: 0x0001450E
	public bool hasBorder
	{
		get
		{
			return (this.borderLeft | this.borderRight | this.borderTop | this.borderBottom) != 0;
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x06002129 RID: 8489 RVA: 0x00016331 File Offset: 0x00014531
	public bool hasPadding
	{
		get
		{
			return (this.paddingLeft | this.paddingRight | this.paddingTop | this.paddingBottom) != 0;
		}
	}

	// Token: 0x0600212A RID: 8490 RVA: 0x00016354 File Offset: 0x00014554
	public void SetRect(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	// Token: 0x0600212B RID: 8491 RVA: 0x00016373 File Offset: 0x00014573
	public void SetPadding(int left, int bottom, int right, int top)
	{
		this.paddingLeft = left;
		this.paddingBottom = bottom;
		this.paddingRight = right;
		this.paddingTop = top;
	}

	// Token: 0x0600212C RID: 8492 RVA: 0x00016392 File Offset: 0x00014592
	public void SetBorder(int left, int bottom, int right, int top)
	{
		this.borderLeft = left;
		this.borderBottom = bottom;
		this.borderRight = right;
		this.borderTop = top;
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x000F9F1C File Offset: 0x000F811C
	public void CopyFrom(UISpriteData sd)
	{
		this.name = sd.name;
		this.x = sd.x;
		this.y = sd.y;
		this.width = sd.width;
		this.height = sd.height;
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
		this.paddingLeft = sd.paddingLeft;
		this.paddingRight = sd.paddingRight;
		this.paddingTop = sd.paddingTop;
		this.paddingBottom = sd.paddingBottom;
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x000163B1 File Offset: 0x000145B1
	public void CopyBorderFrom(UISpriteData sd)
	{
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
	}

	// Token: 0x04002447 RID: 9287
	public string name = "Sprite";

	// Token: 0x04002448 RID: 9288
	public int x;

	// Token: 0x04002449 RID: 9289
	public int y;

	// Token: 0x0400244A RID: 9290
	public int width;

	// Token: 0x0400244B RID: 9291
	public int height;

	// Token: 0x0400244C RID: 9292
	public int borderLeft;

	// Token: 0x0400244D RID: 9293
	public int borderRight;

	// Token: 0x0400244E RID: 9294
	public int borderTop;

	// Token: 0x0400244F RID: 9295
	public int borderBottom;

	// Token: 0x04002450 RID: 9296
	public int paddingLeft;

	// Token: 0x04002451 RID: 9297
	public int paddingRight;

	// Token: 0x04002452 RID: 9298
	public int paddingTop;

	// Token: 0x04002453 RID: 9299
	public int paddingBottom;
}
