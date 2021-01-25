using System;
using System.Collections;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x0200071A RID: 1818
public class AIManager : MonoBehaviour
{
	// Token: 0x06002B16 RID: 11030 RVA: 0x0001BE6F File Offset: 0x0001A06F
	public static _AIStance GetAIStance()
	{
		if (AIManager.instance == null)
		{
			return _AIStance.Passive;
		}
		return AIManager.instance.aIStance;
	}

	// Token: 0x06002B17 RID: 11031 RVA: 0x0001BE8D File Offset: 0x0001A08D
	private void Awake()
	{
		AIManager.instance = this;
	}

	// Token: 0x06002B18 RID: 11032 RVA: 0x0014FB80 File Offset: 0x0014DD80
	private IEnumerator SkippedAIRoutine()
	{
		yield return new WaitForSeconds(1f);
		GameControlTB.OnEndTurn();
		yield break;
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x0001BE95 File Offset: 0x0001A095
	public static void TauntFlee(int factionID)
	{
		AIManager.instance.StartCoroutine(AIManager.instance._TauntFleeRoutine(factionID));
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x0014FB94 File Offset: 0x0014DD94
	private IEnumerator _TauntFleeRoutine(int factionID)
	{
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		Faction faction = UnitControl.GetFactionInTurn(factionID);
		int i = 0;
		while (i < faction.allUnitList.Count)
		{
			while (GameGlobal.m_bBattleTalk)
			{
				yield return null;
			}
			while (GameControlTB.IsActionInProgress())
			{
				yield return null;
			}
			List<UnitTB> allEnemyUnit = UnitControl.GetAllHostile(faction.factionID);
			if (allEnemyUnit.Count <= 0 && !GameGlobal.m_bDLCMode)
			{
				yield return new WaitForSeconds(0.5f);
				this.EndTurn();
				yield break;
			}
			UnitTB unit = faction.allUnitList[i];
			if (unit.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt, null))
			{
				Debug.Log("Round " + GameControlTB.roundCounter.ToString() + " AI Taunt " + unit.unitName);
				this.unitInAction = true;
				base.StartCoroutine(this._MoveUnit2(unit));
				goto IL_230;
			}
			if (unit.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee, null))
			{
				Debug.Log("Round " + GameControlTB.roundCounter.ToString() + " AI Flee " + unit.unitName);
				this.unitInAction = true;
				base.StartCoroutine(this._MoveUnit2(unit));
				goto IL_230;
			}
			IL_2A3:
			i++;
			continue;
			IL_230:
			while (unit.InAction() || GameControlTB.IsActionInProgress() || this.unitInAction)
			{
				yield return null;
			}
			yield return new WaitForSeconds(this.delayBetweenUnit);
			if (GameControlTB.battleEnded)
			{
				yield break;
			}
			goto IL_2A3;
		}
		UnitControl.bTauntFlee = false;
		yield break;
	}

	// Token: 0x06002B1B RID: 11035 RVA: 0x0001BEAD File Offset: 0x0001A0AD
	public static void AIRoutine(int factionID)
	{
		if (GameGlobal.m_iBattleDifficulty < 7)
		{
			AIManager.instance.StartCoroutine(AIManager.instance._NewAIRoutine(factionID));
		}
		else
		{
			AIManager.instance.StartCoroutine(AIManager.instance._NewAIRoutineGroup(factionID));
		}
	}

	// Token: 0x06002B1C RID: 11036 RVA: 0x0014FBC0 File Offset: 0x0014DDC0
	private IEnumerator _NewAIRoutineGroup(int factionID)
	{
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		Faction faction = UnitControl.GetFactionInTurn(factionID);
		bool bSameUnit = false;
		UnitTB unit = null;
		while (faction.unitYetToMove.Count > 0 || bSameUnit)
		{
			while (GameGlobal.m_bBattleTalk)
			{
				yield return null;
			}
			while (GameControlTB.IsActionInProgress())
			{
				yield return null;
			}
			List<UnitTB> allEnemyUnit = UnitControl.GetAllHostile(faction.factionID);
			if (allEnemyUnit.Count <= 0)
			{
				yield return new WaitForSeconds(0.5f);
				this.EndTurn();
				yield break;
			}
			if (faction.allUnitList.Count <= 0)
			{
				yield return new WaitForSeconds(0.5f);
				this.EndTurn();
				yield break;
			}
			this.nodeList.Clear();
			if (bSameUnit && unit != null && !unit.IsDestroyed())
			{
				this.AnalyseUnit(unit);
			}
			else
			{
				for (int i = 0; i < faction.unitYetToMove.Count; i++)
				{
					faction.currentTurnID = faction.unitYetToMove[i];
					UnitTB unitNode = faction.allUnitList[faction.currentTurnID];
					this.AnalyseUnit(unitNode);
				}
			}
			bSameUnit = false;
			if (this.nodeList.Count <= 0)
			{
				yield return new WaitForSeconds(0.5f);
				this.EndTurn();
				yield break;
			}
			this.nodeList.Sort(new Comparison<AINode>(AIManager.CompareFunc));
			unit = this.nodeList[0].unitAI;
			this.unitInAction = true;
			Debug.Log("Round " + GameControlTB.roundCounter.ToString() + "Group AI Action " + unit.unitName);
			base.StartCoroutine(this._MoveUnit3(this.nodeList[0]));
			while (unit.InAction() || GameControlTB.IsActionInProgress() || this.unitInAction)
			{
				yield return null;
			}
			if (unit != null && !unit.IsDestroyed() && (!unit.attacked || unit.bNightFragrance))
			{
				bSameUnit = true;
			}
			else
			{
				yield return new WaitForSeconds(this.delayBetweenUnit);
			}
			if (GameControlTB.battleEnded)
			{
				yield break;
			}
		}
		faction.allUnitMoved = true;
		yield return new WaitForSeconds(0.5f);
		this.EndTurn();
		yield break;
	}

