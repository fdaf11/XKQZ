using System;

// Token: 0x0200072B RID: 1835
[Serializable]
public class EffectAttr
{
	// Token: 0x06002B6D RID: 11117 RVA: 0x00153234 File Offset: 0x00151434
	public EffectAttr Clone()
	{
		return new EffectAttr
		{
			type = this.type,
			value = this.value,
			valueAlt = this.valueAlt,
			valueSum = this.valueSum,
			bAccumulate = this.bAccumulate,
			bPercent = this.bPercent,
			valueLimit = this.valueLimit
		};
	}

	// Token: 0x040037FC RID: 14332
	public _EffectAttrType type;

	// Token: 0x040037FD RID: 14333
	public float value;

	// Token: 0x040037FE RID: 14334
	public float valueAlt;

	// Token: 0x040037FF RID: 14335
	public float valueLimit;

	// Token: 0x04003800 RID: 14336
	public float valueSum;

	// Token: 0x04003801 RID: 14337
	public _AccumulateType eAccumulateType;

	// Token: 0x04003802 RID: 14338
	public bool bAccumulate;

	// Token: 0x04003803 RID: 14339
	public bool bPercent;
}
