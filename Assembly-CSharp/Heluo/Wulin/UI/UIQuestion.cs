using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200031D RID: 797
	public class UIQuestion : UILayer
	{
		// Token: 0x06001149 RID: 4425 RVA: 0x0000B472 File Offset: 0x00009672
		protected override void Awake()
		{
			this.m_BtnAnswerList = new List<AnswerBtnNode>();
			this.m_QuestionNodeList = new List<QuestionNode>();
			base.Awake();
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0000B490 File Offset: 0x00009690
		private void Start()
		{
			this.m_QuestionMenuNode = null;
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x00094B28 File Offset: 0x00092D28
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIQuestion.<>f__switch$map15 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(5);
					dictionary.Add("Group", 0);
					dictionary.Add("LabelTopic", 1);
					dictionary.Add("BtnAnswer", 2);
					dictionary.Add("LabelEnd", 3);
					dictionary.Add("LabelAnswer", 4);
					UIQuestion.<>f__switch$map15 = dictionary;
				}
				int num;
				if (UIQuestion.<>f__switch$map15.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						this.m_Group.OnClick += this.EndOnClick;
						break;
					case 1:
						this.m_LabelTopic = sender;
						break;
					case 2:
					{
						AnswerBtnNode answerBtnNode = new AnswerBtnNode();
						answerBtnNode.m_BtnAnswer = sender;
						answerBtnNode.m_BtnAnswer.OnHover += this.BtnAnswerOnHover;
						answerBtnNode.m_BtnAnswer.OnClick += this.BtnAnswerOnClick;
						this.m_BtnAnswerList.Add(answerBtnNode);
						break;
					}
					case 3:
						this.m_LabelEnd = sender;
						this.m_LabelEnd.OnClick += this.EndOnClick;
						break;
					case 4:
						this.m_LabelAnswer = sender;
						this.m_LabelAnswer.OnClick += this.LabelAnswerOnClick;
						break;
					}
				}
			}
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x00094CA0 File Offset: 0x00092EA0
		private void EndOnClick(GameObject go)
		{
			if (this.m_LabelEnd.GameObject.activeSelf)
			{
				GameGlobal.m_bCFormOpen = false;
				this.m_Group.GameObject.SetActive(false);
				int iRewardID = 0;
				for (int i = 0; i < this.m_QuestionMenuNode.m_QuestionRewardNodeList.Count; i++)
				{
					if (this.m_iRight < this.m_QuestionMenuNode.m_QuestionRewardNodeList[i].m_iAnswerAmount && i > 0)
					{
						iRewardID = this.m_QuestionMenuNode.m_QuestionRewardNodeList[i - 1].m_iRewardID;
						break;
					}
					if (this.m_iRight == this.m_QuestionMenuNode.m_QuestionRewardNodeList[i].m_iAnswerAmount && i == this.m_QuestionMenuNode.m_QuestionRewardNodeList.Count - 1)
					{
						iRewardID = this.m_QuestionMenuNode.m_QuestionRewardNodeList[i].m_iRewardID;
					}
				}
				if (this.m_iOpenType == 0)
				{
					Game.RewardData.DoRewardID(iRewardID, null);
					if (this.m_QuestionMenuNode.m_iType == 2)
					{
						GameObject[] array = GameObject.FindGameObjectsWithTag("cForm");
						for (int j = 0; j < array.Length; j++)
						{
							if (array[j].name.Equals("cFormTalk"))
							{
								array[j].GetComponent<UITalk>().SetSelectTalk(this.m_QuestionMenuNode.m_strID);
								break;
							}
						}
					}
					if (this.m_QuestionMenuNode.m_iType == 4 && this.OnMovieQuestionClick != null)
					{
						this.OnMovieQuestionClick(go);
					}
				}
			}
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x0000B499 File Offset: 0x00009699
		public void StartMovieQuestionEvent(UIEventListener.VoidDelegate callback)
		{
			Debug.Log("StartMovieQuestionEvent");
			this.OnMovieQuestionClick = callback;
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0000B4AC File Offset: 0x000096AC
		public void EndMovieQuestionEvnet()
		{
			Debug.Log("EndMovieQuestionEvnet");
			this.OnMovieQuestionClick = null;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00094E38 File Offset: 0x00093038
		public void SetQuestion(int iID, int iType)
		{
			this.m_iOpenType = iType;
			GameGlobal.m_bCFormOpen = true;
			this.m_Group.GameObject.SetActive(true);
			this.m_iIndex = 0;
			this.m_iRight = 0;
			this.m_LabelEnd.GameObject.SetActive(false);
			this.m_QuestionMenuNode = Game.QuestionMenu.GetQuestionMenuNode(iID);
			QuestionGroupNode questionGroupNode = Game.QuestionData.GetQuestionGroupNode(iID);
			List<QuestionNode> list = new List<QuestionNode>();
			for (int i = 0; i < questionGroupNode.m_QuestionNodeList.Count; i++)
			{
				list.Add(questionGroupNode.m_QuestionNodeList[i]);
			}
			this.m_QuestionNodeList.Clear();
			for (int j = 0; j < this.m_QuestionMenuNode.m_iQuestionAmount; j++)
			{
				int num = Random.Range(0, list.Count);
				this.m_QuestionNodeList.Add(list[num]);
				list.RemoveAt(num);
			}
			this.SetQuestionData();
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00094F2C File Offset: 0x0009312C
		private void SetQuestionData()
		{
			this.m_LabelAnswer.GameObject.SetActive(false);
			this.m_LabelTopic.GameObject.SetActive(true);
			this.m_LabelTopic.Text = this.m_QuestionNodeList[this.m_iIndex].m_strQuestionMsg;
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < this.m_BtnAnswerList.Count; j++)
				{
					int iBtnID = this.m_BtnAnswerList[j].m_BtnAnswer.GetComponent<BtnData>().m_iBtnID;
					if (iBtnID == i)
					{
						this.m_BtnAnswerList[j].m_BtnAnswer.GameObject.SetActive(true);
						int num = Random.Range(0, this.m_QuestionNodeList[this.m_iIndex].m_QuestionBtnList.Count);
						this.m_BtnAnswerList[j].m_BtnAnswer.Text = "[000000]" + this.m_astrNumList[i];
						Control btnAnswer = this.m_BtnAnswerList[j].m_BtnAnswer;
						btnAnswer.Text = btnAnswer.Text + this.m_QuestionNodeList[this.m_iIndex].m_QuestionBtnList[num].m_strBtnName + "[-]";
						this.m_BtnAnswerList[j].m_iAnswerType = this.m_QuestionNodeList[this.m_iIndex].m_QuestionBtnList[num].m_iAnswerType;
						this.m_QuestionNodeList[this.m_iIndex].m_QuestionBtnList.RemoveAt(num);
						this.m_BtnAnswerList[j].m_BtnAnswer.GetComponent<UILabel>().width = (int)this.m_BtnAnswerList[j].m_BtnAnswer.GetComponent<UILabel>().printedSize.x;
						this.m_BtnAnswerList[j].m_BtnAnswer.GameObject.GetComponent<BoxCollider>().size = this.m_BtnAnswerList[j].m_BtnAnswer.GetComponent<UILabel>().printedSize;
						break;
					}
				}
			}
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00095150 File Offset: 0x00093350
		private void ShowAnswer()
		{
			this.m_LabelAnswer.GameObject.SetActive(true);
			string text = string.Empty;
			if (this.m_bRight)
			{
				text = Game.StringTable.GetString(270002);
			}
			else
			{
				for (int i = 0; i < this.m_BtnAnswerList.Count; i++)
				{
					if (this.m_BtnAnswerList[i].m_iAnswerType == 1)
					{
						text = Game.StringTable.GetString(270001);
						text += this.m_BtnAnswerList[i].m_BtnAnswer.Text;
						break;
					}
				}
			}
			this.m_LabelAnswer.Text = text;
			this.m_bRight = false;
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0009520C File Offset: 0x0009340C
		private void LabelAnswerOnClick(GameObject go)
		{
			if (this.m_QuestionMenuNode.m_iQuestionAmount == this.m_iIndex)
			{
				this.m_LabelAnswer.GameObject.SetActive(false);
				for (int i = 0; i < this.m_BtnAnswerList.Count; i++)
				{
					this.m_BtnAnswerList[i].m_BtnAnswer.Text = string.Empty;
				}
				this.m_LabelTopic.Text = string.Empty;
				string @string = Game.StringTable.GetString(200096);
				this.m_LabelEnd.Text = string.Format(@string, this.m_iRight);
				this.m_LabelEnd.GameObject.SetActive(true);
			}
			else
			{
				this.SetQuestionData();
			}
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x000952D0 File Offset: 0x000934D0
		private void BtnAnswerOnClick(GameObject go)
		{
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			for (int i = 0; i < this.m_BtnAnswerList.Count; i++)
			{
				this.m_BtnAnswerList[i].m_BtnAnswer.GetComponent<UILabel>().width = 608;
				if (this.m_BtnAnswerList[i].m_BtnAnswer.GetComponent<BtnData>().m_iBtnID == iBtnID && this.m_BtnAnswerList[i].m_iAnswerType == 1)
				{
					this.m_bRight = true;
					this.m_iRight++;
				}
			}
			this.m_iIndex++;
			this.m_LabelTopic.GameObject.SetActive(false);
			for (int j = 0; j < this.m_BtnAnswerList.Count; j++)
			{
				this.m_BtnAnswerList[j].m_BtnAnswer.GameObject.SetActive(false);
			}
			this.ShowAnswer();
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x000953D0 File Offset: 0x000935D0
		private void BtnAnswerOnHover(GameObject go, bool bHover)
		{
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			for (int i = 0; i < this.m_BtnAnswerList.Count; i++)
			{
				int iBtnID2 = this.m_BtnAnswerList[i].m_BtnAnswer.GetComponent<BtnData>().m_iBtnID;
				if (iBtnID2 == iBtnID)
				{
					if (bHover)
					{
						this.m_BtnAnswerList[i].m_BtnAnswer.Text = TextParser.ChangeColor(this.m_BtnAnswerList[i].m_BtnAnswer.Text, "[A50000]");
					}
					else
					{
						this.m_BtnAnswerList[i].m_BtnAnswer.Text = TextParser.ChangeColor(this.m_BtnAnswerList[i].m_BtnAnswer.Text, "[000000]");
					}
				}
				else
				{
					this.m_BtnAnswerList[i].m_BtnAnswer.Text = TextParser.ChangeColor(this.m_BtnAnswerList[i].m_BtnAnswer.Text, "[000000]");
				}
			}
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x040014F7 RID: 5367
		private Control m_Group;

		// Token: 0x040014F8 RID: 5368
		private Control m_LabelTopic;

		// Token: 0x040014F9 RID: 5369
		private Control m_LabelEnd;

		// Token: 0x040014FA RID: 5370
		private Control m_LabelAnswer;

		// Token: 0x040014FB RID: 5371
		private List<AnswerBtnNode> m_BtnAnswerList;

		// Token: 0x040014FC RID: 5372
		private List<QuestionNode> m_QuestionNodeList;

		// Token: 0x040014FD RID: 5373
		private bool m_bRight;

		// Token: 0x040014FE RID: 5374
		private int m_iIndex;

		// Token: 0x040014FF RID: 5375
		private int m_iRight;

		// Token: 0x04001500 RID: 5376
		private int m_iOpenType;

		// Token: 0x04001501 RID: 5377
		private QuestionMenuNode m_QuestionMenuNode;

		// Token: 0x04001502 RID: 5378
		private string[] m_astrNumList = new string[]
		{
			"一. ",
			"二. ",
			"三. ",
			"四. "
		};

		// Token: 0x04001503 RID: 5379
		private UIEventListener.VoidDelegate OnMovieQuestionClick;
	}
}
