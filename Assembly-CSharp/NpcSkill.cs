using System;
using Newtonsoft.Json;

// Token: 0x020001F4 RID: 500
public class NpcSkill
{
	// Token: 0x04000A44 RID: 2628
	[JsonIgnore]
	public NpcSkill.SkillType m_SkillType;

	// Token: 0x04000A45 RID: 2629
	public int iLevel;

	// Token: 0x04000A46 RID: 2630
	[JsonIgnore]
	public int iCurExp;

	// Token: 0x04000A47 RID: 2631
	[JsonIgnore]
	public int iNextExp;

	// Token: 0x04000A48 RID: 2632
	public int m_iAccumulationExp;

	// Token: 0x04000A49 RID: 2633
	public bool bUse;

	// Token: 0x04000A4A RID: 2634
	public int iSkillID;

	// Token: 0x020001F5 RID: 501
	public enum SkillType
	{
		// Token: 0x04000A4C RID: 2636
		Routine,
		// Token: 0x04000A4D RID: 2637
		Neigong
	}
}
