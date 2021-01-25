using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200036A RID: 874
	public class UIReadyCombat : UILayer
	{
		// Token: 0x060013FB RID: 5115 RVA: 0x000AC4D8 File Offset: 0x000AA6D8
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIReadyCombat.<>f__switch$map52 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(18);
					dictionary.Add("TurnLabel", 0);
					dictionary.Add("TurnValue", 1);
					dictionary.Add("TipText", 2);
					dictionary.Add("Prestige", 3);
					dictionary.Add("PrestigeTip", 4);
					dictionary.Add("Gold", 5);
					dictionary.Add("Hero", 6);
					dictionary.Add("InfoHero", 7);
					dictionary.Add("MissionOrig", 8);
					dictionary.Add("Mission", 9);
					dictionary.Add("Close", 10);
					dictionary.Add("MissionInfo", 11);
					dictionary.Add("MissionTitle", 12);
					dictionary.Add("MissionName", 13);
					dictionary.Add("MissionDesc", 14);
					dictionary.Add("GoButton", 15);
					dictionary.Add("ExpValue", 16);
					dictionary.Add("Item", 17);
					UIReadyCombat.<>f__switch$map52 = dictionary;
				}
				int num;
				if (UIReadyCombat.<>f__switch$map52.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_pTurnLabel = sender;
						break;
					case 1:
						this.m_pTurnValue = sender;
						break;
					case 2:
						this.m_pTipText = sender;
						break;
					case 3:
					{
						this.m_pPrestige = sender;
						Control control = sender;
						control.OnHover += this.PrestigeOnHover;
						break;
					}
					case 4:
						this.m_pPrestigeTip = sender;
						break;
					case 5:
						this.m_pGold = sender;
						break;
					case 6:
					{
						RCHeroFace component = sender.gameObject.GetComponent<RCHeroFace>();
						this.heroList.Add(component);
						Control control = sender;
						control.OnHover += this.HeroFaceOnHover;
						control.OnClick += this.HeroFaceOnClick;
						break;
					}
					case 7:
					{
						RCHeroFace component = sender.gameObject.GetComponent<RCHeroFace>();
						this.rewardHeroList.Add(component);
						break;
					}
					case 8:
						this.m_pMissionOrig = sender;
						break;
					case 9:
					{
						MissionPoint component2 = sender.gameObject.GetComponent<MissionPoint>();
						this.missionPointList.Add(component2);
						Control control = sender;
						control.OnHover += this.MissionPointOnHover;
						control.OnClick += this.MissionPointOnClick;
						break;
					}
					case 10:
					{
						this.m_pClose = sender;
						Control control = sender;
						control.OnHover += this.CloseOnHover;
						control.OnClick += this.CloseOnClick;
						break;
					}
					case 11:
						this.m_pMissionInfo = sender;
						break;
					case 12:
						this.m_pMissionTitle = sender;
						break;
					case 13:
						this.m_pMissionName = sender;
						break;
					case 14:
						this.m_pMissionDesc = sender;
						break;
					case 15:
					{
						this.m_pGoButton = sender;
						Control control = sender;
						control.OnHover += this.GoButtonOnHover;
						control.OnClick += this.GoButtonOnClick;
						break;
					}
					case 16:
						this.m_pExpValue = sender;
						break;
					case 17:
					{
						Control control = sender;
						this.itemList.Add(control);
						break;
					}
					}
				}
			}
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x000AC88C File Offset: 0x000AAA8C
		protected override void AssignWidget(Widget sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIReadyCombat.<>f__switch$map53 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
					dictionary.Add("GameMenu", 0);
					UIReadyCombat.<>f__switch$map53 = dictionary;
				}
				int num;
				if (UIReadyCombat.<>f__switch$map53.TryGetValue(name, ref num))
				{
					if (num == 0)
					{
						this.m_GameMenu = (sender as WgCombatMenu);
					}
				}
			}
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0000CDED File Offset: 0x0000AFED
		protected override void Awake()
		{
			base.Awake();
			if (!Game.layerDeleteList.Contains(this))
			{
				Game.layerDeleteList.Add(this);
			}
			GameGlobal.m_bUIReadyCombat = true;
			this.Show();
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0000CE1C File Offset: 0x0000B01C
		private void OnDestroy()
		{
			this.Hide();
			GameGlobal.m_bUIReadyCombat = false;
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x000AC8F8 File Offset: 0x000AAAF8
		public override void Show()
		{
			Game.g_InputManager.Push(this);
			this.bShowButton = true;
			base.EnterState(1);
			this.UpdateData();
			if (GameGlobal.m_bDLCFirstStart)
			{
				GameGlobal.m_bDLCFirstStart = false;
				Game.UI.Get<UITalk>().SetTalkData(GameGlobal.m_strDLCFirstTalk);
			}
			if (Game.Variable["DLC_continue"] >= 1)
			{
				Game.UI.Get<UITalk>().SetTalkData("111000001");
			}
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0000B863 File Offset: 0x00009A63
		public override void Hide()
		{
			base.Hide();
			Game.g_InputManager.Pop();
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x000AC974 File Offset: 0x000AAB74
		public void HideButton()
		{
			this.bShowButton = false;
			this.m_GameMenu.Obj.SetActive(false);
			for (int i = 0; i < this.missionPointList.Count; i++)
			{
				this.missionPointList[i].Select(false);
				this.missionPointList[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x000AC9E0 File Offset: 0x000AABE0
		public void ShowButton()
		{
			this.bShowButton = true;
			this.m_GameMenu.Obj.SetActive(true);
			for (int i = 0; i < this.missionPointList.Count; i++)
			{
				this.missionPointList[i].gameObject.SetActive(true);
			}
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x000ACA38 File Offset: 0x000AAC38
		private void GenerateMisstionPoint(int iCount)
		{
			for (int i = 0; i < iCount; i++)
			{
				GameObject gameObject = Object.Instantiate(this.m_pMissionOrig.GameObject) as GameObject;
				gameObject.transform.parent = this.m_pMissionOrig.GameObject.transform.parent;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localEulerAngles = Vector3.zero;
				MissionPoint component = gameObject.GetComponent<MissionPoint>();
				component.iIndex = this.missionPointList.Count;
				gameObject.name = "Mission";
				this.AssignControl(gameObject.transform);
			}
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x000ACADC File Offset: 0x000AACDC
		public void UpdateGold()
		{
			this.m_pGold.Text = BackpackStatus.m_Instance.GetMoney().ToString();
			this.m_pPrestige.Text = TeamStatus.m_Instance.GetPrestigePoints().ToString();
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x000ACB24 File Offset: 0x000AAD24
		public void UpdateData()
		{
			this.m_pGold.Text = BackpackStatus.m_Instance.GetMoney().ToString();
			this.m_pPrestige.Text = TeamStatus.m_Instance.GetPrestigePoints().ToString();
			List<CharacterData> teamMemberList = TeamStatus.m_Instance.GetTeamMemberList();
			for (int i = 0; i < this.heroList.Count; i++)
			{
				if (this.heroList[i].iIndex < teamMemberList.Count)
				{
					this.heroList[i].UpdateHeroFace(teamMemberList[this.heroList[i].iIndex]);
				}
				else
				{
					this.heroList[i].Hide();
				}
			}
			foreach (MissionPoint missionPoint in this.missionPointList)
			{
				missionPoint.Hide();
			}
			List<LevelTurnNode> nowLevelList = TeamStatus.m_Instance.GetNowLevelList();
			if (nowLevelList.Count > this.missionPointList.Count)
			{
				this.GenerateMisstionPoint(nowLevelList.Count - this.missionPointList.Count);
			}
			for (int j = 0; j < this.missionPointList.Count; j++)
			{
				if (this.missionPointList[j].iIndex < nowLevelList.Count)
				{
					int iLevelID = nowLevelList[this.missionPointList[j].iIndex].iLevelID;
					MissionLevelNode missionLevelNode = Game.MissionLeveData.GetMissionLevelNode(iLevelID);
					if (missionLevelNode != null)
					{
						MissionPositionNode missionPositionNode = Game.MissionPositionData.GetMissionPositionNode(missionLevelNode.iMissionPosition);
						if (missionPositionNode != null)
						{
							this.missionPointList[j].SetMissionNode(missionPositionNode, missionLevelNode.iType);
							this.missionPointList[j].Select(false);
						}
					}
				}
			}
			this.m_pMissionInfo.GameObject.SetActive(false);
			int num = Random.Range(0, this.strBGMusicArray.Length);
			Game.PlayBGMusicMapPath(this.strBGMusicArray[num]);
			this.m_GameMenu.ResetShopAndInfo();
			if (!this.bShowButton)
			{
				this.HideButton();
			}
			if (TeamStatus.m_Instance.m_TeamMessageList.Count > 0)
			{
				base.StartCoroutine(this.ShowTeamMessage());
			}
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0000CE2A File Offset: 0x0000B02A
		public void ResetShopAndInfo()
		{
			if (this.m_GameMenu != null)
			{
				this.m_GameMenu.ResetShopAndInfo();
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x000ACDAC File Offset: 0x000AAFAC
		private IEnumerator ShowTeamMessage()
		{
			yield return null;
			this.m_pTipText.Text = TeamStatus.m_Instance.m_TeamMessageList[0];
			TeamStatus.m_Instance.m_TeamMessageList.RemoveAt(0);
			yield return new WaitForSeconds(5f);
			if (TeamStatus.m_Instance.m_TeamMessageList.Count > 0)
			{
				base.StartCoroutine(this.ShowTeamMessage());
			}
			else
			{
				this.m_pTipText.Text = string.Empty;
			}
			yield break;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x000ACDC8 File Offset: 0x000AAFC8
		private void PrestigeOnHover(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.m_pPrestigeTip.GameObject.SetActive(true);
				this.m_pPrestigeTip.GameObject.GetComponentInChildren<UILabel>().text = TeamStatus.m_Instance.GetPrestigeString();
			}
			else
			{
				this.m_pPrestigeTip.GameObject.SetActive(false);
			}
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x000ACE24 File Offset: 0x000AB024
		private void HeroFaceOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				RCHeroFace component = go.GetComponent<RCHeroFace>();
				component.SetHover(bHover);
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x000ACE4C File Offset: 0x000AB04C
		private void HeroFaceOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (GameGlobal.m_bPlayerTalk)
			{
				return;
			}
			RCHeroFace component = go.GetComponent<RCHeroFace>();
			component.SetHover(false);
			component.OpenDLCCharacterStatus();
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x000ACE84 File Offset: 0x000AB084
		private void MissionPointOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				MissionPoint component = go.GetComponent<MissionPoint>();
				component.SetHover(bHover);
			}
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x000ACEAC File Offset: 0x000AB0AC
		private void MissionPointOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (GameGlobal.m_bPlayerTalk)
			{
				return;
			}
			MissionPoint component = go.GetComponent<MissionPoint>();
			component.SetHover(false);
			if (this.oldMissionPoint != null)
			{
				this.oldMissionPoint.Select(false);
			}
			component.Select(true);
			this.oldMissionPoint = component;
			List<LevelTurnNode> nowLevelList = TeamStatus.m_Instance.GetNowLevelList();
			if (component.iIndex < nowLevelList.Count)
			{
				int iLevelID = nowLevelList[component.iIndex].iLevelID;
				MissionLevelNode missionLevelNode = Game.MissionLeveData.GetMissionLevelNode(iLevelID);
				if (missionLevelNode != null)
				{
					this.UpdateMissionInfo(missionLevelNode);
				}
			}
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x000ACF50 File Offset: 0x000AB150
		private void UpdateMissionInfo(MissionLevelNode mln)
		{
			this.nowMissionLevelNode = mln;
			this.m_pMissionInfo.GameObject.SetActive(true);
			this.m_pMissionTitle.Text = Game.StringTable.GetString(this.MissionTitleArray[mln.iType]);
			this.m_pMissionName.Text = mln.strName;
			this.m_pMissionDesc.Text = mln.strDesc;
			foreach (RCHeroFace rcheroFace in this.rewardHeroList)
			{
				rcheroFace.ShowBackOnly();
			}
			for (int i = 0; i < this.rewardHeroList.Count; i++)
			{
				if (this.rewardHeroList[i].iIndex < mln.m_ShaqList.Count)
				{
					int iId = mln.m_ShaqList[this.rewardHeroList[i].iIndex];
					CharacterData characterData = NPC.m_instance.GetCharacterData(iId);
					if (characterData != null)
					{
						this.rewardHeroList[i].UpdateHeroFace(characterData);
					}
				}
			}
			foreach (Control control in this.itemList)
			{
				control.GameObject.SetActive(false);
			}
			for (int j = 0; j < this.itemList.Count; j++)
			{
				ImageData component = this.itemList[j].GameObject.GetComponent<ImageData>();
				if (!(component == null))
				{
					this.itemList[j].GameObject.SetActive(true);
					if (component.m_iIndex == 0)
					{
						this.itemList[j].GetComponent<UISprite>().spriteName = "Prestige";
						this.itemList[j].GameObject.GetComponentInChildren<UILabel>().text = string.Format(Game.StringTable.GetString(100172), mln.iFame);
					}
					else if (component.m_iIndex == 1)
					{
						this.itemList[j].GetComponent<UISprite>().spriteName = "Gold";
						this.itemList[j].GameObject.GetComponentInChildren<UILabel>().text = string.Format(Game.StringTable.GetString(100171), mln.iMoney);
					}
					else if (component.m_iIndex == 2)
					{
						this.itemList[j].GetComponent<UISprite>().spriteName = "Exp";
						this.itemList[j].GameObject.GetComponentInChildren<UILabel>().text = string.Format(Game.StringTable.GetString(100173), mln.iExp);
					}
					else
					{
						this.itemList[j].GameObject.SetActive(false);
					}
				}
			}
			if (mln.iType == 0 || mln.iRound == 0)
			{
				this.m_pTurnLabel.GameObject.SetActive(false);
			}
			else
			{
				this.m_pTurnLabel.GameObject.SetActive(true);
				int nowRound = YoungHeroTime.m_instance.GetNowRound();
				int num = TeamStatus.m_Instance.GetLevelStartTurn(mln.iLevelID);
				num = num + mln.iRound - nowRound;
				this.m_pTurnValue.Text = num.ToString() + Game.StringTable.GetString(110037);
			}
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0000CE48 File Offset: 0x0000B048
		private void CloseOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				if (bHover)
				{
					this.m_pClose.SpriteName = "CancelHover";
				}
				else
				{
					this.m_pClose.SpriteName = "Cancel";
				}
			}
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x000AD31C File Offset: 0x000AB51C
		private void CloseOnClick(GameObject go)
		{
			this.m_pClose.SpriteName = "Cancel";
			this.m_pMissionInfo.GameObject.SetActive(false);
			if (this.oldMissionPoint != null)
			{
				this.oldMissionPoint.Select(false);
				this.oldMissionPoint = null;
			}
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0000CE7F File Offset: 0x0000B07F
		private void GoButtonOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				if (bHover)
				{
					this.m_pGoButton.SpriteName = "RCButtonHover";
				}
				else
				{
					this.m_pGoButton.SpriteName = "RCButton";
				}
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x000AD370 File Offset: 0x000AB570
		private void GoButtonOnClick(GameObject go)
		{
			if (this.nowMissionLevelNode == null)
			{
				return;
			}
			if (TeamStatus.m_Instance.GetDLCUnitIsOver())
			{
				string @string = Game.StringTable.GetString(990087);
				Game.UI.Get<UIMapMessage>().SetMsg(@string);
				UIDLCCharacter uidlccharacter = Game.UI.Get<UIDLCCharacter>();
				uidlccharacter.Show(UIDLCCharacter.UIType.Unit, "0");
				return;
			}
			for (int i = 0; i < this.nowMissionLevelNode.m_ShaqList.Count; i++)
			{
				CharacterData teamMemberID = TeamStatus.m_Instance.GetTeamMemberID(this.nowMissionLevelNode.m_ShaqList[i]);
				if (teamMemberID == null)
				{
					string npcName = Game.NpcData.GetNpcName(this.nowMissionLevelNode.m_ShaqList[i]);
					string msg = string.Format(Game.StringTable.GetString(990105), npcName);
					Game.UI.Get<UIMapMessage>().SetMsg(msg);
					return;
				}
				if (teamMemberID.iHurtTurn > 0)
				{
					string msg2 = string.Format(Game.StringTable.GetString(990104), teamMemberID._NpcDataNode.m_strNpcName);
					Game.UI.Get<UIMapMessage>().SetMsg(msg2);
					return;
				}
			}
			this.m_pGoButton.SpriteName = "RCButton";
			this.m_pMissionInfo.GameObject.SetActive(false);
			if (this.oldMissionPoint != null)
			{
				this.oldMissionPoint = null;
			}
			Game.UI.Get<UITalk>().SetTalkData(this.nowMissionLevelNode.strTalkID);
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x000AD4EC File Offset: 0x000AB6EC
		public void TalkEndCallBack()
		{
			if (this.nowMissionLevelNode != null)
			{
				int iBattleAreaID = this.nowMissionLevelNode.iBattleAreaID;
				Game.g_BattleControl.SetDLCLevelID(this.nowMissionLevelNode.iLevelID);
				if (this.nowMissionLevelNode.iType == 2 || this.nowMissionLevelNode.iType == 4)
				{
					TeamStatus.m_Instance.DLC_FinishLevel(this.nowMissionLevelNode.iLevelID);
					this.UpdateData();
				}
				this.nowMissionLevelNode = null;
				if (iBattleAreaID != 0)
				{
					Game.g_BattleControl.StartBattle(iBattleAreaID);
				}
			}
			else if (!GameGlobal.m_bDoTalkReward)
			{
				Save.m_Instance.AutoSave();
			}
		}

		// Token: 0x0400182A RID: 6186
		public GameObject goMissionPoint;

		// Token: 0x0400182B RID: 6187
		private Control m_pTipText;

		// Token: 0x0400182C RID: 6188
		private Control m_pPrestige;

		// Token: 0x0400182D RID: 6189
		private Control m_pPrestigeTip;

		// Token: 0x0400182E RID: 6190
		private Control m_pGold;

		// Token: 0x0400182F RID: 6191
		private Control m_pClose;

		// Token: 0x04001830 RID: 6192
		private Control m_pMissionInfo;

		// Token: 0x04001831 RID: 6193
		private Control m_pMissionTitle;

		// Token: 0x04001832 RID: 6194
		private Control m_pMissionName;

		// Token: 0x04001833 RID: 6195
		private Control m_pMissionDesc;

		// Token: 0x04001834 RID: 6196
		private Control m_pGoButton;

		// Token: 0x04001835 RID: 6197
		private Control m_pExpValue;

		// Token: 0x04001836 RID: 6198
		private Control m_pMissionOrig;

		// Token: 0x04001837 RID: 6199
		private Control m_pTurnLabel;

		// Token: 0x04001838 RID: 6200
		private Control m_pTurnValue;

		// Token: 0x04001839 RID: 6201
		private WgCombatMenu m_GameMenu;

		// Token: 0x0400183A RID: 6202
		private List<RCHeroFace> heroList = new List<RCHeroFace>();

		// Token: 0x0400183B RID: 6203
		private List<RCHeroFace> rewardHeroList = new List<RCHeroFace>();

		// Token: 0x0400183C RID: 6204
		private List<MissionPoint> missionPointList = new List<MissionPoint>();

		// Token: 0x0400183D RID: 6205
		private List<Control> itemList = new List<Control>();

		// Token: 0x0400183E RID: 6206
		private MissionPoint oldMissionPoint;

		// Token: 0x0400183F RID: 6207
		private MissionLevelNode nowMissionLevelNode;

		// Token: 0x04001840 RID: 6208
		private int[] MissionTitleArray = new int[]
		{
			100164,
			100164,
			100165,
			100166,
			100166,
			100166,
			100166,
			100166
		};

		// Token: 0x04001841 RID: 6209
		private string[] strBGMusicArray = new string[]
		{
			"Y000004",
			"Y000008",
			"Y000011",
			"Y000012",
			"Y000071"
		};

		// Token: 0x04001842 RID: 6210
		private bool bShowButton = true;

		// Token: 0x04001843 RID: 6211
		private string[] ItemTypeArray = new string[]
		{
			"ui_ItemType0",
			"ui_ItemType0",
			"ui_ItemType1",
			"ui_ItemType2",
			"ui_ItemType3",
			"ui_ItemType4",
			"ui_ItemType5",
			"ui_ItemType6",
			"ui_ItemType7"
		};

		// Token: 0x0200036B RID: 875
		private enum eState
		{
			// Token: 0x04001847 RID: 6215
			None,
			// Token: 0x04001848 RID: 6216
			SelectMission
		}
	}
}
