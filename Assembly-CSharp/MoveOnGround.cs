using System;
using UnityEngine;

// Token: 0x020005D4 RID: 1492
public class MoveOnGround : MonoBehaviour
{
	// Token: 0x1400003C RID: 60
	// (add) Token: 0x0600250A RID: 9482 RVA: 0x000188A0 File Offset: 0x00016AA0
	// (remove) Token: 0x0600250B RID: 9483 RVA: 0x000188B9 File Offset: 0x00016AB9
	public event EventHandler<CollisionInfo> OnCollision;

	// Token: 0x0600250C RID: 9484 RVA: 0x00121088 File Offset: 0x0011F288
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

	// Token: 0x0600250D RID: 9485 RVA: 0x001210D4 File Offset: 0x0011F2D4
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings == null)
		{
			Debug.Log("Prefab root have not script \"PrefabSettings\"");
		}
		this.particles = this.effectSettings.GetComponentsInChildren<ParticleSystem>();
		this.InitDefaultVariables();
		this.isInitialized = true;
	}

	// Token: 0x0600250E RID: 9486 RVA: 0x000188D2 File Offset: 0x00016AD2
	private void OnEnable()
	{
		if (this.isInitialized)
		{
			this.InitDefaultVariables();
		}
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x00121128 File Offset: 0x0011F328
	private void InitDefaultVariables()
	{
		foreach (ParticleSystem particleSystem in this.particles)
		{
			particleSystem.Stop();
		}
		this.isFinished = false;
		this.tTarget = this.effectSettings.Target.transform;
		if (this.IsRootMove)
		{
			this.tRoot = this.effectSettings.transform;
		}
		else
		{
			this.tRoot = base.transform.parent;
			this.tRoot.localPosition = Vector3.zero;
		}
		this.targetPos = this.tRoot.position + Vector3.Normalize(this.tTarget.position - this.tRoot.position) * this.effectSettings.MoveDistance;
		RaycastHit raycastHit;
		Physics.Raycast(this.tRoot.position, Vector3.down, ref raycastHit);
		this.tRoot.position = raycastHit.point;
		foreach (ParticleSystem particleSystem2 in this.particles)
		{
			particleSystem2.Play();
		}
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x00121258 File Offset: 0x0011F458
	private void Update()
	{
		if (this.tTarget == null || this.isFinished)
		{
			return;
		}
		Vector3 position = this.tRoot.position;
		RaycastHit raycastHit;
		Physics.Raycast(new Vector3(position.x, 0.5f, position.z), Vector3.down, ref raycastHit);
		this.tRoot.position = raycastHit.point;
		position = this.tRoot.position;
		Vector3 vector = (!this.effectSettings.IsHomingMove) ? this.targetPos : this.tTarget.position;
		Vector3 vector2;
		vector2..ctor(vector.x, 0f, vector.z);
		if (Vector3.Distance(new Vector3(position.x, 0f, position.z), vector2) <= this.effectSettings.ColliderRadius)
		{
			this.effectSettings.OnCollisionHandler(new CollisionInfo());
			this.isFinished = true;
		}
		this.tRoot.position = Vector3.MoveTowards(position, vector2, this.effectSettings.MoveSpeed * Time.deltaTime);
	}

	// Token: 0x04002D36 RID: 11574
	public bool IsRootMove = true;

	// Token: 0x04002D37 RID: 11575
	private EffectSettings effectSettings;

	// Token: 0x04002D38 RID: 11576
	private Transform tRoot;

	// Token: 0x04002D39 RID: 11577
	private Transform tTarget;

	// Token: 0x04002D3A RID: 11578
	private Vector3 targetPos;

	// Token: 0x04002D3B RID: 11579
	private bool isInitialized;

	// Token: 0x04002D3C RID: 11580
	private bool isFinished;

	// Token: 0x04002D3D RID: 11581
	private ParticleSystem[] particles;
}
