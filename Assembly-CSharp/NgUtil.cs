using System;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class NgUtil
{
	// Token: 0x0600189B RID: 6299 RVA: 0x0000264F File Offset: 0x0000084F
	public static void LogDevelop(object msg)
	{
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x00004B67 File Offset: 0x00002D67
	public static void LogMessage(object msg)
	{
		Debug.Log(msg);
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x00004B67 File Offset: 0x00002D67
	public static void LogError(object msg)
	{
		Debug.Log(msg);
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x000C8CE0 File Offset: 0x000C6EE0
	public static float GetArcRadian(float fHeight, float fWidth)
	{
		float num = fWidth / 2f;
		float arcRadius = NgUtil.GetArcRadius(fHeight, fWidth);
		float num2 = Mathf.Sin(num / arcRadius);
		return num2 * 2f;
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x000C8D10 File Offset: 0x000C6F10
	public static float GetArcRadius(float fHeight, float fWidth)
	{
		float num = fWidth / 2f;
		return (fHeight * fHeight + num * num) / (2f * fHeight);
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x000C8D38 File Offset: 0x000C6F38
	public static float GetArcLength(float fHeight, float fWidth)
	{
		float num = fWidth / 2f;
		float arcRadius = NgUtil.GetArcRadius(fHeight, fWidth);
		float num2 = num / arcRadius;
		return arcRadius * (2f * num2);
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x000C8D64 File Offset: 0x000C6F64
	public static int NextPowerOf2(int val)
	{
		int i;
		for (i = Mathf.ClosestPowerOfTwo(val); i < val; i <<= 1)
		{
		}
		return i;
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x000C8D8C File Offset: 0x000C6F8C
	public static void ClearStrings(string[] strings)
	{
		if (strings == null)
		{
			return;
		}
		for (int i = 0; i < strings.Length; i++)
		{
			strings[i] = string.Empty;
		}
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x000C8DBC File Offset: 0x000C6FBC
	public static void ClearBools(bool[] bools)
	{
		if (bools == null)
		{
			return;
		}
		for (int i = 0; i < bools.Length; i++)
		{
			bools[i] = false;
		}
	}

	// Token: 0x060018A4 RID: 6308 RVA: 0x000C8DE8 File Offset: 0x000C6FE8
	public static void ClearObjects(Object[] objects)
	{
		if (objects == null)
		{
			return;
		}
		for (int i = 0; i < objects.Length; i++)
		{
			objects[i] = null;
		}
	}
}
