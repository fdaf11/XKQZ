using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200087E RID: 2174
	[USequencerEvent("Transform/Parent and reset Transform")]
	[USequencerFriendlyName("Parent and reset Transform")]
	public class USParentAndResetObjectEvent : USEventBase
	{
		// Token: 0x06003465 RID: 13413 RVA: 0x001903DC File Offset: 0x0018E5DC
		public override void FireEvent()
		{
			this.previousParent = this.child.parent;
			this.previousPosition = this.child.localPosition;
			this.previousRotation = this.child.localRotation;
			this.child.parent = this.parent;
			this.child.localPosition = Vector3.zero;
			this.child.localRotation = Quaternion.identity;
		}

		// Token: 0x06003466 RID: 13414 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x00020DCA File Offset: 0x0001EFCA
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x00190450 File Offset: 0x0018E650
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			this.child.parent = this.previousParent;
			this.child.localPosition = this.previousPosition;
			this.child.localRotation = this.previousRotation;
		}

		// Token: 0x04004045 RID: 16453
		public Transform parent;

		// Token: 0x04004046 RID: 16454
		public Transform child;

		// Token: 0x04004047 RID: 16455
		private Transform previousParent;

		// Token: 0x04004048 RID: 16456
		private Vector3 previousPosition;

		// Token: 0x04004049 RID: 16457
		private Quaternion previousRotation;
	}
}
