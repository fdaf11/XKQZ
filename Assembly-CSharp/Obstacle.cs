using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000759 RID: 1881
[RequireComponent(typeof(Collider))]
public class Obstacle : MonoBehaviour
{
	// Token: 0x06002D01 RID: 11521 RVA: 0x0015BE8C File Offset: 0x0015A08C
	private void Start()
	{
		base.collider.enabled = true;
		int layerObstacle = LayerManager.GetLayerObstacle();
		base.gameObject.layer = layerObstacle;
		if (this.occupiedTile != null)
		{
			foreach (Tile tile in this.occupiedTile.GetNeighbours())
			{
				if (tile.obstacle != null && !this.adjacentObs.Contains(tile.obstacle))
				{
					GameObject gameObject = new GameObject();
					gameObject.layer = layerObstacle;
					gameObject.name = "collider";
					gameObject.transform.parent = tile.transform;
					gameObject.transform.position = (this.occupiedTile.pos + tile.pos) / 2f;
					SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
					sphereCollider.radius = GridManager.GetTileSize() * 0.25f;
					this.adjacentObs.Add(tile.obstacle);
					tile.obstacle.adjacentObs.Add(this);
					Obstacle obstacle = gameObject.AddComponent<Obstacle>();
					if (this.coverType == _CoverType.BlockFull || tile.obstacle.coverType == _CoverType.BlockFull)
					{
						obstacle.coverType = _CoverType.BlockFull;
					}
					else
					{
						obstacle.coverType = _CoverType.BlockHalf;
					}
				}
			}
		}
	}

	// Token: 0x04003940 RID: 14656
	public string obsName = "obstacle";

	// Token: 0x04003941 RID: 14657
	[HideInInspector]
	public int prefabID = -1;

	// Token: 0x04003942 RID: 14658
	public _CoverType coverType = _CoverType.BlockHalf;

	// Token: 0x04003943 RID: 14659
	public _ObsType obsType;

	// Token: 0x04003944 RID: 14660
	public _ObsTileType tileType = _ObsTileType.Universal;

	// Token: 0x04003945 RID: 14661
	[HideInInspector]
	public Tile occupiedTile;

	// Token: 0x04003946 RID: 14662
	[HideInInspector]
	public List<Obstacle> adjacentObs = new List<Obstacle>();
}
