using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200076E RID: 1902
public class Tile : MonoBehaviour
{
	// Token: 0x1400004C RID: 76
	// (add) Token: 0x06002D24 RID: 11556 RVA: 0x0001D183 File Offset: 0x0001B383
	// (remove) Token: 0x06002D25 RID: 11557 RVA: 0x0001D19A File Offset: 0x0001B39A
	public static event Tile.OnShowUnitInfoHandler onShowUnitInfoE;

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x06002D26 RID: 11558 RVA: 0x0001D1B1 File Offset: 0x0001B3B1
	// (remove) Token: 0x06002D27 RID: 11559 RVA: 0x0001D1C8 File Offset: 0x0001B3C8
	public static event Tile.TileSetUnit onSetUnit;

	// Token: 0x06002D28 RID: 11560 RVA: 0x0001D1DF File Offset: 0x0001B3DF
	public void AdjustHeight()
	{
		this.AdjustHeight(GridManager.GetBaseHeight(), GridManager.GetTerrainHeightOffset());
	}

	// Token: 0x06002D29 RID: 11561 RVA: 0x0015C5D0 File Offset: 0x0015A7D0
	public void AdjustHeight(float baseHeight, float heightOffset)
	{
		if (this.obstacle != null)
		{
			this.obstacle.transform.parent = null;
		}
		Vector3 vector;
		vector..ctor(base.transform.position.x, baseHeight + 5000f, base.transform.position.z);
		LayerMask layerMask = 2048;
		RaycastHit raycastHit;
		if (Physics.Raycast(vector, -Vector3.up, ref raycastHit, float.PositiveInfinity, layerMask))
		{
			base.transform.position = new Vector3(vector.x, raycastHit.point.y + heightOffset, vector.z);
		}
		else
		{
			base.transform.position = new Vector3(vector.x, baseHeight, vector.z);
		}
		this.pos = base.transform.position;
		if (this.unit != null)
		{
			this.unit.transform.position = this.pos;
		}
		if (this.collectible != null)
		{
			this.collectible.transform.position = this.pos;
		}
		if (this.obstacle != null)
		{
			this.obstacle.transform.parent = base.transform;
		}
	}

