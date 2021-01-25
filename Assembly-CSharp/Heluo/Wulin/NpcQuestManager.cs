using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200023B RID: 571
	public class NpcQuestManager : TextDataManager
	{
		// Token: 0x06000AB9 RID: 2745 RVA: 0x0000883F File Offset: 0x00006A3F
		public NpcQuestManager()
		{
			this.m_NpcQuestList = new List<NpcQuestNode>();
			this.m_LoadFileName = "NpcQuest";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x0000887B File Offset: 0x00006A7B
		public static NpcQuestManager Singleton
		{
			get
			{
				return NpcQuestManager.instance;
			}
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0005983C File Offset: 0x00057A3C
		protected override void LoadFile(string filePath)
		{
			this.m_NpcQuestList.Clear();
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
							NpcQuestNode npcQuestNode = new NpcQuestNode();
							for (int j = 0; j < array3.Length; j++)
							{
								this.generateData(j, array3[j].Trim(), npcQuestNode);
							}
							this.m_NpcQuestList.Add(npcQuestNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00059930 File Offset: 0x00057B30
		private void generateData(int idx, string data, NpcQuestNode node)
		{
			switch (idx)
			{
			case 0:
				node.m_strQuestID = data;
				break;
			case 1:
				if (data == "1")
				{
					node.m_bShow = true;
				}
				else
				{
					node.m_bShow = false;
				}
				break;
			case 2:
				Condition.GenerateList(node.m_NpcConditionList, data);
				break;
			case 3:
			{
				int iRound = 0;
				if (!int.TryParse(data, ref iRound))
				{
					iRound = 0;
				}
				node.m_iRound = iRound;
				break;
			}
			case 4:
				NpcReward.GenerateList(node.m_NpcRewardList, data);
				break;
			case 5:
				node.m_strNote = data;
				break;
			case 6:
				node.m_NpcLines = data;
				break;
			case 7:
				data = data.Replace("\r", string.Empty);
				if (data == "1")
				{
					node.m_bOnly = true;
				}
				else
				{
					node.m_bOnly = false;
				}
				break;
			}
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00059A34 File Offset: 0x00057C34
		public NpcQuestNode GetNPCQuest(string QuestID)
		{
			foreach (NpcQuestNode npcQuestNode in this.m_NpcQuestList)
			{
				if (npcQuestNode.m_strQuestID == QuestID)
				{
					return npcQuestNode;
				}
			}
			return null;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00059AA4 File Offset: 0x00057CA4
		public bool CheckNPCCondition(int NpcID, List<Condition> ConditionList)
		{
			if (ConditionList.Count == 0)
			{
				return true;
			}
			List<Condition> list = new List<Condition>();
			int i = 0;
			while (i < ConditionList.Count)
			{
				Condition condition = ConditionList[i].Clone();
				ConditionKind iType = condition._iType;
				switch (iType)
				{
				case ConditionKind.MoreNpcRoutine:
				case ConditionKind.MoreNpcNeigong:
				case ConditionKind.MoreNpcProperty:
				case ConditionKind.LessNpcProperty:
				case ConditionKind.LessNpcRoutine:
				case ConditionKind.LessNpcNeigong:
					goto IL_89;
				default:
					switch (iType)
					{
					case ConditionKind.NpcMoney:
					case ConditionKind.MoreNpcItem:
					case ConditionKind.LessNpcItem:
						goto IL_89;
					case ConditionKind.NpcQuest:
						condition.m_iValue = NpcID;
						break;
					default:
						if (iType == ConditionKind.CheckSkillMax)
						{
							goto IL_89;
						}
						break;
					}
					break;
				}
				IL_A6:
				list.Add(condition);
				i++;
				continue;
				IL_89:
				condition.m_Parameter = NpcID;
				goto IL_A6;
			}
			return ConditionManager.CheckCondition(list, true, 0, string.Empty);
		}

		// Token: 0x04000BEE RID: 3054
		private static readonly NpcQuestManager instance = new NpcQuestManager();

		// Token: 0x04000BEF RID: 3055
		public List<NpcQuestNode> m_NpcQuestList;
	}
}
