using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200036F RID: 879
	public class UISaveAndLoad : UILayer
	{
		// Token: 0x06001422 RID: 5154 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x000ADEA8 File Offset: 0x000AC0A8
		private void Start()
		{
			CtrlSaveAndLoad saveAndLoadController = this.m_SaveAndLoadController;
			saveAndLoadController.setViewActive = (Action<int, bool>)Delegate.Combine(saveAndLoadController.setViewActive, new Action<int, bool>(this.SetViewActive));
			CtrlSaveAndLoad saveAndLoadController2 = this.m_SaveAndLoadController;
			saveAndLoadController2.setSaveLoadView = (Action<int, string[]>)Delegate.Combine(saveAndLoadController2.setSaveLoadView, new Action<int, string[]>(this.SetSaveLoadView));
			CtrlSaveAndLoad saveAndLoadController3 = this.m_SaveAndLoadController;
			saveAndLoadController3.setTipView = (Action<string, string, Texture2D>)Delegate.Combine(saveAndLoadController3.setTipView, new Action<string, string, Texture2D>(this.SetTipView));
			for (int i = 0; i < this.maxAmount; i++)
			{
				this.CreatSaveLoadItem();
			}
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x000ADF48 File Offset: 0x000AC148
		public override void OnKeyUp(KeyControl.Key key)
		{
			if (this.viewType == 3)
			{
				if (key != KeyControl.Key.L1)
				{
					if (key == KeyControl.Key.R1)
					{
						this.TitleOnKey(false);
					}
				}
				else
				{
					this.TitleOnKey(true);
				}
			}
			switch (key)
			{
			case KeyControl.Key.Up:
				this.ItemOnKey(key, true);
				break;
			case KeyControl.Key.Down:
				this.ItemOnKey(key, false);
				break;
			case KeyControl.Key.Left:
			case KeyControl.Key.Right:
				this.SelectNextButton(key);
				break;
			case KeyControl.Key.OK:
				base.OnCurrentClick();
				break;
			case KeyControl.Key.Cancel:
				this.SaveExitSpriteOnClick(this.m_SaveExitSprite.GameObject);
				break;
			}
			if (key == KeyControl.Key.Save && this.viewType == 1)
			{
				base.EnterState(0);
			}
			else if (key == KeyControl.Key.Load && this.viewType == 3)
			{
				base.EnterState(0);
			}
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x000AE038 File Offset: 0x000AC238
		private void TitleOnKey(bool reverse)
		{
			if (reverse)
			{
				this.selectTypeIndex--;
			}
			else
			{
				this.selectTypeIndex++;
			}
			this.selectTypeIndex = Mathf.Clamp(this.selectTypeIndex, 1, 2);
			if (this.selectTypeIndex == this.NowState)
			{
				return;
			}
			this.TitleOnClick(this.m_TitleList[this.selectTypeIndex - 1].GameObject);
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0000CF24 File Offset: 0x0000B124
		private void ItemOnKey(KeyControl.Key key, bool reverse)
		{
			this.SelectNextButton(key);
			this.selectIndex = int.Parse(this.current.name);
			this.SetScrollBar(this.selectIndex, this.showMaxAmount, this.maxAmount, reverse, this.m_SaveLoadSlider);
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x000AE0B0 File Offset: 0x000AC2B0
		protected override void OnStateEnter(int state)
		{
			switch (state)
			{
			case 0:
				this.Hide();
				break;
			case 1:
				this.m_SaveAndLoadController.UpdateSaveLoadData(1);
				this.m_CheckCover.GameObject.SetActive(false);
				this.m_SaveBlock.GameObject.SetActive(false);
				this.m_SaveLoadItemOnClick.GameObject.SetActive(false);
				break;
			case 2:
				this.m_SaveAndLoadController.UpdateSaveLoadData(2);
				break;
			case 3:
				this.InitCheck();
				this.m_CheckCover.GameObject.SetActive(true);
				break;
			case 4:
				this.m_SaveBlock.GameObject.SetActive(true);
				this.m_CheckCover.GameObject.SetActive(false);
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

		// Token: 0x06001428 RID: 5160 RVA: 0x000AE244 File Offset: 0x000AC444
		protected override void OnStateExit(int state)
		{
			if (state == 1)
			{
				this.m_CurrentArray[state] = this.current;
			}
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x000AE274 File Offset: 0x000AC474
		private void InitCheck()
		{
			for (int i = 0; i < this.m_OptionList.Count; i++)
			{
				this.m_OptionList[i].SpriteName = "ui_sys_09";
			}
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x000AE2B4 File Offset: 0x000AC4B4
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UISaveAndLoad.<>f__switch$map56 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(18);
					dictionary.Add("Group", 0);
					dictionary.Add("TopSelect", 1);
					dictionary.Add("Title", 2);
					dictionary.Add("TitleOn", 3);
					dictionary.Add("TitleOnClick", 4);
					dictionary.Add("GridSaveLoad", 5);
					dictionary.Add("SaveLoadSlider", 6);
					dictionary.Add("SaveLoadItemOn", 7);
					dictionary.Add("SaveLoadItemOnClick", 8);
					dictionary.Add("CurrentMission", 9);
					dictionary.Add("CurrentMissionName", 10);
					dictionary.Add("CurrentMissionExp", 11);
					dictionary.Add("TexturePlace", 12);
					dictionary.Add("SaveExitSprite", 13);
					dictionary.Add("SaveLoadView", 14);
					dictionary.Add("CheckCover", 15);
					dictionary.Add("SaveBlock", 16);
					dictionary.Add("Option", 17);
					UISaveAndLoad.<>f__switch$map56 = dictionary;
				}
				int num;
				if (UISaveAndLoad.<>f__switch$map56.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_TopSelect = sender;
						break;
					case 2:
					{
						Control control = sender;
						control.OnClick += this.TitleOnClick;
						control.OnHover += this.TitleOnHover;
						this.titleIndex++;
						control.GameObject.name = this.titleIndex.ToString();
						this.m_TitleList.Add(control);
						break;
					}
					case 3:
						this.m_TitleOn = sender;
						break;
					case 4:
						this.m_TitleOnClick = sender;
						break;
					case 5:
						this.m_GridSaveLoad = sender;
						break;
					case 6:
						this.m_SaveLoadSlider = sender;
						break;
					case 7:
						this.m_SaveLoadItemOn = sender;
						break;
					case 8:
						this.m_SaveLoadItemOnClick = sender;
						break;
					case 9:
						this.m_CurrentMission = sender;
						break;
					case 10:
						this.m_CurrentMissionName = sender;
						break;
					case 11:
						this.m_CurrentMissionExp = sender;
						break;
					case 12:
						this.m_TexturePlace = sender;
						break;
					case 13:
					{
						Control control = sender;
						control.OnHover += this.SaveExitSpriteOnHover;
						control.OnClick += this.SaveExitSpriteOnClick;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						this.m_SaveExitSprite = sender;
						break;
					}
					case 14:
						this.m_SaveLoadView = sender;
						break;
					case 15:
						this.m_CheckCover = sender;
						break;
					case 16:
						this.m_SaveBlock = sender;
						break;
					case 17:
					{
						Control control = sender;
						control.OnHover += this.OptionOnHover;
						control.OnKeySelect += this.OptionOnKeySelect;
						control.OnClick += this.OptionOnClick;
						this.optionIndex++;
						control.GameObject.name = this.optionIndex.ToString();
						base.SetInputButton(3, control.Listener);
						this.m_OptionList.Add(control);
						break;
					}
					}
				}
			}
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0000CF62 File Offset: 0x0000B162
		public void SelectSaveOrLoad(int type)
		{
			this.Show();
			this.viewType = type;
			this.SetTitleView(this.viewType);
			this.SetMouseActive(true);
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x000AE650 File Offset: 0x000AC850
		public override void Show()
		{
			Game.g_InputManager.Push(this);
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			this.m_Group.GameObject.SetActive(this.m_bShow);
			GameGlobal.m_bCFormOpen = this.m_bShow;
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x000AE69C File Offset: 0x000AC89C
		public override void Hide()
		{
			this.CloseView();
			for (int i = 0; i < Enum.GetNames(typeof(UISaveAndLoad.eState)).Length; i++)
			{
				this.m_CurrentArray[i] = null;
			}
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			this.m_Group.GameObject.SetActive(this.m_bShow);
			GameGlobal.m_bCFormOpen = this.m_bShow;
			Game.g_InputManager.Pop();
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x000AE71C File Offset: 0x000AC91C
		private void SetTitleView(int type)
		{
			int num = 1;
			UILabel component = this.m_TitleList[num].GetComponent<UILabel>();
			if (type == 1)
			{
				component.alpha = 0.27450982f;
				component.collider.enabled = false;
			}
			else
			{
				component.alpha = 1f;
				component.collider.enabled = true;
			}
			this.TitleOnClick(this.m_TitleList[0].GameObject);
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x000AE790 File Offset: 0x000AC990
		private void TitleOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_TitleOn.GameObject.SetActive(false);
			this.SetControl(this.m_TitleOnClick, go, 120f, 1.3f, 0f);
			base.EnterState(int.Parse(go.name));
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0000CF84 File Offset: 0x0000B184
		private void TitleOnHover(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.SetControl(this.m_TitleOn, go, 120f, 1.3f, 0f);
			}
			else
			{
				this.m_TitleOn.GameObject.SetActive(bHover);
			}
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0000CFBE File Offset: 0x0000B1BE
		private void OptionOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.OptionOn(go, bHover);
			}
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0000CFD2 File Offset: 0x0000B1D2
		private void OptionOnKeySelect(GameObject go, bool bSelect)
		{
			this.OptionOn(go, bSelect);
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x00096C50 File Offset: 0x00094E50
		private void OptionOn(GameObject goes, bool bOn)
		{
			UISprite component = goes.GetComponent<UISprite>();
			if (bOn)
			{
				component.spriteName = "ui_sys_10";
			}
			else
			{
				component.spriteName = "ui_sys_09";
			}
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x000AE7E8 File Offset: 0x000AC9E8
		private void OptionOnClick(GameObject go)
		{
			int num = int.Parse(go.name);
			int num2 = num;
			if (num2 != 1)
			{
				if (num2 != 2)
				{
				}
				base.EnterState(1);
				return;
			}
			Debug.Log("點到了確定~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
			Save.m_Instance.SaveData(1, this.currentIndex, -1);
			base.EnterState(4);
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x000AE848 File Offset: 0x000ACA48
		private void SaveLoadItemOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (Save.m_Instance.bSave)
			{
				return;
			}
			go.collider.enabled = false;
			this.currentIndex = int.Parse(go.name);
			bool flag = this.m_SaveAndLoadController.HaveData(this.currentIndex);
			if (this.viewType == 1)
			{
				if (flag)
				{
					this.SetControl(this.m_SaveLoadItemOnClick, go, 0f, 0f, 0f);
					this.m_SaveLoadItemOn.GameObject.SetActive(false);
					base.EnterState(3);
				}
				else
				{
					Save.m_Instance.SaveData(1, this.currentIndex, -1);
					this.m_SaveAndLoadController.SetSaveLoadData();
					base.EnterState(4);
				}
			}
			else
			{
				if (!flag)
				{
					go.collider.enabled = true;
					return;
				}
				Game.UI.Get<UIRumor>().ClearRumor();
				Game.UI.Get<UISaveRumor>().ClearRumorRecord();
				Game.UI.Get<UITalk>().ClearTalkLogRecord();
				Save.m_Instance.LoadData(this.NowState, this.currentIndex);
				base.EnterState(0);
			}
			go.collider.enabled = true;
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0000CFDC File Offset: 0x0000B1DC
		private void SaveLoadItemOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.OnItemStatus(go, bHover, this.m_SaveLoadItemOn);
			}
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0000CFF6 File Offset: 0x0000B1F6
		private void SaveLoadItemOnKeySelect(GameObject go, bool bHover)
		{
			this.OnItemStatus(go, bHover, this.m_SaveLoadItemOn);
			this.ShowKeySelect(go, new Vector3(-620f, 0f, 0f), KeySelect.eSelectDir.Left, 30, 30);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x000AE980 File Offset: 0x000ACB80
		private void OnItemStatus(GameObject goes, bool hover, Control icon)
		{
			if (hover)
			{
				this.SetControl(this.m_SaveLoadItemOn, goes, 0f, 0f, 0f);
				int tipData = int.Parse(goes.name);
				this.m_SaveAndLoadController.SetTipData(tipData);
				if (GameCursor.IsShow)
				{
					this.current = UIEventListener.Get(goes.gameObject);
				}
			}
			else
			{
				this.m_CurrentMission.GameObject.SetActive(hover);
				this.m_SaveLoadItemOn.GameObject.SetActive(hover);
				this.m_TexturePlace.GetComponent<UITexture>().mainTexture = null;
			}
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0000D026 File Offset: 0x0000B226
		private void SetTipView(string name, string tip, Texture2D image)
		{
			this.m_CurrentMission.GameObject.SetActive(true);
			this.m_CurrentMissionName.Text = name;
			this.m_CurrentMissionExp.Text = tip;
			this.m_TexturePlace.GetComponent<UITexture>().mainTexture = image;
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0000D062 File Offset: 0x0000B262
		private void SaveExitSpriteOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			go.GetComponent<UISprite>().spriteName = "cdata_027";
			base.EnterState(0);
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x000A3240 File Offset: 0x000A1440
		private void SaveExitSpriteOnHover(GameObject go, bool bHover)
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

		// Token: 0x0600143C RID: 5180 RVA: 0x000AEA1C File Offset: 0x000ACC1C
		private void SetViewActive(int index, bool have)
		{
			this.m_SaveLoadNoDataList[index].GameObject.SetActive(!have);
			this.m_SaveLoadNumList[index].GameObject.SetActive(have);
			this.m_SaveLoadNameList[index].GameObject.SetActive(have);
			this.m_SaveLoadPlaceList[index].GameObject.SetActive(have);
			this.m_SaveLoadDateList[index].GameObject.SetActive(have);
			this.m_SaveLoadTotalTimeList[index].GameObject.SetActive(have);
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x000AEAB8 File Offset: 0x000ACCB8
		private void SetSaveLoadView(int index, string[] data)
		{
			this.m_SaveLoadNumList[index].Text = (index + 1).ToString("000");
			this.m_SaveLoadNameList[index].Text = data[0];
			this.m_SaveLoadPlaceList[index].Text = data[1];
			this.m_SaveLoadDateList[index].Text = data[2];
			this.m_SaveLoadTotalTimeList[index].Text = data[3];
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0000D087 File Offset: 0x0000B287
		private void CloseView()
		{
			this.m_CurrentMission.GameObject.SetActive(false);
			this.m_SaveLoadItemOn.GameObject.SetActive(false);
			this.m_TitleOn.GameObject.SetActive(false);
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0000D0BC File Offset: 0x0000B2BC
		private void SetMouseActive(bool bReset)
		{
			this.CheckMouseWheel(this.m_SaveLoadItemList.Count, this.maxAmount - 1, this.m_SaveLoadSlider, this.m_SaveLoadView, bReset);
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x0000B1AD File Offset: 0x000093AD
		public override void OnMouseControl(bool bCtrl)
		{
			base.OnMouseControl(bCtrl);
			this.HideKeySelect();
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x000AEB38 File Offset: 0x000ACD38
		private void CreatSaveLoadItem()
		{
			GameObject gameObject = Object.Instantiate(this.m_SaveLoadItem) as GameObject;
			gameObject.transform.parent = this.m_GridSaveLoad.GameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "SaveLoadItem";
			gameObject.transform.Traversal(delegate(Transform x)
			{
				string name = x.name;
				if (name != null)
				{
					if (UISaveAndLoad.<>f__switch$map58 == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
						dictionary.Add("SaveLoadItem", 0);
						dictionary.Add("SaveLoadNum", 1);
						dictionary.Add("SaveLoadName", 2);
						dictionary.Add("SaveLoadPlace", 3);
						dictionary.Add("SaveLoadDate", 4);
						dictionary.Add("SaveLoadNoData", 5);
						dictionary.Add("SaveLoadTotalTime", 6);
						UISaveAndLoad.<>f__switch$map58 = dictionary;
					}
					int num;
					if (UISaveAndLoad.<>f__switch$map58.TryGetValue(name, ref num))
					{
						switch (num)
						{
						case 0:
							x.OnClick += this.SaveLoadItemOnClick;
							x.OnKeySelect += this.SaveLoadItemOnKeySelect;
							x.OnHover += this.SaveLoadItemOnHover;
							x.GameObject.name = this.itemIndex.ToString();
							base.SetInputButton(1, x.Listener);
							base.SetInputButton(2, x.Listener);
							this.m_SaveLoadItemList.Add(x);
							break;
						case 1:
							this.m_SaveLoadNumList.Add(x);
							break;
						case 2:
							this.m_SaveLoadNameList.Add(x);
							break;
						case 3:
							this.m_SaveLoadPlaceList.Add(x);
							break;
						case 4:
							this.m_SaveLoadDateList.Add(x);
							break;
						case 5:
							this.m_SaveLoadNoDataList.Add(x);
							break;
						case 6:
							this.m_SaveLoadTotalTimeList.Add(x);
							break;
						}
					}
				}
			});
			this.itemIndex++;
			this.m_GridSaveLoad.GetComponent<UIGrid>().Reposition();
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x000AEBD4 File Offset: 0x000ACDD4
		private void Update()
		{
			if (this.NowState == 4 && !Save.m_Instance.bSave)
			{
				base.EnterState(1);
			}
			if (this.m_Group.GameObject.activeSelf && this.m_SaveLoadSlider.GameObject.activeSelf)
			{
				this.SetMouseWheel(this.m_SaveLoadSlider);
			}
		}

		// Token: 0x0400185F RID: 6239
		private CtrlSaveAndLoad m_SaveAndLoadController = new CtrlSaveAndLoad();

		// Token: 0x04001860 RID: 6240
		private UIEventListener[] m_CurrentArray = new UIEventListener[Enum.GetNames(typeof(UISaveAndLoad.eState)).Length];

		// Token: 0x04001861 RID: 6241
		private Control m_Group;

		// Token: 0x04001862 RID: 6242
		private Control m_TopSelect;

		// Token: 0x04001863 RID: 6243
		private List<Control> m_TitleList = new List<Control>();

		// Token: 0x04001864 RID: 6244
		private Control m_TitleOn;

		// Token: 0x04001865 RID: 6245
		private Control m_TitleOnClick;

		// Token: 0x04001866 RID: 6246
		private Control m_SaveLoadView;

		// Token: 0x04001867 RID: 6247
		private Control m_GridSaveLoad;

		// Token: 0x04001868 RID: 6248
		private List<Control> m_SaveLoadItemList = new List<Control>();

		// Token: 0x04001869 RID: 6249
		private List<Control> m_SaveLoadNumList = new List<Control>();

		// Token: 0x0400186A RID: 6250
		private List<Control> m_SaveLoadNameList = new List<Control>();

		// Token: 0x0400186B RID: 6251
		private List<Control> m_SaveLoadPlaceList = new List<Control>();

		// Token: 0x0400186C RID: 6252
		private List<Control> m_SaveLoadDateList = new List<Control>();

		// Token: 0x0400186D RID: 6253
		private List<Control> m_SaveLoadNoDataList = new List<Control>();

		// Token: 0x0400186E RID: 6254
		private List<Control> m_SaveLoadTotalTimeList = new List<Control>();

		// Token: 0x0400186F RID: 6255
		private Control m_SaveLoadSlider;

		// Token: 0x04001870 RID: 6256
		private Control m_SaveLoadItemOn;

		// Token: 0x04001871 RID: 6257
		private Control m_SaveLoadItemOnClick;

		// Token: 0x04001872 RID: 6258
		private Control m_CurrentMission;

		// Token: 0x04001873 RID: 6259
		private Control m_CurrentMissionName;

		// Token: 0x04001874 RID: 6260
		private Control m_CurrentMissionExp;

		// Token: 0x04001875 RID: 6261
		private Control m_TexturePlace;

		// Token: 0x04001876 RID: 6262
		private Control m_SaveExitSprite;

		// Token: 0x04001877 RID: 6263
		private Control m_CheckCover;

		// Token: 0x04001878 RID: 6264
		private Control m_SaveBlock;

		// Token: 0x04001879 RID: 6265
		private List<Control> m_OptionList = new List<Control>();

		// Token: 0x0400187A RID: 6266
		public GameObject m_SaveLoadItem;

		// Token: 0x0400187B RID: 6267
		private int itemIndex;

		// Token: 0x0400187C RID: 6268
		private int titleIndex;

		// Token: 0x0400187D RID: 6269
		private int optionIndex;

		// Token: 0x0400187E RID: 6270
		private int TitleID;

		// Token: 0x0400187F RID: 6271
		private int viewType;

		// Token: 0x04001880 RID: 6272
		private int selectTypeIndex = 1;

		// Token: 0x04001881 RID: 6273
		private int selectIndex;

		// Token: 0x04001882 RID: 6274
		private int maxAmount = 20;

		// Token: 0x04001883 RID: 6275
		private int showMaxAmount = 8;

		// Token: 0x04001884 RID: 6276
		private int currentIndex;

		// Token: 0x02000370 RID: 880
		private enum eState
		{
			// Token: 0x04001889 RID: 6281
			None,
			// Token: 0x0400188A RID: 6282
			General,
			// Token: 0x0400188B RID: 6283
			AutoSave,
			// Token: 0x0400188C RID: 6284
			Check,
			// Token: 0x0400188D RID: 6285
			Saving
		}

		// Token: 0x02000371 RID: 881
		private enum option
		{
			// Token: 0x0400188F RID: 6287
			None,
			// Token: 0x04001890 RID: 6288
			Determine,
			// Token: 0x04001891 RID: 6289
			Cancel
		}
	}
}
