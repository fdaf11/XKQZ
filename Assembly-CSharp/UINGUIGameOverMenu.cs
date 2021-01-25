using System;
using System.Collections;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x020007C9 RID: 1993
public class UINGUIGameOverMenu : MonoBehaviour
{
	// Token: 0x0600310B RID: 12555 RVA: 0x0017B834 File Offset: 0x00179A34
	private void Awake()
	{
		this.thisObj = base.gameObject;
		base.transform.localPosition = Vector3.zero;
		UIAnchor[] componentsInChildren = this.thisObj.GetComponentsInChildren<UIAnchor>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		Utility.SetActive(this.thisObj, false);
	}

	// Token: 0x0600310C RID: 12556 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x0600310D RID: 12557 RVA: 0x0001EF16 File Offset: 0x0001D116
	public void OnNextButton()
	{
		Time.timeScale = 1f;
		GameControlTB.LoadNextScene();
	}

	// Token: 0x0600310E RID: 12558 RVA: 0x0017B894 File Offset: 0x00179A94
	private void DLCMenuButton()
	{
		this.iCharCount = 0;
		int dlclevelID = BattleControl.instance.GetDLCLevelID();
		MissionLevelNode missionLevelNode = Game.MissionLeveData.GetMissionLevelNode(dlclevelID);
		int num = missionLevelNode.iMoney;
		int talentValue = TeamStatus.m_Instance.GetTalentValue(TalentEffect.MoreExperiences, true);
		float num2 = 1f + TeamStatus.m_Instance.GetTeamTalentPercentValue(TalentEffect.MoreExperiences, true);
		int num3 = 0;
		float num4 = 1f * (float)missionLevelNode.iExp;
		if (this.iVictoryFactionID == 0)
		{
			num4 = (num4 + (float)talentValue) * num2;
		}
		else
		{
			num4 = 0f;
		}
		int num5 = 0;
		while (num5 < BattleControl.instance.m_teamExploitsList.Count && num3 < this.tmGroup.Length)
		{
			CharacterData characterData = NPC.m_instance.GetCharacterData(BattleControl.instance.m_teamExploitsList[num5].iUnitID);
			if (characterData.NpcType == eNPCType.Teammate)
			{
				this.tmGroup[num3].gameObject.SetActive(true);
				int iValue = Mathf.RoundToInt(num4);
				Debug.Log(characterData._NpcDataNode.m_strNpcName + " 戰鬥經驗上升 " + iValue.ToString());
				this.tmGroup[num3].SetCharacterDataDLC(characterData, iValue);
				this.iCharCount++;
				num3++;
			}
			num5++;
		}
		BattleAreaNode battleAreaNode = BattleControl.instance.m_battleArea.GetBattleAreaNode(BattleControl.instance.iLastBattleID);
		List<int> list = new List<int>();
		if (this.iVictoryFactionID == 0)
		{
			list.AddRange(missionLevelNode.m_ItemList.ToArray());
			for (int i = 0; i < battleAreaNode.m_DropItemList.Count; i++)
			{
				int num6 = Random.Range(0, 100);
				if (battleAreaNode.m_DropItemList[i].DropRate > num6)
				{
					list.Add(battleAreaNode.m_DropItemList[i].ItemID);
				}
			}
		}
		else
		{
			num = 0;
		}
		for (int j = 0; j < this.sprItemGroup.Length; j++)
		{
			this.sprItemGroup[j].gameObject.SetActive(false);
		}
		int num7 = 0;
		if (num > 0)
		{
			num7 = 1;
		}
		for (int k = 0; k < list.Count; k++)
		{
			this.sprItemGroup[k].gameObject.SetActive(true);
			if (list.Count + num7 <= 4)
			{
				this.sprItemGroup[k].gameObject.transform.localPosition = new Vector3(-660f + (float)k * 300f, -440f);
			}
			else if (k < 4)
			{
				this.sprItemGroup[k].gameObject.transform.localPosition = new Vector3(-660f + (float)k * 300f, -400f);
			}
			else
			{
				this.sprItemGroup[k].gameObject.transform.localPosition = new Vector3(-660f + (float)(k - 4) * 300f, -480f);
			}
			this.sprItemGroup[k].gameObject.GetComponentInChildren<UILabel>().text = Game.StringTable.GetString(110040);
			ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(list[k]);
			if (itemDataNode != null)
			{
				int num8 = itemDataNode.m_iItemType - 1;
				this.sprItemGroup[k].spriteName = "ui_ItemType" + num8.ToString();
				this.sprItemGroup[k].gameObject.GetComponentInChildren<UILabel>().text = itemDataNode.m_strItemName;
				BackpackStatus.m_Instance.AddPackItem(list[k], 1, true);
			}
		}
		if (this.iVictoryFactionID == 0 && list.Count < 8 && num > 0)
		{
			int count = list.Count;
			this.sprItemGroup[count].gameObject.SetActive(true);
			if (count < 4)
			{
				this.sprItemGroup[count].gameObject.transform.localPosition = new Vector3(-660f + (float)count * 300f, -440f);
			}
			else
			{
				this.sprItemGroup[count].gameObject.transform.localPosition = new Vector3(-660f + (float)(count - 4) * 300f, -480f);
			}
			this.sprItemGroup[count].spriteName = "ui_thing_024";
			this.sprItemGroup[count].gameObject.GetComponentInChildren<UILabel>().text = num.ToString() + Game.StringTable.GetString(110023);
			BackpackStatus.m_Instance.ChangeMoney(num);
		}
		if (this.iVictoryFactionID == 0)
		{
			TeamStatus.m_Instance.AddPrestigePoints(missionLevelNode.iFame);
		}
		base.StartCoroutine(this.Wait1Sec());
	}

