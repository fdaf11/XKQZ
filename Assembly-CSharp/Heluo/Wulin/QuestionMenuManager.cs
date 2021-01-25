using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x0200024B RID: 587
	public class QuestionMenuManager : TextDataManager
	{
		// Token: 0x06000ADE RID: 2782 RVA: 0x000089CC File Offset: 0x00006BCC
		public QuestionMenuManager()
		{
			this.m_QuestionMenuNodeList = new List<QuestionMenuNode>();
			this.m_LoadFileName = "QuestionMenu";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x00008A08 File Offset: 0x00006C08
		public static QuestionMenuManager Singleton
		{
			get
			{
				return QuestionMenuManager.instance;
			}
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x0005A624 File Offset: 0x00058824
		protected override void LoadFile(string filePath)
		{
			this.m_QuestionMenuNodeList.Clear();
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
							QuestionMenuNode questionMenuNode = new QuestionMenuNode();
							questionMenuNode.m_iQuestionGroupID = int.Parse(array3[0]);
							string[] array4 = array3[1].Split(new char[]
							{
								",".get_Chars(0)
							});
							string[] array5 = array3[2].Split(new char[]
							{
								",".get_Chars(0)
							});
							for (int j = 0; j < array4.Length; j++)
							{
								QuestionRewardNode questionRewardNode = new QuestionRewardNode();
								questionRewardNode.m_iAnswerAmount = int.Parse(array4[j]);
								questionRewardNode.m_iRewardID = int.Parse(array5[j]);
								questionMenuNode.m_QuestionRewardNodeList.Add(questionRewardNode);
							}
							array4 = array3[3].Split(new char[]
							{
								",".get_Chars(0)
							});
							questionMenuNode.m_iType = int.Parse(array4[0]);
							questionMenuNode.m_strID = array4[1];
							questionMenuNode.m_iQuestionAmount = int.Parse(array3[4]);
							this.m_QuestionMenuNodeList.Add(questionMenuNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0005A7DC File Offset: 0x000589DC
		public QuestionMenuNode GetQuestionMenuNode(int iID)
		{
			for (int i = 0; i < this.m_QuestionMenuNodeList.Count; i++)
			{
				if (this.m_QuestionMenuNodeList[i].m_iQuestionGroupID == iID)
				{
					return this.m_QuestionMenuNodeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000C2B RID: 3115
		private static readonly QuestionMenuManager instance = new QuestionMenuManager();

		// Token: 0x04000C2C RID: 3116
		private List<QuestionMenuNode> m_QuestionMenuNodeList;
	}
}
