using System;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public abstract class UIBasicSprite : UIWidget
{
	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06001D8F RID: 7567 RVA: 0x000138E8 File Offset: 0x00011AE8
	// (set) Token: 0x06001D90 RID: 7568 RVA: 0x000138F0 File Offset: 0x00011AF0
	public virtual UIBasicSprite.Type type
	{
		get
		{
			return this.mType;
		}
		set
		{
			if (this.mType != value)
			{
				this.mType = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06001D91 RID: 7569 RVA: 0x0001390B File Offset: 0x00011B0B
	// (set) Token: 0x06001D92 RID: 7570 RVA: 0x00013913 File Offset: 0x00011B13
	public UIBasicSprite.Flip flip
	{
		get
		{
			return this.mFlip;
		}
		set
		{
			if (this.mFlip != value)
			{
				this.mFlip = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06001D93 RID: 7571 RVA: 0x0001392E File Offset: 0x00011B2E
	// (set) Token: 0x06001D94 RID: 7572 RVA: 0x00013936 File Offset: 0x00011B36
	public UIBasicSprite.FillDirection fillDirection
	{
		get
		{
			return this.mFillDirection;
		}
		set
		{
			if (this.mFillDirection != value)
			{
				this.mFillDirection = value;
				this.mChanged = true;
			}
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06001D95 RID: 7573 RVA: 0x00013952 File Offset: 0x00011B52
	// (set) Token: 0x06001D96 RID: 7574 RVA: 0x000E5C50 File Offset: 0x000E3E50
	public float fillAmount
	{
		get
		{
			return this.mFillAmount;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mFillAmount != num)
			{
				this.mFillAmount = num;
				this.mChanged = true;
			}
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06001D97 RID: 7575 RVA: 0x000E5C80 File Offset: 0x000E3E80
	public override int minWidth
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.x + vector.z);
				return Mathf.Max(base.minWidth, ((num & 1) != 1) ? num : (num + 1));
			}
			return base.minWidth;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06001D98 RID: 7576 RVA: 0x000E5CF0 File Offset: 0x000E3EF0
	public override int minHeight
	{
		get
		{
			if (this.type == UIBasicSprite.Type.Sliced || this.type == UIBasicSprite.Type.Advanced)
			{
				Vector4 vector = this.border * this.pixelSize;
				int num = Mathf.RoundToInt(vector.y + vector.w);
				return Mathf.Max(base.minHeight, ((num & 1) != 1) ? num : (num + 1));
			}
			return base.minHeight;
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06001D99 RID: 7577 RVA: 0x0001395A File Offset: 0x00011B5A
	// (set) Token: 0x06001D9A RID: 7578 RVA: 0x00013962 File Offset: 0x00011B62
	public bool invert
	{
		get
		{
			return this.mInvert;
		}
		set
		{
			if (this.mInvert != value)
			{
				this.mInvert = value;
				this.mChanged = true;
			}
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06001D9B RID: 7579 RVA: 0x000E5D60 File Offset: 0x000E3F60
	public bool hasBorder
	{
		get
		{
			Vector4 border = this.border;
			return border.x != 0f || border.y != 0f || border.z != 0f || border.w != 0f;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06001D9C RID: 7580 RVA: 0x00002C2D File Offset: 0x00000E2D
	public virtual bool premultipliedAlpha
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06001D9D RID: 7581 RVA: 0x0001397E File Offset: 0x00011B7E
	public virtual float pixelSize
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06001D9E RID: 7582 RVA: 0x000E5DBC File Offset: 0x000E3FBC
	private Vector4 drawingUVs
	{
		get
		{
			switch (this.mFlip)
			{
			case UIBasicSprite.Flip.Horizontally:
				return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMin, this.mOuterUV.xMin, this.mOuterUV.yMax);
			case UIBasicSprite.Flip.Vertically:
				return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMax, this.mOuterUV.xMax, this.mOuterUV.yMin);
			case UIBasicSprite.Flip.Both:
				return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMax, this.mOuterUV.xMin, this.mOuterUV.yMin);
			default:
				return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMin, this.mOuterUV.xMax, this.mOuterUV.yMax);
			}
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06001D9F RID: 7583 RVA: 0x000E5EB0 File Offset: 0x000E40B0
	private Color32 drawingColor
	{
		get
		{
			Color color = base.color;
			color.a = this.finalAlpha;
			if (this.premultipliedAlpha)
			{
				color = NGUITools.ApplyPMA(color);
			}
			if (QualitySettings.activeColorSpace == 1)
			{
				color.r = Mathf.Pow(color.r, 2.2f);
				color.g = Mathf.Pow(color.g, 2.2f);
				color.b = Mathf.Pow(color.b, 2.2f);
			}
			return color;
		}
	}

	// Token: 0x06001DA0 RID: 7584 RVA: 0x000E5F3C File Offset: 0x000E413C
	protected void Fill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, Rect outer, Rect inner)
	{
		this.mOuterUV = outer;
		this.mInnerUV = inner;
		switch (this.type)
		{
		case UIBasicSprite.Type.Simple:
			this.SimpleFill(verts, uvs, cols);
			break;
		case UIBasicSprite.Type.Sliced:
			this.SlicedFill(verts, uvs, cols);
			break;
		case UIBasicSprite.Type.Tiled:
			this.TiledFill(verts, uvs, cols);
			break;
		case UIBasicSprite.Type.Filled:
			this.FilledFill(verts, uvs, cols);
			break;
		case UIBasicSprite.Type.Advanced:
			this.AdvancedFill(verts, uvs, cols);
			break;
		}
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x000E5FC8 File Offset: 0x000E41C8
	private void SimpleFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 drawingUVs = this.drawingUVs;
		Color32 drawingColor = this.drawingColor;
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
		uvs.Add(new Vector2(drawingUVs.x, drawingUVs.y));
		uvs.Add(new Vector2(drawingUVs.x, drawingUVs.w));
		uvs.Add(new Vector2(drawingUVs.z, drawingUVs.w));
		uvs.Add(new Vector2(drawingUVs.z, drawingUVs.y));
		cols.Add(drawingColor);
		cols.Add(drawingColor);
		cols.Add(drawingColor);
		cols.Add(drawingColor);
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x000E60D0 File Offset: 0x000E42D0
	private void SlicedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Vector4 vector = this.border * this.pixelSize;
		if (vector.x == 0f && vector.y == 0f && vector.z == 0f && vector.w == 0f)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Color32 drawingColor = this.drawingColor;
		Vector4 drawingDimensions = this.drawingDimensions;
		UIBasicSprite.mTempPos[0].x = drawingDimensions.x;
		UIBasicSprite.mTempPos[0].y = drawingDimensions.y;
		UIBasicSprite.mTempPos[3].x = drawingDimensions.z;
		UIBasicSprite.mTempPos[3].y = drawingDimensions.w;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.z;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.x;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.x;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.z;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.w;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.y;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.y;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.w;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMax;
		}
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.centerType != UIBasicSprite.AdvancedType.Invisible || i != 1 || j != 1)
				{
					int num2 = j + 1;
					verts.Add(new Vector3(UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[j].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num2].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[num2].y));
					verts.Add(new Vector3(UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[j].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num2].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[num2].y));
					uvs.Add(new Vector2(UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y));
					cols.Add(drawingColor);
					cols.Add(drawingColor);
					cols.Add(drawingColor);
					cols.Add(drawingColor);
				}
			}
		}
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x000E66A4 File Offset: 0x000E48A4
	private void TiledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector2 vector;
		vector..ctor(this.mInnerUV.width * (float)mainTexture.width, this.mInnerUV.height * (float)mainTexture.height);
		vector *= this.pixelSize;
		if (mainTexture == null || vector.x < 2f || vector.y < 2f)
		{
			return;
		}
		Color32 drawingColor = this.drawingColor;
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 vector2;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			vector2.x = this.mInnerUV.xMax;
			vector2.z = this.mInnerUV.xMin;
		}
		else
		{
			vector2.x = this.mInnerUV.xMin;
			vector2.z = this.mInnerUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			vector2.y = this.mInnerUV.yMax;
			vector2.w = this.mInnerUV.yMin;
		}
		else
		{
			vector2.y = this.mInnerUV.yMin;
			vector2.w = this.mInnerUV.yMax;
		}
		float num = drawingDimensions.x;
		float num2 = drawingDimensions.y;
		float x = vector2.x;
		float y = vector2.y;
		while (num2 < drawingDimensions.w)
		{
			num = drawingDimensions.x;
			float num3 = num2 + vector.y;
			float num4 = vector2.w;
			if (num3 > drawingDimensions.w)
			{
				num4 = Mathf.Lerp(vector2.y, vector2.w, (drawingDimensions.w - num2) / vector.y);
				num3 = drawingDimensions.w;
			}
			while (num < drawingDimensions.z)
			{
				float num5 = num + vector.x;
				float num6 = vector2.z;
				if (num5 > drawingDimensions.z)
				{
					num6 = Mathf.Lerp(vector2.x, vector2.z, (drawingDimensions.z - num) / vector.x);
					num5 = drawingDimensions.z;
				}
				verts.Add(new Vector3(num, num2));
				verts.Add(new Vector3(num, num3));
				verts.Add(new Vector3(num5, num3));
				verts.Add(new Vector3(num5, num2));
				uvs.Add(new Vector2(x, y));
				uvs.Add(new Vector2(x, num4));
				uvs.Add(new Vector2(num6, num4));
				uvs.Add(new Vector2(num6, y));
				cols.Add(drawingColor);
				cols.Add(drawingColor);
				cols.Add(drawingColor);
				cols.Add(drawingColor);
				num += vector.x;
			}
			num2 += vector.y;
		}
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x000E69AC File Offset: 0x000E4BAC
	private void FilledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (this.mFillAmount < 0.001f)
		{
			return;
		}
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector4 drawingUVs = this.drawingUVs;
		Color32 drawingColor = this.drawingColor;
		if (this.mFillDirection == UIBasicSprite.FillDirection.Horizontal || this.mFillDirection == UIBasicSprite.FillDirection.Vertical)
		{
			if (this.mFillDirection == UIBasicSprite.FillDirection.Horizontal)
			{
				float num = (drawingUVs.z - drawingUVs.x) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					drawingUVs.x = drawingUVs.z - num;
				}
				else
				{
					drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * this.mFillAmount;
					drawingUVs.z = drawingUVs.x + num;
				}
			}
			else if (this.mFillDirection == UIBasicSprite.FillDirection.Vertical)
			{
				float num2 = (drawingUVs.w - drawingUVs.y) * this.mFillAmount;
				if (this.mInvert)
				{
					drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					drawingUVs.y = drawingUVs.w - num2;
				}
				else
				{
					drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * this.mFillAmount;
					drawingUVs.w = drawingUVs.y + num2;
				}
			}
		}
		UIBasicSprite.mTempPos[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
		UIBasicSprite.mTempPos[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
		UIBasicSprite.mTempPos[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
		UIBasicSprite.mTempPos[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
		UIBasicSprite.mTempUVs[0] = new Vector2(drawingUVs.x, drawingUVs.y);
		UIBasicSprite.mTempUVs[1] = new Vector2(drawingUVs.x, drawingUVs.w);
		UIBasicSprite.mTempUVs[2] = new Vector2(drawingUVs.z, drawingUVs.w);
		UIBasicSprite.mTempUVs[3] = new Vector2(drawingUVs.z, drawingUVs.y);
		if (this.mFillAmount < 1f)
		{
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial90)
			{
				if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, this.mFillAmount, this.mInvert, 0))
				{
					for (int i = 0; i < 4; i++)
					{
						verts.Add(UIBasicSprite.mTempPos[i]);
						uvs.Add(UIBasicSprite.mTempUVs[i]);
						cols.Add(drawingColor);
					}
				}
				return;
			}
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial180)
			{
				for (int j = 0; j < 2; j++)
				{
					float num3 = 0f;
					float num4 = 1f;
					float num5;
					float num6;
					if (j == 0)
					{
						num5 = 0f;
						num6 = 0.5f;
					}
					else
					{
						num5 = 0.5f;
						num6 = 1f;
					}
					UIBasicSprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num5);
					UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x;
					UIBasicSprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num6);
					UIBasicSprite.mTempPos[3].x = UIBasicSprite.mTempPos[2].x;
					UIBasicSprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num3);
					UIBasicSprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num4);
					UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[1].y;
					UIBasicSprite.mTempPos[3].y = UIBasicSprite.mTempPos[0].y;
					UIBasicSprite.mTempUVs[0].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, num5);
					UIBasicSprite.mTempUVs[1].x = UIBasicSprite.mTempUVs[0].x;
					UIBasicSprite.mTempUVs[2].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, num6);
					UIBasicSprite.mTempUVs[3].x = UIBasicSprite.mTempUVs[2].x;
					UIBasicSprite.mTempUVs[0].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, num3);
					UIBasicSprite.mTempUVs[1].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, num4);
					UIBasicSprite.mTempUVs[2].y = UIBasicSprite.mTempUVs[1].y;
					UIBasicSprite.mTempUVs[3].y = UIBasicSprite.mTempUVs[0].y;
					float num7 = this.mInvert ? (this.mFillAmount * 2f - (float)(1 - j)) : (this.fillAmount * 2f - (float)j);
					if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, Mathf.Clamp01(num7), !this.mInvert, NGUIMath.RepeatIndex(j + 3, 4)))
					{
						for (int k = 0; k < 4; k++)
						{
							verts.Add(UIBasicSprite.mTempPos[k]);
							uvs.Add(UIBasicSprite.mTempUVs[k]);
							cols.Add(drawingColor);
						}
					}
				}
				return;
			}
			if (this.mFillDirection == UIBasicSprite.FillDirection.Radial360)
			{
				for (int l = 0; l < 4; l++)
				{
					float num8;
					float num9;
					if (l < 2)
					{
						num8 = 0f;
						num9 = 0.5f;
					}
					else
					{
						num8 = 0.5f;
						num9 = 1f;
					}
					float num10;
					float num11;
					if (l == 0 || l == 3)
					{
						num10 = 0f;
						num11 = 0.5f;
					}
					else
					{
						num10 = 0.5f;
						num11 = 1f;
					}
					UIBasicSprite.mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num8);
					UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x;
					UIBasicSprite.mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num9);
					UIBasicSprite.mTempPos[3].x = UIBasicSprite.mTempPos[2].x;
					UIBasicSprite.mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num10);
					UIBasicSprite.mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num11);
					UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[1].y;
					UIBasicSprite.mTempPos[3].y = UIBasicSprite.mTempPos[0].y;
					UIBasicSprite.mTempUVs[0].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, num8);
					UIBasicSprite.mTempUVs[1].x = UIBasicSprite.mTempUVs[0].x;
					UIBasicSprite.mTempUVs[2].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, num9);
					UIBasicSprite.mTempUVs[3].x = UIBasicSprite.mTempUVs[2].x;
					UIBasicSprite.mTempUVs[0].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, num10);
					UIBasicSprite.mTempUVs[1].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, num11);
					UIBasicSprite.mTempUVs[2].y = UIBasicSprite.mTempUVs[1].y;
					UIBasicSprite.mTempUVs[3].y = UIBasicSprite.mTempUVs[0].y;
					float num12 = (!this.mInvert) ? (this.mFillAmount * 4f - (float)(3 - NGUIMath.RepeatIndex(l + 2, 4))) : (this.mFillAmount * 4f - (float)NGUIMath.RepeatIndex(l + 2, 4));
					if (UIBasicSprite.RadialCut(UIBasicSprite.mTempPos, UIBasicSprite.mTempUVs, Mathf.Clamp01(num12), this.mInvert, NGUIMath.RepeatIndex(l + 2, 4)))
					{
						for (int m = 0; m < 4; m++)
						{
							verts.Add(UIBasicSprite.mTempPos[m]);
							uvs.Add(UIBasicSprite.mTempUVs[m]);
							cols.Add(drawingColor);
						}
					}
				}
				return;
			}
		}
		for (int n = 0; n < 4; n++)
		{
			verts.Add(UIBasicSprite.mTempPos[n]);
			uvs.Add(UIBasicSprite.mTempUVs[n]);
			cols.Add(drawingColor);
		}
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x000E73C4 File Offset: 0x000E55C4
	private void AdvancedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Vector4 vector = this.border * this.pixelSize;
		if (vector.x == 0f && vector.y == 0f && vector.z == 0f && vector.w == 0f)
		{
			this.SimpleFill(verts, uvs, cols);
			return;
		}
		Color32 drawingColor = this.drawingColor;
		Vector4 drawingDimensions = this.drawingDimensions;
		Vector2 vector2;
		vector2..ctor(this.mInnerUV.width * (float)mainTexture.width, this.mInnerUV.height * (float)mainTexture.height);
		vector2 *= this.pixelSize;
		if (vector2.x < 1f)
		{
			vector2.x = 1f;
		}
		if (vector2.y < 1f)
		{
			vector2.y = 1f;
		}
		UIBasicSprite.mTempPos[0].x = drawingDimensions.x;
		UIBasicSprite.mTempPos[0].y = drawingDimensions.y;
		UIBasicSprite.mTempPos[3].x = drawingDimensions.z;
		UIBasicSprite.mTempPos[3].y = drawingDimensions.w;
		if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.z;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.x;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].x = UIBasicSprite.mTempPos[0].x + vector.x;
			UIBasicSprite.mTempPos[2].x = UIBasicSprite.mTempPos[3].x - vector.z;
			UIBasicSprite.mTempUVs[0].x = this.mOuterUV.xMin;
			UIBasicSprite.mTempUVs[1].x = this.mInnerUV.xMin;
			UIBasicSprite.mTempUVs[2].x = this.mInnerUV.xMax;
			UIBasicSprite.mTempUVs[3].x = this.mOuterUV.xMax;
		}
		if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.w;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.y;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMax;
		}
		else
		{
			UIBasicSprite.mTempPos[1].y = UIBasicSprite.mTempPos[0].y + vector.y;
			UIBasicSprite.mTempPos[2].y = UIBasicSprite.mTempPos[3].y - vector.w;
			UIBasicSprite.mTempUVs[0].y = this.mOuterUV.yMin;
			UIBasicSprite.mTempUVs[1].y = this.mInnerUV.yMin;
			UIBasicSprite.mTempUVs[2].y = this.mInnerUV.yMax;
			UIBasicSprite.mTempUVs[3].y = this.mOuterUV.yMax;
		}
		for (int i = 0; i < 3; i++)
		{
			int num = i + 1;
			for (int j = 0; j < 3; j++)
			{
				if (this.centerType != UIBasicSprite.AdvancedType.Invisible || i != 1 || j != 1)
				{
					int num2 = j + 1;
					if (i == 1 && j == 1)
					{
						if (this.centerType == UIBasicSprite.AdvancedType.Tiled)
						{
							float x = UIBasicSprite.mTempPos[i].x;
							float x2 = UIBasicSprite.mTempPos[num].x;
							float y = UIBasicSprite.mTempPos[j].y;
							float y2 = UIBasicSprite.mTempPos[num2].y;
							float x3 = UIBasicSprite.mTempUVs[i].x;
							float y3 = UIBasicSprite.mTempUVs[j].y;
							for (float num3 = y; num3 < y2; num3 += vector2.y)
							{
								float num4 = x;
								float num5 = UIBasicSprite.mTempUVs[num2].y;
								float num6 = num3 + vector2.y;
								if (num6 > y2)
								{
									num5 = Mathf.Lerp(y3, num5, (y2 - num3) / vector2.y);
									num6 = y2;
								}
								while (num4 < x2)
								{
									float num7 = num4 + vector2.x;
									float num8 = UIBasicSprite.mTempUVs[num].x;
									if (num7 > x2)
									{
										num8 = Mathf.Lerp(x3, num8, (x2 - num4) / vector2.x);
										num7 = x2;
									}
									UIBasicSprite.Fill(verts, uvs, cols, num4, num7, num3, num6, x3, num8, y3, num5, drawingColor);
									num4 += vector2.x;
								}
							}
						}
						else if (this.centerType == UIBasicSprite.AdvancedType.Sliced)
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
						}
					}
					else if (i == 1)
					{
						if ((j == 0 && this.bottomType == UIBasicSprite.AdvancedType.Tiled) || (j == 2 && this.topType == UIBasicSprite.AdvancedType.Tiled))
						{
							float x4 = UIBasicSprite.mTempPos[i].x;
							float x5 = UIBasicSprite.mTempPos[num].x;
							float y4 = UIBasicSprite.mTempPos[j].y;
							float y5 = UIBasicSprite.mTempPos[num2].y;
							float x6 = UIBasicSprite.mTempUVs[i].x;
							float y6 = UIBasicSprite.mTempUVs[j].y;
							float y7 = UIBasicSprite.mTempUVs[num2].y;
							for (float num9 = x4; num9 < x5; num9 += vector2.x)
							{
								float num10 = num9 + vector2.x;
								float num11 = UIBasicSprite.mTempUVs[num].x;
								if (num10 > x5)
								{
									num11 = Mathf.Lerp(x6, num11, (x5 - num9) / vector2.x);
									num10 = x5;
								}
								UIBasicSprite.Fill(verts, uvs, cols, num9, num10, y4, y5, x6, num11, y6, y7, drawingColor);
							}
						}
						else if ((j == 0 && this.bottomType == UIBasicSprite.AdvancedType.Sliced) || (j == 2 && this.topType == UIBasicSprite.AdvancedType.Sliced))
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
						}
					}
					else if (j == 1)
					{
						if ((i == 0 && this.leftType == UIBasicSprite.AdvancedType.Tiled) || (i == 2 && this.rightType == UIBasicSprite.AdvancedType.Tiled))
						{
							float x7 = UIBasicSprite.mTempPos[i].x;
							float x8 = UIBasicSprite.mTempPos[num].x;
							float y8 = UIBasicSprite.mTempPos[j].y;
							float y9 = UIBasicSprite.mTempPos[num2].y;
							float x9 = UIBasicSprite.mTempUVs[i].x;
							float x10 = UIBasicSprite.mTempUVs[num].x;
							float y10 = UIBasicSprite.mTempUVs[j].y;
							for (float num12 = y8; num12 < y9; num12 += vector2.y)
							{
								float num13 = UIBasicSprite.mTempUVs[num2].y;
								float num14 = num12 + vector2.y;
								if (num14 > y9)
								{
									num13 = Mathf.Lerp(y10, num13, (y9 - num12) / vector2.y);
									num14 = y9;
								}
								UIBasicSprite.Fill(verts, uvs, cols, x7, x8, num12, num14, x9, x10, y10, num13, drawingColor);
							}
						}
						else if ((i == 0 && this.leftType == UIBasicSprite.AdvancedType.Sliced) || (i == 2 && this.rightType == UIBasicSprite.AdvancedType.Sliced))
						{
							UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
						}
					}
					else
					{
						UIBasicSprite.Fill(verts, uvs, cols, UIBasicSprite.mTempPos[i].x, UIBasicSprite.mTempPos[num].x, UIBasicSprite.mTempPos[j].y, UIBasicSprite.mTempPos[num2].y, UIBasicSprite.mTempUVs[i].x, UIBasicSprite.mTempUVs[num].x, UIBasicSprite.mTempUVs[j].y, UIBasicSprite.mTempUVs[num2].y, drawingColor);
					}
				}
			}
		}
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x000E7F50 File Offset: 0x000E6150
	private static bool RadialCut(Vector2[] xy, Vector2[] uv, float fill, bool invert, int corner)
	{
		if (fill < 0.001f)
		{
			return false;
		}
		if ((corner & 1) == 1)
		{
			invert = !invert;
		}
		if (!invert && fill > 0.999f)
		{
			return true;
		}
		float num = Mathf.Clamp01(fill);
		if (invert)
		{
			num = 1f - num;
		}
		num *= 1.5707964f;
		float cos = Mathf.Cos(num);
		float sin = Mathf.Sin(num);
		UIBasicSprite.RadialCut(xy, cos, sin, invert, corner);
		UIBasicSprite.RadialCut(uv, cos, sin, invert, corner);
		return true;
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x000E7FD0 File Offset: 0x000E61D0
	private static void RadialCut(Vector2[] xy, float cos, float sin, bool invert, int corner)
	{
		int num = NGUIMath.RepeatIndex(corner + 1, 4);
		int num2 = NGUIMath.RepeatIndex(corner + 2, 4);
		int num3 = NGUIMath.RepeatIndex(corner + 3, 4);
		if ((corner & 1) == 1)
		{
			if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num2].x = xy[num].x;
				}
			}
			else if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num2].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num3].y = xy[num2].y;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (!invert)
			{
				xy[num3].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
			else
			{
				xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
		}
		else
		{
			if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num2].y = xy[num].y;
				}
			}
			else if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num2].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num3].x = xy[num2].x;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (invert)
			{
				xy[num3].y = Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
			else
			{
				xy[num].x = Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
		}
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x000E826C File Offset: 0x000E646C
	private static void Fill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, float v0x, float v1x, float v0y, float v1y, float u0x, float u1x, float u0y, float u1y, Color col)
	{
		verts.Add(new Vector3(v0x, v0y));
		verts.Add(new Vector3(v0x, v1y));
		verts.Add(new Vector3(v1x, v1y));
		verts.Add(new Vector3(v1x, v0y));
		uvs.Add(new Vector2(u0x, u0y));
		uvs.Add(new Vector2(u0x, u1y));
		uvs.Add(new Vector2(u1x, u1y));
		uvs.Add(new Vector2(u1x, u0y));
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
	}

	// Token: 0x040021B1 RID: 8625
	[SerializeField]
	[HideInInspector]
	protected UIBasicSprite.Type mType;

	// Token: 0x040021B2 RID: 8626
	[SerializeField]
	[HideInInspector]
	protected UIBasicSprite.FillDirection mFillDirection = UIBasicSprite.FillDirection.Radial360;

	// Token: 0x040021B3 RID: 8627
	[HideInInspector]
	[Range(0f, 1f)]
	[SerializeField]
	protected float mFillAmount = 1f;

	// Token: 0x040021B4 RID: 8628
	[HideInInspector]
	[SerializeField]
	protected bool mInvert;

	// Token: 0x040021B5 RID: 8629
	[SerializeField]
	[HideInInspector]
	protected UIBasicSprite.Flip mFlip;

	// Token: 0x040021B6 RID: 8630
	[NonSerialized]
	private Rect mInnerUV = default(Rect);

	// Token: 0x040021B7 RID: 8631
	[NonSerialized]
	private Rect mOuterUV = default(Rect);

	// Token: 0x040021B8 RID: 8632
	public UIBasicSprite.AdvancedType centerType = UIBasicSprite.AdvancedType.Sliced;

	// Token: 0x040021B9 RID: 8633
	public UIBasicSprite.AdvancedType leftType = UIBasicSprite.AdvancedType.Sliced;

	// Token: 0x040021BA RID: 8634
	public UIBasicSprite.AdvancedType rightType = UIBasicSprite.AdvancedType.Sliced;

	// Token: 0x040021BB RID: 8635
	public UIBasicSprite.AdvancedType bottomType = UIBasicSprite.AdvancedType.Sliced;

	// Token: 0x040021BC RID: 8636
	public UIBasicSprite.AdvancedType topType = UIBasicSprite.AdvancedType.Sliced;

	// Token: 0x040021BD RID: 8637
	protected static Vector2[] mTempPos = new Vector2[4];

	// Token: 0x040021BE RID: 8638
	protected static Vector2[] mTempUVs = new Vector2[4];

	// Token: 0x020004AD RID: 1197
	public enum Type
	{
		// Token: 0x040021C0 RID: 8640
		Simple,
		// Token: 0x040021C1 RID: 8641
		Sliced,
		// Token: 0x040021C2 RID: 8642
		Tiled,
		// Token: 0x040021C3 RID: 8643
		Filled,
		// Token: 0x040021C4 RID: 8644
		Advanced
	}

	// Token: 0x020004AE RID: 1198
	public enum FillDirection
	{
		// Token: 0x040021C6 RID: 8646
		Horizontal,
		// Token: 0x040021C7 RID: 8647
		Vertical,
		// Token: 0x040021C8 RID: 8648
		Radial90,
		// Token: 0x040021C9 RID: 8649
		Radial180,
		// Token: 0x040021CA RID: 8650
		Radial360
	}

	// Token: 0x020004AF RID: 1199
	public enum AdvancedType
	{
		// Token: 0x040021CC RID: 8652
		Invisible,
		// Token: 0x040021CD RID: 8653
		Sliced,
		// Token: 0x040021CE RID: 8654
		Tiled
	}

	// Token: 0x020004B0 RID: 1200
	public enum Flip
	{
		// Token: 0x040021D0 RID: 8656
		Nothing,
		// Token: 0x040021D1 RID: 8657
		Horizontally,
		// Token: 0x040021D2 RID: 8658
		Vertically,
		// Token: 0x040021D3 RID: 8659
		Both
	}
}
