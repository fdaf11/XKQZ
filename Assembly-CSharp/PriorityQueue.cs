using System;
using System.Collections.Generic;

// Token: 0x020001D2 RID: 466
public class PriorityQueue<T>
{
	// Token: 0x06000995 RID: 2453 RVA: 0x00007CB0 File Offset: 0x00005EB0
	public PriorityQueue()
	{
		this.items = new List<T>();
		this.priorities = new List<int>();
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000996 RID: 2454 RVA: 0x00007CCE File Offset: 0x00005ECE
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x00051A64 File Offset: 0x0004FC64
	public int Enqueue(T item_, int priority_)
	{
		for (int i = 0; i < this.priorities.Count; i++)
		{
			if (this.priorities[i] > priority_)
			{
				this.items.Insert(i, item_);
				this.priorities.Insert(i, priority_);
				return i;
			}
		}
		this.items.Add(item_);
		this.priorities.Add(priority_);
		return this.items.Count - 1;
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x00051AE0 File Offset: 0x0004FCE0
	public T Dequeue()
	{
		T result = this.items[0];
		this.priorities.RemoveAt(0);
		this.items.RemoveAt(0);
		return result;
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x00007CDB File Offset: 0x00005EDB
	public T Peek()
	{
		return this.items[0];
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x00007CE9 File Offset: 0x00005EE9
	public int PeekPriority()
	{
		return this.priorities[0];
	}

	// Token: 0x0400098A RID: 2442
	private List<T> items;

	// Token: 0x0400098B RID: 2443
	private List<int> priorities;
}
