using System;

namespace FSC
{
	// Token: 0x020006ED RID: 1773
	public class XorCipher : Cipher
	{
		// Token: 0x06002A86 RID: 10886 RVA: 0x0001BB2F File Offset: 0x00019D2F
		public XorCipher() : base(1, 32)
		{
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x0001BB3A File Offset: 0x00019D3A
		public unsafe override int SetUpKey(byte* key, uint keySize)
		{
			return base.SetUpKey(key, keySize);
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x0014CECC File Offset: 0x0014B0CC
		public unsafe override int Encrypt(byte* key, uint keySize, void* address, uint dataSize)
		{
			int num = base.Encrypt(key, keySize, address, dataSize);
			if (num != 0)
			{
				return num;
			}
			byte b = 0;
			for (uint num2 = 0U; num2 < dataSize; num2 += 1U)
			{
				byte b2 = (byte)num2;
				((byte*)address)[num2] = (((byte*)address)[num2] + b2 - b ^ key[num2 % keySize]);
				b = ((byte*)address)[num2];
			}
			return 0;
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x0014CF30 File Offset: 0x0014B130
		public unsafe override int Decrypt(byte* key, uint keySize, void* address, uint dataSize)
		{
			int num = base.Decrypt(key, keySize, address, dataSize);
			if (num != 0)
			{
				return num;
			}
			byte b = 0;
			for (uint num2 = 0U; num2 < dataSize; num2 += 1U)
			{
				byte b2 = (byte)num2;
				int num3 = (int)((byte*)address)[num2];
				((byte*)address)[num2] = (((byte*)address)[num2] ^ key[num2 % keySize]) - b2 + b;
				b = (byte)num3;
			}
			return 0;
		}
	}
}
