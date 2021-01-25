using System;
using UnityEngine;

// Token: 0x0200050F RID: 1295
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Texture")]
public class UITexture : UIBasicSprite
{
	// Token: 0x17000356 RID: 854
	// (get) Token: 0x06002147 RID: 8519 RVA: 0x00016551 File Offset: 0x00014751
	// (set) Token: 0x06002148 RID: 8520 RVA: 0x000FADA8 File Offset: 0x000F8FA8
	public override Texture mainTexture
	{
		get
		{
			if (this.mTexture != null)
			{
				return this.mTexture;
			}
			if (this.mMat != null)
			{
				return this.mMat.mainTexture;
			}
			return null;
		}
		set
		{
			if (this.mTexture != value)
			{
				if (this.drawCall != null && this.drawCall.widgetCount == 1 && this.mMat == null)
				{
					this.mTexture = value;
					this.drawCall.mainTexture = value;
				}
				else
				{
					base.RemoveFromPanel();
					this.mTexture = value;
					this.mPMA = -1;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x06002149 RID: 8521 RVA: 0x00016589 File Offset: 0x00014789
	// (set) Token: 0x0600214A RID: 8522 RVA: 0x00016591 File Offset: 0x00014791
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
				this.mShader = null;
				this.mMat = value;
				this.mPMA = -1;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x0600214B RID: 8523 RVA: 0x000FAE2C File Offset: 0x000F902C
	// (set) Token: 0x0600214C RID: 8524 RVA: 0x000FAE80 File Offset: 0x000F9080
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
				if (this.drawCall != null && this.drawCall.widgetCount == 1 && this.mMat == null)
				{
					this.mShader = value;
					this.drawCall.shader = value;
				}
				else
				{
					base.RemoveFromPanel();
					this.mShader = value;
					this.mPMA = -1;
					this.mMat = null;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x0600214D RID: 8525 RVA: 0x000FAF0C File Offset: 0x000F910C
	public override bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x0600214E RID: 8526 RVA: 0x000165C5 File Offset: 0x000147C5
	// (set) Token: 0x0600214F RID: 8527 RVA: 0x000165CD File Offset: 0x000147CD
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

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x06002150 RID: 8528 RVA: 0x000165ED File Offset: 0x000147ED
	// (set) Token: 0x06002151 RID: 8529 RVA: 0x000165F5 File Offset: 0x000147F5
	public Rect uvRect
	{
		get
		{
			return this.mRect;
		}
		set
		{
			if (this.mRect != value)
			{
				this.mRect = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x06002152 RID: 8530 RVA: 0x000FAF7C File Offset: 0x000F917C
	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = base.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			if (this.mTexture != null && this.mType != UIBasicSprite.Type.Tiled)
			{
				int width = this.mTexture.width;
				int height = this.mTexture.height;
				int num5 = 0;
				int num6 = 0;
				float num7 = 1f;
				float num8 = 1f;
				if (width > 0 && height > 0 && (this.mType == UIBasicSprite.Type.Simple || this.mType == UIBasicSprite.Type.Filled))
				{
					if ((width & 1) != 0)
					{
						num5++;
					}
					if ((height & 1) != 0)
					{
						num6++;
					}
					num7 = 1f / (float)width * (float)this.mWidth;
					num8 = 1f / (float)height * (float)this.mHeight;
				}
				if (this.mFlip == UIBasicSprite.Flip.Horizontally || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num += (float)num5 * num7;
				}
				else
				{
					num3 -= (float)num5 * num7;
				}
				if (this.mFlip == UIBasicSprite.Flip.Vertically || this.mFlip == UIBasicSprite.Flip.Both)
				{
					num2 += (float)num6 * num8;
				}
				else
				{
					num4 -= (float)num6 * num8;
				}
			}
			float num9;
			float num10;
			if (this.mFixedAspect)
			{
				num9 = 0f;
				num10 = 0f;
			}
			else
			{
				Vector4 border = this.border;
				num9 = border.x + border.z;
				num10 = border.y + border.w;
			}
			float num11 = Mathf.Lerp(num, num3 - num9, this.mDrawRegion.x);
			float num12 = Mathf.Lerp(num2, num4 - num10, this.mDrawRegion.y);
			float num13 = Mathf.Lerp(num + num9, num3, this.mDrawRegion.z);
			float num14 = Mathf.Lerp(num2 + num10, num4, this.mDrawRegion.w);
			return new Vector4(num11, num12, num13, num14);
		}
	}

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x06002153 RID: 8531 RVA: 0x00016615 File Offset: 0x00014815
	// (set) Token: 0x06002154 RID: 8532 RVA: 0x0001661D File Offset: 0x0001481D
	public bool fixedAspect
	{
		get
		{
			return this.mFixedAspect;
		}
		set
		{
			if (this.mFixedAspect != value)
			{
				this.mFixedAspect = value;
				this.mDrawRegion = new Vector4(0f, 0f, 1f, 1f);
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x000FB190 File Offset: 0x000F9390
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
			int num = mainTexture.width;
			int num2 = mainTexture.height;
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

	// Token: 0x06002156 RID: 8534 RVA: 0x000FB228 File Offset: 0x000F9428
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.mFixedAspect)
		{
			Texture mainTexture = this.mainTexture;
			if (mainTexture != null)
			{
				int num = mainTexture.width;
				int num2 = mainTexture.height;
				if ((num & 1) == 1)
				{
					num++;
				}
				if ((num2 & 1) == 1)
				{
					num2++;
				}
				float num3 = (float)this.mWidth;
				float num4 = (float)this.mHeight;
				float num5 = num3 / num4;
				float num6 = (float)num / (float)num2;
				if (num6 < num5)
				{
					float num7 = (num3 - num4 * num6) / num3 * 0.5f;
					base.drawRegion = new Vector4(num7, 0f, 1f - num7, 1f);
				}
				else
				{
					float num8 = (num4 - num3 / num6) / num4 * 0.5f;
					base.drawRegion = new Vector4(0f, num8, 1f, 1f - num8);
				}
			}
		}
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000FB310 File Offset: 0x000F9510
	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture == null)
		{
			return;
		}
		Rect rect;
		rect..ctor(this.mRect.x * (float)mainTexture.width, this.mRect.y * (float)mainTexture.height, (float)mainTexture.width * this.mRect.width, (float)mainTexture.height * this.mRect.height);
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

	// Token: 0x0400247B RID: 9339
	[SerializeField]
	[HideInInspector]
	private Rect mRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x0400247C RID: 9340
	[SerializeField]
	[HideInInspector]
	private Texture mTexture;

	// Token: 0x0400247D RID: 9341
	[SerializeField]
	[HideInInspector]
	private Material mMat;

	// Token: 0x0400247E RID: 9342
	[HideInInspector]
	[SerializeField]
	private Shader mShader;

	// Token: 0x0400247F RID: 9343
	[HideInInspector]
	[SerializeField]
	private Vector4 mBorder = Vector4.zero;

	// Token: 0x04002480 RID: 9344
	[SerializeField]
	[HideInInspector]
	private bool mFixedAspect;

	// Token: 0x04002481 RID: 9345
	[NonSerialized]
	private int mPMA = -1;
}
