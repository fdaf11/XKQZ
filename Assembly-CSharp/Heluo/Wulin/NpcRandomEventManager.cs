using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200023F RID: 575
	public class NpcRandomEventManager : TextDataManager
	{
		// Token: 0x06000AC3 RID: 2755 RVA: 0x000088A8 File Offset: 0x00006AA8
		public NpcRandomEventManager()
		{
			this.m_NpcRandomList = new List<NpcRandomGroup>();
			this.m_LoadFileName = "NpcRandomEvent";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x000088E4 File Offset: 0x00006AE4
		public static NpcRandomEventManager Singleton
		{
			get
			{
				return NpcRandomEventManager.instance;
			}
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00059B7C File Offset: 0x00057D7C
		protected override void LoadFile(string filePath)
		{
			this.m_NpcRandomList.Clear();
			string[] array = base.ExtractTextFile(filePath);
			string text = string.Empty;
			if (array == null)
			{
				return;
			}
			foreach (string text2 in array)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					if (text2.get_Chars(0) != '#')
					{
						try
						{
							string text3 = text2.Replace("\r", string.Empty);
							string[] array3 = text3.Split(new char[]
							{
								"\t".get_Chars(0)
							});
							NpcRandomNode npcRandomNode = new NpcRandomNode();
							NpcRandomGroup npcRandomGroup = this.CheckNpcRandomGroup(array3[0]);
							npcRandomNode.m_strStartQuest = array3[2];
							npcRandomNode.m_strOverQuest = array3[3];
							for (int j = 4; j < array3.Length; j += 2)
							{
								string text4 = array3[j].Trim();
								if (text4.Length > 2)
								{
									int iWeights;
									if (int.TryParse(array3[j + 1], ref iWeights))
									{
										for (int k = 0; k > npcRandomNode.m_ReandomEventList.Count; k++)
										{
											if (npcRandomNode.m_ReandomEventList[k].m_strQuestID == array3[j])
											{
											}
										}
										NpcReandomQuest npcReandomQuest = new NpcReandomQuest();
										npcReandomQuest.m_strQuestID = array3[j];
										npcReandomQuest.m_iWeights = iWeights;
										npcRandomNode.m_ReandomEventList.Add(npcReandomQuest);
									}
								}
							}
							npcRandomGroup.m_NpcRandomEvent.Add(npcRandomNode);
							text = text2;
						}
						catch
						{
							Debug.LogError(string.Concat(new string[]
							{
								"解析 ",
								filePath,
								" 時發生錯誤 : ",
								text2,
								" 上一行 : ",
								text
							}));
						}
					}
				}
			}
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00059D64 File Offset: 0x00057F64
		public NpcRandomGroup GetNpcRandomNode(int NpcID)
		{
			foreach (NpcRandomGroup npcRandomGroup in this.m_NpcRandomList)
			{
				if (npcRandomGroup.NpcID == NpcID)
				{
					return npcRandomGroup;
				}
			}
			return null;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00059DD0 File Offset: 0x00057FD0
		private NpcRandomGroup CheckNpcRandomGroup(string strID)
		{
			int num = int.Parse(strID);
			NpcRandomGroup npcRandomGroup;
			if (this.m_NpcRandomList.Count == 0)
			{
				npcRandomGroup = new NpcRandomGroup();
				npcRandomGroup.NpcID = num;
				this.m_NpcRandomList.Add(npcRandomGroup);
				return npcRandomGroup;
			}
			for (int i = 0; i < this.m_NpcRandomList.Count; i++)
			{
				if (this.m_NpcRandomList[i].NpcID.Equals(num))
				{
					return this.m_NpcRandomList[i];
				}
			}
			npcRandomGroup = new NpcRandomGroup();
			npcRandomGroup.NpcID = num;
			this.m_NpcRandomList.Add(npcRandomGroup);
			return npcRandomGroup;
		}

		// Token: 0x04000BF9 RID: 3065
		private static readonly NpcRandomEventManager instance = new NpcRandomEventManager();

		// Token: 0x04000BFA RID: 3066
		public List<NpcRandomGroup> m_NpcRandomList;
	}
}
