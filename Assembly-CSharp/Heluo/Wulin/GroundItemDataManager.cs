using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000202 RID: 514
	public class GroundItemDataManager : TextDataManager
	{
		// Token: 0x06000A23 RID: 2595 RVA: 0x000081E2 File Offset: 0x000063E2
		public GroundItemDataManager()
		{
			this.m_GroundItemDataList = new Dictionary<string, GroundItemData>();
			this.m_LoadFileName = "GroundItemData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x0000821E File Offset: 0x0000641E
		public static GroundItemDataManager Singleton
		{
			get
			{
				return GroundItemDataManager.instance;
			}
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x000566EC File Offset: 0x000548EC
		protected override void LoadFile(string filePath)
		{
			this.m_GroundItemDataList.Clear();
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
							string text2 = text.Replace("\r", string.Empty);
							string[] array3 = text2.Split(new char[]
							{
								"\t".get_Chars(0)
							});
							GroundItemData groundItemData = new GroundItemData();
							for (int j = 0; j < array3.Length; j++)
							{
								this.generateData(j, array3[j], groundItemData);
							}
							if (!this.m_GroundItemDataList.ContainsKey(groundItemData.GruondItemID))
							{
								this.m_GroundItemDataList.Add(groundItemData.GruondItemID, groundItemData);
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

		// Token: 0x06000A27 RID: 2599 RVA: 0x0005680C File Offset: 0x00054A0C
		private void generateData(int idx, string data, GroundItemData node)
		{
			switch (idx)
			{
			case 0:
				node.GruondItemID = data;
				break;
			case 1:
				node.ItemID = data;
				break;
			case 2:
				node.Amount = data;
				break;
			}
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00008225 File Offset: 0x00006425
		public GroundItemData GetGroundItemData(string key)
		{
			if (!this.m_GroundItemDataList.ContainsKey(key))
			{
				return null;
			}
			return this.m_GroundItemDataList[key];
		}

		// Token: 0x04000A8E RID: 2702
		private static readonly GroundItemDataManager instance = new GroundItemDataManager();

		// Token: 0x04000A8F RID: 2703
		private Dictionary<string, GroundItemData> m_GroundItemDataList;
	}
}
