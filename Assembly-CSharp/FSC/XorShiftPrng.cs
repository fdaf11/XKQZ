using System;

namespace FSC
{
	// Token: 0x020006EB RID: 1771
	public class XorShiftPrng : Prng
	{
		// Token: 0x06002A80 RID: 10880 RVA: 0x0014CE0C File Offset: 0x0014B00C
		public override void srand(uint seed)
		{
			XorShiftPrng.x += seed;
			XorShiftPrng.y -= seed;
			XorShiftPrng.z *= seed / 3U;
			XorShiftPrng.w -= seed * 3U;
			XorShiftPrng.v += seed;
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x0014CE5C File Offset: 0x0014B05C
		public override uint rand()
		{
			uint num = XorShiftPrng.x ^ XorShiftPrng.x >> 7;
			XorShiftPrng.x = XorShiftPrng.y;
			XorShiftPrng.y = XorShiftPrng.z;
			XorShiftPrng.z = XorShiftPrng.w;
			XorShiftPrng.w = XorShiftPrng.v;
			XorShiftPrng.v = (XorShiftPrng.v ^ XorShiftPrng.v << 6 ^ (num ^ num << 13));
			return (XorShiftPrng.y + XorShiftPrng.y + 1U) * XorShiftPrng.v;
		}

		// Token: 0x040035C8 RID: 13768
		private static uint x = 123456789U;

		// Token: 0x040035C9 RID: 13769
		private static uint y = 362436069U;

		// Token: 0x040035CA RID: 13770
		private static uint z = 521288629U;

		// Token: 0x040035CB RID: 13771
		private static uint w = 88675123U;

		// Token: 0x040035CC RID: 13772
		private static uint v = 886756453U;
	}
}
