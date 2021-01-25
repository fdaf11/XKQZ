using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000029 RID: 41
	public class ChangeState : BaseState
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00002C40 File Offset: 0x00000E40
		protected override void Init(AIBehaviors fsm)
		{
			fsm.PlayAudio();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00002B59 File Offset: 0x00000D59
		protected override bool Reason(AIBehaviors fsm)
		{
			return true;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00023704 File Offset: 0x00021904
		protected override void Action(AIBehaviors fsm)
		{
			Transform transform = fsm.transform;
			Vector3 position = transform.position;
			Quaternion rotation = transform.rotation;
			Object.Instantiate(this.changeInto, position, rotation);
			Object.Destroy(fsm.gameObject);
		}

		// Token: 0x0400009A RID: 154
		public GameObject changeInto;
	}
}
