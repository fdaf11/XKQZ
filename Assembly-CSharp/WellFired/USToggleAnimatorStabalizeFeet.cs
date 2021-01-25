using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000859 RID: 2137
	[USequencerFriendlyName("Toggle Stabalize Feet")]
	[USequencerEvent("Animation (Mecanim)/Animator/Toggle Stabalize Feet")]
	public class USToggleAnimatorStabalizeFeet : USEventBase
	{
		// Token: 0x060033B7 RID: 13239 RVA: 0x0018E084 File Offset: 0x0018C284
		public override void FireEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			this.prevStabalizeFeet = component.stabilizeFeet;
			component.stabilizeFeet = this.stabalizeFeet;
		}

		// Token: 0x060033B8 RID: 13240 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float runningTime)
		{
		}

		// Token: 0x060033B9 RID: 13241 RVA: 0x0002088F File Offset: 0x0001EA8F
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033BA RID: 13242 RVA: 0x0018E0CC File Offset: 0x0018C2CC
		public override void UndoEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				return;
			}
			component.stabilizeFeet = this.prevStabalizeFeet;
		}

		// Token: 0x04003FDE RID: 16350
		public bool stabalizeFeet = true;

		// Token: 0x04003FDF RID: 16351
		private bool prevStabalizeFeet;
	}
}
