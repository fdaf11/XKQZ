using System;

namespace Heluo.Wulin.FSM
{
	// Token: 0x020001A5 RID: 421
	public class EvaluateTriggerTransition : Transition
	{
		// Token: 0x060008D5 RID: 2261 RVA: 0x000074FB File Offset: 0x000056FB
		public EvaluateTriggerTransition()
		{
			this.EvaluateFunction = new Func<bool>(EvaluateTriggerTransition.Dummy);
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00007515 File Offset: 0x00005715
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x0000751D File Offset: 0x0000571D
		public Func<bool> EvaluateFunction { get; set; }

		// Token: 0x060008D8 RID: 2264 RVA: 0x00002C2D File Offset: 0x00000E2D
		public static bool Dummy()
		{
			return false;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x00007526 File Offset: 0x00005726
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x0000264F File Offset: 0x0000084F
		public override bool Triggered
		{
			get
			{
				return this.EvaluateFunction.Invoke();
			}
			set
			{
			}
		}
	}
}
