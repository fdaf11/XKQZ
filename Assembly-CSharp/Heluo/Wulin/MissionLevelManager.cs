using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000222 RID: 546
	public class MissionLevelManager : TextDataManager
	{
		// Token: 0x06000A6C RID: 2668 RVA: 0x0000850C File Offset: 0x0000670C
		public MissionLevelManager()
		{
			this.m_MissionLevelNodeList = new List<MissionLevelNode>();
			this.m_LoadFileName = "MissionLevel";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x00008548 File Offset: 0x00006748
		public static MissionLevelManager Singleton
		{
			get
			{
				return MissionLevelManager.instance;
			}
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00058148 File Offset: 0x00056348
		protected override void LoadFile(string filePath)
		{
			this.m_MissionLevelNodeList.Clear();
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
						if (text.get_Chars(0) != '\r')
						{
							try
							{
								string[] array3 = text.Trim().Split(new char[]
								{
									"\t".get_Chars(0)
								});
								MissionLevelNode missionLevelNode = new MissionLevelNode();
								for (int j = 0; j < array3.Length; j++)
								{
									this.GreatData(j, array3[j], missionLevelNode);
								}
								this.m_MissionLevelNodeList.Add(missionLevelNode);
							}
							catch (Exception ex)
							{
								Debug.LogError(string.Concat(new string[]
								{
									"解析 ",
									filePath,
									" 時發生錯誤 : ",
									text,
									"   "
								}));
								Debug.LogError(ex.Message);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00058278 File Offset: 0x00056478
		private void GreatData(int idx, string data, MissionLevelNode _node)
		{
			switch (idx)
			{
			case 0:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iLevelID = num;
				break;
			}
			case 1:
				_node.strName = data;
				break;
			case 2:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iBattleAreaID = num;
				break;
			}
			case 3:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iType = num;
				break;
			}
			case 4:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iGroup = num;
				break;
			}
			case 5:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iRepeat = num;
				break;
			}
			case 6:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iWeights = num;
				break;
			}
			case 7:
				Condition.GenerateList(_node.m_DisplayConditionList, data);
				break;
			case 8:
				Condition.GenerateList(_node.m_EnterConditionList, data);
				break;
			case 9:
				Condition.GenerateList(_node.m_CloseConditionList, data);
				break;
			case 10:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iRound = num;
				break;
			}
			case 11:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iMissionPosition = num;
				break;
			}
			case 12:
				_node.strDesc = data;
				break;
			case 13:
			{
				string[] array = data.Split(new char[]
				{
					",".get_Chars(0)
				});
				foreach (string text in array)
				{
					int num;
					if (!int.TryParse(text, ref num))
					{
						num = 0;
					}
					if (num != 0)
					{
						_node.m_ShaqList.Add(num);
					}
				}
				break;
			}
			case 14:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iExp = num;
				break;
			}
			case 15:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iFame = num;
				break;
			}
			case 16:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.iMoney = num;
				break;
			}
			case 17:
			{
				string[] array = data.Split(new char[]
				{
					",".get_Chars(0)
				});
				foreach (string text2 in array)
				{
					int num;
					if (!int.TryParse(text2, ref num))
					{
						num = 0;
					}
					if (num != 0)
					{
						_node.m_ItemList.Add(num);
					}
				}
				break;
			}
			case 18:
				_node.strTalkID = data;
				break;
			}
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00058534 File Offset: 0x00056734
		public MissionLevelNode GetMissionLevelNode(string levelid)
		{
			int iLevelID = 0;
			int.TryParse(levelid, ref iLevelID);
			return this.GetMissionLevelNode(iLevelID);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00058554 File Offset: 0x00056754
		public MissionLevelNode GetMissionLevelNode(int iLevelID)
		{
			for (int i = 0; i < this.m_MissionLevelNodeList.Count; i++)
			{
				if (this.m_MissionLevelNodeList[i].iLevelID == iLevelID)
				{
					return this.m_MissionLevelNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0000854F File Offset: 0x0000674F
		public List<MissionLevelNode> GetMissionLevelList()
		{
			return this.m_MissionLevelNodeList;
		}

		// Token: 0x04000B69 RID: 2921
		private static readonly MissionLevelManager instance = new MissionLevelManager();

		// Token: 0x04000B6A RID: 2922
		private List<MissionLevelNode> m_MissionLevelNodeList;
	}
}
