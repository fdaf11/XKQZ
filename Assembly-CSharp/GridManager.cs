using System;
using System.Collections;
using System.Collections.Generic;
using Heluo.Wulin;
using UnityEngine;

// Token: 0x0200074F RID: 1871
[RequireComponent(typeof(GridPainter))]
public class GridManager : MonoBehaviour
{
	// Token: 0x1400004A RID: 74
	// (add) Token: 0x06002C91 RID: 11409 RVA: 0x0001CCF2 File Offset: 0x0001AEF2
	// (remove) Token: 0x06002C92 RID: 11410 RVA: 0x0001CD09 File Offset: 0x0001AF09
	public static event GridManager.HoverTileEnterHandler onHoverTileEnterE;

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x06002C93 RID: 11411 RVA: 0x0001CD20 File Offset: 0x0001AF20
	// (remove) Token: 0x06002C94 RID: 11412 RVA: 0x0001CD37 File Offset: 0x0001AF37
	public static event GridManager.HoverTileExitHandler onHoverTileExitE;

	// Token: 0x06002C95 RID: 11413 RVA: 0x0015687C File Offset: 0x00154A7C
	public void GetAllTile()
	{
		Tile[] componentsInChildren = base.GetComponentsInChildren<Tile>();
		this.allTiles = new List<Tile>();
		foreach (Tile tile in componentsInChildren)
		{
			this.allTiles.Add(tile);
		}
	}

	// Token: 0x06002C96 RID: 11414 RVA: 0x001568C4 File Offset: 0x00154AC4
	private void Awake()
	{
		GridManager.instance = this;
		GridManager.walkableList = new List<Tile>();
		GridManager.hostileList = new List<Tile>();
		this.GetAllTile();
		this.InitPlacementTile(false);
		for (int i = 0; i < this.allTiles.Count; i++)
		{
			this.allTiles[i].pos = this.allTiles[i].transform.position;
			this.allTiles[i].transform.position = new Vector3(this.allTiles[i].pos.x, this.baseHeight, this.allTiles[i].pos.z);
		}
		for (int j = 0; j < this.allTiles.Count; j++)
		{
			this.allTiles[j].SetupNeighbour();
		}
		for (int k = 0; k < this.allTiles.Count; k++)
		{
			this.allTiles[k].transform.position = new Vector3(this.allTiles[k].pos.x, this.allTiles[k].pos.y, this.allTiles[k].pos.z);
		}
		for (int l = 0; l < this.allTiles.Count; l++)
		{
			this.allTiles[l].SetupWall();
		}
	}

