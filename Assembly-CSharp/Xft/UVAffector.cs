using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000554 RID: 1364
	public class UVAffector : Affector
	{
		// Token: 0x0600226B RID: 8811 RVA: 0x0010D4B4 File Offset: 0x0010B6B4
		public UVAffector(UVAnimation frame, float time, EffectNode node, bool randomStart) : base(node, AFFECTORTYPE.UVAffector)
		{
			this.Frames = frame;
			this.UVTime = time;
			this.RandomStartFrame = randomStart;
			if (this.RandomStartFrame)
			{
				this.Frames.curFrame = Random.Range(0, this.Frames.frames.Length - 1);
			}
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x0010D514 File Offset: 0x0010B714
		public override void Reset()
		{
			this.ElapsedTime = 0f;
			this.FirstUpdate = true;
			this.Frames.curFrame = 0;
			this.Frames.numLoops = 0;
			if (this.RandomStartFrame)
			{
				this.Frames.curFrame = Random.Range(0, this.Frames.frames.Length - 1);
			}
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x0010D578 File Offset: 0x0010B778
		public override void Update(float deltaTime)
		{
			this.ElapsedTime += deltaTime;
			float num;
			if (this.UVTime <= 0f)
			{
				num = this.Node.GetLifeTime() / (float)this.Frames.frames.Length;
			}
			else
			{
				num = this.UVTime / (float)this.Frames.frames.Length;
			}
			if (this.ElapsedTime >= num || this.FirstUpdate)
			{
				Vector2 zero = Vector2.zero;
				Vector2 zero2 = Vector2.zero;
				this.Frames.GetNextFrame(ref zero, ref zero2);
				this.Node.LowerLeftUV = zero;
				this.Node.UVDimensions = zero2;
				this.ElapsedTime -= num;
			}
			this.FirstUpdate = false;
		}

		// Token: 0x040028B2 RID: 10418
		protected UVAnimation Frames;

		// Token: 0x040028B3 RID: 10419
		protected float ElapsedTime;

		// Token: 0x040028B4 RID: 10420
		protected float UVTime;

		// Token: 0x040028B5 RID: 10421
		protected bool RandomStartFrame;

		// Token: 0x040028B6 RID: 10422
		protected bool FirstUpdate = true;
	}
}
