using System;
using System.Runtime.InteropServices;

namespace FSC
{
	// Token: 0x020006DC RID: 1756
	public static class EncodeSystemPointerBinding
	{
		// Token: 0x06002A57 RID: 10839 RVA: 0x0014C6E0 File Offset: 0x0014A8E0
		public unsafe static void Generate(uint* data, uint datasize)
		{
			IntPtr moduleHandle = Helper.GetModuleHandle("Kernel32.dll");
			UIntPtr procAddress = Helper.GetProcAddress(moduleHandle, "EncodeSystemPointer");
			EncodeSystemPointerBinding.EncodeSystemPointerDelegate encodeSystemPointerDelegate = (EncodeSystemPointerBinding.EncodeSystemPointerDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)procAddress.ToPointer(), typeof(EncodeSystemPointerBinding.EncodeSystemPointerDelegate));
			if (procAddress != UIntPtr.Zero && datasize >= 4U)
			{
				*data = encodeSystemPointerDelegate(procAddress.ToPointer());
			}
			if (procAddress != UIntPtr.Zero && datasize >= 8U)
			{
				data[1] = encodeSystemPointerDelegate(((UIntPtr)(procAddress.ToUInt32() << 1)).ToPointer());
			}
		}

		// Token: 0x020006DD RID: 1757
		// (Invoke) Token: 0x06002A59 RID: 10841
		private unsafe delegate void* EncodeSystemPointerDelegate(void* ptr);
	}
}
