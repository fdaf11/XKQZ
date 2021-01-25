using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200034C RID: 844
	public class WgCheckUpgradeUnit : Widget
	{
		// Token: 0x06001342 RID: 4930 RVA: 0x0000C5F4 File Offset: 0x0000A7F4
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x000A7384 File Offset: 0x000A5584
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgCheckUpgradeUnit.<>f__switch$map3A == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("LblTitle", 0);
					dictionary.Add("LblText", 1);
					dictionary.Add("OK", 2);
					dictionary.Add("LblOK", 3);
					dictionary.Add("Cancel", 4);
					dictionary.Add("LblCancel", 5);
					WgCheckUpgradeUnit.<>f__switch$map3A = dictionary;
				}
				int num;
				if (WgCheckUpgradeUnit.<>f__switch$map3A.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_LblTitle = control;
						break;
					case 1:
						this.m_LblText = control;
						break;
					case 2:
						this.m_OK = control;
						this.m_OK.OnClick += this.OnBtnClick;
						break;
					case 3:
						this.m_LblOK = control;
						break;
					case 4:
						this.m_Cancel = control;
						this.m_Cancel.OnClick += this.OnBtnClick;
						break;
					case 5:
						this.m_LblCancel = control;
						break;
					}
				}
			}
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x000A74B0 File Offset: 0x000A56B0
		public void ShowWgCheckUpgradeUnit(string title, string msg, string LblOK, string LblCancel, Action callbackOK, Action callbackCancel)
		{
			this.Obj.SetActive(true);
			this.m_LblTitle.Text = title;
			this.m_LblText.Text = msg;
			this.m_LblOK.Text = LblOK;
			this.m_LblCancel.Text = LblCancel;
			this.OnOKClick = callbackOK;
			this.OnCancelClick = callbackCancel;
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0000C7F7 File Offset: 0x0000A9F7
		public void Close()
		{
			this.Obj.SetActive(false);
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x000A750C File Offset: 0x000A570C
		public void OnBtnClick(GameObject obj)
		{
			string name = obj.name;
			if (name != null)
			{
				if (WgCheckUpgradeUnit.<>f__switch$map3B == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
					dictionary.Add("OK", 0);
					dictionary.Add("Cancel", 1);
					WgCheckUpgradeUnit.<>f__switch$map3B = dictionary;
				}
				int num;
				if (WgCheckUpgradeUnit.<>f__switch$map3B.TryGetValue(name, ref num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							if (this.OnCancelClick != null)
							{
								this.OnCancelClick.Invoke();
							}
						}
					}
					else if (this.OnOKClick != null)
					{
						this.OnOKClick.Invoke();
					}
				}
			}
		}

		// Token: 0x0400174D RID: 5965
		private Control m_LblTitle;

		// Token: 0x0400174E RID: 5966
		private Control m_LblText;

		// Token: 0x0400174F RID: 5967
		private Control m_OK;

		// Token: 0x04001750 RID: 5968
		private Control m_LblOK;

		// Token: 0x04001751 RID: 5969
		private Control m_Cancel;

		// Token: 0x04001752 RID: 5970
		private Control m_LblCancel;

		// Token: 0x04001753 RID: 5971
		private Action OnOKClick;

		// Token: 0x04001754 RID: 5972
		private Action OnCancelClick;
	}
}
