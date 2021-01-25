using System;

namespace Heluo.Wulin
{
	// Token: 0x02000204 RID: 516
	public class ItmeEffectNode
	{
		// Token: 0x04000A9A RID: 2714
		public int m_iItemType;

		// Token: 0x04000A9B RID: 2715
		public int m_iRecoverType;

		// Token: 0x04000A9C RID: 2716
		public int m_iValue;

		// Token: 0x04000A9D RID: 2717
		public int m_iMsgID;

		// Token: 0x02000205 RID: 517
		public enum ItemEffectType
		{
			// Token: 0x04000A9F RID: 2719
			None,
			// Token: 0x04000AA0 RID: 2720
			NpcProperty,
			// Token: 0x04000AA1 RID: 2721
			NpcFriend,
			// Token: 0x04000AA2 RID: 2722
			AddRoutine,
			// Token: 0x04000AA3 RID: 2723
			AddNeigong,
			// Token: 0x04000AA4 RID: 2724
			PracticeExp,
			// Token: 0x04000AA5 RID: 2725
			BattleCondition = 7,
			// Token: 0x04000AA6 RID: 2726
			ReplyHp,
			// Token: 0x04000AA7 RID: 2727
			ReplySp,
			// Token: 0x04000AA8 RID: 2728
			AddExp,
			// Token: 0x04000AA9 RID: 2729
			UseSkill = 15,
			// Token: 0x04000AAA RID: 2730
			AddHpRate,
			// Token: 0x04000AAB RID: 2731
			AddSpRate,
			// Token: 0x04000AAC RID: 2732
			RemoveNegativeState
		}
	}
}
