using System;

namespace AIBehavior
{
	// Token: 0x0200002A RID: 42
	public class ChangeTagState : BaseState
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00002C5B File Offset: 0x00000E5B
		protected override void Init(AIBehaviors fsm)
		{
			fsm.tag = this.newTag;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00002B59 File Offset: 0x00000D59
		protected override bool Reason(AIBehaviors fsm)
		{
			return true;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Action(AIBehaviors fsm)
		{
		}

		// Token: 0x0400009B RID: 155
		public string newTag = "Untagged";
	}
}
