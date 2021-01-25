using System;
using UnityEngine;

// Token: 0x02000494 RID: 1172
[Serializable]
public class BMSymbol
{
	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06001C39 RID: 7225 RVA: 0x00012CBC File Offset: 0x00010EBC
	public int length
	{
		get
		{
			if (this.mLength == 0)
			{
				this.mLength = this.sequence.Length;
			}
			return this.mLength;
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06001C3A RID: 7226 RVA: 0x00012CE0 File Offset: 0x00010EE0
	public int offsetX
	{
		get
		{
			return this.mOffsetX;
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06001C3B RID: 7227 RVA: 0x00012CE8 File Offset: 0x00010EE8
	public int offsetY
	{
		get
		{
			return this.mOffsetY;
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06001C3C RID: 7228 RVA: 0x00012CF0 File Offset: 0x00010EF0
	public int width
	{
		get
		{
			return this.mWidth;
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06001C3D RID: 7229 RVA: 0x00012CF8 File Offset: 0x00010EF8
	public int height
	{
		get
		{
			return this.mHeight;
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06001C3E RID: 7230 RVA: 0x00012D00 File Offset: 0x00010F00
	public int advance
	{
		get
		{
			return this.mAdvance;
		}
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06001C3F RID: 7231 RVA: 0x00012D08 File Offset: 0x00010F08
	public Rect uvRect
	{
		get
		{
			return this.mUV;
		}
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x00012D10 File Offset: 0x00010F10
	public void MarkAsChanged()
	{
		this.mIsValid = false;
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x000DC700 File Offset: 0x000DA900
	public bool Validate(UIAtlas atlas)
	{
		if (atlas == null)
		{
			return false;
		}
		if (!this.mIsValid)
		{
			if (string.IsNullOrEmpty(this.spriteName))
			{
				return false;
			}
			this.mSprite = ((!(atlas != null)) ? null : atlas.GetSprite(this.spriteName));
			if (this.mSprite != null)
			{
				Texture texture = atlas.texture;
				if (texture == null)
				{
					this.mSprite = null;
				}
				else
				{
					this.mUV = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
					this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
					this.mOffsetX = this.mSprite.paddingLeft;
					this.mOffsetY = this.mSprite.paddingTop;
					this.mWidth = this.mSprite.width;
					this.mHeight = this.mSprite.height;
					this.mAdvance = this.mSprite.width + (this.mSprite.paddingLeft + this.mSprite.paddingRight);
					this.mIsValid = true;
				}
			}
		}
		return this.mSprite != null;
	}

	// Token: 0x0400212B RID: 8491
	public string sequence;

	// Token: 0x0400212C RID: 8492
	public string spriteName;

	// Token: 0x0400212D RID: 8493
	private UISpriteData mSprite;

	// Token: 0x0400212E RID: 8494
	private bool mIsValid;

	// Token: 0x0400212F RID: 8495
	private int mLength;

	// Token: 0x04002130 RID: 8496
	private int mOffsetX;

	// Token: 0x04002131 RID: 8497
	private int mOffsetY;

	// Token: 0x04002132 RID: 8498
	private int mWidth;

	// Token: 0x04002133 RID: 8499
	private int mHeight;

	// Token: 0x04002134 RID: 8500
	private int mAdvance;

	// Token: 0x04002135 RID: 8501
	private Rect mUV;
}
