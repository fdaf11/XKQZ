using System;
using System.Collections.Generic;
using Heluo.Wulin.UI;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200024F RID: 591
	public class RewardDataManager : TextDataManager
	{
		// Token: 0x06000AE6 RID: 2790 RVA: 0x00008A22 File Offset: 0x00006C22
		public RewardDataManager()
		{
			this.m_RewardDataNodeList = new List<RewardDataNode>();
			this.m_LoadFileName = "RewardData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00008A64 File Offset: 0x00006C64
		public static RewardDataManager Singleton
		{
			get
			{
				return RewardDataManager.instance;
			}
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0005A940 File Offset: 0x00058B40
		protected override void LoadFile(string filePath)
		{
			this.m_RewardDataNodeList.Clear();
			string[] array = base.ExtractTextFile(filePath);
			if (array == null)
			{
				return;
			}
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (text.get_Chars(0) != '#')
					{
						try
						{
							string[] array3 = text.Split(new char[]
							{
								"\t".get_Chars(0)
							});
							RewardDataNode rewardDataNode = new RewardDataNode();
							rewardDataNode.m_iRewardID = int.Parse(array3[0]);
							string text2 = array3[1].Replace(")*(", "*");
							text2 = text2.Replace("\r", string.Empty);
							text2 = text2.Substring(1, text2.Length - 2);
							string[] array4 = text2.Split(new char[]
							{
								"*".get_Chars(0)
							});
							for (int j = 0; j < array4.Length; j++)
							{
								string[] args = array4[j].Split(new char[]
								{
									",".get_Chars(0)
								});
								MapRewardNode mapRewardNode = new MapRewardNode(args);
								rewardDataNode.m_MapRewardNodeList.Add(mapRewardNode);
							}
							this.m_RewardDataNodeList.Add(rewardDataNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
			this.m_RewardDataNodeList.Sort((RewardDataNode A, RewardDataNode B) => A.m_iRewardID.CompareTo(B.m_iRewardID));
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0005AAE8 File Offset: 0x00058CE8
		public RewardDataNode GetMapRewardNode(int iRewardID)
		{
			for (int i = 0; i < this.m_RewardDataNodeList.Count; i++)
			{
				if (this.m_RewardDataNodeList[i].m_iRewardID == iRewardID)
				{
					return this.m_RewardDataNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0005AB38 File Offset: 0x00058D38
		public bool CheckRewardHaveChangeScene(int iRewardID)
		{
			RewardDataNode mapRewardNode = this.GetMapRewardNode(iRewardID);
			if (mapRewardNode == null)
			{
				return false;
			}
			for (int i = 0; i < mapRewardNode.m_MapRewardNodeList.Count; i++)
			{
				MapRewardNode mapRewardNode2 = mapRewardNode.m_MapRewardNodeList[i];
				if (mapRewardNode2._RewardType == RewardType.GameEnd || mapRewardNode2._RewardType == RewardType.GameStart || mapRewardNode2._RewardType == RewardType.ReadyFight)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0005ABA8 File Offset: 0x00058DA8
		public void DoRewardID(int iRewardID, RewardDataNode _RewardDataNode = null)
		{
			if (_RewardDataNode == null)
			{
				Debug.Log("iRewardID = " + iRewardID.ToString());
				_RewardDataNode = this.GetMapRewardNode(iRewardID);
			}
			if (_RewardDataNode == null)
			{
				Debug.LogError("No Found DoRewardID " + iRewardID.ToString());
				return;
			}
			this.mod_JudgeMODBattleReward(iRewardID, ref _RewardDataNode);
			for (int i = 0; i < _RewardDataNode.m_MapRewardNodeList.Count; i++)
			{
				MapRewardNode mapRewardNode = _RewardDataNode.m_MapRewardNodeList[i];
				RewardType rewardType = mapRewardNode._RewardType;
				int num = int.Parse(mapRewardNode.m_strGType);
				int iAmount = mapRewardNode.m_iAmount;
				int iMsgID = mapRewardNode.m_iMsgID;
				CParaValue parameter = mapRewardNode.m_Parameter;
				string aString = string.Empty;
				switch (rewardType)
				{
				case RewardType.PropertyType:
					aString = Game.NpcData.GetNpcName(parameter);
					NPC.m_instance.SetCharacterProperty(parameter, num, iAmount);
					this.MapMessage(iMsgID, aString, iAmount, string.Empty);
					break;
				case RewardType.AddRoutine:
					if (NPC.m_instance.AddRoutine(parameter, iAmount, num))
					{
						aString = Game.NpcData.GetNpcName(parameter);
						string getRoutineName = Game.RoutineNewData.GetGetRoutineName(iAmount);
						this.MapMessage(iMsgID, aString, 0, getRoutineName);
					}
					break;
				case RewardType.AddNeigong:
					if (NPC.m_instance.AddNeigong(parameter, iAmount, num))
					{
						aString = Game.NpcData.GetNpcName(parameter);
						string neigongName = Game.NeigongData.GetNeigongName(iAmount);
						this.MapMessage(iMsgID, aString, 0, neigongName);
					}
					break;
				case RewardType.ToBattle:
					if (GameGlobal.m_bBigMapMode)
					{
						BigMapController.m_Instance.BigMapGoToBattle(mapRewardNode.m_Parameter);
					}
					break;
				case RewardType.AddItem:
					aString = Game.ItemData.GetItemName(parameter);
					if (BackpackStatus.m_Instance.AddPackItem(parameter, iAmount, true))
					{
						this.MapMessage(iMsgID, aString, iAmount, string.Empty);
					}
					if (GameGlobal.m_bBattle && UINGUI.instance != null)
					{
						UINGUI.instance.UpdateItemBackpack();
					}
					break;
				case RewardType.AddTeamMember:
					aString = Game.NpcData.GetNpcName(parameter);
					if (TeamStatus.m_Instance.AddTeamMember(parameter))
					{
						this.MapMessage(iMsgID, aString, 0, string.Empty);
					}
					break;
				case RewardType.LessTeamMember:
					aString = Game.NpcData.GetNpcName(parameter);
					TeamStatus.m_Instance.LessTeamMember(parameter);
					this.MapMessage(iMsgID, aString, 0, string.Empty);
					break;
				case RewardType.Money:
					BackpackStatus.m_Instance.ChangeMoney(iAmount);
					if (iAmount > 0)
					{
						this.MapMessage(iMsgID, iAmount.ToString(), 0, string.Empty);
					}
					else
					{
						this.MapMessage(iMsgID, (-iAmount).ToString(), 0, string.Empty);
					}
					break;
				case RewardType.AttributePoint:
				{
					int talentValue = TeamStatus.m_Instance.GetTalentValue(TalentEffect.MoreAttributePoints, true);
					int iValue = Mathf.RoundToInt((1f + TeamStatus.m_Instance.GetTeamTalentPercentValue(TalentEffect.MoreAttributePoints, true)) * (float)(iAmount + talentValue));
					TeamStatus.m_Instance.ChangeAttributePoints(iValue);
					this.MapMessage(iMsgID, iValue.ToString(), 0, string.Empty);
					break;
				}
				case RewardType.TeamExp:
				{
					int talentValue2 = TeamStatus.m_Instance.GetTalentValue(TalentEffect.MoreExperiences, true);
					int iExp = Mathf.RoundToInt((1f + TeamStatus.m_Instance.GetTeamTalentPercentValue(TalentEffect.MoreExperiences, true)) * (float)(iAmount + talentValue2));
					TeamStatus.m_Instance.AddTeamExp(iExp);
					this.MapMessage(iMsgID, iExp.ToString(), 0, string.Empty);
					break;
				}
				case RewardType.AddAbilityId:
				{
					string text = parameter;
					Game.Variable["Ability_" + text] = iAmount;
					break;
				}
				case RewardType.PlayMovie:
					TeamStatus.m_Instance.CheckMovieAndPlay(parameter);
					break;
				case RewardType.GameEnd:
					if (Game.UI.Get<UIEnd>() == null)
					{
						Game.UI.CreateUI("cFormEnd");
					}
					if (Game.UI.Get<UIEnd>() != null)
					{
						Game.UI.Get<UIEnd>().PlayEnd(num, iMsgID);
					}
					break;
				case RewardType.AffterDay:
					this.SetAffter();
					break;
				case RewardType.AddTalent:
					if (NPC.m_instance.AddTalent(parameter, iAmount))
					{
						aString = Game.NpcData.GetNpcName(parameter);
						string talentName = Game.TalentNewData.GetTalentName(iAmount);
						this.MapMessage(iMsgID, aString, 0, talentName);
					}
					break;
				case RewardType.Prequel:
					if (Game.UI.Get<UIBiographies>() == null)
					{
						Game.UI.CreateUI("cFormBiographies");
					}
					Game.UI.Get<UIBiographies>().StartBiographies(parameter);
					break;
				case RewardType.Round:
					YoungHeroTime.m_instance.AddRound(iAmount);
					break;
				case RewardType.PersonalExp:
					aString = Game.NpcData.GetNpcName(parameter.m_iVal);
					if (GameGlobal.m_bDLCMode)
					{
						NPC.m_instance.DLC_AddNpcExp(parameter.m_iVal, iAmount);
					}
					else
					{
						NPC.m_instance.SetNPCNowPracticeExp(null, iAmount, true, parameter.m_iVal);
					}
					this.MapMessage(iMsgID, aString, iAmount, string.Empty);
					break;
				case RewardType.CollectionQuest:
					MissionStatus.m_instance.AddCollectionQuest(parameter);
					break;
				case RewardType.FlagVariable:
				{
					GlobalVariableManager variable = Game.Variable;
					string key;
					int num2 = variable[key = parameter];
					variable[key] = num2 + iAmount;
					break;
				}
				case RewardType.LessItem:
					aString = Game.ItemData.GetItemName(parameter);
					if (BackpackStatus.m_Instance.LessPackItem(parameter, iAmount, null))
					{
						this.MapMessage(iMsgID, aString, 0, string.Empty);
					}
					if (GameGlobal.m_bBattle && UINGUI.instance != null)
					{
						UINGUI.instance.UpdateItemBackpack();
					}
					break;
				case RewardType.IngQuest:
					MissionStatus.m_instance.AddQuestList(parameter);
					break;
				case RewardType.GameStart:
					Game.UI.Get<UILoad>().LoadStage("GameStart");
					break;
				case RewardType.ReadyFight:
					Game.LoadLevelOnFinish = (Game.LoadSenceOnFinish)Delegate.Combine(Game.LoadLevelOnFinish, new Game.LoadSenceOnFinish(this.GoBackReadyFightOnFinish));
					RewardDataManager.strReadyFightBackTalkNo = parameter;
					Game.UI.Get<UILoad>().LoadStage("ReadyCombat");
					break;
				case RewardType.EquipItem:
				{
					CharacterData characterData = NPC.m_instance.GetCharacterData(parameter.m_iVal);
					BackpackNewDataNode backpackItem = BackpackStatus.m_Instance.GetBackpackItem(iAmount);
					if (characterData != null && backpackItem != null && BackpackStatus.m_Instance.CheckItemUse(characterData, backpackItem._ItemDataNode))
					{
						this.MapMessage(iMsgID, characterData._NpcDataNode.m_strNpcName, 0, backpackItem._ItemDataNode.m_strItemName);
						BackpackStatus.m_Instance.UseItem(characterData, backpackItem);
					}
					if (GameGlobal.m_bBattle && UINGUI.instance != null)
					{
						UINGUI.instance.UpdateUnitInfo();
					}
					break;
				}
				case RewardType.AddDLCUnit:
					aString = Game.NpcData.GetNpcName(parameter);
					TeamStatus.m_Instance.AddDLCUnit(parameter, iAmount);
					this.MapMessage(iMsgID, aString, 0, string.Empty);
					break;
				case RewardType.ChangeNpcID:
					NPC.m_instance.ChangeNpcDataID(parameter, iAmount);
					break;
				case RewardType.DLCUnitLimit:
					TeamStatus.m_Instance.DLCUnitLimit += parameter.m_iVal;
					this.MapMessage(iMsgID, TeamStatus.m_Instance.DLCUnitLimit.ToString(), 0, string.Empty);
					break;
				case RewardType.StoreLimit:
					TeamStatus.m_Instance.DLCStoreLimit += parameter.m_iVal;
					this.MapMessage(iMsgID, TeamStatus.m_Instance.DLCStoreLimit.ToString(), 0, string.Empty);
					break;
				case RewardType.DLCInfoLimit:
					TeamStatus.m_Instance.DLCInfoLimit += parameter.m_iVal;
					this.MapMessage(iMsgID, TeamStatus.m_Instance.DLCInfoLimit.ToString(), 0, string.Empty);
					break;
				case RewardType.StoreRenewTurn:
					TeamStatus.m_Instance.DLCStoreRenewTurn -= parameter.m_iVal;
					if (TeamStatus.m_Instance.DLCStoreRenewTurn < 2)
					{
						TeamStatus.m_Instance.DLCStoreRenewTurn = 2;
					}
					if (TeamStatus.m_Instance.DLCStoreRenewTurn > 10)
					{
						TeamStatus.m_Instance.DLCStoreRenewTurn = 10;
					}
					this.MapMessage(iMsgID, TeamStatus.m_Instance.DLCStoreRenewTurn.ToString(), 0, string.Empty);
					break;
				case RewardType.DLCLinkTalk:
					GameGlobal.m_strDLCLinkTalk = parameter.m_sVal;
					break;
				}
			}
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00008A6B File Offset: 0x00006C6B
		private void SetAffter()
		{
			GameGlobal.m_bAffterMode = true;
			TeamStatus.m_Instance.SetAfferTeam();
			NPC.m_instance.SetAffterNPCData();
			MissionStatus.m_instance.AddCollectionQuest("Q999999");
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0005B460 File Offset: 0x00059660
		private void MapMessage(int iMsgID, string aString, int iValue = 0, string bString = "")
		{
			if (iMsgID == 0)
			{
				return;
			}
			string text = Game.StringTable.GetString(iMsgID);
			if (bString == string.Empty)
			{
				if (iValue == 0)
				{
					text = string.Format(text, aString);
				}
				else if (aString == "null")
				{
					text = string.Format(text, iValue.ToString());
				}
				else
				{
					text = string.Format(text, aString, iValue.ToString());
				}
			}
			else if (iValue == 0)
			{
				text = string.Format(text, aString, bString);
			}
			else
			{
				text = string.Format(text, aString, iValue.ToString(), bString);
			}
			if (GameGlobal.m_bBattle)
			{
				if (UINGUI.instance.IsBattleEnd())
				{
					if (GameGlobal.m_bDLCMode)
					{
						TeamStatus.m_Instance.m_TeamMessageList.Add(text);
					}
				}
				else
				{
					UINGUI.BattleMessage(text);
					UINGUI.DisplayMessage(text);
				}
			}
			else if (Game.UI.Get<UIMapMessage>() != null)
			{
				Game.UI.Get<UIMapMessage>().SetMsg(text);
			}
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0005B570 File Offset: 0x00059770
		public void GoBackReadyFightOnFinish()
		{
			Game.LoadLevelOnFinish = (Game.LoadSenceOnFinish)Delegate.Remove(Game.LoadLevelOnFinish, new Game.LoadSenceOnFinish(this.GoBackReadyFightOnFinish));
			if (RewardDataManager.strReadyFightBackTalkNo != null && RewardDataManager.strReadyFightBackTalkNo != "0")
			{
				Game.UI.Get<UITalk>().SetTalkData(RewardDataManager.strReadyFightBackTalkNo);
			}
			else
			{
				Save.m_Instance.AutoSave();
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0005B5E0 File Offset: 0x000597E0
		public void mod_JudgeMODBattleReward(int iRewardID, ref RewardDataNode _RewardDataNode)
		{
			if (iRewardID == 88000001)
			{
				_RewardDataNode.m_MapRewardNodeList.Clear();
				if (Random.Range(0f, 1f) < (float)(GameGlobal.mod_Difficulty * GameGlobal.mod_NewBattleEnemyCount) / 1000f)
				{
					MapRewardNode mapRewardNode = new MapRewardNode();
					mapRewardNode._RewardType = RewardType.AttributePoint;
					mapRewardNode.m_Parameter = 0;
					mapRewardNode.m_iAmount = Random.Range(1, GameGlobal.mod_Difficulty * GameGlobal.mod_NewBattleEnemyCount);
					mapRewardNode.m_iMsgID = 100206;
					mapRewardNode.m_strGType = "0";
					_RewardDataNode.m_MapRewardNodeList.Add(mapRewardNode);
				}
			}
			if (iRewardID == 88000002)
			{
				_RewardDataNode.m_MapRewardNodeList.Clear();
				if (Random.Range(0f, 1f) < (float)(GameGlobal.mod_Difficulty * GameGlobal.mod_BossBattleEnemyCount) / 500f)
				{
					MapRewardNode mapRewardNode2 = new MapRewardNode();
					mapRewardNode2._RewardType = RewardType.AttributePoint;
					mapRewardNode2.m_Parameter = 0;
					mapRewardNode2.m_iAmount = Random.Range(1, GameGlobal.mod_Difficulty * GameGlobal.mod_BossBattleEnemyCount);
					mapRewardNode2.m_iMsgID = 100206;
					mapRewardNode2.m_strGType = "0";
					_RewardDataNode.m_MapRewardNodeList.Add(mapRewardNode2);
				}
			}
			if (iRewardID == 88000003)
			{
				_RewardDataNode.m_MapRewardNodeList.Clear();
				if (Random.Range(0f, 1f) < (float)(GameGlobal.mod_Difficulty * GameGlobal.mod_RandomBattleEnemyCount) / 750f)
				{
					MapRewardNode mapRewardNode3 = new MapRewardNode();
					mapRewardNode3._RewardType = RewardType.AttributePoint;
					mapRewardNode3.m_Parameter = 0;
					mapRewardNode3.m_iAmount = Random.Range(1, GameGlobal.mod_Difficulty * GameGlobal.mod_RandomBattleEnemyCount);
					mapRewardNode3.m_iMsgID = 100206;
					mapRewardNode3.m_strGType = "0";
					_RewardDataNode.m_MapRewardNodeList.Add(mapRewardNode3);
				}
			}
		}

		// Token: 0x04000C55 RID: 3157
		private static readonly RewardDataManager instance = new RewardDataManager();

		// Token: 0x04000C56 RID: 3158
		private List<RewardDataNode> m_RewardDataNodeList;

		// Token: 0x04000C57 RID: 3159
		private static string strReadyFightBackTalkNo;
	}
}
