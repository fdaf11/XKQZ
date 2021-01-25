using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200086F RID: 2159
	[USequencerEvent("Physics/Apply Force")]
	[USequencerFriendlyName("Apply Force")]
	public class USApplyForceEvent : USEventBase
	{
		// Token: 0x06003424 RID: 13348 RVA: 0x0018F7B8 File Offset: 0x0018D9B8
		public override void FireEvent()
		{
			Rigidbody component = base.AffectedObject.GetComponent<Rigidbody>();
			if (!component)
			{
				Debug.Log("Attempting to apply an impulse to an object, but it has no rigid body from USequencerApplyImpulseEvent::FireEvent");
				return;
			}
			component.AddForceAtPosition(this.direction * this.strength, base.transform.position, this.type);
			this.previousTransform = component.transform;
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x00020C5E File Offset: 0x0001EE5E
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x0018F81C File Offset: 0x0018DA1C
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
			component.Sleep();
			if (this.previousTransform)
			{
				base.AffectedObject.transform.position = this.previousTransform.position;
				base.AffectedObject.transform.rotation = this.previousTransform.rotation;
			}
		}

		// Token: 0x04004020 RID: 16416
		public Vector3 direction = Vector3.up;

		// Token: 0x04004021 RID: 16417
		public float strength = 1f;

		// Token: 0x04004022 RID: 16418
		public ForceMode type = 1;

		// Token: 0x04004023 RID: 16419
		private Transform previousTransform;
	}
}
