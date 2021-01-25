using System;
using UnityEngine;

namespace Exploder.Core
{
	// Token: 0x020000B2 RID: 178
	public class LSHash
	{
		// Token: 0x060003BC RID: 956 RVA: 0x0000496D File Offset: 0x00002B6D
		public LSHash(float bucketSize, int allocSize)
		{
			this.bucketSize2 = bucketSize * bucketSize;
			this.buckets = new Vector3[allocSize];
			this.count = 0;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00004991 File Offset: 0x00002B91
		public int Capacity()
		{
			return this.buckets.Length;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0002E218 File Offset: 0x0002C418
		public void Clear()
		{
			for (int i = 0; i < this.buckets.Length; i++)
			{
				this.buckets[i] = Vector3.zero;
			}
			this.count = 0;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0002E25C File Offset: 0x0002C45C
		public int Hash(Vector3 p)
		{
			for (int i = 0; i < this.count; i++)
			{
				Vector3 vector = this.buckets[i];
				float num = p.x - vector.x;
				float num2 = p.y - vector.y;
				float num3 = p.z - vector.z;
				float num4 = num * num + num2 * num2 + num3 * num3;
				if (num4 < this.bucketSize2)
				{
					return i;
				}
			}
			if (this.count >= this.buckets.Length)
			{
				return this.count - 1;
			}
			this.buckets[this.count++] = p;
			return this.count - 1;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0002E32C File Offset: 0x0002C52C
		public void Hash(Vector3 p0, Vector3 p1, out int hash0, out int hash1)
		{
			float num = p0.x - p1.x;
			float num2 = p0.y - p1.y;
			float num3 = p0.z - p1.z;
			float num4 = num * num + num2 * num2 + num3 * num3;
			if (num4 < this.bucketSize2)
			{
				hash0 = this.Hash(p0);
				hash1 = hash0;
				return;
			}
			hash0 = this.Hash(p0);
			hash1 = this.Hash(p1);
		}

		// Token: 0x040002EB RID: 747
		private readonly Vector3[] buckets;

		// Token: 0x040002EC RID: 748
		private readonly float bucketSize2;

		// Token: 0x040002ED RID: 749
		private int count;
	}
}
