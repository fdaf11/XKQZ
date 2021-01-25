using System;

// Token: 0x0200039B RID: 923
public class YoungHeroTimeNode
{
	// Token: 0x06001560 RID: 5472 RVA: 0x0000D8A3 File Offset: 0x0000BAA3
	public void ReSetDate()
	{
		this.iday = 1;
		this.iRound = 0;
		this.iweek = 1;
		this.iYear = 1;
		this.iMonth = 1;
	}

	// Token: 0x04001A04 RID: 6660
	public int iRound;

	// Token: 0x04001A05 RID: 6661
	public int iday;

	// Token: 0x04001A06 RID: 6662
	public int iweek;

	// Token: 0x04001A07 RID: 6663
	public int iMonth;

	// Token: 0x04001A08 RID: 6664
	public int iYear;
}
