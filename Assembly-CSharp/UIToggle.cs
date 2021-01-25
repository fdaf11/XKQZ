using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x02000487 RID: 1159
[AddComponentMenu("NGUI/Interaction/Toggle")]
[ExecuteInEditMode]
public class UIToggle : UIWidgetContainer
{
	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06001BF6 RID: 7158 RVA: 0x00012A9A File Offset: 0x00010C9A
	// (set) Token: 0x06001BF7 RID: 7159 RVA: 0x000DACB0 File Offset: 0x000D8EB0
	public bool value
	{
		get
		{
			return (!this.mStarted) ? this.startsActive : this.mIsActive;
		}
		set
		{
			if (!this.mStarted)
			{
				this.startsActive = value;
			}
			else if (this.group == 0 || value || this.optionCanBeNone || !this.mStarted)
			{
				this.Set(value);
			}
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06001BF8 RID: 7160 RVA: 0x00012AB8 File Offset: 0x00010CB8
	// (set) Token: 0x06001BF9 RID: 7161 RVA: 0x00012AC0 File Offset: 0x00010CC0
	[Obsolete("Use 'value' instead")]
	public bool isChecked
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x06001BFA RID: 7162 RVA: 0x000DAD04 File Offset: 0x000D8F04
	public static UIToggle GetActiveToggle(int group)
	{
		for (int i = 0; i < UIToggle.list.size; i++)
		{
			UIToggle uitoggle = UIToggle.list[i];
			if (uitoggle != null && uitoggle.group == group && uitoggle.mIsActive)
			{
				return uitoggle;
			}
		}
		return null;
	}

	// Token: 0x06001BFB RID: 7163 RVA: 0x00012AC9 File Offset: 0x00010CC9
	private void OnEnable()
	{
		UIToggle.list.Add(this);
	}

	// Token: 0x06001BFC RID: 7164 RVA: 0x00012AD6 File Offset: 0x00010CD6
	private void OnDisable()
	{
		UIToggle.list.Remove(this);
	}

	// Token: 0x06001BFD RID: 7165 RVA: 0x000DAD60 File Offset: 0x000D8F60
	private void Start()
	{
		if (this.startsChecked)
		{
			this.startsChecked = false;
			this.startsActive = true;
		}
		if (!Application.isPlaying)
		{
			if (this.checkSprite != null && this.activeSprite == null)
			{
				this.activeSprite = this.checkSprite;
				this.checkSprite = null;
			}
			if (this.checkAnimation != null && this.activeAnimation == null)
			{
				this.activeAnimation = this.checkAnimation;
				this.checkAnimation = null;
			}
			if (Application.isPlaying && this.activeSprite != null)
			{
				this.activeSprite.alpha = ((!this.startsActive) ? 0f : 1f);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				this.eventReceiver = null;
				this.functionName = null;
			}
		}
		else
		{
			this.mIsActive = !this.startsActive;
			this.mStarted = true;
			bool flag = this.instantTween;
			this.instantTween = true;
			this.Set(this.startsActive);
			this.instantTween = flag;
		}
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x00012AE4 File Offset: 0x00010CE4
	private void OnClick()
	{
		if (base.enabled)
		{
			this.value = !this.value;
		}
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x000DAE94 File Offset: 0x000D9094
	private void Set(bool state)
	{
		if (!this.mStarted)
		{
			this.mIsActive = state;
			this.startsActive = state;
			if (this.activeSprite != null)
			{
				this.activeSprite.alpha = ((!state) ? 0f : 1f);
			}
		}
		else if (this.mIsActive != state)
		{
			if (this.group != 0 && state)
			{
				int i = 0;
				int size = UIToggle.list.size;
				while (i < size)
				{
					UIToggle uitoggle = UIToggle.list[i];
					if (uitoggle != this && uitoggle.group == this.group)
					{
						uitoggle.Set(false);
					}
					if (UIToggle.list.size != size)
					{
						size = UIToggle.list.size;
						i = 0;
					}
					else
					{
						i++;
					}
				}
			}
			this.mIsActive = state;
			if (this.activeSprite != null)
			{
				if (this.instantTween || !NGUITools.GetActive(this))
				{
					this.activeSprite.alpha = ((!this.mIsActive) ? 0f : 1f);
				}
				else
				{
					TweenAlpha.Begin(this.activeSprite.gameObject, 0.15f, (!this.mIsActive) ? 0f : 1f);
				}
			}
			if (UIToggle.current == null)
			{
				UIToggle uitoggle2 = UIToggle.current;
				UIToggle.current = this;
				if (EventDelegate.IsValid(this.onChange))
				{
					EventDelegate.Execute(this.onChange);
				}
				else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
				{
					this.eventReceiver.SendMessage(this.functionName, this.mIsActive, 1);
				}
				UIToggle.current = uitoggle2;
			}
			if (this.activeAnimation != null)
			{
				ActiveAnimation activeAnimation = ActiveAnimation.Play(this.activeAnimation, null, (!state) ? Direction.Reverse : Direction.Forward, EnableCondition.IgnoreDisabledState, DisableCondition.DoNotDisable);
				if (activeAnimation != null && (this.instantTween || !NGUITools.GetActive(this)))
				{
					activeAnimation.Finish();
				}
			}
		}
	}

	// Token: 0x040020D2 RID: 8402
	public static BetterList<UIToggle> list = new BetterList<UIToggle>();

	// Token: 0x040020D3 RID: 8403
	public static UIToggle current;

	// Token: 0x040020D4 RID: 8404
	public int group;

	// Token: 0x040020D5 RID: 8405
	public UIWidget activeSprite;

	// Token: 0x040020D6 RID: 8406
	public Animation activeAnimation;

	// Token: 0x040020D7 RID: 8407
	public bool startsActive;

	// Token: 0x040020D8 RID: 8408
	public bool instantTween;

	// Token: 0x040020D9 RID: 8409
	public bool optionCanBeNone;

	// Token: 0x040020DA RID: 8410
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x040020DB RID: 8411
	[HideInInspector]
	[SerializeField]
	private UISprite checkSprite;

	// Token: 0x040020DC RID: 8412
	[SerializeField]
	[HideInInspector]
	private Animation checkAnimation;

	// Token: 0x040020DD RID: 8413
	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	// Token: 0x040020DE RID: 8414
	[HideInInspector]
	[SerializeField]
	private string functionName = "OnActivate";

	// Token: 0x040020DF RID: 8415
	[SerializeField]
	[HideInInspector]
	private bool startsChecked;

	// Token: 0x040020E0 RID: 8416
	private bool mIsActive = true;

	// Token: 0x040020E1 RID: 8417
	private bool mStarted;
}
