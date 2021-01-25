using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.PigeonCoopUtil
{
	// Token: 0x020005B9 RID: 1465
	public class CircularBuffer<T> : IEnumerable, IList<T>, ICollection<T>, IEnumerable<T>
	{
		// Token: 0x06002486 RID: 9350 RVA: 0x0001845D File Offset: 0x0001665D
		public CircularBuffer(int capacity)
		{
			if (capacity <= 0)
			{
				throw new ArgumentException("Must be greater than zero", "capacity");
			}
			this.Capacity = capacity;
			this._buffer = new T[capacity];
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06002487 RID: 9351 RVA: 0x00002C2D File Offset: 0x00000E2D
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x0001848F File Offset: 0x0001668F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000383 RID: 899
		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException();
				}
				int num = (this._position - this.Count + index) % this.Capacity;
				return this._buffer[num];
			}
			set
			{
				this.Insert(index, value);
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600248B RID: 9355 RVA: 0x000184A1 File Offset: 0x000166A1
		// (set) Token: 0x0600248C RID: 9356 RVA: 0x000184A9 File Offset: 0x000166A9
		public int Capacity { get; private set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x000184B2 File Offset: 0x000166B2
		// (set) Token: 0x0600248E RID: 9358 RVA: 0x000184BA File Offset: 0x000166BA
		public int Count { get; private set; }

		// Token: 0x0600248F RID: 9359 RVA: 0x0011DBCC File Offset: 0x0011BDCC
		public void Add(T item)
		{
			this._buffer[this._position++ % this.Capacity] = item;
			if (this.Count < this.Capacity)
			{
				this.Count++;
			}
			this._version += 1L;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x0011DC2C File Offset: 0x0011BE2C
		public void Clear()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this._buffer[i] = default(T);
			}
			this._position = 0;
			this.Count = 0;
			this._version += 1L;
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x0011DC84 File Offset: 0x0011BE84
		public bool Contains(T item)
		{
			int num = this.IndexOf(item);
			return num != -1;
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x0011DCA0 File Offset: 0x0011BEA0
		public void CopyTo(T[] array, int arrayIndex)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[i + arrayIndex] = this._buffer[(this._position - this.Count + i) % this.Capacity];
			}
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x0011DCF0 File Offset: 0x0011BEF0
		public IEnumerator<T> GetEnumerator()
		{
			long version = this._version;
			for (int i = 0; i < this.Count; i++)
			{
				if (version != this._version)
				{
					throw new InvalidOperationException("Collection changed");
				}
				yield return this[i];
			}
			yield break;
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x0011DD0C File Offset: 0x0011BF0C
		public int IndexOf(T item)
		{
			for (int i = 0; i < this.Count; i++)
			{
				T t = this._buffer[(this._position - this.Count + i) % this.Capacity];
				if (item == null && t == null)
				{
					return i;
				}
				if (item != null && item.Equals(t))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x0011DD90 File Offset: 0x0011BF90
		public void Insert(int index, T item)
		{
			if (index < 0 || index > this.Count)
			{
				throw new IndexOutOfRangeException();
			}
			if (index == this.Count)
			{
				this.Add(item);
				return;
			}
			int num = Math.Min(this.Count, this.Capacity - 1) - index;
			int num2 = (this._position - this.Count + index) % this.Capacity;
			for (int i = num2 + num; i > num2; i--)
			{
				int num3 = i % this.Capacity;
				int num4 = (i - 1) % this.Capacity;
				this._buffer[num3] = this._buffer[num4];
			}
			this._buffer[num2] = item;
			if (this.Count < this.Capacity)
			{
				this.Count++;
				this._position++;
			}
			this._version += 1L;
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x0011DE84 File Offset: 0x0011C084
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num == -1)
			{
				return false;
			}
			this.RemoveAt(num);
			return true;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x0011DEAC File Offset: 0x0011C0AC
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new IndexOutOfRangeException();
			}
			for (int i = index; i < this.Count - 1; i++)
			{
				int num = (this._position - this.Count + i) % this.Capacity;
				int num2 = (this._position - this.Count + i + 1) % this.Capacity;
				this._buffer[num] = this._buffer[num2];
			}
			int num3 = (this._position - 1) % this.Capacity;
			this._buffer[num3] = default(T);
			this._position--;
			this.Count--;
			this._version += 1L;
		}

		// Token: 0x04002C5A RID: 11354
		private T[] _buffer;

		// Token: 0x04002C5B RID: 11355
		private int _position;

		// Token: 0x04002C5C RID: 11356
		private long _version;
	}
}
