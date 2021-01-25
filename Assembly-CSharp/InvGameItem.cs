using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000426 RID: 1062
[Serializable]
public class InvGameItem
{
	// Token: 0x060019D0 RID: 6608 RVA: 0x00010CB7 File Offset: 0x0000EEB7
	public InvGameItem(int id)
	{
		this.mBaseItemID = id;
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x00010CD4 File Offset: 0x0000EED4
	public InvGameItem(int id, InvBaseItem bi)
	{
		this.mBaseItemID = id;
		this.mBaseItem = bi;
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x060019D2 RID: 6610 RVA: 0x00010CF8 File Offset: 0x0000EEF8
	public int baseItemID
	{
		get
		{
			return this.mBaseItemID;
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x060019D3 RID: 6611 RVA: 0x00010D00 File Offset: 0x0000EF00
	public InvBaseItem baseItem
	{
		get
		{
			if (this.mBaseItem == null)
			{
				this.mBaseItem = InvDatabase.FindByID(this.baseItemID);
			}
			return this.mBaseItem;
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x060019D4 RID: 6612 RVA: 0x00010D24 File Offset: 0x0000EF24
	public string name
	{
		get
		{
			if (this.baseItem == null)
			{
				return null;
			}
			return this.quality.ToString() + " " + this.baseItem.name;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x060019D5 RID: 6613 RVA: 0x000CF93C File Offset: 0x000CDB3C
	public float statMultiplier
	{
		get
		{
			float num = 0f;
			switch (this.quality)
			{
			case InvGameItem.Quality.Broken:
				num = 0f;
				break;
			case InvGameItem.Quality.Cursed:
				num = -1f;
				break;
			case InvGameItem.Quality.Damaged:
				num = 0.25f;
				break;
			case InvGameItem.Quality.Worn:
				num = 0.9f;
				break;
			case InvGameItem.Quality.Sturdy:
				num = 1f;
				break;
			case InvGameItem.Quality.Polished:
				num = 1.1f;
				break;
			case InvGameItem.Quality.Improved:
				num = 1.25f;
				break;
			case InvGameItem.Quality.Crafted:
				num = 1.5f;
				break;
			case InvGameItem.Quality.Superior:
				num = 1.75f;
				break;
			case InvGameItem.Quality.Enchanted:
				num = 2f;
				break;
			case InvGameItem.Quality.Epic:
				num = 2.5f;
				break;
			case InvGameItem.Quality.Legendary:
				num = 3f;
				break;
			}
			float num2 = (float)this.itemLevel / 50f;
			return num * Mathf.Lerp(num2, num2 * num2, 0.5f);
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x060019D6 RID: 6614 RVA: 0x000CFA38 File Offset: 0x000CDC38
	public Color color
	{
		get
		{
			Color result = Color.white;
			switch (this.quality)
			{
			case InvGameItem.Quality.Broken:
				result..ctor(0.4f, 0.2f, 0.2f);
				break;
			case InvGameItem.Quality.Cursed:
				result = Color.red;
				break;
			case InvGameItem.Quality.Damaged:
				result..ctor(0.4f, 0.4f, 0.4f);
				break;
			case InvGameItem.Quality.Worn:
				result..ctor(0.7f, 0.7f, 0.7f);
				break;
			case InvGameItem.Quality.Sturdy:
				result..ctor(1f, 1f, 1f);
				break;
			case InvGameItem.Quality.Polished:
				result = NGUIMath.HexToColor(3774856959U);
				break;
			case InvGameItem.Quality.Improved:
				result = NGUIMath.HexToColor(2480359935U);
				break;
			case InvGameItem.Quality.Crafted:
				result = NGUIMath.HexToColor(1325334783U);
				break;
			case InvGameItem.Quality.Superior:
				result = NGUIMath.HexToColor(12255231U);
				break;
			case InvGameItem.Quality.Enchanted:
				result = NGUIMath.HexToColor(1937178111U);
				break;
			case InvGameItem.Quality.Epic:
				result = NGUIMath.HexToColor(2516647935U);
				break;
			case InvGameItem.Quality.Legendary:
				result = NGUIMath.HexToColor(4287627519U);
				break;
			}
			return result;
		}
	}

	// Token: 0x060019D7 RID: 6615 RVA: 0x000CFB78 File Offset: 0x000CDD78
	public List<InvStat> CalculateStats()
	{
		List<InvStat> list = new List<InvStat>();
		if (this.baseItem != null)
		{
			float statMultiplier = this.statMultiplier;
			List<InvStat> stats = this.baseItem.stats;
			int i = 0;
			int count = stats.Count;
			while (i < count)
			{
				InvStat invStat = stats[i];
				int num = Mathf.RoundToInt(statMultiplier * (float)invStat.amount);
				if (num != 0)
				{
					bool flag = false;
					int j = 0;
					int count2 = list.Count;
					while (j < count2)
					{
						InvStat invStat2 = list[j];
						if (invStat2.id == invStat.id && invStat2.modifier == invStat.modifier)
						{
							invStat2.amount += num;
							flag = true;
							break;
						}
						j++;
					}
					if (!flag)
					{
						list.Add(new InvStat
						{
							id = invStat.id,
							amount = num,
							modifier = invStat.modifier
						});
					}
				}
				i++;
			}
			list.Sort(new Comparison<InvStat>(InvStat.CompareArmor));
		}
		return list;
	}

	// Token: 0x04001E81 RID: 7809
	[SerializeField]
	private int mBaseItemID;

	// Token: 0x04001E82 RID: 7810
	public InvGameItem.Quality quality = InvGameItem.Quality.Sturdy;

	// Token: 0x04001E83 RID: 7811
	public int itemLevel = 1;

	// Token: 0x04001E84 RID: 7812
	private InvBaseItem mBaseItem;

	// Token: 0x02000427 RID: 1063
	public enum Quality
	{
		// Token: 0x04001E86 RID: 7814
		Broken,
		// Token: 0x04001E87 RID: 7815
		Cursed,
		// Token: 0x04001E88 RID: 7816
		Damaged,
		// Token: 0x04001E89 RID: 7817
		Worn,
		// Token: 0x04001E8A RID: 7818
		Sturdy,
		// Token: 0x04001E8B RID: 7819
		Polished,
		// Token: 0x04001E8C RID: 7820
		Improved,
		// Token: 0x04001E8D RID: 7821
		Crafted,
		// Token: 0x04001E8E RID: 7822
		Superior,
		// Token: 0x04001E8F RID: 7823
		Enchanted,
		// Token: 0x04001E90 RID: 7824
		Epic,
		// Token: 0x04001E91 RID: 7825
		Legendary,
		// Token: 0x04001E92 RID: 7826
		_LastDoNotUse
	}
}
