using System;
using System.Collections.Generic;

// Token: 0x02000686 RID: 1670
public class PooledDictionary<T, T2> : Dictionary<T, T2>
{
	// Token: 0x060028AF RID: 10415 RVA: 0x0001AD26 File Offset: 0x00018F26
	public PooledDictionary()
	{
	}

	// Token: 0x060028B0 RID: 10416 RVA: 0x0001AD2E File Offset: 0x00018F2E
	public PooledDictionary(int capacity) : base(capacity)
	{
	}

	// Token: 0x060028B1 RID: 10417 RVA: 0x0001AD37 File Offset: 0x00018F37
	public PooledDictionary(int capacity, IEqualityComparer<T> comparer) : base(capacity, comparer)
	{
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x060028B3 RID: 10419 RVA: 0x0001AD59 File Offset: 0x00018F59
	// (set) Token: 0x060028B4 RID: 10420 RVA: 0x0001AD61 File Offset: 0x00018F61
	public bool recycleable { get; set; }

	// Token: 0x060028B5 RID: 10421 RVA: 0x00141730 File Offset: 0x0013F930
	public static void Init(int initialPoolSize = 10)
	{
		for (int i = 0; i < initialPoolSize; i++)
		{
			PooledDictionary<T, T2>.stack.Push(new PooledDictionary<T, T2>());
		}
	}

	// Token: 0x060028B6 RID: 10422 RVA: 0x00141760 File Offset: 0x0013F960
	public static PooledDictionary<T, T2> Get(int capacity, IEqualityComparer<T> comparer = null)
	{
		Stack<PooledDictionary<T, T2>> stack = PooledDictionary<T, T2>.stack;
		lock (stack)
		{
			if (PooledDictionary<T, T2>.stack.Count > 0)
			{
				PooledDictionary<T, T2>.checkedOut += 1U;
				PooledDictionary<T, T2> pooledDictionary = PooledDictionary<T, T2>.stack.Pop();
				pooledDictionary.recycleable = true;
				return pooledDictionary;
			}
		}
		PooledDictionary<T, T2>.checkedOut += 1U;
		if (comparer != null)
		{
			return new PooledDictionary<T, T2>(capacity, comparer)
			{
				recycleable = true
			};
		}
		return new PooledDictionary<T, T2>(capacity)
		{
			recycleable = true
		};
	}

	// Token: 0x060028B7 RID: 10423 RVA: 0x0001AD6A File Offset: 0x00018F6A
	public static PooledDictionary<T, T2> Get()
	{
		return PooledDictionary<T, T2>.Get(0, null);
	}

	// Token: 0x060028B8 RID: 10424 RVA: 0x0001AD73 File Offset: 0x00018F73
	public static PooledDictionary<T, T2> Get(IEqualityComparer<T> comparer)
	{
		return PooledDictionary<T, T2>.Get(0, comparer);
	}

	// Token: 0x060028B9 RID: 10425 RVA: 0x00141800 File Offset: 0x0013FA00
	public void ReturnToPool(bool force = false)
	{
		if (!this.recycleable && !force)
		{
			return;
		}
		this.recycleable = true;
		PooledDictionary<T, T2>.returned += 1U;
		this.Clear();
		Stack<PooledDictionary<T, T2>> stack = PooledDictionary<T, T2>.stack;
		lock (stack)
		{
			PooledDictionary<T, T2>.stack.Push(this);
		}
	}

	// Token: 0x04003308 RID: 13064
	private static Stack<PooledDictionary<T, T2>> stack = new Stack<PooledDictionary<T, T2>>();

	// Token: 0x04003309 RID: 13065
	private static uint checkedOut = 0U;

	// Token: 0x0400330A RID: 13066
	private static uint returned = 0U;
}
