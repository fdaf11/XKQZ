using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000253 RID: 595
	public class Skill
	{
		// Token: 0x06000AF9 RID: 2809 RVA: 0x00008AF2 File Offset: 0x00006CF2
		public Skill()
		{
			this._OpenList = new List<Condition>();
		}

		// Token: 0x04000C67 RID: 3175
		public bool m_bOpen;

		// Token: 0x04000C68 RID: 3176
		public string SkillName;

		// Token: 0x04000C69 RID: 3177
		public int m_iConditionID;

		// Token: 0x04000C6A RID: 3178
		public List<Condition> _OpenList;
	}
}
