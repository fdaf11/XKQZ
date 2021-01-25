using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x02000785 RID: 1925
[RequireComponent(typeof(UnitTBAnimation))]
[RequireComponent(typeof(UnitTBAudio))]
public class UnitTB : MonoBehaviour
{
	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06002DFD RID: 11773 RVA: 0x0001D722 File Offset: 0x0001B922
	// (remove) Token: 0x06002DFE RID: 11774 RVA: 0x0001D739 File Offset: 0x0001B939
	public static event UnitTB.UnitSelectedHandler onUnitSelectedE;

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06002DFF RID: 11775 RVA: 0x0001D750 File Offset: 0x0001B950
	// (remove) Token: 0x06002E00 RID: 11776 RVA: 0x0001D767 File Offset: 0x0001B967
	public static event UnitTB.UnitDeselectedHandler onUnitDeselectedE;

	// Token: 0x14000056 RID: 86
	// (add) Token: 0x06002E01 RID: 11777 RVA: 0x0001D77E File Offset: 0x0001B97E
	// (remove) Token: 0x06002E02 RID: 11778 RVA: 0x0001D795 File Offset: 0x0001B995
	public static event UnitTB.ActionCompletedHandler onActionCompletedE;

	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06002E03 RID: 11779 RVA: 0x0001D7AC File Offset: 0x0001B9AC
	// (remove) Token: 0x06002E04 RID: 11780 RVA: 0x0001D7C3 File Offset: 0x0001B9C3
	public static event UnitTB.NewPositionHandler onNewPositionE;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06002E05 RID: 11781 RVA: 0x0001D7DA File Offset: 0x0001B9DA
	// (remove) Token: 0x06002E06 RID: 11782 RVA: 0x0001D7F1 File Offset: 0x0001B9F1
	public static event UnitTB.TurnDepletedHandler onTurnDepletedE;

	// Token: 0x14000059 RID: 89
	// (add) Token: 0x06002E07 RID: 11783 RVA: 0x0001D808 File Offset: 0x0001BA08
	// (remove) Token: 0x06002E08 RID: 11784 RVA: 0x0001D81F File Offset: 0x0001BA1F
	public static event UnitTB.UnitDestroyedHandler onUnitDestroyedE;

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x06002E09 RID: 11785 RVA: 0x0001D836 File Offset: 0x0001BA36
	// (remove) Token: 0x06002E0A RID: 11786 RVA: 0x0001D84D File Offset: 0x0001BA4D
	public static event UnitTB.EffectAppliedHandler onEffectAppliedE;

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06002E0B RID: 11787 RVA: 0x0001D864 File Offset: 0x0001BA64
	// (remove) Token: 0x06002E0C RID: 11788 RVA: 0x0001D87B File Offset: 0x0001BA7B
	public static event UnitTB.EffectExpiredHandler onEffectExpiredE;

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06002E0D RID: 11789 RVA: 0x0001D892 File Offset: 0x0001BA92
	// (remove) Token: 0x06002E0E RID: 11790 RVA: 0x0001D8A9 File Offset: 0x0001BAA9
	public static event UnitTB.ActorAttack onAttackS;

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06002E0F RID: 11791 RVA: 0x0001D8C0 File Offset: 0x0001BAC0
	// (remove) Token: 0x06002E10 RID: 11792 RVA: 0x0001D8D7 File Offset: 0x0001BAD7
	public static event UnitTB.ActorAttack onAttackE;

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06002E11 RID: 11793 RVA: 0x0001D8EE File Offset: 0x0001BAEE
	// (remove) Token: 0x06002E12 RID: 11794 RVA: 0x0001D905 File Offset: 0x0001BB05
	public static event UnitTB.DamageDone onDamage;

	// Token: 0x06002E13 RID: 11795 RVA: 0x0001D91C File Offset: 0x0001BB1C
	public void SetSpawnInGameFlag(bool flag)
	{
		this.spawnedInGame = flag;
	}

	// Token: 0x06002E14 RID: 11796 RVA: 0x0001D925 File Offset: 0x0001BB25
	public bool GetSpawnInGameFlag()
	{
		return this.spawnedInGame;
	}

	// Token: 0x06002E15 RID: 11797 RVA: 0x0001D92D File Offset: 0x0001BB2D
	public void SetSpawnDuration(int duration)
	{
		this.spawnedLastDuration = duration;
	}

	// Token: 0x06002E16 RID: 11798 RVA: 0x00162010 File Offset: 0x00160210
	private void Awake()
	{
		this.backupLayer = base.gameObject.layer;
		this.thisT = base.transform;
		this.thisObj = base.gameObject;
		this.animationTB = this.thisObj.GetComponent<UnitTBAnimation>();
		this.audioTB = this.thisObj.GetComponent<UnitTBAudio>();
	}

	// Token: 0x06002E17 RID: 11799 RVA: 0x00162068 File Offset: 0x00160268
	private void Start()
	{
		this.fLastNumberTime = 0f;
		if (GameControlTB.IsPlayerFaction(this.factionID))
		{
			this.thisObj.layer = LayerManager.GetLayerUnit();
		}
		else if (GameControlTB.EnableFogOfWar())
		{
			this.SetToInvisible();
		}
		else
		{
			this.SetToVisible();
		}
		this.iDeadCount = 1;
		this.bCheckTalentDeadthNow = false;
		this.bNightFragrance = false;
		this.HP = this.GetFullHP();
		if (GameControlTB.FullAPOnStart())
		{
			this.GainSP(this.GetFullSP());
		}
		else
		{
			this.SP = (int)((float)this.GetFullSP() * GameControlTB.APOnStartPercent());
		}
		this.turnPriority = Mathf.Max(1, this.turnPriority);
		this.InitAbility();
		this.InitNeigong();
		this.InitSPEffect();
		this.UpdateEffectCompare(_EffectPartType.HitPoint, this.HP, this.GetFullHP());
		if (this.movementRange <= 0)
		{
			this.moved = false;
		}
		if (this.attackRangeMin <= 0 && this.attackRangeMax <= 0)
		{
			this.attacked = false;
		}
		this.InitShootPoint();
		if (this.turretObject == null)
		{
			this.turretObject = this.thisT;
		}
		this.ClearSelectedAbility();
		this.bSteal = false;
	}

	// Token: 0x06002E18 RID: 11800 RVA: 0x001621AC File Offset: 0x001603AC
	public int GetRoutineLv(int iRoutine)
	{
		for (int i = 0; i < this.characterData.RoutineList.Count; i++)
		{
			if (this.characterData.RoutineList[i].iSkillID == iRoutine)
			{
				return this.characterData.RoutineList[i].iLevel;
			}
		}
		return 0;
	}

	// Token: 0x06002E19 RID: 11801 RVA: 0x00162210 File Offset: 0x00160410
	public int GetRoutineMartialType(int iRoutine)
	{
		RoutineNewDataNode routineNewData = Game.RoutineNewData.GetRoutineNewData(iRoutine);
		if (routineNewData == null)
		{
			return 0;
		}
		return (int)routineNewData.m_RoutineType;
	}

	// Token: 0x06002E1A RID: 11802 RVA: 0x00162238 File Offset: 0x00160438
	public int GetRoutineMartialAttack(int iRoutine)
	{
		int num = this.GetRoutineMartialType(iRoutine);
		if (num <= 0)
		{
			return 1;
		}
		num += 20;
		return this.characterData._TotalProperty._MartialArts.Get((CharacterData.PropertyType)num);
	}

	// Token: 0x06002E1B RID: 11803 RVA: 0x00162274 File Offset: 0x00160474
	public int GetRoutineMartialDef(int iRoutine)
	{
		int num = this.GetRoutineMartialType(iRoutine);
		if (num <= 0)
		{
			return 0;
		}
		num += 40;
		return this.characterData._MartialDef.Get((CharacterData.PropertyType)num);
	}

	// Token: 0x06002E1C RID: 11804 RVA: 0x0001D936 File Offset: 0x0001BB36
	public int GetMartialTypeDef(int iType)
	{
		if (iType <= 0)
		{
			return 0;
		}
		iType += 40;
		return this.characterData._MartialDef.Get((CharacterData.PropertyType)iType);
	}

