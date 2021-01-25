using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000738 RID: 1848
public class DamageTable : MonoBehaviour
{
	// Token: 0x06002BCC RID: 11212 RVA: 0x0001C41C File Offset: 0x0001A61C
	public static List<ArmorType> GetAllArmorType()
	{
		return DamageTable.armorTypes;
	}

	// Token: 0x06002BCD RID: 11213 RVA: 0x0001C423 File Offset: 0x0001A623
	public static List<DamageType> GetAllDamageType()
	{
		return DamageTable.dmgTypes;
	}

	// Token: 0x06002BCE RID: 11214 RVA: 0x0001C42A File Offset: 0x0001A62A
	private void Awake()
	{
		DamageTable.LoadPrefab();
	}

	// Token: 0x06002BCF RID: 11215 RVA: 0x00155244 File Offset: 0x00153444
	private static void LoadPrefab()
	{
		GameObject gameObject = Resources.Load("DamageArmorList", typeof(GameObject)) as GameObject;
		if (gameObject == null)
		{
			return;
		}
		DamageArmorListPrefab damageArmorListPrefab = gameObject.GetComponent<DamageArmorListPrefab>();
		if (damageArmorListPrefab == null)
		{
			damageArmorListPrefab = gameObject.AddComponent<DamageArmorListPrefab>();
		}
		DamageTable.armorTypes = damageArmorListPrefab.armorList;
		DamageTable.dmgTypes = damageArmorListPrefab.damageList;
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x001552A8 File Offset: 0x001534A8
	public static float GetModifier(int armorID = 0, int dmgID = 0)
	{
		armorID = Mathf.Max(0, armorID);
		dmgID = Mathf.Max(0, dmgID);
		if (armorID < DamageTable.armorTypes.Count && dmgID < DamageTable.dmgTypes.Count)
		{
			return DamageTable.armorTypes[armorID].modifiers[dmgID];
		}
		return 1f;
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x0001C431 File Offset: 0x0001A631
	public static ArmorType GetArmorInfo(int ID)
	{
		if (ID > DamageTable.armorTypes.Count)
		{
			Debug.Log("ArmorType requested does not exist");
			return new ArmorType();
		}
		return DamageTable.armorTypes[ID];
	}

	// Token: 0x06002BD2 RID: 11218 RVA: 0x0001C45E File Offset: 0x0001A65E
	public static DamageType GetDamageInfo(int ID)
	{
		if (ID > DamageTable.dmgTypes.Count)
		{
			Debug.Log("DamageType requested does not exist");
			return new DamageType();
		}
		return DamageTable.dmgTypes[ID];
	}

	// Token: 0x0400388C RID: 14476
	private static List<ArmorType> armorTypes = new List<ArmorType>();

	// Token: 0x0400388D RID: 14477
	private static List<DamageType> dmgTypes = new List<DamageType>();
}
