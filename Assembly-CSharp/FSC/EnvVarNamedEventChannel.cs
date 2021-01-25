using System;
using System.Diagnostics;

namespace FSC
{
	// Token: 0x020006E5 RID: 1765
	public class EnvVarNamedEventChannel : Channel
	{
		// Token: 0x06002A70 RID: 10864 RVA: 0x0014C8CC File Offset: 0x0014AACC
		public unsafe override void Send(Message message)
		{
			string lpName = string.Empty;
			string lpName2 = string.Empty;
			uint[] array = new uint[]
			{
				1678462017U,
				1566196736U
			};
			uint[] array2 = new uint[]
			{
				1511334933U,
				1528314880U
			};
			Process currentProcess = Process.GetCurrentProcess();
			int id = currentProcess.Id;
			array[0] ^= 187578912U;
			array2[0] ^= 892360039U;
			uint* ptr = Helper.UintArrToUintCPtr(array);
			char* ptr2 = (char*)ptr;
			string text = Helper.CharPtrToString(ptr2, 0, 8);
			string text2 = text.Substring(0, 2);
			string text3 = text.Substring(2, 2);
			if (text3.Equals("%o"))
			{
				long iDec = (long)(id ^ 950435991);
				string text4 = Helper.DecimalToBase(iDec, 8);
				lpName = text2 + text4;
			}
			uint* ptr3 = Helper.UintArrToUintCPtr(array2);
			char* ptr4 = (char*)ptr3;
			string text5 = Helper.CharPtrToString(ptr4, 0, 8);
			string text6 = text5.Substring(0, 2);
			string text7 = text5.Substring(2, 2);
			if (text7.Equals("%o"))
			{
				long iDec2 = (long)id ^ (long)((ulong)-148551293);
				string text8 = Helper.DecimalToBase(iDec2, 8);
				lpName2 = text6 + text8;
			}
			IntPtr intPtr = Helper.OpenEvent(1048578U, false, lpName);
			if (intPtr == (IntPtr)(-1L) || intPtr == IntPtr.Zero)
			{
				return;
			}
			uint num = Helper.WaitForSingleObject(intPtr, uint.MaxValue);
			if (num != 0U)
			{
				return;
			}
			IntPtr intPtr2 = Helper.CreateEvent(IntPtr.Zero, false, false, null);
			if (intPtr2 == (IntPtr)(-1L) || intPtr2 == IntPtr.Zero)
			{
				return;
			}
			message.reserved[0] = (uint)intPtr2.ToInt32();
			message.MessageToMessageData();
			MessageData* msgData = message.MsgData;
			Helper.SetEnvironmentVariable(lpName2, msgData.ToString());
			IntPtr intPtr3 = Helper.OpenEvent(1048578U, false, lpName2);
			if (intPtr3 == (IntPtr)(-1L) || intPtr == IntPtr.Zero)
			{
				return;
			}
			Helper.SetEvent(intPtr3);
			num = Helper.WaitForSingleObject(intPtr2, uint.MaxValue);
			if (num != 0U)
			{
				return;
			}
			Helper.CloseHandle(intPtr2);
		}
	}
}
