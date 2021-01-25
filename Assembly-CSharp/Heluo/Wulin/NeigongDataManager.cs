using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200022B RID: 555
	public class NeigongDataManager : TextDataManager
	{
		// Token: 0x06000A8B RID: 2699 RVA: 0x00008645 File Offset: 0x00006845
		public NeigongDataManager()
		{
			this.m_NeigongDataList = new List<NeigongNewDataNode>();
			this.m_LoadFileName = "NeigongData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x00008687 File Offset: 0x00006887
		public static NeigongDataManager Singleton
		{
			get
			{
				return NeigongDataManager.instance;
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00058BF0 File Offset: 0x00056DF0
		protected override void LoadFile(string filePath)
		{
			this.m_NeigongDataList.Clear();
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
								NeigongNewDataNode neigongNewDataNode = new NeigongNewDataNode();
								for (int j = 0; j < array3.Length; j++)
								{
									this.GreatData(j, array3[j], neigongNewDataNode);
								}
								this.m_NeigongDataList.Add(neigongNewDataNode);
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
			this.m_NeigongDataList.Sort((NeigongNewDataNode A, NeigongNewDataNode B) => A.m_iNeigongID.CompareTo(B.m_iNeigongID));
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00058D48 File Offset: 0x00056F48
		private void GreatData(int idx, string data, NeigongNewDataNode _node)
		{
			if (idx >= 8)
			{
				if (idx % 2 == 0)
				{
					idx = 8;
				}
				else
				{
					idx = 9;
				}
			}
			switch (idx)
			{
			case 0:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.m_iNeigongID = num;
				break;
			}
			case 1:
				_node.m_strNeigongName = data;
				break;
			case 2:
				_node.m_strNeigongTip = data.Replace("<br>", "\n");
				break;
			case 3:
				_node.m_strUpgradeNotes = data;
				break;
			case 4:
				LevelUp.GreatData(_node.m_LevelUP, data);
				break;
			case 5:
				LevelUp.GreatData(_node.m_MaxLevelUP, data);
				break;
			case 6:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.m_iExpType = num;
				break;
			}
			case 7:
				_node.sIconImage = data;
				break;
			case 8:
			{
				ConditionEffect conditionEffect = new ConditionEffect();
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				conditionEffect.m_iBattleConditionID = num;
				_node.m_ConditionEffectList.Add(conditionEffect);
				break;
			}
			case 9:
			{
				ConditionEffect conditionEffect2 = _node.GetConditionEffect(_node.m_ConditionEffectList.Count - 1);
				Condition.GenerateList(conditionEffect2.m_ConditionList, data);
				break;
			}
			}
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00058E94 File Offset: 0x00057094
		public NeigongNewDataNode GetNeigongData(int iID)
		{
			int num = this.BinarySearch(iID);
			if (num >= 0)
			{
				return this.m_NeigongDataList[num].Clone();
			}
			return null;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00058EC4 File Offset: 0x000570C4
		public string GetNeigongName(int iID)
		{
			int num = this.BinarySearch(iID);
			if (num >= 0)
			{
				return this.m_NeigongDataList[num].m_strNeigongName;
			}
			return null;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0000868E File Offset: 0x0000688E
		public List<NeigongNewDataNode> GetNeigongList()
		{
			return this.m_NeigongDataList;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00058EF4 File Offset: 0x000570F4
		private int BinarySearch(int id)
		{
			List<NeigongNewDataNode> neigongDataList = this.m_NeigongDataList;
			int i = 0;
			int num = neigongDataList.Count - 1;
			while (i <= num)
			{
				int num2 = (i + num) / 2;
				if (neigongDataList[num2].m_iNeigongID == id)
				{
					return num2;
				}
				if (neigongDataList[num2].m_iNeigongID < id)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2 - 1;
				}
			}
			return -1;
		}

		// Token: 0x04000B8E RID: 2958
		private static readonly NeigongDataManager instance = new NeigongDataManager();

		// Token: 0x04000B8F RID: 2959
		private List<NeigongNewDataNode> m_NeigongDataList;
	}
}
