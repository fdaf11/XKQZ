using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x02000872 RID: 2162
	[USequencerEvent("Render/Change Objects Texture")]
	[USequencerFriendlyName("Change Texture")]
	public class USChangeTexture : USEventBase
	{
		// Token: 0x06003433 RID: 13363 RVA: 0x0018FA28 File Offset: 0x0018DC28
		public override void FireEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			if (!this.newTexture)
			{
				Debug.LogWarning("you've not given a texture to the USChangeTexture Event", this);
				return;
			}
			if (!Application.isPlaying && Application.isEditor)
			{
				this.previousTexture = base.AffectedObject.renderer.sharedMaterial.mainTexture;
				base.AffectedObject.renderer.sharedMaterial.mainTexture = this.newTexture;
			}
			else
			{
				this.previousTexture = base.AffectedObject.renderer.material.mainTexture;
				base.AffectedObject.renderer.material.mainTexture = this.newTexture;
			}
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x0000264F File Offset: 0x0000084F
		public override void ProcessEvent(float deltaTime)
		{
		}

		// Token: 0x06003435 RID: 13365 RVA: 0x00020C76 File Offset: 0x0001EE76
		public override void StopEvent()
		{
			this.UndoEvent();
		}

		// Token: 0x06003436 RID: 13366 RVA: 0x0018FAE8 File Offset: 0x0018DCE8
		public override void UndoEvent()
		{
			if (!base.AffectedObject)
			{
				return;
			}
			if (!this.previousTexture)
			{
				return;
			}
			if (!Application.isPlaying && Application.isEditor)
			{
				base.AffectedObject.renderer.sharedMaterial.mainTexture = this.previousTexture;
			}
			else
			{
				base.AffectedObject.renderer.material.mainTexture = this.previousTexture;
			}
			this.previousTexture = null;
		}

		// Token: 0x04004026 RID: 16422
		public Texture newTexture;

		// Token: 0x04004027 RID: 16423
		private Texture previousTexture;
	}
}
