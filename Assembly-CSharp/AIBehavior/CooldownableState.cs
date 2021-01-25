using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x0200002B RID: 43
	public abstract class CooldownableState : BaseState
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x00002C7F File Offset: 0x00000E7F
		public override void InitState(AIBehaviors fsm)
		{
			if (this.InitCooldown(fsm))
			{
				base.InitState(fsm);
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00002C94 File Offset: 0x00000E94
		protected void TriggerCooldown()
		{
			this.cooledDownTime = Time.time + this.cooldownTime;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00002CA8 File Offset: 0x00000EA8
		protected bool CoolDownFinished()
		{
			return this.cooledDownTime < Time.time;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00023740 File Offset: 0x00021940
		private bool InitCooldown(AIBehaviors fsm)
		{
			this.cooldowns = 0;
			if (this.initResetsCooldown)
			{
				this.cooledDownTime = 0f;
			}
			else if (this.switchStateIfStillCoolingDown && !this.CoolDownFinished())
			{
				if (!(this.stillCoolingDownState == this))
				{
					fsm.ChangeActiveState(this.stillCoolingDownState);
					return false;
				}
				Debug.LogWarning(string.Concat(new string[]
				{
					"Switching back to the same state when a cooldown isn't finished would lock up the system, choose another state other than '",
					this.name,
					"' in the state '",
					this.name,
					"'"
				}));
			}
			return true;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000237E4 File Offset: 0x000219E4
		public override void HandleAction(AIBehaviors fsm)
		{
			if (this.CoolDownFinished())
			{
				if (this.hasCooldownLimit)
				{
					if (this.cooldowns > this.cooldownLimit)
					{
						fsm.ChangeActiveState(this.cooldownLimitExceededState);
						return;
					}
					this.cooldowns++;
				}
				base.HandleAction(fsm);
			}
			else if (this.switchStateIfStillCoolingDown)
			{
				fsm.ChangeActiveState(this.stillCoolingDownState);
			}
		}

		// Token: 0x0400009C RID: 156
		public float cooldownTime;

		// Token: 0x0400009D RID: 157
		private float cooledDownTime;

		// Token: 0x0400009E RID: 158
		public bool initResetsCooldown = true;

		// Token: 0x0400009F RID: 159
		public bool switchStateIfStillCoolingDown;

		// Token: 0x040000A0 RID: 160
		public BaseState stillCoolingDownState;

		// Token: 0x040000A1 RID: 161
		public bool hasCooldownLimit;

		// Token: 0x040000A2 RID: 162
		public int cooldownLimit = 3;

		// Token: 0x040000A3 RID: 163
		public BaseState cooldownLimitExceededState;

		// Token: 0x040000A4 RID: 164
		private int cooldowns;
	}
}
