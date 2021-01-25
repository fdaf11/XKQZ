using System;
using System.Collections.Generic;
using Heluo.Wulin.UI;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200027B RID: 635
	public class MissionStatus : MonoBehaviour
	{
		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06000BC7 RID: 3015 RVA: 0x0000917B File Offset: 0x0000737B
		// (remove) Token: 0x06000BC8 RID: 3016 RVA: 0x00009194 File Offset: 0x00007394
		private event Action Notify;

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000091AD File Offset: 0x000073AD
		private void Awake()
		{
			if (MissionStatus.m_instance == null)
			{
				MissionStatus.m_instance = this;
				this.SetNotify(new Action(MouseEventCube.ResetAllMouseEvent));
				return;
			}
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x000091DD File Offset: 0x000073DD
		public void Reset()
		{
			this.QuestList.Clear();
			this.TimeQuestList.Clear();
			this.m_iNew = 0;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00060F74 File Offset: 0x0005F174
		public string GetQuestTalkID(string strQuestID)
		{
			string result = string.Empty;
			bool flag = false;
			QuestNode questNode = Game.QuestData.GetQuestNode(strQuestID);
			if (questNode == null)
			{
				return result;
			}
			QuestStatus questData = this.GetQuestData(strQuestID);
			if (questData != null)
			{
				MissionStatus.QuestType iType = (MissionStatus.QuestType)questData.iType;
				if (iType == MissionStatus.QuestType.Fail || iType == MissionStatus.QuestType.Finish)
				{
					return result;
				}
				bool flag2 = ConditionManager.CheckCondition(questNode.m_FinshQuestNodeList, true, 0, string.Empty);
				if (flag2)
				{
					this.AddCollectionQuest(strQuestID);
					flag = true;
					result = questNode.m_strFinshQuestManager;
				}
				else
				{
					result = questNode.m_strQuestIngManager;
				}
			}
			else
			{
				bool flag3 = ConditionManager.CheckCondition(questNode.m_QuestOpenNodeList, true, 0, string.Empty);
				if (flag3)
				{
					bool flag2 = ConditionManager.CheckCondition(questNode.m_FinshQuestNodeList, true, 0, string.Empty);
					if (flag2)
					{
						this.AddCollectionQuest(strQuestID);
						flag = true;
						result = questNode.m_strFinshQuestManager;
					}
					else
					{
						this.AddQuestList(strQuestID);
						result = questNode.m_strGetManager;
					}
				}
			}
			if (flag)
			{
				this.SetQuestFinishReward(questNode);
			}
			return result;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00061070 File Offset: 0x0005F270
		public string OldGetQuestTalkID(string strQuestID)
		{
			string result = string.Empty;
			bool flag = false;
			QuestNode questNode = Game.QuestData.GetQuestNode(strQuestID);
			if (questNode == null)
			{
				return result;
			}
			QuestStatus questData = this.GetQuestData(strQuestID);
			bool flag2 = false;
			if (questData != null)
			{
				MissionStatus.QuestType iType = (MissionStatus.QuestType)questData.iType;
				if (iType == MissionStatus.QuestType.Fail)
				{
					return result;
				}
				if (iType == MissionStatus.QuestType.Ing)
				{
					flag2 = ConditionManager.CheckCondition(questNode.m_FinshQuestNodeList, true, 0, string.Empty);
				}
				if (flag2 || iType == MissionStatus.QuestType.Finish)
				{
					if (iType == MissionStatus.QuestType.Ing)
					{
						this.AddCollectionQuest(strQuestID);
						flag = true;
					}
					result = questNode.m_strFinshQuestManager;
				}
				else
				{
					if (iType != MissionStatus.QuestType.Ing && flag2)
					{
						return result;
					}
					result = questNode.m_strQuestIngManager;
				}
			}
			else
			{
				bool flag3 = ConditionManager.CheckCondition(questNode.m_QuestOpenNodeList, true, 0, string.Empty);
				if (flag3)
				{
					flag2 = ConditionManager.CheckCondition(questNode.m_FinshQuestNodeList, true, 0, string.Empty);
					if (flag2)
					{
						this.AddCollectionQuest(strQuestID);
						flag = true;
						result = questNode.m_strFinshQuestManager;
					}
					else if (!flag2)
					{
						this.AddQuestList(strQuestID);
						result = questNode.m_strGetManager;
					}
				}
			}
			if (flag)
			{
				this.SetQuestFinishReward(questNode);
			}
			return result;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00061198 File Offset: 0x0005F398
		private void SetQuestFinishReward(QuestNode _QuestNode)
		{
			RewardDataNode rewardDataNode = new RewardDataNode();
			rewardDataNode.m_iRewardID = 999999;
			for (int i = 0; i < _QuestNode.m_FinshQuestNodeList.Count; i++)
			{
				string text = _QuestNode.m_FinshQuestNodeList[i].m_Parameter.m_iVal.ToString();
				string text2 = _QuestNode.m_FinshQuestNodeList[i].m_iValue.ToString();
				if (_QuestNode.m_FinshQuestNodeList[i]._iType == ConditionKind.Item)
				{
					string[] args = new string[]
					{
						"21",
						text,
						text2,
						"200070",
						"0"
					};
					MapRewardNode mapRewardNode = new MapRewardNode(args);
					rewardDataNode.m_MapRewardNodeList.Add(mapRewardNode);
				}
				else if (_QuestNode.m_FinshQuestNodeList[i]._iType == ConditionKind.TeamMoney)
				{
					string[] args2 = new string[]
					{
						"8",
						"0",
						text2,
						"200068",
						"0"
					};
					MapRewardNode mapRewardNode2 = new MapRewardNode(args2);
					rewardDataNode.m_MapRewardNodeList.Add(mapRewardNode2);
				}
			}
			if (rewardDataNode.m_MapRewardNodeList.Count > 0)
			{
				Game.RewardData.DoRewardID(999999, rewardDataNode);
			}
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x000612DC File Offset: 0x0005F4DC
		public string getNewMissionID()
		{
			string empty = string.Empty;
			for (int i = 0; i < this.QuestList.Count; i++)
			{
				if (this.QuestList[i] != null)
				{
					QuestNode questNode = Game.QuestData.GetQuestNode(this.QuestList[i].m_strQuestID);
					if (questNode != null)
					{
						if (questNode.m_eType == QuestNode.eQuestType.ShowUI && this.QuestList[i].iType == 0)
						{
							return this.QuestList[i].m_strQuestID;
						}
					}
				}
			}
			return empty;
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00061380 File Offset: 0x0005F580
		public QuestStatus GetQuestStatusNode(string strQuestID)
		{
			foreach (QuestStatus questStatus in this.QuestList)
			{
				if (questStatus.m_strQuestID == strQuestID)
				{
					return questStatus;
				}
			}
			foreach (TimeQuestStatus timeQuestStatus in this.TimeQuestList)
			{
				if (timeQuestStatus.m_strQuestID == strQuestID.Trim())
				{
					return timeQuestStatus;
				}
			}
			return null;
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00061450 File Offset: 0x0005F650
		private TimeQuestStatus GetTimeStatusNode(string strQuestID)
		{
			foreach (TimeQuestStatus timeQuestStatus in this.TimeQuestList)
			{
				if (timeQuestStatus.m_strQuestID == strQuestID.Trim())
				{
					return timeQuestStatus;
				}
			}
			return null;
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x000614C4 File Offset: 0x0005F6C4
		public QuestStatus GetQuestData(string strQuestID)
		{
			foreach (QuestStatus questStatus in this.QuestList)
			{
				if (questStatus.m_strQuestID == strQuestID.Trim())
				{
					return questStatus;
				}
			}
			foreach (TimeQuestStatus timeQuestStatus in this.TimeQuestList)
			{
				if (timeQuestStatus.m_strQuestID == strQuestID.Trim())
				{
					return timeQuestStatus;
				}
			}
			return null;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0006159C File Offset: 0x0005F79C
		public bool AddQuestList(string strQuestID)
		{
			QuestNode questNode = Game.QuestData.GetQuestNode(strQuestID);
			if (questNode != null && !this.CheckQuestlist(strQuestID))
			{
				if (questNode.m_LimitQuest.m_iIsLimit == 1)
				{
					TimeQuestStatus timeQuestStatus = new TimeQuestStatus();
					timeQuestStatus.m_strQuestID = strQuestID;
					timeQuestStatus.iType = 0;
					timeQuestStatus.m_iOpenTime = YoungHeroTime.m_instance.GetNowRound();
					timeQuestStatus.m_failTime = timeQuestStatus.m_iOpenTime + questNode.m_LimitQuest.m_iRound;
					timeQuestStatus.bfinish = false;
					this.TimeQuestList.Insert(0, timeQuestStatus);
					GameDebugTool.Log("加 限時 進行中任務" + questNode.m_strQuestID + "  " + questNode.m_strQuestName);
				}
				else
				{
					QuestStatus questStatus = new QuestStatus();
					questStatus.m_strQuestID = strQuestID;
					questStatus.iType = 0;
					this.QuestList.Insert(0, questStatus);
					GameDebugTool.Log("加了一個進行中任務" + questNode.m_strQuestID + "  " + questNode.m_strQuestName);
				}
				if (questNode.m_eType == QuestNode.eQuestType.ShowUI)
				{
					this.m_iNew++;
				}
				if (this.Notify != null)
				{
					this.Notify.Invoke();
				}
				return true;
			}
			Debug.Log("Questdata表 沒有 這個任務ID   " + strQuestID);
			return false;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000616D4 File Offset: 0x0005F8D4
		private bool CheckLimitQuestIsNoOverTime(TimeQuestStatus _TimeQuestStatus)
		{
			return YoungHeroTime.m_instance.CheackRound(eCompareType.GreaterOrEqual, _TimeQuestStatus.m_failTime);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x000616FC File Offset: 0x0005F8FC
		public void RemoveQuest(string strQuestID)
		{
			QuestStatus questStatusNode = this.GetQuestStatusNode(strQuestID);
			if (questStatusNode != null)
			{
				this.QuestList.Remove(questStatusNode);
			}
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00061724 File Offset: 0x0005F924
		public void AddCollectionQuest(string strQuestID)
		{
			QuestNode questNode = Game.QuestData.GetQuestNode(strQuestID);
			if (questNode == null)
			{
				Debug.Log("Questdata表 沒有 這個任務ID   " + strQuestID);
				return;
			}
			QuestStatus questStatus = this.GetQuestStatusNode(strQuestID);
			if (questStatus == null)
			{
				if (!questNode.m_bRepeat)
				{
					questStatus = new QuestStatus();
					questStatus.m_strQuestID = questNode.m_strQuestID;
					questStatus.iType = 1;
					this.QuestList.Insert(0, questStatus);
				}
			}
			else
			{
				TimeQuestStatus timeStatusNode = this.GetTimeStatusNode(strQuestID);
				if (timeStatusNode != null && !questNode.m_bRepeat)
				{
					QuestStatus questStatus2 = new QuestStatus();
					questStatus2.m_strQuestID = questNode.m_strQuestID;
					questStatus2.iType = 1;
					this.QuestList.Insert(0, questStatus2);
				}
				if (!questNode.m_bRepeat)
				{
					questStatus.iType = 1;
					if (questNode.m_eType == QuestNode.eQuestType.ShowUI && this.m_iNew > 0)
					{
						this.m_iNew--;
					}
				}
				else if (this.QuestList.Contains(questStatus))
				{
					this.QuestList.Remove(questStatus);
				}
				else
				{
					this.RemoveTimeQuest(questStatus.m_strQuestID);
				}
			}
			if (this.Notify != null)
			{
				this.Notify.Invoke();
			}
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x0006185C File Offset: 0x0005FA5C
		private void RemoveTimeQuest(string ID)
		{
			TimeQuestStatus timeQuestStatus = null;
			for (int i = 0; i < this.TimeQuestList.Count; i++)
			{
				if (this.TimeQuestList[i].m_strQuestID == ID)
				{
					timeQuestStatus = this.TimeQuestList[i];
					break;
				}
			}
			this.TimeQuestList.Remove(timeQuestStatus);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x000618C4 File Offset: 0x0005FAC4
		public void RemoveCollectionQuest(string strQuestID)
		{
			QuestStatus questStatus = null;
			for (int i = 0; i < this.QuestList.Count; i++)
			{
				if (this.QuestList[i].m_strQuestID == strQuestID)
				{
					questStatus = this.QuestList[i];
					break;
				}
			}
			if (questStatus != null)
			{
				this.QuestList.Remove(questStatus);
			}
			else
			{
				this.RemoveTimeQuest(strQuestID);
			}
			if (this.Notify != null)
			{
				this.Notify.Invoke();
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00061954 File Offset: 0x0005FB54
		public bool CheckQuest(string strQuestID)
		{
			QuestStatus questStatusNode = this.GetQuestStatusNode(strQuestID);
			if (questStatusNode != null)
			{
				return questStatusNode.iType.Equals(0);
			}
			TimeQuestStatus timeStatusNode = this.GetTimeStatusNode(strQuestID);
			return timeStatusNode != null && timeStatusNode.iType.Equals(0);
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x000619A8 File Offset: 0x0005FBA8
		public bool CheckCollectionQuest(string strQuestID)
		{
			QuestStatus questStatusNode = this.GetQuestStatusNode(strQuestID);
			return questStatusNode != null && questStatusNode.iType.Equals(1);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x000619DC File Offset: 0x0005FBDC
		public bool CheckFailQuest(string strQuestID)
		{
			QuestStatus questStatusNode = this.GetQuestStatusNode(strQuestID);
			return questStatusNode != null && questStatusNode.iType.Equals(2);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00061A10 File Offset: 0x0005FC10
		public void Time2Check()
		{
			List<TimeQuestStatus> list = new List<TimeQuestStatus>();
			for (int i = 0; i < this.TimeQuestList.Count; i++)
			{
				if (this.CheckLimitQuestIsNoOverTime(this.TimeQuestList[i]))
				{
					list.Add(this.TimeQuestList[i]);
					if (!this.CheckCollectionQuest(this.TimeQuestList[i].m_strQuestID))
					{
						QuestNode questNode = Game.QuestData.GetQuestNode(this.TimeQuestList[i].m_strQuestID);
						if (!questNode.m_bRepeat)
						{
							QuestStatus questStatus = new QuestStatus();
							questStatus.m_strQuestID = this.TimeQuestList[i].m_strQuestID;
							questStatus.iType = 2;
							this.QuestList.Insert(0, questStatus);
						}
						this.FailToNPCgetReward(questNode);
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				this.TimeQuestList.Remove(list[j]);
			}
			if (this.Notify != null)
			{
				this.Notify.Invoke();
			}
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00061B30 File Offset: 0x0005FD30
		public TimeQuestStatus CheckTimelist(string questid)
		{
			for (int i = 0; i < this.TimeQuestList.Count; i++)
			{
				if (questid == this.TimeQuestList[i].m_strQuestID)
				{
					return this.TimeQuestList[i];
				}
			}
			return null;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00061B84 File Offset: 0x0005FD84
		private void FailToNPCgetReward(QuestNode QN)
		{
			if (GameGlobal.m_bDLCMode)
			{
				return;
			}
			CharacterData characterData = null;
			for (int i = 0; i < QN.m_LimitQuest.m_iNpcidEndList.Count; i++)
			{
				int num = QN.m_LimitQuest.m_iNpcidEndList[i];
				characterData = NPC.m_instance.GetCharacterData(num);
				if (characterData == null)
				{
					Debug.LogError("Quest Data Limite endnpc 找不到 CharacterData : " + num);
				}
				else if (characterData.NpcType != eNPCType.Teammate)
				{
					break;
				}
			}
			NpcQuestNode npcquest = Game.NpcQuestData.GetNPCQuest(QN.m_LimitQuest.m_strNpcQuest);
			if (npcquest != null && characterData != null)
			{
				if (characterData.NpcType == eNPCType.Teammate)
				{
					return;
				}
				NpcRandomEvent.DoNpcQuestReward(characterData, npcquest.m_NpcRewardList);
				characterData.FinishQuestList.Add(QN.m_LimitQuest.m_strNpcQuest);
				string text = npcquest.m_strNote;
				text = string.Format(text, characterData._NpcDataNode.m_strNpcName);
				Rumor rumor = new Rumor(characterData._NpcDataNode.m_strBigHeadImage, characterData._NpcDataNode.m_strNpcName, text, npcquest.m_NpcLines);
				SaveRumor sr = new SaveRumor(characterData.iNpcID, npcquest.m_strQuestID, SaveRumor.RumorType.NpcQuset);
				Game.UI.Get<UIRumor>().AddStrMsg(rumor, sr);
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00061CC8 File Offset: 0x0005FEC8
		public bool CheckQuestlist(string strQuestID)
		{
			foreach (QuestStatus questStatus in this.QuestList)
			{
				if (questStatus.m_strQuestID == strQuestID)
				{
					return true;
				}
			}
			foreach (TimeQuestStatus timeQuestStatus in this.TimeQuestList)
			{
				if (timeQuestStatus.m_strQuestID == strQuestID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00061D94 File Offset: 0x0005FF94
		public List<QuestNode> ReturnQuest(int iType)
		{
			List<QuestNode> list = new List<QuestNode>();
			for (int i = 0; i < this.QuestList.Count; i++)
			{
				QuestNode questNode = Game.QuestData.GetQuestNode(this.QuestList[i].m_strQuestID);
				if (questNode != null)
				{
					if (questNode.m_eType == QuestNode.eQuestType.ShowUI)
					{
						MissionStatus.QuestType iType2 = (MissionStatus.QuestType)this.QuestList[i].iType;
						if (iType2 == (MissionStatus.QuestType)iType)
						{
							list.Add(questNode);
						}
					}
				}
			}
			if (iType == 0)
			{
				for (int j = 0; j < this.TimeQuestList.Count; j++)
				{
					QuestNode questNode2 = Game.QuestData.GetQuestNode(this.TimeQuestList[j].m_strQuestID);
					if (questNode2 != null)
					{
						if (questNode2.m_eType == QuestNode.eQuestType.ShowUI)
						{
							MissionStatus.QuestType iType3 = (MissionStatus.QuestType)this.TimeQuestList[j].iType;
							if (iType3 == (MissionStatus.QuestType)iType)
							{
								list.Add(questNode2);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x000091FC File Offset: 0x000073FC
		public void setNewQuestCount(int iCount)
		{
			this.m_iNew = iCount;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00061E9C File Offset: 0x0006009C
		public List<TimeQuestStatus> GetTimeQuestSaveDataList()
		{
			List<TimeQuestStatus> list = new List<TimeQuestStatus>();
			for (int i = 0; i < this.TimeQuestList.Count; i++)
			{
				if (this.TimeQuestList[i] != null)
				{
					if (this.TimeQuestList[i].m_strQuestID != null)
					{
						list.Add(this.TimeQuestList[i].Clone());
					}
				}
			}
			return list;
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00061F14 File Offset: 0x00060114
		public void LoadQuestList(List<QuestStatus> LoadQuest)
		{
			this.QuestList.Clear();
			if (LoadQuest == null)
			{
				return;
			}
			for (int i = 0; i < LoadQuest.Count; i++)
			{
				this.QuestList.Add(LoadQuest[i].Clone());
			}
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00061F64 File Offset: 0x00060164
		public void LoadTimeQuestList(List<TimeQuestStatus> LoadTimeQuest)
		{
			this.TimeQuestList.Clear();
			if (LoadTimeQuest == null)
			{
				return;
			}
			for (int i = 0; i < LoadTimeQuest.Count; i++)
			{
				this.TimeQuestList.Add(LoadTimeQuest[i].Clone());
			}
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00009205 File Offset: 0x00007405
		public int GetNewQuestAmount()
		{
			return this.m_iNew;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00061FB4 File Offset: 0x000601B4
		public QuestNode RandomBigMapEvent()
		{
			List<QuestNode> list = new List<QuestNode>();
			List<QuestNode> list2 = new List<QuestNode>();
			list.Clear();
			foreach (KeyValuePair<string, QuestNode> keyValuePair in QuestDataManager.Singleton.m_BigMapQuestList)
			{
				QuestNode value = keyValuePair.Value;
				bool flag = this.CheckCollectionQuest(value.m_strQuestID);
				if (!flag)
				{
					if (!(this.BigMapLastQuest == value.m_strQuestID))
					{
						bool flag2 = this.CheckPriority(value.m_QuestOpenNodeList);
						bool flag3 = ConditionManager.CheckCondition(value.m_QuestOpenNodeList, true, 0, string.Empty);
						if (flag3)
						{
							if (flag2)
							{
								list2.Add(value);
							}
							else
							{
								list.Add(value);
							}
						}
					}
				}
			}
			if (list2.Count != 0)
			{
				int num = Random.Range(0, list2.Count);
				this.BigMapLastQuest = list2[num].m_strQuestID;
				return list2[num];
			}
			if (list.Count == 0)
			{
				return null;
			}
			int num2 = Random.Range(0, list.Count);
			this.BigMapLastQuest = list[num2].m_strQuestID;
			return list[num2];
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00062110 File Offset: 0x00060310
		public bool CheckPriority(List<Condition> _ConditionList)
		{
			for (int i = 0; i < _ConditionList.Count; i++)
			{
				if (_ConditionList[i]._iType == ConditionKind.RegionalStability)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0006214C File Offset: 0x0006034C
		private QuestNode RandomRomurEvent()
		{
			List<QuestNode> list = new List<QuestNode>();
			list.Clear();
			foreach (KeyValuePair<string, QuestNode> keyValuePair in QuestDataManager.Singleton.m_RumorQuestList)
			{
				QuestNode value = keyValuePair.Value;
				bool flag = this.CheckQuestlist(value.m_strQuestID);
				if (!flag)
				{
					if (!(this.RandomLastQuest == value.m_strQuestID))
					{
						bool flag2 = ConditionManager.CheckCondition(value.m_QuestOpenNodeList, true, 0, string.Empty);
						if (flag2)
						{
							list.Add(value);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			int num = Random.Range(0, list.Count);
			this.RandomLastQuest = list[num].m_strQuestID;
			return list[num];
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x00062244 File Offset: 0x00060444
		public void RandomRomurQuest()
		{
			if (GameGlobal.m_bDLCMode)
			{
				return;
			}
			QuestNode questNode = this.RandomRomurEvent();
			if (questNode == null)
			{
				return;
			}
			if (this.AddQuestList(questNode.m_strQuestID))
			{
				string name = string.Empty;
				string imageId = string.Empty;
				if (questNode.m_LimitQuest.m_iNpcCall == 0)
				{
					int num = Random.Range(0, 3);
					name = Game.StringTable.GetString(500001 + num);
					imageId = "B000001";
				}
				else
				{
					name = Game.NpcData.GetNpcName(questNode.m_LimitQuest.m_iNpcCall);
					imageId = Game.NpcData.GetBigHeadName(questNode.m_LimitQuest.m_iNpcCall);
				}
				Rumor rumor = new Rumor(imageId, name, questNode.m_strQuestName, questNode.m_strQuestTip);
				SaveRumor sr = new SaveRumor(0, questNode.m_strQuestID, SaveRumor.RumorType.Quest);
				Game.UI.Get<UIRumor>().AddStrMsg(rumor, sr);
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0000917B File Offset: 0x0000737B
		public void SetNotify(Action action)
		{
			this.Notify = (Action)Delegate.Combine(this.Notify, action);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00009194 File Offset: 0x00007394
		public void RemoveNotify(Action action)
		{
			this.Notify = (Action)Delegate.Remove(this.Notify, action);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00062320 File Offset: 0x00060520
		public List<QuestStatus> GetQuestSaveDataList()
		{
			List<QuestStatus> list = new List<QuestStatus>();
			for (int i = 0; i < this.QuestList.Count; i++)
			{
				list.Add(this.QuestList[i].Clone());
			}
			return list;
		}

		// Token: 0x04000D7C RID: 3452
		public static MissionStatus m_instance;

		// Token: 0x04000D7D RID: 3453
		public List<QuestStatus> QuestList = new List<QuestStatus>();

		// Token: 0x04000D7E RID: 3454
		private List<TimeQuestStatus> TimeQuestList = new List<TimeQuestStatus>();

		// Token: 0x04000D7F RID: 3455
		private string RandomLastQuest = string.Empty;

		// Token: 0x04000D80 RID: 3456
		private string BigMapLastQuest = string.Empty;

		// Token: 0x04000D81 RID: 3457
		private int m_iNew;

		// Token: 0x0200027C RID: 636
		private enum QuestType
		{
			// Token: 0x04000D84 RID: 3460
			Ing,
			// Token: 0x04000D85 RID: 3461
			Finish,
			// Token: 0x04000D86 RID: 3462
			Fail
		}
	}
}
