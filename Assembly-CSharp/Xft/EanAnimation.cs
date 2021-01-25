using System;
using System.IO;

namespace Xft
{
	// Token: 0x020005A0 RID: 1440
	public class EanAnimation
	{
		// Token: 0x06002404 RID: 9220 RVA: 0x00119C88 File Offset: 0x00117E88
		public void Load(BinaryReader br, FileStream fs)
		{
			this.Offset = br.ReadInt32();
			this.Offset += 16;
			this.FrameCount = br.ReadInt32();
			this.MipWidth = br.ReadInt32();
			this.MipHeight = br.ReadInt32();
			this.StartX = br.ReadInt32();
			this.StartY = br.ReadInt32();
			this.TileCount = br.ReadUInt16();
			this.TotalCount = br.ReadUInt16();
			this.CellWidth = br.ReadUInt16();
			this.CellHeight = br.ReadUInt16();
			this.Frames = new EanFrame[(int)this.TotalCount];
			long position = fs.Position;
			fs.Seek((long)this.Offset, 0);
			for (int i = 0; i < (int)this.TotalCount; i++)
			{
				this.Frames[i].X = br.ReadUInt16();
				this.Frames[i].Y = br.ReadUInt16();
				this.Frames[i].Width = br.ReadUInt16();
				this.Frames[i].Height = br.ReadUInt16();
			}
			fs.Seek(position, 0);
		}

		// Token: 0x04002BB0 RID: 11184
		public int Offset;

		// Token: 0x04002BB1 RID: 11185
		public int FrameCount;

		// Token: 0x04002BB2 RID: 11186
		public int MipWidth;

		// Token: 0x04002BB3 RID: 11187
		public int MipHeight;

		// Token: 0x04002BB4 RID: 11188
		public int StartX;

		// Token: 0x04002BB5 RID: 11189
		public int StartY;

		// Token: 0x04002BB6 RID: 11190
		public ushort TileCount;

		// Token: 0x04002BB7 RID: 11191
		public ushort TotalCount;

		// Token: 0x04002BB8 RID: 11192
		public ushort CellWidth;

		// Token: 0x04002BB9 RID: 11193
		public ushort CellHeight;

		// Token: 0x04002BBA RID: 11194
		public EanFrame[] Frames;
	}
}
