using System;
using Newtonsoft.Json;

namespace Heluo.Wulin
{
	// Token: 0x020007FE RID: 2046
	public class BackpackNewDataNode
	{
		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x0001F8D5 File Offset: 0x0001DAD5
		[JsonIgnore]
		public int ItemID
		{
			get
			{
				if (this._ItemDataNode == null)
				{
					return 0;
				}
				if (this.m_ItemID == 0)
				{
					this.m_ItemID = this._ItemDataNode.m_iItemID;
				}
				return this.m_ItemID;
			}
		}

		// Token: 0x04003DF2 RID: 15858
		[JsonIgnore]
		public bool m_bOnClick;

		// Token: 0x04003DF3 RID: 15859
		public bool m_bNew = true;

		// Token: 0x04003DF4 RID: 15860
		public int m_iAmount;

		// Token: 0x04003DF5 RID: 15861
		[JsonIgnore]
		public ItemDataNode _ItemDataNode;

		// Token: 0x04003DF6 RID: 15862
		public int m_ItemID;

		// Token: 0x04003DF7 RID: 15863
		public int mod_Guid;
	}
}
