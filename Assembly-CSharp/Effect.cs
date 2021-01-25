using System;
using UnityEngine;

// Token: 0x0200072C RID: 1836
[Serializable]
public class Effect
{
	// Token: 0x04003804 RID: 14340
	public int ID = -1;

	// Token: 0x04003805 RID: 14341
	public string name = string.Empty;

	// Token: 0x04003806 RID: 14342
	public string desp = string.Empty;

	// Token: 0x04003807 RID: 14343
	public Texture icon;

	// Token: 0x04003808 RID: 14344
	public string iconName = string.Empty;

	// Token: 0x04003809 RID: 14345
	public _EffectType effectType;

	// Token: 0x0400380A RID: 14346
	public int minDuration;

	// Token: 0x0400380B RID: 14347
	public int maxDuration = 1;

	// Token: 0x0400380C RID: 14348
	public float removeChance;

	// Token: 0x0400380D RID: 14349
	public float removeByAttackChance;

	// Token: 0x0400380E RID: 14350
	public int countTillNextTurn = 1;
}
