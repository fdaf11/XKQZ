using System;
using UnityEngine;

namespace PigeonCoopToolkit.Effects.Trails
{
	// Token: 0x020005B7 RID: 1463
	[Serializable]
	public class PCTrailRendererData
	{
		// Token: 0x04002C4C RID: 11340
		public Material TrailMaterial;

		// Token: 0x04002C4D RID: 11341
		public float Lifetime = 1f;

		// Token: 0x04002C4E RID: 11342
		public AnimationCurve SizeOverLife = new AnimationCurve();

		// Token: 0x04002C4F RID: 11343
		public Gradient ColorOverLife;

		// Token: 0x04002C50 RID: 11344
		public bool StretchSizeToFit;

		// Token: 0x04002C51 RID: 11345
		public bool StretchColorToFit;

		// Token: 0x04002C52 RID: 11346
		public float MaterialTileLength;

		// Token: 0x04002C53 RID: 11347
		public bool UseForwardOverride;

		// Token: 0x04002C54 RID: 11348
		public Vector3 ForwardOverride;

		// Token: 0x04002C55 RID: 11349
		public bool ForwardOverrideRelative;
	}
}
