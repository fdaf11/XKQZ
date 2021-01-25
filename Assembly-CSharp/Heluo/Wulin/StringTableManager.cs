using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200025F RID: 607
	public class StringTableManager : TextDataManager
	{
		// Token: 0x06000B27 RID: 2855 RVA: 0x00008CB4 File Offset: 0x00006EB4
		public StringTableManager()
		{
			this.m_StringNodeList = new Dictionary<int, string>();
			this.m_LoadFileName = "String_table";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00008CF0 File Offset: 0x00006EF0
		public static StringTableManager Singleton
		{
			get
			{
				return StringTableManager.instance;
			}
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0005CA0C File Offset: 0x0005AC0C
		protected override void LoadFile(string filePath)
		{
			this.m_StringNodeList.Clear();
			string[] array = base.ExtractTextFile(filePath);
			if (array == null)
			{
				return;
			}
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					if (text.get_Chars(0) != '#')
					{
						try
						{
							string[] array3 = text.Split(new char[]
							{
								"\t".get_Chars(0)
							});
							int num = 0;
							if (!int.TryParse(array3[0], ref num))
							{
								num = 0;
								Debug.LogError("StringTable 表，有相同ID  " + array3[0]);
							}
							if (!this.m_StringNodeList.ContainsKey(num))
							{
								this.m_StringNodeList.Add(num, array3[2].Replace("/r", string.Empty));
							}
							else
							{
								Debug.LogError("StringTable 表，有相同ID " + num);
							}
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00008CF7 File Offset: 0x00006EF7
		public string GetString(int iID)
		{
			if (this.m_StringNodeList.ContainsKey(iID))
			{
				return this.m_StringNodeList[iID];
			}
			return string.Empty;
		}

		// Token: 0x04000CA5 RID: 3237
		private static readonly StringTableManager instance = new StringTableManager();

		// Token: 0x04000CA6 RID: 3238
		private Dictionary<int, string> m_StringNodeList;
	}
}
