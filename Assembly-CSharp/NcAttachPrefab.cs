using System;
using UnityEngine;

// Token: 0x020003BF RID: 959
public class NcAttachPrefab : NcEffectBehaviour
{
	// Token: 0x060016CC RID: 5836 RVA: 0x000BCDB8 File Offset: 0x000BAFB8
	public override int GetAnimationState()
	{
		if (this.m_bEnabled && base.enabled && NcEffectBehaviour.IsActive(base.gameObject) && this.m_AttachPrefab != null)
		{
			if (this.m_AttachType == NcAttachPrefab.AttachType.Active && ((this.m_nRepeatCount == 0 && this.m_nCreateCount < 1) || (0f < this.m_fRepeatTime && this.m_nRepeatCount == 0) || (0 < this.m_nRepeatCount && this.m_nCreateCount < this.m_nRepeatCount)))
			{
				return 1;
			}
			if (this.m_AttachType == NcAttachPrefab.AttachType.Destroy)
			{
				return 1;
			}
		}
		return 0;
	}

	// Token: 0x060016CD RID: 5837 RVA: 0x0000EA50 File Offset: 0x0000CC50
	public void UpdateImmediately()
	{
		this.Update();
	}

	// Token: 0x060016CE RID: 5838 RVA: 0x0000EA58 File Offset: 0x0000CC58
	public void CreateAttachInstance()
	{
		this.CreateAttachGameObject();
	}

	// Token: 0x060016CF RID: 5839 RVA: 0x0000EA60 File Offset: 0x0000CC60
	public GameObject GetInstanceObject()
	{
		if (this.m_CreateGameObjects == null)
		{
			this.UpdateImmediately();
		}
		return (this.m_CreateGameObjects != null && this.m_CreateGameObjects.Length >= 1) ? this.m_CreateGameObjects[0] : null;
	}

