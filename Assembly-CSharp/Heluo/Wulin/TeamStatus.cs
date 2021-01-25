using System;
using System.Collections.Generic;
using Heluo.Wulin.UI;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200080F RID: 2063
	public class TeamStatus : MonoBehaviour
	{
		// Token: 0x06003275 RID: 12917 RVA: 0x00186644 File Offset: 0x00184844
		private void Awake()
		{
			if (TeamStatus.m_Instance == null)
			{
				TeamStatus.m_Instance = this;
				this.m_TeamMemberList = new List<CharacterData>();
				this.m_FriendshipList = new List<string>();
				this.m_EventList = new List<int>();
				this.m_TreasureBoxList = new List<string>();
				this.m_SaveTitleDataList = new List<SaveTitleDataNode>();
				this.m_AutoSaveTitleDataList = new List<SaveTitleDataNode>();
				this.m_DLCSaveTitleDataList = new List<SaveTitleDataNode>();
				this.m_DLCAutoSaveTitleDataList = new List<SaveTitleDataNode>();
				this.m_NowLevelList = new List<LevelTurnNode>();
				this.m_FinishLevelList = new List<LevelTurnNode>();
				this.m_TeamMessageList = new List<string>();
				return;
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x0001FAC5 File Offset: 0x0001DCC5
		private void Start()
		{
			Save.m_Instance.LoadTitleData();
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x0001FAD1 File Offset: 0x0001DCD1
		[ContextMenu("StartNewGame")]
		public void StartNewGame(bool bDLC = false)
		{
			this.Reset();
			if (bDLC)
			{
				this.AddTeamMember(this.iXyg_DLC_Player);
				this.UpdateNowLevel();
			}
			else
			{
				this.AddTeamMember(this.iXyg_New_XuanID);
			}
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x001866E8 File Offset: 0x001848E8
		public bool GetNewKnight()
		{
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				if (this.m_TeamMemberList[i].iSkillPoint > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x0018672C File Offset: 0x0018492C
		public void Reset()
		{
			this.AttributePoints = 0;
			this.PrestigePoints = 0;
			this.DLCUnitLimit = 5;
			this.DLCStoreLimit = 5;
			this.DLCStoreRenewTurn = 5;
			this.DLCInfoLimit = 1;
			this.DLCInfoRemain = 0;
			this.MaxTeamMember = 9;
			if (GameGlobal.m_bDLCMode)
			{
				this.MaxTeamMember = 999;
			}
			this.m_bHave = false;
			this.m_TeamMemberList.Clear();
			this.m_FriendshipList.Clear();
			this.m_EventList.Clear();
			this.m_TreasureBoxList.Clear();
			this.m_NowLevelList.Clear();
			this.m_FinishLevelList.Clear();
			this.m_TeamMessageList.Clear();
			this.m_UnitInfoList.Clear();
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x001867E4 File Offset: 0x001849E4
		public bool AddTeamMember(int iNpcID)
		{
			if (this.CheckTeamMember(iNpcID))
			{
				Debug.LogError("隊伍中有相同ID的人" + iNpcID);
				return false;
			}
			if (!this.CheckTeamMemberAmount())
			{
				string @string = Game.StringTable.GetString(210041);
				Game.UI.Get<UIMapMessage>().SetMsg(@string);
				Debug.Log("隊伍已達上限人數,請先剔除隊友");
				return false;
			}
			CharacterData characterData = NPC.m_instance.GetCharacterData(iNpcID);
			if (characterData == null)
			{
				Debug.LogError("CharacterData中沒有這個NPCID :  " + iNpcID);
				return false;
			}
			if (!GameGlobal.m_bAffterMode)
			{
				if (iNpcID == this.iXyg_New_GingiID)
				{
					this.m_bHave = true;
				}
				else
				{
					int num = this.iXiaoyao_PurpleID;
				}
				if (iNpcID == this.iXyg_New_GingiID)
				{
					this.m_TeamMemberList.Insert(1, characterData);
				}
				else if (iNpcID == this.iXiaoyao_PurpleID)
				{
					if (this.m_bHave)
					{
						this.m_TeamMemberList.Insert(2, characterData);
					}
					else
					{
						this.m_TeamMemberList.Insert(1, characterData);
					}
				}
				else
				{
					this.m_TeamMemberList.Add(characterData);
				}
			}
			else
			{
				this.m_TeamMemberList.Add(characterData);
			}
			Game.TalentNewData.NotifyGame(TalentEffect.Observant);
			if (this.AddTeammate != null && !GameGlobal.m_bMovie)
			{
				this.AddTeammate.Invoke();
			}
			NPC.m_instance.SetTeam(iNpcID, eNPCType.Teammate);
			return true;
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x00186924 File Offset: 0x00184B24
		public void LessTeamMember(int iNpcID)
		{
			if (!GameGlobal.m_bAffterMode)
			{
				if (iNpcID == this.iXyg_New_GingiID)
				{
					this.MaxTeamMember--;
				}
				else if (iNpcID == this.iXiaoyao_PurpleID)
				{
					this.MaxTeamMember--;
					this.m_bHave = false;
				}
			}
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				if (iNpcID == this.m_TeamMemberList[i].iNpcID)
				{
					this.m_TeamMemberList.Remove(this.m_TeamMemberList[i]);
					NPC.m_instance.SetTeam(iNpcID, eNPCType.Nothing);
					break;
				}
			}
			Game.TalentNewData.NotifyGame(TalentEffect.Observant);
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x001869E8 File Offset: 0x00184BE8
		public float GetTeamTalentPercentValue(TalentEffect Te, bool max = true)
		{
			float num;
			if (max)
			{
				num = float.MinValue;
			}
			else
			{
				num = float.MaxValue;
			}
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				float npcTalentPercentValue = this.m_TeamMemberList[i].GetNpcTalentPercentValue(Te);
				if (max)
				{
					if (npcTalentPercentValue > num)
					{
						num = npcTalentPercentValue;
					}
				}
				else if (npcTalentPercentValue < num)
				{
					num = npcTalentPercentValue;
				}
			}
			return num;
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x00186A60 File Offset: 0x00184C60
		public int GetTalentValue(TalentEffect Te, bool max = true)
		{
			int num;
			if (max)
			{
				num = int.MinValue;
			}
			else
			{
				num = int.MaxValue;
			}
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				int talentValue = this.m_TeamMemberList[i].GetTalentValue(Te);
				if (max)
				{
					if (talentValue > num)
					{
						num = talentValue;
					}
				}
				else if (talentValue < num)
				{
					num = talentValue;
				}
			}
			return num;
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x00186AD4 File Offset: 0x00184CD4
		public bool CheckTeamTalentEffect(TalentEffect Te)
		{
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				if (this.m_TeamMemberList[i].CheckTalentEffect(Te))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x0001FB04 File Offset: 0x0001DD04
		public void ChangeAttributePoints(int iValue)
		{
			this.AttributePoints += iValue;
			this.AttributePoints = Mathf.Clamp(this.AttributePoints, 0, 9999999);
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x00186B18 File Offset: 0x00184D18
		public void AddTeamExp(int iExp)
		{
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				NPC.m_instance.SetNPCNowPracticeExp(this.m_TeamMemberList[i], iExp, false, 0);
			}
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x0001FB2B File Offset: 0x0001DD2B
		public List<CharacterData> GetTeamMemberList()
		{
			return this.m_TeamMemberList;
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x00186B5C File Offset: 0x00184D5C
		public CharacterData GetTeamMember(string id)
		{
			int npcid = int.Parse(id);
			CharacterData characterData = this.m_TeamMemberList.Find((CharacterData x) => x.iNpcID == npcid);
			if (characterData == null && this.m_TeamMemberList.Count > 0)
			{
				characterData = this.m_TeamMemberList[0];
			}
			return characterData;
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x00186BB8 File Offset: 0x00184DB8
		public CharacterData GetTeamMemberID(int id)
		{
			return this.m_TeamMemberList.Find((CharacterData x) => x.iNpcID == id);
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x0001FB33 File Offset: 0x0001DD33
		public int GetAttributePoints()
		{
			return this.AttributePoints;
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x0001FB3B File Offset: 0x0001DD3B
		public int GetPrestigePoints()
		{
			return this.PrestigePoints;
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x0001FB43 File Offset: 0x0001DD43
		public void SetPrestigePoints(int iPoint)
		{
			this.PrestigePoints = iPoint;
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x00186BEC File Offset: 0x00184DEC
		public void AddPrestigePoints(int point)
		{
			int prestigePoints = this.PrestigePoints;
			this.PrestigePoints += point;
			this.PrestigePoints = Mathf.Clamp(this.PrestigePoints, 0, 9999999);
			List<FameNode> fameList = Game.FameData.GetFameList();
			int num = -1;
			for (int i = 0; i < fameList.Count; i++)
			{
				if (prestigePoints < fameList[i].iFame)
				{
					break;
				}
				num = i;
			}
			for (int j = num + 1; j < fameList.Count; j++)
			{
				if (this.PrestigePoints < fameList[j].iFame)
				{
					break;
				}
				if (ConditionManager.CheckCondition(fameList[j].LevelUpConditionList, true, 0, string.Empty))
				{
					Game.RewardData.DoRewardID(fameList[j].iReward, null);
				}
			}
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x00186CDC File Offset: 0x00184EDC
		public string GetPrestigeString()
		{
			string result = string.Empty;
			List<FameNode> fameList = Game.FameData.GetFameList();
			int num = -1;
			for (int i = 0; i < fameList.Count; i++)
			{
				if (this.PrestigePoints < fameList[i].iFame)
				{
					break;
				}
				num = i;
			}
			if (num + 1 < fameList.Count)
			{
				result = string.Format(Game.StringTable.GetString(990106), fameList[num + 1].iFame.ToString(), fameList[num + 1].strDesc);
			}
			else
			{
				result = Game.StringTable.GetString(990107);
			}
			return result;
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x00186D90 File Offset: 0x00184F90
		public bool CheckTeamMember(int iNpcID)
		{
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				if (iNpcID == this.m_TeamMemberList[i].iNpcID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x0001FB4C File Offset: 0x0001DD4C
		public bool CheckTeamMemberAmount()
		{
			return this.m_TeamMemberList.Count < this.MaxTeamMember;
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x0001FB67 File Offset: 0x0001DD67
		public int GetTeamMemberAmount()
		{
			return this.m_TeamMemberList.Count;
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x00186DD4 File Offset: 0x00184FD4
		public void CheckMovieAndPlay(int iIndex)
		{
			if (this.m_EventList.Contains(iIndex))
			{
				Debug.Log("Reward Call Movie but Movie Already Played ");
				return;
			}
			this.m_EventList.Add(iIndex);
			if (GameSetting.m_Instance != null && GameSetting.m_Instance.GetComponent<MovieEventTrigger>() != null)
			{
				GameSetting.m_Instance.GetComponent<MovieEventTrigger>().PlayMovie(iIndex);
			}
			else
			{
				Debug.Log("Reward Call Movie but MovieEventTrigger is null");
			}
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x00186E50 File Offset: 0x00185050
		public void DLC_FinishLevel(int LevelID)
		{
			for (int i = 0; i < this.m_NowLevelList.Count; i++)
			{
				if (this.m_NowLevelList[i].iLevelID == LevelID)
				{
					this.m_NowLevelList.RemoveAt(i);
					break;
				}
			}
			int nowRound = YoungHeroTime.m_instance.GetNowRound();
			MissionLevelNode missionLevelNode = Game.MissionLeveData.GetMissionLevelNode(LevelID);
			if (missionLevelNode == null)
			{
				return;
			}
			if (missionLevelNode.iRepeat == 0)
			{
				return;
			}
			if (!this.FinishLevelAlreadyHave(LevelID))
			{
				LevelTurnNode levelTurnNode = new LevelTurnNode();
				levelTurnNode.iLevelID = LevelID;
				levelTurnNode.iTurn = nowRound;
				this.m_FinishLevelList.Add(levelTurnNode);
				this.RefreshUnit();
			}
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x00186F00 File Offset: 0x00185100
		public bool CheckDLCMissionLevel(int LevelID, int turn)
		{
			int nowRound = YoungHeroTime.m_instance.GetNowRound();
			int i = 0;
			while (i < this.m_FinishLevelList.Count)
			{
				LevelTurnNode levelTurnNode = this.m_FinishLevelList[i];
				if (levelTurnNode.iLevelID == LevelID)
				{
					if (levelTurnNode.iTurn + turn <= nowRound)
					{
						return true;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x00186F64 File Offset: 0x00185164
		public bool CheckDLCAllTeamMateHurt()
		{
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				if (this.m_TeamMemberList[i].iHurtTurn <= 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x0001FB74 File Offset: 0x0001DD74
		public bool CheckDLCGold(int iType, int iValue)
		{
			return this.CheckRequestType(iType, BackpackStatus.m_Instance.GetMoney(), iValue);
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x0001FB88 File Offset: 0x0001DD88
		public bool CheckDLCFame(int iType, int iValue)
		{
			return this.CheckRequestType(iType, this.PrestigePoints, iValue);
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x00071284 File Offset: 0x0006F484
		private bool CheckRequestType(int iType, int val, int iVal)
		{
			if (iType == 0)
			{
				return val == iVal;
			}
			if (iType == 1)
			{
				return val > iVal;
			}
			if (iType == 3)
			{
				return val >= iVal;
			}
			if (iType == 2)
			{
				return val < iVal;
			}
			if (iType == 4)
			{
				return val <= iVal;
			}
			return iType == 5 && val != iVal;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00186FA8 File Offset: 0x001851A8
		private void NowLevelAddID(int iLevelID)
		{
			int nowRound = YoungHeroTime.m_instance.GetNowRound();
			this.NowLevelAddID(iLevelID, nowRound);
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x00186FC8 File Offset: 0x001851C8
		private void NowLevelAddID(int iLevelID, int iTurn)
		{
			LevelTurnNode levelTurnNode = new LevelTurnNode();
			levelTurnNode.iLevelID = iLevelID;
			levelTurnNode.iTurn = iTurn;
			this.m_NowLevelList.Add(levelTurnNode);
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x00186FF8 File Offset: 0x001851F8
		private bool NowLevelAlreadyHave(int iLevelID)
		{
			for (int i = 0; i < this.m_NowLevelList.Count; i++)
			{
				if (this.m_NowLevelList[i].iLevelID == iLevelID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x0018703C File Offset: 0x0018523C
		public int GetLevelStartTurn(int iLevelID)
		{
			for (int i = 0; i < this.m_NowLevelList.Count; i++)
			{
				if (this.m_NowLevelList[i].iLevelID == iLevelID)
				{
					return this.m_NowLevelList[i].iTurn;
				}
			}
			return 0;
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x00187090 File Offset: 0x00185290
		private bool FinishLevelAlreadyHave(int iLevelID)
		{
			for (int i = 0; i < this.m_FinishLevelList.Count; i++)
			{
				if (this.m_FinishLevelList[i].iLevelID == iLevelID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x001870D4 File Offset: 0x001852D4
		public void DLC_TeamRecover()
		{
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				CharacterData characterData = this.m_TeamMemberList[i];
				if (characterData.iHurtTurn > 0)
				{
					characterData.iHurtTurn--;
					if (characterData.iHurtTurn > 0)
					{
						int num = (5 - characterData.iHurtTurn) * 20;
						if (Random.Range(0, 100) < num)
						{
							characterData.iHurtTurn = 0;
							this.m_TeamMessageList.Add(string.Format(Game.StringTable.GetString(990089), characterData._NpcDataNode.m_strNpcName));
						}
					}
					else
					{
						this.m_TeamMessageList.Add(string.Format(Game.StringTable.GetString(990089), characterData._NpcDataNode.m_strNpcName));
					}
				}
			}
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x001871AC File Offset: 0x001853AC
		public void UpdateNowLevel()
		{
			int nowRound = YoungHeroTime.m_instance.GetNowRound();
			int i = 0;
			while (i < this.m_NowLevelList.Count)
			{
				MissionLevelNode missionLevelNode = Game.MissionLeveData.GetMissionLevelNode(this.m_NowLevelList[i].iLevelID);
				if (missionLevelNode == null)
				{
					this.m_NowLevelList.RemoveAt(i);
				}
				else if (missionLevelNode.iRound > 0 && this.m_NowLevelList[i].iTurn + missionLevelNode.iRound <= nowRound)
				{
					this.DLC_FinishLevel(this.m_NowLevelList[i].iLevelID);
				}
				else if (missionLevelNode.m_CloseConditionList.Count > 0 && ConditionManager.CheckCondition(missionLevelNode.m_CloseConditionList, true, 0, string.Empty))
				{
					this.DLC_FinishLevel(this.m_NowLevelList[i].iLevelID);
				}
				else
				{
					i++;
				}
			}
			List<MissionLevelNode> list = new List<MissionLevelNode>();
			List<int> list2 = new List<int>();
			List<MissionLevelNode> missionLevelList = Game.MissionLeveData.GetMissionLevelList();
			int num = 0;
			for (int j = 0; j < missionLevelList.Count; j++)
			{
				MissionLevelNode missionLevelNode2 = missionLevelList[j];
				if (!this.FinishLevelAlreadyHave(missionLevelNode2.iLevelID))
				{
					if (!this.NowLevelAlreadyHave(missionLevelNode2.iLevelID))
					{
						if (ConditionManager.CheckCondition(missionLevelNode2.m_DisplayConditionList, true, 0, string.Empty))
						{
							if (missionLevelNode2.iType == 1 || missionLevelNode2.iType == 2)
							{
								list2.Add(missionLevelNode2.iLevelID);
							}
							else if (missionLevelNode2.iType == 3)
							{
								list.Add(missionLevelNode2);
								num += missionLevelNode2.iWeights;
							}
						}
					}
				}
			}
			for (int k = 0; k < list2.Count; k++)
			{
				this.NowLevelAddID(list2[k]);
			}
			if (list.Count <= 0)
			{
				return;
			}
			int num2 = Random.Range(0, num);
			for (int l = 0; l < list.Count; l++)
			{
				num2 -= list[l].iWeights;
				if (num2 < 0)
				{
					this.NowLevelAddID(list[l].iLevelID);
					break;
				}
			}
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x00187408 File Offset: 0x00185608
		public void AddInfoMission()
		{
			List<MissionLevelNode> missionLevelList = Game.MissionLeveData.GetMissionLevelList();
			List<int> list = new List<int>();
			for (int i = 0; i < missionLevelList.Count; i++)
			{
				MissionLevelNode missionLevelNode = missionLevelList[i];
				if (!this.FinishLevelAlreadyHave(missionLevelNode.iLevelID))
				{
					if (!this.NowLevelAlreadyHave(missionLevelNode.iLevelID))
					{
						if (ConditionManager.CheckCondition(missionLevelNode.m_DisplayConditionList, true, 0, string.Empty))
						{
							if (missionLevelNode.iType == 3 || missionLevelNode.iType == 4)
							{
								list.Add(missionLevelNode.iLevelID);
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				int num = Random.Range(0, list.Count);
				this.NowLevelAddID(list[num]);
			}
			else
			{
				this.m_TeamMessageList.Add(Game.StringTable.GetString(990101));
			}
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x0001FB98 File Offset: 0x0001DD98
		public List<LevelTurnNode> GetNowLevelList()
		{
			return this.m_NowLevelList;
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x001874F8 File Offset: 0x001856F8
		public string GetDLCLevelID()
		{
			if (this.m_FinishLevelList.Count > 0)
			{
				for (int i = this.m_FinishLevelList.Count - 1; i > -1; i--)
				{
					LevelTurnNode levelTurnNode = this.m_FinishLevelList[i];
					if (levelTurnNode.iLevelID / 100000 == 1)
					{
						return levelTurnNode.iLevelID.ToString();
					}
				}
			}
			else
			{
				for (int j = 0; j < this.m_NowLevelList.Count; j++)
				{
					LevelTurnNode levelTurnNode2 = this.m_NowLevelList[j];
					if (levelTurnNode2.iLevelID / 100000 == 1)
					{
						return levelTurnNode2.iLevelID.ToString();
					}
				}
			}
			return "0";
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x0001FBA0 File Offset: 0x0001DDA0
		public void LoadMovieEvent(List<int> eventList)
		{
			this.m_EventList.Clear();
			if (eventList == null)
			{
				return;
			}
			this.m_EventList.AddRange(eventList);
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x0001FBC0 File Offset: 0x0001DDC0
		public bool CheckMovieEvent(int iID)
		{
			return this.m_EventList.Contains(iID);
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x0001FBCE File Offset: 0x0001DDCE
		public int GetMainPlayerID()
		{
			if (this.m_TeamMemberList.Count < 0)
			{
				return 0;
			}
			return this.m_TeamMemberList[0]._NpcDataNode.m_iNpcID;
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x0001FBF9 File Offset: 0x0001DDF9
		public string GetMainPlayerName()
		{
			if (this.m_TeamMemberList.Count < 0)
			{
				return string.Empty;
			}
			return this.m_TeamMemberList[0]._NpcDataNode.m_strNpcName;
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x0001FC28 File Offset: 0x0001DE28
		public void SetAfferTeam()
		{
			this.m_TeamMemberList.Clear();
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x001875B0 File Offset: 0x001857B0
		public List<DLCUnitInfo> GetDLCUnitInfoList()
		{
			List<DLCUnitInfo> list = new List<DLCUnitInfo>();
			for (int i = 0; i < this.m_UnitInfoList.Count; i++)
			{
				list.Add(this.m_UnitInfoList[i].Clone());
			}
			return list;
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x001875F8 File Offset: 0x001857F8
		public List<LevelTurnNode> GetNowLevelSaveDataList()
		{
			List<LevelTurnNode> list = new List<LevelTurnNode>();
			for (int i = 0; i < this.m_NowLevelList.Count; i++)
			{
				list.Add(this.m_NowLevelList[i].Clone());
			}
			return list;
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x00187640 File Offset: 0x00185840
		public List<LevelTurnNode> GetFinishLevelSaveDataList()
		{
			List<LevelTurnNode> list = new List<LevelTurnNode>();
			for (int i = 0; i < this.m_FinishLevelList.Count; i++)
			{
				list.Add(this.m_FinishLevelList[i].Clone());
			}
			return list;
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x00187688 File Offset: 0x00185888
		public List<int> GetTeamMemberIDList()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				list.Add(this.m_TeamMemberList[i].iNpcID);
			}
			return list;
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x001876D0 File Offset: 0x001858D0
		public void LoadTeamMember(List<int> LoadTeamIDList)
		{
			this.m_TeamMemberList.Clear();
			if (LoadTeamIDList == null)
			{
				return;
			}
			for (int i = 0; i < LoadTeamIDList.Count; i++)
			{
				this.AddTeamMember(LoadTeamIDList[i]);
			}
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x00187714 File Offset: 0x00185914
		public void LoadUnitInfoList(List<DLCUnitInfo> list)
		{
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				DLCUnitInfo dlcunitInfo = new DLCUnitInfo
				{
					GID = list[i].GID,
					ID = list[i].ID,
					LV = list[i].LV
				};
				this.m_UnitInfoList.Add(dlcunitInfo);
				dlcunitInfo.Data = Game.CharacterData.GetCharacterData(list[i].ID);
				dlcunitInfo.Refresh();
			}
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x001877AC File Offset: 0x001859AC
		public void LoadFinishLevel(List<LevelTurnNode> list)
		{
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.m_FinishLevelList.Add(list[i].Clone());
			}
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x001877F0 File Offset: 0x001859F0
		public void LoadNowList(List<LevelTurnNode> list)
		{
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.m_NowLevelList.Add(list[i].Clone());
			}
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x0001FC35 File Offset: 0x0001DE35
		public void LoadSaveTitleData(SaveTitleDataNode[] LoadTitleDataList)
		{
			this.m_SaveTitleDataList.Clear();
			this.m_SaveTitleDataList.AddRange(LoadTitleDataList);
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x0001FC4E File Offset: 0x0001DE4E
		public void LoadAutoSaveTitleData(SaveTitleDataNode[] LoadAutoTitleDataList)
		{
			this.m_AutoSaveTitleDataList.Clear();
			this.m_AutoSaveTitleDataList.AddRange(LoadAutoTitleDataList);
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x00187834 File Offset: 0x00185A34
		public void AddDLCUnit(int id, int lv)
		{
			DLCUnitInfo dlcunitInfo = new DLCUnitInfo
			{
				ID = id,
				LV = lv
			};
			dlcunitInfo.GID = this.GetUniqueID();
			dlcunitInfo.Data = Game.CharacterData.GetCharacterData(id);
			dlcunitInfo.Refresh();
			this.m_UnitInfoList.Add(dlcunitInfo);
			this.m_NewUnit = true;
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x00187890 File Offset: 0x00185A90
		public void DLCUnitLevelUP(string gid)
		{
			DLCUnitInfo dlcunitInfo = this.m_UnitInfoList.Find((DLCUnitInfo x) => x.GID == gid);
			if (dlcunitInfo == null)
			{
				if (this.m_UnitInfoList.Count > 0)
				{
					this.m_UnitInfoList[0].LevelUP();
				}
			}
			else
			{
				dlcunitInfo.LevelUP();
			}
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x001878F8 File Offset: 0x00185AF8
		public void DeleteDLCUnit(string gid)
		{
			DLCUnitInfo dlcunitInfo = this.m_UnitInfoList.Find((DLCUnitInfo x) => x.GID == gid);
			if (dlcunitInfo == null)
			{
				if (this.m_UnitInfoList.Count > 0)
				{
					this.m_UnitInfoList.RemoveAt(0);
				}
			}
			else
			{
				this.m_UnitInfoList.Remove(dlcunitInfo);
			}
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x00187960 File Offset: 0x00185B60
		public void _DeleteDLCUnit(string gid)
		{
			DLCUnitInfo dlcunitInfo = this.m_UnitInfoList.Find((DLCUnitInfo x) => x.GID == gid);
			if (dlcunitInfo != null)
			{
				this.m_UnitInfoList.Remove(dlcunitInfo);
			}
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x0001FC67 File Offset: 0x0001DE67
		public List<DLCUnitInfo> GetDLCUnitList()
		{
			return this.m_UnitInfoList;
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x001879A8 File Offset: 0x00185BA8
		public string GetDLCUnitGID(CharacterData data)
		{
			DLCUnitInfo dlcunitInfo = this.m_UnitInfoList.Find((DLCUnitInfo x) => x.Data == data);
			if (dlcunitInfo != null)
			{
				return dlcunitInfo.GID;
			}
			if (this.m_UnitInfoList.Count > 0)
			{
				return this.m_UnitInfoList[0].GID;
			}
			return string.Empty;
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x0001FC6F File Offset: 0x0001DE6F
		public bool GetDLCUnitIsOver()
		{
			return this.m_UnitInfoList.Count > this.DLCUnitLimit;
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x00187A10 File Offset: 0x00185C10
		public bool CheckDLCUnitLevel(string gid, int GetTeamMemberAmount, int value)
		{
			DLCUnitInfo dlcunitInfo = this.m_UnitInfoList.Find((DLCUnitInfo x) => x.GID == gid);
			return dlcunitInfo != null && this.CheckRequestType(GetTeamMemberAmount, dlcunitInfo.LV, value);
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x00187A58 File Offset: 0x00185C58
		public CharacterData GetDLCUnitData(string gid)
		{
			DLCUnitInfo dlcunitInfo = this.m_UnitInfoList.Find((DLCUnitInfo x) => x.GID == gid);
			if (dlcunitInfo != null)
			{
				return dlcunitInfo.Data;
			}
			if (this.m_UnitInfoList.Count > 0)
			{
				return this.m_UnitInfoList[0].Data;
			}
			return null;
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x00187ABC File Offset: 0x00185CBC
		public string GetUniqueID()
		{
			Random random = new Random();
			int num = PlayerPrefs.GetInt("GIDKEY");
			num++;
			if (num > 999)
			{
				num = 0;
			}
			DateTime dateTime;
			dateTime..ctor(1970, 1, 1, 8, 0, 0, 1);
			double num2 = (DateTime.UtcNow - dateTime).TotalSeconds;
			float num3;
			for (num3 = Time.time * 1000000f; num3 > 2.1474836E+09f; num3 -= 2.1474836E+09f)
			{
			}
			while (num2 > 2147483647.0)
			{
				num2 -= 2147483647.0;
			}
			string text = string.Concat(new string[]
			{
				string.Format("{0:X}", Convert.ToInt32(num2)),
				"-",
				string.Format("{0:X}", Convert.ToInt32(num3)),
				"-",
				string.Format("{0:X}", random.Next(1000000000)),
				"-",
				string.Format("{0:000}", num)
			});
			PlayerPrefs.SetInt("GIDKEY", num);
			Debug.Log("Generated Unique ID: " + text);
			return text;
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x00187C00 File Offset: 0x00185E00
		public void RefreshUnit()
		{
			for (int i = 0; i < this.m_TeamMemberList.Count; i++)
			{
				this.m_TeamMemberList[i].DLC_LevelupHidePassive(string.Empty);
			}
			for (int j = 0; j < this.m_UnitInfoList.Count; j++)
			{
				this.m_UnitInfoList[j].Refresh();
			}
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x0001FC8A File Offset: 0x0001DE8A
		public void LoadTreasureBox(List<string> treasureList)
		{
			if (treasureList == null)
			{
				return;
			}
			this.m_TreasureBoxList.AddRange(treasureList);
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x0001FC9F File Offset: 0x0001DE9F
		[ContextMenu("AddTestUnit")]
		public void AddTestUnit()
		{
			TeamStatus.m_Instance.AddDLCUnit(878001, 1);
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x00187C6C File Offset: 0x00185E6C
		[ContextMenu("AddTestUnit50")]
		public void AddTestUnit50()
		{
			for (int i = 0; i < 50; i++)
			{
				TeamStatus.m_Instance.AddDLCUnit(878001, 1);
			}
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x0001FCB1 File Offset: 0x0001DEB1
		[ContextMenu("AddTestItem")]
		public void AddTestItem()
		{
			Game.RewardData.DoRewardID(871001, null);
			Game.RewardData.DoRewardID(871002, null);
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x00187C9C File Offset: 0x00185E9C
		[ContextMenu("賀特一下")]
		public void Hurtonce()
		{
			CharacterData teamMember = TeamStatus.m_Instance.GetTeamMember("0");
			teamMember.iHurtTurn = 10;
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x0000264F File Offset: 0x0000084F
		[ContextMenu("重讀資料")]
		public void ReLoadCharacterData()
		{
		}

		// Token: 0x04003E1B RID: 15899
		public static TeamStatus m_Instance;

		// Token: 0x04003E1C RID: 15900
		public List<string> m_strcFormName = new List<string>();

		// Token: 0x04003E1D RID: 15901
		public List<int> m_EventList;

		// Token: 0x04003E1E RID: 15902
		public List<string> m_TreasureBoxList;

		// Token: 0x04003E1F RID: 15903
		private List<CharacterData> m_TeamMemberList;

		// Token: 0x04003E20 RID: 15904
		public List<string> m_TeamMessageList;

		// Token: 0x04003E21 RID: 15905
		public List<DLCUnitInfo> m_UnitInfoList = new List<DLCUnitInfo>();

		// Token: 0x04003E22 RID: 15906
		public bool m_NewUnit;

		// Token: 0x04003E23 RID: 15907
		public int DLCUnitLimit = 5;

		// Token: 0x04003E24 RID: 15908
		public int DLCStoreLimit = 5;

		// Token: 0x04003E25 RID: 15909
		public int DLCStoreRenewTurn = 5;

		// Token: 0x04003E26 RID: 15910
		public int DLCInfoLimit = 1;

		// Token: 0x04003E27 RID: 15911
		public int DLCInfoRemain;

		// Token: 0x04003E28 RID: 15912
		private List<string> m_FriendshipList;

		// Token: 0x04003E29 RID: 15913
		private List<LevelTurnNode> m_NowLevelList;

		// Token: 0x04003E2A RID: 15914
		private List<LevelTurnNode> m_FinishLevelList;

		// Token: 0x04003E2B RID: 15915
		public Action AddTeammate;

		// Token: 0x04003E2C RID: 15916
		private int AttributePoints;

		// Token: 0x04003E2D RID: 15917
		private int MaxTeamMember;

		// Token: 0x04003E2E RID: 15918
		private bool m_bHave;

		// Token: 0x04003E2F RID: 15919
		private int PrestigePoints;

		// Token: 0x04003E30 RID: 15920
		public List<SaveTitleDataNode> m_SaveTitleDataList;

		// Token: 0x04003E31 RID: 15921
		public List<SaveTitleDataNode> m_AutoSaveTitleDataList;

		// Token: 0x04003E32 RID: 15922
		public List<SaveTitleDataNode> m_DLCSaveTitleDataList;

		// Token: 0x04003E33 RID: 15923
		public List<SaveTitleDataNode> m_DLCAutoSaveTitleDataList;

		// Token: 0x04003E34 RID: 15924
		private int iXyg_DLC_Player = 879001;

		// Token: 0x04003E35 RID: 15925
		private int iXyg_New_XuanID = 210001;

		// Token: 0x04003E36 RID: 15926
		private int iXyg_New_GingiID = 210002;

		// Token: 0x04003E37 RID: 15927
		private int iXiaoyao_PurpleID = 200000;
	}
}
