using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000310 RID: 784
	public class UIMainSelect : UILayer
	{
		// Token: 0x060010E5 RID: 4325 RVA: 0x00091DEC File Offset: 0x0008FFEC
		protected override void Awake()
		{
			base.Awake();
			this.mod_ButtonList.Clear();
			this.mod_LabelList.Clear();
			this.mod_UITextureList.Clear();
			List<string> list = new List<string>();
			list.Add("Difficulty");
			list.Add("NewBattle");
			list.Add("BossBattle");
			list.Add("RandomBattle");
			list.Add("Shop");
			list.Add("Minigame");
			List<string> list2 = list;
			List<string> list3 = new List<string>();
			list3.Add("难度设置");
			list3.Add("新手战场");
			list3.Add("Boss战场");
			list3.Add("随机战场");
			list3.Add("打开商店");
			list3.Add("小游戏");
			List<string> list4 = list3;
			List<string> list5 = new List<string>();
			list5.Add("mod_SetDifficulty");
			list5.Add("mod_NewBattle");
			list5.Add("mod_BossBattle");
			list5.Add("mod_RandomBattle");
			list5.Add("mod_OpenShop");
			list5.Add("mod_Minigame");
			List<string> list6 = list5;
			for (int i = 0; i < list2.Count; i++)
			{
				GameObject gameObject = new GameObject("image");
				gameObject.transform.SetParent(base.transform);
				UITexture uitexture = gameObject.AddComponent<UITexture>();
				this.mod_UITextureList.Add(uitexture);
				UIButton uibutton = gameObject.AddComponent<UIButton>();
				this.mod_ButtonList.Add(uibutton);
				uibutton.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
				Texture2D mainTexture = Game.mod_Load(list2[i]);
				uitexture.GetComponent<UITexture>().mainTexture = mainTexture;
				uitexture.transform.localPosition = new Vector2(this.m_MoneyValue.GameObject.transform.localPosition.x - 150f * (float)Screen.width / 1920f + (float)(125 * i), this.m_MoneyValue.GameObject.transform.localPosition.y + 450f * (float)Screen.height / 1080f);
				uitexture.gameObject.AddComponent<BoxCollider>().size = new Vector3(100f, 100f, 0f);
				uitexture.autoResizeBoxCollider = true;
				EventDelegate eventDelegate = new EventDelegate(this, list6[i]);
				uibutton.onClick.Add(eventDelegate);
				GameObject gameObject2 = new GameObject("text");
				gameObject2.transform.SetParent(base.transform);
				UILabel uilabel = gameObject2.AddComponent<UILabel>();
				this.mod_LabelList.Add(uilabel);
				uilabel.text = list4[i];
				uilabel.fontSize = 20;
				uilabel.font = this.m_MoneyValue.UILabel.font;
				uilabel.transform.localPosition = new Vector2(this.m_MoneyValue.GameObject.transform.localPosition.x - 150f * (float)Screen.width / 1920f + (float)(125 * i), this.m_MoneyValue.GameObject.transform.localPosition.y + 400f * (float)Screen.height / 1080f);
			}
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00092124 File Offset: 0x00090324
		public override void OnKeyUp(KeyControl.Key key)
		{
			if (this.NowState != 2)
			{
				base.OnKeyUp(key);
				if (key == KeyControl.Key.Menu && this.NowState != 1)
				{
					base.EnterState(0);
				}
				return;
			}
			switch (key)
			{
			case KeyControl.Key.Up:
			case KeyControl.Key.Down:
				this.SelectNextButton(key);
				return;
			case KeyControl.Key.Left:
			case KeyControl.Key.Right:
				return;
			case KeyControl.Key.OK:
				base.OnCurrentClick();
				return;
			case KeyControl.Key.Cancel:
				base.EnterState(1);
				return;
			default:
				return;
			}
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00092190 File Offset: 0x00090390
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIMainSelect.<>f__switch$map10 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(10);
					dictionary.Add("Group", 0);
					dictionary.Add("Option", 1);
					dictionary.Add("OptionOn", 2);
					dictionary.Add("OptionOnClick", 3);
					dictionary.Add("MoneyValue", 4);
					dictionary.Add("MissionNewAmount", 5);
					dictionary.Add("PackNewAmount", 6);
					dictionary.Add("System", 7);
					dictionary.Add("SysOption", 8);
					dictionary.Add("SysOptionOn", 9);
					UIMainSelect.<>f__switch$map10 = dictionary;
				}
				int num;
				if (UIMainSelect.<>f__switch$map10.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
					{
						Control control = sender;
						control.OnHover += this.OptionOnHover;
						control.OnClick += this.OptionOnClick;
						control.OnKeySelect += this.OptionOnKeySelect;
						base.SetInputButton(1, control.Listener);
						control.GameObject.name = this.oIndex.ToString();
						this.oIndex++;
						this.m_OptionList.Add(control);
						break;
					}
					case 2:
						this.m_OptionOn = sender;
						break;
					case 3:
						this.m_OptionOnClick = sender;
						break;
					case 4:
						this.m_MoneyValue = sender;
						break;
					case 5:
						this.m_MissionNewAmount = sender;
						break;
					case 6:
						this.m_PackNewAmount = sender;
						break;
					case 7:
						this.m_System = sender;
						break;
					case 8:
					{
						Control control = sender;
						control.OnHover += this.SysOptionOnHover;
						control.OnClick += this.SysOptionOnClick;
						control.OnKeySelect += this.SysOptionOnKeySelect;
						base.SetInputButton(2, control.Listener);
						control.GameObject.name = this.sIndex.ToString();
						this.sIndex++;
						this.m_SysOptionList.Add(control);
						break;
					}
					case 9:
						this.m_SysOptionOn = sender;
						break;
					}
				}
			}
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00092408 File Offset: 0x00090608
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			UIAudioDataManager.Singleton.PlayUIAudio(UIAudioData.eUIAudio.UpAndDown);
			this.m_bShow = true;
			GameGlobal.m_bCFormOpen = true;
			this.m_Group.GameObject.SetActive(true);
			Game.g_InputManager.Push(this);
			this.UpdateView();
			base.EnterState(1);
			for (int i = 0; i < this.mod_UITextureList.Count; i++)
			{
				this.mod_UITextureList[i].gameObject.SetActive(true);
				this.mod_LabelList[i].gameObject.SetActive(true);
			}
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x000924A4 File Offset: 0x000906A4
		public override void Hide()
		{
			for (int i = 0; i < Enum.GetNames(typeof(UIMainSelect.eState)).Length; i++)
			{
				this.m_CurrentArray[i] = null;
			}
			if (!this.m_bShow)
			{
				return;
			}
			UIAudioDataManager.Singleton.PlayUIAudio(UIAudioData.eUIAudio.UpAndDown);
			this.m_bShow = false;
			GameGlobal.m_bCFormOpen = false;
			this.m_Group.GameObject.SetActive(false);
			Game.g_InputManager.Pop();
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0000B06F File Offset: 0x0000926F
		protected void OptionOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.OnOptionStatus(go, bHover, this.m_OptionOn);
			}
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0000B089 File Offset: 0x00009289
		private void OptionOnKeySelect(GameObject go, bool bSelect)
		{
			this.ShowKeySelect(go, new Vector3(-100f, 0f, 0f), KeySelect.eSelectDir.Left, 32, 32);
			this.OnOptionStatus(go, bSelect, this.m_OptionOn);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00092520 File Offset: 0x00090720
		protected void OptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_OptionOn.GameObject.SetActive(false);
			this.SetControl(this.m_OptionOnClick, go, 0f, 0f, 0f);
			int num = int.Parse(go.name);
			if (num != 5)
			{
				base.EnterState(0);
			}
			switch (num)
			{
			case 0:
				Game.UI.Show<UICharacter>();
				break;
			case 1:
				Game.UI.Show<UITeam>();
				break;
			case 2:
				Game.UI.Show<UIBackpack>();
				break;
			case 3:
				Game.UI.Show<UIMission>();
				break;
			case 4:
				Game.UI.Show<UISaveRumor>();
				break;
			case 5:
				base.EnterState(2);
				break;
			}
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0000B0B9 File Offset: 0x000092B9
		private void InitMain()
		{
			this.m_System.GameObject.SetActive(false);
			this.m_OptionOnClick.GameObject.SetActive(false);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0000B0DD File Offset: 0x000092DD
		private void SysOptionOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.OnOptionStatus(go, bHover, this.m_SysOptionOn);
			}
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0000B0F7 File Offset: 0x000092F7
		private void SysOptionOnKeySelect(GameObject go, bool bSelect)
		{
			this.ShowKeySelect(go, new Vector3(-300f, 0f, 0f), KeySelect.eSelectDir.Left, 32, 32);
			this.OnOptionStatus(go, bSelect, this.m_SysOptionOn);
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x000925FC File Offset: 0x000907FC
		private void SysOptionOnClick(GameObject go)
		{
			this.m_SysOptionOn.GameObject.SetActive(false);
			int num = int.Parse(go.name);
			if (num != 0)
			{
				base.EnterState(0);
			}
			switch (num)
			{
			case 0:
				base.EnterState(1);
				break;
			case 1:
			case 3:
				Game.UI.Get<UISaveAndLoad>().SelectSaveOrLoad(num);
				break;
			case 2:
				Game.UI.Show<UISystemSetting>();
				break;
			case 4:
				Game.UI.Get<UILoad>().LoadStage("GameStart");
				break;
			}
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0000B127 File Offset: 0x00009327
		private void OnOptionStatus(GameObject goes, bool bOn, Control sprite)
		{
			if (bOn)
			{
				this.SetControl(sprite, goes, 0f, 0f, 0f);
			}
			else
			{
				sprite.GameObject.SetActive(false);
			}
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x000926A0 File Offset: 0x000908A0
		public void UpdateView()
		{
			this.money = BackpackStatus.m_Instance.GetMoney();
			this.ChangeShowMoney(this.money);
			int newQuestAmount = MissionStatus.m_instance.GetNewQuestAmount();
			this.SetMissionAmount(newQuestAmount);
			this.SetBackpackAmount();
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0000B157 File Offset: 0x00009357
		public void ChangeShowMoney(int value)
		{
			this.m_MoneyValue.Text = value.ToString();
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0000B16B File Offset: 0x0000936B
		public void SetMissionAmount(int amount)
		{
			this.SetAmount(this.m_MissionNewAmount, amount);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x000926E4 File Offset: 0x000908E4
		public void SetBackpackAmount()
		{
			int amount = BackpackStatus.m_Instance.CheckPackNewItemAmount();
			this.SetAmount(this.m_PackNewAmount, amount);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0000B17A File Offset: 0x0000937A
		private void SetAmount(Control label, int amount)
		{
			if (amount > 0)
			{
				label.GameObject.SetActive(true);
				label.Text = amount.ToString();
			}
			else
			{
				label.GameObject.SetActive(false);
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0009270C File Offset: 0x0009090C
		protected override void OnStateEnter(int state)
		{
			switch (state)
			{
			case 0:
				this.Hide();
				break;
			case 1:
				this.InitMain();
				break;
			case 2:
				this.m_System.GameObject.SetActive(true);
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
						if (!this.current.gameObject.collider.enabled || !this.current.gameObject.activeSelf)
						{
							return;
						}
						base.SetCurrent(this.current, true);
					}
				}
				else
				{
					this.current = this.m_CurrentArray[this.NowState];
					base.SetCurrent(this.current, true);
				}
			}
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0009282C File Offset: 0x00090A2C
		protected override void OnStateExit(int state)
		{
			if (state == 1)
			{
				this.m_CurrentArray[state] = this.current;
			}
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0000B1AD File Offset: 0x000093AD
		public override void OnMouseControl(bool bCtrl)
		{
			base.OnMouseControl(bCtrl);
			this.HideKeySelect();
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0000B1BC File Offset: 0x000093BC
		private void Update()
		{
			if (Input.GetMouseButtonUp(1))
			{
				if (this.NowState == 2)
				{
					base.EnterState(1);
					return;
				}
				base.EnterState(0);
			}
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0000B1DE File Offset: 0x000093DE
		public void mod_SetDifficulty()
		{
			Game.UI.Get<UITalk>().SetTalkData("mod_SetDifficulty");
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0000B1F4 File Offset: 0x000093F4
		public void mod_NewBattle()
		{
			Game.UI.Get<UITalk>().SetTalkData("mod_NewBattle");
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0000B20A File Offset: 0x0000940A
		public void mod_BossBattle()
		{
			Game.UI.Get<UITalk>().SetTalkData("mod_BossBattle");
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0000B220 File Offset: 0x00009420
		public void mod_RandomBattle()
		{
			Game.UI.Get<UITalk>().SetTalkData("mod_RandomBattle");
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0000B236 File Offset: 0x00009436
		public void mod_OpenShop()
		{
			Game.UI.Get<UITalk>().SetTalkData("mod_OpenShop");
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0000B24C File Offset: 0x0000944C
		public void mod_Minigame()
		{
			Game.UI.Get<UITalk>().SetTalkData("mod_Minigame");
		}

		// Token: 0x04001467 RID: 5223
		private Control m_Group;

		// Token: 0x04001468 RID: 5224
		private Control m_MoneyValue;

		// Token: 0x04001469 RID: 5225
		private List<Control> m_OptionList = new List<Control>();

		// Token: 0x0400146A RID: 5226
		private Control m_OptionOn;

		// Token: 0x0400146B RID: 5227
		private Control m_OptionOnClick;

		// Token: 0x0400146C RID: 5228
		private Control m_MissionNewAmount;

		// Token: 0x0400146D RID: 5229
		private Control m_PackNewAmount;

		// Token: 0x0400146E RID: 5230
		private Control m_System;

		// Token: 0x0400146F RID: 5231
		private List<Control> m_SysOptionList = new List<Control>();

		// Token: 0x04001470 RID: 5232
		private Control m_SysOptionOn;

		// Token: 0x04001471 RID: 5233
		private UIEventListener[] m_CurrentArray = new UIEventListener[Enum.GetNames(typeof(UIMainSelect.eState)).Length];

		// Token: 0x04001472 RID: 5234
		private List<Rumor> rumorStrList = new List<Rumor>();

		// Token: 0x04001473 RID: 5235
		private int oIndex;

		// Token: 0x04001474 RID: 5236
		private int sIndex;

		// Token: 0x04001475 RID: 5237
		private int money;

		// Token: 0x04001476 RID: 5238
		private int strMaxCount = 20;

		// Token: 0x04001477 RID: 5239
		private int showMaxCount = 7;

		// Token: 0x04001478 RID: 5240
		private bool setCurrent;

		// Token: 0x0400147A RID: 5242
		public UILabel mod_Label;

		// Token: 0x0400147B RID: 5243
		public UITexture mod_UITexture;

		// Token: 0x0400147C RID: 5244
		public GameObject mod_GameObject;

		// Token: 0x0400147D RID: 5245
		public List<UILabel> mod_LabelList;

		// Token: 0x0400147E RID: 5246
		public List<UITexture> mod_UITextureList;

		// Token: 0x0400147F RID: 5247
		public List<UIButton> mod_ButtonList;

		// Token: 0x02000311 RID: 785
		private enum Option
		{
			// Token: 0x04001481 RID: 5249
			character,
			// Token: 0x04001482 RID: 5250
			team,
			// Token: 0x04001483 RID: 5251
			backpack,
			// Token: 0x04001484 RID: 5252
			mission,
			// Token: 0x04001485 RID: 5253
			rumor,
			// Token: 0x04001486 RID: 5254
			system
		}

		// Token: 0x02000312 RID: 786
		public enum eState
		{
			// Token: 0x04001488 RID: 5256
			none,
			// Token: 0x04001489 RID: 5257
			Main,
			// Token: 0x0400148A RID: 5258
			System
		}
	}
}
