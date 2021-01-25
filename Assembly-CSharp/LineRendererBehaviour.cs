using System;
using UnityEngine;

// Token: 0x020005D5 RID: 1493
public class LineRendererBehaviour : MonoBehaviour
{
	// Token: 0x06002512 RID: 9490 RVA: 0x0012137C File Offset: 0x0011F57C
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

	// Token: 0x06002513 RID: 9491 RVA: 0x001213C8 File Offset: 0x0011F5C8
	private void Start()
	{
		this.GetEffectSettingsComponent(base.transform);
		if (this.effectSettings == null)
		{
			Debug.Log("Prefab root have not script \"PrefabSettings\"");
		}
		this.tRoot = this.effectSettings.transform;
		this.line = base.GetComponent<LineRenderer>();
		this.InitializeDefault();
		this.isInitializedOnStart = true;
	}

	// Token: 0x06002514 RID: 9492 RVA: 0x00121428 File Offset: 0x0011F628
	private void InitializeDefault()
	{
		base.renderer.material.SetFloat("_Chanel", (float)this.currentShaderIndex);
		this.currentShaderIndex++;
		if (this.currentShaderIndex == 3)
		{
			this.currentShaderIndex = 0;
		}
		this.line.SetPosition(0, this.tRoot.position);
		if (this.IsVertical)
		{
			if (Physics.Raycast(this.tRoot.position, Vector3.down, ref this.hit))
			{
				this.line.SetPosition(1, this.hit.point);
				if (this.StartGlow != null)
				{
					this.StartGlow.transform.position = this.tRoot.position;
				}
				if (this.HitGlow != null)
				{
					this.HitGlow.transform.position = this.hit.point;
				}
				if (this.GoLight != null)
				{
					this.GoLight.transform.position = this.hit.point + new Vector3(0f, this.LightHeightOffset, 0f);
				}
				if (this.Particles != null)
				{
					this.Particles.transform.position = this.hit.point + new Vector3(0f, this.ParticlesHeightOffset, 0f);
				}
				if (this.Explosion != null)
				{
					this.Explosion.transform.position = this.hit.point + new Vector3(0f, this.ParticlesHeightOffset, 0f);
				}
			}
		}
		else
		{
			if (this.effectSettings.Target != null)
			{
				this.tTarget = this.effectSettings.Target.transform;
			}
			else if (!this.effectSettings.UseMoveVector)
			{
				Debug.Log("You must setup the the target or the motion vector");
			}
			Vector3 vector;
			if (!this.effectSettings.UseMoveVector)
			{
				vector = (this.tTarget.position - this.tRoot.position).normalized;
			}
			else
			{
				vector = this.tRoot.position + this.effectSettings.MoveVector * this.effectSettings.MoveDistance;
			}
			Vector3 vector2 = this.tRoot.position + vector * this.effectSettings.MoveDistance;
			if (Physics.Raycast(this.tRoot.position, vector, ref this.hit, this.effectSettings.MoveDistance + 1f, this.effectSettings.LayerMask))
			{
				vector2 = (this.tRoot.position + Vector3.Normalize(this.hit.point - this.tRoot.position) * (this.effectSettings.MoveDistance + 1f)).normalized;
			}
			this.line.SetPosition(1, this.hit.point - this.effectSettings.ColliderRadius * vector2);
			Vector3 position = this.hit.point - vector2 * this.ParticlesHeightOffset;
			if (this.StartGlow != null)
			{
				this.StartGlow.transform.position = this.tRoot.position;
			}
			if (this.HitGlow != null)
			{
				this.HitGlow.transform.position = position;
			}
			if (this.GoLight != null)
			{
				this.GoLight.transform.position = this.hit.point - vector2 * this.LightHeightOffset;
			}
			if (this.Particles != null)
			{
				this.Particles.transform.position = position;
			}
			if (this.Explosion != null)
			{
				this.Explosion.transform.position = position;
			}
		}
		CollisionInfo e = new CollisionInfo
		{
			Hit = this.hit
		};
		this.effectSettings.OnCollisionHandler(e);
		if (this.hit.transform != null)
		{
			ShieldCollisionBehaviour component = this.hit.transform.GetComponent<ShieldCollisionBehaviour>();
			if (component != null)
			{
				component.ShieldCollisionEnter(e);
			}
		}
	}

	// Token: 0x06002515 RID: 9493 RVA: 0x00018924 File Offset: 0x00016B24
	private void OnEnable()
	{
		if (this.isInitializedOnStart)
		{
			this.InitializeDefault();
		}
	}

	// Token: 0x04002D3F RID: 11583
	public bool IsVertical;

	// Token: 0x04002D40 RID: 11584
	public float LightHeightOffset = 0.3f;

	// Token: 0x04002D41 RID: 11585
	public float ParticlesHeightOffset = 0.2f;

	// Token: 0x04002D42 RID: 11586
	public float TimeDestroyLightAfterCollision = 4f;

	// Token: 0x04002D43 RID: 11587
	public float TimeDestroyThisAfterCollision = 4f;

	// Token: 0x04002D44 RID: 11588
	public float TimeDestroyRootAfterCollision = 4f;

	// Token: 0x04002D45 RID: 11589
	public GameObject EffectOnHitObject;

	// Token: 0x04002D46 RID: 11590
	public GameObject Explosion;

	// Token: 0x04002D47 RID: 11591
	public GameObject StartGlow;

	// Token: 0x04002D48 RID: 11592
	public GameObject HitGlow;

	// Token: 0x04002D49 RID: 11593
	public GameObject Particles;

	// Token: 0x04002D4A RID: 11594
	public GameObject GoLight;

	// Token: 0x04002D4B RID: 11595
	private EffectSettings effectSettings;

	// Token: 0x04002D4C RID: 11596
	private Transform tRoot;

	// Token: 0x04002D4D RID: 11597
	private Transform tTarget;

	// Token: 0x04002D4E RID: 11598
	private bool isInitializedOnStart;

	// Token: 0x04002D4F RID: 11599
	private LineRenderer line;

	// Token: 0x04002D50 RID: 11600
	private int currentShaderIndex;

	// Token: 0x04002D51 RID: 11601
	private RaycastHit hit;
}
