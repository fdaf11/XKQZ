using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000870 RID: 2160
	[USequencerEvent("Physics/Sleep Rigid Body")]
	[USequencerFriendlyName("Sleep Rigid Body")]
	public class USSleepRigidBody : USEventBase
	{
		// Token: 0x06003429 RID: 13353 RVA: 0x0018F8A0 File Offset: 0x0018DAA0
		public override void FireEvent()
		{
			Rigidbody component = base.AffectedObject.GetComponent<Rigidbody>();
			if (!component)
			{
				Debug.Log("Attempting to Nullify a force on an object, but it has no rigid body from USSleepRigidBody::FireEvent");
				return;
			}
			component.Sleep();
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x00020C66 File Offset: 0x0001EE66
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x0018F8D8 File Offset: 0x0018DAD8
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			Rigidbody component = base.AffectedObject.GetComponent<Rigidbody>();
			if (!component)
			{
				return;
			}
			component.WakeUp();
		}
	}
}
