using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200034E RID: 846
	public class WgDLCNeigong : Widget
	{
		// Token: 0x0600134D RID: 4941 RVA: 0x000A76C4 File Offset: 0x000A58C4
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
			UIDLCCharacter uidlccharacter = layer as UIDLCCharacter;
			this.m_CtrlCharacter = uidlccharacter.m_CtrlCharacter;
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x000A76EC File Offset: 0x000A58EC
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgDLCNeigong.<>f__switch$map3D == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("Name", 0);
					dictionary.Add("LV", 1);
					dictionary.Add("Practice", 2);
					dictionary.Add("Exp", 3);
					dictionary.Add("BtnPractice", 4);
					dictionary.Add("NewKnight", 5);
					WgDLCNeigong.<>f__switch$map3D = dictionary;
				}
				int num;
				if (WgDLCNeigong.<>f__switch$map3D.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Name = control;
						this.m_Name.OnHover += this.NeigongOnHover;
						break;
					case 1:
						this.m_LV = control;
						break;
					case 2:
						this.m_Practice = control;
						this.m_Practice.OnHover += this.PracticeOnHover;
						break;
					case 3:
						this.m_Exp = control;
						break;
					case 4:
						this.m_BtnPractice = control;
						control.OnClick += this.NeigongOnClick;
						break;
					case 5:
						this.m_NewKnight = sender;
						break;
					}
				}
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x000A7830 File Offset: 0x000A5A30
		public void SetNeigong(NpcNeigong neigong, UIDLCCharacter.UIType type, int iNeigongLV)
		{
			if (type == UIDLCCharacter.UIType.Unit)
			{
				this.m_Practice.GameObject.SetActive(false);
				this.m_BtnPractice.GameObject.SetActive(false);
			}
			else
			{
				this.m_Practice.GameObject.SetActive(true);
				this.m_BtnPractice.GameObject.SetActive(true);
			}
			if (neigong != null)
			{
				this.m_Name.Text = neigong.m_Neigong.m_strNeigongName;
			}
			else
			{
				this.m_Name.Text = Game.StringTable.GetString(100100);
			}
			this.m_Exp.GameObject.SetActive(false);
			float num = (float)this.m_CtrlCharacter.m_CurCaraData.DLC_NowLevelExp();
			float num2 = (float)this.m_CtrlCharacter.m_CurCaraData.DLC_NowLevelUpExp();
			this.m_Practice.GameObject.GetComponent<UIProgressBar>().value = num / num2;
			this.m_Exp.Text = num + "/" + num2;
			this.m_LV.Text = Game.StringTable.GetString(990300 + iNeigongLV);
			this.SetHasPoint();
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x000A7958 File Offset: 0x000A5B58
		public void SetHasPoint()
		{
			bool active = this.m_CtrlCharacter.m_CurCaraData.iSkillPoint > 0;
			this.m_NewKnight.GameObject.SetActive(active);
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0000C846 File Offset: 0x0000AA46
		protected void NeigongOnClick(GameObject go)
		{
			if (this.OnNeigongClick != null)
			{
				this.OnNeigongClick.Invoke(go);
			}
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0000C85F File Offset: 0x0000AA5F
		protected void NeigongOnHover(GameObject go, bool bhover)
		{
			if (this.OnNeigongHover != null)
			{
				this.OnNeigongHover.Invoke(go, bhover);
			}
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0000C879 File Offset: 0x0000AA79
		protected void PracticeOnHover(GameObject go, bool bhover)
		{
			this.m_Exp.GameObject.SetActive(bhover);
			this.m_LV.GameObject.SetActive(!bhover);
		}

		// Token: 0x0400175D RID: 5981
		private CtrlDLCCharacter m_CtrlCharacter;

		// Token: 0x0400175E RID: 5982
		public Action<GameObject> OnNeigongClick;

		// Token: 0x0400175F RID: 5983
		public Action<GameObject, bool> OnNeigongHover;

		// Token: 0x04001760 RID: 5984
		private Control m_Name;

		// Token: 0x04001761 RID: 5985
		private Control m_LV;

		// Token: 0x04001762 RID: 5986
		private Control m_Practice;

		// Token: 0x04001763 RID: 5987
		private Control m_Exp;

		// Token: 0x04001764 RID: 5988
		private Control m_BtnPractice;

		// Token: 0x04001765 RID: 5989
		private Control m_NewKnight;
	}
}
