using System;

namespace FSC
{
	// Token: 0x020006EA RID: 1770
	public abstract class Prng
	{
		// Token: 0x06002A7B RID: 10875
		public abstract void srand(uint seed);

		// Token: 0x06002A7C RID: 10876
		public abstract uint rand();

		// Token: 0x06002A7D RID: 10877 RVA: 0x0014CDB8 File Offset: 0x0014AFB8
		public unsafe uint rand(byte* destination, uint size)
		{
			if (destination == null || size == 0U)
			{
				return uint.MaxValue;
			}
			uint num = 0U;
			uint num2 = 0U;
			while (num < size)
			{
				num2 %= 4U;
				uint num3;
				if (num % 4U == 0U)
				{
					num3 = this.rand();
				}
				destination[num] = *(ref num3 + (UIntPtr)(3U - num2));
				num += 1U;
				num2 += 1U;
			}
			return 1U;
		}
	}
}
