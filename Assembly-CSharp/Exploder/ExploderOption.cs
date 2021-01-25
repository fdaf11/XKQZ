using System;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000CA RID: 202
	public class ExploderOption : MonoBehaviour
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x0003459C File Offset: 0x0003279C
		public void DuplicateSettings(ExploderOption options)
		{
			options.Plane2D = this.Plane2D;
			options.CrossSectionVertexColor = this.CrossSectionVertexColor;
			options.CrossSectionUV = this.CrossSectionUV;
			options.SplitMeshIslands = this.SplitMeshIslands;
			options.UseLocalForce = this.UseLocalForce;
			options.Force = this.Force;
		}

		// Token: 0x04000382 RID: 898
		public bool Plane2D;

		// Token: 0x04000383 RID: 899
		public Color CrossSectionVertexColor = Color.white;

		// Token: 0x04000384 RID: 900
		public Vector4 CrossSectionUV = new Vector4(0f, 0f, 1f, 1f);

		// Token: 0x04000385 RID: 901
		public bool SplitMeshIslands;

		// Token: 0x04000386 RID: 902
		public bool UseLocalForce;

		// Token: 0x04000387 RID: 903
		public float Force = 30f;
	}
}
