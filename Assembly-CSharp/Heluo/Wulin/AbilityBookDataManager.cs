using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001D8 RID: 472
	public class AbilityBookDataManager : TextDataManager
	{
		// Token: 0x060009AB RID: 2475 RVA: 0x00007D99 File Offset: 0x00005F99
		public AbilityBookDataManager()
		{
			this.m_AbilityBookNodeList = new List<BookNode>();
			this.m_LoadFileName = "AbilityBookData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x00007DD5 File Offset: 0x00005FD5
		public static AbilityBookDataManager Singleton
		{
			get
			{
				return AbilityBookDataManager.instance;
			}
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00051E04 File Offset: 0x00050004
		protected override void LoadFile(string filePath)
		{
			this.m_AbilityBookNodeList.Clear();
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
							BookNode bookNode = new BookNode();
							for (int j = 0; j < array3.Length; j++)
							{
								this.generateData(j, array3[j], bookNode);
							}
							this.m_AbilityBookNodeList.Add(bookNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00051EF4 File Offset: 0x000500F4
		private void generateData(int idx, string data, BookNode node)
		{
			data = data.Replace("\r", string.Empty);
			switch (idx)
			{
			case 0:
				node.m_iID = base.PraseInt(data);
				break;
			case 1:
				node.m_iAbilityType = base.PraseInt(data);
				break;
			case 2:
				node.m_strAbilityID = data;
				break;
			case 3:
				node.m_strBookMsg = data;
				break;
			case 4:
				node.m_strBookImage = data;
				break;
			case 5:
				node.m_sValueLink = data;
				break;
			case 6:
				node.m_iSkillful = 0;
				break;
			}
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00051FA0 File Offset: 0x000501A0
		public List<BookNode> GetBookNodeList(int type)
		{
			List<BookNode> list = new List<BookNode>();
			for (int i = 0; i < this.m_AbilityBookNodeList.Count; i++)
			{
				if (this.m_AbilityBookNodeList[i].m_iAbilityType == type && Game.Variable["Ability_" + this.m_AbilityBookNodeList[i].m_iID] != -1)
				{
					list.Add(this.m_AbilityBookNodeList[i]);
				}
			}
			return list;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0005202C File Offset: 0x0005022C
		public BookNode GetAbilityNode(int iIndex)
		{
			for (int i = 0; i < this.m_AbilityBookNodeList.Count; i++)
			{
				if (this.m_AbilityBookNodeList[i].m_iID == iIndex)
				{
					return this.m_AbilityBookNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0005207C File Offset: 0x0005027C
		public string GetAbilityName(int iIndex)
		{
			for (int i = 0; i < this.m_AbilityBookNodeList.Count; i++)
			{
				if (this.m_AbilityBookNodeList[i].m_iID == iIndex)
				{
					return this.m_AbilityBookNodeList[i].m_strAbilityID;
				}
			}
			return string.Empty;
		}

		// Token: 0x040009A7 RID: 2471
		private static readonly AbilityBookDataManager instance = new AbilityBookDataManager();

		// Token: 0x040009A8 RID: 2472
		private List<BookNode> m_AbilityBookNodeList;
	}
}
