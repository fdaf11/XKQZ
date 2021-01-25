using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000422 RID: 1058
[Serializable]
public class InvBaseItem
{
	// Token: 0x04001E65 RID: 7781
	public int id16;

	// Token: 0x04001E66 RID: 7782
	public string name;

	// Token: 0x04001E67 RID: 7783
	public string description;

	// Token: 0x04001E68 RID: 7784
	public InvBaseItem.Slot slot;

	// Token: 0x04001E69 RID: 7785
	public int minItemLevel = 1;

	// Token: 0x04001E6A RID: 7786
	public int maxItemLevel = 50;

	// Token: 0x04001E6B RID: 7787
	public List<InvStat> stats = new List<InvStat>();

	// Token: 0x04001E6C RID: 7788
	public GameObject attachment;

	// Token: 0x04001E6D RID: 7789
	public Color color = Color.white;

	// Token: 0x04001E6E RID: 7790
	public UIAtlas iconAtlas;

	// Token: 0x04001E6F RID: 7791
	public string iconName = string.Empty;

	// Token: 0x02000423 RID: 1059
	public enum Slot
	{
		// Token: 0x04001E71 RID: 7793
		None,
		// Token: 0x04001E72 RID: 7794
		Weapon,
		// Token: 0x04001E73 RID: 7795
		Shield,
		// Token: 0x04001E74 RID: 7796
		Body,
		// Token: 0x04001E75 RID: 7797
		Shoulders,
		// Token: 0x04001E76 RID: 7798
		Bracers,
		// Token: 0x04001E77 RID: 7799
		Boots,
		// Token: 0x04001E78 RID: 7800
		Trinket,
		// Token: 0x04001E79 RID: 7801
		_LastDoNotUse
	}
}
