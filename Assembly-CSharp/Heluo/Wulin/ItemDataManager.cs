using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200020D RID: 525
	public class ItemDataManager : TextDataManager
	{
		// Token: 0x06000A32 RID: 2610 RVA: 0x00056AF4 File Offset: 0x00054CF4
		public ItemDataManager()
		{
			this.m_ItemDataNodeList = new List<ItemDataNode>();
			this.m_LoadFileName = "ItemData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
			TextDataManager.AddDLCTextDataToList(this);
			this.m_ItemDataNodeList.Sort((ItemDataNode A, ItemDataNode B) => A.m_iItemID.CompareTo(B.m_iItemID));
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x00008291 File Offset: 0x00006491
		public static ItemDataManager Singleton
		{
			get
			{
				return ItemDataManager.instance;
			}
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x00056B60 File Offset: 0x00054D60
		protected override void LoadFile(string filePath)
		{
			this.m_ItemDataNodeList.Clear();
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
							ItemDataNode itemDataNode = new ItemDataNode();
							itemDataNode.m_iItemID = int.Parse(array3[0]);
							itemDataNode.m_strItemName = array3[1];
							itemDataNode.m_iItemType = int.Parse(array3[2]);
							itemDataNode.m_iItemKind = int.Parse(array3[3]);
							itemDataNode.m_strIcon = array3[4];
							itemDataNode.m_strTip = array3[5].Replace("<br>", "\n");
							itemDataNode.m_iItemBuy = int.Parse(array3[6]);
							itemDataNode.m_iItemSell = int.Parse(array3[7]);
							itemDataNode.m_iItemStack = int.Parse(array3[8]);
							itemDataNode.m_iMixStack = int.Parse(array3[9]);
							itemDataNode.m_iItemUse = int.Parse(array3[10]);
							itemDataNode.m_iUseTime = int.Parse(array3[11]);
							itemDataNode.m_iAddType = int.Parse(array3[12]);
							for (int j = 13; j < 22; j += 4)
							{
								int num = 0;
								if (!int.TryParse(array3[j], ref num))
								{
									num = 0;
								}
								if (num != 0)
								{
									ItmeEffectNode itmeEffectNode = new ItmeEffectNode();
									itmeEffectNode.m_iItemType = int.Parse(array3[j]);
									itmeEffectNode.m_iRecoverType = int.Parse(array3[j + 1]);
									itmeEffectNode.m_iValue = int.Parse(array3[j + 2]);
									itmeEffectNode.m_iMsgID = int.Parse(array3[j + 3]);
									itemDataNode.m_ItmeEffectNodeList.Add(itmeEffectNode);
								}
							}
							itemDataNode.m_iUseMsgID = int.Parse(array3[25]);
							int amsType = 0;
							if (int.TryParse(array3[26], ref amsType))
							{
								itemDataNode.m_AmsType = (WeaponType)amsType;
							}
							if (!array3[27].Equals("0"))
							{
								string text2 = array3[27];
								string[] array4 = text2.Split(new char[]
								{
									",".get_Chars(0)
								});
								for (int k = 0; k < array4.Length; k++)
								{
									int num2 = 0;
									if (int.TryParse(array4[k], ref num2))
									{
										if (!itemDataNode.m_NoUseNpcList.Contains(num2))
										{
											itemDataNode.m_NoUseNpcList.Add(num2);
										}
									}
								}
							}
							if (!array3[28].Equals("0"))
							{
								string text3 = array3[28];
								string[] array5 = text3.Split(new char[]
								{
									",".get_Chars(0)
								});
								for (int l = 0; l < array5.Length; l++)
								{
									itemDataNode.m_CanUseiNpcIDList.Add(int.Parse(array5[l]));
								}
							}
							if (!array3[29].Equals("0"))
							{
								UseLimitNode.GreateData(itemDataNode.m_UseLimitNodeList, array3[29]);
							}
							itemDataNode.m_strStatusTip = array3[30];
							string text4 = array3[31].Replace("\r", string.Empty);
							itemDataNode.m_iLock = int.Parse(text4);
							this.m_ItemDataNodeList.Add(itemDataNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00056F00 File Offset: 0x00055100
		public ItemDataNode GetItemDataNode(string ID)
		{
			int iID = 0;
			if (int.TryParse(ID, ref iID))
			{
				return this.GetItemDataNode(iID);
			}
			return null;
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x00056F28 File Offset: 0x00055128
		private int BinarySearch(int id)
		{
			List<ItemDataNode> itemDataNodeList = this.m_ItemDataNodeList;
			int i = 0;
			int num = itemDataNodeList.Count - 1;
			while (i <= num)
			{
				int num2 = (i + num) / 2;
				if (itemDataNodeList[num2].m_iItemID == id)
				{
					return num2;
				}
				if (itemDataNodeList[num2].m_iItemID < id)
				{
					i = num2 + 1;
				}
				else
				{
					num = num2 - 1;
				}
			}
			return -1;
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00056F90 File Offset: 0x00055190
		public ItemDataNode GetItemDataNode(int iID)
		{
			for (int i = 0; i < this.m_ItemDataNodeList.Count; i++)
			{
				if (this.m_ItemDataNodeList[i].m_iItemID == iID)
				{
					return this.m_ItemDataNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00056FE0 File Offset: 0x000551E0
		public int GetItemID(string strIcon)
		{
			for (int i = 0; i < this.m_ItemDataNodeList.Count; i++)
			{
				if (this.m_ItemDataNodeList[i].m_strIcon.Equals(strIcon))
				{
					return this.m_ItemDataNodeList[i].m_iItemID;
				}
			}
			return 0;
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00057038 File Offset: 0x00055238
		public string GetItemIconID(string strName)
		{
			for (int i = 0; i < this.m_ItemDataNodeList.Count; i++)
			{
				if (this.m_ItemDataNodeList[i].m_strItemName.Equals(strName))
				{
					return this.m_ItemDataNodeList[i].m_strIcon;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00057094 File Offset: 0x00055294
		public string GetItemName(int iID)
		{
			for (int i = 0; i < this.m_ItemDataNodeList.Count; i++)
			{
				if (this.m_ItemDataNodeList[i].m_iItemID == iID)
				{
					return this.m_ItemDataNodeList[i].m_strItemName;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x000570EC File Offset: 0x000552EC
		public string GetItemIconID(int iID)
		{
			for (int i = 0; i < this.m_ItemDataNodeList.Count; i++)
			{
				if (this.m_ItemDataNodeList[i].m_iItemID == iID)
				{
					return this.m_ItemDataNodeList[i].m_strIcon;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00008298 File Offset: 0x00006498
		public List<ItemDataNode> GetItemList()
		{
			return this.m_ItemDataNodeList;
		}

		// Token: 0x04000AE0 RID: 2784
		private static readonly ItemDataManager instance = new ItemDataManager();

		// Token: 0x04000AE1 RID: 2785
		private List<ItemDataNode> m_ItemDataNodeList;
	}
}
