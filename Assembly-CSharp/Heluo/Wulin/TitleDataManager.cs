using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000266 RID: 614
	public class TitleDataManager : TextDataManager
	{
		// Token: 0x06000B49 RID: 2889 RVA: 0x00008DF3 File Offset: 0x00006FF3
		public TitleDataManager()
		{
			this.m_TitleDataNodeList = new List<TitleDataNode>();
			this.m_LoadFileName = "TitleData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00008E2F File Offset: 0x0000702F
		public static TitleDataManager Singleton
		{
			get
			{
				return TitleDataManager.instance;
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0005D35C File Offset: 0x0005B55C
		protected override void LoadFile(string filePath)
		{
			this.m_TitleDataNodeList.Clear();
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
							TitleDataNode titleDataNode = new TitleDataNode();
							titleDataNode.m_iTitleID = int.Parse(array3[0]);
							titleDataNode.m_strTitleNaem = array3[1];
							titleDataNode.m_strTitleExplanation = array3[2];
							this.m_TitleDataNodeList.Add(titleDataNode);
						}
						catch (Exception ex)
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0005D44C File Offset: 0x0005B64C
		public string GetTitleName(int iID)
		{
			for (int i = 0; i < this.m_TitleDataNodeList.Count; i++)
			{
				if (this.m_TitleDataNodeList[i].m_iTitleID == iID)
				{
					return this.m_TitleDataNodeList[i].m_strTitleNaem;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0005D4A4 File Offset: 0x0005B6A4
		public string GetTitleExplanation(int iID)
		{
			for (int i = 0; i < this.m_TitleDataNodeList.Count; i++)
			{
				if (this.m_TitleDataNodeList[i].m_iTitleID == iID)
				{
					return this.m_TitleDataNodeList[i].m_strTitleExplanation;
				}
			}
			return string.Empty;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0005D4FC File Offset: 0x0005B6FC
		public TitleDataNode GetTitleDataNode(int iID)
		{
			for (int i = 0; i < this.m_TitleDataNodeList.Count; i++)
			{
				if (this.m_TitleDataNodeList[i].m_iTitleID == iID)
				{
					return this.m_TitleDataNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000D17 RID: 3351
		private static readonly TitleDataManager instance = new TitleDataManager();

		// Token: 0x04000D18 RID: 3352
		public List<TitleDataNode> m_TitleDataNodeList;
	}
}
