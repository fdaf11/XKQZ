using System;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000152 RID: 338
	[Flags]
	public enum TypeConstraint
	{
		// Token: 0x04000773 RID: 1907
		None = 0,
		// Token: 0x04000774 RID: 1908
		Primitive = 1,
		// Token: 0x04000775 RID: 1909
		UnityPrimitive = 2,
		// Token: 0x04000776 RID: 1910
		UnityObject = 4,
		// Token: 0x04000777 RID: 1911
		AllPrimitive = 3,
		// Token: 0x04000778 RID: 1912
		All = 7
	}
}
