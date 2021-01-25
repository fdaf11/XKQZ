using System;

namespace Sony
{
	// Token: 0x02000700 RID: 1792
	public struct SONY_PEB_LDR_DATA
	{
		// Token: 0x040036B3 RID: 14003
		public uint Length;

		// Token: 0x040036B4 RID: 14004
		public byte Initialized;

		// Token: 0x040036B5 RID: 14005
		public unsafe void* SsHandle;

		// Token: 0x040036B6 RID: 14006
		public SONY_LIST_ENTRY InLoadOrderModuleList;

		// Token: 0x040036B7 RID: 14007
		public SONY_LIST_ENTRY InMemoryOrderModuleList;

		// Token: 0x040036B8 RID: 14008
		public SONY_LIST_ENTRY InInitializationOrderModuleList;

		// Token: 0x040036B9 RID: 14009
		public unsafe void* EntryInProgress;
	}
}
