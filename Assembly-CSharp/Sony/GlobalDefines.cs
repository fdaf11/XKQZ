using System;
using System.Runtime.InteropServices;

namespace Sony
{
	// Token: 0x0200070A RID: 1802
	public struct GlobalDefines
	{
		// Token: 0x040036FD RID: 14077
		public uint dwPattern1;

		// Token: 0x040036FE RID: 14078
		public uint dwPattern2;

		// Token: 0x040036FF RID: 14079
		public uint dwPattern3;

		// Token: 0x04003700 RID: 14080
		public uint dwPattern4;

		// Token: 0x04003701 RID: 14081
		public uint dwPattern5;

		// Token: 0x04003702 RID: 14082
		public uint pPreOEPFnc1;

		// Token: 0x04003703 RID: 14083
		public uint pPreOEPFnc2;

		// Token: 0x04003704 RID: 14084
		public uint pSonyCustomData;

		// Token: 0x04003705 RID: 14085
		[MarshalAs(30, SizeConst = 100)]
		public uint[] dwReserved01;
	}
}
