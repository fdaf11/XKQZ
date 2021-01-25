using System;
using UnityEngine;

// Token: 0x0200047B RID: 1147
[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Interaction/Scroll View")]
[ExecuteInEditMode]
public class UIScrollView : MonoBehaviour
{
	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06001BB5 RID: 7093 RVA: 0x00012818 File Offset: 0x00010A18
	public UIPanel panel
	{
		get
		{
			return this.mPanel;
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x00012820 File Offset: 0x00010A20
	public bool isDragging
	{
		get
		{
			return this.mPressed && this.mDragStarted;
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x00012836 File Offset: 0x00010A36
	public virtual Bounds bounds
	{
		get
		{
			if (!this.mCalculatedBounds)
			{
				this.mCalculatedBounds = true;
				this.mTrans = base.transform;
				this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans);
			}
			return this.mBounds;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x00012873 File Offset: 0x00010A73
	public bool canMoveHorizontally
	{
		get
		{
			return this.movement == UIScrollView.Movement.Horizontal || this.movement == UIScrollView.Movement.Unrestricted || (this.movement == UIScrollView.Movement.Custom && this.customMovement.x != 0f);
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x000D892C File Offset: 0x000D6B2C
	public bool canMoveVertically
	{
		get
		{
			return this.movement == UIScrollView.Movement.Vertical || this.movement == UIScrollView.Movement.Unrestricted || (this.movement == UIScrollView.Movement.Custom && this.customMovement.y != 0f);
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06001BBA RID: 7098 RVA: 0x000D8978 File Offset: 0x000D6B78
	public virtual bool shouldMoveHorizontally
	{
		get
		{
			float num = this.bounds.size.x;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += this.mPanel.clipSoftness.x * 2f;
			}
			return Mathf.RoundToInt(num - this.mPanel.width) > 0;
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06001BBB RID: 7099 RVA: 0x000D89E0 File Offset: 0x000D6BE0
	public virtual bool shouldMoveVertically
	{
		get
		{
			float num = this.bounds.size.y;
			if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
			{
				num += this.mPanel.clipSoftness.y * 2f;
			}
			return Mathf.RoundToInt(num - this.mPanel.height) > 0;
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06001BBC RID: 7100 RVA: 0x000D8A48 File Offset: 0x000D6C48
	protected virtual bool shouldMove
	{
		get
		{
			if (!this.disableDragIfFits)
			{
				return true;
			}
			if (this.mPanel == null)
			{
				this.mPanel = base.GetComponent<UIPanel>();
			}
			Vector4 finalClipRegion = this.mPanel.finalClipRegion;
			Bounds bounds = this.bounds;
			float num = (finalClipRegion.z != 0f) ? (finalClipRegion.z * 0.5f) : ((float)Screen.width);
			float num2 = (finalClipRegion.w != 0f) ? (finalClipRegion.w * 0.5f) : ((float)Screen.height);
			if (this.canMoveHorizontally)
			{
				if (bounds.min.x < finalClipRegion.x - num)
				{
					return true;
				}
				if (bounds.max.x > finalClipRegion.x + num)
				{
					return true;
				}
			}
			if (this.canMoveVertically)
			{
				if (bounds.min.y < finalClipRegion.y - num2)
				{
					return true;
				}
				if (bounds.max.y > finalClipRegion.y + num2)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06001BBD RID: 7101 RVA: 0x000128B3 File Offset: 0x00010AB3
	// (set) Token: 0x06001BBE RID: 7102 RVA: 0x000128BB File Offset: 0x00010ABB
	public Vector3 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
			this.mShouldMove = true;
		}
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x000D8B80 File Offset: 0x000D6D80
	private void Awake()
	{
		this.mTrans = base.transform;
		this.mPanel = base.GetComponent<UIPanel>();
		if (this.mPanel.clipping == UIDrawCall.Clipping.None)
		{
			this.mPanel.clipping = UIDrawCall.Clipping.ConstrainButDontClip;
		}
		if (this.movement != UIScrollView.Movement.Custom && this.scale.sqrMagnitude > 0.001f)
		{
			if (this.scale.x == 1f && this.scale.y == 0f)
			{
				this.movement = UIScrollView.Movement.Horizontal;
			}
			else if (this.scale.x == 0f && this.scale.y == 1f)
			{
				this.movement = UIScrollView.Movement.Vertical;
			}
			else if (this.scale.x == 1f && this.scale.y == 1f)
			{
				this.movement = UIScrollView.Movement.Unrestricted;
			}
			else
			{
				this.movement = UIScrollView.Movement.Custom;
				this.customMovement.x = this.scale.x;
				this.customMovement.y = this.scale.y;
			}
			this.scale = Vector3.zero;
		}
		if (this.contentPivot == UIWidget.Pivot.TopLeft && this.relativePositionOnReset != Vector2.zero)
		{
			this.contentPivot = NGUIMath.GetPivot(new Vector2(this.relativePositionOnReset.x, 1f - this.relativePositionOnReset.y));
			this.relativePositionOnReset = Vector2.zero;
		}
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x000D8D1C File Offset: 0x000D6F1C
	private void OnEnable()
	{
		UIScrollView.list.Add(this);
		if (Application.isPlaying)
		{
			if (this.horizontalScrollBar != null)
			{
				EventDelegate.Add(this.horizontalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
				this.horizontalScrollBar.alpha = ((this.showScrollBars != UIScrollView.ShowCondition.Always && !this.shouldMoveHorizontally) ? 0f : 1f);
			}
			if (this.verticalScrollBar != null)
			{
				EventDelegate.Add(this.verticalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
				this.verticalScrollBar.alpha = ((this.showScrollBars != UIScrollView.ShowCondition.Always && !this.shouldMoveVertically) ? 0f : 1f);
			}
		}
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x000128CB File Offset: 0x00010ACB
	private void OnDisable()
	{
		UIScrollView.list.Remove(this);
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x000128D9 File Offset: 0x00010AD9
	public bool RestrictWithinBounds(bool instant)
	{
		return this.RestrictWithinBounds(instant, true, true);
	}

	// Token: 0x06001BC3 RID: 7107 RVA: 0x000D8DFC File Offset: 0x000D6FFC
	public bool RestrictWithinBounds(bool instant, bool horizontal, bool vertical)
	{
		Bounds bounds = this.bounds;
		Vector3 vector = this.mPanel.CalculateConstrainOffset(bounds.min, bounds.max);
		if (!horizontal)
		{
			vector.x = 0f;
		}
		if (!vertical)
		{
			vector.y = 0f;
		}
		if (vector.sqrMagnitude > 0.1f)
		{
			if (!instant && this.dragEffect == UIScrollView.DragEffect.MomentumAndSpring)
			{
				Vector3 pos = this.mTrans.localPosition + vector;
				pos.x = Mathf.Round(pos.x);
				pos.y = Mathf.Round(pos.y);
				SpringPanel.Begin(this.mPanel.gameObject, pos, 13f).strength = 8f;
			}
			else
			{
				this.MoveRelative(vector);
				if (Mathf.Abs(vector.x) > 0.01f)
				{
					this.mMomentum.x = 0f;
				}
				if (Mathf.Abs(vector.y) > 0.01f)
				{
					this.mMomentum.y = 0f;
				}
				if (Mathf.Abs(vector.z) > 0.01f)
				{
					this.mMomentum.z = 0f;
				}
				this.mScroll = 0f;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001BC4 RID: 7108 RVA: 0x000D8F60 File Offset: 0x000D7160
	public void DisableSpring()
	{
		SpringPanel component = base.GetComponent<SpringPanel>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x000128E4 File Offset: 0x00010AE4
	public void UpdateScrollbars()
	{
		this.UpdateScrollbars(true);
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x000D8F88 File Offset: 0x000D7188
	public virtual void UpdateScrollbars(bool recalculateBounds)
	{
		if (this.mPanel == null)
		{
			return;
		}
		if (this.horizontalScrollBar != null || this.verticalScrollBar != null)
		{
			if (recalculateBounds)
			{
				this.mCalculatedBounds = false;
				this.mShouldMove = this.shouldMove;
			}
			Bounds bounds = this.bounds;
			Vector2 vector = bounds.min;
			Vector2 vector2 = bounds.max;
			if (this.horizontalScrollBar != null && vector2.x > vector.x)
			{
				Vector4 finalClipRegion = this.mPanel.finalClipRegion;
				int num = Mathf.RoundToInt(finalClipRegion.z);
				if ((num & 1) != 0)
				{
					num--;
				}
				float num2 = (float)num * 0.5f;
				num2 = Mathf.Round(num2);
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num2 -= this.mPanel.clipSoftness.x;
				}
				float contentSize = vector2.x - vector.x;
				float viewSize = num2 * 2f;
				float num3 = vector.x;
				float num4 = vector2.x;
				float num5 = finalClipRegion.x - num2;
				float num6 = finalClipRegion.x + num2;
				num3 = num5 - num3;
				num4 -= num6;
				this.UpdateScrollbars(this.horizontalScrollBar, num3, num4, contentSize, viewSize, false);
			}
			if (this.verticalScrollBar != null && vector2.y > vector.y)
			{
				Vector4 finalClipRegion2 = this.mPanel.finalClipRegion;
				int num7 = Mathf.RoundToInt(finalClipRegion2.w);
				if ((num7 & 1) != 0)
				{
					num7--;
				}
				float num8 = (float)num7 * 0.5f;
				num8 = Mathf.Round(num8);
				if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
				{
					num8 -= this.mPanel.clipSoftness.y;
				}
				float contentSize2 = vector2.y - vector.y;
				float viewSize2 = num8 * 2f;
				float num9 = vector.y;
				float num10 = vector2.y;
				float num11 = finalClipRegion2.y - num8;
				float num12 = finalClipRegion2.y + num8;
				num9 = num11 - num9;
				num10 -= num12;
				this.UpdateScrollbars(this.verticalScrollBar, num9, num10, contentSize2, viewSize2, true);
			}
		}
		else if (recalculateBounds)
		{
			this.mCalculatedBounds = false;
		}
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x000D91FC File Offset: 0x000D73FC
	protected void UpdateScrollbars(UIProgressBar slider, float contentMin, float contentMax, float contentSize, float viewSize, bool inverted)
	{
		if (slider == null)
		{
			return;
		}
		this.mIgnoreCallbacks = true;
		float num;
		if (viewSize < contentSize)
		{
			contentMin = Mathf.Clamp01(contentMin / contentSize);
			contentMax = Mathf.Clamp01(contentMax / contentSize);
			num = contentMin + contentMax;
			slider.value = ((!inverted) ? ((num <= 0.001f) ? 1f : (contentMin / num)) : ((num <= 0.001f) ? 0f : (1f - contentMin / num)));
		}
		else
		{
			contentMin = Mathf.Clamp01(-contentMin / contentSize);
			contentMax = Mathf.Clamp01(-contentMax / contentSize);
			num = contentMin + contentMax;
			slider.value = ((!inverted) ? ((num <= 0.001f) ? 1f : (contentMin / num)) : ((num <= 0.001f) ? 0f : (1f - contentMin / num)));
			if (contentSize > 0f)
			{
				contentMin = Mathf.Clamp01(contentMin / contentSize);
				contentMax = Mathf.Clamp01(contentMax / contentSize);
				num = contentMin + contentMax;
			}
		}
		UIScrollBar uiscrollBar = slider as UIScrollBar;
		if (uiscrollBar != null)
		{
			uiscrollBar.barSize = 1f - num;
		}
		this.mIgnoreCallbacks = false;
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x000D9340 File Offset: 0x000D7540
	public virtual void SetDragAmount(float x, float y, bool updateScrollbars)
	{
		if (this.mPanel == null)
		{
			this.mPanel = base.GetComponent<UIPanel>();
		}
		this.DisableSpring();
		Bounds bounds = this.bounds;
		if (bounds.min.x == bounds.max.x || bounds.min.y == bounds.max.y)
		{
			return;
		}
		Vector4 finalClipRegion = this.mPanel.finalClipRegion;
		float num = finalClipRegion.z * 0.5f;
		float num2 = finalClipRegion.w * 0.5f;
		float num3 = bounds.min.x + num;
		float num4 = bounds.max.x - num;
		float num5 = bounds.min.y + num2;
		float num6 = bounds.max.y - num2;
		if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			num3 -= this.mPanel.clipSoftness.x;
			num4 += this.mPanel.clipSoftness.x;
			num5 -= this.mPanel.clipSoftness.y;
			num6 += this.mPanel.clipSoftness.y;
		}
		float num7 = Mathf.Lerp(num3, num4, x);
		float num8 = Mathf.Lerp(num6, num5, y);
		if (!updateScrollbars)
		{
			Vector3 localPosition = this.mTrans.localPosition;
			if (this.canMoveHorizontally)
			{
				localPosition.x += finalClipRegion.x - num7;
			}
			if (this.canMoveVertically)
			{
				localPosition.y += finalClipRegion.y - num8;
			}
			this.mTrans.localPosition = localPosition;
		}
		if (this.canMoveHorizontally)
		{
			finalClipRegion.x = num7;
		}
		if (this.canMoveVertically)
		{
			finalClipRegion.y = num8;
		}
		Vector4 baseClipRegion = this.mPanel.baseClipRegion;
		this.mPanel.clipOffset = new Vector2(finalClipRegion.x - baseClipRegion.x, finalClipRegion.y - baseClipRegion.y);
		if (updateScrollbars)
		{
			this.UpdateScrollbars(this.mDragID == -10);
		}
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x000128ED File Offset: 0x00010AED
	public void InvalidateBounds()
	{
		this.mCalculatedBounds = false;
	}

	// Token: 0x06001BCA RID: 7114 RVA: 0x000D95A8 File Offset: 0x000D77A8
	[ContextMenu("Reset Clipping Position")]
	public void ResetPosition()
	{
		if (NGUITools.GetActive(this))
		{
			this.mCalculatedBounds = false;
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.contentPivot);
			this.SetDragAmount(pivotOffset.x, 1f - pivotOffset.y, false);
			this.SetDragAmount(pivotOffset.x, 1f - pivotOffset.y, true);
		}
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x000D960C File Offset: 0x000D780C
	public void UpdatePosition()
	{
		if (!this.mIgnoreCallbacks && (this.horizontalScrollBar != null || this.verticalScrollBar != null))
		{
			this.mIgnoreCallbacks = true;
			this.mCalculatedBounds = false;
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.contentPivot);
			float x = (!(this.horizontalScrollBar != null)) ? pivotOffset.x : this.horizontalScrollBar.value;
			float y = (!(this.verticalScrollBar != null)) ? (1f - pivotOffset.y) : this.verticalScrollBar.value;
			this.SetDragAmount(x, y, false);
			this.UpdateScrollbars(true);
			this.mIgnoreCallbacks = false;
		}
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x000D96D0 File Offset: 0x000D78D0
	public void OnScrollBar()
	{
		if (!this.mIgnoreCallbacks)
		{
			this.mIgnoreCallbacks = true;
			float x = (!(this.horizontalScrollBar != null)) ? 0f : this.horizontalScrollBar.value;
			float y = (!(this.verticalScrollBar != null)) ? 0f : this.verticalScrollBar.value;
			this.SetDragAmount(x, y, false);
			this.mIgnoreCallbacks = false;
		}
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x000D9750 File Offset: 0x000D7950
	public virtual void MoveRelative(Vector3 relative)
	{
		this.mTrans.localPosition += relative;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= relative.x;
		clipOffset.y -= relative.y;
		this.mPanel.clipOffset = clipOffset;
		this.UpdateScrollbars(false);
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x000D97C0 File Offset: 0x000D79C0
	public void MoveAbsolute(Vector3 absolute)
	{
		Vector3 vector = this.mTrans.InverseTransformPoint(absolute);
		Vector3 vector2 = this.mTrans.InverseTransformPoint(Vector3.zero);
		this.MoveRelative(vector - vector2);
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x000D97F8 File Offset: 0x000D79F8
	public void Press(bool pressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		if (this.smoothDragStart && pressed)
		{
			this.mDragStarted = false;
			this.mDragStartOffset = Vector2.zero;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (!pressed && this.mDragID == UICamera.currentTouchID)
			{
				this.mDragID = -10;
			}
			this.mCalculatedBounds = false;
			this.mShouldMove = this.shouldMove;
			if (!this.mShouldMove)
			{
				return;
			}
			this.mPressed = pressed;
			if (pressed)
			{
				this.mMomentum = Vector3.zero;
				this.mScroll = 0f;
				this.DisableSpring();
				this.mLastPos = UICamera.lastWorldPosition;
				this.mPlane = new Plane(this.mTrans.rotation * Vector3.back, this.mLastPos);
				Vector2 clipOffset = this.mPanel.clipOffset;
				clipOffset.x = Mathf.Round(clipOffset.x);
				clipOffset.y = Mathf.Round(clipOffset.y);
				this.mPanel.clipOffset = clipOffset;
				Vector3 localPosition = this.mTrans.localPosition;
				localPosition.x = Mathf.Round(localPosition.x);
				localPosition.y = Mathf.Round(localPosition.y);
				this.mTrans.localPosition = localPosition;
				if (!this.smoothDragStart)
				{
					this.mDragStarted = true;
					this.mDragStartOffset = Vector2.zero;
					if (this.onDragStarted != null)
					{
						this.onDragStarted();
					}
				}
			}
			else
			{
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
				{
					this.RestrictWithinBounds(this.dragEffect == UIScrollView.DragEffect.None, this.canMoveHorizontally, this.canMoveVertically);
				}
				if (this.mDragStarted && this.onDragFinished != null)
				{
					this.onDragFinished();
				}
				if (!this.mShouldMove && this.onStoppedMoving != null)
				{
					this.onStoppedMoving();
				}
			}
		}
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x000D9A14 File Offset: 0x000D7C14
	public void Drag()
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.mShouldMove)
		{
			if (this.mDragID == -10)
			{
				this.mDragID = UICamera.currentTouchID;
			}
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			if (this.smoothDragStart && !this.mDragStarted)
			{
				this.mDragStarted = true;
				this.mDragStartOffset = UICamera.currentTouch.totalDelta;
				if (this.onDragStarted != null)
				{
					this.onDragStarted();
				}
			}
			Ray ray = (!this.smoothDragStart) ? UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos - this.mDragStartOffset);
			float num = 0f;
			if (this.mPlane.Raycast(ray, ref num))
			{
				Vector3 point = ray.GetPoint(num);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (vector.x != 0f || vector.y != 0f || vector.z != 0f)
				{
					vector = this.mTrans.InverseTransformDirection(vector);
					if (this.movement == UIScrollView.Movement.Horizontal)
					{
						vector.y = 0f;
						vector.z = 0f;
					}
					else if (this.movement == UIScrollView.Movement.Vertical)
					{
						vector.x = 0f;
						vector.z = 0f;
					}
					else if (this.movement == UIScrollView.Movement.Unrestricted)
					{
						vector.z = 0f;
					}
					else
					{
						vector.Scale(this.customMovement);
					}
					vector = this.mTrans.TransformDirection(vector);
				}
				if (this.dragEffect == UIScrollView.DragEffect.None)
				{
					this.mMomentum = Vector3.zero;
				}
				else
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				}
				if (!this.iOSDragEmulation || this.dragEffect != UIScrollView.DragEffect.MomentumAndSpring)
				{
					this.MoveAbsolute(vector);
				}
				else if (this.mPanel.CalculateConstrainOffset(this.bounds.min, this.bounds.max).magnitude > 1f)
				{
					this.MoveAbsolute(vector * 0.5f);
					this.mMomentum *= 0.5f;
				}
				else
				{
					this.MoveAbsolute(vector);
				}
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None && this.dragEffect != UIScrollView.DragEffect.MomentumAndSpring)
				{
					this.RestrictWithinBounds(true, this.canMoveHorizontally, this.canMoveVertically);
				}
			}
		}
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x000D9D30 File Offset: 0x000D7F30
	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.scrollWheelFactor != 0f)
		{
			this.DisableSpring();
			this.mShouldMove |= this.shouldMove;
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			this.mScroll += delta * this.scrollWheelFactor;
		}
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x000D9DB8 File Offset: 0x000D7FB8
	private void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		if (this.showScrollBars != UIScrollView.ShowCondition.Always && (this.verticalScrollBar || this.horizontalScrollBar))
		{
			bool flag = false;
			bool flag2 = false;
			if (this.showScrollBars != UIScrollView.ShowCondition.WhenDragging || this.mDragID != -10 || this.mMomentum.magnitude > 0.01f)
			{
				flag = this.shouldMoveVertically;
				flag2 = this.shouldMoveHorizontally;
			}
			if (this.verticalScrollBar)
			{
				float num = this.verticalScrollBar.alpha;
				num += ((!flag) ? (-deltaTime * 3f) : (deltaTime * 6f));
				num = Mathf.Clamp01(num);
				if (this.verticalScrollBar.alpha != num)
				{
					this.verticalScrollBar.alpha = num;
				}
			}
			if (this.horizontalScrollBar)
			{
				float num2 = this.horizontalScrollBar.alpha;
				num2 += ((!flag2) ? (-deltaTime * 3f) : (deltaTime * 6f));
				num2 = Mathf.Clamp01(num2);
				if (this.horizontalScrollBar.alpha != num2)
				{
					this.horizontalScrollBar.alpha = num2;
				}
			}
		}
		if (!this.mShouldMove)
		{
			return;
		}
		if (!this.mPressed)
		{
			if (this.mMomentum.magnitude > 0.0001f || this.mScroll != 0f)
			{
				if (this.movement == UIScrollView.Movement.Horizontal)
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * 0.05f, 0f, 0f));
				}
				else if (this.movement == UIScrollView.Movement.Vertical)
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(0f, this.mScroll * 0.05f, 0f));
				}
				else if (this.movement == UIScrollView.Movement.Unrestricted)
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * 0.05f, this.mScroll * 0.05f, 0f));
				}
				else
				{
					this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * this.customMovement.x * 0.05f, this.mScroll * this.customMovement.y * 0.05f, 0f));
				}
				this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
				Vector3 absolute = NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
				this.MoveAbsolute(absolute);
				if (this.restrictWithinPanel && this.mPanel.clipping != UIDrawCall.Clipping.None)
				{
					if (NGUITools.GetActive(this.centerOnChild))
					{
						if (this.centerOnChild.nextPageThreshold != 0f)
						{
							this.mMomentum = Vector3.zero;
							this.mScroll = 0f;
						}
						else
						{
							this.centerOnChild.Recenter();
						}
					}
					else
					{
						this.RestrictWithinBounds(false, this.canMoveHorizontally, this.canMoveVertically);
					}
				}
				if (this.onMomentumMove != null)
				{
					this.onMomentumMove();
				}
			}
			else
			{
				this.mScroll = 0f;
				this.mMomentum = Vector3.zero;
				SpringPanel component = base.GetComponent<SpringPanel>();
				if (component != null && component.enabled)
				{
					return;
				}
				this.mShouldMove = false;
				if (this.onStoppedMoving != null)
				{
					this.onStoppedMoving();
				}
			}
		}
		else
		{
			this.mScroll = 0f;
			NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
		}
	}

	// Token: 0x04002083 RID: 8323
	public static BetterList<UIScrollView> list = new BetterList<UIScrollView>();

	// Token: 0x04002084 RID: 8324
	public UIScrollView.Movement movement;

	// Token: 0x04002085 RID: 8325
	public UIScrollView.DragEffect dragEffect = UIScrollView.DragEffect.MomentumAndSpring;

	// Token: 0x04002086 RID: 8326
	public bool restrictWithinPanel = true;

	// Token: 0x04002087 RID: 8327
	public bool disableDragIfFits;

	// Token: 0x04002088 RID: 8328
	public bool smoothDragStart = true;

	// Token: 0x04002089 RID: 8329
	public bool iOSDragEmulation = true;

	// Token: 0x0400208A RID: 8330
	public float scrollWheelFactor = 0.25f;

	// Token: 0x0400208B RID: 8331
	public float momentumAmount = 35f;

	// Token: 0x0400208C RID: 8332
	public UIProgressBar horizontalScrollBar;

	// Token: 0x0400208D RID: 8333
	public UIProgressBar verticalScrollBar;

	// Token: 0x0400208E RID: 8334
	public UIScrollView.ShowCondition showScrollBars = UIScrollView.ShowCondition.OnlyIfNeeded;

	// Token: 0x0400208F RID: 8335
	public Vector2 customMovement = new Vector2(1f, 0f);

	// Token: 0x04002090 RID: 8336
	public UIWidget.Pivot contentPivot;

	// Token: 0x04002091 RID: 8337
	public UIScrollView.OnDragNotification onDragStarted;

	// Token: 0x04002092 RID: 8338
	public UIScrollView.OnDragNotification onDragFinished;

	// Token: 0x04002093 RID: 8339
	public UIScrollView.OnDragNotification onMomentumMove;

	// Token: 0x04002094 RID: 8340
	public UIScrollView.OnDragNotification onStoppedMoving;

	// Token: 0x04002095 RID: 8341
	[SerializeField]
	[HideInInspector]
	private Vector3 scale = new Vector3(1f, 0f, 0f);

	// Token: 0x04002096 RID: 8342
	[HideInInspector]
	[SerializeField]
	private Vector2 relativePositionOnReset = Vector2.zero;

	// Token: 0x04002097 RID: 8343
	protected Transform mTrans;

	// Token: 0x04002098 RID: 8344
	protected UIPanel mPanel;

	// Token: 0x04002099 RID: 8345
	protected Plane mPlane;

	// Token: 0x0400209A RID: 8346
	protected Vector3 mLastPos;

	// Token: 0x0400209B RID: 8347
	protected bool mPressed;

	// Token: 0x0400209C RID: 8348
	protected Vector3 mMomentum = Vector3.zero;

	// Token: 0x0400209D RID: 8349
	protected float mScroll;

	// Token: 0x0400209E RID: 8350
	protected Bounds mBounds;

	// Token: 0x0400209F RID: 8351
	protected bool mCalculatedBounds;

	// Token: 0x040020A0 RID: 8352
	protected bool mShouldMove;

	// Token: 0x040020A1 RID: 8353
	protected bool mIgnoreCallbacks;

	// Token: 0x040020A2 RID: 8354
	protected int mDragID = -10;

	// Token: 0x040020A3 RID: 8355
	protected Vector2 mDragStartOffset = Vector2.zero;

	// Token: 0x040020A4 RID: 8356
	protected bool mDragStarted;

	// Token: 0x040020A5 RID: 8357
	[HideInInspector]
	public UICenterOnChild centerOnChild;

	// Token: 0x0200047C RID: 1148
	public enum Movement
	{
		// Token: 0x040020A7 RID: 8359
		Horizontal,
		// Token: 0x040020A8 RID: 8360
		Vertical,
		// Token: 0x040020A9 RID: 8361
		Unrestricted,
		// Token: 0x040020AA RID: 8362
		Custom
	}

	// Token: 0x0200047D RID: 1149
	public enum DragEffect
	{
		// Token: 0x040020AC RID: 8364
		None,
		// Token: 0x040020AD RID: 8365
		Momentum,
		// Token: 0x040020AE RID: 8366
		MomentumAndSpring
	}

	// Token: 0x0200047E RID: 1150
	public enum ShowCondition
	{
		// Token: 0x040020B0 RID: 8368
		Always,
		// Token: 0x040020B1 RID: 8369
		OnlyIfNeeded,
		// Token: 0x040020B2 RID: 8370
		WhenDragging
	}

	// Token: 0x0200047F RID: 1151
	// (Invoke) Token: 0x06001BD4 RID: 7124
	public delegate void OnDragNotification();
}
