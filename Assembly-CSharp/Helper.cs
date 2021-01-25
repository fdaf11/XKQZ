using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Sony;
using UnityEngine;

// Token: 0x020006EE RID: 1774
public static class Helper
{
	// Token: 0x06002A8B RID: 10891
	[DllImport("kernel32.dll", CharSet = 4)]
	private static extern void OutputDebugString(string message);

	// Token: 0x06002A8C RID: 10892
	[DllImport("kernel32.dll")]
	public static extern IntPtr GetModuleHandle(string lpModuleName);

	// Token: 0x06002A8D RID: 10893
	[DllImport("ntdll.dll", CharSet = 3)]
	private static extern int NtQueryInformationProcess(IntPtr processHandle, int query, ref NtProcessBasicInfo info, int size, out int returnedSize);

	// Token: 0x06002A8E RID: 10894
	[DllImport("kernel32.dll")]
	private static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In] [Out] byte[] buffer, uint size, out IntPtr lpNumberOfBytesRead);

	// Token: 0x06002A8F RID: 10895
	[DllImport("kernel32", CharSet = 2, ExactSpelling = true, SetLastError = true)]
	public static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);

	// Token: 0x06002A90 RID: 10896
	[DllImport("kernel32")]
	public static extern uint GetVersion();

	// Token: 0x06002A91 RID: 10897
	[DllImport("user32.dll", CharSet = 4, EntryPoint = "SendMessageA")]
	public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

	// Token: 0x06002A92 RID: 10898
	[DllImport("Kernel32.dll", CharSet = 4, EntryPoint = "GetVolumeInformationA", SetLastError = true)]
	public static extern bool GetVolumeInformation(string RootPathName, StringBuilder VolumeNameBuffer, int VolumeNameSize, IntPtr VolumeSerialNumber, IntPtr MaximumComponentLength, IntPtr FileSystemFlags, StringBuilder FileSystemNameBuffer, int nFileSystemNameSize);

	// Token: 0x06002A93 RID: 10899
	[DllImport("Kernel32.dll", CharSet = 4, EntryPoint = "GetVolumeInformationW", SetLastError = true)]
	public static extern bool GetVolumeInformation(IntPtr RootPathName, StringBuilder VolumeNameBuffer, int VolumeNameSize, IntPtr VolumeSerialNumber, IntPtr MaximumComponentLength, IntPtr FileSystemFlags, StringBuilder FileSystemNameBuffer, int nFileSystemNameSize);

	// Token: 0x06002A94 RID: 10900
	[DllImport("Kernel32.dll", EntryPoint = "OpenEventA", SetLastError = true)]
	public static extern IntPtr OpenEvent(uint dwDesiredAccess, bool bInheritHandle, string lpName);

	// Token: 0x06002A95 RID: 10901
	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

	// Token: 0x06002A96 RID: 10902
	[DllImport("kernel32.dll", EntryPoint = "CreateEventA")]
	public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

	// Token: 0x06002A97 RID: 10903
	[DllImport("kernel32.dll", EntryPoint = "SetEnvironmentVariableA", SetLastError = true)]
	public static extern bool SetEnvironmentVariable(string lpName, string lpValue);

	// Token: 0x06002A98 RID: 10904
	[DllImport("kernel32.dll")]
	public static extern bool SetEvent(IntPtr hEvent);

	// Token: 0x06002A99 RID: 10905
	[DllImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(2)]
	public static extern bool CloseHandle(IntPtr hObject);

	// Token: 0x06002A9A RID: 10906
	[DllImport("kernel32.dll", CharSet = 2, SetLastError = true)]
	public static extern IntPtr CreateFileA(IntPtr filename, [MarshalAs(8)] FileAccess access, [MarshalAs(8)] FileShare share, IntPtr securityAttributes, [MarshalAs(8)] FileMode creationDisposition, [MarshalAs(8)] FileAttributes flagsAndAttributes, IntPtr templateFile);

	// Token: 0x06002A9B RID: 10907 RVA: 0x0014CF98 File Offset: 0x0014B198
	public static IntPtr GetPebAddress(IntPtr hProcess)
	{
		NtProcessBasicInfo ntProcessBasicInfo = default(NtProcessBasicInfo);
		int num = 0;
		if (Helper.NtQueryInformationProcess(hProcess, 0, ref ntProcessBasicInfo, Marshal.SizeOf(ntProcessBasicInfo.GetType()), out num) == 0)
		{
			return ntProcessBasicInfo.PebBaseAddress;
		}
		return IntPtr.Zero;
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x0014CFE0 File Offset: 0x0014B1E0
	public static byte[] ReadMemoryAtAdress(IntPtr handleProcess, IntPtr memoryAddress, uint bytesToRead, out int bytesRead)
	{
		byte[] array = new byte[bytesToRead];
		IntPtr intPtr;
		Helper.ReadProcessMemory(handleProcess, memoryAddress, array, bytesToRead, out intPtr);
		bytesRead = intPtr.ToInt32();
		return array;
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x0014D00C File Offset: 0x0014B20C
	public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
	{
		GCHandle gchandle = GCHandle.Alloc(bytes, 3);
		T result = (T)((object)Marshal.PtrToStructure(gchandle.AddrOfPinnedObject(), typeof(T)));
		gchandle.Free();
		return result;
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x0014D048 File Offset: 0x0014B248
	public static T ReadMemory<T>(this IntPtr atAddress)
	{
		return (T)((object)Marshal.PtrToStructure(atAddress, typeof(T)));
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x0014D06C File Offset: 0x0014B26C
	public static IntPtr ReadPointer<T>(T struc)
	{
		IntPtr zero = IntPtr.Zero;
		Marshal.StructureToPtr(struc, zero, true);
		return zero;
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x0001BB72 File Offset: 0x00019D72
	public static uint SonyAligneSection(uint value, uint alignment)
	{
		if (value % alignment > 0U)
		{
			return value + (alignment - value % alignment);
		}
		return value;
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x0001BB86 File Offset: 0x00019D86
	public static void CalculateOutput(int inputVal1, int inputVal2, ref int output)
	{
		output = inputVal1 + inputVal2 << inputVal1;
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x0014D090 File Offset: 0x0014B290
	public unsafe static IntPtr obtain_module_address_api_less(uint dllnumber)
	{
		if (dllnumber > 2U)
		{
			return IntPtr.Zero;
		}
		Process currentProcess = Process.GetCurrentProcess();
		IntPtr handle = currentProcess.Handle;
		IntPtr pebAddress = Helper.GetPebAddress(handle);
		IntPtr atAddress = (IntPtr)((void*)pebAddress.ReadMemory<SONY_PEB>().Ldr->InLoadOrderModuleList.Flink);
		SONY_LDR_MODULE sony_LDR_MODULE = atAddress.ReadMemory<SONY_LDR_MODULE>();
		string text = null;
		int[] array = new int[]
		{
			1286553357,
			1285832469,
			1285963521,
			1291337558,
			1286487884,
			1285963535,
			1288322912
		};
		if (dllnumber == 2U)
		{
			for (int i = 0; i < 4 * array.Length >> 2; i++)
			{
				array[i] ^= (1288322918 ^ i);
			}
			IntPtr intPtr = Helper.IntArrToIntPtr(array);
			text = Marshal.PtrToStringUni(intPtr);
		}
		int num = 0;
		while (sony_LDR_MODULE.BaseAddress != null)
		{
			if (text != null)
			{
				if (num > 1 && sony_LDR_MODULE.BaseDllName.Length > 0 && sony_LDR_MODULE.BaseDllName.buffer != IntPtr.Zero && string.Equals(text, sony_LDR_MODULE.BaseDllName.ToString()))
				{
					return (IntPtr)sony_LDR_MODULE.BaseAddress;
				}
			}
			else if ((long)num == (long)((ulong)dllnumber))
			{
				return (IntPtr)sony_LDR_MODULE.BaseAddress;
			}
			sony_LDR_MODULE = ((IntPtr)((void*)sony_LDR_MODULE.InLoadOrderModuleList.Flink)).ReadMemory<SONY_LDR_MODULE>();
			num++;
		}
		return (IntPtr)(-1);
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x0014D1FC File Offset: 0x0014B3FC
	public unsafe static IntPtr get_export_function_index(uint functionindex, IntPtr baseaddressofmodule)
	{
		SONY_IMAGE_DOS_HEADER sony_IMAGE_DOS_HEADER = baseaddressofmodule.ReadMemory<SONY_IMAGE_DOS_HEADER>();
		if (baseaddressofmodule == IntPtr.Zero || !sony_IMAGE_DOS_HEADER.isValid || sony_IMAGE_DOS_HEADER.e_lfanew == 0)
		{
			return (IntPtr)(-1);
		}
		SONY_IMAGE_NT_HEADERS32 sony_IMAGE_NT_HEADERS = default(SONY_IMAGE_NT_HEADERS32);
		IntPtr atAddress = (IntPtr)(baseaddressofmodule.ToInt32() + sony_IMAGE_DOS_HEADER.e_lfanew);
		sony_IMAGE_NT_HEADERS = atAddress.ReadMemory<SONY_IMAGE_NT_HEADERS32>();
		if (!sony_IMAGE_NT_HEADERS.isValid)
		{
			return (IntPtr)(-1);
		}
		SONY_IMAGE_DATA_DIRECTORY exportTable = sony_IMAGE_NT_HEADERS.OptionalHeader.ExportTable;
		if (exportTable.VirtualAddress == 0U || exportTable.Size <= 0U)
		{
			return (IntPtr)(-1);
		}
		IntPtr atAddress2 = (IntPtr)((long)((ulong)(baseaddressofmodule.ToInt32() + (int)exportTable.VirtualAddress)));
		Helper.SONY_IMAGE_EXPORT_DIRECTORY sony_IMAGE_EXPORT_DIRECTORY = atAddress2.ReadMemory<Helper.SONY_IMAGE_EXPORT_DIRECTORY>();
		if (functionindex > 0U && functionindex <= sony_IMAGE_EXPORT_DIRECTORY.NumberOfFunctions)
		{
			IntPtr ptr = (IntPtr)((long)((ulong)(baseaddressofmodule.ToInt32() + (int)sony_IMAGE_EXPORT_DIRECTORY.AddressOfFunctions)));
			uint* ptr2 = Helper.IntPtrToC_UintPtr(ptr);
			return (IntPtr)((long)((ulong)(baseaddressofmodule.ToInt32() + (int)ptr2[functionindex - 1U])));
		}
		return (IntPtr)(-1);
	}

	// Token: 0x06002AA4 RID: 10916 RVA: 0x0014D318 File Offset: 0x0014B518
	public static IntPtr get_export_function_by_name(IntPtr name, IntPtr baseaddressofmodule)
	{
		SONY_IMAGE_DOS_HEADER sony_IMAGE_DOS_HEADER = baseaddressofmodule.ReadMemory<SONY_IMAGE_DOS_HEADER>();
		if (baseaddressofmodule == IntPtr.Zero || !sony_IMAGE_DOS_HEADER.isValid || sony_IMAGE_DOS_HEADER.e_lfanew == 0)
		{
			return (IntPtr)(-1);
		}
		SONY_IMAGE_NT_HEADERS32 sony_IMAGE_NT_HEADERS = default(SONY_IMAGE_NT_HEADERS32);
		IntPtr atAddress = (IntPtr)(baseaddressofmodule.ToInt32() + sony_IMAGE_DOS_HEADER.e_lfanew);
		sony_IMAGE_NT_HEADERS = atAddress.ReadMemory<SONY_IMAGE_NT_HEADERS32>();
		if (!sony_IMAGE_NT_HEADERS.isValid)
		{
			return (IntPtr)(-1);
		}
		SONY_IMAGE_DATA_DIRECTORY exportTable = sony_IMAGE_NT_HEADERS.OptionalHeader.ExportTable;
		if (exportTable.VirtualAddress == 0U || exportTable.Size <= 0U)
		{
			return (IntPtr)(-1);
		}
		IntPtr atAddress2 = (IntPtr)((long)((ulong)(baseaddressofmodule.ToInt32() + (int)exportTable.VirtualAddress)));
		Helper.SONY_IMAGE_EXPORT_DIRECTORY sony_IMAGE_EXPORT_DIRECTORY = atAddress2.ReadMemory<Helper.SONY_IMAGE_EXPORT_DIRECTORY>();
		string text = Marshal.PtrToStringAnsi(name);
		uint num = (uint)baseaddressofmodule.ToInt32();
		int num2 = 0;
		while ((long)num2 < (long)((ulong)(sony_IMAGE_EXPORT_DIRECTORY.NumberOfNames - 1U)))
		{
			uint num3 = num + (uint)((ulong)sony_IMAGE_EXPORT_DIRECTORY.AddressOfNames + (ulong)((long)(num2 * 4)));
			uint num4 = ((IntPtr)((long)((ulong)num3))).ReadMemory<uint>();
			IntPtr intPtr = (IntPtr)((long)((ulong)(num + num4)));
			string text2 = Marshal.PtrToStringAnsi(intPtr);
			if (text2.Equals(text))
			{
				num3 = (uint)((ulong)sony_IMAGE_EXPORT_DIRECTORY.AddressOfNameOrdinals + (ulong)((long)(num2 * 2)));
				IntPtr atAddress3 = (IntPtr)((long)((ulong)(num + num3)));
				short num5 = atAddress3.ReadMemory<short>();
				num3 = (uint)((ulong)sony_IMAGE_EXPORT_DIRECTORY.AddressOfFunctions + (ulong)((long)(num5 * 4)));
				return (IntPtr)((long)((ulong)num + (ulong)((long)((IntPtr)((long)((ulong)(num + num3)))).ReadMemory<IntPtr>().ToInt32())));
			}
			num2++;
		}
		return (IntPtr)(-1);
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x0014D4BC File Offset: 0x0014B6BC
	public static int IntArrCmp(int[] arr1, int[] arr2)
	{
		if (arr1 == null || arr2 == null)
		{
			return 1;
		}
		int num = arr1.Length;
		int num2 = arr2.Length;
		if (num != num2)
		{
			return 2;
		}
		int num3 = 0;
		for (int i = 0; i < num; i++)
		{
			if (arr1[i] != arr2[i])
			{
				num3++;
			}
		}
		return num3;
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x0014D50C File Offset: 0x0014B70C
	public static int CharArrCmp(char[] arr1, char[] arr2)
	{
		if (arr1 == null || arr2 == null)
		{
			return 1;
		}
		int num = arr1.Length;
		int num2 = arr2.Length;
		if (num != num2)
		{
			return 2;
		}
		int num3 = 0;
		for (int i = 0; i < num; i++)
		{
			if (arr1[i] != arr2[i])
			{
				num3++;
			}
		}
		return num3;
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x0014D55C File Offset: 0x0014B75C
	public unsafe static ulong* CastToC_Ptr(ref ulong i)
	{
		return &i;
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x0014D56C File Offset: 0x0014B76C
	public unsafe static long* CastToC_Ptr(ref long i)
	{
		return &i;
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x0014D57C File Offset: 0x0014B77C
	public unsafe static int* CastToC_Ptr(ref int i)
	{
		return &i;
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x0014D58C File Offset: 0x0014B78C
	public unsafe static uint* CastToC_Ptr(ref uint i)
	{
		return &i;
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x0014D59C File Offset: 0x0014B79C
	public unsafe static byte* CastToC_Ptr(ref byte b)
	{
		return &b;
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x0014D5AC File Offset: 0x0014B7AC
	public unsafe static uint* IntPtrToC_UintPtr(IntPtr ptr)
	{
		return (uint*)((void*)ptr);
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x0014D5C4 File Offset: 0x0014B7C4
	public unsafe static int* IntPtrToC_IntPtr(IntPtr ptr)
	{
		return (int*)((void*)ptr);
	}

	// Token: 0x06002AAE RID: 10926 RVA: 0x0014D5DC File Offset: 0x0014B7DC
	public unsafe static byte* IntPtrToC_BytePtr(IntPtr ptr)
	{
		return (byte*)((void*)ptr);
	}

	// Token: 0x06002AAF RID: 10927 RVA: 0x0014D5F4 File Offset: 0x0014B7F4
	public unsafe static char* IntPtrToC_CharPr(IntPtr ptr)
	{
		return (char*)((void*)ptr);
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x0014D60C File Offset: 0x0014B80C
	public unsafe static void* ByteArrToVoidPtr(byte[] arr)
	{
		fixed (void* result = ref (arr != null && arr.Length != 0) ? ref arr[0] : ref *(byte*)null)
		{
			return result;
		}
	}

	// Token: 0x06002AB1 RID: 10929 RVA: 0x0014D638 File Offset: 0x0014B838
	public unsafe static byte[] VoidPtrToByteArr(void* ptr, int startIdx, int length)
	{
		byte[] array = new byte[length];
		Marshal.Copy((IntPtr)ptr, array, 0, length);
		return array;
	}

	// Token: 0x06002AB2 RID: 10930 RVA: 0x0014D65C File Offset: 0x0014B85C
	public unsafe static IntPtr IntArrToIntPtr(int[] arr)
	{
		fixed (int* ptr = ref (arr != null && arr.Length != 0) ? ref arr[0] : ref *null)
		{
			return new IntPtr((void*)ptr);
		}
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x0014D68C File Offset: 0x0014B88C
	public unsafe static uint* UintArrToUintCPtr(uint[] arr)
	{
		fixed (uint* result = ref (arr != null && arr.Length != 0) ? ref arr[0] : ref *null)
		{
			return result;
		}
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x0014D6B8 File Offset: 0x0014B8B8
	public unsafe static char[] CharPtrToCharArr(char* ptr, int startIdx, int length)
	{
		char[] array = new char[length];
		Marshal.Copy((IntPtr)((void*)ptr), array, 0, length);
		return array;
	}

	// Token: 0x06002AB5 RID: 10933 RVA: 0x0014D638 File Offset: 0x0014B838
	public unsafe static byte[] BytePtrToByteArr(byte* ptr, int startIdx, int length)
	{
		byte[] array = new byte[length];
		Marshal.Copy((IntPtr)((void*)ptr), array, 0, length);
		return array;
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x0014D6DC File Offset: 0x0014B8DC
	public unsafe static byte* ByteArrToBytePtr(byte[] arr)
	{
		fixed (byte* result = ref (arr != null && arr.Length != 0) ? ref arr[0] : ref *null)
		{
			return result;
		}
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x0014D708 File Offset: 0x0014B908
	public static string AggregateArray<T>(T[] arr, string splitter = "")
	{
		return Enumerable.Aggregate<T, string>(arr, string.Empty, (string s, T i) => s + i.ToString() + splitter);
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x0014D73C File Offset: 0x0014B93C
	public unsafe static string CharPtrToString(char* ptr, int startIdx, int length)
	{
		byte[] arr = Helper.BytePtrToByteArr((byte*)ptr, startIdx, length);
		return Helper.ByteArrayToString(arr);
	}

	// Token: 0x06002AB9 RID: 10937 RVA: 0x0014D73C File Offset: 0x0014B93C
	public unsafe static string BytePtrToString(byte* ptr, int startIdx, int length)
	{
		byte[] arr = Helper.BytePtrToByteArr(ptr, startIdx, length);
		return Helper.ByteArrayToString(arr);
	}

	// Token: 0x06002ABA RID: 10938 RVA: 0x0014D758 File Offset: 0x0014B958
	public static string ByteArrayToString(byte[] arr)
	{
		ASCIIEncoding asciiencoding = new ASCIIEncoding();
		return asciiencoding.GetString(arr);
	}

	// Token: 0x06002ABB RID: 10939 RVA: 0x0014D774 File Offset: 0x0014B974
	public static string CharArrayToString(char[] arr)
	{
		ASCIIEncoding asciiencoding = new ASCIIEncoding();
		byte[] bytes = asciiencoding.GetBytes(arr);
		return asciiencoding.GetString(bytes);
	}

	// Token: 0x06002ABC RID: 10940 RVA: 0x0014D798 File Offset: 0x0014B998
	public static byte[] StringToByteArray(string str)
	{
		ASCIIEncoding asciiencoding = new ASCIIEncoding();
		return asciiencoding.GetBytes(str);
	}

	// Token: 0x06002ABD RID: 10941 RVA: 0x0014D7B4 File Offset: 0x0014B9B4
	public static string DecimalToBase(long iDec, int numbase)
	{
		string text = string.Empty;
		long[] array = new long[32];
		int num = 32;
		while (iDec > 0L)
		{
			long num2 = iDec % (long)numbase;
			array[--num] = num2;
			iDec /= (long)numbase;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if ((long)array.GetValue(i) >= 10L)
			{
				text += Helper.cHexa[(int)array.GetValue(i) % 10];
			}
			else
			{
				text += array.GetValue(i);
			}
		}
		return text.TrimStart(new char[]
		{
			'0'
		});
	}

	// Token: 0x06002ABE RID: 10942 RVA: 0x0014D868 File Offset: 0x0014BA68
	public static int BaseToDecimal(string sBase, int numbase)
	{
		int num = 0;
		int num2 = 1;
		string text = string.Empty;
		if (numbase > 10)
		{
			for (int i = 0; i < Helper.cHexa.Length; i++)
			{
				text += Helper.cHexa.GetValue(i).ToString();
			}
		}
		int j = sBase.Length - 1;
		while (j >= 0)
		{
			string text2 = sBase.get_Chars(j).ToString();
			int num3;
			if (text2.IndexOfAny(Helper.cHexa) >= 0)
			{
				num3 = Helper.iHexaNumeric[text.IndexOf(sBase.get_Chars(j))];
			}
			else
			{
				num3 = (int)(sBase.get_Chars(j) - '0');
			}
			num += num3 * num2;
			j--;
			num2 *= numbase;
		}
		return num;
	}

	// Token: 0x06002ABF RID: 10943 RVA: 0x0001BB92 File Offset: 0x00019D92
	public static void Print(string s)
	{
		Debug.Log(s);
		Helper.OutputDebugString(s);
	}

	// Token: 0x06002AC0 RID: 10944 RVA: 0x0000264F File Offset: 0x0000084F
	public static void LogInfo(string s)
	{
	}

	// Token: 0x06002AC1 RID: 10945 RVA: 0x0000264F File Offset: 0x0000084F
	public static void LogError(string s)
	{
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x0014D930 File Offset: 0x0014BB30
	public static uint TimeNull()
	{
		double totalSeconds = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		return (uint)totalSeconds;
	}

	// Token: 0x040035CD RID: 13773
	public const long INVALID_HANDLE_VALUE = -1L;

	// Token: 0x040035CE RID: 13774
	public const uint INFINITE = 4294967295U;

	// Token: 0x040035CF RID: 13775
	public const int WAIT_OBJECT_0 = 0;

	// Token: 0x040035D0 RID: 13776
	private const int base10 = 10;

	// Token: 0x040035D1 RID: 13777
	private const int asciiDiff = 48;

	// Token: 0x040035D2 RID: 13778
	private static char[] cHexa = new char[]
	{
		'A',
		'B',
		'C',
		'D',
		'E',
		'F'
	};

	// Token: 0x040035D3 RID: 13779
	private static int[] iHexaNumeric = new int[]
	{
		10,
		11,
		12,
		13,
		14,
		15
	};

	// Token: 0x020006EF RID: 1775
	[Flags]
	public enum SyncObjectAccess : uint
	{
		// Token: 0x040035D5 RID: 13781
		DELETE = 65536U,
		// Token: 0x040035D6 RID: 13782
		READ_CONTROL = 131072U,
		// Token: 0x040035D7 RID: 13783
		WRITE_DAC = 262144U,
		// Token: 0x040035D8 RID: 13784
		WRITE_OWNER = 524288U,
		// Token: 0x040035D9 RID: 13785
		SYNCHRONIZE = 1048576U,
		// Token: 0x040035DA RID: 13786
		EVENT_ALL_ACCESS = 2031619U,
		// Token: 0x040035DB RID: 13787
		EVENT_MODIFY_STATE = 2U,
		// Token: 0x040035DC RID: 13788
		MUTEX_ALL_ACCESS = 2031617U,
		// Token: 0x040035DD RID: 13789
		MUTEX_MODIFY_STATE = 1U,
		// Token: 0x040035DE RID: 13790
		SEMAPHORE_ALL_ACCESS = 2031619U,
		// Token: 0x040035DF RID: 13791
		SEMAPHORE_MODIFY_STATE = 2U,
		// Token: 0x040035E0 RID: 13792
		TIMER_ALL_ACCESS = 2031619U,
		// Token: 0x040035E1 RID: 13793
		TIMER_MODIFY_STATE = 2U,
		// Token: 0x040035E2 RID: 13794
		TIMER_QUERY_STATE = 1U
	}

	// Token: 0x020006F0 RID: 1776
	public struct SECURITY_ATTRIBUTES
	{
		// Token: 0x040035E3 RID: 13795
		public int nLength;

		// Token: 0x040035E4 RID: 13796
		public IntPtr lpSecurityDescriptor;

		// Token: 0x040035E5 RID: 13797
		public int bInheritHandle;
	}

	// Token: 0x020006F1 RID: 1777
	private struct SONY_IMAGE_EXPORT_DIRECTORY
	{
		// Token: 0x040035E6 RID: 13798
		public uint Characteristics;

		// Token: 0x040035E7 RID: 13799
		public uint TimeDateStamp;

		// Token: 0x040035E8 RID: 13800
		public ushort MajorVersion;

		// Token: 0x040035E9 RID: 13801
		public ushort MinorVersion;

		// Token: 0x040035EA RID: 13802
		public uint Name;

		// Token: 0x040035EB RID: 13803
		public uint Base;

		// Token: 0x040035EC RID: 13804
		public uint NumberOfFunctions;

		// Token: 0x040035ED RID: 13805
		public uint NumberOfNames;

		// Token: 0x040035EE RID: 13806
		public uint AddressOfFunctions;

		// Token: 0x040035EF RID: 13807
		public uint AddressOfNames;

		// Token: 0x040035F0 RID: 13808
		public uint AddressOfNameOrdinals;
	}
}
