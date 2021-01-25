using System;
using System.Collections.Generic;

namespace Heluo.Wulin.FSM
{
	// Token: 0x020001A2 RID: 418
	public abstract class StateBase : IState
	{
		// Token: 0x14000026 RID: 38
		// (add) Token: 0x060008B5 RID: 2229 RVA: 0x0000735E File Offset: 0x0000555E
		// (remove) Token: 0x060008B6 RID: 2230 RVA: 0x00007377 File Offset: 0x00005577
		public event Action StateEnter;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x060008B7 RID: 2231 RVA: 0x00007390 File Offset: 0x00005590
		// (remove) Token: 0x060008B8 RID: 2232 RVA: 0x000073A9 File Offset: 0x000055A9
		public event Action StateUpdate;

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x060008B9 RID: 2233 RVA: 0x000073C2 File Offset: 0x000055C2
		// (remove) Token: 0x060008BA RID: 2234 RVA: 0x000073DB File Offset: 0x000055DB
		public event Action StateExit;

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x000073F4 File Offset: 0x000055F4
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x000073FC File Offset: 0x000055FC
		public virtual FiniteStateMachine FSM { get; set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x00007405 File Offset: 0x00005605
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0000740D File Offset: 0x0000560D
		public string Name { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x00007416 File Offset: 0x00005616
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x0000741E File Offset: 0x0000561E
		public bool IsComplete { get; protected set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00007427 File Offset: 0x00005627
		// (set) Token: 0x060008C2 RID: 2242 RVA: 0x0000264F File Offset: 0x0000084F
		public List<Transition> Transitions
		{
			get
			{
				return this.m_Transitions;
			}
			set
			{
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0000742F File Offset: 0x0000562F
		public virtual void OnStateEnter()
		{
			this.IsComplete = false;
			if (this.StateEnter != null)
			{
				this.StateEnter.Invoke();
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0000744E File Offset: 0x0000564E
		public virtual void OnStateUpdate()
		{
			if (this.StateUpdate != null)
			{
				this.StateUpdate.Invoke();
			}
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00007466 File Offset: 0x00005666
		public virtual void OnStateExit()
		{
			if (this.StateExit != null)
			{
				this.StateExit.Invoke();
			}
		}

		// Token: 0x0400085A RID: 2138
		private List<Transition> m_Transitions = new List<Transition>();
	}
}
