using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x0200046F RID: 1135
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Play Tween")]
public class UIPlayTween : MonoBehaviour
{
	// Token: 0x06001B44 RID: 6980 RVA: 0x00012338 File Offset: 0x00010538
	private void Awake()
	{
		if (this.eventReceiver != null && EventDelegate.IsValid(this.onFinished))
		{
			this.eventReceiver = null;
			this.callWhenFinished = null;
		}
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x00012369 File Offset: 0x00010569
	private void Start()
	{
		this.mStarted = true;
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x000D5F98 File Offset: 0x000D4198
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

	// Token: 0x06001B47 RID: 6983 RVA: 0x000D605C File Offset: 0x000D425C
	private void OnDisable()
	{
		UIToggle component = base.GetComponent<UIToggle>();
		if (component != null)
		{
			EventDelegate.Remove(component.onChange, new EventDelegate.Callback(this.OnToggle));
		}
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x0001238F File Offset: 0x0001058F
	private void OnDragOver()
	{
		if (this.trigger == Trigger.OnHover)
		{
			this.OnHover(true);
		}
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x000D6094 File Offset: 0x000D4294
	private void OnHover(bool isOver)
	{
		if (base.enabled && (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver)))
		{
			this.mActivated = (isOver && this.trigger == Trigger.OnHover);
			this.Play(isOver);
		}
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x000123A4 File Offset: 0x000105A4
	private void OnDragOut()
	{
		if (base.enabled && this.mActivated)
		{
			this.mActivated = false;
			this.Play(false);
		}
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x000D60FC File Offset: 0x000D42FC
	private void OnPress(bool isPressed)
	{
		if (base.enabled && (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed)))
		{
			this.mActivated = (isPressed && this.trigger == Trigger.OnPress);
			this.Play(isPressed);
		}
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x000123CA File Offset: 0x000105CA
	private void OnClick()
	{
		if (base.enabled && this.trigger == Trigger.OnClick)
		{
			this.Play(true);
		}
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x000123E9 File Offset: 0x000105E9
	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == Trigger.OnDoubleClick)
		{
			this.Play(true);
		}
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x000D6164 File Offset: 0x000D4364
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected)))
		{
			this.mActivated = (isSelected && this.trigger == Trigger.OnSelect);
			this.Play(isSelected);
		}
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x000D61D0 File Offset: 0x000D43D0
	private void OnToggle()
	{
		if (!base.enabled || UIToggle.current == null)
		{
			return;
		}
		if (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && UIToggle.current.value) || (this.trigger == Trigger.OnActivateFalse && !UIToggle.current.value))
		{
			this.Play(UIToggle.current.value);
		}
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x000D624C File Offset: 0x000D444C
	private void Update()
	{
		if (this.disableWhenFinished != DisableCondition.DoNotDisable && this.mTweens != null)
		{
			bool flag = true;
			bool flag2 = true;
			int i = 0;
			int num = this.mTweens.Length;
			while (i < num)
			{
				UITweener uitweener = this.mTweens[i];
				if (uitweener.tweenGroup == this.tweenGroup)
				{
					if (uitweener.enabled)
					{
						flag = false;
						break;
					}
					if (uitweener.direction != (Direction)this.disableWhenFinished)
					{
						flag2 = false;
					}
				}
				i++;
			}
			if (flag)
			{
				if (flag2)
				{
					NGUITools.SetActive(this.tweenTarget, false);
				}
				this.mTweens = null;
			}
		}
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x000D62F8 File Offset: 0x000D44F8
	public void Play(bool forward)
	{
		this.mActive = 0;
		GameObject gameObject = (!(this.tweenTarget == null)) ? this.tweenTarget : base.gameObject;
		if (!NGUITools.GetActive(gameObject))
		{
			if (this.ifDisabledOnPlay != EnableCondition.EnableThenPlay)
			{
				return;
			}
			NGUITools.SetActive(gameObject, true);
		}
		this.mTweens = ((!this.includeChildren) ? gameObject.GetComponents<UITweener>() : gameObject.GetComponentsInChildren<UITweener>());
		if (this.mTweens.Length == 0)
		{
			if (this.disableWhenFinished != DisableCondition.DoNotDisable)
			{
				NGUITools.SetActive(this.tweenTarget, false);
			}
		}
		else
		{
			bool flag = false;
			if (this.playDirection == Direction.Reverse)
			{
				forward = !forward;
			}
			int i = 0;
			int num = this.mTweens.Length;
			while (i < num)
			{
				UITweener uitweener = this.mTweens[i];
				if (uitweener.tweenGroup == this.tweenGroup)
				{
					if (!flag && !NGUITools.GetActive(gameObject))
					{
						flag = true;
						NGUITools.SetActive(gameObject, true);
					}
					this.mActive++;
					if (this.playDirection == Direction.Toggle)
					{
						EventDelegate.Add(uitweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
						uitweener.Toggle();
					}
					else
					{
						if (this.resetOnPlay || (this.resetIfDisabled && !uitweener.enabled))
						{
							uitweener.Play(forward);
							uitweener.ResetToBeginning();
						}
						EventDelegate.Add(uitweener.onFinished, new EventDelegate.Callback(this.OnFinished), true);
						uitweener.Play(forward);
					}
				}
				i++;
			}
		}
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x000D6490 File Offset: 0x000D4690
	private void OnFinished()
	{
		if (--this.mActive == 0 && UIPlayTween.current == null)
		{
			UIPlayTween.current = this;
			EventDelegate.Execute(this.onFinished);
			if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
			{
				this.eventReceiver.SendMessage(this.callWhenFinished, 1);
			}
			this.eventReceiver = null;
			UIPlayTween.current = null;
		}
	}

	// Token: 0x04002024 RID: 8228
	public static UIPlayTween current;

	// Token: 0x04002025 RID: 8229
	public GameObject tweenTarget;

	// Token: 0x04002026 RID: 8230
	public int tweenGroup;

	// Token: 0x04002027 RID: 8231
	public Trigger trigger;

	// Token: 0x04002028 RID: 8232
	public Direction playDirection = Direction.Forward;

	// Token: 0x04002029 RID: 8233
	public bool resetOnPlay;

	// Token: 0x0400202A RID: 8234
	public bool resetIfDisabled;

	// Token: 0x0400202B RID: 8235
	public EnableCondition ifDisabledOnPlay;

	// Token: 0x0400202C RID: 8236
	public DisableCondition disableWhenFinished;

	// Token: 0x0400202D RID: 8237
	public bool includeChildren;

	// Token: 0x0400202E RID: 8238
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x0400202F RID: 8239
	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	// Token: 0x04002030 RID: 8240
	[HideInInspector]
	[SerializeField]
	private string callWhenFinished;

	// Token: 0x04002031 RID: 8241
	private UITweener[] mTweens;

	// Token: 0x04002032 RID: 8242
	private bool mStarted;

	// Token: 0x04002033 RID: 8243
	private int mActive;

	// Token: 0x04002034 RID: 8244
	private bool mActivated;
}
