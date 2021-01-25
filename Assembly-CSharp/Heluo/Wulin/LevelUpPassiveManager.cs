using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000214 RID: 532
	public class LevelUpPassiveManager : TextDataManager
	{
		// Token: 0x06000A4B RID: 2635 RVA: 0x00008359 File Offset: 0x00006559
		public LevelUpPassiveManager()
		{
			this.m_PassiveNodeList = new List<LevelUpPassiveNode>();
			this.m_LoadFileName = "LevelUpPassive";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x00008395 File Offset: 0x00006595
		public static LevelUpPassiveManager Singleton
		{
			get
			{
				return LevelUpPassiveManager.instance;
			}
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x000574A8 File Offset: 0x000556A8
		protected override void LoadFile(string filePath)
		{
			this.m_PassiveNodeList.Clear();
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
								LevelUpPassiveNode levelUpPassiveNode = new LevelUpPassiveNode();
								for (int j = 0; j < array3.Length; j++)
								{
									this.GreatData(j, array3[j], levelUpPassiveNode);
								}
								this.m_PassiveNodeList.Add(levelUpPassiveNode);
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

		// Token: 0x06000A4F RID: 2639 RVA: 0x000575D8 File Offset: 0x000557D8
		private void GreatData(int idx, string data, LevelUpPassiveNode _node)
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
				_node.iID = num;
				break;
			}
			case 1:
				_node.strName = data;
				break;
			case 2:
				_node.strIcon = data;
				break;
			case 3:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				if (num == 0)
				{
					_node.bAuto = false;
				}
				else
				{
					_node.bAuto = true;
				}
				break;
			}
			case 4:
				Condition.GenerateList(_node.m_RequestConditionList, data);
				break;
			case 5:
				PassiveNode.GenerateList(_node.m_PassiveNodeList, data);
				break;
			case 6:
				_node.strDesc = data.Replace("<br>", "\n");
				break;
			case 7:
			{
				int num;
				if (!int.TryParse(data, ref num))
				{
					num = 0;
				}
				_node.price = num;
				break;
			}
			}
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000576CC File Offset: 0x000558CC
		public LevelUpPassiveNode GetLevelUpPassiveNode(int iID)
		{
			for (int i = 0; i < this.m_PassiveNodeList.Count; i++)
			{
				if (this.m_PassiveNodeList[i].iID == iID)
				{
					return this.m_PassiveNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0000839C File Offset: 0x0000659C
		public List<LevelUpPassiveNode> GetPassiveNodeList()
		{
			return this.m_PassiveNodeList;
		}

		// Token: 0x04000B06 RID: 2822
		private static readonly LevelUpPassiveManager instance = new LevelUpPassiveManager();

		// Token: 0x04000B07 RID: 2823
		private List<LevelUpPassiveNode> m_PassiveNodeList;
	}
}
