using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001CA RID: 458
	public class MiningDataManager
	{
		// Token: 0x06000976 RID: 2422 RVA: 0x00007BEC File Offset: 0x00005DEC
		public MiningDataManager()
		{
			this.m_MiningItemNodeList = new List<MiningItemNode>();
			this.LoadFile("MiningData");
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x00007C16 File Offset: 0x00005E16
		public static MiningDataManager Singleton
		{
			get
			{
				return MiningDataManager.instance;
			}
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00051104 File Offset: 0x0004F304
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
							MiningItemNode miningItemNode = new MiningItemNode();
							string[] array4 = array3[0].Split(new char[]
							{
								",".get_Chars(0)
							});
							for (int j = 0; j < array4.Length; j++)
							{
								miningItemNode.m_ProbabilityList.Add(int.Parse(array4[j]));
							}
							miningItemNode.m_iItemID = int.Parse(array3[1]);
							miningItemNode.m_iGetAmount = int.Parse(array3[2]);
							miningItemNode.m_strImageID = array3[3];
							miningItemNode.m_iType = int.Parse(array3[4]);
							miningItemNode.m_strMapID = array3[5];
							miningItemNode.m_iBatter = int.Parse(array3[6]);
							this.m_MiningItemNodeList.Add(miningItemNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00007C1D File Offset: 0x00005E1D
		public List<MiningItemNode> GetMiningItemList()
		{
			return this.m_MiningItemNodeList;
		}

		// Token: 0x04000964 RID: 2404
		private static readonly MiningDataManager instance = new MiningDataManager();

		// Token: 0x04000965 RID: 2405
		private List<MiningItemNode> m_MiningItemNodeList;
	}
}
