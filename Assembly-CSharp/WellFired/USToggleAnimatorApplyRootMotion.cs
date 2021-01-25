using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000858 RID: 2136
	[USequencerFriendlyName("Toggle Apply Root Motion")]
	[USequencerEvent("Animation (Mecanim)/Animator/Toggle Apply Root Motion")]
	public class USToggleAnimatorApplyRootMotion : USEventBase
	{
		// Token: 0x060033B2 RID: 13234 RVA: 0x0018E008 File Offset: 0x0018C208
		public override void FireEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			this.prevApplyRootMotion = component.applyRootMotion;
			component.applyRootMotion = this.applyRootMotion;
		}

		// Token: 0x060033B3 RID: 13235 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float runningTime)
		{
		}

		// Token: 0x060033B4 RID: 13236 RVA: 0x00020878 File Offset: 0x0001EA78
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x0018E050 File Offset: 0x0018C250
		public override void UndoEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				return;
			}
			component.applyRootMotion = this.prevApplyRootMotion;
		}

		// Token: 0x04003FDC RID: 16348
		public bool applyRootMotion = true;

		// Token: 0x04003FDD RID: 16349
		private bool prevApplyRootMotion;
	}
}
