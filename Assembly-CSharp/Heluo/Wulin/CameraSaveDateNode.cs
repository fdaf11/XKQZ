using System;

namespace Heluo.Wulin
{
	// Token: 0x020002CA RID: 714
	public class CameraSaveDateNode
	{
		// Token: 0x06000E22 RID: 3618 RVA: 0x00009A26 File Offset: 0x00007C26
		public void Copy(CameraSaveDateNode csdn)
		{
			this.m_fDistance = csdn.m_fDistance;
			this.m_fRotation = csdn.m_fRotation;
			this.m_fTilt = csdn.m_fTilt;
		}

		// Token: 0x0400106A RID: 4202
		public float m_fDistance;

		// Token: 0x0400106B RID: 4203
		public float m_fRotation;

		// Token: 0x0400106C RID: 4204
		public float m_fTilt;
	}
}
