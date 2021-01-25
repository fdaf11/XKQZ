using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x0200021C RID: 540
	public class MapTalkNode
	{
		// Token: 0x06000A61 RID: 2657 RVA: 0x00008453 File Offset: 0x00006653
		public MapTalkNode()
		{
			this.m_MapTalkButtonNodeList = new List<MapTalkButtonNode>();
			this.m_MapTalkMapTalkConditionList = new List<Condition>();
		}

		// Token: 0x04000B2A RID: 2858
		public int m_iOrder;

		// Token: 0x04000B2B RID: 2859
		public int m_iNpcID;

		// Token: 0x04000B2C RID: 2860
		public int m_iNpcIDEX;

		// Token: 0x04000B2D RID: 2861
		public MapTalkNode.eImageDirectionType m_ImageDirection;

		// Token: 0x04000B2E RID: 2862
		public List<Condition> m_MapTalkMapTalkConditionList;

		// Token: 0x04000B2F RID: 2863
		public string m_strNpcVoice;

		// Token: 0x04000B30 RID: 2864
		public string m_strActionID;

		// Token: 0x04000B31 RID: 2865
		public string m_strManager;

		// Token: 0x04000B32 RID: 2866
		public bool m_bInFields;

		// Token: 0x04000B33 RID: 2867
		public List<MapTalkButtonNode> m_MapTalkButtonNodeList;

		// Token: 0x04000B34 RID: 2868
		public string m_strNextQuestID;

		// Token: 0x04000B35 RID: 2869
		public float m_fDestroyTime;

		// Token: 0x04000B36 RID: 2870
		public int m_iGiftID;

		// Token: 0x0200021D RID: 541
		public enum eImageDirectionType
		{
			// Token: 0x04000B38 RID: 2872
			Left,
			// Token: 0x04000B39 RID: 2873
			Right,
			// Token: 0x04000B3A RID: 2874
			NoPainted,
			// Token: 0x04000B3B RID: 2875
			Aside,
			// Token: 0x04000B3C RID: 2876
			Center,
			// Token: 0x04000B3D RID: 2877
			MidText
		}
	}
}
