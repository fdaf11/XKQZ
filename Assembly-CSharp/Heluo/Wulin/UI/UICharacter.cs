using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x0200033A RID: 826
	public class UICharacter : UILayer
	{
		// Token: 0x060012A5 RID: 4773 RVA: 0x0000C12F File Offset: 0x0000A32F
		protected override void Awake()
		{
			this.m_IconEquipList = new List<Control>();
			this.m_EquipArray = new int[3];
			base.Awake();
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x000A2894 File Offset: 0x000A0A94
		private void Start()
		{
			this.m_CtrlCharacter.m_UICharacter = this;
			CtrlCharacter ctrlCharacter = this.m_CtrlCharacter;
			ctrlCharacter.OnSetCurrent = (Action<UIEventListener>)Delegate.Combine(ctrlCharacter.OnSetCurrent, new Action<UIEventListener>(base.SetCurrentOnly));
			CtrlCharacter ctrlCharacter2 = this.m_CtrlCharacter;
			ctrlCharacter2.OnEnterState = (Action<int>)Delegate.Combine(ctrlCharacter2.OnEnterState, new Action<int>(base.EnterState));
			base.CreateUIWidget<WgCharaProperty>("WgCharaProperty", this.m_GridProperty.GameObject, this.m_TextGroupProperty, this.m_PropertyList, this.m_CtrlCharacter.PropertyArray.Length);
			base.CreateUIWidget<WgCharaAtkType>("AtkType", this.m_GridAtk.GameObject, this.m_AtkType, this.m_AtkTypeList, this.m_CtrlCharacter.m_AtkTypeShowMax);
			int cloneCount = 8;
			base.CreateUIWidget<WgCharaDefType>("DefType", this.m_GridDef.GameObject, this.m_DefType, this.m_DefTypeList, cloneCount);
			base.CreateUIWidget<WgGroupSkill>("Talentl", this.m_GridTalentl.GameObject, this.m_WgGroupSkill, this.m_TalentlList, this.m_CtrlCharacter.m_iOnceSkillMax);
			for (int i = 0; i < this.m_TalentlList.Count; i++)
			{
				this.m_TalentlList[i].GroupSkillOnClick = new Action<PracticeDataNode>(this.m_CtrlCharacter.GroupSkillOnClick);
				this.m_TalentlList[i].GroupSkillOnHover = new Action<GameObject, bool>(this.m_CtrlCharacter.GroupSkillOnHover);
				this.m_TalentlList[i].SetCurrent = new Action<GameObject>(this.m_CtrlCharacter.SetCurrent);
			}
			base.CreateUIWidget<WgGroupSkill>("Martial", this.m_GridMartial.GameObject, this.m_WgGroupSkill, this.m_MartialList, this.m_CtrlCharacter.m_iOnceSkillMax);
			for (int j = 0; j < this.m_MartialList.Count; j++)
			{
				this.m_MartialList[j].GroupSkillOnClick = new Action<PracticeDataNode>(this.m_CtrlCharacter.GroupSkillOnClick);
				this.m_MartialList[j].GroupSkillOnHover = new Action<GameObject, bool>(this.m_CtrlCharacter.GroupSkillOnHover);
				this.m_MartialList[j].SetCurrent = new Action<GameObject>(this.m_CtrlCharacter.SetCurrent);
			}
			base.CreateUIWidget<WgGroupSkill>("Neigong", this.m_GridNeigong.GameObject, this.m_WgGroupSkill, this.m_NeigongList, this.m_CtrlCharacter.m_iOnceSkillMax);
			for (int k = 0; k < this.m_NeigongList.Count; k++)
			{
				this.m_NeigongList[k].GroupSkillOnClick = new Action<PracticeDataNode>(this.m_CtrlCharacter.GroupSkillOnClick);
				this.m_NeigongList[k].GroupSkillOnHover = new Action<GameObject, bool>(this.m_CtrlCharacter.GroupSkillOnHover);
				this.m_NeigongList[k].SetCurrent = new Action<GameObject>(this.m_CtrlCharacter.SetCurrent);
			}
			base.CreateUIWidget<WgPracticeUp>("PracticeUp", this.m_PracticeUpGrid.GameObject, this.m_PracticeUp, this.m_PracticeUpLlist, 5);
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x000A2B9C File Offset: 0x000A0D9C
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UICharacter.<>f__switch$map2E == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(24);
					dictionary.Add("Group", 0);
					dictionary.Add("BackTalentl", 1);
					dictionary.Add("IconSect", 2);
					dictionary.Add("LblSect", 3);
					dictionary.Add("LblTitle", 4);
					dictionary.Add("LblName", 5);
					dictionary.Add("LblIntrduction", 6);
					dictionary.Add("IconMember", 7);
					dictionary.Add("IconEquip", 8);
					dictionary.Add("GridTalentl", 9);
					dictionary.Add("GridMartial", 10);
					dictionary.Add("GridNeigong", 11);
					dictionary.Add("GridProperty", 12);
					dictionary.Add("GridAtk", 13);
					dictionary.Add("GridDef", 14);
					dictionary.Add("IconMartialProperty", 15);
					dictionary.Add("IconChangeTeamMemberR", 16);
					dictionary.Add("IconChangeTeamMemberL", 17);
					dictionary.Add("IconCloseTwoForm", 18);
					dictionary.Add("IconCharacterExit", 19);
					dictionary.Add("PracticeUpGrid", 20);
					dictionary.Add("EquipOn", 21);
					dictionary.Add("FunctionBtn", 22);
					dictionary.Add("LblAttributePoints", 23);
					UICharacter.<>f__switch$map2E = dictionary;
				}
				int num;
				if (UICharacter.<>f__switch$map2E.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 2:
						this.m_IconSect = sender;
						break;
					case 3:
						this.m_LblSect = sender;
						break;
					case 4:
						this.m_LblTitle = sender;
						break;
					case 5:
						this.m_LblName = sender;
						break;
					case 6:
						this.m_LblIntrduction = sender;
						break;
					case 7:
						this.m_IconMember = sender;
						break;
					case 8:
					{
						Control control = sender;
						control.OnClick += this.IconEquipOnClick;
						control.OnHover += this.IconEquipOnHover;
						control.OnKeySelect += this.IconEquipOnKeySelect;
						base.SetInputButton(1, control.Listener);
						this.m_IconEquipList.Add(control);
						break;
					}
					case 9:
						this.m_GridTalentl = sender;
						break;
					case 10:
						this.m_GridMartial = sender;
						break;
					case 11:
						this.m_GridNeigong = sender;
						break;
					case 12:
						this.m_GridProperty = sender;
						break;
					case 13:
						this.m_GridAtk = sender;
						break;
					case 14:
						this.m_GridDef = sender;
						break;
					case 15:
						this.m_IconMartialProperty = sender;
						break;
					case 16:
					{
						Control control = sender;
						this.m_ChangeMemberList[1] = control.Listener;
						control.OnClick += this.IconChangeTeamMemberOnClick;
						break;
					}
					case 17:
					{
						Control control = sender;
						this.m_ChangeMemberList[0] = control.Listener;
						control.OnClick += this.IconChangeTeamMemberOnClick;
						break;
					}
					case 18:
					{
						Control control = sender;
						control.OnClick += this.IconCloseTwoFormOnClick;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					case 19:
					{
						Control control = sender;
						control.OnHover += this.IconCharacterExitOnHover;
						control.OnClick += this.IconCharacterExitOnClick;
						this.m_IconCharacterExit = sender;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					case 20:
						this.m_PracticeUpGrid = sender;
						break;
					case 21:
						this.m_EquipOn = sender;
						break;
					case 22:
					{
						Control control = sender;
						base.SetInputButton(1, control.Listener);
						control.OnHover += this.FunctionBtnOnHover;
						control.OnClick += this.FunctionBtnOnClick;
						control.OnKeySelect += this.FunctionBtnOnKeySelect;
						this.m_FunctionBtnList.Add(control);
						break;
					}
					case 23:
						this.m_LblAttributePoints = sender;
						break;
					}
				}
			}
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x000A3018 File Offset: 0x000A1218
		protected override void AssignWidget(Widget sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UICharacter.<>f__switch$map2F == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(8);
					dictionary.Add("EquipTip", 0);
					dictionary.Add("SkillInfoForm", 1);
					dictionary.Add("PracticeTwoForm", 2);
					dictionary.Add("BackpackTwoForm", 3);
					dictionary.Add("NowPractice", 4);
					dictionary.Add("RemoveTeammate", 5);
					dictionary.Add("UpgradeStatus", 6);
					dictionary.Add("UseItemTwoForm", 7);
					UICharacter.<>f__switch$map2F = dictionary;
				}
				int num;
				if (UICharacter.<>f__switch$map2F.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_EquipTip = (sender as WgCharaEquipTip);
						break;
					case 1:
						this.m_SkillInfoForm = (sender as WgSkillInfoForm);
						break;
					case 2:
						this.m_PracticeTwoForm = (sender as WgPracticeTwoForm);
						break;
					case 3:
					{
						this.m_BackpackTwoForm = (sender as WgBackpackTwoForm);
						WgBackpackTwoForm backpackTwoForm = this.m_BackpackTwoForm;
						backpackTwoForm.OnCloseClick = (Action<GameObject>)Delegate.Combine(backpackTwoForm.OnCloseClick, new Action<GameObject>(this.IconCloseTwoFormOnClick));
						WgBackpackTwoForm backpackTwoForm2 = this.m_BackpackTwoForm;
						backpackTwoForm2.OnCloseHover = (Action<GameObject, bool>)Delegate.Combine(backpackTwoForm2.OnCloseHover, new Action<GameObject, bool>(this.IconCloseTwoFormOnHover));
						WgBackpackTwoForm backpackTwoForm3 = this.m_BackpackTwoForm;
						backpackTwoForm3.SetCurrent = (Action<GameObject>)Delegate.Combine(backpackTwoForm3.SetCurrent, new Action<GameObject>(this.m_CtrlCharacter.SetCurrent));
						WgBackpackTwoForm backpackTwoForm4 = this.m_BackpackTwoForm;
						backpackTwoForm4.UseEquip = (Action<GameObject>)Delegate.Combine(backpackTwoForm4.UseEquip, new Action<GameObject>(this.m_CtrlCharacter.UseEqupip));
						WgBackpackTwoForm backpackTwoForm5 = this.m_BackpackTwoForm;
						backpackTwoForm5.RemoveEqupip = (Action<GameObject, int>)Delegate.Combine(backpackTwoForm5.RemoveEqupip, new Action<GameObject, int>(this.m_CtrlCharacter.RemoveEqupip));
						break;
					}
					case 4:
						this.m_NowPractice = (sender as WgNowPractice);
						break;
					case 5:
						this.m_WgRemoveTeammate = (sender as WgRemoveTeammate);
						break;
					case 6:
						this.m_WgUpgradeStatus = (sender as WgUpgradeStatus);
						break;
					case 7:
						this.m_WgUseItemTwoForm = (sender as WgUseItemTwoForm);
						break;
					}
				}
			}
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0000C14E File Offset: 0x0000A34E
		private void IconCharacterExitOnClick(GameObject go)
		{
			go.GetComponent<UISprite>().spriteName = "cdata_027";
			base.BackState();
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x000A3240 File Offset: 0x000A1440
		private void IconCharacterExitOnHover(GameObject go, bool bHover)
		{
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

		// Token: 0x060012AB RID: 4779 RVA: 0x000A3240 File Offset: 0x000A1440
		public void IconCloseTwoFormOnHover(GameObject go, bool bHover)
		{
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

		// Token: 0x060012AC RID: 4780 RVA: 0x0000C166 File Offset: 0x0000A366
		public void IconCloseTwoFormOnClick(GameObject go)
		{
			go.GetComponent<UISprite>().spriteName = "cdata_027";
			base.EnterState(1);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x000A3278 File Offset: 0x000A1478
		private void IconEquipOnKeySelect(GameObject go, bool bSelect)
		{
			this.SetControl(this.m_EquipOn, go, 0f, 0f, 0f);
			this.m_EquipOn.GameObject.SetActive(bSelect);
			this.m_EquipTip.Obj.SetActive(bSelect);
			UIEventListener component = go.GetComponent<UIEventListener>();
			if (component.parameter != null)
			{
				this.SetEquipTipData(go, component.parameter as BackpackNewDataNode);
				this.m_EquipTip.SpriteName = "cdata_024";
			}
			else
			{
				this.SetControl(this.m_EquipTip.Obj, go, new Vector3(-660f, 0f, 0f));
				this.m_EquipTip.SetEquipTipNone();
			}
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x000A3330 File Offset: 0x000A1530
		private void IconEquipOnHover(GameObject go, bool bHover)
		{
			if (this.m_EquipTip.SpriteName == "cdata_023" && !bHover)
			{
				return;
			}
			this.SetControl(this.m_EquipOn, go, 0f, 0f, 0f);
			this.m_EquipOn.GameObject.SetActive(bHover);
			this.m_EquipTip.Obj.SetActive(bHover);
			if (bHover)
			{
				UIEventListener component = go.GetComponent<UIEventListener>();
				if (component.parameter != null)
				{
					this.SetEquipTipData(go, component.parameter as BackpackNewDataNode);
				}
				else
				{
					this.SetControl(this.m_EquipTip.Obj, go, new Vector3(-660f, 0f, 0f));
					this.m_EquipTip.SetEquipTipNone();
				}
				this.m_CtrlCharacter.SetCurrent(go);
				this.m_EquipTip.SpriteName = "cdata_024";
			}
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0000C17F File Offset: 0x0000A37F
		public void SetState(int state)
		{
			base.EnterState(state);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000A340C File Offset: 0x000A160C
		private void IconEquipOnClick(GameObject go)
		{
			if (Input.GetMouseButtonUp(1))
			{
				return;
			}
			UIEventListener component = go.GetComponent<UIEventListener>();
			if (component.parameter != null)
			{
				this.SetEquipTipData(go, component.parameter as BackpackNewDataNode);
				this.m_EquipTip.SpriteName = "cdata_023";
			}
			go.GetComponent<BtnData>().m_iOnClick = 1;
			this.NowClickEquip = go;
			int iBtnID = go.GetComponent<BtnData>().m_iBtnID;
			this.m_CtrlCharacter.UpdateBackpackTwoFormData(iBtnID);
			base.EnterState(2);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0000C188 File Offset: 0x0000A388
		public void SetMartialProperty(int idx)
		{
			this.SetControl(this.m_IconMartialProperty, this.m_PropertyList[idx].Obj, -19f, 0f, 0f);
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0000C1B6 File Offset: 0x0000A3B6
		private void IconChangeTeamMemberOnClick(GameObject go)
		{
			this.m_CtrlCharacter.ChangeTeamMember(go);
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
		public void UpdateSkillTowForm(int talentID)
		{
			this.ResetTwoForm();
			this.m_SkillInfoForm.UpdateSkillTowForm(talentID);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0000C1D8 File Offset: 0x0000A3D8
		public void UpdateSkillTowForm(object param, int maxSp)
		{
			this.ResetTwoForm();
			this.m_SkillInfoForm.UpdateSkillTowForm(param, maxSp);
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0000C1ED File Offset: 0x0000A3ED
		public void SetNeigongUse(int idx, bool buse)
		{
			this.m_NeigongList[idx].SetUsing(buse);
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x0000C201 File Offset: 0x0000A401
		public void SetNeigong(int idx, NpcNeigong neigong)
		{
			this.m_NeigongList[idx].SetGroupSkillText(neigong);
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x0000C215 File Offset: 0x0000A415
		public void SetMartialUse(int idx, bool buse)
		{
			this.m_MartialList[idx].SetUsing(buse);
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x0000C229 File Offset: 0x0000A429
		public void SetMartial(int idx, NpcRoutine routine)
		{
			this.m_MartialList[idx].SetGroupSkillText(routine);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x0000C23D File Offset: 0x0000A43D
		public void SetTalent(int idx, int id)
		{
			this.m_TalentlList[idx].SetGroupSkillText(id);
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0000C251 File Offset: 0x0000A451
		public void SetCharaDefType(int index, CharacterData.PropertyType type, int val)
		{
			this.m_DefTypeList[index].SetDefTypeText(type, val);
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0000C266 File Offset: 0x0000A466
		public void SetCharaAtkType(int index, CharacterData.PropertyType type, int val)
		{
			this.m_AtkTypeList[index].SetAtkTypeText(type, val);
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0000C27B File Offset: 0x0000A47B
		public void SetAttributePoints(string text)
		{
			this.m_LblAttributePoints.Text = text;
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x000A348C File Offset: 0x000A168C
		public void SetCharaInfo(string[] text, Texture texMapHead)
		{
			this.m_IconSect.SpriteName = text[0];
			this.m_LblSect.Text = text[1];
			this.m_LblTitle.Text = text[2];
			this.m_LblName.Text = text[3];
			this.m_LblIntrduction.Text = text[4];
			this.m_IconMember.Texture = texMapHead;
			if (text[5] == "200000" || text[5] == "210001" || text[5] == "210002")
			{
				this.m_FunctionBtnList[3].GameObject.SetActive(false);
			}
			else
			{
				this.m_FunctionBtnList[3].GameObject.SetActive(true);
			}
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0000C289 File Offset: 0x0000A489
		public void SetCharaProperty(string[] text, int idx)
		{
			this.m_PropertyList[idx].SetPropertyText(text[0], text[1]);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x000A3554 File Offset: 0x000A1754
		public void UpdateCharacterData(CharacterData _CharacterData)
		{
			this.m_EquipArray[0] = _CharacterData.iEquipWeaponID;
			this.m_EquipArray[1] = _CharacterData.iEquipArrorID;
			this.m_EquipArray[2] = _CharacterData.iEquipNecklaceID;
			this.SetEquipData();
			this.m_NowPractice.SetIconPracticeOpen("cdata_040");
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x000A35A4 File Offset: 0x000A17A4
		public void SetPractice(string title, string exp, float fval, List<LevelUp> addValueList)
		{
			this.m_NowPractice.SetPractice(title, exp, fval);
			for (int i = 0; i < this.m_PracticeUpLlist.Count; i++)
			{
				this.m_PracticeUpLlist[i].Obj.SetActive(false);
				if (addValueList != null)
				{
					if (i < addValueList.Count)
					{
						this.m_PracticeUpLlist[i].SetPracticeUpGroupText(addValueList[i].m_Type, addValueList[i].m_iValue);
					}
				}
			}
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x000A363C File Offset: 0x000A183C
		private void SetEquipData()
		{
			for (int i = 0; i < this.m_EquipArray.Length; i++)
			{
				ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(this.m_EquipArray[i]);
				if (itemDataNode != null)
				{
					BackpackNewDataNode backpackNewDataNode = new BackpackNewDataNode();
					backpackNewDataNode._ItemDataNode = itemDataNode;
					string strIcon = itemDataNode.m_strIcon;
					this.SetEquipTexture(strIcon, this.m_IconEquipList, i);
					this.m_IconEquipList[i].Listener.parameter = backpackNewDataNode;
				}
				else
				{
					this.m_IconEquipList[i].Listener.parameter = null;
					this.SetEquipTexture(null, this.m_IconEquipList, i);
				}
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x000A36E0 File Offset: 0x000A18E0
		public void SetEquipTipData(GameObject goes, BackpackNewDataNode node)
		{
			if (this.NowClickEquip != null)
			{
				this.SetControl(this.m_EquipTip.Obj, this.NowClickEquip, new Vector3(-660f, 0f, 0f));
			}
			else
			{
				this.SetControl(this.m_EquipTip.Obj, goes, new Vector3(-660f, 0f, 0f));
			}
			string equipTip = string.Empty;
			string text = string.Empty;
			string text2 = string.Empty;
			if (node != null)
			{
				equipTip = node._ItemDataNode.m_strItemName;
				for (int i = 0; i < node._ItemDataNode.m_ItmeEffectNodeList.Count; i++)
				{
					int iItemType = node._ItemDataNode.m_ItmeEffectNodeList[i].m_iItemType;
					int iRecoverType = node._ItemDataNode.m_ItmeEffectNodeList[i].m_iRecoverType;
					int iValue = node._ItemDataNode.m_ItmeEffectNodeList[i].m_iValue;
					if (iItemType == 1)
					{
						string @string = Game.StringTable.GetString(110100 + iRecoverType);
						if (iValue >= 0)
						{
							string text3 = text;
							text = string.Concat(new object[]
							{
								text3,
								@string,
								" +",
								iValue,
								"\n"
							});
						}
						else
						{
							string text4 = text;
							text = string.Concat(new object[]
							{
								text4,
								@string,
								" ",
								iValue,
								"\n"
							});
						}
					}
					else if (iItemType == 7)
					{
						string string2 = Game.StringTable.GetString(160004);
						ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(iRecoverType);
						text2 = string2 + ":\n" + conditionNode.m_strName;
					}
				}
				if (this.m_CtrlCharacter.m_CurCaraData != null && node._ItemDataNode.m_iItemType == 1 && this.m_CtrlCharacter.m_CurCaraData.mod_WeaponGuid >= 1000 && Game.Variable.mod_EquipDic[this.m_CtrlCharacter.m_CurCaraData.mod_WeaponGuid] != null)
				{
					if (string.IsNullOrEmpty(text2))
					{
						text2 = Game.StringTable.GetString(160004) + ":";
					}
					List<ItmeEffectNode> list = Game.Variable.mod_EquipDic[this.m_CtrlCharacter.m_CurCaraData.mod_WeaponGuid];
					for (int j = 0; j < list.Count; j++)
					{
						ConditionNode conditionNode2 = Game.g_BattleControl.m_battleAbility.GetConditionNode(list[j].m_iRecoverType);
						text2 = text2 + "\n" + conditionNode2.m_strName;
					}
				}
			}
			this.m_EquipTip.SetEquipTip(equipTip, text, text2);
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0000C2A2 File Offset: 0x0000A4A2
		public void UpdateBackpackTwoFormData(CharacterData cdata, List<BackpackNewDataNode> sortList, int iType)
		{
			this.m_BackpackTwoForm.SetBackpackEquipData(cdata, sortList, iType);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x000A3998 File Offset: 0x000A1B98
		public void ReSetGroupSkill()
		{
			for (int i = 0; i < this.m_TalentlList.Count; i++)
			{
				this.m_TalentlList[i].ReSetGroupSkill();
				this.m_MartialList[i].ReSetGroupSkill();
				this.m_NeigongList[i].ReSetGroupSkill();
			}
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0000C2B2 File Offset: 0x0000A4B2
		public void UpdatePracticeTwoFormView(List<NpcRoutine> routineList, List<NpcNeigong> neigongList)
		{
			this.ResetTwoForm();
			this.m_PracticeTwoForm.UpdatePracticeTwoForm(routineList, neigongList);
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x000A39F4 File Offset: 0x000A1BF4
		private void SetEquipTexture(string _Icon, List<Control> m_IconList, int iIndex)
		{
			Texture texture = Game.g_Item.Load("2dtexture/gameui/item/" + _Icon) as Texture;
			if (texture != null)
			{
				m_IconList[iIndex].GetComponent<UITexture>().mainTexture = texture;
				return;
			}
			m_IconList[iIndex].GetComponent<UITexture>().mainTexture = null;
			GameDebugTool.Log("無此裝備貼圖 : " + _Icon);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x000A3A5C File Offset: 0x000A1C5C
		public void ResetTwoForm()
		{
			this.m_BackpackTwoForm.Obj.SetActive(false);
			this.m_PracticeTwoForm.Obj.SetActive(false);
			this.m_SkillInfoForm.Obj.SetActive(false);
			this.m_EquipTip.SetActive(false);
			this.m_IconMartialProperty.GameObject.SetActive(false);
			this.m_WgUpgradeStatus.Obj.SetActive(false);
			this.m_WgRemoveTeammate.ShowRemoveTeammate(false);
			this.m_WgUseItemTwoForm.Obj.SetActive(false);
			this.NowClickEquip = null;
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x000A3AF0 File Offset: 0x000A1CF0
		public void FunctionBtnOnHover(GameObject go, bool bHover)
		{
			Control control = this.m_FunctionBtnList.Find((Control x) => x.GameObject == go);
			if (bHover)
			{
				control.SpriteName = "cdata_041";
			}
			else
			{
				control.SpriteName = "cdata_040";
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x000A3B44 File Offset: 0x000A1D44
		public void FunctionBtnOnKeySelect(GameObject go, bool bSelect)
		{
			this.m_EquipTip.Obj.SetActive(false);
			Control control = this.m_FunctionBtnList.Find((Control x) => x.GameObject == go);
			if (bSelect)
			{
				control.SpriteName = "cdata_041";
			}
			else
			{
				control.SpriteName = "cdata_040";
			}
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x000A3BA8 File Offset: 0x000A1DA8
		public void FunctionBtnOnClick(GameObject go)
		{
			if (!go.collider.enabled)
			{
				return;
			}
			if (!go.activeSelf)
			{
				return;
			}
			switch (this.m_FunctionBtnList.FindIndex((Control x) => x.GameObject == go))
			{
			case 0:
				base.EnterState(4);
				break;
			case 1:
				base.EnterState(5);
				break;
			case 2:
				base.EnterState(6);
				break;
			case 3:
				base.EnterState(7);
				break;
			}
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0000C2C7 File Offset: 0x0000A4C7
		public void SetWgUpgradeStatus(CharacterData cd)
		{
			this.m_WgUpgradeStatus.SetWgUpgradeStatus(cd);
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0000C2D5 File Offset: 0x0000A4D5
		public void UpdateUseItemTwoFormData(CharacterData cdata, List<BackpackNewDataNode> sortList, ItemDataNode.ItemType Type)
		{
			this.m_WgUseItemTwoForm.SetBackpackItemData(cdata, sortList, Type);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x000A3C50 File Offset: 0x000A1E50
		public void ShowByID(int npdID)
		{
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			TeamStatus.m_Instance.m_strcFormName.Add(base.gameObject.name);
			GameGlobal.m_bCFormOpen = true;
			Game.g_InputManager.Push(this);
			this.m_Group.GameObject.SetActive(true);
			this.m_CtrlCharacter.SetCurNpcID(npdID, false);
			base.EnterState(1);
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x000A3CC0 File Offset: 0x000A1EC0
		public override void Show()
		{
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			TeamStatus.m_Instance.m_strcFormName.Add(base.gameObject.name);
			GameGlobal.m_bCFormOpen = true;
			Game.g_InputManager.Push(this);
			this.m_Group.GameObject.SetActive(true);
			this.m_CtrlCharacter.SetCurNpcID(0, false);
			base.EnterState(1);
			if (this.m_EquipTip.mod_GemIconList.Count < 1 && this.m_IconEquipList.Count > 0)
			{
				for (int i = 0; i < 6; i++)
				{
					this.m_EquipTip.mod_GemIconList.Add(Object.Instantiate(this.m_IconEquipList[0].GameObject) as GameObject);
					this.m_EquipTip.mod_GemIconList[i].SetActive(false);
					string[] fontNames = "wuxiaName,Arial Unicode MS".Split(new char[]
					{
						','
					});
					this.m_EquipTip.mod_GemEffectList.Add(Object.Instantiate(this.m_LblAttributePoints.GameObject) as GameObject);
					this.m_EquipTip.mod_GemEffectList[i].AddComponent<UILabel>();
					this.m_EquipTip.mod_GemEffectList[i].GetComponent<UILabel>().text = "12345";
					this.m_EquipTip.mod_GemEffectList[i].GetComponent<UILabel>().font.dynamicFont.fontNames = fontNames;
					this.m_EquipTip.mod_GemEffectList[i].GetComponent<UILabel>().trueTypeFont = (Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font);
					this.m_EquipTip.mod_GemEffectList[i].GetComponent<UILabel>().fontSize = 20;
					this.m_EquipTip.mod_GemEffectList[i].SetActive(false);
				}
			}
			Game.mod_CurrentCharacterData = this.m_CtrlCharacter.m_CurCaraData;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x000A3EB8 File Offset: 0x000A20B8
		public override void Hide()
		{
			this.ResetTwoForm();
			this.m_Group.GameObject.SetActive(false);
			GameGlobal.m_bCFormOpen = false;
			TeamStatus.m_Instance.m_strcFormName.Remove(base.gameObject.name);
			if (!this.m_bShow)
			{
				return;
			}
			this.m_bShow = false;
			Game.g_InputManager.Pop();
			for (int i = 0; i < 8; i++)
			{
				this.m_CurStateArray[i] = null;
			}
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x000A3F38 File Offset: 0x000A2138
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

		// Token: 0x060012D1 RID: 4817 RVA: 0x000A3FF0 File Offset: 0x000A21F0
		public override void OnKeyUp(KeyControl.Key key)
		{
			switch (key)
			{
			case KeyControl.Key.Up:
			case KeyControl.Key.Down:
			case KeyControl.Key.Left:
			case KeyControl.Key.Right:
				this.SelectNextButton(key);
				break;
			case KeyControl.Key.OK:
				base.OnCurrentClick();
				break;
			case KeyControl.Key.Cancel:
				if (this.NowState >= 2)
				{
					base.EnterState(1);
				}
				else
				{
					base.BackState();
				}
				break;
			case KeyControl.Key.X:
				if (this.NowState == 1)
				{
					this.m_ChangeMemberList[0].onClick(this.m_ChangeMemberList[0].gameObject);
				}
				break;
			case KeyControl.Key.Y:
				if (this.NowState == 1)
				{
					this.m_ChangeMemberList[1].onClick(this.m_ChangeMemberList[1].gameObject);
				}
				break;
			default:
				if (key == KeyControl.Key.Character)
				{
					base.EnterState(0);
				}
				break;
			}
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x000A40D8 File Offset: 0x000A22D8
		protected override void OnStateEnter(int state)
		{
			switch (state)
			{
			case 0:
				this.Hide();
				break;
			case 1:
				this.ResetTwoForm();
				this.m_CtrlCharacter.UpdateCaracterData();
				break;
			case 3:
				this.m_NowPractice.SetIconPracticeOpen("cdata_040");
				this.m_CtrlCharacter.UpdatePracticeTwoFormData();
				break;
			case 4:
				this.m_CtrlCharacter.SetWgUpgradeStatus();
				break;
			case 5:
				this.m_CtrlCharacter.UpdateUseItemTwoFormData(ItemDataNode.ItemType.Solution);
				break;
			case 6:
				this.m_CtrlCharacter.UpdateUseItemTwoFormData(ItemDataNode.ItemType.TipsBook);
				break;
			case 7:
				this.m_WgRemoveTeammate.ShowRemoveTeammate(true);
				break;
			}
			if (!GameCursor.IsShow)
			{
				if (!this.controls.ContainsKey(this.NowState))
				{
					return;
				}
				if (this.m_CurStateArray[state] == null)
				{
					List<UIEventListener> list = this.controls[this.NowState];
					if (list != null && list.Count > 0)
					{
						this.current = list[0];
						base.SetCurrent(this.current, true);
					}
				}
				else
				{
					this.current = this.m_CurStateArray[state];
					base.SetCurrent(this.current, true);
				}
			}
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x000A4228 File Offset: 0x000A2428
		protected override void OnStateExit(int state)
		{
			if (state == 1)
			{
				this.m_CurStateArray[state] = this.current;
			}
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0000C2E5 File Offset: 0x0000A4E5
		private void Update()
		{
			if (Input.GetMouseButtonUp(1))
			{
				if (this.NowState >= 2)
				{
					base.EnterState(1);
				}
				else
				{
					base.BackState();
				}
			}
		}

		// Token: 0x0400168E RID: 5774
		public CtrlCharacter m_CtrlCharacter = new CtrlCharacter();

		// Token: 0x0400168F RID: 5775
		private UIEventListener[] m_CurStateArray = new UIEventListener[8];

		// Token: 0x04001690 RID: 5776
		private Control m_Group;

		// Token: 0x04001691 RID: 5777
		private int m_iNowNpcID;

		// Token: 0x04001692 RID: 5778
		private Control m_IconSect;

		// Token: 0x04001693 RID: 5779
		private Control m_LblSect;

		// Token: 0x04001694 RID: 5780
		private Control m_LblTitle;

		// Token: 0x04001695 RID: 5781
		private Control m_LblName;

		// Token: 0x04001696 RID: 5782
		private Control m_LblIntrduction;

		// Token: 0x04001697 RID: 5783
		private Control m_IconMember;

		// Token: 0x04001698 RID: 5784
		private Control m_EquipOn;

		// Token: 0x04001699 RID: 5785
		public List<Control> m_IconEquipList;

		// Token: 0x0400169A RID: 5786
		private WgCharaEquipTip m_EquipTip;

		// Token: 0x0400169B RID: 5787
		private GameObject NowClickEquip;

		// Token: 0x0400169C RID: 5788
		public GameObject m_WgGroupSkill;

		// Token: 0x0400169D RID: 5789
		private Control m_GridTalentl;

		// Token: 0x0400169E RID: 5790
		private List<WgGroupSkill> m_TalentlList = new List<WgGroupSkill>();

		// Token: 0x0400169F RID: 5791
		private Control m_GridMartial;

		// Token: 0x040016A0 RID: 5792
		private List<WgGroupSkill> m_MartialList = new List<WgGroupSkill>();

		// Token: 0x040016A1 RID: 5793
		private Control m_GridNeigong;

		// Token: 0x040016A2 RID: 5794
		private List<WgGroupSkill> m_NeigongList = new List<WgGroupSkill>();

		// Token: 0x040016A3 RID: 5795
		private Control m_PracticeUpGrid;

		// Token: 0x040016A4 RID: 5796
		private WgNowPractice m_NowPractice;

		// Token: 0x040016A5 RID: 5797
		public GameObject m_PracticeUp;

		// Token: 0x040016A6 RID: 5798
		private List<WgPracticeUp> m_PracticeUpLlist = new List<WgPracticeUp>();

		// Token: 0x040016A7 RID: 5799
		private Control m_GridProperty;

		// Token: 0x040016A8 RID: 5800
		public GameObject m_TextGroupProperty;

		// Token: 0x040016A9 RID: 5801
		private List<WgCharaProperty> m_PropertyList = new List<WgCharaProperty>();

		// Token: 0x040016AA RID: 5802
		private Control m_GridAtk;

		// Token: 0x040016AB RID: 5803
		public GameObject m_AtkType;

		// Token: 0x040016AC RID: 5804
		private List<WgCharaAtkType> m_AtkTypeList = new List<WgCharaAtkType>();

		// Token: 0x040016AD RID: 5805
		private Control m_GridDef;

		// Token: 0x040016AE RID: 5806
		public GameObject m_DefType;

		// Token: 0x040016AF RID: 5807
		private List<WgCharaDefType> m_DefTypeList = new List<WgCharaDefType>();

		// Token: 0x040016B0 RID: 5808
		private WgBackpackTwoForm m_BackpackTwoForm;

		// Token: 0x040016B1 RID: 5809
		private WgSkillInfoForm m_SkillInfoForm;

		// Token: 0x040016B2 RID: 5810
		private WgPracticeTwoForm m_PracticeTwoForm;

		// Token: 0x040016B3 RID: 5811
		private Control m_IconMartialProperty;

		// Token: 0x040016B4 RID: 5812
		private Control m_IconCharacterExit;

		// Token: 0x040016B5 RID: 5813
		private UIEventListener[] m_ChangeMemberList = new UIEventListener[2];

		// Token: 0x040016B6 RID: 5814
		private int[] m_EquipArray;

		// Token: 0x040016B7 RID: 5815
		private List<Control> m_FunctionBtnList = new List<Control>();

		// Token: 0x040016B8 RID: 5816
		private WgRemoveTeammate m_WgRemoveTeammate;

		// Token: 0x040016B9 RID: 5817
		private Control m_LblAttributePoints;

		// Token: 0x040016BA RID: 5818
		private WgUpgradeStatus m_WgUpgradeStatus;

		// Token: 0x040016BB RID: 5819
		private WgUseItemTwoForm m_WgUseItemTwoForm;

		// Token: 0x0200033B RID: 827
		public enum eState
		{
			// Token: 0x040016BF RID: 5823
			None,
			// Token: 0x040016C0 RID: 5824
			CharaData,
			// Token: 0x040016C1 RID: 5825
			Equit,
			// Token: 0x040016C2 RID: 5826
			Practice,
			// Token: 0x040016C3 RID: 5827
			UpgradeStatus,
			// Token: 0x040016C4 RID: 5828
			UseMedicine,
			// Token: 0x040016C5 RID: 5829
			UseSecretBook,
			// Token: 0x040016C6 RID: 5830
			RemoveTeammate,
			// Token: 0x040016C7 RID: 5831
			Count
		}

		// Token: 0x0200033C RID: 828
		private enum eFunctionBenType
		{
			// Token: 0x040016C9 RID: 5833
			UpgradeStatus,
			// Token: 0x040016CA RID: 5834
			UseMedicine,
			// Token: 0x040016CB RID: 5835
			UseSecretBook,
			// Token: 0x040016CC RID: 5836
			RemoveTeammate
		}
	}
}
