using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000583 RID: 1411
	public class WaveRandom
	{
		// Token: 0x0600235A RID: 9050 RVA: 0x00115694 File Offset: 0x00113894
		public void Reset()
		{
			this.seeds[0] = Random.Range(1f, 2f);
			this.seeds[1] = Random.Range(1f, 2f);
			this.seeds[2] = Random.Range(1f, 2f);
			this.seed = 0;
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x001156F0 File Offset: 0x001138F0
		public Vector3 GetRandom(float minAmp, float maxAmp, float minRand, float maxRand, int len)
		{
			float num = maxAmp - minAmp;
			this.seed++;
			if (this.seed >= len)
			{
				this.seed = 0;
			}
			float num2 = 3.1415927f / (float)len * (float)this.seed;
			float num3 = 1.2732395f * num2 - 0.40528473f * num2 * num2;
			float num4 = minAmp + num3 * num;
			float num5 = 6.2831855f;
			for (int i = 0; i < 3; i++)
			{
				if (this.seeds[i] >= 1f)
				{
					this.seeds[i] = this.seeds[i] - 1f;
					this.dSeeds[i] = Random.Range(minRand, maxRand);
				}
				this.seeds[i] += this.dSeeds[i];
				num2 = this.seeds[i] * num5;
				if (num2 > 3.1415927f)
				{
					num2 -= num5;
				}
				if (num2 < 0f)
				{
					num3 = 1.2732395f * num2 + 0.40528473f * num2 * num2;
				}
				else
				{
					num3 = 1.2732395f * num2 - 0.40528473f * num2 * num2;
				}
				this.disp[i] = num3 * num4;
			}
			return this.disp;
		}

		// Token: 0x04002ACD RID: 10957
		protected int seed;

		// Token: 0x04002ACE RID: 10958
		protected float[] dSeeds = new float[3];

		// Token: 0x04002ACF RID: 10959
		protected float[] seeds = new float[3];

		// Token: 0x04002AD0 RID: 10960
		protected Vector3 disp = Vector3.zero;
	}
}