	// Token: 0x06002C97 RID: 11415 RVA: 0x00156A5C File Offset: 0x00154C5C
	private void Start()
	{
		this.InitIndicator();
		GridManager.instance.allPlaceableTiles = new List<Tile>();
		for (int i = 0; i < GridManager.instance.allTiles.Count; i++)
		{
			Tile tile = GridManager.instance.allTiles[i];
			if (tile.openForPlacement && tile.unit == null)
			{
				GridManager.instance.allPlaceableTiles.Add(tile);
			}
		}
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown += new Action<KeyControl.Key>(this.OnKeyDown);
			UINGUI.instance.Move += new Action<Vector2>(this.OnMove);
			UINGUI.instance.MouseControl += new Action<bool>(this.OnMouseControl);
		}
	}

	// Token: 0x06002C98 RID: 11416 RVA: 0x00156B30 File Offset: 0x00154D30
	private void OnDestroy()
	{
		if (this.indicatorSelect != null)
		{
			Object.Destroy(this.indicatorCursor.gameObject);
		}
		if (this.indicatorCursor != null)
		{
			Object.Destroy(this.indicatorCursor.gameObject);
		}
		if (this.indicatorH != null)
		{
			foreach (Transform transform in this.indicatorH)
			{
				if (transform != null && transform.gameObject != null)
				{
					Object.Destroy(transform.gameObject);
				}
			}
			this.indicatorH = null;
		}
		if (this.indicatorHostile != null && this.indicatorHostile.gameObject != null)
		{
			Object.Destroy(this.indicatorHostile.gameObject);
		}
		if (UINGUI.instance != null)
		{
			UINGUI.instance.KeyDown -= new Action<KeyControl.Key>(this.OnKeyDown);
			UINGUI.instance.Move -= new Action<Vector2>(this.OnMove);
			UINGUI.instance.MouseControl -= new Action<bool>(this.OnMouseControl);
		}
	}

	// Token: 0x06002C99 RID: 11417 RVA: 0x00156C64 File Offset: 0x00154E64
	private void OnMouseControl(bool bMouse)
	{
		if (UINGUI.instance.battleControlState == BattleControlState.Unselect || UINGUI.instance.battleControlState == BattleControlState.Move || UINGUI.instance.battleControlState == BattleControlState.SelectTarget)
		{
			if (bMouse)
			{
				CameraControl.instance.trackNowTile = null;
				CameraControl.instance.trackTile = false;
			}
			else
			{
				CameraControl.instance.trackNowTile = this.nowCursorTile;
				CameraControl.instance.trackTile = true;
			}
		}
	}

	// Token: 0x06002C9A RID: 11418 RVA: 0x0001CD4E File Offset: 0x0001AF4E
	public void OnMove(Vector2 direction)
	{
		if (UINGUI.instance.battleControlState == BattleControlState.Unselect || UINGUI.instance.battleControlState == BattleControlState.Move || UINGUI.instance.battleControlState == BattleControlState.SelectTarget)
		{
			this.MoveDirect(direction);
		}
	}

	// Token: 0x06002C9B RID: 11419 RVA: 0x00156CE0 File Offset: 0x00154EE0
	private void OnKeyDown(KeyControl.Key keyCode)
	{
		if (GameGlobal.m_bBattleTalk)
		{
			return;
		}
		if (UINGUI.instance.battleControlState == BattleControlState.Unselect)
		{
			if (keyCode != KeyControl.Key.BattleNextUnit)
			{
				if (keyCode != KeyControl.Key.UnitInfo)
				{
					if (keyCode == KeyControl.Key.OK)
					{
						GridManager.Select(this.nowCursorTile);
					}
				}
				else if (this.nowCursorTile != null)
				{
					this.nowCursorTile.OnRightClick();
				}
			}
			else if (GameControlTB.IsPlayerTurn())
			{
				UnitControl.SwitchToNextUnitInTurnList();
			}
		}
		if (UINGUI.instance.battleControlState == BattleControlState.Move)
		{
			switch (keyCode)
			{
			case KeyControl.Key.BattleNextUnit:
				if (GameControlTB.IsPlayerTurn())
				{
					UnitControl.SwitchToNextUnitInTurnList();
				}
				break;
			case KeyControl.Key.UnitInfo:
				if (this.nowCursorTile != null)
				{
					this.nowCursorTile.OnRightClick();
				}
				break;
			default:
				if (keyCode != KeyControl.Key.OK)
				{
					if (keyCode == KeyControl.Key.Cancel)
					{
						GridManager.Deselect();
					}
				}
				else if (this.nowCursorTile != null)
				{
					if (this.nowCursorTile == this.selectedTile)
					{
						if (UnitControl.selectedUnit == this.selectedTile.unit)
						{
							GridManager.ClearWalkableList();
							UINGUI.instance.DelayBattleControlState(BattleControlState.SelectAction);
						}
					}
					else
					{
						this.nowCursorTile.OnTouchMouseDown();
					}
				}
				break;
			case KeyControl.Key.Skill1:
				UINGUI.instance.uiAbilityButtons.OnAbilityButton(UINGUI.instance.uiAbilityButtons.buttonList[0].rootObj);
				GridManager.instance.ReHoverLastTile();
				break;
			case KeyControl.Key.Skill2:
				UINGUI.instance.uiAbilityButtons.OnAbilityButton(UINGUI.instance.uiAbilityButtons.buttonList[1].rootObj);
				GridManager.instance.ReHoverLastTile();
				break;
			case KeyControl.Key.Skill3:
				UINGUI.instance.uiAbilityButtons.OnAbilityButton(UINGUI.instance.uiAbilityButtons.buttonList[2].rootObj);
				GridManager.instance.ReHoverLastTile();
				break;
			case KeyControl.Key.Skill4:
				UINGUI.instance.uiAbilityButtons.OnAbilityButton(UINGUI.instance.uiAbilityButtons.buttonList[3].rootObj);
				GridManager.instance.ReHoverLastTile();
				break;
			case KeyControl.Key.Skill5:
				UINGUI.instance.uiAbilityButtons.OnAbilityButton(UINGUI.instance.uiAbilityButtons.buttonList[4].rootObj);
				GridManager.instance.ReHoverLastTile();
				break;
			case KeyControl.Key.Skill6:
				UINGUI.instance.uiAbilityButtons.OnAbilityButton(UINGUI.instance.uiAbilityButtons.buttonList[5].rootObj);
				GridManager.instance.ReHoverLastTile();
				break;
			case KeyControl.Key.SelectItem:
				UINGUI.instance.ShowBattleItemInBackpack();
				break;
			case KeyControl.Key.RestTurn:
				UINGUI.instance.uiHUD.OnEndTurnButton();
				break;
			}
		}
		if (UINGUI.instance.battleControlState == BattleControlState.SelectTarget)
		{
			if (keyCode != KeyControl.Key.OK)
			{
				if (keyCode != KeyControl.Key.Cancel)
				{
					if (keyCode == KeyControl.Key.UnitInfo)
					{
						if (this.nowCursorTile != null)
						{
							this.nowCursorTile.OnRightClick();
						}
					}
				}
				else
				{
					this.SelectTargetKeyCancel();
				}
			}
			else if (this.nowCursorTile != null)
			{
				this.nowCursorTile.OnTouchMouseDown();
			}
		}
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x00157078 File Offset: 0x00155278
	private void SelectTargetKeyCancel()
	{
		if (UnitControl.selectedUnit != null)
		{
			UnitControl.selectedUnit.SetActiveAbilityPendingTarget(null);
			UnitControl.selectedUnit.SetActiveItemPendingTarget(null);
		}
		GridManager.targetTileSelectMode = false;
		GridManager.currentUAB = null;
		GridManager.uABScrTile = null;
		GridManager.OnHoverTargetTileExit();
		foreach (Tile tile in GridManager.tilesInAbilityRange)
		{
			tile.SetState(_TileState.Default);
		}
		GridManager.tilesInAbilityRange = new List<Tile>();
		UINGUI.instance.DelayBattleControlState(BattleControlState.SelectAction);
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x00157124 File Offset: 0x00155324
	private void MoveDirect(Vector2 direction)
	{
		if (direction.sqrMagnitude <= 0.5f)
		{
			return;
		}
		if (this.ftime + this.ftick > Time.time)
		{
			return;
		}
		this.ftime = Time.time;
		List<Tile> list;
		if (UINGUI.instance.battleControlState == BattleControlState.Unselect)
		{
			list = GridManager.GetAllTiles();
		}
		else if (UINGUI.instance.battleControlState == BattleControlState.Move)
		{
			list = new List<Tile>();
			list.AddRange(GridManager.walkableList.ToArray());
			list.Add(this.selectedTile);
		}
		else if (UINGUI.instance.battleControlState == BattleControlState.SelectTarget)
		{
			list = GridManager.GetTilesWithinRange(GridManager.uABScrTile, GridManager.currentUAB.range);
			GridManager.OnHoverExit();
		}
		else
		{
			list = new List<Tile>();
		}
		Tile directNearTileInGroup = this.GetDirectNearTileInGroup(list, this.nowCursorTile, direction);
		if (directNearTileInGroup != null)
		{
			this.nowCursorTile = directNearTileInGroup;
			CameraControl.instance.trackNowTile = this.nowCursorTile;
			GridManager.OnHoverEnter(this.nowCursorTile);
		}
	}

	// Token: 0x06002C9E RID: 11422 RVA: 0x0015722C File Offset: 0x0015542C
	private void SetClear()
	{
		foreach (Tile tile in this.allTiles)
		{
			tile.SetState(_TileState.Default);
		}
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x00157288 File Offset: 0x00155488
	private void InitIndicator()
	{
		if (this.indicatorSelect == null)
		{
			if (this.type == _TileType.Square)
			{
				this.indicatorSelect = (Resources.Load("ScenePrefab/SquareIndicators/SqIndicatorSelect", typeof(Transform)) as Transform);
			}
			else if (this.type == _TileType.Hex)
			{
				this.indicatorSelect = (Resources.Load("ScenePrefab/HexIndicators/HexIndicatorSelect_Projector", typeof(Transform)) as Transform);
			}
			if (this.indicatorSelect != null)
			{
				this.indicatorSelect = (Transform)Object.Instantiate(this.indicatorSelect);
				this.indicatorSelect.parent = base.transform;
				this.indicatorSelect.position = new Vector3(0f, 9999f, 0f);
				this.indicatorSelect.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
		}
		if (this.indicatorCursor == null)
		{
			if (this.type == _TileType.Square)
			{
				this.indicatorCursor = (Resources.Load("ScenePrefab/SquareIndicators/SqIndicatorCursor", typeof(Transform)) as Transform);
			}
			else if (this.type == _TileType.Hex)
			{
				this.indicatorCursor = (Resources.Load("ScenePrefab/HexIndicators/HexIndicatorCursor_Projector", typeof(Transform)) as Transform);
			}
			if (this.indicatorCursor != null)
			{
				this.indicatorCursor = (Transform)Object.Instantiate(this.indicatorCursor);
				this.indicatorCursor.parent = base.transform;
				this.indicatorCursor.position = new Vector3(0f, 9999f, 0f);
				this.indicatorCursor.localEulerAngles = new Vector3(0f, 0f, 0f);
			}
		}
		if (this.indicatorHostile == null)
		{
			if (this.type == _TileType.Square)
			{
				this.indicatorHostile = (Resources.Load("ScenePrefab/SquareIndicators/SqIndicatorHostile", typeof(Transform)) as Transform);
			}
			else if (this.type == _TileType.Hex)
			{
				this.indicatorHostile = (Resources.Load("ScenePrefab/HexIndicators/HexIndicatorHostile_Projector", typeof(Transform)) as Transform);
			}
			if (this.indicatorHostile != null)
			{
				this.indicatorHostile = (Transform)Object.Instantiate(this.indicatorHostile);
			}
			this.indicatorHostile.parent = base.transform;
		}
		if (this.indicatorHostile != null)
		{
			this.indicatorHostile.position = new Vector3(0f, 9999f, 0f);
			this.indicatorHostile.localEulerAngles = new Vector3(0f, 0f, 0f);
			int allUnitCount = UnitControl.GetAllUnitCount();
			if (allUnitCount > 0)
			{
				this.indicatorH = new Transform[this.MAX_INDICATOR_HOSTILE];
				this.indicatorH[0] = this.indicatorHostile;
				for (int i = 1; i < this.MAX_INDICATOR_HOSTILE; i++)
				{
					this.indicatorH[i] = (Transform)Object.Instantiate(this.indicatorHostile);
					this.indicatorH[i].parent = this.indicatorH[0].parent;
					this.indicatorH[i].localEulerAngles = new Vector3(0f, 0f, 0f);
				}
			}
		}
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x001575E0 File Offset: 0x001557E0
	private void GenerateSquareGrid()
	{
		if (this.squareTilePrefab == null)
		{
			this.squareTilePrefab = (Resources.Load("ScenePrefab/SquareTile", typeof(GameObject)) as GameObject);
		}
		float num = float.NegativeInfinity;
		float num2 = float.PositiveInfinity;
		float num3 = float.NegativeInfinity;
		float num4 = float.PositiveInfinity;
		float num5 = this.gridToTileSizeRatio * this.gridSize;
		DateTime now = DateTime.Now;
		int num6 = 0;
		while ((float)num6 < this.width)
		{
			int num7 = 0;
			while ((float)num7 < this.length)
			{
				float num8 = (float)num6 * num5;
				float num9 = (float)num7 * num5;
				if (num9 > num)
				{
					num = num9;
				}
				if (num9 < num2)
				{
					num2 = num9;
				}
				if (num8 > num3)
				{
					num3 = num8;
				}
				if (num8 < num4)
				{
					num4 = num8;
				}
				Vector3 localPosition;
				localPosition..ctor(num8, this.baseHeight, num9);
				GameObject gameObject = (GameObject)Object.Instantiate(this.squareTilePrefab);
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = localPosition;
				gameObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
				gameObject.transform.localScale *= this.gridSize;
				Tile component = gameObject.GetComponent<Tile>();
				this.allTiles.Add(component);
				num7++;
			}
			num6++;
		}
		DateTime now2 = DateTime.Now;
		Debug.Log("Generate grid, time:" + (now2 - now).TotalMilliseconds);
		float num10 = (Mathf.Abs(num) - Mathf.Abs(num2)) / 2f;
		float num11 = (Mathf.Abs(num3) - Mathf.Abs(num4)) / 2f;
		for (int i = 0; i < this.allTiles.Count; i++)
		{
			Tile tile = this.allTiles[i];
			Transform transform = tile.transform;
			transform.position += new Vector3(-num11, 0f, -num10);
			tile.pos = transform.position;
		}
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x0015781C File Offset: 0x00155A1C
	private void GenerateHexGrid()
	{
		if (this.hexTilePrefab == null)
		{
			this.hexTilePrefab = (Resources.Load("ScenePrefab/HexTileProjector", typeof(GameObject)) as GameObject);
		}
		float num = 0.86627907f;
		float num2 = this.gridToTileSizeRatio * this.gridSize;
		float num3 = num2 * num;
		float num4 = float.NegativeInfinity;
		float num5 = float.PositiveInfinity;
		float num6 = float.NegativeInfinity;
		float num7 = float.PositiveInfinity;
		int num8 = 0;
		while ((float)num8 < this.width)
		{
			float num9 = 0.5f * (float)(num8 % 2);
			int num10;
			if (num8 % 2 == 1)
			{
				num10 = (int)(this.length / 2f);
			}
			else
			{
				num10 = (int)(this.length / 2f + this.length % 2f);
			}
			for (int i = 0; i < num10; i++)
			{
				float num11 = (float)num8 * num3;
				float num12 = (float)i * num2 + num2 * num9;
				if (num12 > num4)
				{
					num4 = num12;
				}
				if (num12 < num5)
				{
					num5 = num12;
				}
				if (num11 > num6)
				{
					num6 = num11;
				}
				if (num11 < num7)
				{
					num7 = num11;
				}
				Vector3 localPosition;
				localPosition..ctor(num11, this.baseHeight, num12);
				GameObject gameObject = (GameObject)Object.Instantiate(this.hexTilePrefab);
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = localPosition;
				gameObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
				gameObject.transform.localScale *= this.gridSize * 1.1628f;
				Tile component = gameObject.GetComponent<Tile>();
				this.allTiles.Add(component);
			}
			num8++;
		}
		float num13 = (Mathf.Abs(num4) - Mathf.Abs(num5)) / 2f;
		float num14 = (Mathf.Abs(num6) - Mathf.Abs(num7)) / 2f;
		foreach (Tile tile in this.allTiles)
		{
			Transform transform = tile.transform;
			transform.position += new Vector3(-num14, 0f, -num13);
			tile.pos = transform.position;
		}
	}

	// Token: 0x06002CA2 RID: 11426 RVA: 0x00157AA0 File Offset: 0x00155CA0
	public void GenerateGrid(bool generateNew)
	{
		DateTime now = DateTime.Now;
		if (generateNew)
		{
			for (int i = 0; i < this.allTiles.Count; i++)
			{
				if (this.allTiles[i] != null)
				{
					if (this.allTiles[i].unit != null)
					{
						Object.DestroyImmediate(this.allTiles[i].unit.gameObject);
					}
					else if (this.allTiles[i].obstacle != null)
					{
						Object.DestroyImmediate(this.allTiles[i].obstacle.gameObject);
					}
					else if (this.allTiles[i].collectible != null)
					{
						Object.DestroyImmediate(this.allTiles[i].collectible.gameObject);
					}
					Object.DestroyImmediate(this.allTiles[i].gameObject);
				}
			}
			this.allTiles = new List<Tile>();
			if (this.type == _TileType.Square)
			{
				this.GenerateSquareGrid();
			}
			else if (this.type == _TileType.Hex)
			{
				this.GenerateHexGrid();
			}
			int num = (int)((float)this.allTiles.Count * this.unwalkableRate);
			if (num > 0)
			{
				for (int j = 0; j < num; j++)
				{
					int num2 = Random.Range(0, this.allTiles.Count);
					while (!this.allTiles[num2].walkable || this.allTiles[num2].unit != null)
					{
						num2 = Random.Range(0, this.allTiles.Count);
					}
					if (!this.useObstacle)
					{
						this.allTiles[num2].walkable = false;
						this.allTiles[num2].Start();
					}
					else
					{
						int num3 = 0;
						if (this.enableInvUnwalkable)
						{
							num3++;
						}
						if (this.enableVUnwalkable)
						{
							num3++;
						}
						int num4 = Random.Range(0, this.obstaclePrefabList.Count + num3);
						if (num4 < num3)
						{
							if (this.enableInvUnwalkable && num4 == 0)
							{
								this.allTiles[num2].SetToUnwalkable(false);
							}
							else
							{
								this.allTiles[num2].SetToUnwalkable(true);
							}
						}
						else
						{
							GameObject gameObject = (GameObject)Object.Instantiate(this.obstaclePrefabList[num4 - num3].gameObject);
							gameObject.transform.localScale *= this.gridSize;
							gameObject.transform.parent = this.allTiles[num2].transform;
							gameObject.transform.localPosition = Vector3.zero;
							gameObject.GetComponent<Collider>().enabled = false;
							Obstacle component = gameObject.GetComponent<Obstacle>();
							this.allTiles[num2].obstacle = component;
							component.occupiedTile = this.allTiles[num2];
							this.allTiles[num2].SetToUnwalkable(false);
						}
					}
				}
			}
			for (int k = 0; k < this.allTiles.Count; k++)
			{
				this.allTiles[k].gameObject.layer = 8;
			}
			if (this.addColletible && this.collectiblePrefabList.Count > 0)
			{
				int num5 = Random.Range(this.minCollectibleCount, this.maxCollectibleCount);
				if (num5 > 0)
				{
					List<Tile> list = new List<Tile>();
					for (int l = 0; l < this.allTiles.Count; l++)
					{
						if (this.allTiles[l].walkable)
						{
							list.Add(this.allTiles[l]);
						}
					}
					for (int m = 0; m < num5; m++)
					{
						int num6 = Random.Range(0, list.Count);
						int num7 = Random.Range(0, this.collectiblePrefabList.Count);
						GridManager.InsertCollectible(this.collectiblePrefabList[num7], list[num6]);
						list.RemoveAt(num6);
						if (list.Count == 0)
						{
							break;
						}
					}
				}
			}
			this.GenerateUnit();
		}
		this.InitPlacementTile(true);
		if (generateNew)
		{
			DateTime now2 = DateTime.Now;
			Debug.Log("Everything else, time:" + (now2 - now).TotalMilliseconds);
		}
	}

	// Token: 0x06002CA3 RID: 11427 RVA: 0x00157F68 File Offset: 0x00156168
	private void InitPlacementTile(bool flag)
	{
		GameControlTB gameControlTB = (GameControlTB)Object.FindObjectOfType(typeof(GameControlTB));
		for (int i = 0; i < this.playerPlacementAreas.Count; i++)
		{
			this.allPlaceableTiles = new List<Tile>();
			List<Tile> tileWithinRect = this.GetTileWithinRect(this.playerPlacementAreas[i]);
			for (int j = 0; j < tileWithinRect.Count; j++)
			{
				Tile tile = tileWithinRect[j];
				if (tile.walkable && tile.unit == null)
				{
					tile.openForPlacement = true;
					tile.placementID = gameControlTB.playerFactionID[i];
					if (flag)
					{
						tile.SetState(_TileState.Walkable);
					}
					this.allPlaceableTiles.Add(tile);
				}
			}
		}
	}

	// Token: 0x06002CA4 RID: 11428 RVA: 0x00158038 File Offset: 0x00156238
	private void OnNewPlacement(PlayerUnits pUnits)
	{
		this.allPlaceableTiles = new List<Tile>();
		for (int i = 0; i < this.allTiles.Count; i++)
		{
			Tile tile = this.allTiles[i];
			if (tile.openForPlacement && tile.placementID == pUnits.factionID)
			{
				tile.openForPlacement = true;
				tile.placementID = pUnits.factionID;
				tile.SetState(_TileState.Walkable);
				this.allPlaceableTiles.Add(tile);
			}
			else
			{
				tile.SetState(_TileState.Default);
			}
		}
	}

	// Token: 0x06002CA5 RID: 11429 RVA: 0x001580C8 File Offset: 0x001562C8
	public static void InsertCollectible(CollectibleTB collectible, Tile tile)
	{
		GameObject gameObject = (GameObject)Object.Instantiate(collectible.gameObject);
		gameObject.transform.parent = tile.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.rotation = Quaternion.Euler(0f, (float)(Random.Range(0, 6) * 60), 0f);
		CollectibleTB component = gameObject.GetComponent<CollectibleTB>();
		tile.collectible = component;
		component.occupiedTile = tile;
	}

	// Token: 0x06002CA6 RID: 11430 RVA: 0x00158144 File Offset: 0x00156344
	public void GenerateUnit()
	{
		for (int i = 0; i < this.allTiles.Count; i++)
		{
			if (this.allTiles[i].unit != null)
			{
				Object.DestroyImmediate(this.allTiles[i].unit.gameObject);
				this.allTiles[i].unit = null;
			}
		}
		Transform transform = null;
		foreach (object obj in base.transform.parent)
		{
			Transform transform2 = (Transform)obj;
			if (transform2.gameObject.name == "Units")
			{
				transform = transform2;
				break;
			}
		}
		int j = 0;
		foreach (Faction faction in this.factionList)
		{
			foreach (FactionSpawnInfo factionSpawnInfo in faction.spawnInfo)
			{
				bool flag = true;
				if (factionSpawnInfo.unitPrefabs.Length > 0)
				{
					for (int k = 0; k < factionSpawnInfo.unitPrefabs.Length; k++)
					{
						if (factionSpawnInfo.unitPrefabs[k] == null)
						{
							flag = false;
						}
					}
				}
				else
				{
					flag = false;
				}
				if (flag)
				{
					if (factionSpawnInfo.unitCount > 0)
					{
						List<Tile> tileWithinRect = this.GetTileWithinRect(factionSpawnInfo.area);
						for (int l = 0; l < tileWithinRect.Count; l++)
						{
							Tile tile = tileWithinRect[l];
							if (tile.unit != null || !tile.walkable || tile.collectible != null)
							{
								tileWithinRect.RemoveAt(l);
								l--;
							}
						}
						if (factionSpawnInfo.spawnQuota == _SpawnQuota.UnitBased)
						{
							int num = Mathf.Min(factionSpawnInfo.unitCount, tileWithinRect.Count);
							for (j = 0; j < num; j++)
							{
								int num2 = Random.Range(0, tileWithinRect.Count);
								Tile tile2 = tileWithinRect[num2];
								int num3 = Random.Range(0, factionSpawnInfo.unitPrefabs.Length);
								GameObject gameObject = (GameObject)Object.Instantiate(factionSpawnInfo.unitPrefabs[num3].gameObject);
								gameObject.transform.position = tile2.transform.position;
								if (transform != null)
								{
									gameObject.transform.parent = transform;
								}
								int num4 = factionSpawnInfo.unitRotation - 1;
								if (num4 == -1)
								{
									gameObject.transform.rotation = Quaternion.Euler(0f, (float)(Random.Range(0, 6) * 60), 0f);
								}
								else
								{
									gameObject.transform.rotation = Quaternion.Euler(0f, (float)(num4 * 60), 0f);
								}
								UnitTB component = gameObject.GetComponent<UnitTB>();
								component.factionID = faction.factionID;
								component.occupiedTile = tile2;
								tile2.unit = component;
								gameObject.name = string.Concat(new object[]
								{
									"Gen",
									component.factionID,
									"_",
									component.unitName,
									"_",
									j.ToString()
								});
								tileWithinRect.RemoveAt(num2);
								if (tileWithinRect.Count == 0)
								{
									break;
								}
							}
						}
						else
						{
							int m = factionSpawnInfo.budget;
							int num5 = 99999999;
							for (int n = 0; n < factionSpawnInfo.unitPrefabs.Length; n++)
							{
								if (factionSpawnInfo.unitPrefabs[n].pointCost < num5)
								{
									num5 = factionSpawnInfo.unitPrefabs[n].pointCost;
								}
							}
							while (m >= num5)
							{
								int num6 = Random.Range(0, tileWithinRect.Count);
								Tile tile3 = tileWithinRect[num6];
								int num7 = Random.Range(0, factionSpawnInfo.unitPrefabs.Length);
								GameObject gameObject2 = (GameObject)Object.Instantiate(factionSpawnInfo.unitPrefabs[num7].gameObject);
								gameObject2.transform.position = tile3.transform.position;
								if (transform != null)
								{
									gameObject2.transform.parent = transform;
								}
								int num8 = factionSpawnInfo.unitRotation - 1;
								if (num8 == -1)
								{
									gameObject2.transform.rotation = Quaternion.Euler(0f, (float)(Random.Range(0, 6) * 60), 0f);
								}
								else
								{
									gameObject2.transform.rotation = Quaternion.Euler(0f, (float)(num8 * 60), 0f);
								}
								UnitTB component2 = gameObject2.GetComponent<UnitTB>();
								component2.factionID = faction.factionID;
								component2.occupiedTile = tile3;
								tile3.unit = component2;
								gameObject2.name = string.Concat(new object[]
								{
									"Gen",
									component2.factionID,
									"_",
									component2.unitName,
									"_",
									j.ToString()
								});
								tileWithinRect.RemoveAt(num6);
								if (tileWithinRect.Count == 0)
								{
									break;
								}
								m -= Mathf.Max(1, component2.pointCost);
							}
						}
					}
				}
				else
				{
					Debug.Log("unit assignment error for faction: " + faction.factionID);
				}
			}
		}
	}

	// Token: 0x06002CA7 RID: 11431 RVA: 0x0001CD87 File Offset: 0x0001AF87
	private void OnEnable()
	{
		GameControlTB.onBattleStartE += this.OnBattleStart;
		GameControlTB.onNextTurnE += this.OnNextTurn;
		UnitControl.onNewPlacementE += this.OnNewPlacement;
	}

	// Token: 0x06002CA8 RID: 11432 RVA: 0x0001CDBC File Offset: 0x0001AFBC
	private void OnDisable()
	{
		GameControlTB.onBattleStartE -= this.OnBattleStart;
		GameControlTB.onNextTurnE -= this.OnNextTurn;
		UnitControl.onNewPlacementE -= this.OnNewPlacement;
	}

	// Token: 0x06002CA9 RID: 11433 RVA: 0x0015875C File Offset: 0x0015695C
	private void OnBattleStart()
	{
		for (int i = 0; i < this.allTiles.Count; i++)
		{
			Tile tile = this.allTiles[i];
			if (tile.openForPlacement)
			{
				tile.openForPlacement = false;
				tile.SetState(_TileState.Default);
			}
		}
		this.allPlaceableTiles = new List<Tile>();
	}

	// Token: 0x06002CAA RID: 11434 RVA: 0x0001CDF1 File Offset: 0x0001AFF1
	private void OnNextTurn()
	{
		if (this.selectedTile != null)
		{
			GridManager.Deselect();
		}
	}

	// Token: 0x06002CAB RID: 11435 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06002CAC RID: 11436 RVA: 0x001587B8 File Offset: 0x001569B8
	public static void Select(Tile ht)
	{
		string @string = Game.StringTable.GetString(100176);
		string string2 = Game.StringTable.GetString(100175);
		string string3 = Game.StringTable.GetString(100177);
		string string4 = Game.StringTable.GetString(100178);
		string string5 = Game.StringTable.GetString(100179);
		if (GridManager.targetTileSelectMode)
		{
			if (GridManager.Distance(ht, GridManager.instance.selectedTile) <= GridManager.currentUAB.range)
			{
				if (GridManager.currentUAB.targetArea == _TargetArea.Default && GridManager.currentUAB.aoeRange == 0)
				{
					if (GridManager.currentUAB.targetType == _AbilityTargetType.AllUnits)
					{
						if (ht.unit == null)
						{
							GameControlTB.DisplayMessage(@string);
						}
						else
						{
							UnitControl.selectedUnit.UnitAbilityTargetSelected(ht);
							GridManager.ExitTargetTileSelectMode();
						}
					}
					else if (GridManager.currentUAB.targetType == _AbilityTargetType.Friendly)
					{
						int factionID = UnitControl.selectedUnit.factionID;
						if (ht.unit == null)
						{
							GameControlTB.DisplayMessage(@string);
						}
						else if (!ht.unit.CheckFriendFaction(factionID))
						{
							GameControlTB.DisplayMessage(string2);
						}
						else
						{
							UnitControl.selectedUnit.UnitAbilityTargetSelected(ht);
							GridManager.ExitTargetTileSelectMode();
						}
					}
					else if (GridManager.currentUAB.targetType == _AbilityTargetType.Hostile)
					{
						int factionID2 = UnitControl.selectedUnit.factionID;
						if (ht.unit == null)
						{
							GameControlTB.DisplayMessage(@string);
						}
						else if (ht.unit.CheckFriendFaction(factionID2))
						{
							GameControlTB.DisplayMessage(string3);
						}
						else
						{
							UnitControl.selectedUnit.UnitAbilityTargetSelected(ht);
							GridManager.ExitTargetTileSelectMode();
						}
					}
					else if (GridManager.currentUAB.targetType == _AbilityTargetType.EmptyTile)
					{
						if (ht.unit != null)
						{
							GameControlTB.DisplayMessage(string4);
						}
						else
						{
							UnitControl.selectedUnit.UnitAbilityTargetSelected(ht);
							GridManager.ExitTargetTileSelectMode();
						}
					}
					else
					{
						UnitControl.selectedUnit.UnitAbilityTargetSelected(ht);
						GridManager.ExitTargetTileSelectMode();
					}
				}
				else
				{
					UnitControl.selectedUnit.UnitAbilityTargetSelected(GridManager.currentAOETileGroup);
					GridManager.ExitTargetTileSelectMode();
				}
			}
			else
			{
				GameControlTB.DisplayMessage(string5);
			}
			return;
		}
		if (ht == null || ht.unit == null)
		{
			return;
		}
		if (!GameControlTB.IsPlayerFaction(ht.unit.factionID))
		{
			return;
		}
		if (UnitControl.selectedUnit != null && ht.unit.factionID != UnitControl.selectedUnit.factionID)
		{
			return;
		}
		if (GridManager.instance.selectedTile != null)
		{
			GridManager.Deselect();
		}
		GridManager.instance.selectedTile = ht;
		ht.SetState(_TileState.Selected);
		UnitTB unit = ht.unit;
		unit.Select();
		if (ht != null)
		{
			GridManager.instance.indicatorSelect.localPosition = ht.thisT.localPosition;
		}
		else
		{
			GridManager.instance.indicatorSelect.position = new Vector3(0f, 99999f, 0f);
		}
		if (unit.IsStunned())
		{
			return;
		}
		GridManager.ResetHostileList(ht);
		GridManager.ResetWalkableList(ht);
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x00158AEC File Offset: 0x00156CEC
	public static void AISelect(Tile tile)
	{
		if (GridManager.instance.selectedTile != null)
		{
			GridManager.Deselect();
		}
		GridManager.instance.selectedTile = tile;
		tile.SetState(_TileState.Selected);
		UnitTB unit = tile.unit;
		unit.Select();
		if (tile != null)
		{
			GridManager.instance.indicatorSelect.localPosition = tile.thisT.localPosition;
		}
		else
		{
			GridManager.instance.indicatorSelect.position = new Vector3(0f, 99999f, 0f);
		}
		GridManager.ResetHostileList(tile);
		GridManager.ResetWalkableList(tile);
	}

	// Token: 0x06002CAE RID: 11438 RVA: 0x00158B8C File Offset: 0x00156D8C
	public static void Deselect()
	{
		if (GridManager.instance.selectedTile == null)
		{
			return;
		}
		GridManager.instance.nowCursorTile = GridManager.instance.selectedTile;
		GridManager.ClearWalkableList();
		GridManager.ClearHostileList();
		if (GridManager.instance.selectedTile.unit != null)
		{
			GridManager.instance.selectedTile.unit.Deselect();
		}
		GridManager.instance.selectedTile.SetState(_TileState.Default);
		GridManager.instance.selectedTile = null;
		GridManager.instance.indicatorSelect.position = new Vector3(0f, 99999f, 0f);
		GridManager.OnHoverTargetTileExit();
		GridManager.ExitTargetTileSelectMode();
	}

	// Token: 0x06002CAF RID: 11439 RVA: 0x00158C44 File Offset: 0x00156E44
	public static void ResetHostileList(Tile ht)
	{
		UnitTB unit = ht.unit;
		if (unit == null)
		{
			GridManager.ClearHostileList();
			return;
		}
		if (GameControlTB.AttackAPCostRule() == _AttackAPCostRule.PerAttack && unit.SP <= 0)
		{
			GridManager.ClearHostileList();
			return;
		}
		if (!unit.attacked && !unit.GetAttackDisabled())
		{
			List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(ht, unit.GetUnitAbilityRangeMin(), unit.GetUnitAbilityRangeMax());
			foreach (Tile tile in tilesWithinRange)
			{
				if (tile.unit != null && !tile.unit.CheckFriendFaction(unit.factionID))
				{
					bool flag = true;
					if (GameControlTB.EnableFogOfWar())
					{
						int num = GridManager.Distance(tile, ht);
						flag = (GridManager.IsInLOS(ht, tile) & num < unit.GetUnitSight() & tile.unit.IsVisibleToPlayer());
					}
					if (flag)
					{
						tile.SetState(_TileState.Hostile);
						tile.attackableToSelected = true;
						if (!GridManager.hostileList.Contains(tile))
						{
							GridManager.hostileList.Add(tile);
						}
					}
				}
			}
			GridManager.PlaceIndicatorH();
		}
	}

	// Token: 0x06002CB0 RID: 11440 RVA: 0x00158D84 File Offset: 0x00156F84
	public static void ResetWalkableList(Tile ht)
	{
		UnitTB unit = ht.unit;
		if (unit == null)
		{
			GridManager.ClearWalkableList();
			return;
		}
		if (unit.attacked && !GameControlTB.AllowMovementAfterAttack())
		{
			return;
		}
		if (!unit.moved && unit.stun <= 0 && !unit.GetMovementDisabled())
		{
			int num = unit.GetMoveRange();
			if (GameControlTB.MovementAPCostRule() == _MovementAPCostRule.PerTile)
			{
				num = Mathf.Min(unit.SP, num);
			}
			else if (GameControlTB.MovementAPCostRule() == _MovementAPCostRule.PerMove && unit.SP < 1)
			{
				num = 0;
			}
			if (num == 0)
			{
				GridManager.ClearWalkableList();
				return;
			}
			GridManager.walkableList = GridManager.GetWalkableTilesWithinRange(ht, num);
			for (int i = 0; i < GridManager.walkableList.Count; i++)
			{
				Tile tile = GridManager.walkableList[i];
				if (tile.unit == null)
				{
					tile.walkableToSelected = true;
					tile.SetState(_TileState.Walkable);
				}
			}
		}
	}

	// Token: 0x06002CB1 RID: 11441 RVA: 0x00158E80 File Offset: 0x00157080
	public static void ClearHostileList()
	{
		GridManager.ClearAllIndicatorH();
		for (int i = 0; i < GridManager.hostileList.Count; i++)
		{
			Tile tile = GridManager.hostileList[i];
			tile.SetState(_TileState.Default);
			tile.attackableToSelected = false;
		}
		GridManager.hostileList = new List<Tile>();
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x00158ED4 File Offset: 0x001570D4
	public static void ClearWalkableList()
	{
		for (int i = 0; i < GridManager.walkableList.Count; i++)
		{
			Tile tile = GridManager.walkableList[i];
			tile.SetState(_TileState.Default);
			tile.walkableToSelected = false;
		}
		GridManager.walkableList = new List<Tile>();
	}

	// Token: 0x06002CB3 RID: 11443 RVA: 0x00158F20 File Offset: 0x00157120
	public static void OnHoverTargetTileSelect(Tile rootTile)
	{
		if (GridManager.Distance(rootTile, GridManager.uABScrTile) > GridManager.currentUAB.range)
		{
			return;
		}
		GridManager.currentAOETileGroup = new List<Tile>();
		if (GridManager.currentUAB.targetArea == _TargetArea.Default)
		{
			GridManager.currentAOETileGroup = new List<Tile>();
			if (GridManager.currentUAB.aoeRange >= 1)
			{
				GridManager.currentAOETileGroup = GridManager.GetAOETile(rootTile, GridManager.currentUAB.aoeRange);
			}
			GridManager.currentAOETileGroup.Add(rootTile);
		}
		else if (GridManager.currentUAB.targetArea == _TargetArea.Line)
		{
			GridManager.currentAOETileGroup = GridManager.GetTileInLine(GridManager.uABScrTile, rootTile, GridManager.currentUAB.range);
		}
		else if (GridManager.currentUAB.targetArea == _TargetArea.Cone)
		{
			if (GridManager.instance.type == _TileType.Hex)
			{
				GridManager.currentAOETileGroup = GridManager.GetTileInCone60(GridManager.uABScrTile, rootTile, GridManager.currentUAB.range);
			}
			else if (GridManager.instance.type == _TileType.Square)
			{
				GridManager.currentAOETileGroup = GridManager.GetTileInLine(GridManager.uABScrTile, rootTile, GridManager.currentUAB.range);
			}
		}
		else if (GridManager.currentUAB.targetArea == _TargetArea.Fan1 && GridManager.instance.type == _TileType.Hex)
		{
			GridManager.currentAOETileGroup = GridManager.GetTileInFan1(GridManager.uABScrTile, rootTile, GridManager.currentUAB.range);
		}
		if (GridManager.currentUAB.effectType == _EffectType.Heal)
		{
			foreach (Tile tile in GridManager.currentAOETileGroup)
			{
				tile.SetState(_TileState.Walkable);
			}
		}
		else if (GridManager.currentUAB.effectType == _EffectType.Buff)
		{
			foreach (Tile tile2 in GridManager.currentAOETileGroup)
			{
				tile2.SetState(_TileState.AbilityRange);
			}
		}
		else if (GridManager.currentUAB.effectType == _EffectType.Damage || GridManager.currentUAB.effectType == _EffectType.Debuff || GridManager.currentUAB.effectType == _EffectType.SkillGroup)
		{
			foreach (Tile tile3 in GridManager.currentAOETileGroup)
			{
				tile3.SetState(_TileState.Hostile);
			}
		}
		if (GridManager.onHoverTileEnterE != null)
		{
			GridManager.onHoverTileEnterE(rootTile, GridManager.currentUAB);
		}
	}

	// Token: 0x06002CB4 RID: 11444 RVA: 0x001591C8 File Offset: 0x001573C8
	public static void OnHoverTargetTileExit()
	{
		foreach (Tile tile in GridManager.currentAOETileGroup)
		{
			if (tile != null)
			{
				if (GridManager.tilesInAbilityRange.Contains(tile))
				{
					tile.SetState(_TileState.AbilityRange);
				}
				else
				{
					tile.SetState(_TileState.Default);
				}
			}
		}
		if (GridManager.onHoverTileExitE != null)
		{
			GridManager.onHoverTileExitE();
		}
	}

	// Token: 0x06002CB5 RID: 11445 RVA: 0x0001CE09 File Offset: 0x0001B009
	public void ReHoverLastTile()
	{
		if (this.LastHoverTile != null)
		{
			GridManager.OnHoverEnter(this.LastHoverTile);
		}
	}

	// Token: 0x06002CB6 RID: 11446 RVA: 0x00159260 File Offset: 0x00157460
	public static void OnHoverEnter(Tile tile)
	{
		GridManager.instance.LastHoverTile = tile;
		GridManager.instance.indicatorCursor.localPosition = tile.thisT.localPosition;
		if (GridManager.targetTileSelectMode)
		{
			GridManager.OnHoverTargetTileSelect(tile);
			return;
		}
		UnitTB selectedUnit = UnitControl.selectedUnit;
		if (selectedUnit != null && !selectedUnit.attacked && tile.walkableToSelected && selectedUnit.IsControllable())
		{
			int rangeMin = Mathf.Max(selectedUnit.GetUnitAbilityRangeMin(), 0);
			int rangeMax = Mathf.Max(selectedUnit.GetUnitAbilityRangeMax(), 0);
			List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(tile, rangeMin, rangeMax);
			foreach (Tile tile2 in tilesWithinRange)
			{
				if (tile2.unit != null && tile2.unit.factionID != selectedUnit.factionID && !tile2.attackableToSelected)
				{
					bool flag = true;
					if (GameControlTB.EnableFogOfWar())
					{
						int num = GridManager.Distance(tile, tile2);
						flag = (GridManager.IsInLOS(tile, tile2) & num <= selectedUnit.GetUnitSight() & tile2.unit.IsVisibleToPlayer());
					}
					if (flag)
					{
						GridManager.tempHostileTileList.Add(tile2);
						tile2.SetState(_TileState.Hostile);
					}
				}
			}
		}
		if (GridManager.onHoverTileEnterE != null)
		{
			GridManager.onHoverTileEnterE(tile, null);
		}
	}

	// Token: 0x06002CB7 RID: 11447 RVA: 0x001593E0 File Offset: 0x001575E0
	public static void OnHoverExit()
	{
		GridManager.instance.LastHoverTile = null;
		if (GridManager.targetTileSelectMode)
		{
			GridManager.OnHoverTargetTileExit();
			return;
		}
		GridManager.instance.indicatorCursor.position = new Vector3(0f, 99999f, 0f);
		foreach (Tile tile in GridManager.tempHostileTileList)
		{
			if (!tile.attackableToSelected)
			{
				tile.SetState(_TileState.Default);
			}
		}
		GridManager.tempHostileTileList = new List<Tile>();
		if (GridManager.onHoverTileExitE != null)
		{
			GridManager.onHoverTileExitE();
		}
	}

	// Token: 0x06002CB8 RID: 11448 RVA: 0x0001CE27 File Offset: 0x0001B027
	public static void PlaceUnitAt(Tile tile)
	{
		GridManager.instance.allPlaceableTiles.Remove(tile);
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x0001CE3A File Offset: 0x0001B03A
	public static void RemoveUnitAt(Tile tile)
	{
		GridManager.instance.allPlaceableTiles.Add(tile);
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x0001CE4C File Offset: 0x0001B04C
	public static bool AllPlaceableTileOccupied()
	{
		return GridManager.instance.allPlaceableTiles.Count <= 0;
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x0001CE66 File Offset: 0x0001B066
	public static List<Tile> GetAllPlaceableTiles()
	{
		return GridManager.instance.allPlaceableTiles;
	}

	// Token: 0x06002CBC RID: 11452 RVA: 0x001594A0 File Offset: 0x001576A0
	public static List<Tile> GetAllNeighbouringTile(Tile tile)
	{
		List<Tile> list = new List<Tile>();
		list.Add(tile);
		foreach (Tile tile2 in tile.GetNeighbours())
		{
			list.Add(tile2);
		}
		return list;
	}

	// Token: 0x06002CBD RID: 11453 RVA: 0x0001CE72 File Offset: 0x0001B072
	public static bool IsInTargetTileSelectMode()
	{
		return GridManager.targetTileSelectMode;
	}

	// Token: 0x06002CBE RID: 11454 RVA: 0x00159508 File Offset: 0x00157708
	public static void SetTargetTileSelectMode(Tile tile, UnitAbility uAB)
	{
		GridManager.targetTileSelectMode = true;
		GridManager.currentUAB = uAB;
		GridManager.uABScrTile = tile;
		GridManager.instance.nowCursorTile = GridManager.uABScrTile;
		foreach (Tile tile2 in GridManager.walkableList)
		{
			tile2.SetState(_TileState.Default);
		}
		foreach (Tile tile3 in GridManager.hostileList)
		{
			tile3.SetState(_TileState.Default);
		}
		if (uAB.targetArea == _TargetArea.Default)
		{
			List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(GridManager.uABScrTile, GridManager.currentUAB.range);
			foreach (Tile tile4 in tilesWithinRange)
			{
				if (tile4.walkable)
				{
					if (uAB.targetType == _AbilityTargetType.AllTile)
					{
						if (tile4.unit == null)
						{
							GridManager.tilesInAbilityRange.Add(tile4);
							tile4.SetState(_TileState.AbilityRange);
						}
					}
					else
					{
						GridManager.tilesInAbilityRange.Add(tile4);
						tile4.SetState(_TileState.AbilityRange);
					}
				}
			}
		}
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x00159684 File Offset: 0x00157884
	public static void ExitTargetTileSelectMode()
	{
		GridManager.targetTileSelectMode = false;
		GridManager.currentUAB = null;
		if (GridManager.uABScrTile != null)
		{
			GridManager.uABScrTile = null;
		}
		GridManager.OnHoverTargetTileExit();
		foreach (Tile tile in GridManager.tilesInAbilityRange)
		{
			tile.SetState(_TileState.Default);
		}
		GridManager.tilesInAbilityRange = new List<Tile>();
		if (UnitControl.selectedUnit != null)
		{
			foreach (Tile tile2 in GridManager.walkableList)
			{
				tile2.SetState(_TileState.Walkable);
			}
			foreach (Tile tile3 in GridManager.hostileList)
			{
				tile3.SetState(_TileState.Hostile);
			}
		}
	}

	// Token: 0x06002CC0 RID: 11456 RVA: 0x001597B4 File Offset: 0x001579B4
	private IEnumerator HoverCoroutine()
	{
		Vector3 mousePos = Input.mousePosition;
		Tile currentTile = null;
		while (GridManager.targetTileSelectMode)
		{
			if (Vector3.Distance(mousePos, Input.mousePosition) > 5f)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, ref hit, float.PositiveInfinity))
				{
					Tile tile = hit.collider.gameObject.GetComponent<Tile>();
					if (tile != currentTile)
					{
						currentTile = tile;
						GridManager.OnHoverTargetTileExit();
						if (tile != null)
						{
							GridManager.OnHoverTargetTileSelect(tile);
						}
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CC1 RID: 11457 RVA: 0x001597C8 File Offset: 0x001579C8
	public static void PlaceIndicatorH()
	{
		if (GridManager.instance.indicatorH.Length < GridManager.hostileList.Count)
		{
			foreach (Transform transform in GridManager.instance.indicatorH)
			{
				if (!(transform == GridManager.instance.indicatorHostile))
				{
					Object.Destroy(transform.gameObject);
				}
			}
			GridManager.instance.indicatorHostile.position = new Vector3(0f, 9999f, 0f);
			GridManager.instance.indicatorHostile.localEulerAngles = new Vector3(0f, 0f, 0f);
			int allUnitCount = UnitControl.GetAllUnitCount();
			if (allUnitCount > 0)
			{
				GridManager.instance.indicatorH = new Transform[allUnitCount + 2];
				GridManager.instance.indicatorH[0] = GridManager.instance.indicatorHostile;
				for (int j = 1; j < allUnitCount + 2; j++)
				{
					GridManager.instance.indicatorH[j] = (Transform)Object.Instantiate(GridManager.instance.indicatorHostile);
					GridManager.instance.indicatorH[j].parent = GridManager.instance.indicatorH[0].parent;
					GridManager.instance.indicatorH[j].localEulerAngles = new Vector3(0f, 0f, 0f);
				}
			}
		}
		for (int k = 0; k < GridManager.hostileList.Count; k++)
		{
			GridManager.instance.indicatorH[k].localPosition = GridManager.hostileList[k].transform.localPosition;
		}
	}

	// Token: 0x06002CC2 RID: 11458 RVA: 0x0015997C File Offset: 0x00157B7C
	public static void ClearAllIndicatorH()
	{
		for (int i = 0; i < GridManager.instance.indicatorH.Length; i++)
		{
			GridManager.instance.indicatorH[i].position = new Vector3(0f, 9999f, 0f);
		}
	}

	// Token: 0x06002CC3 RID: 11459 RVA: 0x001599CC File Offset: 0x00157BCC
	public static void ClearIndicatorH(Tile tile)
	{
		for (int i = 0; i < GridManager.instance.indicatorH.Length; i++)
		{
			if (GridManager.instance.indicatorH[i].localPosition == tile.gameObject.transform.localPosition)
			{
				GridManager.instance.indicatorH[i].position = new Vector3(0f, 9999f, 0f);
			}
		}
	}

	// Token: 0x06002CC4 RID: 11460 RVA: 0x0001CE79 File Offset: 0x0001B079
	public static bool IsInLOS(Tile targetTile, Tile srcTile)
	{
		return GridManager.IsInLOS(targetTile.pos, srcTile.pos, false);
	}

	// Token: 0x06002CC5 RID: 11461 RVA: 0x0001CE8D File Offset: 0x0001B08D
	public static bool IsInLOS(Tile targetTile, Tile srcTile, bool debugging)
	{
		return GridManager.IsInLOS(targetTile.pos, srcTile.pos, debugging);
	}

	// Token: 0x06002CC6 RID: 11462 RVA: 0x0001CEA1 File Offset: 0x0001B0A1
	public static bool IsInLOS(Vector3 pos1, Vector3 pos2)
	{
		return GridManager.IsInLOS(pos1, pos2, false);
	}

	// Token: 0x06002CC7 RID: 11463 RVA: 0x00159A48 File Offset: 0x00157C48
	public static bool IsInLOS(Vector3 pos1, Vector3 pos2, bool debugging)
	{
		float num = Vector3.Distance(pos2, pos1);
		Vector3 normalized = (pos2 - pos1).normalized;
		Vector3 vector;
		vector..ctor(-normalized.z, 0f, normalized.x);
		Vector3 normalized2 = vector.normalized;
		LayerMask layerMask = 1 << LayerManager.GetLayerObstacle();
		float num2 = 1.5f;
		float num3 = 0.4f;
		if (GridManager.instance.type == _TileType.Square)
		{
			num3 = 0.6f;
		}
		RaycastHit[] array = Physics.RaycastAll(pos1, normalized, num, layerMask);
		bool flag = true;
		foreach (RaycastHit raycastHit in array)
		{
			Obstacle component = raycastHit.collider.gameObject.GetComponent<Obstacle>();
			if (component.coverType == _CoverType.BlockFull)
			{
				flag = false;
				break;
			}
		}
		if (debugging)
		{
			if (!flag)
			{
				Debug.DrawLine(pos1, pos2, Color.red, num2);
			}
			else
			{
				Debug.DrawLine(pos1, pos2, Color.white, num2);
			}
		}
		array = Physics.RaycastAll(pos1 + normalized2 * GridManager.GetTileSize() * num3, normalized, num, layerMask);
		bool flag2 = true;
		foreach (RaycastHit raycastHit2 in array)
		{
			Obstacle component2 = raycastHit2.collider.gameObject.GetComponent<Obstacle>();
			if (component2.coverType == _CoverType.BlockFull)
			{
				flag2 = false;
				break;
			}
		}
		if (debugging)
		{
			Vector3 vector2 = pos1 + normalized2 * GridManager.GetTileSize() * num3;
			Vector3 vector3 = pos2 + normalized2 * GridManager.GetTileSize() * num3;
			if (!flag2)
			{
				Debug.DrawLine(vector2, vector3, Color.red, num2);
			}
			else
			{
				Debug.DrawLine(vector2, vector3, Color.white, num2);
			}
		}
		array = Physics.RaycastAll(pos1 - normalized2 * GridManager.GetTileSize() * num3, normalized, num, layerMask);
		bool flag3 = true;
		foreach (RaycastHit raycastHit3 in array)
		{
			Obstacle component3 = raycastHit3.collider.gameObject.GetComponent<Obstacle>();
			if (component3.coverType == _CoverType.BlockFull)
			{
				flag3 = false;
				break;
			}
		}
		if (debugging)
		{
			Vector3 vector4 = pos1 - normalized2 * GridManager.GetTileSize() * num3;
			Vector3 vector5 = pos2 - normalized2 * GridManager.GetTileSize() * num3;
			if (!flag3)
			{
				Debug.DrawLine(vector4, vector5, Color.red, num2);
			}
			else
			{
				Debug.DrawLine(vector4, vector5, Color.white, num2);
			}
		}
		bool result = true;
		if (!flag && !flag2 && !flag3)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06002CC8 RID: 11464 RVA: 0x0001CEAB File Offset: 0x0001B0AB
	public static void LOS(Tile targetTile)
	{
		GridManager.IsInLOS(targetTile, GridManager.instance.allTiles[0]);
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x00159D50 File Offset: 0x00157F50
	public static List<Tile> GetTileInFan1(Tile tileOrigin, Tile targetTile, int range)
	{
		float num = Utility.VectorToAngle(new Vector2(targetTile.thisT.localPosition.x - tileOrigin.thisT.localPosition.x, targetTile.thisT.localPosition.z - tileOrigin.thisT.localPosition.z));
		num = Mathf.Round(num);
		if (num >= 0f && num < 60f)
		{
			num = 30f;
		}
		else if (num >= 60f && num < 120f)
		{
			num = 90f;
		}
		else if (num >= 120f && num < 180f)
		{
			num = 150f;
		}
		else if (num >= 180f && num < 240f)
		{
			num = 210f;
		}
		else if (num >= 240f && num < 300f)
		{
			num = 270f;
		}
		else
		{
			num = 330f;
		}
		List<Tile> aoetile = GridManager.GetAOETile(tileOrigin, range);
		List<Tile> list = new List<Tile>();
		for (int i = 0; i < aoetile.Count; i++)
		{
			Tile tile = aoetile[i];
			float num2 = Utility.VectorToAngle(new Vector2(tile.thisT.localPosition.x - tileOrigin.thisT.localPosition.x, tile.thisT.localPosition.z - tileOrigin.thisT.localPosition.z));
			num2 = Mathf.Round(num2);
			if (num2 >= 330f && num == 30f)
			{
				num2 -= 360f;
			}
			if (num2 <= 30f && num == 330f)
			{
				num2 += 360f;
			}
			if (Mathf.Abs(num2 - num) < 61f)
			{
				list.Add(tile);
			}
		}
		return list;
	}

	// Token: 0x06002CCA RID: 11466 RVA: 0x00159F64 File Offset: 0x00158164
	public static List<Tile> GetTileInCone60(Tile tileOrigin, Tile targetTile, int range)
	{
		float num = Utility.VectorToAngle(new Vector2(targetTile.thisT.localPosition.x - tileOrigin.thisT.localPosition.x, targetTile.thisT.localPosition.z - tileOrigin.thisT.localPosition.z));
		num = Mathf.Round(num);
		if (num >= 30f && num < 90f)
		{
			num = 60f;
		}
		else if (num >= 90f && num < 150f)
		{
			num = 120f;
		}
		else if (num >= 150f && num < 210f)
		{
			num = 180f;
		}
		else if (num >= 210f && num < 270f)
		{
			num = 240f;
		}
		else if (num >= 270f && num < 330f)
		{
			num = 300f;
		}
		else
		{
			num = 360f;
		}
		List<Tile> aoetile = GridManager.GetAOETile(tileOrigin, range);
		List<Tile> list = new List<Tile>();
		for (int i = 0; i < aoetile.Count; i++)
		{
			Tile tile = aoetile[i];
			float num2 = Utility.VectorToAngle(new Vector2(tile.thisT.localPosition.x - tileOrigin.thisT.localPosition.x, tile.thisT.localPosition.z - tileOrigin.thisT.localPosition.z));
			num2 = Mathf.Round(num2);
			if (num2 <= 30f && num == 360f)
			{
				num2 += 360f;
			}
			if (Mathf.Abs(num2 - num) < 31f)
			{
				list.Add(tile);
			}
		}
		return list;
	}

	// Token: 0x06002CCB RID: 11467 RVA: 0x0015A154 File Offset: 0x00158354
	public static List<Tile> GetTileInLine(Tile tileOrigin, Tile targetTile, int range)
	{
		Vector2 dir;
		dir..ctor(targetTile.thisT.localPosition.x - tileOrigin.thisT.localPosition.x, targetTile.thisT.localPosition.z - tileOrigin.thisT.localPosition.z);
		float num = Utility.VectorToAngle(dir);
		num = Mathf.Round(num);
		List<Tile> list = new List<Tile>();
		Tile tile = tileOrigin;
		int i = 0;
		while (i < range)
		{
			bool flag = false;
			List<Tile> neighbours = tile.GetNeighbours();
			for (int j = 0; j < neighbours.Count; j++)
			{
				Tile tile2 = neighbours[j];
				Vector2 dir2;
				dir2..ctor(tile2.thisT.localPosition.x - tile.thisT.localPosition.x, tile2.thisT.localPosition.z - tile.thisT.localPosition.z);
				float num2 = Utility.VectorToAngle(dir2);
				num2 = Mathf.Round(num2);
				if (num2 == num)
				{
					flag = true;
					list.Add(tile2);
					tile = tile2;
					i++;
					Debug.DrawLine(tile.pos, tile.pos + new Vector3(0f, 2f, 0f), Color.red, 0.1f);
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		return list;
	}

	// Token: 0x06002CCC RID: 11468 RVA: 0x0015A2E4 File Offset: 0x001584E4
	public static Tile GetChargeFrontTile(Tile tileOrigin, Tile targetTile)
	{
		int num = GridManager.Distance(tileOrigin, targetTile);
		if (num <= 1)
		{
			return null;
		}
		Tile result = null;
		int num2 = num;
		float num3 = 365f;
		Quaternion quaternion = Quaternion.LookRotation(targetTile.pos - tileOrigin.pos);
		List<Tile> list = new List<Tile>();
		list = GridManager.GetTilesWithinRange(targetTile, num2 - 1);
		foreach (Tile tile in list)
		{
			int num4 = GridManager.Distance(targetTile, tile);
			Quaternion quaternion2 = Quaternion.LookRotation(targetTile.pos - tile.pos);
			float num5 = Quaternion.Angle(quaternion, quaternion2);
			if (num5 <= 90f)
			{
				if (!(tile.unit != null))
				{
					if (tile.walkable)
					{
						if (tile.invisible)
						{
							if (!tile.bUnitOrder)
							{
								if (num2 > num4)
								{
									num2 = num4;
									num3 = num5;
									result = tile;
								}
								else if (num3 > num5 && num2 == num4)
								{
									num3 = num5;
									result = tile;
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06002CCD RID: 11469 RVA: 0x0015A434 File Offset: 0x00158634
	public static Tile GetChargeBackTile(Tile tileOrigin, Tile targetTile)
	{
		Tile result = null;
		int num = 3;
		float num2 = 365f;
		Quaternion quaternion = Quaternion.LookRotation(targetTile.pos - tileOrigin.pos);
		List<Tile> list = new List<Tile>();
		list = GridManager.GetTilesWithinRange(targetTile, num - 1);
		foreach (Tile tile in list)
		{
			int num3 = GridManager.Distance(targetTile, tile);
			Quaternion quaternion2 = Quaternion.LookRotation(tile.pos - targetTile.pos);
			float num4 = Quaternion.Angle(quaternion, quaternion2);
			if (num4 <= 90f)
			{
				if (!(tile.unit != null))
				{
					if (tile.walkable)
					{
						if (tile.invisible)
						{
							if (!tile.bUnitOrder)
							{
								if (num > num3)
								{
									num = num3;
									num2 = num4;
									result = tile;
								}
								else if (num2 > num4 && num == num3)
								{
									num2 = num4;
									result = tile;
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06002CCE RID: 11470 RVA: 0x0015A570 File Offset: 0x00158770
	public static Tile GetKnockBackTile(Tile tileOrigin, Tile targetTile, int iRange)
	{
		if (iRange < 1)
		{
			iRange = 1;
		}
		int num = 0;
		float num2 = 365f;
		Tile result = null;
		Quaternion quaternion = Quaternion.LookRotation(targetTile.pos - tileOrigin.pos);
		List<Tile> list = new List<Tile>();
		list = GridManager.GetTilesWithinRange(targetTile, iRange);
		foreach (Tile tile in list)
		{
			int num3 = GridManager.Distance(targetTile, tile);
			Quaternion quaternion2 = Quaternion.LookRotation(tile.pos - targetTile.pos);
			float num4 = Quaternion.Angle(quaternion, quaternion2);
			if (num4 <= 35f)
			{
				if (!(tile.unit != null) || tile.unit.bSpeicalAction)
				{
					if (tile.walkable)
					{
						if (tile.invisible)
						{
							if (!tile.bUnitOrder)
							{
								if (num < num3)
								{
									num = num3;
									num2 = num4;
									result = tile;
								}
								else if (num2 > num4 && num == num3)
								{
									num2 = num4;
									result = tile;
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06002CCF RID: 11471 RVA: 0x0015A6C4 File Offset: 0x001588C4
	public static Tile GetPullCloseTile(Tile tileOrigin, Tile targetTile)
	{
		Vector2 dir;
		dir..ctor(targetTile.thisT.localPosition.x - tileOrigin.thisT.localPosition.x, targetTile.thisT.localPosition.z - tileOrigin.thisT.localPosition.z);
		float num = Utility.VectorToAngle(dir);
		num = Mathf.Round(num);
		if (num >= 0f && num < 60f)
		{
			num = 30f;
		}
		else if (num >= 60f && num < 120f)
		{
			num = 90f;
		}
		else if (num >= 120f && num < 180f)
		{
			num = 150f;
		}
		else if (num >= 180f && num < 240f)
		{
			num = 210f;
		}
		else if (num >= 240f && num < 300f)
		{
			num = 270f;
		}
		else
		{
			num = 330f;
		}
		int num2 = GridManager.Distance(tileOrigin, targetTile);
		if (num2 <= 1)
		{
			return null;
		}
		int num3 = 0;
		while (num3 + 1 < num2)
		{
			float num4 = 180f;
			Tile tile = null;
			List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(tileOrigin, num3 + 1, num3 + 1);
			for (int i = 0; i < tilesWithinRange.Count; i++)
			{
				Tile tile2 = tilesWithinRange[i];
				float num5 = Utility.VectorToAngle(new Vector2(tile2.thisT.localPosition.x - tileOrigin.thisT.localPosition.x, tile2.thisT.localPosition.z - tileOrigin.thisT.localPosition.z));
				num5 = Mathf.Round(num5);
				if (num5 >= 330f && num == 30f)
				{
					num5 -= 360f;
				}
				if (num5 <= 30f && num == 330f)
				{
					num5 += 360f;
				}
				float num6 = Mathf.Abs(num5 - num);
				if (num6 < 60.5f)
				{
					if (!(tile2.unit != null))
					{
						if (tile2.walkable)
						{
							if (tile2.invisible)
							{
								if (!tile2.bUnitOrder)
								{
									if (num6 < num4)
									{
										num4 = num6;
										tile = tile2;
									}
								}
							}
						}
					}
				}
			}
			if (tile != null)
			{
				return tile;
			}
			num3++;
		}
		return null;
	}

	// Token: 0x06002CD0 RID: 11472 RVA: 0x0001CEC4 File Offset: 0x0001B0C4
	public static List<Tile> GetAOETile(Tile tile, int range)
	{
		return GridManager.GetTilesWithinRange(tile, range);
	}

	// Token: 0x06002CD1 RID: 11473 RVA: 0x0001CECD File Offset: 0x0001B0CD
	public static List<Tile> GetTilesWithinRange(Tile rootHT, int range)
	{
		return GridManager.GetTilesWithinRange(rootHT, 1, range);
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x0015A978 File Offset: 0x00158B78
	public static List<Tile> GetTilesWithinRange(Tile rootHT, int rangeMin, int rangeMax)
	{
		if (rangeMin == 0 && rangeMax == 0)
		{
			return new List<Tile>();
		}
		List<Tile> neighbours = rootHT.GetNeighbours();
		if (rangeMax == 1)
		{
			List<Tile> list = new List<Tile>();
			for (int i = 0; i < neighbours.Count; i++)
			{
				list.Add(neighbours[i]);
			}
			return list;
		}
		List<Tile> list2 = new List<Tile>();
		List<Tile> list3 = new List<Tile>();
		List<Tile> list4 = new List<Tile>();
		foreach (Tile tile in neighbours)
		{
			if (!list4.Contains(tile))
			{
				list4.Add(tile);
			}
		}
		for (int j = 0; j < rangeMax; j++)
		{
			list3 = list4;
			list4 = new List<Tile>();
			foreach (Tile tile2 in list3)
			{
				foreach (Tile tile3 in tile2.GetNeighbours())
				{
					if (!list2.Contains(tile3) && !list3.Contains(tile3) && !list4.Contains(tile3))
					{
						list4.Add(tile3);
					}
				}
			}
			foreach (Tile tile4 in list3)
			{
				if (tile4 != rootHT && !list2.Contains(tile4))
				{
					list2.Add(tile4);
				}
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			Tile srcTile = list2[k];
			if (GridManager.Distance(srcTile, rootHT) < rangeMin)
			{
				list2.RemoveAt(k);
				k--;
			}
		}
		return list2;
	}

	// Token: 0x06002CD3 RID: 11475 RVA: 0x0015ABC0 File Offset: 0x00158DC0
	public static List<Tile> GetWalkableTilesWithinRangeH(Tile rootHT, int range)
	{
		if (range == 0)
		{
			return new List<Tile>();
		}
		List<Tile> neighbours = rootHT.GetNeighbours();
		if (range == 1)
		{
			List<Tile> list = new List<Tile>();
			for (int i = 0; i < neighbours.Count; i++)
			{
				if (neighbours[i].walkable && neighbours[i].unit != null && GridManager.CheckTileMovablilityToHeight(rootHT, neighbours[i], range))
				{
					list.Add(neighbours[i]);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].rangeRequired = 0;
			}
			return list;
		}
		List<Tile> list2 = new List<Tile>();
		List<Tile> list3 = new List<Tile>();
		List<Tile> list4 = new List<Tile>();
		foreach (Tile tile in neighbours)
		{
			if (tile.walkable && tile.unit == null && !list4.Contains(tile) && GridManager.CheckTileMovablilityToHeight(rootHT, tile, range))
			{
				list4.Add(tile);
			}
		}
		for (int k = 0; k < range; k++)
		{
			list3 = list4;
			list4 = new List<Tile>();
			foreach (Tile tile2 in list3)
			{
				foreach (Tile tile3 in tile2.GetNeighbours())
				{
					if (tile3.walkable && tile3.unit == null && !list4.Contains(tile3) && !list2.Contains(tile3) && !list3.Contains(tile3) && GridManager.CheckTileMovablilityToHeight(tile2, tile3, range))
					{
						list4.Add(tile3);
					}
				}
			}
			foreach (Tile tile4 in list3)
			{
				if (tile4 != rootHT && !list2.Contains(tile4))
				{
					list2.Add(tile4);
				}
			}
		}
		for (int l = 0; l < list2.Count; l++)
		{
			list2[l].rangeRequired = 0;
		}
		for (int m = 0; m < list3.Count; m++)
		{
			list3[m].rangeRequired = 0;
		}
		for (int n = 0; n < list4.Count; n++)
		{
			list4[n].rangeRequired = 0;
		}
		return list2;
	}

	// Token: 0x06002CD4 RID: 11476 RVA: 0x0015AF00 File Offset: 0x00159100
	public static bool CheckTileMovablilityToHeight(Tile srcTile, Tile tgtTile, int range)
	{
		int moveCostH = GameControlTB.GetMoveCostH();
		if (srcTile.heightLevel != tgtTile.heightLevel)
		{
			int num = 1 + Mathf.Abs(srcTile.heightLevel - tgtTile.heightLevel) * moveCostH;
			tgtTile.rangeRequired = num + srcTile.rangeRequired;
			if (tgtTile.rangeRequired <= range)
			{
				return true;
			}
		}
		else
		{
			tgtTile.rangeRequired = 1 + srcTile.rangeRequired;
			if (tgtTile.rangeRequired <= range)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002CD5 RID: 11477 RVA: 0x0015AF7C File Offset: 0x0015917C
	public static List<Tile> GetWalkableTilesWithinRange(Tile rootHT, int range)
	{
		if (range == 0)
		{
			return new List<Tile>();
		}
		List<Tile> neighbours = rootHT.GetNeighbours();
		if (range == 1)
		{
			List<Tile> list = new List<Tile>();
			for (int i = 0; i < neighbours.Count; i++)
			{
				if (neighbours[i].walkable && neighbours[i].unit == null)
				{
					list.Add(neighbours[i]);
				}
			}
			return list;
		}
		UnitTB unit = rootHT.unit;
		float num = 0f;
		int num2 = 0;
		if (unit != null)
		{
			num = unit.GetFlyOnGrass();
			num2 = unit.factionID;
		}
		List<Tile> list2 = new List<Tile>();
		List<Tile> list3 = new List<Tile>();
		List<Tile> list4 = new List<Tile>();
		foreach (Tile tile in neighbours)
		{
			if (!(tile.unit != null) || tile.unit.CheckFriendFaction(num2) || tile.unit.bStealth)
			{
				if (tile.walkable && !list4.Contains(tile))
				{
					list4.Add(tile);
				}
			}
		}
		for (int j = 0; j < range; j++)
		{
			list3 = list4;
			list4 = new List<Tile>();
			foreach (Tile tile2 in list3)
			{
				if (num >= tile2.GetZoneControl(num2))
				{
					foreach (Tile tile3 in tile2.GetNeighbours())
					{
						if (!(tile3.unit != null) || tile3.unit.CheckFriendFaction(num2) || tile3.unit.bStealth)
						{
							if (tile3.walkable && !list4.Contains(tile3))
							{
								list4.Add(tile3);
							}
						}
					}
				}
			}
			foreach (Tile tile4 in list3)
			{
				if (tile4 != rootHT && !list2.Contains(tile4))
				{
					list2.Add(tile4);
				}
			}
		}
		int k = 0;
		while (k < list2.Count)
		{
			if (list2[k].unit != null)
			{
				list2.RemoveAt(k);
			}
			else
			{
				k++;
			}
		}
		return list2;
	}

	// Token: 0x06002CD6 RID: 11478 RVA: 0x0015B2A0 File Offset: 0x001594A0
	public List<Tile> GetTileWithinRect(Rect rect)
	{
		if (rect.width == 0f || rect.height == 0f)
		{
			return new List<Tile>();
		}
		if (rect.width < 0f)
		{
			rect.width *= -1f;
			rect.x -= rect.width;
		}
		if (rect.height < 0f)
		{
			rect.height *= -1f;
			rect.y -= rect.height;
		}
		this.GetAllTile();
		List<Tile> list = new List<Tile>();
		foreach (Tile tile in this.allTiles)
		{
			Vector2 vector;
			vector..ctor(tile.transform.position.x, tile.transform.position.z);
			if (rect.Contains(vector))
			{
				list.Add(tile);
			}
		}
		return list;
	}

	// Token: 0x06002CD7 RID: 11479 RVA: 0x0015B3E0 File Offset: 0x001595E0
	public static Tile GetNearestTile(Vector3 pos)
	{
		pos.y = GridManager.instance.baseHeight;
		Tile result = null;
		float num = float.PositiveInfinity;
		foreach (Tile tile in GridManager.instance.allTiles)
		{
			float num2 = Vector3.Distance(tile.pos, pos);
			if (num2 < num)
			{
				num = num2;
				result = tile;
			}
		}
		return result;
	}

	// Token: 0x06002CD8 RID: 11480 RVA: 0x0001CED7 File Offset: 0x0001B0D7
	public static int Distance(Tile srcTile, Tile targetTile)
	{
		return AStar.Distance(srcTile, targetTile, false);
	}

	// Token: 0x06002CD9 RID: 11481 RVA: 0x0001CEE1 File Offset: 0x0001B0E1
	public static int WalkDistance(Tile srcTile, Tile targetTile)
	{
		return AStar.Distance(srcTile, targetTile, true);
	}

	// Token: 0x06002CDA RID: 11482 RVA: 0x0001CEEB File Offset: 0x0001B0EB
	public static List<Tile> GetAllTiles()
	{
		return GridManager.instance.allTiles;
	}

	// Token: 0x06002CDB RID: 11483 RVA: 0x0001CEF7 File Offset: 0x0001B0F7
	public static float GetTileSize()
	{
		return GridManager.instance.gridSize * GridManager.instance.gridToTileSizeRatio;
	}

	// Token: 0x06002CDC RID: 11484 RVA: 0x0001CF0E File Offset: 0x0001B10E
	public static float GetBaseHeight()
	{
		return GridManager.instance.baseHeight;
	}

	// Token: 0x06002CDD RID: 11485 RVA: 0x0001CF1A File Offset: 0x0001B11A
	public static float GetTerrainHeightOffset()
	{
		return GridManager.instance.terrainHeightOffset;
	}

	// Token: 0x06002CDE RID: 11486 RVA: 0x0015B46C File Offset: 0x0015966C
	private void OnDrawGizmos()
	{
		if (this.showGizmo)
		{
			Gizmos.color = Color.white;
			for (int i = 0; i < this.playerPlacementAreas.Count; i++)
			{
				Vector3 vector;
				vector..ctor(this.playerPlacementAreas[i].x, this.baseHeight, this.playerPlacementAreas[i].y + this.playerPlacementAreas[i].height);
				Vector3 vector2;
				vector2..ctor(this.playerPlacementAreas[i].x + this.playerPlacementAreas[i].width, this.baseHeight, this.playerPlacementAreas[i].y + this.playerPlacementAreas[i].height);
				Vector3 vector3;
				vector3..ctor(this.playerPlacementAreas[i].x + this.playerPlacementAreas[i].width, this.baseHeight, this.playerPlacementAreas[i].y);
				Vector3 vector4;
				vector4..ctor(this.playerPlacementAreas[i].x, this.baseHeight, this.playerPlacementAreas[i].y);
				Gizmos.DrawLine(vector, vector2);
				Gizmos.DrawLine(vector2, vector3);
				Gizmos.DrawLine(vector3, vector4);
				Gizmos.DrawLine(vector4, vector);
			}
			foreach (Faction faction in this.factionList)
			{
				Gizmos.color = faction.color;
				foreach (FactionSpawnInfo factionSpawnInfo in faction.spawnInfo)
				{
					Vector3 vector5;
					vector5..ctor(factionSpawnInfo.area.x, this.baseHeight, factionSpawnInfo.area.y + factionSpawnInfo.area.height);
					Vector3 vector6;
					vector6..ctor(factionSpawnInfo.area.x + factionSpawnInfo.area.width, this.baseHeight, factionSpawnInfo.area.y + factionSpawnInfo.area.height);
					Vector3 vector7;
					vector7..ctor(factionSpawnInfo.area.x + factionSpawnInfo.area.width, this.baseHeight, factionSpawnInfo.area.y);
					Vector3 vector8;
					vector8..ctor(factionSpawnInfo.area.x, this.baseHeight, factionSpawnInfo.area.y);
					Gizmos.DrawLine(vector5, vector6);
					Gizmos.DrawLine(vector6, vector7);
					Gizmos.DrawLine(vector7, vector8);
					Gizmos.DrawLine(vector8, vector5);
				}
			}
		}
	}

	// Token: 0x06002CDF RID: 11487 RVA: 0x0015B79C File Offset: 0x0015999C
	public Tile GetFactionTile(int iFaction)
	{
		List<Tile> list = new List<Tile>();
		List<Tile> list2 = new List<Tile>();
		for (int i = 0; i < this.allTiles.Count; i++)
		{
			Tile tile = this.allTiles[i];
			if (!(tile == null))
			{
				if (tile.placementID >= 0)
				{
					list2.Add(tile);
					if (tile.placementID == iFaction)
					{
						list.Add(tile);
						if (!tile.bUnitOrder)
						{
							if (!(tile.unit != null))
							{
								return tile;
							}
						}
					}
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			Tile tile = list[j];
			for (int k = 0; k < tile.neighbours.Count; k++)
			{
				Tile tile2 = tile.neighbours[k];
				if (tile2.invisible)
				{
					if (tile2.walkable)
					{
						if (!tile2.bUnitOrder)
						{
							if (!(tile2.unit != null))
							{
								return tile2;
							}
						}
					}
				}
			}
		}
		Debug.LogWarning("陣營已無出生點 隨機擺放");
		for (int l = 0; l < list2.Count; l++)
		{
			Tile tile = list2[l];
			if (tile.invisible)
			{
				if (tile.walkable)
				{
					if (tile.unit == null)
					{
						return tile;
					}
					for (int m = 0; m < tile.neighbours.Count; m++)
					{
						Tile tile3 = tile.neighbours[m];
						if (tile3.invisible)
						{
							if (tile3.walkable)
							{
								if (!tile3.bUnitOrder)
								{
									if (!(tile3.unit != null))
									{
										return tile3;
									}
								}
							}
						}
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06002CE0 RID: 11488 RVA: 0x0015B9C8 File Offset: 0x00159BC8
	public Tile GetTileByName(string strTileName)
	{
		foreach (Tile tile in this.allTiles)
		{
			if (!(tile.name != strTileName))
			{
				return tile;
			}
		}
		return null;
	}

	// Token: 0x06002CE1 RID: 11489 RVA: 0x0015BA3C File Offset: 0x00159C3C
	public Tile GetNoUnitOnTileByName(string strTileName)
	{
		foreach (Tile tile in this.allTiles)
		{
			if (!(tile.name != strTileName))
			{
				if (tile.unit == null && !tile.bUnitOrder && tile.walkable && tile.invisible)
				{
					return tile;
				}
				for (int i = 0; i < tile.neighbours.Count; i++)
				{
					Tile tile2 = tile.neighbours[i];
					if (tile2.invisible)
					{
						if (tile2.walkable)
						{
							if (!tile2.bUnitOrder)
							{
								if (!(tile2.unit != null))
								{
									return tile2;
								}
							}
						}
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06002CE2 RID: 11490 RVA: 0x0015BB5C File Offset: 0x00159D5C
	public Tile GetDirectNearTileInGroup(List<Tile> targetList, Tile origTile, Vector2 dir)
	{
		if (Camera.main == null)
		{
			return null;
		}
		if (dir.x == 0f && dir.y == 0f)
		{
			return null;
		}
		if (targetList == null)
		{
			return null;
		}
		if (origTile == null)
		{
			return null;
		}
		Tile result = null;
		int num = int.MaxValue;
		float num2 = 0f;
		Vector3 vector = Camera.main.transform.TransformDirection(new Vector3(dir.x, 0f, dir.y));
		vector.y = 0f;
		Quaternion quaternion = Quaternion.LookRotation(vector);
		foreach (Tile tile in targetList)
		{
			if (!(tile == origTile))
			{
				if (!(tile == null))
				{
					Vector3 vector2 = tile.pos - origTile.pos;
					int num3 = GridManager.Distance(origTile, tile);
					if (num3 <= num)
					{
						vector2.y = 0f;
						Quaternion quaternion2 = Quaternion.LookRotation(vector2);
						float num4 = Quaternion.Angle(quaternion, quaternion2);
						if (num4 <= 45.5f)
						{
							if (num3 < num)
							{
								num = num3;
								num2 = num4;
								result = tile;
							}
							else if (num4 < num2)
							{
								num2 = num4;
								result = tile;
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x040038F9 RID: 14585
	public _TileType type;

	// Token: 0x040038FA RID: 14586
	public float gridSize = 2.5f;

	// Token: 0x040038FB RID: 14587
	public float width = 7f;

	// Token: 0x040038FC RID: 14588
	public float length = 11f;

	// Token: 0x040038FD RID: 14589
	public float baseHeight;

	// Token: 0x040038FE RID: 14590
	public float terrainHeightOffset = 0.02f;

	// Token: 0x040038FF RID: 14591
	public float gridToTileSizeRatio = 1f;

	// Token: 0x04003900 RID: 14592
	public float unwalkableRate;

	// Token: 0x04003901 RID: 14593
	public bool useObstacle;

	// Token: 0x04003902 RID: 14594
	[HideInInspector]
	public bool enableInvUnwalkable;

	// Token: 0x04003903 RID: 14595
	[HideInInspector]
	public bool enableVUnwalkable = true;

	// Token: 0x04003904 RID: 14596
	[HideInInspector]
	public List<Obstacle> obstaclePrefabList = new List<Obstacle>();

	// Token: 0x04003905 RID: 14597
	public bool addColletible;

	// Token: 0x04003906 RID: 14598
	public int minCollectibleCount = 1;

	// Token: 0x04003907 RID: 14599
	public int maxCollectibleCount = 3;

	// Token: 0x04003908 RID: 14600
	[HideInInspector]
	public List<CollectibleTB> collectiblePrefabList = new List<CollectibleTB>();

	// Token: 0x04003909 RID: 14601
	[HideInInspector]
	public Transform indicatorSelect;

	// Token: 0x0400390A RID: 14602
	[HideInInspector]
	public Transform indicatorCursor;

	// Token: 0x0400390B RID: 14603
	[HideInInspector]
	public Transform indicatorHostile;

	// Token: 0x0400390C RID: 14604
	private Transform[] indicatorH;

	// Token: 0x0400390D RID: 14605
	private int MAX_INDICATOR_HOSTILE = 37;

	// Token: 0x0400390E RID: 14606
	public static List<Tile> walkableList = new List<Tile>();

	// Token: 0x0400390F RID: 14607
	public static List<Tile> hostileList = new List<Tile>();

	// Token: 0x04003910 RID: 14608
	public GameObject hexTilePrefab;

	// Token: 0x04003911 RID: 14609
	public GameObject squareTilePrefab;

	// Token: 0x04003912 RID: 14610
	[HideInInspector]
	public List<Faction> factionList = new List<Faction>();

	// Token: 0x04003913 RID: 14611
	[HideInInspector]
	private List<Tile> allTiles = new List<Tile>();

	// Token: 0x04003914 RID: 14612
	[HideInInspector]
	public List<Tile> allPlaceableTiles = new List<Tile>();

	// Token: 0x04003915 RID: 14613
	public List<Rect> playerPlacementAreas = new List<Rect>();

	// Token: 0x04003916 RID: 14614
	public Tile LastHoverTile;

	// Token: 0x04003917 RID: 14615
	public static GridManager instance;

	// Token: 0x04003918 RID: 14616
	public Tile nowCursorTile;

	// Token: 0x04003919 RID: 14617
	private float ftime;

	// Token: 0x0400391A RID: 14618
	private float ftick = 0.2f;

	// Token: 0x0400391B RID: 14619
	private Tile selectedTile;

	// Token: 0x0400391C RID: 14620
	private static List<Tile> currentAOETileGroup = new List<Tile>();

	// Token: 0x0400391D RID: 14621
	public static List<Tile> tempHostileTileList = new List<Tile>();

	// Token: 0x0400391E RID: 14622
	private static UnitAbility currentUAB = null;

	// Token: 0x0400391F RID: 14623
	private static Tile uABScrTile = null;

	// Token: 0x04003920 RID: 14624
	private static bool targetTileSelectMode = false;

	// Token: 0x04003921 RID: 14625
	private static List<Tile> tilesInAbilityRange = new List<Tile>();

	// Token: 0x04003922 RID: 14626
	public bool showGizmo = true;

	// Token: 0x02000750 RID: 1872
	// (Invoke) Token: 0x06002CE4 RID: 11492
	public delegate void HoverTileEnterHandler(Tile tile, UnitAbility uab);

	// Token: 0x02000751 RID: 1873
	// (Invoke) Token: 0x06002CE8 RID: 11496
	public delegate void HoverTileExitHandler();
}
