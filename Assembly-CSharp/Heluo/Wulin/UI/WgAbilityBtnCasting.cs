using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000334 RID: 820
	public class WgAbilityBtnCasting : Widget
	{
		// Token: 0x0600124B RID: 4683 RVA: 0x0000BDE0 File Offset: 0x00009FE0
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
			this.m_UIAbility = (this.ParentLayer as UIAbility);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x0009FBEC File Offset: 0x0009DDEC
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgAbilityBtnCasting.<>f__switch$map26 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("BtnCasting", 0);
					dictionary.Add("LabelCastingName", 1);
					dictionary.Add("SelectBox", 2);
					WgAbilityBtnCasting.<>f__switch$map26 = dictionary;
				}
				int num;
				if (WgAbilityBtnCasting.<>f__switch$map26.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						control.OnClick += this.BtnCastingOnClick;
						control.OnHover += this.BtnCastingOnHover;
						control.OnKeySelect += this.BtnCastingOnKeySelect;
						if (this.m_UIAbility == null)
						{
							this.m_UIAbility = (this.ParentLayer as UIAbility);
						}
						this.m_UIAbility.SetInputButton(1, control.Listener);
						this.m_BtnCasting = control;
						break;
					case 1:
						this.m_LabelCastingName = control;
						break;
					case 2:
						this.m_SelectBox = control;
						break;
					}
				}
			}
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x0000BDFA File Offset: 0x00009FFA
		public void Hide()
		{
			this.Obj.SetActive(false);
			this.m_BtnCasting.Listener.parameter = null;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x0009FD00 File Offset: 0x0009DF00
		public void SetWgAbilityBtnCasting(Texture2D tex, bool bOpen, string text, AlchemyProduceNode node)
		{
			this.m_BtnCasting.Listener.parameter = node;
			this.Obj.SetActive(true);
			this.m_BtnCasting.Texture = tex;
			if (!bOpen)
			{
				this.m_BtnCasting.TextureColor = new Color(0.25f, 0.25f, 0.25f, 1f);
			}
			else
			{
				this.m_BtnCasting.TextureColor = new Color(1f, 1f, 1f, 1f);
			}
			this.m_LabelCastingName.Text = text;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0009FD98 File Offset: 0x0009DF98
		private void BtnCastingOnHover(GameObject go, bool bHover)
		{
			this.m_SelectBox.GameObject.SetActive(bHover);
			AlchemyProduceNode node = go.GetComponent<UIEventListener>().parameter as AlchemyProduceNode;
			this.m_UIAbility.SetItemTip(node, bHover);
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0009FDD4 File Offset: 0x0009DFD4
		private void BtnCastingOnClick(GameObject go)
		{
			AlchemyProduceNode node = go.GetComponent<UIEventListener>().parameter as AlchemyProduceNode;
			this.m_UIAbility.BtnCastingOnClick(node);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0009FD98 File Offset: 0x0009DF98
		private void BtnCastingOnKeySelect(GameObject go, bool bSelect)
		{
			this.m_SelectBox.GameObject.SetActive(bSelect);
			AlchemyProduceNode node = go.GetComponent<UIEventListener>().parameter as AlchemyProduceNode;
			this.m_UIAbility.SetItemTip(node, bSelect);
		}

		// Token: 0x04001633 RID: 5683
		private UIAbility m_UIAbility;

		// Token: 0x04001634 RID: 5684
		private Control m_BtnCasting;

		// Token: 0x04001635 RID: 5685
		private Control m_LabelCastingName;

		// Token: 0x04001636 RID: 5686
		private Control m_SelectBox;
	}
}
