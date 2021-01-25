using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000873 RID: 2163
	[USequencerEvent("Render/Toggle Mesh Renderer")]
	[USequencerFriendlyName("Toggle Mesh Renderer")]
	public class USMeshRenderDisable : USEventBase
	{
		// Token: 0x06003438 RID: 13368 RVA: 0x0018FB70 File Offset: 0x0018DD70
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			MeshRenderer component = base.AffectedObject.GetComponent<MeshRenderer>();
			if (!component)
			{
				Debug.LogWarning("You didn't add a Mesh Renderer to the Affected Object", base.AffectedObject);
				return;
			}
			this.previousEnable = component.enabled;
			component.enabled = this.enable;
		}

		// Token: 0x06003439 RID: 13369 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x0600343A RID: 13370 RVA: 0x00020C7E File Offset: 0x0001EE7E
		public override void EndEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x0600343B RID: 13371 RVA: 0x00020C7E File Offset: 0x0001EE7E
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x0018FBD0 File Offset: 0x0018DDD0
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			MeshRenderer component = base.AffectedObject.GetComponent<MeshRenderer>();
			if (!component)
			{
				Debug.LogWarning("You didn't add a Mesh Renderer to the Affected Object", base.AffectedObject);
				return;
			}
			component.enabled = this.previousEnable;
		}

		// Token: 0x04004028 RID: 16424
		public bool enable;

		// Token: 0x04004029 RID: 16425
		private bool previousEnable;
	}
}
