using System;
using UnityEngine;

namespace Exploder.Demo
{
	// Token: 0x020000A8 RID: 168
	public class DemoSelfExplode : MonoBehaviour
	{
		// Token: 0x06000393 RID: 915 RVA: 0x0000479E File Offset: 0x0000299E
		private void Start()
		{
			Application.targetFrameRate = 60;
			if (!this.Camera)
			{
				this.Camera = Camera.main;
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000047C2 File Offset: 0x000029C2
		private bool IsExplodable(GameObject obj)
		{
			return obj.CompareTag(ExploderObject.Tag);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0002D920 File Offset: 0x0002BB20
		private void Update()
		{
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
			{
				Ray ray;
				if (this.Camera)
				{
					ray = this.Camera.ScreenPointToRay(Input.mousePosition);
				}
				else
				{
					ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				}
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, ref raycastHit))
				{
					GameObject gameObject = raycastHit.collider.gameObject;
					if (this.IsExplodable(gameObject) && Input.GetMouseButtonDown(0))
					{
						this.ExplodeObject(gameObject);
					}
				}
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0002D9B4 File Offset: 0x0002BBB4
		private void ExplodeObject(GameObject obj)
		{
			ExploderObject component = obj.GetComponent<ExploderObject>();
			if (component)
			{
				component.Explode(new ExploderObject.OnExplosion(this.OnExplosion));
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000475B File Offset: 0x0000295B
		private void OnExplosion(float time, ExploderObject.ExplosionState state)
		{
			if (state == ExploderObject.ExplosionState.ExplosionFinished)
			{
			}
		}

		// Token: 0x040002CF RID: 719
		public Camera Camera;
	}
}
