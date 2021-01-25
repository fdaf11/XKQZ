using System;
using System.Runtime.InteropServices;

namespace Sony
{
	// Token: 0x020006F7 RID: 1783
	public struct SONY_IMAGE_DOS_HEADER
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06002ACD RID: 10957 RVA: 0x0001BBC8 File Offset: 0x00019DC8
		private string _e_magic
		{
			get
			{
				return new string(this.e_magic);
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06002ACE RID: 10958 RVA: 0x0001BBD5 File Offset: 0x00019DD5
		public bool isValid
		{
			get
			{
				return this._e_magic == "MZ";
			}
		}

		// Token: 0x04003644 RID: 13892
		[MarshalAs(30, SizeConst = 2)]
		public char[] e_magic;

		// Token: 0x04003645 RID: 13893
		public ushort e_cblp;

		// Token: 0x04003646 RID: 13894
		public ushort e_cp;

		// Token: 0x04003647 RID: 13895
		public ushort e_crlc;

		// Token: 0x04003648 RID: 13896
		public ushort e_cparhdr;

		// Token: 0x04003649 RID: 13897
		public ushort e_minalloc;

		// Token: 0x0400364A RID: 13898
		public ushort e_maxalloc;

		// Token: 0x0400364B RID: 13899
		public ushort e_ss;

		// Token: 0x0400364C RID: 13900
		public ushort e_sp;

		// Token: 0x0400364D RID: 13901
		public ushort e_csum;

		// Token: 0x0400364E RID: 13902
		public ushort e_ip;

		// Token: 0x0400364F RID: 13903
		public ushort e_cs;

		// Token: 0x04003650 RID: 13904
		public ushort e_lfarlc;

		// Token: 0x04003651 RID: 13905
		public ushort e_ovno;

		// Token: 0x04003652 RID: 13906
		[MarshalAs(30, SizeConst = 4)]
		public ushort[] e_res1;

		// Token: 0x04003653 RID: 13907
		public ushort e_oemid;

		// Token: 0x04003654 RID: 13908
		public ushort e_oeminfo;

		// Token: 0x04003655 RID: 13909
		[MarshalAs(30, SizeConst = 10)]
		public ushort[] e_res2;

		// Token: 0x04003656 RID: 13910
		public int e_lfanew;
	}
}
