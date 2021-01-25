using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200032B RID: 811
	public class UITalk : UILayer
	{
		// Token: 0x060011E9 RID: 4585 RVA: 0x0009A670 File Offset: 0x00098870
		protected override void Awake()
		{
			this.m_LabelSelectRightList = new List<Control>();
			this.m_LabelSelectLeftList = new List<Control>();
			this.m_SelectNoPaintedList = new List<Control>();
			this.m_LabelSelectCenterList = new List<Control>();
			this.m_TalkStrList = new List<string>();
			this.m_TalkNameList = new List<string>();
			base.Awake();
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0000BA15 File Offset: 0x00009C15
		private void Start()
		{
			this.m_iIndex = 0;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0009A6C8 File Offset: 0x000988C8
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UITalk.<>f__switch$map22 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(32);
					dictionary.Add("AvatarIconLeft", 0);
					dictionary.Add("ManagerLabelLeft", 1);
					dictionary.Add("AvatarIconRight", 2);
					dictionary.Add("ManagerLabelRight", 3);
					dictionary.Add("AvatarIconCenterR", 4);
					dictionary.Add("LabelNameCenterR", 5);
					dictionary.Add("AvatarIconCenterL", 6);
					dictionary.Add("LabelNameCenterL", 7);
					dictionary.Add("ManagerLabelCenter", 8);
					dictionary.Add("ClickBack", 9);
					dictionary.Add("NextPageLeft", 10);
					dictionary.Add("NextPageRight", 11);
					dictionary.Add("LabelNameLeft", 12);
					dictionary.Add("LabelNameRight", 13);
					dictionary.Add("LabelSelectRight", 14);
					dictionary.Add("LabelSelectLeft", 15);
					dictionary.Add("LabelNameNoPainted", 16);
					dictionary.Add("LabelSelectNoPainted", 17);
					dictionary.Add("LabelSelectCenter", 18);
					dictionary.Add("ManagerLabelNoPainted", 19);
					dictionary.Add("NextPageNoPainted", 20);
					dictionary.Add("NoPaintedGroup", 21);
					dictionary.Add("AsideGroup", 22);
					dictionary.Add("ManagerLabelAside", 23);
					dictionary.Add("NextPageAside", 24);
					dictionary.Add("TalkBackLeft", 25);
					dictionary.Add("LeftGroup", 26);
					dictionary.Add("RightGroup", 27);
					dictionary.Add("CenterGroup", 28);
					dictionary.Add("MidGroup", 29);
					dictionary.Add("ManagerLabelMid", 30);
					dictionary.Add("NextPageMid", 31);
					UITalk.<>f__switch$map22 = dictionary;
				}
				int num;
				if (UITalk.<>f__switch$map22.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_AvatarIconLeft = sender;
						break;
					case 1:
						this.m_ManagerLabelLeft = sender;
						break;
					case 2:
						this.m_AvatarIconRight = sender;
						break;
					case 3:
						this.m_ManagerLabelRight = sender;
						break;
					case 4:
						this.m_AvatarIconCenterR = sender;
						break;
					case 5:
						this.m_LabelNameCenterR = sender;
						break;
					case 6:
						this.m_AvatarIconCenterL = sender;
						break;
					case 7:
						this.m_LabelNameCenterL = sender;
						break;
					case 8:
						this.m_ManagerLabelCenter = sender;
						break;
					case 9:
						this.m_ClickBack = sender;
						this.m_ClickBack.OnClick += this.ClickBackOnClick;
						base.SetInputButton(1, this.m_ClickBack.Listener);
						base.SetCurrent(this.m_ClickBack.Listener, true);
						break;
					case 10:
						this.m_NextPageLeft = sender;
						break;
					case 11:
						this.m_NextPageRight = sender;
						break;
					case 12:
						this.m_LabelNameLeft = sender;
						break;
					case 13:
						this.m_LabelNameRight = sender;
						break;
					case 14:
					{
						Control control = sender;
						control.OnClick += this.SelectOnClick;
						control.OnHover += this.OnMouseHover;
						control.OnKeySelect += this.OnKeySelect;
						this.m_LabelSelectRightList.Add(control);
						base.SetInputButton(2, control.Listener);
						break;
					}
					case 15:
					{
						Control control = sender;
						control.OnClick += this.SelectOnClick;
						control.OnHover += this.OnMouseHover;
						control.OnKeySelect += this.OnKeySelect;
						this.m_LabelSelectLeftList.Add(control);
						base.SetInputButton(2, control.Listener);
						break;
					}
					case 16:
						this.m_LabelNameNoPainted = sender;
						break;
					case 17:
					{
						Control control = sender;
						control.OnClick += this.SelectOnClick;
						control.OnHover += this.OnMouseHover;
						control.OnKeySelect += this.OnKeySelect;
						this.m_SelectNoPaintedList.Add(control);
						base.SetInputButton(2, control.Listener);
						break;
					}
					case 18:
					{
						Control control = sender;
						control.OnClick += this.SelectOnClick;
						control.OnHover += this.OnMouseHover;
						control.OnKeySelect += this.OnKeySelect;
						this.m_LabelSelectCenterList.Add(control);
						base.SetInputButton(2, control.Listener);
						break;
					}
					case 19:
						this.m_ManagerLabelNoPainted = sender;
						break;
					case 20:
						this.m_NextPageNoPainted = sender;
						break;
					case 21:
						this.m_NoPaintedGroup = sender;
						break;
					case 22:
						this.m_AsideGroup = sender;
						break;
					case 23:
						this.m_ManagerLabelAside = sender;
						break;
					case 24:
						this.m_NextPageAside = sender;
						break;
					case 26:
						this.m_LeftGroup = sender;
						break;
					case 27:
						this.m_RightGroup = sender;
						break;
					case 28:
						this.m_CenterGroup = sender;
						break;
					case 29:
						this.m_MidGroup = sender;
						break;
					case 30:
						this.m_ManagerLabelMid = sender;
						break;
					case 31:
						this.m_NextPageMid = sender;
						break;
					}
				}
			}
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0009ACA8 File Offset: 0x00098EA8
		public void ClickBackOnClick(GameObject go)
		{
			if (this.m_bSelect)
			{
				return;
			}
			if (GameGlobal.m_bMovie)
			{
				if (this.OnMovieClick != null)
				{
					this.OnMovieClick(go);
				}
				else
				{
					Time.timeScale = 1f;
					Debug.LogWarning("Movie Talk Why Click In Normal");
					this.ReSetVisible();
				}
				return;
			}
			int num = this.m_iIndex;
			if (this.m_MapTalkTypeNode.m_MapTalkNodeList.Count >= this.m_iIndex)
			{
				for (int i = this.m_iIndex + 1; i < this.m_MapTalkTypeNode.m_MapTalkNodeList.Count; i++)
				{
					if (this.m_MapTalkTypeNode.m_MapTalkNodeList[i].m_MapTalkMapTalkConditionList.Count <= 0)
					{
						break;
					}
					if (ConditionManager.CheckCondition(this.m_MapTalkTypeNode.m_MapTalkNodeList[i].m_MapTalkMapTalkConditionList, true, 0, string.Empty))
					{
						break;
					}
					this.m_iIndex++;
				}
			}
			else
			{
				Debug.Log(string.Concat(new object[]
				{
					this.m_MapTalkTypeNode.m_strTalkGroupID,
					" 的  對話數量 ",
					this.m_MapTalkTypeNode.m_MapTalkNodeList.Count,
					"    對話 m_iIndex",
					this.m_iIndex
				}));
				num = this.m_MapTalkTypeNode.m_MapTalkNodeList.Count - 1;
			}
			this.m_iIndex++;
			if (this.m_MapTalkTypeNode.m_MapTalkNodeList.Count - 1 >= this.m_iIndex)
			{
				int iGiftID = this.m_MapTalkTypeNode.m_MapTalkNodeList[num].m_iGiftID;
				if (iGiftID != 0)
				{
					GameGlobal.m_bDoTalkReward = true;
					base.StartCoroutine(this.DoRewardID(iGiftID));
				}
				this.ReSetVisible();
				this.SetIconAndManager();
			}
			else
			{
				bool flag = false;
				int iGiftID2 = this.m_MapTalkTypeNode.m_MapTalkNodeList[num].m_iGiftID;
				if (iGiftID2 != 0)
				{
					base.StartCoroutine(this.DoRewardID(iGiftID2));
					flag = true;
				}
				if (!flag)
				{
					this.ReSetData();
				}
			}
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x0009AED4 File Offset: 0x000990D4
		private IEnumerator DoRewardID(int iGrftID)
		{
			GameGlobal.m_bDoTalkReward = true;
			bool bEndTalk = false;
			if (this.m_MapTalkTypeNode.m_MapTalkNodeList.Count - 1 < this.m_iIndex)
			{
				bEndTalk = true;
				this.ReSetData();
			}
			yield return new WaitForSeconds(0.05f);
			Game.RewardData.DoRewardID(iGrftID, null);
			GameGlobal.m_bDoTalkReward = false;
			if (GameGlobal.m_bDLCMode)
			{
				Game.UI.Get<UIReadyCombat>().UpdateData();
				if (bEndTalk)
				{
					Save.m_Instance.AutoSave();
				}
			}
			yield break;
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x0009AF00 File Offset: 0x00099100
		private void SelectOnClick(GameObject go)
		{
			if (this.m_bMovie)
			{
				if (this.OnMovieOptionClick != null)
				{
					this.OnMovieOptionClick(go);
					base.EnterState(1);
				}
				else
				{
					Time.timeScale = 1f;
					Debug.LogWarning("Movie Talk Why Click In Normal");
					this.ReSetVisible();
				}
				return;
			}
			MapTalkNode mapTalkNode = this.m_MapTalkTypeNode.m_MapTalkNodeList[this.m_iIndex];
			int iGiftID = this.m_MapTalkTypeNode.m_MapTalkNodeList[this.m_iIndex].m_iGiftID;
			if (iGiftID != 0)
			{
				Game.RewardData.DoRewardID(iGiftID, null);
			}
			MapTalkButtonNode mapTalkButtonNode = mapTalkNode.m_MapTalkButtonNodeList[go.GetComponent<SelectBtn>().m_iIndex];
			this.ReSetVisible();
			this.m_bSelect = false;
			this.m_iButtonType = mapTalkButtonNode.m_iButtonType;
			switch (mapTalkButtonNode.m_iButtonType)
			{
			case 0:
			case 1:
				this.ReSetData();
				break;
			case 2:
				base.EnterState(1);
				this.SetSelectTalk(mapTalkButtonNode.m_strBArg);
				break;
			case 3:
			{
				this.ReSetData();
				PlayerController.m_Instance.TalkStop();
				string[] array = mapTalkButtonNode.m_strBArg.Split(new char[]
				{
					",".get_Chars(0)
				});
				if (array.Length > 1)
				{
					this.iNextTalkID = array[1];
				}
				if (!int.TryParse(array[0], ref this.iStoreID))
				{
					this.iStoreID = 0;
				}
				Game.UI.Show<UIShop>();
				break;
			}
			case 4:
			{
				this.ReSetData();
				UIAbility orCreat = Game.UI.GetOrCreat<UIAbility>();
				if (mapTalkButtonNode.m_strBArg == "5")
				{
					orCreat.StartAlchemy();
				}
				if (mapTalkButtonNode.m_strBArg == "6")
				{
					orCreat.StartForging();
				}
				break;
			}
			case 5:
				this.ReSetData();
				break;
			case 6:
				this.ReSetData();
				Game.g_BattleControl.StartBattle(int.Parse(mapTalkButtonNode.m_strBArg));
				break;
			case 7:
				this.ReSetData();
				GameSetting.m_Instance.GetComponent<MovieEventTrigger>().PlayMovie(int.Parse(mapTalkButtonNode.m_strBArg));
				break;
			case 8:
			{
				string questTalkID = MissionStatus.m_instance.GetQuestTalkID(mapTalkButtonNode.m_strBArg);
				this.SetTalkMsg(questTalkID);
				break;
			}
			case 9:
				this.ReSetData();
				break;
			case 10:
				this.ReSetData();
				break;
			case 11:
			{
				this.ReSetData();
				UIQuestion uiquestion = Game.UI.Get<UIQuestion>();
				if (uiquestion != null)
				{
					uiquestion.SetQuestion(int.Parse(mapTalkButtonNode.m_strBArg), 0);
				}
				break;
			}
			case 12:
				this.ReSetData();
				break;
			}
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0000BA1E File Offset: 0x00009C1E
		public void CloseShopTalk()
		{
			if (this.iNextTalkID.Length == 0)
			{
				return;
			}
			this.SetTalkData(this.iNextTalkID);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0009B1C0 File Offset: 0x000993C0
		public void ReSetData()
		{
			base.EnterState(0);
			Time.timeScale = 1f;
			if (GameGlobal.m_bDLCMode)
			{
				GameGlobal.m_bPlayerTalk = false;
				this.m_iButtonType = -1;
				this.m_ClickBack.GameObject.SetActive(false);
				this.m_iIndex = 0;
				this.ReSetVisible();
				if (GameGlobal.m_strDLCLinkTalk == string.Empty)
				{
					Game.UI.Get<UIReadyCombat>().TalkEndCallBack();
				}
				else
				{
					this.SetTalkMsg(GameGlobal.m_strDLCLinkTalk);
					GameGlobal.m_strDLCLinkTalk = string.Empty;
				}
				return;
			}
			GameObject gameObject = GameObject.FindWithTag("Player");
			if (gameObject != null)
			{
				GameGlobal.m_bPlayerTalk = false;
				if (this.m_iButtonType != 3)
				{
					GameObject target = PlayerController.m_Instance.Target;
					if (target != null)
					{
						target.GetComponent<NpcCollider>().ReSetNpcLook();
						GameGlobal.m_bPlayerTalk = false;
					}
					PlayerController.m_Instance.ReSetMoveDate();
				}
				this.m_iButtonType = -1;
				this.m_ClickBack.GameObject.SetActive(false);
				this.m_iIndex = 0;
				this.ReSetVisible();
			}
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x0009B2D4 File Offset: 0x000994D4
		private void ReSetVisible()
		{
			this.m_LeftGroup.GameObject.SetActive(false);
			this.m_RightGroup.GameObject.SetActive(false);
			this.m_CenterGroup.GameObject.SetActive(false);
			this.m_NoPaintedGroup.GameObject.SetActive(false);
			this.m_AsideGroup.GameObject.SetActive(false);
			this.m_MidGroup.GameObject.SetActive(false);
			for (int i = 0; i < this.m_LabelSelectRightList.Count; i++)
			{
				this.m_LabelSelectLeftList[i].GameObject.SetActive(false);
				this.m_LabelSelectRightList[i].GameObject.SetActive(false);
				this.m_SelectNoPaintedList[i].GameObject.SetActive(false);
				this.m_LabelSelectCenterList[i].GameObject.SetActive(false);
				this.m_LabelSelectLeftList[i].GetComponent<UILabel>().width = 975;
				this.m_LabelSelectRightList[i].GetComponent<UILabel>().width = 975;
				this.m_SelectNoPaintedList[i].GetComponent<UILabel>().width = 975;
				this.m_LabelSelectCenterList[i].GetComponent<UILabel>().width = 975;
			}
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0009B42C File Offset: 0x0009962C
		private void ClickNextPagVisible(MapTalkNode.eImageDirectionType iImageDirectio)
		{
			Control control;
			if (iImageDirectio == MapTalkNode.eImageDirectionType.Left)
			{
				control = this.m_NextPageLeft;
			}
			else if (iImageDirectio == MapTalkNode.eImageDirectionType.Right)
			{
				control = this.m_NextPageRight;
			}
			else if (iImageDirectio == MapTalkNode.eImageDirectionType.NoPainted)
			{
				control = this.m_NextPageNoPainted;
			}
			else if (iImageDirectio == MapTalkNode.eImageDirectionType.MidText)
			{
				control = this.m_NextPageMid;
			}
			else
			{
				control = this.m_NextPageAside;
			}
			if (this.m_MapTalkTypeNode.m_MapTalkNodeList.Count - 1 > this.m_iIndex)
			{
				control.GameObject.SetActive(true);
			}
			else
			{
				control.GameObject.SetActive(false);
			}
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0009B4C8 File Offset: 0x000996C8
		public void SetTalkData(string StringMsg)
		{
			if (!GameGlobal.m_bDLCMode)
			{
				GameObject gameObject = GameObject.FindWithTag("Player");
				if (gameObject != null)
				{
					gameObject.GetComponent<NavMeshAgent>().Stop(true);
				}
			}
			this.m_ClickBack.GameObject.SetActive(true);
			GameGlobal.m_bPlayerTalk = true;
			this.m_MapTalkTypeNode = Game.MapTalkData.GetMapTalkTypeNode(StringMsg);
			if (this.m_MapTalkTypeNode != null)
			{
				this.m_iIndex = 0;
				this.SetIconAndManager();
				base.gameObject.SetActive(true);
				return;
			}
			Debug.LogError("對話編號 發生錯誤   " + StringMsg);
			StringMsg = string.Empty;
			this.ReSetData();
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0000BA3D File Offset: 0x00009C3D
		public void StartMovieEvent(UIEventListener.VoidDelegate callback, UIEventListener.VoidDelegate selectcallback)
		{
			this.OnMovieClick = callback;
			this.OnMovieOptionClick = selectcallback;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0000BA4D File Offset: 0x00009C4D
		public void EndMovieEvnet()
		{
			base.EnterState(0);
			this.OnMovieClick = null;
			this.OnMovieOptionClick = null;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0009B564 File Offset: 0x00099764
		public bool GetMovieTalkNpcTile(int movieID, int Step)
		{
			this.m_MapTalkTypeNode = Game.MapTalkData.GetMapTalkTypeNode(movieID.ToString());
			this.m_iIndex = Step;
			MapTalkNode mapTalkNode = this.m_MapTalkTypeNode.m_MapTalkNodeList[this.m_iIndex];
			return mapTalkNode.m_iNpcID < 0;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0009B5B8 File Offset: 0x000997B8
		public MapTalkNode GetMovieTalkNode(int movieID, int Step)
		{
			this.m_MapTalkTypeNode = Game.MapTalkData.GetMapTalkTypeNode(movieID.ToString());
			if (this.m_MapTalkTypeNode == null)
			{
				return null;
			}
			return this.m_MapTalkTypeNode.m_MapTalkNodeList[Step];
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0009B5FC File Offset: 0x000997FC
		public void CheckJumpMovieTalkQuest(int movieID, int step)
		{
			this.m_MapTalkTypeNode = Game.MapTalkData.GetMapTalkTypeNode(movieID.ToString());
			if (this.m_MapTalkTypeNode == null)
			{
				return;
			}
			this.m_iIndex = step;
			if (this.m_iIndex >= this.m_MapTalkTypeNode.m_MapTalkNodeList.Count)
			{
				return;
			}
			MapTalkNode mapTalkNode = this.m_MapTalkTypeNode.m_MapTalkNodeList[this.m_iIndex];
			if (mapTalkNode.m_strNextQuestID.Length > 2 && !MissionStatus.m_instance.CheckQuest(mapTalkNode.m_strNextQuestID))
			{
				MissionStatus.m_instance.AddQuestList(mapTalkNode.m_strNextQuestID);
			}
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0009B6A0 File Offset: 0x000998A0
		public bool SetMovieTalk(GameObject go, int movieID, int step)
		{
			this.m_MapTalkTypeNode = Game.MapTalkData.GetMapTalkTypeNode(movieID.ToString());
			if (this.m_MapTalkTypeNode == null)
			{
				return false;
			}
			this.m_iIndex = step;
			if (this.m_iIndex >= this.m_MapTalkTypeNode.m_MapTalkNodeList.Count)
			{
				return false;
			}
			MapTalkNode mapTalkNode = this.m_MapTalkTypeNode.m_MapTalkNodeList[this.m_iIndex];
			if (mapTalkNode == null)
			{
				return false;
			}
			string text = string.Empty;
			if (mapTalkNode.m_iNpcID != 0)
			{
				text = Game.NpcData.GetImageName(mapTalkNode.m_iNpcID);
			}
			else
			{
				text = TeamStatus.m_Instance.GetMainPlayerID().ToString();
			}
			if (text != string.Empty)
			{
				Texture2D texture2D = Game.g_MapHeadBundle.Load("2dtexture/gameui/maphead/" + text) as Texture2D;
				if (texture2D != null)
				{
					this.m_ClickBack.GameObject.SetActive(true);
					this.m_bMovie = true;
					this.SetIconAndManager();
					return true;
				}
			}
			this.m_ClickBack.GameObject.SetActive(true);
			this.m_bMovie = true;
			this.SetIconAndManager();
			return true;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0000BA64 File Offset: 0x00009C64
		public void ResetMovieTalk()
		{
			this.m_ClickBack.GameObject.SetActive(false);
			this.m_bMovie = false;
			this.m_iIndex = 0;
			this.ReSetVisible();
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0000BA8B File Offset: 0x00009C8B
		public void SetSelectTalk(string strIndex)
		{
			this.SetTalkMsg(strIndex);
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0009B7C4 File Offset: 0x000999C4
		public void SetTalkMsg(string strIndex)
		{
			MapTalkTypeNode mapTalkTypeNode = Game.MapTalkData.GetMapTalkTypeNode(strIndex);
			if (mapTalkTypeNode == null)
			{
				string strManager;
				if (this.m_MapTalkTypeNode != null)
				{
					strManager = "無連接對話 " + strIndex + " 上一個 TalkGroupID = " + this.m_MapTalkTypeNode.m_strTalkGroupID;
				}
				else
				{
					strManager = "無對話 " + strIndex;
				}
				mapTalkTypeNode = new MapTalkTypeNode();
				mapTalkTypeNode.m_strTalkGroupID = "8825252";
				MapTalkNode mapTalkNode = new MapTalkNode();
				mapTalkNode.m_strManager = strManager;
				mapTalkNode.m_iNpcID = 99;
				mapTalkNode.m_ImageDirection = MapTalkNode.eImageDirectionType.Aside;
				mapTalkNode.m_strNextQuestID = "0";
				mapTalkNode.m_strActionID = "stand01";
				mapTalkNode.m_strNpcVoice = "0";
				mapTalkTypeNode.m_MapTalkNodeList.Add(mapTalkNode);
				this.m_MapTalkTypeNode = mapTalkTypeNode;
			}
			else
			{
				this.m_MapTalkTypeNode = mapTalkTypeNode;
				this.mod_JudgeDifficultySetting(strIndex);
				this.mod_JudgeNewBattleUnitCount(strIndex);
				this.mod_JudgeBossBattleUnitCount(strIndex);
				this.mod_JudgeRandomBattleUnitCount(strIndex);
				this.mod_JudgeMinigame(strIndex);
				if (strIndex.Contains("modShilian_") && strIndex.Split(new char[]
				{
					'_'
				}).Length < 4)
				{
					GameGlobal.mod_ShilianLayer = 1;
					GameGlobal.mod_ShilianEnemyCount = 0;
					this.mod_JudgeShilian(strIndex);
					if (TeamStatus.m_Instance.GetTeamMemberAmount() < GameGlobal.mod_ShilianEnemyCount)
					{
						Game.UI.Get<UIMapMessage>().SetMsg("队伍人数不足！");
						return;
					}
					Game.g_BattleControl.StartBattle(88000004);
				}
				if (strIndex.Contains("mod_Minigame_"))
				{
					return;
				}
			}
			this.m_iIndex = 0;
			this.SetIconAndManager();
			this.m_ClickBack.GameObject.SetActive(true);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0009B93C File Offset: 0x00099B3C
		private void SetIconAndManager()
		{
			string msg = string.Empty;
			MapTalkNode mapTalkNode = this.m_MapTalkTypeNode.m_MapTalkNodeList[this.m_iIndex];
			if (mapTalkNode.m_strNextQuestID.Length > 2 && !MissionStatus.m_instance.CheckQuest(mapTalkNode.m_strNextQuestID))
			{
				MissionStatus.m_instance.AddQuestList(mapTalkNode.m_strNextQuestID);
			}
			List<string> list = TextParser.IDtoString(mapTalkNode.m_strManager);
			if (list != null)
			{
				msg = TextParser.StringToFormat(mapTalkNode.m_strManager, list);
			}
			else
			{
				msg = mapTalkNode.m_strManager;
			}
			string name = string.Empty;
			string iconID = string.Empty;
			if (mapTalkNode.m_iNpcID == 0)
			{
				iconID = TeamStatus.m_Instance.GetMainPlayerID().ToString();
				GameObject gameObject = GameObject.FindWithTag("Player");
				name = TeamStatus.m_Instance.GetMainPlayerName();
				if (gameObject != null)
				{
					gameObject.GetComponent<PlayerController>().SetTalkAni(mapTalkNode.m_strActionID, 2);
				}
			}
			else if (mapTalkNode.m_iNpcID == 99)
			{
				iconID = string.Empty;
				name = string.Empty;
			}
			else if (GameGlobal.m_bDLCMode)
			{
				iconID = NPC.m_instance.GetImageName(mapTalkNode.m_iNpcID);
				name = NPC.m_instance.GetNpcName(mapTalkNode.m_iNpcID);
			}
			else
			{
				iconID = Game.NpcData.GetImageName(mapTalkNode.m_iNpcID);
				name = Game.NpcData.GetNpcName(mapTalkNode.m_iNpcID);
			}
			MapTalkNode.eImageDirectionType imageDirection = mapTalkNode.m_ImageDirection;
			List<Control> list2 = null;
			if (imageDirection == MapTalkNode.eImageDirectionType.Left)
			{
				this.SetTalkUI(this.m_AvatarIconLeft, iconID, this.m_ManagerLabelLeft, msg, this.m_LabelNameLeft, name, mapTalkNode);
				list2 = this.m_LabelSelectLeftList;
				this.OpenType = 0;
			}
			else if (imageDirection == MapTalkNode.eImageDirectionType.Right)
			{
				this.SetTalkUI(this.m_AvatarIconRight, iconID, this.m_ManagerLabelRight, msg, this.m_LabelNameRight, name, mapTalkNode);
				list2 = this.m_LabelSelectRightList;
				this.OpenType = 1;
			}
			else if (imageDirection == MapTalkNode.eImageDirectionType.NoPainted)
			{
				this.SetTalkUI(null, iconID, this.m_ManagerLabelNoPainted, msg, this.m_LabelNameNoPainted, name, mapTalkNode);
				list2 = this.m_SelectNoPaintedList;
				this.OpenType = 2;
			}
			else if (imageDirection == MapTalkNode.eImageDirectionType.Center)
			{
				if (mapTalkNode.m_iNpcIDEX != 0)
				{
					if (GameGlobal.m_bDLCMode)
					{
						iconID = NPC.m_instance.GetImageName(mapTalkNode.m_iNpcID);
						name = NPC.m_instance.GetNpcName(mapTalkNode.m_iNpcID);
					}
					else
					{
						iconID = Game.NpcData.GetImageName(mapTalkNode.m_iNpcID);
						name = Game.NpcData.GetNpcName(mapTalkNode.m_iNpcID);
					}
					this.SetTalkUI(this.m_AvatarIconCenterL, iconID, this.m_LabelNameCenterL, name, mapTalkNode);
					if (GameGlobal.m_bDLCMode)
					{
						iconID = NPC.m_instance.GetImageName(mapTalkNode.m_iNpcIDEX);
						name = NPC.m_instance.GetNpcName(mapTalkNode.m_iNpcIDEX);
					}
					else
					{
						iconID = Game.NpcData.GetImageName(mapTalkNode.m_iNpcIDEX);
						name = Game.NpcData.GetNpcName(mapTalkNode.m_iNpcIDEX);
					}
					this.SetTalkUI(this.m_AvatarIconCenterR, iconID, this.m_ManagerLabelCenter, msg, this.m_LabelNameCenterR, name, mapTalkNode);
				}
				else
				{
					this.SetTalkUI(this.m_AvatarIconCenterR, iconID, this.m_ManagerLabelCenter, msg, this.m_LabelNameCenterR, name, mapTalkNode);
				}
				list2 = this.m_LabelSelectCenterList;
				this.OpenType = 3;
			}
			else if (imageDirection == MapTalkNode.eImageDirectionType.MidText)
			{
				this.SetTalkUI(null, iconID, this.m_ManagerLabelMid, msg, null, name, mapTalkNode);
			}
			else
			{
				this.SetTalkUI(null, iconID, this.m_ManagerLabelAside, msg, null, name, mapTalkNode);
			}
			if (mapTalkNode.m_bInFields)
			{
				for (int i = 0; i < mapTalkNode.m_MapTalkButtonNodeList.Count; i++)
				{
					list2[i].GameObject.SetActive(true);
					list2[i].Text = mapTalkNode.m_MapTalkButtonNodeList[i].m_strButtonName;
					list2[i].GetComponent<UILabel>().width = (int)list2[i].GetComponent<UILabel>().printedSize.x;
					list2[i].GameObject.GetComponent<BoxCollider>().size = list2[i].GetComponent<UILabel>().printedSize;
				}
				base.EnterState(2);
				if (!GameCursor.IsShow)
				{
					base.SetCurrent(list2[0].Listener, true);
				}
				else
				{
					base.SetCurrent(list2[0].Listener, false);
				}
				this.m_bSelect = true;
			}
			this.ClickNextPagVisible(mapTalkNode.m_ImageDirection);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x0009BDD4 File Offset: 0x00099FD4
		private void SetTalkUI(Control Icon, string IconID, Control TalkName, string Name, MapTalkNode _MapTalkNode)
		{
			MapTalkNode.eImageDirectionType imageDirection = _MapTalkNode.m_ImageDirection;
			if (!IconID.Equals("0") && Icon != null)
			{
				Texture2D mainTexture = Game.g_MapHeadBundle.Load("2dtexture/gameui/maphead/" + IconID) as Texture2D;
				Icon.GetComponent<UITexture>().mainTexture = mainTexture;
			}
			if (TalkName != null)
			{
				if (imageDirection == MapTalkNode.eImageDirectionType.NoPainted)
				{
					TalkName.Text = Name + ":";
				}
				else
				{
					TalkName.Text = Name;
				}
			}
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0009BE54 File Offset: 0x0009A054
		private void SetTalkUI(Control Icon, string IconID, Control TalkMsg, string Msg, Control TalkName, string Name, MapTalkNode _MapTalkNode)
		{
			this.Show();
			this.iNextTalkID = string.Empty;
			string strNpcVoice = _MapTalkNode.m_strNpcVoice;
			MapTalkNode.eImageDirectionType imageDirection = _MapTalkNode.m_ImageDirection;
			if (int.Parse(strNpcVoice) != 0)
			{
				AudioSource component = Icon.GameObject.GetComponent<AudioSource>();
				if (component != null)
				{
					component.clip = null;
					component.Stop();
					if (Game.g_AudioBundle.Contains("audio/UI/" + strNpcVoice))
					{
						AudioClip clip = Game.g_AudioBundle.Load("audio/UI/" + strNpcVoice) as AudioClip;
						component.clip = clip;
						component.loop = false;
						component.Play();
					}
					else
					{
						Debug.Log("audio/UI/" + strNpcVoice + " no found");
					}
				}
				else
				{
					Debug.Log("audioSource is null");
				}
			}
			if (!IconID.Equals("0") && Icon != null)
			{
				Texture2D mainTexture = Game.g_MapHeadBundle.Load("2dtexture/gameui/maphead/" + IconID) as Texture2D;
				Icon.GetComponent<UITexture>().mainTexture = mainTexture;
			}
			if (this.m_TalkStrList.Count > 19)
			{
				this.m_TalkStrList.RemoveAt(0);
				this.m_TalkNameList.RemoveAt(0);
			}
			if (TalkName != null)
			{
				if (imageDirection == MapTalkNode.eImageDirectionType.NoPainted)
				{
					TalkName.Text = Name + ":";
				}
				else
				{
					TalkName.Text = Name;
				}
				this.m_TalkNameList.Add(Name);
			}
			else
			{
				this.m_TalkNameList.Add(string.Empty);
			}
			TalkMsg.Text = Msg;
			this.m_TalkStrList.Add(TalkMsg.Text);
			if (imageDirection == MapTalkNode.eImageDirectionType.Left)
			{
				this.m_LeftGroup.GameObject.SetActive(true);
			}
			else if (imageDirection == MapTalkNode.eImageDirectionType.Right)
			{
				this.m_RightGroup.GameObject.SetActive(true);
			}
			else if (imageDirection == MapTalkNode.eImageDirectionType.Aside)
			{
				this.m_AsideGroup.GameObject.SetActive(true);
			}
			else if (imageDirection == MapTalkNode.eImageDirectionType.Center)
			{
				this.m_CenterGroup.GameObject.SetActive(true);
			}
			else if (imageDirection == MapTalkNode.eImageDirectionType.MidText)
			{
				this.m_MidGroup.GameObject.SetActive(true);
			}
			else if (imageDirection == MapTalkNode.eImageDirectionType.NoPainted)
			{
				this.m_NoPaintedGroup.GameObject.SetActive(true);
				string text = string.Empty;
				if (_MapTalkNode.m_iNpcID == 1)
				{
					GameObject gameObject = GameObject.FindWithTag("Player");
					text = gameObject.GetComponent<PlayerController>().Target.name;
				}
				else
				{
					text = _MapTalkNode.m_iNpcID.ToString();
				}
				if (!GameGlobal.m_bMovie)
				{
				}
			}
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0000BA94 File Offset: 0x00009C94
		private void OnMouseHover(GameObject go, bool bHover)
		{
			if (bHover && GameCursor.IsShow)
			{
				go.GetComponent<UILabel>().color = Color.red;
			}
			else
			{
				go.GetComponent<UILabel>().color = Color.black;
			}
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0009C0F0 File Offset: 0x0009A2F0
		private void UpdateSelectColor(int iType)
		{
			if (iType == 0)
			{
				for (int i = 0; i < this.m_LabelSelectLeftList.Count; i++)
				{
					Control control = this.m_LabelSelectLeftList[i];
					control.GameObject.GetComponent<UILabel>().color = Color.black;
				}
			}
			else if (iType == 1)
			{
				for (int i = 0; i < this.m_LabelSelectRightList.Count; i++)
				{
					Control control2 = this.m_LabelSelectRightList[i];
					control2.GameObject.GetComponent<UILabel>().color = Color.black;
				}
			}
			else if (iType == 2)
			{
				for (int i = 0; i < this.m_SelectNoPaintedList.Count; i++)
				{
					Control control3 = this.m_SelectNoPaintedList[i];
					control3.GameObject.GetComponent<UILabel>().color = Color.black;
				}
			}
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x0009C1D8 File Offset: 0x0009A3D8
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			Game.UI.Hide<UIMainSelect>();
			this.m_bShow = true;
			GameGlobal.m_bPlayerTalk = true;
			Game.g_InputManager.Push(this);
			base.EnterState(1);
			if (GameGlobal.m_bDLCMode)
			{
				UIReadyCombat uireadyCombat = Game.UI.Get<UIReadyCombat>();
				if (uireadyCombat != null)
				{
					uireadyCombat.HideButton();
				}
			}
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x0009C244 File Offset: 0x0009A444
		public override void Hide()
		{
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			GameGlobal.m_bPlayerTalk = false;
			Game.g_InputManager.Pop();
			if (GameGlobal.m_bDLCMode)
			{
				UIReadyCombat uireadyCombat = Game.UI.Get<UIReadyCombat>();
				if (uireadyCombat != null)
				{
					uireadyCombat.ShowButton();
				}
			}
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0000BACB File Offset: 0x00009CCB
		public override void OnMouseControl(bool bCtrl)
		{
			if (bCtrl)
			{
				this.UpdateSelectColor(this.OpenType);
			}
			base.OnMouseControl(bCtrl);
			this.HideKeySelect();
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x0009C29C File Offset: 0x0009A49C
		private void OnKeySelect(GameObject go, bool bselt)
		{
			this.UpdateSelectColor(this.OpenType);
			if (bselt && !GameCursor.IsShow)
			{
				go.GetComponent<UILabel>().color = Color.red;
				this.ShowKeySelect(go, new Vector3(-50f, -20f, 0f), KeySelect.eSelectDir.Left, 64, 64);
			}
			else
			{
				go.GetComponent<UILabel>().color = Color.black;
			}
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x0009C30C File Offset: 0x0009A50C
		protected override void OnStateEnter(int state)
		{
			if (state != 0)
			{
				if (state != 1)
				{
				}
				List<UIEventListener> list = this.controls[this.NowState];
				if (list != null && list.Count > 0)
				{
					this.current = list[0];
					if (GameCursor.IsShow)
					{
						base.SetCurrentOnly(this.current);
					}
					else
					{
						base.SetCurrent(this.current, true);
					}
				}
				return;
			}
			this.Hide();
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x0000BAEC File Offset: 0x00009CEC
		public override void OnKeyUp(KeyControl.Key key)
		{
			if (key <= KeyControl.Key.Right)
			{
				this.SelectNextButton(key);
				return;
			}
			if (key != KeyControl.Key.OK)
			{
				if (key == KeyControl.Key.TalkLog)
				{
					Game.UI.Show<UISaveTalk>();
				}
				return;
			}
			base.OnCurrentClick();
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x0000BB16 File Offset: 0x00009D16
		public void ClearTalkLogRecord()
		{
			this.m_TalkStrList.Clear();
			this.m_TalkNameList.Clear();
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x0000BB2E File Offset: 0x00009D2E
		public void mod_JudgeDifficultySetting(string talkID)
		{
			if (talkID.Contains("mod_Difficulty_"))
			{
				GameGlobal.mod_Difficulty = Convert.ToInt32(talkID.Split(new char[]
				{
					'_'
				})[2]);
			}
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x0000BB5A File Offset: 0x00009D5A
		public void mod_JudgeNewBattleUnitCount(string talkID)
		{
			if (talkID.Contains("mod_NewBattle_"))
			{
				GameGlobal.mod_NewBattleEnemyCount = Convert.ToInt32(talkID.Split(new char[]
				{
					'_'
				})[2]);
				Game.g_BattleControl.StartBattle(88000001);
			}
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x0000BB95 File Offset: 0x00009D95
		public void mod_JudgeBossBattleUnitCount(string talkID)
		{
			if (talkID.Contains("mod_BossBattle_"))
			{
				GameGlobal.mod_BossBattleEnemyCount = Convert.ToInt32(talkID.Split(new char[]
				{
					'_'
				})[2]);
				Game.g_BattleControl.StartBattle(88000002);
			}
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x0000BBD0 File Offset: 0x00009DD0
		public void mod_JudgeRandomBattleUnitCount(string talkID)
		{
			if (talkID.Contains("mod_RandomBattle_"))
			{
				GameGlobal.mod_RandomBattleEnemyCount = Convert.ToInt32(talkID.Split(new char[]
				{
					'_'
				})[2]);
				Game.g_BattleControl.StartBattle(88000003);
			}
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x0009C394 File Offset: 0x0009A594
		public void mod_JudgeMinigame(string talkID)
		{
			if (talkID.Contains("mod_Minigame_"))
			{
				int num = Convert.ToInt32(talkID.Split(new char[]
				{
					'_'
				})[2]);
				if (num == 1)
				{
					Game.UI.GetOrCreat<UIAbility>().StartAlchemy();
					return;
				}
				if (num == 2)
				{
					Game.UI.GetOrCreat<UIAbility>().StartForging();
					return;
				}
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x0009C3F0 File Offset: 0x0009A5F0
		public void mod_JudgeShilian(string talkID)
		{
			string[] array = talkID.Split(new char[]
			{
				'_'
			});
			string text = array[1];
			int mod_ShilianEnemyCount;
			if (text.Equals("B"))
			{
				mod_ShilianEnemyCount = 2;
			}
			else if (text.Equals("C"))
			{
				mod_ShilianEnemyCount = 3;
			}
			else if (text.Equals("D"))
			{
				mod_ShilianEnemyCount = 4;
			}
			else if (text.Equals("E"))
			{
				mod_ShilianEnemyCount = 5;
			}
			else if (text.Equals("F"))
			{
				mod_ShilianEnemyCount = 6;
			}
			else if (text.Equals("G"))
			{
				mod_ShilianEnemyCount = 7;
			}
			else if (text.Equals("H"))
			{
				mod_ShilianEnemyCount = 8;
			}
			else if (text.Equals("I"))
			{
				mod_ShilianEnemyCount = 9;
			}
			else
			{
				mod_ShilianEnemyCount = 1;
			}
			int mod_ShilianLayer = Convert.ToInt32(array[2]);
			GameGlobal.mod_ShilianEnemyCount = mod_ShilianEnemyCount;
			GameGlobal.mod_ShilianLayer = mod_ShilianLayer;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x0000BC0B File Offset: 0x00009E0B
		public void mod_ShilianBattle(int enemyCount, int Layer)
		{
			GameGlobal.mod_ShilianEnemyCount = enemyCount;
			GameGlobal.mod_ShilianLayer = Layer;
			Game.g_BattleControl.StartBattle(88000004);
		}

		// Token: 0x04001595 RID: 5525
		private Control m_AvatarIconLeft;

		// Token: 0x04001596 RID: 5526
		private Control m_ManagerLabelLeft;

		// Token: 0x04001597 RID: 5527
		private Control m_AvatarIconRight;

		// Token: 0x04001598 RID: 5528
		private Control m_ManagerLabelRight;

		// Token: 0x04001599 RID: 5529
		private Control m_ManagerLabelNoPainted;

		// Token: 0x0400159A RID: 5530
		private Control m_AvatarIconCenterR;

		// Token: 0x0400159B RID: 5531
		private Control m_LabelNameCenterR;

		// Token: 0x0400159C RID: 5532
		private Control m_AvatarIconCenterL;

		// Token: 0x0400159D RID: 5533
		private Control m_LabelNameCenterL;

		// Token: 0x0400159E RID: 5534
		private Control m_ManagerLabelCenter;

		// Token: 0x0400159F RID: 5535
		private Control m_ClickBack;

		// Token: 0x040015A0 RID: 5536
		private Control m_NextPageLeft;

		// Token: 0x040015A1 RID: 5537
		private Control m_NextPageRight;

		// Token: 0x040015A2 RID: 5538
		private Control m_NextPageNoPainted;

		// Token: 0x040015A3 RID: 5539
		private Control m_LabelNameLeft;

		// Token: 0x040015A4 RID: 5540
		private Control m_LabelNameRight;

		// Token: 0x040015A5 RID: 5541
		private Control m_LabelNameNoPainted;

		// Token: 0x040015A6 RID: 5542
		private Control m_LeftGroup;

		// Token: 0x040015A7 RID: 5543
		private Control m_RightGroup;

		// Token: 0x040015A8 RID: 5544
		private Control m_CenterGroup;

		// Token: 0x040015A9 RID: 5545
		private Control m_NoPaintedGroup;

		// Token: 0x040015AA RID: 5546
		private Control m_AsideGroup;

		// Token: 0x040015AB RID: 5547
		private Control m_MidGroup;

		// Token: 0x040015AC RID: 5548
		private Control m_ManagerLabelMid;

		// Token: 0x040015AD RID: 5549
		private Control m_NextPageMid;

		// Token: 0x040015AE RID: 5550
		private Control m_ManagerLabelAside;

		// Token: 0x040015AF RID: 5551
		private Control m_NextPageAside;

		// Token: 0x040015B0 RID: 5552
		private List<Control> m_LabelSelectRightList;

		// Token: 0x040015B1 RID: 5553
		private List<Control> m_LabelSelectLeftList;

		// Token: 0x040015B2 RID: 5554
		private List<Control> m_SelectNoPaintedList;

		// Token: 0x040015B3 RID: 5555
		private List<Control> m_LabelSelectCenterList;

		// Token: 0x040015B4 RID: 5556
		private int m_iIndex;

		// Token: 0x040015B5 RID: 5557
		private MapTalkTypeNode m_MapTalkTypeNode;

		// Token: 0x040015B6 RID: 5558
		public bool m_bSelect;

		// Token: 0x040015B7 RID: 5559
		private int m_iButtonType = -1;

		// Token: 0x040015B8 RID: 5560
		public bool m_bMovie;

		// Token: 0x040015B9 RID: 5561
		private UIEventListener.VoidDelegate OnMovieClick;

		// Token: 0x040015BA RID: 5562
		private UIEventListener.VoidDelegate OnMovieOptionClick;

		// Token: 0x040015BB RID: 5563
		public List<string> m_TalkStrList;

		// Token: 0x040015BC RID: 5564
		public List<string> m_TalkNameList;

		// Token: 0x040015BD RID: 5565
		public int iStoreID;

		// Token: 0x040015BE RID: 5566
		private string iNextTalkID = string.Empty;

		// Token: 0x040015BF RID: 5567
		private int OpenType;

		// Token: 0x0200032C RID: 812
		private enum eState
		{
			// Token: 0x040015C2 RID: 5570
			None,
			// Token: 0x040015C3 RID: 5571
			Talk,
			// Token: 0x040015C4 RID: 5572
			Select
		}

		// Token: 0x0200032D RID: 813
		private enum SelectType
		{
			// Token: 0x040015C6 RID: 5574
			None,
			// Token: 0x040015C7 RID: 5575
			CloseTalkUI,
			// Token: 0x040015C8 RID: 5576
			NextTalkID,
			// Token: 0x040015C9 RID: 5577
			OpenShop,
			// Token: 0x040015CA RID: 5578
			OpenAbility,
			// Token: 0x040015CB RID: 5579
			None5,
			// Token: 0x040015CC RID: 5580
			GoBattle,
			// Token: 0x040015CD RID: 5581
			PlayMovie,
			// Token: 0x040015CE RID: 5582
			OtherQuest,
			// Token: 0x040015CF RID: 5583
			None9,
			// Token: 0x040015D0 RID: 5584
			None10,
			// Token: 0x040015D1 RID: 5585
			QuestionGame,
			// Token: 0x040015D2 RID: 5586
			None12
		}
	}
}
