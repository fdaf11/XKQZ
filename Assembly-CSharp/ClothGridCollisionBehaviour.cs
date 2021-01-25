using System;
using UnityEngine;

// Token: 0x020005D6 RID: 1494
public class ClothGridCollisionBehaviour : MonoBehaviour
{
	// Token: 0x1400003D RID: 61
	// (add) Token: 0x06002517 RID: 9495 RVA: 0x00018937 File Offset: 0x00016B37
	// (remove) Token: 0x06002518 RID: 9496 RVA: 0x00018950 File Offset: 0x00016B50
	public event EventHandler<CollisionInfo> OnCollision;

	// Token: 0x06002519 RID: 9497 RVA: 0x001218D4 File Offset: 0x0011FAD4
	private void GetEffectSettingsComponent(Transform tr)
	{
		Transform parent = tr.parent;
		if (parent != null)
		{
			this.effectSettings = parent.GetComponentInChildren<EffectSettings>();
			if (this.effectSettings == null)
			{
				this.GetEffectSettingsComponent(parent.transform);
			}
		}
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x00018969 File Offset: 0x00016B69
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings == null)
		{
			Debug.Log("Prefab root have not script \"PrefabSettings\"");
		}
		this.tRoot = this.effectSettings.transform;
		this.InitDefaultVariables();
	}

	// Token: 0x0600251B RID: 9499 RVA: 0x00121920 File Offset: 0x0011FB20
	private void InitDefaultVariables()
	{
		this.tTarget = this.effectSettings.Target.transform;
		if (this.IsLookAt)
		{
			this.tRoot.LookAt(this.tTarget);
		}
		Vector3 vector = this.CenterPoint();
		this.targetPos = vector + Vector3.Normalize(this.tTarget.position - vector) * this.effectSettings.MoveDistance;
		Vector3 vector2 = Vector3.Normalize(this.tTarget.position - vector) * this.effectSettings.MoveSpeed / 100f;
		for (int i = 0; i < this.AttachedPoints.Length; i++)
		{
			GameObject gameObject = this.AttachedPoints[i];
			Rigidbody rigidbody = gameObject.rigidbody;
			rigidbody.useGravity = false;
			rigidbody.AddForce(vector2, 1);
			gameObject.SetActive(true);
		}
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x00121A0C File Offset: 0x0011FC0C
	private Vector3 CenterPoint()
	{
		return (base.transform.TransformPoint(this.AttachedPoints[0].transform.localPosition) + base.transform.TransformPoint(this.AttachedPoints[2].transform.localPosition)) / 2f;
	}

	// Token: 0x0600251D RID: 9501 RVA: 0x00121A64 File Offset: 0x0011FC64
	private void Update()
	{
		if (this.tTarget == null || this.onCollision)
		{
			return;
		}
		Vector3 vector = this.CenterPoint();
		RaycastHit hit = default(RaycastHit);
		float num = Vector3.Distance(vector, this.targetPos);
		float num2 = this.effectSettings.MoveSpeed * Time.deltaTime;
		if (num2 > num)
		{
			num2 = num;
		}
		if (num <= this.effectSettings.ColliderRadius)
		{
			this.DeactivateAttachedPoints(hit);
		}
		Vector3 normalized = (this.targetPos - vector).normalized;
		if (Physics.Raycast(vector, normalized, ref hit, num2 + this.effectSettings.ColliderRadius))
		{
			this.targetPos = hit.point - normalized * this.effectSettings.ColliderRadius;
			this.DeactivateAttachedPoints(hit);
		}
	}

	// Token: 0x0600251E RID: 9502 RVA: 0x00121B3C File Offset: 0x0011FD3C
	private void DeactivateAttachedPoints(RaycastHit hit)
	{
		for (int i = 0; i < this.AttachedPoints.Length; i++)
		{
			GameObject gameObject = this.AttachedPoints[i];
			gameObject.SetActive(false);
		}
		CollisionInfo e = new CollisionInfo
		{
			Hit = hit
		};
		this.effectSettings.OnCollisionHandler(e);
		if (hit.transform != null)
		{
			ShieldCollisionBehaviour component = hit.transform.GetComponent<ShieldCollisionBehaviour>();
			if (component != null)
			{
				component.ShieldCollisionEnter(e);
			}
		}
		this.onCollision = true;
	}

	// Token: 0x04002D52 RID: 11602
	public GameObject[] AttachedPoints;

	// Token: 0x04002D53 RID: 11603
	public bool IsLookAt;

	// Token: 0x04002D54 RID: 11604
	private EffectSettings effectSettings;

	// Token: 0x04002D55 RID: 11605
	private Transform tRoot;

	// Token: 0x04002D56 RID: 11606
	private Transform tTarget;

	// Token: 0x04002D57 RID: 11607
	private Vector3 targetPos;

	// Token: 0x04002D58 RID: 11608
	private bool onCollision;
}
