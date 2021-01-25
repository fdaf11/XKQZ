using System;
using System.Collections.Generic;

// Token: 0x0200067D RID: 1661
public static class AllocatedArray<T>
{
	// Token: 0x06002892 RID: 10386 RVA: 0x0001AC1C File Offset: 0x00018E1C
	private static T[] AllocateArray(int size)
	{
		return new T[size];
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x00141570 File Offset: 0x0013F770
	public static T[] Get(int size)
	{
		if (!AllocatedArray<T>.allocatedArrays.ContainsKey(size))
		{
			return AllocatedArray<T>.AllocateArray(size);
		}
		if (AllocatedArray<T>.allocatedArrays[size].Count == 0)
		{
			return AllocatedArray<T>.AllocateArray(size);
		}
		return AllocatedArray<T>.allocatedArrays[size].Pop();
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x001415C0 File Offset: 0x0013F7C0
	public static void Return(T[] array)
	{
		if (!AllocatedArray<T>.allocatedArrays.ContainsKey(array.Length))
		{
			AllocatedArray<T>.allocatedArrays.Add(array.Length, new Stack<T[]>());
		}
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = default(T);
		}
		AllocatedArray<T>.allocatedArrays[array.Length].Push(array);
	}

	// Token: 0x040032FF RID: 13055
	private static Dictionary<int, Stack<T[]>> allocatedArrays = new Dictionary<int, Stack<T[]>>(Comparers.Int);
}
