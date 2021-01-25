using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020004C0 RID: 1216
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Widget")]
public class UIWidget : UIRect
{
	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06001E38 RID: 7736 RVA: 0x000140C9 File Offset: 0x000122C9
	// (set) Token: 0x06001E39 RID: 7737 RVA: 0x000EA5B0 File Offset: 0x000E87B0
	public UIDrawCall.OnRenderCallback onRender
	{
		get
		{
			return this.mOnRender;
		}
		set
		{
			if (this.mOnRender != value)
			{
				if (this.drawCall != null && this.drawCall.onRender != null && this.mOnRender != null)
				{
					UIDrawCall uidrawCall = this.drawCall;
					uidrawCall.onRender = (UIDrawCall.OnRenderCallback)Delegate.Remove(uidrawCall.onRender, this.mOnRender);
				}
				this.mOnRender = value;
				if (this.drawCall != null)
				{
					UIDrawCall uidrawCall2 = this.drawCall;
					uidrawCall2.onRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(uidrawCall2.onRender, value);
				}
			}
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06001E3A RID: 7738 RVA: 0x000140D1 File Offset: 0x000122D1
	// (set) Token: 0x06001E3B RID: 7739 RVA: 0x000140D9 File Offset: 0x000122D9
	public Vector4 drawRegion
	{
		get
		{
			return this.mDrawRegion;
		}
		set
		{
			if (this.mDrawRegion != value)
			{
				this.mDrawRegion = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06001E3C RID: 7740 RVA: 0x0001410A File Offset: 0x0001230A
	public Vector2 pivotOffset
	{
		get
		{
			return NGUIMath.GetPivotOffset(this.pivot);
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06001E3D RID: 7741 RVA: 0x00014117 File Offset: 0x00012317
	// (set) Token: 0x06001E3E RID: 7742 RVA: 0x000EA650 File Offset: 0x000E8850
	public int width
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			int minWidth = this.minWidth;
			if (value < minWidth)
			{
				value = minWidth;
			}
			if (this.mWidth != value && this.keepAspectRatio != UIWidget.AspectRatioSource.BasedOnHeight)
			{
				if (this.isAnchoredHorizontally)
				{
					if (this.leftAnchor.target != null && this.rightAnchor.target != null)
					{
						if (this.mPivot == UIWidget.Pivot.BottomLeft || this.mPivot == UIWidget.Pivot.Left || this.mPivot == UIWidget.Pivot.TopLeft)
						{
							NGUIMath.AdjustWidget(this, 0f, 0f, (float)(value - this.mWidth), 0f);
						}
						else if (this.mPivot == UIWidget.Pivot.BottomRight || this.mPivot == UIWidget.Pivot.Right || this.mPivot == UIWidget.Pivot.TopRight)
						{
							NGUIMath.AdjustWidget(this, (float)(this.mWidth - value), 0f, 0f, 0f);
						}
						else
						{
							int num = value - this.mWidth;
							num -= (num & 1);
							if (num != 0)
							{
								NGUIMath.AdjustWidget(this, (float)(-(float)num) * 0.5f, 0f, (float)num * 0.5f, 0f);
							}
						}
					}
					else if (this.leftAnchor.target != null)
					{
						NGUIMath.AdjustWidget(this, 0f, 0f, (float)(value - this.mWidth), 0f);
					}
					else
					{
						NGUIMath.AdjustWidget(this, (float)(this.mWidth - value), 0f, 0f, 0f);
					}
				}
				else
				{
					this.SetDimensions(value, this.mHeight);
				}
			}
		}
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06001E3F RID: 7743 RVA: 0x0001411F File Offset: 0x0001231F
	// (set) Token: 0x06001E40 RID: 7744 RVA: 0x000EA7F0 File Offset: 0x000E89F0
	public int height
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			int minHeight = this.minHeight;
			if (value < minHeight)
			{
				value = minHeight;
			}
			if (this.mHeight != value && this.keepAspectRatio != UIWidget.AspectRatioSource.BasedOnWidth)
			{
				if (this.isAnchoredVertically)
				{
					if (this.bottomAnchor.target != null && this.topAnchor.target != null)
					{
						if (this.mPivot == UIWidget.Pivot.BottomLeft || this.mPivot == UIWidget.Pivot.Bottom || this.mPivot == UIWidget.Pivot.BottomRight)
						{
							NGUIMath.AdjustWidget(this, 0f, 0f, 0f, (float)(value - this.mHeight));
						}
						else if (this.mPivot == UIWidget.Pivot.TopLeft || this.mPivot == UIWidget.Pivot.Top || this.mPivot == UIWidget.Pivot.TopRight)
						{
							NGUIMath.AdjustWidget(this, 0f, (float)(this.mHeight - value), 0f, 0f);
						}
						else
						{
							int num = value - this.mHeight;
							num -= (num & 1);
							if (num != 0)
							{
								NGUIMath.AdjustWidget(this, 0f, (float)(-(float)num) * 0.5f, 0f, (float)num * 0.5f);
							}
						}
					}
					else if (this.bottomAnchor.target != null)
					{
						NGUIMath.AdjustWidget(this, 0f, 0f, 0f, (float)(value - this.mHeight));
					}
					else
					{
						NGUIMath.AdjustWidget(this, 0f, (float)(this.mHeight - value), 0f, 0f);
					}
				}
				else
				{
					this.SetDimensions(this.mWidth, value);
				}
			}
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06001E41 RID: 7745 RVA: 0x00014127 File Offset: 0x00012327
	// (set) Token: 0x06001E42 RID: 7746 RVA: 0x000EA990 File Offset: 0x000E8B90
	public Color color
	{
		get
		{
			return this.mColor;
		}
		set
		{
			if (this.mColor != value)
			{
				bool includeChildren = this.mColor.a != value.a;
				this.mColor = value;
				this.Invalidate(includeChildren);
			}
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06001E43 RID: 7747 RVA: 0x0001412F File Offset: 0x0001232F
	// (set) Token: 0x06001E44 RID: 7748 RVA: 0x0001413C File Offset: 0x0001233C
	public override float alpha
	{
		get
		{
			return this.mColor.a;
		}
		set
		{
			if (this.mColor.a != value)
			{
				this.mColor.a = value;
				this.Invalidate(true);
			}
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06001E45 RID: 7749 RVA: 0x00014162 File Offset: 0x00012362
	public bool isVisible
	{
		get
		{
			return this.mIsVisibleByPanel && this.mIsVisibleByAlpha && this.mIsInFront && this.finalAlpha > 0.001f && NGUITools.GetActive(this);
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06001E46 RID: 7750 RVA: 0x0001419E File Offset: 0x0001239E
	public bool hasVertices
	{
		get
		{
			return this.geometry != null && this.geometry.hasVertices;
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06001E47 RID: 7751 RVA: 0x000141B9 File Offset: 0x000123B9
	// (set) Token: 0x06001E48 RID: 7752 RVA: 0x000141C1 File Offset: 0x000123C1
	public UIWidget.Pivot rawPivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				this.mPivot = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06001E49 RID: 7753 RVA: 0x000141B9 File Offset: 0x000123B9
	// (set) Token: 0x06001E4A RID: 7754 RVA: 0x000EA9D4 File Offset: 0x000E8BD4
	public UIWidget.Pivot pivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				Vector3 vector = this.worldCorners[0];
				this.mPivot = value;
				this.mChanged = true;
				Vector3 vector2 = this.worldCorners[0];
				Transform cachedTransform = base.cachedTransform;
				Vector3 vector3 = cachedTransform.position;
				float z = cachedTransform.localPosition.z;
				vector3.x += vector.x - vector2.x;
				vector3.y += vector.y - vector2.y;
				base.cachedTransform.position = vector3;
				vector3 = base.cachedTransform.localPosition;
				vector3.x = Mathf.Round(vector3.x);
				vector3.y = Mathf.Round(vector3.y);
				vector3.z = z;
				base.cachedTransform.localPosition = vector3;
			}
		}
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06001E4B RID: 7755 RVA: 0x000141ED File Offset: 0x000123ED
	// (set) Token: 0x06001E4C RID: 7756 RVA: 0x000EAACC File Offset: 0x000E8CCC
	public int depth
	{
		get
		{
			return this.mDepth;
		}
		set
		{
			if (this.mDepth != value)
			{
				if (this.panel != null)
				{
					this.panel.RemoveWidget(this);
				}
				this.mDepth = value;
				if (this.panel != null)
				{
					this.panel.AddWidget(this);
					if (!Application.isPlaying)
					{
						this.panel.SortWidgets();
						this.panel.RebuildAllDrawCalls();
					}
				}
			}
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06001E4D RID: 7757 RVA: 0x000EAB48 File Offset: 0x000E8D48
	public int raycastDepth
	{
		get
		{
			if (this.panel == null)
			{
				this.CreatePanel();
			}
			return (!(this.panel != null)) ? this.mDepth : (this.mDepth + this.panel.depth * 1000);
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06001E4E RID: 7758 RVA: 0x000EABA4 File Offset: 0x000E8DA4
	public override Vector3[] localCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			this.mCorners[0] = new Vector3(num, num2);
			this.mCorners[1] = new Vector3(num, num4);
			this.mCorners[2] = new Vector3(num3, num4);
			this.mCorners[3] = new Vector3(num3, num2);
			return this.mCorners;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06001E4F RID: 7759 RVA: 0x000EAC58 File Offset: 0x000E8E58
	public virtual Vector2 localSize
	{
		get
		{
			Vector3[] localCorners = this.localCorners;
			return localCorners[2] - localCorners[0];
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06001E50 RID: 7760 RVA: 0x000EAC90 File Offset: 0x000E8E90
	public Vector3 localCenter
	{
		get
		{
			Vector3[] localCorners = this.localCorners;
			return Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06001E51 RID: 7761 RVA: 0x000EACC8 File Offset: 0x000E8EC8
	public override Vector3[] worldCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			Transform cachedTransform = base.cachedTransform;
			this.mCorners[0] = cachedTransform.TransformPoint(num, num2, 0f);
			this.mCorners[1] = cachedTransform.TransformPoint(num, num4, 0f);
			this.mCorners[2] = cachedTransform.TransformPoint(num3, num4, 0f);
			this.mCorners[3] = cachedTransform.TransformPoint(num3, num2, 0f);
			return this.mCorners;
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06001E52 RID: 7762 RVA: 0x000141F5 File Offset: 0x000123F5
	public Vector3 worldCenter
	{
		get
		{
			return base.cachedTransform.TransformPoint(this.localCenter);
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06001E53 RID: 7763 RVA: 0x000EADA0 File Offset: 0x000E8FA0
	public virtual Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			return new Vector4((this.mDrawRegion.x != 0f) ? Mathf.Lerp(num, num3, this.mDrawRegion.x) : num, (this.mDrawRegion.y != 0f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.y) : num2, (this.mDrawRegion.z != 1f) ? Mathf.Lerp(num, num3, this.mDrawRegion.z) : num3, (this.mDrawRegion.w != 1f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.w) : num4);
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06001E54 RID: 7764 RVA: 0x00008E8C File Offset: 0x0000708C
	// (set) Token: 0x06001E55 RID: 7765 RVA: 0x00014208 File Offset: 0x00012408
	public virtual Material material
	{
		get
		{
			return null;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no material setter");
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06001E56 RID: 7766 RVA: 0x000EAEA8 File Offset: 0x000E90A8
	// (set) Token: 0x06001E57 RID: 7767 RVA: 0x0001421F File Offset: 0x0001241F
	public virtual Texture mainTexture
	{
		get
		{
			Material material = this.material;
			return (!(material != null)) ? null : material.mainTexture;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no mainTexture setter");
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06001E58 RID: 7768 RVA: 0x000EAED4 File Offset: 0x000E90D4
	// (set) Token: 0x06001E59 RID: 7769 RVA: 0x00014236 File Offset: 0x00012436
	public virtual Shader shader
	{
		get
		{
			Material material = this.material;
			return (!(material != null)) ? null : material.shader;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no shader setter");
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06001E5A RID: 7770 RVA: 0x0001424D File Offset: 0x0001244D
	[Obsolete("There is no relative scale anymore. Widgets now have width and height instead")]
	public Vector2 relativeSize
	{
		get
		{
			return Vector2.one;
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06001E5B RID: 7771 RVA: 0x000EAF00 File Offset: 0x000E9100
	public bool hasBoxCollider
	{
		get
		{
			BoxCollider boxCollider = base.collider as BoxCollider;
			return boxCollider != null || base.GetComponent<BoxCollider2D>() != null;
		}
	}

	// Token: 0x06001E5C RID: 7772 RVA: 0x000EAF34 File Offset: 0x000E9134
	public void SetDimensions(int w, int h)
	{
		if (this.mWidth != w || this.mHeight != h)
		{
			this.mWidth = w;
			this.mHeight = h;
			if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnWidth)
			{
				this.mHeight = Mathf.RoundToInt((float)this.mWidth / this.aspectRatio);
			}
			else if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnHeight)
			{
				this.mWidth = Mathf.RoundToInt((float)this.mHeight * this.aspectRatio);
			}
			else if (this.keepAspectRatio == UIWidget.AspectRatioSource.Free)
			{
				this.aspectRatio = (float)this.mWidth / (float)this.mHeight;
			}
			this.mMoved = true;
			if (this.autoResizeBoxCollider)
			{
				this.ResizeCollider();
			}
			this.MarkAsChanged();
		}
	}

	// Token: 0x06001E5D RID: 7773 RVA: 0x000EAFFC File Offset: 0x000E91FC
	public override Vector3[] GetSides(Transform relativeTo)
	{
		Vector2 pivotOffset = this.pivotOffset;
		float num = -pivotOffset.x * (float)this.mWidth;
		float num2 = -pivotOffset.y * (float)this.mHeight;
		float num3 = num + (float)this.mWidth;
		float num4 = num2 + (float)this.mHeight;
		float num5 = (num + num3) * 0.5f;
		float num6 = (num2 + num4) * 0.5f;
		Transform cachedTransform = base.cachedTransform;
		this.mCorners[0] = cachedTransform.TransformPoint(num, num6, 0f);
		this.mCorners[1] = cachedTransform.TransformPoint(num5, num4, 0f);
		this.mCorners[2] = cachedTransform.TransformPoint(num3, num6, 0f);
		this.mCorners[3] = cachedTransform.TransformPoint(num5, num2, 0f);
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				this.mCorners[i] = relativeTo.InverseTransformPoint(this.mCorners[i]);
			}
		}
		return this.mCorners;
	}

	// Token: 0x06001E5E RID: 7774 RVA: 0x00014254 File Offset: 0x00012454
	public override float CalculateFinalAlpha(int frameID)
	{
		if (this.mAlphaFrameID != frameID)
		{
			this.mAlphaFrameID = frameID;
			this.UpdateFinalAlpha(frameID);
		}
		return this.finalAlpha;
	}

	// Token: 0x06001E5F RID: 7775 RVA: 0x000EB13C File Offset: 0x000E933C
	protected void UpdateFinalAlpha(int frameID)
	{
		if (!this.mIsVisibleByAlpha || !this.mIsInFront)
		{
			this.finalAlpha = 0f;
		}
		else
		{
			UIRect parent = base.parent;
			this.finalAlpha = ((!(base.parent != null)) ? this.mColor.a : (parent.CalculateFinalAlpha(frameID) * this.mColor.a));
		}
	}

	// Token: 0x06001E60 RID: 7776 RVA: 0x000EB1B0 File Offset: 0x000E93B0
	public override void Invalidate(bool includeChildren)
	{
		this.mChanged = true;
		this.mAlphaFrameID = -1;
		if (this.panel != null)
		{
			bool visibleByPanel = (!this.hideIfOffScreen && !this.panel.hasCumulativeClipping) || this.panel.IsVisible(this);
			this.UpdateVisibility(this.CalculateCumulativeAlpha(Time.frameCount) > 0.001f, visibleByPanel);
			this.UpdateFinalAlpha(Time.frameCount);
			if (includeChildren)
			{
				base.Invalidate(true);
			}
		}
	}

	// Token: 0x06001E61 RID: 7777 RVA: 0x000EB23C File Offset: 0x000E943C
	public float CalculateCumulativeAlpha(int frameID)
	{
		UIRect parent = base.parent;
		return (!(parent != null)) ? this.mColor.a : (parent.CalculateFinalAlpha(frameID) * this.mColor.a);
	}

	// Token: 0x06001E62 RID: 7778 RVA: 0x000EB280 File Offset: 0x000E9480
	public override void SetRect(float x, float y, float width, float height)
	{
		Vector2 pivotOffset = this.pivotOffset;
		float num = Mathf.Lerp(x, x + width, pivotOffset.x);
		float num2 = Mathf.Lerp(y, y + height, pivotOffset.y);
		int num3 = Mathf.FloorToInt(width + 0.5f);
		int num4 = Mathf.FloorToInt(height + 0.5f);
		if (pivotOffset.x == 0.5f)
		{
			num3 = num3 >> 1 << 1;
		}
		if (pivotOffset.y == 0.5f)
		{
			num4 = num4 >> 1 << 1;
		}
		Transform transform = base.cachedTransform;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = Mathf.Floor(num + 0.5f);
		localPosition.y = Mathf.Floor(num2 + 0.5f);
		if (num3 < this.minWidth)
		{
			num3 = this.minWidth;
		}
		if (num4 < this.minHeight)
		{
			num4 = this.minHeight;
		}
		transform.localPosition = localPosition;
		this.width = num3;
		this.height = num4;
		if (base.isAnchored)
		{
			transform = transform.parent;
			if (this.leftAnchor.target)
			{
				this.leftAnchor.SetHorizontal(transform, x);
			}
			if (this.rightAnchor.target)
			{
				this.rightAnchor.SetHorizontal(transform, x + width);
			}
			if (this.bottomAnchor.target)
			{
				this.bottomAnchor.SetVertical(transform, y);
			}
			if (this.topAnchor.target)
			{
				this.topAnchor.SetVertical(transform, y + height);
			}
		}
	}

	// Token: 0x06001E63 RID: 7779 RVA: 0x00014276 File Offset: 0x00012476
	public void ResizeCollider()
	{
		if (NGUITools.GetActive(this))
		{
			NGUITools.UpdateWidgetCollider(base.gameObject);
		}
	}

	// Token: 0x06001E64 RID: 7780 RVA: 0x000EB420 File Offset: 0x000E9620
	[DebuggerStepThrough]
	[DebuggerHidden]
	public static int FullCompareFunc(UIWidget left, UIWidget right)
	{
		int num = UIPanel.CompareFunc(left.panel, right.panel);
		return (num != 0) ? num : UIWidget.PanelCompareFunc(left, right);
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x000EB454 File Offset: 0x000E9654
	[DebuggerStepThrough]
	[DebuggerHidden]
	public static int PanelCompareFunc(UIWidget left, UIWidget right)
	{
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		Material material = left.material;
		Material material2 = right.material;
		if (material == material2)
		{
			return 0;
		}
		if (material != null)
		{
			return -1;
		}
		if (material2 != null)
		{
			return 1;
		}
		return (material.GetInstanceID() >= material2.GetInstanceID()) ? 1 : -1;
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x0001428E File Offset: 0x0001248E
	public Bounds CalculateBounds()
	{
		return this.CalculateBounds(null);
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x000EB4D8 File Offset: 0x000E96D8
	public Bounds CalculateBounds(Transform relativeParent)
	{
		if (relativeParent == null)
		{
			Vector3[] localCorners = this.localCorners;
			Bounds result;
			result..ctor(localCorners[0], Vector3.zero);
			for (int i = 1; i < 4; i++)
			{
				result.Encapsulate(localCorners[i]);
			}
			return result;
		}
		Matrix4x4 worldToLocalMatrix = relativeParent.worldToLocalMatrix;
		Vector3[] worldCorners = this.worldCorners;
		Bounds result2;
		result2..ctor(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[0]), Vector3.zero);
		for (int j = 1; j < 4; j++)
		{
			result2.Encapsulate(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[j]));
		}
		return result2;
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x000EB59C File Offset: 0x000E979C
	public void SetDirty()
	{
		if (this.drawCall != null)
		{
			this.drawCall.isDirty = true;
		}
		else if (this.isVisible && this.hasVertices)
		{
			this.CreatePanel();
		}
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x00014297 File Offset: 0x00012497
	public void RemoveFromPanel()
	{
		if (this.panel != null)
		{
			this.panel.RemoveWidget(this);
			this.panel = null;
		}
		this.drawCall = null;
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x000EB5E8 File Offset: 0x000E97E8
	public virtual void MarkAsChanged()
	{
		if (NGUITools.GetActive(this))
		{
			this.mChanged = true;
			if (this.panel != null && base.enabled && NGUITools.GetActive(base.gameObject) && !this.mPlayMode)
			{
				this.SetDirty();
				this.CheckLayer();
			}
		}
	}

	// Token: 0x06001E6B RID: 7787 RVA: 0x000EB64C File Offset: 0x000E984C
	public UIPanel CreatePanel()
	{
		if (this.mStarted && this.panel == null && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.panel = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
			if (this.panel != null)
			{
				this.mParentFound = false;
				this.panel.AddWidget(this);
				this.CheckLayer();
				this.Invalidate(true);
			}
		}
		return this.panel;
	}

	// Token: 0x06001E6C RID: 7788 RVA: 0x000EB6E4 File Offset: 0x000E98E4
	public void CheckLayer()
	{
		if (this.panel != null && this.panel.gameObject.layer != base.gameObject.layer)
		{
			Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = this.panel.gameObject.layer;
		}
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x000EB748 File Offset: 0x000E9948
	public override void ParentHasChanged()
	{
		base.ParentHasChanged();
		if (this.panel != null)
		{
			UIPanel uipanel = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
			if (this.panel != uipanel)
			{
				this.RemoveFromPanel();
				this.CreatePanel();
			}
		}
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x000142C4 File Offset: 0x000124C4
	protected virtual void Awake()
	{
		this.mGo = base.gameObject;
		this.mPlayMode = Application.isPlaying;
	}

	// Token: 0x06001E6F RID: 7791 RVA: 0x000EB7A4 File Offset: 0x000E99A4
	protected override void OnInit()
	{
		base.OnInit();
		this.RemoveFromPanel();
		this.mMoved = true;
		if (this.mWidth == 100 && this.mHeight == 100 && base.cachedTransform.localScale.magnitude > 8f)
		{
			this.UpgradeFrom265();
			base.cachedTransform.localScale = Vector3.one;
		}
		base.Update();
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x000EB818 File Offset: 0x000E9A18
	protected virtual void UpgradeFrom265()
	{
		Vector3 localScale = base.cachedTransform.localScale;
		this.mWidth = Mathf.Abs(Mathf.RoundToInt(localScale.x));
		this.mHeight = Mathf.Abs(Mathf.RoundToInt(localScale.y));
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
	}

	// Token: 0x06001E71 RID: 7793 RVA: 0x000142DD File Offset: 0x000124DD
	protected override void OnStart()
	{
		this.CreatePanel();
	}

	// Token: 0x06001E72 RID: 7794 RVA: 0x000EB86C File Offset: 0x000E9A6C
	protected override void OnAnchor()
	{
		Transform cachedTransform = base.cachedTransform;
		Transform parent = cachedTransform.parent;
		Vector3 localPosition = cachedTransform.localPosition;
		Vector2 pivotOffset = this.pivotOffset;
		float num;
		float num2;
		float num3;
		float num4;
		if (this.leftAnchor.target == this.bottomAnchor.target && this.leftAnchor.target == this.rightAnchor.target && this.leftAnchor.target == this.topAnchor.target)
		{
			Vector3[] sides = this.leftAnchor.GetSides(parent);
			if (sides != null)
			{
				num = NGUIMath.Lerp(sides[0].x, sides[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				num2 = NGUIMath.Lerp(sides[0].x, sides[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				num3 = NGUIMath.Lerp(sides[3].y, sides[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				num4 = NGUIMath.Lerp(sides[3].y, sides[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
				this.mIsInFront = true;
			}
			else
			{
				Vector3 localPos = base.GetLocalPos(this.leftAnchor, parent);
				num = localPos.x + (float)this.leftAnchor.absolute;
				num3 = localPos.y + (float)this.bottomAnchor.absolute;
				num2 = localPos.x + (float)this.rightAnchor.absolute;
				num4 = localPos.y + (float)this.topAnchor.absolute;
				this.mIsInFront = (!this.hideIfOffScreen || localPos.z >= 0f);
			}
		}
		else
		{
			this.mIsInFront = true;
			if (this.leftAnchor.target)
			{
				Vector3[] sides2 = this.leftAnchor.GetSides(parent);
				if (sides2 != null)
				{
					num = NGUIMath.Lerp(sides2[0].x, sides2[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				}
				else
				{
					num = base.GetLocalPos(this.leftAnchor, parent).x + (float)this.leftAnchor.absolute;
				}
			}
			else
			{
				num = localPosition.x - pivotOffset.x * (float)this.mWidth;
			}
			if (this.rightAnchor.target)
			{
				Vector3[] sides3 = this.rightAnchor.GetSides(parent);
				if (sides3 != null)
				{
					num2 = NGUIMath.Lerp(sides3[0].x, sides3[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				}
				else
				{
					num2 = base.GetLocalPos(this.rightAnchor, parent).x + (float)this.rightAnchor.absolute;
				}
			}
			else
			{
				num2 = localPosition.x - pivotOffset.x * (float)this.mWidth + (float)this.mWidth;
			}
			if (this.bottomAnchor.target)
			{
				Vector3[] sides4 = this.bottomAnchor.GetSides(parent);
				if (sides4 != null)
				{
					num3 = NGUIMath.Lerp(sides4[3].y, sides4[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				}
				else
				{
					num3 = base.GetLocalPos(this.bottomAnchor, parent).y + (float)this.bottomAnchor.absolute;
				}
			}
			else
			{
				num3 = localPosition.y - pivotOffset.y * (float)this.mHeight;
			}
			if (this.topAnchor.target)
			{
				Vector3[] sides5 = this.topAnchor.GetSides(parent);
				if (sides5 != null)
				{
					num4 = NGUIMath.Lerp(sides5[3].y, sides5[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
				}
				else
				{
					num4 = base.GetLocalPos(this.topAnchor, parent).y + (float)this.topAnchor.absolute;
				}
			}
			else
			{
				num4 = localPosition.y - pivotOffset.y * (float)this.mHeight + (float)this.mHeight;
			}
		}
		Vector3 vector;
		vector..ctor(Mathf.Lerp(num, num2, pivotOffset.x), Mathf.Lerp(num3, num4, pivotOffset.y), localPosition.z);
		int num5 = Mathf.FloorToInt(num2 - num + 0.5f);
		int num6 = Mathf.FloorToInt(num4 - num3 + 0.5f);
		if (this.keepAspectRatio != UIWidget.AspectRatioSource.Free && this.aspectRatio != 0f)
		{
			if (this.keepAspectRatio == UIWidget.AspectRatioSource.BasedOnHeight)
			{
				num5 = Mathf.RoundToInt((float)num6 * this.aspectRatio);
			}
			else
			{
				num6 = Mathf.RoundToInt((float)num5 / this.aspectRatio);
			}
		}
		if (num5 < this.minWidth)
		{
			num5 = this.minWidth;
		}
		if (num6 < this.minHeight)
		{
			num6 = this.minHeight;
		}
		if (Vector3.SqrMagnitude(localPosition - vector) > 0.001f)
		{
			base.cachedTransform.localPosition = vector;
			if (this.mIsInFront)
			{
				this.mChanged = true;
			}
		}
		if (this.mWidth != num5 || this.mHeight != num6)
		{
			this.mWidth = num5;
			this.mHeight = num6;
			if (this.mIsInFront)
			{
				this.mChanged = true;
			}
			if (this.autoResizeBoxCollider)
			{
				this.ResizeCollider();
			}
		}
	}

	// Token: 0x06001E73 RID: 7795 RVA: 0x000142E6 File Offset: 0x000124E6
	protected override void OnUpdate()
	{
		if (this.panel == null)
		{
			this.CreatePanel();
		}
	}

	// Token: 0x06001E74 RID: 7796 RVA: 0x00014300 File Offset: 0x00012500
	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			this.MarkAsChanged();
		}
	}

	// Token: 0x06001E75 RID: 7797 RVA: 0x0001430E File Offset: 0x0001250E
	protected override void OnDisable()
	{
		this.RemoveFromPanel();
		base.OnDisable();
	}

	// Token: 0x06001E76 RID: 7798 RVA: 0x0001431C File Offset: 0x0001251C
	private void OnDestroy()
	{
		this.RemoveFromPanel();
	}

	// Token: 0x06001E77 RID: 7799 RVA: 0x00014324 File Offset: 0x00012524
	public bool UpdateVisibility(bool visibleByAlpha, bool visibleByPanel)
	{
		if (this.mIsVisibleByAlpha != visibleByAlpha || this.mIsVisibleByPanel != visibleByPanel)
		{
			this.mChanged = true;
			this.mIsVisibleByAlpha = visibleByAlpha;
			this.mIsVisibleByPanel = visibleByPanel;
			return true;
		}
		return false;
	}

	// Token: 0x06001E78 RID: 7800 RVA: 0x000EBE90 File Offset: 0x000EA090
	public bool UpdateTransform(int frame)
	{
		if (!this.mMoved && !this.panel.widgetsAreStatic && base.cachedTransform.hasChanged)
		{
			this.mTrans.hasChanged = false;
			this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
			this.mMatrixFrame = frame;
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			Transform cachedTransform = base.cachedTransform;
			Vector3 vector = cachedTransform.TransformPoint(num, num2, 0f);
			Vector3 vector2 = cachedTransform.TransformPoint(num3, num4, 0f);
			vector = this.panel.worldToLocal.MultiplyPoint3x4(vector);
			vector2 = this.panel.worldToLocal.MultiplyPoint3x4(vector2);
			if (Vector3.SqrMagnitude(this.mOldV0 - vector) > 1E-06f || Vector3.SqrMagnitude(this.mOldV1 - vector2) > 1E-06f)
			{
				this.mMoved = true;
				this.mOldV0 = vector;
				this.mOldV1 = vector2;
			}
		}
		if (this.mMoved && this.onChange != null)
		{
			this.onChange();
		}
		return this.mMoved || this.mChanged;
	}

	// Token: 0x06001E79 RID: 7801 RVA: 0x000EC010 File Offset: 0x000EA210
	public bool UpdateGeometry(int frame)
	{
		float num = this.CalculateFinalAlpha(frame);
		if (this.mIsVisibleByAlpha && this.mLastAlpha != num)
		{
			this.mChanged = true;
		}
		this.mLastAlpha = num;
		if (this.mChanged)
		{
			this.mChanged = false;
			if (this.mIsVisibleByAlpha && num > 0.001f && this.shader != null)
			{
				bool hasVertices = this.geometry.hasVertices;
				if (this.fillGeometry)
				{
					this.geometry.Clear();
					this.OnFill(this.geometry.verts, this.geometry.uvs, this.geometry.cols);
				}
				if (this.geometry.hasVertices)
				{
					if (this.mMatrixFrame != frame)
					{
						this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
						this.mMatrixFrame = frame;
					}
					this.geometry.ApplyTransform(this.mLocalToPanel);
					this.mMoved = false;
					return true;
				}
				return hasVertices;
			}
			else if (this.geometry.hasVertices)
			{
				if (this.fillGeometry)
				{
					this.geometry.Clear();
				}
				this.mMoved = false;
				return true;
			}
		}
		else if (this.mMoved && this.geometry.hasVertices)
		{
			if (this.mMatrixFrame != frame)
			{
				this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
				this.mMatrixFrame = frame;
			}
			this.geometry.ApplyTransform(this.mLocalToPanel);
			this.mMoved = false;
			return true;
		}
		this.mMoved = false;
		return false;
	}

	// Token: 0x06001E7A RID: 7802 RVA: 0x00014356 File Offset: 0x00012556
	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		this.geometry.WriteToBuffers(v, u, c, n, t);
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x000EC1D4 File Offset: 0x000EA3D4
	public virtual void MakePixelPerfect()
	{
		Vector3 localPosition = base.cachedTransform.localPosition;
		localPosition.z = Mathf.Round(localPosition.z);
		localPosition.x = Mathf.Round(localPosition.x);
		localPosition.y = Mathf.Round(localPosition.y);
		base.cachedTransform.localPosition = localPosition;
		Vector3 localScale = base.cachedTransform.localScale;
		base.cachedTransform.localScale = new Vector3(Mathf.Sign(localScale.x), Mathf.Sign(localScale.y), 1f);
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06001E7C RID: 7804 RVA: 0x00003023 File Offset: 0x00001223
	public virtual int minWidth
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06001E7D RID: 7805 RVA: 0x00003023 File Offset: 0x00001223
	public virtual int minHeight
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06001E7E RID: 7806 RVA: 0x0001436A File Offset: 0x0001256A
	// (set) Token: 0x06001E7F RID: 7807 RVA: 0x0000264F File Offset: 0x0000084F
	public virtual Vector4 border
	{
		get
		{
			return Vector4.zero;
		}
		set
		{
		}
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x0000264F File Offset: 0x0000084F
	public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
	}

	// Token: 0x04002237 RID: 8759
	[SerializeField]
	[HideInInspector]
	protected Color mColor = Color.white;

	// Token: 0x04002238 RID: 8760
	[SerializeField]
	[HideInInspector]
	protected UIWidget.Pivot mPivot = UIWidget.Pivot.Center;

	// Token: 0x04002239 RID: 8761
	[SerializeField]
	[HideInInspector]
	protected int mWidth = 100;

	// Token: 0x0400223A RID: 8762
	[HideInInspector]
	[SerializeField]
	protected int mHeight = 100;

	// Token: 0x0400223B RID: 8763
	[SerializeField]
	[HideInInspector]
	protected int mDepth;

	// Token: 0x0400223C RID: 8764
	public UIWidget.OnDimensionsChanged onChange;

	// Token: 0x0400223D RID: 8765
	public UIWidget.OnPostFillCallback onPostFill;

	// Token: 0x0400223E RID: 8766
	public UIDrawCall.OnRenderCallback mOnRender;

	// Token: 0x0400223F RID: 8767
	public bool autoResizeBoxCollider;

	// Token: 0x04002240 RID: 8768
	public bool hideIfOffScreen;

	// Token: 0x04002241 RID: 8769
	public UIWidget.AspectRatioSource keepAspectRatio;

	// Token: 0x04002242 RID: 8770
	public float aspectRatio = 1f;

	// Token: 0x04002243 RID: 8771
	public UIWidget.HitCheck hitCheck;

	// Token: 0x04002244 RID: 8772
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x04002245 RID: 8773
	[NonSerialized]
	public UIGeometry geometry = new UIGeometry();

	// Token: 0x04002246 RID: 8774
	[NonSerialized]
	public bool fillGeometry = true;

	// Token: 0x04002247 RID: 8775
	[NonSerialized]
	protected bool mPlayMode = true;

	// Token: 0x04002248 RID: 8776
	[NonSerialized]
	protected Vector4 mDrawRegion = new Vector4(0f, 0f, 1f, 1f);

	// Token: 0x04002249 RID: 8777
	[NonSerialized]
	private Matrix4x4 mLocalToPanel;

	// Token: 0x0400224A RID: 8778
	[NonSerialized]
	private bool mIsVisibleByAlpha = true;

	// Token: 0x0400224B RID: 8779
	[NonSerialized]
	private bool mIsVisibleByPanel = true;

	// Token: 0x0400224C RID: 8780
	[NonSerialized]
	private bool mIsInFront = true;

	// Token: 0x0400224D RID: 8781
	[NonSerialized]
	private float mLastAlpha;

	// Token: 0x0400224E RID: 8782
	[NonSerialized]
	private bool mMoved;

	// Token: 0x0400224F RID: 8783
	[NonSerialized]
	public UIDrawCall drawCall;

	// Token: 0x04002250 RID: 8784
	[NonSerialized]
	protected Vector3[] mCorners = new Vector3[4];

	// Token: 0x04002251 RID: 8785
	[NonSerialized]
	private int mAlphaFrameID = -1;

	// Token: 0x04002252 RID: 8786
	private int mMatrixFrame = -1;

	// Token: 0x04002253 RID: 8787
	private Vector3 mOldV0;

	// Token: 0x04002254 RID: 8788
	private Vector3 mOldV1;

	// Token: 0x020004C1 RID: 1217
	public enum Pivot
	{
		// Token: 0x04002256 RID: 8790
		TopLeft,
		// Token: 0x04002257 RID: 8791
		Top,
		// Token: 0x04002258 RID: 8792
		TopRight,
		// Token: 0x04002259 RID: 8793
		Left,
		// Token: 0x0400225A RID: 8794
		Center,
		// Token: 0x0400225B RID: 8795
		Right,
		// Token: 0x0400225C RID: 8796
		BottomLeft,
		// Token: 0x0400225D RID: 8797
		Bottom,
		// Token: 0x0400225E RID: 8798
		BottomRight
	}

	// Token: 0x020004C2 RID: 1218
	public enum AspectRatioSource
	{
		// Token: 0x04002260 RID: 8800
		Free,
		// Token: 0x04002261 RID: 8801
		BasedOnWidth,
		// Token: 0x04002262 RID: 8802
		BasedOnHeight
	}

	// Token: 0x020004C3 RID: 1219
	// (Invoke) Token: 0x06001E82 RID: 7810
	public delegate void OnDimensionsChanged();

	// Token: 0x020004C4 RID: 1220
	// (Invoke) Token: 0x06001E86 RID: 7814
	public delegate void OnPostFillCallback(UIWidget widget, int bufferOffset, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols);

	// Token: 0x020004C5 RID: 1221
	// (Invoke) Token: 0x06001E8A RID: 7818
	public delegate bool HitCheck(Vector3 worldPos);
}
