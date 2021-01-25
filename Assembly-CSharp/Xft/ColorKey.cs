using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x0200055B RID: 1371
	[Serializable]
	public class ColorKey : IComparable
	{
		// Token: 0x0600227A RID: 8826 RVA: 0x0001702E File Offset: 0x0001522E
		public ColorKey(float age, Color color)
		{
			this.t = age;
			this.Color = color;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x00017044 File Offset: 0x00015244
		public int CompareTo(object obj)
		{
			return -((ColorKey)obj).t.CompareTo(this.t);
		}

		// Token: 0x040028D8 RID: 10456
		public float t;

		// Token: 0x040028D9 RID: 10457
		public Color Color;
	}
}
