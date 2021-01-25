using System;
using System.IO;

namespace Xft
{
	// Token: 0x020005A1 RID: 1441
	internal class EanFile
	{
		// Token: 0x06002406 RID: 9222 RVA: 0x00119DC0 File Offset: 0x00117FC0
		public void Load(BinaryReader br, FileStream fs)
		{
			this.Header = br.ReadInt32();
			this.Version = br.ReadInt32();
			this.Reserved = br.ReadInt32();
			this.AnimCount = br.ReadInt32();
			this.Anims = new EanAnimation[this.AnimCount];
			for (int i = 0; i < this.AnimCount; i++)
			{
				this.Anims[i] = new EanAnimation();
				this.Anims[i].Load(br, fs);
			}
		}

		// Token: 0x04002BBB RID: 11195
		public int Header;

		// Token: 0x04002BBC RID: 11196
		public int Version;

		// Token: 0x04002BBD RID: 11197
		public int Reserved;

		// Token: 0x04002BBE RID: 11198
		public int AnimCount;

		// Token: 0x04002BBF RID: 11199
		public EanAnimation[] Anims;
	}
}
