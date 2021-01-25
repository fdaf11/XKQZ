using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000377 RID: 887
	public class UITeam : UILayer
	{
		// Token: 0x060014AA RID: 5290 RVA: 0x0000A503 File Offset: 0x00008703
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x000B2850 File Offset: 0x000B0A50
		private void Start()
		{
			this.m_iAction = 0;
			this.m_iTeamMemberBtnID = 0;
			for (int i = 0; i < 9; i++)
			{
				this.CreatTeamMember();
			}
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x000B2884 File Offset: 0x000B0A84
		protected override void AssignControl(Transform sender)
		{
			Control control = sender;
			string name = sender.name;
			if (name != null)
			{
				if (UITeam.<>f__switch$map5F == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(24);
					dictionary.Add("Group", 0);
					dictionary.Add("LblAttributePoints", 1);
					dictionary.Add("GridTeamMember", 2);
					dictionary.Add("OnTeamMember", 3);
					dictionary.Add("TeamSelect", 4);
					dictionary.Add("TeamSelectOption", 5);
					dictionary.Add("GetoutLabel", 6);
					dictionary.Add("UpgradeStatus", 7);
					dictionary.Add("LblNow", 8);
					dictionary.Add("LblMax", 9);
					dictionary.Add("AddOption", 10);
					dictionary.Add("LessOption", 11);
					dictionary.Add("SetPointOption", 12);
					dictionary.Add("UpgradeName", 13);
					dictionary.Add("LblStr", 14);
					dictionary.Add("LblCon", 15);
					dictionary.Add("LblInt", 16);
					dictionary.Add("LblDex", 17);
					dictionary.Add("StatusTip", 18);
					dictionary.Add("LblStatusTip", 19);
					dictionary.Add("Getout", 20);
					dictionary.Add("LblGetOut", 21);
					dictionary.Add("OnGetOut", 22);
					dictionary.Add("IconTeamExit", 23);
					UITeam.<>f__switch$map5F = dictionary;
				}
				int num;
				if (UITeam.<>f__switch$map5F.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_LblAttributePoints = sender;
						break;
					case 2:
						this.m_GridTeamMember = sender;
						break;
					case 3:
						this.m_OnTeamMember = sender;
						break;
					case 4:
						this.m_TeamSelect = sender;
						break;
					case 5:
						control = sender;
						control.OnHover += this.TeamSelectOptionOnHover;
						control.OnClick += this.TeamSelectOptionOnClick;
						control.OnKeySelect += this.TeamSelectOptionOnKeySelect;
						this.m_TeamSelectOptionList.Add(control);
						base.SetInputButton(2, control.Listener);
						break;
					case 6:
						this.m_GetoutLabel = sender;
						break;
					case 7:
						this.m_UpgradeStatus = sender;
						break;
					case 8:
						control = sender;
						this.m_LblNowList.Add(control);
						break;
					case 9:
						control = sender;
						this.m_LblMaxList.Add(control);
						break;
					case 10:
						control = sender;
						control.OnClick += this.AddOptionOnClick;
						control.OnKeySelect += this.AddOptionOnKeySelect;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.AddSub);
						this.m_AddOptionList.Add(control);
						base.SetInputButton(3, control.Listener);
						break;
					case 11:
						control = sender;
						control.OnClick += this.LessOptionOnClick;
						control.OnKeySelect += this.LessOptionOnKeySelect;
						this.m_LessOptionList.Add(control);
						base.SetInputButton(3, control.Listener);
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.AddSub);
						break;
					case 12:
						control = sender;
						control.OnClick += this.SetPointOptionOnClick;
						control.OnKeySelect += this.SetPointOptionOnKeySelect;
						this.m_SetPointOptionList.Add(control);
						base.SetInputButton(3, control.Listener);
						break;
					case 13:
						this.m_UpgradeName = control;
						break;
					case 14:
						control.OnHover += this.StatusOnHover;
						this.m_LblStr = control;
						break;
					case 15:
						control.OnHover += this.StatusOnHover;
						this.m_LblCon = control;
						break;
					case 16:
						control.OnHover += this.StatusOnHover;
						this.m_LblInt = control;
						break;
					case 17:
						control.OnHover += this.StatusOnHover;
						this.m_LblDex = control;
						break;
					case 18:
						this.m_StatusTip = control;
						break;
					case 19:
						this.m_LblStatusTip = control;
						break;
					case 20:
						this.m_Getout = sender;
						break;
					case 21:
						control = sender;
						control.OnClick += this.LblGetOutOnClick;
						control.OnHover += this.LblGetOutOnHover;
						control.OnKeySelect += this.LblGetOutOnKeySelect;
						base.SetInputButton(4, control.Listener);
						break;
					case 22:
						this.m_OnGetOut = sender;
						break;
					case 23:
						control = sender;
						control.OnHover += this.IconTeamExitOnHover;
						control.OnClick += this.IconTeamExitOnClick;
						this.m_IconTeamExit = sender;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
				}
			}
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x000B2DA8 File Offset: 0x000B0FA8
		public override void Show()
		{
			Game.UI.Hide<UIMainSelect>();
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			Game.g_InputManager.Push(this);
			TeamStatus.m_Instance.m_strcFormName.Add(base.gameObject.name);
			GameGlobal.m_bCFormOpen = true;
			this.m_Group.GameObject.SetActive(true);
			this.SetAttributePoints();
			this.UpdateTeamMember();
			base.EnterState(1);
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x000B2E24 File Offset: 0x000B1024
		public override void Hide()
		{
			this.m_Group.GameObject.SetActive(false);
			this.ResetUITeamMember();
			GameGlobal.m_bCFormOpen = false;
			TeamStatus.m_Instance.m_strcFormName.Remove(base.gameObject.name);
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			Game.g_InputManager.Pop();
			this.NowState = 0;
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0000C14E File Offset: 0x0000A34E
		private void IconTeamExitOnClick(GameObject go)
		{
			go.GetComponent<UISprite>().spriteName = "cdata_027";
			base.BackState();
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00095C18 File Offset: 0x00093E18
		private void IconTeamExitOnHover(GameObject go, bool bHover)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
			UISprite component = go.GetComponent<UISprite>();
			if (bHover)
			{
				component.spriteName = "cdata_028";
			}
			else
			{
				component.spriteName = "cdata_027";
			}
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x000B2E90 File Offset: 0x000B1090
		private void TeamMemberAllDataOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.m_iOnClickNpcID = go.GetComponent<CharacterDataNode>().m_CharacterDataNode.iNpcID;
			this.m_TeamSelect.GameObject.SetActive(true);
			this.SetControl(this.m_OnTeamMember, go, 0f, 0f, 0f);
			if (this.m_iOnClickNpcID == 210001 || this.m_iOnClickNpcID == 210002 || this.m_iOnClickNpcID == 200000)
			{
				this.SetGetOutStatus(1f);
				this.m_TeamSelectOptionList[2].Collider.enabled = true;
			}
			else
			{
				this.SetGetOutStatus(1f);
				this.m_TeamSelectOptionList[2].Collider.enabled = true;
			}
			this.CurrentSelect = go;
			base.EnterState(2);
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x000B2F64 File Offset: 0x000B1164
		private void TeamMemberAllDataOnHover(GameObject go, bool isOver)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
			if (isOver)
			{
				this.SetControl(this.m_OnTeamMember, go, 0f, 0f, 0f);
			}
			else
			{
				this.m_OnTeamMember.GameObject.SetActive(false);
			}
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x000B2FB4 File Offset: 0x000B11B4
		private void TeamMemberAllDataOnKeySelect(GameObject go, bool isKeySelect)
		{
			if (isKeySelect)
			{
				this.ShowKeySelect(go, new Vector3(90f, -50f, 0f), KeySelect.eSelectDir.RightBottom, 32, 32);
				this.SetControl(this.m_OnTeamMember, go, 0f, 0f, 0f);
			}
			else
			{
				this.m_OnTeamMember.GameObject.SetActive(false);
				this.HideKeySelect();
			}
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x000A3F38 File Offset: 0x000A2138
		public override void OnMouseControl(bool bCtrl)
		{
			if (!this.controls.ContainsKey(this.NowState))
			{
				return;
			}
			List<UIEventListener> list = this.controls[this.NowState];
			for (int i = 0; i < list.Count; i++)
			{
				if (bCtrl)
				{
					if (list[i].onKeySelect != null)
					{
						list[i].onKeySelect(list[i].gameObject, false);
					}
				}
				else if (list[i].onHover != null)
				{
					list[i].onHover(list[i].gameObject, false);
				}
			}
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0000D3FA File Offset: 0x0000B5FA
		private void TeamSelectOptionOnHover(GameObject go, bool bHover)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
			this.OptionOnHover(go, bHover, "team_031", "team_030");
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x000B3020 File Offset: 0x000B1220
		private void TeamSelectOptionOnClick(GameObject go)
		{
			switch (go.GetComponent<BtnData>().m_iBtnID)
			{
			case 0:
			{
				base.EnterState(0);
				UICharacter uicharacter = Game.UI.Get<UICharacter>();
				if (uicharacter != null)
				{
					uicharacter.ShowByID(this.m_iOnClickNpcID);
				}
				break;
			}
			case 1:
				this.m_UpgradeStatus.GameObject.SetActive(true);
				this.SetPointData();
				this.m_iAction = 1;
				for (int i = 0; i < 4; i++)
				{
					this.UpdateSetOption(this.m_LblNowList[i], this.m_LblMaxList[i], this.iValue[i], this.iValueCurrent[i], this.iValueMax[i], this.m_LessOptionList[i], this.m_AddOptionList[i]);
				}
				base.EnterState(3);
				break;
			case 2:
				this.m_Getout.GameObject.SetActive(true);
				base.EnterState(4);
				break;
			}
			this.m_TeamSelect.GameObject.SetActive(false);
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x000B312C File Offset: 0x000B132C
		private void TeamSelectOptionOnKeySelect(GameObject go, bool bSelect)
		{
			this.OptionOnHover(go, bSelect, "team_031", "team_030");
			if (bSelect)
			{
				this.ShowKeySelect(go, new Vector3(-195f, 0f, 0f), KeySelect.eSelectDir.Left, 32, 32);
			}
			else
			{
				this.HideKeySelect();
			}
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x000B317C File Offset: 0x000B137C
		private void AddOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			if (this.attributePoints <= 0)
			{
				return;
			}
			if (!go.collider.enabled)
			{
				return;
			}
			int num = this.m_AddOptionList.FindIndex((Control x) => x.GameObject == go);
			this.attributePoints--;
			this.m_LblAttributePoints.Text = this.attributePoints.ToString();
			this.iValue[num] = this.iValue[num] + 1;
			this.UpdateSetOption(this.m_LblNowList[num], this.m_LblMaxList[num], this.iValue[num], this.iValueCurrent[num], this.iValueMax[num], this.m_LessOptionList[num], this.m_AddOptionList[num]);
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0000D419 File Offset: 0x0000B619
		private void AddOptionOnKeySelect(GameObject go, bool bSelect)
		{
			this.OptionOnHover(go, bSelect, "team_033", "team_032");
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x000B3264 File Offset: 0x000B1464
		private void LessOptionOnClick(GameObject go)
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
			this.attributePoints++;
			this.m_LblAttributePoints.Text = this.attributePoints.ToString();
			this.iValue[num] = this.iValue[num] - 1;
			this.UpdateSetOption(this.m_LblNowList[num], this.m_LblMaxList[num], this.iValue[num], this.iValueCurrent[num], this.iValueMax[num], this.m_LessOptionList[num], this.m_AddOptionList[num]);
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0000D42D File Offset: 0x0000B62D
		private void LessOptionOnKeySelect(GameObject go, bool bSelect)
		{
			this.OptionOnHover(go, bSelect, "team_036", "team_035");
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x000B333C File Offset: 0x000B153C
		private void SetPointOptionOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			int num = this.m_SetPointOptionList.FindIndex((Control x) => x.GameObject == go);
			go.GetComponent<UISprite>().spriteName = "ui_sys_09";
			if (num == 0)
			{
				NPC.m_instance.SetCharacterProperty(this.m_iOnClickNpcID, 11, this.iValue[0] - this.iValueCurrent[0]);
				NPC.m_instance.SetCharacterProperty(this.m_iOnClickNpcID, 12, this.iValue[1] - this.iValueCurrent[1]);
				NPC.m_instance.SetCharacterProperty(this.m_iOnClickNpcID, 13, this.iValue[2] - this.iValueCurrent[2]);
				NPC.m_instance.SetCharacterProperty(this.m_iOnClickNpcID, 14, this.iValue[3] - this.iValueCurrent[3]);
				this.SetTeamMemberData();
				TeamStatus.m_Instance.ChangeAttributePoints(this.attributePoints - this.points);
				this.ResetUITeamMember();
				this.SetAttributePoints();
			}
			else
			{
				this.BackToPrevious();
			}
			base.EnterState(1);
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0000D441 File Offset: 0x0000B641
		private void SetPointOptionOnKeySelect(GameObject go, bool bSelect)
		{
			this.OptionOnHover(go, bSelect, "ui_sys_10", "ui_sys_09");
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x000B345C File Offset: 0x000B165C
		private void StatusOnHover(GameObject go, bool hover)
		{
			if (hover)
			{
				string name = go.name;
				if (name != null)
				{
					if (UITeam.<>f__switch$map60 == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
						dictionary.Add("LblStr", 0);
						dictionary.Add("LblCon", 1);
						dictionary.Add("LblInt", 2);
						dictionary.Add("LblDex", 3);
						UITeam.<>f__switch$map60 = dictionary;
					}
					int num;
					if (UITeam.<>f__switch$map60.TryGetValue(name, ref num))
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

		// Token: 0x060014BF RID: 5311 RVA: 0x000B3594 File Offset: 0x000B1794
		private void LblGetOutOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			this.ResetUITeamMember();
			if (go.GetComponent<BtnData>().m_iBtnID == 0)
			{
				TeamStatus.m_Instance.LessTeamMember(this.m_iOnClickNpcID);
				this.UpdateTeamMember();
			}
			else
			{
				this.BackToPrevious();
			}
			base.EnterState(1);
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x000B35EC File Offset: 0x000B17EC
		private void LblGetOutOnHover(GameObject go, bool isOver)
		{
			if (!GameCursor.IsShow)
			{
				return;
			}
			if (isOver)
			{
				this.SetControl(this.m_OnGetOut, go, 0f, 1f, 0f);
			}
			else
			{
				this.m_OnGetOut.GameObject.SetActive(false);
			}
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0000D455 File Offset: 0x0000B655
		private void LblGetOutOnKeySelect(GameObject go, bool bSelect)
		{
			if (bSelect)
			{
				this.SetControl(this.m_OnGetOut, go, 0f, 1f, 0f);
			}
			else
			{
				this.m_OnGetOut.GameObject.SetActive(false);
			}
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0000D48F File Offset: 0x0000B68F
		private void UpdateTeamMember()
		{
			this.m_TeamMemberList = TeamStatus.m_Instance.GetTeamMemberList();
			this.ResetTeamMember();
			this.SetTeamMemberData();
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x000B363C File Offset: 0x000B183C
		private void ResetTeamMember()
		{
			for (int i = 0; i < this.m_TeamMemberAllDataList.Count; i++)
			{
				this.m_TeamMemberAllDataList[i].GameObject.SetActive(false);
				this.m_BackTeamMemberList[i].GameObject.SetActive(true);
			}
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x000B3694 File Offset: 0x000B1894
		private void SetTeamMemberData()
		{
			for (int i = 0; i < this.m_TeamMemberAllDataList.Count; i++)
			{
				if (i < this.m_TeamMemberList.Count)
				{
					this.m_TeamMemberAllDataList[i].GameObject.SetActive(true);
					this.m_BackTeamMemberList[i].GameObject.SetActive(false);
					CharacterData characterData = this.m_TeamMemberList[i];
					Texture texture = Game.g_TeamHeadBundle.Load("2dtexture/gameui/teamhead/" + characterData._NpcDataNode.m_strMemberImage) as Texture;
					this.m_TeamMemberAllDataList[i].GetComponent<CharacterDataNode>().m_CharacterDataNode = characterData;
					if (texture != null)
					{
						this.m_IconTeamMemberList[i].GetComponent<UITexture>().mainTexture = texture;
					}
					else
					{
						this.m_IconTeamMemberList[i].GetComponent<UITexture>().mainTexture = null;
					}
					this.m_IconWaponList[i].SpriteName = (characterData._NpcDataNode.m_iMartialType + 130001).ToString();
					this.m_LblNameList[i].Text = characterData._NpcDataNode.m_strNpcName;
					this.m_LblHPValueList[i].Text = characterData._TotalProperty.Get(CharacterData.PropertyType.MaxHP).ToString();
					this.m_LblSPValueList[i].Text = characterData._TotalProperty.Get(CharacterData.PropertyType.MaxSP).ToString();
					this.m_LblStrValueList[i].Text = characterData._TotalProperty.Get(CharacterData.PropertyType.Strength).ToString();
					this.m_LblConValueList[i].Text = characterData._TotalProperty.Get(CharacterData.PropertyType.Constitution).ToString();
					this.m_LblIntValueList[i].Text = characterData._TotalProperty.Get(CharacterData.PropertyType.Intelligence).ToString();
					this.m_LblDexValueList[i].Text = characterData._TotalProperty.Get(CharacterData.PropertyType.Dexterity).ToString();
					this.m_LblDodgeValueList[i].Text = characterData._TotalProperty.Get(CharacterData.PropertyType.Dodge).ToString() + "%";
					this.m_LblFightBackValueList[i].Text = characterData._TotalProperty.Get(CharacterData.PropertyType.Counter).ToString() + "%";
					this.m_LblCtiricalValuelList[i].Text = characterData._TotalProperty.Get(CharacterData.PropertyType.Critical).ToString() + "%";
					this.m_LblComboValueList[i].Text = characterData._TotalProperty.Get(CharacterData.PropertyType.Combo).ToString() + "%";
				}
			}
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x0000D4AD File Offset: 0x0000B6AD
		private void SetAttributePoints()
		{
			this.points = TeamStatus.m_Instance.GetAttributePoints();
			this.attributePoints = this.points;
			this.m_LblAttributePoints.Text = this.attributePoints.ToString();
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x000B3978 File Offset: 0x000B1B78
		private void SetPointData()
		{
			CharacterData characterData = NPC.m_instance.GetCharacterData(this.m_iOnClickNpcID);
			this.m_UpgradeName.Text = characterData._NpcDataNode.m_strNpcName;
			for (int i = 0; i < 4; i++)
			{
				if (i == 0)
				{
					this.iValueCurrent[i] = characterData._TotalProperty.Get(CharacterData.PropertyType.Strength);
					this.iValueMax[i] = characterData._TotalProperty.Get(CharacterData.PropertyType.StrengthMax);
				}
				else if (i == 1)
				{
					this.iValueCurrent[i] = characterData._TotalProperty.Get(CharacterData.PropertyType.Constitution);
					this.iValueMax[i] = characterData._TotalProperty.Get(CharacterData.PropertyType.ConstitutionMax);
				}
				else if (i == 2)
				{
					this.iValueCurrent[i] = characterData._TotalProperty.Get(CharacterData.PropertyType.Intelligence);
					this.iValueMax[i] = characterData._TotalProperty.Get(CharacterData.PropertyType.IntelligenceMax);
				}
				else if (i == 3)
				{
					this.iValueCurrent[i] = characterData._TotalProperty.Get(CharacterData.PropertyType.Dexterity);
					this.iValueMax[i] = characterData._TotalProperty.Get(CharacterData.PropertyType.DexterityMax);
				}
				this.m_LblNowList[i].Text = this.iValueCurrent[i].ToString();
				this.m_LblMaxList[i].Text = this.iValueMax[i].ToString();
				this.iValue[i] = this.iValueCurrent[i];
			}
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x000B3AE4 File Offset: 0x000B1CE4
		private void CreatTeamMember()
		{
			GameObject gameObject = Object.Instantiate(this.m_TeamMember) as GameObject;
			gameObject.transform.parent = this.m_GridTeamMember.GameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject.transform.localScale = Vector3.one;
			gameObject.name = "TeamMember";
			gameObject.transform.Traversal(delegate(Transform x)
			{
				string name = x.name;
				if (name != null)
				{
					if (UITeam.<>f__switch$map62 == null)
					{
						Dictionary<string, int> dictionary = new Dictionary<string, int>(15);
						dictionary.Add("TeamMemberAllData", 0);
						dictionary.Add("IconTeamMember", 1);
						dictionary.Add("IconWapon", 2);
						dictionary.Add("LblName", 3);
						dictionary.Add("LblHPValue", 4);
						dictionary.Add("LblSPValue", 5);
						dictionary.Add("LblStrValue", 6);
						dictionary.Add("LblConValue", 7);
						dictionary.Add("LblIntValue", 8);
						dictionary.Add("LblDexValue", 9);
						dictionary.Add("LblDodgeValue", 10);
						dictionary.Add("LblFightBackValue", 11);
						dictionary.Add("LblCtiricalValue", 12);
						dictionary.Add("LblComboValue", 13);
						dictionary.Add("BackTeamMember", 14);
						UITeam.<>f__switch$map62 = dictionary;
					}
					int num;
					if (UITeam.<>f__switch$map62.TryGetValue(name, ref num))
					{
						switch (num)
						{
						case 0:
						{
							Control control = x;
							x.GetComponent<BtnData>().m_iBtnID = this.m_iTeamMemberBtnID;
							control.OnClick += this.TeamMemberAllDataOnClick;
							control.OnHover += this.TeamMemberAllDataOnHover;
							control.OnKeySelect += this.TeamMemberAllDataOnKeySelect;
							this.m_TeamMemberAllDataList.Add(x);
							base.SetInputButton(1, control.Listener);
							break;
						}
						case 1:
							this.m_IconTeamMemberList.Add(x);
							break;
						case 2:
							this.m_IconWaponList.Add(x);
							break;
						case 3:
							this.m_LblNameList.Add(x);
							break;
						case 4:
							this.m_LblHPValueList.Add(x);
							break;
						case 5:
							this.m_LblSPValueList.Add(x);
							break;
						case 6:
							this.m_LblStrValueList.Add(x);
							break;
						case 7:
							this.m_LblConValueList.Add(x);
							break;
						case 8:
							this.m_LblIntValueList.Add(x);
							break;
						case 9:
							this.m_LblDexValueList.Add(x);
							break;
						case 10:
							this.m_LblDodgeValueList.Add(x);
							break;
						case 11:
							this.m_LblFightBackValueList.Add(x);
							break;
						case 12:
							this.m_LblCtiricalValuelList.Add(x);
							break;
						case 13:
							this.m_LblComboValueList.Add(x);
							break;
						case 14:
							this.m_BackTeamMemberList.Add(x);
							break;
						}
					}
				}
			});
			this.m_GridTeamMember.GetComponent<UIGrid>().Reposition();
			this.m_iTeamMemberBtnID++;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x000B3B8C File Offset: 0x000B1D8C
		private void ResetSelectOption(List<Control> addOption, List<Control> lessOption)
		{
			for (int i = 0; i < addOption.Count; i++)
			{
				if (addOption != null)
				{
					addOption[i].Collider.enabled = false;
					addOption[i].UISprite.color = new Color(1f, 1f, 1f, 0.23529412f);
				}
				if (lessOption != null)
				{
					lessOption[i].Collider.enabled = false;
					lessOption[i].UISprite.color = new Color(1f, 1f, 1f, 0.23529412f);
				}
			}
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0000D4E1 File Offset: 0x0000B6E1
		private void ResetUITeamMember()
		{
			this.m_TeamSelect.GameObject.SetActive(false);
			this.m_OnTeamMember.GameObject.SetActive(false);
			this.ResetTeamSelset();
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x000B3C34 File Offset: 0x000B1E34
		private void ResetTeamSelset()
		{
			for (int i = 0; i < this.m_TeamSelectOptionList.Count; i++)
			{
				this.m_TeamSelectOptionList[i].SpriteName = "team_030";
			}
			this.m_UpgradeStatus.GameObject.SetActive(false);
			this.m_Getout.GameObject.SetActive(false);
			this.m_OnGetOut.GameObject.SetActive(false);
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0000D50B File Offset: 0x0000B70B
		private void BackToPrevious()
		{
			this.m_TeamSelect.GameObject.SetActive(true);
			this.ResetTeamSelset();
			this.SetAttributePoints();
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0000D52A File Offset: 0x0000B72A
		private void UpdateSetOption(Control label, Control lblMax, int value, int current, int max, Control lessOption, Control addOption)
		{
			this.SetPointLabel(value, current, max, label, lblMax);
			this.CheckOptionStatus(value, current, max, lessOption, addOption);
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x000AAB20 File Offset: 0x000A8D20
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

		// Token: 0x060014CE RID: 5326 RVA: 0x000B3CA8 File Offset: 0x000B1EA8
		private void CheckOptionStatus(int value, int current, int max, Control lessOption, Control addOption)
		{
			addOption.Collider.enabled = true;
			addOption.UISprite.color = Color.white;
			lessOption.Collider.enabled = true;
			lessOption.UISprite.color = Color.white;
			if (this.attributePoints <= 0 || value >= max)
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

		// Token: 0x060014CF RID: 5327 RVA: 0x000B3D70 File Offset: 0x000B1F70
		private void SetGetOutStatus(float alpha)
		{
			this.m_TeamSelectOptionList[2].UISprite.color = new Color(1f, 1f, 1f, alpha);
			this.m_GetoutLabel.UILabel.color = new Color(1f, 1f, 1f, alpha);
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x000AACA4 File Offset: 0x000A8EA4
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

		// Token: 0x060014D1 RID: 5329 RVA: 0x0000D548 File Offset: 0x0000B748
		public int GetOnClickNpcID()
		{
			return this.m_iOnClickNpcID;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x000B3DD0 File Offset: 0x000B1FD0
		public override void OnKeyUp(KeyControl.Key key)
		{
			base.OnKeyUp(key);
			if (key == KeyControl.Key.Team)
			{
				base.EnterState(0);
			}
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x000B3E00 File Offset: 0x000B2000
		protected override void OnStateEnter(int state)
		{
			switch (state)
			{
			case 0:
				this.Hide();
				break;
			case 1:
				this.ResetUITeamMember();
				break;
			case 2:
				this.BackToPrevious();
				break;
			}
			if (!GameCursor.IsShow)
			{
				if (!this.controls.ContainsKey(this.NowState))
				{
					return;
				}
				List<UIEventListener> list = this.controls[this.NowState];
				if (list != null && list.Count > 0)
				{
					this.current = list[0];
					base.SetCurrent(this.current, true);
				}
			}
		}

		// Token: 0x04001912 RID: 6418
		public GameObject m_TeamMember;

		// Token: 0x04001913 RID: 6419
		private Control m_Group;

		// Token: 0x04001914 RID: 6420
		private Control m_LblAttributePoints;

		// Token: 0x04001915 RID: 6421
		private Control m_OnTeamMember;

		// Token: 0x04001916 RID: 6422
		private Control m_GridTeamMember;

		// Token: 0x04001917 RID: 6423
		private List<Control> m_TeamMemberAllDataList = new List<Control>();

		// Token: 0x04001918 RID: 6424
		private List<Control> m_IconTeamMemberList = new List<Control>();

		// Token: 0x04001919 RID: 6425
		private List<Control> m_IconWaponList = new List<Control>();

		// Token: 0x0400191A RID: 6426
		private List<Control> m_LblNameList = new List<Control>();

		// Token: 0x0400191B RID: 6427
		private List<Control> m_LblHPValueList = new List<Control>();

		// Token: 0x0400191C RID: 6428
		private List<Control> m_LblSPValueList = new List<Control>();

		// Token: 0x0400191D RID: 6429
		private List<Control> m_LblStrValueList = new List<Control>();

		// Token: 0x0400191E RID: 6430
		private List<Control> m_LblConValueList = new List<Control>();

		// Token: 0x0400191F RID: 6431
		private List<Control> m_LblIntValueList = new List<Control>();

		// Token: 0x04001920 RID: 6432
		private List<Control> m_LblDexValueList = new List<Control>();

		// Token: 0x04001921 RID: 6433
		private List<Control> m_LblDodgeValueList = new List<Control>();

		// Token: 0x04001922 RID: 6434
		private List<Control> m_LblFightBackValueList = new List<Control>();

		// Token: 0x04001923 RID: 6435
		private List<Control> m_LblCtiricalValuelList = new List<Control>();

		// Token: 0x04001924 RID: 6436
		private List<Control> m_LblComboValueList = new List<Control>();

		// Token: 0x04001925 RID: 6437
		private List<Control> m_BackTeamMemberList = new List<Control>();

		// Token: 0x04001926 RID: 6438
		private Control m_TeamSelect;

		// Token: 0x04001927 RID: 6439
		private List<Control> m_TeamSelectOptionList = new List<Control>();

		// Token: 0x04001928 RID: 6440
		private Control m_GetoutLabel;

		// Token: 0x04001929 RID: 6441
		private Control m_Getout;

		// Token: 0x0400192A RID: 6442
		private Control m_OnGetOut;

		// Token: 0x0400192B RID: 6443
		private Control m_UpgradeStatus;

		// Token: 0x0400192C RID: 6444
		private Control m_UpgradeName;

		// Token: 0x0400192D RID: 6445
		private List<Control> m_AddOptionList = new List<Control>();

		// Token: 0x0400192E RID: 6446
		private List<Control> m_LessOptionList = new List<Control>();

		// Token: 0x0400192F RID: 6447
		private List<Control> m_LblNowList = new List<Control>();

		// Token: 0x04001930 RID: 6448
		private List<Control> m_LblMaxList = new List<Control>();

		// Token: 0x04001931 RID: 6449
		private List<Control> m_SetPointOptionList = new List<Control>();

		// Token: 0x04001932 RID: 6450
		private Control m_LblStr;

		// Token: 0x04001933 RID: 6451
		private Control m_LblCon;

		// Token: 0x04001934 RID: 6452
		private Control m_LblInt;

		// Token: 0x04001935 RID: 6453
		private Control m_LblDex;

		// Token: 0x04001936 RID: 6454
		private Control m_StatusTip;

		// Token: 0x04001937 RID: 6455
		private Control m_LblStatusTip;

		// Token: 0x04001938 RID: 6456
		private Control m_IconTeamExit;

		// Token: 0x04001939 RID: 6457
		private List<CharacterData> m_TeamMemberList;

		// Token: 0x0400193A RID: 6458
		private int m_iTeamMemberBtnID;

		// Token: 0x0400193B RID: 6459
		private int m_iOnClickNpcID;

		// Token: 0x0400193C RID: 6460
		private int[] iValue = new int[4];

		// Token: 0x0400193D RID: 6461
		private int[] iValueCurrent = new int[4];

		// Token: 0x0400193E RID: 6462
		private int[] iValueMax = new int[4];

		// Token: 0x0400193F RID: 6463
		private int[] iValueWarningID = new int[]
		{
			110104,
			110105,
			110106,
			110107
		};

		// Token: 0x04001940 RID: 6464
		private int attributePoints;

		// Token: 0x04001941 RID: 6465
		private int points;

		// Token: 0x04001942 RID: 6466
		private int m_iAction;

		// Token: 0x04001943 RID: 6467
		private List<Control> m_IconFriendshipList = new List<Control>();

		// Token: 0x04001944 RID: 6468
		private List<Control> m_LblFriendshipList = new List<Control>();

		// Token: 0x04001945 RID: 6469
		private Control m_TipFriendship;

		// Token: 0x04001946 RID: 6470
		private List<Control> m_IconTeacherList = new List<Control>();

		// Token: 0x04001947 RID: 6471
		private Control m_TipTeacher;

		// Token: 0x04001948 RID: 6472
		private Control m_OnTeacher;

		// Token: 0x04001949 RID: 6473
		private Control m_OnClickTeacher;

		// Token: 0x0400194A RID: 6474
		public GameObject m_TeacherNode;

		// Token: 0x0400194B RID: 6475
		private Control m_GridTeacher;

		// Token: 0x0400194C RID: 6476
		private List<Control> m_TeacherNodeList = new List<Control>();

		// Token: 0x0400194D RID: 6477
		private List<Control> m_IconTeacherNodeList = new List<Control>();

		// Token: 0x0400194E RID: 6478
		private List<Control> m_LblTeacherList = new List<Control>();

		// Token: 0x0400194F RID: 6479
		private List<Control> m_LblTeacherEffectList = new List<Control>();

		// Token: 0x04001950 RID: 6480
		private Control m_TeacherScrollBar;

		// Token: 0x04001951 RID: 6481
		private GameObject CurrentSelect;

		// Token: 0x02000378 RID: 888
		private enum eState
		{
			// Token: 0x04001957 RID: 6487
			None,
			// Token: 0x04001958 RID: 6488
			Team,
			// Token: 0x04001959 RID: 6489
			SelectOption,
			// Token: 0x0400195A RID: 6490
			SetPoint,
			// Token: 0x0400195B RID: 6491
			Getout
		}

		// Token: 0x02000379 RID: 889
		private enum eValueType
		{
			// Token: 0x0400195D RID: 6493
			Str,
			// Token: 0x0400195E RID: 6494
			Con,
			// Token: 0x0400195F RID: 6495
			Int,
			// Token: 0x04001960 RID: 6496
			Dex,
			// Token: 0x04001961 RID: 6497
			Count
		}
	}
}
