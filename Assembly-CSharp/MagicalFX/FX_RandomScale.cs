using System;
using UnityEngine;

namespace MagicalFX
{
	// Token: 0x0200065B RID: 1627
	public class FX_RandomScale : MonoBehaviour
	{
		// Token: 0x060027F7 RID: 10231 RVA: 0x0013C008 File Offset: 0x0013A208
		private void Start()
		{
			this.scaleTarget = base.transform.localScale * Random.Range(this.ScaleMin, this.ScaleMax);
			if (!this.Blend)
			{
				base.transform.localScale = this.scaleTarget;
			}
			else
			{
				base.transform.localScale = this.scaleTarget * 0.2f;
			}
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x0001A592 File Offset: 0x00018792
		private void Update()
		{
			if (this.Blend)
			{
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, this.scaleTarget, 0.5f);
			}
		}

		// Token: 0x040031F3 RID: 12787
		public bool Blend;

		// Token: 0x040031F4 RID: 12788
		public float BlendSpeed = 0.5f;

		// Token: 0x040031F5 RID: 12789
		public float ScaleMin;

		// Token: 0x040031F6 RID: 12790
		public float ScaleMax = 1f;

		// Token: 0x040031F7 RID: 12791
		private Vector3 scaleTarget;
	}
}
