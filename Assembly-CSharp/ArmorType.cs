using System;
using System.Collections.Generic;

// Token: 0x0200073A RID: 1850
[Serializable]
public class ArmorType
{
	// Token: 0x06002BD4 RID: 11220 RVA: 0x00155304 File Offset: 0x00153504
	public ArmorType(int ID, string n, string d, List<float> mods)
	{
		this.typeID = ID;
		this.name = n;
		this.desp = d;
		this.modifiers = mods;
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x0001C4B0 File Offset: 0x0001A6B0
	public ArmorType()
	{
		this.modifiers.Add(1f);
	}

	// Token: 0x06002BD6 RID: 11222 RVA: 0x0015535C File Offset: 0x0015355C
	public ArmorType(int modsNum)
	{
		for (int i = 0; i < modsNum; i++)
		{
			this.modifiers.Add(1f);
		}
	}

	// Token: 0x04003891 RID: 14481
	public int typeID = -1;

	// Token: 0x04003892 RID: 14482
	public string name = "ArmorName";

	// Token: 0x04003893 RID: 14483
	public string desp = string.Empty;

	// Token: 0x04003894 RID: 14484
	public List<float> modifiers = new List<float>();
}
