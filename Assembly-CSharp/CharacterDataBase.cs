using System;
using System.Collections.Generic;
using Heluo.Wulin;
using Newtonsoft.Json;

// Token: 0x020001F8 RID: 504
public class CharacterDataBase
{
	// Token: 0x06000A08 RID: 2568 RVA: 0x00054E84 File Offset: 0x00053084
	public CharacterDataBase()
	{
		this.WeaponTypeList = new List<WeaponType>();
		this._MartialArts = new MartialArts();
		this._MartialDef = new MartialDef();
		this.Itemlist = new List<NpcItem>();
		this.RoutineList = new List<NpcRoutine>();
		this.NeigongList = new List<NpcNeigong>();
		this.TalentList = new List<int>();
		this.sVoicList = new List<string>();
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x00054EF0 File Offset: 0x000530F0
	public virtual CharacterData Clone()
	{
		CharacterData characterData = new CharacterData();
		characterData._NpcDataNode = null;
		characterData.iNpcID = this.iNpcID;
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
		characterData.iEquipWeaponID = this.iEquipWeaponID;
		characterData.iEquipArrorID = this.iEquipArrorID;
		characterData.iEquipNecklaceID = this.iEquipNecklaceID;
		characterData.TalentList.AddRange(this.TalentList.ToArray());
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
		characterData.sVoicList.AddRange(this.sVoicList);
		characterData.iPrice = this.iPrice;
		characterData.bCaptive = this.bCaptive;
		characterData.iJoin = this.iJoin;
		characterData.iSmuggleSuccess = this.iSmuggleSuccess;
		characterData.iSmuggleSurvive = this.iSmuggleSurvive;
		characterData.iHonor = this.iHonor;
		return characterData;
	}

	// Token: 0x04000A50 RID: 2640
	public int iNpcID;

	// Token: 0x04000A51 RID: 2641
	public List<WeaponType> WeaponTypeList;

	// Token: 0x04000A52 RID: 2642
	public int iMaxHp;

	// Token: 0x04000A53 RID: 2643
	public int iMaxSp;

	// Token: 0x04000A54 RID: 2644
	public int iMoveStep;

	// Token: 0x04000A55 RID: 2645
	public int iMoney;

	// Token: 0x04000A56 RID: 2646
	public int iStr;

	// Token: 0x04000A57 RID: 2647
	public int iInt;

	// Token: 0x04000A58 RID: 2648
	public int iDex;

	// Token: 0x04000A59 RID: 2649
	public int iCon;

	// Token: 0x04000A5A RID: 2650
	public int iMaxStr;

	// Token: 0x04000A5B RID: 2651
	public int iMaxInt;

	// Token: 0x04000A5C RID: 2652
	public int iMaxDex;

	// Token: 0x04000A5D RID: 2653
	public int iMaxCon;

	// Token: 0x04000A5E RID: 2654
	public int iCri;

	// Token: 0x04000A5F RID: 2655
	public int iCounter;

	// Token: 0x04000A60 RID: 2656
	public int iDodge;

	// Token: 0x04000A61 RID: 2657
	public int iCombo;

	// Token: 0x04000A62 RID: 2658
	public int iDefendCri;

	// Token: 0x04000A63 RID: 2659
	public int iDefendCounter;

	// Token: 0x04000A64 RID: 2660
	public int iDefendCombo;

	// Token: 0x04000A65 RID: 2661
	public int iDefendDodge;

	// Token: 0x04000A66 RID: 2662
	public MartialArts _MartialArts;

	// Token: 0x04000A67 RID: 2663
	public MartialDef _MartialDef;

	// Token: 0x04000A68 RID: 2664
	public int iEquipWeaponID;

	// Token: 0x04000A69 RID: 2665
	public int iEquipArrorID;

	// Token: 0x04000A6A RID: 2666
	public int iEquipNecklaceID;

	// Token: 0x04000A6B RID: 2667
	public List<NpcItem> Itemlist;

	// Token: 0x04000A6C RID: 2668
	public List<NpcRoutine> RoutineList;

	// Token: 0x04000A6D RID: 2669
	public List<NpcNeigong> NeigongList;

	// Token: 0x04000A6E RID: 2670
	public List<int> TalentList;

	// Token: 0x04000A6F RID: 2671
	[JsonIgnore]
	public List<string> sVoicList;

	// Token: 0x04000A70 RID: 2672
	[JsonIgnore]
	public int iPrice;

	// Token: 0x04000A71 RID: 2673
	[JsonIgnore]
	public bool bCaptive;

	// Token: 0x04000A72 RID: 2674
	[JsonIgnore]
	public int iJoin;

	// Token: 0x04000A73 RID: 2675
	[JsonIgnore]
	public int iSmuggleSuccess;

	// Token: 0x04000A74 RID: 2676
	[JsonIgnore]
	public int iSmuggleSurvive;

	// Token: 0x04000A75 RID: 2677
	[JsonIgnore]
	public int iHonor;
}
