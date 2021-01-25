using System;

namespace Sony
{
	// Token: 0x020006F8 RID: 1784
	public struct SONY_IMAGE_FILE_HEADER
	{
		// Token: 0x04003657 RID: 13911
		public ushort Machine;

		// Token: 0x04003658 RID: 13912
		public ushort NumberOfSections;

		// Token: 0x04003659 RID: 13913
		public uint TimeDateStamp;

		// Token: 0x0400365A RID: 13914
		public uint PointerToSymbolTable;

		// Token: 0x0400365B RID: 13915
		public uint NumberOfSymbols;

		// Token: 0x0400365C RID: 13916
		public ushort SizeOfOptionalHeader;

		// Token: 0x0400365D RID: 13917
		public ushort Characteristics;
	}
}
