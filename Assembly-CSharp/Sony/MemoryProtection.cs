using System;

namespace Sony
{
	// Token: 0x02000706 RID: 1798
	[Flags]
	public enum MemoryProtection
	{
		// Token: 0x040036E0 RID: 14048
		Execute = 16,
		// Token: 0x040036E1 RID: 14049
		ExecuteRead = 32,
		// Token: 0x040036E2 RID: 14050
		ExecuteReadWrite = 64,
		// Token: 0x040036E3 RID: 14051
		ExecuteWriteCopy = 128,
		// Token: 0x040036E4 RID: 14052
		NoAccess = 1,
		// Token: 0x040036E5 RID: 14053
		ReadOnly = 2,
		// Token: 0x040036E6 RID: 14054
		ReadWrite = 4,
		// Token: 0x040036E7 RID: 14055
		WriteCopy = 8,
		// Token: 0x040036E8 RID: 14056
		GuardModifierflag = 256,
		// Token: 0x040036E9 RID: 14057
		NoCacheModifierflag = 512,
		// Token: 0x040036EA RID: 14058
		WriteCombineModifierflag = 1024
	}
}
