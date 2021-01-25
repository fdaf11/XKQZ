using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200087F RID: 2175
	[USequencerEvent("Transform/Warp To Object")]
	[USequencerFriendlyName("Warp To Object")]
	public class USWarpToObject : USEventBase
	{
		// Token: 0x0600346A RID: 13418 RVA: 0x001904A4 File Offset: 0x0018E6A4
		public override void FireEvent()
		{
			if (this.objectToWarpTo)
			{
				base.AffectedObject.transform.position = this.objectToWarpTo.transform.position;
				if (this.useObjectRotation)
				{
					base.AffectedObject.transform.rotation = this.objectToWarpTo.transform.rotation;
				}
			}
			else
			{
				Debug.LogError(base.AffectedObject.name + ": No Object attached to WarpToObjectSequencer Script");
			}
			this.previousTransform = base.AffectedObject.transform;
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x00020DD2 File Offset: 0x0001EFD2
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x0019053C File Offset: 0x0018E73C
		public override void UndoEvent()
		{
			if (this.previousTransform)
			{
				base.AffectedObject.transform.position = this.previousTransform.position;
				base.AffectedObject.transform.rotation = this.previousTransform.rotation;
			}
		}

		// Token: 0x0400404A RID: 16458
		public GameObject objectToWarpTo;

		// Token: 0x0400404B RID: 16459
		public bool useObjectRotation;

		// Token: 0x0400404C RID: 16460
		private Transform previousTransform;
	}
}
