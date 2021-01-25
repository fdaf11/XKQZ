using System;
using System.Collections.Generic;

namespace Heluo.Wulin.FSM
{
	// Token: 0x020001A0 RID: 416
	public class VariableFSM : FiniteStateMachine
	{
		// Token: 0x060008A6 RID: 2214 RVA: 0x00007307 File Offset: 0x00005507
		public VariableFSM()
		{
			this.Variables = new Dictionary<string, object>();
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0000731A File Offset: 0x0000551A
		// (set) Token: 0x060008A8 RID: 2216 RVA: 0x00007322 File Offset: 0x00005522
		public Dictionary<string, object> Variables { get; private set; }

		// Token: 0x060008A9 RID: 2217 RVA: 0x0000732B File Offset: 0x0000552B
		public void AddVariable(string name, object obj)
		{
			if (this.Variables.ContainsKey(name))
			{
				this.Variables[name] = obj;
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0004DEF0 File Offset: 0x0004C0F0
		public T GetVariable<T>(string name)
		{
			if (this.Variables.ContainsKey(name))
			{
				return (T)((object)this.Variables[name]);
			}
			return default(T);
		}
	}
}
