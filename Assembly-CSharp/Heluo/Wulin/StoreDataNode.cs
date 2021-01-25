using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Heluo.Wulin
{
	// Token: 0x0200025C RID: 604
	public class StoreDataNode
	{
		// Token: 0x06000B18 RID: 2840 RVA: 0x00008C39 File Offset: 0x00006E39
		public StoreDataNode()
		{
			this.m_StoreItemNodeList = new List<StoreItemNode>();
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0005C4EC File Offset: 0x0005A6EC
		public StoreDataNode Clone()
		{
			StoreDataNode storeDataNode = new StoreDataNode();
			storeDataNode.m_iStoreID = this.m_iStoreID;
			storeDataNode.m_bSave = this.m_bSave;
			foreach (StoreItemNode storeItemNode in this.m_StoreItemNodeList)
			{
				storeDataNode.m_StoreItemNodeList.Add(storeItemNode.Clone());
			}
			return storeDataNode;
		}

		// Token: 0x04000C9D RID: 3229
		public int m_iStoreID;

		// Token: 0x04000C9E RID: 3230
		[JsonIgnore]
		public bool m_bSave;

		// Token: 0x04000C9F RID: 3231
		public List<StoreItemNode> m_StoreItemNodeList;
	}
}
