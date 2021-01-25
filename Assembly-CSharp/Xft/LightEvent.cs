using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000590 RID: 1424
	public class LightEvent : XftEvent
	{
		// Token: 0x060023B9 RID: 9145 RVA: 0x00017C2D File Offset: 0x00015E2D
		public LightEvent(XftEventComponent owner) : base(XEventType.Light, owner)
		{
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x0011747C File Offset: 0x0011567C
		public override void Initialize()
		{
			if (this.m_owner.LightComp == null)
			{
				Debug.LogWarning("you should assign a light source to Light Event to use it!");
				return;
			}
			this.m_elapsedTime = 0f;
			XffectComponent.SetActive(this.m_owner.LightComp.gameObject, false);
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x00017C37 File Offset: 0x00015E37
		public override void Reset()
		{
			base.Reset();
			if (this.m_owner.LightComp == null)
			{
				return;
			}
			XffectComponent.SetActive(this.m_owner.LightComp.gameObject, false);
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x00017C6C File Offset: 0x00015E6C
		public override void OnBegin()
		{
			base.OnBegin();
			if (this.m_owner.LightComp != null)
			{
				XffectComponent.SetActive(this.m_owner.LightComp.gameObject, true);
			}
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x001174CC File Offset: 0x001156CC
		public override void Update(float deltaTime)
		{
			if (this.m_owner.LightComp == null)
			{
				return;
			}
			this.m_elapsedTime += deltaTime;
			float intensity;
			if (this.m_owner.LightIntensityType == MAGTYPE.Curve_OBSOLETE)
			{
				intensity = this.m_owner.LightIntensityCurve.Evaluate(this.m_elapsedTime - this.m_owner.StartTime);
			}
			else if (this.m_owner.LightIntensityType == MAGTYPE.Fixed)
			{
				intensity = this.m_owner.LightIntensity;
			}
			else
			{
				intensity = this.m_owner.LightIntensityCurveX.Evaluate(this.m_elapsedTime - this.m_owner.StartTime);
			}
			this.m_owner.LightComp.intensity = intensity;
			float range;
			if (this.m_owner.LightRangeType == MAGTYPE.Curve_OBSOLETE)
			{
				range = this.m_owner.LightRangeCurve.Evaluate(this.m_elapsedTime - this.m_owner.StartTime);
			}
			else if (this.m_owner.LightRangeType == MAGTYPE.Fixed)
			{
				range = this.m_owner.LightRange;
			}
			else
			{
				range = this.m_owner.LightRangeCurveX.Evaluate(this.m_elapsedTime - this.m_owner.StartTime);
			}
			this.m_owner.LightComp.range = range;
		}
	}
}
