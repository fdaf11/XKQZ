using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000374 RID: 884
	public class UISystemSetting : UILayer
	{
		// Token: 0x06001458 RID: 5208 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x000AF5F4 File Offset: 0x000AD7F4
		private void Start()
		{
			CtrlSystemSetting systemSettingController = this.m_SystemSettingController;
			systemSettingController.setView = (Action<SettingData>)Delegate.Combine(systemSettingController.setView, new Action<SettingData>(this.SetView));
			for (int i = 0; i < Screen.resolutions.Length; i++)
			{
				Resolution resolution = Screen.resolutions[i];
				if (resolution.width <= 1920)
				{
					this.CreatResolutionValue();
				}
			}
			this.m_ResolutionGrid.GetComponent<UIGrid>().Reposition();
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x000AF678 File Offset: 0x000AD878
		public override void OnKeyUp(KeyControl.Key key)
		{
			if (this.NowState == 3 || this.NowState == 4 || this.NowState == 7 || this.NowState == 9 || this.NowState == 10 || this.NowState == 12 || this.NowState == 13 || this.NowState == 14 || this.NowState == 6 || this.NowState == 5 || this.NowState == 8 || this.NowState == 11 || this.NowState == 15 || this.NowState == 16 || this.NowState == 17)
			{
				switch (key)
				{
				case KeyControl.Key.Up:
					this.SelectNextState(true, key);
					break;
				case KeyControl.Key.Down:
					this.SelectNextState(false, key);
					break;
				case KeyControl.Key.Left:
					this.SetNext(true, key);
					break;
				case KeyControl.Key.Right:
					this.SetNext(false, key);
					break;
				case KeyControl.Key.OK:
					base.OnCurrentClick();
					break;
				case KeyControl.Key.Cancel:
					base.EnterState(22);
					break;
				case KeyControl.Key.L1:
					this.TitleOnKey(true);
					break;
				case KeyControl.Key.R1:
					this.TitleOnKey(false);
					break;
				}
			}
			else if (this.NowState == 20)
			{
				switch (key)
				{
				case KeyControl.Key.Up:
				case KeyControl.Key.Down:
					this.SelectNextButton(key);
					break;
				case KeyControl.Key.OK:
					base.OnCurrentClick();
					break;
				case KeyControl.Key.Cancel:
					base.EnterState(3);
					break;
				case KeyControl.Key.L1:
					this.TitleOnKey(true);
					break;
				case KeyControl.Key.R1:
					this.TitleOnKey(false);
					break;
				}
			}
			else if (this.NowState == 18 || this.NowState == 19)
			{
				switch (key)
				{
				case KeyControl.Key.Up:
					this.SelectNextState(true, key);
					break;
				case KeyControl.Key.Down:
					this.SelectNextState(false, key);
					break;
				case KeyControl.Key.Left:
					this.ValueLeftRight(-0.05f, key);
					break;
				case KeyControl.Key.Right:
					this.ValueLeftRight(0.05f, key);
					break;
				case KeyControl.Key.Cancel:
					base.EnterState(22);
					break;
				case KeyControl.Key.L1:
					this.TitleOnKey(true);
					break;
				case KeyControl.Key.R1:
					this.TitleOnKey(false);
					break;
				}
			}
			else if (this.NowState == 2)
			{
				switch (key)
				{
				case KeyControl.Key.Up:
					this.SelectNextButton(key);
					this.selectIndex = int.Parse(this.current.name);
					this.SetScrollBar(this.selectIndex, 9, 28, true, this.m_KeySlider);
					break;
				case KeyControl.Key.Down:
					this.SelectNextButton(key);
					this.selectIndex = int.Parse(this.current.name);
					this.SetScrollBar(this.selectIndex, 9, 28, false, this.m_KeySlider);
					break;
				case KeyControl.Key.Left:
				case KeyControl.Key.Right:
					this.SelectNextButton(key);
					break;
				case KeyControl.Key.OK:
					base.OnCurrentClick();
					break;
				case KeyControl.Key.Cancel:
					base.EnterState(22);
					break;
				case KeyControl.Key.L1:
					this.TitleOnKey(true);
					break;
				case KeyControl.Key.R1:
					this.TitleOnKey(false);
					break;
				}
			}
			else if (this.NowState == 22 || this.NowState == 23)
			{
				switch (key)
				{
				case KeyControl.Key.Left:
				case KeyControl.Key.Right:
					this.SelectNextButton(key);
					break;
				case KeyControl.Key.OK:
					base.OnCurrentClick();
					break;
				}
			}
			Debug.Log(this.current);
			Debug.Log((UISystemSetting.eState)this.NowState);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x000AFA74 File Offset: 0x000ADC74
		private void ValueLeftRight(float LeftRight, KeyControl.Key key)
		{
			if (this.NowState == 18)
			{
				this.MusicValueOn(true);
				this.m_MusicSlider.value += LeftRight;
			}
			else
			{
				this.SoundValueOn(true);
				this.m_SoundSlider.value += LeftRight;
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x000AFAC8 File Offset: 0x000ADCC8
		private void SetNext(bool bReverse, KeyControl.Key key)
		{
			if (this.NowState == 5)
			{
				this.SetNextBar(bReverse, new Action<GameObject>(this.QualityPointOnClick), this.m_QualityPointList);
			}
			else if (this.NowState == 8)
			{
				this.SetNextBar(bReverse, new Action<GameObject>(this.ShadowDistancePointOnClick), this.m_ShadowDistancePointList);
			}
			else if (this.NowState == 11)
			{
				this.SetNextBar(bReverse, new Action<GameObject>(this.ImageQualityPointOnClick), this.m_ImageQualityPointList);
			}
			else if (this.NowState == 15)
			{
				this.SetNextBar(bReverse, new Action<GameObject>(this.ResourcesCountPointOnClick), this.m_ResourcesCountPointList);
			}
			else if (this.NowState == 17)
			{
				this.SetNextBar(bReverse, new Action<GameObject>(this.SSAAMultiplierPointOnClick), this.m_SSAAMultiplierPointList);
			}
			else
			{
				this.SelectNextButton(key);
			}
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x000AFBB4 File Offset: 0x000ADDB4
		private void SetNextBar(bool bReverse, Action<GameObject> onClick, List<Control> control)
		{
			int num = this.m_IndexArray[this.NowState];
			if (bReverse)
			{
				num--;
			}
			else
			{
				num++;
			}
			num = Mathf.Clamp(num, 0, control.Count - 1);
			this.m_IndexArray[this.NowState] = num;
			onClick.Invoke(control[num].GameObject);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x000AFC14 File Offset: 0x000ADE14
		private void SelectNextState(bool bReverse, KeyControl.Key key)
		{
			int num;
			if (bReverse)
			{
				num = this.NowState - 1;
			}
			else
			{
				num = this.NowState + 1;
			}
			num = Mathf.Clamp(num, 3, 19);
			if (bReverse)
			{
				if (this.NowState == 5 || this.NowState == 18)
				{
					this.selectIndex -= 2;
				}
				else
				{
					this.selectIndex--;
				}
			}
			else if (this.NowState == 4 || this.NowState == 17)
			{
				this.selectIndex += 2;
			}
			else
			{
				this.selectIndex++;
			}
			this.selectIndex = Mathf.Clamp(this.selectIndex, 0, 18);
			this.SetScrollBar(this.selectIndex, 12, 19, bReverse, this.m_SystemSlider);
			base.EnterState(num);
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x000AFCFC File Offset: 0x000ADEFC
		protected override void SetScrollBar(int iSelectIdx, int iShowAmount, int iMaxAmount, bool bReverse, Control ScrollBar)
		{
			float num = ScrollBar.sliderValue * (float)(iMaxAmount - iShowAmount) + 1f;
			int num2 = Mathf.Clamp((int)Mathf.Round(num), 1, iMaxAmount);
			int num3 = num2 + iShowAmount - 1;
			if (ScrollBar == this.m_SystemSlider)
			{
				if (!bReverse && iSelectIdx == 17)
				{
					num2++;
					num3++;
				}
				else if (bReverse && iSelectIdx == 1)
				{
					num2--;
					num3--;
				}
			}
			else
			{
				if (bReverse && iSelectIdx == 2)
				{
					ScrollBar.sliderValue = 0f;
					return;
				}
				if (!bReverse && iSelectIdx == 22)
				{
					num2++;
					num3++;
				}
			}
			iSelectIdx++;
			GameDebugTool.Log("iSelectIdx : " + iSelectIdx);
			GameDebugTool.Log(string.Concat(new object[]
			{
				"sliderValue : ",
				ScrollBar.sliderValue,
				" f : ",
				Mathf.Round(num),
				" start : ",
				num2,
				" end : ",
				num3
			}));
			Debug.Log("iSelectIdx : " + iSelectIdx);
			Debug.Log(string.Concat(new object[]
			{
				"sliderValue : ",
				ScrollBar.sliderValue,
				" f : ",
				Mathf.Round(num),
				" start : ",
				num2,
				" end : ",
				num3
			}));
			if (iSelectIdx < num2 || iSelectIdx > num3)
			{
				ScrollBar.GameObject.SetActive(true);
				float sliderValue;
				if (bReverse)
				{
					sliderValue = Mathf.Clamp((float)(num3 - iShowAmount - 1) / (float)(iMaxAmount - iShowAmount), 0f, 1f);
				}
				else
				{
					sliderValue = Mathf.Clamp((float)(num3 - iShowAmount + 1) / (float)(iMaxAmount - iShowAmount), 0f, 1f);
				}
				ScrollBar.sliderValue = sliderValue;
				GameDebugTool.Log("sliderValue : " + ScrollBar.sliderValue);
			}
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x000AFF1C File Offset: 0x000AE11C
		private void TitleOnKey(bool reverse)
		{
			if (reverse)
			{
				this.selectType--;
			}
			else
			{
				this.selectType++;
			}
			this.selectType = Mathf.Clamp(this.selectType, 1, 2);
			if (this.selectType == this.NowState)
			{
				return;
			}
			this.TitleTypeOnClick(this.m_TitleTypeList[this.selectType - 1].GameObject);
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x000AFF94 File Offset: 0x000AE194
		protected override void OnStateEnter(int state)
		{
			this.current = null;
			switch (state)
			{
			case 0:
				this.Hide();
				break;
			case 1:
				this.m_CurrentArray[2] = null;
				if (GameCursor.IsShow)
				{
					base.EnterState(3);
				}
				this.InitSystemView();
				break;
			case 2:
				this.InitKeyView();
				break;
			case 3:
				this.selectIndex = 0;
				this.m_FromResolution.GameObject.SetActive(false);
				this.m_ResolutionOnClick.GameObject.SetActive(false);
				this.m_MultipleOptionOn.GameObject.SetActive(false);
				break;
			case 4:
				this.m_ResolutionOn.GameObject.SetActive(false);
				break;
			case 5:
			case 8:
			case 11:
			case 15:
			case 17:
				this.m_MusicValueForeground.UISprite.color = Color.white;
				this.m_MultipleOptionOn.GameObject.SetActive(false);
				break;
			case 16:
				for (int i = 0; i < this.m_WarningOptionList.Count; i++)
				{
					this.m_WarningOptionList[i].SpriteName = "ui_sys_09";
				}
				this.m_WarningForm.GameObject.SetActive(false);
				break;
			case 18:
				this.m_SoundValueForeground.UISprite.color = Color.white;
				break;
			case 19:
				this.m_MusicValueForeground.UISprite.color = Color.white;
				break;
			case 20:
				this.m_FromResolution.GameObject.SetActive(true);
				break;
			case 22:
				this.m_DetermineForm.GameObject.SetActive(true);
				break;
			case 23:
				this.m_WarningForm.GameObject.SetActive(true);
				break;
			}
			if (!GameCursor.IsShow)
			{
				if (!this.controls.ContainsKey(this.NowState))
				{
					return;
				}
				if (this.m_CurrentArray[this.NowState] == null)
				{
					List<UIEventListener> list = this.controls[this.NowState];
					if (list != null && list.Count > 0)
					{
						this.current = list[0];
					}
				}
				else
				{
					this.current = this.m_CurrentArray[this.NowState];
				}
				if (!this.current.gameObject.collider.enabled || !this.current.gameObject.activeSelf)
				{
					return;
				}
				base.SetCurrent(this.current, true);
			}
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x000B0250 File Offset: 0x000AE450
		protected override void OnStateExit(int state)
		{
			if (state == 2 || state == 3)
			{
				this.m_CurrentArray[state] = this.current;
			}
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x000B0288 File Offset: 0x000AE488
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UISystemSetting.<>f__switch$map5C == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(61);
					dictionary.Add("Group", 0);
					dictionary.Add("TitleType", 1);
					dictionary.Add("TitleOn", 2);
					dictionary.Add("TitleOnClick", 3);
					dictionary.Add("System", 4);
					dictionary.Add("HotKey", 5);
					dictionary.Add("ResolutionValue", 6);
					dictionary.Add("ResolutionOn", 7);
					dictionary.Add("ResolutionOnClick", 8);
					dictionary.Add("FromResolution", 9);
					dictionary.Add("ResolutionGrid", 10);
					dictionary.Add("FullScreenOption", 11);
					dictionary.Add("FullScreenOptionOnClick", 12);
					dictionary.Add("QualityPoint", 13);
					dictionary.Add("QualityThumb", 14);
					dictionary.Add("QualityValue", 15);
					dictionary.Add("ShadowDistancePoint", 16);
					dictionary.Add("ShadowDistanceThumb", 17);
					dictionary.Add("ShadowDistanceValue", 18);
					dictionary.Add("ImageQualityPoint", 19);
					dictionary.Add("ImageQualityThumb", 20);
					dictionary.Add("ImageQualityValue", 21);
					dictionary.Add("ResourcesCountPoint", 22);
					dictionary.Add("ResourcesCountThumb", 23);
					dictionary.Add("ResourcesCountValue", 24);
					dictionary.Add("OwnSettingOption", 25);
					dictionary.Add("OwnSettingOnClick", 26);
					dictionary.Add("ShadowOption", 27);
					dictionary.Add("ShadowOptionOnClick", 28);
					dictionary.Add("VSyncOption", 29);
					dictionary.Add("VSyncOptionOnClick", 30);
					dictionary.Add("AntiAliasingOption", 31);
					dictionary.Add("AntiAliasingOptionOnClick", 32);
					dictionary.Add("SSAOOption", 33);
					dictionary.Add("SSAOOptionOnClick", 34);
					dictionary.Add("LensEffectsOption", 35);
					dictionary.Add("LensEffectsOptionOnClick", 36);
					dictionary.Add("BloomOption", 37);
					dictionary.Add("BloomOptionOnClick", 38);
					dictionary.Add("SSAAOption", 39);
					dictionary.Add("SSAAOptionOnClick", 40);
					dictionary.Add("SSAAMultiplierPoint", 41);
					dictionary.Add("SSAAMultiplierThumb", 42);
					dictionary.Add("SSAAMultiplierValue", 43);
					dictionary.Add("MultipleOptionOn", 44);
					dictionary.Add("MusicValueSlider", 45);
					dictionary.Add("MusicValueForeground", 46);
					dictionary.Add("SoundValueSlider", 47);
					dictionary.Add("SoundValueForeground", 48);
					dictionary.Add("SystemSlider", 49);
					dictionary.Add("SystemScrollView", 50);
					dictionary.Add("LblKey", 51);
					dictionary.Add("KeyOptionOn", 52);
					dictionary.Add("KeyOptionOnClick", 53);
					dictionary.Add("KeySlider", 54);
					dictionary.Add("KeyScrollView", 55);
					dictionary.Add("SystemExitSprite", 56);
					dictionary.Add("DetermineForm", 57);
					dictionary.Add("DeterminOption", 58);
					dictionary.Add("WarningForm", 59);
					dictionary.Add("WarningOption", 60);
					UISystemSetting.<>f__switch$map5C = dictionary;
				}
				int num;
				if (UISystemSetting.<>f__switch$map5C.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
					{
						Control control = sender;
						control.OnClick += this.TitleTypeOnClick;
						control.OnHover += this.TitleTypeOnHover;
						this.m_TitleTypeList.Add(control);
						break;
					}
					case 2:
						this.m_TitleOn = sender;
						break;
					case 3:
						this.m_TitleOnClick = sender;
						break;
					case 4:
						this.m_System = sender;
						break;
					case 5:
						this.m_HotKey = sender;
						break;
					case 6:
					{
						Control control = sender;
						control.OnHover += this.ResolutionValueOnHover;
						control.OnKeySelect += this.ResolutionValueOnKeySelect;
						control.OnClick += this.ResolutionValueOnClick;
						base.SetInputButton(1, control.Listener);
						base.SetInputButton(3, control.Listener);
						this.m_ResolutionValue = sender;
						break;
					}
					case 7:
						this.m_ResolutionOn = sender;
						break;
					case 8:
						this.m_ResolutionOnClick = sender;
						break;
					case 9:
					{
						Control control = sender;
						control.OnClick += this.FromResolutionOnClick;
						this.m_FromResolution = sender;
						break;
					}
					case 10:
						this.m_ResolutionGrid = sender;
						break;
					case 11:
					{
						Control control = sender;
						control.OnClick += this.FullScreenOptionOnClick;
						control.OnHover += this.OptionOnHover;
						control.OnKeySelect += this.OptionOnKeySelect;
						base.SetInputButton(4, control.Listener);
						this.m_FullScreenOptionList.Add(control);
						break;
					}
					case 12:
						this.m_FullScreenOptionOnClick = sender;
						break;
					case 13:
					{
						Control control = sender;
						control.OnClick += this.QualityPointOnClick;
						base.SetInputButton(5, control.Listener);
						this.m_QualityPointList.Add(control);
						break;
					}
					case 14:
						this.m_QualityThumb = sender;
						break;
					case 15:
						this.m_QualityValue = sender;
						break;
					case 16:
					{
						Control control = sender;
						control.OnClick += this.ShadowDistancePointOnClick;
						base.SetInputButton(8, control.Listener);
						this.m_ShadowDistancePointList.Add(control);
						break;
					}
					case 17:
						this.m_ShadowDistanceThumb = sender;
						break;
					case 18:
						this.m_ShadowDistanceValue = sender;
						break;
					case 19:
					{
						Control control = sender;
						control.OnClick += this.ImageQualityPointOnClick;
						base.SetInputButton(11, control.Listener);
						this.m_ImageQualityPointList.Add(control);
						break;
					}
					case 20:
						this.m_ImageQualityThumb = sender;
						break;
					case 21:
						this.m_ImageQualityValue = sender;
						break;
					case 22:
					{
						Control control = sender;
						control.OnClick += this.ResourcesCountPointOnClick;
						base.SetInputButton(15, control.Listener);
						this.m_ResourcesCountPointList.Add(control);
						break;
					}
					case 23:
						this.m_ResourcesCountThumb = sender;
						break;
					case 24:
						this.m_ResourcesCountValue = sender;
						break;
					case 25:
					{
						Control control = sender;
						control.OnClick += this.OwnSettingOptionOnClick;
						control.OnHover += this.OptionOnHover;
						control.OnKeySelect += this.OptionOnKeySelect;
						base.SetInputButton(6, control.Listener);
						this.m_OwnSettingOption = sender;
						break;
					}
					case 26:
						this.m_OwnSettingOnClick = sender;
						break;
					case 27:
					{
						Control control = sender;
						control.OnClick += this.ShadowOptionOnClick;
						control.OnHover += this.OptionOnHover;
						control.OnKeySelect += this.OptionOnKeySelect;
						base.SetInputButton(7, control.Listener);
						this.m_ShadowOptionList.Add(control);
						break;
					}
					case 28:
						this.m_ShadowOptionOnClick = sender;
						break;
					case 29:
					{
						Control control = sender;
						control.OnClick += this.VSyncOptionOnClick;
						control.OnHover += this.OptionOnHover;
						control.OnKeySelect += this.OptionOnKeySelect;
						base.SetInputButton(9, control.Listener);
						this.m_VSyncOptionList.Add(control);
						break;
					}
					case 30:
						this.m_VSyncOptionOnClick = sender;
						break;
					case 31:
					{
						Control control = sender;
						control.OnClick += this.AntiAliasingOptionOnClick;
						control.OnHover += this.OptionOnHover;
						control.OnKeySelect += this.OptionOnKeySelect;
						base.SetInputButton(10, control.Listener);
						this.m_AntiAliasingOptionList.Add(control);
						break;
					}
					case 32:
						this.m_AntiAliasingOptionClick = sender;
						break;
					case 33:
					{
						Control control = sender;
						control.OnClick += this.SSAOOptionOnClick;
						control.OnHover += this.OptionOnHover;
						control.OnKeySelect += this.OptionOnKeySelect;
						base.SetInputButton(12, control.Listener);
						this.m_SSAOOptionList.Add(control);
						break;
					}
					case 34:
						this.m_SSAOOptionClick = sender;
						break;
					case 35:
					{
						Control control = sender;
						control.OnClick += this.LensEffectsOptionOnClick;
						control.OnHover += this.OptionOnHover;
						control.OnKeySelect += this.OptionOnKeySelect;
						base.SetInputButton(13, control.Listener);
						this.m_LensEffectsOptionList.Add(control);
						break;
					}
					case 36:
						this.m_LensEffectsOptionClick = sender;
						break;
					case 37:
					{
						Control control = sender;
						control.OnClick += this.BloomOptionOnClick;
						control.OnHover += this.OptionOnHover;
						control.OnKeySelect += this.OptionOnKeySelect;
						base.SetInputButton(14, control.Listener);
						this.m_BloomOptionList.Add(control);
						break;
					}
					case 38:
						this.m_BloomOptionOnClick = sender;
						break;
					case 39:
					{
						Control control = sender;
						control.OnClick += this.SSAAOptionOnClick;
						control.OnHover += this.OptionOnHover;
						control.OnKeySelect += this.OptionOnKeySelect;
						base.SetInputButton(16, control.Listener);
						this.m_SSAAOptionList.Add(control);
						break;
					}
					case 40:
						this.m_SSAAOptionOnClick = sender;
						break;
					case 41:
					{
						Control control = sender;
						control.OnClick += this.SSAAMultiplierPointOnClick;
						base.SetInputButton(17, control.Listener);
						this.m_SSAAMultiplierPointList.Add(control);
						break;
					}
					case 42:
						this.m_SSAAMultiplierThumb = sender;
						break;
					case 43:
						this.m_SSAAMultiplierValue = sender;
						break;
					case 44:
						this.m_MultipleOptionOn = sender;
						break;
					case 45:
					{
						Control control = sender;
						control.OnHover += this.MusicValueOnHover;
						control.OnKeySelect += this.MusicValueOnKeySelect;
						base.SetInputButton(18, control.Listener);
						this.m_MusicSlider = sender.GetComponent<UISlider>();
						EventDelegate.Add(this.m_MusicSlider.onChange, new EventDelegate.Callback(this.MusicValueOnChange));
						this.m_MusicValueSlider = sender;
						break;
					}
					case 46:
						this.m_MusicValueForeground = sender;
						break;
					case 47:
					{
						Control control = sender;
						control.OnHover += this.SoundValuOnHover;
						control.OnKeySelect += this.SoundValuOnKeySelect;
						base.SetInputButton(19, control.Listener);
						this.m_SoundSlider = sender.GetComponent<UISlider>();
						EventDelegate.Add(this.m_SoundSlider.onChange, new EventDelegate.Callback(this.SoundValueOnChange));
						this.m_SoundValueSlider = sender;
						break;
					}
					case 48:
						this.m_SoundValueForeground = sender;
						break;
					case 49:
						this.m_SystemSlider = sender;
						break;
					case 50:
						this.m_SystemScrollView = sender;
						break;
					case 51:
					{
						Control control = sender;
						control = sender;
						control.OnHover += this.LblKeyOnHover;
						control.OnKeySelect += this.LblKeyOnKeySelect;
						control.OnClick += this.LblKeyOnClick;
						base.SetInputButton(2, control.Listener);
						this.m_LblKeyList.Add(control);
						sender.name = this.keyIndex.ToString();
						this.keyIndex++;
						if (this.keyIndex > 27)
						{
							this.keyIndex = 2;
						}
						else if (this.keyIndex == 21)
						{
							this.keyIndex = 22;
						}
						break;
					}
					case 52:
					{
						Control control = sender;
						this.m_KeyOptionOn = sender;
						break;
					}
					case 53:
						this.m_KeyOptionOnClick = sender;
						break;
					case 54:
						this.m_KeySlider = sender;
						break;
					case 55:
						this.m_KeyScrollView = sender;
						break;
					case 56:
					{
						Control control = sender;
						control.OnHover += this.ExitSpriteOnHover;
						control.OnClick += this.ExitSpriteOnClick;
						this.m_SystemExitSprite = sender;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					case 57:
						this.m_DetermineForm = sender;
						break;
					case 58:
					{
						Control control = sender;
						control.OnHover += this.FormOptionOnHover;
						control.OnKeySelect += this.FormOptionOnKeySelect;
						control.OnClick += this.DeterminOptionOnClick;
						this.m_DeterminOptionList.Add(control);
						base.SetInputButton(22, control.Listener);
						break;
					}
					case 59:
						this.m_WarningForm = sender;
						break;
					case 60:
					{
						Control control = sender;
						control.OnHover += this.FormOptionOnHover;
						control.OnKeySelect += this.FormOptionOnKeySelect;
						control.OnClick += this.WarningOptionOnClick;
						this.m_WarningOptionList.Add(control);
						base.SetInputButton(23, control.Listener);
						break;
					}
					}
				}
			}
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x000B1118 File Offset: 0x000AF318
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			GameGlobal.m_bCFormOpen = this.m_bShow;
			Game.g_InputManager.Push(this);
			this.m_Group.GameObject.SetActive(this.m_bShow);
			this.m_Group.GameObject.SetActive(true);
			this.interim = this.m_SystemSettingController.UpdateSystemData();
			this.TitleTypeOnClick(this.m_TitleTypeList[0].GameObject);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x000B11A0 File Offset: 0x000AF3A0
		public override void Hide()
		{
			this.m_SystemSettingController.GameSettingOnClose();
			for (int i = 0; i < this.m_DeterminOptionList.Count; i++)
			{
				this.m_DeterminOptionList[i].SpriteName = "ui_sys_09";
			}
			this.m_DetermineForm.GameObject.SetActive(false);
			this.m_Group.GameObject.SetActive(false);
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			GameGlobal.m_bCFormOpen = this.m_bShow;
			Game.g_InputManager.Pop();
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0000D198 File Offset: 0x0000B398
		private void FormOptionOnKeySelect(GameObject go, bool bSelect)
		{
			this.FormOptionOn(go, bSelect);
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0000D1A2 File Offset: 0x0000B3A2
		private void FormOptionOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.FormOptionOn(go, bHover);
			}
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00096C50 File Offset: 0x00094E50
		private void FormOptionOn(GameObject go, bool bHover)
		{
			UISprite component = go.GetComponent<UISprite>();
			if (bHover)
			{
				component.spriteName = "ui_sys_10";
			}
			else
			{
				component.spriteName = "ui_sys_09";
			}
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0000D1B6 File Offset: 0x0000B3B6
		private void WarningOptionOnClick(GameObject go)
		{
			if (go.GetComponent<BtnData>().m_iBtnID == 0)
			{
				this.SSAAOpen();
			}
			base.EnterState(16);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0000D1D6 File Offset: 0x0000B3D6
		private void DeterminOptionOnClick(GameObject go)
		{
			if (go.GetComponent<BtnData>().m_iBtnID == 0)
			{
				this.m_SystemSettingController.IniSettingData(this.interim);
			}
			base.EnterState(0);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0000D200 File Offset: 0x0000B400
		private void ExitSpriteOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			go.GetComponent<UISprite>().spriteName = "cdata_027";
			base.EnterState(22);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x000A3240 File Offset: 0x000A1440
		private void ExitSpriteOnHover(GameObject go, bool bHover)
		{
			UISprite component = go.GetComponent<UISprite>();
			if (bHover)
			{
				component.spriteName = "cdata_028";
			}
			else
			{
				component.spriteName = "cdata_027";
			}
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x000B1238 File Offset: 0x000AF438
		private void TitleTypeOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_TitleOn.GameObject.SetActive(false);
			this.SetControl(this.m_TitleOnClick, go, 120f, 1.3f, 0f);
			this.selectType = go.GetComponent<BtnData>().m_iBtnID;
			this.current = null;
			base.EnterState(this.selectType);
			if (this.selectType == 2)
			{
				this.selectIndex = 2;
				this.m_KeyScrollView.GetComponent<UIScrollView>().ResetPosition();
			}
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0000D226 File Offset: 0x0000B426
		private void TitleTypeOnHover(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.SetControl(this.m_TitleOn, go, 120f, 1.3f, 0f);
			}
			else
			{
				this.m_TitleOn.GameObject.SetActive(false);
			}
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x000B12C8 File Offset: 0x000AF4C8
		private void ResolutionValueOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_ResolutionOn.GameObject.SetActive(false);
			this.SetControl(this.m_ResolutionOnClick, go, 0f, 10f, 0f);
			base.EnterState(20);
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0000D260 File Offset: 0x0000B460
		private void ResolutionValueOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.ResolutionValueOn(go, bHover);
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0000D274 File Offset: 0x0000B474
		private void ResolutionValueOnKeySelect(GameObject go, bool bSelect)
		{
			this.ResolutionValueOn(go, bSelect);
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x000B1318 File Offset: 0x000AF518
		private void ResolutionValueOn(GameObject goes, bool bOn)
		{
			if (bOn)
			{
				this.SetControl(this.m_ResolutionOn, goes, 0f, 10f, 0f);
				if (this.NowState != 20)
				{
					base.EnterState(3);
				}
			}
			else
			{
				this.m_ResolutionOn.GameObject.SetActive(false);
			}
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x000B1374 File Offset: 0x000AF574
		private void ResolutionValueOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			string text = go.GetComponent<UILabel>().text;
			this.m_ResolutionValue.Text = text;
			int num = text.IndexOf("x");
			if (num >= 0)
			{
				string text2 = text.Substring(0, num);
				string text3 = text.Substring(num + 1, text.Length - num - 1);
				this.interim.iScreenWidth = int.Parse(text2);
				this.interim.iScreenHeight = int.Parse(text3);
			}
			base.EnterState(3);
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0000D27E File Offset: 0x0000B47E
		private void FromResolutionOnClick(GameObject go)
		{
			base.EnterState(3);
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x000B1400 File Offset: 0x000AF600
		private void FullScreenOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.SetControl(this.m_FullScreenOptionOnClick, go, 0f, 0f, 0f);
			this.interim.bFullScreen = this.BtnIDToBool(iBtnID);
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x000B1454 File Offset: 0x000AF654
		private void QualityPointOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.m_QualityValue.Text = Game.StringTable.GetString(100089 + iBtnID);
			this.SetControl(this.m_QualityThumb, go, 0f, 0f, 0f);
			this.m_OwnSettingOnClick.GameObject.SetActive(false);
			this.interim.bOwnSetting = false;
			SettingDataNode settingDataNode = Game.SettingData.GetSettingDataNode(iBtnID);
			this.SetEffectView(settingDataNode);
			this.interim.settingDataNode = settingDataNode;
			this.interim.settingDataNode.m_iQualityLevel = iBtnID;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x000B1500 File Offset: 0x000AF700
		private void OwnSettingOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (this.m_OwnSettingOnClick.GameObject.activeSelf)
			{
				for (int i = 0; i < this.m_QualityPointList.Count; i++)
				{
					if (this.m_QualityPointList[i].GetComponent<BtnData>().m_iBtnID == this.interim.settingDataNode.m_iQualityLevel)
					{
						this.QualityPointOnClick(this.m_QualityPointList[i].GameObject);
					}
				}
			}
			else
			{
				this.m_OwnSettingOnClick.GameObject.SetActive(true);
			}
			this.interim.bOwnSetting = this.m_OwnSettingOnClick.GameObject.activeSelf;
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x000B15C0 File Offset: 0x000AF7C0
		private void ShadowOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.OptionOnClickBase(go, this.m_ShadowOptionOnClick);
			this.interim.settingDataNode.m_bShadow = this.BtnIDToBool(iBtnID);
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x000B160C File Offset: 0x000AF80C
		private void ShadowDistancePointOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.OptionOnClickBase(go, this.m_ShadowDistanceThumb);
			this.m_ShadowDistanceValue.Text = iBtnID.ToString();
			this.interim.settingDataNode.m_fShadowDistance = (float)iBtnID;
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0000D287 File Offset: 0x0000B487
		private void VSyncOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.OptionOnClickBase(go, this.m_VSyncOptionOnClick);
			this.interim.settingDataNode.m_iVSync = go.GetComponent<BtnData>().m_iBtnID;
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x000B1664 File Offset: 0x000AF864
		private void AntiAliasingOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.OptionOnClickBase(go, this.m_AntiAliasingOptionClick);
			this.interim.settingDataNode.m_iAntiAliasing = go.GetComponent<BtnData>().m_iBtnID;
			if (this.interim.settingDataNode.m_iAntiAliasing == 2 && this.interim.bSSAA)
			{
				for (int i = 0; i < this.m_SSAAOptionList.Count; i++)
				{
					if (this.m_SSAAOptionList[i].GetComponent<BtnData>().m_iBtnID == 0)
					{
						this.SSAAOptionOnClick(this.m_SSAAOptionList[i].GameObject);
					}
				}
			}
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x000B171C File Offset: 0x000AF91C
		private void ImageQualityPointOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.OptionOnClickBase(go, this.m_ImageQualityThumb);
			this.m_ImageQualityValue.Text = Game.StringTable.GetString(100103 - iBtnID);
			this.interim.settingDataNode.m_iImageQuality = iBtnID;
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x000B177C File Offset: 0x000AF97C
		private void SSAOOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.OptionOnClickBase(go, this.m_SSAOOptionClick);
			this.interim.settingDataNode.m_bSSAO = this.BtnIDToBool(iBtnID);
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x000B17C8 File Offset: 0x000AF9C8
		private void LensEffectsOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.OptionOnClickBase(go, this.m_LensEffectsOptionClick);
			this.interim.settingDataNode.m_bLensEffects = this.BtnIDToBool(iBtnID);
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x000B1814 File Offset: 0x000AFA14
		private void BloomOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.OptionOnClickBase(go, this.m_BloomOptionOnClick);
			this.interim.settingDataNode.m_bBloom = this.BtnIDToBool(iBtnID);
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x000B1860 File Offset: 0x000AFA60
		private void ResourcesCountPointOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.OptionOnClickBase(go, this.m_ResourcesCountThumb);
			this.m_ResourcesCountValue.Text = Game.StringTable.GetString(100100 + iBtnID);
			this.interim.settingDataNode.m_iResourcesCount = iBtnID;
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x000B18C0 File Offset: 0x000AFAC0
		private void SSAAOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			if (iBtnID == 1)
			{
				base.EnterState(23);
			}
			else
			{
				this.OptionOnClickBase(go, this.m_SSAAOptionOnClick);
				this.interim.bSSAA = this.BtnIDToBool(0);
			}
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x000B1918 File Offset: 0x000AFB18
		private void SSAAOpen()
		{
			if (this.interim.settingDataNode.m_iAntiAliasing == 2)
			{
				for (int i = 0; i < this.m_AntiAliasingOptionList.Count; i++)
				{
					if (this.m_AntiAliasingOptionList[i].GetComponent<BtnData>().m_iBtnID == 0)
					{
						this.AntiAliasingOptionOnClick(this.m_AntiAliasingOptionList[i].GameObject);
					}
				}
			}
			for (int j = 0; j < this.m_SSAAOptionList.Count; j++)
			{
				if (this.m_SSAAOptionList[j].GetComponent<BtnData>().m_iBtnID == 1)
				{
					this.OptionOnClickBase(this.m_SSAAOptionList[j].GameObject, this.m_SSAAOptionOnClick);
					this.interim.bSSAA = this.BtnIDToBool(1);
				}
			}
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x000B19F0 File Offset: 0x000AFBF0
		private void SSAAMultiplierPointOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.OptionOnClickBase(go, this.m_SSAAMultiplierThumb);
			float fSSAA = (float)iBtnID / 10f + 1f;
			this.m_SSAAMultiplierValue.Text = fSSAA.ToString();
			this.interim.fSSAA = fSSAA;
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x000B1A50 File Offset: 0x000AFC50
		private void OptionOnClickBase(GameObject goes, Control onClick)
		{
			this.SetControl(onClick, goes, 0f, 0f, 0f);
			if (onClick == this.m_SSAAMultiplierThumb)
			{
				return;
			}
			this.m_OwnSettingOnClick.GameObject.SetActive(true);
			this.interim.bOwnSetting = true;
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x000B1AA0 File Offset: 0x000AFCA0
		private void OptionOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.OptionOn(go, bHover);
				if (bHover)
				{
					this.selectIndex = go.GetComponent<BtnData>().m_iBtnType;
					int num;
					if (this.selectIndex != 1)
					{
						num = 2;
					}
					else
					{
						num = 3;
					}
					base.EnterState(go.GetComponent<BtnData>().m_iBtnType + num);
					this.current = UIEventListener.Get(go);
				}
			}
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0000D2BD File Offset: 0x0000B4BD
		private void OptionOnKeySelect(GameObject go, bool bSelect)
		{
			this.OptionOn(go, bSelect);
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0000D2C7 File Offset: 0x0000B4C7
		private void OptionOn(GameObject goes, bool bOn)
		{
			if (bOn)
			{
				this.SetControl(this.m_MultipleOptionOn, goes, 0f, 0f, 0f);
			}
			else
			{
				this.m_MultipleOptionOn.GameObject.SetActive(false);
			}
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0000D301 File Offset: 0x0000B501
		private void MusicValueOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.MusicValueOn(bHover);
			}
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0000D314 File Offset: 0x0000B514
		private void MusicValueOnKeySelect(GameObject go, bool bSelect)
		{
			this.MusicValueOn(bSelect);
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x000B1B0C File Offset: 0x000AFD0C
		private void MusicValueOn(bool bOn)
		{
			if (bOn)
			{
				this.m_MusicValueForeground.UISprite.color = new Color(0f, 0.627451f, 1f, 1f);
				base.EnterState(18);
			}
			else
			{
				this.m_MusicValueForeground.UISprite.color = Color.white;
			}
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0000D31D File Offset: 0x0000B51D
		private void SoundValuOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.SoundValueOn(bHover);
			}
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0000D330 File Offset: 0x0000B530
		private void SoundValuOnKeySelect(GameObject go, bool bSelect)
		{
			this.SoundValueOn(bSelect);
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x000B1B6C File Offset: 0x000AFD6C
		private void SoundValueOn(bool bOn)
		{
			if (bOn)
			{
				this.m_SoundValueForeground.UISprite.color = new Color(0f, 0.627451f, 1f, 1f);
				base.EnterState(19);
			}
			else
			{
				this.m_SoundValueForeground.UISprite.color = Color.white;
			}
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x000B1BCC File Offset: 0x000AFDCC
		public void MusicValueOnChange()
		{
			GameGlobal.m_fMusicValue = this.m_MusicSlider.value;
			Game.SetBGMusicValue();
			string value = GameGlobal.m_fMusicValue.ToString("0.00");
			IniFile.IniWriteValue("setting", "MusicValue", value);
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x000B1C10 File Offset: 0x000AFE10
		public void SoundValueOnChange()
		{
			GameGlobal.m_fSoundValue = this.m_SoundSlider.value;
			string value = GameGlobal.m_fSoundValue.ToString("0.00");
			IniFile.IniWriteValue("setting", "SoundValue", value);
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x0000D339 File Offset: 0x0000B539
		private void LblKeyOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.current = UIEventListener.Get(go);
				this.LblKeyOn(go, bHover);
			}
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x0000D359 File Offset: 0x0000B559
		private void LblKeyOnKeySelect(GameObject go, bool bSelect)
		{
			this.LblKeyOn(go, bSelect);
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0000D363 File Offset: 0x0000B563
		private void LblKeyOn(GameObject goes, bool bOn)
		{
			if (bOn)
			{
				this.SetControl(this.m_KeyOptionOn, goes, 0f, 0f, 0f);
			}
			else
			{
				this.m_KeyOptionOn.GameObject.SetActive(false);
			}
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x000B1C50 File Offset: 0x000AFE50
		private void LblKeyOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (go.GetComponent<BtnData>().m_iOnClick == 1)
			{
				string @string = Game.StringTable.GetString(100148);
				Game.UI.Get<UIMapMessage>().SetMsg(@string);
				return;
			}
			this.m_KeyOptionOn.GameObject.SetActive(false);
			this.SetControl(this.m_KeyOptionOnClick, go, 0f, 0f, 0f);
			this.keyObj = go;
			base.EnterState(21);
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x000B1CD8 File Offset: 0x000AFED8
		private void CheckAnyKey(KeyCode keyCode)
		{
			Debug.Log("KeyCode down: " + keyCode);
			int iBtnID = this.keyObj.GetComponent<BtnData>().m_iBtnID;
			string text = Convert.ToString(keyCode);
			if (text.Contains("Joystick"))
			{
				this.DoKeyAction(2, iBtnID, keyCode);
			}
			else
			{
				this.DoKeyAction(1, iBtnID, keyCode);
			}
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x000B1D40 File Offset: 0x000AFF40
		private void DoKeyAction(int keyboard, int keyIndex, KeyCode keyCode)
		{
			int iBtnType = this.keyObj.GetComponent<BtnData>().m_iBtnType;
			if (iBtnType == keyboard)
			{
				this.SetKeyView(iBtnType, keyIndex, keyCode.ToString());
				if (iBtnType == 1)
				{
					this.interim.keyboard[keyIndex] = keyCode;
				}
				else
				{
					this.interim.joystick[keyIndex] = keyCode;
				}
			}
			else
			{
				string @string = Game.StringTable.GetString(100145 + iBtnType);
				Game.UI.Get<UIMapMessage>().SetMsg(@string);
			}
			base.StartCoroutine(this.ChangeState(0.1f));
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x000B1DDC File Offset: 0x000AFFDC
		private IEnumerator ChangeState(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);
			base.EnterState(2);
			yield break;
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x000B1E08 File Offset: 0x000B0008
		private void SetView(SettingData data)
		{
			this.m_ResolutionValue.Text = data.iScreenWidth.ToString() + "x" + data.iScreenHeight.ToString();
			for (int i = 0; i < Screen.resolutions.Length; i++)
			{
				Resolution resolution = Screen.resolutions[i];
				if (resolution.width <= 1920)
				{
					if (resolution.width / 16 != 0 || resolution.height / 9 != 0)
					{
						resolution.height = (int)((float)resolution.width * 0.5625f);
					}
					string text = resolution.width.ToString() + "x" + resolution.height.ToString();
					this.m_ResolutionValueOptionList[i].Text = text;
				}
			}
			this.m_FromResolution.GetComponent<UISprite>().height = 52 * Screen.resolutions.Length;
			this.SetOptionBase(this.m_FullScreenOptionList, this.BoolToInt(data.bFullScreen), this.m_FullScreenOptionOnClick, 99);
			this.SetOptionBase(this.m_QualityPointList, data.settingDataNode.m_iQualityLevel, this.m_QualityThumb, 5);
			this.m_OwnSettingOnClick.GameObject.SetActive(data.bOwnSetting);
			this.SetOptionBase(this.m_SSAAOptionList, this.BoolToInt(data.bSSAA), this.m_SSAAOptionOnClick, 99);
			float num = data.fSSAA - 1f;
			num *= 10f;
			int dataValue = Convert.ToInt32(num);
			this.SetOptionBase(this.m_SSAAMultiplierPointList, dataValue, this.m_SSAAMultiplierThumb, 17);
			if (data.bOwnSetting)
			{
				this.SetEffectView(data.settingDataNode);
			}
			else
			{
				SettingDataNode settingDataNode = Game.SettingData.GetSettingDataNode(data.settingDataNode.m_iQualityLevel);
				this.SetEffectView(settingDataNode);
			}
			this.m_MusicSlider.value = data.fMusicValue;
			this.m_SoundSlider.value = data.fSoundValue;
			for (int j = 0; j < data.keyboard.Length; j++)
			{
				this.SetKeyView(1, j, data.keyboard[j].ToString());
			}
			for (int k = 0; k < data.joystick.Length; k++)
			{
				this.SetKeyView(2, k, data.joystick[k].ToString());
			}
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x000B207C File Offset: 0x000B027C
		public void SetEffectView(SettingDataNode node)
		{
			this.SetOptionBase(this.m_ShadowOptionList, this.BoolToInt(node.m_bShadow), this.m_ShadowOptionOnClick, 99);
			this.SetOptionBase(this.m_ShadowDistancePointList, (int)node.m_fShadowDistance, this.m_ShadowDistanceThumb, 8);
			this.SetOptionBase(this.m_VSyncOptionList, node.m_iVSync, this.m_VSyncOptionOnClick, 99);
			this.SetOptionBase(this.m_AntiAliasingOptionList, node.m_iAntiAliasing, this.m_AntiAliasingOptionClick, 99);
			this.SetOptionBase(this.m_ImageQualityPointList, node.m_iImageQuality, this.m_ImageQualityThumb, 11);
			this.SetOptionBase(this.m_SSAOOptionList, this.BoolToInt(node.m_bSSAO), this.m_SSAOOptionClick, 99);
			this.SetOptionBase(this.m_LensEffectsOptionList, this.BoolToInt(node.m_bLensEffects), this.m_LensEffectsOptionClick, 99);
			this.SetOptionBase(this.m_BloomOptionList, this.BoolToInt(node.m_bBloom), this.m_BloomOptionOnClick, 99);
			this.SetOptionBase(this.m_ResourcesCountPointList, node.m_iResourcesCount, this.m_ResourcesCountThumb, 15);
			if (node.m_iAntiAliasing == 2)
			{
				this.interim.bSSAA = false;
				this.SetOptionBase(this.m_SSAAOptionList, this.BoolToInt(this.interim.bSSAA), this.m_SSAAOptionOnClick, 99);
			}
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x000B21C8 File Offset: 0x000B03C8
		private void SetOptionBase(List<Control> control, int dataValue, Control onClick, int state = 99)
		{
			for (int i = 0; i < control.Count; i++)
			{
				if (control[i].GetComponent<BtnData>().m_iBtnID == dataValue)
				{
					this.SetControl(onClick, control[i].GameObject, 0f, 0f, 0f);
					if (onClick == this.m_QualityThumb)
					{
						this.m_QualityValue.Text = Game.StringTable.GetString(100089 + dataValue);
					}
					else if (onClick == this.m_ShadowDistanceThumb)
					{
						this.m_ShadowDistanceValue.Text = dataValue.ToString();
					}
					else if (onClick == this.m_ImageQualityThumb)
					{
						this.m_ImageQualityValue.Text = Game.StringTable.GetString(100103 - dataValue);
					}
					else if (onClick == this.m_ResourcesCountThumb)
					{
						this.m_ResourcesCountValue.Text = Game.StringTable.GetString(100100 + dataValue);
					}
					else if (onClick == this.m_SSAAMultiplierThumb)
					{
						float num = (float)dataValue / 10f + 1f;
						this.m_SSAAMultiplierValue.Text = num.ToString();
					}
					if (state != 99)
					{
						this.m_IndexArray[state] = i;
					}
				}
			}
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x000B2310 File Offset: 0x000B0510
		private void SetKeyView(int type, int key, string value)
		{
			for (int i = 0; i < this.m_LblKeyList.Count; i++)
			{
				BtnData component = this.m_LblKeyList[i].GetComponent<BtnData>();
				if (component.m_iBtnType == type && component.m_iBtnID == key)
				{
					this.m_LblKeyList[i].Text = value;
				}
			}
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x000B2378 File Offset: 0x000B0578
		private void InitSystemView()
		{
			this.m_System.GameObject.SetActive(true);
			this.m_HotKey.GameObject.SetActive(false);
			this.m_SoundValueSlider.GetComponent<UISprite>().color = Color.white;
			this.m_MusicValueSlider.GetComponent<UISprite>().color = Color.white;
			this.m_SystemScrollView.GetComponent<UIScrollView>().ResetPosition();
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0000D39D File Offset: 0x0000B59D
		private void InitKeyView()
		{
			this.m_System.GameObject.SetActive(false);
			this.m_HotKey.GameObject.SetActive(true);
			this.m_KeyOptionOnClick.GameObject.SetActive(false);
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x000B23E4 File Offset: 0x000B05E4
		public void CreatResolutionValue()
		{
			GameObject gameObject = Object.Instantiate(this.m_ResolutionValueOption) as GameObject;
			gameObject.transform.parent = this.m_ResolutionGrid.GameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "ResolutionValueOption";
			gameObject.transform.Traversal(delegate(Transform x)
			{
				string name = x.name;
				if (name != null)
				{
					if (UISystemSetting.<>f__switch$map5E == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
						dictionary.Add("ResolutionValueOption", 0);
						UISystemSetting.<>f__switch$map5E = dictionary;
					}
					int num;
					if (UISystemSetting.<>f__switch$map5E.TryGetValue(name, ref num))
					{
						if (num == 0)
						{
							x.OnClick += this.ResolutionValueOptionOnClick;
							x.OnKeySelect += this.ResolutionValueOnKeySelect;
							x.OnHover += this.ResolutionValueOnHover;
							base.SetInputButton(20, x.Listener);
							this.m_ResolutionValueOptionList.Add(x);
						}
					}
				}
			});
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x0000D3D2 File Offset: 0x0000B5D2
		private bool BtnIDToBool(int btnID)
		{
			return btnID == 1;
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0000D3DE File Offset: 0x0000B5DE
		private int BoolToInt(bool b)
		{
			if (b)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0000B1AD File Offset: 0x000093AD
		public override void OnMouseControl(bool bCtrl)
		{
			base.OnMouseControl(bCtrl);
			this.HideKeySelect();
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x000B2470 File Offset: 0x000B0670
		private void Update()
		{
			if (this.m_Group.GameObject.activeSelf && this.m_System.GameObject.activeSelf)
			{
				this.SetMouseWheel(this.m_SystemSlider);
			}
			else if (this.m_Group.GameObject.activeSelf && this.m_HotKey.GameObject.activeSelf && this.NowState != 21)
			{
				this.SetMouseWheel(this.m_KeySlider);
			}
			if (this.NowState == 21)
			{
				if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
				{
					return;
				}
				if (Input.anyKeyDown)
				{
					foreach (object obj in Enum.GetValues(typeof(KeyCode)))
					{
						KeyCode keyCode = (int)obj;
						if (Input.GetKeyDown(keyCode))
						{
							this.CheckAnyKey(keyCode);
						}
					}
				}
			}
		}

		// Token: 0x040018A7 RID: 6311
		private CtrlSystemSetting m_SystemSettingController = new CtrlSystemSetting();

		// Token: 0x040018A8 RID: 6312
		private UIEventListener[] m_CurrentArray = new UIEventListener[Enum.GetNames(typeof(UISystemSetting.eState)).Length];

		// Token: 0x040018A9 RID: 6313
		private int[] m_IndexArray = new int[Enum.GetNames(typeof(UISystemSetting.eState)).Length];

		// Token: 0x040018AA RID: 6314
		public SettingData interim = new SettingData();

		// Token: 0x040018AB RID: 6315
		private Control m_Group;

		// Token: 0x040018AC RID: 6316
		private List<Control> m_TitleTypeList = new List<Control>();

		// Token: 0x040018AD RID: 6317
		private Control m_TitleOn;

		// Token: 0x040018AE RID: 6318
		private Control m_TitleOnClick;

		// Token: 0x040018AF RID: 6319
		private Control m_System;

		// Token: 0x040018B0 RID: 6320
		private Control m_SystemSlider;

		// Token: 0x040018B1 RID: 6321
		private Control m_SystemScrollView;

		// Token: 0x040018B2 RID: 6322
		public GameObject m_ResolutionValueOption;

		// Token: 0x040018B3 RID: 6323
		private Control m_ResolutionValue;

		// Token: 0x040018B4 RID: 6324
		private Control m_ResolutionOn;

		// Token: 0x040018B5 RID: 6325
		private Control m_ResolutionOnClick;

		// Token: 0x040018B6 RID: 6326
		private Control m_ResolutionGrid;

		// Token: 0x040018B7 RID: 6327
		private Control m_FromResolution;

		// Token: 0x040018B8 RID: 6328
		private List<Control> m_ResolutionValueOptionList = new List<Control>();

		// Token: 0x040018B9 RID: 6329
		private Control m_MultipleOptionOn;

		// Token: 0x040018BA RID: 6330
		private Control m_OwnSettingOption;

		// Token: 0x040018BB RID: 6331
		private Control m_OwnSettingOnClick;

		// Token: 0x040018BC RID: 6332
		private Control m_FullScreenOptionOnClick;

		// Token: 0x040018BD RID: 6333
		private Control m_ShadowOptionOnClick;

		// Token: 0x040018BE RID: 6334
		private Control m_VSyncOptionOnClick;

		// Token: 0x040018BF RID: 6335
		private Control m_AntiAliasingOptionClick;

		// Token: 0x040018C0 RID: 6336
		private Control m_SSAOOptionClick;

		// Token: 0x040018C1 RID: 6337
		private Control m_LensEffectsOptionClick;

		// Token: 0x040018C2 RID: 6338
		private Control m_BloomOptionOnClick;

		// Token: 0x040018C3 RID: 6339
		private Control m_QualityThumb;

		// Token: 0x040018C4 RID: 6340
		private Control m_ShadowDistanceThumb;

		// Token: 0x040018C5 RID: 6341
		private Control m_ImageQualityThumb;

		// Token: 0x040018C6 RID: 6342
		private Control m_ResourcesCountThumb;

		// Token: 0x040018C7 RID: 6343
		private Control m_SSAAOptionOnClick;

		// Token: 0x040018C8 RID: 6344
		private Control m_SSAAMultiplierThumb;

		// Token: 0x040018C9 RID: 6345
		private Control m_QualityValue;

		// Token: 0x040018CA RID: 6346
		private Control m_ShadowDistanceValue;

		// Token: 0x040018CB RID: 6347
		private Control m_ImageQualityValue;

		// Token: 0x040018CC RID: 6348
		private Control m_ResourcesCountValue;

		// Token: 0x040018CD RID: 6349
		private Control m_SSAAMultiplierValue;

		// Token: 0x040018CE RID: 6350
		private List<Control> m_FullScreenOptionList = new List<Control>();

		// Token: 0x040018CF RID: 6351
		private List<Control> m_ShadowOptionList = new List<Control>();

		// Token: 0x040018D0 RID: 6352
		private List<Control> m_VSyncOptionList = new List<Control>();

		// Token: 0x040018D1 RID: 6353
		private List<Control> m_AntiAliasingOptionList = new List<Control>();

		// Token: 0x040018D2 RID: 6354
		private List<Control> m_SSAOOptionList = new List<Control>();

		// Token: 0x040018D3 RID: 6355
		private List<Control> m_LensEffectsOptionList = new List<Control>();

		// Token: 0x040018D4 RID: 6356
		private List<Control> m_BloomOptionList = new List<Control>();

		// Token: 0x040018D5 RID: 6357
		private List<Control> m_QualityPointList = new List<Control>();

		// Token: 0x040018D6 RID: 6358
		private List<Control> m_ShadowDistancePointList = new List<Control>();

		// Token: 0x040018D7 RID: 6359
		private List<Control> m_ImageQualityPointList = new List<Control>();

		// Token: 0x040018D8 RID: 6360
		private List<Control> m_ResourcesCountPointList = new List<Control>();

		// Token: 0x040018D9 RID: 6361
		private List<Control> m_SSAAOptionList = new List<Control>();

		// Token: 0x040018DA RID: 6362
		private List<Control> m_SSAAMultiplierPointList = new List<Control>();

		// Token: 0x040018DB RID: 6363
		private Control m_MusicValueSlider;

		// Token: 0x040018DC RID: 6364
		private Control m_SoundValueSlider;

		// Token: 0x040018DD RID: 6365
		private Control m_MusicValueForeground;

		// Token: 0x040018DE RID: 6366
		private Control m_SoundValueForeground;

		// Token: 0x040018DF RID: 6367
		private UISlider m_MusicSlider;

		// Token: 0x040018E0 RID: 6368
		private UISlider m_SoundSlider;

		// Token: 0x040018E1 RID: 6369
		private Control m_DetermineForm;

		// Token: 0x040018E2 RID: 6370
		private List<Control> m_DeterminOptionList = new List<Control>();

		// Token: 0x040018E3 RID: 6371
		private Control m_WarningForm;

		// Token: 0x040018E4 RID: 6372
		private List<Control> m_WarningOptionList = new List<Control>();

		// Token: 0x040018E5 RID: 6373
		private Control m_HotKey;

		// Token: 0x040018E6 RID: 6374
		private Control m_KeyOptionOn;

		// Token: 0x040018E7 RID: 6375
		private Control m_KeyOptionOnClick;

		// Token: 0x040018E8 RID: 6376
		private List<Control> m_LblKeyList = new List<Control>();

		// Token: 0x040018E9 RID: 6377
		private Control m_KeySlider;

		// Token: 0x040018EA RID: 6378
		private Control m_KeyScrollView;

		// Token: 0x040018EB RID: 6379
		private Control m_SystemExitSprite;

		// Token: 0x040018EC RID: 6380
		private GameObject keyObj;

		// Token: 0x040018ED RID: 6381
		private int selectType = 1;

		// Token: 0x040018EE RID: 6382
		private int selectIndex;

		// Token: 0x040018EF RID: 6383
		private int keyIndex = 2;

		// Token: 0x040018F0 RID: 6384
		private bool bChangeState;

		// Token: 0x02000375 RID: 885
		private enum eState
		{
			// Token: 0x040018F5 RID: 6389
			None,
			// Token: 0x040018F6 RID: 6390
			SystemSetting,
			// Token: 0x040018F7 RID: 6391
			KeySetting,
			// Token: 0x040018F8 RID: 6392
			ScreenResolution,
			// Token: 0x040018F9 RID: 6393
			FullScreen,
			// Token: 0x040018FA RID: 6394
			ScreenQuality,
			// Token: 0x040018FB RID: 6395
			OwnSetting,
			// Token: 0x040018FC RID: 6396
			Shadow,
			// Token: 0x040018FD RID: 6397
			ShadowDistance,
			// Token: 0x040018FE RID: 6398
			VSync,
			// Token: 0x040018FF RID: 6399
			AntiAliasing,
			// Token: 0x04001900 RID: 6400
			ImageQuality,
			// Token: 0x04001901 RID: 6401
			SSAO,
			// Token: 0x04001902 RID: 6402
			LensEffects,
			// Token: 0x04001903 RID: 6403
			Bloom,
			// Token: 0x04001904 RID: 6404
			ResourcesCount,
			// Token: 0x04001905 RID: 6405
			SSAA,
			// Token: 0x04001906 RID: 6406
			SSAAMultiplier,
			// Token: 0x04001907 RID: 6407
			MusicValue,
			// Token: 0x04001908 RID: 6408
			SoundValue,
			// Token: 0x04001909 RID: 6409
			Resolutions,
			// Token: 0x0400190A RID: 6410
			Key,
			// Token: 0x0400190B RID: 6411
			Determine,
			// Token: 0x0400190C RID: 6412
			Warning
		}
	}
}
