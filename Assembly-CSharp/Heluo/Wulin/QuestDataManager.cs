using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000244 RID: 580
	public class QuestDataManager : TextDataManager
	{
		// Token: 0x06000ACC RID: 2764 RVA: 0x00059F28 File Offset: 0x00058128
		public QuestDataManager()
		{
			this.m_QuestNodeList = new Dictionary<string, QuestNode>();
			this.m_BigMapQuestList = new Dictionary<string, QuestNode>();
			this.m_MouseQuestList = new Dictionary<string, QuestNode>();
			this.m_RumorQuestList = new Dictionary<string, QuestNode>();
			this.m_LoadFileName = "QuestDataManager";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x00008933 File Offset: 0x00006B33
		public static QuestDataManager Singleton
		{
			get
			{
				return QuestDataManager.instance;
			}
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00059F84 File Offset: 0x00058184
		protected override void LoadFile(string filePath)
		{
			this.m_QuestNodeList.Clear();
			this.m_BigMapQuestList.Clear();
			this.m_MouseQuestList.Clear();
			this.m_RumorQuestList.Clear();
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
							QuestNode questNode = new QuestNode();
							questNode.m_strQuestID = array3[0];
							questNode.m_strQuestName = array3[1];
							questNode.m_eType = (QuestNode.eQuestType)int.Parse(array3[2]);
							questNode.m_strQuestTip = array3[3];
							if (array3[4].Length > 2)
							{
								string[] array4 = array3[4].Split(new char[]
								{
									",".get_Chars(0)
								});
								for (int j = 0; j < array4.Length; j++)
								{
									LimitQuestNode.GetData(j, questNode.m_LimitQuest, array4[j]);
								}
							}
							else
							{
								questNode.m_LimitQuest.m_iIsLimit = int.Parse(array3[4]);
							}
							string text2 = array3[5].Replace(")*(", "*");
							if (text2.Length > 2)
							{
								text2 = text2.Substring(1, text2.Length - 2);
							}
							string[] array5 = text2.Split(new char[]
							{
								"*".get_Chars(0)
							});
							for (int k = 0; k < array5.Length; k++)
							{
								string[] array6 = array5[k].Split(new char[]
								{
									",".get_Chars(0)
								});
								if (array6.Length > 2)
								{
									Condition condition = new Condition(array6);
									questNode.m_QuestOpenNodeList.Add(condition);
								}
							}
							text2 = array3[6].Replace(")*(", "*");
							if (text2.Length > 2)
							{
								text2 = text2.Substring(1, text2.Length - 2);
							}
							array5 = text2.Split(new char[]
							{
								"*".get_Chars(0)
							});
							for (int k = 0; k < array5.Length; k++)
							{
								string[] array7 = array5[k].Split(new char[]
								{
									",".get_Chars(0)
								});
								if (array7.Length > 1)
								{
									Condition condition2 = new Condition(array7);
									questNode.m_FinshQuestNodeList.Add(condition2);
								}
							}
							questNode.m_strGetManager = array3[7];
							questNode.m_strQuestIngManager = array3[8];
							questNode.m_strFinshQuestManager = array3[9];
							questNode.m_iGiftID = int.Parse(array3[10]);
							string text3 = array3[11].Replace("\r", string.Empty);
							if (text3 == "1")
							{
								questNode.m_bRepeat = true;
							}
							else
							{
								questNode.m_bRepeat = false;
							}
							if (questNode.m_eType == QuestNode.eQuestType.MouseEvent)
							{
								this.AddNodeToList(this.m_MouseQuestList, questNode.m_strQuestID, questNode);
							}
							else if (questNode.m_eType == QuestNode.eQuestType.BigMap)
							{
								this.AddNodeToList(this.m_BigMapQuestList, questNode.m_strQuestID, questNode);
							}
							else if (questNode.m_LimitQuest.m_iIsLimit == 1)
							{
								this.AddNodeToList(this.m_RumorQuestList, questNode.m_strQuestID, questNode);
							}
							else
							{
								this.AddNodeToList(this.m_QuestNodeList, questNode.m_strQuestID, questNode);
							}
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0000893A File Offset: 0x00006B3A
		private void AddNodeToList(Dictionary<string, QuestNode> list, string key, QuestNode node)
		{
			if (!list.ContainsKey(key))
			{
				list.Add(key, node);
			}
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0005A35C File Offset: 0x0005855C
		public QuestNode GetQuestNode(string strID)
		{
			if (strID == null)
			{
				return null;
			}
			if (this.m_QuestNodeList.ContainsKey(strID))
			{
				return this.m_QuestNodeList[strID];
			}
			if (this.m_BigMapQuestList.ContainsKey(strID))
			{
				return this.m_BigMapQuestList[strID];
			}
			if (this.m_MouseQuestList.ContainsKey(strID))
			{
				return this.m_MouseQuestList[strID];
			}
			if (this.m_RumorQuestList.ContainsKey(strID))
			{
				return this.m_RumorQuestList[strID];
			}
			return null;
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0005A3EC File Offset: 0x000585EC
		public string GetQuestInfo(string strID)
		{
			QuestNode questNode = this.GetQuestNode(strID);
			if (questNode != null)
			{
				return string.Concat(new string[]
				{
					strID,
					" ",
					questNode.m_strQuestName,
					" ",
					questNode.m_strQuestTip
				});
			}
			return strID;
		}

		// Token: 0x04000C17 RID: 3095
		private static readonly QuestDataManager instance = new QuestDataManager();

		// Token: 0x04000C18 RID: 3096
		public Dictionary<string, QuestNode> m_QuestNodeList;

		// Token: 0x04000C19 RID: 3097
		public Dictionary<string, QuestNode> m_BigMapQuestList;

		// Token: 0x04000C1A RID: 3098
		public Dictionary<string, QuestNode> m_MouseQuestList;

		// Token: 0x04000C1B RID: 3099
		public Dictionary<string, QuestNode> m_RumorQuestList;
	}
}
