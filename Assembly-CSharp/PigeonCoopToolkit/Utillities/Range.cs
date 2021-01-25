using System;

namespace PigeonCoopToolkit.Utillities
{
	// Token: 0x020005BC RID: 1468
	[Serializable]
	public class Range
	{
		// Token: 0x060024A1 RID: 9377 RVA: 0x000184E1 File Offset: 0x000166E1
		public bool WithinRange(float value)
		{
			return this.Min <= value && this.Max >= value;
		}

		// Token: 0x04002C64 RID: 11364
		public float Min;

		// Token: 0x04002C65 RID: 11365
		public float Max;
	}
}
