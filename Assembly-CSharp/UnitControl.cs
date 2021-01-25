using System;
using System.Collections;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000773 RID: 1907
public class UnitControl : MonoBehaviour
{
	// Token: 0x1400004E RID: 78
	// (add) Token: 0x06002D6B RID: 11627 RVA: 0x0001D3E2 File Offset: 0x0001B5E2
	// (remove) Token: 0x06002D6C RID: 11628 RVA: 0x0001D3F9 File Offset: 0x0001B5F9
	public static event UnitControl.NewFactionPlacementHandler onNewPlacementE;

	// Token: 0x1400004F RID: 79
	// (add) Token: 0x06002D6D RID: 11629 RVA: 0x0001D410 File Offset: 0x0001B610
	// (remove) Token: 0x06002D6E RID: 11630 RVA: 0x0001D427 File Offset: 0x0001B627
	public static event UnitControl.PlacementUpdateHandler onPlacementUpdateE;

	// Token: 0x14000050 RID: 80
	// (add) Token: 0x06002D6F RID: 11631 RVA: 0x0001D43E File Offset: 0x0001B63E
	// (remove) Token: 0x06002D70 RID: 11632 RVA: 0x0001D455 File Offset: 0x0001B655
	public static event UnitControl.NewUnitInRuntimeHandler onNewUnitInRuntimeE;

	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06002D71 RID: 11633 RVA: 0x0001D46C File Offset: 0x0001B66C
	// (remove) Token: 0x06002D72 RID: 11634 RVA: 0x0001D483 File Offset: 0x0001B683
	public static event UnitControl.FactionDefeatedHandler onFactionDefeatedE;

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06002D73 RID: 11635 RVA: 0x0001D49A File Offset: 0x0001B69A
	// (remove) Token: 0x06002D74 RID: 11636 RVA: 0x0001D4B1 File Offset: 0x0001B6B1
	public static event UnitControl.UnitFactionChangedHandler onUnitFactionChangedE;

