using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000852 RID: 2130
	[USequencerFriendlyName("Set Mecanim Bool")]
	[USequencerEvent("Animation (Mecanim)/Animator/Set Value/Bool")]
	internal class USSetAnimatorBool : USEventBase
	{
		// Token: 0x06003394 RID: 13204 RVA: 0x0018DBEC File Offset: 0x0018BDEC
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
			this.prevValue = component.GetBool(this.hash);
			component.SetBool(this.hash, this.Value);
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x0018DBEC File Offset: 0x0018BDEC
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
			this.prevValue = component.GetBool(this.hash);
			component.SetBool(this.hash, this.Value);
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x000207D7 File Offset: 0x0001E9D7
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x0018DC70 File Offset: 0x0018BE70
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
			component.SetBool(this.hash, this.prevValue);
		}

		// Token: 0x04003FC9 RID: 16329
		public string valueName = string.Empty;

		// Token: 0x04003FCA RID: 16330
		public bool Value = true;

		// Token: 0x04003FCB RID: 16331
		private bool prevValue;

		// Token: 0x04003FCC RID: 16332
		private int hash;
	}
}
