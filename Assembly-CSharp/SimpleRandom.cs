using System;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class SimpleRandom
{
	// Token: 0x060009A6 RID: 2470 RVA: 0x00007D59 File Offset: 0x00005F59
	public static void SetRandomSeed()
	{
		SimpleRandom.m_iCallTimes = 0;
		SimpleRandom.m_iRandomSeed = Random.Range(5000, 10000);
		SimpleRandom.m_iNext = Random.Range(5000, 10000);
		SimpleRandom.m_iPrime = 10001;
		SimpleRandom.m_bHasSeed = true;
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x00051D5C File Offset: 0x0004FF5C
	public static int PositiveOrNegative(int min, int max)
	{
		int num = SimpleRandom.Range(min, max);
		int num2 = SimpleRandom.Range(0, 100);
		if (num2 <= 50)
		{
			return num;
		}
		return -num;
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00051D88 File Offset: 0x0004FF88
	public static int Range(int min, int max)
	{
		int num = SimpleRandom.GetRandomNum() + min;
		num %= max - min;
		return num + min;
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00051DA8 File Offset: 0x0004FFA8
	public static int GetRandomNum()
	{
		if (!SimpleRandom.m_bHasSeed || SimpleRandom.m_iCallTimes > 10001)
		{
			SimpleRandom.SetRandomSeed();
		}
		SimpleRandom.m_iCallTimes++;
		SimpleRandom.m_iNext += SimpleRandom.m_iRandomSeed;
		SimpleRandom.m_iNext %= SimpleRandom.m_iPrime;
		return SimpleRandom.m_iNext;
	}

	// Token: 0x04000993 RID: 2451
	private static int m_iRandomSeed;

	// Token: 0x04000994 RID: 2452
	private static int m_iNext;

	// Token: 0x04000995 RID: 2453
	private static int m_iPrime;

	// Token: 0x04000996 RID: 2454
	private static int m_iCallTimes;

	// Token: 0x04000997 RID: 2455
	private static bool m_bHasSeed;
}
