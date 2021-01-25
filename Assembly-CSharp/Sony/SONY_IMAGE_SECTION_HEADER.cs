using System;
using System.Runtime.InteropServices;

namespace Sony
{
	// Token: 0x020006F4 RID: 1780
	[StructLayout(2)]
	public struct SONY_IMAGE_SECTION_HEADER
	{
		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06002ACC RID: 10956 RVA: 0x0001BBBB File Offset: 0x00019DBB
		public string Section
		{
			get
			{
				return new string(this.Name);
			}
		}

		// Token: 0x0400360D RID: 13837
		[FieldOffset(0)]
		[MarshalAs(30, SizeConst = 8)]
		public char[] Name;

		// Token: 0x0400360E RID: 13838
		[FieldOffset(8)]
		public uint VirtualSize;

		// Token: 0x0400360F RID: 13839
		[FieldOffset(12)]
		public uint VirtualAddress;

		// Token: 0x04003610 RID: 13840
		[FieldOffset(16)]
		public uint SizeOfRawData;

		// Token: 0x04003611 RID: 13841
		[FieldOffset(20)]
		public uint PointerToRawData;

		// Token: 0x04003612 RID: 13842
		[FieldOffset(24)]
		public uint PointerToRelocations;

		// Token: 0x04003613 RID: 13843
		[FieldOffset(28)]
		public uint PointerToLinenumbers;

		// Token: 0x04003614 RID: 13844
		[FieldOffset(32)]
		public ushort NumberOfRelocations;

		// Token: 0x04003615 RID: 13845
		[FieldOffset(34)]
		public ushort NumberOfLinenumbers;

		// Token: 0x04003616 RID: 13846
		[FieldOffset(36)]
		public DataSectionFlags Characteristics;
	}
}
