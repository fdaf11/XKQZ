using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000248 RID: 584
	public class QuestionDataManager : TextDataManager
	{
		// Token: 0x06000AD6 RID: 2774 RVA: 0x00008976 File Offset: 0x00006B76
		public QuestionDataManager()
		{
			this.m_QuestionGroupNodeeList = new List<QuestionGroupNode>();
			this.m_LoadFileName = "QuestionData";
			this.LoadFile(this.m_LoadFileName);
			TextDataManager.AddTextDataToList(this);
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x000089B2 File Offset: 0x00006BB2
		public static QuestionDataManager Singleton
		{
			get
			{
				return QuestionDataManager.instance;
			}
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0005A43C File Offset: 0x0005863C
		protected override void LoadFile(string filePath)
		{
			this.m_QuestionGroupNodeeList.Clear();
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
							QuestionGroupNode questionGroupNode = this.CheckQuestionGroupNode(int.Parse(array3[0]));
							QuestionNode questionNode = new QuestionNode();
							questionNode.m_strQuestionMsg = array3[1];
							for (int j = 2; j < array3.Length; j += 2)
							{
								QuestionBtnNode questionBtnNode = new QuestionBtnNode();
								questionBtnNode.m_strBtnName = array3[j];
								questionBtnNode.m_iAnswerType = int.Parse(array3[j + 1]);
								questionNode.m_QuestionBtnList.Add(questionBtnNode);
							}
							questionGroupNode.m_QuestionNodeList.Add(questionNode);
						}
						catch
						{
							Debug.LogError("解析 " + filePath + " 時發生錯誤 : " + text);
						}
					}
				}
			}
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0005A56C File Offset: 0x0005876C
		private QuestionGroupNode CheckQuestionGroupNode(int iID)
		{
			for (int i = 0; i < this.m_QuestionGroupNodeeList.Count; i++)
			{
				if (this.m_QuestionGroupNodeeList[i].m_iQuestionGroupID == iID)
				{
					return this.m_QuestionGroupNodeeList[i];
				}
			}
			QuestionGroupNode questionGroupNode = new QuestionGroupNode();
			questionGroupNode.m_iQuestionGroupID = iID;
			this.m_QuestionGroupNodeeList.Add(questionGroupNode);
			return questionGroupNode;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0005A5D4 File Offset: 0x000587D4
		public QuestionGroupNode GetQuestionGroupNode(int iID)
		{
			for (int i = 0; i < this.m_QuestionGroupNodeeList.Count; i++)
			{
				if (this.m_QuestionGroupNodeeList[i].m_iQuestionGroupID == iID)
				{
					return this.m_QuestionGroupNodeeList[i];
				}
			}
			return null;
		}

		// Token: 0x04000C22 RID: 3106
		private static readonly QuestionDataManager instance = new QuestionDataManager();

		// Token: 0x04000C23 RID: 3107
		private List<QuestionGroupNode> m_QuestionGroupNodeeList;
	}
}