	// Token: 0x06002E1D RID: 11805 RVA: 0x001622A8 File Offset: 0x001604A8
	private void InitShootPoint()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < this.shootPoints.Count; i++)
		{
			if (this.shootPoints[i] != null)
			{
				list.Add(this.shootPoints[i]);
			}
		}
		this.shootPoints = list;
		if (this.shootPoints.Count == 0)
		{
			this.shootPoints.Add(this.thisT);
		}
	}

	// Token: 0x06002E1E RID: 11806 RVA: 0x00162328 File Offset: 0x00160528
	private void InitAbility()
	{
		this.unitAbilityList.Clear();
		foreach (int id in this.abilityIDList)
		{
			UnitAbility unitAbility = AbilityManagerTB.GetUnitAbility(id);
			if (unitAbility != null)
			{
				this.unitAbilityList.Add(unitAbility);
				this.unitAbilityList[this.unitAbilityList.Count - 1].factionID = this.factionID;
				this.unitAbilityList[this.unitAbilityList.Count - 1].cooldown = 0;
			}
		}
	}

	// Token: 0x06002E1F RID: 11807 RVA: 0x001623E0 File Offset: 0x001605E0
	private void InitSPEffect()
	{
		if (this.characterData == null)
		{
			return;
		}
		if (this.characterData._TotalProperty.Get(CharacterData.PropertyType.Strength) >= this.iSPEffectValue)
		{
			this.iShockValue = this.characterData._TotalProperty.Get(CharacterData.PropertyType.Strength) * 10;
		}
		if (this.characterData._TotalProperty.Get(CharacterData.PropertyType.Constitution) >= this.iSPEffectValue)
		{
			this.iDamageAbsorbToSPValue = this.characterData._TotalProperty.Get(CharacterData.PropertyType.Constitution) * 10;
		}
		if (this.characterData._TotalProperty.Get(CharacterData.PropertyType.Dexterity) >= this.iSPEffectValue)
		{
			this.iRemoveFlowerValue = this.characterData._TotalProperty.Get(CharacterData.PropertyType.Dexterity) * 10;
		}
		if (this.characterData._TotalProperty.Get(CharacterData.PropertyType.Intelligence) >= this.iSPEffectValue)
		{
			this.iClearCDValue = this.characterData._TotalProperty.Get(CharacterData.PropertyType.Intelligence) * 10;
		}
	}

	// Token: 0x06002E20 RID: 11808 RVA: 0x001624DC File Offset: 0x001606DC
	public bool Calculate4Measurements(CharacterData.PropertyType type, ref int iVal)
	{
		if (GameGlobal.m_bDLCMode)
		{
			return false;
		}
		int num = this.characterData._TotalProperty.Get(type);
		if (num < this.iSPEffectValue)
		{
			return false;
		}
		if (Random.Range(0, this.iMaxEffect) < iVal)
		{
			iVal = 0;
			return true;
		}
		iVal += num;
		return false;
	}

	// Token: 0x06002E21 RID: 11809 RVA: 0x00162534 File Offset: 0x00160734
	private void OnEnable()
	{
		GameControlTB.onNewRoundE += this.OnNewRound;
		GameControlTB.onUnitNextTurnE += this.OnNextTurn;
		UnitTB.onNewPositionE = (UnitTB.NewPositionHandler)Delegate.Combine(UnitTB.onNewPositionE, new UnitTB.NewPositionHandler(this.OnCheckFogOfWar));
		UnitTB.onUnitDestroyedE = (UnitTB.UnitDestroyedHandler)Delegate.Combine(UnitTB.onUnitDestroyedE, new UnitTB.UnitDestroyedHandler(this.OnCheckFogOfWar));
	}

	// Token: 0x06002E22 RID: 11810 RVA: 0x001625A4 File Offset: 0x001607A4
	private void OnDisable()
	{
		GameControlTB.onNewRoundE -= this.OnNewRound;
		GameControlTB.onUnitNextTurnE -= this.OnNextTurn;
		UnitTB.onNewPositionE = (UnitTB.NewPositionHandler)Delegate.Remove(UnitTB.onNewPositionE, new UnitTB.NewPositionHandler(this.OnCheckFogOfWar));
		UnitTB.onUnitDestroyedE = (UnitTB.UnitDestroyedHandler)Delegate.Remove(UnitTB.onUnitDestroyedE, new UnitTB.UnitDestroyedHandler(this.OnCheckFogOfWar));
	}

	// Token: 0x06002E23 RID: 11811 RVA: 0x00162614 File Offset: 0x00160814
	public void OnNewRound(int roundCounter)
	{
		if (this.occupiedTile == null)
		{
			return;
		}
		if (this.spawnedInGame && this.spawnedLastDuration > 0)
		{
			this.spawnedLastDuration--;
			if (this.spawnedLastDuration <= 0)
			{
				base.StartCoroutine(this.Destroyed());
			}
		}
		this.CalculateNeigong();
		this.CalculateAura();
		this.ClearAllFlag();
		this.ApplyEquipCondition();
		if (this.useItemCD > 0)
		{
			this.useItemCD--;
		}
	}

	// Token: 0x06002E24 RID: 11812 RVA: 0x0001D958 File Offset: 0x0001BB58
	public void ClearAllFlag()
	{
		this.iDeadPlus = 0;
		this.iAttackPlus = 0;
		this.moved = false;
		this.attacked = false;
		this.abilityTriggered = false;
		this.bActionPlayerConfuse = false;
		this.bNightFragrance = false;
		this.actionQueued = 0;
	}

	// Token: 0x06002E25 RID: 11813 RVA: 0x0001D992 File Offset: 0x0001BB92
	public void Select()
	{
		if (UnitTB.onUnitSelectedE != null)
		{
			UnitTB.onUnitSelectedE(this);
		}
	}

	// Token: 0x06002E26 RID: 11814 RVA: 0x001626A4 File Offset: 0x001608A4
	public void Deselect()
	{
		if (this.bIsMoving)
		{
			this.audioTB.StopMove();
			this.animationTB.StopMove();
			this.bIsMoving = false;
			this.ClearMovePath();
			this.moveOrigTile = null;
		}
		if (UnitTB.onUnitDeselectedE != null)
		{
			UnitTB.onUnitDeselectedE();
		}
	}

	// Token: 0x06002E27 RID: 11815 RVA: 0x0001D9A9 File Offset: 0x0001BBA9
	public bool IsControllable()
	{
		return this.factionID == GameControlTB.GetCurrentPlayerFactionID() && !UnitControl.bTauntFlee;
	}

	// Token: 0x06002E28 RID: 11816 RVA: 0x001626FC File Offset: 0x001608FC
	public bool Move(Tile newTile)
	{
		if (this.moved && this.stun > 0)
		{
			return false;
		}
		if (this.GetMovementDisabled())
		{
			return false;
		}
		this.moveRemain--;
		if (this.moveRemain <= 0)
		{
			this.moved = true;
		}
		this.actionQueued++;
		if (this.bNightFragrance)
		{
			UINGUI.OnEndTurn();
			UINGUI.instance.uiAbilityButtons.Hide();
			UINGUI.instance.uiHUD.HideControlUnitInfo();
		}
		base.StartCoroutine(this.MoveRoutine(this.occupiedTile, newTile));
		return true;
	}

	// Token: 0x06002E29 RID: 11817 RVA: 0x001627A4 File Offset: 0x001609A4
	public void BackToOrigTile()
	{
		if (!this.bIsMoving)
		{
			return;
		}
		this.bIsMoving = false;
		this.audioTB.StopMove();
		this.animationTB.StopMove();
		GridManager.Deselect();
		this.moveRemain++;
		if (this.moveRemain > 0)
		{
			this.moved = false;
		}
		this.ResetTilePostion(this.moveOrigTile);
		this.thisT.rotation = this.beforeMoveRot;
		this.Select();
		GridManager.Select(this.occupiedTile);
	}

	// Token: 0x06002E2A RID: 11818 RVA: 0x00162830 File Offset: 0x00160A30
	public void ClearMovePath()
	{
		foreach (Tile tile in this.movingPathList)
		{
			tile.SetState(_TileState.Default);
		}
		this.movingPathList.Clear();
	}

	// Token: 0x06002E2B RID: 11819 RVA: 0x00162898 File Offset: 0x00160A98
	public void ResetTilePostion(Tile newTile)
	{
		if (!this.IsDestroyed())
		{
			if (this.occupiedTile != null)
			{
				this.occupiedTile.ClearUnit();
			}
			if (newTile.unit != null)
			{
				Debug.Log(string.Concat(new string[]
				{
					"NewTile ",
					newTile.name,
					" have ",
					newTile.unit.unitName,
					" ",
					this.unitName,
					" want to stand there"
				}));
				UnitTB unit = newTile.unit;
				newTile.ClearUnit();
				unit.occupiedTile = null;
			}
			this.occupiedTile = newTile;
			this.thisT.position = newTile.pos;
			newTile.SetUnit(this);
		}
	}

	// Token: 0x06002E2C RID: 11820 RVA: 0x00162960 File Offset: 0x00160B60
	private IEnumerator MoveRoutine(Tile origin, Tile target)
	{
		float fFlyLevel = this.GetFlyOnGrass();
		List<Tile> path = AStar.SearchWalkableTile2(origin, target, this.factionID, fFlyLevel);
		while (!GameControlTB.ActionCommenced())
		{
			yield return null;
		}
		Debug.LogWarning(this.unitName + " MoveRoutine actionInProgress = true");
		this.beforeMoveRot = this.thisT.rotation;
		this.moveOrigTile = origin;
		origin.ClearUnit();
		GridManager.Deselect();
		this.audioTB.PlayMove();
		this.animationTB.PlayMove();
		while (path.Count > 1)
		{
			Quaternion wantedRot = Quaternion.LookRotation(path[1].pos - path[0].pos);
			float fovertime = 0f;
			for (;;)
			{
				fovertime += Time.deltaTime;
				float frot = Time.deltaTime * this.rotateSpeed;
				frot = Mathf.Clamp(frot, 0.05f, 1f);
				this.thisT.rotation = Quaternion.Slerp(this.thisT.rotation, wantedRot, frot);
				if (Quaternion.Angle(this.thisT.rotation, wantedRot) < 2f || fovertime > 0.5f)
				{
					break;
				}
				yield return null;
			}
			this.thisT.rotation = wantedRot;
			this.movingPathList.Add(path[0]);
			path[0].SetState(_TileState.Walkable);
			path.RemoveAt(0);
			if (path[0].unit != null)
			{
				path[0].unit.animationTB.PlayDodge();
			}
			fovertime = 0f;
			for (;;)
			{
				fovertime += Time.deltaTime;
				float dist = Vector3.Distance(this.thisT.position, path[0].pos);
				if (dist < Time.deltaTime * this.moveSpeed || dist < 0.1f || fovertime > 0.5f)
				{
					break;
				}
				Vector3 vPosDiff = path[0].pos - this.thisT.position;
				vPosDiff.Normalize();
				vPosDiff = vPosDiff * Time.deltaTime * this.moveSpeed;
				this.thisT.position = this.thisT.position + vPosDiff;
				yield return null;
			}
			this.thisT.position = path[0].pos;
			this.occupiedTile = path[0];
			if (GameControlTB.EnableFogOfWar())
			{
				if (this.factionID == GameControlTB.GetPlayerFactionID())
				{
					if (UnitTB.onNewPositionE != null)
					{
						UnitTB.onNewPositionE(this);
					}
				}
				else
				{
					this.AIUnitCheckFogOfWar();
				}
			}
		}
		this.occupiedTile = target;
		this.occupiedTile.SetUnit(this);
		this.thisT.rotation = Quaternion.Euler(0f, this.thisT.rotation.eulerAngles.y, 0f);
		yield return new WaitForSeconds(0.25f);
		this.actionQueued--;
		if (this.actionQueued <= 0)
		{
			this.actionQueued = 0;
			base.StartCoroutine(this.ActionComplete(0.05f));
		}
		if (this.bTileEventCantReMove)
		{
			this.audioTB.StopMove();
			this.animationTB.StopMove();
			this.bIsMoving = false;
			this.ClearMovePath();
			this.moveOrigTile = null;
			this.bTileEventCantReMove = false;
		}
		else
		{
			this.bIsMoving = true;
		}
		if (this.bNightFragrance)
		{
			this.bNightFragrance = false;
			UnitControl.MoveUnit(this);
		}
		else
		{
			UINGUI.instance.DelayBattleControlState(BattleControlState.SelectAction);
		}
		yield break;
	}

	// Token: 0x06002E2D RID: 11821 RVA: 0x00162998 File Offset: 0x00160B98
	public List<Tile> FilterTileByAbilityTargetType(List<Tile> targetTileList, UnitAbility ability)
	{
		if (ability == null)
		{
			return targetTileList;
		}
		for (int i = 0; i < targetTileList.Count; i++)
		{
			Tile tile = targetTileList[i];
			if (ability.targetType == _AbilityTargetType.AllUnits && (tile.unit == null || tile.unit.IsDestroyed() || tile.unit.bStealth))
			{
				targetTileList.RemoveAt(i);
				i--;
			}
			else if (ability.targetType == _AbilityTargetType.Friendly && (tile.unit == null || !this.CheckFriendFaction(tile.unit.factionID) || tile.unit.IsDestroyed()))
			{
				targetTileList.RemoveAt(i);
				i--;
			}
			else if (ability.targetType == _AbilityTargetType.Hostile && (tile.unit == null || this.CheckFriendFaction(tile.unit.factionID) || tile.unit.IsDestroyed() || tile.unit.bStealth))
			{
				targetTileList.RemoveAt(i);
				i--;
			}
			else if (ability.targetType == _AbilityTargetType.EmptyTile && (tile.unit != null || tile.unit.IsDestroyed()))
			{
				targetTileList.RemoveAt(i);
				i--;
			}
		}
		return targetTileList;
	}

	// Token: 0x06002E2E RID: 11822 RVA: 0x00162B0C File Offset: 0x00160D0C
	private bool AssistAttack(List<Tile> targetTileList)
	{
		if (this.GetUnitTileAbsoluteDebuff(_EffectPartType.AttackPreturn, null))
		{
			return false;
		}
		if (this.unitAbilityList.Count <= 0)
		{
			return false;
		}
		if (this.unitAbilityList[0].effectType != _EffectType.Damage && this.unitAbilityList[0].effectType != _EffectType.Debuff)
		{
			return false;
		}
		if (this.unitAbilityList[0].cooldown > 0)
		{
			return false;
		}
		if (this.SP < this.GetAbilityCost(this.unitAbilityList[0], false))
		{
			return false;
		}
		if (targetTileList.Count <= 0)
		{
			return false;
		}
		List<Tile> list = this.CheckAssistAttack(targetTileList, this.unitAbilityList[0]);
		if (list.Count <= 0)
		{
			return false;
		}
		AttackInstance attackInstance = new AttackInstance();
		attackInstance.unitAbility = this.unitAbilityList[0];
		attackInstance.type = _AttackType.Skill_AssistAttack;
		attackInstance.srcUnit = this;
		this.SP -= this.GetAbilityCost(attackInstance.unitAbility, false);
		this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
		if (this.Calculate4Measurements(CharacterData.PropertyType.Intelligence, ref this.iClearCDValue))
		{
			new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.ClearCD, 0f);
			for (int i = 0; i < this.unitAbilityList.Count; i++)
			{
				this.unitAbilityList[i].cooldown = 0;
			}
		}
		if (this.Calculate4Measurements(CharacterData.PropertyType.Strength, ref this.iShockValue))
		{
			new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.Shock, 0f);
			attackInstance.shock = true;
		}
		base.StartCoroutine(this.AttackRoutineSkill(list, attackInstance));
		return true;
	}

	// Token: 0x06002E2F RID: 11823 RVA: 0x00162D00 File Offset: 0x00160F00
	private List<Tile> CheckAssistAttack(List<Tile> origTargetTileList, UnitAbility uab)
	{
		List<Tile> list = new List<Tile>();
		List<Tile> list2 = new List<Tile>();
		if (uab.targetArea == _TargetArea.Default)
		{
			if (uab.requireTargetSelection)
			{
				List<Tile> list3 = new List<Tile>();
				list3.AddRange(GridManager.GetTilesWithinRange(this.occupiedTile, uab.range));
				foreach (Tile tile in list3)
				{
					list2.Clear();
					list.Clear();
					list2.Add(tile);
					if (uab.aoeRange != 0)
					{
						list2.AddRange(GridManager.GetTilesWithinRange(tile, uab.aoeRange));
					}
					list.AddRange(this.GetTileInTileList(origTargetTileList, list2));
					if (list.Count > 0)
					{
						return list;
					}
					list.Clear();
				}
			}
			else
			{
				if (uab.aoeRange != 0)
				{
					list2.AddRange(GridManager.GetTilesWithinRange(this.occupiedTile, uab.aoeRange));
				}
				list.AddRange(this.GetTileInTileList(origTargetTileList, list2));
				if (list.Count > 0)
				{
					return list;
				}
				list.Clear();
			}
		}
		else
		{
			foreach (Tile targetTile in this.occupiedTile.neighbours)
			{
				list2.Clear();
				list.Clear();
				if (uab.targetArea == _TargetArea.Line)
				{
					list2.AddRange(GridManager.GetTileInLine(this.occupiedTile, targetTile, uab.range));
				}
				else if (uab.targetArea == _TargetArea.Cone)
				{
					list2.AddRange(GridManager.GetTileInCone60(this.occupiedTile, targetTile, uab.range));
				}
				else if (uab.targetArea == _TargetArea.Fan1)
				{
					list2.AddRange(GridManager.GetTileInFan1(this.occupiedTile, targetTile, uab.range));
				}
				list.AddRange(this.GetTileInTileList(origTargetTileList, list2));
				if (list.Count > 0)
				{
					return list;
				}
				list.Clear();
			}
		}
		return list;
	}

	// Token: 0x06002E30 RID: 11824 RVA: 0x00162F30 File Offset: 0x00161130
	private List<Tile> GetTileInTileList(List<Tile> targetList, List<Tile> assistList)
	{
		List<Tile> list = new List<Tile>();
		foreach (Tile tile in assistList)
		{
			if (!(tile.unit == null))
			{
				if (!this.CheckFriendFaction(tile.unit.factionID))
				{
					if (targetList.Contains(tile))
					{
						list.Add(tile);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06002E31 RID: 11825 RVA: 0x00162FCC File Offset: 0x001611CC
	public bool Attack(List<Tile> targetTileList, UnitAbility ability)
	{
		if ((ability == null && this.attacked) || this.stun > 0)
		{
			return false;
		}
		if (this.GetAttackDisabled())
		{
			return false;
		}
		this.lastAttackTileList.Clear();
		this.lastAttackTileList.AddRange(targetTileList);
		targetTileList = this.FilterTileByAbilityTargetType(targetTileList, ability);
		if (targetTileList.Count <= 0)
		{
			return false;
		}
		this.attackRemain--;
		if (this.attackRemain <= 0)
		{
			this.moved = true;
			this.attacked = true;
		}
		UnitControl.MoveUnit(this);
		this.m_AlreadyCheckPlusAttack = false;
		AttackInstance attackInstance = new AttackInstance();
		attackInstance.unitAbility = ability;
		attackInstance.srcUnit = this;
		if (this.SP < 0)
		{
			this.SP = 0;
			new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.SpLow, 0f);
			attackInstance.outofAP = true;
		}
		attackInstance.type = _AttackType.Skill_Normal;
		if (ability.effectType == _EffectType.Damage)
		{
			if (this.GetUnitAbilityAbsoluteBuff(_EffectPartType.Cleanup, ability))
			{
				string s = string.Format(Game.StringTable.GetString(260027), ability.name);
				this.ClearnDebuffCount(99, s);
			}
			else if (this.GetUnitAbilityValue(_EffectPartType.Cleanup, ability, 0, true) >= 1f)
			{
				int iCount = Mathf.RoundToInt(this.GetUnitAbilityValue(_EffectPartType.Cleanup, ability, 0, true));
				string s2 = string.Format(Game.StringTable.GetString(260027), ability.name);
				this.ClearnDebuffCount(iCount, s2);
			}
		}
		if (this.Calculate4Measurements(CharacterData.PropertyType.Intelligence, ref this.iClearCDValue))
		{
			new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.ClearCD, 0f);
			for (int i = 0; i < this.unitAbilityList.Count; i++)
			{
				this.unitAbilityList[i].cooldown = 0;
			}
		}
		if (this.Calculate4Measurements(CharacterData.PropertyType.Strength, ref this.iShockValue))
		{
			new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.Shock, 0f);
			attackInstance.shock = true;
		}
		if (!this.GetUnitAbilityAbsoluteBuff(_EffectPartType.Capture, ability))
		{
			this.actionQueued += this.occupiedTile.CheckTalentAssistAttack(this, targetTileList);
		}
		base.StartCoroutine(this.AttackRoutineSkill(targetTileList, attackInstance));
		if (this.attacked)
		{
			GridManager.ClearHostileList();
			if (this.moved)
			{
				GridManager.Deselect();
			}
		}
		return true;
	}

	// Token: 0x06002E32 RID: 11826 RVA: 0x00163284 File Offset: 0x00161484
	public void CounterAttack(UnitTB target, UnitAbility uab, bool bCounterAbility, bool bNeedSP)
	{
		if (this.unitAbilityList.Count <= 0)
		{
			target.CounterAttackComplete();
			return;
		}
		List<Tile> list = new List<Tile>();
		list.Add(target.occupiedTile);
		AttackInstance attackInstance = new AttackInstance();
		attackInstance.srcUnit = this;
		attackInstance.type = _AttackType.Skill_Counter;
		attackInstance.unitAbility = uab;
		attackInstance.counterAbility = bCounterAbility;
		if (bNeedSP)
		{
			this.SP -= this.GetAbilityCost(attackInstance.unitAbility, false);
			this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
			if (this.SP < 0)
			{
				this.SP = 0;
				new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.SpLow, 0f);
				attackInstance.outofAP = true;
			}
		}
		base.StartCoroutine(this.AttackRoutineSkill(list, attackInstance));
	}

	// Token: 0x06002E33 RID: 11827 RVA: 0x00163370 File Offset: 0x00161570
	public void DoubleAttack(UnitAbility uab, List<Tile> targetTileList)
	{
		Debug.LogWarning("DoubleAttack");
		targetTileList = this.FilterTileByAbilityTargetType(targetTileList, uab);
		if (targetTileList.Count <= 0)
		{
			Debug.LogWarning("No Tile Cancel DoubleAttack");
			base.StartCoroutine(this.ActionComplete(0.2f));
			return;
		}
		AttackInstance attackInstance = new AttackInstance();
		attackInstance.srcUnit = this;
		attackInstance.type = _AttackType.Skill_DoubleAttack;
		attackInstance.unitAbility = uab;
		this.SP -= this.GetAbilityCost(attackInstance.unitAbility, false);
		this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
		if (this.SP < 0)
		{
			this.SP = 0;
			new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.SpLow, 0f);
			attackInstance.outofAP = true;
		}
		base.StartCoroutine(this.AttackRoutineSkill(targetTileList, attackInstance));
	}

	// Token: 0x06002E34 RID: 11828 RVA: 0x00163460 File Offset: 0x00161660
	public int GetDebuffCount()
	{
		int num = 0;
		for (int i = 0; i < this.unitConditionList.Count; i++)
		{
			if (this.unitConditionList[i].m_iCondType == _ConditionType.Debuff || this.unitConditionList[i].m_iCondType == _ConditionType.StackDebuff)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002E35 RID: 11829 RVA: 0x001634C0 File Offset: 0x001616C0
	public int AIUseItemSelfScore(ItemDataNode pItemDataNode)
	{
		int num = 0;
		int num2 = this.fullHP - this.HP;
		int num3 = this.fullSP - this.SP;
		int debuffCount = this.GetDebuffCount();
		float num4 = (float)this.HP;
		float num5 = (float)this.SP;
		num4 /= (float)this.fullHP;
		num5 /= (float)this.fullSP;
		float num6 = 1f + this.CheckTalentSelfAndTeamPlus(TalentEffect.ItemEffectPlus, true, false);
		foreach (ItmeEffectNode itmeEffectNode in pItemDataNode.m_ItmeEffectNodeList)
		{
			switch (itmeEffectNode.m_iItemType)
			{
			case 7:
				num += 100;
				break;
			case 8:
				if (num4 < 0.5f)
				{
					int num7 = Mathf.RoundToInt(num6 * (float)itmeEffectNode.m_iValue);
					if (num7 > num2)
					{
						int num8 = num7 - num2;
						num7 = num2 - num8;
						if (num7 < 0)
						{
							num7 = 0;
						}
					}
					num += Mathf.RoundToInt((1f - num4) * (float)num7);
				}
				break;
			case 9:
				if (num5 < 0.5f)
				{
					int num7 = Mathf.RoundToInt(num6 * (float)itmeEffectNode.m_iValue);
					if (num7 > num3)
					{
						int num8 = num7 - num3;
						num7 = num3 - num8;
						if (num7 < 0)
						{
							num7 = 0;
						}
					}
					num += Mathf.RoundToInt((1f - num5) * (float)num7);
				}
				break;
			case 16:
				if (num4 < 0.5f)
				{
					int num7 = Mathf.RoundToInt(0.01f * num6 * (float)itmeEffectNode.m_iValue * (float)this.fullHP);
					if (num7 > num2)
					{
						int num8 = num7 - num2;
						num7 = num2 - num8;
						if (num7 < 0)
						{
							num7 = 0;
						}
					}
					num += Mathf.RoundToInt((1f - num4) * (float)num7);
				}
				break;
			case 17:
				if (num5 < 0.5f)
				{
					int num7 = Mathf.RoundToInt(0.01f * num6 * (float)itmeEffectNode.m_iValue * (float)this.fullSP);
					if (num7 > num3)
					{
						int num8 = num7 - num3;
						num7 = num3 - num8;
						if (num7 < 0)
						{
							num7 = 0;
						}
					}
					num += Mathf.RoundToInt((1f - num5) * (float)num7);
				}
				break;
			case 18:
				if (num4 < 0.5f)
				{
					num += Mathf.Min(debuffCount, Mathf.RoundToInt(num6 * (float)itmeEffectNode.m_iValue)) * 150;
				}
				break;
			}
		}
		return num;
	}

	// Token: 0x06002E36 RID: 11830 RVA: 0x00163780 File Offset: 0x00161980
	private void ApplyFood(ItemDataNode pItemDataNode)
	{
		float num = 1f + this.CheckTalentSelfAndTeamPlus(TalentEffect.ItemEffectPlus, true, true);
		foreach (ItmeEffectNode itmeEffectNode in pItemDataNode.m_ItmeEffectNodeList)
		{
			switch (itmeEffectNode.m_iItemType)
			{
			case 7:
				this.ApplyConditionID(itmeEffectNode.m_iRecoverType, pItemDataNode.m_strItemName, true);
				break;
			case 8:
				this.ApplyHeal(Mathf.RoundToInt(num * (float)itmeEffectNode.m_iValue));
				break;
			case 9:
				this.ApplyInternalForce(Mathf.RoundToInt(num * (float)itmeEffectNode.m_iValue));
				break;
			case 16:
				this.ApplyHeal(Mathf.RoundToInt(num * (float)this.fullHP * (float)itmeEffectNode.m_iValue / 100f));
				break;
			case 17:
				this.ApplyInternalForce(Mathf.RoundToInt(num * (float)this.fullSP * (float)itmeEffectNode.m_iValue / 100f));
				break;
			case 18:
				this.ClearnDebuffCount(Mathf.RoundToInt(num * (float)itmeEffectNode.m_iValue), pItemDataNode.m_strItemName);
				break;
			}
		}
	}

	// Token: 0x06002E37 RID: 11831 RVA: 0x001638F4 File Offset: 0x00161AF4
	private IEnumerator UseItemSelf(ItemDataNode pItemDataNode)
	{
		while (!GameControlTB.ActionCommenced())
		{
			yield return null;
		}
		Debug.LogWarning(this.unitName + " UseItemSelf actionInProgress = true");
		float fValue = 1f + this.CheckTalentSelfAndTeamPlus(TalentEffect.ItemEffectPlus, true, true);
		List<AttackInstance> atkInstList = new List<AttackInstance>();
		foreach (ItmeEffectNode ien in pItemDataNode.m_ItmeEffectNodeList)
		{
			AttackInstance aInst = null;
			switch (ien.m_iItemType)
			{
			case 7:
				aInst = new AttackInstance();
				aInst.srcUnit = this;
				aInst.targetUnit = this;
				aInst.heal = true;
				aInst.unitAbility = new UnitAbility();
				aInst.unitAbility.name = pItemDataNode.m_strItemName;
				aInst.unitAbility.effectType = _EffectType.Heal;
				aInst.unitAbility.skillID = ien.m_iRecoverType;
				aInst.unitAbility.chainedAbilityIDList.Add(ien.m_iRecoverType);
				aInst.unitAbility.bItemSkill = true;
				break;
			case 8:
				aInst = new AttackInstance();
				aInst.srcUnit = this;
				aInst.targetUnit = this;
				aInst.heal = true;
				aInst.damageDone = Mathf.RoundToInt(fValue * (float)ien.m_iValue);
				aInst.unitAbility = new UnitAbility();
				aInst.unitAbility.name = pItemDataNode.m_strItemName;
				aInst.unitAbility.effectType = _EffectType.Heal;
				aInst.unitAbility.skillID = ien.m_iRecoverType;
				aInst.unitAbility.bItemSkill = true;
				break;
			case 9:
				aInst = new AttackInstance();
				aInst.srcUnit = this;
				aInst.targetUnit = this;
				aInst.heal = true;
				aInst.damageSPDone = Mathf.RoundToInt(fValue * (float)ien.m_iValue);
				aInst.unitAbility = new UnitAbility();
				aInst.unitAbility.name = pItemDataNode.m_strItemName;
				aInst.unitAbility.effectType = _EffectType.Heal;
				aInst.unitAbility.skillID = ien.m_iRecoverType;
				aInst.unitAbility.bItemSkill = true;
				break;
			case 16:
				aInst = new AttackInstance();
				aInst.srcUnit = this;
				aInst.targetUnit = this;
				aInst.heal = true;
				aInst.damageDone = Mathf.RoundToInt(fValue * (float)this.fullHP * (float)ien.m_iValue / 100f);
				aInst.unitAbility = new UnitAbility();
				aInst.unitAbility.name = pItemDataNode.m_strItemName;
				aInst.unitAbility.effectType = _EffectType.Heal;
				aInst.unitAbility.skillID = ien.m_iRecoverType;
				aInst.unitAbility.bItemSkill = true;
				break;
			case 17:
				aInst = new AttackInstance();
				aInst.srcUnit = this;
				aInst.targetUnit = this;
				aInst.heal = true;
				aInst.damageSPDone = Mathf.RoundToInt(fValue * (float)this.fullSP * (float)ien.m_iValue / 100f);
				aInst.unitAbility = new UnitAbility();
				aInst.unitAbility.name = pItemDataNode.m_strItemName;
				aInst.unitAbility.effectType = _EffectType.Heal;
				aInst.unitAbility.skillID = ien.m_iRecoverType;
				aInst.unitAbility.bItemSkill = true;
				break;
			case 18:
				aInst = new AttackInstance();
				aInst.srcUnit = this;
				aInst.targetUnit = this;
				aInst.heal = true;
				aInst.cleanDebuffCount = Mathf.RoundToInt(fValue * (float)ien.m_iValue);
				aInst.unitAbility = new UnitAbility();
				aInst.unitAbility.name = pItemDataNode.m_strItemName;
				aInst.unitAbility.effectType = _EffectType.Heal;
				aInst.unitAbility.skillID = ien.m_iRecoverType;
				aInst.unitAbility.bItemSkill = true;
				break;
			}
			IL_5DD:
			if (aInst == null)
			{
				continue;
			}
			if (aInst.srcUnit.bStealth)
			{
				aInst.critical = true;
				aInst.damageDone = Mathf.RoundToInt(1.5f * (float)aInst.damageDone);
				aInst.damageSPDone = Mathf.RoundToInt(1.5f * (float)aInst.damageSPDone);
				aInst.cleanDebuffCount = Mathf.RoundToInt(1.5f * (float)aInst.cleanDebuffCount);
			}
			atkInstList.Add(aInst);
			this.actionQueued++;
			continue;
			goto IL_5DD;
		}
		this.animationTB.PlayUseItemSelf(atkInstList);
		yield break;
	}

	// Token: 0x06002E38 RID: 11832 RVA: 0x00163920 File Offset: 0x00161B20
	private static int CompareTileDistFunc(Tile tile_a, Tile tile_b)
	{
		if (tile_a == null)
		{
			return 0;
		}
		if (tile_a.unit == null)
		{
			return 0;
		}
		if (tile_b == null)
		{
			return 0;
		}
		if (tile_b.unit == null)
		{
			return 0;
		}
		int num = GridManager.Distance(UnitTB.tileCompareBase, tile_a);
		int num2 = GridManager.Distance(UnitTB.tileCompareBase, tile_b);
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06002E39 RID: 11833 RVA: 0x0016399C File Offset: 0x00161B9C
	private IEnumerator AttackRoutineSkill(List<Tile> targetTileList, AttackInstance attInstance)
	{
		this.lastAttackInstance = attInstance;
		if (attInstance.type == _AttackType.Skill_Normal)
		{
			while (!GameControlTB.ActionCommenced())
			{
				yield return null;
			}
			Debug.LogWarning(this.unitName + " AttackRoutineSkill actionInProgress = true");
		}
		this.bAttackPlus = false;
		this.bSteal = false;
		float fAngle = 360f;
		Quaternion wantedRot = this.thisT.rotation;
		if (targetTileList.Count > 1)
		{
			foreach (Tile tile in targetTileList)
			{
				if (!(tile == null))
				{
					if (!(tile.unit == null))
					{
						if (tile.unit == this)
						{
							fAngle = 0f;
							wantedRot = this.thisT.rotation;
							break;
						}
						Quaternion Rot = Quaternion.LookRotation(tile.pos - this.thisT.position);
						float theAngle = Quaternion.Angle(this.thisT.rotation, Rot);
						if (theAngle < fAngle)
						{
							wantedRot = Rot;
							fAngle = theAngle;
						}
						tile.unit.bBeTarget = true;
					}
				}
			}
		}
		else if (targetTileList.Count == 1)
		{
			if (targetTileList[0].unit == this)
			{
				fAngle = 0f;
				wantedRot = this.thisT.rotation;
			}
			else
			{
				wantedRot = Quaternion.LookRotation(targetTileList[0].pos - this.thisT.position);
				fAngle = Quaternion.Angle(this.thisT.rotation, wantedRot);
			}
		}
		if (fAngle < 5f || fAngle >= 360f)
		{
			this.thisT.rotation = wantedRot;
		}
		else
		{
			float fovertime = 0f;
			for (;;)
			{
				fovertime += Time.deltaTime;
				float frot = Time.deltaTime * this.rotateSpeed;
				frot = Mathf.Clamp(frot, 0.05f, 1f);
				this.thisT.rotation = Quaternion.Slerp(this.thisT.rotation, wantedRot, frot);
				if (Quaternion.Angle(this.thisT.rotation, wantedRot) < 2f || fovertime > 0.5f)
				{
					break;
				}
				yield return null;
			}
			this.thisT.rotation = wantedRot;
		}
		attInstance.fTalentSkillRate = this.TalentLessMoney();
		List<AttackInstance> atkInstList = new List<AttackInstance>();
		UnitTB.tileCompareBase = this.occupiedTile;
		targetTileList.Sort(new Comparison<Tile>(UnitTB.CompareTileDistFunc));
		foreach (Tile tile2 in targetTileList)
		{
			if (!(tile2 == null))
			{
				if (!(tile2.unit == null))
				{
					AttackInstance aInst = attInstance.Clone();
					aInst.Process(tile2.unit);
					atkInstList.Add(aInst);
					if (UnitTB.onAttackS != null && !aInst.heal)
					{
						UnitTB.onAttackS(this, tile2.unit);
					}
					this.actionQueued++;
				}
			}
		}
		foreach (Tile tile3 in targetTileList)
		{
			if (!(tile3 == null))
			{
				if (!(tile3.unit == null))
				{
					tile3.unit.bBeTarget = false;
				}
			}
		}
		if (this.actionQueued <= 0)
		{
			this.actionQueued = 0;
			this.ActionComplete(0.26f);
			yield break;
		}
		if (atkInstList.Count > 0 && this.bCharge)
		{
			base.StartCoroutine(this.ChargeMove(this.tChargeToTile));
			while (this.bSpeicalAction)
			{
				yield return new WaitForSeconds(0.1f);
			}
			this.bCharge = false;
			this.tChargeToTile = null;
			foreach (AttackInstance aInst2 in atkInstList)
			{
				aInst2.targetUnit.RotateToUnit(aInst2.srcUnit);
			}
			atkInstList[0].srcUnit.RotateToUnit(atkInstList[0].targetUnit);
		}
		if (attInstance.unitAbility.skillIDList.Count > 1)
		{
			int index = Random.Range(0, attInstance.unitAbility.skillIDList.Count);
			int skillID = attInstance.unitAbility.skillIDList[index];
			this.animationTB.PlaySkill(skillID, atkInstList);
		}
		else
		{
			this.animationTB.PlaySkill(attInstance.unitAbility.skillID, atkInstList);
		}
		if (!attInstance.unitAbility.bItemSkill && !GameGlobal.m_bDLCMode)
		{
			NpcRoutine nr = this.characterData.GetRoutineData(attInstance.unitAbility.ID);
			if (nr != null)
			{
				int exp = this.GetRoutineMartialAttack(attInstance.unitAbility.ID);
				float fexp = (0.5f * (float)attInstance.unitAbility.cdDuration + 1f) * (float)exp;
				exp = Mathf.RoundToInt(fexp);
				int iLvUp = NPC.m_instance.SetRoutineExp(this.characterData, attInstance.unitAbility.ID, exp);
				if (iLvUp >= 0)
				{
					string str = string.Format(Game.StringTable.GetString(260016), this.unitName, attInstance.unitAbility.name, exp.ToString());
					UINGUI.BattleMessage(str);
				}
				if (iLvUp > 0)
				{
					int iRoutineLv = this.GetRoutineLv(attInstance.unitAbility.ID);
					string str = string.Format(Game.StringTable.GetString(260017), this.unitName, attInstance.unitAbility.name, iRoutineLv.ToString());
					UINGUI.BattleMessage(str);
					str = nr.GetLvUpString(iLvUp);
					str = string.Format(Game.StringTable.GetString(260018), this.unitName, str);
					UINGUI.BattleMessage(str);
				}
			}
		}
		if (attInstance.unitAbility.effectType == _EffectType.Damage || attInstance.unitAbility.effectType == _EffectType.Debuff)
		{
			this.RemoveConditionByAttack();
		}
		yield break;
	}

	// Token: 0x06002E3A RID: 11834 RVA: 0x0001D9C8 File Offset: 0x0001BBC8
	public void HitTarget(UnitTB target, AttackInstance attInstance)
	{
		this.ReleaseStealth();
		if (!attInstance.heal)
		{
			this.CheckTalentWhenAttack(target);
		}
		base.StartCoroutine(this._HitTarget(target, attInstance));
	}

	// Token: 0x06002E3B RID: 11835 RVA: 0x001639D4 File Offset: 0x00161BD4
	private IEnumerator _HitTarget(UnitTB target, AttackInstance attInstance)
	{
		if (target != null)
		{
			target.ApplyHitEffect(attInstance);
			yield return null;
			if (!this.IsDestroyed())
			{
				if (attInstance.moveFlower)
				{
					attInstance = target.MoveFlower(attInstance);
				}
				else if (attInstance.captureTarget)
				{
					this.CaptureTargert(target);
				}
				else
				{
					if (!attInstance.missed && (attInstance.type == _AttackType.Skill_Normal || attInstance.type == _AttackType.Skill_DoubleAttack) && !this.EffectPartAfterSuccess(_EffectPartType.HitChance, attInstance.unitAbility, target))
					{
						this.CheckStackCondition(attInstance);
					}
					if (attInstance.critical && !attInstance.heal)
					{
						this.EffectPartAfterSuccess(_EffectPartType.CriticalChance, attInstance.unitAbility, target);
					}
					if (!attInstance.damageAbsorbToSP)
					{
						float fVampireHitPoint = this.GetEffectPartValuePercent(_EffectPartType.VampireHitPoint, attInstance.unitAbility, 0);
						if (this.occupiedTile != null)
						{
							fVampireHitPoint += this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.VampireHitPoint, this);
						}
						fVampireHitPoint *= 0.01f;
						fVampireHitPoint = Mathf.Clamp(fVampireHitPoint, 0f, 1f);
						if (fVampireHitPoint > 0f && !attInstance.heal)
						{
							int value = Mathf.RoundToInt(fVampireHitPoint * (float)attInstance.damageDone);
							this.ApplyHeal(value);
						}
						float fVampireInternalForce = this.GetEffectPartValuePercent(_EffectPartType.VampireInternalForce, attInstance.unitAbility, 0);
						if (this.occupiedTile != null)
						{
							fVampireInternalForce += this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.VampireInternalForce, this);
						}
						fVampireInternalForce *= 0.01f;
						fVampireInternalForce = Mathf.Clamp(fVampireInternalForce, 0f, 1f);
						if (fVampireInternalForce > 0f && !attInstance.heal)
						{
							int value2 = Mathf.RoundToInt(fVampireInternalForce * (float)attInstance.damageDone);
							this.ApplyInternalForce(value2);
						}
					}
					yield return null;
					if (!target.IsDestroyed())
					{
						attInstance = target.CheckCounterAttack(attInstance);
					}
				}
			}
		}
		yield return new WaitForSeconds(0.1f);
		if (attInstance.counterAttacking)
		{
			yield break;
		}
		if (!this.IsDestroyed())
		{
			if (!attInstance.captureTarget)
			{
				if (target.IsDestroyed())
				{
					if (attInstance.type == _AttackType.Skill_Normal || attInstance.type == _AttackType.Skill_DoubleAttack)
					{
						yield return new WaitForSeconds(target.destroyEffectDuration);
						if (this.GetUnitTileAbsoluteBuff(_EffectPartType.KillOneMore, attInstance.unitAbility))
						{
							this.moveRemain = this.GetMovePerTurn();
							this.attackRemain = this.GetAttackPerTurn();
							this.iDeadPlus++;
							if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible())
							{
								new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.DeadPlus, 0f);
							}
							this.moved = false;
							this.attacked = false;
							UnitControl.OneMoreMoveUnit(this);
						}
					}
					this.EffectPartAfterSuccess(_EffectPartType.KillOneMore, attInstance.unitAbility, attInstance.srcUnit);
					this.CheckTalentKillOne();
				}
				else if (attInstance.protect || attInstance.knockback || attInstance.pullclose)
				{
					while (target.bSpeicalAction)
					{
						yield return null;
					}
				}
			}
		}
		this.actionQueued--;
		if (this.actionQueued <= 0)
		{
			this.actionQueued = 0;
			if (attInstance.type == _AttackType.Skill_Normal)
			{
				float length = attInstance.animationLength - attInstance.lastEmitHitTime;
				if (length < 0f)
				{
					length = 0f;
				}
				if (this.CheckDblAttack(attInstance.unitAbility) && !this.IsDestroyed())
				{
					yield return new WaitForSeconds(length + 0.15f);
					this.DoubleAttack(attInstance.unitAbility, this.lastAttackTileList);
				}
				else if (this.IsDestroyed() || this.bAttackPlus)
				{
					base.StartCoroutine(this.ActionComplete(length + 0.1f));
				}
				else
				{
					this.CheckTalentNightFragrance();
					base.StartCoroutine(this.ActionComplete(0.15f));
				}
			}
			else if (attInstance.type == _AttackType.Skill_Counter)
			{
				attInstance.targetUnit.CounterAttackComplete();
			}
			else if (attInstance.type == _AttackType.Skill_DoubleAttack)
			{
				this.CheckTalentNightFragrance();
				base.StartCoroutine(this.ActionComplete(0.22f));
			}
			else if (attInstance.type == _AttackType.Skill_AssistAttack)
			{
				this.assistTheUnit.AssistAttackComplete();
			}
		}
		yield break;
	}

	// Token: 0x06002E3C RID: 11836 RVA: 0x00163A0C File Offset: 0x00161C0C
	private void CaptureTargert(UnitTB target)
	{
		if (this.CheckFriendFaction(0))
		{
			TeamStatus.m_Instance.AddDLCUnit(target.characterData.iNpcID, target.characterData.iLevel);
		}
		else
		{
			TeamStatus.m_Instance._DeleteDLCUnit(target.dlcCharGuid);
		}
		target.LeaveBattle(false);
	}

	// Token: 0x06002E3D RID: 11837 RVA: 0x0001D9F1 File Offset: 0x0001BBF1
	public void AssistAttackComplete()
	{
		this.actionQueued--;
		Debug.Log(this.unitName + " TeamMate AssistAttackComplete actionQueued = " + this.actionQueued.ToString());
		this.CheckActionQueued(this.lastAttackInstance);
	}

	// Token: 0x06002E3E RID: 11838 RVA: 0x0001DA2D File Offset: 0x0001BC2D
	public void CounterAttackComplete()
	{
		this.actionQueued--;
		Debug.Log(this.unitName + " be CounterAttackComplete actionQueued = " + this.actionQueued.ToString());
		this.CheckActionQueued(this.lastAttackInstance);
	}

	// Token: 0x06002E3F RID: 11839 RVA: 0x00163A64 File Offset: 0x00161C64
	private void CheckActionQueued(AttackInstance attInstance)
	{
		if (this.actionQueued <= 0)
		{
			this.actionQueued = 0;
			if (this.IsDestroyed() || attInstance == null)
			{
				base.StartCoroutine(this.ActionComplete(0.08f));
				this.lastAttackInstance = null;
				return;
			}
			if (attInstance.type == _AttackType.Skill_Normal)
			{
				float num = attInstance.animationLength - attInstance.lastEmitHitTime;
				if (num < 0f)
				{
					num = 0f;
				}
				if (this.CheckDblAttack(attInstance.unitAbility))
				{
					this.DoubleAttack(attInstance.unitAbility, this.lastAttackTileList);
				}
				else if (this.IsDestroyed() || this.bAttackPlus)
				{
					base.StartCoroutine(this.ActionComplete(num + 0.1f));
				}
				else
				{
					this.CheckTalentNightFragrance();
					base.StartCoroutine(this.ActionComplete(0.1f));
				}
			}
			else if (attInstance.type == _AttackType.Skill_DoubleAttack)
			{
				this.CheckTalentNightFragrance();
				base.StartCoroutine(this.ActionComplete(0.24f));
			}
			else
			{
				Debug.LogError("Error Call Kao for the Bug");
				base.StartCoroutine(this.ActionComplete(0.23f));
			}
			this.lastAttackInstance = null;
		}
	}

	// Token: 0x06002E40 RID: 11840 RVA: 0x00163B9C File Offset: 0x00161D9C
	private IEnumerator ActionComplete(float delay)
	{
		Debug.Log(this.unitName + " ActionComplete Delay " + delay.ToString());
		yield return new WaitForSeconds(delay);
		Debug.Log(this.unitName + " ActionComplete");
		if (UnitTB.onActionCompletedE != null)
		{
			UnitTB.onActionCompletedE(this);
		}
		if (this.bActionPlayerConfuse)
		{
			this.bActionPlayerConfuse = false;
			if (GameControlTB.IsPlayerFaction(this.iOrigFaction))
			{
				while (GameControlTB.IsActionInProgress())
				{
					yield return null;
				}
				if (UnitTB.onTurnDepletedE != null)
				{
					UnitTB.onTurnDepletedE();
				}
			}
			yield break;
		}
		if (GameControlTB.IsPlayerFaction(this.factionID))
		{
			if (this.IsAllActionCompleted())
			{
				while (GameControlTB.IsActionInProgress())
				{
					yield return null;
				}
				if (UnitTB.onTurnDepletedE != null)
				{
					UnitTB.onTurnDepletedE();
				}
			}
			else
			{
				this.Select();
				GridManager.Select(this.occupiedTile);
			}
		}
		yield break;
	}

	// Token: 0x06002E41 RID: 11841 RVA: 0x00163BC8 File Offset: 0x00161DC8
	private void AddThreat(AttackInstance aInst)
	{
		if (aInst.heal)
		{
			List<UnitTB> allHostile = UnitControl.GetAllHostile(aInst.srcUnit.factionID);
			foreach (UnitTB unitTB in allHostile)
			{
				unitTB.AddThreatValue(aInst.srcUnit, aInst.damageDone);
			}
		}
		else
		{
			this.AddThreatValue(aInst.srcUnit, aInst.damageDone);
		}
	}

	// Token: 0x06002E42 RID: 11842 RVA: 0x00163C5C File Offset: 0x00161E5C
	public void AddThreatValue(UnitTB unit, int value)
	{
		bool flag = false;
		foreach (ThreatNode threatNode in this.threatList)
		{
			if (threatNode.unit == unit)
			{
				threatNode.ThreatValue += value;
				this.iTotalThreat += value;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			ThreatNode threatNode2 = new ThreatNode();
			threatNode2.unit = unit;
			threatNode2.ThreatValue = value;
			this.iTotalThreat += value;
			this.threatList.Add(threatNode2);
		}
	}

	// Token: 0x06002E43 RID: 11843 RVA: 0x00163D1C File Offset: 0x00161F1C
	private void ReUpdateThreatList()
	{
		for (int i = 0; i < this.threatList.Count; i++)
		{
			if (this.threatList[i].unit == null)
			{
				this.iTotalThreat -= this.threatList[i].ThreatValue;
				this.threatList.RemoveAt(i);
				i--;
			}
			else if (this.threatList[i].unit.IsDestroyed())
			{
				this.iTotalThreat -= this.threatList[i].ThreatValue;
				this.threatList.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06002E44 RID: 11844 RVA: 0x00163DE0 File Offset: 0x00161FE0
	public UnitTB GetHighestThreat()
	{
		int num = 0;
		UnitTB result = null;
		for (int i = 0; i < this.threatList.Count; i++)
		{
			if (num < this.threatList[i].ThreatValue)
			{
				num = this.threatList[i].ThreatValue;
				result = this.threatList[i].unit;
			}
		}
		return result;
	}

	// Token: 0x06002E45 RID: 11845 RVA: 0x00163E4C File Offset: 0x0016204C
	public int GetTargetThreatValue(UnitTB target)
	{
		for (int i = 0; i < this.threatList.Count; i++)
		{
			if (target == this.threatList[i].unit)
			{
				return this.threatList[i].ThreatValue;
			}
		}
		return 0;
	}

	// Token: 0x06002E46 RID: 11846 RVA: 0x00163EA4 File Offset: 0x001620A4
	public float GetThreatUnitValue(UnitTB target)
	{
		float num = 1f;
		if (!this.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt, null))
		{
			switch (this.aiMode)
			{
			case _AIMode.Threat:
				if (this.iTotalThreat <= 0)
				{
					return 1f;
				}
				if (target == null)
				{
					return 1f;
				}
				num = (float)(this.GetTargetThreatValue(target) / this.iTotalThreat);
				num += 1f;
				break;
			case _AIMode.Stand:
				num = 1f;
				break;
			case _AIMode.Chase:
				if (target == null)
				{
					return 1f;
				}
				if (this.aiTarget != null)
				{
					if (this.aiTarget == target)
					{
						num = 1.5f;
					}
					else
					{
						num = 0.5f;
					}
				}
				else
				{
					num = 1f;
				}
				break;
			case _AIMode.Follow:
				num = 1f;
				break;
			case _AIMode.MaxEnemy:
				num = 1f;
				break;
			case _AIMode.Guard:
				num = 0f;
				break;
			case _AIMode.Protect:
				if (target == null)
				{
					return 1f;
				}
				if (this.aiTarget != null)
				{
					if (this.aiTarget == target)
					{
						num = 1.5f;
					}
					else
					{
						num = 0.5f;
					}
				}
				else
				{
					num = 1f;
				}
				break;
			}
			return num;
		}
		if (target == null)
		{
			return 1f;
		}
		UnitTB unitTB = UnitControl.instance.FindUnit(this.FindConditionEffectPartID(_EffectPartType.Taunt));
		if (unitTB == target)
		{
			return 3f;
		}
		return 1f;
	}

	// Token: 0x06002E47 RID: 11847 RVA: 0x00164048 File Offset: 0x00162248
	public float GetAITileValue(Tile moveTile)
	{
		float num = 100f;
		Tile tile = null;
		bool flag = true;
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt, null))
		{
			UnitTB unitTB = UnitControl.instance.FindUnit(this.FindConditionEffectPartID(_EffectPartType.Taunt));
			if (unitTB == null)
			{
				Debug.LogError("WTF Cant found Taunt unit ");
				return 0f;
			}
			tile = unitTB.occupiedTile;
		}
		else if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee, null))
		{
			UnitTB unitTB = UnitControl.instance.FindUnit(this.FindConditionEffectPartID(_EffectPartType.Flee));
			if (unitTB == null)
			{
				Debug.LogError("WTF Cant found Flee unit ");
				return 0f;
			}
			tile = unitTB.occupiedTile;
			flag = false;
		}
		else if (this.bNightFragrance)
		{
			if (this.lastAttackTileList.Count > 0)
			{
				tile = this.lastAttackTileList[0];
			}
			flag = false;
		}
		else
		{
			switch (this.aiMode)
			{
			case _AIMode.Threat:
			{
				UnitTB unitTB = this.GetHighestThreat();
				if (unitTB == null)
				{
					unitTB = UnitControl.GetNearestHostile(this);
				}
				if (unitTB != null)
				{
					tile = unitTB.occupiedTile;
				}
				break;
			}
			case _AIMode.Chase:
				if (this.aiTarget != null)
				{
					UnitTB unitTB = this.aiTarget;
					tile = this.aiTarget.occupiedTile;
				}
				break;
			case _AIMode.Follow:
				if (this.aiTarget != null)
				{
					UnitTB unitTB = this.aiTarget;
					tile = this.aiTarget.occupiedTile;
				}
				break;
			case _AIMode.Guard:
				num = 1000f;
				if (this.aiTile != null)
				{
					tile = this.aiTile;
				}
				break;
			case _AIMode.Protect:
				if (this.aiTarget != null)
				{
					UnitTB unitTB = this.aiTarget;
					tile = this.aiTarget.occupiedTile;
				}
				break;
			}
		}
		if (tile == null)
		{
			return 0f;
		}
		int num2 = GridManager.WalkDistance(this.occupiedTile, tile);
		int num3 = GridManager.WalkDistance(moveTile, tile);
		float result;
		if (flag)
		{
			int num4 = num2 - num3;
			result = num * (float)num4;
		}
		else
		{
			int num5 = num3 - num2;
			result = num * (float)num5;
		}
		return result;
	}

	// Token: 0x06002E48 RID: 11848 RVA: 0x001642A4 File Offset: 0x001624A4
	public float GetAITileRate(Tile moveTile)
	{
		float num = 0.1f;
		float num2 = 0f;
		Tile tile = null;
		bool flag = true;
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt, null))
		{
			UnitTB unitTB = UnitControl.instance.FindUnit(this.FindConditionEffectPartID(_EffectPartType.Taunt));
			if (unitTB == null)
			{
				Debug.LogError("WTF Cant found Taunt unit ");
				return 0f;
			}
			tile = unitTB.occupiedTile;
		}
		else if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee, null))
		{
			UnitTB unitTB = UnitControl.instance.FindUnit(this.FindConditionEffectPartID(_EffectPartType.Flee));
			if (unitTB == null)
			{
				Debug.LogError("WTF Cant found Flee unit ");
				return 0f;
			}
			tile = unitTB.occupiedTile;
			flag = false;
		}
		else if (this.bNightFragrance)
		{
			if (this.lastAttackTileList.Count > 0)
			{
				tile = this.lastAttackTileList[0];
			}
			flag = false;
		}
		else
		{
			switch (this.aiMode)
			{
			case _AIMode.Threat:
			{
				UnitTB unitTB = this.GetHighestThreat();
				if (unitTB == null)
				{
					unitTB = UnitControl.GetNearestHostile(this);
				}
				if (unitTB != null)
				{
					tile = unitTB.occupiedTile;
				}
				break;
			}
			case _AIMode.Stand:
				num2 = 0.1f;
				break;
			case _AIMode.Chase:
				if (this.aiTarget != null)
				{
					UnitTB unitTB = this.aiTarget;
					tile = this.aiTarget.occupiedTile;
				}
				break;
			case _AIMode.Follow:
				if (this.aiTarget != null)
				{
					UnitTB unitTB = this.aiTarget;
					tile = this.aiTarget.occupiedTile;
				}
				break;
			case _AIMode.Guard:
				num = 0.2f;
				num2 = 0.1f;
				if (this.aiTile != null)
				{
					tile = this.aiTile;
				}
				break;
			case _AIMode.Protect:
				if (this.aiTarget != null)
				{
					UnitTB unitTB = this.aiTarget;
					tile = this.aiTarget.occupiedTile;
				}
				break;
			}
		}
		if (tile == null)
		{
			return 0f;
		}
		int num3 = GridManager.WalkDistance(this.occupiedTile, tile);
		int num4 = GridManager.WalkDistance(moveTile, tile);
		float num6;
		if (flag)
		{
			int num5 = num3 - num4;
			num6 = num * (float)num5;
			num6 += num2;
		}
		else
		{
			int num7 = num4 - num3;
			num6 = num * (float)num7;
			num6 += num2;
		}
		return num6;
	}

	// Token: 0x06002E49 RID: 11849 RVA: 0x0001DA69 File Offset: 0x0001BC69
	public UnitAbility GetCountAttackAbility()
	{
		if (this.unitAbilityList.Count <= 0)
		{
			return null;
		}
		return this.unitAbilityList[0];
	}

	// Token: 0x06002E4A RID: 11850 RVA: 0x0016451C File Offset: 0x0016271C
	public AttackInstance MoveFlower(AttackInstance attInstance)
	{
		UnitAbility unitAbility;
		if (this.CheckSameWeapon(attInstance.srcUnit))
		{
			unitAbility = attInstance.unitAbility;
			attInstance.counterAttacking = true;
			base.StartCoroutine(this.MoveFlowerRoutine(attInstance.srcUnit, unitAbility, true));
			return attInstance;
		}
		if (this.unitAbilityList.Count <= 0)
		{
			return attInstance;
		}
		unitAbility = this.unitAbilityList[0];
		if (unitAbility.effectType == _EffectType.Heal || unitAbility.effectType == _EffectType.Buff)
		{
			return attInstance;
		}
		List<Tile> abilityAttackAbleTiles = this.GetAbilityAttackAbleTiles(this.occupiedTile, this.unitAbilityList[0]);
		if (abilityAttackAbleTiles.Count <= 0)
		{
			return attInstance;
		}
		if (abilityAttackAbleTiles.Contains(attInstance.srcUnit.occupiedTile))
		{
			attInstance.counterAttacking = true;
			base.StartCoroutine(this.MoveFlowerRoutine(attInstance.srcUnit, unitAbility, false));
		}
		return attInstance;
	}

	// Token: 0x06002E4B RID: 11851 RVA: 0x001645F4 File Offset: 0x001627F4
	public AttackInstance CheckCounterAttack(AttackInstance attInstance)
	{
		if ((attInstance.type == _AttackType.Skill_Normal || attInstance.type == _AttackType.Skill_DoubleAttack) && GameControlTB.IsCounterAttackEnabled() && !this.IsDestroyed() && attInstance.unitAbility.effectType != _EffectType.Heal && attInstance.unitAbility.effectType != _EffectType.Buff && !this.GetCounterAttackDisabled() && !attInstance.srcUnit.IsDestroyed() && attInstance.counterattack && !attInstance.protect && this.unitAbilityList.Count >= 1)
		{
			if (this.GetUnitTileAbsoluteDebuff(_EffectPartType.AttackPreturn, null))
			{
				return attInstance;
			}
			UnitAbility unitAbility;
			bool bCounterAbility;
			if (this.GetUnitTileAbsoluteBuff(_EffectPartType.CounterAbility, null) && this.CheckSameWeapon(attInstance.srcUnit))
			{
				unitAbility = attInstance.unitAbility;
				bCounterAbility = true;
			}
			else
			{
				unitAbility = this.unitAbilityList[0];
				bCounterAbility = false;
				if (this.unitAbilityList[0].cooldown > 0)
				{
					return attInstance;
				}
			}
			if (unitAbility.effectType == _EffectType.Heal || unitAbility.effectType == _EffectType.Buff)
			{
				return attInstance;
			}
			List<Tile> abilityAttackAbleTiles = this.GetAbilityAttackAbleTiles(this.occupiedTile, unitAbility);
			if (abilityAttackAbleTiles.Count <= 0)
			{
				return attInstance;
			}
			if (abilityAttackAbleTiles.Contains(attInstance.srcUnit.occupiedTile))
			{
				this.EffectPartAfterSuccess(_EffectPartType.CounterAttackChance, null, attInstance.srcUnit);
				this.ApplyCounterAttack();
				attInstance.counterAttacking = true;
				base.StartCoroutine(this.CounterAttackRoutine(attInstance.srcUnit, unitAbility, bCounterAbility));
			}
		}
		return attInstance;
	}

	// Token: 0x06002E4C RID: 11852 RVA: 0x00002C2D File Offset: 0x00000E2D
	private bool CheckWeaponRoutineSame()
	{
		return false;
	}

	// Token: 0x06002E4D RID: 11853 RVA: 0x00002C2D File Offset: 0x00000E2D
	private bool CheckSameWeapon(UnitTB targetUnit)
	{
		return false;
	}

	// Token: 0x06002E4E RID: 11854 RVA: 0x00164774 File Offset: 0x00162974
	public bool CheckUnWeapon(int iSkillID)
	{
		int routineMartialType = this.GetRoutineMartialType(iSkillID);
		return routineMartialType == 5 || routineMartialType == 4 || routineMartialType == 0;
	}

	// Token: 0x06002E4F RID: 11855 RVA: 0x001647A0 File Offset: 0x001629A0
	public void ApplyHitEffect(AttackInstance attInstance)
	{
		this.AddThreat(attInstance);
		this.ApplyDamageAndBattleMessage(attInstance);
		if (UnitTB.onAttackE != null && !attInstance.heal)
		{
			UnitTB.onAttackE(attInstance.srcUnit, this);
		}
		if (!attInstance.heal && !attInstance.missed)
		{
			this.RemoveConditionOnHit();
		}
	}

	// Token: 0x06002E50 RID: 11856 RVA: 0x00164800 File Offset: 0x00162A00
	private IEnumerator CounterAttackRoutine(UnitTB srcUnit, UnitAbility uab, bool bCounterAbility)
	{
		if (!this.GetUnitTileAbsoluteBuff(_EffectPartType.CounterPreturn, uab))
		{
			this.counterAttackRemain--;
		}
		yield return new WaitForSeconds(0.1f);
		if (srcUnit.IsDestroyed())
		{
			srcUnit.CounterAttackComplete();
			yield break;
		}
		UnitAbility uabTemp = uab.Clone();
		this.CheckTalentCountAttack(uabTemp);
		this.CounterAttack(srcUnit, uabTemp, bCounterAbility, true);
		yield break;
	}

	// Token: 0x06002E51 RID: 11857 RVA: 0x00164848 File Offset: 0x00162A48
	private IEnumerator MoveFlowerRoutine(UnitTB srcUnit, UnitAbility uab, bool bCounterAbility)
	{
		yield return new WaitForSeconds(0.1f);
		if (srcUnit.IsDestroyed())
		{
			srcUnit.CounterAttackComplete();
			yield break;
		}
		this.CounterAttack(srcUnit, uab, bCounterAbility, false);
		yield break;
	}

	// Token: 0x06002E52 RID: 11858 RVA: 0x00164890 File Offset: 0x00162A90
	public void ApplyCounterAttack()
	{
		if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible())
		{
			new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.Counter, 0f);
		}
	}

	// Token: 0x06002E53 RID: 11859 RVA: 0x001648E8 File Offset: 0x00162AE8
	public int ApplyHeal(int val)
	{
		if (this.IsDestroyed() && !this.bCheckTalentDeadthNow)
		{
			return 0;
		}
		int num = Mathf.Min(this.GetFullHP() - this.HP, val);
		this.HP += val;
		this.HP = Mathf.Min(this.GetFullHP(), this.HP);
		this.UpdateEffectCompare(_EffectPartType.HitPoint, this.HP, this.GetFullHP());
		if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible() && num != 0)
		{
			float num2 = Time.time;
			if (this.fLastNumberTime + 0.5f > num2)
			{
				this.fLastNumberTime += 0.5f;
				num2 = this.fLastNumberTime + 0.5f - num2;
			}
			else
			{
				this.fLastNumberTime = num2;
				num2 = 0f;
			}
			new EffectOverlay(this.thisT.position + new Vector3(0f, 1f, 0f), num.ToString(), _OverlayType.HealthUp, num2);
		}
		return num;
	}

	// Token: 0x06002E54 RID: 11860 RVA: 0x001649F4 File Offset: 0x00162BF4
	public int ApplyInternalForce(int val)
	{
		int num = Mathf.Min(this.GetFullSP() - this.SP, val);
		this.SP += val;
		this.SP = Mathf.Min(this.GetFullSP(), this.SP);
		this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
		if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible() && num != 0)
		{
			float num2 = Time.time;
			if (this.fLastNumberTime + 0.5f > num2)
			{
				this.fLastNumberTime += 0.5f;
				num2 = this.fLastNumberTime + 0.5f - num2;
			}
			else
			{
				this.fLastNumberTime = num2;
				num2 = 0f;
			}
			new EffectOverlay(this.thisT.position + new Vector3(0f, 1f, 0f), num.ToString(), _OverlayType.InternalForceUp, num2);
		}
		return num;
	}

	// Token: 0x06002E55 RID: 11861 RVA: 0x00164AE8 File Offset: 0x00162CE8
	public int ApplyLessInternalForce(int val)
	{
		int num = Mathf.Min(this.SP, val);
		this.SP -= val;
		this.SP = Mathf.Max(0, this.SP);
		this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
		if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible() && num != 0)
		{
			float num2 = Time.time;
			if (this.fLastNumberTime + 0.5f > num2)
			{
				this.fLastNumberTime += 0.5f;
				num2 = this.fLastNumberTime + 0.5f - num2;
			}
			else
			{
				this.fLastNumberTime = num2;
				num2 = 0f;
			}
			new EffectOverlay(this.thisT.position + new Vector3(0f, 1f, 0f), num.ToString(), _OverlayType.InternalForceDown, num2);
		}
		return num;
	}

	// Token: 0x06002E56 RID: 11862 RVA: 0x00164BD0 File Offset: 0x00162DD0
	public void ShowBattleMessageDamage(AttackInstance attInstance)
	{
		if (attInstance.captureTarget)
		{
			string msg = string.Format(Game.StringTable.GetString(260045), attInstance.srcUnit.unitName, attInstance.targetUnit.unitName);
			UINGUI.BattleMessage(msg);
			UINGUI.DisplayMessage(msg);
			return;
		}
		if (attInstance.moveFlower)
		{
			UINGUI.BattleMessage(string.Format(Game.StringTable.GetString(260013), attInstance.targetUnit.unitName, attInstance.srcUnit.unitName, attInstance.unitAbility.name));
			return;
		}
		if (attInstance.damageAbsorbToSP)
		{
			UINGUI.BattleMessage(string.Format(Game.StringTable.GetString(260014), new object[]
			{
				attInstance.targetUnit.unitName,
				attInstance.srcUnit.unitName,
				attInstance.unitAbility.name,
				attInstance.damageDone
			}));
			return;
		}
		if (attInstance.missed)
		{
			if (attInstance.isCounterAttack)
			{
				UINGUI.BattleMessage(string.Format(Game.StringTable.GetString(260011), attInstance.srcUnit.unitName, attInstance.targetUnit.unitName));
			}
			else
			{
				UINGUI.BattleMessage(string.Format(Game.StringTable.GetString(260012), attInstance.srcUnit.unitName, attInstance.targetUnit.unitName, attInstance.unitAbility.name));
			}
			return;
		}
		string text;
		if (attInstance.heal)
		{
			text = "[00FF00FF]";
		}
		else
		{
			text = "[FF0000FF]";
		}
		string text2 = string.Empty;
		text2 = text + attInstance.damageDone.ToString() + "[-]";
		string text3 = string.Empty;
		text3 = text + attInstance.damageSPDone.ToString() + "[-]";
		if (attInstance.plusDamage != 0)
		{
			text2 = text2 + " (" + (attInstance.damageDone - attInstance.plusDamage).ToString();
			if (attInstance.plusDamage > 0)
			{
				text2 = text2 + "[00FF00FF] +" + attInstance.plusDamage.ToString() + "[-])";
			}
			else
			{
				text2 = string.Concat(new string[]
				{
					text2,
					text,
					" ",
					attInstance.plusDamage.ToString(),
					"[-])"
				});
			}
		}
		if (attInstance.heal)
		{
			if (attInstance.critical)
			{
				UINGUI.BattleMessage(string.Format(Game.StringTable.GetString(260040), new object[]
				{
					attInstance.srcUnit.unitName,
					attInstance.targetUnit.unitName,
					attInstance.unitAbility.name,
					text2
				}));
			}
			else if (attInstance.damageDone > 0)
			{
				UINGUI.BattleMessage(string.Format(Game.StringTable.GetString(260039), new object[]
				{
					attInstance.srcUnit.unitName,
					attInstance.targetUnit.unitName,
					attInstance.unitAbility.name,
					text2
				}));
			}
			if (attInstance.damageSPDone > 0)
			{
				UINGUI.BattleMessage(string.Format(Game.StringTable.GetString(260038), new object[]
				{
					attInstance.srcUnit.unitName,
					attInstance.targetUnit.unitName,
					attInstance.unitAbility.name,
					text3
				}));
			}
		}
		else if (attInstance.protect)
		{
			if (attInstance.protectUnit.GetUnitTileAbsoluteBuff(_EffectPartType.HitPoint, null))
			{
				text2 = Game.StringTable.GetString(260037);
			}
			if (attInstance.critical)
			{
				string msg = string.Format(Game.StringTable.GetString(260008), new object[]
				{
					attInstance.srcUnit.unitName,
					attInstance.targetUnit.unitName,
					attInstance.unitAbility.name,
					attInstance.protectUnit.unitName,
					text2
				});
				UINGUI.BattleMessage(msg);
			}
			else
			{
				string msg = string.Format(Game.StringTable.GetString(260009), new object[]
				{
					attInstance.srcUnit.unitName,
					attInstance.targetUnit.unitName,
					attInstance.unitAbility.name,
					attInstance.protectUnit.unitName,
					text2
				});
				UINGUI.BattleMessage(msg);
			}
			float num = attInstance.protectUnit.GetEffectPartValuePercent(_EffectPartType.DamageReflex, null, 0);
			if (attInstance.protectUnit.occupiedTile != null)
			{
				num += attInstance.protectUnit.occupiedTile.GetEffectPartValuePercent(_EffectPartType.DamageReflex, attInstance.protectUnit);
			}
			num *= 0.01f;
			if (num > 0f)
			{
				num = (float)attInstance.damageDone * num;
				int num2 = Mathf.RoundToInt(num);
				string text4 = text + num2 + "[-]";
				string msg = string.Format(Game.StringTable.GetString(260010), attInstance.srcUnit.unitName, text4);
				UINGUI.BattleMessage(msg);
			}
			if (attInstance.iDefPlusLv > 0)
			{
				string msg = string.Format(Game.StringTable.GetString(260036), attInstance.protectUnit.unitName, Game.StringTable.GetString(attInstance.iAbilityType + 110140), attInstance.iDefPlusLv.ToString());
				UINGUI.BattleMessage(msg);
			}
		}
		else
		{
			if (attInstance.targetUnit.GetUnitTileAbsoluteBuff(_EffectPartType.HitPoint, null))
			{
				text2 = Game.StringTable.GetString(260037);
			}
			string msg;
			if (attInstance.critical)
			{
				if (attInstance.isCounterAttack)
				{
					msg = string.Format(Game.StringTable.GetString(260035), attInstance.srcUnit.unitName, attInstance.targetUnit.unitName, text2);
				}
				else
				{
					msg = string.Format(Game.StringTable.GetString(260034), new object[]
					{
						attInstance.srcUnit.unitName,
						attInstance.targetUnit.unitName,
						attInstance.unitAbility.name,
						text2
					});
				}
			}
			else if (attInstance.isCounterAttack)
			{
				msg = string.Format(Game.StringTable.GetString(260033), attInstance.srcUnit.unitName, attInstance.targetUnit.unitName, text2);
			}
			else
			{
				msg = string.Format(Game.StringTable.GetString(260032), new object[]
				{
					attInstance.srcUnit.unitName,
					attInstance.targetUnit.unitName,
					attInstance.unitAbility.name,
					text2
				});
			}
			UINGUI.BattleMessage(msg);
			float num3 = attInstance.targetUnit.GetEffectPartValuePercent(_EffectPartType.DamageReflex, null, 0);
			if (attInstance.targetUnit.occupiedTile != null)
			{
				num3 += attInstance.targetUnit.occupiedTile.GetEffectPartValuePercent(_EffectPartType.DamageReflex, attInstance.targetUnit);
			}
			num3 *= 0.01f;
			if (num3 > 0f)
			{
				num3 = (float)attInstance.damageDone * num3;
				int num4 = Mathf.RoundToInt(num3);
				string text5 = text + num4 + "[-]";
				msg = string.Format(Game.StringTable.GetString(260010), attInstance.srcUnit.unitName, text5);
				UINGUI.BattleMessage(msg);
			}
			if (attInstance.iDefPlusLv > 0)
			{
				msg = string.Format(Game.StringTable.GetString(260036), attInstance.targetUnit.unitName, Game.StringTable.GetString(attInstance.iAbilityType + 110140), attInstance.iDefPlusLv.ToString());
				UINGUI.BattleMessage(msg);
			}
		}
	}

	// Token: 0x06002E57 RID: 11863 RVA: 0x0016538C File Offset: 0x0016358C
	public void ApplyDamageAndBattleMessage(AttackInstance attInstance)
	{
		this.ShowBattleMessageDamage(attInstance);
		int num = 0;
		if (attInstance.moveFlower)
		{
			new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.RemoveFlower, 0f);
			return;
		}
		if (attInstance.damageAbsorbToSP)
		{
			new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.DamageAbsorbToSP, 0f);
			this.ApplyInternalForce(attInstance.damageDone);
			return;
		}
		if (attInstance.missed)
		{
			this.EffectPartAfterSuccess(_EffectPartType.DodgeChance, null, attInstance.srcUnit);
			if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible())
			{
				new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.Miss, 0f);
			}
			float num2 = this.fDodgeSpRate * (float)this.GetFullSP();
			this.ApplyLessInternalForce(Mathf.RoundToInt(num2));
			return;
		}
		if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible() && attInstance.critical && attInstance.unitAbility.effectType != _EffectType.Heal && attInstance.unitAbility.effectType != _EffectType.Buff)
		{
			new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), attInstance.damageDone.ToString(), _OverlayType.Critical, 0f);
			this.CheckTalentBeCritical();
		}
		if (attInstance.heal)
		{
			if (attInstance.damageSPDone > 0)
			{
				int num3 = this.ApplyInternalForce(attInstance.damageSPDone);
				float fDamageDone = this.fHealSpExpRate * (float)num3;
				if (UnitTB.onDamage != null)
				{
					UnitTB.onDamage(attInstance.srcUnit, fDamageDone);
				}
			}
			if (attInstance.damageDone > 0)
			{
				int num3 = this.ApplyHeal(attInstance.damageDone);
				float fDamageDone = this.fHealHpExpRate * (float)num3;
				if (UnitTB.onDamage != null)
				{
					UnitTB.onDamage(attInstance.srcUnit, fDamageDone);
				}
			}
			if (attInstance.cleanDebuffCount > 0)
			{
				this.RemoveDebuff(attInstance);
			}
		}
		else
		{
			int dmg = attInstance.damageDone;
			float num4 = this.GetEffectPartValuePercent(_EffectPartType.DamageReflex, null, 0);
			if (this.occupiedTile != null)
			{
				num4 += this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.DamageReflex, this);
			}
			num4 *= 0.01f;
			float num5 = this.GetEffectPartValuePercent(_EffectPartType.InternalForceShield, null, 0);
			if (this.occupiedTile != null)
			{
				num5 += this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.InternalForceShield, this);
			}
			num5 *= 0.01f;
			num5 = Mathf.Clamp(num5, 0f, 1f);
			if (num4 > 0f)
			{
				num4 = (float)attInstance.damageDone * num4;
				int dmg2 = Mathf.RoundToInt(num4);
				attInstance.srcUnit.ApplyDamage(dmg2, null);
			}
			if (num5 > 0f)
			{
				num = Mathf.RoundToInt(num5 * (float)attInstance.damageDone);
				if (this.SP >= num)
				{
					dmg = Mathf.RoundToInt((1f - num5) * (float)attInstance.damageDone);
				}
				else
				{
					num = this.SP;
					dmg = attInstance.damageDone - this.SP;
				}
			}
			int num3 = this.ApplyDamage(dmg, attInstance.srcUnit);
			float fDamageDone = this.fTankHpExpRate * (float)num3;
			if (UnitTB.onDamage != null)
			{
				UnitTB.onDamage(this, fDamageDone);
			}
			fDamageDone = this.fDamageExpRate * (float)num3;
			if (UnitTB.onDamage != null)
			{
				UnitTB.onDamage(attInstance.srcUnit, fDamageDone);
			}
			if (num > 0)
			{
				num3 = this.ApplyLessInternalForce(num);
				fDamageDone = this.fTankHpExpRate * (float)num3;
				if (UnitTB.onDamage != null)
				{
					UnitTB.onDamage(this, fDamageDone);
				}
			}
		}
	}

	// Token: 0x06002E58 RID: 11864 RVA: 0x0016577C File Offset: 0x0016397C
	public int ApplyDamage(int dmg, UnitTB unitSrc = null)
	{
		if (this.GetUnitTileAbsoluteBuff(_EffectPartType.HitPoint, null))
		{
			dmg = 0;
			if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible())
			{
				float num = Time.time;
				if (this.fLastNumberTime + 0.5f > num)
				{
					this.fLastNumberTime += 0.5f;
					num = this.fLastNumberTime + 0.5f - num;
				}
				else
				{
					this.fLastNumberTime = num;
					num = 0f;
				}
				new EffectOverlay(this.thisT.position + new Vector3(0f, 1f, 0f), dmg.ToString(), _OverlayType.HealthDown, num);
			}
		}
		if (dmg == 0)
		{
			return dmg;
		}
		if (unitSrc != null && dmg >= this.HP && unitSrc.CheckTalentBuddhaMercy())
		{
			dmg = this.HP - 1;
		}
		if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible())
		{
			float num2 = Time.time;
			if (this.fLastNumberTime + 0.5f > num2)
			{
				this.fLastNumberTime += 0.5f;
				num2 = this.fLastNumberTime + 0.5f - num2;
			}
			else
			{
				this.fLastNumberTime = num2;
				num2 = 0f;
			}
			new EffectOverlay(this.thisT.position + new Vector3(0f, 1f, 0f), dmg.ToString(), _OverlayType.HealthDown, num2);
		}
		return this._ApplyDamage(dmg, unitSrc);
	}

	// Token: 0x06002E59 RID: 11865 RVA: 0x0001DA8A File Offset: 0x0001BC8A
	public void LeaveBattle(bool bRunOut)
	{
		base.StartCoroutine(this.Leave(bRunOut));
	}

	// Token: 0x06002E5A RID: 11866 RVA: 0x001658FC File Offset: 0x00163AFC
	private IEnumerator ChargeMove(Tile chargeTile)
	{
		GameObject effect = null;
		string path = "Effects/CharacterEffectShadow/ChiliShadowEffect02";
		if (Game.g_EffectsBundle.Contains(path))
		{
			effect = (Game.g_EffectsBundle.Load(path) as GameObject);
		}
		GameObject goEffect = Object.Instantiate(effect) as GameObject;
		goEffect.tag = "DynamicEffect";
		goEffect.transform.parent = this.animationTB.moveAniBody.transform;
		Tile OrigTile = this.occupiedTile;
		OrigTile.ClearUnit();
		this.animationTB.PlayMove();
		Quaternion wantedRot = Quaternion.LookRotation(chargeTile.pos - OrigTile.pos);
		float fovertime = 0f;
		for (;;)
		{
			fovertime += Time.deltaTime;
			float frot = Time.deltaTime * this.rotateSpeed;
			frot = Mathf.Clamp(frot, 0.05f, 1f);
			this.thisT.rotation = Quaternion.Slerp(this.thisT.rotation, wantedRot, frot);
			if (Quaternion.Angle(this.thisT.rotation, wantedRot) < 2f || fovertime > 0.5f)
			{
				break;
			}
			yield return null;
		}
		this.thisT.rotation = wantedRot;
		fovertime = 0f;
		for (;;)
		{
			fovertime += Time.deltaTime;
			float dist = Vector3.Distance(this.thisT.position, chargeTile.pos);
			if (dist < Time.deltaTime * this.moveSpeed * 1.5f || dist < 0.1f || fovertime > 0.5f)
			{
				break;
			}
			Vector3 vPosDiff = chargeTile.pos - this.thisT.position;
			vPosDiff.Normalize();
			vPosDiff = vPosDiff * Time.deltaTime * this.moveSpeed * 1.5f;
			this.thisT.position = this.thisT.position + vPosDiff;
			yield return null;
		}
		this.thisT.position = chargeTile.pos;
		this.occupiedTile = chargeTile;
		this.occupiedTile.SetUnit(this);
		this.animationTB.PlayIdle();
		this.bSpeicalAction = false;
		Object.Destroy(goEffect, 0.05f);
		yield return new WaitForSeconds(0.05f);
		yield break;
	}

	// Token: 0x06002E5B RID: 11867 RVA: 0x00165928 File Offset: 0x00163B28
	private IEnumerator Leave(bool bRunOut)
	{
		this.bLeaveBattle = true;
		this.HP = 0;
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		UnitTB enemyUnit = UnitControl.GetNearestHostile(this);
		Tile OrigTile = this.occupiedTile;
		if (UnitTB.onUnitDestroyedE != null)
		{
			UnitTB.onUnitDestroyedE(this);
		}
		this.occupiedTile.ClearUnit();
		if (GameControlTB.IsPlayerFaction(this.factionID) && UnitTB.onTurnDepletedE != null)
		{
			UnitTB.onTurnDepletedE();
		}
		if (bRunOut)
		{
			this.animationTB.PlayMove();
			string path = "Effects/CharacterEffectShadow/ShiyanShadowEffect01";
			if (Game.g_EffectsBundle.Contains(path))
			{
				GameObject effect = Game.g_EffectsBundle.Load(path) as GameObject;
				GameObject goEffect = Object.Instantiate(effect) as GameObject;
				goEffect.tag = "DynamicEffect";
				goEffect.transform.parent = this.animationTB.moveAniBody.transform;
				Object.Destroy(goEffect, 5f);
			}
			float time = 0f;
			Tile targetTile = null;
			if (enemyUnit != null)
			{
				targetTile = enemyUnit.occupiedTile;
			}
			else
			{
				targetTile = GridManager.GetAllTiles()[0];
			}
			Quaternion wantedRot = Quaternion.LookRotation(OrigTile.pos - targetTile.pos);
			float fovertime = 0f;
			for (;;)
			{
				fovertime += Time.deltaTime;
				float frot = Time.deltaTime * this.rotateSpeed;
				frot = Mathf.Clamp(frot, 0.05f, 1f);
				this.thisT.rotation = Quaternion.Slerp(this.thisT.rotation, wantedRot, frot);
				if (Quaternion.Angle(this.thisT.rotation, wantedRot) < 2f || fovertime > 0.5f)
				{
					break;
				}
				yield return null;
			}
			this.thisT.rotation = wantedRot;
			Vector3 vPosDiff = this.thisT.position - targetTile.pos;
			vPosDiff.Normalize();
			while (time < 4f)
			{
				time += Time.deltaTime;
				this.thisT.position = this.thisT.position + vPosDiff * Time.deltaTime * this.moveSpeed * 0.3f;
				yield return null;
			}
		}
		if (UnitControl.HideUnitWhenKilled())
		{
			this.thisT.position = new Vector3(0f, 999999f, 0f);
		}
		yield return new WaitForSeconds(0.25f);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002E5C RID: 11868 RVA: 0x00165954 File Offset: 0x00163B54
	public int _ApplyDamage(int dmg, UnitTB unitSrc)
	{
		if (this.IsDestroyed())
		{
			return 0;
		}
		int result = Mathf.Min(this.HP, dmg);
		this.HP -= dmg;
		this.HP = Mathf.Max(0, this.HP);
		if (this.HP <= 0)
		{
			this.CheckTalentDeadth(unitSrc);
		}
		else
		{
			this.CheckTalentAutoQuit();
		}
		if (this.HP <= 0)
		{
			if (UnitTB.onUnitDestroyedE != null)
			{
				UnitTB.onUnitDestroyedE(this);
			}
			this.occupiedTile.ClearUnit();
			if (GameGlobal.m_bDLCMode && this.CheckFriendFaction(0))
			{
				if (this.characterData.bCaptive)
				{
					TeamStatus.m_Instance._DeleteDLCUnit(this.dlcCharGuid);
				}
				else
				{
					this.characterData.iHurtTurn = 5;
				}
			}
			base.StartCoroutine(this.Destroyed());
			return 2 * dmg;
		}
		this.UpdateEffectCompare(_EffectPartType.HitPoint, this.HP, this.GetFullHP());
		return result;
	}

	// Token: 0x06002E5D RID: 11869 RVA: 0x00165A54 File Offset: 0x00163C54
	private IEnumerator Destroyed()
	{
		this.audioTB.PlayDestroy();
		float aniDuration = this.animationTB.PlayDestroyed();
		if (this.destroyedEffect != null)
		{
			GameObject obj = (GameObject)Object.Instantiate(this.destroyedEffect, this.thisT.position, this.thisT.rotation);
			Object.Destroy(obj, 10f);
		}
		this.destroyEffectDuration = aniDuration + 0.25f;
		yield return new WaitForSeconds(this.destroyEffectDuration);
		while (GameGlobal.m_bBattleTalk)
		{
			yield return null;
		}
		while (GameControlTB.IsActionInProgress())
		{
			yield return null;
		}
		if (UnitControl.HideUnitWhenKilled())
		{
			this.thisT.position = new Vector3(0f, 999999f, 0f);
		}
		yield return new WaitForSeconds(0.25f);
		if (UnitControl.DestroyUnitObject())
		{
			Object.Destroy(base.gameObject);
		}
		yield break;
	}

	// Token: 0x06002E5E RID: 11870 RVA: 0x0001DA9A File Offset: 0x0001BC9A
	public void RotateToUnit(UnitTB unit)
	{
		if (unit == null)
		{
			return;
		}
		base.StartCoroutine(this._RotateToUnit(unit));
	}

	// Token: 0x06002E5F RID: 11871 RVA: 0x00165A70 File Offset: 0x00163C70
	private IEnumerator _RotateToUnit(UnitTB unit)
	{
		Vector3 pos = this.occupiedTile.pos;
		Vector3 pos2 = unit.occupiedTile.pos;
		pos2.y = pos.y;
		Quaternion wantedRot = Quaternion.LookRotation(pos2 - pos);
		float fovertime = 0f;
		for (;;)
		{
			fovertime += Time.deltaTime;
			float frot = Time.deltaTime * this.rotateSpeed;
			frot = Mathf.Clamp(frot, 0.05f, 1f);
			this.thisT.rotation = Quaternion.Slerp(this.thisT.rotation, wantedRot, frot);
			if (Quaternion.Angle(this.thisT.rotation, wantedRot) < 2f || fovertime > 0.5f)
			{
				break;
			}
			yield return null;
		}
		this.thisT.rotation = wantedRot;
		yield break;
	}

	// Token: 0x06002E60 RID: 11872 RVA: 0x00165A9C File Offset: 0x00163C9C
	public void GainSP(int val)
	{
		this.SP = Mathf.Clamp(this.SP += val, 0, this.GetFullSP());
		this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
	}

	// Token: 0x06002E61 RID: 11873 RVA: 0x00165AE0 File Offset: 0x00163CE0
	public bool IsAllActionCompleted()
	{
		bool flag = GameControlTB.MovementAPCostRule() != _MovementAPCostRule.None;
		bool flag2 = GameControlTB.AttackAPCostRule() != _AttackAPCostRule.None;
		if (this.IsDestroyed())
		{
			return true;
		}
		if (flag && flag2 && this.SP <= 0)
		{
			return true;
		}
		if (flag && this.SP <= 0)
		{
			this.moved = true;
		}
		if (flag2 && this.SP <= 0)
		{
			this.attacked = true;
		}
		if (this.SP <= 0 && this.moved && this.attacked)
		{
			return true;
		}
		bool flag3 = GameControlTB.AllowAbilityAfterAttack();
		bool flag4 = GameControlTB.AllowMovementAfterAttack();
		bool flag5 = this.IsAllAbilityDisabled();
		if (UnitControl.activeFactionCount <= 1)
		{
			return true;
		}
		if (!GameControlTB.IsPlayerFaction(this.factionID))
		{
			flag5 = true;
		}
		if (flag3 && flag4)
		{
			return (this.moved & this.attacked) && flag5;
		}
		if (!flag3 && flag4)
		{
			if (this.attacked)
			{
				return this.moved;
			}
			return this.moved & this.attacked;
		}
		else
		{
			if (!flag3 || flag4)
			{
				return !flag3 && !flag4 && this.attacked;
			}
			if (this.attacked)
			{
				return flag5;
			}
			return flag5 & this.attacked;
		}
	}

	// Token: 0x06002E62 RID: 11874 RVA: 0x00165C38 File Offset: 0x00163E38
	public bool IsAllAbilityDisabled()
	{
		if (this.stun > 0 || this.abilityDisabled > 0)
		{
			return true;
		}
		bool result = false;
		for (int i = 0; i < this.unitAbilityList.Count; i++)
		{
			if (this.IsAbilityAvailable(i) != 0)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06002E63 RID: 11875 RVA: 0x00165C8C File Offset: 0x00163E8C
	private IEnumerator UnitItemTargetSelectedRoutine(List<Tile> tileList)
	{
		UnitAbility unitAbility = AbilityManagerTB.GetUnitAbility(this.activeItemPendingTarget.GetSkillID());
		this.activeItemPendingTarget = null;
		yield return null;
		UnitAbility unitAbilityTemp = unitAbility.Clone();
		unitAbilityTemp.bItemSkill = true;
		this.Attack(tileList, unitAbilityTemp);
		yield break;
	}

	// Token: 0x06002E64 RID: 11876 RVA: 0x00165CB8 File Offset: 0x00163EB8
	private IEnumerator UnitAbilityTargetSelectedRoutine(List<Tile> tileList)
	{
		UnitAbility unitAbility = this.activeAbilityPendingTarget;
		this.activeAbilityPendingTarget = null;
		yield return null;
		this.Attack(tileList, unitAbility);
		yield break;
	}

	// Token: 0x06002E65 RID: 11877 RVA: 0x0001DAB7 File Offset: 0x0001BCB7
	private void CheckWeaponRoutine(ItemDataNode pItemDataNode)
	{
		if (this.CheckWeaponRoutineSame())
		{
			return;
		}
	}

	// Token: 0x06002E66 RID: 11878 RVA: 0x00165CE4 File Offset: 0x00163EE4
	private int EquipItem(ItemDataNode pItemDataNode)
	{
		if (!this.bMainPlayer)
		{
			return 6;
		}
		if (pItemDataNode == null)
		{
			return 9;
		}
		ItemDataNode itemDataNode = null;
		if (itemDataNode != null)
		{
			foreach (ItmeEffectNode itmeEffectNode in itemDataNode.m_ItmeEffectNodeList)
			{
				if (itmeEffectNode.m_iItemType == 15 && itmeEffectNode.m_iValue != 0)
				{
					this.RemoveConditionID(itmeEffectNode.m_iValue, itemDataNode.m_strItemName);
				}
			}
		}
		foreach (ItmeEffectNode itmeEffectNode2 in pItemDataNode.m_ItmeEffectNodeList)
		{
			if (itmeEffectNode2.m_iItemType == 15 && itmeEffectNode2.m_iValue != 0)
			{
				this.ApplyConditionID(itmeEffectNode2.m_iValue, pItemDataNode.m_strItemName, true);
			}
		}
		if (pItemDataNode.m_iItemType == 1)
		{
			this.CheckWeaponRoutine(pItemDataNode);
		}
		return 0;
	}

	// Token: 0x06002E67 RID: 11879 RVA: 0x00165E04 File Offset: 0x00164004
	private int IsItemAbility(ItemDataNode pItemDataNode)
	{
		if (this.attacked)
		{
			return 5;
		}
		if (pItemDataNode == null)
		{
			return 1;
		}
		if (pItemDataNode.m_iItemUse != 1)
		{
			return 2;
		}
		if (pItemDataNode.m_iUseTime != 2)
		{
			return 3;
		}
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.Giddy, null))
		{
			return 6;
		}
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.Stun, null))
		{
			return 7;
		}
		bool flag = false;
		foreach (ItmeEffectNode itmeEffectNode in pItemDataNode.m_ItmeEffectNodeList)
		{
			if (itmeEffectNode.m_iItemType == 15)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return 4;
		}
		return 0;
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x00165EC8 File Offset: 0x001640C8
	private void ItemActivated(ItemDataNode pItemDataNode)
	{
		if (!GameGlobal.m_bDLCMode)
		{
			this.useItemCD = GameControlTB.UseItemCoolDown() + this.CheckTeamTalentValue(TalentEffect.TeamItemCD, false, false, true) + this.CheckTeamTalentValue(TalentEffect.EnemyItemCD, true, false, false);
		}
		if (BattleControl.instance != null)
		{
			BattleControl.instance.iTacticItemCount--;
			if (UINGUI.instance != null)
			{
				UINGUI.instance.uiHUD.UpdateItemButton();
			}
		}
		BackpackStatus.m_Instance.LessPackItem(pItemDataNode.m_iItemID, 1, null);
	}

	// Token: 0x06002E69 RID: 11881 RVA: 0x00165F5C File Offset: 0x0016415C
	public void AI_ItemActivated(ItemDataNode pItemDataNode)
	{
		if (!GameGlobal.m_bDLCMode)
		{
			this.useItemCD = GameControlTB.UseItemCoolDown() + this.CheckTeamTalentValue(TalentEffect.TeamItemCD, false, false, true) + this.CheckTeamTalentValue(TalentEffect.EnemyItemCD, true, false, false);
		}
		this.characterData.LessNpcItem(pItemDataNode.m_iItemID, 1);
	}

	// Token: 0x06002E6A RID: 11882 RVA: 0x0001DAC5 File Offset: 0x0001BCC5
	public void AI_ActivateUseItemSelf(ItemDataNode pItemDataNode)
	{
		this.attackRemain--;
		if (this.attackRemain <= 0)
		{
			this.moved = true;
			this.attacked = true;
		}
		UnitControl.MoveUnit(this);
		base.StartCoroutine(this.UseItemSelf(pItemDataNode));
	}

	// Token: 0x06002E6B RID: 11883 RVA: 0x00165FB0 File Offset: 0x001641B0
	private void _ActivateUseItemSelf(ItemDataNode pItemDataNode)
	{
		this.attackRemain--;
		if (this.attackRemain <= 0)
		{
			this.moved = true;
			this.attacked = true;
		}
		UnitControl.MoveUnit(this);
		base.StartCoroutine(this.UseItemSelf(pItemDataNode));
		this.ItemActivated(pItemDataNode);
	}

	// Token: 0x06002E6C RID: 11884 RVA: 0x00166000 File Offset: 0x00164200
	private void _ActivateUseItem(ItemDataNode pItemDataNode)
	{
		int skillID = pItemDataNode.GetSkillID();
		UnitAbility unitAbility = AbilityManagerTB.GetUnitAbility(skillID);
		if (unitAbility == null)
		{
			return;
		}
		if (!unitAbility.requireTargetSelection)
		{
			List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(this.occupiedTile, unitAbility.aoeRange);
			tilesWithinRange.Add(this.occupiedTile);
			List<Tile> list = new List<Tile>();
			list.AddRange(tilesWithinRange);
			list = this.FilterTileByAbilityTargetType(list, unitAbility);
			if (list.Count <= 0)
			{
				list.Clear();
				return;
			}
			list.Clear();
			this.ItemActivated(pItemDataNode);
			UnitAbility unitAbility2 = unitAbility.Clone();
			unitAbility2.bItemSkill = true;
			this.Attack(tilesWithinRange, unitAbility2);
		}
		else
		{
			GridManager.SetTargetTileSelectMode(this.occupiedTile, unitAbility);
			this.activeItemPendingTarget = pItemDataNode;
			this.activeAbilityPendingTarget = null;
			UINGUI.instance.DelayBattleControlState(BattleControlState.SelectTarget);
		}
	}

	// Token: 0x06002E6D RID: 11885 RVA: 0x001660CC File Offset: 0x001642CC
	public int UseItem(ItemDataNode pItemDataNode)
	{
		int num = this.IsItemAbility(pItemDataNode);
		if (num <= 0)
		{
			this._ActivateUseItem(pItemDataNode);
			return 0;
		}
		if (num != 4)
		{
			return num;
		}
		if (this.IsSelfItem(pItemDataNode))
		{
			this._ActivateUseItemSelf(pItemDataNode);
			return 0;
		}
		return num;
	}

	// Token: 0x06002E6E RID: 11886 RVA: 0x00166110 File Offset: 0x00164310
	private bool IsSelfItem(ItemDataNode pItemDataNode)
	{
		bool result = false;
		foreach (ItmeEffectNode itmeEffectNode in pItemDataNode.m_ItmeEffectNodeList)
		{
			switch (itmeEffectNode.m_iItemType)
			{
			case 7:
			case 8:
			case 9:
			case 16:
			case 17:
			case 18:
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06002E6F RID: 11887 RVA: 0x001661B8 File Offset: 0x001643B8
	public int ActivateAbility(int ID)
	{
		int num = this.IsAbilityAvailable(ID);
		if (num > 0)
		{
			return num;
		}
		UnitAbility unitAbility = this.unitAbilityList[ID];
		return this._ActivateAbility(unitAbility);
	}

	// Token: 0x06002E70 RID: 11888 RVA: 0x001661EC File Offset: 0x001643EC
	public int _ActivateAbility(UnitAbility unitAbility)
	{
		if (unitAbility.requireTargetSelection)
		{
			GridManager.SetTargetTileSelectMode(this.occupiedTile, unitAbility);
			this.activeAbilityPendingTarget = unitAbility;
			this.activeItemPendingTarget = null;
			UINGUI.instance.DelayBattleControlState(BattleControlState.SelectTarget);
			return 0;
		}
		List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(this.occupiedTile, unitAbility.aoeRange);
		tilesWithinRange.Add(this.occupiedTile);
		List<Tile> list = new List<Tile>();
		list.AddRange(tilesWithinRange);
		list = this.FilterTileByAbilityTargetType(list, unitAbility);
		if (list.Count <= 0)
		{
			list.Clear();
			return 10;
		}
		list.Clear();
		this.AbilityActivated(unitAbility, false);
		this.Attack(tilesWithinRange, unitAbility);
		return 0;
	}

	// Token: 0x06002E71 RID: 11889 RVA: 0x0001DB03 File Offset: 0x0001BD03
	public void ClearSelectedAbility()
	{
		if (this.activeAbilityPendingTarget != null)
		{
			this.activeAbilityPendingTarget = null;
		}
		if (this.activeItemPendingTarget != null)
		{
			this.activeItemPendingTarget = null;
		}
	}

	// Token: 0x06002E72 RID: 11890 RVA: 0x0001DB29 File Offset: 0x0001BD29
	public ItemDataNode GetActiveItemPendingTarget()
	{
		return this.activeItemPendingTarget;
	}

	// Token: 0x06002E73 RID: 11891 RVA: 0x0001DB31 File Offset: 0x0001BD31
	public void SetActiveItemPendingTarget(ItemDataNode item)
	{
		this.activeItemPendingTarget = item;
	}

	// Token: 0x06002E74 RID: 11892 RVA: 0x00166290 File Offset: 0x00164490
	public void UnitAbilityTargetSelected(Tile tile)
	{
		List<Tile> list = new List<Tile>();
		list.Add(tile);
		this.UnitAbilityTargetSelected(list);
	}

	// Token: 0x06002E75 RID: 11893 RVA: 0x001662B4 File Offset: 0x001644B4
	public void UnitAbilityTargetSelected(List<Tile> list)
	{
		if (this.activeAbilityPendingTarget != null)
		{
			List<Tile> list2 = new List<Tile>();
			list2.AddRange(list);
			list2 = this.FilterTileByAbilityTargetType(list2, this.activeAbilityPendingTarget);
			if (list2.Count <= 0)
			{
				list2.Clear();
				this.activeAbilityPendingTarget = null;
				UINGUI.instance.DelayBattleControlState(BattleControlState.SelectAction);
				return;
			}
			list2.Clear();
			this.AbilityActivated(this.activeAbilityPendingTarget, false);
			base.StartCoroutine(this.UnitAbilityTargetSelectedRoutine(list));
			this.activeAbilityPendingTarget = null;
		}
		else if (this.activeItemPendingTarget != null)
		{
			UnitAbility unitAbility = AbilityManagerTB.GetUnitAbility(this.activeItemPendingTarget.GetSkillID());
			List<Tile> list3 = new List<Tile>();
			list3.AddRange(list);
			list3 = this.FilterTileByAbilityTargetType(list3, unitAbility);
			if (list3.Count <= 0)
			{
				list3.Clear();
				this.activeItemPendingTarget = null;
				UINGUI.instance.DelayBattleControlState(BattleControlState.SelectAction);
				return;
			}
			list3.Clear();
			this.ItemActivated(this.activeItemPendingTarget);
			base.StartCoroutine(this.UnitItemTargetSelectedRoutine(list));
			this.activeItemPendingTarget = null;
		}
	}

	// Token: 0x06002E76 RID: 11894 RVA: 0x0001DB3A File Offset: 0x0001BD3A
	public UnitAbility GetActiveAbilityPendingTarget()
	{
		return this.activeAbilityPendingTarget;
	}

	// Token: 0x06002E77 RID: 11895 RVA: 0x0001DB42 File Offset: 0x0001BD42
	public void SetActiveAbilityPendingTarget(UnitAbility uAB)
	{
		this.activeAbilityPendingTarget = uAB;
	}

	// Token: 0x06002E78 RID: 11896 RVA: 0x001663C0 File Offset: 0x001645C0
	public void AbilityActivated(UnitAbility uAB, bool reselect)
	{
		GameControlTB.LockUnitSwitching();
		this.abilityTriggered = true;
		UnitControl.MoveUnit(this);
		this.SP -= this.GetAbilityCost(uAB, false);
		this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
		uAB.cooldown = uAB.cdDuration;
		uAB.countTillNextTurn = UnitControl.activeFactionCount;
		uAB.useCount++;
		if (reselect)
		{
			GridManager.Select(this.occupiedTile);
		}
	}

	// Token: 0x06002E79 RID: 11897 RVA: 0x00166440 File Offset: 0x00164640
	private IEnumerator SpawnAbilityEffect(GameObject effect, float delay, Vector3 pos)
	{
		yield return new WaitForSeconds(delay);
		Object.Instantiate(effect, pos, effect.transform.rotation);
		yield break;
	}

	// Token: 0x06002E7A RID: 11898 RVA: 0x0001DB4B File Offset: 0x0001BD4B
	public int GetAbilityCD(int ID)
	{
		return this.unitAbilityList[ID].cooldown;
	}

	// Token: 0x06002E7B RID: 11899 RVA: 0x00166480 File Offset: 0x00164680
	public int IsAbilityAvailable(int ID)
	{
		if (ID >= this.unitAbilityList.Count)
		{
			return 11;
		}
		if (this.activeAbilityPendingTarget != null && this.activeAbilityPendingTarget.ID == this.unitAbilityList[ID].ID)
		{
			return 7;
		}
		if (!GameControlTB.AllowAbilityAfterAttack() && this.attacked)
		{
			return 6;
		}
		if (this.stun > 0)
		{
			return 4;
		}
		if (this.abilityDisabled > 0)
		{
			return 5;
		}
		UnitAbility unitAbility = this.unitAbilityList[ID];
		if (unitAbility.useLimit > 0 && unitAbility.useCount >= unitAbility.useLimit)
		{
			return 1;
		}
		if (this.SP < this.GetAbilityCost(unitAbility, false))
		{
			return 2;
		}
		if (unitAbility.cooldown > 0)
		{
			return 3;
		}
		if (this.GetAttackDisabled())
		{
			return 9;
		}
		return 0;
	}

	// Token: 0x06002E7C RID: 11900 RVA: 0x00166560 File Offset: 0x00164760
	public void ApplyConditionID(int ID, string name, bool bShowContinueLog)
	{
		if (ID == 0)
		{
			return;
		}
		ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(ID);
		if (conditionNode == null)
		{
			return;
		}
		ConditionNode conditionNode2 = conditionNode.Clone();
		if (conditionNode2 != null)
		{
			this.ApplyCondition(conditionNode2, bShowContinueLog);
		}
	}

	// Token: 0x06002E7D RID: 11901 RVA: 0x001665A4 File Offset: 0x001647A4
	private void RemoveConditionByAttack()
	{
		int i = 0;
		while (i < this.unitConditionList.Count)
		{
			ConditionNode conditionNode = this.unitConditionList[i];
			if (conditionNode.m_iCondType == _ConditionType.StackBuff || conditionNode.m_iCondType == _ConditionType.StackDebuff || conditionNode.m_iRemoveByAttack <= 0)
			{
				i++;
			}
			else
			{
				conditionNode.m_iRemoveByAttack--;
				if (conditionNode.m_iRemoveByAttack <= 0)
				{
					string msg = string.Format(Game.StringTable.GetString(260028), conditionNode.m_strName);
					UINGUI.BattleMessage(msg);
					this.unitConditionList.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x06002E7E RID: 11902 RVA: 0x00166654 File Offset: 0x00164854
	private void RemoveConditionOnHit()
	{
		int i = 0;
		while (i < this.unitConditionList.Count)
		{
			ConditionNode conditionNode = this.unitConditionList[i];
			if (conditionNode.m_iCondType == _ConditionType.StackBuff || conditionNode.m_iCondType == _ConditionType.StackDebuff || conditionNode.m_iRemoveOnHit <= 0)
			{
				i++;
			}
			else
			{
				conditionNode.m_iRemoveOnHit--;
				if (conditionNode.m_iRemoveOnHit <= 0)
				{
					string msg = string.Format(Game.StringTable.GetString(260028), conditionNode.m_strName);
					UINGUI.BattleMessage(msg);
					this.unitConditionList.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x06002E7F RID: 11903 RVA: 0x00166704 File Offset: 0x00164904
	public void RemoveConditionID(int ID, string name)
	{
		if (ID == 0)
		{
			return;
		}
		if (Game.g_BattleControl.m_battleAbility.GetConditionNode(ID) == null)
		{
			return;
		}
		int i = 0;
		while (i < this.unitConditionList.Count)
		{
			ConditionNode conditionNode = this.unitConditionList[i];
			if (conditionNode.m_iConditionID == ID)
			{
				this.unitConditionList.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06002E80 RID: 11904 RVA: 0x00166778 File Offset: 0x00164978
	private bool RemoveConditionID_Po(int ID, string name)
	{
		bool result = false;
		int i = 0;
		while (i < this.unitConditionList.Count)
		{
			ConditionNode conditionNode = this.unitConditionList[i];
			if (conditionNode.m_iConditionID == ID)
			{
				this.unitConditionList.RemoveAt(i);
				result = true;
			}
			else
			{
				i++;
			}
		}
		return result;
	}

	// Token: 0x06002E81 RID: 11905 RVA: 0x001667D0 File Offset: 0x001649D0
	private bool StackCondition(ConditionNode conditionNode, int iTotalTarget, int iDamageDone)
	{
		int num = conditionNode.m_iRemoveByAttack;
		int num2 = Mathf.Clamp(iTotalTarget, 1, 20);
		int num3 = iDamageDone / this.iAttackDivDamage;
		num = Mathf.Clamp(num / num2 - num3, 0, 100);
		if (Random.Range(0, 100) < num)
		{
			this.iAttackPlus++;
			this.bAttackPlus = true;
			if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible())
			{
				new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.AttackPlus, 0f);
			}
			this.CalculateCondition(conditionNode);
			return true;
		}
		return false;
	}

	// Token: 0x06002E82 RID: 11906 RVA: 0x00166880 File Offset: 0x00164A80
	private void CheckStackCondition(AttackInstance attInstance)
	{
		for (int i = 0; i < this.unitConditionList.Count; i++)
		{
			ConditionNode conditionNode = this.unitConditionList[i];
			if (conditionNode.m_iCondType == _ConditionType.StackBuff || conditionNode.m_iCondType == _ConditionType.StackDebuff)
			{
				if (this.m_AlreadyCheckPlusAttack)
				{
					i++;
					continue;
				}
				this.m_AlreadyCheckPlusAttack = true;
				if (this.StackCondition(this.unitConditionList[i], attInstance.totalTarget, attInstance.damageDone))
				{
					string text = "[FF0000FF]" + conditionNode.m_strName + "[-]";
					text = string.Format(Game.StringTable.GetString(260030), this.unitName, text);
					UINGUI.BattleMessage(text);
					this.unitConditionList.RemoveAt(i);
					continue;
				}
			}
		}
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x00166958 File Offset: 0x00164B58
	private void RemoveConditionEffectPartAbsoluteDebuff(_EffectPartType type, string conditionName)
	{
		bool flag = false;
		int i = 0;
		while (i < this.unitConditionList.Count)
		{
			if (this.unitConditionList[i].GetEffectPartAbsoluteDebuff(type))
			{
				string text = string.Format(Game.StringTable.GetString(260027), "[00FF00FF]" + conditionName + "[-]");
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					this.unitName,
					" [FF0000FF]",
					this.unitConditionList[i].m_strName,
					"[-]"
				});
				text = string.Format(Game.StringTable.GetString(260028), text);
				UINGUI.BattleMessage(text);
				this.unitConditionList.RemoveAt(i);
				flag = true;
			}
			else
			{
				i++;
			}
		}
		if (flag && type == _EffectPartType.Neigong)
		{
			this.UpdateEffectCompare(_EffectPartType.HitPoint, this.HP, this.GetFullHP());
			this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
		}
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x00166A64 File Offset: 0x00164C64
	public void ApplyCondition(ConditionNode conditionNode, bool bShowContinueLog)
	{
		if (conditionNode.GetEffectPartAbsoluteDebuff(_EffectPartType.Poison) && this.GetUnitTileAbsoluteBuff(_EffectPartType.Poison, null))
		{
			return;
		}
		if (conditionNode.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong) && this.GetUnitTileAbsoluteBuff(_EffectPartType.Neigong, null))
		{
			return;
		}
		if (conditionNode.GetEffectPartAbsoluteDebuff(_EffectPartType.Giddy) && this.GetUnitTileAbsoluteBuff(_EffectPartType.Giddy, null))
		{
			return;
		}
		if (conditionNode.GetEffectPartAbsoluteDebuff(_EffectPartType.Stun) && this.GetUnitTileAbsoluteBuff(_EffectPartType.Stun, null))
		{
			return;
		}
		if (conditionNode.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee))
		{
			if (this.GetUnitTileAbsoluteBuff(_EffectPartType.Flee, null))
			{
				return;
			}
			if (this.characterData.CheckTalentEffect(TalentEffect.PinkSkeleton))
			{
				string talentNameHaveEffect = this.characterData.GetTalentNameHaveEffect(TalentEffect.PinkSkeleton);
				string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentNameHaveEffect) + conditionNode.m_strName + Game.StringTable.GetString(260037);
				UINGUI.BattleMessage(msg);
				return;
			}
		}
		if (conditionNode.GetEffectPartAbsoluteBuff(_EffectPartType.Poison))
		{
			this.RemoveConditionEffectPartAbsoluteDebuff(_EffectPartType.Poison, conditionNode.m_strName);
		}
		if (conditionNode.GetEffectPartAbsoluteBuff(_EffectPartType.Neigong))
		{
			this.RemoveConditionEffectPartAbsoluteDebuff(_EffectPartType.Neigong, conditionNode.m_strName);
		}
		if (conditionNode.GetEffectPartAbsoluteBuff(_EffectPartType.Giddy))
		{
			this.RemoveConditionEffectPartAbsoluteDebuff(_EffectPartType.Giddy, conditionNode.m_strName);
		}
		if (conditionNode.GetEffectPartAbsoluteBuff(_EffectPartType.Stun))
		{
			this.RemoveConditionEffectPartAbsoluteDebuff(_EffectPartType.Stun, conditionNode.m_strName);
		}
		if (conditionNode.GetEffectPartAbsoluteBuff(_EffectPartType.Flee))
		{
			this.RemoveConditionEffectPartAbsoluteDebuff(_EffectPartType.Flee, conditionNode.m_strName);
		}
		bool flag = false;
		for (int i = 0; i < this.unitConditionList.Count; i++)
		{
			if (this.unitConditionList[i].m_iConditionID == conditionNode.m_iConditionID)
			{
				if (conditionNode.m_iCondType == _ConditionType.StackBuff || conditionNode.m_iCondType == _ConditionType.StackDebuff)
				{
					if (!this.m_AlreadyCheckPlusAttack)
					{
						this.m_AlreadyCheckPlusAttack = true;
						this.unitConditionList[i].m_iRemoveByAttack += this.unitConditionList[i].m_iRemoveOnHit;
						int iDamageDone = 0;
						if (conditionNode.m_effectPartList.Count > 0)
						{
							iDamageDone = conditionNode.m_effectPartList[0].m_iValueBase;
						}
						if (this.StackCondition(this.unitConditionList[i], 0, iDamageDone))
						{
							string text = "[FF0000FF]" + conditionNode.m_strName + "[-]";
							text = string.Format(Game.StringTable.GetString(260030), this.unitName, text);
							UINGUI.BattleMessage(text);
							this.unitConditionList.RemoveAt(i);
						}
						else
						{
							string text2 = "[FF0000FF]" + conditionNode.m_strName + "[-]";
							text2 = string.Format(Game.StringTable.GetString(260029), this.unitName, text2, this.unitConditionList[i].m_iRemoveByAttack);
							UINGUI.BattleMessage(text2);
						}
						return;
					}
				}
				else
				{
					flag = true;
					if (this.unitConditionList[i].m_iMaxTurn < conditionNode.m_iMaxTurn)
					{
						this.unitConditionList[i].m_iMaxTurn = conditionNode.m_iMaxTurn;
					}
					if (this.unitConditionList[i].m_iRemoveByAttack < conditionNode.m_iRemoveByAttack)
					{
						this.unitConditionList[i].m_iRemoveByAttack = conditionNode.m_iRemoveByAttack;
					}
					if (this.unitConditionList[i].m_iRemoveOnHit < conditionNode.m_iRemoveOnHit)
					{
						this.unitConditionList[i].m_iRemoveOnHit = conditionNode.m_iRemoveOnHit;
					}
					this.unitConditionList[i].m_iTargetUnitID = conditionNode.m_iTargetUnitID;
					for (int j = 0; j < this.unitConditionList[i].m_effectPartList.Count; j++)
					{
						if (this.unitConditionList[i].m_effectPartList[j].m_iValueBase < conditionNode.m_effectPartList[j].m_iValueBase)
						{
							this.unitConditionList[i].m_effectPartList[j].m_iValueBase = conditionNode.m_effectPartList[j].m_iValueBase;
						}
					}
					if (bShowContinueLog)
					{
						string text3 = "[FF0000FF]" + conditionNode.m_strName + "[-]";
						text3 = string.Format(Game.StringTable.GetString(260031), this.unitName, text3);
						UINGUI.BattleMessage(text3);
					}
				}
			}
		}
		if (!flag)
		{
			bool effectPartAbsoluteBuff = this.GetEffectPartAbsoluteBuff(_EffectPartType.HitPoint, null);
			bool effectPartAbsoluteDebuff = this.GetEffectPartAbsoluteDebuff(_EffectPartType.MovePreturn, null);
			bool effectPartAbsoluteDebuff2 = this.GetEffectPartAbsoluteDebuff(_EffectPartType.AttackPreturn, null);
			ConditionNode conditionNode2 = conditionNode.Clone();
			conditionNode2.InitEffectPart();
			conditionNode2.m_iCountTillNextTurn = UnitControl.activeFactionCount;
			if (conditionNode2.m_iCondType == _ConditionType.StackBuff || conditionNode2.m_iCondType == _ConditionType.StackDebuff)
			{
				if (this.m_AlreadyCheckPlusAttack)
				{
					return;
				}
				this.m_AlreadyCheckPlusAttack = true;
				int iDamageDone2 = 0;
				if (conditionNode.m_effectPartList.Count > 0)
				{
					iDamageDone2 = conditionNode.m_effectPartList[0].m_iValueBase;
				}
				if (this.StackCondition(conditionNode2, 0, iDamageDone2))
				{
					string text4 = "[FF0000FF]" + conditionNode.m_strName + "[-]";
					text4 = string.Format(Game.StringTable.GetString(260030), this.unitName, text4);
					UINGUI.BattleMessage(text4);
					return;
				}
				string text5 = "[FF0000FF]" + conditionNode.m_strName + "[-]";
				text5 = string.Format(Game.StringTable.GetString(260029), this.unitName, text5, conditionNode2.m_iRemoveByAttack);
				UINGUI.BattleMessage(text5);
				this.unitConditionList.Add(conditionNode2);
				this.UpdateAuras();
				if (UnitTB.onEffectAppliedE != null)
				{
					UnitTB.onEffectAppliedE(this);
				}
				return;
			}
			else
			{
				if (conditionNode.m_iCondType == _ConditionType.InstantBuff || conditionNode.m_iCondType == _ConditionType.InstantDebuff)
				{
					this.CalculateCondition(conditionNode2);
					this.ApplyConditionCheckCleanup(conditionNode2);
					return;
				}
				string text6 = "[FF0000FF]" + conditionNode.m_strName + "[-]";
				text6 = string.Format(Game.StringTable.GetString(260001), this.unitName, text6);
				UINGUI.BattleMessage(text6);
				conditionNode2.m_iCountTillNextTurn = UnitControl.activeFactionCount;
				this.IfUnitGetFleeSetStateAlreadyMove(conditionNode2);
				this.unitConditionList.Add(conditionNode2);
				this.CalculateCondition(conditionNode2);
				this.UpdateAuras();
				this.ApplyConditionCheckCleanup(conditionNode2);
				if (!effectPartAbsoluteBuff && this.GetEffectPartAbsoluteBuff(_EffectPartType.HitPoint, null) && this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible())
				{
					new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.CantHurt, 0f);
				}
				if ((!effectPartAbsoluteDebuff2 || !effectPartAbsoluteDebuff) && this.GetEffectPartAbsoluteDebuff(_EffectPartType.MovePreturn, null) && this.GetEffectPartAbsoluteDebuff(_EffectPartType.AttackPreturn, null) && this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible())
				{
					new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.Stun, 0f);
				}
				if (conditionNode2.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong))
				{
					this.UpdateEffectCompare(_EffectPartType.HitPoint, this.HP, this.GetFullHP());
					this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
				}
				this.CheckEffectObject();
				if (UnitTB.onEffectAppliedE != null)
				{
					UnitTB.onEffectAppliedE(this);
				}
			}
		}
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x001671F0 File Offset: 0x001653F0
	private void IfUnitGetFleeSetStateAlreadyMove(ConditionNode condition)
	{
		if (condition.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee) && !this.attacked && GameControlTB.GetCurrentPlayerFactionID() == this.factionID)
		{
			this.moved = true;
			this.attacked = true;
			this.attackRemain = 0;
			this.moveRemain = 0;
			UnitControl.MoveUnit(this);
		}
	}

	// Token: 0x06002E86 RID: 11910 RVA: 0x00167248 File Offset: 0x00165448
	private void ApplyConditionCheckCleanup(ConditionNode condition)
	{
		for (int i = 0; i < condition.m_effectPartList.Count; i++)
		{
			if (condition.m_effectPartList[i].m_effectPartType == _EffectPartType.Cleanup)
			{
				if (condition.m_effectPartList[i].m_effectAccumulateType == _EffectAccumulateType.Absolute)
				{
					if (condition.m_iCondType == _ConditionType.Buff || condition.m_iCondType == _ConditionType.InstantBuff || condition.m_iCondType == _ConditionType.StackBuff)
					{
						base.StartCoroutine(this.DelayCleanDebuff(99, condition.m_strName));
						break;
					}
					base.StartCoroutine(this.DelayCleanBuff(99, condition.m_strName));
					break;
				}
				else if (condition.m_effectPartList[i].m_effectAccumulateType == _EffectAccumulateType.None)
				{
					if (condition.m_iCondType == _ConditionType.Buff || condition.m_iCondType == _ConditionType.InstantBuff || condition.m_iCondType == _ConditionType.StackBuff)
					{
						base.StartCoroutine(this.DelayCleanDebuff(condition.m_effectPartList[i].m_iValueSum, condition.m_strName));
						break;
					}
					base.StartCoroutine(this.DelayCleanBuff(condition.m_effectPartList[i].m_iValueSum, condition.m_strName));
					break;
				}
			}
		}
	}

	// Token: 0x06002E87 RID: 11911 RVA: 0x00167388 File Offset: 0x00165588
	private IEnumerator DelayCleanBuff(int iCount, string Name)
	{
		yield return null;
		string S = string.Format(Game.StringTable.GetString(260027), Name);
		this.ClearnBuffCount(iCount, S);
		yield break;
	}

	// Token: 0x06002E88 RID: 11912 RVA: 0x001673C0 File Offset: 0x001655C0
	private IEnumerator DelayCleanDebuff(int iCount, string Name)
	{
		yield return null;
		string S = string.Format(Game.StringTable.GetString(260027), Name);
		this.ClearnDebuffCount(iCount, S);
		yield break;
	}

	// Token: 0x06002E89 RID: 11913 RVA: 0x0001DB5E File Offset: 0x0001BD5E
	public void ApplyCollectibleEffect(Effect effect)
	{
		effect.countTillNextTurn = UnitControl.activeFactionCount;
		this.activeCollectibleAbilityEffectList.Add(effect);
		if (UnitTB.onEffectAppliedE != null)
		{
			UnitTB.onEffectAppliedE(this);
		}
	}

	// Token: 0x06002E8A RID: 11914 RVA: 0x001673F8 File Offset: 0x001655F8
	public void ReduceUnitAbilityCountTillNextDown()
	{
		for (int i = 0; i < this.unitAbilityList.Count; i++)
		{
			UnitAbility unitAbility = this.unitAbilityList[i];
			if (unitAbility.cooldown > 0)
			{
				unitAbility.countTillNextTurn--;
				if (unitAbility.countTillNextTurn <= 0)
				{
					unitAbility.cooldown--;
				}
			}
		}
		for (int j = 0; j < this.activeUnitAbilityEffectList.Count; j++)
		{
			this.activeUnitAbilityEffectList[j].countTillNextTurn--;
		}
		for (int k = 0; k < this.activeCollectibleAbilityEffectList.Count; k++)
		{
			this.activeCollectibleAbilityEffectList[k].countTillNextTurn--;
		}
	}

	// Token: 0x06002E8B RID: 11915 RVA: 0x001674CC File Offset: 0x001656CC
	private void ClearnBuffCount(int iCount, string S1)
	{
		bool flag = false;
		int num = 0;
		while (num < this.unitConditionList.Count && iCount > 0)
		{
			ConditionNode conditionNode = this.unitConditionList[num];
			if (conditionNode.m_iCondType == _ConditionType.Buff || conditionNode.m_iCondType == _ConditionType.Buff)
			{
				string text = S1 + conditionNode.m_strName;
				text = string.Format(Game.StringTable.GetString(260028), text);
				UINGUI.BattleMessage(text);
				this.unitConditionList.RemoveAt(num);
				flag = true;
				num--;
				iCount--;
				this.CheckEffectObject();
				if (UnitTB.onEffectExpiredE != null)
				{
					UnitTB.onEffectExpiredE(this);
				}
			}
			num++;
		}
		this.UpdateEffectCompare(_EffectPartType.HitPoint, this.HP, this.GetFullHP());
		this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
		if (flag)
		{
			this.UpdateAuras();
		}
	}

	// Token: 0x06002E8C RID: 11916 RVA: 0x001675B4 File Offset: 0x001657B4
	private void ClearnDebuffCount(int iCount, string S1)
	{
		bool flag = false;
		int num = 0;
		while (num < this.unitConditionList.Count && iCount > 0)
		{
			ConditionNode conditionNode = this.unitConditionList[num];
			if (conditionNode.m_iCondType == _ConditionType.Debuff || conditionNode.m_iCondType == _ConditionType.StackDebuff)
			{
				if (conditionNode.m_iConditionID == 500000)
				{
					Debug.Log("內傷 gong");
				}
				string text = S1 + conditionNode.m_strName;
				text = string.Format(Game.StringTable.GetString(260028), text);
				UINGUI.BattleMessage(text);
				this.unitConditionList.RemoveAt(num);
				flag = true;
				num--;
				iCount--;
				this.CheckEffectObject();
				if (UnitTB.onEffectExpiredE != null)
				{
					UnitTB.onEffectExpiredE(this);
				}
			}
			num++;
		}
		this.UpdateEffectCompare(_EffectPartType.HitPoint, this.HP, this.GetFullHP());
		this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
		if (flag)
		{
			this.UpdateAuras();
		}
	}

	// Token: 0x06002E8D RID: 11917 RVA: 0x001676B8 File Offset: 0x001658B8
	private void ClearnByNeigong()
	{
		if (this.GetUnitTileAbsoluteDebuff(_EffectPartType.Cleanup, null))
		{
			List<string> effectString = this.GetEffectString(_EffectPartType.Cleanup);
			string s = string.Empty;
			if (effectString != null && effectString.Count > 0)
			{
				s = string.Format(Game.StringTable.GetString(260027), effectString[0]);
			}
			this.ClearnBuffCount(99, s);
		}
		if (this.GetUnitTileAbsoluteBuff(_EffectPartType.Cleanup, null))
		{
			List<string> effectString2 = this.GetEffectString(_EffectPartType.Cleanup);
			string s2 = string.Empty;
			if (effectString2 != null && effectString2.Count > 0)
			{
				s2 = string.Format(Game.StringTable.GetString(260027), effectString2[0]);
			}
			this.ClearnDebuffCount(99, s2);
		}
		else if (this.GetEffectPartValue(_EffectPartType.Cleanup, null, 0) + this.occupiedTile.GetEffectPartValue(_EffectPartType.Cleanup, this) >= 1f)
		{
			int iCount = Mathf.RoundToInt(this.GetEffectPartValue(_EffectPartType.Cleanup, null, 0) + this.occupiedTile.GetEffectPartValue(_EffectPartType.Cleanup, this));
			List<string> effectString3 = this.GetEffectString(_EffectPartType.Cleanup);
			string text = string.Empty;
			foreach (string text2 in effectString3)
			{
				text = text + text2 + " ";
			}
			text = string.Format(Game.StringTable.GetString(260027), text);
			this.ClearnDebuffCount(iCount, text);
		}
	}

	// Token: 0x06002E8E RID: 11918 RVA: 0x00167840 File Offset: 0x00165A40
	public void OnNextTurn()
	{
		if (this.occupiedTile == null)
		{
			return;
		}
		this.ClearnByNeigong();
		for (int i = 0; i < this.unitAbilityList.Count; i++)
		{
			UnitAbility unitAbility = this.unitAbilityList[i];
			if (unitAbility.cooldown > 0)
			{
				unitAbility.countTillNextTurn--;
				if (unitAbility.countTillNextTurn == 0)
				{
					unitAbility.cooldown--;
					Debug.Log(string.Concat(new string[]
					{
						this.unitName,
						" ",
						unitAbility.name,
						" CD -1 Now CD = ",
						unitAbility.cooldown.ToString()
					}));
					unitAbility.countTillNextTurn = UnitControl.activeFactionCount;
				}
			}
		}
		for (int j = 0; j < this.unitConditionList.Count; j++)
		{
			ConditionNode conditionNode = this.unitConditionList[j];
			if (conditionNode.m_iCondType != _ConditionType.StackBuff && conditionNode.m_iCondType != _ConditionType.StackDebuff)
			{
				conditionNode.m_iCountTillNextTurn--;
				if (conditionNode.m_iCountTillNextTurn <= 0)
				{
					conditionNode.m_iCountTillNextTurn = UnitControl.activeFactionCount;
					bool flag = this.CalculateUnitCondition(conditionNode);
					if (flag)
					{
						string text = this.unitName + " " + conditionNode.m_strName;
						text = string.Format(Game.StringTable.GetString(260028), text);
						UINGUI.BattleMessage(text);
						this.unitConditionList.Remove(conditionNode);
						j--;
						this.CheckEffectObject();
						if (UnitTB.onEffectExpiredE != null)
						{
							UnitTB.onEffectExpiredE(this);
						}
					}
				}
			}
		}
		if (this.iConfuseTurn > 0)
		{
			this.iCountTillNextTurn--;
			if (this.iCountTillNextTurn <= 0)
			{
				this.iConfuseTurn--;
				if (this.iConfuseTurn <= 0)
				{
					UnitControl.ChangeUnitFaction(this, this.iOrigFaction);
				}
				else
				{
					this.iCountTillNextTurn = UnitControl.activeFactionCount;
				}
			}
		}
		this.UpdateAuras();
		this.UpdateEffectCompare(_EffectPartType.HitPoint, this.HP, this.GetFullHP());
		this.UpdateEffectCompare(_EffectPartType.InternalForce, this.SP, this.GetFullSP());
		this.moveRemain = this.GetMovePerTurn();
		this.attackRemain = this.GetAttackPerTurn();
		this.counterAttackRemain = this.GetCounterPerTurn();
	}

	// Token: 0x06002E8F RID: 11919 RVA: 0x00167A9C File Offset: 0x00165C9C
	private void RemoveDebuff(AttackInstance aInst)
	{
		int cleanDebuffCount = aInst.cleanDebuffCount;
		string s = string.Format(Game.StringTable.GetString(260026), aInst.srcUnit.unitName, aInst.unitAbility.name, aInst.targetUnit.unitName);
		this.ClearnDebuffCount(cleanDebuffCount, s);
	}

	// Token: 0x06002E90 RID: 11920 RVA: 0x00167AF0 File Offset: 0x00165CF0
	public void InitNeigong()
	{
		if (this.unitNeigong == null)
		{
			return;
		}
		if (this.unitNeigong != null)
		{
			if (this.factionID != 0)
			{
				List<int> list = Enumerable.ToList<int>(Enumerable.Select<NeigongNewDataNode, int>(Game.NeigongData.GetNeigongList(), (NeigongNewDataNode x) => x.m_iNeigongID));
				for (int i = 0; i < GameGlobal.mod_Difficulty; i++)
				{
					int num = Random.Range(0, list.Count);
					NPC.m_instance.AddNeigong(this.characterData.iNpcID, list[num], 10);
					list.RemoveAt(num);
				}
				foreach (NpcNeigong npcNeigong in this.characterData.NeigongList)
				{
					npcNeigong.bUse = true;
				}
			}
			List<NpcNeigong> list2 = this.characterData.mod_GetNowUsedNeigong().FindAll((NpcNeigong x) => x.m_Neigong != this.unitNeigong);
			for (int j = 0; j < list2.Count; j++)
			{
				if (j >= GameGlobal.mod_Difficulty - 1)
				{
					this.characterData.mod_SetNeigongUnuse(list2[j].iSkillID);
				}
			}
			list2 = this.characterData.mod_GetNowUsedNeigong().FindAll((NpcNeigong x) => x.m_Neigong != this.unitNeigong);
			foreach (NpcNeigong npcNeigong2 in list2)
			{
				if (npcNeigong2 != null)
				{
					NeigongNewDataNode neigong = npcNeigong2.m_Neigong;
					if (neigong != null)
					{
						for (int k = 0; k < neigong.m_ConditionEffectList.Count; k++)
						{
							List<Condition> conditionList = neigong.m_ConditionEffectList[k].m_ConditionList;
							if (conditionList.Count > 0 && npcNeigong2.iLevel >= conditionList[0].m_iValue)
							{
								this.unitNeigong.m_iConditionList.Add(neigong.m_ConditionEffectList[k].m_iBattleConditionID);
							}
						}
					}
				}
			}
		}
		this.unitNeigongConditionList.Clear();
		for (int l = 0; l < this.unitNeigong.m_iConditionList.Count; l++)
		{
			ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(this.unitNeigong.m_iConditionList[l]);
			if (conditionNode != null)
			{
				ConditionNode conditionNode2 = conditionNode.Clone();
				conditionNode2.InitEffectPart();
				this.unitNeigongConditionList.Add(conditionNode2);
			}
		}
		ConditionNode conditionNode3 = Game.g_BattleControl.m_battleAbility.GetConditionNode(400008);
		if (conditionNode3 != null)
		{
			ConditionNode conditionNode4 = conditionNode3.Clone();
			conditionNode4.InitEffectPart();
			this.unitNeigongConditionList.Add(conditionNode4);
		}
		this.UpdateAuras();
	}

	// Token: 0x06002E91 RID: 11921 RVA: 0x00167DC0 File Offset: 0x00165FC0
	public void CalculateEffectPart(EffectPart part, int iRange)
	{
		if (iRange <= 0)
		{
			switch (part.m_effectAccumulateType)
			{
			case _EffectAccumulateType.None:
				part.m_iValueSum = Random.Range(part.m_iValue1, part.m_iValue2);
				break;
			case _EffectAccumulateType.Plus:
			{
				int num = Random.Range(part.m_iValue1, part.m_iValue2);
				part.m_iValueSum += num;
				if (part.m_iValueLimit > 0)
				{
					if (part.m_iValueSum > part.m_iValueLimit)
					{
						part.m_iValueSum = part.m_iValueLimit;
					}
				}
				else if (part.m_iValueLimit < 0 && part.m_iValueSum < part.m_iValueLimit)
				{
					part.m_iValueSum = part.m_iValueLimit;
				}
				break;
			}
			case _EffectAccumulateType.Minus:
			{
				int num = Random.Range(part.m_iValue1, part.m_iValue2);
				part.m_iValueSum -= num;
				if (part.m_iValueSum < 0)
				{
					part.m_iValueSum = 0;
				}
				break;
			}
			case _EffectAccumulateType.Multiply:
				part.m_iValueSum *= part.m_iValue2;
				if (part.m_iValueSum > part.m_iValueLimit)
				{
					part.m_iValueSum = part.m_iValueLimit;
				}
				break;
			}
		}
		else
		{
			switch (part.m_effectAccumulateType)
			{
			case _EffectAccumulateType.Auras:
			case _EffectAccumulateType.AurasAll:
			case _EffectAccumulateType.AurasEnemy:
				if (part.m_iValueLimit > 0)
				{
					if (part.m_iValueLimit == 1)
					{
						part.m_iValueSum = part.m_iValue1;
					}
					else
					{
						int num = iRange - 1;
						int num2 = part.m_iValueLimit - 1;
						float num3 = (float)(part.m_iValue2 - part.m_iValue1);
						num3 = num3 * (float)num / (float)num2;
						part.m_iValueSum = part.m_iValue1 + Mathf.RoundToInt(num3);
					}
				}
				else
				{
					Debug.LogWarning("靈氣類的 內功 或 狀態 EffectPart 距離為 0");
					part.m_iValueSum = 0;
				}
				break;
			}
		}
	}

	// Token: 0x06002E92 RID: 11922 RVA: 0x00167FA4 File Offset: 0x001661A4
	public void DirectEffectPart(string name, EffectPart part, _ConditionType ctType, int iLv)
	{
		_EffectPartType effectPartType = part.m_effectPartType;
		if (effectPartType != _EffectPartType.HitPoint)
		{
			if (effectPartType != _EffectPartType.InternalForce)
			{
				if (effectPartType != _EffectPartType.MovePreturn)
				{
					if (effectPartType == _EffectPartType.AttackPreturn)
					{
						if (ctType == _ConditionType.Buff || ctType == _ConditionType.InstantBuff || ctType == _ConditionType.StackBuff)
						{
							this.attackRemain += part.m_iValueSum;
							if (this.attacked && this.attackRemain >= 0)
							{
								this.attacked = false;
								UnitControl.OneMoreMoveUnit(this);
							}
						}
						else if (ctType == _ConditionType.Debuff || ctType == _ConditionType.InstantDebuff || ctType == _ConditionType.StackDebuff)
						{
							this.attackRemain -= part.m_iValueSum;
							if (!this.attacked && this.attackRemain <= 0)
							{
								this.attacked = true;
							}
						}
					}
				}
				else if (ctType == _ConditionType.Buff || ctType == _ConditionType.InstantBuff || ctType == _ConditionType.StackBuff)
				{
					this.moveRemain += part.m_iValueSum;
					if (this.moved && this.moveRemain >= 0)
					{
						this.moved = false;
						UnitControl.OneMoreMoveUnit(this);
					}
				}
				else if (ctType == _ConditionType.Debuff || ctType == _ConditionType.InstantDebuff || ctType == _ConditionType.StackDebuff)
				{
					this.moveRemain -= part.m_iValueSum;
					if (!this.moved && this.moveRemain <= 0)
					{
						this.moved = true;
					}
				}
			}
			else
			{
				int num2;
				if (part.m_bPercent)
				{
					float num;
					if (part.m_iValueBase == 0 || ctType == _ConditionType.Buff || ctType == _ConditionType.InstantBuff)
					{
						num = (float)(this.fullSP * part.m_iValueSum) * 0.01f;
					}
					else
					{
						num = (float)(part.m_iValueBase * part.m_iValueSum) * 0.01f;
					}
					num = num * (float)iLv * 0.1f;
					num2 = Mathf.RoundToInt(num);
				}
				else
				{
					float num = (float)(part.m_iValueSum * iLv) * 0.1f;
					num2 = Mathf.RoundToInt(num);
				}
				if (ctType == _ConditionType.Buff || ctType == _ConditionType.InstantBuff)
				{
					string msg = string.Format(Game.StringTable.GetString(260022), this.unitName, name, num2);
					UINGUI.BattleMessage(msg);
					this.ApplyInternalForce(num2);
				}
				else if (ctType == _ConditionType.Debuff || ctType == _ConditionType.InstantDebuff)
				{
					string msg2 = string.Format(Game.StringTable.GetString(260023), this.unitName, name, num2);
					UINGUI.BattleMessage(msg2);
					this.ApplyLessInternalForce(num2);
				}
			}
		}
		else
		{
			int num2;
			if (part.m_bPercent)
			{
				float num;
				if (part.m_iValueBase == 0 || ctType == _ConditionType.Buff || ctType == _ConditionType.InstantBuff)
				{
					num = (float)(this.fullHP * part.m_iValueSum) * 0.01f;
				}
				else
				{
					num = (float)(part.m_iValueBase * part.m_iValueSum) * 0.01f;
				}
				num = num * (float)iLv * 0.1f;
				num2 = Mathf.RoundToInt(num);
			}
			else
			{
				float num = (float)(part.m_iValueSum * iLv) * 0.1f;
				num2 = Mathf.RoundToInt(num);
			}
			if (ctType == _ConditionType.Buff || ctType == _ConditionType.InstantBuff)
			{
				string msg3 = string.Format(Game.StringTable.GetString(260024), this.unitName, name, num2);
				UINGUI.BattleMessage(msg3);
				this.ApplyHeal(num2);
			}
			else if (ctType == _ConditionType.Debuff || ctType == _ConditionType.InstantDebuff)
			{
				string msg4 = string.Format(Game.StringTable.GetString(260025), this.unitName, name, num2);
				UINGUI.BattleMessage(msg4);
				this.ApplyDamage(num2, null);
			}
		}
	}

	// Token: 0x06002E93 RID: 11923 RVA: 0x00168330 File Offset: 0x00166530
	private void CalculateAura()
	{
		List<string> effectString = this.occupiedTile.GetEffectString(_EffectPartType.HitPoint);
		if (effectString.Count > 0)
		{
			float effectPartValue = this.occupiedTile.GetEffectPartValue(_EffectPartType.HitPoint, this);
			float effectPartValuePercent = this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.HitPoint, this);
			float num = effectPartValue + effectPartValuePercent * (float)this.fullHP * 0.01f;
			string text = string.Empty;
			for (int i = 0; i < effectString.Count; i++)
			{
				text = text + effectString[i] + " ";
			}
			int num2 = Mathf.RoundToInt(num);
			if (num2 > 0)
			{
				string msg = string.Format(Game.StringTable.GetString(260024), this.unitName, text, num2);
				UINGUI.BattleMessage(msg);
				this.ApplyHeal(num2);
			}
			else if (num2 < 0)
			{
				num2 = Mathf.Abs(num2);
				string msg2 = string.Format(Game.StringTable.GetString(260025), this.unitName, text, num2);
				UINGUI.BattleMessage(msg2);
				this.ApplyDamage(num2, null);
			}
		}
		effectString = this.occupiedTile.GetEffectString(_EffectPartType.InternalForce);
		if (effectString.Count > 0)
		{
			float effectPartValue2 = this.occupiedTile.GetEffectPartValue(_EffectPartType.InternalForce, this);
			float effectPartValuePercent2 = this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.InternalForce, this);
			float num3 = effectPartValue2 + effectPartValuePercent2 * (float)this.fullSP * 0.01f;
			string text2 = string.Empty;
			for (int j = 0; j < effectString.Count; j++)
			{
				text2 = text2 + effectString[j] + " ";
			}
			int num4 = Mathf.RoundToInt(num3);
			if (num4 > 0)
			{
				string msg3 = string.Format(Game.StringTable.GetString(260022), this.unitName, text2, num4);
				UINGUI.BattleMessage(msg3);
				this.GainSP(num4);
			}
			else if (num4 < 0)
			{
				this.GainSP(num4);
				num4 = Mathf.Abs(num4);
				string msg4 = string.Format(Game.StringTable.GetString(260023), this.unitName, text2, num4);
				UINGUI.BattleMessage(msg4);
			}
		}
	}

	// Token: 0x06002E94 RID: 11924 RVA: 0x00168570 File Offset: 0x00166770
	private void CalculateNeigong()
	{
		if (this.unitNeigong == null)
		{
			return;
		}
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, null))
		{
			return;
		}
		for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
		{
			ConditionNode conditionNode = this.unitNeigongConditionList[i];
			for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
			{
				EffectPart effectPart = conditionNode.m_effectPartList[j];
				if (effectPart.m_effectAccumulateType != _EffectAccumulateType.Absolute && effectPart.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasAll && effectPart.m_effectAccumulateType != _EffectAccumulateType.AfterAction && effectPart.m_effectAccumulateType != _EffectAccumulateType.Compare && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasSelf && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy)
				{
					this.CalculateEffectPart(effectPart, 0);
					this.DirectEffectPart(this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName, effectPart, _ConditionType.Buff, this.iNeigongLv);
				}
			}
		}
	}

	// Token: 0x06002E95 RID: 11925 RVA: 0x0001DB8C File Offset: 0x0001BD8C
	private bool CalculateUnitCondition(ConditionNode condition)
	{
		condition.m_iMaxTurn--;
		if (condition.m_iMaxTurn <= 0)
		{
			return true;
		}
		this.CalculateCondition(condition);
		return false;
	}

	// Token: 0x06002E96 RID: 11926 RVA: 0x00168678 File Offset: 0x00166878
	private void CalculateCondition(ConditionNode condition)
	{
		for (int i = 0; i < condition.m_effectPartList.Count; i++)
		{
			EffectPart part = condition.m_effectPartList[i];
			this.CalculateEffectPart(part, 0);
			this.DirectEffectPart(condition.m_strName, part, condition.m_iCondType, 10);
		}
	}

	// Token: 0x06002E97 RID: 11927 RVA: 0x0001DBB2 File Offset: 0x0001BDB2
	private void OnCheckFogOfWar(UnitTB unit)
	{
		if (this.CheckFriendFaction(unit.factionID))
		{
			return;
		}
		this.ReUpdateThreatList();
		if (GameControlTB.EnableFogOfWar() && unit.factionID == GameControlTB.GetPlayerFactionID())
		{
			this.AIUnitCheckFogOfWar();
		}
	}

	// Token: 0x06002E98 RID: 11928 RVA: 0x001686CC File Offset: 0x001668CC
	private void AIUnitCheckFogOfWar()
	{
		if (this.factionID == GameControlTB.GetPlayerFactionID())
		{
			return;
		}
		List<UnitTB> allUnitsOfFaction = UnitControl.GetAllUnitsOfFaction(GameControlTB.GetPlayerFactionID());
		bool flag = false;
		foreach (UnitTB unitTB in allUnitsOfFaction)
		{
			if (unitTB.HP > 0)
			{
				int num = GridManager.Distance(unitTB.occupiedTile, this.occupiedTile);
				flag = (GridManager.IsInLOS(unitTB.thisT.position, this.occupiedTile.pos) & num <= unitTB.sight);
				if (flag)
				{
					break;
				}
			}
		}
		if (flag)
		{
			this.SetToVisible();
		}
		else
		{
			this.SetToInvisible();
		}
	}

	// Token: 0x06002E99 RID: 11929 RVA: 0x001687A4 File Offset: 0x001669A4
	public void SetToVisible()
	{
		if (this.factionID == GameControlTB.GetPlayerFactionID())
		{
			if (this.thisObj.layer != LayerManager.GetLayerUnit())
			{
				this.thisObj.layer = LayerManager.GetLayerUnit();
				Utility.SetLayerRecursively(this.thisT, LayerManager.GetLayerUnit());
			}
		}
		else if (this.thisObj.layer != LayerManager.GetLayerUnitAI())
		{
			this.thisObj.layer = LayerManager.GetLayerUnitAI();
			Utility.SetLayerRecursively(this.thisT, LayerManager.GetLayerUnitAI());
		}
		if (AIManager.GetAIStance() == _AIStance.Trigger)
		{
			this.triggered = true;
		}
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x0001DBEC File Offset: 0x0001BDEC
	public void SetToInvisible()
	{
		if (this.thisObj.layer != LayerManager.GetLayerUnitAIInvisible())
		{
			this.thisObj.layer = LayerManager.GetLayerUnitAIInvisible();
			Utility.SetLayerRecursively(this.thisT, LayerManager.GetLayerUnitAIInvisible());
		}
	}

	// Token: 0x06002E9B RID: 11931 RVA: 0x0001DC23 File Offset: 0x0001BE23
	public bool IsVisibleToPlayer()
	{
		return this.factionID == GameControlTB.GetPlayerFactionID() || this.thisObj.layer == LayerManager.GetLayerUnitAI();
	}

	// Token: 0x06002E9C RID: 11932 RVA: 0x0001DC4F File Offset: 0x0001BE4F
	public float GetUnitHitRate()
	{
		return this.fHitRate;
	}

	// Token: 0x06002E9D RID: 11933 RVA: 0x0001DC57 File Offset: 0x0001BE57
	public float GetTileHitRate(Tile origTile)
	{
		if (origTile == null)
		{
			return 0f;
		}
		return origTile.GetEffectPartValuePercent(_EffectPartType.HitChance, this) * 0.01f;
	}

	// Token: 0x06002E9E RID: 11934 RVA: 0x00168844 File Offset: 0x00166A44
	public float GetAbilityHitRate(UnitAbility uab, int range, Tile origTile)
	{
		if (this.GetEffectPartAbsoluteBuff(_EffectPartType.HitChance, uab))
		{
			return 1f;
		}
		if (origTile != null && origTile.GetEffectPartAbsoluteBuff(_EffectPartType.HitChance, this))
		{
			return 1f;
		}
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.HitChance, uab))
		{
			return 0f;
		}
		if (origTile != null && origTile.GetEffectPartAbsoluteDebuff(_EffectPartType.HitChance, this))
		{
			return 0f;
		}
		return this.GetUnitHitRate() + this.GetTileHitRate(origTile) + this.GetEffectPartValuePercent(_EffectPartType.HitChance, uab, range) * 0.01f;
	}

	// Token: 0x06002E9F RID: 11935 RVA: 0x001688D4 File Offset: 0x00166AD4
	public float GetAbilityToTargetHitRate(UnitTB target, UnitAbility uab, Tile origTile)
	{
		if (target == null)
		{
			return 0f;
		}
		if (this.GetUnitTileAbsoluteBuff(_EffectPartType.HitChance, uab))
		{
			return 1f;
		}
		if (origTile != null && origTile.GetEffectPartAbsoluteBuff(_EffectPartType.HitChance, this))
		{
			return 1f;
		}
		if (target.GetEffectPartAbsoluteBuff(_EffectPartType.DodgeChance, uab))
		{
			return 0f;
		}
		if (target.occupiedTile != null && target.occupiedTile.GetEffectPartAbsoluteBuff(_EffectPartType.DodgeChance, this))
		{
			return 0f;
		}
		if (target.GetEffectPartAbsoluteDebuff(_EffectPartType.DodgeChance, uab))
		{
			return 1f;
		}
		if (target.occupiedTile != null && target.occupiedTile.GetEffectPartAbsoluteDebuff(_EffectPartType.DodgeChance, this))
		{
			return 1f;
		}
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.HitChance, uab))
		{
			return 0f;
		}
		if (origTile != null && origTile.GetEffectPartAbsoluteDebuff(_EffectPartType.HitChance, this))
		{
			return 0f;
		}
		Quaternion quaternion = Quaternion.LookRotation(target.thisT.position - origTile.pos);
		float num = Quaternion.Angle(quaternion, target.thisT.rotation);
		int range = GridManager.Distance(origTile, target.occupiedTile);
		float num2 = this.GetAbilityHitRate(uab, range, origTile);
		if (num < 0f)
		{
			Debug.Log("angle = " + num.ToString());
		}
		if (num < 31f)
		{
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.HitChance, 400003, 0) * 0.01f;
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.HitChance, 400004, 0) * 0.01f;
		}
		else if (num > 149f)
		{
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.HitChance, 400001, 0) * 0.01f;
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.HitChance, 400002, 0) * 0.01f;
		}
		else if (num < 91f)
		{
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.HitChance, 400005, 0) * 0.01f;
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.HitChance, 400006, 0) * 0.01f;
		}
		return Mathf.Clamp(num2 - target.GetDodgeRate(uab, range), 0f, 1f);
	}

	// Token: 0x06002EA0 RID: 11936 RVA: 0x0001DC79 File Offset: 0x0001BE79
	public float GetUnitDodgeRate()
	{
		return this.fDodgeRate;
	}

	// Token: 0x06002EA1 RID: 11937 RVA: 0x0001DC81 File Offset: 0x0001BE81
	public float GetTileDodgeRate()
	{
		if (this.occupiedTile == null)
		{
			return 0f;
		}
		return this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.DodgeChance, this) * 0.01f;
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x00168B00 File Offset: 0x00166D00
	public float GetDodgeRate(UnitAbility uab, int range)
	{
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.DodgeChance, uab))
		{
			return 0f;
		}
		if (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteDebuff(_EffectPartType.DodgeChance, this))
		{
			return 0f;
		}
		if (this.GetEffectPartAbsoluteBuff(_EffectPartType.DodgeChance, uab))
		{
			return 1f;
		}
		if (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteBuff(_EffectPartType.DodgeChance, this))
		{
			return 1f;
		}
		return this.GetUnitDodgeRate() + this.GetTileDodgeRate() + this.GetEffectPartValuePercent(_EffectPartType.DodgeChance, uab, range) * 0.01f;
	}

	// Token: 0x06002EA3 RID: 11939 RVA: 0x0001DCAD File Offset: 0x0001BEAD
	public float GetUnitCriticalRate()
	{
		return this.fCriticalRate;
	}

	// Token: 0x06002EA4 RID: 11940 RVA: 0x0001DCB5 File Offset: 0x0001BEB5
	public float GetTileCriticalRate(Tile origTile)
	{
		if (origTile == null)
		{
			return 0f;
		}
		return origTile.GetEffectPartValuePercent(_EffectPartType.CriticalChance, this) * 0.01f;
	}

	// Token: 0x06002EA5 RID: 11941 RVA: 0x00168BA4 File Offset: 0x00166DA4
	public float GetAbilityCriticalRate(UnitAbility uab, int range, Tile origTile)
	{
		if (this.GetEffectPartAbsoluteBuff(_EffectPartType.CriticalChance, uab))
		{
			return 1f;
		}
		if (origTile != null && origTile.GetEffectPartAbsoluteBuff(_EffectPartType.CriticalChance, this))
		{
			return 1f;
		}
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.CriticalChance, uab))
		{
			return 0f;
		}
		if (origTile != null && origTile.GetEffectPartAbsoluteDebuff(_EffectPartType.CriticalChance, this))
		{
			return 0f;
		}
		return this.GetUnitCriticalRate() + this.GetTileCriticalRate(origTile) + this.GetEffectPartValuePercent(_EffectPartType.CriticalChance, uab, range) * 0.01f;
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x00168C38 File Offset: 0x00166E38
	public float GetAbilityToTargetCriticalRate(UnitTB target, UnitAbility uab, Tile origTile)
	{
		if (target == null)
		{
			return 0f;
		}
		if (this.GetEffectPartAbsoluteBuff(_EffectPartType.CriticalChance, uab))
		{
			return 1f;
		}
		if (origTile != null && origTile.GetEffectPartAbsoluteBuff(_EffectPartType.CriticalChance, this))
		{
			return 1f;
		}
		if (target.GetEffectPartAbsoluteBuff(_EffectPartType.DefCriticalChance, uab))
		{
			return 0f;
		}
		if (target.occupiedTile != null && target.occupiedTile.GetEffectPartAbsoluteBuff(_EffectPartType.DefCriticalChance, this))
		{
			return 0f;
		}
		if (target.GetEffectPartAbsoluteDebuff(_EffectPartType.DefCriticalChance, uab))
		{
			return 1f;
		}
		if (target.occupiedTile != null && target.occupiedTile.GetEffectPartAbsoluteDebuff(_EffectPartType.DefCriticalChance, this))
		{
			return 1f;
		}
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.CriticalChance, uab))
		{
			return 0f;
		}
		if (origTile != null && origTile.GetEffectPartAbsoluteDebuff(_EffectPartType.CriticalChance, this))
		{
			return 0f;
		}
		Quaternion quaternion = Quaternion.LookRotation(target.thisT.position - origTile.pos);
		float num = Quaternion.Angle(quaternion, target.thisT.rotation);
		int range = GridManager.Distance(origTile, target.occupiedTile);
		float num2 = this.GetAbilityCriticalRate(uab, range, origTile);
		if (num < 31f)
		{
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.CriticalChance, 400003, 0) * 0.01f;
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.CriticalChance, 400004, 0) * 0.01f;
		}
		else if (num > 149f)
		{
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.CriticalChance, 400001, 0) * 0.01f;
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.CriticalChance, 400002, 0) * 0.01f;
		}
		else if (num < 91f)
		{
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.CriticalChance, 400005, 0) * 0.01f;
			num2 += this.GetConditionInstantValuePercent(_EffectPartType.CriticalChance, 400006, 0) * 0.01f;
		}
		num2 -= target.GetDefanceCriticalRate(uab, range);
		if (target.GetEffectPartAbsoluteDebuff(_EffectPartType.BeCriticalChanceDouble, null) || target.occupiedTile.GetEffectPartAbsoluteDebuff(_EffectPartType.BeCriticalChanceDouble, target))
		{
			num2 *= 2f;
		}
		return Mathf.Clamp(num2, 0f, 1f);
	}

	// Token: 0x06002EA7 RID: 11943 RVA: 0x0001DCD7 File Offset: 0x0001BED7
	public float GetUnitCritDef()
	{
		return this.fDefCriticalRate;
	}

	// Token: 0x06002EA8 RID: 11944 RVA: 0x0001DCDF File Offset: 0x0001BEDF
	public float GetTileCritDef()
	{
		if (this.occupiedTile == null)
		{
			return 0f;
		}
		return this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.DefCriticalChance, this) * 0.01f;
	}

	// Token: 0x06002EA9 RID: 11945 RVA: 0x00168E70 File Offset: 0x00167070
	public float GetDefanceCriticalRate(UnitAbility uab, int range)
	{
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.DefCriticalChance, uab))
		{
			return 0f;
		}
		if (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteDebuff(_EffectPartType.DefCriticalChance, this))
		{
			return 0f;
		}
		if (this.GetEffectPartAbsoluteBuff(_EffectPartType.DefCriticalChance, uab))
		{
			return 1f;
		}
		if (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteBuff(_EffectPartType.DefCriticalChance, this))
		{
			return 1f;
		}
		return this.GetUnitCritDef() + this.GetTileCritDef() + this.GetEffectPartValuePercent(_EffectPartType.DefCriticalChance, uab, range) * 0.01f;
	}

	// Token: 0x06002EAA RID: 11946 RVA: 0x0001DD0C File Offset: 0x0001BF0C
	public float GetUnitDefanceCounterRate()
	{
		return this.fDefCounterRate;
	}

	// Token: 0x06002EAB RID: 11947 RVA: 0x0001DD14 File Offset: 0x0001BF14
	public float GetTileDefanceCounterRate()
	{
		if (this.occupiedTile == null)
		{
			return 0f;
		}
		return this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.DefCounterAttackChance, this) * 0.01f;
	}

	// Token: 0x06002EAC RID: 11948 RVA: 0x00168F1C File Offset: 0x0016711C
	public float GetAbilityDefanceCounterRate(UnitAbility uab, int range)
	{
		if (this.GetEffectPartAbsoluteBuff(_EffectPartType.DefCounterAttackChance, uab))
		{
			return 1f;
		}
		if (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteBuff(_EffectPartType.DefCounterAttackChance, this))
		{
			return 1f;
		}
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.DefCounterAttackChance, uab))
		{
			return 0f;
		}
		if (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteDebuff(_EffectPartType.DefCounterAttackChance, this))
		{
			return 0f;
		}
		return this.GetUnitDefanceCounterRate() + this.GetTileDefanceCounterRate() + this.GetEffectPartValuePercent(_EffectPartType.DefCounterAttackChance, uab, range) * 0.01f;
	}

	// Token: 0x06002EAD RID: 11949 RVA: 0x00168FC8 File Offset: 0x001671C8
	public float GetAbilityToTargetCounterRate(UnitTB srcUnit, UnitAbility unitAbility)
	{
		if (srcUnit == null)
		{
			return 0f;
		}
		if (srcUnit.GetEffectPartAbsoluteBuff(_EffectPartType.DefCounterAttackChance, unitAbility))
		{
			return 0f;
		}
		if (srcUnit.occupiedTile != null && srcUnit.occupiedTile.GetEffectPartAbsoluteBuff(_EffectPartType.DefCounterAttackChance, this))
		{
			return 0f;
		}
		if (this.GetEffectPartAbsoluteBuff(_EffectPartType.CounterAttackChance, unitAbility))
		{
			return 1f;
		}
		if (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteBuff(_EffectPartType.CounterAttackChance, this))
		{
			return 1f;
		}
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.CounterAttackChance, unitAbility))
		{
			return 0f;
		}
		if (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteDebuff(_EffectPartType.CounterAttackChance, this))
		{
			return 0f;
		}
		if (srcUnit.GetEffectPartAbsoluteDebuff(_EffectPartType.DefCounterAttackChance, unitAbility))
		{
			return 1f;
		}
		if (srcUnit.occupiedTile != null && srcUnit.occupiedTile.GetEffectPartAbsoluteDebuff(_EffectPartType.DefCounterAttackChance, this))
		{
			return 1f;
		}
		int range = GridManager.Distance(srcUnit.occupiedTile, this.occupiedTile);
		float abilityDefanceCounterRate = srcUnit.GetAbilityDefanceCounterRate(unitAbility, range);
		float num = this.GetCounterRate(unitAbility, range);
		Quaternion quaternion = Quaternion.LookRotation(this.thisT.position - srcUnit.thisT.position);
		float num2 = Quaternion.Angle(quaternion, this.thisT.rotation);
		if (num2 < 31f)
		{
			num += this.GetConditionInstantValuePercent(_EffectPartType.CounterAttackChance, 400003, 0) * 0.01f;
			num += this.GetConditionInstantValuePercent(_EffectPartType.CounterAttackChance, 400004, 0) * 0.01f;
		}
		else if (num2 > 149f)
		{
			num += this.GetConditionInstantValuePercent(_EffectPartType.CounterAttackChance, 400001, 0) * 0.01f;
			num += this.GetConditionInstantValuePercent(_EffectPartType.CounterAttackChance, 400002, 0) * 0.01f;
		}
		else if (num2 < 91f)
		{
			num += this.GetConditionInstantValuePercent(_EffectPartType.CounterAttackChance, 400005, 0) * 0.01f;
			num += this.GetConditionInstantValuePercent(_EffectPartType.CounterAttackChance, 400006, 0) * 0.01f;
		}
		return Mathf.Clamp(num - abilityDefanceCounterRate, 0f, 1f);
	}

	// Token: 0x06002EAE RID: 11950 RVA: 0x0001DD41 File Offset: 0x0001BF41
	public float GetUnitCounterRate()
	{
		return this.fCounterRate;
	}

	// Token: 0x06002EAF RID: 11951 RVA: 0x0001DD49 File Offset: 0x0001BF49
	public float GetTileCounterRate()
	{
		if (this.occupiedTile == null)
		{
			return 0f;
		}
		return this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.CounterAttackChance, this) * 0.01f;
	}

	// Token: 0x06002EB0 RID: 11952 RVA: 0x001691FC File Offset: 0x001673FC
	public float GetCounterRate(UnitAbility uab, int range)
	{
		if (this.GetEffectPartAbsoluteDebuff(_EffectPartType.CounterAttackChance, uab))
		{
			return 0f;
		}
		if (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteDebuff(_EffectPartType.CounterAttackChance, this))
		{
			return 0f;
		}
		if (this.GetEffectPartAbsoluteBuff(_EffectPartType.CounterAttackChance, uab))
		{
			return 1f;
		}
		if (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteBuff(_EffectPartType.CounterAttackChance, this))
		{
			return 1f;
		}
		return this.GetUnitCounterRate() + this.GetTileCounterRate() + this.GetEffectPartValuePercent(_EffectPartType.CounterAttackChance, uab, range) * 0.01f;
	}

	// Token: 0x06002EB1 RID: 11953 RVA: 0x001692A0 File Offset: 0x001674A0
	public Tile CheckChargeTile(UnitTB target, UnitAbility uab)
	{
		Tile tile = null;
		if (this.GetUnitTileAbsoluteBuff(_EffectPartType.Charge1, uab))
		{
			tile = GridManager.GetChargeFrontTile(this.occupiedTile, target.occupiedTile);
		}
		if (this.GetUnitTileAbsoluteBuff(_EffectPartType.Charge2, uab))
		{
			tile = GridManager.GetChargeBackTile(this.occupiedTile, target.occupiedTile);
		}
		if (tile == null)
		{
			return null;
		}
		if (tile.unit != null)
		{
			return null;
		}
		if (!tile.walkable)
		{
			return null;
		}
		if (!tile.invisible)
		{
			return null;
		}
		if (tile.bUnitOrder)
		{
			return null;
		}
		return tile;
	}

	// Token: 0x06002EB2 RID: 11954 RVA: 0x00169338 File Offset: 0x00167538
	public Tile CheckKnockBackTile(UnitTB target, UnitAbility uab)
	{
		int iRange = Mathf.RoundToInt(this.GetUnitAbilityValue(_EffectPartType.Knockback, uab, 0, true));
		if (!this.GetUnitTileAbsoluteBuff(_EffectPartType.Knockback, uab))
		{
			return null;
		}
		Tile tileOrigin;
		if (this.bCharge)
		{
			tileOrigin = this.tChargeToTile;
		}
		else
		{
			tileOrigin = this.occupiedTile;
		}
		Tile knockBackTile = GridManager.GetKnockBackTile(tileOrigin, target.occupiedTile, iRange);
		if (knockBackTile == null)
		{
			return null;
		}
		if (knockBackTile.unit != null && !knockBackTile.unit.bSpeicalAction)
		{
			return null;
		}
		if (!knockBackTile.walkable)
		{
			return null;
		}
		if (!knockBackTile.invisible)
		{
			return null;
		}
		if (knockBackTile.bUnitOrder)
		{
			return null;
		}
		return knockBackTile;
	}

	// Token: 0x06002EB3 RID: 11955 RVA: 0x001693EC File Offset: 0x001675EC
	public Tile CheckPullCloseTile(UnitTB target, UnitAbility uab)
	{
		if (!this.GetUnitTileAbsoluteBuff(_EffectPartType.PullClose, uab))
		{
			return null;
		}
		Tile tileOrigin;
		if (this.bCharge)
		{
			tileOrigin = this.tChargeToTile;
		}
		else
		{
			tileOrigin = this.occupiedTile;
		}
		return GridManager.GetPullCloseTile(tileOrigin, target.occupiedTile);
	}

	// Token: 0x06002EB4 RID: 11956 RVA: 0x0001DD75 File Offset: 0x0001BF75
	public float GetUnitDamageReduc()
	{
		return (float)this.iDamageReduc;
	}

	// Token: 0x06002EB5 RID: 11957 RVA: 0x0001DD7E File Offset: 0x0001BF7E
	public float GetTileDamageReduc()
	{
		if (this.occupiedTile == null)
		{
			return 0f;
		}
		return this.occupiedTile.GetEffectPartValue(_EffectPartType.DamageReduction, this);
	}

	// Token: 0x06002EB6 RID: 11958 RVA: 0x0001DDA4 File Offset: 0x0001BFA4
	public float GetTileDamageReducMod()
	{
		if (this.occupiedTile == null)
		{
			return 0f;
		}
		return this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.DamageReduction, this) * 0.01f;
	}

	// Token: 0x06002EB7 RID: 11959 RVA: 0x00169434 File Offset: 0x00167634
	public float GetDamageReduc(float fOrigDR)
	{
		if (GameGlobal.m_bDLCMode)
		{
			return fOrigDR + this.GetEffectPartValuePercent(_EffectPartType.DamageReduction, null, 0) + this.GetTileDamageReducMod() * 100f;
		}
		float num = fOrigDR + this.GetTileDamageReduc() + this.GetEffectPartValue(_EffectPartType.DamageReduction, null, 0);
		num *= 1f + this.GetEffectPartValuePercent(_EffectPartType.DamageReduction, null, 0) * 0.01f + this.GetTileDamageReducMod();
		return Mathf.Max(0f, num);
	}

	// Token: 0x06002EB8 RID: 11960 RVA: 0x001694A4 File Offset: 0x001676A4
	public int GetAbilityDamage(UnitAbility uab)
	{
		int routineLv = this.GetRoutineLv(uab.ID);
		float num = 0.1f * uab.damageMin;
		float num2 = num * (float)this.GetAbilityCost(uab, true);
		float num3 = this.GetEffectPartValuePercent(_EffectPartType.Damage, uab, 0);
		num3 += this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.Damage, this);
		num3 *= 0.01f;
		num3 += 1f;
		if (!GameGlobal.m_bDLCMode)
		{
			num3 += 0.1f * (float)routineLv;
		}
		num3 += this.GetTalentDamage(uab) + this.occupiedTile.GetTalentDamagePercent(this);
		num2 *= num3;
		num2 += this.GetEffectPartValue(_EffectPartType.Damage, uab, 0);
		num2 += (float)this.characterData._TotalProperty.Get(CharacterData.PropertyType.Attack);
		return Mathf.RoundToInt(num2);
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x0016955C File Offset: 0x0016775C
	public int GetAbilityToTargetHeal(UnitTB unitTarget, UnitAbility uab, Tile OrigTile)
	{
		int routineLv = this.GetRoutineLv(uab.ID);
		int iRange = GridManager.Distance(OrigTile, unitTarget.occupiedTile);
		float num = 0.1f * uab.damageMin;
		float num2 = this.GetEffectPartValue(_EffectPartType.Damage, uab, iRange);
		num2 += OrigTile.GetEffectPartValue(_EffectPartType.Damage, this);
		float num3 = this.GetEffectPartValuePercent(_EffectPartType.Damage, uab, iRange);
		num3 += OrigTile.GetEffectPartValuePercent(_EffectPartType.Damage, this);
		num3 *= 0.01f;
		num3 += 1f;
		if (!GameGlobal.m_bDLCMode)
		{
			num3 += 0.1f * (float)routineLv;
		}
		num3 += this.GetTalentDamage(uab) + this.occupiedTile.GetTalentDamagePercent(this);
		if (GameGlobal.m_iBattleDifficulty > 8)
		{
			num3 = Mathf.Clamp(num3, 0.75f, 32f);
		}
		else if (GameGlobal.m_iBattleDifficulty > 5)
		{
			num3 = Mathf.Clamp(num3, 0.5f, 32f);
		}
		else
		{
			num3 = Mathf.Clamp(num3, 0.1f, 32f);
		}
		float num4 = num * (float)this.GetAbilityCost(uab, true);
		float num5 = num4;
		float num6 = num5 * num3;
		float num7 = this.GetUnitAbilityValue(_EffectPartType.HitPoint, uab, iRange, false);
		num6 += num7;
		num7 = 0.01f * (float)unitTarget.fullHP * this.GetUnitAbilityValuePercent(_EffectPartType.HitPoint, uab, iRange, false);
		num6 += num7;
		if (uab.bItemSkill)
		{
			float num8 = 1f + this.CheckTalentSelfAndTeamPlus(TalentEffect.ItemEffectPlus, true, false);
			num6 *= num8;
		}
		return Mathf.RoundToInt(num6);
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x001696D8 File Offset: 0x001678D8
	public int GetAbilityToTargetDamage(UnitTB unitTarget, UnitAbility uab, Tile OrigTile)
	{
		if (uab.effectType == _EffectType.Heal || uab.effectType == _EffectType.Buff)
		{
			return this.GetAbilityToTargetHeal(unitTarget, uab, OrigTile);
		}
		int routineLv = this.GetRoutineLv(uab.ID);
		int iRange = GridManager.Distance(OrigTile, unitTarget.occupiedTile);
		float num = 0.1f * uab.damageMin;
		float num2 = this.GetEffectPartValue(_EffectPartType.Damage, uab, iRange);
		num2 += OrigTile.GetEffectPartValue(_EffectPartType.Damage, this);
		float num3 = this.GetEffectPartValuePercent(_EffectPartType.Damage, uab, iRange);
		num3 += OrigTile.GetEffectPartValuePercent(_EffectPartType.Damage, this);
		num3 *= 0.01f;
		num3 += 1f;
		if (!GameGlobal.m_bDLCMode)
		{
			num3 += 0.1f * (float)routineLv;
		}
		num3 += this.GetTalentDamage(uab) + this.occupiedTile.GetTalentDamagePercent(this);
		if (GameGlobal.m_iBattleDifficulty > 8)
		{
			num3 = Mathf.Clamp(num3, 0.75f, 32f);
		}
		else if (GameGlobal.m_iBattleDifficulty > 5)
		{
			num3 = Mathf.Clamp(num3, 0.5f, 32f);
		}
		else
		{
			num3 = Mathf.Clamp(num3, 0.1f, 32f);
		}
		float num4 = num * (float)this.GetAbilityCost(uab, true);
		float num5 = num4;
		num5 *= num3;
		float num6 = (float)this.GetRoutineMartialAttack(uab.ID);
		float num7 = unitTarget.GetDamageReduc((float)unitTarget.GetRoutineMartialDef(uab.ID));
		if (this.GetUnitTileAbsoluteDebuff(_EffectPartType.DamageReduction, uab))
		{
			if (GameGlobal.m_bDLCMode)
			{
				if (num7 > 0f)
				{
					num7 = 0f;
				}
			}
			else
			{
				num7 = 0f;
			}
		}
		float num8;
		if (GameGlobal.m_bDLCMode)
		{
			num7 = 100f - num7;
			if (num7 < 5f)
			{
				num7 = 5f;
			}
			num8 = num5 * num7 * 0.01f;
		}
		else
		{
			if (num6 + num7 > 0f)
			{
				num7 = (num6 + num6) / (num6 + num7);
			}
			else
			{
				num7 = 0f;
			}
			if (num7 == 0f)
			{
				num8 = 1f;
			}
			else
			{
				num8 = num5 * num7;
			}
		}
		int num9 = this.characterData._TotalProperty.Get(CharacterData.PropertyType.Attack);
		num8 += (float)num9 + num2;
		float num10 = unitTarget.fDamageReduc;
		float num11 = 1f - unitTarget.GetEffectPartValuePercent(_EffectPartType.RealDmgReduc, null, 0) * 0.01f;
		if (unitTarget.occupiedTile != null)
		{
			num11 -= 0.01f * unitTarget.occupiedTile.GetEffectPartValuePercent(_EffectPartType.RealDmgReduc, unitTarget);
		}
		num10 = Mathf.Clamp01(num10);
		num11 = Mathf.Clamp01(num11);
		num10 *= num11;
		num8 *= num10;
		float num12 = this.GetUnitAbilityValue(_EffectPartType.HitPoint, uab, iRange, false);
		num8 -= num12;
		num12 = 0.01f * (float)unitTarget.fullHP * this.GetUnitAbilityValuePercent(_EffectPartType.HitPoint, uab, iRange, false);
		num8 -= num12;
		if (uab.bItemSkill)
		{
			float num13 = 1f + this.CheckTalentSelfAndTeamPlus(TalentEffect.ItemEffectPlus, true, false);
			num8 *= num13;
		}
		return Mathf.RoundToInt(num8);
	}

	// Token: 0x06002EBB RID: 11963 RVA: 0x0001DDD0 File Offset: 0x0001BFD0
	public int GetUnitMovePerTurn()
	{
		return Mathf.Max(0, this.movePerTurn);
	}

	// Token: 0x06002EBC RID: 11964 RVA: 0x0001DDDE File Offset: 0x0001BFDE
	public int GetTileMovePerTurn()
	{
		if (this.occupiedTile == null)
		{
			return 0;
		}
		return Mathf.RoundToInt(this.occupiedTile.GetEffectPartValue(_EffectPartType.MovePreturn, this));
	}

	// Token: 0x06002EBD RID: 11965 RVA: 0x0001DE06 File Offset: 0x0001C006
	public int GetMovePerTurn()
	{
		return this.GetUnitMovePerTurn() + this.GetTileMovePerTurn() + Mathf.RoundToInt(this.GetEffectPartValue(_EffectPartType.MovePreturn, null, 0));
	}

	// Token: 0x06002EBE RID: 11966 RVA: 0x0001DE25 File Offset: 0x0001C025
	public int GetUnitAttackPerTurn()
	{
		return Mathf.Max(0, this.attackPerTurn);
	}

	// Token: 0x06002EBF RID: 11967 RVA: 0x0001DE33 File Offset: 0x0001C033
	public int GetTileAttackPerTurn()
	{
		if (this.occupiedTile == null)
		{
			return 0;
		}
		return Mathf.RoundToInt(this.occupiedTile.GetEffectPartValue(_EffectPartType.AttackPreturn, this));
	}

	// Token: 0x06002EC0 RID: 11968 RVA: 0x0001DE5B File Offset: 0x0001C05B
	public int GetAttackPerTurn()
	{
		return this.GetUnitAttackPerTurn() + this.GetTileAttackPerTurn() + Mathf.RoundToInt(this.GetEffectPartValue(_EffectPartType.AttackPreturn, null, 0));
	}

	// Token: 0x06002EC1 RID: 11969 RVA: 0x0001DE7A File Offset: 0x0001C07A
	public int GetUnitCounterPerTurn()
	{
		return Mathf.Max(0, this.counterPerTurn);
	}

	// Token: 0x06002EC2 RID: 11970 RVA: 0x0001DE88 File Offset: 0x0001C088
	public int GetTileCounterPerTurn()
	{
		if (this.occupiedTile == null)
		{
			return 0;
		}
		return Mathf.RoundToInt(this.occupiedTile.GetEffectPartValue(_EffectPartType.CounterPreturn, this));
	}

	// Token: 0x06002EC3 RID: 11971 RVA: 0x0001DEB0 File Offset: 0x0001C0B0
	public int GetCounterPerTurn()
	{
		return this.GetUnitCounterPerTurn() + this.GetTileCounterPerTurn() + Mathf.RoundToInt(this.GetEffectPartValue(_EffectPartType.CounterPreturn, null, 0));
	}

	// Token: 0x06002EC4 RID: 11972 RVA: 0x0001DECF File Offset: 0x0001C0CF
	public int GetUnitFullHP()
	{
		return this.fullHP;
	}

	// Token: 0x06002EC5 RID: 11973 RVA: 0x0001DED7 File Offset: 0x0001C0D7
	public int GetUnitFullSP()
	{
		return this.fullSP;
	}

	// Token: 0x06002EC6 RID: 11974 RVA: 0x0001DEDF File Offset: 0x0001C0DF
	public int GetUnitHP()
	{
		return this.HP;
	}

	// Token: 0x06002EC7 RID: 11975 RVA: 0x0001DEE7 File Offset: 0x0001C0E7
	public int GetUnitSP()
	{
		return this.SP;
	}

	// Token: 0x06002EC8 RID: 11976 RVA: 0x0001DEEF File Offset: 0x0001C0EF
	public int GetUnitTurnPriority()
	{
		return Mathf.Max(1, this.turnPriority);
	}

	// Token: 0x06002EC9 RID: 11977 RVA: 0x0001DEFD File Offset: 0x0001C0FD
	public int GetUnitSight()
	{
		return Mathf.Max(0, this.sight);
	}

	// Token: 0x06002ECA RID: 11978 RVA: 0x001699CC File Offset: 0x00167BCC
	public int GetUnitAbilityRangeMin()
	{
		int num = 100;
		for (int i = 0; i < this.unitAbilityList.Count; i++)
		{
			if (this.IsAbilityAvailable(i) == 0)
			{
				num = 1;
			}
		}
		if (num == 100)
		{
			return 0;
		}
		if (num < 0)
		{
			return 0;
		}
		return num;
	}

	// Token: 0x06002ECB RID: 11979 RVA: 0x00169A1C File Offset: 0x00167C1C
	public int GetUnitAbilityRangeMax()
	{
		int num = 0;
		for (int i = 0; i < this.unitAbilityList.Count; i++)
		{
			UnitAbility unitAbility = this.unitAbilityList[i];
			if (this.IsAbilityAvailable(i) == 0)
			{
				if (unitAbility.requireTargetSelection)
				{
					if (num < unitAbility.range)
					{
						num = unitAbility.range;
					}
				}
				else if (num < unitAbility.aoeRange)
				{
					num = unitAbility.aoeRange;
				}
			}
		}
		return num;
	}

	// Token: 0x06002ECC RID: 11980 RVA: 0x0000F098 File Offset: 0x0000D298
	public float GetTileHP()
	{
		return 0f;
	}

	// Token: 0x06002ECD RID: 11981 RVA: 0x0000F098 File Offset: 0x0000D298
	public float GetTileAP()
	{
		return 0f;
	}

	// Token: 0x06002ECE RID: 11982 RVA: 0x0001DF0B File Offset: 0x0001C10B
	public float GetTileHPGain()
	{
		return (float)((!(this.occupiedTile == null)) ? this.occupiedTile.HPGainModifier : 0);
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x0001DF30 File Offset: 0x0001C130
	public float GetTileAPGain()
	{
		return (float)((!(this.occupiedTile == null)) ? this.occupiedTile.APGainModifier : 0);
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x0000F098 File Offset: 0x0000D298
	public float GetTileTurnPriority()
	{
		return 0f;
	}

	// Token: 0x06002ED1 RID: 11985 RVA: 0x0001DF55 File Offset: 0x0001C155
	public float GetTileSight()
	{
		return (float)((!(this.occupiedTile == null)) ? this.occupiedTile.sightModifier : 0);
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x0001DF7A File Offset: 0x0001C17A
	public float GetTileAttackRange()
	{
		return (float)((!(this.occupiedTile == null)) ? this.occupiedTile.attRangeModifier : 0);
	}

	// Token: 0x06002ED3 RID: 11987 RVA: 0x0001DF9F File Offset: 0x0001C19F
	public float GetTileDmgRMin()
	{
		return (float)((!(this.occupiedTile == null)) ? this.occupiedTile.damageModifier : 0);
	}

	// Token: 0x06002ED4 RID: 11988 RVA: 0x0001DF9F File Offset: 0x0001C19F
	public float GetTileDmgMMin()
	{
		return (float)((!(this.occupiedTile == null)) ? this.occupiedTile.damageModifier : 0);
	}

	// Token: 0x06002ED5 RID: 11989 RVA: 0x0001DF9F File Offset: 0x0001C19F
	public float GetTileDmgRMax()
	{
		return (float)((!(this.occupiedTile == null)) ? this.occupiedTile.damageModifier : 0);
	}

	// Token: 0x06002ED6 RID: 11990 RVA: 0x0001DF9F File Offset: 0x0001C19F
	public float GetTileDmgMMax()
	{
		return (float)((!(this.occupiedTile == null)) ? this.occupiedTile.damageModifier : 0);
	}

	// Token: 0x06002ED7 RID: 11991 RVA: 0x0000F098 File Offset: 0x0000D298
	public float GetTileDmgCounter()
	{
		return 0f;
	}

	// Token: 0x06002ED8 RID: 11992 RVA: 0x0001DFC4 File Offset: 0x0001C1C4
	public float GetTileMeleeAttack()
	{
		return (!(this.occupiedTile == null)) ? this.occupiedTile.attackModifier : 0f;
	}

	// Token: 0x06002ED9 RID: 11993 RVA: 0x0001DFEC File Offset: 0x0001C1EC
	public float GetTileRangeCritical()
	{
		return (!(this.occupiedTile == null)) ? this.occupiedTile.criticalModifier : 0f;
	}

	// Token: 0x06002EDA RID: 11994 RVA: 0x0001DFEC File Offset: 0x0001C1EC
	public float GetTileMeleeCritical()
	{
		return (!(this.occupiedTile == null)) ? this.occupiedTile.criticalModifier : 0f;
	}

	// Token: 0x06002EDB RID: 11995 RVA: 0x0001E014 File Offset: 0x0001C214
	public float GetTileCounterAttackChance()
	{
		return (!(this.occupiedTile == null)) ? this.occupiedTile.counterAttackModifier : 0f;
	}

	// Token: 0x06002EDC RID: 11996 RVA: 0x0001E03C File Offset: 0x0001C23C
	public float GetTileDefend()
	{
		return (!(this.occupiedTile == null)) ? this.occupiedTile.defendModifier : 0f;
	}

	// Token: 0x06002EDD RID: 11997 RVA: 0x00002C2D File Offset: 0x00000E2D
	public int GetTileDmgReduc()
	{
		return 0;
	}

	// Token: 0x06002EDE RID: 11998 RVA: 0x0000F098 File Offset: 0x0000D298
	public float GetTileAPCostAttack()
	{
		return 0f;
	}

	// Token: 0x06002EDF RID: 11999 RVA: 0x0000F098 File Offset: 0x0000D298
	public float GetTileAPCostMove()
	{
		return 0f;
	}

	// Token: 0x06002EE0 RID: 12000 RVA: 0x0001E064 File Offset: 0x0001C264
	public int GetFullHP()
	{
		return this.GetUnitFullHP() + (int)this.GetTileHP();
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x0001E074 File Offset: 0x0001C274
	public int GetFullSP()
	{
		return this.GetUnitFullSP() + (int)this.GetTileAP();
	}

	// Token: 0x06002EE2 RID: 12002 RVA: 0x0001E084 File Offset: 0x0001C284
	public int GetTurnPriority()
	{
		return this.GetUnitTurnPriority() + (int)this.GetTileTurnPriority();
	}

	// Token: 0x06002EE3 RID: 12003 RVA: 0x0001E094 File Offset: 0x0001C294
	public int GetUnitMoveRange()
	{
		return Mathf.Max(0, this.movementRange);
	}

	// Token: 0x06002EE4 RID: 12004 RVA: 0x0001E0A2 File Offset: 0x0001C2A2
	public int GetTileMoveRange()
	{
		if (this.occupiedTile == null)
		{
			return 0;
		}
		return (int)this.occupiedTile.GetEffectPartValue(_EffectPartType.MoveRange, this);
	}

	// Token: 0x06002EE5 RID: 12005 RVA: 0x0001E0C6 File Offset: 0x0001C2C6
	public int GetMoveRange()
	{
		return this.GetUnitMoveRange() + (int)this.GetEffectPartValue(_EffectPartType.MoveRange, null, 0) + this.GetTileMoveRange();
	}

	// Token: 0x06002EE6 RID: 12006 RVA: 0x0001E0E1 File Offset: 0x0001C2E1
	public float GetUnitFlyOnGrass()
	{
		return Mathf.Max(0f, this.fFlyOnGrass);
	}

	// Token: 0x06002EE7 RID: 12007 RVA: 0x0001E0F3 File Offset: 0x0001C2F3
	public float GetTileFlyOnGrass()
	{
		if (this.occupiedTile == null)
		{
			return 0f;
		}
		return this.occupiedTile.GetEffectPartValue(_EffectPartType.FlyOnGrass, this);
	}

	// Token: 0x06002EE8 RID: 12008 RVA: 0x0001E11A File Offset: 0x0001C31A
	public float GetFlyOnGrass()
	{
		return this.GetUnitFlyOnGrass() + this.GetEffectPartValue(_EffectPartType.FlyOnGrass, null, 0) + this.GetTileFlyOnGrass();
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x0001E134 File Offset: 0x0001C334
	public int GetSight()
	{
		return this.GetUnitSight() + (int)this.GetTileSight();
	}

	// Token: 0x06002EEA RID: 12010 RVA: 0x0001E144 File Offset: 0x0001C344
	public int GetStun()
	{
		return Mathf.Max(0, this.stun);
	}

	// Token: 0x06002EEB RID: 12011 RVA: 0x00169A98 File Offset: 0x00167C98
	public bool GetAttackDisabled()
	{
		return this.GetEffectPartAbsoluteDebuff(_EffectPartType.AttackPreturn, null) || this.occupiedTile.GetEffectPartAbsoluteBuff(_EffectPartType.AttackPreturn, this) || (this.attacked || this.attackRemain <= 0);
	}

	// Token: 0x06002EEC RID: 12012 RVA: 0x0001E152 File Offset: 0x0001C352
	public bool GetMovementDisabled()
	{
		return this.GetUnitTileAbsoluteDebuff(_EffectPartType.MovePreturn, null) || (this.moveRemain <= 0 || this.moved);
	}

	// Token: 0x06002EED RID: 12013 RVA: 0x0001E17E File Offset: 0x0001C37E
	public bool GetCounterAttackDisabled()
	{
		return this.GetUnitTileAbsoluteDebuff(_EffectPartType.CounterPreturn, null) || this.counterAttackRemain <= 0;
	}

	// Token: 0x06002EEE RID: 12014 RVA: 0x0001E19F File Offset: 0x0001C39F
	public int GetAbilityDisabled()
	{
		return Mathf.Max(0, this.abilityDisabled);
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x0001E1AD File Offset: 0x0001C3AD
	public bool InAction()
	{
		return this.actionQueued > 0;
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x0001E1BE File Offset: 0x0001C3BE
	public bool IsStunned()
	{
		return this.stun > 0;
	}

	// Token: 0x06002EF1 RID: 12017 RVA: 0x0001E1CF File Offset: 0x0001C3CF
	public bool MovedForTheRound()
	{
		return this.moved || this.attacked || this.abilityTriggered;
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x0001E1F5 File Offset: 0x0001C3F5
	public List<ConditionNode> GetAllCondition()
	{
		return this.unitConditionList;
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x0001E1FD File Offset: 0x0001C3FD
	public bool IsDestroyed()
	{
		return this.HP <= 0;
	}

	// Token: 0x06002EF4 RID: 12020 RVA: 0x00169AE4 File Offset: 0x00167CE4
	private void UpdateEffectCompare(_EffectPartType typePart, int iNow, int iMax)
	{
		bool effectPartAbsoluteDebuff = this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, null);
		int num = iNow * 100;
		if (iMax < 1)
		{
			iMax = 1;
		}
		num /= iMax;
		if (this.unitNeigong != null)
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (effectPart.m_effectPartType == typePart)
					{
						if (effectPart.m_effectAccumulateType == _EffectAccumulateType.Compare)
						{
							if (effectPart.m_bPercent)
							{
								if (num < effectPart.m_iValueLimit)
								{
									if (effectPartAbsoluteDebuff)
									{
										this.RemoveConditionID(effectPart.m_iValue1, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName);
									}
									else
									{
										this.ApplyConditionID(effectPart.m_iValue1, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName, false);
									}
									this.RemoveConditionID(effectPart.m_iValue2, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName);
								}
								else
								{
									if (effectPartAbsoluteDebuff)
									{
										this.RemoveConditionID(effectPart.m_iValue2, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName);
									}
									else
									{
										this.ApplyConditionID(effectPart.m_iValue2, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName, false);
									}
									this.RemoveConditionID(effectPart.m_iValue1, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName);
								}
							}
							else if (iNow < effectPart.m_iValueLimit)
							{
								if (effectPartAbsoluteDebuff)
								{
									this.RemoveConditionID(effectPart.m_iValue1, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName);
								}
								else
								{
									this.ApplyConditionID(effectPart.m_iValue1, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName, false);
								}
								this.RemoveConditionID(effectPart.m_iValue2, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName);
							}
							else
							{
								if (effectPartAbsoluteDebuff)
								{
									this.RemoveConditionID(effectPart.m_iValue2, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName);
								}
								else
								{
									this.ApplyConditionID(effectPart.m_iValue2, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName, false);
								}
								this.RemoveConditionID(effectPart.m_iValue1, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06002EF5 RID: 12021 RVA: 0x00169DDC File Offset: 0x00167FDC
	private void _ClearnupSelfOrTargetCondition(ConditionNode condition, string Name, UnitTB targetUnit)
	{
		bool flag = false;
		for (int i = 0; i < condition.m_effectPartList.Count; i++)
		{
			EffectPart effectPart = condition.m_effectPartList[i];
			if (effectPart.m_effectPartType == _EffectPartType.Cleanup)
			{
				if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AfterAction)
				{
					if (condition.m_iCondType == _ConditionType.InstantDebuff)
					{
						if (condition.m_iCondTarget == 0)
						{
							if (this.RemoveConditionID_Po(effectPart.m_iValue1, Name))
							{
								flag = true;
							}
						}
						else if (condition.m_iCondTarget == 1)
						{
							if (targetUnit.RemoveConditionID_Po(effectPart.m_iValue1, Name))
							{
								flag = true;
							}
						}
						else
						{
							if (this.RemoveConditionID_Po(effectPart.m_iValue1, Name))
							{
								flag = true;
							}
							if (targetUnit.RemoveConditionID_Po(effectPart.m_iValue1, Name))
							{
								flag = true;
							}
						}
					}
				}
			}
		}
		if (flag)
		{
			new EffectOverlay(this.thisT.position + new Vector3(0f, 0.5f, 0f), string.Empty, _OverlayType.PoBati, 0f);
		}
	}

	// Token: 0x06002EF6 RID: 12022 RVA: 0x00169EF0 File Offset: 0x001680F0
	public void ClearnupSelfOrTargetCondition(UnitAbility currentAbility, UnitTB targetUnit)
	{
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, currentAbility))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				if (conditionNode.m_iCondType == _ConditionType.InstantBuff || conditionNode.m_iCondType == _ConditionType.InstantDebuff)
				{
					this._ClearnupSelfOrTargetCondition(conditionNode, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName, targetUnit);
				}
			}
		}
		for (int j = 0; j < this.unitConditionList.Count; j++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[j];
			if (conditionNode2.m_iCondType == _ConditionType.InstantBuff || conditionNode2.m_iCondType == _ConditionType.InstantDebuff)
			{
				this._ClearnupSelfOrTargetCondition(conditionNode2, conditionNode2.m_strName, targetUnit);
			}
		}
		List<int> list = new List<int>();
		if (currentAbility != null && currentAbility.chainedAbilityIDList.Count > 0)
		{
			list.AddRange(currentAbility.chainedAbilityIDList.ToArray());
		}
		list.AddRange(this.GetEquipCondition().ToArray());
		for (int k = 0; k < list.Count; k++)
		{
			if (list[k] != 0)
			{
				ConditionNode conditionNode3 = Game.g_BattleControl.m_battleAbility.GetConditionNode(list[k]);
				if (conditionNode3 == null)
				{
					Debug.LogError(list[k].ToString() + " 狀態找不到");
				}
				else if (conditionNode3.m_iCondType == _ConditionType.InstantBuff || conditionNode3.m_iCondType == _ConditionType.InstantDebuff)
				{
					this._ClearnupSelfOrTargetCondition(conditionNode3, conditionNode3.m_strName, targetUnit);
				}
			}
		}
	}

	// Token: 0x06002EF7 RID: 12023 RVA: 0x0016A0BC File Offset: 0x001682BC
	public void ApplyConditionIDCheckFriend(int ID, string name, bool bShowContinueLog, int iSrcFactionID)
	{
		if (ID == 0)
		{
			return;
		}
		bool flag = this.CheckFriendFaction(iSrcFactionID);
		ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(ID);
		if (conditionNode == null)
		{
			return;
		}
		if ((conditionNode.m_iCondType == _ConditionType.Buff || conditionNode.m_iCondType == _ConditionType.InstantBuff || conditionNode.m_iCondType == _ConditionType.StackBuff) && !flag)
		{
			return;
		}
		if ((conditionNode.m_iCondType == _ConditionType.Debuff || conditionNode.m_iCondType == _ConditionType.InstantDebuff || conditionNode.m_iCondType == _ConditionType.StackDebuff) && flag)
		{
			return;
		}
		ConditionNode conditionNode2 = conditionNode.Clone();
		if (conditionNode2 != null)
		{
			this.ApplyCondition(conditionNode2, bShowContinueLog);
		}
	}

	// Token: 0x06002EF8 RID: 12024 RVA: 0x0016A15C File Offset: 0x0016835C
	public bool EffectPartAfterSuccess(_EffectPartType typePart, UnitAbility currentAbility, UnitTB targetUnit)
	{
		bool result = false;
		if (currentAbility != null && (currentAbility.effectType == _EffectType.Damage || currentAbility.effectType == _EffectType.Debuff))
		{
		}
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, currentAbility))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (effectPart.m_effectPartType == typePart)
					{
						if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AfterAction)
						{
							result = true;
							if (effectPart.m_iValue1 != 0)
							{
								this.ApplyConditionIDCheckFriend(effectPart.m_iValue1, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName, true, this.factionID);
							}
							if (effectPart.m_iValue2 != 0 && targetUnit != null)
							{
								targetUnit.ApplyConditionIDCheckFriend(effectPart.m_iValue2, this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName, true, this.factionID);
							}
						}
					}
				}
			}
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (effectPart2.m_effectPartType == typePart)
				{
					if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.AfterAction)
					{
						result = true;
						if (effectPart2.m_iValue1 != 0)
						{
							this.ApplyConditionIDCheckFriend(effectPart2.m_iValue1, conditionNode2.m_strName, true, this.factionID);
						}
						if (effectPart2.m_iValue2 != 0 && targetUnit != null)
						{
							targetUnit.ApplyConditionIDCheckFriend(effectPart2.m_iValue2, conditionNode2.m_strName, true, this.factionID);
						}
					}
				}
			}
		}
		List<int> list = new List<int>();
		if (currentAbility != null && currentAbility.chainedAbilityIDList.Count > 0)
		{
			list.AddRange(currentAbility.chainedAbilityIDList.ToArray());
		}
		list.AddRange(this.GetEquipCondition().ToArray());
		for (int m = 0; m < list.Count; m++)
		{
			if (list[m] != 0)
			{
				ConditionNode conditionNode3 = Game.g_BattleControl.m_battleAbility.GetConditionNode(list[m]);
				if (conditionNode3 == null)
				{
					Debug.LogError(list[m].ToString() + " 狀態找不到");
				}
				else
				{
					for (int n = 0; n < conditionNode3.m_effectPartList.Count; n++)
					{
						EffectPart effectPart3 = conditionNode3.m_effectPartList[n];
						if (effectPart3.m_effectPartType == typePart)
						{
							if (effectPart3.m_effectAccumulateType == _EffectAccumulateType.AfterAction)
							{
								result = true;
								if (effectPart3.m_iValue1 != 0)
								{
									this.ApplyConditionIDCheckFriend(effectPart3.m_iValue1, conditionNode3.m_strName, true, this.factionID);
								}
								if (effectPart3.m_iValue2 != 0 && targetUnit != null)
								{
									targetUnit.ApplyConditionIDCheckFriend(effectPart3.m_iValue2, conditionNode3.m_strName, true, this.factionID);
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06002EF9 RID: 12025 RVA: 0x0016A4F0 File Offset: 0x001686F0
	public bool GetUnitAbilityAbsoluteBuff(_EffectPartType typePart, UnitAbility uAb)
	{
		bool result = false;
		List<int> list = new List<int>();
		if (uAb != null && uAb.chainedAbilityIDList.Count > 0)
		{
			list.AddRange(uAb.chainedAbilityIDList.ToArray());
		}
		list.AddRange(this.GetEquipCondition().ToArray());
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != 0)
			{
				ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(list[i]);
				if (conditionNode == null)
				{
					Debug.LogError(list[i].ToString() + " 狀態找不到");
				}
				else if (conditionNode.m_iCondType == _ConditionType.InstantBuff)
				{
					for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
					{
						EffectPart effectPart = conditionNode.m_effectPartList[j];
						if (typePart == effectPart.m_effectPartType && effectPart.m_effectAccumulateType == _EffectAccumulateType.Absolute)
						{
							result = true;
							break;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06002EFA RID: 12026 RVA: 0x0016A60C File Offset: 0x0016880C
	public bool GetEffectPartAbsoluteBuff(_EffectPartType typePart, UnitAbility currentAbility)
	{
		bool flag = false;
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, currentAbility))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (typePart == effectPart.m_effectPartType && effectPart.m_effectAccumulateType == _EffectAccumulateType.Absolute)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		if (flag)
		{
			return flag;
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (typePart == effectPart2.m_effectPartType && effectPart2.m_effectAccumulateType == _EffectAccumulateType.Absolute && (conditionNode2.m_iCondType == _ConditionType.Buff || conditionNode2.m_iCondType == _ConditionType.InstantBuff))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (this.GetUnitAbilityAbsoluteBuff(typePart, currentAbility))
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x06002EFB RID: 12027 RVA: 0x0001E212 File Offset: 0x0001C412
	public bool GetUnitTileAbsoluteBuff(_EffectPartType typePart, UnitAbility uAb)
	{
		return this.GetEffectPartAbsoluteBuff(typePart, uAb) || (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteBuff(typePart, this));
	}

	// Token: 0x06002EFC RID: 12028 RVA: 0x0016A770 File Offset: 0x00168970
	public bool GetUnitAbilityAbsoluteDebuff(_EffectPartType typePart, UnitAbility uAb)
	{
		bool result = false;
		List<int> list = new List<int>();
		if (uAb != null && uAb.chainedAbilityIDList.Count > 0)
		{
			list.AddRange(uAb.chainedAbilityIDList.ToArray());
		}
		list.AddRange(this.GetEquipCondition().ToArray());
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != 0)
			{
				ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(list[i]);
				if (conditionNode == null)
				{
					Debug.LogError(list[i].ToString() + " 狀態找不到");
				}
				else if (conditionNode.m_iCondType == _ConditionType.InstantDebuff)
				{
					for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
					{
						EffectPart effectPart = conditionNode.m_effectPartList[j];
						if (typePart == effectPart.m_effectPartType && effectPart.m_effectAccumulateType == _EffectAccumulateType.Absolute)
						{
							result = true;
							break;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06002EFD RID: 12029 RVA: 0x0016A88C File Offset: 0x00168A8C
	public bool GetEffectPartAbsoluteDebuff(_EffectPartType typePart, UnitAbility currentAbility)
	{
		bool result = false;
		for (int i = 0; i < this.unitConditionList.Count; i++)
		{
			ConditionNode conditionNode = this.unitConditionList[i];
			for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
			{
				EffectPart effectPart = conditionNode.m_effectPartList[j];
				if (typePart == effectPart.m_effectPartType && effectPart.m_effectAccumulateType == _EffectAccumulateType.Absolute && (conditionNode.m_iCondType == _ConditionType.Debuff || conditionNode.m_iCondType == _ConditionType.InstantDebuff))
				{
					result = true;
					break;
				}
			}
		}
		if (this.GetUnitAbilityAbsoluteDebuff(typePart, currentAbility))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06002EFE RID: 12030 RVA: 0x0001E249 File Offset: 0x0001C449
	public bool GetUnitTileAbsoluteDebuff(_EffectPartType typePart, UnitAbility uAb)
	{
		return this.GetEffectPartAbsoluteDebuff(typePart, uAb) || (this.occupiedTile != null && this.occupiedTile.GetEffectPartAbsoluteDebuff(typePart, this));
	}

	// Token: 0x06002EFF RID: 12031 RVA: 0x0016A938 File Offset: 0x00168B38
	public float GetConditionInstantValue(_EffectPartType typePart, int ConditionID, int iRange)
	{
		float num = 0f;
		ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(ConditionID);
		if (conditionNode == null)
		{
			Debug.LogError(ConditionID.ToString() + " 狀態找不到");
			return num;
		}
		if (conditionNode.m_iCondType != _ConditionType.InstantBuff && conditionNode.m_iCondType != _ConditionType.InstantDebuff)
		{
			return num;
		}
		for (int i = 0; i < conditionNode.m_effectPartList.Count; i++)
		{
			EffectPart effectPart = conditionNode.m_effectPartList[i];
			if (effectPart.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy && effectPart.m_effectAccumulateType != _EffectAccumulateType.AfterAction && effectPart.m_effectAccumulateType != _EffectAccumulateType.Compare && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
			{
				if (typePart == effectPart.m_effectPartType && !effectPart.m_bPercent)
				{
					this.CalculateEffectPart(effectPart, 0);
					if (conditionNode.m_iCondType == _ConditionType.InstantBuff)
					{
						num += (float)effectPart.m_iValueSum;
					}
					else if (conditionNode.m_iCondType == _ConditionType.InstantDebuff)
					{
						num -= (float)effectPart.m_iValueSum;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002F00 RID: 12032 RVA: 0x0016AA54 File Offset: 0x00168C54
	public float GetUnitAbilityValue(_EffectPartType typePart, UnitAbility uAb, int iRange, bool bIncludeEquip = true)
	{
		float num = 0f;
		List<int> list = new List<int>();
		if (uAb != null && uAb.chainedAbilityIDList.Count > 0)
		{
			list.AddRange(uAb.chainedAbilityIDList.ToArray());
		}
		if (bIncludeEquip)
		{
			list.AddRange(this.GetEquipCondition().ToArray());
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != 0)
			{
				num += this.GetConditionInstantValue(typePart, list[i], iRange);
			}
		}
		return num;
	}

	// Token: 0x06002F01 RID: 12033 RVA: 0x0016AAE8 File Offset: 0x00168CE8
	public float GetEffectPartValue(_EffectPartType typePart, UnitAbility currentAbility, int iRange)
	{
		float num = 0f;
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, currentAbility))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (effectPart.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy && effectPart.m_effectAccumulateType != _EffectAccumulateType.AfterAction && effectPart.m_effectAccumulateType != _EffectAccumulateType.Compare && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
					{
						if (typePart == effectPart.m_effectPartType && !effectPart.m_bPercent)
						{
							num += (float)effectPart.m_iValueSum;
						}
					}
				}
			}
			if (num != 0f)
			{
				num = num * (float)this.iNeigongLv * 0.1f;
			}
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (effectPart2.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart2.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy && effectPart2.m_effectAccumulateType != _EffectAccumulateType.AfterAction && effectPart2.m_effectAccumulateType != _EffectAccumulateType.Compare && effectPart2.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
				{
					if (conditionNode2.m_iCondType != _ConditionType.StackBuff && conditionNode2.m_iCondType != _ConditionType.StackDebuff)
					{
						if (typePart == effectPart2.m_effectPartType && !effectPart2.m_bPercent)
						{
							if (conditionNode2.m_iCondType == _ConditionType.Buff || conditionNode2.m_iCondType == _ConditionType.InstantBuff)
							{
								num += (float)effectPart2.m_iValueSum;
							}
							else if (conditionNode2.m_iCondType == _ConditionType.Debuff || conditionNode2.m_iCondType == _ConditionType.InstantDebuff)
							{
								num -= (float)effectPart2.m_iValueSum;
							}
						}
					}
				}
			}
		}
		return num + this.GetUnitAbilityValue(typePart, currentAbility, iRange, true);
	}

	// Token: 0x06002F02 RID: 12034 RVA: 0x0016AD2C File Offset: 0x00168F2C
	public float GetConditionInstantValuePercent(_EffectPartType typePart, int ConditionID, int iRange)
	{
		float num = 0f;
		ConditionNode conditionNode = Game.g_BattleControl.m_battleAbility.GetConditionNode(ConditionID);
		if (conditionNode == null)
		{
			Debug.LogError(ConditionID.ToString() + " 狀態找不到");
			return num;
		}
		if (conditionNode.m_iCondType != _ConditionType.InstantBuff && conditionNode.m_iCondType != _ConditionType.InstantDebuff)
		{
			return num;
		}
		for (int i = 0; i < conditionNode.m_effectPartList.Count; i++)
		{
			EffectPart effectPart = conditionNode.m_effectPartList[i];
			if (effectPart.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy && effectPart.m_effectAccumulateType != _EffectAccumulateType.AfterAction && effectPart.m_effectAccumulateType != _EffectAccumulateType.Compare && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
			{
				if (typePart == effectPart.m_effectPartType && effectPart.m_bPercent)
				{
					this.CalculateEffectPart(effectPart, 0);
					if (conditionNode.m_iCondType == _ConditionType.InstantBuff)
					{
						num += (float)effectPart.m_iValueSum;
					}
					else if (conditionNode.m_iCondType == _ConditionType.InstantDebuff)
					{
						num -= (float)effectPart.m_iValueSum;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x0016AE48 File Offset: 0x00169048
	public float GetUnitAbilityValuePercent(_EffectPartType typePart, UnitAbility uAb, int iRange, bool bIncludeEquip = true)
	{
		float num = 0f;
		List<int> list = new List<int>();
		if (uAb != null && uAb.chainedAbilityIDList.Count > 0)
		{
			list.AddRange(uAb.chainedAbilityIDList.ToArray());
		}
		if (bIncludeEquip)
		{
			list.AddRange(this.GetEquipCondition().ToArray());
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != 0)
			{
				num += this.GetConditionInstantValuePercent(typePart, list[i], iRange);
			}
		}
		return num;
	}

	// Token: 0x06002F04 RID: 12036 RVA: 0x0016AEDC File Offset: 0x001690DC
	public float GetEffectPartValuePercent(_EffectPartType typePart, UnitAbility currentAbility, int iRange)
	{
		float num = 0f;
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, currentAbility))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (effectPart.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy && effectPart.m_effectAccumulateType != _EffectAccumulateType.AfterAction && effectPart.m_effectAccumulateType != _EffectAccumulateType.Compare && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
					{
						if (typePart == effectPart.m_effectPartType && effectPart.m_bPercent)
						{
							num += (float)effectPart.m_iValueSum;
						}
					}
				}
			}
			if (num != 0f)
			{
				num = num * (float)this.iNeigongLv * 0.1f;
			}
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (effectPart2.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart2.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy && effectPart2.m_effectAccumulateType != _EffectAccumulateType.AfterAction && effectPart2.m_effectAccumulateType != _EffectAccumulateType.Compare && effectPart2.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
				{
					if (conditionNode2.m_iCondType != _ConditionType.StackBuff && conditionNode2.m_iCondType != _ConditionType.StackDebuff)
					{
						if (typePart == effectPart2.m_effectPartType && effectPart2.m_bPercent)
						{
							if (conditionNode2.m_iCondType == _ConditionType.Buff || conditionNode2.m_iCondType == _ConditionType.InstantBuff)
							{
								num += (float)effectPart2.m_iValueSum;
							}
							else if (conditionNode2.m_iCondType == _ConditionType.Debuff || conditionNode2.m_iCondType == _ConditionType.InstantDebuff)
							{
								num -= (float)effectPart2.m_iValueSum;
							}
						}
					}
				}
			}
		}
		return num + this.GetUnitAbilityValuePercent(typePart, currentAbility, iRange, true);
	}

	// Token: 0x06002F05 RID: 12037 RVA: 0x0016B120 File Offset: 0x00169320
	public int GetEffectPartAuraRange(UnitAbility currentAbility)
	{
		int num = 0;
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, currentAbility))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if ((effectPart.m_effectAccumulateType == _EffectAccumulateType.Auras || effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasAll || effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy) && num < effectPart.m_iValueLimit && num < effectPart.m_iValueLimit)
					{
						num = effectPart.m_iValueLimit;
					}
				}
			}
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if ((effectPart2.m_effectAccumulateType == _EffectAccumulateType.Auras || effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasAll || effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy) && num < effectPart2.m_iValueLimit && num < effectPart2.m_iValueLimit)
				{
					num = effectPart2.m_iValueLimit;
				}
			}
		}
		if (currentAbility != null)
		{
			for (int m = 0; m < currentAbility.chainedAbilityIDList.Count; m++)
			{
				if (currentAbility.chainedAbilityIDList[m] != 0)
				{
					ConditionNode conditionNode3 = Game.g_BattleControl.m_battleAbility.GetConditionNode(currentAbility.chainedAbilityIDList[m]);
					if (conditionNode3 == null)
					{
						Debug.LogError(currentAbility.chainedAbilityIDList[m].ToString() + " 狀態找不到");
					}
					else if (conditionNode3.m_iCondType == _ConditionType.InstantBuff || conditionNode3.m_iCondType == _ConditionType.InstantDebuff)
					{
						for (int n = 0; n < conditionNode3.m_effectPartList.Count; n++)
						{
							EffectPart effectPart3 = conditionNode3.m_effectPartList[n];
							if ((effectPart3.m_effectAccumulateType == _EffectAccumulateType.Auras || effectPart3.m_effectAccumulateType == _EffectAccumulateType.AurasAll || effectPart3.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy) && num < effectPart3.m_iValueLimit && num < effectPart3.m_iValueLimit)
							{
								num = effectPart3.m_iValueLimit;
							}
						}
					}
				}
			}
		}
		int talentAreaRange = this.GetTalentAreaRange();
		if (num < talentAreaRange)
		{
			num = talentAreaRange;
		}
		return num;
	}

	// Token: 0x06002F06 RID: 12038 RVA: 0x0016B3CC File Offset: 0x001695CC
	public bool GetEffectPartAurasAbsoluteBuff(_EffectPartType typePart, int iRange, int iTileUnitFactionID)
	{
		bool flag = this.CheckFriendFaction(iTileUnitFactionID);
		bool flag2 = false;
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, null))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (this.CheckAurasAbsoluteBuff(effectPart))
					{
						if (typePart == effectPart.m_effectPartType)
						{
							if (effectPart.m_iValueLimit >= iRange)
							{
								if (flag && effectPart.m_effectAccumulateType == _EffectAccumulateType.Auras)
								{
									flag2 = true;
									break;
								}
								if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
								{
									flag2 = true;
									break;
								}
							}
						}
					}
				}
				if (flag2)
				{
					break;
				}
			}
		}
		if (flag2)
		{
			return flag2;
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (this.CheckAurasAbsoluteBuff(effectPart2))
				{
					if (typePart == effectPart2.m_effectPartType)
					{
						if (effectPart2.m_iValueLimit >= iRange)
						{
							if (flag && effectPart2.m_effectAccumulateType == _EffectAccumulateType.Auras)
							{
								flag2 = true;
								break;
							}
							if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
							{
								flag2 = true;
								break;
							}
						}
					}
				}
			}
			if (flag2)
			{
				break;
			}
		}
		return flag2;
	}

	// Token: 0x06002F07 RID: 12039 RVA: 0x0016B59C File Offset: 0x0016979C
	public bool GetEffectPartAurasAbsoluteDebuff(_EffectPartType typePart, int iRange, int iTileUnitFactionID)
	{
		bool flag = this.CheckFriendFaction(iTileUnitFactionID);
		bool flag2 = false;
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, null))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (this.CheckAurasAbsoluteDebuff(effectPart))
					{
						if (typePart == effectPart.m_effectPartType)
						{
							if (effectPart.m_iValueLimit >= iRange)
							{
								if (!flag && effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy)
								{
									flag2 = true;
									break;
								}
								if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
								{
									flag2 = true;
									break;
								}
							}
						}
					}
				}
				if (flag2)
				{
					break;
				}
			}
		}
		if (flag2)
		{
			return flag2;
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (this.CheckAurasAbsoluteDebuff(effectPart2))
				{
					if (typePart == effectPart2.m_effectPartType)
					{
						if (effectPart2.m_iValueLimit >= iRange)
						{
							if (!flag && effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy)
							{
								flag2 = true;
								break;
							}
							if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
							{
								flag2 = true;
								break;
							}
						}
					}
				}
			}
			if (flag2)
			{
				break;
			}
		}
		return flag2;
	}

	// Token: 0x06002F08 RID: 12040 RVA: 0x0016B770 File Offset: 0x00169970
	public float GetEffectPartAurasValue(_EffectPartType typePart, int iRange, int iTileUnitFactionID)
	{
		bool flag = this.CheckFriendFaction(iTileUnitFactionID);
		float num = 0f;
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, null))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (!effectPart.m_bPercent)
					{
						if (typePart == effectPart.m_effectPartType)
						{
							if (!this.CheckAurasAbsoluteBuff(effectPart))
							{
								if (!this.CheckAurasAbsoluteDebuff(effectPart))
								{
									if (effectPart.m_iValueLimit >= iRange)
									{
										if (effectPart.m_effectAccumulateType == _EffectAccumulateType.Auras && flag)
										{
											this.CalculateEffectPart(effectPart, iRange);
											num += (float)effectPart.m_iValueSum;
										}
										if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy && !flag)
										{
											this.CalculateEffectPart(effectPart, iRange);
											num -= (float)effectPart.m_iValueSum;
										}
										if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
										{
											this.CalculateEffectPart(effectPart, iRange);
											num += (float)effectPart.m_iValueSum;
										}
									}
								}
							}
						}
					}
				}
			}
			if (num != 0f)
			{
				num = num * (float)this.iNeigongLv * 0.1f;
			}
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (!effectPart2.m_bPercent)
				{
					if (typePart == effectPart2.m_effectPartType)
					{
						if (!this.CheckAurasAbsoluteBuff(effectPart2))
						{
							if (!this.CheckAurasAbsoluteDebuff(effectPart2))
							{
								if (effectPart2.m_iValueLimit >= iRange)
								{
									if (conditionNode2.m_iCondType != _ConditionType.StackBuff && conditionNode2.m_iCondType != _ConditionType.StackDebuff)
									{
										this.CalculateEffectPart(effectPart2, iRange);
										if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.Auras && flag)
										{
											num += (float)effectPart2.m_iValueSum;
										}
										if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy && !flag)
										{
											num -= (float)effectPart2.m_iValueSum;
										}
										if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
										{
											if (conditionNode2.m_iCondType == _ConditionType.Buff || conditionNode2.m_iCondType == _ConditionType.InstantBuff || conditionNode2.m_iCondType == _ConditionType.StackBuff)
											{
												num += (float)effectPart2.m_iValueSum;
											}
											else
											{
												num -= (float)effectPart2.m_iValueSum;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x0016BA50 File Offset: 0x00169C50
	public float GetEffectPartAurasValuePercent(_EffectPartType typePart, int iRange, int iTileUnitFactionID)
	{
		bool flag = this.CheckFriendFaction(iTileUnitFactionID);
		float num = 0f;
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, null))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (effectPart.m_bPercent)
					{
						if (typePart == effectPart.m_effectPartType)
						{
							if (!this.CheckAurasAbsoluteBuff(effectPart))
							{
								if (!this.CheckAurasAbsoluteDebuff(effectPart))
								{
									if (effectPart.m_iValueLimit >= iRange)
									{
										if (effectPart.m_effectAccumulateType == _EffectAccumulateType.Auras && flag)
										{
											this.CalculateEffectPart(effectPart, iRange);
											num += (float)effectPart.m_iValueSum;
										}
										if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy && !flag)
										{
											this.CalculateEffectPart(effectPart, iRange);
											num -= (float)effectPart.m_iValueSum;
										}
										if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
										{
											this.CalculateEffectPart(effectPart, iRange);
											num += (float)effectPart.m_iValueSum;
										}
									}
								}
							}
						}
					}
				}
			}
			if (num != 0f)
			{
				num = num * (float)this.iNeigongLv * 0.1f;
			}
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (effectPart2.m_bPercent)
				{
					if (typePart == effectPart2.m_effectPartType)
					{
						if (!this.CheckAurasAbsoluteBuff(effectPart2))
						{
							if (!this.CheckAurasAbsoluteDebuff(effectPart2))
							{
								if (effectPart2.m_iValueLimit >= iRange)
								{
									if (conditionNode2.m_iCondType != _ConditionType.StackBuff && conditionNode2.m_iCondType != _ConditionType.StackDebuff)
									{
										this.CalculateEffectPart(effectPart2, iRange);
										if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.Auras && flag)
										{
											num += (float)effectPart2.m_iValueSum;
										}
										if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy && !flag)
										{
											num -= (float)effectPart2.m_iValueSum;
										}
										if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
										{
											if (conditionNode2.m_iCondType == _ConditionType.Buff || conditionNode2.m_iCondType == _ConditionType.InstantBuff || conditionNode2.m_iCondType == _ConditionType.StackBuff)
											{
												num += (float)effectPart2.m_iValueSum;
											}
											else
											{
												num -= (float)effectPart2.m_iValueSum;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002F0A RID: 12042 RVA: 0x0001E280 File Offset: 0x0001C480
	public bool CheckAurasAbsoluteBuff(EffectPart part)
	{
		return part.m_iValue1 == 0 && part.m_iValue2 == 0 && (part.m_effectAccumulateType == _EffectAccumulateType.Auras || part.m_effectAccumulateType == _EffectAccumulateType.AurasAll);
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x0001E2B3 File Offset: 0x0001C4B3
	public bool CheckAurasAbsoluteDebuff(EffectPart part)
	{
		return part.m_iValue1 == 0 && part.m_iValue2 == 0 && (part.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy || part.m_effectAccumulateType == _EffectAccumulateType.AurasAll);
	}

	// Token: 0x06002F0C RID: 12044 RVA: 0x0001E2E7 File Offset: 0x0001C4E7
	public bool CheckFriendFaction(int iUnitFactionID)
	{
		return (this.factionID == 0 && iUnitFactionID == 10) || (this.factionID == 10 && iUnitFactionID == 0) || iUnitFactionID == this.factionID;
	}

	// Token: 0x06002F0D RID: 12045 RVA: 0x0016BD30 File Offset: 0x00169F30
	public bool CheckDblAttack(UnitAbility uab)
	{
		List<Tile> list = this.FilterTileByAbilityTargetType(this.lastAttackTileList, uab);
		if (list.Count <= 0)
		{
			return false;
		}
		float num = this.GetEffectPartValuePercent(_EffectPartType.DoubleAttackChance, uab, 0) + this.occupiedTile.GetEffectPartValuePercent(_EffectPartType.DoubleAttackChance, this);
		if (this.GetUnitTileAbsoluteDebuff(_EffectPartType.DoubleAttackChance, uab))
		{
			num = 0f;
		}
		if (this.GetUnitTileAbsoluteBuff(_EffectPartType.DoubleAttackChance, uab))
		{
			num = 100f;
		}
		num -= 1f * (float)(this.iAttackPlus * this.iAttackPlusDownDouble + this.iDeadPlus * this.iDeadPlusDownDouble);
		num = Mathf.Clamp(num * 0.01f, 0f, 1f);
		if (Random.Range(0f, 1f) < num)
		{
			List<string> effectString = this.GetEffectString(_EffectPartType.DoubleAttackChance);
			if (effectString.Count > 0)
			{
				string msg = string.Format(Game.StringTable.GetString(260030), effectString[0], Game.StringTable.GetString(262014));
				UINGUI.BattleMessage(msg);
				new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), Game.StringTable.GetString(262014), _OverlayType.Talent, 0f);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002F0E RID: 12046 RVA: 0x0016BE78 File Offset: 0x0016A078
	private void ApplyEquipCondition()
	{
		if (this.characterData == null)
		{
			return;
		}
		if (this.characterData._EquipWeapon.iConditionList.Count > 0)
		{
			ItemDataNode itemDataNode = Game.ItemData.GetItemDataNode(this.characterData.iEquipWeaponID);
			foreach (int id in this.characterData._EquipWeapon.iConditionList)
			{
				this.ApplyConditionID(id, itemDataNode.m_strItemName, false);
			}
		}
		if (this.characterData._EquipArror.iConditionList.Count > 0)
		{
			ItemDataNode itemDataNode2 = Game.ItemData.GetItemDataNode(this.characterData.iEquipArrorID);
			foreach (int id2 in this.characterData._EquipArror.iConditionList)
			{
				this.ApplyConditionID(id2, itemDataNode2.m_strItemName, false);
			}
		}
		if (this.characterData._EquipNecklace.iConditionList.Count > 0)
		{
			ItemDataNode itemDataNode3 = Game.ItemData.GetItemDataNode(this.characterData.iEquipNecklaceID);
			foreach (int id3 in this.characterData._EquipNecklace.iConditionList)
			{
				this.ApplyConditionID(id3, itemDataNode3.m_strItemName, false);
			}
		}
	}

	// Token: 0x06002F0F RID: 12047 RVA: 0x0016C040 File Offset: 0x0016A240
	public bool IsEquipYagu()
	{
		return this.characterData.iEquipWeaponID == 100936 || this.characterData.iEquipArrorID == 100936 || this.characterData.iEquipNecklaceID == 100936;
	}

	// Token: 0x06002F10 RID: 12048 RVA: 0x0016C090 File Offset: 0x0016A290
	public List<int> GetEquipCondition()
	{
		List<int> list = new List<int>();
		if (this.characterData == null)
		{
			return list;
		}
		if (this.characterData._EquipWeapon.iConditionList.Count > 0)
		{
			list.AddRange(this.characterData._EquipWeapon.iConditionList.ToArray());
		}
		if (this.characterData._EquipArror.iConditionList.Count > 0)
		{
			list.AddRange(this.characterData._EquipArror.iConditionList.ToArray());
		}
		if (this.characterData._EquipNecklace.iConditionList.Count > 0)
		{
			list.AddRange(this.characterData._EquipNecklace.iConditionList.ToArray());
		}
		return list;
	}

	// Token: 0x06002F11 RID: 12049 RVA: 0x0016C154 File Offset: 0x0016A354
	public float GetRecoveHPMPValue(int iDiv)
	{
		float num;
		if (GameGlobal.m_bDLCMode)
		{
			num = 20f;
		}
		else
		{
			num = 10f;
		}
		float num2 = num / (float)(iDiv + this.iBeStealCount);
		float num3 = 0f;
		float num4 = 1f + this.CheckTalentSelfAndTeamPlus(TalentEffect.RecoverHP, true, false);
		num2 *= num4;
		float num5 = (float)this.HP;
		num5 /= (float)this.GetFullHP();
		if (num5 > 0.5f)
		{
			num5 = 0f;
		}
		else
		{
			num5 = 1f - num5;
		}
		float num6 = (float)this.GetFullHP() * num2 * 0.01f;
		num3 += Mathf.Min((float)this.GetFullHP() - (float)this.HP, num6) * num5;
		num2 = 40f / (float)iDiv;
		num4 = 1f + this.CheckTalentSelfAndTeamPlus(TalentEffect.RecoverSP, true, false);
		num2 *= num4;
		num5 = (float)this.SP;
		num5 /= (float)this.GetFullSP();
		if (num5 > 0.5f)
		{
			num5 = 0f;
		}
		else
		{
			num5 = 1f - num5;
		}
		num6 = (float)this.GetFullSP() * num2 * 0.01f;
		return num3 + Mathf.Min((float)this.GetFullSP() - (float)this.SP, num6) * num5;
	}

	// Token: 0x06002F12 RID: 12050 RVA: 0x0016C290 File Offset: 0x0016A490
	public void RecoveHPMP(int iDiv)
	{
		if (iDiv <= 0)
		{
			iDiv = 1;
		}
		EffectPart effectPart = new EffectPart();
		effectPart.m_effectAccumulateType = _EffectAccumulateType.None;
		effectPart.m_bPercent = true;
		float num = 1f + this.CheckTalentSelfAndTeamPlus(TalentEffect.RecoverHP, true, false);
		effectPart.m_effectPartType = _EffectPartType.HitPoint;
		float num2;
		if (GameGlobal.m_bDLCMode)
		{
			num2 = 20f;
		}
		else
		{
			num2 = 10f;
		}
		num2 /= (float)(iDiv + this.iBeStealCount);
		effectPart.m_iValueSum = Mathf.RoundToInt(num * num2);
		this.DirectEffectPart("休息", effectPart, _ConditionType.Buff, 10);
		num = 1f + this.CheckTalentSelfAndTeamPlus(TalentEffect.RecoverSP, true, true);
		effectPart.m_effectPartType = _EffectPartType.InternalForce;
		num2 = 40f;
		num2 /= (float)iDiv;
		effectPart.m_iValueSum = Mathf.RoundToInt(num * num2);
		this.DirectEffectPart("休息", effectPart, _ConditionType.Buff, 10);
		this.CheckTalentStealth();
		this.attacked = true;
		this.moved = true;
	}

	// Token: 0x06002F13 RID: 12051 RVA: 0x0016C374 File Offset: 0x0016A574
	public void RestAndRecoveHPMP()
	{
		this.bNightFragrance = false;
		UnitControl.MoveUnit(this);
		if (!this.attacked)
		{
			if (this.moved)
			{
				this.RecoveHPMP(2);
			}
			else
			{
				this.RecoveHPMP(1);
			}
		}
		if (UnitTB.onTurnDepletedE != null)
		{
			UnitTB.onTurnDepletedE();
		}
	}

	// Token: 0x06002F14 RID: 12052 RVA: 0x0016C3CC File Offset: 0x0016A5CC
	public void ChangeNeigong(int iNeigongNewID)
	{
		NpcNeigong npcNeigong = this.characterData.SetNowUseNeigong(iNeigongNewID);
		if (npcNeigong == null)
		{
			this.iNeigongLv = 0;
			this.unitNeigong = null;
			return;
		}
		this.iNeigongLv = npcNeigong.iLevel;
		this.unitNeigong = npcNeigong.m_Neigong;
		this.InitNeigong();
		this.attackRemain--;
		if (this.attackRemain <= 0)
		{
			this.moved = true;
			this.attacked = true;
		}
		UnitControl.MoveUnit(this);
		if (UnitTB.onTurnDepletedE != null)
		{
			UnitTB.onTurnDepletedE();
		}
	}

	// Token: 0x06002F15 RID: 12053 RVA: 0x0000264F File Offset: 0x0000084F
	public void ChangeRoutine(int iRoutineNewID)
	{
	}

	// Token: 0x06002F16 RID: 12054 RVA: 0x0016C45C File Offset: 0x0016A65C
	private IEnumerator ChangeModel(string strModelName)
	{
		yield return null;
		this.animationTB.ClearModel();
		GameObject goPrefab = Game.g_ModelBundle.Load(strModelName + "_ModelPrefab") as GameObject;
		GameObject goModel = (GameObject)Object.Instantiate(goPrefab);
		goModel.name = goPrefab.name;
		MeleeWeaponTrail[] mwtArray = goModel.GetComponents<MeleeWeaponTrail>();
		foreach (MeleeWeaponTrail mwt in mwtArray)
		{
			mwt.Emit = false;
		}
		mwtArray = goModel.GetComponentsInChildren<MeleeWeaponTrail>();
		foreach (MeleeWeaponTrail mwt2 in mwtArray)
		{
			mwt2.Emit = false;
		}
		goModel.transform.parent = base.gameObject.transform;
		goModel.transform.localPosition = new Vector3(0f, 0f, 0f);
		goModel.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		if (goModel.GetComponent<NpcCollider>())
		{
			goModel.GetComponent<NpcCollider>().m_strModelName = strModelName;
			goModel.GetComponent<NpcCollider>().enabled = false;
		}
		if (goModel.GetComponent<PlayerController>())
		{
			goModel.GetComponent<PlayerController>().m_strModelName = strModelName;
			goModel.GetComponent<PlayerController>().enabled = false;
		}
		if (goModel.GetComponent<NavMeshAgent>())
		{
			goModel.GetComponent<NavMeshAgent>().enabled = false;
		}
		if (goModel.GetComponent<AudioListener>())
		{
			goModel.GetComponent<AudioListener>().enabled = false;
		}
		this.animationTB.SetAnimation(goModel);
		this.animationTB.InitAnimation();
		this.animationTB.StopMove();
		yield break;
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x0016C488 File Offset: 0x0016A688
	public void ClearAuras(Tile tile)
	{
		int effectPartAuraRange = this.GetEffectPartAuraRange(null);
		if (effectPartAuraRange > 0)
		{
			List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(tile, effectPartAuraRange);
			foreach (Tile tile2 in tilesWithinRange)
			{
				tile2.RemoveAurasUnit(this);
			}
		}
	}

	// Token: 0x06002F18 RID: 12056 RVA: 0x0016C4F4 File Offset: 0x0016A6F4
	public void SetAuras(Tile tile)
	{
		int effectPartAuraRange = this.GetEffectPartAuraRange(null);
		if (effectPartAuraRange > 0 && !this.IsDestroyed())
		{
			List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(tile, effectPartAuraRange);
			foreach (Tile tile2 in tilesWithinRange)
			{
				tile2.AddAurasUnit(this);
			}
		}
	}

	// Token: 0x06002F19 RID: 12057 RVA: 0x0016C56C File Offset: 0x0016A76C
	public void UpdateAuras()
	{
		if (this.occupiedTile == null)
		{
			return;
		}
		int effectPartAuraRange = this.GetEffectPartAuraRange(null);
		if (effectPartAuraRange != this.iNowAuraRange)
		{
			if (this.iNowAuraRange != 0)
			{
				List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(this.occupiedTile, this.iNowAuraRange);
				for (int i = 0; i < tilesWithinRange.Count; i++)
				{
					Tile tile = tilesWithinRange[i];
					if (!(tile == null))
					{
						tile.RemoveAurasUnit(this);
					}
				}
			}
			if (effectPartAuraRange != 0)
			{
				List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(this.occupiedTile, effectPartAuraRange);
				for (int j = 0; j < tilesWithinRange.Count; j++)
				{
					Tile tile2 = tilesWithinRange[j];
					if (!(tile2 == null))
					{
						tile2.AddAurasUnit(this);
					}
				}
			}
			this.iNowAuraRange = effectPartAuraRange;
		}
	}

	// Token: 0x06002F1A RID: 12058 RVA: 0x0016C64C File Offset: 0x0016A84C
	public List<Tile> GetAttackAbleTiles(Tile srcTile)
	{
		List<Tile> list = new List<Tile>();
		for (int i = 0; i < this.unitAbilityList.Count; i++)
		{
			if (this.IsAbilityAvailable(i) == 0)
			{
				List<Tile> abilityAttackAbleTiles = this.GetAbilityAttackAbleTiles(srcTile, this.unitAbilityList[i]);
				foreach (Tile tile in abilityAttackAbleTiles)
				{
					if (!list.Contains(tile))
					{
						list.Add(tile);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06002F1B RID: 12059 RVA: 0x0016C6F8 File Offset: 0x0016A8F8
	public List<Tile> GetAbilityAttackAbleTiles(Tile srcTile, UnitAbility uab)
	{
		List<Tile> list = new List<Tile>();
		if (uab.targetArea == _TargetArea.Line)
		{
			foreach (Tile targetTile in srcTile.neighbours)
			{
				list.AddRange(GridManager.GetTileInLine(srcTile, targetTile, uab.range));
			}
		}
		else if (uab.targetArea == _TargetArea.Default)
		{
			if (uab.requireTargetSelection)
			{
				list.AddRange(GridManager.GetTilesWithinRange(srcTile, uab.range + uab.aoeRange));
			}
			else
			{
				list.AddRange(GridManager.GetTilesWithinRange(srcTile, uab.aoeRange));
			}
		}
		else
		{
			list.AddRange(GridManager.GetTilesWithinRange(srcTile, uab.range));
		}
		return list;
	}

	// Token: 0x06002F1C RID: 12060 RVA: 0x0016C7D4 File Offset: 0x0016A9D4
	public List<string> GetEffectString(_EffectPartType typePart)
	{
		List<string> list = new List<string>();
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, null))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (effectPart.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
					{
						if (effectPart.m_effectAccumulateType != _EffectAccumulateType.AfterAction)
						{
							if (effectPart.m_effectAccumulateType != _EffectAccumulateType.Compare)
							{
								if (typePart == effectPart.m_effectPartType)
								{
									list.Add(this.unitNeigong.m_strNeigongName + "-" + conditionNode.m_strName);
								}
							}
						}
					}
				}
			}
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (effectPart2.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart2.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy && effectPart2.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
				{
					if (effectPart2.m_effectAccumulateType != _EffectAccumulateType.AfterAction)
					{
						if (effectPart2.m_effectAccumulateType != _EffectAccumulateType.Compare)
						{
							if (typePart == effectPart2.m_effectPartType)
							{
								list.Add(conditionNode2.m_strName);
							}
						}
					}
				}
			}
		}
		if (this.occupiedTile != null)
		{
			list.AddRange(this.occupiedTile.GetEffectString(typePart));
		}
		return list;
	}

	// Token: 0x06002F1D RID: 12061 RVA: 0x0016C9C4 File Offset: 0x0016ABC4
	public List<string> GetEffectValueString(_EffectPartType typePart)
	{
		List<string> list = new List<string>();
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, null))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (effectPart.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy && effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
					{
						if (effectPart.m_effectAccumulateType != _EffectAccumulateType.AfterAction)
						{
							if (effectPart.m_effectAccumulateType != _EffectAccumulateType.Compare)
							{
								if (typePart == effectPart.m_effectPartType)
								{
									string text;
									if (effectPart.m_effectAccumulateType == _EffectAccumulateType.Absolute)
									{
										text = "[FFC880]" + this.GetEffectAbsoluteStringBuff(typePart);
									}
									else
									{
										float num = (float)effectPart.m_iValueSum;
										num = num * (float)this.iNeigongLv * 0.1f;
										if (effectPart.m_bPercent)
										{
											text = "[00FF00]+" + num.ToString() + "%[-]";
										}
										else
										{
											text = "[00FF00]+" + num.ToString() + "[-]";
										}
									}
									list.Add(text);
								}
							}
						}
					}
				}
			}
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (effectPart2.m_effectAccumulateType != _EffectAccumulateType.Auras && effectPart2.m_effectAccumulateType != _EffectAccumulateType.AurasEnemy && effectPart2.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
				{
					if (effectPart2.m_effectAccumulateType != _EffectAccumulateType.AfterAction)
					{
						if (effectPart2.m_effectAccumulateType != _EffectAccumulateType.Compare)
						{
							if (typePart == effectPart2.m_effectPartType)
							{
								string text;
								if (conditionNode2.m_iCondType == _ConditionType.Buff || conditionNode2.m_iCondType == _ConditionType.InstantBuff || conditionNode2.m_iCondType == _ConditionType.StackBuff)
								{
									if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.Absolute)
									{
										text = "[FFC880]" + this.GetEffectAbsoluteStringBuff(typePart);
									}
									else
									{
										text = "[00FF00]+" + effectPart2.m_iValueSum.ToString();
									}
								}
								else if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.Absolute)
								{
									text = "[FF4040]" + this.GetEffectAbsoluteStringDebuff(typePart);
								}
								else
								{
									text = "[FF0000]-" + effectPart2.m_iValueSum.ToString();
								}
								if (effectPart2.m_bPercent && effectPart2.m_effectAccumulateType != _EffectAccumulateType.Absolute)
								{
									text += "%";
								}
								text += "[-]";
								list.Add(text);
							}
						}
					}
				}
			}
		}
		if (this.occupiedTile != null)
		{
			list.AddRange(this.occupiedTile.GetEffectValue(typePart));
		}
		return list;
	}

	// Token: 0x06002F1E RID: 12062 RVA: 0x0016CCF0 File Offset: 0x0016AEF0
	private string GetEffectAbsoluteStringBuff(_EffectPartType typePart)
	{
		return Game.StringTable.GetString((int)(262000 + typePart));
	}

	// Token: 0x06002F1F RID: 12063 RVA: 0x0016CD10 File Offset: 0x0016AF10
	private string GetEffectAbsoluteStringDebuff(_EffectPartType typePart)
	{
		return Game.StringTable.GetString((int)(263000 + typePart));
	}

	// Token: 0x06002F20 RID: 12064 RVA: 0x0016CD30 File Offset: 0x0016AF30
	public List<string> GetAuraEffectName(_EffectPartType typePart, int iRange, int iTileUnitFactionID)
	{
		bool flag = this.CheckFriendFaction(iTileUnitFactionID);
		List<string> list = new List<string>();
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, null))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (typePart == effectPart.m_effectPartType)
					{
						if (effectPart.m_iValueLimit >= iRange)
						{
							if (effectPart.m_effectAccumulateType == _EffectAccumulateType.Auras && flag)
							{
								list.Add(string.Concat(new string[]
								{
									this.unitName,
									":",
									this.unitNeigong.m_strNeigongName,
									"-",
									conditionNode.m_strName
								}));
							}
							if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy && !flag)
							{
								list.Add(string.Concat(new string[]
								{
									this.unitName,
									":",
									this.unitNeigong.m_strNeigongName,
									"-",
									conditionNode.m_strName
								}));
							}
							if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
							{
								list.Add(string.Concat(new string[]
								{
									this.unitName,
									":",
									this.unitNeigong.m_strNeigongName,
									"-",
									conditionNode.m_strName
								}));
							}
						}
					}
				}
			}
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (typePart == effectPart2.m_effectPartType)
				{
					if (effectPart2.m_iValueLimit >= iRange)
					{
						if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.Auras && flag)
						{
							list.Add(this.unitName + ":" + conditionNode2.m_strName);
						}
						if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy && !flag)
						{
							list.Add(this.unitName + ":" + conditionNode2.m_strName);
						}
						if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
						{
							list.Add(this.unitName + ":" + conditionNode2.m_strName);
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06002F21 RID: 12065 RVA: 0x0016CFE8 File Offset: 0x0016B1E8
	public List<string> GetAuraEffectValue(_EffectPartType typePart, int iRange, int iTileUnitFactionID)
	{
		bool flag = this.CheckFriendFaction(iTileUnitFactionID);
		string text = string.Empty;
		List<string> list = new List<string>();
		if (this.unitNeigong != null && !this.GetEffectPartAbsoluteDebuff(_EffectPartType.Neigong, null))
		{
			for (int i = 0; i < this.unitNeigongConditionList.Count; i++)
			{
				ConditionNode conditionNode = this.unitNeigongConditionList[i];
				for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
				{
					EffectPart effectPart = conditionNode.m_effectPartList[j];
					if (typePart == effectPart.m_effectPartType)
					{
						if (effectPart.m_effectAccumulateType == _EffectAccumulateType.Auras || effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy || effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
						{
							if (effectPart.m_iValueLimit >= iRange)
							{
								if (this.CheckAurasAbsoluteBuff(effectPart))
								{
									text = "[FFC880]" + this.GetEffectAbsoluteStringBuff(typePart);
								}
								else if (this.CheckAurasAbsoluteDebuff(effectPart))
								{
									text = "[FF4040]" + this.GetEffectAbsoluteStringDebuff(typePart);
								}
								else if (effectPart.m_effectAccumulateType == _EffectAccumulateType.Auras && flag)
								{
									this.CalculateEffectPart(effectPart, iRange);
									text = "[00FF00]+" + ((float)this.iNeigongLv * 0.1f * (float)effectPart.m_iValueSum).ToString();
									if (effectPart.m_bPercent)
									{
										text += "%";
									}
								}
								else if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy && !flag)
								{
									this.CalculateEffectPart(effectPart, iRange);
									text = "[FF0000]-" + ((float)this.iNeigongLv * 0.1f * (float)effectPart.m_iValueSum).ToString();
									if (effectPart.m_bPercent)
									{
										text += "%";
									}
								}
								else
								{
									if (effectPart.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
									{
										goto IL_240;
									}
									this.CalculateEffectPart(effectPart, iRange);
									float num = (float)this.iNeigongLv * 0.1f * (float)effectPart.m_iValueSum;
									if (flag)
									{
										text = "[00FF00]+" + num.ToString();
									}
									else
									{
										text = "[FF0000]-" + num.ToString();
									}
									if (effectPart.m_bPercent)
									{
										text += "%";
									}
								}
								list.Add(text);
							}
						}
					}
					IL_240:;
				}
			}
		}
		for (int k = 0; k < this.unitConditionList.Count; k++)
		{
			ConditionNode conditionNode2 = this.unitConditionList[k];
			for (int l = 0; l < conditionNode2.m_effectPartList.Count; l++)
			{
				EffectPart effectPart2 = conditionNode2.m_effectPartList[l];
				if (typePart == effectPart2.m_effectPartType)
				{
					if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.Auras || effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy || effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
					{
						if (effectPart2.m_iValueLimit >= iRange)
						{
							if (this.CheckAurasAbsoluteBuff(effectPart2))
							{
								text = "[FFC880]" + this.GetEffectAbsoluteStringBuff(typePart);
							}
							else if (this.CheckAurasAbsoluteDebuff(effectPart2))
							{
								text = "[FF4040]" + this.GetEffectAbsoluteStringDebuff(typePart);
							}
							else if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.Auras && flag)
							{
								this.CalculateEffectPart(effectPart2, iRange);
								text = "[00FF00]+" + effectPart2.m_iValueSum.ToString();
								if (effectPart2.m_bPercent)
								{
									text += "%";
								}
							}
							else if (effectPart2.m_effectAccumulateType == _EffectAccumulateType.AurasEnemy && !flag)
							{
								this.CalculateEffectPart(effectPart2, iRange);
								text = "[FF0000]-" + effectPart2.m_iValueSum.ToString();
								if (effectPart2.m_bPercent)
								{
									text += "%";
								}
							}
							else
							{
								if (effectPart2.m_effectAccumulateType != _EffectAccumulateType.AurasAll)
								{
									goto IL_44F;
								}
								this.CalculateEffectPart(effectPart2, iRange);
								if (flag)
								{
									text = "[00FF00]+" + effectPart2.m_iValueSum.ToString();
								}
								else
								{
									text = "[FF0000]-" + effectPart2.m_iValueSum.ToString();
								}
								if (effectPart2.m_bPercent)
								{
									text += "%";
								}
							}
							list.Add(text);
						}
					}
				}
				IL_44F:;
			}
		}
		return list;
	}

	// Token: 0x06002F22 RID: 12066 RVA: 0x0016D478 File Offset: 0x0016B678
	public int GetAbilityCost(UnitAbility uab, bool bOrig)
	{
		float num = 1f * (float)this.iDeadPlus + 1f + this.CheckTeamTalentPercent(TalentEffect.TeamManaCost, false, false, true) + this.CheckTeamTalentPercent(TalentEffect.EnemyManaCost, true, false, false);
		if (bOrig)
		{
			num = (float)uab.cost * 0.01f * (float)this.fullSP;
		}
		else
		{
			num = num * (float)uab.cost * 0.01f * (float)this.fullSP;
		}
		return Mathf.RoundToInt(num);
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x0016D4F0 File Offset: 0x0016B6F0
	public void CheckAI()
	{
		switch (this.aiMode)
		{
		case _AIMode.Chase:
		case _AIMode.Follow:
		case _AIMode.Protect:
			if (this.aiTarget == null)
			{
				this.aiMode = _AIMode.Threat;
			}
			break;
		case _AIMode.Guard:
			if (this.aiTile == null)
			{
				this.aiMode = _AIMode.Threat;
			}
			break;
		}
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x0016D560 File Offset: 0x0016B760
	private void CheckTalentAutoQuit()
	{
		if (this.IsDestroyed())
		{
			return;
		}
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			bool flag = false;
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.m_TalentEffect == TalentEffect.AutoQuitBattle)
						{
							int num = this.HP * 100 / this.fullHP;
							if (talentResultPart.iValue > num)
							{
								flag = true;
								string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
								UINGUI.BattleMessage(msg);
							}
						}
					}
				}
				if (flag)
				{
					new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
					this.LeaveBattle(true);
					TeamStatus.m_Instance.LessTeamMember(this.characterData.iNpcID);
					break;
				}
			}
		}
	}

	// Token: 0x06002F25 RID: 12069 RVA: 0x0016D6BC File Offset: 0x0016B8BC
	public void CheckTalentTeamMateDeadth()
	{
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			bool flag = false;
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.m_TalentEffect == TalentEffect.TeamMateRemainTactic)
						{
							flag = true;
							string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
							UINGUI.BattleMessage(msg);
							this.ApplyConditionID(talentResultPart.iValue, talentData.m_strTalentName, false);
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.TeamMateRemainBuddyJoin && this.CheckFriendFaction(0) && UnitControl.instance.playerUnitsList[0].starting.Count > 0 && Random.Range(0, 100) < talentResultPart.iValue)
						{
							flag = true;
							UnitTB unitTB = BattleControl.instance.TalentTeamMateRemainBuddyJoin(this.occupiedTile);
							string text = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
							text += string.Format(Game.StringTable.GetString(260015), "[40FF40FF]" + unitTB.unitName + "[-]");
							UINGUI.BattleMessage(text);
						}
					}
				}
				if (flag)
				{
					new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
				}
			}
		}
	}

	// Token: 0x06002F26 RID: 12070 RVA: 0x0016D8A0 File Offset: 0x0016BAA0
	private void CheckTalentDeadth(UnitTB unitSrc)
	{
		if (this.iDeadCount <= 0)
		{
			return;
		}
		this.iDeadCount--;
		this.bCheckTalentDeadthNow = true;
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			bool flag = false;
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.m_TalentEffect == TalentEffect.SelfRemainTacticAll)
						{
							flag = true;
							string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
							UINGUI.BattleMessage(msg);
							if (talentResultPart.iUpDown == 0)
							{
								this.ApplyConditionID(talentResultPart.iValue, talentData.m_strTalentName, false);
							}
							else
							{
								List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(this.occupiedTile, talentResultPart.iUpDown);
								foreach (Tile tile in tilesWithinRange)
								{
									if (!(tile.unit == null))
									{
										tile.unit.ApplyConditionID(talentResultPart.iValue, this.unitName, false);
									}
								}
							}
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.SelfRemainTacticEnemy)
						{
							flag = true;
							string msg2 = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
							UINGUI.BattleMessage(msg2);
							if (talentResultPart.iUpDown != 0)
							{
								List<Tile> tilesWithinRange2 = GridManager.GetTilesWithinRange(this.occupiedTile, talentResultPart.iUpDown);
								foreach (Tile tile2 in tilesWithinRange2)
								{
									if (!(tile2.unit == null))
									{
										if (!this.CheckFriendFaction(tile2.unit.factionID))
										{
											tile2.unit.ApplyConditionID(talentResultPart.iValue, talentData.m_strTalentName, false);
										}
									}
								}
							}
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.SelfRemainTacticFriend)
						{
							flag = true;
							string msg3 = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
							UINGUI.BattleMessage(msg3);
							if (talentResultPart.iUpDown == 0)
							{
								this.ApplyConditionID(talentResultPart.iValue, talentData.m_strTalentName, false);
							}
							else
							{
								List<Tile> tilesWithinRange3 = GridManager.GetTilesWithinRange(this.occupiedTile, talentResultPart.iUpDown);
								foreach (Tile tile3 in tilesWithinRange3)
								{
									if (!(tile3.unit == null))
									{
										if (this.CheckFriendFaction(tile3.unit.factionID))
										{
											tile3.unit.ApplyConditionID(talentResultPart.iValue, talentData.m_strTalentName, false);
										}
									}
								}
							}
						}
						if (unitSrc != null && talentResultPart.m_TalentEffect == TalentEffect.SelfRemainEyeToEye)
						{
							flag = true;
							string msg4 = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
							UINGUI.BattleMessage(msg4);
							unitSrc.ApplyConditionID(talentResultPart.iValue, talentData.m_strTalentName, false);
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.SelfRemainRendomEnemyConfuse)
						{
							flag = true;
							List<UnitTB> allHostile = UnitControl.GetAllHostile(this.factionID);
							List<UnitTB> list = new List<UnitTB>();
							if (talentResultPart.iUpDown >= allHostile.Count)
							{
								list.AddRange(allHostile.ToArray());
							}
							else
							{
								for (int k = 0; k < talentResultPart.iUpDown; k++)
								{
									int num = Random.Range(0, allHostile.Count);
									list.Add(allHostile[num]);
									allHostile.RemoveAt(num);
								}
							}
							for (int l = 0; l < list.Count; l++)
							{
								list[l].Confuse(talentResultPart.iValue, this.factionID, talentData.m_strTalentName);
								string msg5 = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName) + string.Format(Game.StringTable.GetString(260043), list[l].unitName, talentResultPart.iValue);
								UINGUI.BattleMessage(msg5);
								msg5 = string.Format(Game.StringTable.GetString(260043), list[l].unitName, talentResultPart.iValue);
								UINGUI.DisplayMessage(msg5);
							}
						}
					}
				}
				if (flag)
				{
					new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
				}
			}
		}
		this.bCheckTalentDeadthNow = false;
	}

	// Token: 0x06002F27 RID: 12071 RVA: 0x0016DE30 File Offset: 0x0016C030
	private void CheckTalentWhenAttack(UnitTB targetUnit)
	{
		if (targetUnit == null)
		{
			return;
		}
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.m_TalentEffect == TalentEffect.EatSomthing)
						{
							if (Random.Range(0, 100) >= talentResultPart.iValue)
							{
								goto IL_8A8;
							}
							ItemDataNode itemDataNode;
							if (this.CheckFriendFaction(0))
							{
								List<int> list = new List<int>();
								BackpackStatus.m_Instance.SortBattleItem();
								List<BackpackNewDataNode> sortTeamBackpack = BackpackStatus.m_Instance.GetSortTeamBackpack();
								foreach (BackpackNewDataNode backpackNewDataNode in sortTeamBackpack)
								{
									if (backpackNewDataNode._ItemDataNode.m_iItemType == 4 && backpackNewDataNode._ItemDataNode.m_iItemKind == 1)
									{
										list.Add(backpackNewDataNode._ItemDataNode.m_iItemID);
									}
								}
								if (list.Count <= 0)
								{
									goto IL_8A8;
								}
								int num = Random.Range(0, list.Count);
								itemDataNode = Game.ItemData.GetItemDataNode(list[num]);
								BackpackStatus.m_Instance.LessPackItem(itemDataNode.m_iItemID, 1, null);
							}
							else
							{
								if (this.characterData.Itemlist.Count <= 0)
								{
									goto IL_8A8;
								}
								List<int> list2 = new List<int>();
								foreach (NpcItem npcItem in this.characterData.Itemlist)
								{
									ItemDataNode itemDataNode2 = Game.ItemData.GetItemDataNode(npcItem.m_iItemID);
									if (itemDataNode2 != null)
									{
										if (itemDataNode2.m_iItemType == 4 && itemDataNode2.m_iItemKind == 1)
										{
											list2.Add(itemDataNode2.m_iItemID);
										}
									}
								}
								if (list2.Count <= 0)
								{
									goto IL_8A8;
								}
								int num2 = Random.Range(0, list2.Count);
								itemDataNode = Game.ItemData.GetItemDataNode(list2[num2]);
								this.characterData.LessNpcItem(itemDataNode.m_iItemID, 1);
							}
							this.ApplyFood(itemDataNode);
							string text = string.Format(Game.StringTable.GetString(260042), itemDataNode.m_strItemName);
							UINGUI.DisplayMessage(this.unitName + text);
							text = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName) + text;
							UINGUI.BattleMessage(text);
							new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.DrinkSomthing)
						{
							if (Random.Range(0, 100) >= talentResultPart.iValue)
							{
								goto IL_8A8;
							}
							ItemDataNode itemDataNode3;
							if (this.CheckFriendFaction(0))
							{
								List<int> list3 = new List<int>();
								BackpackStatus.m_Instance.SortBattleItem();
								List<BackpackNewDataNode> sortTeamBackpack2 = BackpackStatus.m_Instance.GetSortTeamBackpack();
								foreach (BackpackNewDataNode backpackNewDataNode2 in sortTeamBackpack2)
								{
									if (backpackNewDataNode2._ItemDataNode.m_iItemType == 4 && backpackNewDataNode2._ItemDataNode.m_iItemKind == 2)
									{
										list3.Add(backpackNewDataNode2._ItemDataNode.m_iItemID);
									}
								}
								if (list3.Count <= 0)
								{
									goto IL_8A8;
								}
								int num3 = Random.Range(0, list3.Count);
								itemDataNode3 = Game.ItemData.GetItemDataNode(list3[num3]);
								BackpackStatus.m_Instance.LessPackItem(itemDataNode3.m_iItemID, 1, null);
							}
							else
							{
								if (this.characterData.Itemlist.Count <= 0)
								{
									goto IL_8A8;
								}
								List<int> list4 = new List<int>();
								foreach (NpcItem npcItem2 in this.characterData.Itemlist)
								{
									ItemDataNode itemDataNode4 = Game.ItemData.GetItemDataNode(npcItem2.m_iItemID);
									if (itemDataNode4 != null)
									{
										if (itemDataNode4.m_iItemType == 4 && itemDataNode4.m_iItemKind == 2)
										{
											list4.Add(itemDataNode4.m_iItemID);
										}
									}
								}
								if (list4.Count <= 0)
								{
									goto IL_8A8;
								}
								int num4 = Random.Range(0, list4.Count);
								itemDataNode3 = Game.ItemData.GetItemDataNode(list4[num4]);
								this.characterData.LessNpcItem(itemDataNode3.m_iItemID, 1);
							}
							this.ApplyFood(itemDataNode3);
							string text2 = string.Format(Game.StringTable.GetString(260044), itemDataNode3.m_strItemName);
							UINGUI.DisplayMessage(this.unitName + text2);
							text2 = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName) + text2;
							UINGUI.BattleMessage(text2);
							new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.Steal)
						{
							if (!this.bSteal)
							{
								float num5 = 100f * (float)targetUnit.HP / (float)targetUnit.fullHP;
								if (num5 < (float)talentResultPart.iUpDown)
								{
									num5 = 1f - Mathf.Clamp01(num5 / (float)talentResultPart.iUpDown);
									num5 = num5 * (float)talentResultPart.iValue * 0.01f;
									if (Random.Range(0f, 1f) <= num5)
									{
										int npcItemCount = targetUnit.characterData.GetNpcItemCount();
										int num6;
										if (npcItemCount > 0)
										{
											num6 = Random.Range(0, npcItemCount + targetUnit.iBeStealMoneryRate);
										}
										else
										{
											num6 = 1;
										}
										if (num6 >= npcItemCount)
										{
											int num7;
											if (this.CheckFriendFaction(0))
											{
												if (targetUnit.characterData.iMoney > 30)
												{
													num7 = targetUnit.characterData.iMoney / Random.Range(20, 31);
												}
												else
												{
													num7 = 1;
												}
												targetUnit.characterData.LessMoney(num7);
												BackpackStatus.m_Instance.ChangeMoney(num7);
											}
											else
											{
												int money = BackpackStatus.m_Instance.GetMoney();
												if (money > 30)
												{
													num7 = money / Random.Range(20, 31);
												}
												else
												{
													num7 = 1;
												}
												this.characterData.AddMoney(num7);
												BackpackStatus.m_Instance.ChangeMoney(-num7);
											}
											string text3 = string.Format(Game.StringTable.GetString(260021), num7);
											UINGUI.DisplayMessage(text3);
											text3 = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName) + text3;
											UINGUI.BattleMessage(text3);
											targetUnit.AddThreatValue(this, num7);
										}
										else
										{
											int npcItemIndexID = targetUnit.characterData.GetNpcItemIndexID(num6);
											ItemDataNode itemDataNode5 = Game.ItemData.GetItemDataNode(npcItemIndexID);
											if (itemDataNode5 != null)
											{
												if (this.CheckFriendFaction(0))
												{
													targetUnit.characterData.LessNpcItem(npcItemIndexID, 1);
													BackpackStatus.m_Instance.AddPackItem(npcItemIndexID, 1, true);
												}
												else
												{
													targetUnit.characterData.LessNpcItem(npcItemIndexID, 1);
													this.characterData.AddNpcItem(npcItemIndexID, 1);
												}
												string text4 = string.Format(Game.StringTable.GetString(260020), itemDataNode5.m_strItemName);
												UINGUI.DisplayMessage(text4);
												text4 = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName) + text4;
												UINGUI.BattleMessage(text4);
											}
											targetUnit.AddThreatValue(this, itemDataNode5.m_iItemSell);
											targetUnit.iBeStealCount++;
											targetUnit.iBeStealMoneryRate += targetUnit.iBeStealCount;
										}
										new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
										this.bSteal = true;
									}
								}
							}
						}
					}
					IL_8A8:;
				}
			}
		}
	}

	// Token: 0x06002F28 RID: 12072 RVA: 0x0016E748 File Offset: 0x0016C948
	private void CheckTalentStealth()
	{
		if (this.characterData.CheckTalentEffect(TalentEffect.Stealth) && UnitControl.GetAllFriend(this.factionID).Count > 1 && !this.bStealth)
		{
			this.bStealth = true;
			GameObject gameObject = Resources.Load("BattleField/TalentStealth", typeof(GameObject)) as GameObject;
			this.goStealth = (Object.Instantiate(gameObject) as GameObject);
			this.goStealth.transform.parent = base.transform;
			string talentNameHaveEffect = this.characterData.GetTalentNameHaveEffect(TalentEffect.Stealth);
			string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentNameHaveEffect);
			UINGUI.BattleMessage(msg);
			new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentNameHaveEffect.ToString(), _OverlayType.Talent, 0f);
		}
	}

	// Token: 0x06002F29 RID: 12073 RVA: 0x0001E322 File Offset: 0x0001C522
	private void ReleaseStealth()
	{
		if (this.bStealth)
		{
			this.bStealth = false;
			if (this.goStealth != null)
			{
				Object.DestroyImmediate(this.goStealth);
				this.goStealth = null;
			}
		}
	}

	// Token: 0x06002F2A RID: 12074 RVA: 0x0016E838 File Offset: 0x0016CA38
	public bool CheckTalentBuddhaMercy()
	{
		if (this.characterData.CheckTalentEffect(TalentEffect.BuddhaMercy) && UnitControl.GetAllFriend(this.factionID).Count > 1)
		{
			string talentNameHaveEffect = this.characterData.GetTalentNameHaveEffect(TalentEffect.BuddhaMercy);
			int talentValue = this.characterData.GetTalentValue(TalentEffect.BuddhaMercy);
			this.ApplyConditionID(talentValue, talentNameHaveEffect, false);
			string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentNameHaveEffect);
			UINGUI.BattleMessage(msg);
			new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentNameHaveEffect.ToString(), _OverlayType.Talent, 0f);
			return true;
		}
		return false;
	}

	// Token: 0x06002F2B RID: 12075 RVA: 0x0016E8F0 File Offset: 0x0016CAF0
	public bool CheckTalentNightFragrance()
	{
		if (this.characterData.CheckTalentEffect(TalentEffect.NightFragrance))
		{
			string talentNameHaveEffect = this.characterData.GetTalentNameHaveEffect(TalentEffect.NightFragrance);
			string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentNameHaveEffect);
			UINGUI.BattleMessage(msg);
			new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentNameHaveEffect.ToString(), _OverlayType.Talent, 0f);
			this.moveRemain = this.GetMovePerTurn();
			this.moved = false;
			UnitControl.OneMoreMoveUnit(this);
			this.bNightFragrance = true;
			return true;
		}
		return false;
	}

	// Token: 0x06002F2C RID: 12076 RVA: 0x0001E359 File Offset: 0x0001C559
	public void YouAreLastOne()
	{
		this.ReleaseStealth();
	}

	// Token: 0x06002F2D RID: 12077 RVA: 0x0016E9A0 File Offset: 0x0016CBA0
	public void CheckTalentPosition(UnitTB target, UnitAbility uab, Tile origTile, bool bShowLog)
	{
		if (target == null)
		{
			return;
		}
		if (target.IsEquipYagu())
		{
			uab.chainedAbilityIDList.Add(900129);
		}
		float num = Quaternion.Angle(Quaternion.LookRotation(target.thisT.position - origTile.pos), target.thisT.rotation);
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			bool flag = false;
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.m_TalentEffect == TalentEffect.AttackFromBack && num < 31f)
						{
							flag = true;
							uab.chainedAbilityIDList.Add(talentResultPart.iValue);
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.AttackFromFace && num > 149f)
						{
							flag = true;
							uab.chainedAbilityIDList.Add(talentResultPart.iValue);
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.AttackFromSide && num > 31f && num < 149f)
						{
							flag = true;
							uab.chainedAbilityIDList.Add(talentResultPart.iValue);
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.LastOneUnit && UnitControl.GetAllFriend(this.factionID).Count <= 1)
						{
							flag = true;
							uab.chainedAbilityIDList.Add(talentResultPart.iValue);
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.FirstAction && UnitControl.IsFactionFirstAction(this.factionID))
						{
							flag = true;
							uab.chainedAbilityIDList.Add(talentResultPart.iValue);
						}
					}
				}
				if (flag && bShowLog)
				{
					UINGUI.BattleMessage(string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName));
					new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f + (float)i, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
				}
			}
		}
	}

	// Token: 0x06002F2E RID: 12078 RVA: 0x0016EBC4 File Offset: 0x0016CDC4
	public float TalentLessMoney()
	{
		float num = 0f;
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			bool flag = false;
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			int num2 = 0;
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.m_TalentEffect == TalentEffect.MoneyAttack)
						{
							if (Random.Range(0, 100) < 20)
							{
								flag = true;
								int num3 = talentResultPart.iValue * talentResultPart.iUpDown;
								int money = BackpackStatus.m_Instance.GetMoney();
								float num4 = 0.005f * (float)money;
								num2 = Mathf.RoundToInt(num4);
								num2 = Mathf.Clamp(num2, 0, num3);
								BackpackStatus.m_Instance.ChangeMoney(-num2);
								num4 = 1f * (float)num2;
								if (talentResultPart.iUpDown >= 0)
								{
									num += 0.01f * num4 / (float)talentResultPart.iUpDown;
								}
							}
						}
					}
				}
				if (flag)
				{
					string text = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
					text += string.Format(Game.StringTable.GetString(200068), num2);
					UINGUI.BattleMessage(text);
					new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
				}
			}
		}
		return num;
	}

	// Token: 0x06002F2F RID: 12079 RVA: 0x0016ED94 File Offset: 0x0016CF94
	public float GetTalentDamage(UnitAbility uab)
	{
		float num = 0f;
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.m_TalentEffect == TalentEffect.LowHPHighDamageUp)
						{
							float num2 = 1f * (float)this.HP / (float)this.fullHP;
							num2 = 1f - num2;
							num2 = Mathf.Clamp01(num2);
							num2 = num2 * (float)talentResultPart.iValue * 0.01f;
							num += num2;
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.LowSPHighDamageUp)
						{
							float num3 = 1f * (float)this.SP / (float)this.fullSP;
							num3 = 1f - num3;
							num3 = Mathf.Clamp01(num3);
							num3 = num3 * (float)talentResultPart.iValue * 0.01f;
							num += num3;
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.AroundEnemyDamageUp)
						{
							List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(this.occupiedTile, talentResultPart.iUpDown);
							int num4 = 0;
							foreach (Tile tile in tilesWithinRange)
							{
								if (!(tile.unit == null))
								{
									if (!this.CheckFriendFaction(tile.unit.factionID))
									{
										num4++;
									}
								}
							}
							num += 0.01f * (float)num4 * (float)talentResultPart.iValue;
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.AroundFriendDamageUp)
						{
							List<Tile> tilesWithinRange2 = GridManager.GetTilesWithinRange(this.occupiedTile, talentResultPart.iUpDown);
							int num5 = 0;
							foreach (Tile tile2 in tilesWithinRange2)
							{
								if (!(tile2.unit == null))
								{
									if (this.CheckFriendFaction(tile2.unit.factionID))
									{
										num5++;
									}
								}
							}
							num += 0.01f * (float)num5 * (float)talentResultPart.iValue;
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.AroundMaleDamageUp)
						{
							List<Tile> tilesWithinRange3 = GridManager.GetTilesWithinRange(this.occupiedTile, talentResultPart.iUpDown);
							int num6 = 0;
							foreach (Tile tile3 in tilesWithinRange3)
							{
								if (!(tile3.unit == null))
								{
									if (tile3.unit.characterData._NpcDataNode.m_Gender == GenderType.Male)
									{
										num6++;
									}
								}
							}
							num += 0.01f * (float)num6 * (float)talentResultPart.iValue;
						}
						if (talentResultPart.m_TalentEffect == TalentEffect.AroundFemaleDamageUp)
						{
							List<Tile> tilesWithinRange4 = GridManager.GetTilesWithinRange(this.occupiedTile, talentResultPart.iUpDown);
							int num7 = 0;
							foreach (Tile tile4 in tilesWithinRange4)
							{
								if (!(tile4.unit == null))
								{
									if (tile4.unit.characterData._NpcDataNode.m_Gender == GenderType.Female)
									{
										num7++;
									}
								}
							}
							num += 0.01f * (float)num7 * (float)talentResultPart.iValue;
						}
					}
				}
			}
		}
		if (this.CheckUnWeapon(uab.skillID))
		{
			num += this.CheckTalentSelfAndTeamPlus(TalentEffect.WristEffectPlus, true, false);
		}
		else
		{
			num += this.CheckTalentSelfAndTeamPlus(TalentEffect.WeaponeEffectPlus, true, false);
		}
		return num;
	}

	// Token: 0x06002F30 RID: 12080 RVA: 0x0016F1D0 File Offset: 0x0016D3D0
	private void CheckTalentCountAttack(UnitAbility uab)
	{
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			bool flag = false;
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.m_TalentEffect == TalentEffect.CounterAttackTactic)
						{
							flag = true;
							uab.chainedAbilityIDList.Add(talentResultPart.iValue);
						}
					}
				}
				if (flag)
				{
					string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
					UINGUI.BattleMessage(msg);
					new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
				}
			}
		}
	}

	// Token: 0x06002F31 RID: 12081 RVA: 0x0016F2F4 File Offset: 0x0016D4F4
	private void CheckTalentKillOne()
	{
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			bool flag = false;
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.m_TalentEffect == TalentEffect.KillOnePlusBuff)
						{
							flag = true;
							this.ApplyConditionID(talentResultPart.iValue, talentData.m_strTalentName, false);
						}
					}
				}
				if (flag)
				{
					string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
					UINGUI.BattleMessage(msg);
					new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
				}
			}
		}
	}

	// Token: 0x06002F32 RID: 12082 RVA: 0x0016F41C File Offset: 0x0016D61C
	private void CheckTalentBeCritical()
	{
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			bool flag = false;
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.m_TalentEffect == TalentEffect.BeCriticalTactic)
						{
							flag = true;
							this.ApplyConditionID(talentResultPart.iValue, talentData.m_strTalentName, false);
						}
					}
				}
				if (flag)
				{
					string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
					UINGUI.BattleMessage(msg);
					new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
				}
			}
		}
	}

	// Token: 0x06002F33 RID: 12083 RVA: 0x0016F540 File Offset: 0x0016D740
	public float CheckTalentSelfAndTeamPlus(TalentEffect te, bool bMax, bool bShowLog)
	{
		float num = this.characterData.GetNpcTalentPercentValue(te);
		if (num != 0f)
		{
			if (bShowLog)
			{
				string talentNameHaveEffect = this.characterData.GetTalentNameHaveEffect(te);
				string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentNameHaveEffect);
				UINGUI.BattleMessage(msg);
				new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentNameHaveEffect, _OverlayType.Talent, 0f);
			}
		}
		else
		{
			num = this.CheckTeamTalentPercent(te, bMax, bShowLog, true);
			num *= 0.5f;
		}
		return num;
	}

	// Token: 0x06002F34 RID: 12084 RVA: 0x0016F5E8 File Offset: 0x0016D7E8
	public float CheckTeamTalentPercent(TalentEffect te, bool bMax, bool bShowLog, bool bMyTeam)
	{
		bool flag = false;
		string text = string.Empty;
		float num;
		if (bMax)
		{
			num = float.MinValue;
		}
		else
		{
			num = float.MaxValue;
		}
		if (bMyTeam)
		{
			List<UnitTB> allFriend = UnitControl.GetAllFriend(this.factionID);
			for (int i = 0; i < allFriend.Count; i++)
			{
				CharacterData characterData = allFriend[i].characterData;
				if (characterData.CheckTalentEffect(te))
				{
					float npcTalentPercentValue = characterData.GetNpcTalentPercentValue(te);
					if (bMax)
					{
						if (npcTalentPercentValue > num)
						{
							flag = true;
							num = npcTalentPercentValue;
							text = characterData.GetTalentNameHaveEffect(te);
						}
					}
					else if (npcTalentPercentValue < num)
					{
						flag = true;
						num = npcTalentPercentValue;
						text = characterData.GetTalentNameHaveEffect(te);
					}
				}
			}
		}
		else
		{
			List<UnitTB> allHostile = UnitControl.GetAllHostile(this.factionID);
			for (int j = 0; j < allHostile.Count; j++)
			{
				CharacterData characterData2 = allHostile[j].characterData;
				if (characterData2.CheckTalentEffect(te))
				{
					float npcTalentPercentValue2 = characterData2.GetNpcTalentPercentValue(te);
					if (bMax)
					{
						if (npcTalentPercentValue2 > num)
						{
							flag = true;
							num = npcTalentPercentValue2;
							text = characterData2.GetTalentNameHaveEffect(te);
						}
					}
					else if (npcTalentPercentValue2 < num)
					{
						flag = true;
						num = npcTalentPercentValue2;
						text = characterData2.GetTalentNameHaveEffect(te);
					}
				}
			}
		}
		if (bShowLog && flag)
		{
			string msg = string.Format(Game.StringTable.GetString(260019), Game.StringTable.GetString(110016), text);
			UINGUI.BattleMessage(msg);
		}
		if (flag)
		{
			return num;
		}
		return 0f;
	}

	// Token: 0x06002F35 RID: 12085 RVA: 0x0016F778 File Offset: 0x0016D978
	public int CheckTeamTalentValue(TalentEffect te, bool bMax, bool bShowLog, bool bMyTeam)
	{
		bool flag = false;
		string text = string.Empty;
		int num;
		if (bMax)
		{
			num = int.MinValue;
		}
		else
		{
			num = int.MaxValue;
		}
		if (bMyTeam)
		{
			List<UnitTB> allFriend = UnitControl.GetAllFriend(this.factionID);
			for (int i = 0; i < allFriend.Count; i++)
			{
				CharacterData characterData = allFriend[i].characterData;
				if (characterData.CheckTalentEffect(te))
				{
					int talentValue = characterData.GetTalentValue(te);
					if (bMax)
					{
						if (talentValue > num)
						{
							flag = true;
							num = talentValue;
							text = characterData.GetTalentNameHaveEffect(te);
						}
					}
					else if (talentValue < num)
					{
						flag = true;
						num = talentValue;
						text = characterData.GetTalentNameHaveEffect(te);
					}
				}
			}
		}
		else
		{
			List<UnitTB> allHostile = UnitControl.GetAllHostile(this.factionID);
			for (int j = 0; j < allHostile.Count; j++)
			{
				CharacterData characterData2 = allHostile[j].characterData;
				if (characterData2.CheckTalentEffect(te))
				{
					int talentValue2 = characterData2.GetTalentValue(te);
					if (bMax)
					{
						if (talentValue2 > num)
						{
							flag = true;
							num = talentValue2;
							text = characterData2.GetTalentNameHaveEffect(te);
						}
					}
					else if (talentValue2 < num)
					{
						flag = true;
						num = talentValue2;
						text = characterData2.GetTalentNameHaveEffect(te);
					}
				}
			}
		}
		if (bShowLog && flag)
		{
			string msg = string.Format(Game.StringTable.GetString(260019), Game.StringTable.GetString(110016), text);
			UINGUI.BattleMessage(msg);
		}
		if (flag)
		{
			return num;
		}
		return 0;
	}

	// Token: 0x06002F36 RID: 12086 RVA: 0x0016F900 File Offset: 0x0016DB00
	private int GetTalentAreaRange()
	{
		int num = 0;
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if ((talentResultPart.m_TalentEffect == TalentEffect.NearMaleDamageUp || talentResultPart.m_TalentEffect == TalentEffect.NearFemaleDamageUp) && num < talentResultPart.iUpDown)
						{
							num = talentResultPart.iUpDown;
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002F37 RID: 12087 RVA: 0x0016F9D4 File Offset: 0x0016DBD4
	public float GetTalentAreaDamagePercent(int iRange, UnitTB targetUnit)
	{
		float num = 0f;
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.iUpDown >= iRange)
						{
							if (talentResultPart.m_TalentEffect == TalentEffect.NearMaleDamageUp && targetUnit.characterData._NpcDataNode.m_Gender == GenderType.Male)
							{
								num += 0.01f * (float)talentResultPart.iValue;
							}
							if (talentResultPart.m_TalentEffect == TalentEffect.NearFemaleDamageUp && targetUnit.characterData._NpcDataNode.m_Gender == GenderType.Female)
							{
								num += 0.01f * (float)talentResultPart.iValue;
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002F38 RID: 12088 RVA: 0x0016FAF4 File Offset: 0x0016DCF4
	public void Confuse(int iTurn, int iWantToFaction, string confuseTalentName)
	{
		if (this.characterData.CheckTalentEffect(TalentEffect.PinkSkeleton))
		{
			string talentNameHaveEffect = this.characterData.GetTalentNameHaveEffect(TalentEffect.PinkSkeleton);
			string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentNameHaveEffect) + confuseTalentName + Game.StringTable.GetString(260037);
			UINGUI.BattleMessage(msg);
			UINGUI.DisplayMessage(msg);
		}
		else
		{
			this.iOrigFaction = this.factionID;
			this.iConfuseTurn = iTurn;
			this.iCountTillNextTurn = UnitControl.activeFactionCount;
			UnitControl.ChangeUnitFaction(this, iWantToFaction);
			this.bActionPlayerConfuse = true;
		}
	}

	// Token: 0x06002F39 RID: 12089 RVA: 0x0016FB98 File Offset: 0x0016DD98
	public bool CheckTalentAssistAttack(int iRange, UnitTB needAssistUnit, List<Tile> tileList)
	{
		for (int i = 0; i < this.characterData.TalentList.Count; i++)
		{
			int iID = this.characterData.TalentList[i];
			TalentNewDataNode talentData = Game.TalentNewData.GetTalentData(iID);
			if (talentData != null)
			{
				for (int j = 0; j < talentData.m_cEffetPartList.Count; j++)
				{
					TalentResultPart talentResultPart = talentData.m_cEffetPartList[j];
					if (talentResultPart != null)
					{
						if (talentResultPart.iUpDown >= iRange)
						{
							if (talentResultPart.m_TalentEffect == TalentEffect.AssistAttack)
							{
								if (this.CheckFriendFaction(needAssistUnit.factionID))
								{
									if (Random.Range(0, 100) < talentResultPart.iValue && this.AssistAttack(tileList))
									{
										this.assistTheUnit = needAssistUnit;
										string msg = string.Format(Game.StringTable.GetString(260019), this.unitName, talentData.m_strTalentName);
										UINGUI.BattleMessage(msg);
										new EffectOverlay(this.thisT.position + new Vector3(0f, 1.5f, 0f), talentData.m_strTalentName.ToString(), _OverlayType.Talent, 0f);
										return true;
									}
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06002F3A RID: 12090 RVA: 0x0016FCFC File Offset: 0x0016DEFC
	public int GetEffectPartAbsoluteBuffOnHitCount(_EffectPartType typePart)
	{
		int num = 0;
		for (int i = 0; i < this.unitConditionList.Count; i++)
		{
			ConditionNode conditionNode = this.unitConditionList[i];
			for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
			{
				EffectPart effectPart = conditionNode.m_effectPartList[j];
				if (typePart == effectPart.m_effectPartType && effectPart.m_effectAccumulateType == _EffectAccumulateType.Absolute && (conditionNode.m_iCondType == _ConditionType.Buff || conditionNode.m_iCondType == _ConditionType.InstantBuff))
				{
					if (conditionNode.m_iRemoveOnHit == 0)
					{
						return 0;
					}
					if (num < conditionNode.m_iRemoveOnHit)
					{
						num = conditionNode.m_iRemoveOnHit;
						break;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002F3B RID: 12091 RVA: 0x0016FDB8 File Offset: 0x0016DFB8
	public int GetEffectPartAurasAbsoluteBuffOnHitCount(_EffectPartType typePart, int iRange, int iTileUnitFactionID)
	{
		bool flag = this.CheckFriendFaction(iTileUnitFactionID);
		int num = 0;
		for (int i = 0; i < this.unitConditionList.Count; i++)
		{
			ConditionNode conditionNode = this.unitConditionList[i];
			for (int j = 0; j < conditionNode.m_effectPartList.Count; j++)
			{
				EffectPart effectPart = conditionNode.m_effectPartList[j];
				if (this.CheckAurasAbsoluteBuff(effectPart))
				{
					if (typePart == effectPart.m_effectPartType)
					{
						if (effectPart.m_iValueLimit >= iRange)
						{
							if (flag && effectPart.m_effectAccumulateType == _EffectAccumulateType.Auras)
							{
								if (conditionNode.m_iRemoveOnHit == 0)
								{
									return 0;
								}
								if (num < conditionNode.m_iRemoveOnHit)
								{
									num = conditionNode.m_iRemoveOnHit;
									break;
								}
							}
							if (effectPart.m_effectAccumulateType == _EffectAccumulateType.AurasAll)
							{
								if (conditionNode.m_iRemoveOnHit == 0)
								{
									return 0;
								}
								if (num < conditionNode.m_iRemoveOnHit)
								{
									num = conditionNode.m_iRemoveOnHit;
									break;
								}
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002F3C RID: 12092 RVA: 0x0016FEC8 File Offset: 0x0016E0C8
	public void RemoveTauntFleeCondition(int iUnitID)
	{
		int i = 0;
		while (i < this.unitConditionList.Count)
		{
			ConditionNode conditionNode = this.unitConditionList[i];
			if ((conditionNode.GetEffectPartAbsoluteDebuff(_EffectPartType.Taunt) || conditionNode.GetEffectPartAbsoluteDebuff(_EffectPartType.Flee)) && conditionNode.m_iTargetUnitID == iUnitID)
			{
				this.unitConditionList.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
		this.CheckEffectObject();
	}

	// Token: 0x06002F3D RID: 12093 RVA: 0x0016FF3C File Offset: 0x0016E13C
	public int FindConditionEffectPartID(_EffectPartType part)
	{
		for (int i = 0; i < this.unitConditionList.Count; i++)
		{
			ConditionNode conditionNode = this.unitConditionList[i];
			if (conditionNode.GetEffectPartAbsoluteDebuff(part))
			{
				return conditionNode.m_iTargetUnitID;
			}
		}
		return 0;
	}

	// Token: 0x06002F3E RID: 12094 RVA: 0x0016FF88 File Offset: 0x0016E188
	public void CheckEffectObject()
	{
		int num = this.FindConditionEffectPartID(_EffectPartType.Flee);
		if (num != 0 && this.goFlee == null)
		{
			this.goFlee = this.ApplyEffectObject("Effects/Flee");
		}
		if (num == 0 && this.goFlee != null)
		{
			Object.DestroyImmediate(this.goFlee);
		}
		num = this.FindConditionEffectPartID(_EffectPartType.Taunt);
		if (num != 0 && this.goTaunt == null)
		{
			this.goTaunt = this.ApplyEffectObject("Effects/Taunt");
		}
		if (num == 0 && this.goTaunt != null)
		{
			Object.DestroyImmediate(this.goTaunt);
		}
	}

	// Token: 0x06002F3F RID: 12095 RVA: 0x0017003C File Offset: 0x0016E23C
	private GameObject ApplyEffectObject(string str)
	{
		GameObject gameObject = null;
		if (Game.g_EffectsBundle.Contains(str))
		{
			gameObject = (Game.g_EffectsBundle.Load(str) as GameObject);
		}
		GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
		gameObject2.tag = "DynamicEffect";
		gameObject2.transform.parent = this.animationTB.moveAniBody.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localScale = Vector3.one;
		return gameObject2;
	}

	// Token: 0x06002F40 RID: 12096 RVA: 0x001700C0 File Offset: 0x0016E2C0
	public bool ProcessBeCapture()
	{
		float num = (float)this.HP;
		num /= (float)this.fullHP;
		if (!this.characterData.bCaptive)
		{
			UINGUI.DisplayMessage(Game.StringTable.GetString(990109));
			return false;
		}
		if (num > 0.5f)
		{
			return false;
		}
		float num2 = 0.01f * (float)this.characterData.iJoin;
		num2 *= 1f + (1f - num * 2f);
		return Random.Range(0f, 1f) <= num2;
	}

	// Token: 0x04003A4D RID: 14925
	public string dlcCharGuid = string.Empty;

	// Token: 0x04003A4E RID: 14926
	public CharacterData characterData;

	// Token: 0x04003A4F RID: 14927
	public Texture icon;

	// Token: 0x04003A50 RID: 14928
	public Texture iconTalk;

	// Token: 0x04003A51 RID: 14929
	public string iconName = string.Empty;

	// Token: 0x04003A52 RID: 14930
	public int iUnitID;

	// Token: 0x04003A53 RID: 14931
	public string unitName = "name";

	// Token: 0x04003A54 RID: 14932
	public string desp = "nothing";

	// Token: 0x04003A55 RID: 14933
	public int factionID;

	// Token: 0x04003A56 RID: 14934
	public List<int> routineIDList = new List<int>();

	// Token: 0x04003A57 RID: 14935
	[HideInInspector]
	public bool bMainPlayer;

	// Token: 0x04003A58 RID: 14936
	[HideInInspector]
	public int iRoutineID;

	// Token: 0x04003A59 RID: 14937
	public int fullHP = 500;

	// Token: 0x04003A5A RID: 14938
	public int HP = 500;

	// Token: 0x04003A5B RID: 14939
	public int fullSP = 200;

	// Token: 0x04003A5C RID: 14940
	public int SP = 200;

	// Token: 0x04003A5D RID: 14941
	public float fFlyOnGrass;

	// Token: 0x04003A5E RID: 14942
	public int movementRange = 3;

	// Token: 0x04003A5F RID: 14943
	public int movePerTurn = 1;

	// Token: 0x04003A60 RID: 14944
	public int moveRemain = 1;

	// Token: 0x04003A61 RID: 14945
	public int attackPerTurn = 1;

	// Token: 0x04003A62 RID: 14946
	public int attackRemain = 1;

	// Token: 0x04003A63 RID: 14947
	public int counterPerTurn = 1;

	// Token: 0x04003A64 RID: 14948
	public int counterAttackRemain = 1;

	// Token: 0x04003A65 RID: 14949
	public int useItemCD;

	// Token: 0x04003A66 RID: 14950
	public UnitTBAnimation animationTB;

	// Token: 0x04003A67 RID: 14951
	public UnitTBAudio audioTB;

	// Token: 0x04003A68 RID: 14952
	private float rotateSpeed = 20f;

	// Token: 0x04003A69 RID: 14953
	private float moveSpeed = 14f;

	// Token: 0x04003A6A RID: 14954
	public Tile occupiedTile;

	// Token: 0x04003A6B RID: 14955
	public List<int> abilityIDList = new List<int>();

	// Token: 0x04003A6C RID: 14956
	public List<UnitAbility> unitAbilityList = new List<UnitAbility>();

	// Token: 0x04003A6D RID: 14957
	public NeigongNewDataNode unitNeigong;

	// Token: 0x04003A6E RID: 14958
	public List<ConditionNode> unitNeigongConditionList = new List<ConditionNode>();

	// Token: 0x04003A6F RID: 14959
	public int iNeigongLv;

	// Token: 0x04003A70 RID: 14960
	public List<ConditionNode> unitConditionList = new List<ConditionNode>();

	// Token: 0x04003A71 RID: 14961
	public List<Tile> lastAttackTileList = new List<Tile>();

	// Token: 0x04003A72 RID: 14962
	public float fHitRate = 1f;

	// Token: 0x04003A73 RID: 14963
	public float fDodgeRate;

	// Token: 0x04003A74 RID: 14964
	public float fCriticalRate;

	// Token: 0x04003A75 RID: 14965
	public float fDefCriticalRate;

	// Token: 0x04003A76 RID: 14966
	public float fCounterRate;

	// Token: 0x04003A77 RID: 14967
	public float fDefCounterRate;

	// Token: 0x04003A78 RID: 14968
	public int iDamageReduc;

	// Token: 0x04003A79 RID: 14969
	public float fDamageReduc = 1f;

	// Token: 0x04003A7A RID: 14970
	public int iDeadPlus;

	// Token: 0x04003A7B RID: 14971
	public int iAttackPlus;

	// Token: 0x04003A7C RID: 14972
	public bool bAttackPlus;

	// Token: 0x04003A7D RID: 14973
	private int iAttackDivDamage = 200;

	// Token: 0x04003A7E RID: 14974
	private int iDeadPlusDownDouble = 10;

	// Token: 0x04003A7F RID: 14975
	private int iAttackPlusDownDouble = 5;

	// Token: 0x04003A80 RID: 14976
	public int iBeStealCount;

	// Token: 0x04003A81 RID: 14977
	public int iBeStealMoneryRate;

	// Token: 0x04003A82 RID: 14978
	public int iNowAuraRange;

	// Token: 0x04003A83 RID: 14979
	public bool bSpeicalAction;

	// Token: 0x04003A84 RID: 14980
	public bool bCharge;

	// Token: 0x04003A85 RID: 14981
	public Tile tChargeToTile;

	// Token: 0x04003A86 RID: 14982
	public bool bBeTarget;

	// Token: 0x04003A87 RID: 14983
	public bool bLeaveBattle;

	// Token: 0x04003A88 RID: 14984
	public bool bIsMoving;

	// Token: 0x04003A89 RID: 14985
	private Tile moveOrigTile;

	// Token: 0x04003A8A RID: 14986
	private Quaternion beforeMoveRot;

	// Token: 0x04003A8B RID: 14987
	private List<Tile> movingPathList = new List<Tile>();

	// Token: 0x04003A8C RID: 14988
	private bool m_AlreadyCheckPlusAttack;

	// Token: 0x04003A8D RID: 14989
	private float fDodgeSpRate = 0.05f;

	// Token: 0x04003A8E RID: 14990
	private float fHealSpExpRate = 0.33f;

	// Token: 0x04003A8F RID: 14991
	private float fHealHpExpRate = 0.33f;

	// Token: 0x04003A90 RID: 14992
	private float fTankHpExpRate = 0.2f;

	// Token: 0x04003A91 RID: 14993
	private float fDamageExpRate = 0.2f;

	// Token: 0x04003A92 RID: 14994
	private int iMaxEffect = 10000;

	// Token: 0x04003A93 RID: 14995
	private int iSPEffectValue = 80;

	// Token: 0x04003A94 RID: 14996
	public int iClearCDValue;

	// Token: 0x04003A95 RID: 14997
	public int iDamageAbsorbToSPValue;

	// Token: 0x04003A96 RID: 14998
	public int iRemoveFlowerValue;

	// Token: 0x04003A97 RID: 14999
	public int iShockValue;

	// Token: 0x04003A98 RID: 15000
	public _AttackMode attackMode;

	// Token: 0x04003A99 RID: 15001
	public int damageType;

	// Token: 0x04003A9A RID: 15002
	public int damageTypeMelee;

	// Token: 0x04003A9B RID: 15003
	public int damageTypeRange;

	// Token: 0x04003A9C RID: 15004
	public int attackRangeMelee = 1;

	// Token: 0x04003A9D RID: 15005
	public int attackRangeMin;

	// Token: 0x04003A9E RID: 15006
	public int attackRangeMax = 4;

	// Token: 0x04003A9F RID: 15007
	public int armorType;

	// Token: 0x04003AA0 RID: 15008
	public float waitingTime;

	// Token: 0x04003AA1 RID: 15009
	public float waitedTime;

	// Token: 0x04003AA2 RID: 15010
	public float baseTurnPriority;

	// Token: 0x04003AA3 RID: 15011
	public int stun;

	// Token: 0x04003AA4 RID: 15012
	public int abilityDisabled;

	// Token: 0x04003AA5 RID: 15013
	public int unitAbilityGroupSkillID = -1;

	// Token: 0x04003AA6 RID: 15014
	[HideInInspector]
	public List<ThreatNode> threatList = new List<ThreatNode>();

	// Token: 0x04003AA7 RID: 15015
	public int iTotalThreat;

	// Token: 0x04003AA8 RID: 15016
	public _AIMode aiMode;

	// Token: 0x04003AA9 RID: 15017
	public UnitTB aiTarget;

	// Token: 0x04003AAA RID: 15018
	public Tile aiTile;

	// Token: 0x04003AAB RID: 15019
	public bool triggered;

	// Token: 0x04003AAC RID: 15020
	[HideInInspector]
	public bool moved;

	// Token: 0x04003AAD RID: 15021
	[HideInInspector]
	public bool attacked;

	// Token: 0x04003AAE RID: 15022
	[HideInInspector]
	public bool abilityTriggered;

	// Token: 0x04003AAF RID: 15023
	public int actionQueued;

	// Token: 0x04003AB0 RID: 15024
	public Transform turretObject;

	// Token: 0x04003AB1 RID: 15025
	public Transform shootPoint;

	// Token: 0x04003AB2 RID: 15026
	public List<Transform> shootPoints = new List<Transform>();

	// Token: 0x04003AB3 RID: 15027
	private bool spawnedInGame;

	// Token: 0x04003AB4 RID: 15028
	private int spawnedLastDuration = -1;

	// Token: 0x04003AB5 RID: 15029
	public int pointCost = 5;

	// Token: 0x04003AB6 RID: 15030
	public int pointReward;

	// Token: 0x04003AB7 RID: 15031
	[HideInInspector]
	public int prefabID;

	// Token: 0x04003AB8 RID: 15032
	public int turnPriority = 5;

	// Token: 0x04003AB9 RID: 15033
	public int sight = 5;

	// Token: 0x04003ABA RID: 15034
	public int backupLayer;

	// Token: 0x04003ABB RID: 15035
	public bool bStealth;

	// Token: 0x04003ABC RID: 15036
	private bool bSteal;

	// Token: 0x04003ABD RID: 15037
	private GameObject goStealth;

	// Token: 0x04003ABE RID: 15038
	private GameObject goFlee;

	// Token: 0x04003ABF RID: 15039
	private GameObject goTaunt;

	// Token: 0x04003AC0 RID: 15040
	private int iDeadCount;

	// Token: 0x04003AC1 RID: 15041
	private bool bCheckTalentDeadthNow;

	// Token: 0x04003AC2 RID: 15042
	public bool bNightFragrance;

	// Token: 0x04003AC3 RID: 15043
	private float fLastNumberTime;

	// Token: 0x04003AC4 RID: 15044
	private int iOrigFaction;

	// Token: 0x04003AC5 RID: 15045
	private int iConfuseTurn;

	// Token: 0x04003AC6 RID: 15046
	private int iCountTillNextTurn;

	// Token: 0x04003AC7 RID: 15047
	private bool bActionPlayerConfuse;

	// Token: 0x04003AC8 RID: 15048
	[HideInInspector]
	public Transform thisT;

	// Token: 0x04003AC9 RID: 15049
	[HideInInspector]
	public GameObject thisObj;

	// Token: 0x04003ACA RID: 15050
	private UnitTB assistTheUnit;

	// Token: 0x04003ACB RID: 15051
	private AttackInstance lastAttackInstance;

	// Token: 0x04003ACC RID: 15052
	public bool bTileEventCantReMove;

	// Token: 0x04003ACD RID: 15053
	private static Tile tileCompareBase;

	// Token: 0x04003ACE RID: 15054
	public GameObject destroyedEffect;

	// Token: 0x04003ACF RID: 15055
	public float destroyEffectDuration = 2f;

	// Token: 0x04003AD0 RID: 15056
	public bool spawnCollectibleUponDestroyed;

	// Token: 0x04003AD1 RID: 15057
	public List<CollectibleTB> collectibleList = new List<CollectibleTB>();

	// Token: 0x04003AD2 RID: 15058
	[HideInInspector]
	private ItemDataNode activeItemPendingTarget;

	// Token: 0x04003AD3 RID: 15059
	[HideInInspector]
	private UnitAbility activeAbilityPendingTarget;

	// Token: 0x04003AD4 RID: 15060
	public List<UnitAbility> activeUnitAbilityEffectList = new List<UnitAbility>();

	// Token: 0x04003AD5 RID: 15061
	public List<Effect> activeCollectibleAbilityEffectList = new List<Effect>();

	// Token: 0x02000786 RID: 1926
	// (Invoke) Token: 0x06002F44 RID: 12100
	public delegate void UnitSelectedHandler(UnitTB unit);

	// Token: 0x02000787 RID: 1927
	// (Invoke) Token: 0x06002F48 RID: 12104
	public delegate void UnitDeselectedHandler();

	// Token: 0x02000788 RID: 1928
	// (Invoke) Token: 0x06002F4C RID: 12108
	public delegate void ActionCompletedHandler(UnitTB unit);

	// Token: 0x02000789 RID: 1929
	// (Invoke) Token: 0x06002F50 RID: 12112
	public delegate void NewPositionHandler(UnitTB unit);

	// Token: 0x0200078A RID: 1930
	// (Invoke) Token: 0x06002F54 RID: 12116
	public delegate void TurnDepletedHandler();

	// Token: 0x0200078B RID: 1931
	// (Invoke) Token: 0x06002F58 RID: 12120
	public delegate void UnitDestroyedHandler(UnitTB unit);

	// Token: 0x0200078C RID: 1932
	// (Invoke) Token: 0x06002F5C RID: 12124
	public delegate void EffectAppliedHandler(UnitTB unit);

	// Token: 0x0200078D RID: 1933
	// (Invoke) Token: 0x06002F60 RID: 12128
	public delegate void EffectExpiredHandler(UnitTB unit);

	// Token: 0x0200078E RID: 1934
	// (Invoke) Token: 0x06002F64 RID: 12132
	public delegate void ActorAttack(UnitTB unitSrc, UnitTB unitDect);

	// Token: 0x0200078F RID: 1935
	// (Invoke) Token: 0x06002F68 RID: 12136
	public delegate void DamageDone(UnitTB unitSrc, float fDamageDone);
}
