using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200021E RID: 542
	public class MapTalkTypeNode
	{
		// Token: 0x06000A62 RID: 2658 RVA: 0x00008471 File Offset: 0x00006671
		public MapTalkTypeNode()
		{
			this.m_MapTalkNodeList = new List<MapTalkNode>();
		}

		// Token: 0x04000B3E RID: 2878
		public string m_strTalkGroupID;

		// Token: 0x04000B3F RID: 2879
		public List<MapTalkNode> m_MapTalkNodeList;
	}
}
