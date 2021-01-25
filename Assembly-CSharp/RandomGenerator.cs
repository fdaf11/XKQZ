using System;

// Token: 0x02000609 RID: 1545
public class RandomGenerator
{
	// Token: 0x06002637 RID: 9783 RVA: 0x00019704 File Offset: 0x00017904
	public RandomGenerator(uint val)
	{
		this.SetSeed(val);
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x00019713 File Offset: 0x00017913
	public RandomGenerator()
	{
		this.SetSeed(RandomGenerator.counter++);
	}

	// Token: 0x0600263A RID: 9786 RVA: 0x00128BAC File Offset: 0x00126DAC
	public uint GenerateUint()
	{
		uint num = this.a ^ this.a << 11;
		this.a = this.b;
		this.b = this.c;
		this.c = this.d;
		return this.d = (this.d ^ this.d >> 19 ^ (num ^ num >> 8));
	}

	// Token: 0x0600263B RID: 9787 RVA: 0x0001972E File Offset: 0x0001792E
	public int Range(int max)
	{
		return (int)((ulong)this.GenerateUint() % (ulong)((long)max));
	}

	// Token: 0x0600263C RID: 9788 RVA: 0x0001973B File Offset: 0x0001793B
	public int Range(int min, int max)
	{
		return min + (int)((ulong)this.GenerateUint() % (ulong)((long)(max - min)));
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x0001974C File Offset: 0x0001794C
	public float GenerateFloat()
	{
		return 2.3283064E-10f * this.GenerateUint();
	}

	// Token: 0x0600263E RID: 9790 RVA: 0x00128C10 File Offset: 0x00126E10
	public float GenerateRangeFloat()
	{
		uint num = this.GenerateUint();
		return 4.656613E-10f * (float)num;
	}

	// Token: 0x0600263F RID: 9791 RVA: 0x0001975C File Offset: 0x0001795C
	public double GenerateDouble()
	{
		return 2.3283064370807974E-10 * this.GenerateUint();
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x00128C2C File Offset: 0x00126E2C
	public double GenerateRangeDouble()
	{
		uint num = this.GenerateUint();
		return 4.656612874161595E-10 * (double)num;
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x00128C4C File Offset: 0x00126E4C
	public void SetSeed(uint val)
	{
		this.a = val;
		this.b = (val ^ 1842502087U);
		this.c = (val >> 5 ^ 1357980759U);
		this.d = (val >> 7 ^ 273326509U);
		for (uint num = 0U; num < 4U; num += 1U)
		{
			this.a = this.GenerateUint();
		}
	}

	// Token: 0x04002F10 RID: 12048
	private const uint B = 1842502087U;

	// Token: 0x04002F11 RID: 12049
	private const uint C = 1357980759U;

	// Token: 0x04002F12 RID: 12050
	private const uint D = 273326509U;

	// Token: 0x04002F13 RID: 12051
	private static uint counter;

	// Token: 0x04002F14 RID: 12052
	private uint a;

	// Token: 0x04002F15 RID: 12053
	private uint b;

	// Token: 0x04002F16 RID: 12054
	private uint c;

	// Token: 0x04002F17 RID: 12055
	private uint d;
}
