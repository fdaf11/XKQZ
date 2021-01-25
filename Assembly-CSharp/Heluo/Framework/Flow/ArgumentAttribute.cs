using System;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000150 RID: 336
	[AttributeUsage(256, Inherited = false, AllowMultiple = false)]
	public sealed class ArgumentAttribute : Attribute
	{
		// Token: 0x06000710 RID: 1808 RVA: 0x00006342 File Offset: 0x00004542
		public ArgumentAttribute()
		{
			this.Description = string.Empty;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00006355 File Offset: 0x00004555
		public ArgumentAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00006364 File Offset: 0x00004564
		public ArgumentAttribute(string name, string desc)
		{
			this.Name = name;
			this.Description = desc;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0000637A File Offset: 0x0000457A
		public ArgumentAttribute(string name, string desc, bool optional)
		{
			this.Name = name;
			this.Description = desc;
			this.Optional = optional;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00006397 File Offset: 0x00004597
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x0000639F File Offset: 0x0000459F
		public string Name { get; private set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x000063A8 File Offset: 0x000045A8
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x000063B0 File Offset: 0x000045B0
		public string Description { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x000063B9 File Offset: 0x000045B9
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x000063C1 File Offset: 0x000045C1
		public string DefaultVariable { get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x000063CA File Offset: 0x000045CA
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x000063D2 File Offset: 0x000045D2
		public bool Optional { get; set; }
	}
}
