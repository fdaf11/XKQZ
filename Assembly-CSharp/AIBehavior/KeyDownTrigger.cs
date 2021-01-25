using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000045 RID: 69
	public class KeyDownTrigger : BaseTrigger
	{
		// Token: 0x06000139 RID: 313 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Init()
		{
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00003113 File Offset: 0x00001313
		protected override bool Evaluate(AIBehaviors fsm)
		{
			return Input.GetKeyDown(this.keycode);
		}

		// Token: 0x040000FC RID: 252
		public KeyCode keycode = 101;
	}
}
