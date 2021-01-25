using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000855 RID: 2133
	[USequencerFriendlyName("Set Layer Weight")]
	[USequencerEvent("Animation (Mecanim)/Animator/Set Layer Weight")]
	public class USSetAnimatorLayerWeight : USEventBase
	{
		// Token: 0x060033A3 RID: 13219 RVA: 0x0018DE50 File Offset: 0x0018C050
		public override void FireEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			if (this.layerIndex < 0)
			{
				Debug.LogWarning("Set Animator Layer weight, incorrect index : " + this.layerIndex);
				return;
			}
			this.prevLayerWeight = component.GetLayerWeight(this.layerIndex);
			component.SetLayerWeight(this.layerIndex, this.layerWeight);
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float runningTime)
		{
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x0002082F File Offset: 0x0001EA2F
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x0018DECC File Offset: 0x0018C0CC
		public override void UndoEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				return;
			}
			if (this.layerIndex < 0)
			{
				return;
			}
			component.SetLayerWeight(this.layerIndex, this.prevLayerWeight);
		}

		// Token: 0x04003FD5 RID: 16341
		public float layerWeight = 1f;

		// Token: 0x04003FD6 RID: 16342
		public int layerIndex = -1;

		// Token: 0x04003FD7 RID: 16343
		private float prevLayerWeight;
	}
}
