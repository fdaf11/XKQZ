using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200037D RID: 893
	public class UIWordImage : UILayer
	{
		// Token: 0x060014DC RID: 5340 RVA: 0x0000D589 File Offset: 0x0000B789
		private void Start()
		{
			this.m_iNowType = 0;
			this.m_bOutSide = false;
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x000B4194 File Offset: 0x000B2394
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIWordImage.<>f__switch$map63 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("Group", 0);
					dictionary.Add("WordBase", 1);
					dictionary.Add("LabelShow", 2);
					UIWordImage.<>f__switch$map63 = dictionary;
				}
				int num;
				if (UIWordImage.<>f__switch$map63.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_WordBase = sender;
						break;
					case 2:
						this.m_LabelShow = sender;
						break;
					}
				}
			}
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x000B4248 File Offset: 0x000B2448
		public void ShowTheWord(string strWord)
		{
			if (this.m_iNowType == 0)
			{
				this.m_bOutSide = true;
				return;
			}
			this.m_LabelShow.Text = strWord;
			this.m_Group.GameObject.SetActive(true);
			this.m_WordBase.GetComponent<TweenAlpha>().ResetToBeginning();
			this.m_WordBase.GetComponent<TweenAlpha>().Play();
			this.m_iNowType = 0;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x0000D599 File Offset: 0x0000B799
		public void SetType(int iType)
		{
			if (this.m_bOutSide)
			{
				this.m_bOutSide = false;
			}
			else
			{
				this.m_iNowType = iType;
			}
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x0000D5B9 File Offset: 0x0000B7B9
		public void EndShow()
		{
			this.m_Group.GameObject.SetActive(false);
		}

		// Token: 0x04001965 RID: 6501
		private Control m_Group;

		// Token: 0x04001966 RID: 6502
		private Control m_WordBase;

		// Token: 0x04001967 RID: 6503
		private Control m_LabelShow;

		// Token: 0x04001968 RID: 6504
		private int m_iNowType;

		// Token: 0x04001969 RID: 6505
		private bool m_bOutSide;
	}
}
