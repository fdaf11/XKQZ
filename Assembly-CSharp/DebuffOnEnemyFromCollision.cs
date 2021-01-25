using System;
using UnityEngine;

// Token: 0x020005F5 RID: 1525
public class DebuffOnEnemyFromCollision : MonoBehaviour
{
	// Token: 0x060025C3 RID: 9667 RVA: 0x00019245 File Offset: 0x00017445
	private void Start()
	{
		this.EffectSettings.CollisionEnter += new EventHandler<CollisionInfo>(this.EffectSettings_CollisionEnter);
	}

	// Token: 0x060025C4 RID: 9668 RVA: 0x001249FC File Offset: 0x00122BFC
	private void EffectSettings_CollisionEnter(object sender, CollisionInfo e)
	{
		if (this.Effect == null)
		{
			return;
		}
		Collider[] array = Physics.OverlapSphere(base.transform.position, this.EffectSettings.EffectRadius, this.EffectSettings.LayerMask);
		foreach (Collider collider in array)
		{
			Transform transform = collider.transform;
			Renderer componentInChildren = transform.GetComponentInChildren<Renderer>();
			GameObject gameObject = Object.Instantiate(this.Effect) as GameObject;
			gameObject.transform.parent = componentInChildren.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.GetComponent<AddMaterialOnHit>().UpdateMaterial(collider.transform);
		}
	}

	// Token: 0x060025C5 RID: 9669 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x04002E4E RID: 11854
	public EffectSettings EffectSettings;

	// Token: 0x04002E4F RID: 11855
	public GameObject Effect;
}