	// Token: 0x06002B1D RID: 11037 RVA: 0x0014FBEC File Offset: 0x0014DDEC
	private IEnumerator _NewAIRoutine(int factionID)
	{
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		Faction faction = UnitControl.GetFactionInTurn(factionID);
		bool bSameUnit = false;
		while (faction.unitYetToMove.Count > 0 || bSameUnit)
		{
			while (GameGlobal.m_bBattleTalk)
			{
				yield return null;
			}
			while (GameControlTB.IsActionInProgress())
			{
				yield return null;
			}
			List<UnitTB> allEnemyUnit = UnitControl.GetAllHostile(faction.factionID);
			if (allEnemyUnit.Count <= 0)
			{
				yield return new WaitForSeconds(0.5f);
				this.EndTurn();
				yield break;
			}
			if (faction.allUnitList.Count <= 0)
			{
				yield return new WaitForSeconds(0.5f);
				this.EndTurn();
				yield break;
			}
			if (bSameUnit)
			{
				bSameUnit = false;
			}
			else
			{
				if (faction.unitYetToMove.Count <= 0)
				{
					this.EndTurn();
					yield break;
				}
				int rand = Random.Range(0, faction.unitYetToMove.Count);
				faction.currentTurnID = faction.unitYetToMove[rand];
			}
			UnitTB unit = faction.allUnitList[faction.currentTurnID];
			this.unitInAction = true;
			Debug.Log("Round " + GameControlTB.roundCounter.ToString() + " AI Action " + unit.unitName);
			if (unit.fullSP > GameGlobal.m_iAISP)
			{
				base.StartCoroutine(this._MoveUnit2(unit));
			}
			else if (GameGlobal.m_bDLCMode)
			{
				base.StartCoroutine(this._MoveUnit2(unit));
			}
			else if (GameGlobal.m_iBattleDifficulty < 4)
			{
				base.StartCoroutine(this._MoveUnit(unit));
			}
			else
			{
				base.StartCoroutine(this._MoveUnit2(unit));
			}
			while (unit.InAction() || GameControlTB.IsActionInProgress() || this.unitInAction)
			{
				yield return null;
			}
			if (unit != null && !unit.IsDestroyed() && (!unit.attacked || unit.bNightFragrance))
			{
				bSameUnit = true;
			}
			else
			{
				yield return new WaitForSeconds(this.delayBetweenUnit);
			}
			if (GameControlTB.battleEnded)
			{
				yield break;
			}
		}
		faction.allUnitMoved = true;
		yield return new WaitForSeconds(0.5f);
		this.EndTurn();
		yield break;
	}

	// Token: 0x06002B1E RID: 11038 RVA: 0x0014FC18 File Offset: 0x0014DE18
	private IEnumerator _AIRoutine(int factionID)
	{
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		Faction faction = UnitControl.GetFactionInTurn(factionID);
		Debug.Log("Round " + GameControlTB.roundCounter.ToString() + " _AIRoutine faction =" + faction.factionID.ToString());
		List<UnitTB> allUnit = faction.allUnitList;
		int counter = 0;
		int count = allUnit.Count;
		List<int> IDList = new List<int>();
		for (int i = 0; i < count; i++)
		{
			IDList.Add(i);
		}
		for (int j = 0; j < count; j++)
		{
			int rand = Random.Range(0, IDList.Count);
			int ID = IDList[rand];
			UnitTB unit = allUnit[ID];
			IDList.RemoveAt(rand);
			counter++;
			this.unitInAction = true;
			Debug.Log("Round " + GameControlTB.roundCounter.ToString() + " AI Action " + unit.unitName);
			base.StartCoroutine(this._MoveUnit(unit));
			yield return null;
			yield return null;
			while (unit.InAction() || GameControlTB.IsActionInProgress() || this.unitInAction)
			{
				yield return null;
			}
			if (unit.IsDestroyed())
			{
				for (int k = rand; k < IDList.Count; k++)
				{
					if (IDList[k] >= ID)
					{
						List<int> list2;
						List<int> list = list2 = IDList;
						int num2;
						int num = num2 = k;
						num2 = list2[num2];
						list[num] = num2 - 1;
					}
				}
			}
			yield return new WaitForSeconds(this.delayBetweenUnit);
			if (GameControlTB.battleEnded)
			{
				yield break;
			}
		}
		faction.allUnitMoved = true;
		yield return new WaitForSeconds(0.5f);
		this.EndTurn();
		yield break;
	}

