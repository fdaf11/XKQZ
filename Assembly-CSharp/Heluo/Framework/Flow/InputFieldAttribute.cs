using System;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200014F RID: 335
	[AttributeUsage(256, Inherited = true, AllowMultiple = false)]
	internal sealed class InputFieldAttribute : Attribute
	{
		// Token: 0x0600070B RID: 1803 RVA: 0x0000630A File Offset: 0x0000450A
		public InputFieldAttribute(string name = null, bool horizontal = false)
		{
			this.Name = name;
			this.Horizontal = horizontal;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00006320 File Offset: 0x00004520
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x00006328 File Offset: 0x00004528
		public string Name { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00006331 File Offset: 0x00004531
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x00006339 File Offset: 0x00004539
		public bool Horizontal { get; set; }
	}
}
