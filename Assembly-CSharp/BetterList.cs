using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000495 RID: 1173
public class BetterList<T>
{
	// Token: 0x06001C43 RID: 7235 RVA: 0x000DC860 File Offset: 0x000DAA60
	public IEnumerator<T> GetEnumerator()
	{
		if (this.buffer != null)
		{
			for (int i = 0; i < this.size; i++)
			{
				yield return this.buffer[i];
			}
		}
		yield break;
	}

	// Token: 0x17000241 RID: 577
	[DebuggerHidden]
	public T this[int i]
	{
		get
		{
			return this.buffer[i];
		}
		set
		{
			this.buffer[i] = value;
		}
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x000DC87C File Offset: 0x000DAA7C
	private void AllocateMore()
	{
		T[] array = (this.buffer == null) ? new T[32] : new T[Mathf.Max(this.buffer.Length << 1, 32)];
		if (this.buffer != null && this.size > 0)
		{
			this.buffer.CopyTo(array, 0);
		}
		this.buffer = array;
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x000DC8E4 File Offset: 0x000DAAE4
	private void Trim()
	{
		if (this.size > 0)
		{
			if (this.size < this.buffer.Length)
			{
				T[] array = new T[this.size];
				for (int i = 0; i < this.size; i++)
				{
					array[i] = this.buffer[i];
				}
				this.buffer = array;
			}
		}
		else
		{
			this.buffer = null;
		}
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x00012D36 File Offset: 0x00010F36
	public void Clear()
	{
		this.size = 0;
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x00012D3F File Offset: 0x00010F3F
	public void Release()
	{
		this.size = 0;
		this.buffer = null;
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x000DC95C File Offset: 0x000DAB5C
	public void Add(T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		this.buffer[this.size++] = item;
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x000DC9AC File Offset: 0x000DABAC
	public void Insert(int index, T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		if (index > -1 && index < this.size)
		{
			for (int i = this.size; i > index; i--)
			{
				this.buffer[i] = this.buffer[i - 1];
			}
			this.buffer[index] = item;
			this.size++;
		}
		else
		{
			this.Add(item);
		}
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x000DCA48 File Offset: 0x000DAC48
	public bool Contains(T item)
	{
		if (this.buffer == null)
		{
			return false;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x000DCAA0 File Offset: 0x000DACA0
	public int IndexOf(T item)
	{
		if (this.buffer == null)
		{
			return -1;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x000DCAF8 File Offset: 0x000DACF8
	public bool Remove(T item)
	{
		if (this.buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.size; i++)
			{
				if (@default.Equals(this.buffer[i], item))
				{
					this.size--;
					this.buffer[i] = default(T);
					for (int j = i; j < this.size; j++)
					{
						this.buffer[j] = this.buffer[j + 1];
					}
					this.buffer[this.size] = default(T);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x000DCBB8 File Offset: 0x000DADB8
	public void RemoveAt(int index)
	{
		if (this.buffer != null && index > -1 && index < this.size)
		{
			this.size--;
			this.buffer[index] = default(T);
			for (int i = index; i < this.size; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
			this.buffer[this.size] = default(T);
		}
	}

	// Token: 0x06001C50 RID: 7248 RVA: 0x000DCC54 File Offset: 0x000DAE54
	public T Pop()
	{
		if (this.buffer != null && this.size != 0)
		{
			T result = this.buffer[--this.size];
			this.buffer[this.size] = default(T);
			return result;
		}
		return default(T);
	}

	// Token: 0x06001C51 RID: 7249 RVA: 0x00012D4F File Offset: 0x00010F4F
	public T[] ToArray()
	{
		this.Trim();
		return this.buffer;
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x000DCCBC File Offset: 0x000DAEBC
	[DebuggerHidden]
	[DebuggerStepThrough]
	public void Sort(BetterList<T>.CompareFunc comparer)
	{
		int num = 0;
		int num2 = this.size - 1;
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = num; i < num2; i++)
			{
				if (comparer(this.buffer[i], this.buffer[i + 1]) > 0)
				{
					T t = this.buffer[i];
					this.buffer[i] = this.buffer[i + 1];
					this.buffer[i + 1] = t;
					flag = true;
				}
				else if (!flag)
				{
					num = ((i != 0) ? (i - 1) : 0);
				}
			}
		}
	}

	// Token: 0x04002136 RID: 8502
	public T[] buffer;

	// Token: 0x04002137 RID: 8503
	public int size;

	// Token: 0x02000496 RID: 1174
	// (Invoke) Token: 0x06001C54 RID: 7252
	public delegate int CompareFunc(T left, T right);
}
