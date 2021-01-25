using System;
using System.Runtime.InteropServices;

namespace Sony
{
	// Token: 0x020006FD RID: 1789
	[StructLayout(2)]
	public struct SONY_IMAGE_OPTIONAL_HEADER32
	{
		// Token: 0x04003680 RID: 13952
		[FieldOffset(0)]
		public MagicType Magic;

		// Token: 0x04003681 RID: 13953
		[FieldOffset(2)]
		public byte MajorLinkerVersion;

		// Token: 0x04003682 RID: 13954
		[FieldOffset(3)]
		public byte MinorLinkerVersion;

		// Token: 0x04003683 RID: 13955
		[FieldOffset(4)]
		public uint SizeOfCode;

		// Token: 0x04003684 RID: 13956
		[FieldOffset(8)]
		public uint SizeOfInitializedData;

		// Token: 0x04003685 RID: 13957
		[FieldOffset(12)]
		public uint SizeOfUninitializedData;

		// Token: 0x04003686 RID: 13958
		[FieldOffset(16)]
		public uint AddressOfEntryPoint;

		// Token: 0x04003687 RID: 13959
		[FieldOffset(20)]
		public uint BaseOfCode;

		// Token: 0x04003688 RID: 13960
		[FieldOffset(24)]
		public uint BaseOfData;

		// Token: 0x04003689 RID: 13961
		[FieldOffset(28)]
		public uint ImageBase;

		// Token: 0x0400368A RID: 13962
		[FieldOffset(32)]
		public uint SectionAlignment;

		// Token: 0x0400368B RID: 13963
		[FieldOffset(36)]
		public uint FileAlignment;

		// Token: 0x0400368C RID: 13964
		[FieldOffset(40)]
		public ushort MajorOperatingSystemVersion;

		// Token: 0x0400368D RID: 13965
		[FieldOffset(42)]
		public ushort MinorOperatingSystemVersion;

		// Token: 0x0400368E RID: 13966
		[FieldOffset(44)]
		public ushort MajorImageVersion;

		// Token: 0x0400368F RID: 13967
		[FieldOffset(46)]
		public ushort MinorImageVersion;

		// Token: 0x04003690 RID: 13968
		[FieldOffset(48)]
		public ushort MajorSubsystemVersion;

		// Token: 0x04003691 RID: 13969
		[FieldOffset(50)]
		public ushort MinorSubsystemVersion;

		// Token: 0x04003692 RID: 13970
		[FieldOffset(52)]
		public uint Win32VersionValue;

		// Token: 0x04003693 RID: 13971
		[FieldOffset(56)]
		public uint SizeOfImage;

		// Token: 0x04003694 RID: 13972
		[FieldOffset(60)]
		public uint SizeOfHeaders;

		// Token: 0x04003695 RID: 13973
		[FieldOffset(64)]
		public uint CheckSum;

		// Token: 0x04003696 RID: 13974
		[FieldOffset(68)]
		public SubSystemType Subsystem;

		// Token: 0x04003697 RID: 13975
		[FieldOffset(70)]
		public DllCharacteristicsType DllCharacteristics;

		// Token: 0x04003698 RID: 13976
		[FieldOffset(72)]
		public uint SizeOfStackReserve;

		// Token: 0x04003699 RID: 13977
		[FieldOffset(76)]
		public uint SizeOfStackCommit;

		// Token: 0x0400369A RID: 13978
		[FieldOffset(80)]
		public uint SizeOfHeapReserve;

		// Token: 0x0400369B RID: 13979
		[FieldOffset(84)]
		public uint SizeOfHeapCommit;

		// Token: 0x0400369C RID: 13980
		[FieldOffset(88)]
		public uint LoaderFlags;

		// Token: 0x0400369D RID: 13981
		[FieldOffset(92)]
		public uint NumberOfRvaAndSizes;

		// Token: 0x0400369E RID: 13982
		[FieldOffset(96)]
		public SONY_IMAGE_DATA_DIRECTORY ExportTable;

		// Token: 0x0400369F RID: 13983
		[FieldOffset(104)]
		public SONY_IMAGE_DATA_DIRECTORY ImportTable;

		// Token: 0x040036A0 RID: 13984
		[FieldOffset(112)]
		public SONY_IMAGE_DATA_DIRECTORY ResourceTable;

		// Token: 0x040036A1 RID: 13985
		[FieldOffset(120)]
		public SONY_IMAGE_DATA_DIRECTORY ExceptionTable;

		// Token: 0x040036A2 RID: 13986
		[FieldOffset(128)]
		public SONY_IMAGE_DATA_DIRECTORY CertificateTable;

		// Token: 0x040036A3 RID: 13987
		[FieldOffset(136)]
		public SONY_IMAGE_DATA_DIRECTORY BaseRelocationTable;

		// Token: 0x040036A4 RID: 13988
		[FieldOffset(144)]
		public SONY_IMAGE_DATA_DIRECTORY Debug;

		// Token: 0x040036A5 RID: 13989
		[FieldOffset(152)]
		public SONY_IMAGE_DATA_DIRECTORY Architecture;

		// Token: 0x040036A6 RID: 13990
		[FieldOffset(160)]
		public SONY_IMAGE_DATA_DIRECTORY GlobalPtr;

		// Token: 0x040036A7 RID: 13991
		[FieldOffset(168)]
		public SONY_IMAGE_DATA_DIRECTORY TLSTable;

		// Token: 0x040036A8 RID: 13992
		[FieldOffset(176)]
		public SONY_IMAGE_DATA_DIRECTORY LoadConfigTable;

		// Token: 0x040036A9 RID: 13993
		[FieldOffset(184)]
		public SONY_IMAGE_DATA_DIRECTORY BoundImport;

		// Token: 0x040036AA RID: 13994
		[FieldOffset(192)]
		public SONY_IMAGE_DATA_DIRECTORY IAT;

		// Token: 0x040036AB RID: 13995
		[FieldOffset(200)]
		public SONY_IMAGE_DATA_DIRECTORY DelayImportDescriptor;

		// Token: 0x040036AC RID: 13996
		[FieldOffset(208)]
		public SONY_IMAGE_DATA_DIRECTORY CLRRuntimeHeader;

		// Token: 0x040036AD RID: 13997
		[FieldOffset(216)]
		public SONY_IMAGE_DATA_DIRECTORY Reserved;
	}
}