	// Token: 0x06002B1F RID: 11039 RVA: 0x0001BEEB File Offset: 0x0001A0EB
	public void EndTurn()
	{
		Debug.LogWarning("Round " + GameControlTB.roundCounter.ToString() + " AI EndTurn ");
		if (!GameControlTB.battleEnded)
		{
			GameControlTB.OnEndTurn();
		}
	}

	// Token: 0x06002B20 RID: 11040 RVA: 0x0014FC44 File Offset: 0x0014DE44
	public Tile Analyse(UnitTB unit)
	{
		List<UnitTB> hostileInAbilityFormTile = UnitControl.GetHostileInAbilityFormTile(unit.occupiedTile, unit, false);
		unit.occupiedTile.AIHostileList = hostileInAbilityFormTile;
		if (hostileInAbilityFormTile.Count != 0)
		{
			return unit.occupiedTile;
		}
		if (unit.GetMovementDisabled() || unit.aiMode == _AIMode.Stand)
		{
			return unit.occupiedTile;
		}
		List<UnitTB> list = new List<UnitTB>();
		list = UnitControl.GetAllHostile(unit.factionID);
		List<Tile> walkableTilesWithinRange = GridManager.GetWalkableTilesWithinRange(unit.occupiedTile, unit.GetMoveRange());
		List<Tile> list2 = new List<Tile>();
		for (int i = 0; i < walkableTilesWithinRange.Count; i++)
		{
			walkableTilesWithinRange[i].AIHostileList = new List<UnitTB>();
			bool visibleOnly = GameControlTB.EnableFogOfWar();
			List<UnitTB> hostileInAbilityFormTile2 = UnitControl.GetHostileInAbilityFormTile(walkableTilesWithinRange[i], unit, visibleOnly);
			walkableTilesWithinRange[i].AIHostileList = hostileInAbilityFormTile2;
			if (hostileInAbilityFormTile2.Count > 0)
			{
				list2.Add(walkableTilesWithinRange[i]);
			}
		}
		List<Tile> list3 = new List<Tile>();
		for (int j = 0; j < list2.Count; j++)
		{
			if (walkableTilesWithinRange.Contains(list2[j]))
			{
				list3.Add(list2[j]);
			}
		}
		if (list3.Count > 0)
		{
			int num = Random.Range(0, list3.Count);
			return list3[num];
		}
		UnitTB highestThreat = unit.GetHighestThreat();
		if (highestThreat != null)
		{
			return this.GetNearestTileWithWalkable(walkableTilesWithinRange, highestThreat.occupiedTile);
		}
		if (this.aIStance == _AIStance.Active || this.aIStance == _AIStance.Trigger)
		{
			if (list.Count == 0)
			{
				if (walkableTilesWithinRange.Count > 0)
				{
					int num2 = Random.Range(0, walkableTilesWithinRange.Count);
					return walkableTilesWithinRange[num2];
				}
			}
			else
			{
				List<float> list4 = new List<float>();
				for (int k = 0; k < list.Count; k++)
				{
					list4.Add((float)GridManager.Distance(unit.occupiedTile, list[k].occupiedTile));
				}
				float num3 = 0f;
				int num4 = 0;
				if (num4 < list.Count)
				{
					float num5 = 1E+09f;
					int num6 = 0;
					for (int l = 0; l < list.Count; l++)
					{
						if (list4[l] < num5 && list4[l] > num3)
						{
							num5 = list4[l];
							num6 = num4;
						}
					}
					return this.GetNearestTileWithWalkable(walkableTilesWithinRange, list[num6].occupiedTile);
				}
			}
		}
		return null;
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x0014FEDC File Offset: 0x0014E0DC
	private Tile GetNearestTileWithWalkable(List<Tile> tileList, Tile target)
	{
		Tile result = null;
		int num = 1000;
		for (int i = 0; i < tileList.Count; i++)
		{
			int num2 = GridManager.WalkDistance(tileList[i], target);
			if (num2 >= 0)
			{
				if (num2 < num)
				{
					num = num2;
					result = tileList[i];
				}
			}
		}
		return result;
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x0014FF34 File Offset: 0x0014E134
	private List<Tile> GetNearestTileWithWalkableList(List<Tile> tileList, Tile target)
	{
		List<Tile> list = new List<Tile>();
		int num = 1000;
		for (int i = 0; i < tileList.Count; i++)
		{
			int num2 = GridManager.WalkDistance(tileList[i], target);
			if (num2 >= 0)
			{
				if (num2 < num)
				{
					list.Clear();
					num = num2;
					list.Add(tileList[i]);
				}
				else if (num2 == num)
				{
					list.Add(tileList[i]);
				}
			}
		}
		return list;
	}

	// Token: 0x06002B23 RID: 11043 RVA: 0x0014FFB4 File Offset: 0x0014E1B4
	private List<Tile> GetFarestTileWithWalkableList(List<Tile> tileList, Tile target)
	{
		List<Tile> list = new List<Tile>();
		int num = 0;
		for (int i = 0; i < tileList.Count; i++)
		{
			int num2 = GridManager.WalkDistance(tileList[i], target);
			if (num2 >= 0)
			{
				if (num2 > num)
				{
					list.Clear();
					num = num2;
					list.Add(tileList[i]);
				}
				else if (num2 == num)
				{
					list.Add(tileList[i]);
				}
			}
		}
		return list;
	}

	// Token: 0x06002B24 RID: 11044 RVA: 0x00150030 File Offset: 0x0014E230
	private IEnumerator _MoveUnit(UnitTB unit)
	{
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		if (unit == null || unit.IsDestroyed() || unit.occupiedTile == null)
		{
			this.unitInAction = false;
			yield break;
		}
		GridManager.AISelect(unit.occupiedTile);
		yield return new WaitForSeconds(0.5f);
		if (!unit.IsStunned())
		{
			Tile destinationTile = null;
			if (this.aIStance == _AIStance.Trigger)
			{
				if (unit.triggered)
				{
					destinationTile = this.Analyse(unit);
				}
			}
			else
			{
				destinationTile = this.Analyse(unit);
			}
			if (destinationTile == unit.occupiedTile)
			{
				if (destinationTile.AIHostileList.Count != 0 && !unit.GetAttackDisabled())
				{
					base.StartCoroutine(this.AIAttack(unit));
				}
				else
				{
					UnitControl.MoveUnit(unit);
					unit.RecoveHPMP(1);
					this.unitInAction = false;
				}
				yield break;
			}
			if (destinationTile != null)
			{
				unit.Move(destinationTile);
				while (unit.InAction() || GameControlTB.IsActionInProgress())
				{
					yield return null;
				}
				if (destinationTile.AIHostileList.Count != 0)
				{
					base.StartCoroutine(this.AIAttack(unit));
				}
				else
				{
					if (unit.bNightFragrance)
					{
						unit.bNightFragrance = false;
					}
					UnitControl.MoveUnit(unit);
					if (!unit.attacked)
					{
						unit.RecoveHPMP(2);
					}
					this.unitInAction = false;
				}
			}
			else
			{
				this.NoActionForUnit(unit);
			}
		}
		else
		{
			this.NoActionForUnit(unit);
		}
		yield break;
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x0015005C File Offset: 0x0014E25C
	private IEnumerator AIAttack(UnitTB unit)
	{
		UnitTB targetUnit = unit.occupiedTile.AIHostileList[Random.Range(0, unit.occupiedTile.AIHostileList.Count)];
		UnitAbility[] uAb = new UnitAbility[unit.unitAbilityList.Count];
		int skillcount = 0;
		for (int i = 0; i < unit.unitAbilityList.Count; i++)
		{
			if (unit.IsAbilityAvailable(i) == 0)
			{
				List<Tile> tileList = unit.GetAbilityAttackAbleTiles(unit.occupiedTile, unit.unitAbilityList[i]);
				if (tileList.Contains(targetUnit.occupiedTile))
				{
					uAb[skillcount] = unit.unitAbilityList[i];
					skillcount++;
				}
			}
		}
		if (skillcount == 0)
		{
			this.unitInAction = false;
			Debug.Log("no Ability");
			yield break;
		}
		int selectAbility = Random.Range(0, skillcount);
		UnitAbility unitAbility = uAb[selectAbility];
		List<Tile> targetTileList = new List<Tile>();
		if (unitAbility.targetArea == _TargetArea.Line)
		{
			targetTileList.AddRange(GridManager.GetTileInLine(unit.occupiedTile, targetUnit.occupiedTile, unitAbility.range));
		}
		else if (unitAbility.targetArea == _TargetArea.Default)
		{
			if (unitAbility.requireTargetSelection)
			{
				targetTileList.Add(targetUnit.occupiedTile);
				if (unitAbility.aoeRange != 0)
				{
					targetTileList.AddRange(GridManager.GetTilesWithinRange(targetUnit.occupiedTile, unitAbility.aoeRange));
				}
			}
			else
			{
				targetTileList.Add(unit.occupiedTile);
				if (unitAbility.aoeRange != 0)
				{
					targetTileList.AddRange(GridManager.GetTilesWithinRange(unit.occupiedTile, unitAbility.aoeRange));
				}
			}
		}
		else if (unitAbility.targetArea == _TargetArea.Cone)
		{
			targetTileList.AddRange(GridManager.GetTileInCone60(unit.occupiedTile, targetUnit.occupiedTile, unitAbility.range));
		}
		else if (unitAbility.targetArea == _TargetArea.Fan1)
		{
			targetTileList.AddRange(GridManager.GetTileInFan1(unit.occupiedTile, targetUnit.occupiedTile, unitAbility.range));
		}
		Debug.Log(string.Concat(new string[]
		{
			"Round ",
			GameControlTB.roundCounter.ToString(),
			" ",
			unit.unitName,
			" ",
			unitAbility.name
		}));
		unit.AbilityActivated(unitAbility, false);
		unit.Attack(targetTileList, unitAbility);
		while (unit.InAction() || GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		this.unitInAction = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002B26 RID: 11046 RVA: 0x0001BF1A File Offset: 0x0001A11A
	private void NoActionForUnit(UnitTB unit)
	{
		GridManager.Deselect();
		UnitControl.MoveUnit(unit);
		unit.RecoveHPMP(1);
		this.unitInAction = false;
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x00150088 File Offset: 0x0014E288
	private List<Tile> WalkableTiles(UnitTB unit)
	{
		List<Tile> list = new List<Tile>();
		List<Tile> walkableTilesWithinRange = GridManager.GetWalkableTilesWithinRange(unit.occupiedTile, unit.GetMoveRange());
		if (unit.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt, null))
		{
			list.AddRange(walkableTilesWithinRange.ToArray());
		}
		else if (unit.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee, null))
		{
			list.AddRange(walkableTilesWithinRange.ToArray());
		}
		else if (!unit.GetMovementDisabled() && unit.aiMode != _AIMode.Stand)
		{
			list.AddRange(walkableTilesWithinRange.ToArray());
		}
		list.Add(unit.occupiedTile);
		return list;
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x0015011C File Offset: 0x0014E31C
	private void AnalyseAddAINode(UnitTB unit, Tile origTile, List<Tile> tileList, UnitAbility uab, ItemDataNode idn)
	{
		if (tileList == null)
		{
			return;
		}
		if (tileList.Count <= 0)
		{
			return;
		}
		List<UnitTB> list = new List<UnitTB>();
		AINode ainode = new AINode();
		float aitileRate = unit.GetAITileRate(origTile);
		float aitileValue = unit.GetAITileValue(origTile);
		ainode.unitAI = unit;
		ainode.tileMove = origTile;
		ainode.idn = idn;
		ainode.uAb = uab;
		ainode.iAbilityValue = Mathf.RoundToInt(aitileValue);
		ainode.targetTileList.AddRange(tileList.ToArray());
		foreach (Tile tile in tileList)
		{
			if (!(tile == null))
			{
				if (!(tile.unit == null))
				{
					if ((uab.effectType == _EffectType.Damage || uab.effectType == _EffectType.Debuff) && !unit.CheckFriendFaction(tile.unit.factionID))
					{
						UnitTB unitTB = tile.unit;
						if (tile.GetEffectPartAbsoluteBuff(_EffectPartType.Protect, tile.unit))
						{
							UnitTB effectPartUnit = tile.GetEffectPartUnit(_EffectPartType.Protect);
							if (effectPartUnit != null && !list.Contains(effectPartUnit) && !tileList.Contains(effectPartUnit.occupiedTile))
							{
								unitTB = effectPartUnit;
								list.Add(effectPartUnit);
							}
						}
						if (!unitTB.GetEffectPartAbsoluteBuff(_EffectPartType.HitPoint, null) || unitTB.GetEffectPartAbsoluteBuffOnHitCount(_EffectPartType.HitPoint) != 0)
						{
							if (!tile.GetEffectPartAbsoluteBuff(_EffectPartType.HitPoint, tile.unit) || tile.GetEffectPartAbsoluteBuffOnHitCount(_EffectPartType.HitPoint, tile.unit) != 0)
							{
								if (!tile.unit.bStealth)
								{
									ainode.iAbilityTargetCount++;
								}
								UnitAbility uab2 = uab.Clone();
								unit.CheckTalentPosition(unitTB, uab2, origTile, false);
								int num = unit.GetAbilityToTargetDamage(unitTB, uab2, origTile);
								float abilityToTargetHitRate = unit.GetAbilityToTargetHitRate(unitTB, uab2, origTile);
								float num2 = unit.GetAbilityToTargetCriticalRate(unitTB, uab2, origTile) * 0.5f + 1f;
								float num3;
								if (unit.aiMode == _AIMode.Protect && unit.aiTarget != null)
								{
									num3 = unit.aiTarget.GetThreatUnitValue(tile.unit);
								}
								else
								{
									num3 = unit.GetThreatUnitValue(tile.unit);
								}
								float num4 = abilityToTargetHitRate * num2 * (float)num;
								num = Mathf.RoundToInt(num4);
								float num5;
								if (unitTB.GetUnitHP() < num)
								{
									ainode.iDeadCount++;
									num5 = 1.5f;
								}
								else
								{
									num5 = 1f;
								}
								num3 += aitileRate;
								num4 = abilityToTargetHitRate * num2 * (float)num * num3 * num5;
								num = Mathf.RoundToInt(num4);
								ainode.iAbilityValue += num;
							}
						}
					}
					else if ((uab.effectType == _EffectType.Heal || uab.effectType == _EffectType.Buff) && unit.CheckFriendFaction(tile.unit.factionID))
					{
						UnitAbility unitAbility = uab.Clone();
						unit.CheckTalentPosition(tile.unit, unitAbility, origTile, false);
						float num4 = 0f;
						int num6 = GridManager.Distance(origTile, tile.unit.occupiedTile);
						int abilityToTargetDamage = unit.GetAbilityToTargetDamage(tile.unit, unitAbility, origTile);
						float num7 = unit.GetUnitAbilityValue(_EffectPartType.InternalForce, unitAbility, num6, false);
						float num8 = num7;
						num7 = 0.01f * (float)tile.unit.fullSP * unit.GetUnitAbilityValuePercent(_EffectPartType.InternalForce, unitAbility, num6, false);
						num8 += num7;
						int num9 = Mathf.RoundToInt(num8);
						int num10 = tile.unit.fullHP - tile.unit.HP;
						int num11 = tile.unit.fullSP - tile.unit.SP;
						int debuffCount = tile.unit.GetDebuffCount();
						int num12;
						if (unit.GetUnitAbilityAbsoluteBuff(_EffectPartType.Cleanup, unitAbility))
						{
							num12 = 99;
						}
						else
						{
							num12 = Mathf.RoundToInt(unit.GetUnitAbilityValue(_EffectPartType.Cleanup, unitAbility, num6, true));
						}
						float num13 = (float)tile.unit.HP;
						float num14 = (float)tile.unit.SP;
						num13 /= (float)tile.unit.fullHP;
						num14 /= (float)tile.unit.fullSP;
						float num3 = 1f + aitileRate;
						float num2 = unit.GetAbilityCriticalRate(unitAbility, num6, unit.occupiedTile) * 0.5f + 1f;
						if (num13 < 0.5f)
						{
							num4 += num2 * (1f - num13) * (float)Mathf.Min(num10, abilityToTargetDamage) * num3;
						}
						if (num14 < 0.5f)
						{
							num4 += (1f - num14) * (float)Mathf.Min(num11, num9) * num3;
						}
						num4 += 150f * (float)Mathf.Min(debuffCount, num12) * num3;
						int num = Mathf.RoundToInt(num4);
						ainode.iAbilityValue += num;
						ainode.iAbilityTargetCount++;
					}
				}
			}
		}
		if (ainode.iAbilityTargetCount > 0 && ainode.iAbilityValue > 0)
		{
			this.nodeList.Add(ainode);
		}
	}

	// Token: 0x06002B29 RID: 11049 RVA: 0x00150654 File Offset: 0x0014E854
	private void AnalyseItemFormTile(UnitTB unit, Tile tile, ItemDataNode idn)
	{
		List<Tile> list = new List<Tile>();
		int skillID = idn.GetSkillID();
		if (skillID != 0)
		{
			UnitAbility unitAbility = AbilityManagerTB.GetUnitAbility(skillID);
			if (unitAbility != null)
			{
				if (unitAbility.targetArea == _TargetArea.Default)
				{
					if (unitAbility.requireTargetSelection)
					{
						List<Tile> list2 = new List<Tile>();
						list2.Add(tile);
						list2.AddRange(GridManager.GetTilesWithinRange(tile, unitAbility.range));
						foreach (Tile tile2 in list2)
						{
							list.Clear();
							list.Add(tile2);
							if (unitAbility.aoeRange != 0)
							{
								list.AddRange(GridManager.GetTilesWithinRange(tile2, unitAbility.aoeRange));
							}
							this.AnalyseAddAINode(unit, tile, list, unitAbility, idn);
						}
					}
					else
					{
						list.Add(tile);
						if (unitAbility.aoeRange != 0)
						{
							list.AddRange(GridManager.GetTilesWithinRange(tile, unitAbility.aoeRange));
						}
						this.AnalyseAddAINode(unit, tile, list, unitAbility, idn);
					}
				}
				else
				{
					foreach (Tile targetTile in tile.neighbours)
					{
						list.Clear();
						if (unitAbility.targetArea == _TargetArea.Line)
						{
							list.AddRange(GridManager.GetTileInLine(tile, targetTile, unitAbility.range));
						}
						else if (unitAbility.targetArea == _TargetArea.Cone)
						{
							list.AddRange(GridManager.GetTileInCone60(tile, targetTile, unitAbility.range));
						}
						else if (unitAbility.targetArea == _TargetArea.Fan1)
						{
							list.AddRange(GridManager.GetTileInFan1(tile, targetTile, unitAbility.range));
						}
						this.AnalyseAddAINode(unit, tile, list, unitAbility, idn);
					}
				}
			}
		}
		else
		{
			AINode ainode = new AINode();
			ainode.unitAI = unit;
			ainode.tileMove = tile;
			ainode.idn = idn;
			ainode.uAb = null;
			ainode.targetTileList.Add(tile);
			ainode.iAbilityValue = unit.AIUseItemSelfScore(idn);
			this.nodeList.Add(ainode);
		}
	}

	// Token: 0x06002B2A RID: 11050 RVA: 0x00150880 File Offset: 0x0014EA80
	private void AnalyseAbilityFormTile(UnitTB unit, Tile tile, UnitAbility uab)
	{
		List<Tile> list = new List<Tile>();
		bool flag = false;
		Tile tile2 = null;
		if (unit.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt, null))
		{
			UnitTB unitTB = UnitControl.instance.FindUnit(unit.FindConditionEffectPartID(_EffectPartType.Taunt));
			if (unitTB != null)
			{
				flag = true;
				tile2 = unitTB.occupiedTile;
			}
		}
		if (uab.targetArea == _TargetArea.Default)
		{
			if (uab.requireTargetSelection)
			{
				List<Tile> list2 = new List<Tile>();
				list2.Add(tile);
				list2.AddRange(GridManager.GetTilesWithinRange(tile, uab.range));
				foreach (Tile tile3 in list2)
				{
					list.Clear();
					list.Add(tile3);
					if (uab.aoeRange != 0)
					{
						list.AddRange(GridManager.GetTilesWithinRange(tile3, uab.aoeRange));
					}
					if (!flag || list.Contains(tile2))
					{
						this.AnalyseAddAINode(unit, tile, list, uab, null);
					}
				}
			}
			else
			{
				list.Add(tile);
				if (uab.aoeRange != 0)
				{
					list.AddRange(GridManager.GetTilesWithinRange(tile, uab.aoeRange));
				}
				if (flag && !list.Contains(tile2))
				{
					return;
				}
				this.AnalyseAddAINode(unit, tile, list, uab, null);
			}
		}
		else
		{
			foreach (Tile targetTile in tile.neighbours)
			{
				list.Clear();
				if (uab.targetArea == _TargetArea.Line)
				{
					list.AddRange(GridManager.GetTileInLine(tile, targetTile, uab.range));
				}
				else if (uab.targetArea == _TargetArea.Cone)
				{
					list.AddRange(GridManager.GetTileInCone60(tile, targetTile, uab.range));
				}
				else if (uab.targetArea == _TargetArea.Fan1)
				{
					list.AddRange(GridManager.GetTileInFan1(tile, targetTile, uab.range));
				}
				if (!flag || list.Contains(tile2))
				{
					this.AnalyseAddAINode(unit, tile, list, uab, null);
				}
			}
		}
	}

	// Token: 0x06002B2B RID: 11051 RVA: 0x00150ABC File Offset: 0x0014ECBC
	public void AnalyseUnit(UnitTB unit)
	{
		List<Tile> list = this.WalkableTiles(unit);
		unit.CheckAI();
		if (list == null)
		{
			return;
		}
		if (list.Count <= 0)
		{
			return;
		}
		foreach (Tile tile in list)
		{
			if (!(tile == null))
			{
				float num = unit.GetAITileValue(tile);
				float num2 = unit.GetAITileRate(tile);
				float threatUnitValue = unit.GetThreatUnitValue(null);
				num2 += threatUnitValue;
				if (tile == unit.occupiedTile)
				{
					num += num2 * unit.GetRecoveHPMPValue(1);
				}
				else
				{
					num += num2 * unit.GetRecoveHPMPValue(2);
				}
				AINode ainode = new AINode();
				ainode.unitAI = unit;
				ainode.tileMove = tile;
				ainode.iAbilityValue = Mathf.RoundToInt(num);
				this.nodeList.Add(ainode);
				if (!unit.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee, null))
				{
					if (unit.useItemCD == 0 && !unit.attacked && !unit.GetEffectPartAbsoluteDebuff(_EffectPartType.Giddy, null) && !unit.GetEffectPartAbsoluteDebuff(_EffectPartType.Stun, null) && GameGlobal.m_iBattleDifficulty > 3)
					{
						for (int i = 0; i < unit.characterData.Itemlist.Count; i++)
						{
							int iItemID = unit.characterData.Itemlist[i].m_iItemID;
							ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(iItemID);
							if (itemDataNode != null)
							{
								if (itemDataNode.m_iUseTime == 2)
								{
									this.AnalyseItemFormTile(unit, tile, itemDataNode);
								}
							}
						}
					}
					for (int j = 0; j < unit.unitAbilityList.Count; j++)
					{
						if (unit.unitAbilityList[j] != null)
						{
							if (unit.IsAbilityAvailable(j) == 0)
							{
								this.AnalyseAbilityFormTile(unit, tile, unit.unitAbilityList[j]);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x0001BF35 File Offset: 0x0001A135
	private static int CompareFunc(AINode node_a, AINode node_b)
	{
		if (node_a.iAbilityValue > node_b.iAbilityValue)
		{
			return -1;
		}
		if (node_a.iAbilityValue < node_b.iAbilityValue)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x00150CF0 File Offset: 0x0014EEF0
	private IEnumerator _MoveUnit2(UnitTB unit)
	{
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		if (unit == null || unit.IsDestroyed() || unit.occupiedTile == null)
		{
			this.unitInAction = false;
			yield break;
		}
		GridManager.AISelect(unit.occupiedTile);
		yield return new WaitForSeconds(0.5f);
		bool move = false;
		if (!unit.IsStunned())
		{
			this.nodeList.Clear();
			this.AnalyseUnit(unit);
			if (this.nodeList.Count > 0)
			{
				this.nodeList.Sort(new Comparison<AINode>(AIManager.CompareFunc));
			}
			if (this.nodeList.Count > 0)
			{
				AINode node = this.nodeList[0];
				if (node.tileMove != unit.occupiedTile)
				{
					unit.Move(node.tileMove);
					while (unit.InAction() || GameControlTB.IsActionInProgress())
					{
						yield return null;
					}
					move = true;
				}
				if (node.iAbilityTargetCount > 0)
				{
					if (node.idn == null)
					{
						node.unitAI.AbilityActivated(node.uAb, false);
					}
					else
					{
						node.unitAI.AI_ItemActivated(node.idn);
					}
					unit.Attack(node.targetTileList, node.uAb);
					while (unit.InAction() || GameControlTB.IsActionInProgress())
					{
						yield return null;
					}
					this.unitInAction = false;
					yield return null;
				}
				else if (node.idn != null)
				{
					node.unitAI.AI_ItemActivated(node.idn);
					node.unitAI.AI_ActivateUseItemSelf(node.idn);
					while (node.unitAI.InAction() || GameControlTB.IsActionInProgress())
					{
						yield return null;
					}
					this.unitInAction = false;
					yield return null;
				}
				else
				{
					if (unit.bNightFragrance)
					{
						unit.bNightFragrance = false;
					}
					UnitControl.MoveUnit(unit);
					if (!unit.attacked)
					{
						if (move)
						{
							unit.RecoveHPMP(2);
						}
						else
						{
							unit.RecoveHPMP(1);
						}
					}
					this.unitInAction = false;
				}
			}
			else
			{
				this.NoActionForUnit(unit);
			}
		}
		else
		{
			this.NoActionForUnit(unit);
		}
		yield break;
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x00150D1C File Offset: 0x0014EF1C
	private IEnumerator _MoveUnit3(AINode node)
	{
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		if (node.unitAI == null || node.unitAI.IsDestroyed() || node.unitAI.occupiedTile == null)
		{
			this.unitInAction = false;
			yield break;
		}
		if (node.tileMove != node.unitAI.occupiedTile || node.iAbilityTargetCount > 0)
		{
			GridManager.AISelect(node.unitAI.occupiedTile);
			yield return new WaitForSeconds(0.5f);
		}
		bool move = false;
		if (!node.unitAI.IsStunned())
		{
			if (node.tileMove != node.unitAI.occupiedTile)
			{
				node.unitAI.Move(node.tileMove);
				while (node.unitAI.InAction() || GameControlTB.IsActionInProgress())
				{
					yield return null;
				}
				move = true;
			}
			if (node.iAbilityTargetCount > 0)
			{
				if (node.idn == null)
				{
					node.unitAI.AbilityActivated(node.uAb, false);
				}
				else
				{
					node.unitAI.AI_ItemActivated(node.idn);
					node.uAb = node.uAb.Clone();
					node.uAb.bItemSkill = true;
				}
				node.unitAI.Attack(node.targetTileList, node.uAb);
				while (node.unitAI.InAction() || GameControlTB.IsActionInProgress())
				{
					yield return null;
				}
				this.unitInAction = false;
				yield return null;
			}
			else if (node.idn != null)
			{
				node.unitAI.AI_ItemActivated(node.idn);
				node.unitAI.AI_ActivateUseItemSelf(node.idn);
				while (node.unitAI.InAction() || GameControlTB.IsActionInProgress())
				{
					yield return null;
				}
				this.unitInAction = false;
				yield return null;
			}
			else
			{
				if (node.unitAI.bNightFragrance)
				{
					node.unitAI.bNightFragrance = false;
				}
				UnitControl.MoveUnit(node.unitAI);
				if (!node.unitAI.attacked)
				{
					if (move)
					{
						node.unitAI.RecoveHPMP(2);
					}
					else
					{
						node.unitAI.RecoveHPMP(1);
					}
				}
				this.unitInAction = false;
			}
		}
		else
		{
			this.NoActionForUnit(node.unitAI);
		}
		yield break;
	}

	// Token: 0x04003783 RID: 14211
	public static AIManager instance;

	// Token: 0x04003784 RID: 14212
	public float delayBetweenUnit = 0.25f;

	// Token: 0x04003785 RID: 14213
	public _AIStance aIStance;

	// Token: 0x04003786 RID: 14214
	private List<AINode> nodeList = new List<AINode>();

	// Token: 0x04003787 RID: 14215
	public bool unitInAction;
}
