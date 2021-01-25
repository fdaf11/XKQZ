using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200086B RID: 2155
	[USequencerEvent("Light/Set Ambient Light")]
	[USequencerFriendlyName("Set Ambient Light")]
	public class USSetAmbientLightEvent : USEventBase
	{
		// Token: 0x06003411 RID: 13329 RVA: 0x00020BB4 File Offset: 0x0001EDB4
		public override void FireEvent()
		{
			this.prevLightColor = RenderSettings.ambientLight;
			RenderSettings.ambientLight = this.lightColor;
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x00020BCC File Offset: 0x0001EDCC
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003414 RID: 13332 RVA: 0x00020BD4 File Offset: 0x0001EDD4
		public override void UndoEvent()
		{
			RenderSettings.ambientLight = this.prevLightColor;
		}

		// Token: 0x0400401C RID: 16412
		public Color lightColor = Color.red;

		// Token: 0x0400401D RID: 16413
		private Color prevLightColor;
	}
}
