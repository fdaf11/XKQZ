using System;
using UnityEngine;

// Token: 0x0200083A RID: 2106
public static class TOD_Util
{
	// Token: 0x06003355 RID: 13141 RVA: 0x0002030A File Offset: 0x0001E50A
	public static Color Linear(Color color)
	{
		return (QualitySettings.activeColorSpace != 1) ? color : color.linear;
	}

	// Token: 0x06003356 RID: 13142 RVA: 0x00020324 File Offset: 0x0001E524
	public static Color ExpRGB(Color color, float exposure)
	{
		if (exposure == 1f)
		{
			return color;
		}
		return new Color(color.r * exposure, color.g * exposure, color.b * exposure, color.a);
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x0002035A File Offset: 0x0001E55A
	public static Color ExpRGBA(Color color, float exposure)
	{
		if (exposure == 1f)
		{
			return color;
		}
		return new Color(color.r * exposure, color.g * exposure, color.b * exposure, color.a * exposure);
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x0018CFDC File Offset: 0x0018B1DC
	public static Color PowRGB(Color color, float power)
	{
		if (power == 1f)
		{
			return color;
		}
		return new Color(Mathf.Pow(color.r, power), Mathf.Pow(color.g, power), Mathf.Pow(color.b, power), color.a);
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x0018D02C File Offset: 0x0018B22C
	public static Color PowRGBA(Color color, float power)
	{
		if (power == 1f)
		{
			return color;
		}
		return new Color(Mathf.Pow(color.r, power), Mathf.Pow(color.g, power), Mathf.Pow(color.b, power), Mathf.Pow(color.a, power));
	}

	// Token: 0x0600335A RID: 13146 RVA: 0x00020392 File Offset: 0x0001E592
	public static Vector3 Inverse(Vector3 vector)
	{
		return new Vector3(1f / vector.x, 1f / vector.y, 1f / vector.z);
	}
}
