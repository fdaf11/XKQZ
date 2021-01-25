using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exploder.Core
{
	// Token: 0x020000B0 RID: 176
	public class Contour
	{
		// Token: 0x060003B6 RID: 950 RVA: 0x0000494D File Offset: 0x00002B4D
		public Contour(int trianglesNum)
		{
			this.AllocateBuffers(trianglesNum);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0002DDF0 File Offset: 0x0002BFF0
		public void AllocateBuffers(int trianglesNum)
		{
			if (this.lsHash == null || this.lsHash.Capacity() < trianglesNum * 2)
			{
				this.midPoints = new ArrayDictionary<Contour.MidPoint>(trianglesNum * 2);
				this.contour = new List<Dictionary<int, int>>();
				this.lsHash = new LSHash(0.001f, trianglesNum * 2);
			}
			else
			{
				this.lsHash.Clear();
				foreach (Dictionary<int, int> dictionary in this.contour)
				{
					dictionary.Clear();
				}
				this.contour.Clear();
				if (this.midPoints.Size < trianglesNum * 2)
				{
					this.midPoints = new ArrayDictionary<Contour.MidPoint>(trianglesNum * 2);
				}
				else
				{
					this.midPoints.Clear();
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000495C File Offset: 0x00002B5C
		// (set) Token: 0x060003B9 RID: 953 RVA: 0x00004964 File Offset: 0x00002B64
		public int MidPointsCount { get; private set; }

		// Token: 0x060003BA RID: 954 RVA: 0x0002DEE0 File Offset: 0x0002C0E0
		public void AddTriangle(int triangleID, int id0, int id1, Vector3 v0, Vector3 v1)
		{
			int num;
			int num2;
			this.lsHash.Hash(v0, v1, out num, out num2);
			if (num == num2)
			{
				return;
			}
			Contour.MidPoint value;
			if (this.midPoints.TryGetValue(num, out value))
			{
				if (value.idNext == 2147483647 && value.idPrev != num2)
				{
					value.idNext = num2;
				}
				else if (value.idPrev == 2147483647 && value.idNext != num2)
				{
					value.idPrev = num2;
				}
				this.midPoints[num] = value;
			}
			else
			{
				this.midPoints.Add(num, new Contour.MidPoint
				{
					id = num,
					vertexId = id0,
					idNext = num2,
					idPrev = int.MaxValue
				});
			}
			if (this.midPoints.TryGetValue(num2, out value))
			{
				if (value.idNext == 2147483647 && value.idPrev != num)
				{
					value.idNext = num;
				}
				else if (value.idPrev == 2147483647 && value.idNext != num)
				{
					value.idPrev = num;
				}
				this.midPoints[num2] = value;
			}
			else
			{
				this.midPoints.Add(num2, new Contour.MidPoint
				{
					id = num2,
					vertexId = id1,
					idPrev = num,
					idNext = int.MaxValue
				});
			}
			this.MidPointsCount = this.midPoints.Count;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0002E078 File Offset: 0x0002C278
		public bool FindContours()
		{
			if (this.midPoints.Count == 0)
			{
				return false;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>(this.midPoints.Count);
			int num = this.midPoints.Count * 2;
			Contour.MidPoint firstValue = this.midPoints.GetFirstValue();
			dictionary.Add(firstValue.id, firstValue.vertexId);
			this.midPoints.Remove(firstValue.id);
			int num2 = firstValue.idNext;
			while (this.midPoints.Count > 0)
			{
				if (num2 == 2147483647)
				{
					return false;
				}
				Contour.MidPoint midPoint;
				if (!this.midPoints.TryGetValue(num2, out midPoint))
				{
					this.contour.Clear();
					return false;
				}
				dictionary.Add(midPoint.id, midPoint.vertexId);
				this.midPoints.Remove(midPoint.id);
				if (dictionary.ContainsKey(midPoint.idNext))
				{
					if (dictionary.ContainsKey(midPoint.idPrev))
					{
						this.contour.Add(new Dictionary<int, int>(dictionary));
						dictionary.Clear();
						if (this.midPoints.Count == 0)
						{
							break;
						}
						firstValue = this.midPoints.GetFirstValue();
						dictionary.Add(firstValue.id, firstValue.vertexId);
						this.midPoints.Remove(firstValue.id);
						num2 = firstValue.idNext;
						continue;
					}
					else
					{
						num2 = midPoint.idPrev;
					}
				}
				else
				{
					num2 = midPoint.idNext;
				}
				num--;
				if (num == 0)
				{
					this.contour.Clear();
					return false;
				}
			}
			return true;
		}

		// Token: 0x040002E3 RID: 739
		public List<Dictionary<int, int>> contour;

		// Token: 0x040002E4 RID: 740
		private ArrayDictionary<Contour.MidPoint> midPoints;

		// Token: 0x040002E5 RID: 741
		private LSHash lsHash;

		// Token: 0x020000B1 RID: 177
		private struct MidPoint
		{
			// Token: 0x040002E7 RID: 743
			public int id;

			// Token: 0x040002E8 RID: 744
			public int vertexId;

			// Token: 0x040002E9 RID: 745
			public int idNext;

			// Token: 0x040002EA RID: 746
			public int idPrev;
		}
	}
}
