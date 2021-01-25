using System;
using System.Runtime.InteropServices;

namespace FSC
{
	// Token: 0x020006E8 RID: 1768
	[StructLayout(2)]
	public struct CSharpPayload
	{
		// Token: 0x06002A79 RID: 10873 RVA: 0x0014CD18 File Offset: 0x0014AF18
		public void Init(int _inputVal1, int _inputVal2)
		{
			this.inputVal1 = _inputVal1;
			this.inputVal2 = _inputVal2;
			this.output = 0;
			this.FSC_ENCRYPTION_BEGIN = 0;
			this.FSC_ENCRYPTION_END = 0;
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
		}

		// Token: 0x040035A0 RID: 13728
		[FieldOffset(0)]
		public byte FSC_ENCRYPTION_BEGIN;

		// Token: 0x040035A1 RID: 13729
		[FieldOffset(4)]
		public int inputVal1;

		// Token: 0x040035A2 RID: 13730
		[FieldOffset(8)]
		public int inputVal2;

		// Token: 0x040035A3 RID: 13731
		[FieldOffset(12)]
		public int output;

		// Token: 0x040035A4 RID: 13732
		[FieldOffset(16)]
		public byte padding0;

		// Token: 0x040035A5 RID: 13733
		[FieldOffset(17)]
		public byte padding1;

		// Token: 0x040035A6 RID: 13734
		[FieldOffset(18)]
		public byte padding2;

		// Token: 0x040035A7 RID: 13735
		[FieldOffset(19)]
		public byte padding3;

		// Token: 0x040035A8 RID: 13736
		[FieldOffset(20)]
		public byte padding4;

		// Token: 0x040035A9 RID: 13737
		[FieldOffset(21)]
		public byte padding5;

		// Token: 0x040035AA RID: 13738
		[FieldOffset(22)]
		public byte padding6;

		// Token: 0x040035AB RID: 13739
		[FieldOffset(23)]
		public byte padding7;

		// Token: 0x040035AC RID: 13740
		[FieldOffset(24)]
		public byte padding8;

		// Token: 0x040035AD RID: 13741
		[FieldOffset(25)]
		public byte padding9;

		// Token: 0x040035AE RID: 13742
		[FieldOffset(26)]
		public byte padding10;

		// Token: 0x040035AF RID: 13743
		[FieldOffset(27)]
		public byte padding11;

		// Token: 0x040035B0 RID: 13744
		[FieldOffset(28)]
		public byte padding12;

		// Token: 0x040035B1 RID: 13745
		[FieldOffset(29)]
		public byte padding13;

		// Token: 0x040035B2 RID: 13746
		[FieldOffset(30)]
		public byte padding14;

		// Token: 0x040035B3 RID: 13747
		[FieldOffset(31)]
		public byte padding15;

		// Token: 0x040035B4 RID: 13748
		[FieldOffset(32)]
		public byte FSC_ENCRYPTION_END;
	}
}
