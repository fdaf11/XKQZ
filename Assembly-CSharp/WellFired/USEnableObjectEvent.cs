using System;

namespace WellFired
{
	// Token: 0x0200086C RID: 2156
	[USequencerEvent("Object/Toggle Object")]
	[USequencerFriendlyName("Toggle Object")]
	public class USEnableObjectEvent : USEventBase
	{
		// Token: 0x06003416 RID: 13334 RVA: 0x00020BE1 File Offset: 0x0001EDE1
		public override void FireEvent()
		{
			this.prevEnable = base.AffectedObject.activeSelf;
			base.AffectedObject.SetActive(this.enable);
		}

		// Token: 0x06003417 RID: 13335 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x06003418 RID: 13336 RVA: 0x00020C05 File Offset: 0x0001EE05
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x00020C0D File Offset: 0x0001EE0D
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			base.AffectedObject.SetActive(this.prevEnable);
		}

		// Token: 0x0400401E RID: 16414
		public bool enable;

		// Token: 0x0400401F RID: 16415
		private bool prevEnable;
	}
}
