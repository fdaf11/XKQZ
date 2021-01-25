using System;
using System.Runtime.InteropServices;

namespace FSC
{
	// Token: 0x020006D9 RID: 1753
	[StructLayout(2)]
	public struct MessageData
	{
		// Token: 0x06002A38 RID: 10808 RVA: 0x0001B919 File Offset: 0x00019B19
		public unsafe void Init(int _type, void* _payload, int _payloadSize)
		{
			this.type = _type;
			this.attributes = 0;
			this.payload = _payload;
			this.payloadSize = _payloadSize;
			this.sessionId = 0U;
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x0001B93E File Offset: 0x00019B3E
		public unsafe void Init(int _type, int _attributes, void* _payload, int _payloadSize)
		{
			this.type = _type;
			this.attributes = _attributes;
			this.payload = _payload;
			this.payloadSize = _payloadSize;
			this.sessionId = 0U;
		}

		// Token: 0x04003569 RID: 13673
		[FieldOffset(0)]
		public uint reserved0;

		// Token: 0x0400356A RID: 13674
		[FieldOffset(4)]
		public uint reserved1;

		// Token: 0x0400356B RID: 13675
		[FieldOffset(8)]
		public uint reserved2;

		// Token: 0x0400356C RID: 13676
		[FieldOffset(12)]
		public uint reserved3;

		// Token: 0x0400356D RID: 13677
		[FieldOffset(16)]
		public uint reserved4;

		// Token: 0x0400356E RID: 13678
		[FieldOffset(20)]
		public uint reserved5;

		// Token: 0x0400356F RID: 13679
		[FieldOffset(24)]
		public uint reserved6;

		// Token: 0x04003570 RID: 13680
		[FieldOffset(28)]
		public uint reserved7;

		// Token: 0x04003571 RID: 13681
		[FieldOffset(32)]
		public int type;

		// Token: 0x04003572 RID: 13682
		[FieldOffset(36)]
		public int attributes;

		// Token: 0x04003573 RID: 13683
		[FieldOffset(40)]
		public unsafe void* payload;

		// Token: 0x04003574 RID: 13684
		[FieldOffset(44)]
		public int payloadSize;

		// Token: 0x04003575 RID: 13685
		[FieldOffset(48)]
		public uint payloadIntegrityValue;

		// Token: 0x04003576 RID: 13686
		[FieldOffset(52)]
		public uint sessionId;
	}
}
