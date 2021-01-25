using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000321 RID: 801
	public class UIShop : UILayer
	{
		// Token: 0x06001178 RID: 4472 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00095F60 File Offset: 0x00094160
		private void Start()
		{
			CtrlShop shopController = this.m_ShopController;
			shopController.setView = (Action<ItemData, int>)Delegate.Combine(shopController.setView, new Action<ItemData, int>(this.SetView));
			CtrlShop shopController2 = this.m_ShopController;
			shopController2.complementAmount = (Action<int, int>)Delegate.Combine(shopController2.complementAmount, new Action<int, int>(this.ComplementAmount));
			CtrlShop shopController3 = this.m_ShopController;
			shopController3.setMoneyView = (Action<int>)Delegate.Combine(shopController3.setMoneyView, new Action<int>(this.SetMoneyView));
			CtrlShop shopController4 = this.m_ShopController;
			shopController4.setItemTipView = (Action<TipData>)Delegate.Combine(shopController4.setItemTipView, new Action<TipData>(this.SetItemTipView));
			CtrlShop shopController5 = this.m_ShopController;
			shopController5.setInputView = (Action<int, int, int>)Delegate.Combine(shopController5.setInputView, new Action<int, int, int>(this.SetInputView));
			CtrlShop shopController6 = this.m_ShopController;
			shopController6.initView = (Action)Delegate.Combine(shopController6.initView, new Action(this.InitView));
			CtrlShop shopController7 = this.m_ShopController;
			shopController7.setMouseActive = (Action<int, bool>)Delegate.Combine(shopController7.setMouseActive, new Action<int, bool>(this.SetMouseActive));
			this.packItemIndex = 0;
			this.shopItemIndex = 0;
			for (int i = 0; i < 30; i++)
			{
				this.CreatPackItem();
				this.CreatShopItem();
			}
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x000960AC File Offset: 0x000942AC
		public override void OnKeyUp(KeyControl.Key key)
		{
			switch (this.NowState)
			{
			case 1:
			case 2:
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
					base.EnterState(0);
					break;
				case KeyControl.Key.L1:
					this.ComputeIndex(true);
					break;
				case KeyControl.Key.R1:
					this.ComputeIndex(false);
					break;
				}
				break;
			case 3:
			case 4:
				switch (key)
				{
				case KeyControl.Key.Up:
				case KeyControl.Key.Down:
				case KeyControl.Key.Left:
				case KeyControl.Key.Right:
					this.SelectNextButton(key);
					break;
				case KeyControl.Key.OK:
					base.OnCurrentClick();
					break;
				}
				break;
			}
			Debug.Log(this.current);
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x000961A8 File Offset: 0x000943A8
		protected override void OnStateEnter(int state)
		{
			switch (state)
			{
			case 0:
				this.Hide();
				break;
			case 1:
			case 2:
				this.m_InputArea.GameObject.SetActive(false);
				this.HideKeySelect();
				this.m_PackItemOn.GameObject.SetActive(false);
				this.m_ShopItemOn.GameObject.SetActive(false);
				this.m_PackItemOnClick.GameObject.SetActive(false);
				this.m_ShopItemOnClick.GameObject.SetActive(false);
				break;
			case 3:
			case 4:
				this.initInputView();
				this.m_InputArea.GameObject.SetActive(true);
				break;
			}
			if (!GameCursor.IsShow)
			{
				if (this.NowState == 1 || this.NowState == 2)
				{
					int count = this.m_ShopController.GetCount(this.NowState + 2);
					if (count <= 0)
					{
						this.current = null;
						return;
					}
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

		// Token: 0x0600117C RID: 4476 RVA: 0x0009635C File Offset: 0x0009455C
		protected override void OnStateExit(int state)
		{
			if (state == 1 || state == 2)
			{
				this.m_CurrentArray[state] = this.current;
			}
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00096394 File Offset: 0x00094594
		private void ComputeIndex(bool reverse = true)
		{
			this.stateIndex = this.NowState;
			if (reverse)
			{
				this.stateIndex--;
			}
			else
			{
				this.stateIndex++;
			}
			this.stateIndex = Mathf.Clamp(this.stateIndex, 1, 2);
			int showAmount = this.m_ShopController.GetShowAmount();
			this.SetMouseActive(showAmount, true);
			this.current = null;
			base.EnterState(this.stateIndex);
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00096410 File Offset: 0x00094610
		private void ItemOnKey(KeyControl.Key key, bool reverse = true)
		{
			this.SelectNextButton(key);
			int iSelectIdx = int.Parse(this.current.name);
			int count = this.m_ShopController.GetCount(this.NowState + 2);
			if (this.NowState == 1)
			{
				this.SetScrollBar(iSelectIdx, 16, count, reverse, this.m_PackSlider);
			}
			else
			{
				this.SetScrollBar(iSelectIdx, 16, count, reverse, this.m_ShopSlider);
			}
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0009647C File Offset: 0x0009467C
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIShop.<>f__switch$map1A == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(23);
					dictionary.Add("Group", 0);
					dictionary.Add("ExitSprite", 1);
					dictionary.Add("ItemEffect", 2);
					dictionary.Add("GridPackItem", 3);
					dictionary.Add("PackSlider", 4);
					dictionary.Add("GridShopItem", 5);
					dictionary.Add("ShopSlider", 6);
					dictionary.Add("PackItemOn", 7);
					dictionary.Add("ShopItemOn", 8);
					dictionary.Add("PackItemOnClick", 9);
					dictionary.Add("ShopItemOnClick", 10);
					dictionary.Add("PackValue", 11);
					dictionary.Add("PackViewSprite", 12);
					dictionary.Add("ShopViewSprite", 13);
					dictionary.Add("PackView", 14);
					dictionary.Add("ShopView", 15);
					dictionary.Add("InputArea", 16);
					dictionary.Add("InputAmount", 17);
					dictionary.Add("Amount", 18);
					dictionary.Add("Add", 19);
					dictionary.Add("Less", 20);
					dictionary.Add("Price", 21);
					dictionary.Add("InputOption", 22);
					UIShop.<>f__switch$map1A = dictionary;
				}
				int num;
				if (UIShop.<>f__switch$map1A.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
					{
						Control control = sender;
						control.OnHover += this.ExitSpriteOnHover;
						control.OnClick += this.ExitSpriteOnClick;
						this.m_ExitSprite = sender;
						break;
					}
					case 2:
					{
						Control control = sender;
						this.m_ItemEffectList.Add(control);
						break;
					}
					case 3:
						this.m_GridPackItem = sender;
						break;
					case 4:
						this.m_PackSlider = sender;
						break;
					case 5:
						this.m_GridShopItem = sender;
						break;
					case 6:
						this.m_ShopSlider = sender;
						break;
					case 7:
						this.m_PackItemOn = sender;
						break;
					case 8:
						this.m_ShopItemOn = sender;
						break;
					case 9:
						this.m_PackItemOnClick = sender;
						break;
					case 10:
						this.m_ShopItemOnClick = sender;
						break;
					case 11:
						this.m_PackValue = sender;
						break;
					case 12:
					{
						Control control = sender;
						control.OnHover += this.PackViewSpriteOnHover;
						break;
					}
					case 13:
					{
						Control control = sender;
						control.OnHover += this.ShopViewSpriteOnHover;
						break;
					}
					case 14:
						this.m_PackView = sender;
						break;
					case 15:
						this.m_ShopView = sender;
						break;
					case 16:
						this.m_InputArea = sender;
						break;
					case 17:
						this.m_InputAmount = sender;
						break;
					case 18:
						this.m_Amount = sender;
						break;
					case 19:
					{
						Control control = sender;
						control.OnHover += this.AddOnHover;
						control.OnClick += this.AddOnClick;
						control.OnKeySelect += this.AddOnKeySelect;
						base.SetInputButton(4, control.Listener);
						base.SetInputButton(3, control.Listener);
						this.m_Add = sender;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.AddSub);
						break;
					}
					case 20:
					{
						Control control = sender;
						control.OnHover += this.LessOnHover;
						control.OnClick += this.LessOnClick;
						control.OnKeySelect += this.LessOnKeySelect;
						base.SetInputButton(4, control.Listener);
						base.SetInputButton(3, control.Listener);
						this.m_Less = sender;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.AddSub);
						break;
					}
					case 21:
						this.m_Price = sender;
						break;
					case 22:
					{
						Control control = sender;
						control.OnHover += this.InputOptionOnHover;
						control.OnClick += this.InputOptionOnClick;
						control.OnKeySelect += this.InputOptionOnKeySelect;
						base.SetInputButton(4, control.Listener);
						base.SetInputButton(3, control.Listener);
						control.GameObject.name = this.optionIndex.ToString();
						this.optionIndex++;
						this.m_InputOptionList.Add(control);
						break;
					}
					}
				}
			}
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00096958 File Offset: 0x00094B58
		protected override void AssignWidget(Widget sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIShop.<>f__switch$map1B == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
					dictionary.Add("ExplainArea", 0);
					UIShop.<>f__switch$map1B = dictionary;
				}
				int num;
				if (UIShop.<>f__switch$map1B.TryGetValue(name, ref num))
				{
					if (num == 0)
					{
						this.m_ExplainArea = (sender as WgBackPackTip);
					}
				}
			}
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x000969C4 File Offset: 0x00094BC4
		public override void Show()
		{
			Game.UI.Hide<UIMainSelect>();
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			GameGlobal.m_bCFormOpen = true;
			Game.g_InputManager.Push(this);
			this.m_Group.GameObject.SetActive(this.m_bShow);
			this.m_ShopController.UpdateShopData(true);
			base.EnterState(1);
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00096A28 File Offset: 0x00094C28
		public override void Hide()
		{
			this.ChangeItemNewStatus();
			for (int i = 0; i < Enum.GetNames(typeof(UIShop.eState)).Length; i++)
			{
				this.m_CurrentArray[i] = null;
			}
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			GameGlobal.m_bCFormOpen = false;
			this.m_Group.GameObject.SetActive(this.m_bShow);
			Game.g_InputManager.Pop();
			Game.UI.Get<UITalk>().CloseShopTalk();
			if (GameGlobal.m_bDLCMode)
			{
				Game.UI.Get<UIReadyCombat>().UpdateGold();
			}
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x0000B627 File Offset: 0x00009827
		private void ExitSpriteOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_ExitSprite.SpriteName = "cdata_027";
			base.EnterState(0);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00095C18 File Offset: 0x00093E18
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

		// Token: 0x06001185 RID: 4485 RVA: 0x00096AC8 File Offset: 0x00094CC8
		private void ChangeItemNewStatus()
		{
			for (int i = 0; i < this.m_TeamBackpackList.Count; i++)
			{
				BackpackStatus.m_Instance.ChangeNew(this.m_TeamBackpackList[i]);
			}
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00096B08 File Offset: 0x00094D08
		private void initInputView()
		{
			this.m_PackItemOn.GameObject.SetActive(false);
			this.m_ShopItemOn.GameObject.SetActive(false);
			this.m_ShopController.GetItemIndex(this.currentIndex, this.NowState);
			string @string = Game.StringTable.GetString(130006 + this.NowState);
			this.m_InputAmount.Text = @string;
			this.m_ShopController.InitAmountPrice(this.NowState);
			for (int i = 0; i < this.m_InputOptionList.Count; i++)
			{
				this.m_InputOptionList[i].SpriteName = "ui_sys_09";
			}
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00096BB4 File Offset: 0x00094DB4
		private void InputOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (int.Parse(go.name) == 0)
			{
				this.m_ShopController.CheckOut(this.NowState);
			}
			if (this.NowState == 4)
			{
				base.EnterState(2);
			}
			else
			{
				base.EnterState(1);
			}
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00096C10 File Offset: 0x00094E10
		private void InputOptionOnHover(GameObject go, bool bHover)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
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

		// Token: 0x06001189 RID: 4489 RVA: 0x00096C50 File Offset: 0x00094E50
		private void InputOptionOnKeySelect(GameObject go, bool bSelect)
		{
			UISprite component = go.GetComponent<UISprite>();
			if (bSelect)
			{
				component.spriteName = "ui_sys_10";
			}
			else
			{
				component.spriteName = "ui_sys_09";
			}
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00096C88 File Offset: 0x00094E88
		private void SetInputView(int amount, int price, int max = 99)
		{
			int money = BackpackStatus.m_Instance.GetMoney();
			this.m_Add.UISprite.color = Color.white;
			this.m_Add.Collider.enabled = true;
			this.m_Less.UISprite.color = Color.white;
			this.m_Less.Collider.enabled = true;
			this.m_Amount.UILabel.color = Color.white;
			this.m_Price.UILabel.color = Color.white;
			this.m_InputOptionList[0].Collider.enabled = true;
			this.m_InputOptionList[0].GetComponent<UISprite>().alpha = 1f;
			if (amount == 0)
			{
				this.m_Less.UISprite.color = new Color(1f, 1f, 1f, 0.5f);
				this.m_Less.Collider.enabled = false;
			}
			if (amount >= max)
			{
				this.m_Add.UISprite.color = new Color(1f, 1f, 1f, 0.5f);
				this.m_Add.Collider.enabled = false;
			}
			if (money < price && this.NowState == 4)
			{
				this.m_Amount.UILabel.color = Color.red;
				this.m_Price.UILabel.color = Color.red;
				this.m_InputOptionList[0].Collider.enabled = false;
				this.m_InputOptionList[0].GetComponent<UISprite>().alpha = 0.5f;
			}
			else if (amount == max)
			{
				this.m_Amount.UILabel.color = new Color(0.11764706f, 1f, 0f, 1f);
			}
			this.m_Amount.Text = amount.ToString();
			this.m_Price.Text = price.ToString();
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0000B64C File Offset: 0x0000984C
		private void AddOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_ShopController.ComplementAdd(this.NowState);
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x00096E94 File Offset: 0x00095094
		private void AddOnHover(GameObject go, bool bHover)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
			UISprite component = go.GetComponent<UISprite>();
			if (bHover)
			{
				component.spriteName = "team_033";
			}
			else
			{
				component.spriteName = "team_032";
			}
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x00096ED4 File Offset: 0x000950D4
		private void AddOnKeySelect(GameObject go, bool bSelect)
		{
			UISprite component = go.GetComponent<UISprite>();
			if (bSelect)
			{
				component.spriteName = "team_033";
			}
			else
			{
				component.spriteName = "team_032";
			}
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0000B66B File Offset: 0x0000986B
		private void LessOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_ShopController.ComplementLess(this.NowState);
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00096F0C File Offset: 0x0009510C
		private void LessOnKeySelect(GameObject go, bool bSelect)
		{
			UISprite component = go.GetComponent<UISprite>();
			if (bSelect)
			{
				component.spriteName = "team_036";
			}
			else
			{
				component.spriteName = "team_035";
			}
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00096F44 File Offset: 0x00095144
		private void LessOnHover(GameObject go, bool bHover)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
			UISprite component = go.GetComponent<UISprite>();
			if (bHover)
			{
				component.spriteName = "team_036";
			}
			else
			{
				component.spriteName = "team_035";
			}
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0000B68A File Offset: 0x0000988A
		private void PackItemOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.SetControl(this.m_PackItemOnClick, go, 260f, -4f, 0f);
			base.EnterState(3);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0000B6BB File Offset: 0x000098BB
		private void ShopItemOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.SetControl(this.m_ShopItemOnClick, go, 260f, -4f, 0f);
			base.EnterState(4);
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0000B6EC File Offset: 0x000098EC
		private void PackViewSpriteOnHover(GameObject go, bool bHover)
		{
			this.packSlider = bHover;
			this.SpriteOnHover(bHover, 1);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0000B6FD File Offset: 0x000098FD
		private void ShopViewSpriteOnHover(GameObject go, bool bHover)
		{
			this.shopSlider = bHover;
			this.SpriteOnHover(bHover, 2);
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0000B70E File Offset: 0x0000990E
		private void SpriteOnHover(bool hover, int state)
		{
			if (hover && GameCursor.IsShow)
			{
				this.current = null;
				base.EnterState(state);
			}
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0000B72E File Offset: 0x0000992E
		private void PackItemOnHover(GameObject go, bool bHover)
		{
			if (!go.gameObject.activeSelf || !GameCursor.IsShow)
			{
				return;
			}
			this.OnItemStatus(go, bHover, this.m_PackItemOn, true);
			this.packSlider = bHover;
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00096F84 File Offset: 0x00095184
		private void PackItemOnKeySelect(GameObject go, bool bSelect)
		{
			if (!go.gameObject.activeSelf || GameCursor.IsShow)
			{
				return;
			}
			this.ShowKeySelect(go, new Vector3(-20f, 0f, 0f), KeySelect.eSelectDir.Left, 20, 20);
			this.OnItemStatus(go, bSelect, this.m_PackItemOn, true);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0000B761 File Offset: 0x00009961
		private void ShopItemOnHover(GameObject go, bool bHover)
		{
			if (!go.gameObject.activeSelf || !GameCursor.IsShow)
			{
				return;
			}
			this.OnItemStatus(go, bHover, this.m_ShopItemOn, false);
			this.shopSlider = bHover;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00096FDC File Offset: 0x000951DC
		private void ShopItemOnKeySelect(GameObject go, bool bSelect)
		{
			if (!go.gameObject.activeSelf || GameCursor.IsShow)
			{
				return;
			}
			this.ShowKeySelect(go, new Vector3(-20f, 0f, 0f), KeySelect.eSelectDir.Left, 20, 20);
			this.OnItemStatus(go, bSelect, this.m_ShopItemOn, false);
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0000B794 File Offset: 0x00009994
		private void SetItemTipView(TipData tipDtat)
		{
			this.m_ExplainArea.SetActive(true);
			this.m_ExplainArea.SetItemTip(tipDtat);
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00097034 File Offset: 0x00095234
		private void OnItemStatus(GameObject goes, bool bOn, Control onItem, bool bPack = true)
		{
			if (bOn)
			{
				this.currentIndex = int.Parse(goes.name);
				this.m_ShopController.SetTipData(this.currentIndex, bPack);
				this.SetControl(onItem, goes, 260f, -4f, 0f);
				if (GameCursor.IsShow)
				{
					this.current = UIEventListener.Get(goes.gameObject);
					if (onItem == this.m_PackItemOn)
					{
						if (this.NowState != 1)
						{
							this.current = null;
							base.EnterState(1);
						}
					}
					else if (this.NowState != 2)
					{
						this.current = null;
						base.EnterState(2);
					}
				}
			}
			else
			{
				onItem.GameObject.SetActive(false);
				this.m_ExplainArea.SetActive(false);
			}
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0000B7AE File Offset: 0x000099AE
		private void InitView()
		{
			this.ResetItemStatus(this.m_PackItemList);
			this.ResetItemStatus(this.m_ShopItemList);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00097100 File Offset: 0x00095300
		private void ResetItemStatus(List<Control> itemList)
		{
			for (int i = 0; i < itemList.Count; i++)
			{
				itemList[i].GameObject.SetActive(false);
			}
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0000B7C8 File Offset: 0x000099C8
		private void SetMoneyView(int money)
		{
			this.m_PackValue.Text = money.ToString();
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0000B7DC File Offset: 0x000099DC
		private void ComplementAmount(int packCount, int shopCount)
		{
			this.ComplementAmountBase(packCount, this.m_PackItemList, this.m_GridPackItem, new Action(this.CreatPackItem));
			this.ComplementAmountBase(shopCount, this.m_ShopItemList, this.m_GridShopItem, new Action(this.CreatShopItem));
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00097138 File Offset: 0x00095338
		private void ComplementAmountBase(int count, List<Control> itemList, Control grid, Action creat)
		{
			if (count > itemList.Count)
			{
				int num = count - itemList.Count;
				for (int i = 0; i < num; i++)
				{
					creat.Invoke();
				}
			}
			grid.GetComponent<UIGrid>().Reposition();
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00097180 File Offset: 0x00095380
		private void SetView(ItemData itemData, int index)
		{
			if (itemData.amount == -1)
			{
				this.m_ShopItemList[index].GameObject.SetActive(true);
				this.m_ShopItemNameList[index].Text = itemData.name;
				this.m_ShopItemPriceList[index].Text = itemData.price.ToString();
			}
			else
			{
				this.m_PackItemList[index].GameObject.SetActive(true);
				this.m_PackItemNameList[index].Text = itemData.name;
				this.m_PackItemPriceList[index].Text = itemData.price.ToString();
				this.m_PackItemNewList[index].GameObject.SetActive(itemData.bNew);
				this.m_PackItemAmountList[index].Text = itemData.amount.ToString();
			}
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0009726C File Offset: 0x0009546C
		private void SetMouseActive(int showAmount, bool bReset)
		{
			int count = this.m_ShopController.GetCount(3);
			int count2 = this.m_ShopController.GetCount(4);
			this.CheckMouseWheel(count, showAmount, this.m_PackSlider, this.m_PackView, bReset);
			this.CheckMouseWheel(count2, showAmount, this.m_ShopSlider, this.m_ShopView, bReset);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x000972C0 File Offset: 0x000954C0
		private void CreatPackItem()
		{
			GameObject gameObject = Object.Instantiate(this.m_PackItem) as GameObject;
			gameObject.transform.parent = this.m_GridPackItem.GameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "PackItem";
			gameObject.transform.Traversal(delegate(Transform x)
			{
				string name = x.name;
				if (name != null)
				{
					if (UIShop.<>f__switch$map1D == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
						dictionary.Add("PackItem", 0);
						dictionary.Add("PackItemNew", 1);
						dictionary.Add("PackItemName", 2);
						dictionary.Add("PackItemPrice", 3);
						dictionary.Add("PackItemAmount", 4);
						UIShop.<>f__switch$map1D = dictionary;
					}
					int num;
					if (UIShop.<>f__switch$map1D.TryGetValue(name, ref num))
					{
						switch (num)
						{
						case 0:
						{
							x.OnClick += this.PackItemOnClick;
							x.OnKeySelect += this.PackItemOnKeySelect;
							x.OnHover += this.PackItemOnHover;
							x.GameObject.name = this.packItemIndex.ToString();
							UIEventListener listener = UIEventListener.Get(x.gameObject);
							base.SetInputButton(1, listener);
							this.m_PackItemList.Add(x);
							break;
						}
						case 1:
							this.m_PackItemNewList.Add(x);
							break;
						case 2:
							this.m_PackItemNameList.Add(x);
							break;
						case 3:
							this.m_PackItemPriceList.Add(x);
							break;
						case 4:
							this.m_PackItemAmountList.Add(x);
							break;
						}
					}
				}
			});
			this.packItemIndex++;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x00097358 File Offset: 0x00095558
		private void CreatShopItem()
		{
			GameObject gameObject = Object.Instantiate(this.m_ShopItem) as GameObject;
			gameObject.transform.parent = this.m_GridShopItem.GameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "ShopItem";
			gameObject.transform.Traversal(delegate(Transform x)
			{
				string name = x.name;
				if (name != null)
				{
					if (UIShop.<>f__switch$map1F == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
						dictionary.Add("ShopItem", 0);
						dictionary.Add("ShopItemName", 1);
						dictionary.Add("ShopItemPrice", 2);
						UIShop.<>f__switch$map1F = dictionary;
					}
					int num;
					if (UIShop.<>f__switch$map1F.TryGetValue(name, ref num))
					{
						switch (num)
						{
						case 0:
						{
							x.OnClick += this.ShopItemOnClick;
							x.OnKeySelect += this.ShopItemOnKeySelect;
							x.OnHover += this.ShopItemOnHover;
							x.GameObject.name = this.shopItemIndex.ToString();
							UIEventListener listener = UIEventListener.Get(x.gameObject);
							base.SetInputButton(2, listener);
							this.m_ShopItemList.Add(x);
							break;
						}
						case 1:
							this.m_ShopItemNameList.Add(x);
							break;
						case 2:
							this.m_ShopItemPriceList.Add(x);
							break;
						}
					}
				}
			});
			this.shopItemIndex++;
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x000973F0 File Offset: 0x000955F0
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

		// Token: 0x060011A6 RID: 4518 RVA: 0x000974AC File Offset: 0x000956AC
		private void Update()
		{
			if (Game.UI.Get<UIShop>() != null && this.m_Group.GameObject.activeSelf)
			{
				if (this.m_PackSlider.GameObject.activeSelf && this.packSlider)
				{
					this.SetMouseWheel(this.m_PackSlider);
				}
				if (this.m_ShopSlider.GameObject.activeSelf && this.shopSlider)
				{
					this.SetMouseWheel(this.m_ShopSlider);
				}
			}
		}

		// Token: 0x04001524 RID: 5412
		private CtrlShop m_ShopController = new CtrlShop();

		// Token: 0x04001525 RID: 5413
		private UIEventListener[] m_CurrentArray = new UIEventListener[Enum.GetNames(typeof(UIShop.eState)).Length];

		// Token: 0x04001526 RID: 5414
		private GameObject go_player;

		// Token: 0x04001527 RID: 5415
		private GameObject target;

		// Token: 0x04001528 RID: 5416
		private Control m_Group;

		// Token: 0x04001529 RID: 5417
		private Control m_ExitSprite;

		// Token: 0x0400152A RID: 5418
		private WgBackPackTip m_ExplainArea;

		// Token: 0x0400152B RID: 5419
		private List<Control> m_ItemEffectList = new List<Control>();

		// Token: 0x0400152C RID: 5420
		private Control m_PackItemOn;

		// Token: 0x0400152D RID: 5421
		private Control m_ShopItemOn;

		// Token: 0x0400152E RID: 5422
		private Control m_PackItemOnClick;

		// Token: 0x0400152F RID: 5423
		private Control m_ShopItemOnClick;

		// Token: 0x04001530 RID: 5424
		public GameObject m_PackItem;

		// Token: 0x04001531 RID: 5425
		private Control m_PackView;

		// Token: 0x04001532 RID: 5426
		private Control m_GridPackItem;

		// Token: 0x04001533 RID: 5427
		private List<Control> m_PackItemList = new List<Control>();

		// Token: 0x04001534 RID: 5428
		private List<Control> m_PackItemNewList = new List<Control>();

		// Token: 0x04001535 RID: 5429
		private List<Control> m_PackItemAmountList = new List<Control>();

		// Token: 0x04001536 RID: 5430
		private List<Control> m_PackItemNameList = new List<Control>();

		// Token: 0x04001537 RID: 5431
		private List<Control> m_PackItemPriceList = new List<Control>();

		// Token: 0x04001538 RID: 5432
		private Control m_PackSlider;

		// Token: 0x04001539 RID: 5433
		private Control m_PackValue;

		// Token: 0x0400153A RID: 5434
		public GameObject m_ShopItem;

		// Token: 0x0400153B RID: 5435
		private Control m_ShopView;

		// Token: 0x0400153C RID: 5436
		private Control m_GridShopItem;

		// Token: 0x0400153D RID: 5437
		private List<Control> m_ShopItemList = new List<Control>();

		// Token: 0x0400153E RID: 5438
		private List<Control> m_ShopItemNameList = new List<Control>();

		// Token: 0x0400153F RID: 5439
		private List<Control> m_ShopItemPriceList = new List<Control>();

		// Token: 0x04001540 RID: 5440
		private Control m_ShopSlider;

		// Token: 0x04001541 RID: 5441
		private Control m_PackViewSprite;

		// Token: 0x04001542 RID: 5442
		private Control m_ShopViewSprite;

		// Token: 0x04001543 RID: 5443
		private Control m_InputArea;

		// Token: 0x04001544 RID: 5444
		private Control m_InputAmount;

		// Token: 0x04001545 RID: 5445
		private Control m_Amount;

		// Token: 0x04001546 RID: 5446
		private Control m_Add;

		// Token: 0x04001547 RID: 5447
		private Control m_Less;

		// Token: 0x04001548 RID: 5448
		private Control m_Price;

		// Token: 0x04001549 RID: 5449
		private List<Control> m_InputOptionList = new List<Control>();

		// Token: 0x0400154A RID: 5450
		private bool packSlider;

		// Token: 0x0400154B RID: 5451
		private bool shopSlider;

		// Token: 0x0400154C RID: 5452
		private int stateIndex;

		// Token: 0x0400154D RID: 5453
		private int packItemIndex;

		// Token: 0x0400154E RID: 5454
		private int shopItemIndex;

		// Token: 0x0400154F RID: 5455
		private int optionIndex;

		// Token: 0x04001550 RID: 5456
		private int currentIndex;

		// Token: 0x04001551 RID: 5457
		private List<BackpackNewDataNode> m_TeamBackpackList = new List<BackpackNewDataNode>();

		// Token: 0x02000322 RID: 802
		private enum eState
		{
			// Token: 0x04001559 RID: 5465
			None,
			// Token: 0x0400155A RID: 5466
			enterPack,
			// Token: 0x0400155B RID: 5467
			enterShop,
			// Token: 0x0400155C RID: 5468
			enterSell,
			// Token: 0x0400155D RID: 5469
			enterBuy
		}
	}
}
