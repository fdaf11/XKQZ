using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200019D RID: 413
	public class BowDataManager
	{
		// Token: 0x06000886 RID: 2182 RVA: 0x000071F5 File Offset: 0x000053F5
		public BowDataManager()
		{
			this.m_BowDataNodeList = new List<BowDataNode>();
			this.LoadFile("BowData");
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0000721F File Offset: 0x0000541F
		public static BowDataManager Singleton
		{
			get
			{
				return BowDataManager.instance;
			}
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0004DC14 File Offset: 0x0004BE14
		private void LoadFile(string filePath)
		{
			string[] array = Game.ExtractTextFile(filePath);
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
							BowDataNode bowDataNode = new BowDataNode();
							bowDataNode.m_iBowID = int.Parse(array3[0]);
							bowDataNode.m_iSpeed = int.Parse(array3[1]);
							bowDataNode.m_iAtk = int.Parse(array3[2]);
							bowDataNode.m_fReload = float.Parse(array3[3]);
							this.m_BowDataNodeList.Add(bowDataNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0004DD10 File Offset: 0x0004BF10
		public BowDataNode GetBowDataNode(int iID)
		{
			for (int i = 0; i < this.m_BowDataNodeList.Count; i++)
			{
				if (this.m_BowDataNodeList[i].m_iBowID == iID)
				{
					return this.m_BowDataNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000851 RID: 2129
		private static readonly BowDataManager instance = new BowDataManager();

		// Token: 0x04000852 RID: 2130
		public List<BowDataNode> m_BowDataNodeList;
	}
}
