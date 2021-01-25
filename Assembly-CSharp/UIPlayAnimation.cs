using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x0200046C RID: 1132
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Play Animation")]
public class UIPlayAnimation : MonoBehaviour
{
	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06001B28 RID: 6952 RVA: 0x0001214A File Offset: 0x0001034A
	private bool dualState
	{
		get
		{
			return this.trigger == Trigger.OnPress || this.trigger == Trigger.OnHover;
		}
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x000D58C0 File Offset: 0x000D3AC0
	private void Awake()
	{
		UIButton component = base.GetComponent<UIButton>();
		if (component != null)
		{
			this.dragHighlight = component.dragHighlight;
		}
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x06001B2A RID: 6954 RVA: 0x000D591C File Offset: 0x000D3B1C
	private void Start()
	{
		this.mStarted = true;
		if (this.target == null && this.animator == null)
		{
			this.animator = base.GetComponentInChildren<Animator>();
		}
		if (this.animator != null)
		{
			if (this.animator.enabled)
			{
				this.animator.enabled = false;
			}
			return;
		}
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<Animation>();
		}
		if (this.target != null && this.target.enabled)
		{
			this.target.enabled = false;
		}
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x000D59D8 File Offset: 0x000D3BD8
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
		if (UICamera.currentTouch != null)
		{
			if (this.trigger == Trigger.OnPress || this.trigger == Trigger.OnPressTrue)
			{
				this.mActivated = (UICamera.currentTouch.pressed == base.gameObject);
			}
			if (this.trigger == Trigger.OnHover || this.trigger == Trigger.OnHoverTrue)
			{
				this.mActivated = (UICamera.currentTouch.current == base.gameObject);
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x000D5A9C File Offset: 0x000D3C9C
	private void OnDisable()
	{
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Remove(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x000D5AD4 File Offset: 0x000D3CD4
	private void OnHover(bool isOver)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver))
		{
			this.Play(isOver, this.dualState);
		}
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000D5B2C File Offset: 0x000D3D2C
	private void OnPress(bool isPressed)
	{
		if (!base.enabled)
		{
			return;
		}
		if (UICamera.currentTouchID < -1)
		{
			return;
		}
		if (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed))
		{
			this.Play(isPressed, this.dualState);
		}
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x00012164 File Offset: 0x00010364
	private void OnClick()
	{
		if (UICamera.currentTouchID < -1)
		{
			return;
		}
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true, false);
		}
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x00012190 File Offset: 0x00010390
	private void OnDoubleClick()
	{
		if (UICamera.currentTouchID < -1)
		{
			return;
		}
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true, false);
		}
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x000D5B90 File Offset: 0x000D3D90
	private void OnSelect(bool isSelected)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected))
		{
			this.Play(isSelected, this.dualState);
		}
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x000D5BEC File Offset: 0x000D3DEC
	private void OnToggle()
	{
		if (!base.enabled || UIToggle.current == null)
		{
			return;
		}
		if (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && UIToggle.current.value) || (this.trigger == Trigger.OnActivateFalse && !UIToggle.current.value))
		{
			this.Play(UIToggle.current.value, this.dualState);
		}
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x000D5C70 File Offset: 0x000D3E70
	private void OnDragOver()
	{
		if (base.enabled && this.dualState)
		{
			if (UICamera.currentTouch.dragged == base.gameObject)
			{
				this.Play(true, true);
			}
			else if (this.dragHighlight && this.trigger == Trigger.OnPress)
			{
				this.Play(true, true);
			}
		}
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x000121BE File Offset: 0x000103BE
	private void OnDragOut()
	{
		if (base.enabled && this.dualState && UICamera.hoveredObject != base.gameObject)
		{
			this.Play(false, true);
		}
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x000121F3 File Offset: 0x000103F3
	private void OnDrop(GameObject go)
	{
		if (base.enabled && this.trigger == Trigger.OnPress && UICamera.currentTouch.dragged != base.gameObject)
		{
			this.Play(false, true);
		}
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x0001222E File Offset: 0x0001042E
	public void Play(bool forward)
	{
		this.Play(forward, true);
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x000D5CDC File Offset: 0x000D3EDC
	public void Play(bool forward, bool onlyIfDifferent)
	{
		if (this.target || this.animator)
		{
			if (onlyIfDifferent)
			{
				if (this.mActivated == forward)
				{
					return;
				}
				this.mActivated = forward;
			}
			if (this.clearSelection && UICamera.selectedObject == base.gameObject)
			{
				UICamera.selectedObject = null;
			}
			int num = (int)(-(int)this.playDirection);
			Direction direction = (Direction)((!forward) ? num : ((int)this.playDirection));
			ActiveAnimation activeAnimation = (!this.target) ? ActiveAnimation.Play(this.animator, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished) : ActiveAnimation.Play(this.target, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished);
			if (activeAnimation != null)
			{
				if (this.resetOnPlay)
				{
					activeAnimation.Reset();
				}
				for (int i = 0; i < this.onFinished.Count; i++)
				{
					EventDelegate.Add(activeAnimation.onFinished, new EventDelegate.Callback(this.OnFinished), true);
				}
			}
		}
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x000D5E08 File Offset: 0x000D4008
	private void OnFinished()
	{
		if (UIPlayAnimation.current == null)
		{
			UIPlayAnimation.current = this;
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, 1);
			}
			this.eventReceiver = null;
			UIPlayAnimation.current = null;
		}
	}

	// Token: 0x04002005 RID: 8197
	public static UIPlayAnimation current;

	// Token: 0x04002006 RID: 8198
	public Animation target;

	// Token: 0x04002007 RID: 8199
	public Animator animator;

	// Token: 0x04002008 RID: 8200
	public string clipName;

	// Token: 0x04002009 RID: 8201
	public Trigger trigger;

	// Token: 0x0400200A RID: 8202
	public Direction playDirection = Direction.Forward;

	// Token: 0x0400200B RID: 8203
	public bool resetOnPlay;

	// Token: 0x0400200C RID: 8204
	public bool clearSelection;

	// Token: 0x0400200D RID: 8205
	public EnableCondition ifDisabledOnPlay;

	// Token: 0x0400200E RID: 8206
	public DisableCondition disableWhenFinished;

	// Token: 0x0400200F RID: 8207
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04002010 RID: 8208
	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	// Token: 0x04002011 RID: 8209
	[HideInInspector]
	[SerializeField]
	private string callWhenFinished;

	// Token: 0x04002012 RID: 8210
	private bool mStarted;

	// Token: 0x04002013 RID: 8211
	private bool mActivated;

	// Token: 0x04002014 RID: 8212
	private bool dragHighlight;
}
