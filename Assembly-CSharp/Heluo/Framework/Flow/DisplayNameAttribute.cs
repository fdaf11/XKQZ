using System;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000156 RID: 342
	[AttributeUsage(960, AllowMultiple = false)]
	public class DisplayNameAttribute : PropertyAttribute
	{
		// Token: 0x0600072D RID: 1837 RVA: 0x00006487 File Offset: 0x00004687
		public DisplayNameAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x00006496 File Offset: 0x00004696
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0000649E File Offset: 0x0000469E
		public string Name { get; protected set; }
	}
}
