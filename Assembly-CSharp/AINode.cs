using System;
using System.Collections.Generic;
using Heluo.Wulin;

// Token: 0x02000719 RID: 1817
internal class AINode
{
	// Token: 0x0400377B RID: 14203
	public UnitTB unitAI;

	// Token: 0x0400377C RID: 14204
	public Tile tileMove;

	// Token: 0x0400377D RID: 14205
	public ItemDataNode idn;

	// Token: 0x0400377E RID: 14206
	public UnitAbility uAb;

	// Token: 0x0400377F RID: 14207
	public List<Tile> targetTileList = new List<Tile>();

	// Token: 0x04003780 RID: 14208
	public int iDeadCount;

	// Token: 0x04003781 RID: 14209
	public int iAbilityTargetCount;

	// Token: 0x04003782 RID: 14210
	public int iAbilityValue;
}
