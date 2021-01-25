using System;
using System.IO;

// Token: 0x020003F3 RID: 1011
public class NgFile
{
	// Token: 0x06001813 RID: 6163 RVA: 0x000C6384 File Offset: 0x000C4584
	public static string PathSeparatorNormalize(string path)
	{
		char[] array = path.ToCharArray();
		for (int i = 0; i < path.Length; i++)
		{
			if (path.get_Chars(i) == '/' || path.get_Chars(i) == '\\')
			{
				array[i] = '/';
			}
		}
		path = new string(array);
		return path;
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x0000FB76 File Offset: 0x0000DD76
	public static string CombinePath(string path1, string path2)
	{
		return NgFile.PathSeparatorNormalize(Path.Combine(path1, path2));
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x0000FB84 File Offset: 0x0000DD84
	public static string CombinePath(string path1, string path2, string path3)
	{
		return NgFile.PathSeparatorNormalize(Path.Combine(Path.Combine(path1, path2), path3));
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x000C63DC File Offset: 0x000C45DC
	public static string GetSplit(string path, int nIndex)
	{
		if (nIndex < 0)
		{
			return path;
		}
		string[] array = path.Split(new char[]
		{
			'/',
			'\\'
		});
		if (nIndex < array.Length)
		{
			return array[nIndex];
		}
		return string.Empty;
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x000C641C File Offset: 0x000C461C
	public static string GetFilename(string path)
	{
		int num = path.Length - 1;
		while (0 <= num)
		{
			if (path.get_Chars(num) == '/' || path.get_Chars(num) == '\\')
			{
				if (num == path.Length - 1)
				{
					return string.Empty;
				}
				return NgFile.TrimFileExt(path.Substring(num + 1));
			}
			else
			{
				num--;
			}
		}
		return NgFile.TrimFileExt(path);
	}

	// Token: 0x06001818 RID: 6168 RVA: 0x000C6488 File Offset: 0x000C4688
	public static string GetFilenameExt(string path)
	{
		int num = path.Length - 1;
		while (0 <= num)
		{
			if (path.get_Chars(num) == '/' || path.get_Chars(num) == '\\')
			{
				if (num == path.Length - 1)
				{
					return string.Empty;
				}
				return path.Substring(num + 1);
			}
			else
			{
				num--;
			}
		}
		return path;
	}

	// Token: 0x06001819 RID: 6169 RVA: 0x000C64EC File Offset: 0x000C46EC
	public static string GetFileExt(string path)
	{
		int num = path.Length - 1;
		while (0 <= num)
		{
			if (path.get_Chars(num) == '.')
			{
				return path.Substring(num + 1);
			}
			num--;
		}
		return string.Empty;
	}

	// Token: 0x0600181A RID: 6170 RVA: 0x000C6530 File Offset: 0x000C4730
	public static string TrimFilenameExt(string path)
	{
		int num = path.Length - 1;
		while (0 <= num)
		{
			if (path.get_Chars(num) == '/' || path.get_Chars(num) == '\\')
			{
				return path.Substring(0, num);
			}
			num--;
		}
		return string.Empty;
	}

	// Token: 0x0600181B RID: 6171 RVA: 0x000C6580 File Offset: 0x000C4780
	public static string TrimFileExt(string filename)
	{
		int num = filename.Length - 1;
		while (0 <= num)
		{
			if (filename.get_Chars(num) == '.')
			{
				return filename.Substring(0, num);
			}
			num--;
		}
		return filename;
	}

	// Token: 0x0600181C RID: 6172 RVA: 0x000C65C0 File Offset: 0x000C47C0
	public static string TrimLastFolder(string path)
	{
		int num = path.Length - 1;
		while (0 <= num)
		{
			if ((path.get_Chars(num) == '/' || path.get_Chars(num) == '\\') && num != path.Length - 1)
			{
				return path.Substring(0, num);
			}
			num--;
		}
		return string.Empty;
	}

	// Token: 0x0600181D RID: 6173 RVA: 0x000C6620 File Offset: 0x000C4820
	public static string GetLastFolder(string path)
	{
		int num = path.Length - 1;
		while (0 <= num)
		{
			if ((path.get_Chars(num) == '/' || path.get_Chars(num) == '\\') && num != path.Length - 1)
			{
				if (path.get_Chars(path.Length - 1) == '/' || path.get_Chars(path.Length - 1) == '\\')
				{
					return path.Substring(num + 1, path.Length - num - 2);
				}
				return path.Substring(num + 1, path.Length - num - 1);
			}
			else
			{
				num--;
			}
		}
		return path;
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x0000FB98 File Offset: 0x0000DD98
	public static bool CompareExtName(string srcPath, string tarLowerExt, bool bCheckCase)
	{
		if (bCheckCase)
		{
			return NgFile.GetFilenameExt(srcPath).ToLower() == tarLowerExt;
		}
		return NgFile.GetFilenameExt(srcPath) == tarLowerExt;
	}
}
