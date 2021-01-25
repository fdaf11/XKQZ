using System;
using UnityEngine;

// Token: 0x020005CE RID: 1486
public class ProjectileCollisionBehaviour : MonoBehaviour
{
	// Token: 0x060024E9 RID: 9449 RVA: 0x0011FF1C File Offset: 0x0011E11C
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

	// Token: 0x060024EA RID: 9450 RVA: 0x0011FF68 File Offset: 0x0011E168
	private void Start()
	{
		this.t = base.transform;
		this.GetEffectSettingsComponent(this.t);
		if (this.effectSettings == null)
		{
			Debug.Log("Prefab root or children have not script \"PrefabSettings\"");
		}
		if (!this.IsRootMove)
		{
			this.startParentPosition = base.transform.parent.position;
		}
		if (this.GoLight != null)
		{
			this.tLight = this.GoLight.transform;
		}
		this.InitializeDefault();
		this.isInitializedOnStart = true;
	}

	// Token: 0x060024EB RID: 9451 RVA: 0x00018788 File Offset: 0x00016988
	private void OnEnable()
	{
		if (this.isInitializedOnStart)
		{
			this.InitializeDefault();
		}
	}

	// Token: 0x060024EC RID: 9452 RVA: 0x0001879B File Offset: 0x0001699B
	private void OnDisable()
	{
		if (this.ResetParentPositionOnDisable && this.isInitializedOnStart && !this.IsRootMove)
		{
			base.transform.parent.position = this.startParentPosition;
		}
	}

	// Token: 0x060024ED RID: 9453 RVA: 0x0011FFF8 File Offset: 0x0011E1F8
	private void InitializeDefault()
	{
		this.hit = default(RaycastHit);
		this.onCollision = false;
		this.smootRandomPos = default(Vector3);
		this.oldSmootRandomPos = default(Vector3);
		this.deltaSpeed = 0f;
		this.startTime = 0f;
		this.randomSpeed = 0f;
		this.randomRadiusX = 0f;
		this.randomRadiusY = 0f;
		this.randomDirection1 = 0;
		this.randomDirection2 = 0;
		this.randomDirection3 = 0;
		this.frameDroped = false;
		this.tRoot = ((!this.IsRootMove) ? base.transform.parent : this.effectSettings.transform);
		this.startPosition = this.tRoot.position;
		if (this.effectSettings == null)
		{
			return;
		}
		if (this.effectSettings.Target != null)
		{
			this.tTarget = this.effectSettings.Target.transform;
		}
		else if (!this.effectSettings.UseMoveVector)
		{
			Debug.Log("You must setup the the target or the motion vector");
		}
		if ((double)this.effectSettings.EffectRadius > 0.001)
		{
			Vector2 vector = Random.insideUnitCircle * this.effectSettings.EffectRadius;
			this.randomTargetOffsetXZVector = new Vector3(vector.x, 0f, vector.y);
		}
		else
		{
			this.randomTargetOffsetXZVector = Vector3.zero;
		}
		if (!this.effectSettings.UseMoveVector)
		{
			this.forwardDirection = this.tRoot.position + (this.tTarget.position + this.randomTargetOffsetXZVector - this.tRoot.position).normalized * this.effectSettings.MoveDistance;
			this.GetTargetHit();
		}
		else
		{
			this.forwardDirection = this.tRoot.position + this.effectSettings.MoveVector * this.effectSettings.MoveDistance;
		}
		if (this.IsLookAt)
		{
			if (!this.effectSettings.UseMoveVector)
			{
				this.tRoot.LookAt(this.tTarget);
			}
			else
			{
				this.tRoot.LookAt(this.forwardDirection);
			}
		}
		this.InitRandomVariables();
	}

