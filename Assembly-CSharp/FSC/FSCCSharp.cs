using System;

namespace FSC
{
	// Token: 0x020006D7 RID: 1751
	public class FSCCSharp
	{
		// Token: 0x06002A31 RID: 10801 RVA: 0x0001B909 File Offset: 0x00019B09
		private uint FSC_DIFF(uint A, uint B)
		{
			if (A > B)
			{
				return A - B;
			}
			return B - A;
		}

		// Token: 0x0400354D RID: 13645
		public const int FSC_SENDMESSAGE_HWND = 0;

		// Token: 0x0400354E RID: 13646
		public const int FSC_SENDMESSAGE_MSG = 780;

		// Token: 0x0400354F RID: 13647
		public const int FSC_SENDMESSAGE_WPARAM = 3;

		// Token: 0x04003550 RID: 13648
		public const int FSC_MAX_KEY_SIZE = 32;

		// Token: 0x04003551 RID: 13649
		public const int FSC_MAX_BLOCK_SIZE = 16;

		// Token: 0x04003552 RID: 13650
		public static readonly Prng PrngCore = new XorShiftPrng();

		// Token: 0x04003553 RID: 13651
		public static readonly Cipher CipherCore = new XorCipher();

		// Token: 0x04003554 RID: 13652
		public static readonly Channel SMS_WRAPPER_CHANNEL_1 = new GetVolumeInformationAHookChannel();

		// Token: 0x04003555 RID: 13653
		public static readonly Channel SMS_WRAPPER_CHANNEL_2 = new EnvVarNamedEventChannel();

		// Token: 0x04003556 RID: 13654
		public static readonly Endpoint.FunctionBindingNumberGenerator FSC_BINDINGNUMBERGENERATOR_CLIENTSERVER_1 = new Endpoint.FunctionBindingNumberGenerator(EncodeSystemPointerBinding.Generate);
	}
}
