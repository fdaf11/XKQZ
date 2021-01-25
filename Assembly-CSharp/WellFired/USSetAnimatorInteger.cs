using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000854 RID: 2132
	[USequencerFriendlyName("Set Mecanim Integer")]
	[USequencerEvent("Animation (Mecanim)/Animator/Set Value/Integer")]
	public class USSetAnimatorInteger : USEventBase
	{
		// Token: 0x0600339E RID: 13214 RVA: 0x0018DD84 File Offset: 0x0018BF84
		public override void FireEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			if (this.valueName.Length == 0)
			{
				Debug.LogWarning("Invalid name passed to the uSequencer Event Set Float", this);
				return;
			}
			this.hash = Animator.StringToHash(this.valueName);
			this.prevValue = component.GetInteger(this.hash);
			component.SetInteger(this.hash, this.Value);
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x0018DD84 File Offset: 0x0018BF84
		public override void ProcessEvent(float runningTime)
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				Debug.LogWarning("Affected Object has no Animator component, for uSequencer Event", this);
				return;
			}
			if (this.valueName.Length == 0)
			{
				Debug.LogWarning("Invalid name passed to the uSequencer Event Set Float", this);
				return;
			}
			this.hash = Animator.StringToHash(this.valueName);
			this.prevValue = component.GetInteger(this.hash);
			component.SetInteger(this.hash, this.Value);
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x0002080D File Offset: 0x0001EA0D
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x0018DE08 File Offset: 0x0018C008
		public override void UndoEvent()
		{
			Animator component = base.AffectedObject.GetComponent<Animator>();
			if (!component)
			{
				return;
			}
			if (this.valueName.Length == 0)
			{
				return;
			}
			component.SetInteger(this.hash, this.prevValue);
		}

		// Token: 0x04003FD1 RID: 16337
		public string valueName = string.Empty;

		// Token: 0x04003FD2 RID: 16338
		public int Value;

		// Token: 0x04003FD3 RID: 16339
		private int prevValue;

		// Token: 0x04003FD4 RID: 16340
		private int hash;
	}
}
