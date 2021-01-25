using System;
using System.Collections.Generic;

// Token: 0x020002D2 RID: 722
public static class TextParser
{
	// Token: 0x06000E44 RID: 3652 RVA: 0x00075CE4 File Offset: 0x00073EE4
	public static List<string> IDtoString(string Msg)
	{
		if (Msg == string.Empty || Msg == null)
		{
			return null;
		}
		if (Msg.IndexOf("#") < 0)
		{
			return null;
		}
		List<string> result = new List<string>();
		int i = 0;
		while (i < Msg.Length)
		{
			int num = Msg.IndexOf("<", i, Msg.Length - i);
			if (num > -1)
			{
				int num2 = Msg.IndexOf(">", num, Msg.Length - num);
				string text = Msg.Substring(num + 1, num2 - num - 1);
				string text2 = text.Substring(0, 1);
				int num3 = Msg.IndexOf("#", num, num2 - num);
				if (!text2.Equals("N"))
				{
					int num4 = int.Parse(Msg.Substring(num3 + 1, num2 - (num3 + 1)));
				}
				string empty = string.Empty;
				string text3 = text2;
				if (text3 != null)
				{
					if (TextParser.<>f__switch$map1 == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(8);
						dictionary.Add("N", 0);
						dictionary.Add("Z", 1);
						dictionary.Add("P", 2);
						dictionary.Add("T", 3);
						dictionary.Add("S", 4);
						dictionary.Add("K", 5);
						dictionary.Add("I", 6);
						dictionary.Add("A", 7);
						TextParser.<>f__switch$map1 = dictionary;
					}
					int num5;
					if (TextParser.<>f__switch$map1.TryGetValue(text3, ref num5))
					{
						switch (num5)
						{
						}
					}
				}
				i = num2 + 1;
			}
			else
			{
				i = Msg.Length;
			}
		}
		return result;
	}

	// Token: 0x06000E45 RID: 3653 RVA: 0x00075ECC File Offset: 0x000740CC
	public static string StringToFormat(string strMsg, List<string> strIDList)
	{
		string text = string.Empty;
		int i = 0;
		int num = 0;
		while (i < strMsg.Length)
		{
			int num2 = strMsg.IndexOf("<", i, strMsg.Length - i);
			if (num2 > -1)
			{
				int num3 = strMsg.IndexOf(">", num2, strMsg.Length - num2);
				text += strMsg.Substring(i, num2 - i);
				text += strIDList[num];
				num++;
				i = num3 + 1;
			}
			else
			{
				text += strMsg.Substring(i, strMsg.Length - i);
				i = strMsg.Length;
			}
		}
		return text;
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x00075F78 File Offset: 0x00074178
	public static string ChangeColor(string strMsg, string strColor)
	{
		int num = strMsg.IndexOf("]");
		return strColor + strMsg.Substring(num + 1, strMsg.Length - (num + 1));
	}

	// Token: 0x06000E47 RID: 3655 RVA: 0x00075FB4 File Offset: 0x000741B4
	public static List<string> StraightWord(string strMsg, int iLength)
	{
		List<string> list = new List<string>();
		int i = 0;
		string text = string.Empty;
		string text2 = string.Empty;
		while (i < strMsg.Length)
		{
			int num;
			if (i + iLength > strMsg.Length)
			{
				num = strMsg.IndexOf("[", i, strMsg.Length - i);
			}
			else
			{
				num = strMsg.IndexOf("[", i, iLength);
			}
			if (num > -1)
			{
				int num2 = text.Length;
				int num3 = strMsg.IndexOf("]", num, strMsg.Length - num);
				text += strMsg.Substring(i, num - i);
				int j = text.Length - num2;
				text2 = strMsg.Substring(num, num3 - num + 1);
				text += strMsg.Substring(num, num3 - num + 1);
				num2 = text.Length - j;
				i = num3 + 1;
				int num4 = iLength - j;
				if (i + num4 > strMsg.Length)
				{
					num4 = strMsg.Length - i;
				}
				num = strMsg.IndexOf("[", i, num4);
				while (j < iLength)
				{
					if (num > -1)
					{
						num3 = strMsg.IndexOf("]", num, strMsg.Length - num);
						text += strMsg.Substring(i, num - i);
						j = text.Length - num2;
						text += strMsg.Substring(num, num3 - num + 1);
						text2 = strMsg.Substring(num, num3 - num + 1);
						num2 += text2.Length;
						i = num3 + 1;
						if (i >= strMsg.Length)
						{
							break;
						}
						int num5 = iLength - j;
						if (i + num5 > strMsg.Length)
						{
							num5 = strMsg.Length - i;
						}
						num = strMsg.IndexOf("[", i, num5);
					}
					else
					{
						int num6 = iLength - j;
						if (i + num6 > strMsg.Length)
						{
							num6 = strMsg.Length - i;
						}
						text += strMsg.Substring(i, num6);
						i += num6;
						j = text.Length;
						if (text2.Length > 4)
						{
							text += "[-]";
						}
					}
				}
				list.Add(text);
				text = string.Empty;
				if (text2.Length > 4)
				{
					text += text2;
				}
			}
			else
			{
				if (i + iLength > strMsg.Length)
				{
					list.Add(strMsg.Substring(i, strMsg.Length - i));
				}
				else
				{
					list.Add(strMsg.Substring(i, iLength));
				}
				i += iLength;
			}
		}
		return list;
	}
}
