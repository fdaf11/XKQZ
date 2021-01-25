using System;
using System.Runtime.InteropServices;

namespace Sony
{
	// Token: 0x02000708 RID: 1800
	public struct OriginData
	{
		// Token: 0x040036F7 RID: 14071
		public uint dwPid;

		// Token: 0x040036F8 RID: 14072
		public uint dwOriginWindowHandle;

		// Token: 0x040036F9 RID: 14073
		[MarshalAs(30, SizeConst = 20)]
		public uint[] dwReserved01;
	}
}
