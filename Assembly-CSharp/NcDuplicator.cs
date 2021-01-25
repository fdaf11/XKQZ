using System;
using UnityEngine;

// Token: 0x020003D4 RID: 980
public class NcDuplicator : NcEffectBehaviour
{
	// Token: 0x0600174C RID: 5964 RVA: 0x000BFF40 File Offset: 0x000BE140
	public override int GetAnimationState()
	{
		if (base.enabled && NcEffectBehaviour.IsActive(base.gameObject) && (this.m_nDuplicateCount == 0 || (this.m_nDuplicateCount != 0 && this.m_nCreateCount < this.m_nDuplicateCount)))
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x0000F18D File Offset: 0x0000D38D
	public GameObject GetCloneObject()
	{
		return this.m_ClonObject;
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x000BFF94 File Offset: 0x000BE194
	private void Awake()
	{
		this.m_nCreateCount = 0;
		this.m_fStartTime = -this.m_fDuplicateTime;
		this.m_ClonObject = null;
		this.m_bInvoke = false;
		if (!base.enabled)
		{
			return;
		}
		if (base.transform.parent != null && base.enabled && NcEffectBehaviour.IsActive(base.gameObject) && base.GetComponent<NcDontActive>() == null)
		{
			this.InitCloneObject();
		}
	}

	// Token: 0x0600174F RID: 5967 RVA: 0x0000F195 File Offset: 0x0000D395
	protected override void OnDestroy()
	{
		if (this.m_ClonObject != null)
		{
			Object.Destroy(this.m_ClonObject);
		}
		base.OnDestroy();
	}

	// Token: 0x06001750 RID: 5968 RVA: 0x0000F1B9 File Offset: 0x0000D3B9
	private void Start()
	{
		if (this.m_bInvoke)
		{
			this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
			this.CreateCloneObject();
			base.InvokeRepeating("CreateCloneObject", this.m_fDuplicateTime, this.m_fDuplicateTime);
		}
	}

	// Token: 0x06001751 RID: 5969 RVA: 0x000C0018 File Offset: 0x000BE218
	private void Update()
	{
		if (this.m_bInvoke)
		{
			return;
		}
		if ((this.m_nDuplicateCount == 0 || this.m_nCreateCount < this.m_nDuplicateCount) && this.m_fStartTime + this.m_fDuplicateTime <= NcEffectBehaviour.GetEngineTime())
		{
			this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
			this.CreateCloneObject();
		}
	}

	// Token: 0x06001752 RID: 5970 RVA: 0x000C0078 File Offset: 0x000BE278
	private void InitCloneObject()
	{
		if (!(this.m_ClonObject == null))
		{
			return;
		}
		this.m_ClonObject = base.CreateGameObject(base.gameObject);
		if (this.m_ClonObject == null)
		{
			return;
		}
		NcEffectBehaviour.HideNcDelayActive(this.m_ClonObject);
		NcDuplicator component = this.m_ClonObject.GetComponent<NcDuplicator>();
		if (component != null)
		{
			Object.Destroy(component);
		}
		NcDelayActive component2 = this.m_ClonObject.GetComponent<NcDelayActive>();
		if (component2 != null)
		{
			Object.Destroy(component2);
		}
		Component[] components = base.transform.GetComponents<Component>();
		for (int i = 0; i < components.Length; i++)
		{
			if (!(components[i] is Transform) && !(components[i] is NcDuplicator))
			{
				Object.Destroy(components[i]);
			}
		}
		NcEffectBehaviour.RemoveAllChildObject(base.gameObject, false);
	}

	// Token: 0x06001753 RID: 5971 RVA: 0x000C0158 File Offset: 0x000BE358
	private void CreateCloneObject()
	{
		if (this.m_ClonObject == null)
		{
			return;
		}
		GameObject gameObject;
		if (base.transform.parent == null)
		{
			gameObject = base.CreateGameObject(base.gameObject);
		}
		else
		{
			gameObject = base.CreateGameObject(base.transform.parent.gameObject, this.m_ClonObject);
		}
		NcEffectBehaviour.SetActiveRecursively(gameObject, true);
		if (gameObject == null)
		{
			return;
		}
		if (0f < this.m_fDuplicateLifeTime)
		{
			NcAutoDestruct ncAutoDestruct = gameObject.GetComponent<NcAutoDestruct>();
			if (ncAutoDestruct == null)
			{
				ncAutoDestruct = gameObject.AddComponent<NcAutoDestruct>();
			}
			ncAutoDestruct.m_fLifeTime = this.m_fDuplicateLifeTime;
		}
		Vector3 position = gameObject.transform.position;
		gameObject.transform.position = new Vector3(Random.Range(-this.m_RandomRange.x, this.m_RandomRange.x) + position.x, Random.Range(-this.m_RandomRange.y, this.m_RandomRange.y) + position.y, Random.Range(-this.m_RandomRange.z, this.m_RandomRange.z) + position.z);
		gameObject.transform.position += this.m_AddStartPos;
		gameObject.transform.localRotation *= Quaternion.Euler(this.m_AccumStartRot.x * (float)this.m_nCreateCount, this.m_AccumStartRot.y * (float)this.m_nCreateCount, this.m_AccumStartRot.z * (float)this.m_nCreateCount);
		GameObject gameObject2 = gameObject;
		gameObject2.name = gameObject2.name + " " + this.m_nCreateCount;
		this.m_nCreateCount++;
		if (this.m_bInvoke && this.m_nDuplicateCount <= this.m_nCreateCount)
		{
			base.CancelInvoke("CreateCloneObject");
		}
	}

	// Token: 0x06001754 RID: 5972 RVA: 0x000C0354 File Offset: 0x000BE554
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fDuplicateTime /= fSpeedRate;
		this.m_fDuplicateLifeTime /= fSpeedRate;
		if (bRuntime && this.m_ClonObject != null)
		{
			NsEffectManager.AdjustSpeedRuntime(this.m_ClonObject, fSpeedRate);
		}
	}

	// Token: 0x04001BAE RID: 7086
	public float m_fDuplicateTime = 0.1f;

	// Token: 0x04001BAF RID: 7087
	public int m_nDuplicateCount = 3;

	// Token: 0x04001BB0 RID: 7088
	public float m_fDuplicateLifeTime;

	// Token: 0x04001BB1 RID: 7089
	public Vector3 m_AddStartPos = Vector3.zero;

	// Token: 0x04001BB2 RID: 7090
	public Vector3 m_AccumStartRot = Vector3.zero;

	// Token: 0x04001BB3 RID: 7091
	public Vector3 m_RandomRange = Vector3.zero;

	// Token: 0x04001BB4 RID: 7092
	protected int m_nCreateCount;

	// Token: 0x04001BB5 RID: 7093
	protected float m_fStartTime;

	// Token: 0x04001BB6 RID: 7094
	protected GameObject m_ClonObject;

	// Token: 0x04001BB7 RID: 7095
	protected bool m_bInvoke;
}
