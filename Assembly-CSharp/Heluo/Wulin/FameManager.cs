using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001FF RID: 511
	public class FameManager : TextDataManager
	{
		// Token: 0x06000A1C RID: 2588 RVA: 0x00008184 File Offset: 0x00006384
		public FameManager()
		{
			this.m_FameNodeList = new List<FameNode>();
			this.m_LoadFileName = "Fame";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x000081C0 File Offset: 0x000063C0
		public static FameManager Singleton
		{
			get
			{
				return FameManager.instance;
			}
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00056568 File Offset: 0x00054768
		protected override void LoadFile(string filePath)
		{
			this.m_FameNodeList.Clear();
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
								FameNode fameNode = new FameNode();
								int num = 0;
								if (!int.TryParse(array3[0], ref num))
								{
									num = 0;
								}
								fameNode.iFame = num;
								fameNode.strDesc = array3[1];
								Condition.GenerateList(fameNode.LevelUpConditionList, array3[2]);
								if (!int.TryParse(array3[3], ref num))
								{
									num = 0;
								}
								fameNode.iReward = num;
								this.m_FameNodeList.Add(fameNode);
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
			this.m_FameNodeList.Sort((FameNode A, FameNode B) => A.iFame.CompareTo(B.iFame));
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x000081C7 File Offset: 0x000063C7
		public List<FameNode> GetFameList()
		{
			return this.m_FameNodeList;
		}

		// Token: 0x04000A84 RID: 2692
		private static readonly FameManager instance = new FameManager();

		// Token: 0x04000A85 RID: 2693
		private List<FameNode> m_FameNodeList;
	}
}
