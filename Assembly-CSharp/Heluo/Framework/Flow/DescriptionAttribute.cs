using System;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000151 RID: 337
	[AttributeUsage(4, Inherited = false, AllowMultiple = false)]
	public sealed class DescriptionAttribute : Attribute
	{
		// Token: 0x0600071C RID: 1820 RVA: 0x000063DB File Offset: 0x000045DB
		public DescriptionAttribute(string desc)
		{
			this.Value = desc;
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x000063EA File Offset: 0x000045EA
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x000063F2 File Offset: 0x000045F2
		public string Value { get; set; }
	}
}
