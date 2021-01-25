using System;
using UnityEngine;

// Token: 0x02000480 RID: 1152
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/NGUI Slider")]
public class UISlider : UIProgressBar
{
	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06001BD8 RID: 7128 RVA: 0x000127F3 File Offset: 0x000109F3
	// (set) Token: 0x06001BD9 RID: 7129 RVA: 0x000127FB File Offset: 0x000109FB
	[Obsolete("Use 'value' instead")]
	public float sliderValue
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

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06001BDA RID: 7130 RVA: 0x00012910 File Offset: 0x00010B10
	// (set) Token: 0x06001BDB RID: 7131 RVA: 0x0000264F File Offset: 0x0000084F
	[Obsolete("Use 'fillDirection' instead")]
	public bool inverted
	{
		get
		{
			return base.isInverted;
		}
		set
		{
		}
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x000DA1BC File Offset: 0x000D83BC
	protected override void Upgrade()
	{
		if (this.direction != UISlider.Direction.Upgraded)
		{
			this.mValue = this.rawValue;
			if (this.foreground != null)
			{
				this.mFG = this.foreground.GetComponent<UIWidget>();
			}
			if (this.direction == UISlider.Direction.Horizontal)
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			else
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom);
			}
			this.direction = UISlider.Direction.Upgraded;
		}
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x000DA24C File Offset: 0x000D844C
	protected override void OnStart()
	{
		GameObject go = (!(this.mBG != null) || (!(this.mBG.collider != null) && !(this.mBG.GetComponent<Collider2D>() != null))) ? base.gameObject : this.mBG.gameObject;
		UIEventListener uieventListener = UIEventListener.Get(go);
		UIEventListener uieventListener2 = uieventListener;
		uieventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener2.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
		UIEventListener uieventListener3 = uieventListener;
		uieventListener3.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener3.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
		if (this.thumb != null && (this.thumb.collider != null || this.thumb.GetComponent<Collider2D>() != null) && (this.mFG == null || this.thumb != this.mFG.cachedTransform))
		{
			UIEventListener uieventListener4 = UIEventListener.Get(this.thumb.gameObject);
			UIEventListener uieventListener5 = uieventListener4;
			uieventListener5.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uieventListener5.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
			UIEventListener uieventListener6 = uieventListener4;
			uieventListener6.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uieventListener6.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
		}
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x000DA3B8 File Offset: 0x000D85B8
	protected void OnPressBackground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastTouchPosition);
		if (!isPressed && this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x00012918 File Offset: 0x00010B18
	protected void OnDragBackground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastTouchPosition);
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x000DA40C File Offset: 0x000D860C
	protected void OnPressForeground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		if (isPressed)
		{
			this.mOffset = ((!(this.mFG == null)) ? (base.value - base.ScreenToValue(UICamera.lastTouchPosition)) : 0f);
		}
		else if (this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x00012942 File Offset: 0x00010B42
	protected void OnDragForeground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = this.mOffset + base.ScreenToValue(UICamera.lastTouchPosition);
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x000DA484 File Offset: 0x000D8684
	protected void OnKey(KeyCode key)
	{
		if (base.enabled)
		{
			float num = ((float)this.numberOfSteps <= 1f) ? 0.125f : (1f / (float)(this.numberOfSteps - 1));
			switch (this.mFill)
			{
			case UIProgressBar.FillDirection.LeftToRight:
				if (key == 276)
				{
					base.value = this.mValue - num;
				}
				else if (key == 275)
				{
					base.value = this.mValue + num;
				}
				break;
			case UIProgressBar.FillDirection.RightToLeft:
				if (key == 276)
				{
					base.value = this.mValue + num;
				}
				else if (key == 275)
				{
					base.value = this.mValue - num;
				}
				break;
			case UIProgressBar.FillDirection.BottomToTop:
				if (key == 274)
				{
					base.value = this.mValue - num;
				}
				else if (key == 273)
				{
					base.value = this.mValue + num;
				}
				break;
			case UIProgressBar.FillDirection.TopToBottom:
				if (key == 274)
				{
					base.value = this.mValue + num;
				}
				else if (key == 273)
				{
					base.value = this.mValue - num;
				}
				break;
			}
		}
	}

	// Token: 0x040020B3 RID: 8371
	[SerializeField]
	[HideInInspector]
	private Transform foreground;

	// Token: 0x040020B4 RID: 8372
	[HideInInspector]
	[SerializeField]
	private float rawValue = 1f;

	// Token: 0x040020B5 RID: 8373
	[SerializeField]
	[HideInInspector]
	private UISlider.Direction direction = UISlider.Direction.Upgraded;

	// Token: 0x040020B6 RID: 8374
	[SerializeField]
	[HideInInspector]
	protected bool mInverted;

	// Token: 0x02000481 RID: 1153
	private enum Direction
	{
		// Token: 0x040020B8 RID: 8376
		Horizontal,
		// Token: 0x040020B9 RID: 8377
		Vertical,
		// Token: 0x040020BA RID: 8378
		Upgraded
	}
}
