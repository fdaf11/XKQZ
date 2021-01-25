using System;
using System.Collections.Generic;

// Token: 0x02000685 RID: 1669
public static class GenericObjectPool<T>
{
	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x060028AB RID: 10411 RVA: 0x00141628 File Offset: 0x0013F828
	public static int Count
	{
		get
		{
			Stack<T> stack = GenericObjectPool<T>.pool;
			int count;
			lock (stack)
			{
				count = GenericObjectPool<T>.pool.Count;
			}
			return count;
		}
	}

	// Token: 0x060028AC RID: 10412 RVA: 0x00141670 File Offset: 0x0013F870
	public static void InitPool(int count)
	{
		for (int i = 0; i < count; i++)
		{
			T obj = GenericObjectPool<T>.Get();
			GenericObjectPool<T>.Return(obj);
		}
	}

	// Token: 0x060028AD RID: 10413 RVA: 0x0014169C File Offset: 0x0013F89C
	public static T Get()
	{
		if (GenericObjectPool<T>.Count > 0)
		{
			Stack<T> stack = GenericObjectPool<T>.pool;
			T result;
			lock (stack)
			{
				result = GenericObjectPool<T>.pool.Pop();
			}
			return result;
		}
		return Activator.CreateInstance<T>();
	}

	// Token: 0x060028AE RID: 10414 RVA: 0x001416F0 File Offset: 0x0013F8F0
	public static void Return(T obj)
	{
		Stack<T> stack = GenericObjectPool<T>.pool;
		lock (stack)
		{
			GenericObjectPool<T>.pool.Push(obj);
		}
	}

	// Token: 0x04003307 RID: 13063
	private static Stack<T> pool = new Stack<T>();
}
