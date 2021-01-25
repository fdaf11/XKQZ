using System;

// Token: 0x02000092 RID: 146
[Serializable]
public class MainGameObj
{
	// Token: 0x06000338 RID: 824 RVA: 0x00004467 File Offset: 0x00002667
	public MainGameObj()
	{
		this.knight = new Character();
		this.rogue = new Character();
		this.wizard = new Character();
	}

	// Token: 0x0400025A RID: 602
	public static MainGameObj current;

	// Token: 0x0400025B RID: 603
	public Character knight;

	// Token: 0x0400025C RID: 604
	public Character rogue;

	// Token: 0x0400025D RID: 605
	public Character wizard;
}
