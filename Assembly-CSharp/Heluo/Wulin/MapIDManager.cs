using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200021A RID: 538
	public class MapIDManager : TextDataManager
	{
		// Token: 0x06000A59 RID: 2649 RVA: 0x00008410 File Offset: 0x00006610
		public MapIDManager()
		{
			this.m_MapIDNodeList = new List<MapIDNode>();
			this.m_LoadFileName = "MapID";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0000844C File Offset: 0x0000664C
		public static MapIDManager Singleton
		{
			get
			{
				return MapIDManager.instance;
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00057878 File Offset: 0x00055A78
		protected override void LoadFile(string filePath)
		{
			this.m_MapIDNodeList.Clear();
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
							MapIDNode mapIDNode = new MapIDNode();
							for (int j = 0; j < array3.Length; j++)
							{
								this.generateData(j, array3[j], mapIDNode);
							}
							this.m_MapIDNodeList.Add(mapIDNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0005797C File Offset: 0x00055B7C
		private void generateData(int idx, string data, MapIDNode node)
		{
			switch (idx)
			{
			case 0:
				node.m_strMapID = data;
				break;
			case 1:
				node.m_strMapName = data;
				break;
			case 2:
				Condition.GenerateList(node.m_OpenCdn, data);
				break;
			case 3:
				Condition.GenerateList(node.m_CloseCdn, data);
				break;
			case 4:
				node.IsAllOpenCdn = base.PraseBool(data);
				break;
			case 5:
				node.IsAllCloseCdn = base.PraseBool(data);
				break;
			case 6:
			{
				if (data.Length <= 2)
				{
					return;
				}
				string text = data.Substring(1, data.Length - 2);
				string[] array = text.Split(new char[]
				{
					",".get_Chars(0)
				});
				float[] array2 = new float[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = array[i];
					array2[i] = base.PraseFloat(text2);
				}
				node.Pos = new Vector3(array2[0], array2[1], array2[2]);
				break;
			}
			case 7:
				node.AngleY = base.PraseFloat(data);
				break;
			case 8:
				if (data.Length > 2)
				{
					node.MapIcon = data;
				}
				break;
			case 9:
				node.Range = base.PraseFloat(data);
				break;
			}
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00057AE8 File Offset: 0x00055CE8
		public string GetMapName(string MapID)
		{
			for (int i = 0; i < this.m_MapIDNodeList.Count; i++)
			{
				if (this.m_MapIDNodeList[i].m_strMapID.Equals(MapID))
				{
					return this.m_MapIDNodeList[i].m_strMapName;
				}
			}
			return MapID;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00057B40 File Offset: 0x00055D40
		public MapIDNode GetMapIDNode(string MapID)
		{
			MapIDNode result = null;
			for (int i = 0; i < this.m_MapIDNodeList.Count; i++)
			{
				if (this.m_MapIDNodeList[i].m_strMapID.Equals(MapID))
				{
					result = this.m_MapIDNodeList[i];
					break;
				}
			}
			return result;
		}

		// Token: 0x04000B25 RID: 2853
		private static readonly MapIDManager instance = new MapIDManager();

		// Token: 0x04000B26 RID: 2854
		public List<MapIDNode> m_MapIDNodeList;
	}
}
