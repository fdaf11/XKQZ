using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000871 RID: 2161
	[USequencerFriendlyName("Change Color")]
	[USequencerEvent("Render/Change Objects Color")]
	public class USChangeColor : USEventBase
	{
		// Token: 0x0600342E RID: 13358 RVA: 0x0018F914 File Offset: 0x0018DB14
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			if (!Application.isPlaying && Application.isEditor)
			{
				this.previousColor = base.AffectedObject.renderer.sharedMaterial.color;
				base.AffectedObject.renderer.sharedMaterial.color = this.newColor;
			}
			else
			{
				this.previousColor = base.AffectedObject.renderer.material.color;
				base.AffectedObject.renderer.material.color = this.newColor;
			}
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x00020C6E File Offset: 0x0001EE6E
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x0018F9B8 File Offset: 0x0018DBB8
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			if (!Application.isPlaying && Application.isEditor)
			{
				base.AffectedObject.renderer.sharedMaterial.color = this.previousColor;
			}
			else
			{
				base.AffectedObject.renderer.material.color = this.previousColor;
			}
		}

		// Token: 0x04004024 RID: 16420
		public Color newColor;

		// Token: 0x04004025 RID: 16421
		private Color previousColor;
	}
}
