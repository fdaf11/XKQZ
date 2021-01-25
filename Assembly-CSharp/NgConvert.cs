using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
public class NgConvert
{
	// Token: 0x0600187E RID: 6270 RVA: 0x000C7ED8 File Offset: 0x000C60D8
	public static string GetTabSpace(int nTab)
	{
		string text = "    ";
		string text2 = string.Empty;
		for (int i = 0; i < nTab; i++)
		{
			text2 += text;
		}
		return text2;
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x000C7F0C File Offset: 0x000C610C
	public static string[] GetIntStrings(int start, int count)
	{
		string[] array = new string[count];
		for (int i = start; i < count; i++)
		{
			array[i] = i.ToString();
		}
		return array;
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x000C7F40 File Offset: 0x000C6140
	public static int[] GetIntegers(int start, int count)
	{
		int[] array = new int[count];
		for (int i = start; i < count; i++)
		{
			array[i] = i;
		}
		return array;
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x000C7F6C File Offset: 0x000C616C
	public static ArrayList ToArrayList<TT>(TT[] data)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < data.Length; i++)
		{
			arrayList.Add(data[i]);
		}
		return arrayList;
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x000C7FA8 File Offset: 0x000C61A8
	public static TT[] ToArray<TT>(ArrayList data)
	{
		TT[] array = new TT[data.Count];
		int num = 0;
		foreach (object obj in data)
		{
			TT tt = (TT)((object)obj);
			if (tt != null)
			{
				array[num] = tt;
			}
			num++;
		}
		return array;
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x000C8028 File Offset: 0x000C6228
	public static TT[] ResizeArray<TT>(TT[] src, int nResize)
	{
		TT[] array = new TT[nResize];
		int num = 0;
		while (num < src.Length && num < nResize)
		{
			array[num] = src[num];
			num++;
		}
		return array;
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x000C8068 File Offset: 0x000C6268
	public static TT[] ResizeArray<TT>(TT[] src, int nResize, TT defaultValue)
	{
		TT[] array = new TT[nResize];
		int i = 0;
		while (i < src.Length && i < nResize)
		{
			array[i] = src[i];
			i++;
		}
		while (i < array.Length)
		{
			array[i] = defaultValue;
			i++;
		}
		return array;
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x000C80C0 File Offset: 0x000C62C0
	public static string[] ResizeArray(string[] src, int nResize)
	{
		string[] array = new string[nResize];
		int num = 0;
		while (num < src.Length && num < nResize)
		{
			array[num] = src[num];
			num++;
		}
		return array;
	}

	// Token: 0x06001886 RID: 6278 RVA: 0x000C80F8 File Offset: 0x000C62F8
	public static GameObject[] ResizeArray(GameObject[] src, int nResize)
	{
		GameObject[] array = new GameObject[nResize];
		int num = 0;
		while (num < src.Length && num < nResize)
		{
			array[num] = src[num];
			num++;
		}
		return array;
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x000C8130 File Offset: 0x000C6330
	public static GUIContent[] ResizeArray(GUIContent[] src, int nResize)
	{
		GUIContent[] array = new GUIContent[nResize];
		int num = 0;
		while (num < src.Length && num < nResize)
		{
			array[num] = src[num];
			num++;
		}
		return array;
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x000C8168 File Offset: 0x000C6368
	public static GUIContent[] StringsToContents(string[] strings)
	{
		if (strings == null)
		{
			return null;
		}
		GUIContent[] array = new GUIContent[strings.Length];
		for (int i = 0; i < strings.Length; i++)
		{
			array[i] = new GUIContent(strings[i], strings[i]);
		}
		return array;
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x000C81AC File Offset: 0x000C63AC
	public static string[] ContentsToStrings(GUIContent[] contents)
	{
		if (contents == null)
		{
			return null;
		}
		string[] array = new string[contents.Length];
		for (int i = 0; i < contents.Length; i++)
		{
			array[i] = contents[i].text;
		}
		return array;
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x000C81EC File Offset: 0x000C63EC
	public static uint ToUint(string value, uint nDefaultValue)
	{
		value = value.Trim();
		if (value == string.Empty)
		{
			value = "0";
		}
		uint result;
		if (uint.TryParse(value, ref result))
		{
			return result;
		}
		return nDefaultValue;
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x000C8228 File Offset: 0x000C6428
	public static int ToInt(string value, int nDefaultValue)
	{
		value = value.Trim();
		if (value == string.Empty)
		{
			value = "0";
		}
		int result;
		if (int.TryParse(value, ref result))
		{
			return result;
		}
		return nDefaultValue;
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x000C8264 File Offset: 0x000C6464
	public static float ToFloat(string value, float fDefaultValue)
	{
		value = value.Trim();
		if (value == string.Empty)
		{
			value = "0";
		}
		float result;
		if (float.TryParse(value, ref result))
		{
			return result;
		}
		return fDefaultValue;
	}

	// Token: 0x0600188D RID: 6285 RVA: 0x000C82A0 File Offset: 0x000C64A0
	public static string GetVaildFloatString(string strInput, ref float fCompleteValue)
	{
		int i = 0;
		int num = 0;
		string text = "0123456789";
		strInput = strInput.Trim();
		while (i < strInput.Length)
		{
			if (text.Contains(strInput.get_Chars(i).ToString()))
			{
				i++;
			}
			else if (strInput.get_Chars(i) == '+' || strInput.get_Chars(i) == '-')
			{
				if (i == 0)
				{
					i++;
				}
				else
				{
					strInput = strInput.Remove(i, 1);
				}
			}
			else if (strInput.get_Chars(i) == '.')
			{
				num++;
				i++;
				if (num != 1)
				{
					strInput = strInput.Remove(i - 1, 1);
				}
			}
			else
			{
				i++;
			}
		}
		float num2;
		if (strInput == string.Empty || !float.TryParse(strInput, ref num2))
		{
			return strInput;
		}
		if (strInput.get_Chars(strInput.Length - 1) == '.')
		{
			return strInput;
		}
		fCompleteValue = num2;
		return null;
	}
}
