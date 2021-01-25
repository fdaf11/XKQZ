using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002EC RID: 748
	public class TipData
	{
		// Token: 0x06000FA6 RID: 4006 RVA: 0x0000A64A File Offset: 0x0000884A
		public TipData()
		{
			this.appendList = new List<string>();
			this.limitList = new List<string>();
			this.limitNPCList = new List<string>();
		}

		// Token: 0x040012B6 RID: 4790
		public string name;

		// Token: 0x040012B7 RID: 4791
		public string explain;

		// Token: 0x040012B8 RID: 4792
		public Texture texture;

		// Token: 0x040012B9 RID: 4793
		public List<string> appendList;

		// Token: 0x040012BA RID: 4794
		public List<string> limitList;

		// Token: 0x040012BB RID: 4795
		public List<string> limitNPCList;
	}
}
