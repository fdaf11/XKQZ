using System;
using System.Runtime.InteropServices;
using FSC;
using Sony;

// Token: 0x0200070C RID: 1804
public static class dss_api
{
	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x0001BC5E File Offset: 0x00019E5E
	// (set) Token: 0x06002AD6 RID: 10966 RVA: 0x0001BC65 File Offset: 0x00019E65
	public static dss_api.CrackDetectedDelegate CrackDetected
	{
		get
		{
			return dss_api._crackDetected;
		}
		set
		{
			dss_api._crackDetected = value;
		}
	}

	// Token: 0x06002AD7 RID: 10967 RVA: 0x0001BC6D File Offset: 0x00019E6D
	private static void CrackDetectedCallback(dss_api.FailReason reason, int iD)
	{
		Helper.LogInfo(string.Concat(new object[]
		{
			"Crack detected! Reason: ",
			reason,
			" ID: ",
			iD
		}));
	}

	// Token: 0x06002AD8 RID: 10968 RVA: 0x0000264F File Offset: 0x0000084F
	public static void Init()
	{
	}

	// Token: 0x06002AD9 RID: 10969 RVA: 0x0001BCA1 File Offset: 0x00019EA1
	public static void AssetsFailed()
	{
		dss_api._crackDetected(dss_api.FailReason.ASSETS_FAILED, dss_api._iID);
		dss_api._iID++;
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x0014DDC0 File Offset: 0x0014BFC0
	public unsafe static void LicenceCheck()
	{
		Helper.LogInfo("Start LicenceCheck");
		Endpoint endpoint = new Endpoint(4, 1U, null, null);
		endpoint.SetTxChannel(FSCCSharp.SMS_WRAPPER_CHANNEL_1);
		int inputVal = 5;
		int inputVal2 = 20;
		int payloadSize = sizeof(CSharpPayload);
		CSharpPayload csharpPayload = default(CSharpPayload);
		csharpPayload.Init(inputVal, inputVal2);
		Message message = new Message(3, (void*)(&csharpPayload), payloadSize);
		endpoint.Send(message);
		int num = endpoint.Receive(message);
		if (num != 0)
		{
			dss_api._crackDetected(dss_api.FailReason.LICENCE_FAILED, dss_api._iID);
			return;
		}
		CSharpPayload* payload = (CSharpPayload*)message.payload;
		int num2 = 0;
		Helper.CalculateOutput(inputVal, inputVal2, ref num2);
		if (num2 != payload->output)
		{
			dss_api._crackDetected(dss_api.FailReason.LICENCE_FAILED, dss_api._iID);
			return;
		}
		Helper.LogInfo("End LicenceCheck");
		dss_api._iID++;
	}

	// Token: 0x06002ADB RID: 10971 RVA: 0x0014DE8C File Offset: 0x0014C08C
	public static void DataDirectoryCheck()
	{
		uint num = 0U;
		uint num2 = 5U;
		SonySecurityApi.DataDirectoryCheck(dss_api._iID, ref num, num2);
		if (num == num2)
		{
			dss_api._crackDetected(dss_api.FailReason.DATA_DIRECTORY_FAILED, dss_api._iID);
		}
		dss_api._iID++;
	}

	// Token: 0x06002ADC RID: 10972 RVA: 0x0014DED0 File Offset: 0x0014C0D0
	public static void CompareImageSizeCheck()
	{
		uint num = 0U;
		uint num2 = 5U;
		SonySecurityApi.CompareImageSizeCheck(dss_api._iID, ref num, num2);
		if (num == num2)
		{
			dss_api._crackDetected(dss_api.FailReason.COMPARE_IMAGESIZE_FAILED, dss_api._iID);
		}
		dss_api._iID++;
	}

	// Token: 0x06002ADD RID: 10973 RVA: 0x0014DF14 File Offset: 0x0014C114
	public static void HiddenSectionCheck()
	{
		uint num = 0U;
		uint num2 = 5U;
		SonySecurityApi.HiddenSectionCheck(dss_api._iID, ref num, num2);
		if (num == num2)
		{
			dss_api._crackDetected(dss_api.FailReason.HIDDEN_SECTION_FAILED, dss_api._iID);
		}
		dss_api._iID++;
	}

	// Token: 0x0400370C RID: 14092
	private static int _iID = 0;

	// Token: 0x0400370D RID: 14093
	public static dss_api.CrackDetectedDelegate _crackDetected = new dss_api.CrackDetectedDelegate(dss_api.CrackDetectedCallback);

	// Token: 0x0200070D RID: 1805
	public enum FailReason
	{
		// Token: 0x0400370F RID: 14095
		ASSETS_FAILED,
		// Token: 0x04003710 RID: 14096
		LICENCE_FAILED,
		// Token: 0x04003711 RID: 14097
		DATA_DIRECTORY_FAILED,
		// Token: 0x04003712 RID: 14098
		COMPARE_IMAGESIZE_FAILED,
		// Token: 0x04003713 RID: 14099
		HIDDEN_SECTION_FAILED
	}

	// Token: 0x0200070E RID: 1806
	[StructLayout(2)]
	public struct Unmanaged_Small_PEB
	{
		// Token: 0x04003714 RID: 14100
		[FieldOffset(0)]
		public byte Reserved1_0;

		// Token: 0x04003715 RID: 14101
		[FieldOffset(1)]
		public byte Reserved1_1;

		// Token: 0x04003716 RID: 14102
		[FieldOffset(2)]
		public byte BeingDebugged;

		// Token: 0x04003717 RID: 14103
		[FieldOffset(3)]
		public byte Reserved2;

		// Token: 0x04003718 RID: 14104
		[FieldOffset(4)]
		public IntPtr Reserved3_0;

		// Token: 0x04003719 RID: 14105
		[FieldOffset(8)]
		public IntPtr Reserved3_1;
	}

	// Token: 0x0200070F RID: 1807
	// (Invoke) Token: 0x06002ADF RID: 10975
	public delegate void CrackDetectedDelegate(dss_api.FailReason reason, int iD);
}
