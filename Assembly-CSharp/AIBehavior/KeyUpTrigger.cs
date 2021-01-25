using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000046 RID: 70
	public class KeyUpTrigger : BaseTrigger
	{
		// Token: 0x0600013C RID: 316 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Init()
		{
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00003130 File Offset: 0x00001330
		protected override bool Evaluate(AIBehaviors fsm)
		{
			return Input.GetKeyUp(this.keycode);
		}

		// Token: 0x040000FD RID: 253
		public KeyCode keycode = 101;
	}
}
