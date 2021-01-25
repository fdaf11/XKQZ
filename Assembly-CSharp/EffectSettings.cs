using System;
using UnityEngine;

// Token: 0x020005DB RID: 1499
public class EffectSettings : MonoBehaviour
{
	// Token: 0x1400003E RID: 62
	// (add) Token: 0x0600252C RID: 9516 RVA: 0x00018A45 File Offset: 0x00016C45
	// (remove) Token: 0x0600252D RID: 9517 RVA: 0x00018A5E File Offset: 0x00016C5E
	public event EventHandler<CollisionInfo> CollisionEnter;

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x0600252E RID: 9518 RVA: 0x00018A77 File Offset: 0x00016C77
	// (remove) Token: 0x0600252F RID: 9519 RVA: 0x00018A90 File Offset: 0x00016C90
	public event EventHandler EffectDeactivated;

	// Token: 0x06002530 RID: 9520 RVA: 0x00018AA9 File Offset: 0x00016CA9
	private void Start()
	{
		if (this.InstanceBehaviour == EffectSettings.DeactivationEnum.DestroyAfterTime)
		{
			Object.Destroy(base.gameObject, this.DestroyTimeDelay);
		}
	}

	// Token: 0x06002531 RID: 9521 RVA: 0x00121E70 File Offset: 0x00120070
	public void OnCollisionHandler(CollisionInfo e)
	{
		for (int i = 0; i < this.lastActiveIndex; i++)
		{
			base.Invoke("SetGoActive", this.active_value[i]);
		}
		for (int j = 0; j < this.lastInactiveIndex; j++)
		{
			base.Invoke("SetGoInactive", this.inactive_value[j]);
		}
		EventHandler<CollisionInfo> collisionEnter = this.CollisionEnter;
		if (collisionEnter != null)
		{
			collisionEnter.Invoke(this, e);
		}
		if (this.InstanceBehaviour == EffectSettings.DeactivationEnum.Deactivate && !this.deactivatedIsWait)
		{
			this.deactivatedIsWait = true;
			base.Invoke("Deactivate", this.DeactivateTimeDelay);
		}
		if (this.InstanceBehaviour == EffectSettings.DeactivationEnum.DestroyAfterCollision)
		{
			Object.Destroy(base.gameObject, this.DestroyTimeDelay);
		}
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x00121F34 File Offset: 0x00120134
	public void OnEffectDeactivatedHandler()
	{
		EventHandler effectDeactivated = this.EffectDeactivated;
		if (effectDeactivated != null)
		{
			effectDeactivated.Invoke(this, EventArgs.Empty);
		}
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x00018AC8 File Offset: 0x00016CC8
	public void Deactivate()
	{
		this.OnEffectDeactivatedHandler();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x00018ADC File Offset: 0x00016CDC
	private void SetGoActive()
	{
		this.active_key[this.currentActiveGo].SetActive(false);
		this.currentActiveGo++;
		if (this.currentActiveGo >= this.lastActiveIndex)
		{
			this.currentActiveGo = 0;
		}
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x00018B17 File Offset: 0x00016D17
	private void SetGoInactive()
	{
		this.inactive_Key[this.currentInactiveGo].SetActive(true);
		this.currentInactiveGo++;
		if (this.currentInactiveGo >= this.lastInactiveIndex)
		{
			this.currentInactiveGo = 0;
		}
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x00121F5C File Offset: 0x0012015C
	public void OnEnable()
	{
		for (int i = 0; i < this.lastActiveIndex; i++)
		{
			this.active_key[i].SetActive(true);
		}
		for (int j = 0; j < this.lastInactiveIndex; j++)
		{
			this.inactive_Key[j].SetActive(false);
		}
		this.deactivatedIsWait = false;
	}

	// Token: 0x06002537 RID: 9527 RVA: 0x00018B52 File Offset: 0x00016D52
	public void OnDisable()
	{
		base.CancelInvoke("SetGoActive");
		base.CancelInvoke("SetGoInactive");
		base.CancelInvoke("Deactivate");
		this.currentActiveGo = 0;
		this.currentInactiveGo = 0;
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x00018B83 File Offset: 0x00016D83
	public void RegistreActiveElement(GameObject go, float time)
	{
		this.active_key[this.lastActiveIndex] = go;
		this.active_value[this.lastActiveIndex] = time;
		this.lastActiveIndex++;
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x00018BAF File Offset: 0x00016DAF
	public void RegistreInactiveElement(GameObject go, float time)
	{
		this.inactive_Key[this.lastInactiveIndex] = go;
		this.inactive_value[this.lastInactiveIndex] = time;
		this.lastInactiveIndex++;
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x00018BDB File Offset: 0x00016DDB
	public void SetTarget(GameObject go)
	{
		this.Target = go;
	}

	// Token: 0x04002D68 RID: 11624
	[Tooltip("Type of the effect")]
	public EffectSettings.EffectTypeEnum EffectType;

	// Token: 0x04002D69 RID: 11625
	[Tooltip("The radius of the collider is required to correctly calculate the collision point. For example, if the radius 0.5m, then the position of the collision is shifted on 0.5m relative motion vector.")]
	public float ColliderRadius = 0.2f;

	// Token: 0x04002D6A RID: 11626
	[Tooltip("The radius of the \"Area Of Damage (AOE)\"")]
	public float EffectRadius;

	// Token: 0x04002D6B RID: 11627
	[Tooltip("Get the position of the movement of the motion vector, and not to follow to the target.")]
	public bool UseMoveVector;

	// Token: 0x04002D6C RID: 11628
	[Tooltip("A projectile will be moved to the target (any object)")]
	public GameObject Target;

	// Token: 0x04002D6D RID: 11629
	[Tooltip("Motion vector for the projectile (eg Vector3.Forward)")]
	public Vector3 MoveVector = Vector3.forward;

	// Token: 0x04002D6E RID: 11630
	[Tooltip("The speed of the projectile")]
	public float MoveSpeed = 1f;

	// Token: 0x04002D6F RID: 11631
	[Tooltip("Should the projectile have move to the target, until the target not reaches?")]
	public bool IsHomingMove;

	// Token: 0x04002D70 RID: 11632
	[Tooltip("Distance flight of the projectile, after which the projectile is deactivated and call a collision event with a null value \"RaycastHit\"")]
	public float MoveDistance = 20f;

	// Token: 0x04002D71 RID: 11633
	[Tooltip("Allows you to smoothly activate / deactivate effects which have an indefinite lifetime")]
	public bool IsVisible = true;

	// Token: 0x04002D72 RID: 11634
	[Tooltip("Whether to deactivate or destroy the effect after a collision. Deactivation allows you to reuse the effect without instantiating, using \"effect.SetActive (true)\"")]
	public EffectSettings.DeactivationEnum InstanceBehaviour = EffectSettings.DeactivationEnum.Nothing;

	// Token: 0x04002D73 RID: 11635
	[Tooltip("Delay before deactivating effect. (For example, after effect, some particles must have time to disappear).")]
	public float DeactivateTimeDelay = 4f;

	// Token: 0x04002D74 RID: 11636
	[Tooltip("Delay before deleting effect. (For example, after effect, some particles must have time to disappear).")]
	public float DestroyTimeDelay = 10f;

	// Token: 0x04002D75 RID: 11637
	[Tooltip("Allows you to adjust the layers, which can interact with the projectile.")]
	public LayerMask LayerMask = -1;

	// Token: 0x04002D76 RID: 11638
	private GameObject[] active_key = new GameObject[100];

	// Token: 0x04002D77 RID: 11639
	private float[] active_value = new float[100];

	// Token: 0x04002D78 RID: 11640
	private GameObject[] inactive_Key = new GameObject[100];

	// Token: 0x04002D79 RID: 11641
	private float[] inactive_value = new float[100];

	// Token: 0x04002D7A RID: 11642
	private int lastActiveIndex;

	// Token: 0x04002D7B RID: 11643
	private int lastInactiveIndex;

	// Token: 0x04002D7C RID: 11644
	private int currentActiveGo;

	// Token: 0x04002D7D RID: 11645
	private int currentInactiveGo;

	// Token: 0x04002D7E RID: 11646
	private bool deactivatedIsWait;

	// Token: 0x020005DC RID: 1500
	public enum EffectTypeEnum
	{
		// Token: 0x04002D82 RID: 11650
		Projectile,
		// Token: 0x04002D83 RID: 11651
		AOE,
		// Token: 0x04002D84 RID: 11652
		Other
	}

	// Token: 0x020005DD RID: 1501
	public enum DeactivationEnum
	{
		// Token: 0x04002D86 RID: 11654
		Deactivate,
		// Token: 0x04002D87 RID: 11655
		DestroyAfterCollision,
		// Token: 0x04002D88 RID: 11656
		DestroyAfterTime,
		// Token: 0x04002D89 RID: 11657
		Nothing
	}
}
