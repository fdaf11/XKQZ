using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200058B RID: 1419
	public class FractalNoise
	{
		// Token: 0x06002398 RID: 9112 RVA: 0x00017AD1 File Offset: 0x00015CD1
		public FractalNoise(float inH, float inLacunarity, float inOctaves) : this(inH, inLacunarity, inOctaves, null)
		{
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x00116CD8 File Offset: 0x00114ED8
		public FractalNoise(float inH, float inLacunarity, float inOctaves, Perlin noise)
		{
			this.m_Lacunarity = inLacunarity;
			this.m_Octaves = inOctaves;
			this.m_IntOctaves = (int)inOctaves;
			this.m_Exponent = new float[this.m_IntOctaves + 1];
			float num = 1f;
			for (int i = 0; i < this.m_IntOctaves + 1; i++)
			{
				this.m_Exponent[i] = (float)Math.Pow((double)this.m_Lacunarity, (double)(-(double)inH));
				num *= this.m_Lacunarity;
			}
			if (noise == null)
			{
				this.m_Noise = new Perlin();
			}
			else
			{
				this.m_Noise = noise;
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x00116D74 File Offset: 0x00114F74
		public float HybridMultifractal(float x, float y, float offset)
		{
			float num = (this.m_Noise.Noise(x, y) + offset) * this.m_Exponent[0];
			float num2 = num;
			x *= this.m_Lacunarity;
			y *= this.m_Lacunarity;
			int i;
			for (i = 1; i < this.m_IntOctaves; i++)
			{
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				float num3 = (this.m_Noise.Noise(x, y) + offset) * this.m_Exponent[i];
				num += num2 * num3;
				num2 *= num3;
				x *= this.m_Lacunarity;
				y *= this.m_Lacunarity;
			}
			float num4 = this.m_Octaves - (float)this.m_IntOctaves;
			return num + num4 * this.m_Noise.Noise(x, y) * this.m_Exponent[i];
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x00116E40 File Offset: 0x00115040
		public float RidgedMultifractal(float x, float y, float offset, float gain)
		{
			float num = Mathf.Abs(this.m_Noise.Noise(x, y));
			num = offset - num;
			num *= num;
			float num2 = num;
			for (int i = 1; i < this.m_IntOctaves; i++)
			{
				x *= this.m_Lacunarity;
				y *= this.m_Lacunarity;
				float num3 = num * gain;
				num3 = Mathf.Clamp01(num3);
				num = Mathf.Abs(this.m_Noise.Noise(x, y));
				num = offset - num;
				num *= num;
				num *= num3;
				num2 += num * this.m_Exponent[i];
			}
			return num2;
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x00116ED4 File Offset: 0x001150D4
		public float BrownianMotion(float x, float y)
		{
			float num = 0f;
			long num2;
			for (num2 = 0L; num2 < (long)this.m_IntOctaves; num2 += 1L)
			{
				num = this.m_Noise.Noise(x, y) * this.m_Exponent[(int)(checked((IntPtr)num2))];
				x *= this.m_Lacunarity;
				y *= this.m_Lacunarity;
			}
			float num3 = this.m_Octaves - (float)this.m_IntOctaves;
			return num + num3 * this.m_Noise.Noise(x, y) * this.m_Exponent[(int)(checked((IntPtr)num2))];
		}

		// Token: 0x04002AFA RID: 11002
		private Perlin m_Noise;

		// Token: 0x04002AFB RID: 11003
		private float[] m_Exponent;

		// Token: 0x04002AFC RID: 11004
		private int m_IntOctaves;

		// Token: 0x04002AFD RID: 11005
		private float m_Octaves;

		// Token: 0x04002AFE RID: 11006
		private float m_Lacunarity;
	}
}
