using System;
using UnityEngine;

// Token: 0x0200069C RID: 1692
[Serializable]
public class FilmicCurve
{
	// Token: 0x06002909 RID: 10505 RVA: 0x00145D34 File Offset: 0x00143F34
	public float ComputeK(float t, float c, float b, float s, float w)
	{
		float num = (1f - t) * (c - b);
		float num2 = (1f - s) * (w - c) + (1f - t) * (c - b);
		return num / num2;
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x00145D6C File Offset: 0x00143F6C
	public float Toe(float x, float t, float c, float b, float s, float w, float k)
	{
		float num = this.m_ToeCoef.x * x;
		float num2 = this.m_ToeCoef.y * x;
		return (num + this.m_ToeCoef.z) / (num2 + this.m_ToeCoef.w);
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x00145DB0 File Offset: 0x00143FB0
	public float Shoulder(float x, float t, float c, float b, float s, float w, float k)
	{
		float num = this.m_ShoulderCoef.x * x;
		float num2 = this.m_ShoulderCoef.y * x;
		return (num + this.m_ShoulderCoef.z) / (num2 + this.m_ShoulderCoef.w) + k;
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x0001B09B File Offset: 0x0001929B
	public float Graph(float x, float t, float c, float b, float s, float w, float k)
	{
		if (x <= this.m_CrossOverPoint)
		{
			return this.Toe(x, t, c, b, s, w, k);
		}
		return this.Shoulder(x, t, c, b, s, w, k);
	}

	// Token: 0x0600290D RID: 10509 RVA: 0x0001B0CC File Offset: 0x000192CC
	public void StoreK()
	{
		this.m_k = this.ComputeK(this.m_ToeStrength, this.m_CrossOverPoint, this.m_BlackPoint, this.m_ShoulderStrength, this.m_WhitePoint);
	}

	// Token: 0x0600290E RID: 10510 RVA: 0x00145DF8 File Offset: 0x00143FF8
	public void ComputeShaderCoefficients(float t, float c, float b, float s, float w, float k)
	{
		float num = k * (1f - t);
		float num2 = k * (1f - t) * -b;
		float num3 = -t;
		float num4 = c - (1f - t) * b;
		this.m_ToeCoef = new Vector4(num, num3, num2, num4);
		float num5 = 1f - k;
		float num6 = (1f - k) * -c;
		float num7 = (1f - s) * w - c;
		this.m_ShoulderCoef = new Vector4(num5, s, num6, num7);
	}

	// Token: 0x0600290F RID: 10511 RVA: 0x0001B0F8 File Offset: 0x000192F8
	public void UpdateCoefficients()
	{
		this.StoreK();
		this.ComputeShaderCoefficients(this.m_ToeStrength, this.m_CrossOverPoint, this.m_BlackPoint, this.m_ShoulderStrength, this.m_WhitePoint, this.m_k);
	}

	// Token: 0x040033F4 RID: 13300
	[SerializeField]
	public float m_BlackPoint;

	// Token: 0x040033F5 RID: 13301
	[SerializeField]
	public float m_WhitePoint = 1f;

	// Token: 0x040033F6 RID: 13302
	[SerializeField]
	public float m_CrossOverPoint = 0.5f;

	// Token: 0x040033F7 RID: 13303
	[SerializeField]
	public float m_ToeStrength;

	// Token: 0x040033F8 RID: 13304
	[SerializeField]
	public float m_ShoulderStrength;

	// Token: 0x040033F9 RID: 13305
	[SerializeField]
	public float m_LuminositySaturationPoint = 0.95f;

	// Token: 0x040033FA RID: 13306
	public float m_k;

	// Token: 0x040033FB RID: 13307
	public Vector4 m_ToeCoef;

	// Token: 0x040033FC RID: 13308
	public Vector4 m_ShoulderCoef;
}
