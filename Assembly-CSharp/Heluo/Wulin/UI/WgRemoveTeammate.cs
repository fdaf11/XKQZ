using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000356 RID: 854
	public class WgRemoveTeammate : Widget
	{
		// Token: 0x06001386 RID: 4998 RVA: 0x0000CA76 File Offset: 0x0000AC76
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
			this.m_UICharacter = (layer as UICharacter);
			this.m_CtrlCharacter = this.m_UICharacter.m_CtrlCharacter;
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x000A8A68 File Offset: 0x000A6C68
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgRemoveTeammate.<>f__switch$map44 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
					dictionary.Add("Btn", 0);
					dictionary.Add("Hover", 1);
					WgRemoveTeammate.<>f__switch$map44 = dictionary;
				}
				int num;
				if (WgRemoveTeammate.<>f__switch$map44.TryGetValue(name, ref num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							this.m_HoverList.Add(control);
						}
					}
					else
					{
						control.OnClick += this.BtnOnClick;
						control.OnHover += this.BtnOnHover;
						control.OnKeySelect += this.BtnKeySelect;
						this.m_BtnList.Add(control);
						this.ParentLayer.SetInputButton(7, control.Listener);
					}
				}
			}
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x000A8B48 File Offset: 0x000A6D48
		public void ShowRemoveTeammate(bool bShow)
		{
			for (int i = 0; i < this.m_HoverList.Count; i++)
			{
				this.m_HoverList[i].GameObject.SetActive(false);
			}
			this.Obj.SetActive(bShow);
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x000A8B94 File Offset: 0x000A6D94
		public void BtnOnClick(GameObject go)
		{
			int num = this.m_BtnList.FindIndex((Control x) => x.GameObject == go);
			WgRemoveTeammate.eBtnType eBtnType = (WgRemoveTeammate.eBtnType)num;
			WgRemoveTeammate.eBtnType eBtnType2 = eBtnType;
			if (eBtnType2 != WgRemoveTeammate.eBtnType.OK)
			{
				if (eBtnType2 == WgRemoveTeammate.eBtnType.Cancel)
				{
					this.m_CtrlCharacter.RemoveTeammate(false);
				}
			}
			else
			{
				this.m_CtrlCharacter.RemoveTeammate(true);
			}
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x000A8C00 File Offset: 0x000A6E00
		public void BtnOnHover(GameObject go, bool bHover)
		{
			int num = this.m_BtnList.FindIndex((Control x) => x.GameObject == go);
			this.m_HoverList[num].GameObject.SetActive(bHover);
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x000A8C4C File Offset: 0x000A6E4C
		public void BtnKeySelect(GameObject go, bool bSelect)
		{
			int num = this.m_BtnList.FindIndex((Control x) => x.GameObject == go);
			this.m_HoverList[num].GameObject.SetActive(bSelect);
		}

		// Token: 0x04001799 RID: 6041
		private CtrlCharacter m_CtrlCharacter;

		// Token: 0x0400179A RID: 6042
		private UICharacter m_UICharacter;

		// Token: 0x0400179B RID: 6043
		private List<Control> m_BtnList = new List<Control>();

		// Token: 0x0400179C RID: 6044
		private List<Control> m_HoverList = new List<Control>();

		// Token: 0x02000357 RID: 855
		private enum eBtnType
		{
			// Token: 0x0400179F RID: 6047
			OK,
			// Token: 0x040017A0 RID: 6048
			Cancel
		}
	}
}
