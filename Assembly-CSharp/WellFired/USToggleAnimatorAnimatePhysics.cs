using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000857 RID: 2135
	[USequencerFriendlyName("Toggle Animate Physics")]
	[USequencerEvent("Animation (Mecanim)/Animator/Toggle Animate Physics")]
	public class USToggleAnimatorAnimatePhysics : USEventBase
	{
		// Token: 0x060033AD RID: 13229 RVA: 0x0018DF8C File Offset: 0x0018C18C
		public override void FireEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			this.prevAnimatePhysics = component.animatePhysics;
			component.animatePhysics = this.animatePhysics;
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float runningTime)
		{
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x00020861 File Offset: 0x0001EA61
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x0018DFD4 File Offset: 0x0018C1D4
		public override void UndoEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				return;
			}
			component.animatePhysics = this.prevAnimatePhysics;
		}

		// Token: 0x04003FDA RID: 16346
		public bool animatePhysics = true;

		// Token: 0x04003FDB RID: 16347
		private bool prevAnimatePhysics;
	}
}
