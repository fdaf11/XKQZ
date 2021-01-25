using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000724 RID: 1828
public class AStar : MonoBehaviour
{
	// Token: 0x06002B66 RID: 11110 RVA: 0x00152C80 File Offset: 0x00150E80
	public static List<Tile> SearchWalkableTile2(Tile originTile, Tile destTile, int iFaction, float fFlyLevel)
	{
		List<Tile> list = new List<Tile>();
		List<Tile> list2 = new List<Tile>();
		Tile tile = originTile;
		while (!(tile == destTile))
		{
			list.Add(tile);
			tile.listState2 = _ListState.Close;
			tile.ProcessWalkableNeighbour(destTile, iFaction);
			foreach (Tile tile2 in tile.GetNeighbours())
			{
				if (!(tile2.unit != null) || tile2.unit.CheckFriendFaction(iFaction) || tile2.unit.bStealth)
				{
					if ((tile2.listState2 == _ListState.Unassigned && tile2.walkable) || tile2 == destTile)
					{
						if (tile2 == destTile)
						{
							tile2.listState2 = _ListState.Open;
							list2.Add(tile2);
						}
						else
						{
							float zoneControl = tile2.GetZoneControl(iFaction);
							if (fFlyLevel >= zoneControl)
							{
								tile2.listState2 = _ListState.Open;
								list2.Add(tile2);
							}
							else
							{
								tile2.listState2 = _ListState.Close;
								list.Add(tile2);
							}
						}
					}
				}
			}
			tile = null;
			float num = float.PositiveInfinity;
			int num2 = 0;
			for (int i = 0; i < list2.Count; i++)
			{
				if (list2[i].scoreF2 < num)
				{
					num = list2[i].scoreF2;
					tile = list2[i];
					num2 = i;
				}
			}
			if (tile == null)
			{
				IL_1A4:
				List<Tile> list3 = new List<Tile>();
				while (tile != null)
				{
					list3.Add(tile);
					tile = tile.parent2;
				}
				list3 = AStar.InvertTileArray(list3);
				AStar.ResetGraph2(destTile, list2, list);
				return list3;
			}
			list2.RemoveAt(num2);
		}
		goto IL_1A4;
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x00152E7C File Offset: 0x0015107C
	public static int Distance(Tile srcTile, Tile targetTile, bool bWalkAble)
	{
		List<Tile> list = new List<Tile>();
		List<Tile> list2 = new List<Tile>();
		srcTile.scoreG = 0f;
		Tile tile = srcTile;
		if (srcTile == null)
		{
			Debug.Log("src tile is null!!!");
		}
		while (!(tile == targetTile))
		{
			list.Add(tile);
			tile.listState = _ListState.Close;
			tile.ProcessAllNeighbours(targetTile);
			foreach (Tile tile2 in tile.GetNeighbours())
			{
				if (bWalkAble && !tile2.walkable)
				{
					tile2.listState = _ListState.Close;
					list.Add(tile2);
				}
				else if (bWalkAble && !tile2.invisible)
				{
					tile2.listState = _ListState.Close;
					list.Add(tile2);
				}
				else if (tile2.listState == _ListState.Unassigned)
				{
					tile2.listState = _ListState.Open;
					list2.Add(tile2);
				}
			}
			tile = null;
			float num = float.PositiveInfinity;
			int num2 = 0;
			for (int i = 0; i < list2.Count; i++)
			{
				if (list2[i].scoreF < num)
				{
					num = list2[i].scoreF;
					tile = list2[i];
					num2 = i;
				}
			}
			if (tile == null)
			{
				IL_176:
				int num3 = 0;
				while (tile != null)
				{
					num3++;
					tile = tile.parent;
				}
				AStar.ResetGraph(targetTile, list2, list);
				return num3 - 1;
			}
			list2.RemoveAt(num2);
		}
		goto IL_176;
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x0015303C File Offset: 0x0015123C
	private static List<Vector3> InvertArray(List<Vector3> p)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < p.Count; i++)
		{
			list.Add(p[p.Count - (i + 1)]);
		}
		return list;
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x00153080 File Offset: 0x00151280
	private static List<Tile> InvertTileArray(List<Tile> p)
	{
		List<Tile> list = new List<Tile>();
		for (int i = 0; i < p.Count; i++)
		{
			list.Add(p[p.Count - (i + 1)]);
		}
		return list;
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x001530C4 File Offset: 0x001512C4
	private static void ResetGraph(Tile hTile, List<Tile> oList, List<Tile> cList)
	{
		hTile.listState = _ListState.Unassigned;
		hTile.parent = null;
		foreach (Tile tile in oList)
		{
			tile.listState = _ListState.Unassigned;
			tile.parent = null;
		}
		foreach (Tile tile2 in cList)
		{
			tile2.listState = _ListState.Unassigned;
			tile2.parent = null;
		}
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x0015317C File Offset: 0x0015137C
	private static void ResetGraph2(Tile hTile, List<Tile> oList, List<Tile> cList)
	{
		hTile.listState2 = _ListState.Unassigned;
		hTile.parent2 = null;
		foreach (Tile tile in oList)
		{
			tile.listState2 = _ListState.Unassigned;
			tile.parent2 = null;
		}
		foreach (Tile tile2 in cList)
		{
			tile2.listState2 = _ListState.Unassigned;
			tile2.parent2 = null;
		}
	}
}
