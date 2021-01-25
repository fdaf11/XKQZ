using System;
using System.Runtime.InteropServices;

namespace Sony
{
	// Token: 0x02000704 RID: 1796
	[StructLayout(0, Pack = 4)]
	public struct NtProcessBasicInfo
	{
		// Token: 0x040036CF RID: 14031
		public IntPtr ExitStatus;

		// Token: 0x040036D0 RID: 14032
		public IntPtr PebBaseAddress;

		// Token: 0x040036D1 RID: 14033
		public IntPtr AffinityMask;

		// Token: 0x040036D2 RID: 14034
		public IntPtr BasePriority;

		// Token: 0x040036D3 RID: 14035
		public UIntPtr UniqueProcessId;

		// Token: 0x040036D4 RID: 14036
		public UIntPtr InheritedFromUniqueProcessId;
	}
}
