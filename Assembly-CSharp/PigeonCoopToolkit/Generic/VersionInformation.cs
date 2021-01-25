using System;

namespace PigeonCoopToolkit.Generic
{
	// Token: 0x020005B8 RID: 1464
	[Serializable]
	public class VersionInformation
	{
		// Token: 0x06002483 RID: 9347 RVA: 0x00018431 File Offset: 0x00016631
		public VersionInformation(string name, int major, int minor, int patch)
		{
			this.Name = name;
			this.Major = major;
			this.Minor = minor;
			this.Patch = patch;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x0011DA98 File Offset: 0x0011BC98
		public override string ToString()
		{
			return string.Format("{0} {1}.{2}.{3}", new object[]
			{
				this.Name,
				this.Major,
				this.Minor,
				this.Patch
			});
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x0011DAE8 File Offset: 0x0011BCE8
		public bool Match(VersionInformation other, bool looseMatch)
		{
			if (looseMatch)
			{
				return other.Name == this.Name && other.Major == this.Major && other.Minor == this.Minor;
			}
			return other.Name == this.Name && other.Major == this.Major && other.Minor == this.Minor && other.Patch == this.Patch;
		}

		// Token: 0x04002C56 RID: 11350
		public string Name;

		// Token: 0x04002C57 RID: 11351
		public int Major = 1;

		// Token: 0x04002C58 RID: 11352
		public int Minor;

		// Token: 0x04002C59 RID: 11353
		public int Patch;
	}
}
