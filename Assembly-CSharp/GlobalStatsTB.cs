using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200074D RID: 1869
public class GlobalStatsTB : MonoBehaviour
{
	// Token: 0x14000049 RID: 73
	// (add) Token: 0x06002C76 RID: 11382 RVA: 0x0001CBE6 File Offset: 0x0001ADE6
	// (remove) Token: 0x06002C77 RID: 11383 RVA: 0x0001CBFD File Offset: 0x0001ADFD
	public static event GlobalStatsTB.PlayerPointChangedHandler onPlayerPointChangedE;

	// Token: 0x06002C78 RID: 11384 RVA: 0x0001CC14 File Offset: 0x0001AE14
	public static int GetPlayerFactionCount()
	{
		return GlobalStatsTB.playerFactionCount;
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x0001CC1B File Offset: 0x0001AE1B
	public static void SetPlayerFactionCount(int count)
	{
		GlobalStatsTB.playerFactionCount = count;
	}

	// Token: 0x06002C7A RID: 11386 RVA: 0x0001CC23 File Offset: 0x0001AE23
	public static int GetPlayerPoint()
	{
		return GlobalStatsTB.playerPoint;
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x0015633C File Offset: 0x0015453C
	public static List<UnitTB> GetPlayerUnitList()
	{
		List<UnitTB> list = new List<UnitTB>();
		foreach (UnitTB unitTB in GlobalStatsTB.playerUnitList)
		{
			list.Add(unitTB);
		}
		return list;
	}

	// Token: 0x06002C7C RID: 11388 RVA: 0x0015639C File Offset: 0x0015459C
	public static List<UnitTB> GetTempPlayerUnitList()
	{
		List<UnitTB> list = new List<UnitTB>();
		foreach (UnitTB unitTB in GlobalStatsTB.tempPlayerUnitList)
		{
			list.Add(unitTB);
		}
		return list;
	}

	// Token: 0x06002C7D RID: 11389 RVA: 0x001563FC File Offset: 0x001545FC
	public static void Init()
	{
		if (GlobalStatsTB.loaded)
		{
			return;
		}
		new GameObject
		{
			name = "GlobalStats"
		}.AddComponent<GlobalStatsTB>();
	}

	// Token: 0x06002C7E RID: 11390 RVA: 0x0015642C File Offset: 0x0015462C
	private void Awake()
	{
		GlobalStatsTB.LoadUnit();
		if (this.mode == _Mode.Load)
		{
			this.LoadData();
		}
		else if (this.mode == _Mode.New)
		{
			this.NewData();
		}
		else if (this.mode == _Mode.LoadXNew)
		{
			if (!PlayerPrefs.HasKey("DataExisted"))
			{
				this.NewData();
			}
			else
			{
				this.LoadData();
			}
		}
		GlobalStatsTB.loaded = true;
	}

	// Token: 0x06002C7F RID: 11391 RVA: 0x0001CC2A File Offset: 0x0001AE2A
	public void NewData()
	{
		GlobalStatsTB.playerPoint = GlobalStatsTB.defaultStartingPoint;
		GlobalStatsTB.playerUnitList = new List<UnitTB>();
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x001564A0 File Offset: 0x001546A0
	public void LoadData()
	{
		GlobalStatsTB.playerPoint = PlayerPrefs.GetInt("PlayerPoint", GlobalStatsTB.defaultStartingPoint);
		int @int = PlayerPrefs.GetInt("PlayerUnitCount", 0);
		for (int i = 0; i < @int; i++)
		{
			GlobalStatsTB.playerUnitIDList.Add(PlayerPrefs.GetInt("PlayerUnit" + i.ToString(), 0));
		}
		foreach (int num in GlobalStatsTB.playerUnitIDList)
		{
			foreach (UnitTB unitTB in GlobalStatsTB.unitPrefabList)
			{
				if (unitTB.prefabID == num)
				{
					GlobalStatsTB.playerUnitList.Add(unitTB);
				}
			}
		}
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x001565A4 File Offset: 0x001547A4
	public static void LoadUnit()
	{
		GameObject gameObject = Resources.Load("UnitPrefabList", typeof(GameObject)) as GameObject;
		if (gameObject != null)
		{
			UnitListPrefab component = gameObject.GetComponent<UnitListPrefab>();
			if (component != null)
			{
				GlobalStatsTB.unitPrefabList = component.unitList;
			}
		}
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x0001CC40 File Offset: 0x0001AE40
	public static void GainPlayerPoint(int point)
	{
		GlobalStatsTB.playerPoint += point;
		PlayerPrefs.SetInt("PlayerPoint", GlobalStatsTB.playerPoint);
		GlobalStatsTB.SetDataExistedFlag();
		if (GlobalStatsTB.onPlayerPointChangedE != null)
		{
			GlobalStatsTB.onPlayerPointChangedE(point);
		}
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x0001CC77 File Offset: 0x0001AE77
	public static void SetPlayerPoint()
	{
		PlayerPrefs.SetInt("PlayerPoint", GlobalStatsTB.playerPoint);
		GlobalStatsTB.SetDataExistedFlag();
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x0001CC8D File Offset: 0x0001AE8D
	public static void SetPlayerPoint(int point)
	{
		GlobalStatsTB.playerPoint = point;
		PlayerPrefs.SetInt("PlayerPoint", GlobalStatsTB.playerPoint);
		GlobalStatsTB.SetDataExistedFlag();
	}

	// Token: 0x06002C85 RID: 11397 RVA: 0x001565F8 File Offset: 0x001547F8
	public static void SetPlayerUnitList(List<UnitTB> list)
	{
		GlobalStatsTB.playerUnitList = list;
		PlayerPrefs.SetInt("PlayerUnitCount", GlobalStatsTB.playerUnitList.Count);
		for (int i = 0; i < GlobalStatsTB.playerUnitList.Count; i++)
		{
			PlayerPrefs.SetInt("PlayerUnit" + i.ToString(), GlobalStatsTB.playerUnitList[i].prefabID);
		}
		GlobalStatsTB.SetDataExistedFlag();
	}

	// Token: 0x06002C86 RID: 11398 RVA: 0x0001CCA9 File Offset: 0x0001AEA9
	public static void SetTempPlayerUnitList(List<UnitTB> list)
	{
		GlobalStatsTB.tempPlayerUnitList = list;
	}

	// Token: 0x06002C87 RID: 11399 RVA: 0x00156668 File Offset: 0x00154868
	public static void SetTempPlayerUnitList(List<PlayerUnits> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (i == 0)
			{
				GlobalStatsTB.tempPlayerUnitList = list[i].starting;
			}
			else if (i == 1)
			{
				GlobalStatsTB.tempPlayerUnitList1 = list[i].starting;
			}
			else if (i == 2)
			{
				GlobalStatsTB.tempPlayerUnitList2 = list[i].starting;
			}
			else if (i == 3)
			{
				GlobalStatsTB.tempPlayerUnitList3 = list[i].starting;
			}
			else if (i == 4)
			{
				GlobalStatsTB.tempPlayerUnitList4 = list[i].starting;
			}
		}
	}

	// Token: 0x06002C88 RID: 11400 RVA: 0x00156718 File Offset: 0x00154918
	public static List<UnitTB> GetTempPlayerUnitList(int ID)
	{
		if (ID == 0)
		{
			return GlobalStatsTB.tempPlayerUnitList;
		}
		if (ID == 1)
		{
			return GlobalStatsTB.tempPlayerUnitList1;
		}
		if (ID == 2)
		{
			return GlobalStatsTB.tempPlayerUnitList2;
		}
		if (ID == 3)
		{
			return GlobalStatsTB.tempPlayerUnitList3;
		}
		if (ID == 4)
		{
			return GlobalStatsTB.tempPlayerUnitList4;
		}
		return GlobalStatsTB.tempPlayerUnitList;
	}

	// Token: 0x06002C89 RID: 11401 RVA: 0x0001CCB1 File Offset: 0x0001AEB1
	public static void SetDataExistedFlag()
	{
		if (!PlayerPrefs.HasKey("DataExisted"))
		{
			PlayerPrefs.SetInt("DataExisted", 1);
		}
	}

	// Token: 0x06002C8A RID: 11402 RVA: 0x0001CCCD File Offset: 0x0001AECD
	public static void ResetAll()
	{
		PlayerPrefs.DeleteAll();
		GlobalStatsTB.playerPoint = GlobalStatsTB.defaultStartingPoint;
		GlobalStatsTB.playerUnitList = new List<UnitTB>();
		GlobalStatsTB.playerUnitIDList = new List<int>();
	}

	// Token: 0x040038EB RID: 14571
	public _Mode mode = _Mode.Load;

	// Token: 0x040038EC RID: 14572
	public static bool loaded = false;

	// Token: 0x040038ED RID: 14573
	public static int playerPoint = 20;

	// Token: 0x040038EE RID: 14574
	public static List<int> playerUnitIDList = new List<int>();

	// Token: 0x040038EF RID: 14575
	public static List<UnitTB> unitPrefabList = new List<UnitTB>();

	// Token: 0x040038F0 RID: 14576
	public static List<UnitTB> playerUnitList = new List<UnitTB>();

	// Token: 0x040038F1 RID: 14577
	public static List<UnitTB> tempPlayerUnitList = new List<UnitTB>();

	// Token: 0x040038F2 RID: 14578
	public static List<UnitTB> tempPlayerUnitList1 = new List<UnitTB>();

	// Token: 0x040038F3 RID: 14579
	public static List<UnitTB> tempPlayerUnitList2 = new List<UnitTB>();

	// Token: 0x040038F4 RID: 14580
	public static List<UnitTB> tempPlayerUnitList3 = new List<UnitTB>();

	// Token: 0x040038F5 RID: 14581
	public static List<UnitTB> tempPlayerUnitList4 = new List<UnitTB>();

	// Token: 0x040038F6 RID: 14582
	public static int playerFactionCount = 1;

	// Token: 0x040038F7 RID: 14583
	private static int defaultStartingPoint = 35;

	// Token: 0x0200074E RID: 1870
	// (Invoke) Token: 0x06002C8C RID: 11404
	public delegate void PlayerPointChangedHandler(int point);
}
