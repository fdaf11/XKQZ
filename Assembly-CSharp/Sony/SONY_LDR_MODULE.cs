using System;

namespace Sony
{
	// Token: 0x02000703 RID: 1795
	public struct SONY_LDR_MODULE
	{
		// Token: 0x040036C2 RID: 14018
		public SONY_LIST_ENTRY InLoadOrderModuleList;

		// Token: 0x040036C3 RID: 14019
		public SONY_LIST_ENTRY InMemoryOrderModuleList;

		// Token: 0x040036C4 RID: 14020
		public SONY_LIST_ENTRY InInitializationOrderModuleList;

		// Token: 0x040036C5 RID: 14021
		public unsafe void* BaseAddress;

		// Token: 0x040036C6 RID: 14022
		public unsafe void* EntryPoint;

		// Token: 0x040036C7 RID: 14023
		public uint SizeOfImage;

		// Token: 0x040036C8 RID: 14024
		public SONY_UNICODE_STRING FullDllName;

		// Token: 0x040036C9 RID: 14025
		public SONY_UNICODE_STRING BaseDllName;

		// Token: 0x040036CA RID: 14026
		public uint Flags;

		// Token: 0x040036CB RID: 14027
		public short LoadCount;

		// Token: 0x040036CC RID: 14028
		public short TlsIndex;

		// Token: 0x040036CD RID: 14029
		public SONY_LIST_ENTRY HashTableEntry;

		// Token: 0x040036CE RID: 14030
		public uint TimeDateStamp;
	}
}
