using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000351 RID: 849
	public class WgNowPractice : Widget
	{
		// Token: 0x0600136A RID: 4970 RVA: 0x0000C94B File Offset: 0x0000AB4B
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
			this.m_UICharacter = (layer as UICharacter);
			this.m_CtrlCharacter = this.m_UICharacter.m_CtrlCharacter;
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x000A8100 File Offset: 0x000A6300
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgNowPractice.<>f__switch$map40 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("LblNowPractice", 0);
					dictionary.Add("SliderPractice", 1);
					dictionary.Add("LblNowPracticeExp", 2);
					dictionary.Add("IconPracticeOpen", 3);
					WgNowPractice.<>f__switch$map40 = dictionary;
				}
				int num;
				if (WgNowPractice.<>f__switch$map40.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_LblNowParactice = sender;
						break;
					case 1:
						this.m_SliderPractice = sender;
						break;
					case 2:
						this.m_LblNowPracticeExp = sender;
						break;
					case 3:
						control = sender;
						control.OnClick += this.IconPracticeOpenOnClick;
						control.OnHover += this.IconPracticeOpenOnHover;
						control.OnKeySelect += this.IconPracticeOpenOnKeySelect;
						this.m_IconPracticeOpen = control;
						this.ParentLayer.SetInputButton(1, control.Listener);
						break;
					}
				}
			}
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0000C971 File Offset: 0x0000AB71
		public void SetIconPracticeOpen(string name)
		{
			this.m_IconPracticeOpen.SpriteName = name;
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0000C97F File Offset: 0x0000AB7F
		public void SetPractice(string title, string exp, float fval)
		{
			this.m_LblNowParactice.Text = title;
			this.m_LblNowPracticeExp.Text = exp;
			this.m_SliderPractice.sliderValue = fval;
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x0000C9A5 File Offset: 0x0000ABA5
		private void IconPracticeOpenOnHover(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.m_IconPracticeOpen.SpriteName = "cdata_041";
			}
			else
			{
				this.m_IconPracticeOpen.SpriteName = "cdata_040";
			}
			if (bHover)
			{
				this.m_CtrlCharacter.SetCurrent(go);
			}
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x0000C9E4 File Offset: 0x0000ABE4
		private void IconPracticeOpenOnKeySelect(GameObject go, bool bHover)
		{
			if (bHover)
			{
				this.m_IconPracticeOpen.SpriteName = "cdata_041";
			}
			else
			{
				this.m_IconPracticeOpen.SpriteName = "cdata_040";
			}
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0000CA11 File Offset: 0x0000AC11
		private void IconPracticeOpenOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_UICharacter.SetState(3);
		}

		// Token: 0x0400177A RID: 6010
		private CtrlCharacter m_CtrlCharacter;

		// Token: 0x0400177B RID: 6011
		private UICharacter m_UICharacter;

		// Token: 0x0400177C RID: 6012
		private Control m_IconPracticeOpen;

		// Token: 0x0400177D RID: 6013
		private Control m_LblNowParactice;

		// Token: 0x0400177E RID: 6014
		private Control m_LblNowPracticeExp;

		// Token: 0x0400177F RID: 6015
		private Control m_SliderPractice;
	}
}
