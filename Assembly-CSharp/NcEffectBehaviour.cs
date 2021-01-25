using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003A7 RID: 935
public class NcEffectBehaviour : MonoBehaviour
{
	// Token: 0x060015B1 RID: 5553 RVA: 0x0000DCF4 File Offset: 0x0000BEF4
	public NcEffectBehaviour()
	{
		this.m_MeshFilter = null;
	}

	// Token: 0x060015B3 RID: 5555 RVA: 0x0000DD03 File Offset: 0x0000BF03
	public static float GetEngineTime()
	{
		if (Time.time == 0f)
		{
			return 1E-06f;
		}
		return Time.time;
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x0000679C File Offset: 0x0000499C
	public static float GetEngineDeltaTime()
	{
		return Time.deltaTime;
	}

	// Token: 0x060015B5 RID: 5557 RVA: 0x0000DD1F File Offset: 0x0000BF1F
	public virtual int GetAnimationState()
	{
		return -1;
	}

	// Token: 0x060015B6 RID: 5558 RVA: 0x000B9070 File Offset: 0x000B7270
	public static GameObject GetRootInstanceEffect()
	{
		if (!NcEffectBehaviour.IsSafe())
		{
			return null;
		}
		if (NcEffectBehaviour.m_RootInstance == null)
		{
			NcEffectBehaviour.m_RootInstance = GameObject.Find("_InstanceObject");
			if (NcEffectBehaviour.m_RootInstance == null)
			{
				NcEffectBehaviour.m_RootInstance = new GameObject("_InstanceObject");
			}
		}
		return NcEffectBehaviour.m_RootInstance;
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x0000DD22 File Offset: 0x0000BF22
	public static Texture[] PreloadTexture(GameObject tarObj)
	{
		return NsEffectManager.PreloadResource(tarObj);
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x0000DD2A File Offset: 0x0000BF2A
	protected static void SetActive(GameObject target, bool bActive)
	{
		if (target == null)
		{
			return;
		}
		target.SetActive(bActive);
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x000B90CC File Offset: 0x000B72CC
	protected static void SetActiveRecursively(GameObject target, bool bActive)
	{
		if (target == null)
		{
			return;
		}
		int num = target.transform.childCount - 1;
		while (0 <= num)
		{
			if (num < target.transform.childCount)
			{
				NcEffectBehaviour.SetActiveRecursively(target.transform.GetChild(num).gameObject, bActive);
			}
			num--;
		}
		target.SetActive(bActive);
	}

	// Token: 0x060015BA RID: 5562 RVA: 0x0000DD40 File Offset: 0x0000BF40
	protected static bool IsActive(GameObject target)
	{
		return !(target == null) && target.activeInHierarchy && target.activeSelf;
	}

	// Token: 0x060015BB RID: 5563 RVA: 0x000B9134 File Offset: 0x000B7334
	protected static void RemoveAllChildObject(GameObject parent, bool bImmediate)
	{
		int num = parent.transform.childCount - 1;
		while (0 <= num)
		{
			if (num < parent.transform.childCount)
			{
				Transform child = parent.transform.GetChild(num);
				if (bImmediate)
				{
					Object.DestroyImmediate(child.gameObject);
				}
				else
				{
					Object.Destroy(child.gameObject);
				}
			}
			num--;
		}
	}

	// Token: 0x060015BC RID: 5564 RVA: 0x0000DD64 File Offset: 0x0000BF64
	public static void HideNcDelayActive(GameObject tarObj)
	{
		NcEffectBehaviour.SetActiveRecursively(tarObj, false);
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x0000DD6D File Offset: 0x0000BF6D
	protected void AddRuntimeMaterial(Material addMaterial)
	{
		if (this.m_RuntimeMaterials == null)
		{
			this.m_RuntimeMaterials = new List<Material>();
		}
		if (!this.m_RuntimeMaterials.Contains(addMaterial))
		{
			this.m_RuntimeMaterials.Add(addMaterial);
		}
	}

	// Token: 0x060015BE RID: 5566 RVA: 0x000B91A0 File Offset: 0x000B73A0
	public static string GetMaterialColorName(Material mat)
	{
		string[] array = new string[]
		{
			"_Color",
			"_TintColor",
			"_EmisColor"
		};
		if (mat != null)
		{
			foreach (string text in array)
			{
				if (mat.HasProperty(text))
				{
					return text;
				}
			}
		}
		return null;
	}

	// Token: 0x060015BF RID: 5567 RVA: 0x000B9204 File Offset: 0x000B7404
	protected void DisableEmit()
	{
		NcParticleSystem[] componentsInChildren = base.gameObject.GetComponentsInChildren<NcParticleSystem>(true);
		foreach (NcParticleSystem ncParticleSystem in componentsInChildren)
		{
			if (ncParticleSystem != null)
			{
				ncParticleSystem.SetDisableEmit();
			}
		}
		NcAttachPrefab[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<NcAttachPrefab>(true);
		foreach (NcAttachPrefab ncAttachPrefab in componentsInChildren2)
		{
			if (ncAttachPrefab != null)
			{
				ncAttachPrefab.enabled = false;
			}
		}
		ParticleSystem[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
		foreach (ParticleSystem particleSystem in componentsInChildren3)
		{
			if (particleSystem != null)
			{
				particleSystem.enableEmission = false;
			}
		}
		ParticleEmitter[] componentsInChildren4 = base.gameObject.GetComponentsInChildren<ParticleEmitter>(true);
		foreach (ParticleEmitter particleEmitter in componentsInChildren4)
		{
			if (particleEmitter != null)
			{
				particleEmitter.emit = false;
			}
		}
	}

	// Token: 0x060015C0 RID: 5568 RVA: 0x0000DDA2 File Offset: 0x0000BFA2
	public static bool IsSafe()
	{
		return !NcEffectBehaviour.m_bShuttingDown && !Application.isLoadingLevel;
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x0000DDB9 File Offset: 0x0000BFB9
	protected GameObject CreateEditorGameObject(GameObject srcGameObj)
	{
		if (srcGameObj.name.Contains("flare 24"))
		{
			return srcGameObj;
		}
		return srcGameObj;
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x0000DDD3 File Offset: 0x0000BFD3
	public GameObject CreateGameObject(string name)
	{
		if (!NcEffectBehaviour.IsSafe())
		{
			return null;
		}
		return this.CreateEditorGameObject(new GameObject(name));
	}

	// Token: 0x060015C3 RID: 5571 RVA: 0x0000DDED File Offset: 0x0000BFED
	public GameObject CreateGameObject(GameObject original)
	{
		if (!NcEffectBehaviour.IsSafe())
		{
			return null;
		}
		return this.CreateEditorGameObject((GameObject)Object.Instantiate(original));
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x0000DE0C File Offset: 0x0000C00C
	public GameObject CreateGameObject(GameObject prefabObj, Vector3 position, Quaternion rotation)
	{
		if (!NcEffectBehaviour.IsSafe())
		{
			return null;
		}
		return this.CreateEditorGameObject((GameObject)Object.Instantiate(prefabObj, position, rotation));
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x000B9320 File Offset: 0x000B7520
	public GameObject CreateGameObject(GameObject parentObj, GameObject prefabObj)
	{
		if (!NcEffectBehaviour.IsSafe())
		{
			return null;
		}
		GameObject gameObject = this.CreateGameObject(prefabObj);
		if (parentObj != null)
		{
			this.ChangeParent(parentObj.transform, gameObject.transform, true, null);
		}
		return gameObject;
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x000B9364 File Offset: 0x000B7564
	public GameObject CreateGameObject(GameObject parentObj, Transform parentTrans, GameObject prefabObj)
	{
		if (!NcEffectBehaviour.IsSafe())
		{
			return null;
		}
		GameObject gameObject = this.CreateGameObject(prefabObj);
		if (parentObj != null)
		{
			this.ChangeParent(parentObj.transform, gameObject.transform, true, parentTrans);
		}
		return gameObject;
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x000B93A8 File Offset: 0x000B75A8
	protected TT AddNcComponentToObject<TT>(GameObject toObj) where TT : NcEffectBehaviour
	{
		NcEffectBehaviour ncEffectBehaviour = toObj.AddComponent<TT>();
		if (this.m_bReplayState)
		{
			ncEffectBehaviour.OnSetReplayState();
		}
		return (TT)((object)ncEffectBehaviour);
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x000B93D8 File Offset: 0x000B75D8
	protected void ChangeParent(Transform newParent, Transform child, bool bKeepingLocalTransform, Transform addTransform)
	{
		NcTransformTool ncTransformTool = null;
		if (bKeepingLocalTransform)
		{
			ncTransformTool = new NcTransformTool(child.transform);
			if (addTransform != null)
			{
				ncTransformTool.AddTransform(addTransform);
			}
		}
		child.parent = newParent;
		if (bKeepingLocalTransform)
		{
			ncTransformTool.CopyToLocalTransform(child.transform);
		}
		if (bKeepingLocalTransform)
		{
			NcBillboard[] componentsInChildren = child.GetComponentsInChildren<NcBillboard>();
			foreach (NcBillboard ncBillboard in componentsInChildren)
			{
				ncBillboard.UpdateBillboard();
			}
		}
	}

	// Token: 0x060015C9 RID: 5577 RVA: 0x000B945C File Offset: 0x000B765C
	protected void UpdateMeshColors(Color color)
	{
		if (this.m_MeshFilter == null)
		{
			this.m_MeshFilter = (MeshFilter)base.gameObject.GetComponent(typeof(MeshFilter));
		}
		if (this.m_MeshFilter == null || this.m_MeshFilter.sharedMesh == null || this.m_MeshFilter.mesh == null)
		{
			return;
		}
		Color[] array = new Color[this.m_MeshFilter.mesh.vertexCount];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		this.m_MeshFilter.mesh.colors = array;
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x000B9520 File Offset: 0x000B7720
	protected virtual void OnDestroy()
	{
		if (this.m_RuntimeMaterials != null)
		{
			foreach (Material material in this.m_RuntimeMaterials)
			{
				Object.Destroy(material);
			}
			this.m_RuntimeMaterials = null;
		}
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x0000DE2D File Offset: 0x0000C02D
	public void OnApplicationQuit()
	{
		NcEffectBehaviour.m_bShuttingDown = true;
	}

	// Token: 0x060015CC RID: 5580 RVA: 0x0000264F File Offset: 0x0000084F
	public virtual void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x0000264F File Offset: 0x0000084F
	public virtual void OnSetActiveRecursively(bool bActive)
	{
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x0000264F File Offset: 0x0000084F
	public virtual void OnUpdateToolData()
	{
	}

	// Token: 0x060015CF RID: 5583 RVA: 0x0000DE35 File Offset: 0x0000C035
	public virtual void OnSetReplayState()
	{
		this.m_bReplayState = true;
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x0000264F File Offset: 0x0000084F
	public virtual void OnResetReplayStage(bool bClearOldParticle)
	{
	}

	// Token: 0x04001A45 RID: 6725
	private static bool m_bShuttingDown;

	// Token: 0x04001A46 RID: 6726
	private static GameObject m_RootInstance;

	// Token: 0x04001A47 RID: 6727
	public float m_fUserTag;

	// Token: 0x04001A48 RID: 6728
	protected MeshFilter m_MeshFilter;

	// Token: 0x04001A49 RID: 6729
	protected List<Material> m_RuntimeMaterials;

	// Token: 0x04001A4A RID: 6730
	protected bool m_bReplayState;

	// Token: 0x04001A4B RID: 6731
	protected NcEffectInitBackup m_NcEffectInitBackup;

	// Token: 0x020003A8 RID: 936
	public class _RuntimeIntance
	{
		// Token: 0x060015D1 RID: 5585 RVA: 0x0000DE3E File Offset: 0x0000C03E
		public _RuntimeIntance(GameObject parentGameObject, GameObject childGameObject)
		{
			this.m_ParentGameObject = parentGameObject;
			this.m_ChildGameObject = childGameObject;
		}

		// Token: 0x04001A4C RID: 6732
		public GameObject m_ParentGameObject;

		// Token: 0x04001A4D RID: 6733
		public GameObject m_ChildGameObject;
	}
}