	// Token: 0x0600310F RID: 12559 RVA: 0x0017BD80 File Offset: 0x00179F80
	public void OnMenuButton()
	{
		Time.timeScale = 1f;
		int num = 0;
		Utility.SetActive(this.scoreObj, true);
		Utility.SetActive(this.loseObj, false);
		Utility.SetActive(this.winObj, false);
		Utility.SetActive(this.endObj, false);
		this.backgroundObj.collider.enabled = false;
		Utility.SetActive(this.backgroundObj, false);
		List<int> list = new List<int>();
		for (int i = 0; i < this.tmGroup.Length; i++)
		{
			this.tmGroup[i].gameObject.SetActive(false);
		}
		if (GameGlobal.m_bDLCMode)
		{
			this.DLCMenuButton();
			return;
		}
		this.iCharCount = 0;
		for (int j = 0; j < BattleControl.instance.m_enemyExploitsList.Count; j++)
		{
			CharacterData characterData = NPC.m_instance.GetCharacterData(BattleControl.instance.m_enemyExploitsList[j].iUnitID);
			if (characterData != null)
			{
				float num2 = BattleControl.instance.m_enemyExploitsList[j].fValue;
				if (this.iVictoryFactionID == 0)
				{
					num2 = 0.2f * num2;
					if (characterData.RoutineList.Count > 0)
					{
						int num3 = characterData.RoutineList[0].m_Routine.m_iRequestSP;
						int iDamage = characterData.RoutineList[0].m_Routine.m_iDamage;
						float num4;
						if (num3 < iDamage)
						{
							num4 = Random.Range(0.005f * (float)num3, 0.005f * (float)iDamage) * (float)characterData.iMoney;
						}
						else
						{
							num4 = Random.Range(0.005f * (float)iDamage, 0.005f * (float)num3) * (float)characterData.iMoney;
						}
						num3 = Mathf.RoundToInt(num4);
						characterData.LessMoney(num3);
						num += num3;
					}
				}
				if (characterData.GetNpcItemCount() > 0)
				{
					list.Add(characterData.iNpcID);
				}
				int nowPracticeExp = Mathf.RoundToInt(num2);
				Debug.Log(characterData._NpcDataNode.m_strNpcName + " 戰鬥經驗上升 " + nowPracticeExp.ToString());
				characterData.SetNowPracticeExp(nowPracticeExp);
			}
		}
		int num5 = 0;
		int talentValue = TeamStatus.m_Instance.GetTalentValue(TalentEffect.MoreExperiences, true);
		float num6 = 1f + TeamStatus.m_Instance.GetTeamTalentPercentValue(TalentEffect.MoreExperiences, true);
		for (int k = 0; k < BattleControl.instance.m_teamExploitsList.Count; k++)
		{
			CharacterData characterData2 = NPC.m_instance.GetCharacterData(BattleControl.instance.m_teamExploitsList[k].iUnitID);
			if (characterData2.NpcType == eNPCType.Teammate)
			{
				this.tmGroup[num5].gameObject.SetActive(true);
				float num7 = BattleControl.instance.m_teamExploitsList[k].fValue;
				num7 = (num7 + (float)talentValue) * num6;
				if (this.iVictoryFactionID != 0)
				{
					num7 = 0.2f * num7;
				}
				int iValue = Mathf.RoundToInt(num7);
				Debug.Log(characterData2._NpcDataNode.m_strNpcName + " 戰鬥經驗上升 " + iValue.ToString());
				this.tmGroup[num5].SetCharacterData(characterData2, iValue);
				this.iCharCount++;
				num5++;
			}
			else
			{
				float num8 = BattleControl.instance.m_teamExploitsList[k].fValue;
				if (this.iVictoryFactionID != 0)
				{
					num8 = 0.2f * num8;
				}
				int nowPracticeExp2 = Mathf.RoundToInt(num8);
				Debug.Log(characterData2._NpcDataNode.m_strNpcName + " 戰鬥經驗上升 " + nowPracticeExp2.ToString());
				characterData2.SetNowPracticeExp(nowPracticeExp2);
			}
		}
		num /= 10;
		BattleAreaNode battleAreaNode = BattleControl.instance.m_battleArea.GetBattleAreaNode(BattleControl.instance.iLastBattleID);
		if (BattleControl.instance.iLastBattleID == 88000001)
		{
			int num9 = GameGlobal.mod_Difficulty * GameGlobal.mod_NewBattleEnemyCount;
			num = Random.Range(50, 101) * num9;
			this.mod_RewardNewBattle(ref battleAreaNode);
		}
		if (BattleControl.instance.iLastBattleID == 88000002)
		{
			int num10 = GameGlobal.mod_Difficulty * GameGlobal.mod_BossBattleEnemyCount;
			num = Random.Range(200, 301) * num10;
			this.mod_RewardBossBattle(ref battleAreaNode);
		}
		if (BattleControl.instance.iLastBattleID == 88000003)
		{
			int num11 = GameGlobal.mod_Difficulty * GameGlobal.mod_RandomBattleEnemyCount;
			num = Random.Range(100, 201) * num11;
			this.mod_RewardRandomBattle(ref battleAreaNode);
		}
		if (BattleControl.instance.iLastBattleID == 88000004)
		{
			int num12 = GameGlobal.mod_Difficulty * GameGlobal.mod_Difficulty * GameGlobal.mod_ShilianEnemyCount * GameGlobal.mod_ShilianLayer;
			num = Random.Range(10, 21) * num12;
			this.mod_RewardShilianBattle(ref battleAreaNode);
		}
		List<int> list2 = new List<int>();
		if (this.iVictoryFactionID == 0)
		{
			for (int l = 0; l < battleAreaNode.m_DropItemList.Count; l++)
			{
				int num13 = Random.Range(0, 100);
				if (battleAreaNode.m_DropItemList[l].DropRate > num13)
				{
					if (battleAreaNode.m_DropItemList[l].ItemID == 999999)
					{
						if (list.Count > 0)
						{
							int num14 = Random.Range(0, list.Count);
							CharacterData characterData3 = NPC.m_instance.GetCharacterData(list[num14]);
							if (characterData3 != null)
							{
								int npcItemCount = characterData3.GetNpcItemCount();
								if (npcItemCount > 0)
								{
									int index = Random.Range(0, npcItemCount);
									int npcItemIndexID = characterData3.GetNpcItemIndexID(index);
									list2.Add(npcItemIndexID);
									characterData3.LessNpcItem(npcItemIndexID, 1);
									list.RemoveAt(num14);
								}
							}
						}
					}
					else
					{
						list2.Add(battleAreaNode.m_DropItemList[l].ItemID);
					}
				}
			}
		}
		for (int m = 0; m < this.sprItemGroup.Length; m++)
		{
			this.sprItemGroup[m].gameObject.SetActive(false);
		}
		int num15 = 0;
		if (num > 0)
		{
			num15 = 1;
		}
		for (int n = 0; n < list2.Count; n++)
		{
			this.sprItemGroup[n].gameObject.SetActive(true);
			if (list2.Count + num15 <= 4)
			{
				this.sprItemGroup[n].gameObject.transform.localPosition = new Vector3(-660f + (float)n * 300f, -440f);
			}
			else if (n < 4)
			{
				this.sprItemGroup[n].gameObject.transform.localPosition = new Vector3(-660f + (float)n * 300f, -400f);
			}
			else
			{
				this.sprItemGroup[n].gameObject.transform.localPosition = new Vector3(-660f + (float)(n - 4) * 300f, -480f);
			}
			this.sprItemGroup[n].gameObject.GetComponentInChildren<UILabel>().text = Game.StringTable.GetString(110040);
			ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(list2[n]);
			if (itemDataNode != null)
			{
				int num16 = itemDataNode.m_iItemType - 1;
				this.sprItemGroup[n].spriteName = "ui_ItemType" + num16.ToString();
				this.sprItemGroup[n].gameObject.GetComponentInChildren<UILabel>().text = itemDataNode.m_strItemName;
				BackpackStatus.m_Instance.AddPackItem(list2[n], 1, true);
			}
		}
		if (this.iVictoryFactionID == 0 && list2.Count < 8 && num > 0)
		{
			int count = list2.Count;
			this.sprItemGroup[count].gameObject.SetActive(true);
			if (count < 4)
			{
				this.sprItemGroup[count].gameObject.transform.localPosition = new Vector3(-660f + (float)count * 300f, -440f);
			}
			else
			{
				this.sprItemGroup[count].gameObject.transform.localPosition = new Vector3(-660f + (float)(count - 4) * 300f, -480f);
			}
			this.sprItemGroup[count].spriteName = "ui_thing_024";
			this.sprItemGroup[count].gameObject.GetComponentInChildren<UILabel>().text = num.ToString() + Game.StringTable.GetString(110023);
			BackpackStatus.m_Instance.ChangeMoney(num);
		}
		base.StartCoroutine(this.Wait1Sec());
	}

