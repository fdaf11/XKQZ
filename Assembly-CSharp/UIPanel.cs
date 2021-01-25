using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000500 RID: 1280
[AddComponentMenu("NGUI/UI/NGUI Panel")]
[ExecuteInEditMode]
public class UIPanel : UIRect
{
	// Token: 0x17000324 RID: 804
	// (get) Token: 0x060020A2 RID: 8354 RVA: 0x000F6894 File Offset: 0x000F4A94
	public static int nextUnusedDepth
	{
		get
		{
			int num = int.MinValue;
			int i = 0;
			int count = UIPanel.list.Count;
			while (i < count)
			{
				num = Mathf.Max(num, UIPanel.list[i].depth);
				i++;
			}
			return (num != int.MinValue) ? (num + 1) : 0;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x060020A3 RID: 8355 RVA: 0x00015E60 File Offset: 0x00014060
	public override bool canBeAnchored
	{
		get
		{
			return this.mClipping != UIDrawCall.Clipping.None;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x060020A4 RID: 8356 RVA: 0x00015E6E File Offset: 0x0001406E
	// (set) Token: 0x060020A5 RID: 8357 RVA: 0x000F68F0 File Offset: 0x000F4AF0
	public override float alpha
	{
		get
		{
			return this.mAlpha;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mAlpha != num)
			{
				this.mAlphaFrameID = -1;
				this.mResized = true;
				this.mAlpha = num;
				this.SetDirty();
			}
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x060020A6 RID: 8358 RVA: 0x00015E76 File Offset: 0x00014076
	// (set) Token: 0x060020A7 RID: 8359 RVA: 0x00015E7E File Offset: 0x0001407E
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
				this.mDepth = value;
				UIPanel.list.Sort(new Comparison<UIPanel>(UIPanel.CompareFunc));
			}
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x060020A8 RID: 8360 RVA: 0x00015EA9 File Offset: 0x000140A9
	// (set) Token: 0x060020A9 RID: 8361 RVA: 0x00015EB1 File Offset: 0x000140B1
	public int sortingOrder
	{
		get
		{
			return this.mSortingOrder;
		}
		set
		{
			if (this.mSortingOrder != value)
			{
				this.mSortingOrder = value;
				this.UpdateDrawCalls();
			}
		}
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x000F692C File Offset: 0x000F4B2C
	public static int CompareFunc(UIPanel a, UIPanel b)
	{
		if (!(a != b) || !(a != null) || !(b != null))
		{
			return 0;
		}
		if (a.mDepth < b.mDepth)
		{
			return -1;
		}
		if (a.mDepth > b.mDepth)
		{
			return 1;
		}
		return (a.GetInstanceID() >= b.GetInstanceID()) ? 1 : -1;
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x060020AB RID: 8363 RVA: 0x000F69A0 File Offset: 0x000F4BA0
	public float width
	{
		get
		{
			return this.GetViewSize().x;
		}
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x060020AC RID: 8364 RVA: 0x000F69BC File Offset: 0x000F4BBC
	public float height
	{
		get
		{
			return this.GetViewSize().y;
		}
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x060020AD RID: 8365 RVA: 0x00015ECC File Offset: 0x000140CC
	public bool halfPixelOffset
	{
		get
		{
			return this.mHalfPixelOffset;
		}
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x060020AE RID: 8366 RVA: 0x00015ED4 File Offset: 0x000140D4
	public bool usedForUI
	{
		get
		{
			return base.anchorCamera != null && this.mCam.isOrthoGraphic;
		}
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x060020AF RID: 8367 RVA: 0x000F69D8 File Offset: 0x000F4BD8
	public Vector3 drawCallOffset
	{
		get
		{
			if (this.mHalfPixelOffset && base.anchorCamera != null && this.mCam.isOrthoGraphic)
			{
				float num = 1f / this.GetWindowSize().y / this.mCam.orthographicSize;
				return new Vector3(-num, num);
			}
			return Vector3.zero;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x060020B0 RID: 8368 RVA: 0x00015EF5 File Offset: 0x000140F5
	// (set) Token: 0x060020B1 RID: 8369 RVA: 0x00015EFD File Offset: 0x000140FD
	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mResized = true;
				this.mClipping = value;
				this.mMatrixFrame = -1;
			}
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x060020B2 RID: 8370 RVA: 0x00015F20 File Offset: 0x00014120
	public UIPanel parentPanel
	{
		get
		{
			return this.mParentPanel;
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x060020B3 RID: 8371 RVA: 0x000F6A40 File Offset: 0x000F4C40
	public int clipCount
	{
		get
		{
			int num = 0;
			UIPanel uipanel = this;
			while (uipanel != null)
			{
				if (uipanel.mClipping == UIDrawCall.Clipping.SoftClip || uipanel.mClipping == UIDrawCall.Clipping.TextureMask)
				{
					num++;
				}
				uipanel = uipanel.mParentPanel;
			}
			return num;
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x060020B4 RID: 8372 RVA: 0x00015F28 File Offset: 0x00014128
	public bool hasClipping
	{
		get
		{
			return this.mClipping == UIDrawCall.Clipping.SoftClip || this.mClipping == UIDrawCall.Clipping.TextureMask;
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x060020B5 RID: 8373 RVA: 0x00015F42 File Offset: 0x00014142
	public bool hasCumulativeClipping
	{
		get
		{
			return this.clipCount != 0;
		}
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x060020B6 RID: 8374 RVA: 0x00015F50 File Offset: 0x00014150
	[Obsolete("Use 'hasClipping' or 'hasCumulativeClipping' instead")]
	public bool clipsChildren
	{
		get
		{
			return this.hasCumulativeClipping;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x060020B7 RID: 8375 RVA: 0x00015F58 File Offset: 0x00014158
	// (set) Token: 0x060020B8 RID: 8376 RVA: 0x000F6A88 File Offset: 0x000F4C88
	public Vector2 clipOffset
	{
		get
		{
			return this.mClipOffset;
		}
		set
		{
			if (Mathf.Abs(this.mClipOffset.x - value.x) > 0.001f || Mathf.Abs(this.mClipOffset.y - value.y) > 0.001f)
			{
				this.mClipOffset = value;
				this.InvalidateClipping();
				if (this.onClipMove != null)
				{
					this.onClipMove(this);
				}
			}
		}
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x000F6B00 File Offset: 0x000F4D00
	private void InvalidateClipping()
	{
		this.mResized = true;
		this.mMatrixFrame = -1;
		this.mCullTime = ((this.mCullTime != 0f) ? (RealTime.time + 0.15f) : 0.001f);
		int i = 0;
		int count = UIPanel.list.Count;
		while (i < count)
		{
			UIPanel uipanel = UIPanel.list[i];
			if (uipanel != this && uipanel.parentPanel == this)
			{
				uipanel.InvalidateClipping();
			}
			i++;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x060020BA RID: 8378 RVA: 0x00015F60 File Offset: 0x00014160
	// (set) Token: 0x060020BB RID: 8379 RVA: 0x00015F68 File Offset: 0x00014168
	public Texture2D clipTexture
	{
		get
		{
			return this.mClipTexture;
		}
		set
		{
			if (this.mClipTexture != value)
			{
				this.mClipTexture = value;
			}
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x060020BC RID: 8380 RVA: 0x00015F82 File Offset: 0x00014182
	// (set) Token: 0x060020BD RID: 8381 RVA: 0x00015F8A File Offset: 0x0001418A
	[Obsolete("Use 'finalClipRegion' or 'baseClipRegion' instead")]
	public Vector4 clipRange
	{
		get
		{
			return this.baseClipRegion;
		}
		set
		{
			this.baseClipRegion = value;
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x060020BE RID: 8382 RVA: 0x00015F93 File Offset: 0x00014193
	// (set) Token: 0x060020BF RID: 8383 RVA: 0x000F6B94 File Offset: 0x000F4D94
	public Vector4 baseClipRegion
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			if (Mathf.Abs(this.mClipRange.x - value.x) > 0.001f || Mathf.Abs(this.mClipRange.y - value.y) > 0.001f || Mathf.Abs(this.mClipRange.z - value.z) > 0.001f || Mathf.Abs(this.mClipRange.w - value.w) > 0.001f)
			{
				this.mResized = true;
				this.mCullTime = ((this.mCullTime != 0f) ? (RealTime.time + 0.15f) : 0.001f);
				this.mClipRange = value;
				this.mMatrixFrame = -1;
				UIScrollView component = base.GetComponent<UIScrollView>();
				if (component != null)
				{
					component.UpdatePosition();
				}
				if (this.onClipMove != null)
				{
					this.onClipMove(this);
				}
			}
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x060020C0 RID: 8384 RVA: 0x000F6C9C File Offset: 0x000F4E9C
	public Vector4 finalClipRegion
	{
		get
		{
			Vector2 viewSize = this.GetViewSize();
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				return new Vector4(this.mClipRange.x + this.mClipOffset.x, this.mClipRange.y + this.mClipOffset.y, viewSize.x, viewSize.y);
			}
			return new Vector4(0f, 0f, viewSize.x, viewSize.y);
		}
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x060020C1 RID: 8385 RVA: 0x00015F9B File Offset: 0x0001419B
	// (set) Token: 0x060020C2 RID: 8386 RVA: 0x00015FA3 File Offset: 0x000141A3
	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoftness;
		}
		set
		{
			if (this.mClipSoftness != value)
			{
				this.mClipSoftness = value;
			}
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x060020C3 RID: 8387 RVA: 0x000F6D1C File Offset: 0x000F4F1C
	public override Vector3[] localCorners
	{
		get
		{
			if (this.mClipping == UIDrawCall.Clipping.None)
			{
				Vector3[] worldCorners = this.worldCorners;
				Transform cachedTransform = base.cachedTransform;
				for (int i = 0; i < 4; i++)
				{
					worldCorners[i] = cachedTransform.InverseTransformPoint(worldCorners[i]);
				}
				return worldCorners;
			}
			float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
			float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
			float num3 = num + this.mClipRange.z;
			float num4 = num2 + this.mClipRange.w;
			UIPanel.mCorners[0] = new Vector3(num, num2);
			UIPanel.mCorners[1] = new Vector3(num, num4);
			UIPanel.mCorners[2] = new Vector3(num3, num4);
			UIPanel.mCorners[3] = new Vector3(num3, num2);
			return UIPanel.mCorners;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x060020C4 RID: 8388 RVA: 0x000F6E50 File Offset: 0x000F5050
	public override Vector3[] worldCorners
	{
		get
		{
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
				float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
				float num3 = num + this.mClipRange.z;
				float num4 = num2 + this.mClipRange.w;
				Transform cachedTransform = base.cachedTransform;
				UIPanel.mCorners[0] = cachedTransform.TransformPoint(num, num2, 0f);
				UIPanel.mCorners[1] = cachedTransform.TransformPoint(num, num4, 0f);
				UIPanel.mCorners[2] = cachedTransform.TransformPoint(num3, num4, 0f);
				UIPanel.mCorners[3] = cachedTransform.TransformPoint(num3, num2, 0f);
			}
			else
			{
				if (base.anchorCamera != null)
				{
					return this.mCam.GetWorldCorners(base.cameraRayDistance);
				}
				Vector2 viewSize = this.GetViewSize();
				float num5 = -0.5f * viewSize.x;
				float num6 = -0.5f * viewSize.y;
				float num7 = num5 + viewSize.x;
				float num8 = num6 + viewSize.y;
				UIPanel.mCorners[0] = new Vector3(num5, num6);
				UIPanel.mCorners[1] = new Vector3(num5, num8);
				UIPanel.mCorners[2] = new Vector3(num7, num8);
				UIPanel.mCorners[3] = new Vector3(num7, num6);
				if ((this.anchorOffset && this.mCam == null) || this.mCam.transform.parent != base.cachedTransform)
				{
					Vector3 position = base.cachedTransform.position;
					for (int i = 0; i < 4; i++)
					{
						UIPanel.mCorners[i] += position;
					}
				}
			}
			return UIPanel.mCorners;
		}
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x000F70A8 File Offset: 0x000F52A8
	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			float num = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
			float num2 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
			float num3 = num + this.mClipRange.z;
			float num4 = num2 + this.mClipRange.w;
			float num5 = (num + num3) * 0.5f;
			float num6 = (num2 + num4) * 0.5f;
			Transform cachedTransform = base.cachedTransform;
			UIRect.mSides[0] = cachedTransform.TransformPoint(num, num6, 0f);
			UIRect.mSides[1] = cachedTransform.TransformPoint(num5, num4, 0f);
			UIRect.mSides[2] = cachedTransform.TransformPoint(num3, num6, 0f);
			UIRect.mSides[3] = cachedTransform.TransformPoint(num5, num2, 0f);
			if (relativeTo != null)
			{
				for (int i = 0; i < 4; i++)
				{
					UIRect.mSides[i] = relativeTo.InverseTransformPoint(UIRect.mSides[i]);
				}
			}
			return UIRect.mSides;
		}
		if (base.anchorCamera != null && this.anchorOffset)
		{
			Vector3[] sides = this.mCam.GetSides(base.cameraRayDistance);
			Vector3 position = base.cachedTransform.position;
			for (int j = 0; j < 4; j++)
			{
				sides[j] += position;
			}
			if (relativeTo != null)
			{
				for (int k = 0; k < 4; k++)
				{
					sides[k] = relativeTo.InverseTransformPoint(sides[k]);
				}
			}
			return sides;
		}
		return base.GetSides(relativeTo);
	}

	// Token: 0x060020C6 RID: 8390 RVA: 0x00015FBD File Offset: 0x000141BD
	public override void Invalidate(bool includeChildren)
	{
		this.mAlphaFrameID = -1;
		base.Invalidate(includeChildren);
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x000F72D8 File Offset: 0x000F54D8
	public override float CalculateFinalAlpha(int frameID)
	{
		if (this.mAlphaFrameID != frameID)
		{
			this.mAlphaFrameID = frameID;
			UIRect parent = base.parent;
			this.finalAlpha = ((!(base.parent != null)) ? this.mAlpha : (parent.CalculateFinalAlpha(frameID) * this.mAlpha));
		}
		return this.finalAlpha;
	}

	// Token: 0x060020C8 RID: 8392 RVA: 0x000F7338 File Offset: 0x000F5538
	public override void SetRect(float x, float y, float width, float height)
	{
		int num = Mathf.FloorToInt(width + 0.5f);
		int num2 = Mathf.FloorToInt(height + 0.5f);
		num = num >> 1 << 1;
		num2 = num2 >> 1 << 1;
		Transform transform = base.cachedTransform;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = Mathf.Floor(x + 0.5f);
		localPosition.y = Mathf.Floor(y + 0.5f);
		if (num < 2)
		{
			num = 2;
		}
		if (num2 < 2)
		{
			num2 = 2;
		}
		this.baseClipRegion = new Vector4(localPosition.x, localPosition.y, (float)num, (float)num2);
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

	// Token: 0x060020C9 RID: 8393 RVA: 0x000F7470 File Offset: 0x000F5670
	public bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		this.UpdateTransformMatrix();
		a = this.worldToLocal.MultiplyPoint3x4(a);
		b = this.worldToLocal.MultiplyPoint3x4(b);
		c = this.worldToLocal.MultiplyPoint3x4(c);
		d = this.worldToLocal.MultiplyPoint3x4(d);
		UIPanel.mTemp[0] = a.x;
		UIPanel.mTemp[1] = b.x;
		UIPanel.mTemp[2] = c.x;
		UIPanel.mTemp[3] = d.x;
		float num = Mathf.Min(UIPanel.mTemp);
		float num2 = Mathf.Max(UIPanel.mTemp);
		UIPanel.mTemp[0] = a.y;
		UIPanel.mTemp[1] = b.y;
		UIPanel.mTemp[2] = c.y;
		UIPanel.mTemp[3] = d.y;
		float num3 = Mathf.Min(UIPanel.mTemp);
		float num4 = Mathf.Max(UIPanel.mTemp);
		return num2 >= this.mMin.x && num4 >= this.mMin.y && num <= this.mMax.x && num3 <= this.mMax.y;
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x000F75A8 File Offset: 0x000F57A8
	public bool IsVisible(Vector3 worldPos)
	{
		if (this.mAlpha < 0.001f)
		{
			return false;
		}
		if (this.mClipping == UIDrawCall.Clipping.None || this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip)
		{
			return true;
		}
		this.UpdateTransformMatrix();
		Vector3 vector = this.worldToLocal.MultiplyPoint3x4(worldPos);
		return vector.x >= this.mMin.x && vector.y >= this.mMin.y && vector.x <= this.mMax.x && vector.y <= this.mMax.y;
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x000F7658 File Offset: 0x000F5858
	public bool IsVisible(UIWidget w)
	{
		UIPanel uipanel = this;
		Vector3[] array = null;
		while (uipanel != null)
		{
			if ((uipanel.mClipping == UIDrawCall.Clipping.None || uipanel.mClipping == UIDrawCall.Clipping.ConstrainButDontClip) && !w.hideIfOffScreen)
			{
				uipanel = uipanel.mParentPanel;
			}
			else
			{
				if (array == null)
				{
					array = w.worldCorners;
				}
				if (!uipanel.IsVisible(array[0], array[1], array[2], array[3]))
				{
					return false;
				}
				uipanel = uipanel.mParentPanel;
			}
		}
		return true;
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x000F76FC File Offset: 0x000F58FC
	public bool Affects(UIWidget w)
	{
		if (w == null)
		{
			return false;
		}
		UIPanel panel = w.panel;
		if (panel == null)
		{
			return false;
		}
		UIPanel uipanel = this;
		while (uipanel != null)
		{
			if (uipanel == panel)
			{
				return true;
			}
			if (!uipanel.hasCumulativeClipping)
			{
				return false;
			}
			uipanel = uipanel.mParentPanel;
		}
		return false;
	}

	// Token: 0x060020CD RID: 8397 RVA: 0x00015FCD File Offset: 0x000141CD
	[ContextMenu("Force Refresh")]
	public void RebuildAllDrawCalls()
	{
		this.mRebuild = true;
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x000F7764 File Offset: 0x000F5964
	public void SetDirty()
	{
		int i = 0;
		int count = this.drawCalls.Count;
		while (i < count)
		{
			this.drawCalls[i].isDirty = true;
			i++;
		}
		this.Invalidate(true);
	}

	// Token: 0x060020CF RID: 8399 RVA: 0x000F77A8 File Offset: 0x000F59A8
	private void Awake()
	{
		this.mGo = base.gameObject;
		this.mTrans = base.transform;
		this.mHalfPixelOffset = (Application.platform == 2 || Application.platform == 10 || Application.platform == 5 || Application.platform == 7);
		if (this.mHalfPixelOffset)
		{
			this.mHalfPixelOffset = (SystemInfo.graphicsShaderLevel < 40 && SystemInfo.graphicsDeviceVersion.Contains("Direct3D"));
		}
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x000F7830 File Offset: 0x000F5A30
	private void FindParent()
	{
		Transform parent = base.cachedTransform.parent;
		this.mParentPanel = ((!(parent != null)) ? null : NGUITools.FindInParents<UIPanel>(parent.gameObject));
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x00015FD6 File Offset: 0x000141D6
	public override void ParentHasChanged()
	{
		base.ParentHasChanged();
		this.FindParent();
	}

	// Token: 0x060020D2 RID: 8402 RVA: 0x00015FE4 File Offset: 0x000141E4
	protected override void OnStart()
	{
		this.mLayer = this.mGo.layer;
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x00015FF7 File Offset: 0x000141F7
	protected override void OnEnable()
	{
		this.mRebuild = true;
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		this.OnStart();
		base.OnEnable();
		this.mMatrixFrame = -1;
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x000F786C File Offset: 0x000F5A6C
	protected override void OnInit()
	{
		base.OnInit();
		this.FindParent();
		if (base.rigidbody == null && this.mParentPanel == null)
		{
			UICamera uicamera = (!(base.anchorCamera != null)) ? null : this.mCam.GetComponent<UICamera>();
			if (uicamera != null && (uicamera.eventType == UICamera.EventType.UI_3D || uicamera.eventType == UICamera.EventType.World_3D))
			{
				Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
				rigidbody.isKinematic = true;
				rigidbody.useGravity = false;
			}
		}
		this.mRebuild = true;
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		UIPanel.list.Add(this);
		UIPanel.list.Sort(new Comparison<UIPanel>(UIPanel.CompareFunc));
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x000F7940 File Offset: 0x000F5B40
	protected override void OnDisable()
	{
		int i = 0;
		int count = this.drawCalls.Count;
		while (i < count)
		{
			UIDrawCall uidrawCall = this.drawCalls[i];
			if (uidrawCall != null)
			{
				UIDrawCall.Destroy(uidrawCall);
			}
			i++;
		}
		this.drawCalls.Clear();
		UIPanel.list.Remove(this);
		this.mAlphaFrameID = -1;
		this.mMatrixFrame = -1;
		if (UIPanel.list.Count == 0)
		{
			UIDrawCall.ReleaseAll();
			UIPanel.mUpdateFrame = -1;
		}
		base.OnDisable();
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x000F79D0 File Offset: 0x000F5BD0
	private void UpdateTransformMatrix()
	{
		int frameCount = Time.frameCount;
		if (this.mMatrixFrame != frameCount)
		{
			this.mMatrixFrame = frameCount;
			this.worldToLocal = base.cachedTransform.worldToLocalMatrix;
			Vector2 vector = this.GetViewSize() * 0.5f;
			float num = this.mClipOffset.x + this.mClipRange.x;
			float num2 = this.mClipOffset.y + this.mClipRange.y;
			this.mMin.x = num - vector.x;
			this.mMin.y = num2 - vector.y;
			this.mMax.x = num + vector.x;
			this.mMax.y = num2 + vector.y;
		}
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x000F7A98 File Offset: 0x000F5C98
	protected override void OnAnchor()
	{
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return;
		}
		Transform cachedTransform = base.cachedTransform;
		Transform parent = cachedTransform.parent;
		Vector2 viewSize = this.GetViewSize();
		Vector2 vector = cachedTransform.localPosition;
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
			}
			else
			{
				Vector2 vector2 = base.GetLocalPos(this.leftAnchor, parent);
				num = vector2.x + (float)this.leftAnchor.absolute;
				num3 = vector2.y + (float)this.bottomAnchor.absolute;
				num2 = vector2.x + (float)this.rightAnchor.absolute;
				num4 = vector2.y + (float)this.topAnchor.absolute;
			}
		}
		else
		{
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
				num = this.mClipRange.x - 0.5f * viewSize.x;
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
				num2 = this.mClipRange.x + 0.5f * viewSize.x;
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
				num3 = this.mClipRange.y - 0.5f * viewSize.y;
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
				num4 = this.mClipRange.y + 0.5f * viewSize.y;
			}
		}
		num -= vector.x + this.mClipOffset.x;
		num2 -= vector.x + this.mClipOffset.x;
		num3 -= vector.y + this.mClipOffset.y;
		num4 -= vector.y + this.mClipOffset.y;
		float num5 = Mathf.Lerp(num, num2, 0.5f);
		float num6 = Mathf.Lerp(num3, num4, 0.5f);
		float num7 = num2 - num;
		float num8 = num4 - num3;
		float num9 = Mathf.Max(2f, this.mClipSoftness.x);
		float num10 = Mathf.Max(2f, this.mClipSoftness.y);
		if (num7 < num9)
		{
			num7 = num9;
		}
		if (num8 < num10)
		{
			num8 = num10;
		}
		this.baseClipRegion = new Vector4(num5, num6, num7, num8);
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x000F8040 File Offset: 0x000F6240
	private void LateUpdate()
	{
		if (UIPanel.mUpdateFrame != Time.frameCount)
		{
			UIPanel.mUpdateFrame = Time.frameCount;
			int i = 0;
			int count = UIPanel.list.Count;
			while (i < count)
			{
				UIPanel.list[i].UpdateSelf();
				i++;
			}
			int num = 3000;
			int j = 0;
			int count2 = UIPanel.list.Count;
			while (j < count2)
			{
				UIPanel uipanel = UIPanel.list[j];
				if (uipanel.renderQueue == UIPanel.RenderQueue.Automatic)
				{
					uipanel.startingRenderQueue = num;
					uipanel.UpdateDrawCalls();
					num += uipanel.drawCalls.Count;
				}
				else if (uipanel.renderQueue == UIPanel.RenderQueue.StartAt)
				{
					uipanel.UpdateDrawCalls();
					if (uipanel.drawCalls.Count != 0)
					{
						num = Mathf.Max(num, uipanel.startingRenderQueue + uipanel.drawCalls.Count);
					}
				}
				else
				{
					uipanel.UpdateDrawCalls();
					if (uipanel.drawCalls.Count != 0)
					{
						num = Mathf.Max(num, uipanel.startingRenderQueue + 1);
					}
				}
				j++;
			}
		}
	}

	// Token: 0x060020D9 RID: 8409 RVA: 0x000F8164 File Offset: 0x000F6364
	private void UpdateSelf()
	{
		this.mUpdateTime = RealTime.time;
		this.UpdateTransformMatrix();
		this.UpdateLayers();
		this.UpdateWidgets();
		if (this.mRebuild)
		{
			this.mRebuild = false;
			this.FillAllDrawCalls();
		}
		else
		{
			int i = 0;
			while (i < this.drawCalls.Count)
			{
				UIDrawCall uidrawCall = this.drawCalls[i];
				if (uidrawCall.isDirty && !this.FillDrawCall(uidrawCall))
				{
					UIDrawCall.Destroy(uidrawCall);
					this.drawCalls.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
		if (this.mUpdateScroll)
		{
			this.mUpdateScroll = false;
			UIScrollView component = base.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars();
			}
		}
	}

	// Token: 0x060020DA RID: 8410 RVA: 0x00016021 File Offset: 0x00014221
	public void SortWidgets()
	{
		this.mSortWidgets = false;
		this.widgets.Sort(new Comparison<UIWidget>(UIWidget.PanelCompareFunc));
	}

	// Token: 0x060020DB RID: 8411 RVA: 0x000F8230 File Offset: 0x000F6430
	private void FillAllDrawCalls()
	{
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall.Destroy(this.drawCalls[i]);
		}
		this.drawCalls.Clear();
		Material material = null;
		Texture texture = null;
		Shader shader = null;
		UIDrawCall uidrawCall = null;
		int num = 0;
		if (this.mSortWidgets)
		{
			this.SortWidgets();
		}
		for (int j = 0; j < this.widgets.Count; j++)
		{
			UIWidget uiwidget = this.widgets[j];
			if (uiwidget.isVisible && uiwidget.hasVertices)
			{
				Material material2 = uiwidget.material;
				Texture mainTexture = uiwidget.mainTexture;
				Shader shader2 = uiwidget.shader;
				if (material != material2 || texture != mainTexture || shader != shader2)
				{
					if (uidrawCall != null && uidrawCall.verts.size != 0)
					{
						this.drawCalls.Add(uidrawCall);
						uidrawCall.UpdateGeometry(num);
						uidrawCall.onRender = this.mOnRender;
						this.mOnRender = null;
						num = 0;
						uidrawCall = null;
					}
					material = material2;
					texture = mainTexture;
					shader = shader2;
				}
				if (material != null || shader != null || texture != null)
				{
					if (uidrawCall == null)
					{
						uidrawCall = UIDrawCall.Create(this, material, texture, shader);
						uidrawCall.depthStart = uiwidget.depth;
						uidrawCall.depthEnd = uidrawCall.depthStart;
						uidrawCall.panel = this;
					}
					else
					{
						int depth = uiwidget.depth;
						if (depth < uidrawCall.depthStart)
						{
							uidrawCall.depthStart = depth;
						}
						if (depth > uidrawCall.depthEnd)
						{
							uidrawCall.depthEnd = depth;
						}
					}
					uiwidget.drawCall = uidrawCall;
					num++;
					if (this.generateNormals)
					{
						uiwidget.WriteToBuffers(uidrawCall.verts, uidrawCall.uvs, uidrawCall.cols, uidrawCall.norms, uidrawCall.tans);
					}
					else
					{
						uiwidget.WriteToBuffers(uidrawCall.verts, uidrawCall.uvs, uidrawCall.cols, null, null);
					}
					if (uiwidget.mOnRender != null)
					{
						if (this.mOnRender == null)
						{
							this.mOnRender = uiwidget.mOnRender;
						}
						else
						{
							this.mOnRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(this.mOnRender, uiwidget.mOnRender);
						}
					}
				}
			}
			else
			{
				uiwidget.drawCall = null;
			}
		}
		if (uidrawCall != null && uidrawCall.verts.size != 0)
		{
			this.drawCalls.Add(uidrawCall);
			uidrawCall.UpdateGeometry(num);
			uidrawCall.onRender = this.mOnRender;
			this.mOnRender = null;
		}
	}

	// Token: 0x060020DC RID: 8412 RVA: 0x000F8510 File Offset: 0x000F6710
	private bool FillDrawCall(UIDrawCall dc)
	{
		if (dc != null)
		{
			dc.isDirty = false;
			int num = 0;
			int i = 0;
			while (i < this.widgets.Count)
			{
				UIWidget uiwidget = this.widgets[i];
				if (uiwidget == null)
				{
					this.widgets.RemoveAt(i);
				}
				else
				{
					if (uiwidget.drawCall == dc)
					{
						if (uiwidget.isVisible && uiwidget.hasVertices)
						{
							num++;
							if (this.generateNormals)
							{
								uiwidget.WriteToBuffers(dc.verts, dc.uvs, dc.cols, dc.norms, dc.tans);
							}
							else
							{
								uiwidget.WriteToBuffers(dc.verts, dc.uvs, dc.cols, null, null);
							}
							if (uiwidget.mOnRender != null)
							{
								if (this.mOnRender == null)
								{
									this.mOnRender = uiwidget.mOnRender;
								}
								else
								{
									this.mOnRender = (UIDrawCall.OnRenderCallback)Delegate.Combine(this.mOnRender, uiwidget.mOnRender);
								}
							}
						}
						else
						{
							uiwidget.drawCall = null;
						}
					}
					i++;
				}
			}
			if (dc.verts.size != 0)
			{
				dc.UpdateGeometry(num);
				dc.onRender = this.mOnRender;
				this.mOnRender = null;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x000F8670 File Offset: 0x000F6870
	private void UpdateDrawCalls()
	{
		Transform cachedTransform = base.cachedTransform;
		bool usedForUI = this.usedForUI;
		if (this.clipping != UIDrawCall.Clipping.None)
		{
			this.drawCallClipRange = this.finalClipRegion;
			this.drawCallClipRange.z = this.drawCallClipRange.z * 0.5f;
			this.drawCallClipRange.w = this.drawCallClipRange.w * 0.5f;
		}
		else
		{
			this.drawCallClipRange = Vector4.zero;
		}
		if (this.drawCallClipRange.z == 0f)
		{
			this.drawCallClipRange.z = (float)Screen.width * 0.5f;
		}
		if (this.drawCallClipRange.w == 0f)
		{
			this.drawCallClipRange.w = (float)Screen.height * 0.5f;
		}
		if (this.halfPixelOffset)
		{
			this.drawCallClipRange.x = this.drawCallClipRange.x - 0.5f;
			this.drawCallClipRange.y = this.drawCallClipRange.y + 0.5f;
		}
		Vector3 vector;
		if (usedForUI)
		{
			Transform parent = base.cachedTransform.parent;
			vector = base.cachedTransform.localPosition;
			if (parent != null)
			{
				vector = parent.TransformPoint(vector);
			}
			vector += this.drawCallOffset;
		}
		else
		{
			vector = cachedTransform.position;
		}
		Quaternion rotation = cachedTransform.rotation;
		Vector3 lossyScale = cachedTransform.lossyScale;
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall uidrawCall = this.drawCalls[i];
			Transform cachedTransform2 = uidrawCall.cachedTransform;
			cachedTransform2.position = vector;
			cachedTransform2.rotation = rotation;
			cachedTransform2.localScale = lossyScale;
			uidrawCall.renderQueue = ((this.renderQueue != UIPanel.RenderQueue.Explicit) ? (this.startingRenderQueue + i) : this.startingRenderQueue);
			uidrawCall.alwaysOnScreen = (this.alwaysOnScreen && (this.mClipping == UIDrawCall.Clipping.None || this.mClipping == UIDrawCall.Clipping.ConstrainButDontClip));
			uidrawCall.sortingOrder = this.mSortingOrder;
			uidrawCall.clipTexture = this.mClipTexture;
		}
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000F888C File Offset: 0x000F6A8C
	private void UpdateLayers()
	{
		if (this.mLayer != base.cachedGameObject.layer)
		{
			this.mLayer = this.mGo.layer;
			NGUITools.SetChildLayer(base.cachedTransform, this.mLayer);
			base.ResetAnchors();
			for (int i = 0; i < this.drawCalls.Count; i++)
			{
				this.drawCalls[i].gameObject.layer = this.mLayer;
			}
		}
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x000F8910 File Offset: 0x000F6B10
	private void UpdateWidgets()
	{
		bool flag = !this.cullWhileDragging && this.mCullTime > this.mUpdateTime;
		bool flag2 = false;
		if (this.mForced != flag)
		{
			this.mForced = flag;
			this.mResized = true;
		}
		bool hasCumulativeClipping = this.hasCumulativeClipping;
		int i = 0;
		int count = this.widgets.Count;
		while (i < count)
		{
			UIWidget uiwidget = this.widgets[i];
			if (uiwidget.panel == this && uiwidget.enabled)
			{
				int frameCount = Time.frameCount;
				if (uiwidget.UpdateTransform(frameCount) || this.mResized)
				{
					bool visibleByAlpha = flag || uiwidget.CalculateCumulativeAlpha(frameCount) > 0.001f;
					uiwidget.UpdateVisibility(visibleByAlpha, flag || (!hasCumulativeClipping && !uiwidget.hideIfOffScreen) || this.IsVisible(uiwidget));
				}
				if (uiwidget.UpdateGeometry(frameCount))
				{
					flag2 = true;
					if (!this.mRebuild)
					{
						if (uiwidget.drawCall != null)
						{
							uiwidget.drawCall.isDirty = true;
						}
						else
						{
							this.FindDrawCall(uiwidget);
						}
					}
				}
			}
			i++;
		}
		if (flag2 && this.onGeometryUpdated != null)
		{
			this.onGeometryUpdated();
		}
		this.mResized = false;
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x000F8A88 File Offset: 0x000F6C88
	public UIDrawCall FindDrawCall(UIWidget w)
	{
		Material material = w.material;
		Texture mainTexture = w.mainTexture;
		int depth = w.depth;
		for (int i = 0; i < this.drawCalls.Count; i++)
		{
			UIDrawCall uidrawCall = this.drawCalls[i];
			int num = (i != 0) ? (this.drawCalls[i - 1].depthEnd + 1) : int.MinValue;
			int num2 = (i + 1 != this.drawCalls.Count) ? (this.drawCalls[i + 1].depthStart - 1) : int.MaxValue;
			if (num <= depth && num2 >= depth)
			{
				if (uidrawCall.baseMaterial == material && uidrawCall.mainTexture == mainTexture)
				{
					if (w.isVisible)
					{
						w.drawCall = uidrawCall;
						if (w.hasVertices)
						{
							uidrawCall.isDirty = true;
						}
						return uidrawCall;
					}
				}
				else
				{
					this.mRebuild = true;
				}
				return null;
			}
		}
		this.mRebuild = true;
		return null;
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x000F8BA4 File Offset: 0x000F6DA4
	public void AddWidget(UIWidget w)
	{
		this.mUpdateScroll = true;
		if (this.widgets.Count == 0)
		{
			this.widgets.Add(w);
		}
		else if (this.mSortWidgets)
		{
			this.widgets.Add(w);
			this.SortWidgets();
		}
		else if (UIWidget.PanelCompareFunc(w, this.widgets[0]) == -1)
		{
			this.widgets.Insert(0, w);
		}
		else
		{
			int i = this.widgets.Count;
			while (i > 0)
			{
				if (UIWidget.PanelCompareFunc(w, this.widgets[--i]) != -1)
				{
					this.widgets.Insert(i + 1, w);
					break;
				}
			}
		}
		this.FindDrawCall(w);
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x000F8C7C File Offset: 0x000F6E7C
	public void RemoveWidget(UIWidget w)
	{
		if (this.widgets.Remove(w) && w.drawCall != null)
		{
			int depth = w.depth;
			if (depth == w.drawCall.depthStart || depth == w.drawCall.depthEnd)
			{
				this.mRebuild = true;
			}
			w.drawCall.isDirty = true;
			w.drawCall = null;
		}
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x00016041 File Offset: 0x00014241
	public void Refresh()
	{
		this.mRebuild = true;
		UIPanel.mUpdateFrame = -1;
		if (UIPanel.list.Count > 0)
		{
			UIPanel.list[0].LateUpdate();
		}
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x000F8CF0 File Offset: 0x000F6EF0
	public virtual Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		Vector4 finalClipRegion = this.finalClipRegion;
		float num = finalClipRegion.z * 0.5f;
		float num2 = finalClipRegion.w * 0.5f;
		Vector2 minRect;
		minRect..ctor(min.x, min.y);
		Vector2 maxRect;
		maxRect..ctor(max.x, max.y);
		Vector2 minArea;
		minArea..ctor(finalClipRegion.x - num, finalClipRegion.y - num2);
		Vector2 maxArea;
		maxArea..ctor(finalClipRegion.x + num, finalClipRegion.y + num2);
		if (this.softBorderPadding && this.clipping == UIDrawCall.Clipping.SoftClip)
		{
			minArea.x += this.mClipSoftness.x;
			minArea.y += this.mClipSoftness.y;
			maxArea.x -= this.mClipSoftness.x;
			maxArea.y -= this.mClipSoftness.y;
		}
		return NGUIMath.ConstrainRect(minRect, maxRect, minArea, maxArea);
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x000F8E08 File Offset: 0x000F7008
	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		Vector3 vector = targetBounds.min;
		Vector3 vector2 = targetBounds.max;
		float num = 1f;
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				num = root.pixelSizeAdjustment;
			}
		}
		if (num != 1f)
		{
			vector /= num;
			vector2 /= num;
		}
		Vector3 vector3 = this.CalculateConstrainOffset(vector, vector2) * num;
		if (vector3.sqrMagnitude > 0f)
		{
			if (immediate)
			{
				target.localPosition += vector3;
				targetBounds.center += vector3;
				SpringPosition component = target.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else
			{
				SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + vector3, 13f);
				springPosition.ignoreTimeScale = true;
				springPosition.worldSpace = false;
			}
			return true;
		}
		return false;
	}

	// Token: 0x060020E6 RID: 8422 RVA: 0x000F8F14 File Offset: 0x000F7114
	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.cachedTransform, target);
		return this.ConstrainTargetToBounds(target, ref bounds, immediate);
	}

	// Token: 0x060020E7 RID: 8423 RVA: 0x00016070 File Offset: 0x00014270
	public static UIPanel Find(Transform trans)
	{
		return UIPanel.Find(trans, false, -1);
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x0001607A File Offset: 0x0001427A
	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		return UIPanel.Find(trans, createIfMissing, -1);
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x000F8F38 File Offset: 0x000F7138
	public static UIPanel Find(Transform trans, bool createIfMissing, int layer)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(trans);
		if (uipanel != null)
		{
			return uipanel;
		}
		return (!createIfMissing) ? null : NGUITools.CreateUI(trans, false, layer);
	}

	// Token: 0x060020EA RID: 8426 RVA: 0x000F8F70 File Offset: 0x000F7170
	private Vector2 GetWindowSize()
	{
		UIRoot root = base.root;
		Vector2 vector = NGUITools.screenSize;
		if (root != null)
		{
			vector *= root.GetPixelSizeAdjustment(Mathf.RoundToInt(vector.y));
		}
		return vector;
	}

	// Token: 0x060020EB RID: 8427 RVA: 0x000F8FB0 File Offset: 0x000F71B0
	public Vector2 GetViewSize()
	{
		if (this.mClipping != UIDrawCall.Clipping.None)
		{
			return new Vector2(this.mClipRange.z, this.mClipRange.w);
		}
		return NGUITools.screenSize;
	}

	// Token: 0x040023F7 RID: 9207
	public static List<UIPanel> list = new List<UIPanel>();

	// Token: 0x040023F8 RID: 9208
	public UIPanel.OnGeometryUpdated onGeometryUpdated;

	// Token: 0x040023F9 RID: 9209
	public bool showInPanelTool = true;

	// Token: 0x040023FA RID: 9210
	public bool generateNormals;

	// Token: 0x040023FB RID: 9211
	public bool widgetsAreStatic;

	// Token: 0x040023FC RID: 9212
	public bool cullWhileDragging = true;

	// Token: 0x040023FD RID: 9213
	public bool alwaysOnScreen;

	// Token: 0x040023FE RID: 9214
	public bool anchorOffset;

	// Token: 0x040023FF RID: 9215
	public bool softBorderPadding = true;

	// Token: 0x04002400 RID: 9216
	public UIPanel.RenderQueue renderQueue;

	// Token: 0x04002401 RID: 9217
	public int startingRenderQueue = 3000;

	// Token: 0x04002402 RID: 9218
	[NonSerialized]
	public List<UIWidget> widgets = new List<UIWidget>();

	// Token: 0x04002403 RID: 9219
	[NonSerialized]
	public List<UIDrawCall> drawCalls = new List<UIDrawCall>();

	// Token: 0x04002404 RID: 9220
	[NonSerialized]
	public Matrix4x4 worldToLocal = Matrix4x4.identity;

	// Token: 0x04002405 RID: 9221
	[NonSerialized]
	public Vector4 drawCallClipRange = new Vector4(0f, 0f, 1f, 1f);

	// Token: 0x04002406 RID: 9222
	public UIPanel.OnClippingMoved onClipMove;

	// Token: 0x04002407 RID: 9223
	[SerializeField]
	[HideInInspector]
	private Texture2D mClipTexture;

	// Token: 0x04002408 RID: 9224
	[SerializeField]
	[HideInInspector]
	private float mAlpha = 1f;

	// Token: 0x04002409 RID: 9225
	[SerializeField]
	[HideInInspector]
	private UIDrawCall.Clipping mClipping;

	// Token: 0x0400240A RID: 9226
	[HideInInspector]
	[SerializeField]
	private Vector4 mClipRange = new Vector4(0f, 0f, 300f, 200f);

	// Token: 0x0400240B RID: 9227
	[SerializeField]
	[HideInInspector]
	private Vector2 mClipSoftness = new Vector2(4f, 4f);

	// Token: 0x0400240C RID: 9228
	[HideInInspector]
	[SerializeField]
	private int mDepth;

	// Token: 0x0400240D RID: 9229
	[HideInInspector]
	[SerializeField]
	private int mSortingOrder;

	// Token: 0x0400240E RID: 9230
	private bool mRebuild;

	// Token: 0x0400240F RID: 9231
	private bool mResized;

	// Token: 0x04002410 RID: 9232
	[SerializeField]
	private Vector2 mClipOffset = Vector2.zero;

	// Token: 0x04002411 RID: 9233
	private float mCullTime;

	// Token: 0x04002412 RID: 9234
	private float mUpdateTime;

	// Token: 0x04002413 RID: 9235
	private int mMatrixFrame = -1;

	// Token: 0x04002414 RID: 9236
	private int mAlphaFrameID;

	// Token: 0x04002415 RID: 9237
	private int mLayer = -1;

	// Token: 0x04002416 RID: 9238
	private static float[] mTemp = new float[4];

	// Token: 0x04002417 RID: 9239
	private Vector2 mMin = Vector2.zero;

	// Token: 0x04002418 RID: 9240
	private Vector2 mMax = Vector2.zero;

	// Token: 0x04002419 RID: 9241
	private bool mHalfPixelOffset;

	// Token: 0x0400241A RID: 9242
	private bool mSortWidgets;

	// Token: 0x0400241B RID: 9243
	private bool mUpdateScroll;

	// Token: 0x0400241C RID: 9244
	private UIPanel mParentPanel;

	// Token: 0x0400241D RID: 9245
	private static Vector3[] mCorners = new Vector3[4];

	// Token: 0x0400241E RID: 9246
	private static int mUpdateFrame = -1;

	// Token: 0x0400241F RID: 9247
	private UIDrawCall.OnRenderCallback mOnRender;

	// Token: 0x04002420 RID: 9248
	private bool mForced;

	// Token: 0x02000501 RID: 1281
	public enum RenderQueue
	{
		// Token: 0x04002422 RID: 9250
		Automatic,
		// Token: 0x04002423 RID: 9251
		StartAt,
		// Token: 0x04002424 RID: 9252
		Explicit
	}

	// Token: 0x02000502 RID: 1282
	// (Invoke) Token: 0x060020ED RID: 8429
	public delegate void OnGeometryUpdated();

	// Token: 0x02000503 RID: 1283
	// (Invoke) Token: 0x060020F1 RID: 8433
	public delegate void OnClippingMoved(UIPanel panel);
}
