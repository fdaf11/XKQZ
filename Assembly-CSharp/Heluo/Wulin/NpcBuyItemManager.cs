using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200022F RID: 559
	public class NpcBuyItemManager : TextDataManager
	{
		// Token: 0x06000A9D RID: 2717 RVA: 0x000086FF File Offset: 0x000068FF
		public NpcBuyItemManager()
		{
			this.NpcBuyItem = new Dictionary<int, List<int>>();
			this.m_LoadFileName = "NpcBuyItem";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0000873B File Offset: 0x0000693B
		public static NpcBuyItemManager Singleton
		{
			get
			{
				return NpcBuyItemManager.instance;
			}
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x00059130 File Offset: 0x00057330
		protected override void LoadFile(string filePath)
		{
			this.NpcBuyItem.Clear();
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
							List<int> list = new List<int>();
							if (!int.TryParse(array3[0], ref num))
							{
								num = 0;
								Debug.LogError("NpcBuyItem 表，解析錯誤  " + array3[0]);
							}
							else if (!this.NpcBuyItem.ContainsKey(num))
							{
								for (int j = 1; j < array3.Length; j++)
								{
									int num2 = 0;
									if (int.TryParse(array3[j].Replace("/r", string.Empty), ref num2))
									{
										list.Add(num2);
									}
								}
								this.NpcBuyItem.Add(num, list);
							}
							else
							{
								Debug.LogError("NpcBuyItem 表，有相同ID " + num);
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

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00008742 File Offset: 0x00006942
		public List<int> GetBuyList(int iID)
		{
			if (this.NpcBuyItem.ContainsKey(iID))
			{
				return this.NpcBuyItem[iID];
			}
			return null;
		}

		// Token: 0x04000B9B RID: 2971
		private static readonly NpcBuyItemManager instance = new NpcBuyItemManager();

		// Token: 0x04000B9C RID: 2972
		private Dictionary<int, List<int>> NpcBuyItem;
	}
}
