using System;
using UnityEngine;

// Token: 0x02000479 RID: 1145
[AddComponentMenu("NGUI/Interaction/NGUI Scroll Bar")]
[ExecuteInEditMode]
public class UIScrollBar : UISlider
{
	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06001BAB RID: 7083 RVA: 0x000127F3 File Offset: 0x000109F3
	// (set) Token: 0x06001BAC RID: 7084 RVA: 0x000127FB File Offset: 0x000109FB
	[Obsolete("Use 'value' instead")]
	public float scrollValue
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06001BAD RID: 7085 RVA: 0x00012804 File Offset: 0x00010A04
	// (set) Token: 0x06001BAE RID: 7086 RVA: 0x000D840C File Offset: 0x000D660C
	public float barSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mSize != num)
			{
				this.mSize = num;
				this.mIsDirty = true;
				if (NGUITools.GetActive(this))
				{
					if (UIProgressBar.current == null && this.onChange != null)
					{
						UIProgressBar.current = this;
						EventDelegate.Execute(this.onChange);
						UIProgressBar.current = null;
					}
					this.ForceUpdate();
				}
			}
		}
	}

	// Token: 0x06001BAF RID: 7087 RVA: 0x000D8480 File Offset: 0x000D6680
	protected override void Upgrade()
	{
		if (this.mDir != UIScrollBar.Direction.Upgraded)
		{
			this.mValue = this.mScroll;
			if (this.mDir == UIScrollBar.Direction.Horizontal)
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			else
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.TopToBottom : UIProgressBar.FillDirection.BottomToTop);
			}
			this.mDir = UIScrollBar.Direction.Upgraded;
		}
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x000D84EC File Offset: 0x000D66EC
	protected override void OnStart()
	{
		base.OnStart();
		if (this.mFG != null && this.mFG.gameObject != base.gameObject)
		{
			if (!(this.mFG.collider != null) && !(this.mFG.GetComponent<Collider2D>() != null))
			{
				return;
			}
			UIEventListener uieventListener = UIEventListener.Get(this.mFG.gameObject);
			UIEventListener uieventListener2 = uieventListener;
			uieventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onPress, new UIEventListener.BoolDelegate(base.OnPressForeground));
			UIEventListener uieventListener3 = uieventListener;
			uieventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener3.onDrag, new UIEventListener.VectorDelegate(base.OnDragForeground));
			this.mFG.autoResizeBoxCollider = true;
		}
	}

	// Token: 0x06001BB1 RID: 7089 RVA: 0x000D85C0 File Offset: 0x000D67C0
	protected override float LocalToValue(Vector2 localPos)
	{
		if (!(this.mFG != null))
		{
			return base.LocalToValue(localPos);
		}
		float num = Mathf.Clamp01(this.mSize) * 0.5f;
		float num2 = num;
		float num3 = 1f - num;
		Vector3[] localCorners = this.mFG.localCorners;
		if (base.isHorizontal)
		{
			num2 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, num2);
			num3 = Mathf.Lerp(localCorners[0].x, localCorners[2].x, num3);
			float num4 = num3 - num2;
			if (num4 == 0f)
			{
				return base.value;
			}
			return (!base.isInverted) ? ((localPos.x - num2) / num4) : ((num3 - localPos.x) / num4);
		}
		else
		{
			num2 = Mathf.Lerp(localCorners[0].y, localCorners[1].y, num2);
			num3 = Mathf.Lerp(localCorners[3].y, localCorners[2].y, num3);
			float num5 = num3 - num2;
			if (num5 == 0f)
			{
				return base.value;
			}
			return (!base.isInverted) ? ((localPos.y - num2) / num5) : ((num3 - localPos.y) / num5);
		}
	}

	// Token: 0x06001BB2 RID: 7090 RVA: 0x000D8718 File Offset: 0x000D6918
	public override void ForceUpdate()
	{
		if (this.mFG != null)
		{
			this.mIsDirty = false;
			float num = Mathf.Clamp01(this.mSize) * 0.5f;
			float num2 = Mathf.Lerp(num, 1f - num, base.value);
			float num3 = num2 - num;
			float num4 = num2 + num;
			if (base.isHorizontal)
			{
				this.mFG.drawRegion = ((!base.isInverted) ? new Vector4(num3, 0f, num4, 1f) : new Vector4(1f - num4, 0f, 1f - num3, 1f));
			}
			else
			{
				this.mFG.drawRegion = ((!base.isInverted) ? new Vector4(0f, num3, 1f, num4) : new Vector4(0f, 1f - num4, 1f, 1f - num3));
			}
			if (this.thumb != null)
			{
				Vector4 drawingDimensions = this.mFG.drawingDimensions;
				Vector3 vector;
				vector..ctor(Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, 0.5f), Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, 0.5f));
				base.SetThumbPosition(this.mFG.cachedTransform.TransformPoint(vector));
			}
		}
		else
		{
			base.ForceUpdate();
		}
	}

	// Token: 0x0400207C RID: 8316
	[SerializeField]
	[HideInInspector]
	protected float mSize = 1f;

	// Token: 0x0400207D RID: 8317
	[HideInInspector]
	[SerializeField]
	private float mScroll;

	// Token: 0x0400207E RID: 8318
	[HideInInspector]
	[SerializeField]
	private UIScrollBar.Direction mDir = UIScrollBar.Direction.Upgraded;

	// Token: 0x0200047A RID: 1146
	private enum Direction
	{
		// Token: 0x04002080 RID: 8320
		Horizontal,
		// Token: 0x04002081 RID: 8321
		Vertical,
		// Token: 0x04002082 RID: 8322
		Upgraded
	}
}
