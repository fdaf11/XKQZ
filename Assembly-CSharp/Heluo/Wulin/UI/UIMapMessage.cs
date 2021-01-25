using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000314 RID: 788
	public class UIMapMessage : UILayer
	{
		// Token: 0x06001104 RID: 4356 RVA: 0x0000B262 File Offset: 0x00009462
		protected override void Awake()
		{
			this.m_MessageLabelList = new List<MessageLblNode>();
			this.m_StrMsgList = new List<string>();
			base.Awake();
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0000B280 File Offset: 0x00009480
		private void Start()
		{
			this.m_fNextTime = 0f;
			this.m_iMessageIndex = 0;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0009285C File Offset: 0x00090A5C
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIMapMessage.<>f__switch$map11 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
					dictionary.Add("MessageLabel", 0);
					UIMapMessage.<>f__switch$map11 = dictionary;
				}
				int num;
				if (UIMapMessage.<>f__switch$map11.TryGetValue(name, ref num))
				{
					if (num == 0)
					{
						Control messageLabel = sender;
						MessageLblNode messageLblNode = new MessageLblNode();
						messageLblNode.m_MessageLabel = messageLabel;
						messageLblNode.m_MessageLabel.Text = string.Empty;
						this.m_MessageLabelList.Add(messageLblNode);
					}
				}
			}
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0000B294 File Offset: 0x00009494
		public void SetMsg(string strMsg)
		{
			if (strMsg.Equals(string.Empty))
			{
				return;
			}
			this.m_StrMsgList.Add(strMsg);
			this.m_bMsg = true;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x000928EC File Offset: 0x00090AEC
		private void Update()
		{
			if (this.m_bMsg)
			{
				if (this.m_iMessageIndex < this.m_StrMsgList.Count && (double)(Time.time - this.m_fNextTime) > 0.4)
				{
					this.m_fNextTime = Time.time;
					for (int i = this.m_MessageLabelList.Count - 1; i > 0; i--)
					{
						this.m_MessageLabelList[i].m_MessageLabel.Text = this.m_MessageLabelList[i - 1].m_MessageLabel.Text;
						this.m_MessageLabelList[i].m_fStartTime = this.m_MessageLabelList[i - 1].m_fStartTime;
					}
					this.m_MessageLabelList[0].m_MessageLabel.Text = this.m_StrMsgList[this.m_iMessageIndex];
					this.m_MessageLabelList[0].m_fStartTime = Time.time;
					this.m_iMessageIndex++;
				}
				if (this.m_MessageLabelList[0].m_MessageLabel.Text.Equals(string.Empty) && this.m_fNextTime != 0f)
				{
					this.m_bMsg = false;
					this.m_fNextTime = 0f;
					this.m_iMessageIndex = 0;
					this.m_StrMsgList.Clear();
				}
				for (int j = this.m_MessageLabelList.Count - 1; j >= 0; j--)
				{
					if (!this.m_MessageLabelList[j].m_MessageLabel.Text.Equals(string.Empty))
					{
						if (Time.time - this.m_MessageLabelList[j].m_fStartTime > 2f)
						{
							this.m_MessageLabelList[j].m_MessageLabel.Text = string.Empty;
						}
						else
						{
							float num = Time.time - this.m_MessageLabelList[j].m_fStartTime;
							this.m_MessageLabelList[j].m_MessageLabel.GetComponent<UILabel>().alpha = (2f - num) / 1f;
						}
					}
				}
			}
		}

		// Token: 0x0400148D RID: 5261
		private List<MessageLblNode> m_MessageLabelList;

		// Token: 0x0400148E RID: 5262
		private List<string> m_StrMsgList;

		// Token: 0x0400148F RID: 5263
		private bool m_bMsg;

		// Token: 0x04001490 RID: 5264
		private float m_fNextTime;

		// Token: 0x04001491 RID: 5265
		private int m_iMessageIndex;
	}
}
