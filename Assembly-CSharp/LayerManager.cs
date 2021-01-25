using System;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public class LayerManager : MonoBehaviour
{
	// Token: 0x06002CF7 RID: 11511 RVA: 0x0001CF69 File Offset: 0x0001B169
	private void Awake()
	{
		LayerManager.LoadPrefab();
	}

	// Token: 0x06002CF8 RID: 11512 RVA: 0x0015BE00 File Offset: 0x0015A000
	public static void LoadPrefab()
	{
		GameObject gameObject = Resources.Load("DamageArmorList", typeof(GameObject)) as GameObject;
		if (gameObject == null)
		{
			return;
		}
		LayerListPrefab component = gameObject.GetComponent<LayerListPrefab>();
		if (component != null)
		{
			LayerManager.ui = component.ui;
			LayerManager.unit = component.unit;
			LayerManager.unitAI = component.unitAI;
			LayerManager.unitAIInv = component.unitAIInv;
			LayerManager.tile = component.tile;
			LayerManager.obstacle = component.obstacle;
		}
	}

	// Token: 0x06002CF9 RID: 11513 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06002CFA RID: 11514 RVA: 0x0001CF70 File Offset: 0x0001B170
	public static int GetLayerUI()
	{
		return LayerManager.ui;
	}

	// Token: 0x06002CFB RID: 11515 RVA: 0x0001CF77 File Offset: 0x0001B177
	public static int GetLayerUnit()
	{
		return LayerManager.unit;
	}

	// Token: 0x06002CFC RID: 11516 RVA: 0x0001CF7E File Offset: 0x0001B17E
	public static int GetLayerUnitAI()
	{
		return LayerManager.unitAI;
	}

	// Token: 0x06002CFD RID: 11517 RVA: 0x0001CF85 File Offset: 0x0001B185
	public static int GetLayerUnitAIInvisible()
	{
		return LayerManager.unitAIInv;
	}

	// Token: 0x06002CFE RID: 11518 RVA: 0x0001CF8C File Offset: 0x0001B18C
	public static int GetLayerTile()
	{
		return LayerManager.tile;
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x0001CF93 File Offset: 0x0001B193
	public static int GetLayerObstacle()
	{
		return LayerManager.obstacle;
	}

	// Token: 0x0400392E RID: 14638
	[HideInInspector]
	public static int ui = 31;

	// Token: 0x0400392F RID: 14639
	[HideInInspector]
	public static int unit = 30;

	// Token: 0x04003930 RID: 14640
	[HideInInspector]
	public static int unitAI = 29;

	// Token: 0x04003931 RID: 14641
	[HideInInspector]
	public static int unitAIInv = 28;

	// Token: 0x04003932 RID: 14642
	[HideInInspector]
	public static int tile = 8;

	// Token: 0x04003933 RID: 14643
	[HideInInspector]
	public static int obstacle = 9;

	// Token: 0x04003934 RID: 14644
	[HideInInspector]
	public static int terrain = 10;
}
