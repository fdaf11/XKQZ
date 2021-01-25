using System;
using System.Collections.Generic;

namespace Heluo.Wulin.FSM
{
	// Token: 0x020001A1 RID: 417
	public interface IState
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060008AB RID: 2219
		// (set) Token: 0x060008AC RID: 2220
		string Name { get; set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060008AD RID: 2221
		// (set) Token: 0x060008AE RID: 2222
		FiniteStateMachine FSM { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060008AF RID: 2223
		// (set) Token: 0x060008B0 RID: 2224
		List<Transition> Transitions { get; set; }

		// Token: 0x060008B1 RID: 2225
		void OnStateEnter();

		// Token: 0x060008B2 RID: 2226
		void OnStateUpdate();

		// Token: 0x060008B3 RID: 2227
		void OnStateExit();
	}
}
