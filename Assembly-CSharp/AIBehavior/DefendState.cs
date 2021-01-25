using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x0200002D RID: 45
	public class DefendState : CooldownableState
	{
		// Token: 0x060000CB RID: 203 RVA: 0x000238A8 File Offset: 0x00021AA8
		protected override void Init(AIBehaviors fsm)
		{
			fsm.isDefending = true;
			if (this.defensiveCallScript != null && !string.IsNullOrEmpty(this.startDefendMethodName))
			{
				this.defensiveCallScript.SendMessage(this.startDefendMethodName, this);
			}
			fsm.PlayAudio();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000238F8 File Offset: 0x00021AF8
		protected override void StateEnded(AIBehaviors fsm)
		{
			fsm.isDefending = false;
			if (this.defensiveCallScript != null && !string.IsNullOrEmpty(this.endDefendMethodName))
			{
				this.defensiveCallScript.SendMessage(this.endDefendMethodName, this);
				base.TriggerCooldown();
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00002B59 File Offset: 0x00000D59
		protected override bool Reason(AIBehaviors fsm)
		{
			return true;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00002CFD File Offset: 0x00000EFD
		protected override void Action(AIBehaviors fsm)
		{
			fsm.MoveAgent(fsm.transform.position, 0f, 0f);
		}

		// Token: 0x040000A8 RID: 168
		public float defensiveBonus;

		// Token: 0x040000A9 RID: 169
		public MonoBehaviour defensiveCallScript;

		// Token: 0x040000AA RID: 170
		public string startDefendMethodName = string.Empty;

		// Token: 0x040000AB RID: 171
		public string endDefendMethodName = string.Empty;
	}
}
