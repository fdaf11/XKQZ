using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x020001FD RID: 509
	public class ChildEndDataManager : TextDataManager
	{
		// Token: 0x06000A16 RID: 2582 RVA: 0x0000812E File Offset: 0x0000632E
		public ChildEndDataManager()
		{
			this.m_ChildEndDataNodeList = new List<ChildEndDataNode>();
			this.m_LoadFileName = "ChildEndData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0000816A File Offset: 0x0000636A
		public static ChildEndDataManager Singleton
		{
			get
			{
				return ChildEndDataManager.instance;
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x000563CC File Offset: 0x000545CC
		protected override void LoadFile(string filePath)
		{
			this.m_ChildEndDataNodeList.Clear();
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
							ChildEndDataNode childEndDataNode = new ChildEndDataNode();
							childEndDataNode.m_iEndID = int.Parse(array3[0]);
							Condition.GenerateList(childEndDataNode.m_OpenConditionlist, array3[1]);
							childEndDataNode.m_strDesc = array3[2].Replace("<br>", "\n");
							this.m_ChildEndDataNodeList.Add(childEndDataNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x000564CC File Offset: 0x000546CC
		public List<EndMovieData> CheckEnd()
		{
			List<EndMovieData> list = new List<EndMovieData>();
			for (int i = 0; i < this.m_ChildEndDataNodeList.Count; i++)
			{
				bool flag = ConditionManager.CheckCondition(this.m_ChildEndDataNodeList[i].m_OpenConditionlist, true, 0, string.Empty);
				if (flag)
				{
					EndMovieData endMovieData = new EndMovieData
					{
						movieId = this.m_ChildEndDataNodeList[i].m_iEndID,
						endDesc = this.m_ChildEndDataNodeList[i].m_strDesc
					};
					list.Add(endMovieData);
				}
			}
			return list;
		}

		// Token: 0x04000A7E RID: 2686
		private static readonly ChildEndDataManager instance = new ChildEndDataManager();

		// Token: 0x04000A7F RID: 2687
		private List<ChildEndDataNode> m_ChildEndDataNodeList;
	}
}
