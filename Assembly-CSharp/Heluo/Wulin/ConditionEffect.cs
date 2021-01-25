using System;
using System.Collections.Generic;

namespace Heluo.Wulin
{
	// Token: 0x02000228 RID: 552
	public class ConditionEffect
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x000085F0 File Offset: 0x000067F0
		public ConditionEffect()
		{
			this.m_ConditionList = new List<Condition>();
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x000588FC File Offset: 0x00056AFC
		public void SetConditionNpciD(int iID)
		{
			for (int i = 0; i < this.m_ConditionList.Count; i++)
			{
				this.m_ConditionList[i].SetParID(iID);
			}
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00058938 File Offset: 0x00056B38
		public ConditionEffect Clone()
		{
			ConditionEffect conditionEffect = new ConditionEffect();
			conditionEffect.m_iBattleConditionID = this.m_iBattleConditionID;
			foreach (Condition condition in this.m_ConditionList)
			{
				conditionEffect.m_ConditionList.Add(condition.Clone());
			}
			return conditionEffect;
		}

		// Token: 0x04000B76 RID: 2934
		public List<Condition> m_ConditionList;

		// Token: 0x04000B77 RID: 2935
		public bool bOpen;

		// Token: 0x04000B78 RID: 2936
		public int m_iBattleConditionID;
	}
}
