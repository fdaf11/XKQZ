using System;
using UnityEngine;

// Token: 0x02000446 RID: 1094
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Button Color")]
public class UIButtonColor : UIWidgetContainer
{
	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06001A43 RID: 6723 RVA: 0x000112AA File Offset: 0x0000F4AA
	// (set) Token: 0x06001A44 RID: 6724 RVA: 0x000112B2 File Offset: 0x0000F4B2
	public UIButtonColor.State state
	{
		get
		{
			return this.mState;
		}
		set
		{
			this.SetState(value, false);
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06001A45 RID: 6725 RVA: 0x000112BC File Offset: 0x0000F4BC
	// (set) Token: 0x06001A46 RID: 6726 RVA: 0x000D1DB4 File Offset: 0x000CFFB4
	public Color defaultColor
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mDefaultColor;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			this.mDefaultColor = value;
			UIButtonColor.State state = this.mState;
			this.mState = UIButtonColor.State.Disabled;
			this.SetState(state, false);
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06001A47 RID: 6727 RVA: 0x0000DB01 File Offset: 0x0000BD01
	// (set) Token: 0x06001A48 RID: 6728 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
	public virtual bool isEnabled
	{
		get
		{
			return base.enabled;
		}
		set
		{
			base.enabled = value;
		}
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x000112D5 File Offset: 0x0000F4D5
	public void ResetDefaultColor()
	{
		this.defaultColor = this.mStartingColor;
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x000112E3 File Offset: 0x0000F4E3
	private void Awake()
	{
		if (!this.mInitDone)
		{
			this.OnInit();
		}
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x000112F6 File Offset: 0x0000F4F6
	private void Start()
	{
		if (!this.isEnabled)
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
		if (!Application.isPlaying)
		{
			return;
		}
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x000D1DF0 File Offset: 0x000CFFF0
	protected virtual void OnInit()
	{
		this.mInitDone = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
		this.mWidget = this.tweenTarget.GetComponent<UIWidget>();
		if (this.mWidget != null)
		{
			this.mDefaultColor = this.mWidget.color;
			this.mStartingColor = this.mDefaultColor;
		}
		else
		{
			Renderer renderer = this.tweenTarget.renderer;
			if (renderer != null)
			{
				this.mDefaultColor = ((!Application.isPlaying) ? renderer.sharedMaterial.color : renderer.material.color);
				this.mStartingColor = this.mDefaultColor;
			}
			else
			{
				Light light = this.tweenTarget.light;
				if (light != null)
				{
					this.mDefaultColor = light.color;
					this.mStartingColor = this.mDefaultColor;
				}
				else
				{
					this.tweenTarget = null;
					this.mInitDone = false;
				}
			}
		}
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x000D1EFC File Offset: 0x000D00FC
	protected virtual void OnEnable()
	{
		if (this.mInitDone)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
		if (UICamera.currentTouch != null)
		{
			if (UICamera.currentTouch.pressed == base.gameObject)
			{
				this.OnPress(true);
			}
			else if (UICamera.currentTouch.current == base.gameObject)
			{
				this.OnHover(true);
			}
		}
	}

	// Token: 0x06001A4E RID: 6734 RVA: 0x000D1F78 File Offset: 0x000D0178
	protected virtual void OnDisable()
	{
		if (this.mInitDone && this.tweenTarget != null)
		{
			this.SetState(UIButtonColor.State.Normal, true);
			TweenColor component = this.tweenTarget.GetComponent<TweenColor>();
			if (component != null)
			{
				component.value = this.mDefaultColor;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06001A4F RID: 6735 RVA: 0x000D1FD4 File Offset: 0x000D01D4
	protected virtual void OnHover(bool isOver)
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState((!isOver) ? UIButtonColor.State.Normal : UIButtonColor.State.Hover, false);
			}
		}
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x000D2024 File Offset: 0x000D0224
	protected virtual void OnPress(bool isPressed)
	{
		if (this.isEnabled && UICamera.currentTouch != null)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				if (isPressed)
				{
					this.SetState(UIButtonColor.State.Pressed, false);
				}
				else if (UICamera.currentTouch.current == base.gameObject)
				{
					if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
					{
						this.SetState(UIButtonColor.State.Hover, false);
					}
					else if (UICamera.currentScheme == UICamera.ControlScheme.Mouse && UICamera.hoveredObject == base.gameObject)
					{
						this.SetState(UIButtonColor.State.Hover, false);
					}
					else
					{
						this.SetState(UIButtonColor.State.Normal, false);
					}
				}
				else
				{
					this.SetState(UIButtonColor.State.Normal, false);
				}
			}
		}
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x00011316 File Offset: 0x0000F516
	protected virtual void OnDragOver()
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(UIButtonColor.State.Pressed, false);
			}
		}
	}

	// Token: 0x06001A52 RID: 6738 RVA: 0x0001134D File Offset: 0x0000F54D
	protected virtual void OnDragOut()
	{
		if (this.isEnabled)
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.tweenTarget != null)
			{
				this.SetState(UIButtonColor.State.Normal, false);
			}
		}
	}

	// Token: 0x06001A53 RID: 6739 RVA: 0x000D20F0 File Offset: 0x000D02F0
	protected virtual void OnSelect(bool isSelected)
	{
		if (this.isEnabled && this.tweenTarget != null)
		{
			if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
			{
				this.OnHover(isSelected);
			}
			else if (!isSelected && UICamera.touchCount < 2)
			{
				this.OnHover(isSelected);
			}
		}
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x00011384 File Offset: 0x0000F584
	public virtual void SetState(UIButtonColor.State state, bool instant)
	{
		if (!this.mInitDone)
		{
			this.mInitDone = true;
			this.OnInit();
		}
		if (this.mState != state)
		{
			this.mState = state;
			this.UpdateColor(instant);
		}
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x000D2148 File Offset: 0x000D0348
	public void UpdateColor(bool instant)
	{
		TweenColor tweenColor;
		switch (this.mState)
		{
		case UIButtonColor.State.Hover:
			tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.hover);
			break;
		case UIButtonColor.State.Pressed:
			tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.pressed);
			break;
		case UIButtonColor.State.Disabled:
			tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.disabledColor);
			break;
		default:
			tweenColor = TweenColor.Begin(this.tweenTarget, this.duration, this.mDefaultColor);
			break;
		}
		if (instant && tweenColor != null)
		{
			tweenColor.value = tweenColor.to;
			tweenColor.enabled = false;
		}
	}

	// Token: 0x04001F11 RID: 7953
	public GameObject tweenTarget;

	// Token: 0x04001F12 RID: 7954
	public Color hover = new Color(0.88235295f, 0.78431374f, 0.5882353f, 1f);

	// Token: 0x04001F13 RID: 7955
	public Color pressed = new Color(0.7176471f, 0.6392157f, 0.48235294f, 1f);

	// Token: 0x04001F14 RID: 7956
	public Color disabledColor = Color.grey;

	// Token: 0x04001F15 RID: 7957
	public float duration = 0.2f;

	// Token: 0x04001F16 RID: 7958
	[NonSerialized]
	protected Color mStartingColor;

	// Token: 0x04001F17 RID: 7959
	[NonSerialized]
	protected Color mDefaultColor;

	// Token: 0x04001F18 RID: 7960
	[NonSerialized]
	protected bool mInitDone;

	// Token: 0x04001F19 RID: 7961
	[NonSerialized]
	protected UIWidget mWidget;

	// Token: 0x04001F1A RID: 7962
	[NonSerialized]
	protected UIButtonColor.State mState;

	// Token: 0x02000447 RID: 1095
	public enum State
	{
		// Token: 0x04001F1C RID: 7964
		Normal,
		// Token: 0x04001F1D RID: 7965
		Hover,
		// Token: 0x04001F1E RID: 7966
		Pressed,
		// Token: 0x04001F1F RID: 7967
		Disabled
	}
}
