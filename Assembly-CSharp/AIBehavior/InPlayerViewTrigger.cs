using System;
using UnityEngine;

namespace AIBehavior
{
	// Token: 0x02000043 RID: 67
	public class InPlayerViewTrigger : BaseTrigger
	{
		// Token: 0x06000131 RID: 305 RVA: 0x000030B1 File Offset: 0x000012B1
		protected override void Init()
		{
			this.mainCam = this.GetMainCamera();
			if (this.mainCam == null)
			{
				Debug.LogWarning("No main camera found, 'InPlayerViewTrigger' will not work without a main camera.");
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000030DA File Offset: 0x000012DA
		protected override bool Evaluate(AIBehaviors fsm)
		{
			return this.CheckIfInPlayerCameraView(fsm.transform.position);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00024774 File Offset: 0x00022974
		public bool CheckIfInPlayerCameraView(Vector3 fsmPosition)
		{
			if (this.mainCam == null)
			{
				this.mainCam = this.GetMainCamera();
			}
			if (this.mainCam != null)
			{
				Vector3 vector = this.mainCam.WorldToScreenPoint(fsmPosition);
				if (vector.z > 0f && vector.x > 0f && vector.x < (float)Screen.width && vector.y > 0f && vector.y < (float)Screen.height)
				{
					Ray ray;
					ray..ctor(this.mainCam.transform.position, this.mainCam.transform.forward);
					RaycastHit raycastHit;
					if (!Physics.Raycast(ray, ref raycastHit, 1000f))
					{
						Transform transform = base.transform;
						Transform transform2 = raycastHit.transform;
						while (transform != null)
						{
							if (transform == transform2)
							{
								return false;
							}
							transform = transform.parent;
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000030ED File Offset: 0x000012ED
		private Camera GetMainCamera()
		{
			return Camera.main;
		}

		// Token: 0x040000FB RID: 251
		private Camera mainCam;
	}
}