	// Token: 0x060016D0 RID: 5840 RVA: 0x0000EA9A File Offset: 0x0000CC9A
	public virtual GameObject[] GetInstanceObjects()
	{
		if (this.m_CreateGameObjects == null)
		{
			this.UpdateImmediately();
		}
		return this.m_CreateGameObjects;
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x0000EAB3 File Offset: 0x0000CCB3
	public void SetEnable(bool bEnable)
	{
		this.m_bEnabled = bEnable;
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x0000EABC File Offset: 0x0000CCBC
	protected virtual void Awake()
	{
		this.m_bEnabled = (base.enabled && NcEffectBehaviour.IsActive(base.gameObject) && base.GetComponent<NcDontActive>() == null);
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x0000264F File Offset: 0x0000084F
	protected virtual void Start()
	{
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x000BCE68 File Offset: 0x000BB068
	protected virtual void Update()
	{
		if (this.m_AttachPrefab == null)
		{
			return;
		}
		if (this.m_AttachType == NcAttachPrefab.AttachType.Active)
		{
			if (!this.m_bStartAttach)
			{
				this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
				this.m_bStartAttach = true;
			}
			if (this.m_fStartTime + this.m_fDelayTime <= NcEffectBehaviour.GetEngineTime())
			{
				this.CreateAttachPrefab();
				if ((0f < this.m_fRepeatTime && this.m_nRepeatCount == 0) || this.m_nCreateCount < this.m_nRepeatCount)
				{
					this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
					this.m_fDelayTime = this.m_fRepeatTime;
				}
				else
				{
					base.enabled = false;
				}
			}
		}
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x0000EAEE File Offset: 0x0000CCEE
	protected override void OnDestroy()
	{
		if (this.m_bEnabled && NcEffectBehaviour.IsSafe() && this.m_AttachType == NcAttachPrefab.AttachType.Destroy && this.m_AttachPrefab != null)
		{
			this.CreateAttachPrefab();
		}
		base.OnDestroy();
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x000BCF1C File Offset: 0x000BB11C
	private void CreateAttachPrefab()
	{
		this.CreateAttachGameObject();
		if ((this.m_fRepeatTime == 0f || this.m_AttachType == NcAttachPrefab.AttachType.Destroy) && 0 < this.m_nRepeatCount && this.m_nCreateCount < this.m_nRepeatCount)
		{
			this.CreateAttachPrefab();
		}
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x000BCF70 File Offset: 0x000BB170
	private void CreateAttachGameObject()
	{
		GameObject gameObject = base.CreateGameObject(this.GetTargetGameObject(), (!(this.GetTargetGameObject() == base.gameObject)) ? base.transform : null, this.m_AttachPrefab);
		if (this.m_bReplayState)
		{
			NsEffectManager.SetReplayEffect(gameObject);
		}
		if (gameObject == null)
		{
			return;
		}
		if (this.m_AttachType == NcAttachPrefab.AttachType.Active)
		{
			if (this.m_CreateGameObjects == null)
			{
				this.m_CreateGameObjects = new GameObject[Mathf.Max(1, this.m_nRepeatCount)];
			}
			for (int i = 0; i < this.m_CreateGameObjects.Length; i++)
			{
				if (this.m_CreateGameObjects[i] == null)
				{
					this.m_CreateGameObjects[i] = gameObject;
					break;
				}
			}
		}
		this.m_nCreateCount++;
		Vector3 position = gameObject.transform.position;
		gameObject.transform.position = this.m_AddStartPos + new Vector3(Random.Range(-this.m_RandomRange.x, this.m_RandomRange.x) + position.x, Random.Range(-this.m_RandomRange.y, this.m_RandomRange.y) + position.y, Random.Range(-this.m_RandomRange.z, this.m_RandomRange.z) + position.z);
		gameObject.transform.localRotation *= Quaternion.Euler(this.m_AccumStartRot.x * (float)this.m_nCreateCount, this.m_AccumStartRot.y * (float)this.m_nCreateCount, this.m_AccumStartRot.z * (float)this.m_nCreateCount);
		GameObject gameObject2 = gameObject;
		gameObject2.name = gameObject2.name + " " + this.m_nCreateCount;
		NcEffectBehaviour.SetActiveRecursively(gameObject, true);
		NsEffectManager.AdjustSpeedRuntime(gameObject, this.m_fPrefabSpeed);
		if (0f < this.m_fPrefabLifeTime)
		{
			NcAutoDestruct ncAutoDestruct = gameObject.GetComponent<NcAutoDestruct>();
			if (ncAutoDestruct == null)
			{
				ncAutoDestruct = base.AddNcComponentToObject<NcAutoDestruct>(gameObject);
			}
			ncAutoDestruct.m_fLifeTime = this.m_fPrefabLifeTime;
		}
		if (this.m_bDetachParent)
		{
			NcDetachParent ncDetachParent = gameObject.GetComponent<NcDetachParent>();
			if (ncDetachParent == null)
			{
				ncDetachParent = base.AddNcComponentToObject<NcDetachParent>(gameObject);
			}
		}
		if (0 <= this.m_nSpriteFactoryIndex)
		{
			NcSpriteFactory component = gameObject.GetComponent<NcSpriteFactory>();
			if (component)
			{
				component.SetSprite(this.m_nSpriteFactoryIndex, false);
			}
		}
		this.OnCreateAttachGameObject();
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x0000264F File Offset: 0x0000084F
	protected virtual void OnCreateAttachGameObject()
	{
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x0000EB2E File Offset: 0x0000CD2E
	private GameObject GetTargetGameObject()
	{
		if (this.m_bWorldSpace || this.m_AttachType == NcAttachPrefab.AttachType.Destroy)
		{
			return NcEffectBehaviour.GetRootInstanceEffect();
		}
		return base.gameObject;
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x0000EB53 File Offset: 0x0000CD53
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fDelayTime /= fSpeedRate;
		this.m_fRepeatTime /= fSpeedRate;
		this.m_fPrefabLifeTime /= fSpeedRate;
		this.m_fPrefabSpeed *= fSpeedRate;
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x000BD1F8 File Offset: 0x000BB3F8
	public override void OnSetActiveRecursively(bool bActive)
	{
		if (this.m_CreateGameObjects == null)
		{
			return;
		}
		for (int i = 0; i < this.m_CreateGameObjects.Length; i++)
		{
			if (this.m_CreateGameObjects[i] != null)
			{
				NsEffectManager.SetActiveRecursively(this.m_CreateGameObjects[i], bActive);
			}
		}
	}

	// Token: 0x060016DC RID: 5852 RVA: 0x000BD24C File Offset: 0x000BB44C
	public static void Ng_ChangeLayerWithChild(GameObject rootObj, int nLayer)
	{
		if (rootObj == null)
		{
			return;
		}
		rootObj.layer = nLayer;
		for (int i = 0; i < rootObj.transform.childCount; i++)
		{
			NcAttachPrefab.Ng_ChangeLayerWithChild(rootObj.transform.GetChild(i).gameObject, nLayer);
		}
	}

	// Token: 0x04001AFD RID: 6909
	public NcAttachPrefab.AttachType m_AttachType;

	// Token: 0x04001AFE RID: 6910
	public float m_fDelayTime;

	// Token: 0x04001AFF RID: 6911
	public float m_fRepeatTime;

	// Token: 0x04001B00 RID: 6912
	public int m_nRepeatCount;

	// Token: 0x04001B01 RID: 6913
	public GameObject m_AttachPrefab;

	// Token: 0x04001B02 RID: 6914
	public float m_fPrefabSpeed = 1f;

	// Token: 0x04001B03 RID: 6915
	public float m_fPrefabLifeTime;

	// Token: 0x04001B04 RID: 6916
	public bool m_bWorldSpace;

	// Token: 0x04001B05 RID: 6917
	public Vector3 m_AddStartPos = Vector3.zero;

	// Token: 0x04001B06 RID: 6918
	public Vector3 m_AccumStartRot = Vector3.zero;

	// Token: 0x04001B07 RID: 6919
	public Vector3 m_RandomRange = Vector3.zero;

	// Token: 0x04001B08 RID: 6920
	public int m_nSpriteFactoryIndex = -1;

	// Token: 0x04001B09 RID: 6921
	[HideInInspector]
	public bool m_bDetachParent;

	// Token: 0x04001B0A RID: 6922
	protected float m_fStartTime;

	// Token: 0x04001B0B RID: 6923
	protected int m_nCreateCount;

	// Token: 0x04001B0C RID: 6924
	protected bool m_bStartAttach;

	// Token: 0x04001B0D RID: 6925
	protected GameObject[] m_CreateGameObjects;

	// Token: 0x04001B0E RID: 6926
	protected bool m_bEnabled;

	// Token: 0x020003C0 RID: 960
	public enum AttachType
	{
		// Token: 0x04001B10 RID: 6928
		Active,
		// Token: 0x04001B11 RID: 6929
		Destroy
	}
}
