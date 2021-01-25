using System;
using UnityEngine;

// Token: 0x0200052A RID: 1322
public class MipTexMap : MonoBehaviour
{
	// Token: 0x060021D1 RID: 8657 RVA: 0x001002D0 File Offset: 0x000FE4D0
	private static void BuildMipFilterTex(int size)
	{
		size = Mathf.ClosestPowerOfTwo(size);
		Texture2D texture2D = new Texture2D(size, size, 1, true);
		texture2D.anisoLevel = 3;
		texture2D.filterMode = 2;
		texture2D.mipMapBias = 0f;
		for (int i = 0; i < texture2D.mipmapCount; i++)
		{
			int num = size * size;
			Color[] array = new Color[num];
			float num2 = 1f * (float)i / (float)(texture2D.mipmapCount - 1);
			Color color;
			color..ctor(num2, num2, num2, num2);
			for (int j = 0; j < num; j++)
			{
				array[j] = color;
			}
			texture2D.SetPixels(array, i);
		}
		texture2D.Apply(false, true);
		int num3 = size;
		if (num3 != 64)
		{
			if (num3 != 128)
			{
				if (num3 != 256)
				{
					if (num3 != 512)
					{
						if (num3 != 1024)
						{
							if (num3 != 2048)
							{
								MipTexMap.mipFilterTex512 = texture2D;
							}
							else
							{
								MipTexMap.mipFilterTex2048 = texture2D;
							}
						}
						else
						{
							MipTexMap.mipFilterTex1024 = texture2D;
						}
					}
					else
					{
						MipTexMap.mipFilterTex512 = texture2D;
					}
				}
				else
				{
					MipTexMap.mipFilterTex256 = texture2D;
				}
			}
			else
			{
				MipTexMap.mipFilterTex128 = texture2D;
			}
		}
		else
		{
			MipTexMap.mipFilterTex64 = texture2D;
		}
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x0010041C File Offset: 0x000FE61C
	public static Texture2D GetTex(int size)
	{
		size = Mathf.ClosestPowerOfTwo(size);
		int num = size;
		if (num != 64)
		{
			if (num != 128)
			{
				if (num != 256)
				{
					if (num != 512)
					{
						if (num != 1024)
						{
							if (num != 2048)
							{
								if (MipTexMap.mipFilterTex512)
								{
									return MipTexMap.mipFilterTex512;
								}
								MipTexMap.BuildMipFilterTex(size);
								return MipTexMap.mipFilterTex512;
							}
							else
							{
								if (MipTexMap.mipFilterTex2048)
								{
									return MipTexMap.mipFilterTex2048;
								}
								MipTexMap.BuildMipFilterTex(size);
								return MipTexMap.mipFilterTex2048;
							}
						}
						else
						{
							if (MipTexMap.mipFilterTex1024)
							{
								return MipTexMap.mipFilterTex1024;
							}
							MipTexMap.BuildMipFilterTex(size);
							return MipTexMap.mipFilterTex1024;
						}
					}
					else
					{
						if (MipTexMap.mipFilterTex512)
						{
							return MipTexMap.mipFilterTex512;
						}
						MipTexMap.BuildMipFilterTex(size);
						return MipTexMap.mipFilterTex512;
					}
				}
				else
				{
					if (MipTexMap.mipFilterTex256)
					{
						return MipTexMap.mipFilterTex256;
					}
					MipTexMap.BuildMipFilterTex(size);
					return MipTexMap.mipFilterTex256;
				}
			}
			else
			{
				if (MipTexMap.mipFilterTex128)
				{
					return MipTexMap.mipFilterTex128;
				}
				MipTexMap.BuildMipFilterTex(size);
				return MipTexMap.mipFilterTex128;
			}
		}
		else
		{
			if (MipTexMap.mipFilterTex64)
			{
				return MipTexMap.mipFilterTex64;
			}
			MipTexMap.BuildMipFilterTex(size);
			return MipTexMap.mipFilterTex64;
		}
	}

	// Token: 0x04002537 RID: 9527
	private static Texture2D mipFilterTex64;

	// Token: 0x04002538 RID: 9528
	private static Texture2D mipFilterTex128;

	// Token: 0x04002539 RID: 9529
	private static Texture2D mipFilterTex256;

	// Token: 0x0400253A RID: 9530
	private static Texture2D mipFilterTex512;

	// Token: 0x0400253B RID: 9531
	private static Texture2D mipFilterTex1024;

	// Token: 0x0400253C RID: 9532
	private static Texture2D mipFilterTex2048;
}