	// Token: 0x06002D75 RID: 11637 RVA: 0x0001D4C8 File Offset: 0x0001B6C8
	public static bool HideUnitWhenKilled()
	{
		return UnitControl.instance.hideUnitWhenKilled;
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x0001D4D4 File Offset: 0x0001B6D4
	public static bool DestroyUnitObject()
	{
		return UnitControl.instance.destroyUnitObject;
	}

	// Token: 0x06002D77 RID: 11639 RVA: 0x0015E578 File Offset: 0x0015C778
	private void Awake()
	{
		UnitControl.instance = this;
		this.bInitFaction = false;
		List<Tile> allTiles = GridManager.GetAllTiles();
		foreach (Tile tile in allTiles)
		{
			if (tile.unit != null)
			{
				UnitTB unit = tile.unit;
				this.allUnits.Add(unit);
			}
		}
		UnitControl.currentUnitTurnID = -1;
	}

	// Token: 0x06002D78 RID: 11640 RVA: 0x0015E604 File Offset: 0x0015C804
	private void Start()
	{
		if (this.playerUnitsList == null || this.playerUnitsList.Count == 0)
		{
			this.playerUnitsList = new List<PlayerUnits>();
			this.playerUnitsList.Add(new PlayerUnits(GameControlTB.instance.playerFactionID[0]));
		}
		for (int i = 0; i < this.playerUnitsList.Count; i++)
		{
			PlayerUnits playerUnits = this.playerUnitsList[i];
			for (int j = 0; j < playerUnits.starting.Count; j++)
			{
				if (playerUnits.starting[j] != null)
				{
					playerUnits.starting[j].gameObject.transform.position = new Vector3(0f, 99999f, 0f);
				}
			}
		}
		for (int k = 0; k < this.playerUnitsList.Count; k++)
		{
			PlayerUnits playerUnits2 = this.playerUnitsList[k];
			for (int l = 0; l < playerUnits2.starting.Count; l++)
			{
				if (playerUnits2.starting[l] == null)
				{
					playerUnits2.starting.RemoveAt(l);
					l--;
				}
			}
		}
		if (this.playerUnitsList != null && this.playerUnitsList.Count > 0 && this.playerUnitsList[0].starting.Count > 0)
		{
			if (GameControlTB.EnableUnitPlacement())
			{
				if (UnitControl.onNewPlacementE != null)
				{
					UnitControl.onNewPlacementE(this.playerUnitsList[this.facPlacementID]);
				}
				if (UnitControl.onPlacementUpdateE != null)
				{
					UnitControl.onPlacementUpdateE();
				}
			}
			else
			{
				this._AutoPlaceUnit();
				base.StartCoroutine(this.UnitPlacementCompleted());
			}
		}
		else
		{
			base.StartCoroutine(this.UnitPlacementCompleted());
		}
	}

	// Token: 0x06002D79 RID: 11641 RVA: 0x0015E800 File Offset: 0x0015CA00
	private IEnumerator UnitPlacementCompleted()
	{
		yield return null;
		GameControlTB.UnitPlacementCompleted();
		yield break;
	}

	// Token: 0x06002D7A RID: 11642 RVA: 0x0015E814 File Offset: 0x0015CA14
	private void OnEnable()
	{
		GameControlTB.onBattleStartRealE += this.OnBattleStart;
		GameControlTB.onNewRoundE += this.OnNewRound;
		GameControlTB.onNextTurnE += this.OnNextTurn;
		UnitTB.onUnitSelectedE += this.OnUnitSelected;
		UnitTB.onUnitDeselectedE += this.OnUnitDeselected;
		UnitTB.onUnitDestroyedE += this.OnUnitDestroyed;
	}

	// Token: 0x06002D7B RID: 11643 RVA: 0x0015E888 File Offset: 0x0015CA88
	private void OnDisable()
	{
		GameControlTB.onBattleStartRealE -= this.OnBattleStart;
		GameControlTB.onNewRoundE -= this.OnNewRound;
		GameControlTB.onNextTurnE -= this.OnNextTurn;
		UnitTB.onUnitSelectedE -= this.OnUnitSelected;
		UnitTB.onUnitDeselectedE -= this.OnUnitDeselected;
		UnitTB.onUnitDestroyedE -= this.OnUnitDestroyed;
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x0001D4E0 File Offset: 0x0001B6E0
	private void OnUnitSelected(UnitTB sUnit)
	{
		UnitControl.selectedUnit = sUnit;
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x0001D4E8 File Offset: 0x0001B6E8
	private void OnUnitDeselected()
	{
		UnitControl.selectedUnit = null;
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06002D7F RID: 11647 RVA: 0x0001D4F0 File Offset: 0x0001B6F0
	private void OnBattleStart()
	{
		this.InitFaction();
		GameControlTB.OnNewRound();
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x0015E8FC File Offset: 0x0015CAFC
	private void OnNewRound(int roundCounter)
	{
		this.CheckJoinUnit(roundCounter);
		for (int i = 0; i < this.allFactionList.Count; i++)
		{
			if (this.allFactionList[i].allUnitList.Count > 0)
			{
				this.allFactionList[i].bFirstAction = true;
				this.allFactionList[i].allUnitMoved = false;
			}
		}
		UnitControl.ResetFactionUnitMoveList();
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x0001D4FD File Offset: 0x0001B6FD
	private void OnNextTurn()
	{
		base.StartCoroutine(this._OnNextTurn());
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x0015E974 File Offset: 0x0015CB74
	private IEnumerator _OnNextTurn()
	{
		yield return null;
		UnitControl.bTauntFlee = true;
		AIManager.TauntFlee(GameControlTB.turnID);
		while (UnitControl.bTauntFlee)
		{
			yield return null;
		}
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		yield return null;
		if (!GameControlTB.battleEnded)
		{
			if (GameControlTB.IsPlayerTurn())
			{
				UnitControl.SwitchToNextUnitInTurn();
			}
			else
			{
				AIManager.AIRoutine(GameControlTB.turnID);
			}
		}
		yield break;
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x0001D50C File Offset: 0x0001B70C
	public static void SwitchToNextUnit()
	{
		Debug.Log("SwitchToNextUnit");
		UnitControl.instance.StartCoroutine(UnitControl.instance.DelaySwitchUnit());
	}

	// Token: 0x06002D84 RID: 11652 RVA: 0x0015E988 File Offset: 0x0015CB88
	private IEnumerator DelaySwitchUnit()
	{
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		this.CheckBattleEnd();
		if (this.bBattleOver)
		{
			GameControlTB.BattleEnded(this.iWinFaction);
		}
		else
		{
			UnitControl.SwitchToNextUnitInTurn();
		}
		yield break;
	}

	// Token: 0x06002D85 RID: 11653 RVA: 0x0001D52D File Offset: 0x0001B72D
	public static void DelaySwitchToNextUnitInTurnList()
	{
		Debug.Log("DelaySwitchToNextUnitInTurnList");
		UnitControl.instance.StartCoroutine(UnitControl.instance._DelaySwitchToNextUnitInTurnList());
	}

	// Token: 0x06002D86 RID: 11654 RVA: 0x0015E9A4 File Offset: 0x0015CBA4
	private IEnumerator _DelaySwitchToNextUnitInTurnList()
	{
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		UnitControl.SwitchToNextUnitInTurnList();
		yield break;
	}

	// Token: 0x06002D87 RID: 11655 RVA: 0x0015E9B8 File Offset: 0x0015CBB8
	public static void SwitchToNextUnitInTurnList()
	{
		UnitTB unitTB = UnitControl.selectedUnit;
		if (unitTB == null)
		{
			UnitControl.SwitchToNextUnitInTurn();
		}
		Faction faction = UnitControl.instance.allFactionList[GameControlTB.turnID];
		if (faction.unitYetToMove.Count <= 0)
		{
			return;
		}
		if (faction.unitYetToMove.Count == 1)
		{
			faction.currentTurnID = faction.unitYetToMove[0];
		}
		else
		{
			int num = faction.unitYetToMove.IndexOf(faction.currentTurnID);
			if (num < 0)
			{
				faction.currentTurnID = faction.unitYetToMove[0];
			}
			else if (num + 1 < faction.unitYetToMove.Count)
			{
				faction.currentTurnID = faction.unitYetToMove[num + 1];
			}
			else
			{
				faction.currentTurnID = faction.unitYetToMove[0];
			}
		}
		UnitControl.selectedUnit = faction.allUnitList[faction.currentTurnID];
		UnitControl.selectedUnit.occupiedTile.Select();
	}

	// Token: 0x06002D88 RID: 11656 RVA: 0x0015EAC0 File Offset: 0x0015CCC0
	public static void SwitchToNextUnitInTurn()
	{
		Debug.Log("SwitchToNextUnitInTurn 找下一個");
		GridManager.Deselect();
		if (!GameControlTB.IsPlayerTurn())
		{
			Debug.LogError("SwitchToNextUnitInTurn 變成不是 PlayerTurn ???? ");
			return;
		}
		if (GameControlTB.turnID >= UnitControl.instance.allFactionList.Count)
		{
			GameControlTB.OnEndTurn();
			return;
		}
		Faction faction = UnitControl.instance.allFactionList[GameControlTB.turnID];
		if (!UnitControl.instance.SelectNexUnit(faction))
		{
			return;
		}
	}

	// Token: 0x06002D89 RID: 11657 RVA: 0x0015EB38 File Offset: 0x0015CD38
	private bool SelectNexUnit(Faction faction)
	{
		Debug.Log("SelectNexUnit = " + faction.factionID.ToString());
		if (faction.allUnitList.Count == 0)
		{
			Debug.LogWarning("Error 這個陣營沒有角色");
			faction.allUnitMoved = true;
			GameControlTB.OnEndTurn();
			return false;
		}
		if (faction.allUnitMoved)
		{
			Debug.LogWarning("全部角色都移動過了");
			GameControlTB.OnEndTurn();
			return false;
		}
		if (faction.unitYetToMove.Count <= 0)
		{
			Debug.LogWarning(" 沒有可以移動的角色 ");
			faction.allUnitMoved = true;
			GameControlTB.OnEndTurn();
			return false;
		}
		int num = Random.Range(0, faction.unitYetToMove.Count);
		faction.currentTurnID = faction.unitYetToMove[num];
		if (faction.currentTurnID >= faction.allUnitList.Count)
		{
			UnitControl.selectedUnit = null;
			return false;
		}
		UnitControl.selectedUnit = faction.allUnitList[faction.currentTurnID];
		UnitControl.selectedUnit.occupiedTile.Select();
		return true;
	}

	// Token: 0x06002D8A RID: 11658 RVA: 0x0015EC34 File Offset: 0x0015CE34
	private void InitFaction()
	{
		List<int> list = new List<int>();
		UnitControl.activeFactionCount = 0;
		foreach (UnitTB unitTB in this.allUnits)
		{
			if (!list.Contains(unitTB.factionID))
			{
				UnitControl.activeFactionCount++;
				list.Add(unitTB.factionID);
			}
		}
		bool flag = false;
		List<int> playerFactionIDS = GameControlTB.GetPlayerFactionIDS();
		for (int i = 0; i < playerFactionIDS.Count; i++)
		{
			if (list.Contains(playerFactionIDS[i]))
			{
				flag = true;
			}
		}
		if (!flag)
		{
			Debug.LogWarning("沒有可以操作的陣營");
		}
		GameControlTB.totalFactionInGame = list.Count;
		if (GameControlTB.totalFactionInGame == 1 && !GameGlobal.m_bDLCMode)
		{
			Debug.LogWarning("只有一個陣營 無法開始戰鬥");
			return;
		}
		List<int> playerFactionIDS2 = GameControlTB.GetPlayerFactionIDS();
		GameControlTB.playerFactionTurnID = new List<int>();
		for (int j = 0; j < playerFactionIDS2.Count; j++)
		{
			GameControlTB.playerFactionTurnID.Add(-1);
		}
		for (int k = 0; k < GameControlTB.totalFactionInGame; k++)
		{
			Faction faction = new Faction();
			faction.factionID = list[k];
			if (k < this.factionColors.Count)
			{
				faction.color = this.factionColors[k];
			}
			else
			{
				faction.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
			}
			foreach (UnitTB unitTB2 in this.allUnits)
			{
				if (unitTB2.factionID == list[k])
				{
					faction.allUnitList.Add(unitTB2);
				}
			}
			for (int l = 0; l < playerFactionIDS2.Count; l++)
			{
				if (playerFactionIDS2[l] == faction.factionID)
				{
					faction.isPlayerControl = true;
					GameControlTB.playerFactionTurnID[l] = k;
					GameControlTB.playerFactionExisted = true;
				}
			}
			this.allFactionList.Add(faction);
		}
		this.bInitFaction = true;
		GameControlTB.instance._playerFactionTurnID = GameControlTB.playerFactionTurnID;
	}

	// Token: 0x06002D8B RID: 11659 RVA: 0x0015EEDC File Offset: 0x0015D0DC
	public static Texture GetFactionIcon(int ID)
	{
		if (UnitControl.instance != null)
		{
			foreach (Faction faction in UnitControl.instance.allFactionList)
			{
				if (faction.factionID == ID)
				{
					return faction.icon;
				}
			}
		}
		return null;
	}

	// Token: 0x06002D8C RID: 11660 RVA: 0x0015EF60 File Offset: 0x0015D160
	public static Color GetFactionColor(int ID)
	{
		if (UnitControl.instance != null)
		{
			foreach (Faction faction in UnitControl.instance.allFactionList)
			{
				if (faction.factionID == ID)
				{
					return faction.color;
				}
			}
		}
		return Color.white;
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x0001D54E File Offset: 0x0001B74E
	public static int GetFactionPlacementID()
	{
		return UnitControl.instance.unitPlacementID;
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x0001D55A File Offset: 0x0001B75A
	public static PlayerUnits GetPlayerUnitsBeingPlaced()
	{
		return UnitControl.instance.playerUnitsList[UnitControl.instance.facPlacementID];
	}

	// Token: 0x06002D8F RID: 11663 RVA: 0x0015EFE8 File Offset: 0x0015D1E8
	public static void NextFactionPlacementID()
	{
		UnitControl.ResetUnitPlacementID();
		if (UnitControl.instance.playerUnitsList[UnitControl.instance.facPlacementID].starting.Count == 0)
		{
			UnitControl.instance.playerUnitsList.RemoveAt(0);
		}
		if (UnitControl.onNewPlacementE != null)
		{
			UnitControl.onNewPlacementE(UnitControl.instance.playerUnitsList[0]);
		}
		if (UnitControl.onPlacementUpdateE != null)
		{
			UnitControl.onPlacementUpdateE();
		}
	}

	// Token: 0x06002D90 RID: 11664 RVA: 0x0001D575 File Offset: 0x0001B775
	public static int GetPlayerUnitsRemainng()
	{
		return UnitControl.instance.playerUnitsList.Count;
	}

	// Token: 0x06002D91 RID: 11665 RVA: 0x0001D54E File Offset: 0x0001B74E
	public static int GetUnitPlacementID()
	{
		return UnitControl.instance.unitPlacementID;
	}

	// Token: 0x06002D92 RID: 11666 RVA: 0x0015F06C File Offset: 0x0015D26C
	public static void NextUnitPlacementID()
	{
		UnitControl.instance.unitPlacementID++;
		if (UnitControl.instance.unitPlacementID >= UnitControl.instance.playerUnitsList[UnitControl.instance.facPlacementID].starting.Count)
		{
			UnitControl.instance.unitPlacementID = 0;
		}
	}

	// Token: 0x06002D93 RID: 11667 RVA: 0x0015F0C8 File Offset: 0x0015D2C8
	public static void PrevUnitPlacementID()
	{
		UnitControl.instance.unitPlacementID--;
		if (UnitControl.instance.unitPlacementID < 0)
		{
			UnitControl.instance.unitPlacementID = UnitControl.instance.playerUnitsList[UnitControl.instance.facPlacementID].starting.Count - 1;
		}
	}

	// Token: 0x06002D94 RID: 11668 RVA: 0x0001D586 File Offset: 0x0001B786
	public static void ResetUnitPlacementID()
	{
		UnitControl.instance.unitPlacementID = 0;
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x0015F128 File Offset: 0x0015D328
	public static void PlaceUnitAt(Tile tile)
	{
		if (UnitControl.instance.playerUnitsList.Count == 0)
		{
			return;
		}
		if (UnitControl.instance.playerUnitsList[UnitControl.instance.facPlacementID].starting.Count == 0)
		{
			return;
		}
		PlayerUnits playerUnits = UnitControl.instance.playerUnitsList[UnitControl.instance.facPlacementID];
		if (playerUnits.starting[UnitControl.instance.unitPlacementID] == null)
		{
			playerUnits.starting.RemoveAt(UnitControl.instance.unitPlacementID);
			if (UnitControl.instance.unitPlacementID >= playerUnits.starting.Count)
			{
				UnitControl.instance.unitPlacementID--;
			}
			return;
		}
		UnitTB unitTB = playerUnits.starting[UnitControl.instance.unitPlacementID];
		unitTB.transform.position = tile.thisT.position;
		unitTB.occupiedTile = tile;
		tile.SetUnit(unitTB);
		playerUnits.starting.RemoveAt(UnitControl.instance.unitPlacementID);
		UnitControl.instance.allUnits.Add(unitTB);
		GridManager.PlaceUnitAt(tile);
		UnitTB nearestHostile = UnitControl.GetNearestHostile(unitTB);
		unitTB.RotateToUnit(nearestHostile);
		if (UnitControl.instance.unitPlacementID >= playerUnits.starting.Count)
		{
			UnitControl.instance.unitPlacementID--;
		}
		if (UnitControl.onPlacementUpdateE != null)
		{
			UnitControl.onPlacementUpdateE();
		}
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x0015F2A4 File Offset: 0x0015D4A4
	public static void RemoveUnit(UnitTB unit)
	{
		GridManager.RemoveUnitAt(unit.occupiedTile);
		UnitControl.instance.playerUnitsList[UnitControl.instance.facPlacementID].starting.Insert(0, unit);
		unit.occupiedTile.ClearUnit();
		unit.occupiedTile = null;
		unit.transform.position = new Vector3(0f, 9999f, 0f);
		UnitControl.instance.allUnits.Remove(unit);
		if (UnitControl.instance.unitPlacementID < 0)
		{
			UnitControl.instance.unitPlacementID = 0;
		}
		if (UnitControl.onPlacementUpdateE != null)
		{
			UnitControl.onPlacementUpdateE();
		}
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x0001D593 File Offset: 0x0001B793
	public static void AutoPlaceUnit()
	{
		UnitControl.instance._AutoPlaceUnit();
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x0015F354 File Offset: 0x0015D554
	public void _AutoPlaceUnit()
	{
		List<Tile> allPlaceableTiles = GridManager.GetAllPlaceableTiles();
		for (int i = 0; i < allPlaceableTiles.Count; i++)
		{
			if (allPlaceableTiles[i].unit != null)
			{
				allPlaceableTiles.RemoveAt(i);
				i--;
			}
		}
		PlayerUnits playerUnits = this.playerUnitsList[this.facPlacementID];
		while (playerUnits.starting.Count > 0)
		{
			int num = Random.Range(0, allPlaceableTiles.Count);
			UnitControl.PlaceUnitAt(allPlaceableTiles[num]);
			if (allPlaceableTiles.Count <= 0)
			{
				break;
			}
		}
	}

	// Token: 0x06002D99 RID: 11673 RVA: 0x0015F3F4 File Offset: 0x0015D5F4
	public static bool IsAllFactionPlaced()
	{
		return (UnitControl.instance.playerUnitsList.Count == 1 && UnitControl.instance.playerUnitsList[0].starting.Count == 0) || GridManager.AllPlaceableTileOccupied();
	}

	// Token: 0x06002D9A RID: 11674 RVA: 0x0001D59F File Offset: 0x0001B79F
	public static bool IsFactionAllUnitPlaced()
	{
		return UnitControl.instance.playerUnitsList[UnitControl.instance.facPlacementID].starting.Count == 0 || GridManager.AllPlaceableTileOccupied();
	}

	// Token: 0x06002D9B RID: 11675 RVA: 0x0015F444 File Offset: 0x0015D644
	public static void InsertUnit(UnitTB unit, Tile tile, int factionID, int duration)
	{
		if (unit == null)
		{
			Debug.LogError("InsertUnit unit = null");
			return;
		}
		if (tile.unit != null)
		{
			Debug.LogError("InsertUnit tile already have unit");
			return;
		}
		if (duration > 0)
		{
			unit.SetSpawnInGameFlag(true);
			unit.SetSpawnDuration(duration);
		}
		unit.factionID = factionID;
		unit.transform.position = tile.thisT.position;
		unit.occupiedTile = tile;
		tile.SetUnit(unit);
		if (UnitControl.instance.bInitFaction)
		{
			bool flag = false;
			for (int i = 0; i < UnitControl.instance.allFactionList.Count; i++)
			{
				Faction faction = UnitControl.instance.allFactionList[i];
				if (faction.factionID == factionID)
				{
					if (faction.allUnitList.Count == 0)
					{
						UnitControl.activeFactionCount++;
					}
					faction.unitYetToMove.Add(faction.allUnitList.Count);
					faction.allUnitList.Add(unit);
					faction.allUnitMoved = false;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Faction faction2 = new Faction();
				faction2.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
				faction2.allUnitList.Add(unit);
				faction2.factionID = factionID;
				if (GameControlTB.IsPlayerFaction(factionID))
				{
					faction2.isPlayerControl = true;
					GameControlTB.playerFactionTurnID.Add(GameControlTB.totalFactionInGame);
					GameControlTB.instance.playerFactionID.Add(factionID);
					GameControlTB.playerFactionExisted = true;
				}
				UnitControl.instance.allFactionList.Add(faction2);
				UnitControl.activeFactionCount++;
				GameControlTB.totalFactionInGame++;
				Debug.LogWarning("臨時加入新陣營 還沒測試過 有問題找小高");
			}
		}
		UnitControl.instance.allUnits.Add(unit);
		if (UnitControl.onNewUnitInRuntimeE != null)
		{
			UnitControl.onNewUnitInRuntimeE(unit);
		}
		if (UnitControl.selectedUnit != null && UnitControl.selectedUnit.occupiedTile != null)
		{
			UnitControl.selectedUnit.occupiedTile.Select();
		}
	}

	// Token: 0x06002D9C RID: 11676 RVA: 0x0015F67C File Offset: 0x0015D87C
	public static void ChangeUnitFaction(UnitTB scUnit, int targetFactionID)
	{
		Faction faction = null;
		Faction faction2 = null;
		scUnit.ClearAllFlag();
		for (int i = 0; i < UnitControl.instance.allFactionList.Count; i++)
		{
			Faction faction3 = UnitControl.instance.allFactionList[i];
			if (faction3.factionID == scUnit.factionID)
			{
				faction = faction3;
			}
		}
		for (int j = 0; j < UnitControl.instance.allFactionList.Count; j++)
		{
			Faction faction4 = UnitControl.instance.allFactionList[j];
			if (faction4.factionID == targetFactionID)
			{
				faction2 = faction4;
			}
		}
		if (faction2 == null)
		{
			faction2 = new Faction();
			faction2.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
			faction2.factionID = targetFactionID;
			if (GameControlTB.IsPlayerFaction(targetFactionID))
			{
				faction2.isPlayerControl = true;
				GameControlTB.playerFactionTurnID.Add(GameControlTB.totalFactionInGame);
				GameControlTB.instance.playerFactionID.Add(targetFactionID);
				GameControlTB.playerFactionExisted = true;
			}
			UnitControl.instance.allFactionList.Add(faction2);
			UnitControl.activeFactionCount++;
			GameControlTB.totalFactionInGame++;
			Debug.LogWarning("臨時加入新陣營 還沒測試過 有問題找小高");
		}
		if (faction != null && faction2 != null)
		{
			bool flag = false;
			for (int k = 0; k < faction.allUnitList.Count; k++)
			{
				if (faction.allUnitList[k] == scUnit)
				{
					flag = true;
					if (faction.unitYetToMove.Contains(k))
					{
						faction.unitYetToMove.Remove(k);
						if (faction.unitYetToMove.Count == 0)
						{
							faction.allUnitMoved = true;
						}
					}
					for (int l = 0; l < faction.unitYetToMove.Count; l++)
					{
						if (faction.unitYetToMove[l] > k)
						{
							List<int> unitYetToMove;
							List<int> list = unitYetToMove = faction.unitYetToMove;
							int num2;
							int num = num2 = l;
							num2 = unitYetToMove[num2];
							list[num] = num2 - 1;
						}
					}
					break;
				}
			}
			if (flag)
			{
				faction.allUnitList.Remove(scUnit);
				if (faction.allUnitList.Count == 0)
				{
					UnitControl.activeFactionCount--;
					if (UnitControl.onFactionDefeatedE != null)
					{
						UnitControl.onFactionDefeatedE(faction.factionID);
					}
				}
				else if (faction.allUnitList.Count == 1 && faction.allUnitList[0] != null)
				{
					faction.allUnitList[0].YouAreLastOne();
				}
			}
			if (faction2.allUnitList.Count == 0)
			{
				UnitControl.activeFactionCount++;
			}
			faction2.unitYetToMove.Add(faction2.allUnitList.Count);
			faction2.allUnitList.Add(scUnit);
			faction2.allUnitMoved = false;
			scUnit.factionID = targetFactionID;
			if (UnitControl.onUnitFactionChangedE != null)
			{
				UnitControl.onUnitFactionChangedE(scUnit);
			}
		}
		else
		{
			Debug.Log("faction doesn't exist");
		}
	}

	// Token: 0x06002D9D RID: 11677 RVA: 0x0015F9AC File Offset: 0x0015DBAC
	public static void ResetFactionUnitMoveList()
	{
		for (int i = 0; i < UnitControl.instance.allFactionList.Count; i++)
		{
			Faction faction = UnitControl.instance.allFactionList[i];
			faction.unitYetToMove = new List<int>();
			for (int j = 0; j < faction.allUnitList.Count; j++)
			{
				faction.unitYetToMove.Add(j);
			}
		}
	}

	// Token: 0x06002D9E RID: 11678 RVA: 0x0015FA20 File Offset: 0x0015DC20
	public static void MoveUnit(UnitTB unit)
	{
		if (unit.bIsMoving)
		{
			unit.audioTB.StopMove();
			unit.animationTB.StopMove();
			unit.bIsMoving = false;
			unit.ClearMovePath();
		}
		bool flag = false;
		for (int i = 0; i < UnitControl.instance.allFactionList.Count; i++)
		{
			if (unit.factionID == UnitControl.instance.allFactionList[i].factionID)
			{
				UnitControl.instance.allFactionList[i].bFirstAction = false;
				for (int j = 0; j < UnitControl.instance.allFactionList[i].allUnitList.Count; j++)
				{
					if (UnitControl.instance.allFactionList[i].allUnitList[j] == unit)
					{
						UnitControl.instance.allFactionList[i].unitYetToMove.Remove(j);
						if (UnitControl.instance.allFactionList[i].unitYetToMove.Count == 0)
						{
							UnitControl.instance.allFactionList[i].allUnitMoved = true;
						}
						flag = true;
						break;
					}
				}
			}
		}
		if (!flag)
		{
			Debug.Log("Error: no unit found trying to registered a unit moved ");
		}
	}

	// Token: 0x06002D9F RID: 11679 RVA: 0x0015FB6C File Offset: 0x0015DD6C
	public static void OneMoreMoveUnit(UnitTB unit)
	{
		for (int i = 0; i < UnitControl.instance.allFactionList.Count; i++)
		{
			if (unit.factionID == UnitControl.instance.allFactionList[i].factionID)
			{
				UnitControl.instance.allFactionList[i].allUnitMoved = false;
				if (!UnitControl.instance.allFactionList[i].allUnitList.Contains(unit))
				{
					UnitControl.instance.allFactionList[i].allUnitList.Add(unit);
				}
			}
		}
	}

	// Token: 0x06002DA0 RID: 11680 RVA: 0x0015FC0C File Offset: 0x0015DE0C
	public static void ArrangeAllUnitOnTurnPriority()
	{
		List<UnitTB> list = new List<UnitTB>();
		while (UnitControl.instance.allUnits.Count > 0)
		{
			float num = float.NegativeInfinity;
			int num2 = 0;
			for (int i = 0; i < UnitControl.instance.allUnits.Count; i++)
			{
				if ((float)UnitControl.instance.allUnits[i].GetTurnPriority() > num)
				{
					num = (float)UnitControl.instance.allUnits[i].GetTurnPriority();
					num2 = i;
				}
			}
			list.Add(UnitControl.instance.allUnits[num2]);
			UnitControl.instance.allUnits.RemoveAt(num2);
		}
		UnitControl.instance.allUnits = list;
	}

	// Token: 0x06002DA1 RID: 11681 RVA: 0x0015FCC8 File Offset: 0x0015DEC8
	public static void ShuffleAllUnit()
	{
		for (int i = 0; i < UnitControl.instance.allFactionList.Count; i++)
		{
			Faction faction = UnitControl.instance.allFactionList[i];
			List<UnitTB> list = new List<UnitTB>();
			List<int> list2 = new List<int>();
			for (int j = 0; j < faction.allUnitList.Count; j++)
			{
				list2.Add(j);
			}
			for (int k = 0; k < faction.allUnitList.Count; k++)
			{
				int num = Random.Range(0, list2.Count);
				int num2 = list2[num];
				list.Add(faction.allUnitList[num2]);
				list2.RemoveAt(num);
			}
			faction.allUnitList = list;
		}
	}

	// Token: 0x06002DA2 RID: 11682 RVA: 0x0015FD94 File Offset: 0x0015DF94
	private void OnUnitDestroyed(UnitTB unit)
	{
		if (unit == UnitControl.selectedUnit)
		{
			GridManager.Deselect();
		}
		this.allUnits.Remove(unit);
		if (!this.bInitFaction)
		{
			return;
		}
		foreach (UnitTB unitTB in this.allUnits)
		{
			unitTB.RemoveTauntFleeCondition(unit.iUnitID);
		}
		int factionID = unit.factionID;
		Faction faction = UnitControl.GetFaction(factionID);
		if (faction == null)
		{
			Debug.Log("Error? 無法找到這個角色的陣營 :" + factionID);
		}
		bool flag = false;
		for (int i = 0; i < faction.allUnitList.Count; i++)
		{
			if (faction.allUnitList[i] == unit)
			{
				flag = true;
				if (faction.unitYetToMove.Contains(i))
				{
					faction.unitYetToMove.Remove(i);
					if (faction.unitYetToMove.Count == 0)
					{
						faction.allUnitMoved = true;
					}
				}
				for (int j = 0; j < faction.unitYetToMove.Count; j++)
				{
					if (faction.unitYetToMove[j] > i)
					{
						List<int> unitYetToMove;
						List<int> list = unitYetToMove = faction.unitYetToMove;
						int num2;
						int num = num2 = j;
						num2 = unitYetToMove[num2];
						list[num] = num2 - 1;
					}
				}
				break;
			}
		}
		if (!flag)
		{
			Debug.Log("Error? cant find unit in supposed faction");
			return;
		}
		faction.allUnitList.Remove(unit);
		List<UnitTB> allFriend = UnitControl.GetAllFriend(factionID);
		if (allFriend.Count == 1)
		{
			allFriend[0].YouAreLastOne();
		}
		if (!unit.bLeaveBattle)
		{
			foreach (UnitTB unitTB2 in allFriend)
			{
				unitTB2.CheckTalentTeamMateDeadth();
			}
		}
		if (faction.allUnitList.Count == 0)
		{
			UnitControl.activeFactionCount--;
			if (UnitControl.onFactionDefeatedE != null)
			{
				UnitControl.onFactionDefeatedE(factionID);
			}
		}
		if (this.DeadWillWinList.Contains(unit))
		{
			this.iWinFaction = GameControlTB.GetPlayerFactionID();
			this.bBattleOver = true;
		}
		if (this.DeadWillLostList.Contains(unit))
		{
			List<UnitTB> allHostile = UnitControl.GetAllHostile(GameControlTB.GetPlayerFactionID());
			if (allHostile.Count > 0)
			{
				this.iWinFaction = allHostile[0].factionID;
			}
			else
			{
				this.iWinFaction = 1;
			}
			this.bBattleOver = true;
		}
		if (GameGlobal.m_bDLCMode && TeamStatus.m_Instance.CheckDLCAllTeamMateHurt())
		{
			List<UnitTB> allHostile2 = UnitControl.GetAllHostile(GameControlTB.GetPlayerFactionID());
			if (allHostile2.Count > 0)
			{
				this.iWinFaction = allHostile2[0].factionID;
			}
			else
			{
				this.iWinFaction = 1;
			}
			this.bBattleOver = true;
		}
	}

	// Token: 0x06002DA3 RID: 11683 RVA: 0x001600AC File Offset: 0x0015E2AC
	public void CheckBattleEnd()
	{
		if (UnitControl.activeFactionCount == 1)
		{
			List<Faction> factionsWithUnitRemain = this.GetFactionsWithUnitRemain();
			if (factionsWithUnitRemain.Count == 1)
			{
				Debug.Log("Battle Last One Faction = " + factionsWithUnitRemain[0].factionID.ToString());
				if (GameControlTB.GetPlayerFactionID() == factionsWithUnitRemain[0].factionID)
				{
					if (this.bAllEnemyUnitDead)
					{
						Debug.Log("Set AllEnemyUnitDead ");
						this.iWinFaction = factionsWithUnitRemain[0].factionID;
						this.bBattleOver = true;
					}
				}
				else if (this.bAllFriendUnitDead)
				{
					Debug.Log("Set AllFriendUnitDead ");
					this.iWinFaction = factionsWithUnitRemain[0].factionID;
					this.bBattleOver = true;
				}
			}
			else if (factionsWithUnitRemain.Count <= 0)
			{
				Debug.Log("all unit is dead. error?");
				if (this.bAllFriendUnitDead)
				{
					Debug.Log("Set AllFriendUnitDead ");
					this.iWinFaction = 1;
					this.bBattleOver = true;
				}
			}
		}
		else if (UnitControl.activeFactionCount == 0)
		{
			if (this.bAllFriendUnitDead)
			{
				Debug.Log("Set AllFriendUnitDead ");
				this.iWinFaction = 1;
				this.bBattleOver = true;
			}
		}
		else
		{
			if (UnitControl.GetAllFriend(0).Count <= 0 && this.bAllFriendUnitDead)
			{
				Debug.Log("Set AllFriendUnitDead ");
				this.iWinFaction = 1;
				this.bBattleOver = true;
			}
			if (UnitControl.GetAllHostile(0).Count <= 0 && this.bAllEnemyUnitDead)
			{
				Debug.Log("Set AllEnemyUnitDead ");
				this.iWinFaction = 0;
				this.bBattleOver = true;
			}
		}
	}

	// Token: 0x06002DA4 RID: 11684 RVA: 0x0016024C File Offset: 0x0015E44C
	public static int CheckMovedUnitInFaction(Faction faction)
	{
		int num = 0;
		for (int i = 0; i < faction.allUnitList.Count; i++)
		{
			if (faction.allUnitList[i].IsAllActionCompleted())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002DA5 RID: 11685 RVA: 0x00160294 File Offset: 0x0015E494
	public static int CheckUnmovedUnitInFaction(Faction faction)
	{
		int num = 0;
		for (int i = 0; i < faction.allUnitList.Count; i++)
		{
			if (!faction.allUnitList[i].IsAllActionCompleted())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002DA6 RID: 11686 RVA: 0x001602DC File Offset: 0x0015E4DC
	private List<Faction> GetFactionsWithUnitRemain()
	{
		List<Faction> list = new List<Faction>();
		foreach (Faction faction in this.allFactionList)
		{
			if (faction.allUnitList.Count > 0)
			{
				list.Add(faction);
			}
		}
		return list;
	}

	// Token: 0x06002DA7 RID: 11687 RVA: 0x0001D5D6 File Offset: 0x0001B7D6
	public static int GetActiveFactionsCount()
	{
		return UnitControl.activeFactionCount;
	}

	// Token: 0x06002DA8 RID: 11688 RVA: 0x00160350 File Offset: 0x0015E550
	public static bool IsFactionFirstAction(int iFactionID)
	{
		for (int i = 0; i < UnitControl.instance.allFactionList.Count; i++)
		{
			if (UnitControl.instance.allFactionList[i].factionID == iFactionID)
			{
				return UnitControl.instance.allFactionList[i].bFirstAction;
			}
		}
		return false;
	}

	// Token: 0x06002DA9 RID: 11689 RVA: 0x001603B0 File Offset: 0x0015E5B0
	public static bool IsFactionStillActive(int ID)
	{
		if (ID < 0 || ID >= UnitControl.instance.allFactionList.Count)
		{
			Debug.Log("Faction doesnt exist");
			return false;
		}
		return UnitControl.instance.allFactionList[ID].allUnitList.Count > 0;
	}

	// Token: 0x06002DAA RID: 11690 RVA: 0x0001D5DD File Offset: 0x0001B7DD
	public static bool AllUnitInFactionMoved(int turnID)
	{
		return UnitControl.instance.allFactionList[turnID].allUnitMoved;
	}

	// Token: 0x06002DAB RID: 11691 RVA: 0x00160408 File Offset: 0x0015E608
	public static bool AllUnitInAllFactionMoved()
	{
		for (int i = 0; i < UnitControl.instance.allFactionList.Count; i++)
		{
			if (!UnitControl.instance.allFactionList[i].allUnitMoved)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002DAC RID: 11692 RVA: 0x0001D5FC File Offset: 0x0001B7FC
	public static Faction GetFactionInTurn(int turnID)
	{
		return UnitControl.instance.allFactionList[turnID];
	}

	// Token: 0x06002DAD RID: 11693 RVA: 0x00160454 File Offset: 0x0015E654
	public static Faction GetFaction(int factionID)
	{
		for (int i = 0; i < UnitControl.instance.allFactionList.Count; i++)
		{
			if (UnitControl.instance.allFactionList[i].factionID == factionID)
			{
				return UnitControl.instance.allFactionList[i];
			}
		}
		return null;
	}

	// Token: 0x06002DAE RID: 11694 RVA: 0x001604B0 File Offset: 0x0015E6B0
	public static int GetPlayerFactionTurnID()
	{
		List<int> playerFactionIDS = GameControlTB.GetPlayerFactionIDS();
		for (int i = 0; i < UnitControl.instance.allFactionList.Count; i++)
		{
			if (playerFactionIDS.Contains(UnitControl.instance.allFactionList[i].factionID))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002DAF RID: 11695 RVA: 0x0001D60E File Offset: 0x0001B80E
	public static List<UnitTB> GetUnplacedUnit()
	{
		return UnitControl.instance.playerUnitsList[UnitControl.instance.facPlacementID].starting;
	}

	// Token: 0x06002DB0 RID: 11696 RVA: 0x0001D62E File Offset: 0x0001B82E
	public static List<UnitTB> GetAllUnit()
	{
		return UnitControl.instance.allUnits;
	}

	// Token: 0x06002DB1 RID: 11697 RVA: 0x0001D63A File Offset: 0x0001B83A
	public static int GetAllUnitCount()
	{
		return UnitControl.instance.allUnits.Count;
	}

	// Token: 0x06002DB2 RID: 11698 RVA: 0x00160508 File Offset: 0x0015E708
	public static List<UnitTB> GetAllUnitsOfFaction(int factionID)
	{
		List<UnitTB> list = new List<UnitTB>();
		foreach (UnitTB unitTB in UnitControl.instance.allUnits)
		{
			if (unitTB.factionID == factionID)
			{
				list.Add(unitTB);
			}
		}
		return list;
	}

	// Token: 0x06002DB3 RID: 11699 RVA: 0x0016057C File Offset: 0x0015E77C
	public static List<UnitTB> GetAllFriend(int factionID)
	{
		List<UnitTB> list = new List<UnitTB>();
		foreach (UnitTB unitTB in UnitControl.instance.allUnits)
		{
			if (unitTB.CheckFriendFaction(factionID))
			{
				list.Add(unitTB);
			}
		}
		return list;
	}

	// Token: 0x06002DB4 RID: 11700 RVA: 0x001605F0 File Offset: 0x0015E7F0
	public static List<UnitTB> GetAllHostile(int factionID)
	{
		List<UnitTB> list = new List<UnitTB>();
		foreach (UnitTB unitTB in UnitControl.instance.allUnits)
		{
			if (!unitTB.CheckFriendFaction(factionID))
			{
				list.Add(unitTB);
			}
		}
		return list;
	}

	// Token: 0x06002DB5 RID: 11701 RVA: 0x00160664 File Offset: 0x0015E864
	public static void AllUnitFaceToHostile()
	{
		foreach (UnitTB unitTB in UnitControl.instance.allUnits)
		{
			UnitTB nearestHostile = UnitControl.GetNearestHostile(unitTB);
			unitTB.RotateToUnit(nearestHostile);
		}
	}

	// Token: 0x06002DB6 RID: 11702 RVA: 0x001606CC File Offset: 0x0015E8CC
	public static UnitTB GetNearestHostile(UnitTB srcUnit)
	{
		UnitTB result = null;
		float num = float.PositiveInfinity;
		foreach (UnitTB unitTB in UnitControl.instance.allUnits)
		{
			if (!srcUnit.CheckFriendFaction(unitTB.factionID) && !unitTB.bStealth)
			{
				float num2 = Vector3.Distance(srcUnit.occupiedTile.pos, unitTB.occupiedTile.pos);
				if (num2 < num)
				{
					result = unitTB;
					num = num2;
				}
			}
		}
		return result;
	}

	// Token: 0x06002DB7 RID: 11703 RVA: 0x00160774 File Offset: 0x0015E974
	public static UnitTB GetNearestHostileFromList(UnitTB srcUnit, List<UnitTB> targets)
	{
		UnitTB result = null;
		float num = float.PositiveInfinity;
		for (int i = 0; i < targets.Count; i++)
		{
			float num2 = Vector3.Distance(srcUnit.occupiedTile.pos, targets[i].occupiedTile.pos);
			if (num2 < num)
			{
				result = targets[i];
				num = num2;
			}
		}
		return result;
	}

	// Token: 0x06002DB8 RID: 11704 RVA: 0x001607D4 File Offset: 0x0015E9D4
	public static List<UnitTB> GetUnitInLOSFromList(UnitTB srcUnit, List<UnitTB> targets)
	{
		List<UnitTB> allUnitsOfFaction = UnitControl.GetAllUnitsOfFaction(srcUnit.factionID);
		for (int i = 0; i < targets.Count; i++)
		{
			if (GridManager.IsInLOS(srcUnit.occupiedTile, targets[i].occupiedTile))
			{
				allUnitsOfFaction.Add(targets[i]);
			}
		}
		return allUnitsOfFaction;
	}

	// Token: 0x06002DB9 RID: 11705 RVA: 0x00160830 File Offset: 0x0015EA30
	public static List<UnitTB> GetAllHostileWithinFactionSight(int factionID)
	{
		List<UnitTB> allUnitsOfFaction = UnitControl.GetAllUnitsOfFaction(factionID);
		List<UnitTB> allHostile = UnitControl.GetAllHostile(factionID);
		List<UnitTB> list = new List<UnitTB>();
		for (int i = 0; i < allUnitsOfFaction.Count; i++)
		{
			UnitTB unitTB = allUnitsOfFaction[i];
			for (int j = 0; j < allHostile.Count; j++)
			{
				UnitTB unitTB2 = allHostile[j];
				if (GridManager.IsInLOS(unitTB.occupiedTile, unitTB2.occupiedTile) && GridManager.Distance(unitTB.occupiedTile, unitTB2.occupiedTile) <= unitTB.GetSight())
				{
					list.Add(unitTB2);
				}
			}
		}
		return list;
	}

	// Token: 0x06002DBA RID: 11706 RVA: 0x001608D8 File Offset: 0x0015EAD8
	public static List<UnitTB> GetAllHostileWithinUnitSight(UnitTB srcUnit)
	{
		List<UnitTB> allHostile = UnitControl.GetAllHostile(srcUnit.factionID);
		List<UnitTB> list = new List<UnitTB>();
		for (int i = 0; i < allHostile.Count; i++)
		{
			if (GridManager.IsInLOS(srcUnit.occupiedTile, allHostile[i].occupiedTile))
			{
				list.Add(allHostile[i]);
			}
		}
		return list;
	}

	// Token: 0x06002DBB RID: 11707 RVA: 0x00160938 File Offset: 0x0015EB38
	public static List<UnitTB> GetHostileInAbilityFormTile(Tile srcTile, UnitTB srcUnit, bool visibleOnly)
	{
		List<Tile> attackAbleTiles = srcUnit.GetAttackAbleTiles(srcTile);
		List<UnitTB> list = new List<UnitTB>();
		for (int i = 0; i < attackAbleTiles.Count; i++)
		{
			if (attackAbleTiles[i].unit != null && !attackAbleTiles[i].unit.CheckFriendFaction(srcUnit.factionID) && !attackAbleTiles[i].unit.bStealth)
			{
				if (visibleOnly)
				{
					if (GridManager.IsInLOS(srcTile, attackAbleTiles[i]))
					{
						list.Add(attackAbleTiles[i].unit);
					}
				}
				else
				{
					list.Add(attackAbleTiles[i].unit);
				}
			}
		}
		return list;
	}

	// Token: 0x06002DBC RID: 11708 RVA: 0x0001D64B File Offset: 0x0001B84B
	public void OnDrawGizmos()
	{
		if (UnitControl.selectedUnit != null)
		{
		}
	}

	// Token: 0x06002DBD RID: 11709 RVA: 0x0001D65D File Offset: 0x0001B85D
	public void AddJoinNode(BattleJoinCharacterNode node)
	{
		this.joinByRoundList.Add(node);
	}

	// Token: 0x06002DBE RID: 11710 RVA: 0x0001D66B File Offset: 0x0001B86B
	public void AddJoinTeamNode(BattleJoinCharacterNode node)
	{
		this.joinTeamByRoundList.Add(node);
	}

	// Token: 0x06002DBF RID: 11711 RVA: 0x001609F8 File Offset: 0x0015EBF8
	private void CheckJoinUnit(int iNowRound)
	{
		if (this.joinByRoundList.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.joinByRoundList.Count; i++)
		{
			BattleJoinCharacterNode battleJoinCharacterNode = this.joinByRoundList[i];
			if (iNowRound >= battleJoinCharacterNode.m_iTurn)
			{
				if (BattleControl.instance != null)
				{
					Tile tile;
					if (battleJoinCharacterNode.m_iTile == 0)
					{
						tile = GridManager.instance.GetFactionTile(battleJoinCharacterNode.m_iFaction);
					}
					else
					{
						string strTileName = "Tile" + battleJoinCharacterNode.m_iTile.ToString();
						tile = GridManager.instance.GetNoUnitOnTileByName(strTileName);
					}
					if (tile == null)
					{
						Debug.Log("地方滿了 放不了角色");
						goto IL_116;
					}
					UnitTB unitTB = BattleControl.instance.GenerateBattleUnit(battleJoinCharacterNode.m_iCharID, null);
					if (unitTB == null)
					{
						Debug.Log("角色 有問題放不了角色");
						goto IL_116;
					}
					UnitControl.InsertUnit(unitTB, tile, battleJoinCharacterNode.m_iFaction, 0);
					string msg = string.Format(Game.StringTable.GetString(260015), unitTB.unitName);
					UINGUI.BattleMessage(msg);
				}
				this.joinByRoundList.RemoveAt(i);
				i--;
			}
			IL_116:;
		}
		for (int j = 0; j < this.joinTeamByRoundList.Count; j++)
		{
			BattleJoinCharacterNode battleJoinCharacterNode2 = this.joinTeamByRoundList[j];
			if (iNowRound >= battleJoinCharacterNode2.m_iTurn)
			{
				if (BattleControl.instance != null)
				{
					Tile tile2;
					if (battleJoinCharacterNode2.m_iTile == 0)
					{
						tile2 = GridManager.instance.GetFactionTile(0);
					}
					else
					{
						string strTileName2 = "Tile" + battleJoinCharacterNode2.m_iTile.ToString();
						tile2 = GridManager.instance.GetNoUnitOnTileByName(strTileName2);
					}
					if (tile2 == null)
					{
						Debug.Log("地方滿了 放不了角色");
						goto IL_237;
					}
					UnitTB unitTB2 = BattleControl.instance.GenerateBattleUnit(battleJoinCharacterNode2.m_iCharID, null);
					if (unitTB2 == null)
					{
						Debug.Log("角色 有問題放不了角色");
						goto IL_237;
					}
					UnitControl.InsertUnit(unitTB2, tile2, 0, 0);
					string msg2 = string.Format(Game.StringTable.GetString(260015), unitTB2.unitName);
					UINGUI.BattleMessage(msg2);
				}
				this.joinTeamByRoundList.RemoveAt(j);
				j--;
			}
			IL_237:;
		}
	}

	// Token: 0x06002DC0 RID: 11712 RVA: 0x00160C54 File Offset: 0x0015EE54
	public UnitTB FindUnit(int iD)
	{
		foreach (UnitTB unitTB in this.allUnits)
		{
			if (unitTB.iUnitID == iD)
			{
				return unitTB;
			}
		}
		return null;
	}

	// Token: 0x040039D7 RID: 14807
	[HideInInspector]
	public List<PlayerUnits> playerUnitsList = new List<PlayerUnits>();

	// Token: 0x040039D8 RID: 14808
	[HideInInspector]
	public List<Color> factionColors = new List<Color>();

	// Token: 0x040039D9 RID: 14809
	[HideInInspector]
	public List<UnitTB> allUnits = new List<UnitTB>();

	// Token: 0x040039DA RID: 14810
	private List<UnitTB> allUnitsMoved = new List<UnitTB>();

	// Token: 0x040039DB RID: 14811
	[HideInInspector]
	public List<Faction> allFactionList = new List<Faction>();

	// Token: 0x040039DC RID: 14812
	public static int activeFactionCount;

	// Token: 0x040039DD RID: 14813
	public static UnitTB selectedUnit;

	// Token: 0x040039DE RID: 14814
	public static UnitTB hoveredUnit;

	// Token: 0x040039DF RID: 14815
	public static UnitControl instance;

	// Token: 0x040039E0 RID: 14816
	public static int currentUnitTurnID = -1;

	// Token: 0x040039E1 RID: 14817
	private int highestUnitPriority;

	// Token: 0x040039E2 RID: 14818
	private bool holdWaitedTime;

	// Token: 0x040039E3 RID: 14819
	public static bool bTauntFlee;

	// Token: 0x040039E4 RID: 14820
	public bool hideUnitWhenKilled = true;

	// Token: 0x040039E5 RID: 14821
	public bool destroyUnitObject = true;

	// Token: 0x040039E6 RID: 14822
	private List<BattleJoinCharacterNode> joinByRoundList = new List<BattleJoinCharacterNode>();

	// Token: 0x040039E7 RID: 14823
	private List<BattleJoinCharacterNode> joinTeamByRoundList = new List<BattleJoinCharacterNode>();

	// Token: 0x040039E8 RID: 14824
	public bool bAllEnemyUnitDead;

	// Token: 0x040039E9 RID: 14825
	public bool bAllFriendUnitDead;

	// Token: 0x040039EA RID: 14826
	public List<UnitTB> DeadWillWinList = new List<UnitTB>();

	// Token: 0x040039EB RID: 14827
	public List<UnitTB> DeadWillLostList = new List<UnitTB>();

	// Token: 0x040039EC RID: 14828
	public bool bBattleOver;

	// Token: 0x040039ED RID: 14829
	public int iWinFaction;

	// Token: 0x040039EE RID: 14830
	private bool bInitFaction;

	// Token: 0x040039EF RID: 14831
	private int facPlacementID;

	// Token: 0x040039F0 RID: 14832
	private int unitPlacementID;

	// Token: 0x02000774 RID: 1908
	// (Invoke) Token: 0x06002DC2 RID: 11714
	public delegate void NewFactionPlacementHandler(PlayerUnits pUnits);

	// Token: 0x02000775 RID: 1909
	// (Invoke) Token: 0x06002DC6 RID: 11718
	public delegate void PlacementUpdateHandler();

	// Token: 0x02000776 RID: 1910
	// (Invoke) Token: 0x06002DCA RID: 11722
	public delegate void NewUnitInRuntimeHandler(UnitTB unit);

	// Token: 0x02000777 RID: 1911
	// (Invoke) Token: 0x06002DCE RID: 11726
	public delegate void FactionDefeatedHandler(int ID);

	// Token: 0x02000778 RID: 1912
	// (Invoke) Token: 0x06002DD2 RID: 11730
	public delegate void UnitFactionChangedHandler(UnitTB unit);
}
