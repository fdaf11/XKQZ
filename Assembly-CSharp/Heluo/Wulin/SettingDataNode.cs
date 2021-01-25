using System;

namespace Heluo.Wulin
{
	// Token: 0x02000259 RID: 601
	public class SettingDataNode
	{
		// Token: 0x06000B0E RID: 2830 RVA: 0x0005C1F0 File Offset: 0x0005A3F0
		public SettingDataNode Clone()
		{
			return new SettingDataNode
			{
				m_iQualityLevel = this.m_iQualityLevel,
				m_bShadow = this.m_bShadow,
				m_fShadowDistance = this.m_fShadowDistance,
				m_iVSync = this.m_iVSync,
				m_iAntiAliasing = this.m_iAntiAliasing,
				m_iImageQuality = this.m_iImageQuality,
				m_bSSAO = this.m_bSSAO,
				m_bLensEffects = this.m_bLensEffects,
				m_bBloom = this.m_bBloom,
				m_iResourcesCount = this.m_iResourcesCount
			};
		}

		// Token: 0x04000C8B RID: 3211
		public int m_iQualityLevel;

		// Token: 0x04000C8C RID: 3212
		public bool m_bShadow;

		// Token: 0x04000C8D RID: 3213
		public float m_fShadowDistance;

		// Token: 0x04000C8E RID: 3214
		public int m_iVSync;

		// Token: 0x04000C8F RID: 3215
		public int m_iAntiAliasing;

		// Token: 0x04000C90 RID: 3216
		public int m_iImageQuality;

		// Token: 0x04000C91 RID: 3217
		public bool m_bSSAO;

		// Token: 0x04000C92 RID: 3218
		public bool m_bLensEffects;

		// Token: 0x04000C93 RID: 3219
		public bool m_bBloom;

		// Token: 0x04000C94 RID: 3220
		public int m_iResourcesCount;
	}
}
