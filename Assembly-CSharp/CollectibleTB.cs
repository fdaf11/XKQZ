using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000737 RID: 1847
public class CollectibleTB : MonoBehaviour
{
	// Token: 0x06002BC5 RID: 11205 RVA: 0x001550D8 File Offset: 0x001532D8
	private void Awake()
	{
		this.thisObj = base.gameObject;
		this.thisT = base.transform;
		if (this.icon != null)
		{
			this.effect.icon = this.icon;
			this.effect.iconName = this.icon.name;
		}
	}

	// Token: 0x06002BC6 RID: 11206 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x06002BC7 RID: 11207 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06002BC8 RID: 11208 RVA: 0x0000264F File Offset: 0x0000084F
	public void Init()
	{
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x00155138 File Offset: 0x00153338
	public void Trigger(UnitTB unit)
	{
		if (this.enableAOE && this.aoeRange > 1)
		{
			List<Tile> tilesWithinRange = GridManager.GetTilesWithinRange(this.occupiedTile, this.aoeRange);
			foreach (Tile tile in tilesWithinRange)
			{
				if (tile.unit != null)
				{
					tile.unit.ApplyCollectibleEffect(this.effect);
				}
			}
		}
		else
		{
			unit.ApplyCollectibleEffect(this.effect);
		}
		if (this.triggerEffect != null)
		{
			Object.Instantiate(this.triggerEffect, this.occupiedTile.pos, Quaternion.identity);
		}
		if (this.triggerAudio != null)
		{
			AudioManager.PlaySound(this.triggerAudio, this.thisT.position);
		}
		Object.Destroy(this.thisObj);
	}

	// Token: 0x04003880 RID: 14464
	public int prefabID = -1;

	// Token: 0x04003881 RID: 14465
	public string collectibleName = "collectible";

	// Token: 0x04003882 RID: 14466
	public Texture icon;

	// Token: 0x04003883 RID: 14467
	[HideInInspector]
	public GameObject thisObj;

	// Token: 0x04003884 RID: 14468
	[HideInInspector]
	public Transform thisT;

	// Token: 0x04003885 RID: 14469
	[HideInInspector]
	public Tile occupiedTile;

	// Token: 0x04003886 RID: 14470
	public AudioClip triggerAudio;

	// Token: 0x04003887 RID: 14471
	public GameObject triggerEffect;

	// Token: 0x04003888 RID: 14472
	public float value = 1f;

	// Token: 0x04003889 RID: 14473
	public bool enableAOE;

	// Token: 0x0400388A RID: 14474
	public int aoeRange = 1;

	// Token: 0x0400388B RID: 14475
	public Effect effect = new Effect();
}
