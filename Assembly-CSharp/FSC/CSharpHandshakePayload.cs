using System;
using System.Runtime.InteropServices;

namespace FSC
{
	// Token: 0x020006E7 RID: 1767
	[StructLayout(2)]
	public struct CSharpHandshakePayload
	{
		// Token: 0x06002A73 RID: 10867 RVA: 0x0014CAFC File Offset: 0x0014ACFC
		private CSharpHandshakePayload(uint _iD, uint _nonceClient)
		{
			this.id = _iD;
			this.responseChannel = null;
			this.nonceClient = _nonceClient;
			this.nonceServer = 0U;
			this.integrityValue = 0U;
			this.FSC_ENCRYPTION_BEGIN = 0;
			this.padding0 = 0;
			this.padding1 = 0;
			this.padding2 = 0;
			this.padding3 = 0;
			this.padding4 = 0;
			this.padding5 = 0;
			this.padding6 = 0;
			this.padding7 = 0;
			this.padding8 = 0;
			this.padding9 = 0;
			this.padding10 = 0;
			this.padding11 = 0;
			this.padding12 = 0;
			this.padding13 = 0;
			this.padding14 = 0;
			this.padding15 = 0;
			this.FSC_ENCRYPTION_END = 0;
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x0014CBAC File Offset: 0x0014ADAC
		private CSharpHandshakePayload(uint _id, uint _nonceClient, uint _nonceServer)
		{
			this.id = _id;
			this.responseChannel = null;
			this.nonceClient = _nonceClient;
			this.nonceServer = _nonceServer;
			this.integrityValue = 0U;
			this.FSC_ENCRYPTION_BEGIN = 0;
			this.padding0 = 0;
			this.padding1 = 0;
			this.padding2 = 0;
			this.padding3 = 0;
			this.padding4 = 0;
			this.padding5 = 0;
			this.padding6 = 0;
			this.padding7 = 0;
			this.padding8 = 0;
			this.padding9 = 0;
			this.padding10 = 0;
			this.padding11 = 0;
			this.padding12 = 0;
			this.padding13 = 0;
			this.padding14 = 0;
			this.padding15 = 0;
			this.FSC_ENCRYPTION_END = 0;
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x0001BAB1 File Offset: 0x00019CB1
		public static CSharpHandshakePayload Step1(uint id, Channel responseChannel, uint nonceClient)
		{
			return new CSharpHandshakePayload(id, nonceClient);
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x0001BABA File Offset: 0x00019CBA
		public static CSharpHandshakePayload Step2(uint id, uint nonceClient, uint nonceServer)
		{
			return new CSharpHandshakePayload(id, nonceClient, nonceServer);
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x0014CC5C File Offset: 0x0014AE5C
		public unsafe int Encrypt(byte* key, uint keySize)
		{
			byte* ptr = Helper.CastToC_Ptr(ref this.FSC_ENCRYPTION_END);
			byte* ptr2 = Helper.CastToC_Ptr(ref this.FSC_ENCRYPTION_BEGIN);
			uint dataSize = (uint)((long)(ptr - ptr2));
			uint* result = Helper.CastToC_Ptr(ref this.integrityValue);
			Endpoint.calculateIntegrityValue((void*)ptr2, dataSize, result);
			return FSCCSharp.CipherCore.Encrypt(key, keySize, (void*)ptr2, dataSize);
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x0014CCAC File Offset: 0x0014AEAC
		public unsafe int Decrypt(byte* key, uint keySize)
		{
			byte* ptr = Helper.CastToC_Ptr(ref this.FSC_ENCRYPTION_END);
			byte* ptr2 = Helper.CastToC_Ptr(ref this.FSC_ENCRYPTION_BEGIN);
			uint dataSize = (uint)((long)(ptr - ptr2));
			int result = FSCCSharp.CipherCore.Decrypt(key, keySize, (void*)ptr2, dataSize);
			uint num = this.integrityValue;
			uint num2 = 0U;
			uint* result2 = Helper.CastToC_Ptr(ref num2);
			this.integrityValue = 0U;
			Endpoint.calculateIntegrityValue((void*)ptr2, dataSize, result2);
			if (num != num2)
			{
				return 1;
			}
			return result;
		}

		// Token: 0x04003589 RID: 13705
		[FieldOffset(0)]
		public uint id;

		// Token: 0x0400358A RID: 13706
		[FieldOffset(4)]
		public unsafe void* responseChannel;

		// Token: 0x0400358B RID: 13707
		[FieldOffset(8)]
		public byte FSC_ENCRYPTION_BEGIN;

		// Token: 0x0400358C RID: 13708
		[FieldOffset(12)]
		public uint nonceClient;

		// Token: 0x0400358D RID: 13709
		[FieldOffset(16)]
		public uint nonceServer;

		// Token: 0x0400358E RID: 13710
		[FieldOffset(20)]
		public uint integrityValue;

		// Token: 0x0400358F RID: 13711
		[FieldOffset(24)]
		public byte padding0;

		// Token: 0x04003590 RID: 13712
		[FieldOffset(25)]
		public byte padding1;

		// Token: 0x04003591 RID: 13713
		[FieldOffset(26)]
		public byte padding2;

		// Token: 0x04003592 RID: 13714
		[FieldOffset(27)]
		public byte padding3;

		// Token: 0x04003593 RID: 13715
		[FieldOffset(28)]
		public byte padding4;

		// Token: 0x04003594 RID: 13716
		[FieldOffset(29)]
		public byte padding5;

		// Token: 0x04003595 RID: 13717
		[FieldOffset(30)]
		public byte padding6;

		// Token: 0x04003596 RID: 13718
		[FieldOffset(31)]
		public byte padding7;

		// Token: 0x04003597 RID: 13719
		[FieldOffset(32)]
		public byte padding8;

		// Token: 0x04003598 RID: 13720
		[FieldOffset(33)]
		public byte padding9;

		// Token: 0x04003599 RID: 13721
		[FieldOffset(34)]
		public byte padding10;

		// Token: 0x0400359A RID: 13722
		[FieldOffset(35)]
		public byte padding11;

		// Token: 0x0400359B RID: 13723
		[FieldOffset(36)]
		public byte padding12;

		// Token: 0x0400359C RID: 13724
		[FieldOffset(37)]
		public byte padding13;

		// Token: 0x0400359D RID: 13725
		[FieldOffset(38)]
		public byte padding14;

		// Token: 0x0400359E RID: 13726
		[FieldOffset(39)]
		public byte padding15;

		// Token: 0x0400359F RID: 13727
		[FieldOffset(40)]
		public byte FSC_ENCRYPTION_END;
	}
}