	// Token: 0x06002D2A RID: 11562 RVA: 0x0015C73C File Offset: 0x0015A93C
	public void SetupNeighbour()
	{
		float tileSize = GridManager.GetTileSize();
		Vector3 vector;
		vector..ctor(base.transform.position.x, GridManager.GetBaseHeight(), base.transform.position.z);
		Collider[] array = Physics.OverlapSphere(vector, tileSize * 1f);
		List<Tile> list = new List<Tile>();
		foreach (Collider collider in array)
		{
			Tile component = collider.gameObject.GetComponent<Tile>();
			if (component != null && component != this)
			{
				list.Add(component);
			}
		}
		this.SetNeighbours(list);
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x0001D1F1 File Offset: 0x0001B3F1
	public virtual void Awake()
	{
		this.thisT = base.transform;
	}

	// Token: 0x06002D2C RID: 11564 RVA: 0x0015C7F4 File Offset: 0x0015A9F4
	public virtual void Start()
	{
		base.gameObject.layer = LayerManager.GetLayerTile();
		this.pos = base.transform.position;
		if (!this.walkable)
		{
			if (this.project == null)
			{
				base.renderer.material = this.matUnwalkable;
			}
			else
			{
				this.project.material = this.matUnwalkable;
			}
		}
		else if (this.project == null)
		{
			base.renderer.material = null;
		}
		else
		{
			this.project.material = null;
		}
		if (this.invisible)
		{
			if (this.project == null)
			{
				base.renderer.enabled = true;
			}
			else
			{
				this.project.enabled = true;
			}
		}
		else if (this.project == null)
		{
			base.renderer.enabled = false;
		}
		else
		{
			this.project.enabled = false;
		}
		if (this.unit != null)
		{
			this.unit.occupiedTile = this;
		}
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x0015C924 File Offset: 0x0015AB24
	public void SetToDefault()
	{
		this.walkable = true;
		this.invisible = true;
		this.SetState(_TileState.Default);
		if (this.project == null)
		{
			base.renderer.enabled = true;
		}
		else
		{
			this.project.enabled = true;
		}
		this.placementID = -1;
		this.openForPlacement = false;
	}

	// Token: 0x06002D2E RID: 11566 RVA: 0x0015C984 File Offset: 0x0015AB84
	public void SetToWalkable()
	{
		this.walkable = true;
		this.invisible = true;
		this.SetState(_TileState.Walkable);
		if (this.project == null)
		{
			base.renderer.enabled = true;
		}
		else
		{
			this.project.enabled = true;
		}
		this.openForPlacement = true;
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x0015C9DC File Offset: 0x0015ABDC
	public void SetToUnwalkable(bool flag)
	{
		this.walkable = false;
		this.invisible = flag;
		if (this.project == null)
		{
			base.renderer.material = this.matUnwalkable;
		}
		else
		{
			this.project.material = this.matUnwalkable;
		}
		if (flag)
		{
			if (this.project == null)
			{
				base.renderer.enabled = true;
			}
			else
			{
				this.project.enabled = true;
			}
		}
		else if (this.project == null)
		{
			base.renderer.enabled = false;
		}
		else
		{
			this.project.enabled = false;
		}
		this.openForPlacement = false;
	}

	// Token: 0x06002D30 RID: 11568 RVA: 0x0001D1FF File Offset: 0x0001B3FF
	public void SetNeighbours(List<Tile> nn)
	{
		this.neighbours = nn;
	}

	// Token: 0x06002D31 RID: 11569 RVA: 0x0001D208 File Offset: 0x0001B408
	public List<Tile> GetNeighbours()
	{
		return this.neighbours;
	}

	// Token: 0x06002D32 RID: 11570 RVA: 0x0015CAA0 File Offset: 0x0015ACA0
	public void RemoveNeighbour(Tile tile)
	{
		if (tile == null)
		{
			return;
		}
		if (this.neighbours.Contains(tile))
		{
			this.neighbours.Remove(tile);
			this.disconnectedNeighbour.Add(tile);
		}
		if (tile.neighbours.Contains(this))
		{
			tile.neighbours.Remove(this);
			tile.disconnectedNeighbour.Add(this);
		}
	}

	// Token: 0x06002D33 RID: 11571 RVA: 0x0015CB10 File Offset: 0x0015AD10
	public void AddNeighbour(Tile tile)
	{
		if (tile == null)
		{
			return;
		}
		if (!this.neighbours.Contains(tile))
		{
			this.neighbours.Add(tile);
			this.disconnectedNeighbour.Remove(tile);
		}
		if (!tile.neighbours.Contains(this))
		{
			tile.neighbours.Add(this);
			tile.disconnectedNeighbour.Remove(this);
		}
	}

	// Token: 0x06002D34 RID: 11572 RVA: 0x0015CB80 File Offset: 0x0015AD80
	public void SetupWall()
	{
		for (int i = 0; i < this.walls.Count; i++)
		{
			if (this.walls[i].angle == 360f)
			{
				this.walls[i].angle = 0f;
			}
			for (int j = 0; j < this.neighbours.Count; j++)
			{
				Vector2 dir;
				dir..ctor(this.neighbours[j].pos.x - this.pos.x, this.neighbours[j].pos.z - this.pos.z);
				float num = Utility.VectorToAngle(dir);
				if ((int)num == (int)this.walls[i].angle)
				{
					Tile tile = this.neighbours[j];
					this.AddWall(num);
					float angle = (num <= 180f) ? (num + 180f) : (num - 180f);
					tile.AddWall(angle);
					j--;
				}
			}
		}
	}

	// Token: 0x06002D35 RID: 11573 RVA: 0x0001D210 File Offset: 0x0001B410
	public Tile AddWall(float angle)
	{
		return this.AddWall(angle, null);
	}

	// Token: 0x06002D36 RID: 11574 RVA: 0x0015CCA8 File Offset: 0x0015AEA8
	public Tile AddWall(float angle, Obstacle wallPrefab)
	{
		Tile tile = null;
		if (this.type == _TileType.Hex)
		{
			if (angle < 30f)
			{
				angle += 360f;
			}
			for (int i = 0; i < 6; i++)
			{
				if (angle > (float)(i * 60 + 30) && angle <= (float)((i + 1) * 60 + 30))
				{
					angle = (float)((i + 1) * 60);
				}
			}
		}
		else if (this.type == _TileType.Square)
		{
			if (angle < 45f)
			{
				angle += 360f;
			}
			for (int j = 0; j < 4; j++)
			{
				if (angle > (float)(j * 90 + 45) && angle <= (float)((j + 1) * 90 + 45))
				{
					angle = (float)((j + 1) * 90);
				}
			}
		}
		if (angle >= 360f)
		{
			angle -= 360f;
		}
		for (int k = 0; k < this.neighbours.Count; k++)
		{
			Vector2 dir;
			dir..ctor(this.neighbours[k].pos.x - this.pos.x, this.neighbours[k].pos.z - this.pos.z);
			float num = (float)((int)Utility.VectorToAngle(dir));
			if (this.type == _TileType.Hex)
			{
				if (Mathf.Abs(Mathf.DeltaAngle(angle, num)) < 30f)
				{
					tile = this.neighbours[k];
					break;
				}
			}
			else if (this.type == _TileType.Square && Mathf.Abs(Mathf.DeltaAngle(angle, num)) < 45f)
			{
				tile = this.neighbours[k];
				break;
			}
		}
		for (int l = 0; l < this.walls.Count; l++)
		{
			if (this.walls[l].Contains((int)angle))
			{
				Wall wall = this.walls[l];
				if (wall.tile1 == null || wall.tile2 == null)
				{
					wall.tile1 = this;
					wall.tile2 = tile;
				}
				if (tile != null)
				{
					tile.walls.Add(new Wall(wall.wallObj, tile, this, angle + 180f));
					this.RemoveNeighbour(tile);
				}
				return null;
			}
		}
		Transform transform = null;
		if (wallPrefab != null)
		{
			transform = (Transform)Object.Instantiate(wallPrefab.transform);
			Vector3 vector = Vector3.zero;
			if (tile != null)
			{
				vector = tile.pos - this.pos;
				vector.y = 0f;
			}
			else
			{
				vector..ctor(Mathf.Cos(angle * 0.017453292f), 0f, Mathf.Sin(angle * 0.017453292f));
			}
			if (this.type == _TileType.Hex)
			{
				float num2 = base.transform.localScale.x / 1.1628f;
				transform.position = this.pos + vector.normalized * num2 * 0.5f;
				transform.localScale = new Vector3(num2, num2, num2) * 1.2f;
			}
			else if (this.type == _TileType.Square)
			{
				float x = base.transform.localScale.x;
				transform.position = this.pos + vector.normalized * x * 0.5f;
				transform.localScale = new Vector3(x, x, x);
			}
			float num3 = 90f;
			transform.rotation = Quaternion.Euler(0f, -(angle + num3), 0f);
			transform.parent = base.transform;
		}
		this.walls.Add(new Wall(transform, tile, this, angle));
		if (tile != null)
		{
			tile.walls.Add(new Wall(transform, tile, this, angle + 180f));
			this.RemoveNeighbour(tile);
		}
		return tile;
	}

	// Token: 0x06002D37 RID: 11575 RVA: 0x0015D0D4 File Offset: 0x0015B2D4
	public Tile RemoveWall(float angle)
	{
		Wall wall = null;
		for (int i = 0; i < this.walls.Count; i++)
		{
			if (this.type == _TileType.Hex)
			{
				if (Mathf.Abs(Mathf.DeltaAngle(angle, this.walls[i].angle)) < 30f)
				{
					wall = this.walls[i];
					this.walls.RemoveAt(i);
					break;
				}
			}
			else if (this.type == _TileType.Square && Mathf.Abs(Mathf.DeltaAngle(angle, this.walls[i].angle)) < 45f)
			{
				wall = this.walls[i];
				this.walls.RemoveAt(i);
				break;
			}
		}
		if (wall == null)
		{
			return null;
		}
		Tile tile = null;
		if (wall.tile1 == this)
		{
			tile = wall.tile2;
		}
		else if (wall.tile2 == this)
		{
			tile = wall.tile1;
		}
		if (tile != null)
		{
			this.AddNeighbour(tile);
			for (int j = 0; j < tile.walls.Count; j++)
			{
				if (tile.walls[j].Contains(this, tile))
				{
					tile.walls.RemoveAt(j);
					break;
				}
			}
		}
		if (wall.wallObj != null)
		{
			Object.DestroyImmediate(wall.wallObj.gameObject);
		}
		return tile;
	}

	// Token: 0x06002D38 RID: 11576 RVA: 0x0001D21A File Offset: 0x0001B41A
	public void OnTouchMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.OnTouchMouseDown();
		}
		if (Input.GetMouseButtonDown(1))
		{
			this.OnRightClick();
		}
	}

	// Token: 0x06002D39 RID: 11577 RVA: 0x0001D23E File Offset: 0x0001B43E
	public void OnRightClick()
	{
		if (Tile.onShowUnitInfoE != null)
		{
			Tile.onShowUnitInfoE(this);
		}
	}

	// Token: 0x06002D3A RID: 11578 RVA: 0x0015D25C File Offset: 0x0015B45C
	public void OnTouchMouseEnter()
	{
		if (GameControlTB.IsCursorOnUI(Input.mousePosition))
		{
			return;
		}
		if (!this.walkable && !GridManager.IsInTargetTileSelectMode())
		{
			return;
		}
		GridManager.OnHoverEnter(this);
		if (this.unit != null)
		{
			UnitControl.hoveredUnit = this.unit;
		}
	}

	// Token: 0x06002D3B RID: 11579 RVA: 0x0001D255 File Offset: 0x0001B455
	public void OnTouchMouseExit()
	{
		GridManager.OnHoverExit();
		if (!this.walkable && !GridManager.IsInTargetTileSelectMode())
		{
			return;
		}
		this.SetToDefaultMat();
		if (this.unit != null)
		{
			UnitControl.hoveredUnit = null;
		}
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x0015D2B4 File Offset: 0x0015B4B4
	public void OnTouchMouseDown()
	{
		if (GameGlobal.m_bBattleTalk)
		{
			return;
		}
		if (GameControlTB.IsCursorOnUI(Input.mousePosition))
		{
			return;
		}
		if (GameControlTB.IsUnitPlacementState())
		{
			this.PlaceUnit();
			return;
		}
		if (!GameControlTB.IsPlayerTurn())
		{
			return;
		}
		if (GameControlTB.IsActionInProgress())
		{
			return;
		}
		if (!this.walkable && !GridManager.IsInTargetTileSelectMode())
		{
			return;
		}
		UnitTB selectedUnit = UnitControl.selectedUnit;
		if (selectedUnit != null && selectedUnit.IsControllable())
		{
			if (GridManager.IsInTargetTileSelectMode())
			{
				this.ManualSelect();
			}
			else if (!this.walkableToSelected && !this.attackableToSelected)
			{
				this.ManualSelect();
			}
			else if (!this.attackableToSelected || !(this.unit != null))
			{
				if (this.walkableToSelected)
				{
					selectedUnit.Move(this);
				}
				else
				{
					Debug.Log("error");
				}
			}
			return;
		}
		this.ManualSelect();
	}

	// Token: 0x06002D3D RID: 11581 RVA: 0x0015D3B8 File Offset: 0x0015B5B8
	private void PlaceUnit()
	{
		if (!this.openForPlacement)
		{
			return;
		}
		if (this.unit == null)
		{
			if (this.placementID == UnitControl.GetPlayerUnitsBeingPlaced().factionID)
			{
				UnitControl.PlaceUnitAt(this);
			}
		}
		else if (this.unit.factionID == UnitControl.GetPlayerUnitsBeingPlaced().factionID)
		{
			UnitControl.RemoveUnit(this.unit);
		}
	}

	// Token: 0x06002D3E RID: 11582 RVA: 0x0001D28F File Offset: 0x0001B48F
	public void SetToDefaultMat()
	{
		this.SetState(this.state);
	}

	// Token: 0x06002D3F RID: 11583 RVA: 0x0015D428 File Offset: 0x0015B628
	public void SetState(_TileState ts)
	{
		if (!this.walkable)
		{
			return;
		}
		this.state = ts;
		if (!this.walkable)
		{
			if (this.project == null)
			{
				base.renderer.material = this.matUnwalkable;
			}
			else
			{
				this.project.material = this.matUnwalkable;
			}
			return;
		}
		if (this.state == _TileState.Default)
		{
			if (this.project == null)
			{
				base.renderer.material = this.matNormal;
			}
			else
			{
				this.project.material = this.matNormal;
			}
		}
		else if (this.state == _TileState.Selected)
		{
			if (this.project == null)
			{
				base.renderer.material = this.matNormal;
			}
			else
			{
				this.project.material = this.matNormal;
			}
		}
		else if (this.state == _TileState.Walkable && this.matWalkable != null)
		{
			if (this.project == null)
			{
				base.renderer.material = this.matWalkable;
			}
			else
			{
				this.project.material = this.matWalkable;
			}
		}
		else if (this.state == _TileState.Hostile && this.matHostile != null)
		{
			if (this.project == null)
			{
				base.renderer.material = this.matHostile;
			}
			else
			{
				this.project.material = this.matHostile;
			}
		}
		else if (this.state == _TileState.AbilityRange && this.matAbilityRange != null)
		{
			if (this.project == null)
			{
				base.renderer.material = this.matAbilityRange;
			}
			else
			{
				this.project.material = this.matAbilityRange;
			}
		}
	}

	// Token: 0x06002D40 RID: 11584 RVA: 0x0001D29D File Offset: 0x0001B49D
	public void ManualSelect()
	{
		if (!GridManager.IsInTargetTileSelectMode() && !GameControlTB.AllowUnitSwitching())
		{
			return;
		}
		this.Select();
	}

	// Token: 0x06002D41 RID: 11585 RVA: 0x0001D2BA File Offset: 0x0001B4BA
	public void Select()
	{
		if (!this.walkable)
		{
			return;
		}
		GridManager.Select(this);
	}

	// Token: 0x06002D42 RID: 11586 RVA: 0x0001D2CE File Offset: 0x0001B4CE
	public void SetWalkableToSelectedFlag()
	{
		this.walkableToSelected = true;
	}

	// Token: 0x06002D43 RID: 11587 RVA: 0x0001D2D7 File Offset: 0x0001B4D7
	public void ClearUnit()
	{
		this.unit.ClearAuras(this);
		this.unit = null;
		if (!GameControlTB.IsUnitPlacementState())
		{
			this.SetState(_TileState.Default);
		}
		if (this.attackableToSelected)
		{
			GridManager.ClearIndicatorH(this);
			this.attackableToSelected = false;
		}
	}

	// Token: 0x06002D44 RID: 11588 RVA: 0x0015D628 File Offset: 0x0015B828
	public void SetUnit(UnitTB newUnit)
	{
		this.unit = newUnit;
		this.unit.SetAuras(this);
		if (Tile.onSetUnit != null && this.iBattleScheduleGroup >= 0)
		{
			Tile.onSetUnit(this, this.iBattleScheduleGroup);
		}
		if (this.collectible != null)
		{
			this.collectible.Trigger(newUnit);
		}
	}

	// Token: 0x06002D45 RID: 11589 RVA: 0x0015D68C File Offset: 0x0015B88C
	private IEnumerator SpawnAbilityEffect(GameObject effect, float delay, Vector3 pos)
	{
		yield return new WaitForSeconds(delay);
		Object.Instantiate(effect, pos, effect.transform.rotation);
		yield break;
	}

	// Token: 0x06002D46 RID: 11590 RVA: 0x0015D6CC File Offset: 0x0015B8CC
	public void ProcessWalkableNeighbour2(Tile tile)
	{
		for (int i = 0; i < this.neighbours.Count; i++)
		{
			if (this.neighbours[i] == tile)
			{
				if (this.neighbours[i].listState2 == _ListState.Unassigned && this.neighbours[i].walkable && this.neighbours[i].unit == null)
				{
					this.neighbours[i].scoreG2 = 0f;
					this.neighbours[i].scoreH2 = 0f;
					this.neighbours[i].scoreF2 = 0f;
					this.neighbours[i].parent2 = this;
				}
			}
			else if (this.neighbours[i].walkable && this.neighbours[i].unit == null && this.neighbours[i].listState2 == _ListState.Unassigned)
			{
				this.neighbours[i].scoreG2 = this.scoreG2 + 1f;
				this.neighbours[i].scoreH2 = Vector3.Distance(this.neighbours[i].thisT.position, tile.thisT.position);
				this.neighbours[i].UpdateScoreF2();
				this.neighbours[i].parent2 = this;
			}
		}
	}

	// Token: 0x06002D47 RID: 11591 RVA: 0x0015D86C File Offset: 0x0015BA6C
	public void ProcessWalkableNeighbour(Tile tile, int iFaction)
	{
		for (int i = 0; i < this.neighbours.Count; i++)
		{
			if (!(this.neighbours[i].unit != null) || this.neighbours[i].unit.CheckFriendFaction(iFaction) || this.neighbours[i].unit.bStealth)
			{
				if (this.neighbours[i].walkable || this.neighbours[i] == tile)
				{
					if (this.neighbours[i].listState2 == _ListState.Unassigned)
					{
						this.neighbours[i].scoreG2 = this.scoreG2 + 1f;
						this.neighbours[i].scoreH2 = Vector3.Distance(this.neighbours[i].thisT.position, tile.thisT.position);
						this.neighbours[i].UpdateScoreF2();
						this.neighbours[i].parent2 = this;
					}
					else if (this.neighbours[i].listState2 == _ListState.Open)
					{
						this.tempScoreG2 = this.scoreG2 + 1f;
						if (this.neighbours[i].scoreG2 > this.tempScoreG2)
						{
							this.neighbours[i].parent2 = this;
							this.neighbours[i].scoreG2 = this.tempScoreG2;
							this.neighbours[i].UpdateScoreF2();
						}
					}
				}
			}
		}
	}

	// Token: 0x06002D48 RID: 11592 RVA: 0x0015DA2C File Offset: 0x0015BC2C
	public void ProcessAllNeighbours(Tile tile)
	{
		Vector3 vector = tile.pos;
		for (int i = 0; i < this.neighbours.Count; i++)
		{
			if (this.neighbours[i].listState == _ListState.Unassigned)
			{
				this.neighbours[i].scoreG = this.scoreG + 1f;
				this.neighbours[i].scoreH = Vector3.Distance(this.neighbours[i].thisT.position, vector);
				this.neighbours[i].UpdateScoreF();
				this.neighbours[i].parent = this;
			}
			else if (this.neighbours[i].listState == _ListState.Open)
			{
				this.tempScoreG = this.scoreG + 1f;
				if (this.neighbours[i].scoreG > this.tempScoreG)
				{
					this.neighbours[i].parent = this;
					this.neighbours[i].scoreG = this.tempScoreG;
					this.neighbours[i].UpdateScoreF();
				}
			}
		}
	}

	// Token: 0x06002D49 RID: 11593 RVA: 0x0001D315 File Offset: 0x0001B515
	private void UpdateScoreF()
	{
		this.scoreF = this.scoreG + this.scoreH;
	}

	// Token: 0x06002D4A RID: 11594 RVA: 0x0001D32A File Offset: 0x0001B52A
	private void UpdateScoreF2()
	{
		this.scoreF2 = this.scoreG2 + this.scoreH2;
	}

	// Token: 0x06002D4B RID: 11595 RVA: 0x0001D33F File Offset: 0x0001B53F
	public void RemoveAurasUnit(UnitTB aurasUnit)
	{
		if (this.AurasUnitList.Contains(aurasUnit))
		{
			this.AurasUnitList.Remove(aurasUnit);
		}
	}

	// Token: 0x06002D4C RID: 11596 RVA: 0x0001D35F File Offset: 0x0001B55F
	public void AddAurasUnit(UnitTB aurasUnit)
	{
		if (!this.AurasUnitList.Contains(aurasUnit))
		{
			this.AurasUnitList.Add(aurasUnit);
		}
	}

	// Token: 0x06002D4D RID: 11597 RVA: 0x0015DB64 File Offset: 0x0015BD64
	public UnitTB GetEffectPartUnit(_EffectPartType typePart)
	{
		int iTileUnitFactionID = 100;
		if (this.unit != null)
		{
			iTileUnitFactionID = this.unit.factionID;
		}
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!(this == unitTB.occupiedTile))
			{
				int num = GridManager.Distance(this, unitTB.occupiedTile);
				if (num > 0)
				{
					if (unitTB.GetEffectPartAurasAbsoluteBuff(typePart, num, iTileUnitFactionID))
					{
						if (!unitTB.GetEffectPartAbsoluteDebuff(_EffectPartType.MovePreturn, null))
						{
							return unitTB;
						}
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06002D4E RID: 11598 RVA: 0x0015DC14 File Offset: 0x0015BE14
	public bool GetEffectPartAbsoluteBuff(_EffectPartType typePart, UnitTB unit)
	{
		int iTileUnitFactionID = 100;
		if (unit != null)
		{
			iTileUnitFactionID = unit.factionID;
		}
		else if (this.unit != null)
		{
			iTileUnitFactionID = this.unit.factionID;
		}
		bool result = false;
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!(this == unitTB.occupiedTile))
			{
				if (!(unit != null) || !(unit == unitTB))
				{
					int num = GridManager.Distance(this, unitTB.occupiedTile);
					if (num > 0)
					{
						if (unitTB.GetEffectPartAurasAbsoluteBuff(typePart, num, iTileUnitFactionID))
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

	// Token: 0x06002D4F RID: 11599 RVA: 0x0015DCF0 File Offset: 0x0015BEF0
	public int GetEffectPartAbsoluteBuffOnHitCount(_EffectPartType typePart, UnitTB unit)
	{
		int iTileUnitFactionID = 100;
		if (unit != null)
		{
			iTileUnitFactionID = unit.factionID;
		}
		else if (this.unit != null)
		{
			iTileUnitFactionID = this.unit.factionID;
		}
		int num = 0;
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!(this == unitTB.occupiedTile))
			{
				if (!(unit != null) || !(unit == unitTB))
				{
					int num2 = GridManager.Distance(this, unitTB.occupiedTile);
					if (num2 > 0)
					{
						int effectPartAurasAbsoluteBuffOnHitCount = unitTB.GetEffectPartAurasAbsoluteBuffOnHitCount(typePart, num2, iTileUnitFactionID);
						if (effectPartAurasAbsoluteBuffOnHitCount == 0)
						{
							return 0;
						}
						if (num < effectPartAurasAbsoluteBuffOnHitCount)
						{
							num = effectPartAurasAbsoluteBuffOnHitCount;
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002D50 RID: 11600 RVA: 0x0015DDD0 File Offset: 0x0015BFD0
	public bool GetEffectPartAbsoluteDebuff(_EffectPartType typePart, UnitTB unit)
	{
		int iTileUnitFactionID = 100;
		if (unit != null)
		{
			iTileUnitFactionID = unit.factionID;
		}
		else if (this.unit != null)
		{
			iTileUnitFactionID = this.unit.factionID;
		}
		bool result = false;
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!(this == unitTB.occupiedTile))
			{
				if (!(unit != null) || !(unit == unitTB))
				{
					int num = GridManager.Distance(this, unitTB.occupiedTile);
					if (num > 0)
					{
						if (unitTB.GetEffectPartAurasAbsoluteDebuff(typePart, num, iTileUnitFactionID))
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

	// Token: 0x06002D51 RID: 11601 RVA: 0x0015DEAC File Offset: 0x0015C0AC
	public float GetEffectPartValue(_EffectPartType typePart, UnitTB unit)
	{
		int iTileUnitFactionID = 100;
		if (unit != null)
		{
			iTileUnitFactionID = unit.factionID;
		}
		else if (this.unit != null)
		{
			iTileUnitFactionID = this.unit.factionID;
		}
		float num = 0f;
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!(this == unitTB.occupiedTile))
			{
				if (!(unit != null) || !(unit == unitTB))
				{
					int num2 = GridManager.Distance(this, unitTB.occupiedTile);
					if (num2 > 0)
					{
						num += unitTB.GetEffectPartAurasValue(typePart, num2, iTileUnitFactionID);
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x0015DF7C File Offset: 0x0015C17C
	public float GetEffectPartValuePercent(_EffectPartType typePart, UnitTB unit)
	{
		int iTileUnitFactionID = 100;
		if (unit != null)
		{
			iTileUnitFactionID = unit.factionID;
		}
		else if (this.unit != null)
		{
			iTileUnitFactionID = this.unit.factionID;
		}
		float num = 0f;
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!(this == unitTB.occupiedTile))
			{
				if (!(unit != null) || !(unit == unitTB))
				{
					int num2 = GridManager.Distance(this, unitTB.occupiedTile);
					if (num2 > 0)
					{
						num += unitTB.GetEffectPartAurasValuePercent(typePart, num2, iTileUnitFactionID);
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x0015E04C File Offset: 0x0015C24C
	public float GetZoneControl(int iFactionID)
	{
		float num = 0f;
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!unitTB.CheckFriendFaction(iFactionID))
			{
				if (!unitTB.bStealth)
				{
					int num2 = GridManager.Distance(this, unitTB.occupiedTile);
					if (num2 > 0)
					{
						float effectPartAurasValue = unitTB.GetEffectPartAurasValue(_EffectPartType.ZoneControl, num2, iFactionID);
						if (effectPartAurasValue > num)
						{
							num = effectPartAurasValue;
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x0015E0DC File Offset: 0x0015C2DC
	public List<string> GetEffectString(_EffectPartType typePart)
	{
		List<string> list = new List<string>();
		int iTileUnitFactionID = 100;
		if (this.unit != null)
		{
			iTileUnitFactionID = this.unit.factionID;
		}
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!(this == unitTB.occupiedTile))
			{
				int num = GridManager.Distance(this, unitTB.occupiedTile);
				if (num > 0)
				{
					list.AddRange(unitTB.GetAuraEffectName(typePart, num, iTileUnitFactionID));
				}
			}
		}
		return list;
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x0015E17C File Offset: 0x0015C37C
	public List<string> GetEffectValue(_EffectPartType typePart)
	{
		List<string> list = new List<string>();
		int iTileUnitFactionID = 100;
		if (this.unit != null)
		{
			iTileUnitFactionID = this.unit.factionID;
		}
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!(this == unitTB.occupiedTile))
			{
				int num = GridManager.Distance(this, unitTB.occupiedTile);
				if (num > 0)
				{
					list.AddRange(unitTB.GetAuraEffectValue(typePart, num, iTileUnitFactionID));
				}
			}
		}
		return list;
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x0015E21C File Offset: 0x0015C41C
	public float GetTalentDamagePercent(UnitTB unit)
	{
		float num = 0f;
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!(this == unitTB.occupiedTile))
			{
				if (!(unit != null) || !(unit == unitTB))
				{
					if (!(unitTB == null))
					{
						if (unitTB.HP > 0)
						{
							int num2 = GridManager.Distance(this, unitTB.occupiedTile);
							if (num2 > 0)
							{
								num += unitTB.GetTalentAreaDamagePercent(num2, unit);
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x0015E2D4 File Offset: 0x0015C4D4
	public int CheckTalentAssistAttack(UnitTB unit, List<Tile> tileList)
	{
		int num = 0;
		for (int i = 0; i < this.AurasUnitList.Count; i++)
		{
			UnitTB unitTB = this.AurasUnitList[i];
			if (!(this == unitTB.occupiedTile))
			{
				if (!(unit != null) || !(unit == unitTB))
				{
					if (!(unitTB == null))
					{
						if (unitTB.HP > 0)
						{
							int num2 = GridManager.Distance(this, unitTB.occupiedTile);
							if (num2 > 0)
							{
								if (unitTB.CheckTalentAssistAttack(num2, unit, tileList))
								{
									num++;
								}
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06002D58 RID: 11608 RVA: 0x0015E390 File Offset: 0x0015C590
	public void SetTileEvent(int iColorType, int iGroup)
	{
		this.iBattleScheduleGroup = iGroup;
		if (this.transTileEevent == null)
		{
			this.transTileEevent = (Transform)Object.Instantiate(GridManager.instance.indicatorSelect);
			this.transTileEevent.parent = GridManager.instance.indicatorSelect.parent;
			this.transTileEevent.localEulerAngles = new Vector3(0f, 0f, 0f);
			this.transTileEevent.localPosition = this.thisT.localPosition;
		}
		HexIndicatorProjectorControl component = this.transTileEevent.GetComponent<HexIndicatorProjectorControl>();
		if (component != null)
		{
			component.SetTileColor(iColorType);
		}
		if (this.unit != null && Tile.onSetUnit != null && this.iBattleScheduleGroup >= 0)
		{
			Tile.onSetUnit(this, this.iBattleScheduleGroup);
		}
	}

	// Token: 0x06002D59 RID: 11609 RVA: 0x0001D37E File Offset: 0x0001B57E
	public void ClearTileEvent()
	{
		this.iBattleScheduleGroup = -1;
		if (this.transTileEevent != null)
		{
			Object.DestroyImmediate(this.transTileEevent.gameObject);
		}
	}

	// Token: 0x04003996 RID: 14742
	public _TileType type;

	// Token: 0x04003997 RID: 14743
	public bool walkable = true;

	// Token: 0x04003998 RID: 14744
	public bool invisible = true;

	// Token: 0x04003999 RID: 14745
	[HideInInspector]
	public bool bUnitOrder;

	// Token: 0x0400399A RID: 14746
	[HideInInspector]
	public UnitTB unit;

	// Token: 0x0400399B RID: 14747
	[HideInInspector]
	public _TileState state;

	// Token: 0x0400399C RID: 14748
	public Obstacle obstacle;

	// Token: 0x0400399D RID: 14749
	public Material matNormal;

	// Token: 0x0400399E RID: 14750
	public Material matWalkable;

	// Token: 0x0400399F RID: 14751
	public Material matUnwalkable;

	// Token: 0x040039A0 RID: 14752
	public Material matHostile;

	// Token: 0x040039A1 RID: 14753
	public Material matAbilityRange;

	// Token: 0x040039A2 RID: 14754
	[HideInInspector]
	public Transform thisT;

	// Token: 0x040039A3 RID: 14755
	[HideInInspector]
	public Vector3 pos;

	// Token: 0x040039A4 RID: 14756
	public int heightLevel;

	// Token: 0x040039A5 RID: 14757
	public List<Tile> neighbours = new List<Tile>();

	// Token: 0x040039A6 RID: 14758
	[HideInInspector]
	public List<Tile> disconnectedNeighbour = new List<Tile>();

	// Token: 0x040039A7 RID: 14759
	[HideInInspector]
	public Tile parent;

	// Token: 0x040039A8 RID: 14760
	[HideInInspector]
	public _ListState listState;

	// Token: 0x040039A9 RID: 14761
	[HideInInspector]
	public float scoreG;

	// Token: 0x040039AA RID: 14762
	[HideInInspector]
	public float scoreH;

	// Token: 0x040039AB RID: 14763
	[HideInInspector]
	public float scoreF;

	// Token: 0x040039AC RID: 14764
	[HideInInspector]
	public float tempScoreG;

	// Token: 0x040039AD RID: 14765
	[HideInInspector]
	public int rangeRequired;

	// Token: 0x040039AE RID: 14766
	[HideInInspector]
	public Tile parent2;

	// Token: 0x040039AF RID: 14767
	[HideInInspector]
	public _ListState listState2;

	// Token: 0x040039B0 RID: 14768
	[HideInInspector]
	public float scoreG2;

	// Token: 0x040039B1 RID: 14769
	[HideInInspector]
	public float scoreH2;

	// Token: 0x040039B2 RID: 14770
	[HideInInspector]
	public float scoreF2;

	// Token: 0x040039B3 RID: 14771
	[HideInInspector]
	public float tempScoreG2;

	// Token: 0x040039B4 RID: 14772
	[HideInInspector]
	public CollectibleTB collectible;

	// Token: 0x040039B5 RID: 14773
	[HideInInspector]
	public bool attackableToSelected;

	// Token: 0x040039B6 RID: 14774
	[HideInInspector]
	public bool walkableToSelected;

	// Token: 0x040039B7 RID: 14775
	public bool openForPlacement;

	// Token: 0x040039B8 RID: 14776
	public int placementID = -1;

	// Token: 0x040039B9 RID: 14777
	[HideInInspector]
	public int HPGainModifier;

	// Token: 0x040039BA RID: 14778
	[HideInInspector]
	public int APGainModifier;

	// Token: 0x040039BB RID: 14779
	[HideInInspector]
	public int damageModifier;

	// Token: 0x040039BC RID: 14780
	[HideInInspector]
	public int attRangeModifier;

	// Token: 0x040039BD RID: 14781
	[HideInInspector]
	public float attackModifier;

	// Token: 0x040039BE RID: 14782
	[HideInInspector]
	public float defendModifier;

	// Token: 0x040039BF RID: 14783
	[HideInInspector]
	public float criticalModifier;

	// Token: 0x040039C0 RID: 14784
	[HideInInspector]
	public float critDefModifier;

	// Token: 0x040039C1 RID: 14785
	[HideInInspector]
	public float counterAttackModifier;

	// Token: 0x040039C2 RID: 14786
	[HideInInspector]
	public int sightModifier;

	// Token: 0x040039C3 RID: 14787
	[HideInInspector]
	public List<GameObject> wall = new List<GameObject>();

	// Token: 0x040039C4 RID: 14788
	[HideInInspector]
	public List<UnitTB> AurasUnitList = new List<UnitTB>();

	// Token: 0x040039C5 RID: 14789
	[HideInInspector]
	public List<UnitTB> AIHostileList = new List<UnitTB>();

	// Token: 0x040039C6 RID: 14790
	public List<Wall> walls = new List<Wall>();

	// Token: 0x040039C7 RID: 14791
	public Projector project;

	// Token: 0x040039C8 RID: 14792
	public Transform transTileEevent;

	// Token: 0x040039C9 RID: 14793
	public int iBattleScheduleGroup = -1;

	// Token: 0x0200076F RID: 1903
	// (Invoke) Token: 0x06002D5B RID: 11611
	public delegate void OnShowUnitInfoHandler(Tile tile);

	// Token: 0x02000770 RID: 1904
	// (Invoke) Token: 0x06002D5F RID: 11615
	public delegate void TileSetUnit(Tile tile, int LinkNodeID);
}
