using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000599 RID: 1433
	public class RenderObject
	{
		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060023D7 RID: 9175 RVA: 0x00117F24 File Offset: 0x00116124
		protected float Fps
		{
			get
			{
				float num = 1f / (float)this.Node.Owner.Owner.MaxFps;
				if (!this.Node.Owner.Owner.IgnoreTimeScale)
				{
					num *= Time.timeScale;
				}
				return num;
			}
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x00017DD2 File Offset: 0x00015FD2
		public virtual void Initialize(EffectNode node)
		{
			this.Node = node;
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void Reset()
		{
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void Update(float deltaTime)
		{
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void ApplyShaderParam(float x, float y)
		{
		}

		// Token: 0x04002B81 RID: 11137
		public EffectNode Node;
	}
}
