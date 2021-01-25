using System;
using UnityEngine;

// Token: 0x020003F6 RID: 1014
public class NgMaterial
{
	// Token: 0x0600184C RID: 6220 RVA: 0x000C7398 File Offset: 0x000C5598
	public static bool IsMaterialColor(Material mat)
	{
		string[] array = new string[]
		{
			"_Color",
			"_TintColor",
			"_EmisColor"
		};
		if (mat != null)
		{
			foreach (string text in array)
			{
				if (mat.HasProperty(text))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600184D RID: 6221 RVA: 0x000B91A0 File Offset: 0x000B73A0
	public static string GetMaterialColorName(Material mat)
	{
		string[] array = new string[]
		{
			"_Color",
			"_TintColor",
			"_EmisColor"
		};
		if (mat != null)
		{
			foreach (string text in array)
			{
				if (mat.HasProperty(text))
				{
					return text;
				}
			}
		}
		return null;
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x0000FE76 File Offset: 0x0000E076
	public static Color GetMaterialColor(Material mat)
	{
		return NgMaterial.GetMaterialColor(mat, Color.white);
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x000C73FC File Offset: 0x000C55FC
	public static Color GetMaterialColor(Material mat, Color defaultColor)
	{
		string[] array = new string[]
		{
			"_Color",
			"_TintColor",
			"_EmisColor"
		};
		if (mat != null)
		{
			foreach (string text in array)
			{
				if (mat.HasProperty(text))
				{
					return mat.GetColor(text);
				}
			}
		}
		return defaultColor;
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x000C7464 File Offset: 0x000C5664
	public static void SetMaterialColor(Material mat, Color color)
	{
		string[] array = new string[]
		{
			"_Color",
			"_TintColor",
			"_EmisColor"
		};
		if (mat != null)
		{
			foreach (string text in array)
			{
				if (mat.HasProperty(text))
				{
					mat.SetColor(text, color);
				}
			}
		}
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x000C74CC File Offset: 0x000C56CC
	public static bool IsSameMaterial(Material mat1, Material mat2, bool bCheckAddress)
	{
		return (!bCheckAddress || !(mat1 != mat2)) && !(mat2 == null) && !(mat1.shader != mat2.shader) && !(mat1.mainTexture != mat2.mainTexture) && !(mat1.mainTextureOffset != mat2.mainTextureOffset) && !(mat1.mainTextureScale != mat2.mainTextureScale) && NgMaterial.IsSameColorProperty(mat1, mat2, "_Color") && NgMaterial.IsSameColorProperty(mat1, mat2, "_TintColor") && NgMaterial.IsSameColorProperty(mat1, mat2, "_EmisColor") && NgMaterial.IsSameFloatProperty(mat1, mat2, "_InvFade") && NgMaterial.IsMaskTexture(mat1) == NgMaterial.IsMaskTexture(mat2) && (!NgMaterial.IsMaskTexture(mat1) || !(NgMaterial.GetMaskTexture(mat1) != NgMaterial.GetMaskTexture(mat2)));
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x000C75E0 File Offset: 0x000C57E0
	public static void CopyMaterialArgument(Material srcMat, Material tarMat)
	{
		tarMat.mainTexture = srcMat.mainTexture;
		tarMat.mainTextureOffset = srcMat.mainTextureOffset;
		tarMat.mainTextureScale = srcMat.mainTextureScale;
		if (NgMaterial.IsMaskTexture(srcMat) && NgMaterial.IsMaskTexture(tarMat))
		{
			NgMaterial.SetMaskTexture(tarMat, NgMaterial.GetMaskTexture(srcMat));
		}
		NgMaterial.SetMaterialColor(tarMat, NgMaterial.GetMaterialColor(srcMat, new Color(0.5f, 0.5f, 0.5f, 0.5f)));
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x000C7658 File Offset: 0x000C5858
	public static bool IsSameColorProperty(Material mat1, Material mat2, string propertyName)
	{
		bool flag = mat1.HasProperty(propertyName);
		bool flag2 = mat2.HasProperty(propertyName);
		if (flag && flag2)
		{
			return mat1.GetColor(propertyName) == mat2.GetColor(propertyName);
		}
		return !flag && !flag2;
	}

	// Token: 0x06001854 RID: 6228 RVA: 0x000C76A4 File Offset: 0x000C58A4
	public static void CopyColorProperty(Material srcMat, Material tarMat, string propertyName)
	{
		bool flag = srcMat.HasProperty(propertyName);
		bool flag2 = tarMat.HasProperty(propertyName);
		if (flag && flag2)
		{
			tarMat.SetColor(propertyName, srcMat.GetColor(propertyName));
		}
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x000C76DC File Offset: 0x000C58DC
	public static bool IsSameFloatProperty(Material mat1, Material mat2, string propertyName)
	{
		bool flag = mat1.HasProperty(propertyName);
		bool flag2 = mat2.HasProperty(propertyName);
		if (flag && flag2)
		{
			return mat1.GetFloat(propertyName) == mat2.GetFloat(propertyName);
		}
		return !flag && !flag2;
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x0000FE83 File Offset: 0x0000E083
	public static Texture GetTexture(Material mat, bool bMask)
	{
		if (mat == null)
		{
			return null;
		}
		if (!bMask)
		{
			return mat.mainTexture;
		}
		if (NgMaterial.IsMaskTexture(mat))
		{
			return mat.GetTexture("_Mask");
		}
		return null;
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x0000FEB8 File Offset: 0x0000E0B8
	public static void SetMaskTexture(Material mat, bool bMask, Texture newTexture)
	{
		if (mat == null)
		{
			return;
		}
		if (bMask)
		{
			NgMaterial.SetMaskTexture(mat, newTexture);
		}
		else
		{
			mat.mainTexture = newTexture;
		}
	}

	// Token: 0x06001858 RID: 6232 RVA: 0x0000FEE0 File Offset: 0x0000E0E0
	public static bool IsMaskTexture(Material tarMat)
	{
		return tarMat.HasProperty("_Mask");
	}

	// Token: 0x06001859 RID: 6233 RVA: 0x0000FEED File Offset: 0x0000E0ED
	public static void SetMaskTexture(Material tarMat, Texture maskTex)
	{
		tarMat.SetTexture("_Mask", maskTex);
	}

	// Token: 0x0600185A RID: 6234 RVA: 0x0000FEFB File Offset: 0x0000E0FB
	public static Texture GetMaskTexture(Material mat)
	{
		if (mat == null || !mat.HasProperty("_Mask"))
		{
			return null;
		}
		return mat.GetTexture("_Mask");
	}
}
