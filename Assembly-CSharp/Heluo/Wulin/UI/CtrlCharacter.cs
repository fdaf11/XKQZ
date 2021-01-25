using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002F1 RID: 753
	public class CtrlCharacter : CtrlCharacterBase
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0000A7C5 File Offset: 0x000089C5
		// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x0000A7BC File Offset: 0x000089BC
		public CharacterData m_CurCaraData { get; private set; }

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0000A7CD File Offset: 0x000089CD
		public void SetCurNpcID(int iNpcID, bool bUpdate)
		{
			if (iNpcID == 0)
			{
				iNpcID = 210001;
			}
			this.m_CurNpcID = iNpcID;
			this.m_CurCaraData = NPC.m_instance.GetCharacterData(iNpcID);
			if (bUpdate)
			{
				this.UpdateCaracterData();
			}
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00085F54 File Offset: 0x00084154
		public void UpdateCaracterData()
		{
			this.CharaInfo();
			this.UpgradeAttributePoints(0);
			this.CharaProperty();
			this.CharaAtkDefType();
			this.ReSetGroupSkill();
			this.CharaTalent();
			this.CharaMartial();
			this.CharaNeigong();
			this.SetNowPractice();
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.UpdateCharacterData(this.m_CurCaraData);
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00085FBC File Offset: 0x000841BC
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
			array[5] = this.m_CurNpcID.ToString();
			Texture texMapHead = Game.g_MapHeadBundle.Load("2dtexture/gameui/maphead/" + this.m_CurCaraData._NpcDataNode.m_strHalfImage) as Texture;
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.SetCharaInfo(array, texMapHead);
			}
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00086090 File Offset: 0x00084290
		public void UpgradeAttributePoints(int val = 0)
		{
			int attributePoints = TeamStatus.m_Instance.GetAttributePoints();
			if (val == 0)
			{
				this.statusPoints = TeamStatus.m_Instance.GetAttributePoints();
			}
			else
			{
				this.statusPoints += val;
			}
			this.statusPoints = Mathf.Clamp(this.statusPoints, 0, attributePoints);
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.SetAttributePoints(this.statusPoints.ToString());
			}
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0000A800 File Offset: 0x00008A00
		public int GetAttributePoints()
		{
			return this.statusPoints;
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0008610C File Offset: 0x0008430C
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

		// Token: 0x06000FCD RID: 4045 RVA: 0x000861B8 File Offset: 0x000843B8
		private void CharaAtkDefType()
		{
			Dictionary<CharacterData.PropertyType, int> dictionary = new Dictionary<CharacterData.PropertyType, int>();
			for (int i = 21; i <= 28; i++)
			{
				int num = this.m_CurCaraData._TotalProperty._MartialArts.Get((CharacterData.PropertyType)i);
				dictionary.Add((CharacterData.PropertyType)i, num);
			}
			int num2 = 0;
			foreach (KeyValuePair<CharacterData.PropertyType, int> keyValuePair in Enumerable.Reverse<KeyValuePair<CharacterData.PropertyType, int>>(Enumerable.OrderBy<KeyValuePair<CharacterData.PropertyType, int>, int>(dictionary, (KeyValuePair<CharacterData.PropertyType, int> key) => key.Value)))
			{
				if (num2 >= this.m_AtkTypeShowMax)
				{
					break;
				}
				if (this.m_UICharacter != null)
				{
					this.m_UICharacter.SetCharaAtkType(num2, keyValuePair.Key, keyValuePair.Value);
				}
				num2++;
			}
			num2 = 0;
			for (int j = 41; j <= 48; j++)
			{
				int val = this.m_CurCaraData._TotalProperty._MartialDef.Get((CharacterData.PropertyType)j);
				if (this.m_UICharacter != null)
				{
					this.m_UICharacter.SetCharaDefType(num2, (CharacterData.PropertyType)j, val);
				}
				num2++;
			}
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x0000A808 File Offset: 0x00008A08
		private void ReSetGroupSkill()
		{
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.ReSetGroupSkill();
			}
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00086304 File Offset: 0x00084504
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

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00086360 File Offset: 0x00084560
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

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00086430 File Offset: 0x00084630
		private void CharaNeigong()
		{
			this.m_UseNeigongCount = 0;
			for (int i = 0; i < this.m_CurCaraData.NeigongList.Count; i++)
			{
				if (this.m_UICharacter != null)
				{
					this.m_UICharacter.SetNeigong(i, this.m_CurCaraData.NeigongList[i]);
				}
				if (this.m_CurCaraData.NeigongList[i].bUse)
				{
					this.m_UseNeigongCount++;
				}
				if (this.m_UICharacter != null)
				{
					if (this.m_UseNeigongCount <= GameGlobal.mod_Difficulty)
					{
						this.m_UICharacter.SetNeigongUse(i, this.m_CurCaraData.NeigongList[i].bUse);
					}
					else
					{
						this.m_UICharacter.SetNeigongUse(i, false);
						this.m_CurCaraData.mod_SetNeigongUnuse(this.m_CurCaraData.NeigongList[i].iSkillID);
					}
				}
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00086524 File Offset: 0x00084724
		private void SetNowPractice()
		{
			bool flag = false;
			for (int i = 0; i < this.m_CurCaraData.NeigongList.Count; i++)
			{
				if (this.m_CurCaraData.NeigongList[i].m_Neigong.m_iNeigongID == this.m_CurCaraData.NowPractice)
				{
					object practice = this.m_CurCaraData.NeigongList[i];
					this.SetPractice(practice);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < this.m_CurCaraData.RoutineList.Count; j++)
				{
					if (this.m_CurCaraData.RoutineList[j].m_Routine.m_iRoutineID == this.m_CurCaraData.NowPractice)
					{
						object practice = this.m_CurCaraData.RoutineList[j];
						this.SetPractice(practice);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				if (this.m_CurCaraData.NeigongList.Count > 0)
				{
					object practice = this.m_CurCaraData.NeigongList[0];
					this.SetPractice(practice);
					Debug.LogWarning("當前人物身上沒有" + this.m_CurCaraData.NowPractice + "的武學或內功 直接指定第一個內功");
				}
				else if (this.m_CurCaraData.RoutineList.Count > 0)
				{
					object practice = this.m_CurCaraData.RoutineList[0];
					this.SetPractice(practice);
					Debug.LogWarning("當前人物身上沒有" + this.m_CurCaraData.NowPractice + "的武學或內功 也沒有內功 直接指定第一個武學");
				}
				else
				{
					Debug.LogError("當前人物身上沒有" + this.m_CurCaraData.NowPractice + "的武學或內功");
				}
			}
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x000866F0 File Offset: 0x000848F0
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

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00086840 File Offset: 0x00084A40
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
				return;
			}
			if (node.SkillData.m_SkillType == NpcSkill.SkillType.Neigong)
			{
				NpcNeigong npcNeigong = node.SkillData as NpcNeigong;
				bool flag2 = !npcNeigong.bUse;
				int num2 = this.m_UseNeigongCount;
				if (flag2)
				{
					num2++;
					if (num2 <= GameGlobal.mod_Difficulty)
					{
						npcNeigong.bUse = true;
						this.m_UseNeigongCount = num2;
					}
					else
					{
						npcNeigong.bUse = false;
						Game.UI.Get<UIMapMessage>().SetMsg("当前难度下最多选择" + GameGlobal.mod_Difficulty.ToString() + "个内功！");
					}
				}
				else
				{
					num2--;
					if (num2 < 1)
					{
						npcNeigong.bUse = true;
						Game.UI.Get<UIMapMessage>().SetMsg("至少选择一种内功");
					}
					else
					{
						npcNeigong.bUse = false;
						this.m_UseNeigongCount = num2;
					}
				}
				for (int j = 0; j < this.m_CurCaraData.NeigongList.Count; j++)
				{
					if (this.m_UseNeigongCount <= GameGlobal.mod_Difficulty && this.m_UICharacter != null)
					{
						this.m_UICharacter.SetNeigongUse(j, this.m_CurCaraData.NeigongList[j].bUse);
					}
				}
			}
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00086A88 File Offset: 0x00084C88
		public void SetPractice(object practice)
		{
			NpcNeigong npcNeigong = practice as NpcNeigong;
			NpcRoutine npcRoutine = practice as NpcRoutine;
			List<LevelUp> list = new List<LevelUp>();
			string title;
			string exp;
			float num;
			if (npcNeigong != null)
			{
				title = npcNeigong.m_Neigong.m_strNeigongName + "   " + npcNeigong.iLevel.ToString();
				exp = npcNeigong.iCurExp.ToString() + "/" + npcNeigong.iNextExp.ToString();
				num = (float)npcNeigong.iCurExp;
				num /= (float)npcNeigong.iNextExp;
				num = Mathf.Clamp01(num);
				if (npcNeigong.iLevel >= 10)
				{
					exp = "0/0";
					num = 1f;
				}
				else
				{
					list.AddRange(npcNeigong.m_Neigong.m_LevelUP);
					if (npcNeigong.iLevel == 9)
					{
						list.AddRange(npcNeigong.m_Neigong.m_MaxLevelUP);
					}
				}
				NPC.m_instance.SetNowPractice(this.m_CurNpcID, npcNeigong.m_Neigong.m_iNeigongID);
			}
			else
			{
				title = npcRoutine.m_Routine.m_strRoutineName + "   " + npcRoutine.iLevel.ToString();
				exp = npcRoutine.iCurExp.ToString() + "/" + npcRoutine.iNextExp.ToString();
				num = (float)npcRoutine.iCurExp;
				num /= (float)npcRoutine.iNextExp;
				num = Mathf.Clamp01(num);
				if (npcRoutine.iLevel >= 10)
				{
					exp = "0/0";
					num = 1f;
				}
				else
				{
					list.AddRange(npcRoutine.m_Routine.m_iLevelUP);
				}
				NPC.m_instance.SetNowPractice(this.m_CurNpcID, npcRoutine.m_Routine.m_iRoutineID);
			}
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.SetPractice(title, exp, num, list);
			}
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00086C50 File Offset: 0x00084E50
		public void SetCurrent(GameObject obj)
		{
			UIEventListener component = obj.GetComponent<UIEventListener>();
			if (component != null && this.OnSetCurrent != null)
			{
				this.OnSetCurrent.Invoke(component);
			}
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0000A826 File Offset: 0x00008A26
		public void UpdatePracticeTwoFormData()
		{
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.UpdatePracticeTwoFormView(this.m_CurCaraData.RoutineList, this.m_CurCaraData.NeigongList);
			}
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00086C88 File Offset: 0x00084E88
		public void UpdateBackpackTwoFormData(int iType)
		{
			BackpackStatus.m_Instance.SortItem(iType);
			List<BackpackNewDataNode> sortTeamBackpack = BackpackStatus.m_Instance.GetSortTeamBackpack();
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.UpdateBackpackTwoFormData(this.m_CurCaraData, sortTeamBackpack, iType);
			}
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x00086CD0 File Offset: 0x00084ED0
		public void UseEqupip(GameObject go)
		{
			BackpackNewDataNode backpackNode = go.GetComponent<BackpackDataNode>().m_BackpackNode;
			BackpackStatus.m_Instance.mod_CharacterData = this.m_CurCaraData;
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

		// Token: 0x06000FDA RID: 4058 RVA: 0x00086D54 File Offset: 0x00084F54
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

		// Token: 0x06000FDB RID: 4059 RVA: 0x00086E10 File Offset: 0x00085010
		public void ChangeTeamMember(GameObject go)
		{
			List<CharacterData> teamMemberList = TeamStatus.m_Instance.GetTeamMemberList();
			int num = 0;
			for (int i = 0; i < teamMemberList.Count; i++)
			{
				if (teamMemberList[i]._NpcDataNode.m_iNpcID == this.m_CurNpcID)
				{
					num = i;
					break;
				}
			}
			if (go.name == "IconChangeTeamMemberL")
			{
				num--;
			}
			else
			{
				num++;
			}
			if (num < 0)
			{
				num = teamMemberList.Count - 1;
			}
			if (num >= teamMemberList.Count)
			{
				num = 0;
			}
			this.SetCurNpcID(teamMemberList[num]._NpcDataNode.m_iNpcID, true);
			Game.mod_CurrentCharacterData = teamMemberList[num];
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00086EB0 File Offset: 0x000850B0
		public void RemoveTeammate(bool bRemove)
		{
			if (bRemove)
			{
				TeamStatus.m_Instance.LessTeamMember(this.m_CurNpcID);
				List<CharacterData> teamMemberList = TeamStatus.m_Instance.GetTeamMemberList();
				this.SetCurNpcID(Enumerable.First<CharacterData>(teamMemberList)._NpcDataNode.m_iNpcID, true);
			}
			if (this.OnEnterState != null)
			{
				this.OnEnterState.Invoke(1);
			}
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0000A85A File Offset: 0x00008A5A
		public void SetWgUpgradeStatus()
		{
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.SetWgUpgradeStatus(this.m_CurCaraData);
			}
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00086F0C File Offset: 0x0008510C
		public void SetCharacterProperty(bool set, int[] ivalues, int[] ivaluecurrents)
		{
			if (set)
			{
				int attributePoints = TeamStatus.m_Instance.GetAttributePoints();
				TeamStatus.m_Instance.ChangeAttributePoints(this.statusPoints - attributePoints);
				int num = 11;
				for (int i = 0; i < ivalues.Length; i++)
				{
					NPC.m_instance.SetCharacterProperty(this.m_CurNpcID, num, ivalues[i] - ivaluecurrents[i]);
					num++;
				}
			}
			if (this.OnEnterState != null)
			{
				this.OnEnterState.Invoke(1);
			}
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00086F88 File Offset: 0x00085188
		public void UpdateUseItemTwoFormData(ItemDataNode.ItemType Type)
		{
			BackpackStatus.m_Instance.SortItem((int)Type);
			List<BackpackNewDataNode> sortTeamBackpack = BackpackStatus.m_Instance.GetSortTeamBackpack();
			if (this.m_UICharacter != null)
			{
				this.m_UICharacter.UpdateUseItemTwoFormData(this.m_CurCaraData, sortTeamBackpack, Type);
			}
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00086FD0 File Offset: 0x000851D0
		public void UseMedicine(GameObject go)
		{
			BackpackNewDataNode backpackNode = go.GetComponent<BackpackDataNode>().m_BackpackNode;
			if (backpackNode != null)
			{
				BackpackStatus.m_Instance.UseItem(this.m_CurCaraData, backpackNode);
			}
			this.UpdateUseItemTwoFormData(ItemDataNode.ItemType.Solution);
			this.UpdateCaracterData();
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00087010 File Offset: 0x00085210
		public void UseSecretBook(GameObject go)
		{
			BackpackNewDataNode backpackNode = go.GetComponent<BackpackDataNode>().m_BackpackNode;
			if (backpackNode == null)
			{
				return;
			}
			ItemDataNode itemDataNode = backpackNode._ItemDataNode;
			List<string> list = new List<string>();
			for (int i = 0; i < itemDataNode.m_UseLimitNodeList.Count; i++)
			{
				UseLimitNode useLimitNode = itemDataNode.m_UseLimitNodeList[i];
				string text = string.Empty;
				UseLimitType type = useLimitNode.m_Type;
				if (type != UseLimitType.MoreNpcProperty)
				{
					if (type == UseLimitType.LessNpcProperty)
					{
						text = this.GreaterProperty(this.m_CurCaraData, useLimitNode, false);
					}
				}
				else
				{
					text = this.GreaterProperty(this.m_CurCaraData, useLimitNode, true);
				}
				if (text.Length != 0)
				{
					list.Add(text);
				}
			}
			if (list.Count == 0)
			{
				BackpackStatus.m_Instance.UseItem(this.m_CurCaraData, backpackNode);
				if (backpackNode == null)
				{
					return;
				}
				this.UpdateUseItemTwoFormData(ItemDataNode.ItemType.TipsBook);
				this.UpdateCaracterData();
			}
			else
			{
				for (int j = 0; j < list.Count; j++)
				{
					Game.UI.Get<UIMapMessage>().SetMsg(list[j]);
				}
			}
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00087134 File Offset: 0x00085334
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

		// Token: 0x040012DE RID: 4830
		public UICharacter m_UICharacter;

		// Token: 0x040012DF RID: 4831
		public UIDLCCharacter m_UIDLCCharacter;

		// Token: 0x040012E0 RID: 4832
		private int m_CurNpcID;

		// Token: 0x040012E1 RID: 4833
		public Action<int> OnEnterState;

		// Token: 0x040012E2 RID: 4834
		private List<BackpackNewDataNode> m_SortTeamBackpackList;

		// Token: 0x040012E3 RID: 4835
		public CharacterData.PropertyType[] PropertyArray = new CharacterData.PropertyType[]
		{
			CharacterData.PropertyType.MaxHP,
			CharacterData.PropertyType.MaxSP,
			CharacterData.PropertyType.Strength,
			CharacterData.PropertyType.Constitution,
			CharacterData.PropertyType.Intelligence,
			CharacterData.PropertyType.Dexterity,
			CharacterData.PropertyType.Dodge,
			CharacterData.PropertyType.Counter,
			CharacterData.PropertyType.Critical,
			CharacterData.PropertyType.MoveStep
		};

		// Token: 0x040012E4 RID: 4836
		public int m_AtkTypeShowMax = 3;

		// Token: 0x040012E5 RID: 4837
		public int m_iOnceSkillMax = 12;

		// Token: 0x040012E6 RID: 4838
		public int m_UseMartialCount;

		// Token: 0x040012E7 RID: 4839
		public int m_UseMartialMax = 6;

		// Token: 0x040012E8 RID: 4840
		private int statusPoints;

		// Token: 0x040012E9 RID: 4841
		public Action<UIEventListener> OnSetCurrent;

		// Token: 0x040012EC RID: 4844
		public int m_UseNeigongCount;
	}
}
