using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200035E RID: 862
	public class WgUpgradeStatus : Widget
	{
		// Token: 0x060013B2 RID: 5042 RVA: 0x0000CB28 File Offset: 0x0000AD28
		public override void InitWidget(UILayer layer)
		{
			base.InitWidget(layer);
			this.m_UICharacter = (layer as UICharacter);
			this.m_CtrlCharacter = this.m_UICharacter.m_CtrlCharacter;
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x000AA520 File Offset: 0x000A8720
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (WgUpgradeStatus.<>f__switch$map49 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(12);
					dictionary.Add("UpgradeName", 0);
					dictionary.Add("AddOption", 1);
					dictionary.Add("LessOption", 2);
					dictionary.Add("LblNow", 3);
					dictionary.Add("LblMax", 4);
					dictionary.Add("SetPointOption", 5);
					dictionary.Add("LblStr", 6);
					dictionary.Add("LblCon", 7);
					dictionary.Add("LblInt", 8);
					dictionary.Add("LblDex", 9);
					dictionary.Add("StatusTip", 10);
					dictionary.Add("LblStatusTip", 11);
					WgUpgradeStatus.<>f__switch$map49 = dictionary;
				}
				int num;
				if (WgUpgradeStatus.<>f__switch$map49.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_UpgradeName = control;
						break;
					case 1:
						control.OnClick += this.AddOptionOnClick;
						control.OnKeySelect += this.AddOptionOnKeySelect;
						this.ParentLayer.SetInputButton(4, control.Listener);
						this.m_AddOptionList.Add(control);
						break;
					case 2:
						control.OnClick += this.LessOptionOnClick;
						control.OnKeySelect += this.LessOptionOnKeySelect;
						this.ParentLayer.SetInputButton(4, control.Listener);
						this.m_LessOptionList.Add(control);
						break;
					case 3:
						this.m_LblNowList.Add(control);
						break;
					case 4:
						this.m_LblMaxList.Add(control);
						break;
					case 5:
						control.OnClick += this.SetPointOptionOnClick;
						control.OnKeySelect += this.SetPointOptionOnKeySelect;
						this.ParentLayer.SetInputButton(4, control.Listener);
						this.m_SetPointOptionList.Add(control);
						break;
					case 6:
						control.OnHover += this.StatusOnHover;
						this.m_LblStr = control;
						break;
					case 7:
						control.OnHover += this.StatusOnHover;
						this.m_LblCon = control;
						break;
					case 8:
						control.OnHover += this.StatusOnHover;
						this.m_LblInt = control;
						break;
					case 9:
						control.OnHover += this.StatusOnHover;
						this.m_LblDex = control;
						break;
					case 10:
						this.m_StatusTip = control;
						break;
					case 11:
						this.m_LblStatusTip = control;
						break;
					}
				}
			}
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x000AA7CC File Offset: 0x000A89CC
		public void SetWgUpgradeStatus(CharacterData cd)
		{
			this.Obj.SetActive(true);
			int num = 11;
			int num2 = 15;
			for (int i = 0; i < 4; i++)
			{
				this.iValueCurrent[i] = cd._TotalProperty.Get((CharacterData.PropertyType)num);
				this.iValueMax[i] = cd._TotalProperty.Get((CharacterData.PropertyType)num2);
				this.m_LblNowList[i].Text = this.iValueCurrent[i].ToString();
				this.m_LblMaxList[i].Text = this.iValueMax[i].ToString();
				this.iValue[i] = this.iValueCurrent[i];
				this.UpdateSetOption(this.m_LblNowList[i], this.m_LblMaxList[i], this.iValue[i], this.iValueCurrent[i], this.iValueMax[i], this.m_LessOptionList[i], this.m_AddOptionList[i]);
				num++;
				num2++;
			}
			this.m_UpgradeName.Text = cd._NpcDataNode.m_strNpcName;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0000CB4E File Offset: 0x0000AD4E
		public void AddOptionOnKeySelect(GameObject go, bool select)
		{
			this.OptionOnHover(go, select, "team_033", "team_032");
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x000AA8E8 File Offset: 0x000A8AE8
		public void AddOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int attributePoints = this.m_CtrlCharacter.GetAttributePoints();
			if (attributePoints <= 0)
			{
				return;
			}
			if (!go.collider.enabled)
			{
				return;
			}
			int num = this.m_AddOptionList.FindIndex((Control x) => x.GameObject == go);
			this.m_CtrlCharacter.UpgradeAttributePoints(-1);
			this.iValue[num] = this.iValue[num] + 1;
			this.UpdateSetOption(this.m_LblNowList[num], this.m_LblMaxList[num], this.iValue[num], this.iValueCurrent[num], this.iValueMax[num], this.m_LessOptionList[num], this.m_AddOptionList[num]);
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0000CB62 File Offset: 0x0000AD62
		public void LessOptionOnKeySelect(GameObject go, bool select)
		{
			this.OptionOnHover(go, select, "team_036", "team_035");
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x000AA9BC File Offset: 0x000A8BBC
		public void LessOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (!go.collider.enabled)
			{
				return;
			}
			int num = this.m_LessOptionList.FindIndex((Control x) => x.GameObject == go);
			this.m_CtrlCharacter.UpgradeAttributePoints(1);
			this.iValue[num] = this.iValue[num] - 1;
			this.UpdateSetOption(this.m_LblNowList[num], this.m_LblMaxList[num], this.iValue[num], this.iValueCurrent[num], this.iValueMax[num], this.m_LessOptionList[num], this.m_AddOptionList[num]);
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0000CB76 File Offset: 0x0000AD76
		public void SetPointOptionOnKeySelect(GameObject go, bool select)
		{
			this.OptionOnHover(go, select, "ui_sys_10", "ui_sys_09");
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x000AAA7C File Offset: 0x000A8C7C
		public void SetPointOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int num = this.m_SetPointOptionList.FindIndex((Control x) => x.GameObject == go);
			go.GetComponent<UISprite>().spriteName = "ui_sys_09";
			WgUpgradeStatus.eBtnType eBtnType = (WgUpgradeStatus.eBtnType)num;
			WgUpgradeStatus.eBtnType eBtnType2 = eBtnType;
			if (eBtnType2 != WgUpgradeStatus.eBtnType.OK)
			{
				if (eBtnType2 == WgUpgradeStatus.eBtnType.Cancel)
				{
					this.m_CtrlCharacter.SetCharacterProperty(false, this.iValue, this.iValueCurrent);
				}
			}
			else
			{
				this.m_CtrlCharacter.SetCharacterProperty(true, this.iValue, this.iValueCurrent);
			}
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0000CB8A File Offset: 0x0000AD8A
		private void UpdateSetOption(Control label, Control lblMax, int value, int current, int max, Control lessOption, Control addOption)
		{
			this.SetPointLabel(value, current, max, label, lblMax);
			this.CheckOptionStatus(value, current, max, lessOption, addOption);
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x000AAB20 File Offset: 0x000A8D20
		private void SetPointLabel(int value, int current, int max, Control label, Control labelMax)
		{
			labelMax.Text = Mathf.Clamp(max, 0, 500).ToString();
			label.Text = Mathf.Clamp(value, current, max).ToString();
			if (value >= max)
			{
				label.UILabel.color = new Color(0.11764706f, 1f, 0f, 1f);
			}
			else if (value > current)
			{
				label.UILabel.color = new Color(0f, 0.5921569f, 0.83137256f, 1f);
			}
			else
			{
				label.UILabel.color = Color.white;
			}
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x000AABD4 File Offset: 0x000A8DD4
		private void CheckOptionStatus(int value, int current, int max, Control lessOption, Control addOption)
		{
			addOption.Collider.enabled = true;
			addOption.UISprite.color = Color.white;
			lessOption.Collider.enabled = true;
			lessOption.UISprite.color = Color.white;
			int attributePoints = TeamStatus.m_Instance.GetAttributePoints();
			if (attributePoints <= 0 || value >= max)
			{
				addOption.Collider.enabled = false;
				addOption.UISprite.color = new Color(1f, 1f, 1f, 0.23529412f);
			}
			if (value <= current)
			{
				lessOption.Collider.enabled = false;
				lessOption.UISprite.color = new Color(1f, 1f, 1f, 0.23529412f);
			}
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x000AACA4 File Offset: 0x000A8EA4
		private void OptionOnHover(GameObject goes, bool hover, string strHover, string strNormal)
		{
			UISprite component = goes.GetComponent<UISprite>();
			if (hover)
			{
				component.spriteName = strHover;
			}
			else
			{
				component.spriteName = strNormal;
			}
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x000AACD4 File Offset: 0x000A8ED4
		private void StatusOnHover(GameObject go, bool hover)
		{
			if (hover)
			{
				string name = go.name;
				if (name != null)
				{
					if (WgUpgradeStatus.<>f__switch$map4A == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
						dictionary.Add("LblStr", 0);
						dictionary.Add("LblCon", 1);
						dictionary.Add("LblInt", 2);
						dictionary.Add("LblDex", 3);
						WgUpgradeStatus.<>f__switch$map4A = dictionary;
					}
					int num;
					if (WgUpgradeStatus.<>f__switch$map4A.TryGetValue(name, ref num))
					{
						switch (num)
						{
						case 0:
							this.m_LblStatusTip.Text = Game.StringTable.GetString(110161);
							break;
						case 1:
							this.m_LblStatusTip.Text = Game.StringTable.GetString(110162);
							break;
						case 2:
							this.m_LblStatusTip.Text = Game.StringTable.GetString(110163);
							break;
						case 3:
							this.m_LblStatusTip.Text = Game.StringTable.GetString(110164);
							break;
						}
					}
				}
				this.m_StatusTip.GameObject.SetActive(true);
			}
			else
			{
				this.m_StatusTip.GameObject.SetActive(false);
			}
		}

		// Token: 0x040017D7 RID: 6103
		private CtrlCharacter m_CtrlCharacter;

		// Token: 0x040017D8 RID: 6104
		private UICharacter m_UICharacter;

		// Token: 0x040017D9 RID: 6105
		private Control m_UpgradeName;

		// Token: 0x040017DA RID: 6106
		private List<Control> m_AddOptionList = new List<Control>();

		// Token: 0x040017DB RID: 6107
		private List<Control> m_LessOptionList = new List<Control>();

		// Token: 0x040017DC RID: 6108
		private List<Control> m_LblNowList = new List<Control>();

		// Token: 0x040017DD RID: 6109
		private List<Control> m_LblMaxList = new List<Control>();

		// Token: 0x040017DE RID: 6110
		private Control m_LblStr;

		// Token: 0x040017DF RID: 6111
		private Control m_LblCon;

		// Token: 0x040017E0 RID: 6112
		private Control m_LblInt;

		// Token: 0x040017E1 RID: 6113
		private Control m_LblDex;

		// Token: 0x040017E2 RID: 6114
		private Control m_StatusTip;

		// Token: 0x040017E3 RID: 6115
		private Control m_LblStatusTip;

		// Token: 0x040017E4 RID: 6116
		private int[] iValue = new int[4];

		// Token: 0x040017E5 RID: 6117
		private int[] iValueCurrent = new int[4];

		// Token: 0x040017E6 RID: 6118
		private int[] iValueMax = new int[4];

		// Token: 0x040017E7 RID: 6119
		private List<Control> m_SetPointOptionList = new List<Control>();

		// Token: 0x0200035F RID: 863
		private enum eValueType
		{
			// Token: 0x040017EB RID: 6123
			Str,
			// Token: 0x040017EC RID: 6124
			Con,
			// Token: 0x040017ED RID: 6125
			Int,
			// Token: 0x040017EE RID: 6126
			Dex,
			// Token: 0x040017EF RID: 6127
			Count
		}

		// Token: 0x02000360 RID: 864
		private enum eBtnType
		{
			// Token: 0x040017F1 RID: 6129
			OK,
			// Token: 0x040017F2 RID: 6130
			Cancel
		}
	}
}
