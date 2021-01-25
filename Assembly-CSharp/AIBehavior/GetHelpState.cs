using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000032 RID: 50
	public class GetHelpState : CooldownableState
	{
		// Token: 0x060000DA RID: 218 RVA: 0x00023C0C File Offset: 0x00021E0C
		protected override void Init(AIBehaviors fsm)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(this.helpTag);
			Vector3 position = fsm.transform.position;
			foreach (GameObject gameObject in array)
			{
				if (Vector3.Distance(gameObject.transform.position, position) < this.helpRadius)
				{
					Vector3 helpTargetPosition = position;
					helpTargetPosition.y = gameObject.transform.position.y;
					AIBehaviors component = gameObject.GetComponent<AIBehaviors>();
					if (component != null)
					{
						this.HelpAnotherFSM(helpTargetPosition, component);
					}
				}
			}
			fsm.PlayAudio();
			fsm.gameObject.SendMessage("OnGetHelp", 1);
			base.TriggerCooldown();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00023CC8 File Offset: 0x00021EC8
		private void HelpAnotherFSM(Vector3 helpTargetPosition, AIBehaviors otherFSM)
		{
			HelpState state = otherFSM.GetState<HelpState>();
			if (state != null)
			{
				state.helpPoint = helpTargetPosition;
				otherFSM.ChangeActiveState(state);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void StateEnded(AIBehaviors fsm)
		{
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00002B59 File Offset: 0x00000D59
		protected override bool Reason(AIBehaviors fsm)
		{
			return true;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000264F File Offset: 0x0000084F
		protected override void Action(AIBehaviors fsm)
		{
		}

		// Token: 0x040000C8 RID: 200
		public float helpRadius = 30f;

		// Token: 0x040000C9 RID: 201
		public string helpTag = "Untagged";
	}
}
