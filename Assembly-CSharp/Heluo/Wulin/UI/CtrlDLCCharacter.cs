using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002F2 RID: 754
	public class CtrlDLCCharacter : CtrlCharacterBase
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0000A890 File Offset: 0x00008A90
		// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x0000A887 File Offset: 0x00008A87
		public CharacterData m_CurCaraData { get; private set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0000A8A1 File Offset: 0x00008AA1
		// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x0000A898 File Offset: 0x00008A98
		public string NpcID { get; private set; }

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00087228 File Offset: 0x00085428
		public void SetCurNpcID(string id)
		{
			UIDLCCharacter.UIType uitype = this.type;
			if (uitype != UIDLCCharacter.UIType.Kinght)
			{
				if (uitype == UIDLCCharacter.UIType.Unit)
				{
					this.m_CurCaraData = TeamStatus.m_Instance.GetDLCUnitData(id);
					this.NpcID = id;
				}
			}
			else
			{
				this.m_CurCaraData = TeamStatus.m_Instance.GetTeamMember(id);
				this.NpcID = id;
			}
			this.UpdateCaracterData();
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00087290 File Offset: 0x00085490
		public void UpdateCaracterData()
		{
			this.SetUITemplate();
			this.UpdateUnitNumber();
			this.Currency();
			this.CharaList();
			this.CharaInfo();
			this.CharaProperty();
			this.CharaAtkDefType();
			this.ReSetGroupSkill();
			this.CharaTalent();
			this.CharaMartial();
			this.CharaNeigong();
			this.CharaEquip();
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0000A8A9 File Offset: 0x00008AA9
		private void SetUITemplate()
		{
			this.m_UICharacter.SetUITemplate(this.type);
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0000A8BC File Offset: 0x00008ABC
		private void UpdateUnitNumber()
		{
			if (this.type == UIDLCCharacter.UIType.Unit)
			{
				this.m_UICharacter.UpdateUnitNumber(TeamStatus.m_Instance.m_UnitInfoList.Count, TeamStatus.m_Instance.DLCUnitLimit, this.UnitWarringTrigger);
			}
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x000872E8 File Offset: 0x000854E8
		private void Currency()
		{
			int money = BackpackStatus.m_Instance.GetMoney();
			int prestigePoints = TeamStatus.m_Instance.GetPrestigePoints();
			this.m_UICharacter.UpdateCurrency(money, prestigePoints);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x00087318 File Offset: 0x00085518
		private void CharaList()
		{
			UIDLCCharacter.UIType uitype = this.type;
			if (uitype != UIDLCCharacter.UIType.Kinght)
			{
				if (uitype == UIDLCCharacter.UIType.Unit)
				{
					List<DLCUnitInfo> dlcunitList = TeamStatus.m_Instance.GetDLCUnitList();
					if (this.charaCount != dlcunitList.Count)
					{
						this.charaCount = dlcunitList.Count;
						this.m_UICharacter.SetDLCUnitList(dlcunitList, false);
					}
					else
					{
						this.m_UICharacter.SetDLCUnitList(dlcunitList, true);
					}
				}
			}
			else
			{
				List<CharacterData> teamMemberList = TeamStatus.m_Instance.GetTeamMemberList();
				if (this.charaCount != teamMemberList.Count)
				{
					this.charaCount = teamMemberList.Count;
					this.m_UICharacter.SetCharaList(teamMemberList, false);
				}
				else
				{
					this.m_UICharacter.SetCharaList(teamMemberList, true);
				}
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x000873DC File Offset: 0x000855DC
		private void CharaInfo()
		{
			string[] array = new string[6];
			int iID = this.m_CurCaraData._NpcDataNode.m_iMartialType + 140001;
			string text = iID.ToString();
			array[0] = text;
			array[1] = Game.StringTable.GetString(iID);
			array[2] = this.m_CurCaraData._NpcDataNode.m_strTitle;
			array[3] = this.m_CurCaraData._NpcDataNode.m_strNpcName;
			array[4] = this.m_CurCaraData._NpcDataNode.m_strDescription;
			array[5] = this.m_CurCaraData.iNpcID.ToString();
			Texture texMapHead = Game.g_MapHeadBundle.Load("2dtexture/gameui/maphead/" + this.m_CurCaraData._NpcDataNode.m_strHalfImage) as Texture;
			bool isHurt = this.m_CurCaraData.iHurtTurn > 0;
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.SetCharaInfo(array, texMapHead, isHurt);
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x000874C8 File Offset: 0x000856C8
		private void CharaProperty()
		{
			string[] array = new string[2];
			for (int i = 0; i < this.PropertyArray.Length; i++)
			{
				CharacterData.PropertyType propertyType = this.PropertyArray[i];
				string text = string.Empty;
				text = this.m_CurCaraData._TotalProperty.Get(propertyType).ToString();
				if (propertyType >= CharacterData.PropertyType.Dodge && propertyType <= CharacterData.PropertyType.Critical)
				{
					text += "%";
				}
				array[0] = Game.StringTable.GetString((int)(110100 + propertyType));
				array[1] = text;
				if (this.m_UICharacter != null)
				{
					this.m_UICharacter.SetCharaProperty(array, i);
				}
			}
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00087574 File Offset: 0x00085774
		private void CharaAtkDefType()
		{
			Dictionary<CharacterData.PropertyType, int> dictionary = new Dictionary<CharacterData.PropertyType, int>();
			for (int i = 21; i <= 28; i++)
			{
				int num = this.m_CurCaraData._TotalProperty._MartialArts.Get((CharacterData.PropertyType)i);
				dictionary.Add((CharacterData.PropertyType)i, num);
			}
			int num2 = 0;
			for (int j = 41; j <= 48; j++)
			{
				int val = this.m_CurCaraData._TotalProperty._MartialDef.Get((CharacterData.PropertyType)j);
				if (this.m_UICharacter != null)
				{
					if (this.m_CurCaraData.iResistPoint != 0)
					{
						this.m_UICharacter.SetCharaDefType(num2, (CharacterData.PropertyType)j, val, true);
					}
					else
					{
						this.m_UICharacter.SetCharaDefType(num2, (CharacterData.PropertyType)j, val, false);
					}
				}
				num2++;
			}
			this.m_UICharacter.UpdateResistPoint(this.m_CurCaraData.iResistPoint);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0000A8F4 File Offset: 0x00008AF4
		private void ReSetGroupSkill()
		{
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.ReSetGroupSkill();
			}
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00087650 File Offset: 0x00085850
		private void CharaTalent()
		{
			for (int i = 0; i < this.m_CurCaraData.TalentList.Count; i++)
			{
				if (this.m_UICharacter != null)
				{
					this.m_UICharacter.SetTalent(i, this.m_CurCaraData.TalentList[i]);
				}
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x000876AC File Offset: 0x000858AC
		private void CharaMartial()
		{
			this.m_UseMartialCount = 0;
			for (int i = 0; i < this.m_CurCaraData.RoutineList.Count; i++)
			{
				if (this.m_UICharacter != null)
				{
					this.m_UICharacter.SetMartial(i, this.m_CurCaraData.RoutineList[i]);
				}
				if (this.m_CurCaraData.RoutineList[i].bUse)
				{
					this.m_UseMartialCount++;
				}
				if (this.m_UseMartialCount <= this.m_UseMartialMax && this.m_UICharacter != null)
				{
					this.m_UICharacter.SetMartialUse(i, this.m_CurCaraData.RoutineList[i].bUse);
				}
			}
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0008777C File Offset: 0x0008597C
		private void CharaNeigong()
		{
			for (int i = 0; i < this.m_CurCaraData.NeigongList.Count; i++)
			{
				if (this.m_UICharacter != null)
				{
					this.m_UICharacter.SetNeigong(i, this.m_CurCaraData.NeigongList[i], this.type, this.m_CurCaraData.iLevel);
				}
			}
			if (this.m_CurCaraData.NeigongList.Count <= 0 && this.m_UICharacter != null)
			{
				this.m_UICharacter.SetNeigong(0, null, this.type, this.m_CurCaraData.iLevel);
			}
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00087830 File Offset: 0x00085A30
		private void CharaEquip()
		{
			UIDLCCharacter.UIType uitype = this.type;
			if (uitype != UIDLCCharacter.UIType.Kinght)
			{
				if (uitype == UIDLCCharacter.UIType.Unit)
				{
					this.m_UICharacter.UpdateCharacterEquip(null);
				}
			}
			else
			{
				this.m_UICharacter.UpdateCharacterEquip(this.m_CurCaraData);
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00087880 File Offset: 0x00085A80
		public void GroupSkillOnHover(GameObject obj, bool isHover)
		{
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.ResetTwoForm();
			}
			if (!isHover)
			{
				return;
			}
			PracticeDataNode component = obj.transform.parent.GetComponent<PracticeDataNode>();
			if (component == null)
			{
				return;
			}
			if (component.SkillData == null)
			{
				if (this.m_UICharacter != null)
				{
					this.m_UICharacter.UpdateSkillTowForm(component.TalentID);
				}
				return;
			}
			if (component.SkillData.m_SkillType == NpcSkill.SkillType.Routine)
			{
				NpcRoutine npcRoutine = component.SkillData as NpcRoutine;
				int iRoutineID = npcRoutine.m_Routine.m_iRoutineID;
				NpcRoutine routineData = this.m_CurCaraData.GetRoutineData(iRoutineID);
				if (this.m_UICharacter != null)
				{
					this.m_UICharacter.UpdateSkillTowForm(routineData, this.m_CurCaraData._TotalProperty.Get(CharacterData.PropertyType.MaxSP));
				}
			}
			if (component.SkillData.m_SkillType == NpcSkill.SkillType.Neigong)
			{
				NpcNeigong npcNeigong = component.SkillData as NpcNeigong;
				int iNeigongID = npcNeigong.m_Neigong.m_iNeigongID;
				NpcNeigong neigongData = this.m_CurCaraData.GetNeigongData(iNeigongID);
				if (this.m_UICharacter != null)
				{
					this.m_UICharacter.UpdateSkillTowForm(neigongData, this.m_CurCaraData._TotalProperty.Get(CharacterData.PropertyType.MaxSP));
				}
			}
			this.SetCurrent(obj);
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x000879D0 File Offset: 0x00085BD0
		public void GroupSkillOnClick(PracticeDataNode node)
		{
			if (node.SkillData == null)
			{
				return;
			}
			if (node.SkillData.m_SkillType == NpcSkill.SkillType.Routine)
			{
				NpcRoutine npcRoutine = node.SkillData as NpcRoutine;
				bool flag = !npcRoutine.bUse;
				int num = this.m_UseMartialCount;
				if (flag)
				{
					num++;
					if (num <= this.m_UseMartialMax)
					{
						npcRoutine.bUse = true;
						this.m_UseMartialCount = num;
					}
					else
					{
						npcRoutine.bUse = false;
						string @string = Game.StringTable.GetString(110203);
						Game.UI.Get<UIMapMessage>().SetMsg(@string);
					}
				}
				else
				{
					num--;
					if (num < 1)
					{
						npcRoutine.bUse = true;
						string string2 = Game.StringTable.GetString(110204);
						Game.UI.Get<UIMapMessage>().SetMsg(string2);
					}
					else
					{
						npcRoutine.bUse = false;
						this.m_UseMartialCount = num;
					}
				}
				for (int i = 0; i < this.m_CurCaraData.RoutineList.Count; i++)
				{
					if (this.m_UseMartialCount <= this.m_UseMartialMax && this.m_UICharacter != null)
					{
						this.m_UICharacter.SetMartialUse(i, this.m_CurCaraData.RoutineList[i].bUse);
					}
				}
			}
			else if (node.SkillData.m_SkillType == NpcSkill.SkillType.Neigong)
			{
				NpcNeigong npcNeigong = node.SkillData as NpcNeigong;
				if (npcNeigong.bUse)
				{
					string string3 = Game.StringTable.GetString(110205);
					Game.UI.Get<UIMapMessage>().SetMsg(string3);
					return;
				}
				int iSkillID = npcNeigong.iSkillID;
				for (int j = 0; j < this.m_CurCaraData.NeigongList.Count; j++)
				{
					if (this.m_CurCaraData.NeigongList[j].m_Neigong.m_iNeigongID == iSkillID)
					{
						this.m_CurCaraData.NeigongList[j].bUse = true;
						if (this.m_UICharacter != null)
						{
							this.m_UICharacter.SetNeigongUse(j, true);
						}
					}
					else
					{
						this.m_CurCaraData.NeigongList[j].bUse = false;
						if (this.m_UICharacter != null)
						{
							this.m_UICharacter.SetNeigongUse(j, false);
						}
					}
				}
			}
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00087C38 File Offset: 0x00085E38
		public void SetCurrent(GameObject obj)
		{
			UIEventListener component = obj.GetComponent<UIEventListener>();
			if (component != null && this.OnSetCurrent != null)
			{
				this.OnSetCurrent.Invoke(component);
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00087C70 File Offset: 0x00085E70
		public void UpdateBackpackTwoFormData(int iType)
		{
			BackpackStatus.m_Instance.SortItem(iType);
			List<BackpackNewDataNode> sortTeamBackpack = BackpackStatus.m_Instance.GetSortTeamBackpack();
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.UpdateBackpackTwoFormData(this.m_CurCaraData, sortTeamBackpack, iType);
			}
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00087CB8 File Offset: 0x00085EB8
		public void UseEqupip(GameObject go)
		{
			BackpackNewDataNode backpackNode = go.GetComponent<BackpackDataNode>().m_BackpackNode;
			if (backpackNode == null)
			{
				BackpackStatus.m_Instance.UseItem(this.m_CurCaraData, backpackNode);
				return;
			}
			BackpackStatus.m_Instance.UseItem(this.m_CurCaraData, backpackNode);
			this.UpdateBackpackTwoFormData(backpackNode._ItemDataNode.m_iItemType);
			this.UpdateCaracterData();
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.SetEquipTipData(go, backpackNode);
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00087D30 File Offset: 0x00085F30
		public void RemoveEqupip(GameObject go, int iType)
		{
			switch (iType)
			{
			case 1:
				BackpackStatus.m_Instance.AddPackItem(this.m_CurCaraData.iEquipWeaponID, 1, false);
				break;
			case 2:
				BackpackStatus.m_Instance.AddPackItem(this.m_CurCaraData.iEquipArrorID, 1, false);
				break;
			case 3:
				BackpackStatus.m_Instance.AddPackItem(this.m_CurCaraData.iEquipNecklaceID, 1, false);
				break;
			}
			this.m_CurCaraData.SetEquip((ItemDataNode.ItemType)iType, 0);
			this.UpdateBackpackTwoFormData(iType);
			this.UpdateCaracterData();
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.SetEquipTipData(go, null);
			}
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00087134 File Offset: 0x00085334
		private string GreaterProperty(CharacterData _CharacterData, UseLimitNode ULN, bool bMore)
		{
			int characterProperty = NPC.m_instance.getCharacterProperty(0, ULN.m_iInde, _CharacterData);
			int iValue = ULN.m_iValue;
			string @string = Game.StringTable.GetString(110303);
			string string2 = Game.StringTable.GetString(110100 + ULN.m_iInde);
			string result = string.Empty;
			if (bMore)
			{
				if (characterProperty < iValue)
				{
					result = string.Format(@string, string2, (iValue - characterProperty).ToString());
				}
			}
			else if (characterProperty >= iValue)
			{
				result = string.Format(@string, string2, (characterProperty - iValue).ToString());
			}
			return result;
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00087DEC File Offset: 0x00085FEC
		public void OnNeigonClick(GameObject go)
		{
			UpgradeNode upgradeNode = Game.UpgradeData.GetUpgradeNode(this.m_CurCaraData.iNpcID);
			this.m_UICharacter.OpenWgTalentTwoForm(upgradeNode, this.m_CurCaraData);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00087E24 File Offset: 0x00086024
		public void OnNeigonHover(GameObject go, bool bhover)
		{
			this.m_UICharacter.ResetTwoForm();
			if (bhover)
			{
				NpcNeigong param = null;
				if (this.m_CurCaraData.NeigongList.Count > 0)
				{
					param = this.m_CurCaraData.NeigongList[0];
				}
				this.m_UICharacter.UpdateSkillTowForm(param, this.m_CurCaraData._TotalProperty.Get(CharacterData.PropertyType.MaxSP));
			}
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00087E8C File Offset: 0x0008608C
		public void OnLearnSkill(int id)
		{
			this.m_CurCaraData.DLC_AddPassiveNode(id);
			this.UpdateCaracterData();
			UpgradeNode upgradeNode = Game.UpgradeData.GetUpgradeNode(this.m_CurCaraData.iNpcID);
			this.m_UICharacter.OpenWgTalentTwoForm(upgradeNode, this.m_CurCaraData);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0000A912 File Offset: 0x00008B12
		public void NodeOnHover(GameObject go, bool hover)
		{
			if (hover)
			{
				this.m_UICharacter.UpdateSkillTowFormDLC(int.Parse(go.name));
			}
			else
			{
				this.m_UICharacter.ResetTwoForm();
			}
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0000A940 File Offset: 0x00008B40
		public void OnDefAddClick(CharacterData.PropertyType type)
		{
			this.m_CurCaraData.iResistPoint--;
			this.m_CurCaraData._MartialDef.SetPlus(type, 1);
			this.m_CurCaraData.setTotalProperty();
			this.UpdateCaracterData();
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00087ED4 File Offset: 0x000860D4
		public void OnCureOK()
		{
			int num = 100 * this.m_CurCaraData.iLevel;
			int money = BackpackStatus.m_Instance.GetMoney();
			this.m_UICharacter.CloseCheckUI();
			if (money < num)
			{
				string @string = Game.StringTable.GetString(200081);
				Game.UI.Get<UIMapMessage>().SetMsg(@string);
				return;
			}
			BackpackStatus.m_Instance.ChangeMoney(-num);
			this.m_CurCaraData.iHurtTurn = 0;
			this.UpdateCaracterData();
			UIReadyCombat uireadyCombat = Game.UI.Get<UIReadyCombat>();
			uireadyCombat.UpdateData();
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0000A979 File Offset: 0x00008B79
		public void OnCureCancel()
		{
			this.m_UICharacter.CloseCheckUI();
		}

		// Token: 0x040012ED RID: 4845
		public UIDLCCharacter m_UICharacter;

		// Token: 0x040012EE RID: 4846
		private List<BackpackNewDataNode> m_SortTeamBackpackList;

		// Token: 0x040012EF RID: 4847
		public CharacterData.PropertyType[] PropertyArray = new CharacterData.PropertyType[]
		{
			CharacterData.PropertyType.MaxHP,
			CharacterData.PropertyType.MaxSP,
			CharacterData.PropertyType.Dodge,
			CharacterData.PropertyType.Counter,
			CharacterData.PropertyType.Critical,
			CharacterData.PropertyType.MoveStep
		};

		// Token: 0x040012F0 RID: 4848
		public int m_iOnceSkillMax = 12;

		// Token: 0x040012F1 RID: 4849
		public int m_UseMartialCount;

		// Token: 0x040012F2 RID: 4850
		public int m_UseMartialMax = 6;

		// Token: 0x040012F3 RID: 4851
		public Action<UIEventListener> OnSetCurrent;

		// Token: 0x040012F4 RID: 4852
		public UIDLCCharacter.UIType type;

		// Token: 0x040012F5 RID: 4853
		public bool UnitWarringTrigger = true;

		// Token: 0x040012F6 RID: 4854
		public int charaCount;
	}
}
