using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020004F3 RID: 1267
[AddComponentMenu("NGUI/UI/Input Field")]
public class UIInput : MonoBehaviour
{
	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06001FFD RID: 8189 RVA: 0x0001551A File Offset: 0x0001371A
	// (set) Token: 0x06001FFE RID: 8190 RVA: 0x00015533 File Offset: 0x00013733
	public string defaultText
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mDefaultText;
		}
		set
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			this.mDefaultText = value;
			this.UpdateLabel();
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06001FFF RID: 8191 RVA: 0x00015553 File Offset: 0x00013753
	public bool inputShouldBeHidden
	{
		get
		{
			return this.hideInput && this.label != null && !this.label.multiLine && this.inputType != UIInput.InputType.Password;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06002000 RID: 8192 RVA: 0x00015590 File Offset: 0x00013790
	// (set) Token: 0x06002001 RID: 8193 RVA: 0x00015598 File Offset: 0x00013798
	[Obsolete("Use UIInput.value instead")]
	public string text
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

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06002002 RID: 8194 RVA: 0x000155A1 File Offset: 0x000137A1
	// (set) Token: 0x06002003 RID: 8195 RVA: 0x000F2F74 File Offset: 0x000F1174
	public string value
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			return this.mValue;
		}
		set
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			UIInput.mDrawStart = 0;
			if (Application.platform == 22)
			{
				value = value.Replace("\\b", "\b");
			}
			value = this.Validate(value);
			if (this.mValue != value)
			{
				this.mValue = value;
				this.mLoadSavedValue = false;
				if (this.isSelected)
				{
					if (string.IsNullOrEmpty(value))
					{
						this.mSelectionStart = 0;
						this.mSelectionEnd = 0;
					}
					else
					{
						this.mSelectionStart = value.Length;
						this.mSelectionEnd = this.mSelectionStart;
					}
				}
				else
				{
					this.SaveToPlayerPrefs(value);
				}
				this.UpdateLabel();
				this.ExecuteOnChange();
			}
		}
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06002004 RID: 8196 RVA: 0x000155BA File Offset: 0x000137BA
	// (set) Token: 0x06002005 RID: 8197 RVA: 0x000155C2 File Offset: 0x000137C2
	[Obsolete("Use UIInput.isSelected instead")]
	public bool selected
	{
		get
		{
			return this.isSelected;
		}
		set
		{
			this.isSelected = value;
		}
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06002006 RID: 8198 RVA: 0x000155CB File Offset: 0x000137CB
	// (set) Token: 0x06002007 RID: 8199 RVA: 0x000155D8 File Offset: 0x000137D8
	public bool isSelected
	{
		get
		{
			return UIInput.selection == this;
		}
		set
		{
			if (!value)
			{
				if (this.isSelected)
				{
					UICamera.selectedObject = null;
				}
			}
			else
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06002008 RID: 8200 RVA: 0x00015601 File Offset: 0x00013801
	// (set) Token: 0x06002009 RID: 8201 RVA: 0x00015624 File Offset: 0x00013824
	public int cursorPosition
	{
		get
		{
			return (!this.isSelected) ? this.value.Length : this.mSelectionEnd;
		}
		set
		{
			if (this.isSelected)
			{
				this.mSelectionEnd = value;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x0600200A RID: 8202 RVA: 0x0001563E File Offset: 0x0001383E
	// (set) Token: 0x0600200B RID: 8203 RVA: 0x00015661 File Offset: 0x00013861
	public int selectionStart
	{
		get
		{
			return (!this.isSelected) ? this.value.Length : this.mSelectionStart;
		}
		set
		{
			if (this.isSelected)
			{
				this.mSelectionStart = value;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x0600200C RID: 8204 RVA: 0x00015601 File Offset: 0x00013801
	// (set) Token: 0x0600200D RID: 8205 RVA: 0x00015624 File Offset: 0x00013824
	public int selectionEnd
	{
		get
		{
			return (!this.isSelected) ? this.value.Length : this.mSelectionEnd;
		}
		set
		{
			if (this.isSelected)
			{
				this.mSelectionEnd = value;
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x0600200E RID: 8206 RVA: 0x0001567B File Offset: 0x0001387B
	public UITexture caret
	{
		get
		{
			return this.mCaret;
		}
	}

	// Token: 0x0600200F RID: 8207 RVA: 0x000F3038 File Offset: 0x000F1238
	public string Validate(string val)
	{
		if (string.IsNullOrEmpty(val))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder(val.Length);
		for (int i = 0; i < val.Length; i++)
		{
			char c = val.get_Chars(i);
			if (this.onValidate != null)
			{
				c = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, c);
			}
			else if (this.validation != UIInput.Validation.None)
			{
				c = this.Validate(stringBuilder.ToString(), stringBuilder.Length, c);
			}
			if (c != '\0')
			{
				stringBuilder.Append(c);
			}
		}
		if (this.characterLimit > 0 && stringBuilder.Length > this.characterLimit)
		{
			return stringBuilder.ToString(0, this.characterLimit);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06002010 RID: 8208 RVA: 0x000F3108 File Offset: 0x000F1308
	private void Start()
	{
		if (this.selectOnTab != null)
		{
			UIKeyNavigation uikeyNavigation = base.GetComponent<UIKeyNavigation>();
			if (uikeyNavigation == null)
			{
				uikeyNavigation = base.gameObject.AddComponent<UIKeyNavigation>();
				uikeyNavigation.onDown = this.selectOnTab;
			}
			this.selectOnTab = null;
			NGUITools.SetDirty(this);
		}
		if (this.mLoadSavedValue && !string.IsNullOrEmpty(this.savedAs))
		{
			this.LoadValue();
		}
		else
		{
			this.value = this.mValue.Replace("\\n", "\n");
		}
	}

	// Token: 0x06002011 RID: 8209 RVA: 0x000F31A0 File Offset: 0x000F13A0
	protected void Init()
	{
		if (this.mDoInit && this.label != null)
		{
			this.mDoInit = false;
			this.mDefaultText = this.label.text;
			this.mDefaultColor = this.label.color;
			this.label.supportEncoding = false;
			if (this.label.alignment == NGUIText.Alignment.Justified)
			{
				this.label.alignment = NGUIText.Alignment.Left;
				Debug.LogWarning("Input fields using labels with justified alignment are not supported at this time", this);
			}
			this.mPivot = this.label.pivot;
			this.mPosition = this.label.cachedTransform.localPosition.x;
			this.UpdateLabel();
		}
	}

	// Token: 0x06002012 RID: 8210 RVA: 0x00015683 File Offset: 0x00013883
	protected void SaveToPlayerPrefs(string val)
	{
		if (!string.IsNullOrEmpty(this.savedAs))
		{
			if (string.IsNullOrEmpty(val))
			{
				PlayerPrefs.DeleteKey(this.savedAs);
			}
			else
			{
				PlayerPrefs.SetString(this.savedAs, val);
			}
		}
	}

	// Token: 0x06002013 RID: 8211 RVA: 0x000F325C File Offset: 0x000F145C
	protected virtual void OnSelect(bool isSelected)
	{
		if (isSelected)
		{
			if (this.mOnGUI == null)
			{
				this.mOnGUI = base.gameObject.AddComponent<UIInputOnGUI>();
			}
			this.OnSelectEvent();
		}
		else
		{
			if (this.mOnGUI != null)
			{
				Object.Destroy(this.mOnGUI);
				this.mOnGUI = null;
			}
			this.OnDeselectEvent();
		}
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x000156BC File Offset: 0x000138BC
	protected void OnSelectEvent()
	{
		UIInput.selection = this;
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null && NGUITools.GetActive(this))
		{
			this.mSelectMe = Time.frameCount;
		}
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x000F32C8 File Offset: 0x000F14C8
	protected void OnDeselectEvent()
	{
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null && NGUITools.GetActive(this))
		{
			this.mValue = this.value;
			if (string.IsNullOrEmpty(this.mValue))
			{
				this.label.text = this.mDefaultText;
				this.label.color = this.mDefaultColor;
			}
			else
			{
				this.label.text = this.mValue;
			}
			Input.imeCompositionMode = 0;
			this.RestoreLabelPivot();
		}
		UIInput.selection = null;
		this.UpdateLabel();
	}

	// Token: 0x06002016 RID: 8214 RVA: 0x000F3370 File Offset: 0x000F1570
	protected virtual void Update()
	{
		if (this.isSelected)
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			if (this.mSelectMe != -1 && this.mSelectMe != Time.frameCount)
			{
				this.mSelectMe = -1;
				this.mSelectionEnd = ((!string.IsNullOrEmpty(this.mValue)) ? this.mValue.Length : 0);
				UIInput.mDrawStart = 0;
				this.mSelectionStart = ((!this.selectAllTextOnFocus) ? this.mSelectionEnd : 0);
				this.label.color = this.activeTextColor;
				Vector2 compositionCursorPos = (!(UICamera.current != null) || !(UICamera.current.cachedCamera != null)) ? this.label.worldCorners[0] : UICamera.current.cachedCamera.WorldToScreenPoint(this.label.worldCorners[0]);
				compositionCursorPos.y = (float)Screen.height - compositionCursorPos.y;
				Input.imeCompositionMode = 1;
				Input.compositionCursorPos = compositionCursorPos;
				this.UpdateLabel();
				if (string.IsNullOrEmpty(Input.inputString))
				{
					return;
				}
			}
			string compositionString = Input.compositionString;
			if (string.IsNullOrEmpty(compositionString) && !string.IsNullOrEmpty(Input.inputString))
			{
				string inputString = Input.inputString;
				for (int i = 0; i < inputString.Length; i++)
				{
					char c = inputString.get_Chars(i);
					if (c >= ' ')
					{
						if (c != '')
						{
							if (c != '')
							{
								if (c != '')
								{
									if (c != '')
									{
										this.Insert(c.ToString());
									}
								}
							}
						}
					}
				}
			}
			if (UIInput.mLastIME != compositionString)
			{
				this.mSelectionEnd = ((!string.IsNullOrEmpty(compositionString)) ? (this.mValue.Length + compositionString.Length) : this.mSelectionStart);
				UIInput.mLastIME = compositionString;
				this.UpdateLabel();
				this.ExecuteOnChange();
			}
			if (this.mCaret != null && this.mNextBlink < RealTime.time)
			{
				this.mNextBlink = RealTime.time + 0.5f;
				this.mCaret.enabled = !this.mCaret.enabled;
			}
			if (this.isSelected && this.mLastAlpha != this.label.finalAlpha)
			{
				this.UpdateLabel();
			}
		}
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x000F3620 File Offset: 0x000F1820
	protected void DoBackspace()
	{
		if (!string.IsNullOrEmpty(this.mValue))
		{
			if (this.mSelectionStart == this.mSelectionEnd)
			{
				if (this.mSelectionStart < 1)
				{
					return;
				}
				this.mSelectionEnd--;
			}
			this.Insert(string.Empty);
		}
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x000F3674 File Offset: 0x000F1874
	public virtual bool ProcessEvent(Event ev)
	{
		if (this.label == null)
		{
			return false;
		}
		RuntimePlatform platform = Application.platform;
		bool flag = platform == null || platform == 1 || platform == 3;
		bool flag2 = (!flag) ? ((ev.modifiers & 2) != 0) : ((ev.modifiers & 8) != 0);
		if ((ev.modifiers & 4) != null)
		{
			flag2 = false;
		}
		bool flag3 = (ev.modifiers & 1) != 0;
		KeyCode keyCode = ev.keyCode;
		switch (keyCode)
		{
		case 271:
			break;
		default:
			switch (keyCode)
			{
			case 97:
				if (flag2)
				{
					ev.Use();
					this.mSelectionStart = 0;
					this.mSelectionEnd = this.mValue.Length;
					this.UpdateLabel();
				}
				return true;
			default:
				switch (keyCode)
				{
				case 118:
					if (flag2)
					{
						ev.Use();
						this.Insert(NGUITools.clipboard);
					}
					return true;
				default:
					if (keyCode == 8)
					{
						ev.Use();
						this.DoBackspace();
						return true;
					}
					if (keyCode != 13)
					{
						if (keyCode != 127)
						{
							return false;
						}
						ev.Use();
						if (!string.IsNullOrEmpty(this.mValue))
						{
							if (this.mSelectionStart == this.mSelectionEnd)
							{
								if (this.mSelectionStart >= this.mValue.Length)
								{
									return true;
								}
								this.mSelectionEnd++;
							}
							this.Insert(string.Empty);
						}
						return true;
					}
					break;
				case 120:
					if (flag2)
					{
						ev.Use();
						NGUITools.clipboard = this.GetSelection();
						this.Insert(string.Empty);
					}
					return true;
				}
				break;
			case 99:
				if (flag2)
				{
					ev.Use();
					NGUITools.clipboard = this.GetSelection();
				}
				return true;
			}
			break;
		case 273:
			ev.Use();
			if (!string.IsNullOrEmpty(this.mValue))
			{
				this.mSelectionEnd = this.label.GetCharacterIndex(this.mSelectionEnd, 273);
				if (this.mSelectionEnd != 0)
				{
					this.mSelectionEnd += UIInput.mDrawStart;
				}
				if (!flag3)
				{
					this.mSelectionStart = this.mSelectionEnd;
				}
				this.UpdateLabel();
			}
			return true;
		case 274:
			ev.Use();
			if (!string.IsNullOrEmpty(this.mValue))
			{
				this.mSelectionEnd = this.label.GetCharacterIndex(this.mSelectionEnd, 274);
				if (this.mSelectionEnd != this.label.processedText.Length)
				{
					this.mSelectionEnd += UIInput.mDrawStart;
				}
				else
				{
					this.mSelectionEnd = this.mValue.Length;
				}
				if (!flag3)
				{
					this.mSelectionStart = this.mSelectionEnd;
				}
				this.UpdateLabel();
			}
			return true;
		case 275:
			ev.Use();
			if (!string.IsNullOrEmpty(this.mValue))
			{
				this.mSelectionEnd = Mathf.Min(this.mSelectionEnd + 1, this.mValue.Length);
				if (!flag3)
				{
					this.mSelectionStart = this.mSelectionEnd;
				}
				this.UpdateLabel();
			}
			return true;
		case 276:
			ev.Use();
			if (!string.IsNullOrEmpty(this.mValue))
			{
				this.mSelectionEnd = Mathf.Max(this.mSelectionEnd - 1, 0);
				if (!flag3)
				{
					this.mSelectionStart = this.mSelectionEnd;
				}
				this.UpdateLabel();
			}
			return true;
		case 278:
			ev.Use();
			if (!string.IsNullOrEmpty(this.mValue))
			{
				if (this.label.multiLine)
				{
					this.mSelectionEnd = this.label.GetCharacterIndex(this.mSelectionEnd, 278);
				}
				else
				{
					this.mSelectionEnd = 0;
				}
				if (!flag3)
				{
					this.mSelectionStart = this.mSelectionEnd;
				}
				this.UpdateLabel();
			}
			return true;
		case 279:
			ev.Use();
			if (!string.IsNullOrEmpty(this.mValue))
			{
				if (this.label.multiLine)
				{
					this.mSelectionEnd = this.label.GetCharacterIndex(this.mSelectionEnd, 279);
				}
				else
				{
					this.mSelectionEnd = this.mValue.Length;
				}
				if (!flag3)
				{
					this.mSelectionStart = this.mSelectionEnd;
				}
				this.UpdateLabel();
			}
			return true;
		case 280:
			ev.Use();
			if (!string.IsNullOrEmpty(this.mValue))
			{
				this.mSelectionEnd = 0;
				if (!flag3)
				{
					this.mSelectionStart = this.mSelectionEnd;
				}
				this.UpdateLabel();
			}
			return true;
		case 281:
			ev.Use();
			if (!string.IsNullOrEmpty(this.mValue))
			{
				this.mSelectionEnd = this.mValue.Length;
				if (!flag3)
				{
					this.mSelectionStart = this.mSelectionEnd;
				}
				this.UpdateLabel();
			}
			return true;
		}
		ev.Use();
		bool flag4 = this.onReturnKey == UIInput.OnReturnKey.NewLine || (this.onReturnKey == UIInput.OnReturnKey.Default && this.label.multiLine && !flag2 && this.label.overflowMethod != UILabel.Overflow.ClampContent && this.validation == UIInput.Validation.None);
		if (flag4)
		{
			this.Insert("\n");
		}
		else
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.currentKey = ev.keyCode;
			this.Submit();
			UICamera.currentKey = 0;
		}
		return true;
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x000F3BD4 File Offset: 0x000F1DD4
	protected virtual void Insert(string text)
	{
		string leftText = this.GetLeftText();
		string rightText = this.GetRightText();
		int length = rightText.Length;
		StringBuilder stringBuilder = new StringBuilder(leftText.Length + rightText.Length + text.Length);
		stringBuilder.Append(leftText);
		int i = 0;
		int length2 = text.Length;
		while (i < length2)
		{
			char c = text.get_Chars(i);
			if (c == '\b')
			{
				this.DoBackspace();
			}
			else
			{
				if (this.characterLimit > 0 && stringBuilder.Length + length >= this.characterLimit)
				{
					break;
				}
				if (this.onValidate != null)
				{
					c = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, c);
				}
				else if (this.validation != UIInput.Validation.None)
				{
					c = this.Validate(stringBuilder.ToString(), stringBuilder.Length, c);
				}
				if (c != '\0')
				{
					stringBuilder.Append(c);
				}
			}
			i++;
		}
		this.mSelectionStart = stringBuilder.Length;
		this.mSelectionEnd = this.mSelectionStart;
		int j = 0;
		int length3 = rightText.Length;
		while (j < length3)
		{
			char c2 = rightText.get_Chars(j);
			if (this.onValidate != null)
			{
				c2 = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, c2);
			}
			else if (this.validation != UIInput.Validation.None)
			{
				c2 = this.Validate(stringBuilder.ToString(), stringBuilder.Length, c2);
			}
			if (c2 != '\0')
			{
				stringBuilder.Append(c2);
			}
			j++;
		}
		this.mValue = stringBuilder.ToString();
		this.UpdateLabel();
		this.ExecuteOnChange();
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x000F3D8C File Offset: 0x000F1F8C
	protected string GetLeftText()
	{
		int num = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
		return (!string.IsNullOrEmpty(this.mValue) && num >= 0) ? this.mValue.Substring(0, num) : string.Empty;
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x000F3DDC File Offset: 0x000F1FDC
	protected string GetRightText()
	{
		int num = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
		return (!string.IsNullOrEmpty(this.mValue) && num < this.mValue.Length) ? this.mValue.Substring(num) : string.Empty;
	}

	// Token: 0x0600201C RID: 8220 RVA: 0x000F3E34 File Offset: 0x000F2034
	protected string GetSelection()
	{
		if (string.IsNullOrEmpty(this.mValue) || this.mSelectionStart == this.mSelectionEnd)
		{
			return string.Empty;
		}
		int num = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
		int num2 = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
		return this.mValue.Substring(num, num2 - num);
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000F3E9C File Offset: 0x000F209C
	protected int GetCharUnderMouse()
	{
		Vector3[] worldCorners = this.label.worldCorners;
		Ray currentRay = UICamera.currentRay;
		Plane plane;
		plane..ctor(worldCorners[0], worldCorners[1], worldCorners[2]);
		float num;
		return (!plane.Raycast(currentRay, ref num)) ? 0 : (UIInput.mDrawStart + this.label.GetCharacterIndexAtPosition(currentRay.GetPoint(num), false));
	}

	// Token: 0x0600201E RID: 8222 RVA: 0x000F3F18 File Offset: 0x000F2118
	protected virtual void OnPress(bool isPressed)
	{
		if (isPressed && this.isSelected && this.label != null && (UICamera.currentScheme == UICamera.ControlScheme.Mouse || UICamera.currentScheme == UICamera.ControlScheme.Touch))
		{
			this.selectionEnd = this.GetCharUnderMouse();
			if (!Input.GetKey(304) && !Input.GetKey(303))
			{
				this.selectionStart = this.mSelectionEnd;
			}
		}
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x000156FC File Offset: 0x000138FC
	protected virtual void OnDrag(Vector2 delta)
	{
		if (this.label != null && (UICamera.currentScheme == UICamera.ControlScheme.Mouse || UICamera.currentScheme == UICamera.ControlScheme.Touch))
		{
			this.selectionEnd = this.GetCharUnderMouse();
		}
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x00015730 File Offset: 0x00013930
	private void OnDisable()
	{
		this.Cleanup();
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x000F3F94 File Offset: 0x000F2194
	protected virtual void Cleanup()
	{
		if (this.mHighlight)
		{
			this.mHighlight.enabled = false;
		}
		if (this.mCaret)
		{
			this.mCaret.enabled = false;
		}
		if (this.mBlankTex)
		{
			NGUITools.Destroy(this.mBlankTex);
			this.mBlankTex = null;
		}
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000F3FFC File Offset: 0x000F21FC
	public void Submit()
	{
		if (NGUITools.GetActive(this))
		{
			this.mValue = this.value;
			if (UIInput.current == null)
			{
				UIInput.current = this;
				EventDelegate.Execute(this.onSubmit);
				UIInput.current = null;
			}
			this.SaveToPlayerPrefs(this.mValue);
		}
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x000F4054 File Offset: 0x000F2254
	public void UpdateLabel()
	{
		if (this.label != null)
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			bool isSelected = this.isSelected;
			string value = this.value;
			bool flag = string.IsNullOrEmpty(value) && string.IsNullOrEmpty(Input.compositionString);
			this.label.color = ((!flag || isSelected) ? this.activeTextColor : this.mDefaultColor);
			string text;
			if (flag)
			{
				text = ((!isSelected) ? this.mDefaultText : string.Empty);
				this.RestoreLabelPivot();
			}
			else
			{
				if (this.inputType == UIInput.InputType.Password)
				{
					text = string.Empty;
					string text2 = "*";
					if (this.label.bitmapFont != null && this.label.bitmapFont.bmFont != null && this.label.bitmapFont.bmFont.GetGlyph(42) == null)
					{
						text2 = "x";
					}
					int i = 0;
					int length = value.Length;
					while (i < length)
					{
						text += text2;
						i++;
					}
				}
				else
				{
					text = value;
				}
				int num = (!isSelected) ? 0 : Mathf.Min(text.Length, this.cursorPosition);
				string text3 = text.Substring(0, num);
				if (isSelected)
				{
					text3 += Input.compositionString;
				}
				text = text3 + text.Substring(num, text.Length - num);
				if (isSelected && this.label.overflowMethod == UILabel.Overflow.ClampContent && this.label.maxLineCount == 1)
				{
					int num2 = this.label.CalculateOffsetToFit(text);
					if (num2 == 0)
					{
						UIInput.mDrawStart = 0;
						this.RestoreLabelPivot();
					}
					else if (num < UIInput.mDrawStart)
					{
						UIInput.mDrawStart = num;
						this.SetPivotToLeft();
					}
					else if (num2 < UIInput.mDrawStart)
					{
						UIInput.mDrawStart = num2;
						this.SetPivotToLeft();
					}
					else
					{
						num2 = this.label.CalculateOffsetToFit(text.Substring(0, num));
						if (num2 > UIInput.mDrawStart)
						{
							UIInput.mDrawStart = num2;
							this.SetPivotToRight();
						}
					}
					if (UIInput.mDrawStart != 0)
					{
						text = text.Substring(UIInput.mDrawStart, text.Length - UIInput.mDrawStart);
					}
				}
				else
				{
					UIInput.mDrawStart = 0;
					this.RestoreLabelPivot();
				}
			}
			this.label.text = text;
			if (isSelected)
			{
				int num3 = this.mSelectionStart - UIInput.mDrawStart;
				int num4 = this.mSelectionEnd - UIInput.mDrawStart;
				if (this.mBlankTex == null)
				{
					this.mBlankTex = new Texture2D(2, 2, 5, false);
					for (int j = 0; j < 2; j++)
					{
						for (int k = 0; k < 2; k++)
						{
							this.mBlankTex.SetPixel(k, j, Color.white);
						}
					}
					this.mBlankTex.Apply();
				}
				if (num3 != num4)
				{
					if (this.mHighlight == null)
					{
						this.mHighlight = NGUITools.AddWidget<UITexture>(this.label.cachedGameObject);
						this.mHighlight.name = "Input Highlight";
						this.mHighlight.mainTexture = this.mBlankTex;
						this.mHighlight.fillGeometry = false;
						this.mHighlight.pivot = this.label.pivot;
						this.mHighlight.SetAnchor(this.label.cachedTransform);
					}
					else
					{
						this.mHighlight.pivot = this.label.pivot;
						this.mHighlight.mainTexture = this.mBlankTex;
						this.mHighlight.MarkAsChanged();
						this.mHighlight.enabled = true;
					}
				}
				if (this.mCaret == null)
				{
					this.mCaret = NGUITools.AddWidget<UITexture>(this.label.cachedGameObject);
					this.mCaret.name = "Input Caret";
					this.mCaret.mainTexture = this.mBlankTex;
					this.mCaret.fillGeometry = false;
					this.mCaret.pivot = this.label.pivot;
					this.mCaret.SetAnchor(this.label.cachedTransform);
				}
				else
				{
					this.mCaret.pivot = this.label.pivot;
					this.mCaret.mainTexture = this.mBlankTex;
					this.mCaret.MarkAsChanged();
					this.mCaret.enabled = true;
				}
				if (num3 != num4)
				{
					this.label.PrintOverlay(num3, num4, this.mCaret.geometry, this.mHighlight.geometry, this.caretColor, this.selectionColor);
					this.mHighlight.enabled = this.mHighlight.geometry.hasVertices;
				}
				else
				{
					this.label.PrintOverlay(num3, num4, this.mCaret.geometry, null, this.caretColor, this.selectionColor);
					if (this.mHighlight != null)
					{
						this.mHighlight.enabled = false;
					}
				}
				this.mNextBlink = RealTime.time + 0.5f;
				this.mLastAlpha = this.label.finalAlpha;
			}
			else
			{
				this.Cleanup();
			}
		}
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x000F45C8 File Offset: 0x000F27C8
	protected void SetPivotToLeft()
	{
		Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.mPivot);
		pivotOffset.x = 0f;
		this.label.pivot = NGUIMath.GetPivot(pivotOffset);
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x000F4600 File Offset: 0x000F2800
	protected void SetPivotToRight()
	{
		Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.mPivot);
		pivotOffset.x = 1f;
		this.label.pivot = NGUIMath.GetPivot(pivotOffset);
	}

	// Token: 0x06002026 RID: 8230 RVA: 0x00015738 File Offset: 0x00013938
	protected void RestoreLabelPivot()
	{
		if (this.label != null && this.label.pivot != this.mPivot)
		{
			this.label.pivot = this.mPivot;
		}
	}

	// Token: 0x06002027 RID: 8231 RVA: 0x000F4638 File Offset: 0x000F2838
	protected char Validate(string text, int pos, char ch)
	{
		if (this.validation == UIInput.Validation.None || !base.enabled)
		{
			return ch;
		}
		if (this.validation == UIInput.Validation.Integer)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && pos == 0 && !text.Contains("-"))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Float)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && pos == 0 && !text.Contains("-"))
			{
				return ch;
			}
			if (ch == '.' && !text.Contains("."))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Alphanumeric)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch;
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Username)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch - 'A' + 'a';
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Name)
		{
			char c = (text.Length <= 0) ? ' ' : text.get_Chars(Mathf.Clamp(pos, 0, text.Length - 1));
			char c2 = (text.Length <= 0) ? '\n' : text.get_Chars(Mathf.Clamp(pos + 1, 0, text.Length - 1));
			if (ch >= 'a' && ch <= 'z')
			{
				if (c == ' ')
				{
					return ch - 'a' + 'A';
				}
				return ch;
			}
			else if (ch >= 'A' && ch <= 'Z')
			{
				if (c != ' ' && c != '\'')
				{
					return ch - 'A' + 'a';
				}
				return ch;
			}
			else if (ch == '\'')
			{
				if (c != ' ' && c != '\'' && c2 != '\'' && !text.Contains("'"))
				{
					return ch;
				}
			}
			else if (ch == ' ' && c != ' ' && c != '\'' && c2 != ' ' && c2 != '\'')
			{
				return ch;
			}
		}
		return '\0';
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x00015772 File Offset: 0x00013972
	protected void ExecuteOnChange()
	{
		if (UIInput.current == null && EventDelegate.IsValid(this.onChange))
		{
			UIInput.current = this;
			EventDelegate.Execute(this.onChange);
			UIInput.current = null;
		}
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x000157AB File Offset: 0x000139AB
	public void RemoveFocus()
	{
		this.isSelected = false;
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x000157B4 File Offset: 0x000139B4
	public void SaveValue()
	{
		this.SaveToPlayerPrefs(this.mValue);
	}

	// Token: 0x0600202B RID: 8235 RVA: 0x000F48A4 File Offset: 0x000F2AA4
	public void LoadValue()
	{
		if (!string.IsNullOrEmpty(this.savedAs))
		{
			string text = this.mValue.Replace("\\n", "\n");
			this.mValue = string.Empty;
			this.value = ((!PlayerPrefs.HasKey(this.savedAs)) ? text : PlayerPrefs.GetString(this.savedAs));
		}
	}

	// Token: 0x04002381 RID: 9089
	public static UIInput current;

	// Token: 0x04002382 RID: 9090
	public static UIInput selection;

	// Token: 0x04002383 RID: 9091
	public UILabel label;

	// Token: 0x04002384 RID: 9092
	public UIInput.InputType inputType;

	// Token: 0x04002385 RID: 9093
	public UIInput.OnReturnKey onReturnKey;

	// Token: 0x04002386 RID: 9094
	public UIInput.KeyboardType keyboardType;

	// Token: 0x04002387 RID: 9095
	public bool hideInput;

	// Token: 0x04002388 RID: 9096
	[NonSerialized]
	public bool selectAllTextOnFocus = true;

	// Token: 0x04002389 RID: 9097
	public UIInput.Validation validation;

	// Token: 0x0400238A RID: 9098
	public int characterLimit;

	// Token: 0x0400238B RID: 9099
	public string savedAs;

	// Token: 0x0400238C RID: 9100
	[SerializeField]
	[HideInInspector]
	private GameObject selectOnTab;

	// Token: 0x0400238D RID: 9101
	public Color activeTextColor = Color.white;

	// Token: 0x0400238E RID: 9102
	public Color caretColor = new Color(1f, 1f, 1f, 0.8f);

	// Token: 0x0400238F RID: 9103
	public Color selectionColor = new Color(1f, 0.8745098f, 0.5529412f, 0.5f);

	// Token: 0x04002390 RID: 9104
	public List<EventDelegate> onSubmit = new List<EventDelegate>();

	// Token: 0x04002391 RID: 9105
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x04002392 RID: 9106
	public UIInput.OnValidate onValidate;

	// Token: 0x04002393 RID: 9107
	[HideInInspector]
	[SerializeField]
	protected string mValue;

	// Token: 0x04002394 RID: 9108
	[NonSerialized]
	protected string mDefaultText = string.Empty;

	// Token: 0x04002395 RID: 9109
	[NonSerialized]
	protected Color mDefaultColor = Color.white;

	// Token: 0x04002396 RID: 9110
	[NonSerialized]
	protected float mPosition;

	// Token: 0x04002397 RID: 9111
	[NonSerialized]
	protected bool mDoInit = true;

	// Token: 0x04002398 RID: 9112
	[NonSerialized]
	protected UIWidget.Pivot mPivot;

	// Token: 0x04002399 RID: 9113
	[NonSerialized]
	protected bool mLoadSavedValue = true;

	// Token: 0x0400239A RID: 9114
	protected static int mDrawStart;

	// Token: 0x0400239B RID: 9115
	protected static string mLastIME = string.Empty;

	// Token: 0x0400239C RID: 9116
	[NonSerialized]
	protected int mSelectionStart;

	// Token: 0x0400239D RID: 9117
	[NonSerialized]
	protected int mSelectionEnd;

	// Token: 0x0400239E RID: 9118
	[NonSerialized]
	protected UITexture mHighlight;

	// Token: 0x0400239F RID: 9119
	[NonSerialized]
	protected UITexture mCaret;

	// Token: 0x040023A0 RID: 9120
	[NonSerialized]
	protected Texture2D mBlankTex;

	// Token: 0x040023A1 RID: 9121
	[NonSerialized]
	protected float mNextBlink;

	// Token: 0x040023A2 RID: 9122
	[NonSerialized]
	protected float mLastAlpha;

	// Token: 0x040023A3 RID: 9123
	[NonSerialized]
	protected string mCached = string.Empty;

	// Token: 0x040023A4 RID: 9124
	[NonSerialized]
	protected int mSelectMe = -1;

	// Token: 0x040023A5 RID: 9125
	[NonSerialized]
	private UIInputOnGUI mOnGUI;

	// Token: 0x020004F4 RID: 1268
	public enum InputType
	{
		// Token: 0x040023A7 RID: 9127
		Standard,
		// Token: 0x040023A8 RID: 9128
		AutoCorrect,
		// Token: 0x040023A9 RID: 9129
		Password
	}

	// Token: 0x020004F5 RID: 1269
	public enum Validation
	{
		// Token: 0x040023AB RID: 9131
		None,
		// Token: 0x040023AC RID: 9132
		Integer,
		// Token: 0x040023AD RID: 9133
		Float,
		// Token: 0x040023AE RID: 9134
		Alphanumeric,
		// Token: 0x040023AF RID: 9135
		Username,
		// Token: 0x040023B0 RID: 9136
		Name
	}

	// Token: 0x020004F6 RID: 1270
	public enum KeyboardType
	{
		// Token: 0x040023B2 RID: 9138
		Default,
		// Token: 0x040023B3 RID: 9139
		ASCIICapable,
		// Token: 0x040023B4 RID: 9140
		NumbersAndPunctuation,
		// Token: 0x040023B5 RID: 9141
		URL,
		// Token: 0x040023B6 RID: 9142
		NumberPad,
		// Token: 0x040023B7 RID: 9143
		PhonePad,
		// Token: 0x040023B8 RID: 9144
		NamePhonePad,
		// Token: 0x040023B9 RID: 9145
		EmailAddress
	}

	// Token: 0x020004F7 RID: 1271
	public enum OnReturnKey
	{
		// Token: 0x040023BB RID: 9147
		Default,
		// Token: 0x040023BC RID: 9148
		Submit,
		// Token: 0x040023BD RID: 9149
		NewLine
	}

	// Token: 0x020004F8 RID: 1272
	// (Invoke) Token: 0x0600202D RID: 8237
	public delegate char OnValidate(string text, int charIndex, char addedChar);
}
