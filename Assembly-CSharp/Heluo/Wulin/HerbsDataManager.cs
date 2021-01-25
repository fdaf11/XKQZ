using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001B1 RID: 433
	public class HerbsDataManager
	{
		// Token: 0x06000914 RID: 2324 RVA: 0x0000778D File Offset: 0x0000598D
		public HerbsDataManager()
		{
			this.m_HerbsItemGroupList = new List<HerbsItemGroupNode>();
			this.LoadFile("HerbsData");
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x000077B7 File Offset: 0x000059B7
		public static HerbsDataManager Singleton
		{
			get
			{
				return HerbsDataManager.instance;
			}
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0004F0FC File Offset: 0x0004D2FC
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
							HerbsItemGroupNode herbsItemGroupNode = this.CheckHerbsItemGroupNode(int.Parse(array3[1]));
							HerbsItemNode herbsItemNode = new HerbsItemNode();
							herbsItemNode.m_iShowLv = int.Parse(array3[0]);
							herbsItemNode.m_iItemID = int.Parse(array3[2]);
							herbsItemNode.m_strImageID = array3[3];
							herbsItemNode.m_iAmount = int.Parse(array3[4]);
							herbsItemNode.m_strMapID = array3[5];
							herbsItemGroupNode.m_HerbsItemGroupList.Add(herbsItemNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0004F20C File Offset: 0x0004D40C
		private HerbsItemGroupNode CheckHerbsItemGroupNode(int iGruop)
		{
			for (int i = 0; i < this.m_HerbsItemGroupList.Count; i++)
			{
				if (this.m_HerbsItemGroupList[i].m_iGroupID == iGruop)
				{
					return this.m_HerbsItemGroupList[i];
				}
			}
			HerbsItemGroupNode herbsItemGroupNode = new HerbsItemGroupNode();
			herbsItemGroupNode.m_iGroupID = iGruop;
			this.m_HerbsItemGroupList.Add(herbsItemGroupNode);
			return herbsItemGroupNode;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0004F274 File Offset: 0x0004D474
		public List<HerbsItemNode> GetHerbsItemGroupList(int iGruop, int iValue)
		{
			List<HerbsItemNode> list = new List<HerbsItemNode>();
			for (int i = 0; i < this.m_HerbsItemGroupList.Count; i++)
			{
				if (this.m_HerbsItemGroupList[i].m_iGroupID == iGruop)
				{
					for (int j = 0; j < this.m_HerbsItemGroupList[i].m_HerbsItemGroupList.Count; j++)
					{
						if (this.m_HerbsItemGroupList[i].m_HerbsItemGroupList[j].m_iShowLv <= iValue)
						{
							list.Add(this.m_HerbsItemGroupList[i].m_HerbsItemGroupList[j]);
						}
					}
					return list;
				}
			}
			return null;
		}

		// Token: 0x040008AC RID: 2220
		private static readonly HerbsDataManager instance = new HerbsDataManager();

		// Token: 0x040008AD RID: 2221
		private List<HerbsItemGroupNode> m_HerbsItemGroupList;
	}
}
