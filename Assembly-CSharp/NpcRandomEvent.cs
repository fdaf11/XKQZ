using System;
using System.Collections.Generic;
using Heluo.Wulin;
using Heluo.Wulin.UI;
using UnityEngine;

// Token: 0x020002C6 RID: 710
public class NpcRandomEvent : MonoBehaviour
{
	// Token: 0x06000E0E RID: 3598 RVA: 0x00073718 File Offset: 0x00071918
	private static int[] getActionNpcIndex()
	{
		if (NPC.m_instance == null)
		{
			return null;
		}
		int count = Game.NpcRandomEventData.m_NpcRandomList.Count;
		int num = GameGlobal.m_iBattleDifficulty * 3;
		if (count == 0)
		{
			return null;
		}
		int[] array = new int[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = Random.Range(0, count);
			if (NpcRandomEvent.BeDoThings == null || !NpcRandomEvent.BeDoThings.Contains(i))
			{
				for (int j = 0; j < i; j++)
				{
					while (array[j] == array[i])
					{
						j = 0;
						array[i] = Random.Range(0, count);
					}
				}
			}
		}
		return array;
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x000737D0 File Offset: 0x000719D0
	public static void SetNpcListDoSomething()
	{
		if (YoungHeroTime.m_instance != null)
		{
			YoungHeroTime.m_instance.AddRound(1);
		}
		if (NPC.m_instance == null)
		{
			return;
		}
		if (GameGlobal.m_bDLCMode)
		{
			TeamStatus.m_Instance.DLC_TeamRecover();
			TeamStatus.m_Instance.UpdateNowLevel();
			return;
		}
		int[] actionNpcIndex = NpcRandomEvent.getActionNpcIndex();
		if (actionNpcIndex == null)
		{
			return;
		}
		if (NpcRandomEvent.BeDoThings == null)
		{
			NpcRandomEvent.BeDoThings = new List<int>();
		}
		NpcRandomEvent.BeDoThings.Clear();
		NpcRandomEvent.BeDoThings.AddRange(actionNpcIndex);
		foreach (int num in actionNpcIndex)
		{
			CharacterData characterData = NPC.m_instance.GetCharacterData(Game.NpcRandomEventData.m_NpcRandomList[num].NpcID);
			if (characterData.NpcType != eNPCType.Teammate && characterData.NpcType != eNPCType.NothingCanDo)
			{
				NpcRandomEvent.SetNpcDoSomething(characterData);
			}
		}
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x000738BC File Offset: 0x00071ABC
	public static void SetNpcDoSomething(CharacterData CD)
	{
		if (GameGlobal.m_bDLCMode)
		{
			return;
		}
		string text = string.Empty;
		NpcRandomGroup npcRandomNode = Game.NpcRandomEventData.GetNpcRandomNode(CD.iNpcID);
		if (npcRandomNode == null)
		{
			return;
		}
		if (CD.NpcType == eNPCType.NothingCanDo)
		{
			return;
		}
		if (CD.NpcType == eNPCType.DoSomething)
		{
			bool flag = YoungHeroTime.m_instance.CheackRound(eCompareType.GreaterOrEqual, CD.iFinishTime);
			if (!flag)
			{
				return;
			}
			text = CD.strNowDoQuest;
			CD.NpcType = eNPCType.Nothing;
			NpcQuestNode npcquest = Game.NpcQuestData.GetNPCQuest(CD.strNowDoQuest);
			if (!npcquest.m_bOnly && !CD.FinishQuestList.Contains(CD.strNowDoQuest))
			{
				CD.FinishQuestList.Add(CD.strNowDoQuest);
			}
			NpcRandomEvent.DoNpcQuestReward(CD, npcquest.m_NpcRewardList);
			if (npcquest.m_bShow)
			{
				string text2 = npcquest.m_strNote;
				text2 = string.Format(text2, CD._NpcDataNode.m_strNpcName);
				Rumor rumor = new Rumor(CD._NpcDataNode.m_strBigHeadImage, CD._NpcDataNode.m_strNpcName, text2, npcquest.m_NpcLines);
				SaveRumor sr = new SaveRumor(CD.iNpcID, npcquest.m_strQuestID, SaveRumor.RumorType.NpcQuset);
				Game.UI.Get<UIRumor>().AddStrMsg(rumor, sr);
			}
			if (CD.iMoney >= NpcRandomEvent.checkmoney)
			{
				NpcRandomEvent.BuyItem(CD);
			}
			CD.strNowDoQuest = string.Empty;
		}
		bool flag2 = true;
		if (CD.NpcType == eNPCType.Nothing)
		{
			bool flag3 = false;
			string text3 = string.Empty;
			if (flag2)
			{
				int num = 0;
				NpcRandomEvent.WeightsList.Clear();
				for (int i = 0; i < npcRandomNode.m_NpcRandomEvent.Count; i++)
				{
					bool flag4 = NpcRandomEvent.CheckCollectQuestOpen(npcRandomNode.m_NpcRandomEvent[i].m_strStartQuest, true);
					bool flag5 = NpcRandomEvent.CheckCollectQuestOpen(npcRandomNode.m_NpcRandomEvent[i].m_strOverQuest, false);
					if (flag4 && !flag5)
					{
						flag3 = false;
						for (int j = 0; j < npcRandomNode.m_NpcRandomEvent[i].m_ReandomEventList.Count; j++)
						{
							string strQuestID = npcRandomNode.m_NpcRandomEvent[i].m_ReandomEventList[j].m_strQuestID;
							if (text != strQuestID)
							{
								NpcQuestNode npcquest2 = Game.NpcQuestData.GetNPCQuest(strQuestID);
								if (npcquest2 == null)
								{
									Debug.Log(npcRandomNode.NpcID + "NPCQuest 沒有   " + strQuestID);
								}
								else if (!npcquest2.m_bOnly || !NpcRandomEvent.g_NpcQuestList.Contains(npcquest2.m_strQuestID))
								{
									if (Game.NpcQuestData.CheckNPCCondition(CD.iNpcID, npcquest2.m_NpcConditionList))
									{
										num += npcRandomNode.m_NpcRandomEvent[i].m_ReandomEventList[j].m_iWeights;
										if (NpcRandomEvent.WeightsList.ContainsKey(npcRandomNode.m_NpcRandomEvent[i].m_ReandomEventList[j].m_strQuestID))
										{
											Debug.LogError(npcRandomNode.NpcID + "  重複 ID : " + npcRandomNode.m_NpcRandomEvent[i].m_ReandomEventList[j].m_strQuestID);
										}
										else
										{
											NpcRandomEvent.WeightsList.Add(npcRandomNode.m_NpcRandomEvent[i].m_ReandomEventList[j].m_strQuestID, npcRandomNode.m_NpcRandomEvent[i].m_ReandomEventList[j].m_iWeights);
										}
									}
								}
							}
						}
					}
					else if (flag5)
					{
						flag3 = true;
					}
				}
				if (!flag3 && NpcRandomEvent.WeightsList.Count > 0)
				{
					int num2 = SimpleRandom.Range(0, num);
					foreach (KeyValuePair<string, int> keyValuePair in NpcRandomEvent.WeightsList)
					{
						if (num2 <= keyValuePair.Value)
						{
							text3 = keyValuePair.Key;
							break;
						}
						num2 -= keyValuePair.Value;
					}
				}
			}
			if (!flag3)
			{
				CD.strNowDoQuest = text3;
				NpcQuestNode npcquest3 = Game.NpcQuestData.GetNPCQuest(text3);
				if (npcquest3 == null)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"NPCRandomEvent 表    ",
						CD.iNpcID,
						"    ",
						text3,
						" ",
						NpcRandomEvent.WeightsList.Count
					}));
					return;
				}
				CD.iFinishTime = YoungHeroTime.m_instance.AddCheckRound(npcquest3.m_iRound);
				CD.NpcType = eNPCType.DoSomething;
				if (npcquest3.m_bOnly)
				{
					NpcRandomEvent.g_NpcQuestList.Add(text3);
				}
				NpcRandomEvent.WeightsList.Clear();
				text = string.Empty;
			}
			else
			{
				CD.NpcType = eNPCType.NothingCanDo;
				GameDebugTool.Log(CD._NpcDataNode.m_strNpcName + " 沒事做了");
			}
		}
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x00073DFC File Offset: 0x00071FFC
	private static void BuyItem(CharacterData CD)
	{
		if (CD == null)
		{
			return;
		}
		List<int> buyList = Game.NpcBuyItem.GetBuyList(CD.iNpcID);
		if (buyList != null && buyList.Count >= 0)
		{
			int num = Random.Range(0, buyList.Count);
			ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(buyList[num]);
			if (itemDataNode == null)
			{
				return;
			}
			CD.iMoney -= Mathf.FloorToInt((float)itemDataNode.m_iItemBuy * 0.8f);
			CD.AddNpcItem(itemDataNode.m_iItemID, 1);
		}
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x00073E88 File Offset: 0x00072088
	private static bool CheckCollectQuestOpen(string QuestID, bool bopen = true)
	{
		if (bopen)
		{
			if (QuestID == "0" || QuestID == "1")
			{
				return true;
			}
		}
		else if (QuestID == "0" || QuestID == "1")
		{
			return false;
		}
		return MissionStatus.m_instance.CheckCollectionQuest(QuestID);
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x00073EF0 File Offset: 0x000720F0
	public static void DoNpcQuestReward(CharacterData CD, List<NpcReward> RwardList)
	{
		foreach (NpcReward npcReward in RwardList)
		{
			switch (npcReward.m_Type)
			{
			case NpcRewardType.AddNpcProperty:
				NPC.m_instance.SetCharacterProperty(CD.iNpcID, npcReward.m_iID, npcReward.m_iValue);
				break;
			case NpcRewardType.NowPracticeExp:
				NPC.m_instance.SetNPCNowPracticeExp(CD, npcReward.m_iValue, true, 0);
				break;
			case NpcRewardType.GetNewRoutine:
				NPC.m_instance.AddRoutine(CD.iNpcID, npcReward.m_iID, 1);
				break;
			case NpcRewardType.GetNewNeigong:
				NPC.m_instance.AddNeigong(CD.iNpcID, npcReward.m_iID, 1);
				break;
			case NpcRewardType.GetNewWeapon:
				CD.SetEquip(ItemDataNode.ItemType.Weapon, npcReward.m_iID);
				break;
			case NpcRewardType.GetNewArror:
				CD.SetEquip(ItemDataNode.ItemType.Arror, npcReward.m_iID);
				break;
			case NpcRewardType.GetNewNecklace:
				CD.SetEquip(ItemDataNode.ItemType.Necklace, npcReward.m_iID);
				break;
			case NpcRewardType.GetMoney:
				CD.AddMoney(npcReward.m_iID);
				break;
			case NpcRewardType.GetItem:
				CD.AddNpcItem(npcReward.m_iID, npcReward.m_iValue);
				break;
			case NpcRewardType.AddTalent:
				CD.AddTalent(npcReward.m_iID);
				break;
			}
		}
	}

	// Token: 0x0400105B RID: 4187
	private static int checkmoney = 20000;

	// Token: 0x0400105C RID: 4188
	public static List<int> BeDoThings = new List<int>();

	// Token: 0x0400105D RID: 4189
	public static List<string> g_NpcQuestList = new List<string>();

	// Token: 0x0400105E RID: 4190
	private static Dictionary<string, int> WeightsList = new Dictionary<string, int>();
}
