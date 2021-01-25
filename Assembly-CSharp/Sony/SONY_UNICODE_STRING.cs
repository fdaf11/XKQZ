using System;
using System.Runtime.InteropServices;

namespace Sony
{
	// Token: 0x02000702 RID: 1794
	public struct SONY_UNICODE_STRING : IDisposable
	{
		// Token: 0x06002AD1 RID: 10961 RVA: 0x0001BBF4 File Offset: 0x00019DF4
		public SONY_UNICODE_STRING(string s)
		{
			this.Length = (ushort)(s.Length * 2);
			this.MaximumLength = this.Length + 2;
			this.buffer = Marshal.StringToHGlobalUni(s);
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x0001BC20 File Offset: 0x00019E20
		public void Dispose()
		{
			Marshal.FreeHGlobal(this.buffer);
			this.buffer = IntPtr.Zero;
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x0001BC38 File Offset: 0x00019E38
		public override string ToString()
		{
			return Marshal.PtrToStringUni(this.buffer);
		}

		// Token: 0x040036BF RID: 14015
		public ushort Length;

		// Token: 0x040036C0 RID: 14016
		public ushort MaximumLength;

		// Token: 0x040036C1 RID: 14017
		public IntPtr buffer;
	}
}
