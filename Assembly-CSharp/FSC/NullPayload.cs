using System;
using System.Runtime.InteropServices;

namespace FSC
{
	// Token: 0x020006E9 RID: 1769
	[StructLayout(2)]
	public struct NullPayload
	{
		// Token: 0x040035B5 RID: 13749
		[FieldOffset(0)]
		public byte FSC_ENCRYPTION_BEGIN;

		// Token: 0x040035B6 RID: 13750
		[FieldOffset(4)]
		public uint nullVal;

		// Token: 0x040035B7 RID: 13751
		[FieldOffset(8)]
		public byte padding0;

		// Token: 0x040035B8 RID: 13752
		[FieldOffset(9)]
		public byte padding1;

		// Token: 0x040035B9 RID: 13753
		[FieldOffset(10)]
		public byte padding2;

		// Token: 0x040035BA RID: 13754
		[FieldOffset(11)]
		public byte padding3;

		// Token: 0x040035BB RID: 13755
		[FieldOffset(12)]
		public byte padding4;

		// Token: 0x040035BC RID: 13756
		[FieldOffset(13)]
		public byte padding5;

		// Token: 0x040035BD RID: 13757
		[FieldOffset(14)]
		public byte padding6;

		// Token: 0x040035BE RID: 13758
		[FieldOffset(15)]
		public byte padding7;

		// Token: 0x040035BF RID: 13759
		[FieldOffset(16)]
		public byte padding8;

		// Token: 0x040035C0 RID: 13760
		[FieldOffset(17)]
		public byte padding9;

		// Token: 0x040035C1 RID: 13761
		[FieldOffset(18)]
		public byte padding10;

		// Token: 0x040035C2 RID: 13762
		[FieldOffset(19)]
		public byte padding11;

		// Token: 0x040035C3 RID: 13763
		[FieldOffset(20)]
		public byte padding12;

		// Token: 0x040035C4 RID: 13764
		[FieldOffset(21)]
		public byte padding13;

		// Token: 0x040035C5 RID: 13765
		[FieldOffset(22)]
		public byte padding14;

		// Token: 0x040035C6 RID: 13766
		[FieldOffset(23)]
		public byte padding15;

		// Token: 0x040035C7 RID: 13767
		[FieldOffset(24)]
		public byte FSC_ENCRYPTION_END;
	}
}
