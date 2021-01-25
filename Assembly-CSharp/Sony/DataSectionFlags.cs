using System;

namespace Sony
{
	// Token: 0x020006F5 RID: 1781
	[Flags]
	public enum DataSectionFlags : uint
	{
		// Token: 0x04003618 RID: 13848
		TypeReg = 0U,
		// Token: 0x04003619 RID: 13849
		TypeDsect = 1U,
		// Token: 0x0400361A RID: 13850
		TypeNoLoad = 2U,
		// Token: 0x0400361B RID: 13851
		TypeGroup = 4U,
		// Token: 0x0400361C RID: 13852
		TypeNoPadded = 8U,
		// Token: 0x0400361D RID: 13853
		TypeCopy = 16U,
		// Token: 0x0400361E RID: 13854
		ContentCode = 32U,
		// Token: 0x0400361F RID: 13855
		ContentInitializedData = 64U,
		// Token: 0x04003620 RID: 13856
		ContentUninitializedData = 128U,
		// Token: 0x04003621 RID: 13857
		LinkOther = 256U,
		// Token: 0x04003622 RID: 13858
		LinkInfo = 512U,
		// Token: 0x04003623 RID: 13859
		TypeOver = 1024U,
		// Token: 0x04003624 RID: 13860
		LinkRemove = 2048U,
		// Token: 0x04003625 RID: 13861
		LinkComDat = 4096U,
		// Token: 0x04003626 RID: 13862
		NoDeferSpecExceptions = 16384U,
		// Token: 0x04003627 RID: 13863
		RelativeGP = 32768U,
		// Token: 0x04003628 RID: 13864
		MemPurgeable = 131072U,
		// Token: 0x04003629 RID: 13865
		Memory16Bit = 131072U,
		// Token: 0x0400362A RID: 13866
		MemoryLocked = 262144U,
		// Token: 0x0400362B RID: 13867
		MemoryPreload = 524288U,
		// Token: 0x0400362C RID: 13868
		Align1Bytes = 1048576U,
		// Token: 0x0400362D RID: 13869
		Align2Bytes = 2097152U,
		// Token: 0x0400362E RID: 13870
		Align4Bytes = 3145728U,
		// Token: 0x0400362F RID: 13871
		Align8Bytes = 4194304U,
		// Token: 0x04003630 RID: 13872
		Align16Bytes = 5242880U,
		// Token: 0x04003631 RID: 13873
		Align32Bytes = 6291456U,
		// Token: 0x04003632 RID: 13874
		Align64Bytes = 7340032U,
		// Token: 0x04003633 RID: 13875
		Align128Bytes = 8388608U,
		// Token: 0x04003634 RID: 13876
		Align256Bytes = 9437184U,
		// Token: 0x04003635 RID: 13877
		Align512Bytes = 10485760U,
		// Token: 0x04003636 RID: 13878
		Align1024Bytes = 11534336U,
		// Token: 0x04003637 RID: 13879
		Align2048Bytes = 12582912U,
		// Token: 0x04003638 RID: 13880
		Align4096Bytes = 13631488U,
		// Token: 0x04003639 RID: 13881
		Align8192Bytes = 14680064U,
		// Token: 0x0400363A RID: 13882
		LinkExtendedRelocationOverflow = 16777216U,
		// Token: 0x0400363B RID: 13883
		MemoryDiscardable = 33554432U,
		// Token: 0x0400363C RID: 13884
		MemoryNotCached = 67108864U,
		// Token: 0x0400363D RID: 13885
		MemoryNotPaged = 134217728U,
		// Token: 0x0400363E RID: 13886
		MemoryShared = 268435456U,
		// Token: 0x0400363F RID: 13887
		MemoryExecute = 536870912U,
		// Token: 0x04003640 RID: 13888
		MemoryRead = 1073741824U,
		// Token: 0x04003641 RID: 13889
		MemoryWrite = 2147483648U
	}
}
