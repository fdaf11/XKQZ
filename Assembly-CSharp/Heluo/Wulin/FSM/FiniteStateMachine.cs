using System;
using System.Collections.Generic;

namespace Heluo.Wulin.FSM
{
	// Token: 0x0200019F RID: 415
	public class FiniteStateMachine : HashSet<IState>
	{
		// Token: 0x06000893 RID: 2195 RVA: 0x00007226 File Offset: 0x00005426
		public FiniteStateMachine()
		{
			this.AnyStateTransitions = new List<Transition>();
			this.Enabled = true;
			FiniteStateMachine.all.Add(this);
		}

		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000895 RID: 2197 RVA: 0x00007257 File Offset: 0x00005457
		// (remove) Token: 0x06000896 RID: 2198 RVA: 0x00007270 File Offset: 0x00005470
		public event Action<IState, IState> StateChange;

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x00007289 File Offset: 0x00005489
		public static List<FiniteStateMachine> All
		{
			get
			{
				return FiniteStateMachine.all;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00007290 File Offset: 0x00005490
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x00007298 File Offset: 0x00005498
		public string Name { get; set; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x000072A1 File Offset: 0x000054A1
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x000072A9 File Offset: 0x000054A9
		public bool Enabled { get; set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x000072B2 File Offset: 0x000054B2
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x000072BA File Offset: 0x000054BA
		public IState CurrentState { get; private set; }

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x000072C3 File Offset: 0x000054C3
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x000072CB File Offset: 0x000054CB
		public List<Transition> AnyStateTransitions { get; set; }

		// Token: 0x060008A0 RID: 2208 RVA: 0x000072D4 File Offset: 0x000054D4
		public void Destroy()
		{
			FiniteStateMachine.all.Remove(this);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x000072E2 File Offset: 0x000054E2
		public virtual void Start(IState state)
		{
			this.CurrentState = state;
			this.CurrentState.OnStateEnter();
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0004DD60 File Offset: 0x0004BF60
		public void Add(params IState[] states)
		{
			for (int i = 0; i < states.Length; i++)
			{
				this.Add(states[i]);
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x000072F6 File Offset: 0x000054F6
		public void Add(IState state)
		{
			state.FSM = this;
			base.Add(state);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0004DD8C File Offset: 0x0004BF8C
		public virtual void Update()
		{
			if (!this.Enabled)
			{
				return;
			}
			for (int i = 0; i < this.AnyStateTransitions.Count; i++)
			{
				if (this.AnyStateTransitions[i].Enable)
				{
					if (this.AnyStateTransitions[i].Triggered)
					{
						this.OnTransitionTriggered(this.AnyStateTransitions[i]);
						return;
					}
				}
			}
			for (int j = 0; j < this.CurrentState.Transitions.Count; j++)
			{
				if (this.CurrentState.Transitions[j].Enable)
				{
					if (this.CurrentState.Transitions[j].Triggered)
					{
						this.OnTransitionTriggered(this.CurrentState.Transitions[j]);
						return;
					}
				}
			}
			if (this.CurrentState == null)
			{
				return;
			}
			this.CurrentState.OnStateUpdate();
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0004DE90 File Offset: 0x0004C090
		public void OnTransitionTriggered(Transition t)
		{
			IState currentState = this.CurrentState;
			this.CurrentState.OnStateExit();
			this.CurrentState = t.NextState;
			if (this.CurrentState != null)
			{
				this.CurrentState.OnStateEnter();
			}
			if (this.StateChange != null)
			{
				this.StateChange.Invoke(currentState, this.CurrentState);
			}
		}

		// Token: 0x04000853 RID: 2131
		private static List<FiniteStateMachine> all = new List<FiniteStateMachine>();
	}
}
