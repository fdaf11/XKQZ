using System;

namespace Sony
{
	// Token: 0x02000705 RID: 1797
	[Flags]
	public enum AllocationType
	{
		// Token: 0x040036D6 RID: 14038
		Commit = 4096,
		// Token: 0x040036D7 RID: 14039
		Reserve = 8192,
		// Token: 0x040036D8 RID: 14040
		Decommit = 16384,
		// Token: 0x040036D9 RID: 14041
		Release = 32768,
		// Token: 0x040036DA RID: 14042
		Reset = 524288,
		// Token: 0x040036DB RID: 14043
		Physical = 4194304,
		// Token: 0x040036DC RID: 14044
		TopDown = 1048576,
		// Token: 0x040036DD RID: 14045
		WriteWatch = 2097152,
		// Token: 0x040036DE RID: 14046
		LargePages = 536870912
	}
}
