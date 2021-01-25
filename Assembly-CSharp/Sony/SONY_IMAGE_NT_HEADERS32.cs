using System;
using System.Runtime.InteropServices;

namespace Sony
{
	// Token: 0x020006FE RID: 1790
	[StructLayout(2)]
	public struct SONY_IMAGE_NT_HEADERS32
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06002ACF RID: 10959 RVA: 0x0001BBE7 File Offset: 0x00019DE7
		private string _Signature
		{
			get
			{
				return new string(this.Signature);
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06002AD0 RID: 10960 RVA: 0x0014DD70 File Offset: 0x0014BF70
		public bool isValid
		{
			get
			{
				return this._Signature == "PE\0\0" && (this.OptionalHeader.Magic == MagicType.IMAGE_NT_OPTIONAL_HDR32_MAGIC || this.OptionalHeader.Magic == MagicType.IMAGE_NT_OPTIONAL_HDR64_MAGIC);
			}
		}

		// Token: 0x040036AE RID: 13998
		[FieldOffset(0)]
		[MarshalAs(30, SizeConst = 4)]
		public char[] Signature;

		// Token: 0x040036AF RID: 13999
		[FieldOffset(4)]
		public SONY_IMAGE_FILE_HEADER FileHeader;

		// Token: 0x040036B0 RID: 14000
		[FieldOffset(24)]
		public SONY_IMAGE_OPTIONAL_HEADER32 OptionalHeader;
	}
}
