using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000336 RID: 822
	public class UIBackpack : UILayer
	{
		// Token: 0x06001257 RID: 4695 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x000A008C File Offset: 0x0009E28C
		private void Start()
		{
			CtrlBackpack backpackController = this.m_BackpackController;
			backpackController.setItemView = (Action<int, BackpackNewDataNode>)Delegate.Combine(backpackController.setItemView, new Action<int, BackpackNewDataNode>(this.SetItemView));
			CtrlBackpack backpackController2 = this.m_BackpackController;
			backpackController2.setMoney = (Action<int>)Delegate.Combine(backpackController2.setMoney, new Action<int>(this.SetMoney));
			CtrlBackpack backpackController3 = this.m_BackpackController;
			backpackController3.complementAmount = (Action<int>)Delegate.Combine(backpackController3.complementAmount, new Action<int>(this.ComplementAmount));
			CtrlBackpack backpackController4 = this.m_BackpackController;
			backpackController4.setTypeNewAmount = (Action<int, int>)Delegate.Combine(backpackController4.setTypeNewAmount, new Action<int, int>(this.SetTypeNewAmount));
			CtrlBackpack backpackController5 = this.m_BackpackController;
			backpackController5.initMemberView = (Action)Delegate.Combine(backpackController5.initMemberView, new Action(this.InitMemberView));
			CtrlBackpack backpackController6 = this.m_BackpackController;
			backpackController6.setMemberView = (Action<int, MemberData>)Delegate.Combine(backpackController6.setMemberView, new Action<int, MemberData>(this.SetMemberView));
			CtrlBackpack backpackController7 = this.m_BackpackController;
			backpackController7.setItemTipView = (Action<TipData>)Delegate.Combine(backpackController7.setItemTipView, new Action<TipData>(this.SetItemTipView));
			CtrlBackpack backpackController8 = this.m_BackpackController;
			backpackController8.setMouseActive = (Action<int, int, bool>)Delegate.Combine(backpackController8.setMouseActive, new Action<int, int, bool>(this.SetMouseActive));
			CtrlBackpack backpackController9 = this.m_BackpackController;
			backpackController9.resetItem = (Action)Delegate.Combine(backpackController9.resetItem, new Action(this.ResetItem));
			CtrlBackpack backpackController10 = this.m_BackpackController;
			backpackController10.resetClickItem = (Action)Delegate.Combine(backpackController10.resetClickItem, new Action(this.ResetClickItem));
			CtrlBackpack backpackController11 = this.m_BackpackController;
			backpackController11.resetTypeNewAmount = (Action)Delegate.Combine(backpackController11.resetTypeNewAmount, new Action(this.ResetTypeNewAmount));
			CtrlBackpack backpackController12 = this.m_BackpackController;
			backpackController12.setNotUseMemberView = (Action<int, bool>)Delegate.Combine(backpackController12.setNotUseMemberView, new Action<int, bool>(this.SetNotUseMemberView));
			this.m_iCurrentType = 1;
			for (int i = 0; i < 64; i++)
			{
				this.CreatItem();
			}
			this.m_GridItem.GetComponent<UIGrid>().Reposition();
			this.m_BackpackController.GetBackpackData();
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x000A02A8 File Offset: 0x0009E4A8
		public override void OnKeyUp(KeyControl.Key key)
		{
			int sortCount = this.m_BackpackController.GetSortCount();
			switch (this.NowState)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
				switch (key)
				{
				case KeyControl.Key.Up:
					this.ItemOnKey(key, true);
					break;
				case KeyControl.Key.Down:
					this.ItemOnKey(key, false);
					break;
				case KeyControl.Key.OK:
					base.OnCurrentClick();
					break;
				case KeyControl.Key.Cancel:
					this.ExitSpriteOnClick(this.m_ExitSprite.GameObject);
					break;
				case KeyControl.Key.L1:
					this.TypeOnKey(true);
					break;
				case KeyControl.Key.R1:
					this.TypeOnKey(false);
					break;
				}
				break;
			case 7:
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
					this.ExitMember();
					break;
				}
				break;
			}
			if (key == KeyControl.Key.Backpack)
			{
				base.EnterState(0);
			}
			Debug.Log(this.current);
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x000A03F0 File Offset: 0x0009E5F0
		private void ItemOnKey(KeyControl.Key key, bool reverse)
		{
			this.SelectNextButton(key);
			this.m_iSelectItemIndex = int.Parse(this.current.name);
			int sortCount = this.m_BackpackController.GetSortCount();
			int maxAmount = this.m_BackpackController.GetMaxAmount();
			this.SetScrollBar(this.m_iSelectItemIndex, maxAmount, sortCount, reverse, this.m_PackSlider);
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x000A0448 File Offset: 0x0009E648
		private void TypeOnKey(bool reverse = true)
		{
			if (reverse)
			{
				this.m_iSelectTypeIndex--;
			}
			else
			{
				this.m_iSelectTypeIndex++;
			}
			this.m_iSelectTypeIndex = Mathf.Clamp(this.m_iSelectTypeIndex, 1, this.m_ItemTypeList.Count);
			if (this.m_iSelectTypeIndex == this.m_iCurrentType)
			{
				return;
			}
			this.TypeOnClick(this.m_ItemTypeList[this.m_iSelectTypeIndex - 1].GameObject, false);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x000A04CC File Offset: 0x0009E6CC
		protected override void OnStateEnter(int state)
		{
			switch (state)
			{
			case 0:
				this.Hide();
				break;
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
				this.InitView(true);
				break;
			case 7:
				this.InitView(false);
				break;
			}
			if (!GameCursor.IsShow)
			{
				int sortCount = this.m_BackpackController.GetSortCount();
				if (sortCount <= 0)
				{
					this.current = null;
					return;
				}
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

		// Token: 0x0600125D RID: 4701 RVA: 0x000A0608 File Offset: 0x0009E808
		protected override void OnStateExit(int state)
		{
			switch (state)
			{
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
				this.m_CurrentArray[state] = this.current;
				break;
			}
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x000A0654 File Offset: 0x0009E854
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIBackpack.<>f__switch$map28 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(20);
					dictionary.Add("Group", 0);
					dictionary.Add("PackSlider", 1);
					dictionary.Add("ItemType", 2);
					dictionary.Add("NewAmount", 3);
					dictionary.Add("TypeName", 4);
					dictionary.Add("TypeOn", 5);
					dictionary.Add("TypeOnClick", 6);
					dictionary.Add("GridItem", 7);
					dictionary.Add("ItemOn", 8);
					dictionary.Add("ItemOnClick", 9);
					dictionary.Add("Team", 10);
					dictionary.Add("Member", 11);
					dictionary.Add("MemberName", 12);
					dictionary.Add("Equip", 13);
					dictionary.Add("MemberTexture", 14);
					dictionary.Add("MaxHP", 15);
					dictionary.Add("MaxSP", 16);
					dictionary.Add("MoneyAmount", 17);
					dictionary.Add("ExitSprite", 18);
					dictionary.Add("PackScrollView", 19);
					UIBackpack.<>f__switch$map28 = dictionary;
				}
				int num;
				if (UIBackpack.<>f__switch$map28.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_PackSlider = sender;
						break;
					case 2:
					{
						Control control = sender;
						this.m_iSetTypeIndex++;
						control.OnClick += this.ItemTypeOnClick;
						control.OnHover += this.ItemTypeOnHover;
						control.GameObject.name = this.m_iSetTypeIndex.ToString();
						this.m_ItemTypeList.Add(control);
						break;
					}
					case 3:
					{
						Control control = sender;
						this.m_NewAmountList.Add(control);
						break;
					}
					case 4:
						this.m_TypeName = sender;
						break;
					case 5:
						this.m_TypeOn = sender;
						break;
					case 6:
						this.m_TypeOnClick = sender;
						break;
					case 7:
						this.m_GridItem = sender;
						break;
					case 8:
						this.m_ItemOn = sender;
						break;
					case 9:
						this.m_ItemOnClick = sender;
						break;
					case 10:
						this.m_Team = sender;
						this.Team_TweenPos = sender.GetComponent<TweenPosition>();
						EventDelegate.Add(this.Team_TweenPos.onFinished, new EventDelegate.Callback(this.TeamOnFinished));
						break;
					case 11:
					{
						Control control = sender;
						control.OnClick += this.MemberOnClick;
						control.OnKeySelect += this.MemberOnKeySelect;
						control.OnHover += this.MemberOnHover;
						control.GameObject.name = this.m_iSetMemberIndex.ToString();
						base.SetInputButton(7, control.Listener);
						this.m_MemberList.Add(control);
						this.m_iSetMemberIndex++;
						break;
					}
					case 12:
					{
						Control control = sender;
						this.m_MemberNameList.Add(control);
						break;
					}
					case 13:
					{
						Control control = sender;
						this.m_EquipList.Add(control);
						break;
					}
					case 14:
					{
						Control control = sender;
						this.m_MemberTextureList.Add(control);
						break;
					}
					case 15:
					{
						Control control = sender;
						this.m_MaxHPList.Add(control);
						break;
					}
					case 16:
					{
						Control control = sender;
						this.m_MaxSPList.Add(control);
						break;
					}
					case 17:
						this.m_MoneyAmount = sender;
						break;
					case 18:
					{
						Control control = sender;
						control.OnHover += this.ExitSpriteOnHover;
						control.OnClick += this.ExitSpriteOnClick;
						this.m_ExitSprite = sender;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					case 19:
						this.m_PackScrollView = sender;
						break;
					}
				}
			}
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x000A0A88 File Offset: 0x0009EC88
		protected override void AssignWidget(Widget sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIBackpack.<>f__switch$map29 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
					dictionary.Add("ExplainArea", 0);
					UIBackpack.<>f__switch$map29 = dictionary;
				}
				int num;
				if (UIBackpack.<>f__switch$map29.TryGetValue(name, ref num))
				{
					if (num == 0)
					{
						this.m_ExplainArea = (sender as WgBackPackTip);
					}
				}
			}
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x000A0AF4 File Offset: 0x0009ECF4
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			Game.g_InputManager.Push(this);
			GameGlobal.m_bCFormOpen = this.m_bShow;
			this.m_Group.GameObject.SetActive(this.m_bShow);
			this.m_BackpackController.UpdateBackpackView();
			this.TypeOnClick(this.m_ItemTypeList[this.m_iCurrentType - 1].GameObject, true);
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000A0B6C File Offset: 0x0009ED6C
		public override void Hide()
		{
			this.m_BackpackController.CloseView();
			for (int i = 0; i < Enum.GetNames(typeof(UIBackpack.eState)).Length; i++)
			{
				this.m_CurrentArray[i] = null;
			}
			this.m_Group.GameObject.SetActive(false);
			GameGlobal.m_bCFormOpen = false;
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			Game.g_InputManager.Pop();
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x0000BE27 File Offset: 0x0000A027
		private void ExitSpriteOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_ExitSprite.SpriteName = "cdata_027";
			if (this.NowState == 7)
			{
				this.Team_TweenPos.ResetToBeginning();
			}
			base.EnterState(0);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00095C18 File Offset: 0x00093E18
		private void ExitSpriteOnHover(GameObject go, bool bHover)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
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

		// Token: 0x06001264 RID: 4708 RVA: 0x0000BE63 File Offset: 0x0000A063
		private void ItemTypeOnClick(GameObject go)
		{
			this.TypeOnClick(go, false);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x000A0BE4 File Offset: 0x0009EDE4
		private void TypeOnClick(GameObject goes, bool action)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.SetControl(this.m_TypeOnClick, goes, -1f, 2f, 0f);
			this.m_iCurrentType = int.Parse(goes.gameObject.name);
			this.m_TypeName.Text = Game.StringTable.GetString(110062 + this.m_iCurrentType);
			this.m_iSelectTypeIndex = this.m_iCurrentType;
			this.m_iSelectItemIndex = 0;
			this.m_BackpackController.UpdateItemView(action, this.m_iCurrentType);
			this.current = null;
			base.EnterState(this.m_iCurrentType);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0000BE6D File Offset: 0x0000A06D
		private void ItemTypeOnHover(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.SetControl(this.m_TypeOn, go, -1f, 2f, 0f);
			}
			else
			{
				this.m_TypeOn.GameObject.SetActive(false);
			}
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x000A0C88 File Offset: 0x0009EE88
		private void ResetTypeNewAmount()
		{
			for (int i = 0; i < this.m_ItemTypeList.Count; i++)
			{
				this.m_NewAmountList[i].GameObject.SetActive(false);
			}
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x000A0CC8 File Offset: 0x0009EEC8
		private void ItemOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int memberCount = this.m_BackpackController.GetMemberCount();
			if (memberCount > 0)
			{
				if (this.m_iCurrentType == 5)
				{
					return;
				}
				this.m_Team.GameObject.SetActive(true);
				this.m_iSelectItemIndex = int.Parse(go.name);
				if (!this.m_BackpackController.CheckUseTime(this.m_iSelectItemIndex))
				{
					this.m_BackpackController.SetMsg();
					return;
				}
				this.m_BackpackController.CheckMemberCanUse();
				this.m_BackpackController.SetMemberData(this.m_iCurrentType);
				this.Team_TweenPos.PlayForward();
				this.SetControl(this.m_ItemOnClick, go, -80f, 0f, 0f);
				for (int i = 0; i < this.m_MemberList.Count; i++)
				{
					if (this.m_MemberList[i].Collider.enabled)
					{
						this.m_CurrentArray[7] = this.m_MemberList[i].Listener;
						break;
					}
				}
				this.current = UIEventListener.Get(go.gameObject);
				base.EnterState(7);
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0000BEA7 File Offset: 0x0000A0A7
		private void ItmeOnHover(GameObject go, bool bHover)
		{
			if (!go.gameObject.activeSelf || !GameCursor.IsShow)
			{
				return;
			}
			this.SetItemExplain(go, bHover);
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x000A0DFC File Offset: 0x0009EFFC
		private void ItemOnKeySelect(GameObject go, bool bSelect)
		{
			if (!go.gameObject.activeSelf || GameCursor.IsShow)
			{
				return;
			}
			this.SetItemExplain(go, bSelect);
			this.ShowKeySelect(go, new Vector3(-45f, 0f, 0f), KeySelect.eSelectDir.Left, 20, 20);
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x000A0E4C File Offset: 0x0009F04C
		private void SetItemExplain(GameObject goes, bool on)
		{
			if (on)
			{
				int num = int.Parse(goes.gameObject.name);
				this.m_BackpackController.SetTipData(num);
				this.SetControl(this.m_ItemOn, goes, -80f, 0f, 0f);
				if (GameCursor.IsShow)
				{
					this.m_iSelectItemIndex = num;
					this.current = UIEventListener.Get(goes);
				}
			}
			else
			{
				this.m_ItemOn.GameObject.SetActive(false);
				this.m_ExplainArea.SetActive(false);
			}
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0000BECC File Offset: 0x0000A0CC
		private void SetItemTipView(TipData tipDtat)
		{
			this.m_ExplainArea.SetActive(true);
			this.m_ExplainArea.SetItemTip(tipDtat);
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0000BEE6 File Offset: 0x0000A0E6
		private void SetMoney(int money)
		{
			this.m_MoneyAmount.Text = money.ToString();
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x000A0ED8 File Offset: 0x0009F0D8
		private void ComplementAmount(int Count)
		{
			if (Count > this.m_NameList.Count)
			{
				int num = Count - this.m_NameList.Count;
				for (int i = 0; i < num; i++)
				{
					this.CreatItem();
				}
			}
			this.m_GridItem.GetComponent<UIGrid>().Reposition();
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x000A0F2C File Offset: 0x0009F12C
		private void SetItemView(int index, BackpackNewDataNode backpackDataNode)
		{
			this.m_NewList[index].GameObject.SetActive(backpackDataNode.m_bNew);
			this.m_NameList[index].GameObject.SetActive(true);
			this.m_NameList[index].Text = backpackDataNode._ItemDataNode.m_strItemName;
			this.m_PriceList[index].Text = backpackDataNode._ItemDataNode.m_iItemSell.ToString();
			this.m_AmountList[index].Text = backpackDataNode.m_iAmount.ToString();
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0000BEFA File Offset: 0x0000A0FA
		private void SetTypeNewAmount(int index, int newAmount)
		{
			this.m_NewAmountList[index].GameObject.SetActive(true);
			this.m_NewAmountList[index].Text = newAmount.ToString();
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x000A0FC8 File Offset: 0x0009F1C8
		private void ResetItem()
		{
			for (int i = 0; i < this.m_NameList.Count; i++)
			{
				this.m_NameList[i].GameObject.SetActive(false);
				this.m_NewList[i].GameObject.SetActive(false);
			}
		}

		// Token: 0x06001272 RID: 4722 RVA: 0x000A1020 File Offset: 0x0009F220
		private void ResetClickItem()
		{
			if (this.Member_TweenPos != null)
			{
				this.Member_TweenPos.PlayReverse();
			}
			this.Team_TweenPos.PlayReverse();
			this.m_ItemOnClick.GameObject.SetActive(false);
			for (int i = 0; i < this.m_MemberList.Count; i++)
			{
				this.m_MemberList[i].SpriteName = "ui_t_use_001";
			}
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0000BF2B File Offset: 0x0000A12B
		private void SetMouseActive(int count, int maxAmount, bool bReset)
		{
			this.CheckMouseWheel(count, maxAmount, this.m_PackSlider, this.m_PackScrollView, bReset);
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0000BF42 File Offset: 0x0000A142
		private void TeamOnFinished()
		{
			if (this.m_Team.GameObject.transform.localPosition == this.Team_TweenPos.from)
			{
				this.m_Team.GameObject.SetActive(false);
			}
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x000A1098 File Offset: 0x0009F298
		private void MemberOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int index = int.Parse(go.gameObject.name);
			this.m_BackpackController.MemberUseItem(index, this.m_iCurrentType);
			base.EnterState(this.m_iCurrentType);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0000BF7F File Offset: 0x0000A17F
		private void MemberOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.OnMemberStatus(go, bHover);
			}
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0000BF93 File Offset: 0x0000A193
		private void MemberOnKeySelect(GameObject go, bool bSelect)
		{
			this.OnMemberStatus(go, bSelect);
			this.ShowKeySelect(go, new Vector3(280f, 0f, 0f), KeySelect.eSelectDir.Right, 20, 20);
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x000A10E0 File Offset: 0x0009F2E0
		private void OnMemberStatus(GameObject goes, bool on)
		{
			this.Member_TweenPos = goes.GetComponent<TweenPosition>();
			UISprite component = goes.GetComponent<UISprite>();
			if (on)
			{
				this.Member_TweenPos.PlayForward();
				component.spriteName = "ui_t_use_002";
				if (GameCursor.IsShow)
				{
					this.current = UIEventListener.Get(goes.gameObject);
				}
			}
			else
			{
				this.Member_TweenPos.PlayReverse();
				component.spriteName = "ui_t_use_001";
			}
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x000A1154 File Offset: 0x0009F354
		private void InitMemberView()
		{
			for (int i = 0; i < this.m_MemberList.Count; i++)
			{
				this.m_MemberList[i].GameObject.SetActive(false);
			}
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x000A1194 File Offset: 0x0009F394
		private void SetMemberView(int index, MemberData memberData)
		{
			this.m_MemberList[index].GameObject.SetActive(true);
			if (memberData.texture != null)
			{
				this.m_MemberTextureList[index].Texture = memberData.texture;
			}
			else
			{
				this.m_MemberTextureList[index].Texture = null;
			}
			this.m_MemberNameList[index].Text = memberData.name;
			this.m_EquipList[index].Text = memberData.equip;
			this.m_MaxHPList[index].Text = memberData.maxHP;
			this.m_MaxSPList[index].Text = memberData.maxSP;
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x000A1254 File Offset: 0x0009F454
		private void SetNotUseMemberView(int index, bool canUse)
		{
			this.m_MemberList[index].Collider.enabled = canUse;
			UITexture component = this.m_MemberTextureList[index].GetComponent<UITexture>();
			if (canUse)
			{
				component.color = Color.white;
			}
			else
			{
				component.color = new Color(1f, 1f, 1f, 0.5f);
			}
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x000A12C0 File Offset: 0x0009F4C0
		private void InitView(bool bOpen = true)
		{
			this.m_ExplainArea.SetActive(false);
			this.m_TypeOn.GameObject.SetActive(false);
			this.m_ItemOn.GameObject.SetActive(false);
			this.m_ItemOnClick.GameObject.SetActive(!bOpen);
			for (int i = 0; i < this.m_NameList.Count; i++)
			{
				this.m_NameList[i].Collider.enabled = bOpen;
			}
			for (int j = 0; j < this.m_ItemTypeList.Count; j++)
			{
				this.m_ItemTypeList[j].Collider.enabled = bOpen;
			}
			if (!bOpen)
			{
				this.m_PackSlider.GameObject.SetActive(false);
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x000973F0 File Offset: 0x000955F0
		public override void OnMouseControl(bool bCtrl)
		{
			if (!this.controls.ContainsKey(this.NowState))
			{
				return;
			}
			List<UIEventListener> list = this.controls[this.NowState];
			for (int i = 0; i < list.Count; i++)
			{
				if (bCtrl)
				{
					if (list[i].onKeySelect != null)
					{
						this.HideKeySelect();
						list[i].onKeySelect(list[i].gameObject, false);
					}
				}
				else if (list[i].onHover != null)
				{
					list[i].onHover(list[i].gameObject, false);
				}
			}
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x000A138C File Offset: 0x0009F58C
		private void ExitMember()
		{
			this.HideKeySelect();
			this.ResetClickItem();
			int sortCount = this.m_BackpackController.GetSortCount();
			int maxAmount = this.m_BackpackController.GetMaxAmount();
			this.SetMouseActive(sortCount, maxAmount, false);
			base.EnterState(this.m_iCurrentType);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x000A13D4 File Offset: 0x0009F5D4
		private void CreatItem()
		{
			GameObject gameObject = Object.Instantiate(this.ItemNde) as GameObject;
			gameObject.transform.parent = this.m_GridItem.GameObject.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "BackpackItem";
			gameObject.transform.Traversal(delegate(Transform x)
			{
				string name = x.name;
				if (name != null)
				{
					if (UIBackpack.<>f__switch$map2B == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
						dictionary.Add("New", 0);
						dictionary.Add("Name", 1);
						dictionary.Add("Amount", 2);
						dictionary.Add("Price", 3);
						UIBackpack.<>f__switch$map2B = dictionary;
					}
					int num;
					if (UIBackpack.<>f__switch$map2B.TryGetValue(name, ref num))
					{
						switch (num)
						{
						case 0:
							this.m_NewList.Add(x);
							break;
						case 1:
							x.GameObject.name = this.m_iSetItemIndex.ToString();
							x.OnClick += this.ItemOnClick;
							x.OnKeySelect += this.ItemOnKeySelect;
							x.OnHover += this.ItmeOnHover;
							base.SetInputButton(1, x.Listener);
							base.SetInputButton(2, x.Listener);
							base.SetInputButton(3, x.Listener);
							base.SetInputButton(4, x.Listener);
							base.SetInputButton(5, x.Listener);
							base.SetInputButton(6, x.Listener);
							this.m_iSetItemIndex++;
							this.m_NameList.Add(x);
							break;
						case 2:
							this.m_AmountList.Add(x);
							break;
						case 3:
							this.m_PriceList.Add(x);
							break;
						}
					}
				}
			});
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x000A1450 File Offset: 0x0009F650
		private void Update()
		{
			if (this.m_Group.GameObject.activeSelf && this.NowState != 7 && this.m_PackSlider.GameObject.activeSelf)
			{
				this.SetMouseWheel(this.m_PackSlider);
			}
			if (this.NowState == 7 && Input.GetMouseButtonUp(1))
			{
				this.ExitMember();
			}
		}

		// Token: 0x0400163D RID: 5693
		private CtrlBackpack m_BackpackController = new CtrlBackpack();

		// Token: 0x0400163E RID: 5694
		private UIEventListener[] m_CurrentArray = new UIEventListener[Enum.GetNames(typeof(UIBackpack.eState)).Length];

		// Token: 0x0400163F RID: 5695
		private Control m_Group;

		// Token: 0x04001640 RID: 5696
		private Control m_PackSlider;

		// Token: 0x04001641 RID: 5697
		private List<Control> m_NewAmountList = new List<Control>();

		// Token: 0x04001642 RID: 5698
		private List<Control> m_ItemTypeList = new List<Control>();

		// Token: 0x04001643 RID: 5699
		private Control m_TypeName;

		// Token: 0x04001644 RID: 5700
		private Control m_TypeOn;

		// Token: 0x04001645 RID: 5701
		private Control m_TypeOnClick;

		// Token: 0x04001646 RID: 5702
		private Control m_PackScrollView;

		// Token: 0x04001647 RID: 5703
		private Control m_GridItem;

		// Token: 0x04001648 RID: 5704
		private List<Control> m_NameList = new List<Control>();

		// Token: 0x04001649 RID: 5705
		private List<Control> m_NewList = new List<Control>();

		// Token: 0x0400164A RID: 5706
		private List<Control> m_AmountList = new List<Control>();

		// Token: 0x0400164B RID: 5707
		private List<Control> m_PriceList = new List<Control>();

		// Token: 0x0400164C RID: 5708
		private Control m_ItemOn;

		// Token: 0x0400164D RID: 5709
		private Control m_ItemOnClick;

		// Token: 0x0400164E RID: 5710
		private WgBackPackTip m_ExplainArea;

		// Token: 0x0400164F RID: 5711
		private Control m_Team;

		// Token: 0x04001650 RID: 5712
		private List<Control> m_MemberList = new List<Control>();

		// Token: 0x04001651 RID: 5713
		private List<Control> m_EquipList = new List<Control>();

		// Token: 0x04001652 RID: 5714
		private List<Control> m_MemberNameList = new List<Control>();

		// Token: 0x04001653 RID: 5715
		private List<Control> m_MemberTextureList = new List<Control>();

		// Token: 0x04001654 RID: 5716
		private List<Control> m_MaxHPList = new List<Control>();

		// Token: 0x04001655 RID: 5717
		private List<Control> m_MaxSPList = new List<Control>();

		// Token: 0x04001656 RID: 5718
		private Control m_MoneyAmount;

		// Token: 0x04001657 RID: 5719
		private Control m_ExitSprite;

		// Token: 0x04001658 RID: 5720
		private int m_iSetTypeIndex;

		// Token: 0x04001659 RID: 5721
		private int m_iSetItemIndex;

		// Token: 0x0400165A RID: 5722
		private int m_iSetMemberIndex;

		// Token: 0x0400165B RID: 5723
		private int m_iCurrentType;

		// Token: 0x0400165C RID: 5724
		private int m_iSelectTypeIndex;

		// Token: 0x0400165D RID: 5725
		private int m_iSelectItemIndex;

		// Token: 0x0400165E RID: 5726
		private int m_iRowIndex;

		// Token: 0x0400165F RID: 5727
		public GameObject ItemNde;

		// Token: 0x04001660 RID: 5728
		private TweenPosition Team_TweenPos;

		// Token: 0x04001661 RID: 5729
		private TweenPosition Member_TweenPos;

		// Token: 0x02000337 RID: 823
		private enum eState
		{
			// Token: 0x04001667 RID: 5735
			None,
			// Token: 0x04001668 RID: 5736
			enterWapon,
			// Token: 0x04001669 RID: 5737
			enterArror,
			// Token: 0x0400166A RID: 5738
			enterNecklace,
			// Token: 0x0400166B RID: 5739
			enterSolution,
			// Token: 0x0400166C RID: 5740
			enterMission,
			// Token: 0x0400166D RID: 5741
			enterTipsBook,
			// Token: 0x0400166E RID: 5742
			enterMember
		}
	}
}
