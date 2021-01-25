using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000592 RID: 1426
	public class TimeScaleEvent : XftEvent
	{
		// Token: 0x060023C2 RID: 9154 RVA: 0x00017CDA File Offset: 0x00015EDA
		public TimeScaleEvent(XftEventComponent owner) : base(XEventType.TimeScale, owner)
		{
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x00017CF6 File Offset: 0x00015EF6
		public override void Reset()
		{
			base.Reset();
			if (this.mIsFirst)
			{
				this.mOriScale = Time.timeScale;
			}
			Time.timeScale = this.mOriScale;
			this.mIsFirst = false;
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x00017D26 File Offset: 0x00015F26
		public override void OnBegin()
		{
			base.OnBegin();
			Time.timeScale = this.m_owner.TimeScale;
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x001177A4 File Offset: 0x001159A4
		public override void Update(float deltaTime)
		{
			this.m_elapsedTime += deltaTime;
			float elapsedTime = this.m_elapsedTime;
			if (elapsedTime / this.m_owner.TimeScale > this.m_owner.TimeScaleDuration)
			{
				Time.timeScale = this.mOriScale;
			}
		}

		// Token: 0x04002B19 RID: 11033
		protected bool mIsFirst = true;

		// Token: 0x04002B1A RID: 11034
		protected float mOriScale = 1f;
	}
}
