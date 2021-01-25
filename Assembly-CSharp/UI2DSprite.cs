using System;
using UnityEngine;

// Token: 0x020004D9 RID: 1241
[AddComponentMenu("NGUI/UI/NGUI Unity2D Sprite")]
[ExecuteInEditMode]
public class UI2DSprite : UIBasicSprite
{
	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06001F32 RID: 7986 RVA: 0x00014CA3 File Offset: 0x00012EA3
	// (set) Token: 0x06001F33 RID: 7987 RVA: 0x00014CAB File Offset: 0x00012EAB
	public Sprite sprite2D
	{
		get
		{
			return this.mSprite;
		}
		set
		{
			if (this.mSprite != value)
			{
				base.RemoveFromPanel();
				this.mSprite = value;
				this.nextSprite = null;
				base.CreatePanel();
			}
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06001F34 RID: 7988 RVA: 0x00014CD9 File Offset: 0x00012ED9
	// (set) Token: 0x06001F35 RID: 7989 RVA: 0x00014CE1 File Offset: 0x00012EE1
	public override Material material
	{
		get
		{
			return this.mMat;
		}
		set
		{
			if (this.mMat != value)
			{
				base.RemoveFromPanel();
				this.mMat = value;
				this.mPMA = -1;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06001F36 RID: 7990 RVA: 0x000ED908 File Offset: 0x000EBB08
	// (set) Token: 0x06001F37 RID: 7991 RVA: 0x00014D0E File Offset: 0x00012F0E
	public override Shader shader
	{
		get
		{
			if (this.mMat != null)
			{
				return this.mMat.shader;
			}
			if (this.mShader == null)
			{
				this.mShader = Shader.Find("Unlit/Transparent Colored");
			}
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				base.RemoveFromPanel();
				this.mShader = value;
				if (this.mMat == null)
				{
					this.mPMA = -1;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06001F38 RID: 7992 RVA: 0x00014D4C File Offset: 0x00012F4C
	public override Texture mainTexture
	{
		get
		{
			if (this.mSprite != null)
			{
				return this.mSprite.texture;
			}
			if (this.mMat != null)
			{
				return this.mMat.mainTexture;
			}
			return null;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06001F39 RID: 7993 RVA: 0x000ED95C File Offset: 0x000EBB5C
	public override bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Shader shader = this.shader;
				this.mPMA = ((!(shader != null) || !shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06001F3A RID: 7994 RVA: 0x00014D89 File Offset: 0x00012F89
	public override float pixelSize
	{
		get
		{
			return this.mPixelSize;
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06001F3B RID: 7995 RVA: 0x000ED9B4 File Offset: 0x000EBBB4
	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = base.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			if (this.mSprite != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int num5 = Mathf.RoundToInt(this.mSprite.rect.width);
				int num6 = Mathf.RoundToInt(this.mSprite.rect.height);
				int num7 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
				int num8 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
				int num9 = Mathf.RoundToInt(this.mSprite.rect.width - this.mSprite.textureRect.width - this.mSprite.textureRectOffset.x);
				int num10 = Mathf.RoundToInt(this.mSprite.rect.height - this.mSprite.textureRect.height - this.mSprite.textureRectOffset.y);
				float num11 = 1f;
				float num12 = 1f;
				if (num5 > 0 && num6 > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((num5 & 1) != 0)
					{
						num9++;
					}
					if ((num6 & 1) != 0)
					{
						num10++;
					}
					num11 = 1f / (float)num5 * (float)this.mWidth;
					num12 = 1f / (float)num6 * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num += (float)num9 * num11;
					num3 -= (float)num7 * num11;
				}
				else
				{
					num += (float)num7 * num11;
					num3 -= (float)num9 * num11;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num2 += (float)num10 * num12;
					num4 -= (float)num8 * num12;
				}
				else
				{
					num2 += (float)num8 * num12;
					num4 -= (float)num10 * num12;
				}
			}
			float num13;
			float num14;
			if (this.mFixedAspect)
			{
				num13 = 0f;
				num14 = 0f;
			}
			else
			{
				Vector4 vector = this.border * this.pixelSize;
				num13 = vector.x + vector.z;
				num14 = vector.y + vector.w;
			}
			float num15 = Mathf.Lerp(num, num3 - num13, this.mDrawRegion.x);
			float num16 = Mathf.Lerp(num2, num4 - num14, this.mDrawRegion.y);
			float num17 = Mathf.Lerp(num + num13, num3, this.mDrawRegion.z);
			float num18 = Mathf.Lerp(num2 + num14, num4, this.mDrawRegion.w);
			return new Vector4(num15, num16, num17, num18);
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06001F3C RID: 7996 RVA: 0x00014D91 File Offset: 0x00012F91
	// (set) Token: 0x06001F3D RID: 7997 RVA: 0x00014D99 File Offset: 0x00012F99
	public override Vector4 border
	{
		get
		{
			return this.mBorder;
		}
		set
		{
			if (this.mBorder != value)
			{
				this.mBorder = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x06001F3E RID: 7998 RVA: 0x000EDCD0 File Offset: 0x000EBED0
	protected override void OnUpdate()
	{
		if (this.nextSprite != null)
		{
			if (this.nextSprite != this.mSprite)
			{
				this.sprite2D = this.nextSprite;
			}
			this.nextSprite = null;
		}
		base.OnUpdate();
		if (this.mFixedAspect)
		{
			Texture mainTexture = this.mainTexture;
			if (mainTexture != null)
			{
				int num = Mathf.RoundToInt(this.mSprite.rect.width);
				int num2 = Mathf.RoundToInt(this.mSprite.rect.height);
				int num3 = Mathf.RoundToInt(this.mSprite.textureRectOffset.x);
				int num4 = Mathf.RoundToInt(this.mSprite.textureRectOffset.y);
				int num5 = Mathf.RoundToInt(this.mSprite.rect.width - this.mSprite.textureRect.width - this.mSprite.textureRectOffset.x);
				int num6 = Mathf.RoundToInt(this.mSprite.rect.height - this.mSprite.textureRect.height - this.mSprite.textureRectOffset.y);
				num += num3 + num5;
				num2 += num6 + num4;
				float num7 = (float)this.mWidth;
				float num8 = (float)this.mHeight;
				float num9 = num7 / num8;
				float num10 = (float)num / (float)num2;
				if (num10 < num9)
				{
					float num11 = (num7 - num8 * num10) / num7 * 0.5f;
					base.drawRegion = new Vector4(num11, 0f, 1f - num11, 1f);
				}
				else
				{
					float num12 = (num8 - num7 / num10) / num8 * 0.5f;
					base.drawRegion = new Vector4(0f, num12, 1f, 1f - num12);
				}
			}
		}
	}

	// Token: 0x06001F3F RID: 7999 RVA: 0x000EDED0 File Offset: 0x000EC0D0
	public override void MakePixelPerfect()
	{
		base.MakePixelPerfect();
		if (this.mType == UIBasicSprite.Type.Tiled)
		{
			return;
		}
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		if ((this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled || !base.hasBorder) && mainTexture != null)
		{
			Rect rect = this.mSprite.rect;
			int num = Mathf.RoundToInt(rect.width);
			int num2 = Mathf.RoundToInt(rect.height);
			if ((num & 1) == 1)
			{
				num++;
			}
			if ((num2 & 1) == 1)
			{
				num2++;
			}
			base.width = num;
			base.height = num2;
		}
	}

	// Token: 0x06001F40 RID: 8000 RVA: 0x000EDF80 File Offset: 0x000EC180
	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Rect rect = (!(this.mSprite != null)) ? new Rect(0f, 0f, (float)mainTexture.width, (float)mainTexture.height) : this.mSprite.textureRect;
		Rect inner = rect;
		Vector4 border = this.border;
		inner.xMin += border.x;
		inner.yMin += border.y;
		inner.xMax -= border.z;
		inner.yMax -= border.w;
		float num = 1f / (float)mainTexture.width;
		float num2 = 1f / (float)mainTexture.height;
		rect.xMin *= num;
		rect.xMax *= num;
		rect.yMin *= num2;
		rect.yMax *= num2;
		inner.xMin *= num;
		inner.xMax *= num;
		inner.yMin *= num2;
		inner.yMax *= num2;
		int size = verts.size;
		base.Fill(verts, uvs, cols, rect, inner);
		if (this.onPostFill != null)
		{
			this.onPostFill(this, size, verts, uvs, cols);
		}
	}

	// Token: 0x040022C8 RID: 8904
	[SerializeField]
	[HideInInspector]
	private Sprite mSprite;

	// Token: 0x040022C9 RID: 8905
	[HideInInspector]
	[SerializeField]
	private Material mMat;

	// Token: 0x040022CA RID: 8906
	[SerializeField]
	[HideInInspector]
	private Shader mShader;

	// Token: 0x040022CB RID: 8907
	[HideInInspector]
	[SerializeField]
	private Vector4 mBorder = Vector4.zero;

	// Token: 0x040022CC RID: 8908
	[HideInInspector]
	[SerializeField]
	private bool mFixedAspect;

	// Token: 0x040022CD RID: 8909
	[HideInInspector]
	[SerializeField]
	private float mPixelSize = 1f;

	// Token: 0x040022CE RID: 8910
	public Sprite nextSprite;

	// Token: 0x040022CF RID: 8911
	[NonSerialized]
	private int mPMA = -1;
}
