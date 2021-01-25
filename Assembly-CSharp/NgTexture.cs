using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003FA RID: 1018
public class NgTexture
{
	// Token: 0x0600188F RID: 6287 RVA: 0x000C83A0 File Offset: 0x000C65A0
	public static void UnloadTextures(GameObject rootObj)
	{
		if (rootObj == null)
		{
			return;
		}
		Renderer[] componentsInChildren = rootObj.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer.material != null && renderer.material.mainTexture != null)
			{
				Debug.Log("UnloadTextures - " + renderer.material.mainTexture);
				Resources.UnloadAsset(renderer.material.mainTexture);
			}
		}
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x000C8430 File Offset: 0x000C6630
	public static Texture2D CopyTexture(Texture2D srcTex, Texture2D tarTex)
	{
		Color32[] pixels = srcTex.GetPixels32();
		tarTex.SetPixels32(pixels);
		tarTex.Apply(false);
		return tarTex;
	}

	// Token: 0x06001891 RID: 6289 RVA: 0x000C8454 File Offset: 0x000C6654
	public static Texture2D InverseTexture32(Texture2D srcTex, Texture2D tarTex)
	{
		Color32[] pixels = srcTex.GetPixels32();
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i].a = byte.MaxValue - pixels[i].a;
		}
		tarTex.SetPixels32(pixels);
		tarTex.Apply(false);
		return tarTex;
	}

	// Token: 0x06001892 RID: 6290 RVA: 0x000C84AC File Offset: 0x000C66AC
	public static Texture2D CombineTexture(Texture2D baseTexture, Texture2D combineTexture)
	{
		Texture2D texture2D = new Texture2D(baseTexture.width, baseTexture.height, baseTexture.format, false);
		Debug.LogWarning("need \tObject.DestroyImmediate(returnTexture);");
		Color[] pixels = baseTexture.GetPixels();
		Color[] pixels2 = combineTexture.GetPixels();
		Color[] array = new Color[pixels.Length];
		int num = pixels.Length;
		for (int i = 0; i < num; i++)
		{
			array[i] = Color.Lerp(pixels[i], pixels2[i], pixels2[i].a);
		}
		texture2D.SetPixels(array);
		texture2D.Apply(false);
		return texture2D;
	}

	// Token: 0x06001893 RID: 6291 RVA: 0x000C8558 File Offset: 0x000C6758
	public static bool CompareTexture(Texture2D tex1, Texture2D tex2)
	{
		Color[] pixels = tex1.GetPixels();
		Color[] pixels2 = tex2.GetPixels();
		if (pixels.Length != pixels2.Length)
		{
			return false;
		}
		int num = pixels.Length;
		for (int i = 0; i < num; i++)
		{
			if (pixels[i] != pixels2[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001894 RID: 6292 RVA: 0x000C85BC File Offset: 0x000C67BC
	public static Texture2D FindTexture(List<Texture2D> findList, Texture2D findTex)
	{
		for (int i = 0; i < findList.Count; i++)
		{
			if (NgTexture.CompareTexture(findList[i], findTex))
			{
				return findList[i];
			}
		}
		return null;
	}

	// Token: 0x06001895 RID: 6293 RVA: 0x000C85FC File Offset: 0x000C67FC
	public static int FindTextureIndex(List<Texture2D> findList, Texture2D findTex)
	{
		for (int i = 0; i < findList.Count; i++)
		{
			if (NgTexture.CompareTexture(findList[i], findTex))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001896 RID: 6294 RVA: 0x000C8638 File Offset: 0x000C6838
	public static Texture2D CopyTexture(Texture2D srcTex, Rect srcRect, Texture2D tarTex, Rect tarRect)
	{
		Color[] pixels = srcTex.GetPixels((int)srcRect.x, (int)srcRect.y, (int)srcRect.width, (int)srcRect.height);
		tarTex.SetPixels((int)tarRect.x, (int)tarRect.y, (int)tarRect.width, (int)tarRect.height, pixels);
		tarTex.Apply();
		return tarTex;
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x000C869C File Offset: 0x000C689C
	public static Texture2D CopyTextureHalf(Texture2D srcTexture, Texture2D tarHalfTexture)
	{
		if (srcTexture.width != tarHalfTexture.width * 2)
		{
			Debug.LogError("size error");
		}
		if (srcTexture.height != tarHalfTexture.height * 2)
		{
			Debug.LogError("size error");
		}
		Color[] pixels = srcTexture.GetPixels();
		Color[] array = new Color[pixels.Length / 4];
		int width = tarHalfTexture.width;
		int height = tarHalfTexture.height;
		int num = 0;
		int num2 = 2;
		int num3 = num2 * 2;
		for (int i = 0; i < height; i++)
		{
			int j = 0;
			while (j < width)
			{
				array[num] = Color.Lerp(Color.Lerp(pixels[i * width * num3 + j * num2], pixels[i * width * num3 + j * num2 + 1], 0.5f), Color.Lerp(pixels[i * width * num3 + width * num2 + j * num2], pixels[i * width * num3 + width * num2 + j * num2 + 1], 0.5f), 0.5f);
				j++;
				num++;
			}
		}
		tarHalfTexture.SetPixels(array);
		tarHalfTexture.Apply(false);
		return tarHalfTexture;
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x000C87EC File Offset: 0x000C69EC
	public static Texture2D CopyTextureQuad(Texture2D srcTexture, Texture2D tarQuadTexture)
	{
		if (srcTexture.width != tarQuadTexture.width * 4)
		{
			Debug.LogError("size error");
		}
		if (srcTexture.height != tarQuadTexture.height * 4)
		{
			Debug.LogError("size error");
		}
		Color[] pixels = srcTexture.GetPixels();
		Color[] array = new Color[pixels.Length / 16];
		int width = tarQuadTexture.width;
		int height = tarQuadTexture.height;
		int num = 0;
		int num2 = 4;
		int num3 = num2 * 4;
		for (int i = 0; i < height; i++)
		{
			int j = 0;
			while (j < width)
			{
				array[num] = Color.Lerp(Color.Lerp(Color.Lerp(Color.Lerp(pixels[i * width * num3 + j * num2], pixels[i * width * num3 + j * num2 + 1], 0.5f), Color.Lerp(pixels[i * width * num3 + width * num2 + j * num2], pixels[i * width * num3 + width * num2 + j * num2 + 1], 0.5f), 0.5f), Color.Lerp(Color.Lerp(pixels[i * width * num3 + j * num2 + 2], pixels[i * width * num3 + j * num2 + 3], 0.5f), Color.Lerp(pixels[i * width * num3 + width * num2 + j * num2 + 2], pixels[i * width * num3 + width * num2 + j * num2 + 3], 0.5f), 0.5f), 0.5f), Color.Lerp(Color.Lerp(Color.Lerp(pixels[i * width * num3 + width * num2 * 2 + j * num2], pixels[i * width * num3 + width * num2 * 2 + j * num2 + 1], 0.5f), Color.Lerp(pixels[i * width * num3 + width * num2 * 3 + j * num2], pixels[i * width * num3 + width * num2 * 3 + j * num2 + 1], 0.5f), 0.5f), Color.Lerp(Color.Lerp(pixels[i * width * num3 + width * num2 * 2 + j * num2 + 2], pixels[i * width * num3 + width * num2 * 2 + j * num2 + 3], 0.5f), Color.Lerp(pixels[i * width * num3 + width * num2 * 3 + j * num2 + 2], pixels[i * width * num3 + width * num2 * 3 + j * num2 + 3], 0.5f), 0.5f), 0.5f), 0.5f);
				j++;
				num++;
			}
		}
		tarQuadTexture.SetPixels(array);
		tarQuadTexture.Apply(false);
		return tarQuadTexture;
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x000C8B28 File Offset: 0x000C6D28
	public static Texture2D CopyTexture(Texture2D srcTex, Texture2D tarTex, Rect drawRect)
	{
		Rect srcRect;
		srcRect..ctor(0f, 0f, (float)srcTex.width, (float)srcTex.height);
		if (drawRect.x < 0f)
		{
			srcRect.x -= drawRect.x;
			srcRect.width += drawRect.x;
			drawRect.width += drawRect.x;
			drawRect.x = 0f;
		}
		if (drawRect.y < 0f)
		{
			srcRect.y -= drawRect.y;
			srcRect.height += drawRect.y;
			drawRect.height += drawRect.y;
			drawRect.y = 0f;
		}
		if ((float)tarTex.width < drawRect.x + drawRect.width)
		{
			srcRect.width -= drawRect.x + drawRect.width - (float)tarTex.width;
			drawRect.width -= drawRect.x + drawRect.width - (float)tarTex.width;
		}
		if ((float)tarTex.height < drawRect.y + drawRect.height)
		{
			srcRect.height -= drawRect.y + drawRect.height - (float)tarTex.height;
			drawRect.height -= drawRect.y + drawRect.height - (float)tarTex.height;
		}
		return NgTexture.CopyTexture(srcTex, srcRect, tarTex, drawRect);
	}
}
