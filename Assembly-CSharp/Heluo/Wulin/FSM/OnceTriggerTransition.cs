using System;

namespace Heluo.Wulin.FSM
{
	// Token: 0x020001A4 RID: 420
	public class OnceTriggerTransition : Transition
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0004DF2C File Offset: 0x0004C12C
		// (set) Token: 0x060008D3 RID: 2259 RVA: 0x000074EA File Offset: 0x000056EA
		public override bool Triggered
		{
			get
			{
				bool triggered = base.Triggered;
				base.Triggered = false;
				return triggered;
			}
			set
			{
				base.Triggered = value;
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x000074F3 File Offset: 0x000056F3
		public bool Peek()
		{
			return base.Triggered;
		}
	}
}
