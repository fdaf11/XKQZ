using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200036D RID: 877
	public class WgCombatMenu : Widget
	{
		// Token: 0x0600141A RID: 5146 RVA: 0x000AD678 File Offset: 0x000AB878
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (WgCombatMenu.<>f__switch$map54 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(12);
					dictionary.Add("Group", 0);
					dictionary.Add("Knight", 1);
					dictionary.Add("Unit", 2);
					dictionary.Add("NewUnit", 3);
					dictionary.Add("Treasury", 4);
					dictionary.Add("NewTrade", 5);
					dictionary.Add("Trade", 6);
					dictionary.Add("Info", 7);
					dictionary.Add("BtnSystem", 8);
					dictionary.Add("SystemGroup", 9);
					dictionary.Add("SysOption", 10);
					dictionary.Add("SysOptionOn", 11);
					WgCombatMenu.<>f__switch$map54 = dictionary;
				}
				int num;
				if (WgCombatMenu.<>f__switch$map54.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_OptionGroup = sender;
						break;
					case 1:
						this.m_Knight = sender;
						this.m_Knight.OnClick += this.OnButtonClick;
						break;
					case 2:
						this.m_Unit = sender;
						this.m_Unit.OnClick += this.OnButtonClick;
						break;
					case 3:
						this.m_NewUnit = sender;
						break;
					case 4:
						this.m_Treasury = sender;
						this.m_Treasury.OnClick += this.OnButtonClick;
						break;
					case 5:
						this.m_NewTrade = sender;
						break;
					case 6:
						this.m_Trade = sender;
						this.m_Trade.OnClick += this.OnButtonClick;
						break;
					case 7:
						this.m_Info = sender;
						this.m_Info.OnClick += this.OnButtonClick;
						break;
					case 8:
						this.m_System = sender;
						this.m_System.OnClick += this.OnButtonClick;
						break;
					case 9:
						this.m_SystemGroup = sender;
						break;
					case 10:
					{
						Control control = sender;
						control.OnHover += this.SysOptionOnHover;
						control.OnClick += this.SysOptionOnClick;
						control.GameObject.name = this.sIndex.ToString();
						this.sIndex++;
						this.m_SysOptionList.Add(control);
						break;
					}
					case 11:
						this.m_SysOptionOn = sender;
						break;
					}
				}
			}
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x000AD930 File Offset: 0x000ABB30
		public void ResetShopAndInfo()
		{
			this.m_NewTrade.GameObject.SetActive(Game.DLCShopInfo.m_Update);
			this.m_NewUnit.GameObject.SetActive(TeamStatus.m_Instance.m_NewUnit);
			if (!Game.DLCShopInfo.m_UseInfo)
			{
				this.m_Info.GetComponent<UIButton>().state = UIButtonColor.State.Normal;
				this.m_Info.Collider.enabled = true;
			}
			this.m_Knight.GameObject.SetActive(false);
			this.m_Unit.GameObject.SetActive(false);
			this.m_Treasury.GameObject.SetActive(false);
			this.m_Trade.GameObject.SetActive(false);
			this.m_Info.GameObject.SetActive(false);
			this.m_Knight.GameObject.SetActive(true);
			if (Game.Variable["menu1"] >= 1)
			{
				this.m_Unit.GameObject.SetActive(true);
			}
			if (Game.Variable["menu2"] >= 1)
			{
				this.m_Treasury.GameObject.SetActive(true);
			}
			if (Game.Variable["menu3"] >= 1)
			{
				this.m_Trade.GameObject.SetActive(true);
			}
			if (Game.Variable["menu4"] >= 1)
			{
				this.m_Info.GameObject.SetActive(true);
			}
			UIGrid component = this.m_OptionGroup.GetComponent<UIGrid>();
			component.Reposition();
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0000CEDA File Offset: 0x0000B0DA
		private void SysOptionOnHover(GameObject go, bool bHover)
		{
			if (GameCursor.IsShow)
			{
				this.OnOptionStatus(go, bHover, this.m_SysOptionOn);
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0000CEF4 File Offset: 0x0000B0F4
		private void OnOptionStatus(GameObject goes, bool bOn, Control sprite)
		{
			if (bOn)
			{
				this.SetControl(sprite, goes, 0f, 0f, 0f);
			}
			else
			{
				sprite.GameObject.SetActive(false);
			}
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0000AF0D File Offset: 0x0000910D
		protected void SetControl(Control nowSet, GameObject goes, float x, float y, float z)
		{
			nowSet.GameObject.SetActive(true);
			nowSet.GameObject.transform.parent = goes.transform;
			nowSet.GameObject.transform.localPosition = new Vector3(x, y, z);
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x000ADAB8 File Offset: 0x000ABCB8
		private void SysOptionOnClick(GameObject go)
		{
			this.m_SysOptionOn.GameObject.SetActive(false);
			int type = int.Parse(go.name);
			switch (type)
			{
			case 0:
				this.m_OptionGroup.GameObject.SetActive(true);
				this.m_SystemGroup.GameObject.SetActive(false);
				break;
			case 1:
			case 2:
				this.m_OptionGroup.GameObject.SetActive(true);
				this.m_SystemGroup.GameObject.SetActive(false);
				Game.UI.Get<UISaveAndLoad>().SelectSaveOrLoad(type);
				break;
			case 3:
				Game.UI.Get<UILoad>().LoadStage("GameStart");
				break;
			}
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x000ADB78 File Offset: 0x000ABD78
		private void OnButtonClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			string name = go.name;
			if (name != null)
			{
				if (WgCombatMenu.<>f__switch$map55 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("Knight", 0);
					dictionary.Add("Unit", 1);
					dictionary.Add("Treasury", 2);
					dictionary.Add("Trade", 3);
					dictionary.Add("Info", 4);
					dictionary.Add("BtnSystem", 5);
					WgCombatMenu.<>f__switch$map55 = dictionary;
				}
				int num;
				if (WgCombatMenu.<>f__switch$map55.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
					{
						if (TeamStatus.m_Instance.GetTeamMemberList().Count <= 0)
						{
							return;
						}
						if (TeamStatus.m_Instance.GetNewKnight())
						{
							string @string = Game.StringTable.GetString(990087);
							Game.UI.Get<UIMapMessage>().SetMsg(@string);
						}
						UIDLCCharacter uidlccharacter = Game.UI.Get<UIDLCCharacter>();
						uidlccharacter.Show(UIDLCCharacter.UIType.Kinght, "0");
						break;
					}
					case 1:
					{
						if (TeamStatus.m_Instance.GetDLCUnitList().Count <= 0)
						{
							Game.UI.Hide<UIDLCCharacter>();
							string string2 = Game.StringTable.GetString(990077);
							Game.UI.Get<UIMapMessage>().SetMsg(string2);
							return;
						}
						if (TeamStatus.m_Instance.m_NewUnit)
						{
							string string3 = Game.StringTable.GetString(990086);
							Game.UI.Get<UIMapMessage>().SetMsg(string3);
							TeamStatus.m_Instance.m_NewUnit = false;
						}
						UIDLCCharacter uidlccharacter = Game.UI.Get<UIDLCCharacter>();
						uidlccharacter.Show(UIDLCCharacter.UIType.Unit, "0");
						break;
					}
					case 2:
					{
						UIBackpack uibackpack = Game.UI.Get<UIBackpack>();
						uibackpack.Show();
						break;
					}
					case 3:
					{
						UIShop uishop = Game.UI.Get<UIShop>();
						uishop.Show();
						if (Game.DLCShopInfo.m_Update)
						{
							string string4 = Game.StringTable.GetString(990088);
							Game.UI.Get<UIMapMessage>().SetMsg(string4);
						}
						this.m_NewTrade.GameObject.SetActive(false);
						Game.DLCShopInfo.m_Update = false;
						break;
					}
					case 4:
					{
						UIInfoCheck uiinfoCheck = Game.UI.Get<UIInfoCheck>();
						uiinfoCheck.Show();
						break;
					}
					case 5:
						this.m_OptionGroup.GameObject.SetActive(false);
						this.m_SystemGroup.GameObject.SetActive(true);
						break;
					}
				}
			}
		}

		// Token: 0x0400184C RID: 6220
		private Control m_OptionGroup;

		// Token: 0x0400184D RID: 6221
		private Control m_Knight;

		// Token: 0x0400184E RID: 6222
		private Control m_Unit;

		// Token: 0x0400184F RID: 6223
		private Control m_NewUnit;

		// Token: 0x04001850 RID: 6224
		private Control m_Treasury;

		// Token: 0x04001851 RID: 6225
		private Control m_Trade;

		// Token: 0x04001852 RID: 6226
		private Control m_NewTrade;

		// Token: 0x04001853 RID: 6227
		private Control m_Info;

		// Token: 0x04001854 RID: 6228
		private Control m_System;

		// Token: 0x04001855 RID: 6229
		private Control m_SystemGroup;

		// Token: 0x04001856 RID: 6230
		private Control m_SysOptionOn;

		// Token: 0x04001857 RID: 6231
		private List<Control> m_SysOptionList = new List<Control>();

		// Token: 0x04001858 RID: 6232
		private int sIndex;
	}
}
