using System;

// Token: 0x020003EF RID: 1007
public class NgEnum
{
	// Token: 0x04001CF6 RID: 7414
	public static string[] m_TextureSizeStrings = new string[]
	{
		"32",
		"64",
		"128",
		"256",
		"512",
		"1024",
		"2048",
		"4096"
	};

	// Token: 0x04001CF7 RID: 7415
	public static int[] m_TextureSizeIntters = new int[]
	{
		32,
		64,
		128,
		256,
		512,
		1024,
		2048,
		4096
	};

	// Token: 0x020003F0 RID: 1008
	public enum AXIS
	{
		// Token: 0x04001CF9 RID: 7417
		X,
		// Token: 0x04001CFA RID: 7418
		Y,
		// Token: 0x04001CFB RID: 7419
		Z
	}

	// Token: 0x020003F1 RID: 1009
	public enum TRANSFORM
	{
		// Token: 0x04001CFD RID: 7421
		POSITION,
		// Token: 0x04001CFE RID: 7422
		ROTATION,
		// Token: 0x04001CFF RID: 7423
		SCALE
	}

	// Token: 0x020003F2 RID: 1010
	public enum PREFAB_TYPE
	{
		// Token: 0x04001D01 RID: 7425
		All,
		// Token: 0x04001D02 RID: 7426
		ParticleSystem,
		// Token: 0x04001D03 RID: 7427
		LegacyParticle,
		// Token: 0x04001D04 RID: 7428
		NcSpriteFactory
	}
}
