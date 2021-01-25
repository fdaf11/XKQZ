using System;
using System.IO;
using UnityEngine;

namespace Xft
{
	// Token: 0x020005A2 RID: 1442
	public class UVAnimation
	{
		// Token: 0x06002408 RID: 9224 RVA: 0x00017F35 File Offset: 0x00016135
		public void Reset()
		{
			this.curFrame = 0;
			this.stepDir = 1;
			this.numLoops = 0;
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x00017F4C File Offset: 0x0001614C
		public void PlayInReverse()
		{
			this.stepDir = -1;
			this.curFrame = this.frames.Length - 1;
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x00119E44 File Offset: 0x00118044
		public bool GetNextFrame(ref Vector2 uv, ref Vector2 dm)
		{
			if (this.curFrame + this.stepDir >= this.frames.Length || this.curFrame + this.stepDir < 0)
			{
				if (this.stepDir > 0 && this.loopReverse)
				{
					this.stepDir = -1;
					this.curFrame += this.stepDir;
					uv = this.frames[this.curFrame];
					dm = this.UVDimensions[this.curFrame];
				}
				else
				{
					if (this.numLoops + 1 > this.loopCycles && this.loopCycles != -1)
					{
						return false;
					}
					this.numLoops++;
					if (this.loopReverse)
					{
						this.stepDir *= -1;
						this.curFrame += this.stepDir;
					}
					else
					{
						this.curFrame = 0;
					}
					uv = this.frames[this.curFrame];
					dm = this.UVDimensions[this.curFrame];
				}
			}
			else
			{
				this.curFrame += this.stepDir;
				uv = this.frames[this.curFrame];
				dm = this.UVDimensions[this.curFrame];
			}
			return true;
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x00119FDC File Offset: 0x001181DC
		public void BuildFromFile(string path, int index, float uvTime, Texture mainTex)
		{
			if (!File.Exists(path))
			{
				Debug.LogError("wrong ean file path!");
				return;
			}
			FileStream fileStream = new FileStream(path, 3);
			BinaryReader br = new BinaryReader(fileStream);
			EanFile eanFile = new EanFile();
			eanFile.Load(br, fileStream);
			fileStream.Close();
			EanAnimation eanAnimation = eanFile.Anims[index];
			this.frames = new Vector2[(int)eanAnimation.TotalCount];
			this.UVDimensions = new Vector2[(int)eanAnimation.TotalCount];
			int tileCount = (int)eanAnimation.TileCount;
			int num = ((int)eanAnimation.TotalCount + tileCount - 1) / tileCount;
			int num2 = 0;
			int width = mainTex.width;
			int height = mainTex.height;
			for (int i = 0; i < num; i++)
			{
				int num3 = 0;
				while (num3 < tileCount && num2 < (int)eanAnimation.TotalCount)
				{
					Vector2 zero = Vector2.zero;
					zero.x = (float)eanAnimation.Frames[num2].Width / (float)width;
					zero.y = (float)eanAnimation.Frames[num2].Height / (float)height;
					this.frames[num2].x = (float)eanAnimation.Frames[num2].X / (float)width;
					this.frames[num2].y = 1f - (float)eanAnimation.Frames[num2].Y / (float)height;
					this.UVDimensions[num2] = zero;
					this.UVDimensions[num2].y = -this.UVDimensions[num2].y;
					num2++;
					num3++;
				}
			}
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x0011A190 File Offset: 0x00118390
		public Vector2[] BuildUVAnim(Vector2 start, Vector2 cellSize, int cols, int rows, int totalCells)
		{
			int num = 0;
			this.frames = new Vector2[totalCells];
			this.UVDimensions = new Vector2[totalCells];
			this.frames[0] = start;
			for (int i = 0; i < rows; i++)
			{
				int num2 = 0;
				while (num2 < cols && num < totalCells)
				{
					this.frames[num].x = start.x + cellSize.x * (float)num2;
					this.frames[num].y = start.y + cellSize.y * (float)i;
					this.UVDimensions[num] = cellSize;
					num++;
					num2++;
				}
			}
			return this.frames;
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x00017F65 File Offset: 0x00016165
		public void SetAnim(Vector2[] anim)
		{
			this.frames = anim;
		}

		// Token: 0x04002BC0 RID: 11200
		public Vector2[] frames;

		// Token: 0x04002BC1 RID: 11201
		public Vector2[] UVDimensions;

		// Token: 0x04002BC2 RID: 11202
		public int curFrame;

		// Token: 0x04002BC3 RID: 11203
		protected int stepDir = 1;

		// Token: 0x04002BC4 RID: 11204
		public int numLoops;

		// Token: 0x04002BC5 RID: 11205
		public string name;

		// Token: 0x04002BC6 RID: 11206
		public int loopCycles;

		// Token: 0x04002BC7 RID: 11207
		public bool loopReverse;
	}
}
