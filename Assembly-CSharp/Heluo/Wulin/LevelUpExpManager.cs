using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200020F RID: 527
	public class LevelUpExpManager : TextDataManager
	{
		// Token: 0x06000A40 RID: 2624 RVA: 0x000082B3 File Offset: 0x000064B3
		public LevelUpExpManager()
		{
			this.m_LevelUpExpList = new List<LevelUpExpNode>();
			this.m_LoadFileName = "LevelUpExp";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x000082EF File Offset: 0x000064EF
		public static LevelUpExpManager Singleton
		{
			get
			{
				return LevelUpExpManager.instance;
			}
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00057144 File Offset: 0x00055344
		protected override void LoadFile(string filePath)
		{
			this.m_LevelUpExpList.Clear();
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
								LevelUpExpNode levelUpExpNode = new LevelUpExpNode();
								levelUpExpNode.iLevel = int.Parse(array3[0]);
								levelUpExpNode.iExp = int.Parse(array3[1]);
								levelUpExpNode.iTotalExp = int.Parse(array3[2]);
								this.m_LevelUpExpList.Add(levelUpExpNode);
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
			this.m_LevelUpExpList.Sort((LevelUpExpNode A, LevelUpExpNode B) => A.iLevel.CompareTo(B.iLevel));
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x000572A4 File Offset: 0x000554A4
		public int GetNowExpLevel(int iExp)
		{
			for (int i = 0; i < this.m_LevelUpExpList.Count; i++)
			{
				if (iExp < this.m_LevelUpExpList[i].iTotalExp)
				{
					return this.m_LevelUpExpList[i].iLevel;
				}
			}
			if (this.m_LevelUpExpList.Count > 0)
			{
				return this.m_LevelUpExpList[this.m_LevelUpExpList.Count - 1].iLevel;
			}
			return 0;
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00057328 File Offset: 0x00055528
		public int GetLevelExp(int iLevel)
		{
			foreach (LevelUpExpNode levelUpExpNode in this.m_LevelUpExpList)
			{
				if (iLevel == levelUpExpNode.iLevel)
				{
					return levelUpExpNode.iExp;
				}
			}
			return 0;
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00057398 File Offset: 0x00055598
		public int GetLevelTotalExp(int iLevel)
		{
			foreach (LevelUpExpNode levelUpExpNode in this.m_LevelUpExpList)
			{
				if (iLevel == levelUpExpNode.iLevel)
				{
					return levelUpExpNode.iTotalExp;
				}
			}
			return 0;
		}

		// Token: 0x04000AE6 RID: 2790
		private static readonly LevelUpExpManager instance = new LevelUpExpManager();

		// Token: 0x04000AE7 RID: 2791
		private List<LevelUpExpNode> m_LevelUpExpList;
	}
}
