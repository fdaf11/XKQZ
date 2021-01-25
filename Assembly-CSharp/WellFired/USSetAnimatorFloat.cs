using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000853 RID: 2131
	[USequencerEvent("Animation (Mecanim)/Animator/Set Value/Float")]
	[USequencerFriendlyName("Set Mecanim Float")]
	public class USSetAnimatorFloat : USEventBase
	{
		// Token: 0x06003399 RID: 13209 RVA: 0x0018DCB8 File Offset: 0x0018BEB8
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
			this.prevValue = component.GetFloat(this.hash);
			component.SetFloat(this.hash, this.Value);
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x0018DCB8 File Offset: 0x0018BEB8
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
			this.prevValue = component.GetFloat(this.hash);
			component.SetFloat(this.hash, this.Value);
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x000207F2 File Offset: 0x0001E9F2
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x0018DD3C File Offset: 0x0018BF3C
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
			component.SetFloat(this.hash, this.prevValue);
		}

		// Token: 0x04003FCD RID: 16333
		public string valueName = string.Empty;

		// Token: 0x04003FCE RID: 16334
		public float Value;

		// Token: 0x04003FCF RID: 16335
		private float prevValue;

		// Token: 0x04003FD0 RID: 16336
		private int hash;
	}
}
