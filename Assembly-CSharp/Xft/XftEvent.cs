using System;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000594 RID: 1428
	public class XftEvent
	{
		// Token: 0x060023C8 RID: 9160 RVA: 0x00017D4A File Offset: 0x00015F4A
		public XftEvent(XEventType type, XftEventComponent owner)
		{
			this.m_type = type;
			this.m_owner = owner;
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060023C9 RID: 9161 RVA: 0x00017D60 File Offset: 0x00015F60
		public Camera MyCamera
		{
			get
			{
				return this.m_owner.Owner.MyCamera;
			}
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x001177F0 File Offset: 0x001159F0
		protected Camera FindMyCamera()
		{
			int num = 1 << this.m_owner.gameObject.layer;
			Camera[] array = Object.FindObjectsOfType(typeof(Camera)) as Camera[];
			int i = 0;
			int num2 = array.Length;
			while (i < num2)
			{
				Camera camera = array[i];
				if ((camera.cullingMask & num) != 0)
				{
					return camera;
				}
				i++;
			}
			Debug.LogError("can't find proper camera for event:" + this.m_type);
			return null;
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060023CB RID: 9163 RVA: 0x00017D72 File Offset: 0x00015F72
		// (set) Token: 0x060023CC RID: 9164 RVA: 0x00017D7A File Offset: 0x00015F7A
		public bool CanUpdate
		{
			get
			{
				return this.m_canUpdate;
			}
			set
			{
				this.m_canUpdate = value;
			}
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x00017D83 File Offset: 0x00015F83
		public virtual void OnBegin()
		{
			this.CanUpdate = true;
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void Initialize()
		{
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x0000264F File Offset: 0x0000084F
		public virtual void Update(float deltaTime)
		{
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x00017D8C File Offset: 0x00015F8C
		public virtual void Reset()
		{
			this.m_elapsedTime = 0f;
			this.CanUpdate = false;
		}

		// Token: 0x04002B1C RID: 11036
		protected XEventType m_type;

		// Token: 0x04002B1D RID: 11037
		protected XftEventComponent m_owner;

		// Token: 0x04002B1E RID: 11038
		protected float m_elapsedTime;

		// Token: 0x04002B1F RID: 11039
		protected bool m_canUpdate;

		// Token: 0x04002B20 RID: 11040
		protected Camera m_myCamera;
	}
}
