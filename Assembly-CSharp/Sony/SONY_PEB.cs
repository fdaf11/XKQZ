using System;
using System.Runtime.InteropServices;

namespace Sony
{
	// Token: 0x02000701 RID: 1793
	[StructLayout(2)]
	public struct SONY_PEB
	{
		// Token: 0x040036BA RID: 14010
		[FieldOffset(0)]
		[MarshalAs(30, SizeConst = 2)]
		public byte[] Reserved1;

		// Token: 0x040036BB RID: 14011
		[FieldOffset(2)]
		public byte BeingDebugged;

		// Token: 0x040036BC RID: 14012
		[FieldOffset(3)]
		public byte Reserved2;

		// Token: 0x040036BD RID: 14013
		[FieldOffset(4)]
		[MarshalAs(30, SizeConst = 2)]
		public IntPtr[] Reserved3;

		// Token: 0x040036BE RID: 14014
		[FieldOffset(12)]
		public unsafe SONY_PEB_LDR_DATA* Ldr;
	}
}
