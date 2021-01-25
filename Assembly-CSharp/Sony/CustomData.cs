using System;
using System.Runtime.InteropServices;

namespace Sony
{
	// Token: 0x02000709 RID: 1801
	public struct CustomData
	{
		// Token: 0x040036FA RID: 14074
		public uint dwApiVersion;

		// Token: 0x040036FB RID: 14075
		public Type pOriginStuff;

		// Token: 0x040036FC RID: 14076
		[MarshalAs(30, SizeConst = 20)]
		public uint[] dwReserved01;
	}
}
