using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001DB RID: 475
	public class AchievementManager : TextDataManager
	{
		// Token: 0x060009B5 RID: 2485 RVA: 0x00007E0D File Offset: 0x0000600D
		public AchievementManager()
		{
			this.m_AchiKindNodeList = new List<AchievementKindNode>();
			this.m_LoadFileName = "AchievementData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x00007E49 File Offset: 0x00006049
		public static AchievementManager Singleton
		{
			get
			{
				return AchievementManager.instance;
			}
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x000520D4 File Offset: 0x000502D4
		protected override void LoadFile(string filePath)
		{
			this.m_AchiKindNodeList.Clear();
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
							AchievementKindNode achievementKindNode = this.CheckAchiKindNode(int.Parse(array3[0]));
							AchievementDataNode achievementDataNode = new AchievementDataNode();
							achievementDataNode.m_iID = int.Parse(array3[1]);
							achievementDataNode.m_strAchName = array3[2];
							achievementDataNode.m_strAchExplain = array3[3];
							achievementDataNode.m_strUIImage = array3[4];
							achievementDataNode.m_strEndMove = array3[5];
							achievementDataNode.m_iCheckType = int.Parse(array3[6]);
							achievementDataNode.m_strCheckValue = array3[7];
							achievementDataNode.m_iCount = int.Parse(array3[8]);
							achievementDataNode.m_iOpenType = RoundManager.GetAchievementState(achievementDataNode.m_iID);
							achievementDataNode.m_iNow = RoundManager.GetAchievementNow(achievementDataNode.m_iID);
							if (achievementDataNode.m_iCheckType == 5 || achievementDataNode.m_iCheckType == 4 || achievementDataNode.m_iCheckType == 3)
							{
								string[] array4 = achievementDataNode.m_strCheckValue.Split(new char[]
								{
									",".get_Chars(0)
								});
								foreach (string text2 in array4)
								{
									achievementDataNode.m_checkValueList.Add(int.Parse(text2));
								}
							}
							else if (achievementDataNode.m_iCheckType == 2 || achievementDataNode.m_iCheckType == 10)
							{
								string[] array6 = achievementDataNode.m_strCheckValue.Split(new char[]
								{
									",".get_Chars(0)
								});
								achievementDataNode.m_checkStringList.AddRange(array6);
							}
							if (achievementDataNode.m_iOpenType == 1)
							{
								achievementKindNode.m_iFinishCount++;
							}
							achievementKindNode.m_AchiDataNodeList.Add(achievementDataNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0005232C File Offset: 0x0005052C
		public AchievementKindNode CheckAchiKindNode(int iType)
		{
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				if (this.m_AchiKindNodeList[i].m_iAchiType == iType)
				{
					return this.m_AchiKindNodeList[i];
				}
			}
			AchievementKindNode achievementKindNode = new AchievementKindNode();
			achievementKindNode.m_iAchiType = iType;
			this.m_AchiKindNodeList.Add(achievementKindNode);
			return achievementKindNode;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00052394 File Offset: 0x00050594
		public AchievementKindNode GetAchiKindNode(int iType)
		{
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				if (this.m_AchiKindNodeList[i].m_iAchiType == iType)
				{
					return this.m_AchiKindNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x000523E4 File Offset: 0x000505E4
		public void SetMainEnd(int iEndID)
		{
			if (RoundManager.QueryEndNumber(iEndID))
			{
				return;
			}
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iCheckType == 0)
					{
						int num = 0;
						if (int.TryParse(achievementDataNode.m_strCheckValue, ref num))
						{
							if (num == iEndID)
							{
								achievementDataNode.m_iNow++;
								if (achievementDataNode.m_iNow >= achievementDataNode.m_iCount && achievementDataNode.m_iOpenType == 0)
								{
									achievementDataNode.m_iOpenType = 1;
									RoundManager.SetAchievementFinish(achievementDataNode.m_iID);
									RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
								}
								else
								{
									RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
								}
							}
						}
					}
				}
				for (int k = 0; k < achievementKindNode.m_AchiDataNodeList.Count; k++)
				{
					AchievementDataNode achievementDataNode2 = achievementKindNode.m_AchiDataNodeList[k];
					if (achievementDataNode2.m_iCheckType == 4)
					{
						if (achievementDataNode2.m_checkValueList.Contains(iEndID))
						{
							achievementDataNode2.m_iNow++;
							if (achievementDataNode2.m_iNow >= achievementDataNode2.m_iCount && achievementDataNode2.m_iOpenType == 0)
							{
								achievementDataNode2.m_iOpenType = 1;
								RoundManager.SetAchievementFinish(achievementDataNode2.m_iID);
								RoundManager.SetAchievementNow(achievementDataNode2.m_iID, achievementDataNode2.m_iNow);
							}
							else
							{
								RoundManager.SetAchievementNow(achievementDataNode2.m_iID, achievementDataNode2.m_iNow);
							}
						}
					}
				}
			}
			RoundManager.SetMainEnd(iEndID);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x000525B4 File Offset: 0x000507B4
		public void SetChildEnd(int iEndID)
		{
			if (RoundManager.QueryEndNumber(iEndID))
			{
				return;
			}
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iCheckType == 1)
					{
						int num = 0;
						if (int.TryParse(achievementDataNode.m_strCheckValue, ref num))
						{
							if (num == iEndID)
							{
								achievementDataNode.m_iNow++;
								if (achievementDataNode.m_iNow >= achievementDataNode.m_iCount && achievementDataNode.m_iOpenType == 0)
								{
									achievementDataNode.m_iOpenType = 1;
									RoundManager.SetAchievementFinish(achievementDataNode.m_iID);
									RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
								}
								else
								{
									RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
								}
							}
						}
					}
				}
			}
			RoundManager.SetChildEnd(iEndID);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x000526C0 File Offset: 0x000508C0
		public void SetCollectionQuest(string strCollection)
		{
			if (RoundManager.QueryCollection(strCollection))
			{
				return;
			}
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iCheckType == 2)
					{
						if (achievementDataNode.m_checkStringList.Contains(strCollection))
						{
							achievementDataNode.m_iNow++;
							if (achievementDataNode.m_iNow >= achievementDataNode.m_iCount && achievementDataNode.m_iOpenType == 0)
							{
								achievementDataNode.m_iOpenType = 1;
								RoundManager.SetAchievementFinish(achievementDataNode.m_iID);
								RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
							}
							else
							{
								RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
							}
						}
					}
				}
			}
			RoundManager.SetCollection(strCollection);
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x000527BC File Offset: 0x000509BC
		public void SetMovie(int iMovie)
		{
			if (RoundManager.QueryMovie(iMovie))
			{
				return;
			}
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iCheckType == 3)
					{
						if (achievementDataNode.m_checkValueList.Contains(iMovie))
						{
							achievementDataNode.m_iNow++;
							if (achievementDataNode.m_iNow >= achievementDataNode.m_iCount && achievementDataNode.m_iOpenType == 0)
							{
								achievementDataNode.m_iOpenType = 1;
								RoundManager.SetAchievementFinish(achievementDataNode.m_iID);
								RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
							}
							else
							{
								RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
							}
						}
					}
				}
			}
			RoundManager.SetMovie(iMovie);
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x000528B8 File Offset: 0x00050AB8
		public void SetDevelopBtn(int iBtn)
		{
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iCheckType == 5)
					{
						if (achievementDataNode.m_checkValueList.Contains(iBtn))
						{
							achievementDataNode.m_iNow++;
							if (achievementDataNode.m_iNow >= achievementDataNode.m_iCount && achievementDataNode.m_iOpenType == 0)
							{
								achievementDataNode.m_iOpenType = 1;
								RoundManager.SetAchievementFinish(achievementDataNode.m_iID);
								RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
							}
							else
							{
								RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
							}
						}
					}
				}
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000529A4 File Offset: 0x00050BA4
		public void SetTresureOpen(int iStatus, string strChestID)
		{
			if (RoundManager.QueryChest(strChestID))
			{
				return;
			}
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iCheckType == 6)
					{
						int num = 0;
						if (int.TryParse(achievementDataNode.m_strCheckValue, ref num))
						{
							if (num == iStatus)
							{
								achievementDataNode.m_iNow++;
								if (achievementDataNode.m_iNow >= achievementDataNode.m_iCount && achievementDataNode.m_iOpenType == 0)
								{
									achievementDataNode.m_iOpenType = 1;
									RoundManager.SetAchievementFinish(achievementDataNode.m_iID);
									RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
								}
								else
								{
									RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
								}
							}
						}
					}
				}
			}
			RoundManager.SetChest(strChestID);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00052AB0 File Offset: 0x00050CB0
		public void SetMinningAllGet()
		{
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iCheckType == 7)
					{
						achievementDataNode.m_iNow++;
						if (achievementDataNode.m_iNow >= achievementDataNode.m_iCount && achievementDataNode.m_iOpenType == 0)
						{
							achievementDataNode.m_iOpenType = 1;
							RoundManager.SetAchievementFinish(achievementDataNode.m_iID);
							RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
						}
						else
						{
							RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
						}
					}
				}
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00052B84 File Offset: 0x00050D84
		public void SetDevelopQuest(string strDevelop)
		{
			if (RoundManager.QueryDevelop(strDevelop))
			{
				return;
			}
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iCheckType == 10)
					{
						if (achievementDataNode.m_checkStringList.Contains(strDevelop))
						{
							achievementDataNode.m_iNow++;
							if (achievementDataNode.m_iNow >= achievementDataNode.m_iCount && achievementDataNode.m_iOpenType == 0)
							{
								achievementDataNode.m_iOpenType = 1;
								RoundManager.SetAchievementFinish(achievementDataNode.m_iID);
								RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
							}
							else
							{
								RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
							}
						}
					}
				}
			}
			RoundManager.SetDevelop(strDevelop);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00052C80 File Offset: 0x00050E80
		public void SetTitle(int title)
		{
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iCheckType == 11)
					{
						int num = 0;
						if (int.TryParse(achievementDataNode.m_strCheckValue, ref num))
						{
							if (num == title)
							{
								achievementDataNode.m_iNow++;
								if (achievementDataNode.m_iNow >= achievementDataNode.m_iCount && achievementDataNode.m_iOpenType == 0)
								{
									achievementDataNode.m_iOpenType = 1;
									RoundManager.SetAchievementFinish(achievementDataNode.m_iID);
									RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
								}
								else
								{
									RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00052D7C File Offset: 0x00050F7C
		public void SetNoHaveGFriend(int iEndID, List<int> childEndList)
		{
			if (RoundManager.QueryEndNumber(iEndID))
			{
				return;
			}
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iCheckType == 12)
					{
						if (!achievementDataNode.m_checkValueList.Contains(iEndID))
						{
							bool flag = false;
							for (int k = 0; k < childEndList.Count; k++)
							{
								if (achievementDataNode.m_checkValueList.Contains(childEndList[k]))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								achievementDataNode.m_iNow++;
								if (achievementDataNode.m_iNow >= achievementDataNode.m_iCount && achievementDataNode.m_iOpenType == 0)
								{
									achievementDataNode.m_iOpenType = 1;
									RoundManager.SetAchievementFinish(achievementDataNode.m_iID);
									RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
								}
								else
								{
									RoundManager.SetAchievementNow(achievementDataNode.m_iID, achievementDataNode.m_iNow);
								}
							}
						}
					}
				}
			}
			RoundManager.SetMainEnd(iEndID);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00052EC4 File Offset: 0x000510C4
		public int GetAchievementCount(int iID)
		{
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iID == iID)
					{
						return achievementDataNode.m_iCount;
					}
				}
			}
			return 0;
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00052F38 File Offset: 0x00051138
		public int GetAchievementOpen(int iID)
		{
			for (int i = 0; i < this.m_AchiKindNodeList.Count; i++)
			{
				AchievementKindNode achievementKindNode = this.m_AchiKindNodeList[i];
				for (int j = 0; j < achievementKindNode.m_AchiDataNodeList.Count; j++)
				{
					AchievementDataNode achievementDataNode = achievementKindNode.m_AchiDataNodeList[j];
					if (achievementDataNode.m_iID == iID)
					{
						return achievementDataNode.m_iOpenType;
					}
				}
			}
			return 0;
		}

		// Token: 0x040009B8 RID: 2488
		private static readonly AchievementManager instance = new AchievementManager();

		// Token: 0x040009B9 RID: 2489
		private List<AchievementKindNode> m_AchiKindNodeList;
	}
}
