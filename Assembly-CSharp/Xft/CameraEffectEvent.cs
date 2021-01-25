using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000580 RID: 1408
	public class CameraEffectEvent : XftEvent, IComparable<CameraEffectEvent>
	{
		// Token: 0x06002349 RID: 9033 RVA: 0x00017574 File Offset: 0x00015774
		public CameraEffectEvent(CameraEffectEvent.EType etype, XftEventComponent owner) : base(XEventType.CameraEffect, owner)
		{
			this.m_effectType = etype;
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x0001758C File Offset: 0x0001578C
		public int CompareTo(CameraEffectEvent otherObj)
		{
			return this.m_owner.CameraEffectPriority.CompareTo(otherObj.m_owner.CameraEffectPriority);
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x0600234B RID: 9035 RVA: 0x00115610 File Offset: 0x00113810
		public XftCameraEffectComp CameraComp
		{
			get
			{
				this.m_comp = base.MyCamera.gameObject.GetComponent<XftCameraEffectComp>();
				if (this.m_comp == null)
				{
					this.m_comp = base.MyCamera.gameObject.AddComponent<XftCameraEffectComp>();
				}
				return this.m_comp;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x0600234C RID: 9036 RVA: 0x000175A9 File Offset: 0x000157A9
		public CameraEffectEvent.EType EffectType
		{
			get
			{
				return this.m_effectType;
			}
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000175B1 File Offset: 0x000157B1
		public override void Initialize()
		{
			if (!this.CheckSupport())
			{
				Debug.LogWarning("camera effect is not supported on this device:" + this.m_effectType);
				this.m_supported = false;
				return;
			}
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x000175E0 File Offset: 0x000157E0
		public override void OnBegin()
		{
			if (!this.m_supported)
			{
				return;
			}
			base.OnBegin();
			this.CameraComp.AddEvent(this);
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x00017600 File Offset: 0x00015800
		public override void Reset()
		{
			base.Reset();
			this.CameraComp.ResetEvent(this);
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void ToggleCameraComponent(bool flag)
		{
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x00002B59 File Offset: 0x00000D59
		public virtual bool CheckSupport()
		{
			return true;
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void OnPreRender()
		{
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}

		// Token: 0x04002AC0 RID: 10944
		protected CameraEffectEvent.EType m_effectType;

		// Token: 0x04002AC1 RID: 10945
		protected XftCameraEffectComp m_comp;

		// Token: 0x04002AC2 RID: 10946
		protected bool m_supported = true;

		// Token: 0x02000581 RID: 1409
		public enum EType
		{
			// Token: 0x04002AC4 RID: 10948
			RadialBlur,
			// Token: 0x04002AC5 RID: 10949
			RadialBlurMask,
			// Token: 0x04002AC6 RID: 10950
			Glow,
			// Token: 0x04002AC7 RID: 10951
			GlowPerObj,
			// Token: 0x04002AC8 RID: 10952
			ColorInverse,
			// Token: 0x04002AC9 RID: 10953
			Glitch
		}
	}
}
