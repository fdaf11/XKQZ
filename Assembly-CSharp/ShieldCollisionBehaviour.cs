using System;
using UnityEngine;

// Token: 0x020005D0 RID: 1488
public class ShieldCollisionBehaviour : MonoBehaviour
{
	// Token: 0x060024FA RID: 9466 RVA: 0x00120B8C File Offset: 0x0011ED8C
	public void ShieldCollisionEnter(CollisionInfo e)
	{
		if (e.Hit.transform != null)
		{
			if (this.IsWaterInstance)
			{
				GameObject gameObject = Object.Instantiate(this.ExplosionOnHit) as GameObject;
				Transform transform = gameObject.transform;
				transform.parent = base.transform;
				float num = base.transform.localScale.x * this.ScaleWave;
				transform.localScale = new Vector3(num, num, num);
				transform.localPosition = new Vector3(0f, 0.001f, 0f);
				transform.LookAt(e.Hit.point);
			}
			else
			{
				if (this.EffectOnHit != null)
				{
					if (!this.CreateMechInstanceOnHit)
					{
						Transform transform2 = e.Hit.transform;
						Renderer componentInChildren = transform2.GetComponentInChildren<Renderer>();
						GameObject gameObject2 = Object.Instantiate(this.EffectOnHit) as GameObject;
						gameObject2.transform.parent = componentInChildren.transform;
						gameObject2.transform.localPosition = Vector3.zero;
						AddMaterialOnHit component = gameObject2.GetComponent<AddMaterialOnHit>();
						component.SetMaterialQueue(this.currentQueue);
						component.UpdateMaterial(e.Hit);
					}
					else
					{
						GameObject gameObject3 = Object.Instantiate(this.EffectOnHit) as GameObject;
						Transform transform3 = gameObject3.transform;
						transform3.parent = base.renderer.transform;
						transform3.localPosition = Vector3.zero;
						transform3.localScale = base.transform.localScale * this.ScaleWave;
						transform3.LookAt(e.Hit.point);
						transform3.Rotate(this.AngleFix);
						gameObject3.renderer.material.renderQueue = this.currentQueue - 1000;
					}
				}
				if (this.currentQueue > 4000)
				{
					this.currentQueue = 3001;
				}
				else
				{
					this.currentQueue++;
				}
				if (this.ExplosionOnHit != null)
				{
					GameObject gameObject4 = Object.Instantiate(this.ExplosionOnHit, e.Hit.point, default(Quaternion)) as GameObject;
					gameObject4.transform.parent = base.transform;
				}
			}
		}
	}

	// Token: 0x060024FB RID: 9467 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04002D22 RID: 11554
	public GameObject EffectOnHit;

	// Token: 0x04002D23 RID: 11555
	public GameObject ExplosionOnHit;

	// Token: 0x04002D24 RID: 11556
	public bool IsWaterInstance;

	// Token: 0x04002D25 RID: 11557
	public float ScaleWave = 0.89f;

	// Token: 0x04002D26 RID: 11558
	public bool CreateMechInstanceOnHit;

	// Token: 0x04002D27 RID: 11559
	public Vector3 AngleFix;

	// Token: 0x04002D28 RID: 11560
	public int currentQueue = 3001;
}