	// Token: 0x060024EE RID: 9454 RVA: 0x00120270 File Offset: 0x0011E470
	private void Update()
	{
		if (!this.frameDroped)
		{
			this.frameDroped = true;
			return;
		}
		if (this.effectSettings == null)
		{
			return;
		}
		if (((!this.effectSettings.UseMoveVector && this.tTarget == null) || this.onCollision) && this.frameDroped)
		{
			return;
		}
		Vector3 vector;
		if (!this.effectSettings.UseMoveVector)
		{
			vector = ((!this.effectSettings.IsHomingMove) ? this.forwardDirection : this.tTarget.position);
		}
		else
		{
			vector = this.forwardDirection;
		}
		float num = Vector3.Distance(this.tRoot.position, vector);
		float num2 = this.effectSettings.MoveSpeed * Time.deltaTime;
		if (num2 > num)
		{
			num2 = num;
		}
		if (num <= this.effectSettings.ColliderRadius)
		{
			this.hit = default(RaycastHit);
			this.CollisionEnter();
		}
		Vector3 normalized = (vector - this.tRoot.position).normalized;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.tRoot.position, normalized, ref raycastHit, num2 + this.effectSettings.ColliderRadius, this.effectSettings.LayerMask))
		{
			this.hit = raycastHit;
			vector = raycastHit.point - normalized * this.effectSettings.ColliderRadius;
			this.CollisionEnter();
		}
		if (this.IsCenterLightPosition && this.GoLight != null)
		{
			this.tLight.position = (this.startPosition + this.tRoot.position) / 2f;
		}
		Vector3 vector2 = default(Vector3);
		if (this.RandomMoveCoordinates != RandomMoveCoordinates.None)
		{
			this.UpdateSmootRandomhPos();
			vector2 = this.smootRandomPos - this.oldSmootRandomPos;
		}
		float num3 = 1f;
		if (this.Acceleration.length > 0)
		{
			float num4 = (Time.time - this.startTime) / this.AcceleraionTime;
			num3 = this.Acceleration.Evaluate(num4);
		}
		Vector3 vector3 = Vector3.MoveTowards(this.tRoot.position, vector, this.effectSettings.MoveSpeed * Time.deltaTime * num3);
		Vector3 vector4 = vector3 + vector2;
		if (this.IsLookAt && this.effectSettings.IsHomingMove)
		{
			this.tRoot.LookAt(vector4);
		}
		if (this.IsLocalSpaceRandomMove && this.IsRootMove)
		{
			this.tRoot.position = vector3;
			this.t.localPosition += vector2;
		}
		else
		{
			this.tRoot.position = vector4;
		}
		this.oldSmootRandomPos = this.smootRandomPos;
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x00120550 File Offset: 0x0011E750
	private void CollisionEnter()
	{
		if (this.EffectOnHitObject != null && this.hit.transform != null)
		{
			Transform transform = this.hit.transform;
			Renderer componentInChildren = transform.GetComponentInChildren<Renderer>();
			GameObject gameObject = Object.Instantiate(this.EffectOnHitObject) as GameObject;
			gameObject.transform.parent = componentInChildren.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.GetComponent<AddMaterialOnHit>().UpdateMaterial(this.hit);
		}
		if (this.AttachAfterCollision)
		{
			this.tRoot.parent = this.hit.transform;
		}
		if (this.SendCollisionMessage)
		{
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
		this.onCollision = true;
	}

	// Token: 0x060024F0 RID: 9456 RVA: 0x00120670 File Offset: 0x0011E870
	private void InitRandomVariables()
	{
		this.deltaSpeed = this.RandomMoveSpeed * Random.Range(1f, 1000f * this.RandomRange + 1f) / 1000f - 1f;
		this.startTime = Time.time;
		this.randomRadiusX = Random.Range(this.RandomMoveRadius / 20f, this.RandomMoveRadius * 100f) / 100f;
		this.randomRadiusY = Random.Range(this.RandomMoveRadius / 20f, this.RandomMoveRadius * 100f) / 100f;
		this.randomSpeed = Random.Range(this.RandomMoveSpeed / 20f, this.RandomMoveSpeed * 100f) / 100f;
		this.randomDirection1 = ((Random.Range(0, 2) <= 0) ? -1 : 1);
		this.randomDirection2 = ((Random.Range(0, 2) <= 0) ? -1 : 1);
		this.randomDirection3 = ((Random.Range(0, 2) <= 0) ? -1 : 1);
	}

	// Token: 0x060024F1 RID: 9457 RVA: 0x00120788 File Offset: 0x0011E988
	private void GetTargetHit()
	{
		Ray ray;
		ray..ctor(this.tRoot.position, Vector3.Normalize(this.tTarget.position + this.randomTargetOffsetXZVector - this.tRoot.position));
		Collider componentInChildren = this.tTarget.GetComponentInChildren<Collider>();
		RaycastHit raycastHit;
		if (componentInChildren != null && componentInChildren.Raycast(ray, ref raycastHit, this.effectSettings.MoveDistance))
		{
			this.hit = raycastHit;
		}
	}

	// Token: 0x060024F2 RID: 9458 RVA: 0x0012080C File Offset: 0x0011EA0C
	private void UpdateSmootRandomhPos()
	{
		float num = Time.time - this.startTime;
		float num2 = num * this.randomSpeed;
		float num3 = num * this.deltaSpeed;
		float num5;
		float num6;
		if (this.IsDeviation)
		{
			float num4 = Vector3.Distance(this.tRoot.position, this.hit.point) / this.effectSettings.MoveDistance;
			num5 = (float)this.randomDirection2 * Mathf.Sin(num2) * this.randomRadiusX * num4;
			num6 = (float)this.randomDirection3 * Mathf.Sin(num2 + (float)this.randomDirection1 * 3.1415927f / 2f * num + Mathf.Sin(num3)) * this.randomRadiusY * num4;
		}
		else
		{
			num5 = (float)this.randomDirection2 * Mathf.Sin(num2) * this.randomRadiusX;
			num6 = (float)this.randomDirection3 * Mathf.Sin(num2 + (float)this.randomDirection1 * 3.1415927f / 2f * num + Mathf.Sin(num3)) * this.randomRadiusY;
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.XY)
		{
			this.smootRandomPos = new Vector3(num5, num6, 0f);
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.XZ)
		{
			this.smootRandomPos = new Vector3(num5, 0f, num6);
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.YZ)
		{
			this.smootRandomPos = new Vector3(0f, num5, num6);
		}
		if (this.RandomMoveCoordinates == RandomMoveCoordinates.XYZ)
		{
			this.smootRandomPos = new Vector3(num5, num6, (num5 + num6) / 2f * (float)this.randomDirection1);
		}
	}

	// Token: 0x04002CF5 RID: 11509
	public float RandomMoveRadius;

	// Token: 0x04002CF6 RID: 11510
	public float RandomMoveSpeed;

	// Token: 0x04002CF7 RID: 11511
	public float RandomRange;

	// Token: 0x04002CF8 RID: 11512
	public RandomMoveCoordinates RandomMoveCoordinates;

	// Token: 0x04002CF9 RID: 11513
	public GameObject EffectOnHitObject;

	// Token: 0x04002CFA RID: 11514
	public GameObject GoLight;

	// Token: 0x04002CFB RID: 11515
	public AnimationCurve Acceleration;

	// Token: 0x04002CFC RID: 11516
	public float AcceleraionTime = 1f;

	// Token: 0x04002CFD RID: 11517
	public bool IsCenterLightPosition;

	// Token: 0x04002CFE RID: 11518
	public bool IsLookAt;

	// Token: 0x04002CFF RID: 11519
	public bool AttachAfterCollision;

	// Token: 0x04002D00 RID: 11520
	public bool IsRootMove = true;

	// Token: 0x04002D01 RID: 11521
	public bool IsLocalSpaceRandomMove;

	// Token: 0x04002D02 RID: 11522
	public bool IsDeviation;

	// Token: 0x04002D03 RID: 11523
	public bool SendCollisionMessage = true;

	// Token: 0x04002D04 RID: 11524
	public bool ResetParentPositionOnDisable;

	// Token: 0x04002D05 RID: 11525
	private EffectSettings effectSettings;

	// Token: 0x04002D06 RID: 11526
	private Transform tRoot;

	// Token: 0x04002D07 RID: 11527
	private Transform tTarget;

	// Token: 0x04002D08 RID: 11528
	private Transform t;

	// Token: 0x04002D09 RID: 11529
	private Transform tLight;

	// Token: 0x04002D0A RID: 11530
	private Vector3 forwardDirection;

	// Token: 0x04002D0B RID: 11531
	private Vector3 startPosition;

	// Token: 0x04002D0C RID: 11532
	private Vector3 startParentPosition;

	// Token: 0x04002D0D RID: 11533
	private RaycastHit hit;

	// Token: 0x04002D0E RID: 11534
	private Vector3 smootRandomPos;

	// Token: 0x04002D0F RID: 11535
	private Vector3 oldSmootRandomPos;

	// Token: 0x04002D10 RID: 11536
	private float deltaSpeed;

	// Token: 0x04002D11 RID: 11537
	private float startTime;

	// Token: 0x04002D12 RID: 11538
	private float randomSpeed;

	// Token: 0x04002D13 RID: 11539
	private float randomRadiusX;

	// Token: 0x04002D14 RID: 11540
	private float randomRadiusY;

	// Token: 0x04002D15 RID: 11541
	private int randomDirection1;

	// Token: 0x04002D16 RID: 11542
	private int randomDirection2;

	// Token: 0x04002D17 RID: 11543
	private int randomDirection3;

	// Token: 0x04002D18 RID: 11544
	private bool onCollision;

	// Token: 0x04002D19 RID: 11545
	private bool isInitializedOnStart;

	// Token: 0x04002D1A RID: 11546
	private Vector3 randomTargetOffsetXZVector;

	// Token: 0x04002D1B RID: 11547
	private bool frameDroped;
}
