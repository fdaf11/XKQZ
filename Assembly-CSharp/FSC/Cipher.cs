using System;

namespace FSC
{
	// Token: 0x020006EC RID: 1772
	public abstract class Cipher
	{
		// Token: 0x06002A82 RID: 10882 RVA: 0x00002672 File Offset: 0x00000872
		public Cipher(int blockSize, int keySize)
		{
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x0001BB00 File Offset: 0x00019D00
		public unsafe virtual int SetUpKey(byte* key, uint keySize)
		{
			if (key == null || keySize == 0U)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x0001BB11 File Offset: 0x00019D11
		public unsafe virtual int Encrypt(byte* key, uint keySize, void* address, uint dataSize)
		{
			if (key == null || keySize == 0U || address == null || dataSize == 0U)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x0001BB11 File Offset: 0x00019D11
		public unsafe virtual int Decrypt(byte* key, uint keySize, void* address, uint dataSize)
		{
			if (key == null || keySize == 0U || address == null || dataSize == 0U)
			{
				return -1;
			}
			return 0;
		}
	}
}
