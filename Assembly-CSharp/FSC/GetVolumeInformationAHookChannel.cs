using System;

namespace FSC
{
	// Token: 0x020006E4 RID: 1764
	public class GetVolumeInformationAHookChannel : Channel
	{
		// Token: 0x06002A6E RID: 10862 RVA: 0x0014C844 File Offset: 0x0014AA44
		public unsafe override void Send(Message message)
		{
			message.MessageToMessageData();
			uint* ptr = Helper.CastToC_Ptr(ref message.MsgData->reserved0);
			IntPtr intPtr = (IntPtr)((void*)ptr);
			uint num = (uint)intPtr.ToInt32();
			uint num2 = (uint)((IntPtr)(intPtr.ToInt32() + 1)).ToInt32();
			uint num3 = (uint)((IntPtr)(intPtr.ToInt32() + 2)).ToInt32();
			Helper.GetVolumeInformation(null, null, 0, (IntPtr)((long)((ulong)num)), (IntPtr)((long)((ulong)num2)), (IntPtr)((long)((ulong)num3)), null, 0);
		}
	}
}
