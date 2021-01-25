using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000366 RID: 870
	public class UIInfoCheck : UILayer
	{
		// Token: 0x060013D2 RID: 5074 RVA: 0x000AB418 File Offset: 0x000A9618
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIInfoCheck.<>f__switch$map4C == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("Group", 0);
					dictionary.Add("LblTitle", 1);
					dictionary.Add("LblText", 2);
					dictionary.Add("OK", 3);
					dictionary.Add("Cancel", 4);
					dictionary.Add("OKCancel", 5);
					UIInfoCheck.<>f__switch$map4C = dictionary;
				}
				int num;
				if (UIInfoCheck.<>f__switch$map4C.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_LblTitle = sender;
						break;
					case 2:
						this.m_LblText = sender;
						break;
					case 3:
						this.m_OK = sender;
						this.m_OK.OnClick += this.OnOKClick;
						break;
					case 4:
						this.m_Cancel = sender;
						this.m_Cancel.OnClick += this.OnCancelClick;
						break;
					case 5:
						this.m_OKCancel = sender;
						this.m_OKCancel.OnClick += this.OnCancelClick;
						break;
					}
				}
			}
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0000CC19 File Offset: 0x0000AE19
		public void OnCancelClick(GameObject obj)
		{
			this.Hide();
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x000AB580 File Offset: 0x000A9780
		public void OnOKClick(GameObject obj)
		{
			int num = BackpackStatus.m_Instance.GetMoney();
			int dlcinfoRemain = TeamStatus.m_Instance.DLCInfoRemain;
			int dlcinfoLimit = TeamStatus.m_Instance.DLCInfoLimit;
			int num2 = 300 + (dlcinfoLimit - dlcinfoRemain) * 300;
			if (num2 <= num && dlcinfoRemain > 0)
			{
				num -= num2;
				BackpackStatus.m_Instance.setMoney(num);
				TeamStatus.m_Instance.AddInfoMission();
				TeamStatus.m_Instance.DLCInfoRemain--;
				Game.DLCShopInfo.m_UseInfo = true;
				UIReadyCombat uireadyCombat = Game.UI.Get<UIReadyCombat>();
				uireadyCombat.UpdateData();
				this.Hide();
			}
			else
			{
				if (num2 > num)
				{
					string @string = Game.StringTable.GetString(200081);
					Game.UI.Get<UIMapMessage>().SetMsg(@string);
					return;
				}
				if (dlcinfoRemain <= 0)
				{
					string string2 = Game.StringTable.GetString(990095);
					Game.UI.Get<UIMapMessage>().SetMsg(string2);
					return;
				}
			}
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x000AB674 File Offset: 0x000A9874
		public override void Show()
		{
			this.m_Group.GameObject.SetActive(true);
			this.m_OK.GameObject.SetActive(false);
			this.m_Cancel.GameObject.SetActive(false);
			this.m_OKCancel.GameObject.SetActive(false);
			int money = BackpackStatus.m_Instance.GetMoney();
			int dlcinfoRemain = TeamStatus.m_Instance.DLCInfoRemain;
			int dlcinfoLimit = TeamStatus.m_Instance.DLCInfoLimit;
			int num = 300 + (dlcinfoLimit - dlcinfoRemain) * 300;
			if (num <= money && dlcinfoRemain > 0)
			{
				string text = num.ToString();
				string text2 = Game.StringTable.GetString(990100);
				text2 = string.Format(text2, text);
				this.m_LblText.Text = text2;
				this.m_OK.GameObject.SetActive(true);
				this.m_Cancel.GameObject.SetActive(true);
			}
			else if (num > money)
			{
				string @string = Game.StringTable.GetString(200081);
				this.m_LblText.Text = @string;
				this.m_OKCancel.GameObject.SetActive(true);
			}
			else if (dlcinfoRemain <= 0)
			{
				string string2 = Game.StringTable.GetString(990095);
				this.m_LblText.Text = string2;
				this.m_OKCancel.GameObject.SetActive(true);
			}
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x0000CC21 File Offset: 0x0000AE21
		public override void Hide()
		{
			this.m_Group.GameObject.SetActive(false);
		}

		// Token: 0x04001804 RID: 6148
		private Control m_Group;

		// Token: 0x04001805 RID: 6149
		private Control m_LblTitle;

		// Token: 0x04001806 RID: 6150
		private Control m_LblText;

		// Token: 0x04001807 RID: 6151
		private Control m_OK;

		// Token: 0x04001808 RID: 6152
		private Control m_Cancel;

		// Token: 0x04001809 RID: 6153
		private Control m_OKCancel;
	}
}
