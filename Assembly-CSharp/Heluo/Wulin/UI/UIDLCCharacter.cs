using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000340 RID: 832
	public class UIDLCCharacter : UILayer
	{
		// Token: 0x060012DC RID: 4828 RVA: 0x0000C349 File Offset: 0x0000A549
		protected override void Awake()
		{
			this.m_IconEquipList = new List<Control>();
			this.m_EquipArray = new int[3];
			base.Awake();
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x000A42B0 File Offset: 0x000A24B0
		private void Start()
		{
			this.m_CtrlCharacter.m_UICharacter = this;
			CtrlDLCCharacter ctrlCharacter = this.m_CtrlCharacter;
			ctrlCharacter.OnSetCurrent = (Action<UIEventListener>)Delegate.Combine(ctrlCharacter.OnSetCurrent, new Action<UIEventListener>(base.SetCurrentOnly));
			base.CreateUIWidget<WgCharaProperty>("WgCharaProperty", this.m_GridProperty.GameObject, this.m_TextGroupProperty, this.m_PropertyList, this.m_CtrlCharacter.PropertyArray.Length);
			int cloneCount = 8;
			base.CreateUIWidget<WgDLCCharaDefType>("DefType", this.m_GridDef.GameObject, this.m_DefType, this.m_DefTypeList, cloneCount);
			for (int i = 0; i < this.m_DefTypeList.Count; i++)
			{
				if (this.m_DefTypeList[i] != null)
				{
					WgDLCCharaDefType wgDLCCharaDefType = this.m_DefTypeList[i];
					wgDLCCharaDefType.OnAddClick = (Action<CharacterData.PropertyType>)Delegate.Combine(wgDLCCharaDefType.OnAddClick, new Action<CharacterData.PropertyType>(this.m_CtrlCharacter.OnDefAddClick));
				}
			}
			base.CreateUIWidget<WgGroupSkill>("Talentl", this.m_GridTalentl.GameObject, this.m_WgGroupSkill, this.m_TalentlList, this.m_CtrlCharacter.m_iOnceSkillMax);
			for (int j = 0; j < this.m_TalentlList.Count; j++)
			{
				this.m_TalentlList[j].GroupSkillOnClick = new Action<PracticeDataNode>(this.m_CtrlCharacter.GroupSkillOnClick);
				this.m_TalentlList[j].GroupSkillOnHover = new Action<GameObject, bool>(this.m_CtrlCharacter.GroupSkillOnHover);
				this.m_TalentlList[j].SetCurrent = new Action<GameObject>(this.m_CtrlCharacter.SetCurrent);
			}
			base.CreateUIWidget<WgGroupSkill>("Martial", this.m_GridMartial.GameObject, this.m_WgGroupSkill, this.m_MartialList, this.m_CtrlCharacter.m_iOnceSkillMax);
			for (int k = 0; k < this.m_MartialList.Count; k++)
			{
				this.m_MartialList[k].GroupSkillOnClick = new Action<PracticeDataNode>(this.m_CtrlCharacter.GroupSkillOnClick);
				this.m_MartialList[k].GroupSkillOnHover = new Action<GameObject, bool>(this.m_CtrlCharacter.GroupSkillOnHover);
				this.m_MartialList[k].SetCurrent = new Action<GameObject>(this.m_CtrlCharacter.SetCurrent);
			}
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x000A4508 File Offset: 0x000A2708
		protected override void AssignControl(Transform sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIDLCCharacter.<>f__switch$map30 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(23);
					dictionary.Add("Group", 0);
					dictionary.Add("IconSect", 1);
					dictionary.Add("LblSect", 2);
					dictionary.Add("LblTitle", 3);
					dictionary.Add("LblName", 4);
					dictionary.Add("LblIntrduction", 5);
					dictionary.Add("IconMember", 6);
					dictionary.Add("IconEquip", 7);
					dictionary.Add("GridTalentl", 8);
					dictionary.Add("GridMartial", 9);
					dictionary.Add("GridProperty", 10);
					dictionary.Add("GridDef", 11);
					dictionary.Add("IconCloseTwoForm", 12);
					dictionary.Add("IconCharacterExit", 13);
					dictionary.Add("EquipOn", 14);
					dictionary.Add("LabelResistPoint", 15);
					dictionary.Add("Cure", 16);
					dictionary.Add("GoldLabel", 17);
					dictionary.Add("PrestigeLabel", 18);
					dictionary.Add("UnitNumber", 19);
					dictionary.Add("UnitWarring", 20);
					dictionary.Add("WarringOK", 21);
					dictionary.Add("GridEquip", 22);
					UIDLCCharacter.<>f__switch$map30 = dictionary;
				}
				int num;
				if (UIDLCCharacter.<>f__switch$map30.TryGetValue(name, ref num))
				{
					switch (num)
					{
					case 0:
						this.m_Group = sender;
						break;
					case 1:
						this.m_IconSect = sender;
						break;
					case 2:
						this.m_LblSect = sender;
						break;
					case 3:
						this.m_LblTitle = sender;
						break;
					case 4:
						this.m_LblName = sender;
						break;
					case 5:
						this.m_LblIntrduction = sender;
						break;
					case 6:
						this.m_IconMember = sender;
						break;
					case 7:
					{
						Control control = sender;
						control.OnClick += this.IconEquipOnClick;
						control.OnHover += this.IconEquipOnHover;
						control.OnKeySelect += this.IconEquipOnKeySelect;
						base.SetInputButton(1, control.Listener);
						this.m_IconEquipList.Add(control);
						break;
					}
					case 8:
						this.m_GridTalentl = sender;
						break;
					case 9:
						this.m_GridMartial = sender;
						break;
					case 10:
						this.m_GridProperty = sender;
						break;
					case 11:
						this.m_GridDef = sender;
						break;
					case 12:
					{
						Control control = sender;
						control.OnClick += this.IconCloseTwoFormOnClick;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					case 13:
					{
						Control control = sender;
						control.OnHover += this.IconCharacterExitOnHover;
						control.OnClick += this.IconCharacterExitOnClick;
						this.m_IconCharacterExit = sender;
						control.SetPlaySound(UIPlaySound.Trigger.OnClick, UIAudioData.eUIAudio.Cancel);
						break;
					}
					case 14:
						this.m_EquipOn = sender;
						break;
					case 15:
						this.m_LabelResistPoint = sender;
						break;
					case 16:
						this.m_Cure = sender;
						this.m_Cure.OnClick += this.OnCureClick;
						break;
					case 17:
						this.m_GoldLabel = sender;
						break;
					case 18:
						this.m_PrestigeLabel = sender;
						break;
					case 19:
						this.m_UnitNumber = sender;
						break;
					case 20:
						this.m_UnitWarring = sender;
						break;
					case 21:
						this.m_WarringOK = sender;
						this.m_WarringOK.OnClick += this.CloseUnitWarring;
						break;
					case 22:
						this.m_GridEquip = sender;
						break;
					}
				}
			}
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x000A491C File Offset: 0x000A2B1C
		protected override void AssignWidget(Widget sender)
		{
			string name = sender.name;
			if (name != null)
			{
				if (UIDLCCharacter.<>f__switch$map31 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(9);
					dictionary.Add("EquipTip", 0);
					dictionary.Add("SkillInfoForm", 1);
					dictionary.Add("BackpackTwoForm", 2);
					dictionary.Add("UseItemTwoForm", 3);
					dictionary.Add("Neigong", 4);
					dictionary.Add("Talent", 5);
					dictionary.Add("WgCharacterList", 6);
					dictionary.Add("UnitOrder", 7);
					dictionary.Add("WgCheckUpgradeUnit", 8);
					UIDLCCharacter.<>f__switch$map31 = dictionary;
				}
				int num;
				if (UIDLCCharacter.<>f__switch$map31.TryGetValue(name, ref num))
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
					{
						this.m_BackpackTwoForm = (sender as WgBackpackTwoForm);
						WgBackpackTwoForm backpackTwoForm = this.m_BackpackTwoForm;
						backpackTwoForm.OnCloseClick = (Action<GameObject>)Delegate.Combine(backpackTwoForm.OnCloseClick, new Action<GameObject>(this.IconCloseTwoFormOnClick));
						WgBackpackTwoForm backpackTwoForm2 = this.m_BackpackTwoForm;
						backpackTwoForm2.OnCloseHover = (Action<GameObject, bool>)Delegate.Combine(backpackTwoForm2.OnCloseHover, new Action<GameObject, bool>(this.IconCloseTwoFormOnHover));
						WgBackpackTwoForm backpackTwoForm3 = this.m_BackpackTwoForm;
						backpackTwoForm3.UseEquip = (Action<GameObject>)Delegate.Combine(backpackTwoForm3.UseEquip, new Action<GameObject>(this.m_CtrlCharacter.UseEqupip));
						WgBackpackTwoForm backpackTwoForm4 = this.m_BackpackTwoForm;
						backpackTwoForm4.RemoveEqupip = (Action<GameObject, int>)Delegate.Combine(backpackTwoForm4.RemoveEqupip, new Action<GameObject, int>(this.m_CtrlCharacter.RemoveEqupip));
						break;
					}
					case 3:
						this.m_WgUseItemTwoForm = (sender as WgUseItemTwoForm);
						break;
					case 4:
					{
						this.m_Neigong = (sender as WgDLCNeigong);
						WgDLCNeigong neigong = this.m_Neigong;
						neigong.OnNeigongClick = (Action<GameObject>)Delegate.Combine(neigong.OnNeigongClick, new Action<GameObject>(this.m_CtrlCharacter.OnNeigonClick));
						WgDLCNeigong neigong2 = this.m_Neigong;
						neigong2.OnNeigongHover = (Action<GameObject, bool>)Delegate.Combine(neigong2.OnNeigongHover, new Action<GameObject, bool>(this.m_CtrlCharacter.OnNeigonHover));
						break;
					}
					case 5:
					{
						this.m_WgTalentTwoForm = (sender as WgTalentTwoForm);
						WgTalentTwoForm wgTalentTwoForm = this.m_WgTalentTwoForm;
						wgTalentTwoForm.OnLearnSkill = (Action<int>)Delegate.Combine(wgTalentTwoForm.OnLearnSkill, new Action<int>(this.m_CtrlCharacter.OnLearnSkill));
						WgTalentTwoForm wgTalentTwoForm2 = this.m_WgTalentTwoForm;
						wgTalentTwoForm2.NodeOnHover = (Action<GameObject, bool>)Delegate.Combine(wgTalentTwoForm2.NodeOnHover, new Action<GameObject, bool>(this.m_CtrlCharacter.NodeOnHover));
						WgTalentTwoForm wgTalentTwoForm3 = this.m_WgTalentTwoForm;
						wgTalentTwoForm3.OnCloseClick = (Action<GameObject>)Delegate.Combine(wgTalentTwoForm3.OnCloseClick, new Action<GameObject>(this.IconCloseTwoFormOnClick));
						break;
					}
					case 6:
					{
						this.m_WgCharacterList = (sender as WgCharacterList);
						WgCharacterList wgCharacterList = this.m_WgCharacterList;
						wgCharacterList.SetCurNpcID = (Action<string>)Delegate.Combine(wgCharacterList.SetCurNpcID, new Action<string>(this.m_CtrlCharacter.SetCurNpcID));
						break;
					}
					case 7:
						this.m_WgUnitOrder = (sender as WgUnitOrder);
						break;
					case 8:
						this.m_WgCheckUpgradeUnit = (sender as WgCheckUpgradeUnit);
						break;
					}
				}
			}
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x0000C368 File Offset: 0x0000A568
		public void IconCharacterExitOnClick(GameObject go)
		{
			go.GetComponent<UISprite>().spriteName = "cdata_027";
			this.Hide();
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x000A3240 File Offset: 0x000A1440
		public void IconCharacterExitOnHover(GameObject go, bool bHover)
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

		// Token: 0x060012E2 RID: 4834 RVA: 0x000A3240 File Offset: 0x000A1440
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

		// Token: 0x060012E3 RID: 4835 RVA: 0x0000C380 File Offset: 0x0000A580
		public void IconCloseTwoFormOnClick(GameObject go)
		{
			go.GetComponent<UISprite>().spriteName = "cdata_027";
			this.ResetTwoForm();
			this.m_CtrlCharacter.UpdateCaracterData();
			this.m_WgCharacterList.Obj.SetActive(true);
			this.m_Neigong.SetHasPoint();
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x000A4C40 File Offset: 0x000A2E40
		public void IconEquipOnKeySelect(GameObject go, bool bSelect)
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
				this.SetControl(this.m_EquipTip.Obj, go, new Vector3(60f, 0f, 0f));
				this.m_EquipTip.SetEquipTipNone();
			}
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x000A4CF8 File Offset: 0x000A2EF8
		public void IconEquipOnHover(GameObject go, bool bHover)
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
					this.SetControl(this.m_EquipTip.Obj, go, new Vector3(60f, 0f, 0f));
					this.m_EquipTip.SetEquipTipNone();
				}
				this.m_CtrlCharacter.SetCurrent(go);
				this.m_EquipTip.SpriteName = "cdata_024";
			}
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x000A4DE0 File Offset: 0x000A2FE0
		public void IconEquipOnClick(GameObject go)
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
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0000C3BF File Offset: 0x0000A5BF
		public void UpdateSkillTowForm(int talentID)
		{
			this.ResetTwoForm();
			this.m_SkillInfoForm.UpdateSkillTowForm(talentID);
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0000C3D3 File Offset: 0x0000A5D3
		public void UpdateSkillTowForm(object param, int maxSp)
		{
			this.ResetTwoForm();
			this.m_SkillInfoForm.UpdateSkillTowForm(param, maxSp);
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0000264F File Offset: 0x0000084F
		public void SetNeigongUse(int idx, bool buse)
		{
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0000C3E8 File Offset: 0x0000A5E8
		public void SetNeigong(int idx, NpcNeigong neigong, UIDLCCharacter.UIType type, int iNeigongLV)
		{
			this.m_Neigong.SetNeigong(neigong, type, iNeigongLV);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0000C3F9 File Offset: 0x0000A5F9
		public void SetMartialUse(int idx, bool buse)
		{
			this.m_MartialList[idx].SetUsing(buse);
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0000C40D File Offset: 0x0000A60D
		public void SetMartial(int idx, NpcRoutine routine)
		{
			this.m_MartialList[idx].SetGroupSkillText(routine);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0000C421 File Offset: 0x0000A621
		public void SetTalent(int idx, int id)
		{
			this.m_TalentlList[idx].SetGroupSkillText(id);
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0000C435 File Offset: 0x0000A635
		public void SetCharaDefType(int index, CharacterData.PropertyType type, int val, bool isShow)
		{
			this.m_DefTypeList[index].SetDefTypeText(type, val);
			this.m_DefTypeList[index].IsShowAdd(isShow);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x000A4E58 File Offset: 0x000A3058
		public void SetCharaInfo(string[] text, Texture texMapHead, bool isHurt)
		{
			this.m_IconSect.SpriteName = text[0];
			this.m_LblSect.Text = text[1];
			this.m_LblTitle.Text = text[2];
			this.m_LblName.Text = text[3];
			this.m_LblIntrduction.Text = text[4];
			this.m_IconMember.Texture = texMapHead;
			this.m_Cure.GameObject.SetActive(isHurt);
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0000C45D File Offset: 0x0000A65D
		public void SetCharaProperty(string[] text, int idx)
		{
			this.m_PropertyList[idx].SetPropertyText(text[0], text[1]);
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0000C476 File Offset: 0x0000A676
		public void UpdateCharacterEquip(CharacterData _CharacterData)
		{
			if (_CharacterData == null)
			{
				return;
			}
			this.m_EquipArray[0] = _CharacterData.iEquipWeaponID;
			this.m_EquipArray[1] = _CharacterData.iEquipArrorID;
			this.m_EquipArray[2] = _CharacterData.iEquipNecklaceID;
			this.SetEquipData();
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x000A4EC8 File Offset: 0x000A30C8
		public void SetEquipData()
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

		// Token: 0x060012F3 RID: 4851 RVA: 0x000A4F6C File Offset: 0x000A316C
		public void SetEquipTipData(GameObject goes, BackpackNewDataNode node)
		{
			if (this.NowClickEquip != null)
			{
				this.SetControl(this.m_EquipTip.Obj, this.NowClickEquip, new Vector3(60f, 0f, 0f));
			}
			else
			{
				this.SetControl(this.m_EquipTip.Obj, goes, new Vector3(60f, 0f, 0f));
			}
			string equipTip = string.Empty;
			string text = string.Empty;
			string addEffectTip = string.Empty;
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
							string text2 = text;
							text = string.Concat(new object[]
							{
								text2,
								@string,
								" +",
								iValue,
								"  "
							});
						}
						else
						{
							string text2 = text;
							text = string.Concat(new object[]
							{
								text2,
								@string,
								" ",
								iValue,
								"  "
							});
						}
					}
					else if (iItemType == 7)
					{
						string @string = Game.StringTable.GetString(160004);
						ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(iRecoverType);
						addEffectTip = @string + ":\n" + conditionNode.m_strName;
					}
				}
			}
			this.m_EquipTip.SetEquipTip(equipTip, text, addEffectTip);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0000C4AF File Offset: 0x0000A6AF
		public void UpdateBackpackTwoFormData(CharacterData cdata, List<BackpackNewDataNode> sortList, int iType)
		{
			this.m_BackpackTwoForm.SetBackpackEquipData(cdata, sortList, iType);
			this.m_WgCharacterList.Obj.SetActive(false);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x000A5150 File Offset: 0x000A3350
		public void ReSetGroupSkill()
		{
			for (int i = 0; i < this.m_TalentlList.Count; i++)
			{
				this.m_TalentlList[i].ReSetGroupSkill();
				this.m_MartialList[i].ReSetGroupSkill();
			}
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x000A519C File Offset: 0x000A339C
		public void SetEquipTexture(string _Icon, List<Control> m_IconList, int iIndex)
		{
			Texture texture = Game.g_Item.Load("2dtexture/gameui/item/" + _Icon) as Texture;
			if (texture != null)
			{
				m_IconList[iIndex].GetComponent<UITexture>().mainTexture = texture;
			}
			else
			{
				m_IconList[iIndex].GetComponent<UITexture>().mainTexture = null;
				GameDebugTool.Log("無此裝備貼圖 : " + _Icon);
			}
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x000A520C File Offset: 0x000A340C
		public void ResetTwoForm()
		{
			if (!this.m_WgTalentTwoForm.Obj.activeSelf)
			{
				this.m_WgCharacterList.Obj.SetActive(true);
			}
			this.m_BackpackTwoForm.Obj.SetActive(false);
			this.m_SkillInfoForm.Obj.SetActive(false);
			this.m_EquipTip.SetActive(false);
			this.NowClickEquip = null;
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x000A5274 File Offset: 0x000A3474
		public void ShowByID(int npdID)
		{
			if (this.m_bShow)
			{
				return;
			}
			base.SetCurrent(null, false);
			this.m_bShow = true;
			TeamStatus.m_Instance.m_strcFormName.Add(base.gameObject.name);
			GameGlobal.m_bCFormOpen = true;
			Game.g_InputManager.Push(this);
			this.m_Group.GameObject.SetActive(true);
			this.m_CtrlCharacter.SetCurNpcID(npdID.ToString());
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x000A52EC File Offset: 0x000A34EC
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
			base.SetCurrent(null, false);
			this.m_CtrlCharacter.SetCurNpcID("0");
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x000A5360 File Offset: 0x000A3560
		public void Show(UIDLCCharacter.UIType type, string id = "0")
		{
			this.Hide();
			this.m_CtrlCharacter.type = type;
			if (this.m_bShow)
			{
				return;
			}
			this.m_bShow = true;
			TeamStatus.m_Instance.m_strcFormName.Add(base.gameObject.name);
			GameGlobal.m_bCFormOpen = true;
			Game.g_InputManager.Push(this);
			this.m_Group.GameObject.SetActive(true);
			base.SetCurrent(null, false);
			this.m_CtrlCharacter.SetCurNpcID(id);
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x000A53E4 File Offset: 0x000A35E4
		public override void Hide()
		{
			if (!this.m_bShow)
			{
				return;
			}
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
			this.m_WgCharacterList.ClearBtnHero();
			this.m_CtrlCharacter.charaCount = 0;
			UIReadyCombat uireadyCombat = Game.UI.Get<UIReadyCombat>();
			if (uireadyCombat != null)
			{
				uireadyCombat.ResetShopAndInfo();
			}
			this.m_CtrlCharacter.UnitWarringTrigger = true;
			if (GameGlobal.m_bDLCMode)
			{
				Game.UI.Get<UIReadyCombat>().UpdateGold();
			}
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x000A3F38 File Offset: 0x000A2138
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

		// Token: 0x060012FD RID: 4861 RVA: 0x0000C4D0 File Offset: 0x0000A6D0
		public void OpenWgTalentTwoForm(UpgradeNode node, CharacterData data)
		{
			this.m_WgTalentTwoForm.SetTalent(node, data);
			this.m_WgCharacterList.Obj.SetActive(false);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x0000C4F0 File Offset: 0x0000A6F0
		public void UpdateSkillTowFormDLC(int skillTreeID)
		{
			this.ResetTwoForm();
			this.m_SkillInfoForm.UpdateSkillTowFormDLC(skillTreeID);
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x000A54C8 File Offset: 0x000A36C8
		public void UpdateResistPoint(int point)
		{
			for (int i = 0; i < this.m_DefTypeList.Count; i++)
			{
				if (this.m_DefTypeList[i] != null)
				{
					this.m_DefTypeList[i].IsShowAdd(point != 0);
				}
			}
			this.m_LabelResistPoint.Text = point.ToString();
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x0000C504 File Offset: 0x0000A704
		public void SetCharaList(List<CharacterData> charas, bool reset = false)
		{
			if (reset)
			{
				this.m_WgCharacterList.ResetCharaList();
			}
			else
			{
				this.m_WgCharacterList.SetCharacterList(charas);
			}
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x0000C528 File Offset: 0x0000A728
		public void SetDLCUnitList(List<DLCUnitInfo> infos, bool reset = false)
		{
			if (reset)
			{
				this.m_WgCharacterList.ResetCharaList();
			}
			else
			{
				this.m_WgCharacterList.SetDLCUnitList(infos);
			}
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0000C54C File Offset: 0x0000A74C
		public void UpdateCurrency(int iGold, int iPrestige)
		{
			this.m_GoldLabel.Text = iGold.ToString();
			this.m_PrestigeLabel.Text = iPrestige.ToString();
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x000A5534 File Offset: 0x000A3734
		public void UpdateUnitNumber(int val, int max, bool UnitWarringTrigger)
		{
			string @string = Game.StringTable.GetString(990076);
			if (max >= val)
			{
				this.m_UnitNumber.Text = string.Concat(new object[]
				{
					@string,
					" ",
					val,
					"/",
					max
				});
			}
			else
			{
				this.m_UnitNumber.Text = string.Concat(new object[]
				{
					"[ff0000]",
					@string,
					" ",
					val,
					"/",
					max,
					"[-]"
				});
				this.ShowUnitWarring(UnitWarringTrigger);
			}
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0000C572 File Offset: 0x0000A772
		private void ShowUnitWarring(bool UnitWarringTrigger)
		{
			if (UnitWarringTrigger)
			{
				this.m_UnitWarring.GameObject.SetActive(true);
				this.m_CtrlCharacter.UnitWarringTrigger = false;
			}
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0000C597 File Offset: 0x0000A797
		private void CloseUnitWarring(GameObject obj)
		{
			this.m_UnitWarring.GameObject.SetActive(false);
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x000A55EC File Offset: 0x000A37EC
		public void SetUITemplate(UIDLCCharacter.UIType type)
		{
			if (type != UIDLCCharacter.UIType.Kinght)
			{
				if (type == UIDLCCharacter.UIType.Unit)
				{
					this.m_Cure.GameObject.SetActive(false);
					this.m_Neigong.Obj.SetActive(true);
					this.m_UnitNumber.GameObject.SetActive(true);
					this.m_WgUnitOrder.Obj.SetActive(true);
					this.m_GridEquip.GameObject.SetActive(false);
				}
			}
			else
			{
				this.m_Cure.GameObject.SetActive(true);
				this.m_Neigong.Obj.SetActive(true);
				this.m_UnitNumber.GameObject.SetActive(false);
				this.m_WgUnitOrder.Obj.SetActive(false);
				this.m_GridEquip.GameObject.SetActive(true);
			}
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x000A56C4 File Offset: 0x000A38C4
		public void OnCureClick(GameObject obj)
		{
			string @string = Game.StringTable.GetString(990075);
			string text = Game.StringTable.GetString(990083);
			string string2 = Game.StringTable.GetString(100009);
			string string3 = Game.StringTable.GetString(100005);
			text = string.Format(text, 100 * this.m_CtrlCharacter.m_CurCaraData.iLevel);
			this.m_WgCheckUpgradeUnit.ShowWgCheckUpgradeUnit(@string, text, string2, string3, new Action(this.m_CtrlCharacter.OnCureOK), new Action(this.m_CtrlCharacter.OnCureCancel));
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0000C5AA File Offset: 0x0000A7AA
		public void ShowWgCheckUpgradeUnit(string title, string msg, string LblOK, string LblCancel, Action callbackOK, Action callbackCancel)
		{
			this.m_WgCharacterList.Block(true);
			this.m_WgCheckUpgradeUnit.ShowWgCheckUpgradeUnit(title, msg, LblOK, LblCancel, callbackOK, callbackCancel);
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0000C5CC File Offset: 0x0000A7CC
		public void CloseCheckUI()
		{
			this.m_WgCharacterList.Block(false);
			this.m_WgCheckUpgradeUnit.Close();
		}

		// Token: 0x040016D0 RID: 5840
		public CtrlDLCCharacter m_CtrlCharacter = new CtrlDLCCharacter();

		// Token: 0x040016D1 RID: 5841
		private UIEventListener[] m_CurStateArray = new UIEventListener[8];

		// Token: 0x040016D2 RID: 5842
		private Control m_Group;

		// Token: 0x040016D3 RID: 5843
		private Control m_IconSect;

		// Token: 0x040016D4 RID: 5844
		private Control m_LblSect;

		// Token: 0x040016D5 RID: 5845
		private Control m_LblTitle;

		// Token: 0x040016D6 RID: 5846
		private Control m_LblName;

		// Token: 0x040016D7 RID: 5847
		private Control m_LblIntrduction;

		// Token: 0x040016D8 RID: 5848
		private Control m_IconMember;

		// Token: 0x040016D9 RID: 5849
		private Control m_EquipOn;

		// Token: 0x040016DA RID: 5850
		private Control m_Cure;

		// Token: 0x040016DB RID: 5851
		public WgCheckUpgradeUnit m_WgCheckUpgradeUnit;

		// Token: 0x040016DC RID: 5852
		private WgUnitOrder m_WgUnitOrder;

		// Token: 0x040016DD RID: 5853
		private Control m_GoldLabel;

		// Token: 0x040016DE RID: 5854
		private Control m_PrestigeLabel;

		// Token: 0x040016DF RID: 5855
		private List<Control> m_IconEquipList;

		// Token: 0x040016E0 RID: 5856
		private WgCharaEquipTip m_EquipTip;

		// Token: 0x040016E1 RID: 5857
		private GameObject NowClickEquip;

		// Token: 0x040016E2 RID: 5858
		public GameObject m_WgGroupSkill;

		// Token: 0x040016E3 RID: 5859
		private Control m_LabelResistPoint;

		// Token: 0x040016E4 RID: 5860
		private Control m_GridTalentl;

		// Token: 0x040016E5 RID: 5861
		private List<WgGroupSkill> m_TalentlList = new List<WgGroupSkill>();

		// Token: 0x040016E6 RID: 5862
		private Control m_GridMartial;

		// Token: 0x040016E7 RID: 5863
		private List<WgGroupSkill> m_MartialList = new List<WgGroupSkill>();

		// Token: 0x040016E8 RID: 5864
		private WgDLCNeigong m_Neigong;

		// Token: 0x040016E9 RID: 5865
		private Control m_NewKnight;

		// Token: 0x040016EA RID: 5866
		private Control m_GridProperty;

		// Token: 0x040016EB RID: 5867
		public GameObject m_TextGroupProperty;

		// Token: 0x040016EC RID: 5868
		private List<WgCharaProperty> m_PropertyList = new List<WgCharaProperty>();

		// Token: 0x040016ED RID: 5869
		private Control m_GridDef;

		// Token: 0x040016EE RID: 5870
		public GameObject m_DefType;

		// Token: 0x040016EF RID: 5871
		private List<WgDLCCharaDefType> m_DefTypeList = new List<WgDLCCharaDefType>();

		// Token: 0x040016F0 RID: 5872
		private WgBackpackTwoForm m_BackpackTwoForm;

		// Token: 0x040016F1 RID: 5873
		private WgSkillInfoForm m_SkillInfoForm;

		// Token: 0x040016F2 RID: 5874
		private Control m_IconCharacterExit;

		// Token: 0x040016F3 RID: 5875
		private int[] m_EquipArray;

		// Token: 0x040016F4 RID: 5876
		private WgCharacterList m_WgCharacterList;

		// Token: 0x040016F5 RID: 5877
		private WgUseItemTwoForm m_WgUseItemTwoForm;

		// Token: 0x040016F6 RID: 5878
		private WgTalentTwoForm m_WgTalentTwoForm;

		// Token: 0x040016F7 RID: 5879
		private Control m_UnitNumber;

		// Token: 0x040016F8 RID: 5880
		private Control m_UnitWarring;

		// Token: 0x040016F9 RID: 5881
		public Control m_WarringOK;

		// Token: 0x040016FA RID: 5882
		private Control m_GridEquip;

		// Token: 0x02000341 RID: 833
		public enum eState
		{
			// Token: 0x040016FE RID: 5886
			None,
			// Token: 0x040016FF RID: 5887
			CharaData,
			// Token: 0x04001700 RID: 5888
			Equit,
			// Token: 0x04001701 RID: 5889
			Practice,
			// Token: 0x04001702 RID: 5890
			UpgradeStatus,
			// Token: 0x04001703 RID: 5891
			UseMedicine,
			// Token: 0x04001704 RID: 5892
			UseSecretBook,
			// Token: 0x04001705 RID: 5893
			RemoveTeammate,
			// Token: 0x04001706 RID: 5894
			Count
		}

		// Token: 0x02000342 RID: 834
		public enum UIType
		{
			// Token: 0x04001708 RID: 5896
			Kinght,
			// Token: 0x04001709 RID: 5897
			Unit
		}
	}
}
