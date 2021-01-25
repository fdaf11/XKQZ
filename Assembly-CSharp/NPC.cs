using System;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020002BD RID: 701
public class NPC : MonoBehaviour
{
	// Token: 0x06000DB6 RID: 3510 RVA: 0x0000979E File Offset: 0x0000799E
	private void Awake()
	{
		if (NPC.m_instance == null)
		{
			NPC.m_instance = this;
		}
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x000705A8 File Offset: 0x0006E7A8
	[ContextMenu("StartNewGame")]
	public void StartNewGame(bool bDLC = false)
	{
		if (!Save.m_Instance.bLoad)
		{
			Save.m_Instance.bLoad = true;
		}
		this.NpcList.Clear();
		this.NpcList = Game.CharacterData.GetNpcList();
		this.setNpcList(null);
		NpcRandomEvent.g_NpcQuestList.Clear();
		NpcRandomEvent.SetNpcListDoSomething();
		if (bDLC)
		{
			YoungHeroTime.m_instance.UpdateDLCStore();
		}
		Save.m_Instance.m_SaveVersionFix.Add("SaveCharacterFix1020");
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x00070624 File Offset: 0x0006E824
	private void fixNpcQuest1020()
	{
		if (!Save.m_Instance.m_SaveVersionFix.Contains("SaveCharacterFix1020"))
		{
			for (int i = 0; i < this.NpcList.Count; i++)
			{
				CharacterData characterData = this.NpcList[i];
				List<string> list = new List<string>();
				for (int j = 0; j < characterData.FinishQuestList.Count; j++)
				{
					string text = characterData.FinishQuestList[j];
					if (!list.Contains(text))
					{
						list.Add(text);
					}
				}
				characterData.FinishQuestList.Clear();
				characterData.FinishQuestList.AddRange(list);
			}
			Save.m_Instance.m_SaveVersionFix.Add("SaveCharacterFix1020");
		}
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x000706E8 File Offset: 0x0006E8E8
	private void setNpcList(List<CharacterData> LoadList = null)
	{
		for (int i = 0; i < this.NpcList.Count; i++)
		{
			bool flag = false;
			if (LoadList != null)
			{
				for (int j = 0; j < LoadList.Count; j++)
				{
					if (this.NpcList[i].iNpcID == LoadList[j].iNpcID)
					{
						List<string> sVoicList = new List<string>(this.NpcList[i].sVoicList.ToArray());
						int iPrice = this.NpcList[i].iPrice;
						bool bCaptive = this.NpcList[i].bCaptive;
						int iJoin = this.NpcList[i].iJoin;
						int iSmuggleSuccess = this.NpcList[i].iSmuggleSuccess;
						int iSmuggleSurvive = this.NpcList[i].iSmuggleSurvive;
						int iHonor = this.NpcList[i].iHonor;
						this.NpcList[i] = LoadList[j];
						this.NpcList[i].sVoicList = sVoicList;
						this.NpcList[i].iPrice = iPrice;
						this.NpcList[i].bCaptive = bCaptive;
						this.NpcList[i].iJoin = iJoin;
						this.NpcList[i].iSmuggleSuccess = iSmuggleSuccess;
						this.NpcList[i].iSmuggleSurvive = iSmuggleSurvive;
						this.NpcList[i].iHonor = iHonor;
						flag = true;
						break;
					}
				}
			}
			NpcDataNode npcData;
			if (this.NpcList[i].iChangeNPCID == 0)
			{
				npcData = Game.NpcData.GetNpcData(this.NpcList[i].iNpcID);
			}
			else
			{
				npcData = Game.NpcData.GetNpcData(this.NpcList[i].iChangeNPCID);
			}
			if (npcData == null)
			{
				Debug.LogError("找不到 Npc " + this.NpcList[i].iNpcID.ToString() + " 請查 NpcData.txt ");
			}
			else
			{
				this.NpcList[i]._NpcDataNode = npcData;
				if (!flag)
				{
					this.NpcList[i].NpcType = eNPCType.Nothing;
				}
				int num = 0;
				List<NpcNeigong> list = new List<NpcNeigong>();
				foreach (NpcNeigong npcNeigong in this.NpcList[i].NeigongList)
				{
					if (list.Count < 12)
					{
						if (npcNeigong.SetNeigongData(this.NpcList[i].iNpcID))
						{
							if (!flag)
							{
								npcNeigong.SetLv(npcNeigong.iLevel);
								if (num > 1)
								{
									npcNeigong.bUse = false;
								}
								if (npcNeigong.bUse)
								{
									num++;
								}
							}
							else
							{
								npcNeigong.SetExp(0);
							}
						}
						else
						{
							Debug.LogError(string.Concat(new object[]
							{
								"請確認 CharacterData ",
								this.NpcList[i].iNpcID.ToString(),
								" 的內功欄位 ",
								npcNeigong.iSkillID
							}));
						}
						list.Add(npcNeigong);
					}
				}
				this.NpcList[i].NeigongList.Clear();
				this.NpcList[i].NeigongList.AddRange(list);
				if (num != 1 && !flag && this.NpcList[i].NeigongList.Count > 0)
				{
					this.NpcList[i].NeigongList[0].bUse = true;
				}
				int num2 = 6;
				int num3 = 0;
				foreach (NpcRoutine npcRoutine in this.NpcList[i].RoutineList)
				{
					if (npcRoutine.SetRoutine())
					{
						if (!flag)
						{
							npcRoutine.SetLv(npcRoutine.iLevel);
						}
						else
						{
							npcRoutine.SetExp(0);
						}
						num3++;
					}
					else
					{
						Debug.LogError(string.Concat(new object[]
						{
							"請確認 CharacterData ",
							this.NpcList[i].iNpcID.ToString(),
							" 的套路欄位 ",
							npcRoutine.iSkillID
						}));
					}
					if (!flag)
					{
						if (num3 > num2)
						{
							npcRoutine.bUse = false;
						}
						else
						{
							npcRoutine.bUse = true;
						}
					}
				}
				if (!flag)
				{
					if (this.NpcList[i].RoutineList.Count > 0)
					{
						this.NpcList[i].NowPractice = this.NpcList[i].RoutineList[0].m_Routine.m_iRoutineID;
					}
					if (this.NpcList[i].NeigongList.Count > 0)
					{
						this.NpcList[i].NowPractice = this.NpcList[i].NeigongList[0].m_Neigong.m_iNeigongID;
					}
				}
				if (this.NpcList[i]._EquipArror == null)
				{
					this.NpcList[i]._EquipArror = new EquipProperty();
				}
				if (this.NpcList[i]._EquipWeapon == null)
				{
					this.NpcList[i]._EquipWeapon = new EquipProperty();
				}
				if (this.NpcList[i]._EquipNecklace == null)
				{
					this.NpcList[i]._EquipNecklace = new EquipProperty();
				}
				this.NpcList[i].SetEquip(ItemDataNode.ItemType.Weapon, this.NpcList[i].iEquipWeaponID);
				this.NpcList[i].SetEquip(ItemDataNode.ItemType.Arror, this.NpcList[i].iEquipArrorID);
				this.NpcList[i].SetEquip(ItemDataNode.ItemType.Necklace, this.NpcList[i].iEquipNecklaceID);
				this.NpcList[i].DLC_ResetNeigongConditionPassive();
				this.NpcList[i].setTotalProperty();
				this.NpcList[i].binit = false;
			}
		}
		if (GameGlobal.m_bAffterMode)
		{
			this.SetAffterNPCData();
		}
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x000097B6 File Offset: 0x000079B6
	public void LoadNPCList(List<CharacterData> LoadList)
	{
		this.NpcList.Clear();
		this.NpcList = Game.CharacterData.GetNpcList();
		this.setNpcList(LoadList);
		this.fixNpcQuest1020();
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x00070D48 File Offset: 0x0006EF48
	public List<CharacterData> GetSaveDataList()
	{
		List<CharacterData> list = new List<CharacterData>();
		for (int i = 0; i < this.NpcList.Count; i++)
		{
			CharacterData characterData = this.NpcList[i].Clone();
			list.Add(characterData);
		}
		return list;
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x00070D94 File Offset: 0x0006EF94
	public CharacterData GetCharacterData(int iId)
	{
		int num = this.BinarySearch(iId);
		if (num >= 0)
		{
			return this.NpcList[num];
		}
		return null;
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x00070DC0 File Offset: 0x0006EFC0
	private int BinarySearch(int id)
	{
		List<CharacterData> npcList = this.NpcList;
		int i = 0;
		int num = npcList.Count - 1;
		while (i <= num)
		{
			int num2 = (i + num) / 2;
			if (npcList[num2].iNpcID == id)
			{
				return num2;
			}
			if (npcList[num2].iNpcID < id)
			{
				i = num2 + 1;
			}
			else
			{
				num = num2 - 1;
			}
		}
		return -1;
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x00070E28 File Offset: 0x0006F028
	public bool AddTalent(int NpcId, int TalentID)
	{
		CharacterData characterData = this.GetCharacterData(NpcId);
		if (characterData == null)
		{
			Debug.Log("請檢察 " + NpcId);
			return false;
		}
		if (!characterData.CheckTalent(TalentID))
		{
			return false;
		}
		characterData.TalentList.Add(TalentID);
		return true;
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x00070E78 File Offset: 0x0006F078
	public bool AddRoutine(int NpcId, int iRoutineID, int iLevel = 1)
	{
		CharacterData characterData = this.GetCharacterData(NpcId);
		if (characterData == null)
		{
			Debug.Log("請檢察 " + NpcId);
			return false;
		}
		if (!characterData.CheckRoutine(iRoutineID))
		{
			return false;
		}
		RoutineNewDataNode routineNewData = Game.RoutineNewData.GetRoutineNewData(iRoutineID);
		NpcRoutine npcRoutine = new NpcRoutine();
		npcRoutine.m_Routine = routineNewData;
		npcRoutine.iSkillID = iRoutineID;
		npcRoutine.SetLv(iLevel);
		int num = 0;
		for (int i = 0; i < characterData.RoutineList.Count; i++)
		{
			if (characterData.RoutineList[i].bUse)
			{
				num++;
			}
		}
		npcRoutine.bUse = (num <= 6);
		characterData.RoutineList.Add(npcRoutine);
		characterData.setTotalProperty();
		return true;
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x00070F4C File Offset: 0x0006F14C
	public bool AddNeigong(int NpcId, int iNeigongID, int iLevel = 1)
	{
		CharacterData characterData = this.GetCharacterData(NpcId);
		if (characterData == null)
		{
			Debug.Log("沒有這個NPC  ID:  " + NpcId + string.Empty);
			return false;
		}
		if (!characterData.CheckNeigong(iNeigongID))
		{
			return false;
		}
		NpcNeigong npcNeigong = new NpcNeigong();
		npcNeigong.iSkillID = iNeigongID;
		if (npcNeigong.SetNeigongData(NpcId))
		{
			npcNeigong.SetLv(iLevel);
			characterData.NeigongList.Add(npcNeigong);
			characterData.setTotalProperty();
			return true;
		}
		return false;
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x00070FCC File Offset: 0x0006F1CC
	public bool AddNpcItem(int NpcId, int ItemID)
	{
		CharacterData characterData = this.GetCharacterData(NpcId);
		if (characterData == null)
		{
			Debug.Log("請檢察 " + NpcId);
			return false;
		}
		characterData.AddNpcItem(ItemID, 1);
		return true;
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x000097E0 File Offset: 0x000079E0
	public void BattleNowPracticeLvUp(CharacterData _CharacterData)
	{
		_CharacterData.setTotalProperty();
		if (_CharacterData.NpcType != eNPCType.Teammate)
		{
			_CharacterData.GetNextPractice();
		}
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x00071008 File Offset: 0x0006F208
	public void SetNeigongExp(CharacterData CD, int NeigongID, int iExp)
	{
		for (int i = 0; i < CD.NeigongList.Count; i++)
		{
			if (CD.NeigongList[i].m_Neigong.m_iNeigongID == NeigongID)
			{
				CD.NeigongList[i].SetExp(iExp);
			}
		}
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x00071060 File Offset: 0x0006F260
	public int SetRoutineExp(CharacterData CD, int RoutineID, int iExp)
	{
		for (int i = 0; i < CD.RoutineList.Count; i++)
		{
			if (CD.RoutineList[i].m_Routine.m_iRoutineID == RoutineID)
			{
				return CD.RoutineList[i].SetExp(iExp);
			}
		}
		return 0;
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x000710BC File Offset: 0x0006F2BC
	public void SetTeam(int NpcID, eNPCType eNT)
	{
		CharacterData characterData = this.GetCharacterData(NpcID);
		characterData.NpcType = eNT;
		characterData.strNowDoQuest = string.Empty;
		if (eNT == eNPCType.Nothing)
		{
			NpcRandomEvent.SetNpcDoSomething(characterData);
		}
		if (MapData.m_instance != null)
		{
			MapData.m_instance.CheckNpcSpeicalList();
		}
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x0007110C File Offset: 0x0006F30C
	public int GetNeigongLv(int iNpcID, int NeigongID)
	{
		CharacterData characterData = this.GetCharacterData(iNpcID);
		if (characterData == null)
		{
			Debug.Log("GetNeigongLv  " + iNpcID);
			return 0;
		}
		NpcNeigong neigongData = characterData.GetNeigongData(NeigongID);
		if (neigongData == null)
		{
			Debug.Log("GetNeigongLv 檢查   " + NeigongID);
			return 0;
		}
		return neigongData.iLevel;
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x0007116C File Offset: 0x0006F36C
	public int GetRoutineLv(int iNpcID, int iRoutineID)
	{
		CharacterData characterData = this.GetCharacterData(iNpcID);
		if (characterData == null)
		{
			Debug.Log("GetRoutineLv  " + iNpcID);
			return 0;
		}
		NpcRoutine routineData = characterData.GetRoutineData(iRoutineID);
		if (routineData == null)
		{
			Debug.Log("GetRoutineLv 檢查   " + iRoutineID);
			return 0;
		}
		return routineData.iLevel;
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x000711CC File Offset: 0x0006F3CC
	public int getCharacterProperty(int iNpcID, int itype, CharacterData ca = null)
	{
		if (ca == null)
		{
			ca = this.GetCharacterData(iNpcID);
		}
		if (ca == null)
		{
			return 0;
		}
		if (itype >= 41 && itype <= 48)
		{
			return ca._TotalProperty._MartialDef.Get((CharacterData.PropertyType)itype);
		}
		if (itype >= 21 && itype <= 28)
		{
			return ca._TotalProperty._MartialArts.Get((CharacterData.PropertyType)itype);
		}
		return ca._TotalProperty.Get((CharacterData.PropertyType)itype);
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x00071244 File Offset: 0x0006F444
	public bool CheckDLCCharLevel(int iNpcID, int iType, int iLevel)
	{
		CharacterData characterData = this.GetCharacterData(iNpcID);
		if (characterData == null)
		{
			Debug.Log("HaveThisPassive Null npcID = " + iNpcID);
			return false;
		}
		return this.CheckRequestType(iType, characterData.iLevel, iLevel);
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00071284 File Offset: 0x0006F484
	private bool CheckRequestType(int iType, int val, int iVal)
	{
		if (iType == 0)
		{
			return val == iVal;
		}
		if (iType == 1)
		{
			return val > iVal;
		}
		if (iType == 3)
		{
			return val >= iVal;
		}
		if (iType == 2)
		{
			return val < iVal;
		}
		if (iType == 4)
		{
			return val <= iVal;
		}
		return iType == 5 && val != iVal;
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x00071300 File Offset: 0x0006F500
	public bool HaveThisPassive(int iNpcID, int iPassiveMode)
	{
		CharacterData characterData = this.GetCharacterData(iNpcID);
		if (characterData == null)
		{
			Debug.Log("HaveThisPassive Null npcID = " + iNpcID);
			return false;
		}
		return characterData.passiveNodeList.Contains(iPassiveMode);
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x00071340 File Offset: 0x0006F540
	public void AddPassive(int iNpcID, int iPassiveMode)
	{
		CharacterData characterData = this.GetCharacterData(iNpcID);
		if (characterData == null)
		{
			Debug.Log("AddPassive Null npcID = " + iNpcID);
			return;
		}
		if (!characterData.passiveNodeList.Contains(iPassiveMode))
		{
			characterData.passiveNodeList.Add(iPassiveMode);
		}
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x00071390 File Offset: 0x0006F590
	public void ChangeNpcDataID(int iNpcID, int iNewNpcID)
	{
		CharacterData characterData = this.GetCharacterData(iNpcID);
		characterData.iChangeNPCID = iNewNpcID;
		NpcDataNode npcData = Game.NpcData.GetNpcData(iNewNpcID);
		characterData._NpcDataNode = npcData;
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x000713C0 File Offset: 0x0006F5C0
	public string GetImageName(int iID)
	{
		CharacterData characterData = this.GetCharacterData(iID);
		if (characterData == null)
		{
			return Game.NpcData.GetImageName(iID);
		}
		return characterData._NpcDataNode.m_strHalfImage;
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x000713F4 File Offset: 0x0006F5F4
	public string GetNpcName(int iID)
	{
		CharacterData characterData = this.GetCharacterData(iID);
		if (characterData == null)
		{
			return Game.NpcData.GetNpcName(iID);
		}
		return characterData._NpcDataNode.m_strNpcName;
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x00071428 File Offset: 0x0006F628
	public void SetNowPractice(int iNpcID, int iID)
	{
		CharacterData characterData = this.GetCharacterData(iNpcID);
		characterData.NowPractice = iID;
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x00071444 File Offset: 0x0006F644
	public void SetCharacterProperty(int iNpcID, int itype, int iValue)
	{
		CharacterData characterData = this.GetCharacterData(iNpcID);
		characterData.SetValue((CharacterData.PropertyType)itype, iValue);
		characterData.setTotalProperty();
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x0007146C File Offset: 0x0006F66C
	public int GetNPCItemCount(int iNpcID, int Itemid)
	{
		CharacterData characterData = this.GetCharacterData(iNpcID);
		for (int i = 0; i < characterData.Itemlist.Count; i++)
		{
			if (characterData.Itemlist[i].m_iItemID == Itemid)
			{
				return characterData.Itemlist[i].m_iCount;
			}
		}
		return 0;
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x000714C8 File Offset: 0x0006F6C8
	public bool CheckSkill(int iNpcID, int itype)
	{
		CharacterData characterData = this.GetCharacterData(iNpcID);
		return characterData.CheckSkillMax(itype);
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x000714E4 File Offset: 0x0006F6E4
	public void DLC_AddNpcExp(int iNpcid, int iValue)
	{
		CharacterData characterData = this.GetCharacterData(iNpcid);
		if (characterData == null)
		{
			Debug.Log(iNpcid + " Reward error");
			return;
		}
		characterData.DLC_AddExp(iValue);
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x00071520 File Offset: 0x0006F720
	public void SetNPCNowPracticeExp(CharacterData CD, int iValue, bool bNpc = true, int iNpcid = 0)
	{
		if (CD == null)
		{
			CD = this.GetCharacterData(iNpcid);
		}
		if (CD == null)
		{
			Debug.Log(iNpcid + " Reward error");
			return;
		}
		float talentAddExp = CD.GetTalentAddExp();
		iValue = Mathf.FloorToInt((float)iValue * talentAddExp);
		int nowPracticeLevel = CD.GetNowPracticeLevel();
		CD.SetNowPracticeExp(iValue);
		int nowPracticeLevel2 = CD.GetNowPracticeLevel();
		int num = nowPracticeLevel2 - nowPracticeLevel;
		if (num > 0 && !bNpc)
		{
			CD.setTotalProperty();
		}
		if (bNpc)
		{
			if (num > 0)
			{
				CD.setTotalProperty();
			}
			if (nowPracticeLevel2 >= 10)
			{
				CD.GetNextPractice();
				GameDebugTool.Log(CD._NpcDataNode.m_strNpcName + " 目前修練   " + CD.GetNowPracticeName(), Color.yellow);
			}
		}
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x000715E8 File Offset: 0x0006F7E8
	public bool CheckRendomQuest(int iId, string QuestID)
	{
		CharacterData characterData = this.GetCharacterData(iId);
		foreach (string text in characterData.FinishQuestList)
		{
			if (text == QuestID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0007165C File Offset: 0x0006F85C
	public int GetNpcMoney(int iId)
	{
		CharacterData characterData = this.GetCharacterData(iId);
		return characterData.iMoney;
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x00071678 File Offset: 0x0006F878
	public void SetAffterNPCData()
	{
		for (int i = 0; i < this.NpcList.Count; i++)
		{
			if (this.NpcList[i]._NpcDataNode.m_iAffterNpcID != 0)
			{
				int iAffterNpcID = this.NpcList[i]._NpcDataNode.m_iAffterNpcID;
				this.NpcList[i]._NpcDataNode = Game.NpcData.GetNpcData(iAffterNpcID);
			}
		}
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x000716F0 File Offset: 0x0006F8F0
	public void mod_AddNeigong(int NpcId, int iNeigongID, int iLevel)
	{
		CharacterData characterData = this.GetCharacterData(NpcId);
		if (characterData == null)
		{
			Debug.Log("沒有這個NPC  ID:  " + NpcId + string.Empty);
			return;
		}
		NpcNeigong npcNeigong = new NpcNeigong();
		npcNeigong.iSkillID = iNeigongID;
		npcNeigong.SetLv(iLevel);
		npcNeigong.bUse = true;
		characterData.NeigongList.Add(npcNeigong);
		characterData.setTotalProperty();
	}

	// Token: 0x0400100D RID: 4109
	public static NPC m_instance;

	// Token: 0x0400100E RID: 4110
	public List<CharacterData> NpcList = new List<CharacterData>();
}
