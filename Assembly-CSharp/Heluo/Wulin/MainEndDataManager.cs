using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000217 RID: 535
	public class MainEndDataManager : TextDataManager
	{
		// Token: 0x06000A53 RID: 2643 RVA: 0x000083A4 File Offset: 0x000065A4
		public MainEndDataManager()
		{
			this.m_MainEndDataNodeList = new List<MainEndDataNode>();
			this.m_LoadFileName = "MainEndData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x000083E0 File Offset: 0x000065E0
		public static MainEndDataManager Singleton
		{
			get
			{
				return MainEndDataManager.instance;
			}
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0005771C File Offset: 0x0005591C
		protected override void LoadFile(string filePath)
		{
			this.m_MainEndDataNodeList.Clear();
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
							MainEndDataNode mainEndDataNode = new MainEndDataNode();
							mainEndDataNode.m_iEndID = int.Parse(array3[0]);
							mainEndDataNode.m_strMusic = array3[1];
							mainEndDataNode.m_iChildEndPlay = int.Parse(array3[2]);
							mainEndDataNode.m_strDesc = array3[3].Replace("<br>", "\n");
							this.m_MainEndDataNodeList.Add(mainEndDataNode);
						}
						catch (Exception ex)
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00057828 File Offset: 0x00055A28
		public MainEndDataNode GetMainEndDataNode(int iID)
		{
			for (int i = 0; i < this.m_MainEndDataNodeList.Count; i++)
			{
				if (iID == this.m_MainEndDataNodeList[i].m_iEndID)
				{
					return this.m_MainEndDataNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000B0E RID: 2830
		private static readonly MainEndDataManager instance = new MainEndDataManager();

		// Token: 0x04000B0F RID: 2831
		private List<MainEndDataNode> m_MainEndDataNodeList;
	}
}
