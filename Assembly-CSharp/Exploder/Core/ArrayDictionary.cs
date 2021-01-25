using System;

namespace Exploder.Core
{
	// Token: 0x020000AE RID: 174
	internal class ArrayDictionary<T>
	{
		// Token: 0x060003AC RID: 940 RVA: 0x00004894 File Offset: 0x00002A94
		public ArrayDictionary(int size)
		{
			this.dictionary = new ArrayDictionary<T>.DicItem[size];
			this.Size = size;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000048AF File Offset: 0x00002AAF
		public bool ContainsKey(int key)
		{
			return key < this.Size && this.dictionary[key].valid;
		}

		// Token: 0x17000054 RID: 84
		public T this[int key]
		{
			get
			{
				return this.dictionary[key].data;
			}
			set
			{
				this.dictionary[key].data = value;
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0002DC84 File Offset: 0x0002BE84
		public void Clear()
		{
			for (int i = 0; i < this.Size; i++)
			{
				this.dictionary[i].data = default(T);
				this.dictionary[i].valid = false;
			}
			this.Count = 0;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000048F7 File Offset: 0x00002AF7
		public void Add(int key, T data)
		{
			this.dictionary[key].valid = true;
			this.dictionary[key].data = data;
			this.Count++;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000492B File Offset: 0x00002B2B
		public void Remove(int key)
		{
			this.dictionary[key].valid = false;
			this.Count--;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0002DCDC File Offset: 0x0002BEDC
		public T[] ToArray()
		{
			T[] array = new T[this.Count];
			int num = 0;
			for (int i = 0; i < this.Size; i++)
			{
				if (this.dictionary[i].valid)
				{
					array[num++] = this.dictionary[i].data;
					if (num == this.Count)
					{
						return array;
					}
				}
			}
			return null;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0002DD50 File Offset: 0x0002BF50
		public bool TryGetValue(int key, out T value)
		{
			ArrayDictionary<T>.DicItem dicItem = this.dictionary[key];
			if (dicItem.valid)
			{
				value = dicItem.data;
				return true;
			}
			value = default(T);
			return false;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0002DD9C File Offset: 0x0002BF9C
		public T GetFirstValue()
		{
			for (int i = 0; i < this.Size; i++)
			{
				ArrayDictionary<T>.DicItem dicItem = this.dictionary[i];
				if (dicItem.valid)
				{
					return dicItem.data;
				}
			}
			return default(T);
		}

		// Token: 0x040002DE RID: 734
		public int Count;

		// Token: 0x040002DF RID: 735
		public int Size;

		// Token: 0x040002E0 RID: 736
		private readonly ArrayDictionary<T>.DicItem[] dictionary;

		// Token: 0x020000AF RID: 175
		private struct DicItem
		{
			// Token: 0x040002E1 RID: 737
			public T data;

			// Token: 0x040002E2 RID: 738
			public bool valid;
		}
	}
}
