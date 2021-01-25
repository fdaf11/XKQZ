using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// Token: 0x02000498 RID: 1176
public class ByteReader
{
	// Token: 0x06001C5D RID: 7261 RVA: 0x00012D7B File Offset: 0x00010F7B
	public ByteReader(byte[] bytes)
	{
		this.mBuffer = bytes;
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x00012D8A File Offset: 0x00010F8A
	public ByteReader(TextAsset asset)
	{
		this.mBuffer = asset.bytes;
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x000DCE14 File Offset: 0x000DB014
	public static ByteReader Open(string path)
	{
		FileStream fileStream = File.OpenRead(path);
		if (fileStream != null)
		{
			fileStream.Seek(0L, 2);
			byte[] array = new byte[fileStream.Position];
			fileStream.Seek(0L, 0);
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			return new ByteReader(array);
		}
		return null;
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06001C61 RID: 7265 RVA: 0x00012DAA File Offset: 0x00010FAA
	public bool canRead
	{
		get
		{
			return this.mBuffer != null && this.mOffset < this.mBuffer.Length;
		}
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x00012DCA File Offset: 0x00010FCA
	private static string ReadLine(byte[] buffer, int start, int count)
	{
		return Encoding.UTF8.GetString(buffer, start, count);
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x00012DD9 File Offset: 0x00010FD9
	public string ReadLine()
	{
		return this.ReadLine(true);
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x000DCE6C File Offset: 0x000DB06C
	public string ReadLine(bool skipEmptyLines)
	{
		int num = this.mBuffer.Length;
		if (skipEmptyLines)
		{
			while (this.mOffset < num && this.mBuffer[this.mOffset] < 32)
			{
				this.mOffset++;
			}
		}
		int i = this.mOffset;
		if (i < num)
		{
			while (i < num)
			{
				int num2 = (int)this.mBuffer[i++];
				if (num2 == 10 || num2 == 13)
				{
					IL_87:
					string result = ByteReader.ReadLine(this.mBuffer, this.mOffset, i - this.mOffset - 1);
					this.mOffset = i;
					return result;
				}
			}
			i++;
			goto IL_87;
		}
		this.mOffset = num;
		return null;
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x000DCF34 File Offset: 0x000DB134
	public Dictionary<string, string> ReadDictionary()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		char[] array = new char[]
		{
			'='
		};
		while (this.canRead)
		{
			string text = this.ReadLine();
			if (text == null)
			{
				break;
			}
			if (!text.StartsWith("//"))
			{
				string[] array2 = text.Split(array, 2, 1);
				if (array2.Length == 2)
				{
					string text2 = array2[0].Trim();
					string text3 = array2[1].Trim().Replace("\\n", "\n");
					dictionary[text2] = text3;
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x000DCFCC File Offset: 0x000DB1CC
	public BetterList<string> ReadCSV()
	{
		ByteReader.mTemp.Clear();
		string text = string.Empty;
		bool flag = false;
		int num = 0;
		while (this.canRead)
		{
			if (flag)
			{
				string text2 = this.ReadLine(false);
				if (text2 == null)
				{
					return null;
				}
				text2 = text2.Replace("\\n", "\n");
				text = text + "\n" + text2;
			}
			else
			{
				text = this.ReadLine(true);
				if (text == null)
				{
					return null;
				}
				text = text.Replace("\\n", "\n");
				num = 0;
			}
			int i = num;
			int length = text.Length;
			while (i < length)
			{
				char c = text.get_Chars(i);
				if (c == ',')
				{
					if (!flag)
					{
						ByteReader.mTemp.Add(text.Substring(num, i - num));
						num = i + 1;
					}
				}
				else if (c == '"')
				{
					if (flag)
					{
						if (i + 1 >= length)
						{
							ByteReader.mTemp.Add(text.Substring(num, i - num).Replace("\"\"", "\""));
							return ByteReader.mTemp;
						}
						if (text.get_Chars(i + 1) != '"')
						{
							ByteReader.mTemp.Add(text.Substring(num, i - num).Replace("\"\"", "\""));
							flag = false;
							if (text.get_Chars(i + 1) == ',')
							{
								i++;
								num = i + 1;
							}
						}
						else
						{
							i++;
						}
					}
					else
					{
						num = i + 1;
						flag = true;
					}
				}
				i++;
			}
			if (num < text.Length)
			{
				if (flag)
				{
					continue;
				}
				ByteReader.mTemp.Add(text.Substring(num, text.Length - num));
			}
			return ByteReader.mTemp;
		}
		return null;
	}

	// Token: 0x0400213C RID: 8508
	private byte[] mBuffer;

	// Token: 0x0400213D RID: 8509
	private int mOffset;

	// Token: 0x0400213E RID: 8510
	private static BetterList<string> mTemp = new BetterList<string>();
}
