using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.FSM.Unity3D
{
	// Token: 0x020001A6 RID: 422
	public abstract class StateBasedBehaviour : MonoBehaviour, IState
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00007546 File Offset: 0x00005746
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x0000754E File Offset: 0x0000574E
		public FiniteStateMachine FSM { get; set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00007557 File Offset: 0x00005757
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x0000755F File Offset: 0x0000575F
		public bool IsComplete { get; protected set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00007568 File Offset: 0x00005768
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x0000264F File Offset: 0x0000084F
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

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x00007570 File Offset: 0x00005770
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x00007578 File Offset: 0x00005778
		public string Name { get; set; }

		// Token: 0x060008E4 RID: 2276
		public abstract void OnStateEnter();

		// Token: 0x060008E5 RID: 2277
		public abstract void OnStateUpdate();

		// Token: 0x060008E6 RID: 2278
		public abstract void OnStateExit();

		// Token: 0x04000867 RID: 2151
		private List<Transition> m_Transitions = new List<Transition>();
	}
}
