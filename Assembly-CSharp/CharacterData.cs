using System;
using System.Collections.Generic;
using Heluo.Wulin;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x020002BA RID: 698
public class CharacterData : CharacterDataBase
{
	// Token: 0x06000D76 RID: 3446 RVA: 0x0006D1D4 File Offset: 0x0006B3D4
	public CharacterData()
	{
		this._EquipWeapon = new EquipProperty();
		this._EquipArror = new EquipProperty();
		this._EquipNecklace = new EquipProperty();
		this._TotalProperty = new EquipProperty();
		this.FinishQuestList = new List<string>();
		this.passiveNodeList = new List<int>();
		this.iLevel = 1;
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x0006D250 File Offset: 0x0006B450
	public override CharacterData Clone()
	{
		CharacterData characterData = new CharacterData();
		characterData._NpcDataNode = null;
		characterData.iNpcID = this.iNpcID;
		characterData.NpcType = this.NpcType;
		foreach (WeaponType weaponType in this.WeaponTypeList)
		{
			characterData.WeaponTypeList.Add(weaponType);
		}
		characterData.iMoney = this.iMoney;
		characterData.iMaxHp = this.iMaxHp;
		characterData.iMaxSp = this.iMaxSp;
		characterData.iMoveStep = this.iMoveStep;
		characterData.iStr = this.iStr;
		characterData.iInt = this.iInt;
		characterData.iCon = this.iCon;
		characterData.iDex = this.iDex;
		characterData.iMaxStr = this.iMaxStr;
		characterData.iMaxInt = this.iMaxInt;
		characterData.iMaxCon = this.iMaxCon;
		characterData.iMaxDex = this.iMaxDex;
		characterData.iCri = this.iCri;
		characterData.iCounter = this.iCounter;
		characterData.iDodge = this.iDodge;
		characterData.iCombo = this.iCombo;
		characterData.iDefendCri = this.iDefendCri;
		characterData.iDefendCombo = this.iDefendCombo;
		characterData.iDefendCounter = this.iDefendCounter;
		characterData.iDefendDodge = this.iDefendDodge;
		characterData._EquipArror = null;
		characterData._EquipWeapon = null;
		characterData._EquipNecklace = null;
		characterData._TotalProperty = null;
		characterData.iEquipWeaponID = this.iEquipWeaponID;
		characterData.iEquipArrorID = this.iEquipArrorID;
		characterData.iEquipNecklaceID = this.iEquipNecklaceID;
		characterData.NowPractice = this.NowPractice;
		characterData.TalentList = this.TalentList;
		foreach (NpcRoutine npcRoutine in this.RoutineList)
		{
			if (npcRoutine.iSkillID != 0)
			{
				characterData.RoutineList.Add(npcRoutine.Clone());
			}
		}
		foreach (NpcNeigong npcNeigong in this.NeigongList)
		{
			if (npcNeigong.iSkillID != 0)
			{
				characterData.NeigongList.Add(npcNeigong.Clone());
			}
		}
		foreach (NpcItem npcItem in this.Itemlist)
		{
			characterData.Itemlist.Add(npcItem.Clone());
		}
		characterData._MartialArts = this._MartialArts;
		characterData._MartialDef = this._MartialDef;
		characterData.strNowDoQuest = this.strNowDoQuest;
		characterData.FinishQuestList = this.FinishQuestList;
		characterData.iFinishTime = this.iFinishTime;
		characterData.iLevel = this.iLevel;
		characterData.iCurTotalExp = this.iCurTotalExp;
		characterData.iSkillPoint = this.iSkillPoint;
		characterData.iHurtTurn = this.iHurtTurn;
		characterData.passiveNodeList.AddRange(this.passiveNodeList.ToArray());
		characterData.iChangeNPCID = this.iChangeNPCID;
		characterData.mod_WeaponGuid = this.mod_WeaponGuid;
		characterData.mod_ArmorGuid = this.mod_ArmorGuid;
		characterData.mod_NecklaceGuid = this.mod_NecklaceGuid;
		return characterData;
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x00009710 File Offset: 0x00007910
	public EquipProperty GetTotalProperty()
	{
		this.setTotalProperty();
		return this._TotalProperty;
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x0006D5C8 File Offset: 0x0006B7C8
	public NpcNeigong GetNowUseNeigong()
	{
		for (int i = 0; i < this.NeigongList.Count; i++)
		{
			if (this.NeigongList[i].bUse)
			{
				return this.NeigongList[i];
			}
		}
		return null;
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x0006D618 File Offset: 0x0006B818
	private NpcNeigong GetNowPracticeNeigong()
	{
		for (int i = 0; i < this.NeigongList.Count; i++)
		{
			if (this.NeigongList[i].m_Neigong.m_iNeigongID == this.NowPractice)
			{
				return this.NeigongList[i];
			}
		}
		return null;
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x0006D670 File Offset: 0x0006B870
	private NpcRoutine GetNowPracticeRoution()
	{
		for (int i = 0; i < this.RoutineList.Count; i++)
		{
			if (this.RoutineList[i].m_Routine.m_iRoutineID == this.NowPractice)
			{
				return this.RoutineList[i];
			}
		}
		return null;
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x0006D6C8 File Offset: 0x0006B8C8
	public NpcSkill GetNowPractice()
	{
		for (int i = 0; i < this.NeigongList.Count; i++)
		{
			if (this.NeigongList[i].m_Neigong.m_iNeigongID == this.NowPractice)
			{
				return this.NeigongList[i];
			}
		}
		for (int j = 0; j < this.RoutineList.Count; j++)
		{
			if (this.RoutineList[j].m_Routine.m_iRoutineID == this.NowPractice)
			{
				return this.RoutineList[j];
			}
		}
		return null;
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x0006D76C File Offset: 0x0006B96C
	public float GetTalentAddExp()
	{
		float num = 1f;
		NpcSkill nowPractice = this.GetNowPractice();
		if (nowPractice == null)
		{
			return num;
		}
		if (nowPractice.m_SkillType == NpcSkill.SkillType.Neigong)
		{
			num += this.GetNpcTalentPercentValue(TalentEffect.Negiong);
		}
		else if (nowPractice.m_SkillType == NpcSkill.SkillType.Routine)
		{
			NpcRoutine routineData = this.GetRoutineData(nowPractice.iSkillID);
			WeaponType routineType = routineData.m_Routine.m_RoutineType;
			TalentEffect iType = (TalentEffect)(routineType + 20);
			num += this.GetNpcTalentPercentValue(iType);
		}
		return num * (1f + this.GetNpcTalentPercentValue(TalentEffect.ALLMartialExp));
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x0006D7F4 File Offset: 0x0006B9F4
	public string GetNowPracticeName()
	{
		NpcNeigong nowPracticeNeigong = this.GetNowPracticeNeigong();
		if (nowPracticeNeigong != null)
		{
			return nowPracticeNeigong.m_Neigong.m_strNeigongName;
		}
		NpcRoutine nowPracticeRoution = this.GetNowPracticeRoution();
		if (nowPracticeRoution != null)
		{
			return nowPracticeRoution.m_Routine.m_strRoutineName;
		}
		if (this.NeigongList.Count > 0)
		{
			this.NowPractice = this.NeigongList[0].m_Neigong.m_iNeigongID;
			return this.NeigongList[0].m_Neigong.m_strNeigongName;
		}
		if (this.RoutineList.Count > 0)
		{
			this.NowPractice = this.RoutineList[0].m_Routine.m_iRoutineID;
			return this.RoutineList[0].m_Routine.m_strRoutineName;
		}
		return string.Empty;
	}

	// Token: 0x06000D7F RID: 3455 RVA: 0x0006D8C0 File Offset: 0x0006BAC0
	public int GetNowPracticeLevel()
	{
		NpcNeigong nowPracticeNeigong = this.GetNowPracticeNeigong();
		if (nowPracticeNeigong != null)
		{
			return nowPracticeNeigong.iLevel;
		}
		NpcRoutine nowPracticeRoution = this.GetNowPracticeRoution();
		if (nowPracticeRoution != null)
		{
			return nowPracticeRoution.iLevel;
		}
		if (this.NeigongList.Count > 0)
		{
			this.NowPractice = this.NeigongList[0].m_Neigong.m_iNeigongID;
			return this.NeigongList[0].iLevel;
		}
		if (this.RoutineList.Count > 0)
		{
			this.NowPractice = this.RoutineList[0].m_Routine.m_iRoutineID;
			return this.RoutineList[0].iLevel;
		}
		return 0;
	}

	// Token: 0x06000D80 RID: 3456 RVA: 0x0006D974 File Offset: 0x0006BB74
	public int GetNowPracticeCurExp()
	{
		NpcNeigong nowPracticeNeigong = this.GetNowPracticeNeigong();
		if (nowPracticeNeigong != null)
		{
			return nowPracticeNeigong.iCurExp;
		}
		NpcRoutine nowPracticeRoution = this.GetNowPracticeRoution();
		if (nowPracticeRoution != null)
		{
			return nowPracticeRoution.iCurExp;
		}
		if (this.NeigongList.Count > 0)
		{
			this.NowPractice = this.NeigongList[0].m_Neigong.m_iNeigongID;
			return this.NeigongList[0].iCurExp;
		}
		if (this.RoutineList.Count > 0)
		{
			this.NowPractice = this.RoutineList[0].m_Routine.m_iRoutineID;
			return this.RoutineList[0].iCurExp;
		}
		return 0;
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x0006DA28 File Offset: 0x0006BC28
	public int GetNowPracticeNextExp()
	{
		NpcNeigong nowPracticeNeigong = this.GetNowPracticeNeigong();
		if (nowPracticeNeigong != null)
		{
			return nowPracticeNeigong.iNextExp;
		}
		NpcRoutine nowPracticeRoution = this.GetNowPracticeRoution();
		if (nowPracticeRoution != null)
		{
			return nowPracticeRoution.iNextExp;
		}
		if (this.NeigongList.Count > 0)
		{
			this.NowPractice = this.NeigongList[0].m_Neigong.m_iNeigongID;
			return this.NeigongList[0].iNextExp;
		}
		if (this.RoutineList.Count > 0)
		{
			this.NowPractice = this.RoutineList[0].m_Routine.m_iRoutineID;
			return this.RoutineList[0].iNextExp;
		}
		return 0;
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x0006DADC File Offset: 0x0006BCDC
	public int SetNowPracticeExp(int iValue)
	{
		NpcNeigong nowPracticeNeigong = this.GetNowPracticeNeigong();
		if (nowPracticeNeigong != null)
		{
			return nowPracticeNeigong.SetExp(iValue);
		}
		NpcRoutine nowPracticeRoution = this.GetNowPracticeRoution();
		if (nowPracticeRoution != null)
		{
			return nowPracticeRoution.SetExp(iValue);
		}
		return 0;
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x0006DB14 File Offset: 0x0006BD14
	public NpcNeigong SetNowUseNeigong(int iNeigongID)
	{
		NpcNeigong result = null;
		for (int i = 0; i < this.NeigongList.Count; i++)
		{
			if (this.NeigongList[i].iSkillID == iNeigongID)
			{
				this.NeigongList[i].bUse = true;
				result = this.NeigongList[i];
			}
			else
			{
				this.NeigongList[i].bUse = false;
			}
		}
		return result;
	}

	// Token: 0x06000D84 RID: 3460 RVA: 0x0006DB90 File Offset: 0x0006BD90
	public string GetNowPracticeLvupText(int lv)
	{
		NpcNeigong nowPracticeNeigong = this.GetNowPracticeNeigong();
		if (nowPracticeNeigong != null)
		{
			return nowPracticeNeigong.GetLvUpString(lv);
		}
		NpcRoutine nowPracticeRoution = this.GetNowPracticeRoution();
		if (nowPracticeRoution != null)
		{
			return nowPracticeRoution.GetLvUpString(lv);
		}
		return string.Empty;
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x0006DBCC File Offset: 0x0006BDCC
	public void GetNextPractice()
	{
		int num = 10;
		if (this.GetNowPracticeLevel() < num)
		{
			return;
		}
		CharacterData.ePracticeType ePracticeType = (CharacterData.ePracticeType)(Random.Range(1, 10) % 2);
		if (ePracticeType == CharacterData.ePracticeType.Routine)
		{
			foreach (NpcRoutine npcRoutine in this.RoutineList)
			{
				if (npcRoutine.iLevel < 10)
				{
					this.NowPractice = npcRoutine.m_Routine.m_iRoutineID;
					return;
				}
			}
			ePracticeType = CharacterData.ePracticeType.Neigong;
		}
		if (ePracticeType == CharacterData.ePracticeType.Neigong)
		{
			foreach (NpcNeigong npcNeigong in this.NeigongList)
			{
				if (npcNeigong.iLevel < 10)
				{
					this.NowPractice = npcNeigong.m_Neigong.m_iNeigongID;
					return;
				}
			}
		}
		if (this.RoutineList.Count > 0)
		{
			this.NowPractice = this.RoutineList[0].m_Routine.m_iRoutineID;
			return;
		}
		if (this.NeigongList.Count > 0)
		{
			this.NowPractice = this.NeigongList[0].m_Neigong.m_iNeigongID;
			return;
		}
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0006DD34 File Offset: 0x0006BF34
	public bool CheckRoutine(int iRoutineID)
	{
		RoutineNewDataNode routineNewData = Game.RoutineNewData.GetRoutineNewData(iRoutineID);
		if (routineNewData == null)
		{
			Debug.Log(" 請檢察 Routine表 " + iRoutineID);
			return false;
		}
		int num = 12;
		for (int i = 0; i < this.RoutineList.Count; i++)
		{
			if (this.RoutineList[i].m_Routine.m_iRoutineID == routineNewData.m_iRoutineID)
			{
				Debug.Log(this.iNpcID + "   套路已存在");
				return false;
			}
		}
		if (this.RoutineList.Count >= num)
		{
			Debug.LogError(string.Concat(new object[]
			{
				this.iNpcID,
				"  套路數量不得超過 ",
				num,
				" 個"
			}));
			return false;
		}
		return true;
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x0006DE14 File Offset: 0x0006C014
	public bool CheckTalent(int TalentID)
	{
		if (Game.TalentNewData.GetTalentData(TalentID) == null)
		{
			Debug.LogError("請檢察天賦表  " + TalentID);
			return false;
		}
		int num = 5;
		for (int i = 0; i < this.TalentList.Count; i++)
		{
			if (this.TalentList[i] == TalentID)
			{
				Debug.LogError(this.iNpcID + " 有相同ID");
				return false;
			}
		}
		if (this.TalentList.Count >= num)
		{
			Debug.LogError(string.Concat(new object[]
			{
				this.iNpcID,
				" 天賦已超過",
				num,
				"個"
			}));
			return false;
		}
		if (Game.TalentNewData.GetTalentData(TalentID) == null)
		{
			Debug.LogError("請檢察天賦表");
			return false;
		}
		return true;
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x0006DF00 File Offset: 0x0006C100
	public bool CheckNeigong(int iNeigongID)
	{
		NeigongNewDataNode neigongData = Game.NeigongData.GetNeigongData(iNeigongID);
		int num = 12;
		if (neigongData == null)
		{
			Debug.Log("請檢察 NeigongData表:  " + iNeigongID);
			return false;
		}
		for (int i = 0; i < this.NeigongList.Count; i++)
		{
			if (this.NeigongList[i].m_Neigong.m_iNeigongID == neigongData.m_iNeigongID)
			{
				Debug.LogError("內功已存在");
				return false;
			}
		}
		if (this.NeigongList.Count >= num)
		{
			Debug.LogError("內功數量不得超過 " + num + " 個");
			return false;
		}
		return true;
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x0006DFA4 File Offset: 0x0006C1A4
	public bool CheckWeaponType(WeaponType WT)
	{
		bool result = false;
		if (this.WeaponTypeList.Contains(WT))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06000D8A RID: 3466 RVA: 0x0000971E File Offset: 0x0000791E
	public void AddMoney(int iValue)
	{
		this.iMoney += iValue;
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x0000972E File Offset: 0x0000792E
	public void LessMoney(int iValue)
	{
		this.iMoney -= iValue;
		if (this.iMoney < 0)
		{
			this.iMoney = 0;
		}
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x0006DFC8 File Offset: 0x0006C1C8
	public int GetNpcItemCount()
	{
		int num = 0;
		for (int i = 0; i < this.Itemlist.Count; i++)
		{
			num += this.Itemlist[i].m_iCount;
		}
		return num;
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x0006E008 File Offset: 0x0006C208
	public int GetNpcItemIndexID(int index)
	{
		int num = 0;
		for (int i = 0; i < this.Itemlist.Count; i++)
		{
			num += this.Itemlist[i].m_iCount;
			if (index < num)
			{
				return this.Itemlist[i].m_iItemID;
			}
		}
		return 0;
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x0006E064 File Offset: 0x0006C264
	public void LessNpcItem(int ItemID, int iValue)
	{
		if (ItemID == 0)
		{
			return;
		}
		int i = 0;
		while (i < this.Itemlist.Count)
		{
			if (this.Itemlist[i].m_iItemID == ItemID)
			{
				if (this.Itemlist[i].m_iCount > iValue)
				{
					this.Itemlist[i].m_iCount -= iValue;
					break;
				}
				if (this.Itemlist[i].m_iCount == iValue)
				{
					this.Itemlist.RemoveAt(i);
					break;
				}
				this.Itemlist.RemoveAt(i);
				iValue -= this.Itemlist[i].m_iCount;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x0006E130 File Offset: 0x0006C330
	public void AddNpcItem(int ItemID, int iValue)
	{
		if (ItemID == 0)
		{
			return;
		}
		if (Game.ItemData.GetItemDataNode(ItemID) == null)
		{
			Debug.Log("請檢察ItemData表 " + ItemID);
			return;
		}
		bool flag = true;
		if (this.Itemlist.Count > 0)
		{
			foreach (NpcItem npcItem in this.Itemlist)
			{
				if (npcItem.m_iItemID == ItemID)
				{
					npcItem.m_iCount++;
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			NpcItem npcItem2 = new NpcItem();
			npcItem2.m_iItemID = ItemID;
			npcItem2.m_iCount = iValue;
			this.Itemlist.Add(npcItem2);
		}
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x0006E210 File Offset: 0x0006C410
	public int GetValue(CharacterData.PropertyType type)
	{
		switch (type)
		{
		case CharacterData.PropertyType.CurHP:
			return this.iCurHp;
		case CharacterData.PropertyType.MaxHP:
			return this.iMaxHp;
		case CharacterData.PropertyType.CurSP:
			return this.iCurSp;
		case CharacterData.PropertyType.MaxSP:
			return this.iMaxSp;
		case CharacterData.PropertyType.MoveStep:
			return this.iMoveStep;
		default:
			if (type != CharacterData.PropertyType.Attack)
			{
				return 0;
			}
			return this.iAttack;
		case CharacterData.PropertyType.Strength:
			return this.iStr;
		case CharacterData.PropertyType.Constitution:
			return this.iCon;
		case CharacterData.PropertyType.Intelligence:
			return this.iInt;
		case CharacterData.PropertyType.Dexterity:
			return this.iDex;
		case CharacterData.PropertyType.StrengthMax:
			return this.iMaxStr;
		case CharacterData.PropertyType.ConstitutionMax:
			return this.iMaxCon;
		case CharacterData.PropertyType.IntelligenceMax:
			return this.iMaxInt;
		case CharacterData.PropertyType.DexterityMax:
			return this.iMaxDex;
		case CharacterData.PropertyType.UseSword:
		case CharacterData.PropertyType.UseBlade:
		case CharacterData.PropertyType.UseArrow:
		case CharacterData.PropertyType.UseFist:
		case CharacterData.PropertyType.UseGas:
		case CharacterData.PropertyType.UseRope:
		case CharacterData.PropertyType.UseWhip:
		case CharacterData.PropertyType.UsePike:
		case CharacterData.PropertyType.UseSwordMax:
		case CharacterData.PropertyType.UseBladeMax:
		case CharacterData.PropertyType.UseArrowMax:
		case CharacterData.PropertyType.UseFistMax:
		case CharacterData.PropertyType.UseGasMax:
		case CharacterData.PropertyType.UseRopeMax:
		case CharacterData.PropertyType.UseWhipMax:
		case CharacterData.PropertyType.UsePikeMax:
			return this._MartialArts.Get(type);
		case CharacterData.PropertyType.DefSword:
		case CharacterData.PropertyType.DefBlade:
		case CharacterData.PropertyType.DefArrow:
		case CharacterData.PropertyType.DefFist:
		case CharacterData.PropertyType.DefGas:
		case CharacterData.PropertyType.DefRope:
		case CharacterData.PropertyType.DefWhip:
		case CharacterData.PropertyType.DefPike:
			return this._MartialDef.Get(type);
		case CharacterData.PropertyType.Dodge:
			return this.iDodge;
		case CharacterData.PropertyType.Counter:
			return this.iCounter;
		case CharacterData.PropertyType.Critical:
			return this.iCri;
		case CharacterData.PropertyType.Combo:
			return this.iCombo;
		case CharacterData.PropertyType.DefendDodge:
			return this.iDefendDodge;
		case CharacterData.PropertyType.DefendCounter:
			return this.iDefendCounter;
		case CharacterData.PropertyType.DefendCritical:
			return this.iDefendCri;
		case CharacterData.PropertyType.DefendCombo:
			return this.iDefendCombo;
		}
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x0006E3D4 File Offset: 0x0006C5D4
	public void SetValue(CharacterData.PropertyType type, int iValue)
	{
		switch (type)
		{
		case CharacterData.PropertyType.CurHP:
			this.iCurHp += iValue;
			this.iCurHp = Mathf.Clamp(this.iCurHp, 0, this._TotalProperty.Get(CharacterData.PropertyType.MaxHP));
			break;
		case CharacterData.PropertyType.MaxHP:
			this.iMaxHp += iValue;
			this.iMaxHp = Mathf.Clamp(this.iMaxHp, 0, 99999);
			break;
		case CharacterData.PropertyType.CurSP:
			this.iCurSp += iValue;
			this.iCurSp = Mathf.Clamp(this.iCurSp, 0, this._TotalProperty.Get(CharacterData.PropertyType.MaxSP));
			break;
		case CharacterData.PropertyType.MaxSP:
			this.iMaxSp += iValue;
			this.iMaxSp = Mathf.Clamp(this.iMaxSp, 0, 99999);
			break;
		case CharacterData.PropertyType.MoveStep:
			this.iMoveStep += iValue;
			this.iMoveStep = Mathf.Clamp(this.iMoveStep, 0, 100);
			break;
		default:
			if (type == CharacterData.PropertyType.Attack)
			{
				this.iAttack += iValue;
				this.iAttack = Mathf.Clamp(this.iAttack, 0, 99999);
			}
			break;
		case CharacterData.PropertyType.Strength:
			this.iStr += iValue;
			this.iStr = Mathf.Clamp(this.iStr, 0, this._TotalProperty.Get(CharacterData.PropertyType.StrengthMax));
			break;
		case CharacterData.PropertyType.Constitution:
			this.iCon += iValue;
			this.iCon = Mathf.Clamp(this.iCon, 0, this._TotalProperty.Get(CharacterData.PropertyType.ConstitutionMax));
			break;
		case CharacterData.PropertyType.Intelligence:
			this.iInt += iValue;
			this.iInt = Mathf.Clamp(this.iInt, 0, this._TotalProperty.Get(CharacterData.PropertyType.IntelligenceMax));
			break;
		case CharacterData.PropertyType.Dexterity:
			this.iDex += iValue;
			this.iDex = Mathf.Clamp(this.iDex, 0, this._TotalProperty.Get(CharacterData.PropertyType.DexterityMax));
			break;
		case CharacterData.PropertyType.StrengthMax:
			this.iMaxStr += iValue;
			this.iMaxStr = Mathf.Clamp(this.iMaxStr, 0, 999);
			break;
		case CharacterData.PropertyType.ConstitutionMax:
			this.iMaxCon += iValue;
			this.iMaxCon = Mathf.Clamp(this.iMaxCon, 0, 999);
			break;
		case CharacterData.PropertyType.IntelligenceMax:
			this.iMaxInt += iValue;
			this.iMaxInt = Mathf.Clamp(this.iMaxInt, 0, 999);
			break;
		case CharacterData.PropertyType.DexterityMax:
			this.iMaxDex += iValue;
			this.iMaxDex = Mathf.Clamp(this.iMaxDex, 0, 999);
			break;
		case CharacterData.PropertyType.UseSword:
		case CharacterData.PropertyType.UseBlade:
		case CharacterData.PropertyType.UseArrow:
		case CharacterData.PropertyType.UseFist:
		case CharacterData.PropertyType.UseGas:
		case CharacterData.PropertyType.UseRope:
		case CharacterData.PropertyType.UseWhip:
		case CharacterData.PropertyType.UsePike:
			this._MartialArts.Set(type, iValue);
			this._MartialArts.Set(type, Mathf.Clamp(this._MartialArts.Get(type), 0, this._TotalProperty._MartialArts.Get(type + 10)));
			break;
		case CharacterData.PropertyType.UseSwordMax:
		case CharacterData.PropertyType.UseBladeMax:
		case CharacterData.PropertyType.UseArrowMax:
		case CharacterData.PropertyType.UseFistMax:
		case CharacterData.PropertyType.UseGasMax:
		case CharacterData.PropertyType.UseRopeMax:
		case CharacterData.PropertyType.UseWhipMax:
		case CharacterData.PropertyType.UsePikeMax:
			this._MartialArts.Set(type, iValue);
			break;
		case CharacterData.PropertyType.DefSword:
		case CharacterData.PropertyType.DefBlade:
		case CharacterData.PropertyType.DefArrow:
		case CharacterData.PropertyType.DefFist:
		case CharacterData.PropertyType.DefGas:
		case CharacterData.PropertyType.DefRope:
		case CharacterData.PropertyType.DefWhip:
		case CharacterData.PropertyType.DefPike:
			this._MartialDef.SetPlus(type, iValue);
			break;
		case CharacterData.PropertyType.Dodge:
			this.iDodge += iValue;
			this.iDodge = Mathf.Clamp(this.iDodge, 0, 99);
			break;
		case CharacterData.PropertyType.Counter:
			this.iCounter += iValue;
			this.iCounter = Mathf.Clamp(this.iCounter, 0, 100);
			break;
		case CharacterData.PropertyType.Critical:
			this.iCri += iValue;
			this.iCri = Mathf.Clamp(this.iCri, 0, 100);
			break;
		case CharacterData.PropertyType.Combo:
			this.iCombo += iValue;
			this.iCombo = Mathf.Clamp(this.iCombo, 0, 100);
			break;
		case CharacterData.PropertyType.DefendDodge:
			this.iDefendDodge += iValue;
			this.iDefendDodge = Mathf.Clamp(this.iDefendDodge, 0, 100);
			break;
		case CharacterData.PropertyType.DefendCounter:
			this.iDefendCounter += iValue;
			this.iDefendCounter = Mathf.Clamp(this.iDefendCounter, 0, 100);
			break;
		case CharacterData.PropertyType.DefendCritical:
			this.iDefendCri += iValue;
			this.iDefendCri = Mathf.Clamp(this.iDefendCri, 0, 100);
			break;
		case CharacterData.PropertyType.DefendCombo:
			this.iDefendCombo += iValue;
			this.iDefendCombo = Mathf.Clamp(this.iDefendCombo, 0, 100);
			break;
		}
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x0006E900 File Offset: 0x0006CB00
	public void SetEquip(ItemDataNode.ItemType _ItemType, int iItem)
	{
		switch (_ItemType)
		{
		case ItemDataNode.ItemType.Weapon:
			this.SetEquipWeapon(iItem);
			break;
		case ItemDataNode.ItemType.Arror:
			this.SetEquipArror(iItem);
			break;
		case ItemDataNode.ItemType.Necklace:
			this.SetEquipNecklace(iItem);
			break;
		}
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x0006E94C File Offset: 0x0006CB4C
	private void SetEquipWeapon(int iWeapon)
	{
		this.iEquipWeaponID = iWeapon;
		if (this._EquipWeapon == null)
		{
			this._EquipWeapon = new EquipProperty();
		}
		this._EquipWeapon.Reset();
		this._EquipWeapon = this.SetEquipProperty(iWeapon);
		if (!this.binit)
		{
			this.setTotalProperty();
		}
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x0006E9A0 File Offset: 0x0006CBA0
	private void SetEquipArror(int iArror)
	{
		this.iEquipArrorID = iArror;
		if (this._EquipArror == null)
		{
			this._EquipArror = new EquipProperty();
		}
		this._EquipArror.Reset();
		this._EquipArror = this.SetEquipProperty(this.iEquipArrorID);
		if (!this.binit)
		{
			this.setTotalProperty();
		}
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x0006E9F8 File Offset: 0x0006CBF8
	private void SetEquipNecklace(int iNecklace)
	{
		this.iEquipNecklaceID = iNecklace;
		if (this._EquipNecklace == null)
		{
			this._EquipNecklace = new EquipProperty();
		}
		this._EquipNecklace.Reset();
		this._EquipNecklace = this.SetEquipProperty(this.iEquipNecklaceID);
		if (!this.binit)
		{
			this.setTotalProperty();
		}
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x0006EA50 File Offset: 0x0006CC50
	private EquipProperty SetEquipProperty(int _EquipItemID)
	{
		EquipProperty equipProperty = new EquipProperty();
		ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(_EquipItemID);
		equipProperty.Reset();
		if (itemDataNode == null)
		{
			return equipProperty;
		}
		float num = 1f;
		int num2 = 0;
		if (itemDataNode.m_iItemType == 1)
		{
			if (this.CheckWeaponType(itemDataNode.m_AmsType))
			{
				for (int i = 61; i <= 68; i++)
				{
					if (i - 60 == (int)itemDataNode.m_AmsType)
					{
						num += this.GetNpcTalentPercentValue((TalentEffect)i);
						num2 = this.GetTalentValue((TalentEffect)i);
						break;
					}
				}
			}
			num += this.GetNpcTalentPercentValue(TalentEffect.AllWeapon);
			num2 = this.GetTalentValue(TalentEffect.AllWeapon);
		}
		for (int j = 0; j < itemDataNode.m_ItmeEffectNodeList.Count; j++)
		{
			ItmeEffectNode.ItemEffectType iItemType = (ItmeEffectNode.ItemEffectType)itemDataNode.m_ItmeEffectNodeList[j].m_iItemType;
			if (iItemType == ItmeEffectNode.ItemEffectType.NpcProperty)
			{
				int iValue = itemDataNode.m_ItmeEffectNodeList[j].m_iValue;
				CharacterData.PropertyType iRecoverType = (CharacterData.PropertyType)itemDataNode.m_ItmeEffectNodeList[j].m_iRecoverType;
				int num3;
				if (iRecoverType == CharacterData.PropertyType.Attack)
				{
					num3 = Mathf.FloorToInt((float)iValue * (1f + num) + (float)num2);
				}
				else
				{
					num3 = iValue;
				}
				if (iRecoverType >= CharacterData.PropertyType.UseSword && iRecoverType <= CharacterData.PropertyType.UsePike)
				{
					int num4 = equipProperty._MartialArts.Get(iRecoverType);
					if (!equipProperty._MartialArts.Set(iRecoverType, num4 + num3))
					{
						Debug.Log(string.Concat(new object[]
						{
							" ItemData ",
							_EquipItemID,
							"確認 屬性參數 : ",
							itemDataNode.m_ItmeEffectNodeList[j].m_iRecoverType
						}));
					}
				}
				else if (iRecoverType >= CharacterData.PropertyType.DefSword && iRecoverType <= CharacterData.PropertyType.DefPike)
				{
					int num5 = equipProperty._MartialDef.Get(iRecoverType);
					if (!equipProperty._MartialDef.Set(iRecoverType, num5 + num3))
					{
						Debug.Log(string.Concat(new object[]
						{
							" ItemData ",
							_EquipItemID,
							"確認 屬性參數 : ",
							itemDataNode.m_ItmeEffectNodeList[j].m_iRecoverType
						}));
					}
				}
				else
				{
					int num6 = equipProperty.Get(iRecoverType);
					if (!equipProperty.SetPlus(iRecoverType, num6 + num3))
					{
						Debug.Log(string.Concat(new object[]
						{
							" ItemData ",
							_EquipItemID,
							"確認 屬性參數 : ",
							itemDataNode.m_ItmeEffectNodeList[j].m_iRecoverType
						}));
					}
				}
			}
			if (iItemType == ItmeEffectNode.ItemEffectType.BattleCondition)
			{
				equipProperty.iConditionList.Add(itemDataNode.m_ItmeEffectNodeList[j].m_iRecoverType);
			}
		}
		if (itemDataNode.m_iItemType == 1 && this.mod_WeaponGuid >= 1000 && Game.Variable.mod_EquipDic[this.mod_WeaponGuid] != null)
		{
			foreach (ItmeEffectNode itmeEffectNode in Game.Variable.mod_EquipDic[this.mod_WeaponGuid])
			{
				equipProperty.iConditionList.Add(itmeEffectNode.m_iRecoverType);
			}
		}
		if (itemDataNode.m_iItemType == 2 && this.mod_ArmorGuid >= 1000 && Game.Variable.mod_EquipDic[this.mod_ArmorGuid] != null)
		{
			foreach (ItmeEffectNode itmeEffectNode2 in Game.Variable.mod_EquipDic[this.mod_ArmorGuid])
			{
				equipProperty.iConditionList.Add(itmeEffectNode2.m_iRecoverType);
			}
		}
		if (itemDataNode.m_iItemType == 3 && this.mod_NecklaceGuid >= 1000 && Game.Variable.mod_EquipDic[this.mod_NecklaceGuid] != null)
		{
			foreach (ItmeEffectNode itmeEffectNode3 in Game.Variable.mod_EquipDic[this.mod_NecklaceGuid])
			{
				equipProperty.iConditionList.Add(itmeEffectNode3.m_iRecoverType);
			}
		}
		return equipProperty;
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x0006EE7C File Offset: 0x0006D07C
	private EquipProperty SetRoutineProperty()
	{
		EquipProperty equipProperty = new EquipProperty();
		equipProperty.Reset();
		if (GameGlobal.m_bDLCMode)
		{
			return equipProperty;
		}
		foreach (NpcRoutine npcRoutine in this.RoutineList)
		{
			foreach (LevelUp levelUp in npcRoutine.m_Routine.m_iLevelUP)
			{
				CharacterData.PropertyType type = levelUp.m_Type;
				int num = equipProperty._MartialArts.Get(type);
				equipProperty._MartialArts.Set(type, num + levelUp.m_iValue * npcRoutine.iLevel);
			}
		}
		return equipProperty;
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x0006EF68 File Offset: 0x0006D168
	private EquipProperty SetTalentProperty()
	{
		EquipProperty equipProperty = new EquipProperty();
		equipProperty.Reset();
		foreach (int iID in this.TalentList)
		{
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData == null)
			{
				return equipProperty;
			}
			foreach (TalentResultPart talentResultPart in talentData.m_cEffetPartList)
			{
				if ((talentResultPart.m_TalentEffect >= TalentEffect.StrengthMax && talentResultPart.m_TalentEffect <= TalentEffect.DexterityMax) || (talentResultPart.m_TalentEffect >= TalentEffect.Dodge && talentResultPart.m_TalentEffect <= TalentEffect.DefendCritical) || (talentResultPart.m_TalentEffect >= TalentEffect.MaxHP && talentResultPart.m_TalentEffect <= TalentEffect.MaxSP))
				{
					int talentValue = this.GetTalentValue(talentResultPart.m_TalentEffect);
					CharacterData.PropertyType talentEffect = (CharacterData.PropertyType)talentResultPart.m_TalentEffect;
					equipProperty.SetPlus(talentEffect, talentValue);
				}
				if (talentResultPart.m_TalentEffect >= TalentEffect.BladeMax && talentResultPart.m_TalentEffect <= TalentEffect.PikeMax)
				{
					int talentValue2 = this.GetTalentValue(talentResultPart.m_TalentEffect);
					CharacterData.PropertyType talentEffect2 = (CharacterData.PropertyType)talentResultPart.m_TalentEffect;
					equipProperty._MartialArts.SetPlus(talentEffect2, talentValue2);
				}
				if (talentResultPart.m_TalentEffect == TalentEffect.ALLMartialMax)
				{
					foreach (object obj in Enum.GetValues(typeof(MartialArts.eDataField)))
					{
						if ((int)obj >= 31 && (int)obj <= 38)
						{
							int talentValue3 = this.GetTalentValue(talentResultPart.m_TalentEffect);
							equipProperty._MartialArts.SetPlus((CharacterData.PropertyType)((int)obj), talentValue3);
						}
					}
				}
			}
		}
		return equipProperty;
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x0006F1AC File Offset: 0x0006D3AC
	private EquipProperty SetPassiveProperty()
	{
		EquipProperty equipProperty = new EquipProperty();
		equipProperty.Reset();
		if (!GameGlobal.m_bDLCMode)
		{
			return equipProperty;
		}
		foreach (int iID in this.passiveNodeList)
		{
			LevelUpPassiveNode levelUpPassiveNode = Game.LevelUpPassiveData.GetLevelUpPassiveNode(iID);
			if (levelUpPassiveNode != null)
			{
				foreach (PassiveNode passiveNode in levelUpPassiveNode.m_PassiveNodeList)
				{
					if (passiveNode.pNodeType == PassiveNodeType.Property)
					{
						CharacterData.PropertyType iValue = (CharacterData.PropertyType)passiveNode.iValue1;
						equipProperty.DirectSetPlus(iValue, passiveNode.iValue2);
					}
				}
			}
		}
		return equipProperty;
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x0006F2A0 File Offset: 0x0006D4A0
	private EquipProperty SetNeigongProperty()
	{
		EquipProperty equipProperty = new EquipProperty();
		equipProperty.Reset();
		foreach (NpcNeigong npcNeigong in this.NeigongList)
		{
			npcNeigong.m_Neigong.CheckConditionEffectOpen();
			if (!GameGlobal.m_bDLCMode)
			{
				int num = (npcNeigong.iLevel < 10) ? npcNeigong.iLevel : 9;
				foreach (LevelUp levelUp in npcNeigong.m_Neigong.m_LevelUP)
				{
					CharacterData.PropertyType type = levelUp.m_Type;
					int num2 = equipProperty.Get(type);
					equipProperty.Set(type, num2 + levelUp.m_iValue * num);
				}
				if (npcNeigong.iLevel >= 10)
				{
					foreach (LevelUp levelUp2 in npcNeigong.m_Neigong.m_MaxLevelUP)
					{
						if (levelUp2.m_Type != CharacterData.PropertyType.CurHP && levelUp2.m_Type != CharacterData.PropertyType.CurHP)
						{
							CharacterData.PropertyType type2 = levelUp2.m_Type;
							int num3 = equipProperty.Get(type2);
							if (!equipProperty.Set(type2, num3 + levelUp2.m_iValue))
							{
								Debug.Log("NeigongData 確認 屬性參數  :" + (int)levelUp2.m_Type);
							}
						}
					}
				}
			}
		}
		return equipProperty;
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x0006F484 File Offset: 0x0006D684
	public void setTotalProperty()
	{
		if (this._TotalProperty == null)
		{
			this._TotalProperty = new EquipProperty();
		}
		EquipProperty equipProperty = this.SetNeigongProperty();
		EquipProperty equipProperty2 = this.SetRoutineProperty();
		EquipProperty equipProperty3 = this.SetTalentProperty();
		EquipProperty equipProperty4 = this.SetPassiveProperty();
		foreach (object obj in Enum.GetValues(typeof(EquipProperty.eDataField)))
		{
			if ((int)obj < 11 || (int)obj > 14)
			{
				int num = this.GetValue((CharacterData.PropertyType)((int)obj)) + this._EquipWeapon.Get((CharacterData.PropertyType)((int)obj)) + this._EquipArror.Get((CharacterData.PropertyType)((int)obj)) + this._EquipNecklace.Get((CharacterData.PropertyType)((int)obj)) + equipProperty.Get((CharacterData.PropertyType)((int)obj)) + equipProperty3.Get((CharacterData.PropertyType)((int)obj)) + equipProperty4.Get((CharacterData.PropertyType)((int)obj));
				if ((int)obj >= 51 && (int)obj <= 57)
				{
					num = Mathf.Clamp(num, 0, 100);
				}
				if ((int)obj == 1 || (int)obj == 3)
				{
					float num2 = 1f + this.GetNpcTalentPercentValue((TalentEffect)((int)obj));
					num = Mathf.FloorToInt((float)num * num2);
				}
				this._TotalProperty.Set((CharacterData.PropertyType)((int)obj), num);
			}
		}
		this._TotalProperty.Set(CharacterData.PropertyType.Strength, this.iStr + equipProperty.Get(CharacterData.PropertyType.Strength) + equipProperty4.Get(CharacterData.PropertyType.Strength));
		this._TotalProperty.Set(CharacterData.PropertyType.Constitution, this.iCon + equipProperty.Get(CharacterData.PropertyType.Constitution) + equipProperty4.Get(CharacterData.PropertyType.Constitution));
		this._TotalProperty.Set(CharacterData.PropertyType.Intelligence, this.iInt + equipProperty.Get(CharacterData.PropertyType.Intelligence) + equipProperty4.Get(CharacterData.PropertyType.Intelligence));
		this._TotalProperty.Set(CharacterData.PropertyType.Dexterity, this.iDex + equipProperty.Get(CharacterData.PropertyType.Dexterity) + equipProperty4.Get(CharacterData.PropertyType.Dexterity));
		this._TotalProperty.Set(CharacterData.PropertyType.Strength, Mathf.Clamp(this._TotalProperty.Get(CharacterData.PropertyType.Strength), 0, this._TotalProperty.Get(CharacterData.PropertyType.StrengthMax)));
		this._TotalProperty.Set(CharacterData.PropertyType.Constitution, Mathf.Clamp(this._TotalProperty.Get(CharacterData.PropertyType.Constitution), 0, this._TotalProperty.Get(CharacterData.PropertyType.ConstitutionMax)));
		this._TotalProperty.Set(CharacterData.PropertyType.Intelligence, Mathf.Clamp(this._TotalProperty.Get(CharacterData.PropertyType.Intelligence), 0, this._TotalProperty.Get(CharacterData.PropertyType.IntelligenceMax)));
		this._TotalProperty.Set(CharacterData.PropertyType.Dexterity, Mathf.Clamp(this._TotalProperty.Get(CharacterData.PropertyType.Dexterity), 0, this._TotalProperty.Get(CharacterData.PropertyType.DexterityMax)));
		for (int i = 21; i <= 28; i++)
		{
			int val = this._MartialArts.Get((CharacterData.PropertyType)i) + equipProperty2._MartialArts.Get((CharacterData.PropertyType)i) + equipProperty4._MartialArts.Get((CharacterData.PropertyType)i) + this._EquipWeapon._MartialArts.Get((CharacterData.PropertyType)i) + this._EquipNecklace._MartialArts.Get((CharacterData.PropertyType)i) + this._EquipArror._MartialArts.Get((CharacterData.PropertyType)i);
			this._TotalProperty._MartialArts.Set((CharacterData.PropertyType)i, val);
		}
		for (int j = 41; j <= 48; j++)
		{
			int val2 = this._MartialDef.Get((CharacterData.PropertyType)j) + equipProperty4._MartialDef.Get((CharacterData.PropertyType)j) + this._EquipWeapon._MartialDef.Get((CharacterData.PropertyType)j) + this._EquipNecklace._MartialDef.Get((CharacterData.PropertyType)j) + this._EquipArror._MartialDef.Get((CharacterData.PropertyType)j);
			this._TotalProperty._MartialDef.Set((CharacterData.PropertyType)j, val2);
		}
		for (int k = 31; k <= 38; k++)
		{
			int val3 = this._MartialArts.Get((CharacterData.PropertyType)k) + equipProperty3._MartialArts.Get((CharacterData.PropertyType)k) + equipProperty4._MartialArts.Get((CharacterData.PropertyType)k);
			this._TotalProperty._MartialArts.Set((CharacterData.PropertyType)k, val3);
		}
		if (GameGlobal.m_bDLCMode)
		{
			return;
		}
		int num3 = 100;
		for (int l = 11; l <= 14; l++)
		{
			CharacterData.PropertyType propertyType = (CharacterData.PropertyType)l;
			int num4 = (this._TotalProperty.Get(propertyType + 4) <= num3) ? this._TotalProperty.Get(propertyType + 4) : num3;
			int val4 = (int)((float)(this._TotalProperty.Get(propertyType) * num4) * this.m_Rate[l - 11]);
			switch (propertyType)
			{
			case CharacterData.PropertyType.Strength:
				this._TotalProperty.SetPlus(CharacterData.PropertyType.Critical, val4);
				break;
			case CharacterData.PropertyType.Constitution:
				this._TotalProperty.SetPlus(CharacterData.PropertyType.MaxHP, val4);
				break;
			case CharacterData.PropertyType.Intelligence:
				this._TotalProperty.SetPlus(CharacterData.PropertyType.MaxSP, val4);
				break;
			case CharacterData.PropertyType.Dexterity:
				this._TotalProperty.SetPlus(CharacterData.PropertyType.Counter, val4);
				break;
			}
		}
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x0006F9E4 File Offset: 0x0006DBE4
	public bool CheckRendomQuest(string QuestID)
	{
		foreach (string text in this.FinishQuestList)
		{
			if (text == QuestID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x0006FA50 File Offset: 0x0006DC50
	public int GetTalent(int iTalentID)
	{
		foreach (int num in this.TalentList)
		{
			if (num == iTalentID)
			{
				return num;
			}
		}
		Debug.LogError(this._NpcDataNode.m_strNpcName + "  沒有此天賦" + iTalentID);
		return -1;
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x0006FAD4 File Offset: 0x0006DCD4
	public NpcNeigong GetNeigongData(int NeigongID)
	{
		foreach (NpcNeigong npcNeigong in this.NeigongList)
		{
			if (npcNeigong.m_Neigong.m_iNeigongID == NeigongID)
			{
				return npcNeigong;
			}
		}
		Debug.LogError(this.iNpcID + "    的內功編號   " + NeigongID);
		return null;
	}

	// Token: 0x06000D9F RID: 3487 RVA: 0x0006FB64 File Offset: 0x0006DD64
	public NpcRoutine GetRoutineData(int RoutineID)
	{
		foreach (NpcRoutine npcRoutine in this.RoutineList)
		{
			if (npcRoutine.m_Routine.m_iRoutineID == RoutineID)
			{
				return npcRoutine;
			}
		}
		Debug.LogError(this._NpcDataNode.m_strNpcName + "   沒有這個武學編號   " + RoutineID);
		return null;
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x0006FBF4 File Offset: 0x0006DDF4
	public int GetTalentValue(TalentEffect iType)
	{
		int num = 0;
		for (int i = 0; i < this.TalentList.Count; i++)
		{
			num += Game.TalentNewData.GetTalentValue(this.TalentList[i], iType);
		}
		return num;
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x0006FC3C File Offset: 0x0006DE3C
	public float GetNpcTalentPercentValue(TalentEffect iType)
	{
		float num = 0f;
		for (int i = 0; i < this.TalentList.Count; i++)
		{
			num += Game.TalentNewData.GetTalentPercentValue(this.TalentList[i], iType);
		}
		return num;
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x0006FC88 File Offset: 0x0006DE88
	public bool CheckTalentEffect(TalentEffect iType)
	{
		for (int i = 0; i < this.TalentList.Count; i++)
		{
			bool flag = Game.TalentNewData.CheckTalemEffect(this.TalentList[i], iType);
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x0006FCD4 File Offset: 0x0006DED4
	public string GetTalentNameHaveEffect(TalentEffect iType)
	{
		for (int i = 0; i < this.TalentList.Count; i++)
		{
			if (Game.TalentNewData.CheckTalemEffect(this.TalentList[i], iType))
			{
				return Game.TalentNewData.GetTalentName(this.TalentList[i]);
			}
		}
		return string.Empty;
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x0006FD38 File Offset: 0x0006DF38
	public List<TalentResultPart> GetTalentResultPart(TalentEffect iType)
	{
		List<TalentResultPart> list = new List<TalentResultPart>();
		for (int i = 0; i < this.TalentList.Count; i++)
		{
			list.AddRange(Game.TalentNewData.GetTalnetResulePart(this.TalentList[i], iType).ToArray());
		}
		return list;
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x00009751 File Offset: 0x00007951
	public void AddTalent(int iTalentID)
	{
		if (!this.CheckTalent(iTalentID))
		{
			return;
		}
		this.TalentList.Add(iTalentID);
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x0006FD8C File Offset: 0x0006DF8C
	public bool CheckSkillMax(int itype)
	{
		int num = 12;
		int num2 = 12;
		int num3 = 5;
		if (itype == 0)
		{
			if (this.RoutineList.Count >= num)
			{
				Debug.LogError(this.iNpcID + " 的武學已滿 ");
				return false;
			}
		}
		else if (itype == 1)
		{
			if (this.NeigongList.Count >= num2)
			{
				Debug.LogError(this.iNpcID + " 的內功已滿 ");
				return false;
			}
		}
		else if (itype == 2 && this.TalentList.Count >= num3)
		{
			Debug.LogError(this.iNpcID + " 的天賦已滿 ");
			return false;
		}
		return true;
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x0000976C File Offset: 0x0000796C
	public void ClearLevelupString()
	{
		this.strLevelup = string.Empty;
	}

	// Token: 0x06000DA8 RID: 3496 RVA: 0x0006FE2C File Offset: 0x0006E02C
	public int DLC_AddExp(int iExp)
	{
		int num = this.iLevel;
		this.iCurTotalExp += iExp;
		this.iLevel = Game.LevelUpExpData.GetNowExpLevel(this.iCurTotalExp);
		if (num != this.iLevel)
		{
			this.DLC_LevelupHidePassive(string.Empty);
		}
		return this.iLevel - num;
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x00009779 File Offset: 0x00007979
	public int DLC_NowLevelUpExp()
	{
		return Game.LevelUpExpData.GetLevelExp(this.iLevel);
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x0006FE84 File Offset: 0x0006E084
	public int DLC_NowLevelExp()
	{
		int levelTotalExp = Game.LevelUpExpData.GetLevelTotalExp(this.iLevel - 1);
		return this.iCurTotalExp - levelTotalExp;
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x0006FEAC File Offset: 0x0006E0AC
	public void DLC_LevelupHidePassive(string unitGid = "")
	{
		UpgradeNode upgradeNode = Game.UpgradeData.GetUpgradeNode(this.iNpcID);
		if (upgradeNode != null)
		{
			for (int i = 0; i < upgradeNode.m_HidePassiveNodeList.Count; i++)
			{
				int num = upgradeNode.m_HidePassiveNodeList[i];
				if (!this.passiveNodeList.Contains(num))
				{
					LevelUpPassiveNode levelUpPassiveNode = Game.LevelUpPassiveData.GetLevelUpPassiveNode(num);
					if (levelUpPassiveNode != null)
					{
						if (levelUpPassiveNode.bAuto)
						{
							if (ConditionManager.CheckCondition(levelUpPassiveNode.m_RequestConditionList, true, this.iNpcID, unitGid))
							{
								this.DLC_AddPassiveNode(num);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x0006FF5C File Offset: 0x0006E15C
	public void DLC_AddPassiveNode(int passiveNodeID)
	{
		LevelUpPassiveNode levelUpPassiveNode = Game.LevelUpPassiveData.GetLevelUpPassiveNode(passiveNodeID);
		if (levelUpPassiveNode == null)
		{
			return;
		}
		this.iSkillPoint -= levelUpPassiveNode.price;
		this.passiveNodeList.Add(passiveNodeID);
		bool flag = false;
		for (int i = 0; i < levelUpPassiveNode.m_PassiveNodeList.Count; i++)
		{
			PassiveNode passiveNode = levelUpPassiveNode.m_PassiveNodeList[i];
			switch (passiveNode.pNodeType)
			{
			case PassiveNodeType.Property:
				this.DLC_Property(passiveNode.iValue1, passiveNode.iValue2);
				flag = true;
				break;
			case PassiveNodeType.Routine:
				this.DLC_AddRoutine(passiveNode.iValue1, passiveNode.iValue2);
				break;
			case PassiveNodeType.NeigongCondition:
				this.DLC_AddNeigongCondition(passiveNode.iValue1, passiveNode.iValue2);
				if (this.NeigongList.Count > 0 && this.NeigongList[0].m_Neigong != null)
				{
					this.NeigongList[0].m_Neigong.CheckConditionEffectOpen();
				}
				break;
			case PassiveNodeType.Talnet:
				this.DLC_AddTalent(passiveNode.iValue1);
				break;
			case PassiveNodeType.ResistPoint:
				this.iResistPoint += passiveNode.iValue1;
				break;
			case PassiveNodeType.SkillPoint:
				this.iSkillPoint += passiveNode.iValue1;
				break;
			}
		}
		if (flag)
		{
			this.setTotalProperty();
		}
	}

	// Token: 0x06000DAD RID: 3501 RVA: 0x000700D0 File Offset: 0x0006E2D0
	private void DLC_Property(int iType, int iValue)
	{
		string text = Game.StringTable.GetString(110100 + iType) + " [40FF40]+" + iValue.ToString() + "[-]";
		if (this.strLevelup != string.Empty)
		{
			this.strLevelup += "\n";
		}
		this.strLevelup += text;
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x00070144 File Offset: 0x0006E344
	private void DLC_AddRoutine(int iRoutineID, int iOrigRoutineID)
	{
		string getRoutineName = Game.RoutineNewData.GetGetRoutineName(iRoutineID);
		bool flag = false;
		if (iOrigRoutineID != 0)
		{
			NpcRoutine routineData = this.GetRoutineData(iOrigRoutineID);
			if (routineData != null)
			{
				routineData.iSkillID = iRoutineID;
				if (!routineData.SetRoutine())
				{
					routineData.iSkillID = iOrigRoutineID;
					routineData.SetRoutine();
					return;
				}
			}
			else
			{
				flag = this._DLCAddRoutine(iRoutineID);
			}
		}
		else
		{
			flag = this._DLCAddRoutine(iRoutineID);
		}
		if (flag)
		{
			string text = string.Format(Game.StringTable.GetString(180000), this._NpcDataNode.m_strNpcName, getRoutineName);
			if (this.strLevelup != string.Empty)
			{
				this.strLevelup += "\n";
			}
			this.strLevelup += text;
		}
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x00070214 File Offset: 0x0006E414
	private bool _DLCAddRoutine(int iRoutineID)
	{
		if (!this.CheckRoutine(iRoutineID))
		{
			return false;
		}
		RoutineNewDataNode routineNewData = Game.RoutineNewData.GetRoutineNewData(iRoutineID);
		NpcRoutine npcRoutine = new NpcRoutine();
		npcRoutine.m_Routine = routineNewData;
		npcRoutine.iSkillID = iRoutineID;
		npcRoutine.SetLv(10);
		int num = 0;
		for (int i = 0; i < this.RoutineList.Count; i++)
		{
			if (this.RoutineList[i].bUse)
			{
				num++;
			}
		}
		npcRoutine.bUse = (num <= 6);
		this.RoutineList.Add(npcRoutine);
		return true;
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x000702B8 File Offset: 0x0006E4B8
	private void DLC_AddNeigongCondition(int iConditionID, int iOrigConditionID)
	{
		NpcNeigong nowUseNeigong = this.GetNowUseNeigong();
		ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(iConditionID);
		if (nowUseNeigong == null)
		{
			Debug.Log(this._NpcDataNode.m_strNpcName + " not have Neigong");
			return;
		}
		if (conditionNode == null)
		{
			Debug.Log("ConditionID " + iConditionID.ToString() + " not found");
			return;
		}
		if (iOrigConditionID != 0)
		{
			if (!nowUseNeigong.m_Neigong.ReplaceBattleConditionID(iOrigConditionID, iConditionID))
			{
				nowUseNeigong.m_Neigong.AddBattleConditionID(iConditionID);
			}
		}
		else
		{
			nowUseNeigong.m_Neigong.AddBattleConditionID(iConditionID);
		}
		string text = string.Format(Game.StringTable.GetString(260001), nowUseNeigong.m_Neigong.m_strNeigongName, conditionNode.m_strName);
		if (this.strLevelup != string.Empty)
		{
			this.strLevelup += "\n";
		}
		this.strLevelup += text;
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x000703B8 File Offset: 0x0006E5B8
	private void DLC_AddTalent(int talentID)
	{
		if (!this.CheckTalent(talentID))
		{
			return;
		}
		TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(talentID);
		string text = string.Format(Game.StringTable.GetString(210045), string.Empty, talentData.m_strTalentName);
		if (this.strLevelup != string.Empty)
		{
			this.strLevelup += "\n";
		}
		this.strLevelup += text;
		this.AddTalent(talentID);
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x00070444 File Offset: 0x0006E644
	public void DLC_ResetNeigongConditionPassive()
	{
		foreach (int iID in this.passiveNodeList)
		{
			LevelUpPassiveNode levelUpPassiveNode = Game.LevelUpPassiveData.GetLevelUpPassiveNode(iID);
			if (levelUpPassiveNode != null)
			{
				for (int i = 0; i < levelUpPassiveNode.m_PassiveNodeList.Count; i++)
				{
					PassiveNode passiveNode = levelUpPassiveNode.m_PassiveNodeList[i];
					PassiveNodeType pNodeType = passiveNode.pNodeType;
					if (pNodeType == PassiveNodeType.NeigongCondition)
					{
						this.DLC_AddNeigongCondition(passiveNode.iValue1, passiveNode.iValue2);
					}
				}
			}
		}
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x0007050C File Offset: 0x0006E70C
	public List<NpcNeigong> mod_GetNowUsedNeigong()
	{
		List<NpcNeigong> list = new List<NpcNeigong>();
		for (int i = 0; i < this.NeigongList.Count; i++)
		{
			if (this.NeigongList[i].bUse)
			{
				list.Add(this.NeigongList[i]);
			}
		}
		return list;
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x0007055C File Offset: 0x0006E75C
	public void mod_SetNeigongUnuse(int iNeigongID)
	{
		for (int i = 0; i < this.NeigongList.Count; i++)
		{
			if (this.NeigongList[i].iSkillID == iNeigongID)
			{
				this.NeigongList[i].bUse = false;
				return;
			}
		}
	}

	// Token: 0x04000FB6 RID: 4022
	[JsonIgnore]
	public NpcDataNode _NpcDataNode;

	// Token: 0x04000FB7 RID: 4023
	public eNPCType NpcType;

	// Token: 0x04000FB8 RID: 4024
	[JsonIgnore]
	public int iCurHp;

	// Token: 0x04000FB9 RID: 4025
	[JsonIgnore]
	public int iCurSp;

	// Token: 0x04000FBA RID: 4026
	[JsonIgnore]
	public int iAttack;

	// Token: 0x04000FBB RID: 4027
	[JsonIgnore]
	public int iDefense;

	// Token: 0x04000FBC RID: 4028
	[JsonIgnore]
	public EquipProperty _EquipWeapon;

	// Token: 0x04000FBD RID: 4029
	[JsonIgnore]
	public EquipProperty _EquipArror;

	// Token: 0x04000FBE RID: 4030
	[JsonIgnore]
	public EquipProperty _EquipNecklace;

	// Token: 0x04000FBF RID: 4031
	[JsonIgnore]
	public EquipProperty _TotalProperty;

	// Token: 0x04000FC0 RID: 4032
	public int NowPractice;

	// Token: 0x04000FC1 RID: 4033
	public string strNowDoQuest;

	// Token: 0x04000FC2 RID: 4034
	public List<string> FinishQuestList;

	// Token: 0x04000FC3 RID: 4035
	public int iFinishTime;

	// Token: 0x04000FC4 RID: 4036
	public int iLevel;

	// Token: 0x04000FC5 RID: 4037
	public int iCurTotalExp;

	// Token: 0x04000FC6 RID: 4038
	public int iSkillPoint;

	// Token: 0x04000FC7 RID: 4039
	public int iResistPoint;

	// Token: 0x04000FC8 RID: 4040
	public int iHurtTurn;

	// Token: 0x04000FC9 RID: 4041
	public List<int> passiveNodeList;

	// Token: 0x04000FCA RID: 4042
	public int iChangeNPCID;

	// Token: 0x04000FCB RID: 4043
	[JsonIgnore]
	public string strLevelup;

	// Token: 0x04000FCC RID: 4044
	[JsonIgnore]
	public bool binit = true;

	// Token: 0x04000FCD RID: 4045
	[JsonIgnore]
	private float[] m_Rate = new float[]
	{
		0.001f,
		0.1f,
		0.05f,
		0.001f
	};

	// Token: 0x04000FCE RID: 4046
	public int mod_WeaponGuid;

	// Token: 0x04000FCF RID: 4047
	public int mod_ArmorGuid;

	// Token: 0x04000FD0 RID: 4048
	public int mod_NecklaceGuid;

	// Token: 0x020002BB RID: 699
	public enum PropertyType
	{
		// Token: 0x04000FD2 RID: 4050
		CurHP,
		// Token: 0x04000FD3 RID: 4051
		MaxHP,
		// Token: 0x04000FD4 RID: 4052
		CurSP,
		// Token: 0x04000FD5 RID: 4053
		MaxSP,
		// Token: 0x04000FD6 RID: 4054
		MoveStep,
		// Token: 0x04000FD7 RID: 4055
		Fetters,
		// Token: 0x04000FD8 RID: 4056
		Strength = 11,
		// Token: 0x04000FD9 RID: 4057
		Constitution,
		// Token: 0x04000FDA RID: 4058
		Intelligence,
		// Token: 0x04000FDB RID: 4059
		Dexterity,
		// Token: 0x04000FDC RID: 4060
		StrengthMax,
		// Token: 0x04000FDD RID: 4061
		ConstitutionMax,
		// Token: 0x04000FDE RID: 4062
		IntelligenceMax,
		// Token: 0x04000FDF RID: 4063
		DexterityMax,
		// Token: 0x04000FE0 RID: 4064
		UseSword = 21,
		// Token: 0x04000FE1 RID: 4065
		UseBlade,
		// Token: 0x04000FE2 RID: 4066
		UseArrow,
		// Token: 0x04000FE3 RID: 4067
		UseFist,
		// Token: 0x04000FE4 RID: 4068
		UseGas,
		// Token: 0x04000FE5 RID: 4069
		UseRope,
		// Token: 0x04000FE6 RID: 4070
		UseWhip,
		// Token: 0x04000FE7 RID: 4071
		UsePike,
		// Token: 0x04000FE8 RID: 4072
		UseSwordMax = 31,
		// Token: 0x04000FE9 RID: 4073
		UseBladeMax,
		// Token: 0x04000FEA RID: 4074
		UseArrowMax,
		// Token: 0x04000FEB RID: 4075
		UseFistMax,
		// Token: 0x04000FEC RID: 4076
		UseGasMax,
		// Token: 0x04000FED RID: 4077
		UseRopeMax,
		// Token: 0x04000FEE RID: 4078
		UseWhipMax,
		// Token: 0x04000FEF RID: 4079
		UsePikeMax,
		// Token: 0x04000FF0 RID: 4080
		DefSword = 41,
		// Token: 0x04000FF1 RID: 4081
		DefBlade,
		// Token: 0x04000FF2 RID: 4082
		DefArrow,
		// Token: 0x04000FF3 RID: 4083
		DefFist,
		// Token: 0x04000FF4 RID: 4084
		DefGas,
		// Token: 0x04000FF5 RID: 4085
		DefRope,
		// Token: 0x04000FF6 RID: 4086
		DefWhip,
		// Token: 0x04000FF7 RID: 4087
		DefPike,
		// Token: 0x04000FF8 RID: 4088
		Dodge = 51,
		// Token: 0x04000FF9 RID: 4089
		Counter,
		// Token: 0x04000FFA RID: 4090
		Critical,
		// Token: 0x04000FFB RID: 4091
		Combo,
		// Token: 0x04000FFC RID: 4092
		DefendDodge,
		// Token: 0x04000FFD RID: 4093
		DefendCounter,
		// Token: 0x04000FFE RID: 4094
		DefendCritical,
		// Token: 0x04000FFF RID: 4095
		DefendCombo,
		// Token: 0x04001000 RID: 4096
		Attack = 99,
		// Token: 0x04001001 RID: 4097
		DefSwordExp = 141,
		// Token: 0x04001002 RID: 4098
		DefBladeExp,
		// Token: 0x04001003 RID: 4099
		DefArrowExp,
		// Token: 0x04001004 RID: 4100
		DefFistExp,
		// Token: 0x04001005 RID: 4101
		DefGasExp,
		// Token: 0x04001006 RID: 4102
		DefRopeExp,
		// Token: 0x04001007 RID: 4103
		DefWhipExp,
		// Token: 0x04001008 RID: 4104
		DefPikeExp
	}

	// Token: 0x020002BC RID: 700
	private enum ePracticeType
	{
		// Token: 0x0400100A RID: 4106
		Routine,
		// Token: 0x0400100B RID: 4107
		Neigong,
		// Token: 0x0400100C RID: 4108
		Talent
	}
}
