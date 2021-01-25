using System;

namespace Heluo.Wulin
{
	// Token: 0x020001BB RID: 443
	public class SpecialTalkNode
	{
		// Token: 0x0400090D RID: 2317
		public SpecialTalkNode.SpecialType m_Type;

		// Token: 0x0400090E RID: 2318
		public string m_strValue;

		// Token: 0x0400090F RID: 2319
		public string m_strMsgID;

		// Token: 0x020001BC RID: 444
		public enum SpecialType
		{
			// Token: 0x04000911 RID: 2321
			None,
			// Token: 0x04000912 RID: 2322
			Point,
			// Token: 0x04000913 RID: 2323
			PlayingAnimation,
			// Token: 0x04000914 RID: 2324
			Quest,
			// Token: 0x04000915 RID: 2325
			QuestIng,
			// Token: 0x04000916 RID: 2326
			OldQuest,
			// Token: 0x04000917 RID: 2327
			CollectionQuest,
			// Token: 0x04000918 RID: 2328
			MovieEvent,
			// Token: 0x04000919 RID: 2329
			FlagOnce
		}
	}
}
