using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000323 RID: 803
	public class UIStart : UILayer
	{
		// Token: 0x060011AA RID: 4522 RVA: 0x000977F0 File Offset: 0x000959F0
		protected override void Awake()
		{
			base.Awake();
			if (!Game.layerDeleteList.Contains(this))
			{
				Game.layerDeleteList.Add(this);
			}
			GameGlobal.m_bUIStart = true;
			Application.LoadLevelAdditive("M0000_00");
			string.Concat(new string[]
			{
				Game.g_strDataPathToApplicationPath,
				"Mods/",
				GameGlobal.m_strVersion,
				"/Config/Image/"
			});
			Texture2D texture2D = this.mod_Load("ui_menu_01");
			if (texture2D != null)
			{
				base.transform.GetChild(0).transform.GetChild(7).GetComponent<UITexture>().mainTexture = texture2D;
			}
			base.transform.GetChild(0).transform.GetChild(7).gameObject.SetActive(true);
			this.mod_Intro = (Object.Instantiate(this.m_pBtnClose.UILabel) as UILabel);
			this.mod_Intro.transform.SetParent(base.transform.GetChild(0).transform);
			this.mod_Intro.trueTypeFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
			this.mod_Intro.fontSize = 120;
			this.mod_Intro.transform.localScale = new Vector3(2f, 2f, 2f);
			this.Show();
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0000B81C File Offset: 0x00009A1C
		public override void OnKeyUp(KeyControl.Key key)
		{
			if (this.m_bDLCPage && key == KeyControl.Key.Cancel)
			{
				this.OnGameDLCOut(null);
				return;
			}
			base.OnKeyUp(key);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0000B83F File Offset: 0x00009A3F
		private void OnDestroy()
		{
			this.Hide();
			GameGlobal.m_bUIStart = false;
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0000B84D File Offset: 0x00009A4D
		private void Start()
		{
			base.EnterState(1);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0000B856 File Offset: 0x00009A56
		public override void Show()
		{
			Game.g_InputManager.Push(this);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0000B863 File Offset: 0x00009A63
		public override void Hide()
		{
			base.Hide();
			Game.g_InputManager.Pop();
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00097938 File Offset: 0x00095B38
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIStart.<>f__switch$map20 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(10);
					dictionary.Add("GameStart", 0);
					dictionary.Add("GameLoad", 1);
					dictionary.Add("GameDLC", 2);
					dictionary.Add("GameUot", 3);
					dictionary.Add("GameSetting", 4);
					dictionary.Add("GameDLCStart", 5);
					dictionary.Add("GameDLCLoad", 6);
					dictionary.Add("GameDLCOut", 7);
					dictionary.Add("Mask", 8);
					dictionary.Add("Mask2", 9);
					UIStart.<>f__switch$map20 = dictionary;
				}
				int num;
				if (UIStart.<>f__switch$map20.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_pBtnStart = sender;
						this.m_pBtnStart.OnClick += this.StartYoungHero;
						this.m_pBtnStart.OnKeySelect += this.OnOptionKeySelect;
						base.SetInputButton(1, this.m_pBtnStart.Listener);
						break;
					case 1:
						this.m_pBtnLoad = sender;
						this.m_pBtnLoad.OnClick += this.OnGameLoadClick;
						this.m_pBtnLoad.OnKeySelect += this.OnOptionKeySelect;
						base.SetInputButton(1, this.m_pBtnLoad.Listener);
						break;
					case 2:
						this.m_pBtnDlc = sender;
						this.m_pBtnDlc.OnClick += this.OnGameDLCClick;
						this.m_pBtnDlc.OnKeySelect += this.OnOptionKeySelect;
						base.SetInputButton(1, this.m_pBtnDlc.Listener);
						break;
					case 3:
						this.m_pBtnClose = sender;
						this.m_pBtnClose.OnClick += this.OnGameUotClick;
						this.m_pBtnClose.OnKeySelect += this.OnOptionKeySelect;
						base.SetInputButton(1, this.m_pBtnClose.Listener);
						break;
					case 4:
						this.m_GameSetting = sender;
						this.m_GameSetting.OnClick += this.OnGameSettingClick;
						this.m_GameSetting.OnKeySelect += this.OnOptionKeySelect;
						base.SetInputButton(1, this.m_GameSetting.Listener);
						break;
					case 5:
						this.m_pBtnDlcStart = sender;
						this.m_pBtnDlcStart.OnClick += this.StartDLCYoungHero;
						this.m_pBtnDlcStart.OnKeySelect += this.OnOptionKeySelect;
						base.SetInputButton(1, this.m_pBtnDlcStart.Listener);
						break;
					case 6:
						this.m_pBtnDlcLoad = sender;
						this.m_pBtnDlcLoad.OnClick += this.OnDLCGameLoadClick;
						this.m_pBtnDlcLoad.OnKeySelect += this.OnOptionKeySelect;
						base.SetInputButton(1, this.m_pBtnDlcLoad.Listener);
						break;
					case 7:
						this.m_pBtnDlcOut = sender;
						this.m_pBtnDlcOut.OnClick += this.OnGameDLCOut;
						this.m_pBtnDlcOut.OnKeySelect += this.OnOptionKeySelect;
						base.SetInputButton(1, this.m_pBtnDlcOut.Listener);
						break;
					case 8:
						this.m_pMaskTexture01 = sender;
						break;
					case 9:
						this.m_pMaskTexture02 = sender;
						break;
					}
				}
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00002C2D File Offset: 0x00000E2D
		private bool CheckBeggerTalnet()
		{
			return false;
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00097CD4 File Offset: 0x00095ED4
		private void StartYoungHero(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (this.m_bClick)
			{
				return;
			}
			this.m_bClick = true;
			GameGlobal.m_strSaveVersion = GameGlobal.m_strVersion;
			GameGlobal.m_bDLCMode = false;
			TextDataManager.ReLoadAllTextData();
			BattleControl.instance.LoadText();
			if (YoungHeroTime.m_instance != null)
			{
				YoungHeroTime.m_instance.StartNewGame();
			}
			if (NPC.m_instance != null)
			{
				NPC.m_instance.StartNewGame(false);
			}
			if (TeamStatus.m_Instance != null)
			{
				TeamStatus.m_Instance.StartNewGame(false);
			}
			if (MissionStatus.m_instance != null)
			{
				MissionStatus.m_instance.Reset();
			}
			if (BackpackStatus.m_Instance != null)
			{
				BackpackStatus.m_Instance.StartNewGame();
			}
			if (GlobalVariableManager.Singleton != null)
			{
				GlobalVariableManager.Singleton.Data.Clear();
			}
			if (MapData.m_instance != null)
			{
				MapData.m_instance.m_NpcIsFoughtList.Clear();
			}
			if (Game.UI.Get<UIRumor>() != null)
			{
				Game.UI.Get<UIRumor>().ClearRumor();
			}
			if (Game.UI.Get<UIMainSelect>() != null)
			{
				Game.UI.Get<UISaveRumor>().ClearRumorRecord();
			}
			if (Game.UI.Get<UITalk>() != null)
			{
				Game.UI.Get<UITalk>().ClearTalkLogRecord();
			}
			if (Game.StoreData != null)
			{
				Game.StoreData.Clear();
			}
			GameGlobal.m_bAffterMode = false;
			GameGlobal.m_bHouseInside = false;
			GameGlobal.m_iBattleDifficulty = 0;
			GameGlobal.m_TransferPos = new Vector3(1048.2f, 220.07f, 308.2f);
			Game.LoadScene("M0050_01");
			this.m_bClick = false;
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00097E98 File Offset: 0x00096098
		private void StartDLCYoungHero(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (this.m_bClick)
			{
				return;
			}
			this.m_bClick = true;
			GameGlobal.m_strSaveVersion = GameGlobal.m_strVersion;
			GameGlobal.m_bDLCMode = true;
			GameGlobal.m_bDLCFirstStart = true;
			TextDataManager.ReLoadAllDLCTextData();
			BattleControl.instance.LoadDLCText();
			Save.m_Instance.LoadDLCTitleData();
			if (YoungHeroTime.m_instance != null)
			{
				YoungHeroTime.m_instance.StartNewGame();
			}
			if (NPC.m_instance != null)
			{
				NPC.m_instance.StartNewGame(true);
			}
			if (TeamStatus.m_Instance != null)
			{
				TeamStatus.m_Instance.StartNewGame(true);
			}
			if (MissionStatus.m_instance != null)
			{
				MissionStatus.m_instance.Reset();
			}
			if (BackpackStatus.m_Instance != null)
			{
				BackpackStatus.m_Instance.StartNewGame();
			}
			if (GlobalVariableManager.Singleton != null)
			{
				GlobalVariableManager.Singleton.Data.Clear();
			}
			if (MapData.m_instance != null)
			{
				MapData.m_instance.m_NpcIsFoughtList.Clear();
			}
			if (Game.UI.Get<UIRumor>() != null)
			{
				Game.UI.Get<UIRumor>().ClearRumor();
			}
			if (Game.UI.Get<UIMainSelect>() != null)
			{
				Game.UI.Get<UISaveRumor>().ClearRumorRecord();
			}
			if (Game.UI.Get<UITalk>() != null)
			{
				Game.UI.Get<UITalk>().ClearTalkLogRecord();
			}
			if (Game.StoreData != null)
			{
				Game.StoreData.Clear();
			}
			GameGlobal.m_bAffterMode = false;
			GameGlobal.m_bHouseInside = false;
			GameGlobal.m_iBattleDifficulty = 0;
			GameGlobal.m_TransferPos = new Vector3(1048.2f, 220.07f, 308.2f);
			Game.LoadScene("ReadyCombat");
			this.m_bClick = false;
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0000B876 File Offset: 0x00009A76
		private void OnGameSettingClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			Game.UI.Show<UISystemSetting>();
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0009806C File Offset: 0x0009626C
		private void BtnTeamOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			Game.RewardData.DoRewardID(500006, null);
			Game.RewardData.DoRewardID(500007, null);
			Game.RewardData.DoRewardID(500008, null);
			Game.RewardData.DoRewardID(500009, null);
			Game.RewardData.DoRewardID(500010, null);
			Game.RewardData.DoRewardID(500011, null);
			Game.RewardData.DoRewardID(500012, null);
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x000980F8 File Offset: 0x000962F8
		private void OnGameLoadClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (this.m_bClick)
			{
				return;
			}
			this.m_bClick = true;
			GameGlobal.m_bDLCMode = false;
			GameGlobal.m_bDLCFirstStart = false;
			TextDataManager.ReLoadAllTextData();
			BattleControl.instance.LoadText();
			Save.m_Instance.LoadTitleData();
			Game.UI.Get<UISaveAndLoad>().SelectSaveOrLoad(3);
			GameGlobal.m_bAffterMode = false;
			GameGlobal.m_bHouseInside = false;
			GameGlobal.m_iBattleDifficulty = 0;
			GameGlobal.m_TransferPos = new Vector3(1048.2f, 220.07f, 308.2f);
			this.m_bClick = false;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00098198 File Offset: 0x00096398
		private void OnDLCGameLoadClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (this.m_bClick)
			{
				return;
			}
			this.m_bClick = true;
			GameGlobal.m_bDLCMode = true;
			GameGlobal.m_bDLCFirstStart = false;
			TextDataManager.ReLoadAllDLCTextData();
			BattleControl.instance.LoadDLCText();
			Save.m_Instance.LoadDLCTitleData();
			Game.UI.Get<UISaveAndLoad>().SelectSaveOrLoad(3);
			GameGlobal.m_bAffterMode = false;
			GameGlobal.m_bHouseInside = false;
			GameGlobal.m_iBattleDifficulty = 0;
			GameGlobal.m_TransferPos = new Vector3(1048.2f, 220.07f, 308.2f);
			this.m_bClick = false;
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0009822C File Offset: 0x0009642C
		private void OnGameDLCClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_pBtnStart.GameObject.SetActive(false);
			this.m_pBtnLoad.GameObject.SetActive(false);
			this.m_pBtnDlc.GameObject.SetActive(false);
			this.m_pBtnClose.GameObject.SetActive(false);
			this.m_GameSetting.GameObject.SetActive(false);
			this.m_pBtnDlcStart.GameObject.SetActive(true);
			this.m_pBtnDlcLoad.GameObject.SetActive(true);
			this.m_pBtnDlcOut.GameObject.SetActive(true);
			if (!GameCursor.IsShow)
			{
				base.SetCurrent(this.m_pBtnDlcStart.Listener, true);
			}
			else
			{
				base.SetCurrent(this.m_pBtnDlcStart.Listener, false);
			}
			this.m_bDLCPage = true;
			this.m_pMaskTexture01.GameObject.SetActive(false);
			this.m_pMaskTexture02.GameObject.SetActive(true);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0009832C File Offset: 0x0009652C
		private void OnGameDLCOut(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_pBtnStart.GameObject.SetActive(true);
			this.m_pBtnLoad.GameObject.SetActive(true);
			this.m_pBtnDlc.GameObject.SetActive(true);
			this.m_pBtnClose.GameObject.SetActive(true);
			this.m_GameSetting.GameObject.SetActive(true);
			if (!GameCursor.IsShow)
			{
				base.SetCurrent(this.m_pBtnDlc.Listener, true);
			}
			else
			{
				base.SetCurrent(this.m_pBtnDlc.Listener, false);
			}
			this.m_pBtnDlcStart.GameObject.SetActive(false);
			this.m_pBtnDlcLoad.GameObject.SetActive(false);
			this.m_pBtnDlcOut.GameObject.SetActive(false);
			this.m_bDLCPage = false;
			this.m_pMaskTexture01.GameObject.SetActive(true);
			this.m_pMaskTexture02.GameObject.SetActive(false);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0000B88E File Offset: 0x00009A8E
		private void OnGameUotClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			Process.GetCurrentProcess().Kill();
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0000B8A6 File Offset: 0x00009AA6
		private void OnOptionKeySelect(GameObject go, bool isSelect)
		{
			if (isSelect)
			{
				this.ShowKeySelect(go, new Vector3(-180f, 0f, 0f), KeySelect.eSelectDir.Left, 40, 40);
			}
			else
			{
				this.HideKeySelect();
			}
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0009842C File Offset: 0x0009662C
		private void Update()
		{
			this.mod_Intro.text = "义薄云天工作组制作 v0.73版";
			this.mod_Intro.color = Color.blue;
			this.mod_Intro.transform.localPosition = new Vector2(this.m_pBtnClose.GameObject.transform.localPosition.x - 50f, this.m_pBtnClose.GameObject.transform.localPosition.z - 400f);
			for (int i = 0; i < Game.UI.Get<UIMainSelect>().mod_UITextureList.Count; i++)
			{
				Game.UI.Get<UIMainSelect>().mod_UITextureList[i].gameObject.SetActive(false);
				Game.UI.Get<UIMainSelect>().mod_LabelList[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0000264F File Offset: 0x0000084F
		private void StartEndTest()
		{
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0000264F File Offset: 0x0000084F
		private void StartBattleTest()
		{
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x00098514 File Offset: 0x00096714
		public Texture2D mod_Load(string fileName)
		{
			string text = string.Concat(new string[]
			{
				Game.g_strDataPathToApplicationPath,
				"Mods/",
				GameGlobal.m_strVersion,
				"/Config/Image/",
				fileName,
				".png"
			});
			Texture2D texture2D = new Texture2D(2, 2, 5, false, false);
			texture2D.LoadImage(File.ReadAllBytes(text));
			return texture2D;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0000B8D9 File Offset: 0x00009AD9
		public void mod_Test()
		{
			Process.GetCurrentProcess().Kill();
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0000B8E5 File Offset: 0x00009AE5
		public void mod_OnTest(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.mod_Test();
		}

		// Token: 0x0400155E RID: 5470
		private Control m_pBtnStart;

		// Token: 0x0400155F RID: 5471
		private Control m_pBtnLoad;

		// Token: 0x04001560 RID: 5472
		private Control m_pBtnDlc;

		// Token: 0x04001561 RID: 5473
		private Control m_pBtnClose;

		// Token: 0x04001562 RID: 5474
		private Control m_GameSetting;

		// Token: 0x04001563 RID: 5475
		private Control m_pBtnDlcStart;

		// Token: 0x04001564 RID: 5476
		private Control m_pBtnDlcLoad;

		// Token: 0x04001565 RID: 5477
		private Control m_pBtnDlcOut;

		// Token: 0x04001566 RID: 5478
		private Control m_pMaskTexture01;

		// Token: 0x04001567 RID: 5479
		private Control m_pMaskTexture02;

		// Token: 0x04001568 RID: 5480
		private AudioSource m_BtnHover;

		// Token: 0x04001569 RID: 5481
		private AudioSource m_BtnOnClick;

		// Token: 0x0400156A RID: 5482
		private bool m_bDLCPage;

		// Token: 0x0400156B RID: 5483
		private bool m_bClick;

		// Token: 0x0400156D RID: 5485
		public UILabel mod_Intro;

		// Token: 0x0400156E RID: 5486
		public Control mod_BtnTest;

		// Token: 0x02000324 RID: 804
		private enum eState
		{
			// Token: 0x04001570 RID: 5488
			None,
			// Token: 0x04001571 RID: 5489
			Start
		}
	}
}
