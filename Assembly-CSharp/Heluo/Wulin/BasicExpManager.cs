using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001E1 RID: 481
	public class BasicExpManager : TextDataManager
	{
		// Token: 0x060009D3 RID: 2515 RVA: 0x00007EDB File Offset: 0x000060DB
		public BasicExpManager()
		{
			this.m_BasicExpNodeList = new List<BasicExpNode>();
			this.m_LoadFileName = "DevelopExp";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x00007F17 File Offset: 0x00006117
		public static BasicExpManager Singleton
		{
			get
			{
				return BasicExpManager.instance;
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x000533A0 File Offset: 0x000515A0
		protected override void LoadFile(string filePath)
		{
			this.m_BasicExpNodeList.Clear();
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
							BasicExpNode basicExpNode = new BasicExpNode();
							basicExpNode.m_iValue = int.Parse(array3[0]);
							basicExpNode.m_iAccumulationExp = int.Parse(array3[1]);
							this.m_BasicExpNodeList.Add(basicExpNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00053488 File Offset: 0x00051688
		public int CheckLv(int iLv, int iExp)
		{
			for (int i = this.m_BasicExpNodeList.Count - 1; i > 0; i--)
			{
				if (iExp >= this.m_BasicExpNodeList[i].m_iAccumulationExp)
				{
					return this.m_BasicExpNodeList[i].m_iValue;
				}
			}
			return iLv;
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x000534E0 File Offset: 0x000516E0
		public int GetExp(int iLv)
		{
			for (int i = 0; i < this.m_BasicExpNodeList.Count; i++)
			{
				if (this.m_BasicExpNodeList[i].m_iValue == iLv)
				{
					return this.m_BasicExpNodeList[i].m_iAccumulationExp;
				}
			}
			return 0;
		}

		// Token: 0x040009D0 RID: 2512
		private static readonly BasicExpManager instance = new BasicExpManager();

		// Token: 0x040009D1 RID: 2513
		public List<BasicExpNode> m_BasicExpNodeList;
	}
}
