using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Sony
{
	// Token: 0x020006F3 RID: 1779
	public static class SonySecurityApi
	{
		// Token: 0x06002AC5 RID: 10949
		[DllImport("kernel32.dll", CharSet = 4)]
		private static extern void OutputDebugString(string message);

		// Token: 0x06002AC6 RID: 10950
		[DllImport("SPLogDlg", CallingConvention = 3)]
		private static extern int splog(int spnum, EventType type, StringBuilder s);

		// Token: 0x06002AC7 RID: 10951 RVA: 0x0014D964 File Offset: 0x0014BB64
		public static void DataDirectoryCheck(int _iID, ref uint _pdwResult, uint _dwFailedValue)
		{
			SonySecurityApi.SonyDebugBegin(_iID, "DataDirectoryCheck");
			IntPtr moduleHandle = Helper.GetModuleHandle(null);
			SONY_IMAGE_DOS_HEADER sony_IMAGE_DOS_HEADER = moduleHandle.ReadMemory<SONY_IMAGE_DOS_HEADER>();
			SONY_IMAGE_NT_HEADERS32 sony_IMAGE_NT_HEADERS = default(SONY_IMAGE_NT_HEADERS32);
			if (sony_IMAGE_DOS_HEADER.isValid)
			{
				IntPtr atAddress = (IntPtr)(moduleHandle.ToInt32() + sony_IMAGE_DOS_HEADER.e_lfanew);
				sony_IMAGE_NT_HEADERS = atAddress.ReadMemory<SONY_IMAGE_NT_HEADERS32>();
			}
			if (sony_IMAGE_DOS_HEADER.isValid && sony_IMAGE_NT_HEADERS.isValid)
			{
				ulong num = (ulong)sony_IMAGE_NT_HEADERS.OptionalHeader.IAT.Size;
				ulong num2 = (ulong)sony_IMAGE_NT_HEADERS.OptionalHeader.IAT.VirtualAddress;
				ulong num3 = (ulong)sony_IMAGE_NT_HEADERS.OptionalHeader.ImportTable.Size;
				ulong num4 = (ulong)sony_IMAGE_NT_HEADERS.OptionalHeader.ImportTable.VirtualAddress;
				if (num == 0UL || num2 == 0UL || num3 == 0UL || num4 == 0UL)
				{
					_pdwResult = _dwFailedValue;
				}
			}
			else
			{
				_pdwResult = _dwFailedValue;
			}
			int fileLineNumber = new StackFrame(0, true).GetFileLineNumber();
			string fileName = new StackFrame(0, true).GetFileName();
			SonySecurityApi.SonyDebugEnd(_iID, "DataDirectoryCheck", ref _pdwResult, _dwFailedValue, fileLineNumber, fileName);
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x0014DA78 File Offset: 0x0014BC78
		public unsafe static void HiddenSectionCheck(int _iID, ref uint _pdwResult, uint _dwFailedValue)
		{
			SonySecurityApi.SonyDebugBegin(_iID, "HiddenSectionCheck");
			Process currentProcess = Process.GetCurrentProcess();
			IntPtr handle = currentProcess.Handle;
			IntPtr pebAddress = Helper.GetPebAddress(handle);
			IntPtr atAddress = (IntPtr)((void*)pebAddress.ReadMemory<SONY_PEB>().Ldr->InLoadOrderModuleList.Flink);
			SONY_LDR_MODULE sony_LDR_MODULE = atAddress.ReadMemory<SONY_LDR_MODULE>();
			IntPtr moduleHandle = Helper.GetModuleHandle(null);
			SONY_IMAGE_DOS_HEADER sony_IMAGE_DOS_HEADER = moduleHandle.ReadMemory<SONY_IMAGE_DOS_HEADER>();
			SONY_IMAGE_NT_HEADERS32 sony_IMAGE_NT_HEADERS = default(SONY_IMAGE_NT_HEADERS32);
			IntPtr intPtr = IntPtr.Zero;
			SONY_IMAGE_SECTION_HEADER sony_IMAGE_SECTION_HEADER = default(SONY_IMAGE_SECTION_HEADER);
			if (sony_IMAGE_DOS_HEADER.isValid)
			{
				IntPtr atAddress2 = (IntPtr)(moduleHandle.ToInt32() + sony_IMAGE_DOS_HEADER.e_lfanew);
				sony_IMAGE_NT_HEADERS = atAddress2.ReadMemory<SONY_IMAGE_NT_HEADERS32>();
				if (sony_IMAGE_NT_HEADERS.isValid)
				{
					intPtr = (IntPtr)(atAddress2.ToInt32() + 4 + sizeof(SONY_IMAGE_FILE_HEADER) + (int)sony_IMAGE_NT_HEADERS.FileHeader.SizeOfOptionalHeader);
				}
			}
			if (intPtr != IntPtr.Zero)
			{
				uint num = 0U;
				for (uint num2 = 0U; num2 < (uint)sony_IMAGE_NT_HEADERS.FileHeader.NumberOfSections; num2 += 1U)
				{
					IntPtr atAddress3 = (IntPtr)((long)intPtr.ToInt32() + (long)((ulong)num2 * (ulong)((long)Marshal.SizeOf(sony_IMAGE_SECTION_HEADER))));
					SONY_IMAGE_SECTION_HEADER sony_IMAGE_SECTION_HEADER2 = atAddress3.ReadMemory<SONY_IMAGE_SECTION_HEADER>();
					if (sony_IMAGE_SECTION_HEADER2.VirtualAddress > num)
					{
						num = sony_IMAGE_SECTION_HEADER2.VirtualAddress;
						sony_IMAGE_SECTION_HEADER = sony_IMAGE_SECTION_HEADER2;
					}
				}
				uint num3 = Helper.SonyAligneSection(sony_IMAGE_SECTION_HEADER.VirtualSize, sony_IMAGE_NT_HEADERS.OptionalHeader.SectionAlignment);
				uint num4 = sony_IMAGE_SECTION_HEADER.VirtualAddress + num3 + 1134U;
				if (num4 < sony_LDR_MODULE.SizeOfImage)
				{
					_pdwResult = _dwFailedValue;
				}
			}
			if (!sony_IMAGE_DOS_HEADER.isValid || !sony_IMAGE_NT_HEADERS.isValid || intPtr == IntPtr.Zero)
			{
				_pdwResult = _dwFailedValue;
			}
			int fileLineNumber = new StackFrame(0, true).GetFileLineNumber();
			string fileName = new StackFrame(0, true).GetFileName();
			SonySecurityApi.SonyDebugEnd(_iID, "HiddenSectionCheck", ref _pdwResult, _dwFailedValue, fileLineNumber, fileName);
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x0014DC60 File Offset: 0x0014BE60
		public unsafe static void CompareImageSizeCheck(int _iID, ref uint _pdwResult, uint _dwFailedValue)
		{
			SonySecurityApi.SonyDebugBegin(_iID, "CompareImageSizeCheck");
			Process currentProcess = Process.GetCurrentProcess();
			IntPtr handle = currentProcess.Handle;
			IntPtr pebAddress = Helper.GetPebAddress(handle);
			IntPtr atAddress = (IntPtr)((void*)pebAddress.ReadMemory<SONY_PEB>().Ldr->InLoadOrderModuleList.Flink);
			SONY_LDR_MODULE sony_LDR_MODULE = atAddress.ReadMemory<SONY_LDR_MODULE>();
			IntPtr moduleHandle = Helper.GetModuleHandle(null);
			SONY_IMAGE_DOS_HEADER sony_IMAGE_DOS_HEADER = moduleHandle.ReadMemory<SONY_IMAGE_DOS_HEADER>();
			SONY_IMAGE_NT_HEADERS32 sony_IMAGE_NT_HEADERS = default(SONY_IMAGE_NT_HEADERS32);
			if (sony_IMAGE_DOS_HEADER.isValid)
			{
				IntPtr atAddress2 = (IntPtr)(moduleHandle.ToInt32() + sony_IMAGE_DOS_HEADER.e_lfanew);
				sony_IMAGE_NT_HEADERS = atAddress2.ReadMemory<SONY_IMAGE_NT_HEADERS32>();
			}
			if (sony_IMAGE_NT_HEADERS.isValid && (sony_IMAGE_NT_HEADERS.OptionalHeader.SizeOfImage & 268435455U) != sony_LDR_MODULE.SizeOfImage)
			{
				_pdwResult = _dwFailedValue;
			}
			if (!sony_IMAGE_DOS_HEADER.isValid || !sony_IMAGE_NT_HEADERS.isValid)
			{
				_pdwResult = _dwFailedValue;
			}
			int fileLineNumber = new StackFrame(0, true).GetFileLineNumber();
			string fileName = new StackFrame(0, true).GetFileName();
			SonySecurityApi.SonyDebugEnd(_iID, "CompareImageSizeCheck", ref _pdwResult, _dwFailedValue, fileLineNumber, fileName);
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x0000264F File Offset: 0x0000084F
		private static void SonyDebugBegin(int _iID, string _szSpName)
		{
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x0000264F File Offset: 0x0000084F
		private static void SonyDebugEnd(int _iID, string _szSpName, ref uint _pdwResult, uint _dwFailedValue, int lineNumber, string fileName)
		{
		}

		// Token: 0x040035F2 RID: 13810
		private const int SONY_API_VERSION_MAJOR = 4;

		// Token: 0x040035F3 RID: 13811
		private const int SONY_API_VERSION_MINOR = 1;

		// Token: 0x040035F4 RID: 13812
		private const int SONY_API_VERSION_BUILD = 1;

		// Token: 0x040035F5 RID: 13813
		private const int SONY_IMAGE_SIZEOF_SHORT_NAME = 8;

		// Token: 0x040035F6 RID: 13814
		private const int SONY_IMAGE_SIZEOF_SECTION_HEADER = 40;

		// Token: 0x040035F7 RID: 13815
		private const int SONY_IMAGE_DIRECTORY_ENTRY_EXPORT = 0;

		// Token: 0x040035F8 RID: 13816
		private const int SONY_IMAGE_DIRECTORY_ENTRY_IMPORT = 1;

		// Token: 0x040035F9 RID: 13817
		private const int SONY_IMAGE_DIRECTORY_ENTRY_RESOURCE = 2;

		// Token: 0x040035FA RID: 13818
		private const int SONY_IMAGE_DIRECTORY_ENTRY_EXCEPTION = 3;

		// Token: 0x040035FB RID: 13819
		private const int SONY_IMAGE_DIRECTORY_ENTRY_SECURITY = 4;

		// Token: 0x040035FC RID: 13820
		private const int SONY_IMAGE_DIRECTORY_ENTRY_BASERELOC = 5;

		// Token: 0x040035FD RID: 13821
		private const int SONY_IMAGE_DIRECTORY_ENTRY_DEBUG = 6;

		// Token: 0x040035FE RID: 13822
		private const int SONY_IMAGE_DIRECTORY_ENTRY_COPYRIGHT = 7;

		// Token: 0x040035FF RID: 13823
		private const int SONY_IMAGE_DIRECTORY_ENTRY_ARCHITECTURE = 7;

		// Token: 0x04003600 RID: 13824
		private const int SONY_IMAGE_DIRECTORY_ENTRY_GLOBALPTR = 8;

		// Token: 0x04003601 RID: 13825
		private const int SONY_IMAGE_DIRECTORY_ENTRY_TLS = 9;

		// Token: 0x04003602 RID: 13826
		private const int SONY_IMAGE_DIRECTORY_ENTRY_LOAD_CONFIG = 10;

		// Token: 0x04003603 RID: 13827
		private const int SONY_IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT = 11;

		// Token: 0x04003604 RID: 13828
		private const int SONY_IMAGE_DIRECTORY_ENTRY_IAT = 12;

		// Token: 0x04003605 RID: 13829
		private const int SONY_IMAGE_DIRECTORY_ENTRY_DELAY_IMPORT = 13;

		// Token: 0x04003606 RID: 13830
		private const int SONY_IMAGE_DIRECTORY_ENTRY_COM_DESCRIPTOR = 14;

		// Token: 0x04003607 RID: 13831
		private const int SONY_IMAGE_NUMBEROF_DIRECTORY_ENTRIES = 16;

		// Token: 0x04003608 RID: 13832
		private const string SONY_LOG_DLL_NAME = "SPLogDlg.dll";

		// Token: 0x04003609 RID: 13833
		private const string SONY_LOG_FUNCTION_NAME = "splog";

		// Token: 0x0400360A RID: 13834
		private const string SONY_SHOWDLG_FUNCTION_NAME = "showDlg";

		// Token: 0x0400360B RID: 13835
		private static GlobalDefines gSonyGlobalDefines;

		// Token: 0x0400360C RID: 13836
		private static CustomData gSonyCustomData;
	}
}
