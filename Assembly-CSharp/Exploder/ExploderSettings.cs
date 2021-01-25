using System;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000CB RID: 203
	internal class ExploderSettings
	{
		// Token: 0x04000388 RID: 904
		public Vector3 Position;

		// Token: 0x04000389 RID: 905
		public Vector3 ForceVector;

		// Token: 0x0400038A RID: 906
		public float Force;

		// Token: 0x0400038B RID: 907
		public float FrameBudget;

		// Token: 0x0400038C RID: 908
		public float Radius;

		// Token: 0x0400038D RID: 909
		public float DeactivateTimeout;

		// Token: 0x0400038E RID: 910
		public int id;

		// Token: 0x0400038F RID: 911
		public int TargetFragments;

		// Token: 0x04000390 RID: 912
		public DeactivateOptions DeactivateOptions;

		// Token: 0x04000391 RID: 913
		public ExploderObject.FragmentOption FragmentOptions;

		// Token: 0x04000392 RID: 914
		public ExploderObject.SFXOption SfxOptions;

		// Token: 0x04000393 RID: 915
		public ExploderObject.OnExplosion Callback;

		// Token: 0x04000394 RID: 916
		public bool DontUseTag;

		// Token: 0x04000395 RID: 917
		public bool UseForceVector;

		// Token: 0x04000396 RID: 918
		public bool MeshColliders;

		// Token: 0x04000397 RID: 919
		public bool ExplodeSelf;

		// Token: 0x04000398 RID: 920
		public bool HideSelf;

		// Token: 0x04000399 RID: 921
		public bool DestroyOriginalObject;

		// Token: 0x0400039A RID: 922
		public bool ExplodeFragments;

		// Token: 0x0400039B RID: 923
		public bool SplitMeshIslands;

		// Token: 0x0400039C RID: 924
		public bool processing;
	}
}
