using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000262 RID: 610
	public class TalentNewDataNode
	{
		// Token: 0x06000B2D RID: 2861 RVA: 0x00008D1C File Offset: 0x00006F1C
		public TalentNewDataNode()
		{
			this.m_cEffetPartList = new List<TalentResultPart>();
		}

		// Token: 0x04000D0A RID: 3338
		public int m_iTalentID;

		// Token: 0x04000D0B RID: 3339
		public string m_strTalentName;

		// Token: 0x04000D0C RID: 3340
		public string m_strTalentTip;

		// Token: 0x04000D0D RID: 3341
		public string m_strTalentImage;

		// Token: 0x04000D0E RID: 3342
		public List<TalentResultPart> m_cEffetPartList;
	}
}
