using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000860 RID: 2144
	[USequencerFriendlyName("Attach Object To Parent")]
	[USequencerEvent("Attach/Attach To Parent")]
	public class USAttachToParentEvent : USEventBase
	{
		// Token: 0x060033D7 RID: 13271 RVA: 0x0018E8A4 File Offset: 0x0018CAA4
		public override void FireEvent()
		{
			if (!this.parentObject)
			{
				Debug.Log("USAttachEvent has been asked to attach an object, but it hasn't been given a parent from USAttachEvent::FireEvent");
				return;
			}
			this.originalParent = base.AffectedObject.transform.parent;
			base.AffectedObject.transform.parent = this.parentObject;
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x000209EA File Offset: 0x0001EBEA
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x000209F2 File Offset: 0x0001EBF2
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			base.AffectedObject.transform.parent = this.originalParent;
		}

		// Token: 0x04003FF4 RID: 16372
		public Transform parentObject;

		// Token: 0x04003FF5 RID: 16373
		private Transform originalParent;
	}
}
