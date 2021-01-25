using System;
using UnityEngine;

namespace WellFired
{
	// Token: 0x0200084F RID: 2127
	[ExecuteInEditMode]
	public class AmbientLightAdjuster : MonoBehaviour
	{
		// Token: 0x0600338D RID: 13197 RVA: 0x0002077A File Offset: 0x0001E97A
		private void Update()
		{
			RenderSettings.ambientLight = this.ambientLightColor;
		}

		// Token: 0x04003FBF RID: 16319
		public Color ambientLightColor = Color.red;
	}
}
