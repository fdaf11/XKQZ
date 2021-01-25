using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200025D RID: 605
	public class StoreDataManager : TextDataManager
	{
		// Token: 0x06000B1A RID: 2842 RVA: 0x00008C4C File Offset: 0x00006E4C
		public StoreDataManager()
		{
			this.m_StoreDataNodeList = new List<StoreDataNode>();
			this.m_StoreDataChangesList = new List<StoreDataNode>();
			this.m_LoadFileName = "StoreData";
			this.LoadFile("StoreData");
			TextDataManager.AddTextDataToList(this);
			TextDataManager.AddDLCTextDataToList(this);
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00008C98 File Offset: 0x00006E98
		public static StoreDataManager Singleton
		{
			get
			{
				return StoreDataManager.instance;
			}
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00008C9F File Offset: 0x00006E9F
		public void Clear()
		{
			this.m_StoreDataChangesList.Clear();
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0005C570 File Offset: 0x0005A770
		protected override void LoadFile(string filePath)
		{
			this.m_StoreDataChangesList.Clear();
			this.m_StoreDataNodeList.Clear();
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
							StoreDataNode storeDataNode = this.ClickStoreDataNode(int.Parse(array3[0]));
							if (array3[1] == "1")
							{
								storeDataNode.m_bSave = true;
							}
							else
							{
								storeDataNode.m_bSave = false;
							}
							StoreItemNode storeItemNode = new StoreItemNode();
							storeItemNode.m_iItemID = int.Parse(array3[2]);
							storeItemNode.m_iBuyAmount = int.Parse(array3[3]);
							int num = 0;
							if (int.TryParse(array3[4], ref num))
							{
								storeItemNode.bAnd = (num == 1);
							}
							else
							{
								storeItemNode.bAnd = true;
							}
							Condition.GenerateList(storeItemNode.m_ConditionList, array3[5].Replace("\r", string.Empty));
							if (GameGlobal.m_bDLCMode)
							{
								storeItemNode.m_iProbability = int.Parse(array3[6]);
							}
							storeDataNode.m_StoreItemNodeList.Add(storeItemNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0005C708 File Offset: 0x0005A908
		private StoreDataNode ClickStoreDataNode(int iStoreID)
		{
			for (int i = 0; i < this.m_StoreDataNodeList.Count; i++)
			{
				if (this.m_StoreDataNodeList[i].m_iStoreID == iStoreID)
				{
					return this.m_StoreDataNodeList[i];
				}
			}
			StoreDataNode storeDataNode = new StoreDataNode();
			storeDataNode.m_iStoreID = iStoreID;
			this.m_StoreDataNodeList.Add(storeDataNode);
			return storeDataNode;
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0005C770 File Offset: 0x0005A970
		public void AddChangesStoreData(StoreDataNode _StoreDataNode, int ItemID)
		{
			if (!_StoreDataNode.m_bSave)
			{
				return;
			}
			StoreItemNode storeItemNode = null;
			for (int i = 0; i < _StoreDataNode.m_StoreItemNodeList.Count; i++)
			{
				if (_StoreDataNode.m_StoreItemNodeList[i].m_iItemID == ItemID)
				{
					storeItemNode = _StoreDataNode.m_StoreItemNodeList[i];
					break;
				}
			}
			_StoreDataNode.m_StoreItemNodeList.Remove(storeItemNode);
			bool flag = false;
			for (int j = 0; j < this.m_StoreDataChangesList.Count; j++)
			{
				if (this.m_StoreDataChangesList[j].m_iStoreID == _StoreDataNode.m_iStoreID)
				{
					this.m_StoreDataChangesList[j].m_StoreItemNodeList = _StoreDataNode.m_StoreItemNodeList;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.m_StoreDataChangesList.Add(_StoreDataNode);
			}
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0005C848 File Offset: 0x0005AA48
		public StoreDataNode GetStoreDataNode(int iStoreID)
		{
			for (int i = 0; i < this.m_StoreDataChangesList.Count; i++)
			{
				if (this.m_StoreDataChangesList[i].m_iStoreID == iStoreID)
				{
					return this.m_StoreDataChangesList[i];
				}
			}
			for (int j = 0; j < this.m_StoreDataNodeList.Count; j++)
			{
				if (this.m_StoreDataNodeList[j].m_iStoreID == iStoreID)
				{
					return this.m_StoreDataNodeList[j].Clone();
				}
			}
			return null;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00008CAC File Offset: 0x00006EAC
		public List<StoreDataNode> GetStoreChangesData()
		{
			return this.m_StoreDataChangesList;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0005C8E0 File Offset: 0x0005AAE0
		public void LoadChangesData(List<StoreDataNode> LoadData)
		{
			List<StoreDataNode> list = new List<StoreDataNode>();
			this.m_StoreDataChangesList.Clear();
			if (LoadData == null)
			{
				this.m_StoreDataChangesList = list;
				return;
			}
			for (int i = 0; i < LoadData.Count; i++)
			{
				StoreDataNode load = this.GetLoad(LoadData[i]);
				list.Add(load);
			}
			this.m_StoreDataChangesList = list;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x0005C940 File Offset: 0x0005AB40
		private StoreItemNode GetStoreNode(int iStoreID, int iItemId)
		{
			StoreDataNode storeDataNode = this.GetStoreDataNode(iStoreID);
			for (int i = 0; i < storeDataNode.m_StoreItemNodeList.Count; i++)
			{
				if (storeDataNode.m_StoreItemNodeList[i].m_iItemID == iItemId)
				{
					return storeDataNode.m_StoreItemNodeList[i].Clone();
				}
			}
			return null;
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0005C99C File Offset: 0x0005AB9C
		private StoreDataNode GetLoad(StoreDataNode Load)
		{
			StoreDataNode storeDataNode = new StoreDataNode();
			storeDataNode.m_iStoreID = Load.m_iStoreID;
			storeDataNode.m_bSave = true;
			for (int i = 0; i < Load.m_StoreItemNodeList.Count; i++)
			{
				StoreItemNode storeNode = this.GetStoreNode(Load.m_iStoreID, Load.m_StoreItemNodeList[i].m_iItemID);
				storeDataNode.m_StoreItemNodeList.Add(storeNode);
			}
			return storeDataNode;
		}

		// Token: 0x04000CA0 RID: 3232
		private static readonly StoreDataManager instance = new StoreDataManager();

		// Token: 0x04000CA1 RID: 3233
		public List<StoreDataNode> m_StoreDataNodeList;

		// Token: 0x04000CA2 RID: 3234
		private List<StoreDataNode> m_StoreDataChangesList;
	}
}
