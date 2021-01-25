using System;

// Token: 0x0200012F RID: 303
public struct CParaValue
{
	// Token: 0x0600061E RID: 1566 RVA: 0x00005914 File Offset: 0x00003B14
	public CParaValue(int ival)
	{
		this.m_iVal = ival;
		this.m_sVal = "none";
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x00005928 File Offset: 0x00003B28
	public CParaValue(string sval)
	{
		this.m_iVal = 0;
		this.m_sVal = sval;
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x00005938 File Offset: 0x00003B38
	public static implicit operator CParaValue(int ival)
	{
		return new CParaValue(ival);
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00005940 File Offset: 0x00003B40
	public static implicit operator CParaValue(string sval)
	{
		return new CParaValue(sval);
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00005948 File Offset: 0x00003B48
	public static implicit operator int(CParaValue para)
	{
		return para.m_iVal;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x00005951 File Offset: 0x00003B51
	public static implicit operator string(CParaValue para)
	{
		return para.m_sVal;
	}

	// Token: 0x040006C7 RID: 1735
	public int m_iVal;

	// Token: 0x040006C8 RID: 1736
	public string m_sVal;
}