	// Token: 0x06003110 RID: 12560 RVA: 0x0001EF27 File Offset: 0x0001D127
	public void CharDone()
	{
		if (this.iCharCount > 0)
		{
			this.iCharCount--;
		}
	}

	// Token: 0x06003111 RID: 12561 RVA: 0x0017C5C0 File Offset: 0x0017A7C0
	public void OnBillingButton()
	{
		Time.timeScale = 1f;
		Utility.SetActive(this.loseObj, true);
		Utility.SetActive(this.winObj, true);
		Utility.SetActive(this.endObj, true);
		this.backgroundObj.collider.enabled = false;
		Utility.SetActive(this.scoreObj, false);
		Utility.SetActive(this.thisObj, false);
		this.backgroundObj2.collider.enabled = false;
		Game.g_BattleControl.EndBattle(this.iVictoryFactionID);
	}

	// Token: 0x06003112 RID: 12562 RVA: 0x0001EF43 File Offset: 0x0001D143
	public void OnRestartButton()
	{
		Time.timeScale = 1f;
		Application.LoadLevel(Application.loadedLevelName);
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x0017C648 File Offset: 0x0017A848
	public void Show(int vicFactionID)
	{
		this.iVictoryFactionID = vicFactionID;
		Utility.SetActive(this.thisObj, true);
		if (GameControlTB.playerFactionExisted)
		{
			if (GameControlTB.IsHotSeatMode())
			{
				if (GameControlTB.IsPlayerFaction(vicFactionID))
				{
					base.StartCoroutine(this.VictoryResult(true));
				}
				else
				{
					base.StartCoroutine(this.VictoryResult(false));
				}
			}
			else if (GameControlTB.IsPlayerFaction(vicFactionID))
			{
				base.StartCoroutine(this.VictoryResult(true));
			}
			else
			{
				base.StartCoroutine(this.VictoryResult(false));
			}
		}
		else
		{
			base.StartCoroutine(this.EndResult());
		}
		CameraControl.Disable();
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x0000264F File Offset: 0x0000084F
	public void Hide()
	{
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x0017C6F0 File Offset: 0x0017A8F0
	private IEnumerator VictoryResult(bool bWin)
	{
		yield return null;
		Utility.SetActive(this.thisObj, true);
		Utility.SetActive(this.backgroundObj, true);
		if (bWin)
		{
			this.lbReward.gameObject.SetActive(false);
			Utility.SetActive(this.loseObj, false);
			Utility.SetActive(this.endObj, false);
		}
		else
		{
			this.lbReward.gameObject.SetActive(false);
			Utility.SetActive(this.winObj, false);
			Utility.SetActive(this.endObj, false);
		}
		this.backgroundObj.collider.enabled = false;
		this.backgroundObj.GetComponent<UIButton>().tweenTarget = null;
		Color color = new Color(1f, 1f, 1f, 1f);
		TweenColor.Begin(this.backgroundObj, 0.5f, color);
		yield return new WaitForSeconds(0.8f);
		this.backgroundObj.collider.enabled = true;
		yield break;
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x0017C71C File Offset: 0x0017A91C
	private IEnumerator EndResult()
	{
		yield return null;
		Utility.SetActive(this.thisObj, true);
		Utility.SetActive(this.backgroundObj, true);
		this.lbReward.gameObject.SetActive(false);
		Utility.SetActive(this.winObj, false);
		Utility.SetActive(this.loseObj, false);
		this.backgroundObj.collider.enabled = false;
		this.backgroundObj.GetComponent<UIButton>().tweenTarget = null;
		Color color = new Color(1f, 1f, 1f, 1f);
		TweenColor.Begin(this.backgroundObj, 0.5f, color);
		yield return new WaitForSeconds(0.8f);
		this.backgroundObj.collider.enabled = true;
		yield break;
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x0017C738 File Offset: 0x0017A938
	private IEnumerator Wait1Sec()
	{
		yield return null;
		while (this.iCharCount > 0)
		{
			yield return null;
		}
		this.backgroundObj2.collider.enabled = true;
		yield break;
	}

	// Token: 0x06003118 RID: 12568 RVA: 0x0001EF59 File Offset: 0x0001D159
	private void OnEnable()
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown += new Action<KeyControl.Key>(this.OnKeyDown);
		}
	}

	// Token: 0x06003119 RID: 12569 RVA: 0x0001EF81 File Offset: 0x0001D181
	private void OnDisable()
	{
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown -= new Action<KeyControl.Key>(this.OnKeyDown);
		}
	}

	// Token: 0x0600311A RID: 12570 RVA: 0x0017C754 File Offset: 0x0017A954
	private void OnKeyDown(KeyControl.Key keyCode)
	{
		if (UINGUI.instance.battleControlState != BattleControlState.EndBattle)
		{
			return;
		}
		if (keyCode == KeyControl.Key.OK)
		{
			this.KeyDownCloseForm();
		}
	}

	// Token: 0x0600311B RID: 12571 RVA: 0x0017C78C File Offset: 0x0017A98C
	private void KeyDownCloseForm()
	{
		if (this.backgroundObj2.collider.enabled)
		{
			UINGUI.instance.DelayBattleControlState(BattleControlState.None);
			this.OnBillingButton();
		}
		else if (this.backgroundObj.collider.enabled)
		{
			this.OnMenuButton();
		}
	}

	// Token: 0x0600311C RID: 12572 RVA: 0x0017C7E0 File Offset: 0x0017A9E0
	public void mod_RewardNewBattle(ref BattleAreaNode battleAreaNode)
	{
		battleAreaNode.m_DropItemList.Clear();
		List<int> list = new List<int>();
		List<ItemDataNode> itemList = Game.ItemData.GetItemList();
		for (int i = 0; i < itemList.Count; i++)
		{
			list.Add(itemList[i].m_iItemID);
		}
		int num = GameGlobal.mod_NewBattleEnemyCount / 3;
		for (int j = 0; j < num; j++)
		{
			int num2 = Random.Range(0, list.Count);
			DropItem dropItem = new DropItem();
			dropItem.ItemID = list[num2];
			dropItem.DropRate = Random.Range(1, 101);
			battleAreaNode.m_DropItemList.Add(dropItem);
		}
	}

	// Token: 0x0600311D RID: 12573 RVA: 0x0017C888 File Offset: 0x0017AA88
	public void mod_RewardBossBattle(ref BattleAreaNode battleAreaNode)
	{
		battleAreaNode.m_DropItemList.Clear();
		List<int> list = new List<int>();
		List<ItemDataNode> itemList = Game.ItemData.GetItemList();
		for (int i = 0; i < itemList.Count; i++)
		{
			list.Add(itemList[i].m_iItemID);
		}
		int num = GameGlobal.mod_BossBattleEnemyCount / 3 * 2;
		for (int j = 0; j < num; j++)
		{
			int num2 = Random.Range(0, list.Count);
			DropItem dropItem = new DropItem();
			dropItem.ItemID = list[num2];
			dropItem.DropRate = Random.Range(50, 101);
			battleAreaNode.m_DropItemList.Add(dropItem);
		}
	}

	// Token: 0x0600311E RID: 12574 RVA: 0x0017C934 File Offset: 0x0017AB34
	public void mod_RewardRandomBattle(ref BattleAreaNode battleAreaNode)
	{
		battleAreaNode.m_DropItemList.Clear();
		List<int> list = new List<int>();
		List<ItemDataNode> itemList = Game.ItemData.GetItemList();
		for (int i = 0; i < itemList.Count; i++)
		{
			list.Add(itemList[i].m_iItemID);
		}
		int num = GameGlobal.mod_RandomBattleEnemyCount / 3;
		for (int j = 0; j < num; j++)
		{
			int num2 = Random.Range(0, list.Count);
			DropItem dropItem = new DropItem();
			dropItem.ItemID = list[num2];
			dropItem.DropRate = Random.Range(35, 101);
			battleAreaNode.m_DropItemList.Add(dropItem);
		}
	}

	// Token: 0x0600311F RID: 12575 RVA: 0x0017C9DC File Offset: 0x0017ABDC
	public void mod_RewardShilianBattle(ref BattleAreaNode battleAreaNode)
	{
		battleAreaNode.m_DropItemList.Clear();
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		List<ItemDataNode> itemList = Game.ItemData.GetItemList();
		for (int i = 0; i < itemList.Count; i++)
		{
			if (itemList[i].m_iItemID < 810000)
			{
				list.Add(itemList[i].m_iItemID);
			}
			if (itemList[i].m_iItemID > 810000 && itemList[i].m_iItemID < 810118)
			{
				list2.Add(itemList[i].m_iItemID);
			}
			if (itemList[i].m_iItemID >= 810118)
			{
				list3.Add(itemList[i].m_iItemID);
			}
		}
		int num = GameGlobal.mod_ShilianEnemyCount * GameGlobal.mod_ShilianLayer * GameGlobal.mod_Difficulty * GameGlobal.mod_Difficulty;
		int num2 = GameGlobal.mod_Difficulty / 2 + 1;
		for (int j = 0; j < num2; j++)
		{
			int num3 = Random.Range(0, list.Count);
			DropItem dropItem = new DropItem();
			dropItem.ItemID = list[num3];
			dropItem.DropRate = Random.Range(50, 101);
			battleAreaNode.m_DropItemList.Add(dropItem);
		}
		if (Random.Range(0f, 1f) > (float)num / 2000f)
		{
			int num4 = Random.Range(0, list2.Count);
			DropItem dropItem2 = new DropItem();
			dropItem2.ItemID = list2[num4];
			dropItem2.DropRate = 100;
			battleAreaNode.m_DropItemList.Add(dropItem2);
		}
		if (Random.Range(0f, 1f) > (float)num / 2000f)
		{
			int num5 = Random.Range(0, list3.Count);
			DropItem dropItem3 = new DropItem();
			dropItem3.ItemID = list3[num5];
			dropItem3.DropRate = 100;
			battleAreaNode.m_DropItemList.Add(dropItem3);
		}
	}

	// Token: 0x04003CA3 RID: 15523
	private GameObject thisObj;

	// Token: 0x04003CA4 RID: 15524
	private int iVictoryFactionID;

	// Token: 0x04003CA5 RID: 15525
	private int iCharCount;

	// Token: 0x04003CA6 RID: 15526
	public GameObject scoreObj;

	// Token: 0x04003CA7 RID: 15527
	public UITeamMember[] tmGroup;

	// Token: 0x04003CA8 RID: 15528
	public UISprite[] sprItemGroup;

	// Token: 0x04003CA9 RID: 15529
	public GameObject backgroundObj2;

	// Token: 0x04003CAA RID: 15530
	public GameObject backgroundObj;

	// Token: 0x04003CAB RID: 15531
	public GameObject winObj;

	// Token: 0x04003CAC RID: 15532
	public GameObject loseObj;

	// Token: 0x04003CAD RID: 15533
	public GameObject endObj;

	// Token: 0x04003CAE RID: 15534
	public UILabel lbReward;
}
