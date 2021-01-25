using System;

namespace Xft
{
	// Token: 0x0200058F RID: 1423
	public class CameraShakeEvent : XftEvent
	{
		// Token: 0x060023B4 RID: 9140 RVA: 0x00017C12 File Offset: 0x00015E12
		public CameraShakeEvent(XftEventComponent owner) : base(XEventType.CameraShake, owner)
		{
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x00017C1C File Offset: 0x00015E1C
		public override void Initialize()
		{
			this.ToggleCameraShakeComponent(true);
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x00017C25 File Offset: 0x00015E25
		public override void Reset()
		{
			base.Reset();
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x00117398 File Offset: 0x00115598
		public override void OnBegin()
		{
			base.OnBegin();
			this.m_cameraShake.Reset(this.m_owner);
			if (this.m_owner.CameraShakeType == XCameraShakeType.Spring)
			{
				this.m_cameraShake.PositionSpring.AddForce(this.m_owner.PositionForce);
				this.m_cameraShake.RotationSpring.AddForce(this.m_owner.RotationForce);
				this.m_cameraShake.EarthQuakeToggled = this.m_owner.UseEarthQuake;
			}
			this.m_cameraShake.enabled = true;
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x00117424 File Offset: 0x00115624
		protected void ToggleCameraShakeComponent(bool flag)
		{
			this.m_cameraShake = base.MyCamera.gameObject.GetComponent<XftCameraShakeComp>();
			if (this.m_cameraShake == null)
			{
				this.m_cameraShake = base.MyCamera.gameObject.AddComponent<XftCameraShakeComp>();
			}
			this.m_cameraShake.enabled = flag;
		}

		// Token: 0x04002B17 RID: 11031
		protected XftCameraShakeComp m_cameraShake;
	}
}
