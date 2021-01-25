using System;

// Token: 0x020000F1 RID: 241
public class EffectPart
{
	// Token: 0x06000506 RID: 1286 RVA: 0x0003AE58 File Offset: 0x00039058
	public EffectPart Clone()
	{
		return new EffectPart
		{
			m_effectPartType = this.m_effectPartType,
			m_effectAccumulateType = this.m_effectAccumulateType,
			m_bPercent = this.m_bPercent,
			m_iValue1 = this.m_iValue1,
			m_iValue2 = this.m_iValue2,
			m_iValueLimit = this.m_iValueLimit,
			m_iValueSum = this.m_iValueSum,
			m_iValueBase = this.m_iValueBase
		};
	}

	// Token: 0x04000500 RID: 1280
	public _EffectPartType m_effectPartType;

	// Token: 0x04000501 RID: 1281
	public _EffectAccumulateType m_effectAccumulateType;

	// Token: 0x04000502 RID: 1282
	public bool m_bPercent;

	// Token: 0x04000503 RID: 1283
	public int m_iValue1;

	// Token: 0x04000504 RID: 1284
	public int m_iValue2;

	// Token: 0x04000505 RID: 1285
	public int m_iValueLimit;

	// Token: 0x04000506 RID: 1286
	public int m_iValueSum;

	// Token: 0x04000507 RID: 1287
	public int m_iValueBase;

	// Token: 0x04000508 RID: 1288
	public int m_iTargetUnitID;
}
