using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003BB RID: 955
public class NsEffectManager : MonoBehaviour
{
	// Token: 0x060016AA RID: 5802 RVA: 0x000BBF40 File Offset: 0x000BA140
	public static Texture[] PreloadResource(GameObject tarObj)
	{
		if (tarObj == null)
		{
			return new Texture[0];
		}
		List<GameObject> list = new List<GameObject>();
		list.Add(tarObj);
		return NsEffectManager.PreloadResource(tarObj, list);
	}

	// Token: 0x060016AB RID: 5803 RVA: 0x000BBF74 File Offset: 0x000BA174
	public static Component GetComponentInChildren(GameObject tarObj, Type findType)
	{
		if (tarObj == null)
		{
			return null;
		}
		List<GameObject> list = new List<GameObject>();
		list.Add(tarObj);
		return NsEffectManager.GetComponentInChildren(tarObj, findType, list);
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x000BBFA4 File Offset: 0x000BA1A4
	public static GameObject CreateReplayEffect(GameObject tarPrefab)
	{
		if (tarPrefab == null)
		{
			return null;
		}
		if (!NcEffectBehaviour.IsSafe())
		{
			return null;
		}
		GameObject gameObject = (GameObject)Object.Instantiate(tarPrefab);
		NsEffectManager.SetReplayEffect(gameObject);
		return gameObject;
	}

	// Token: 0x060016AD RID: 5805 RVA: 0x000BBFE0 File Offset: 0x000BA1E0
	public static void SetReplayEffect(GameObject instanceObj)
	{
		NsEffectManager.PreloadResource(instanceObj);
		NsEffectManager.SetActiveRecursively(instanceObj, false);
		NcEffectBehaviour[] componentsInChildren = instanceObj.GetComponentsInChildren<NcEffectBehaviour>(true);
		foreach (NcEffectBehaviour ncEffectBehaviour in componentsInChildren)
		{
			ncEffectBehaviour.OnSetReplayState();
		}
	}

	// Token: 0x060016AE RID: 5806 RVA: 0x000BC024 File Offset: 0x000BA224
	public static void RunReplayEffect(GameObject instanceObj, bool bClearOldParticle)
	{
		NsEffectManager.SetActiveRecursively(instanceObj, true);
		NcEffectBehaviour[] componentsInChildren = instanceObj.GetComponentsInChildren<NcEffectBehaviour>(true);
		foreach (NcEffectBehaviour ncEffectBehaviour in componentsInChildren)
		{
			ncEffectBehaviour.OnResetReplayStage(bClearOldParticle);
		}
	}

	// Token: 0x060016AF RID: 5807 RVA: 0x000BC064 File Offset: 0x000BA264
	public static void AdjustSpeedRuntime(GameObject target, float fSpeedRate)
	{
		NcEffectBehaviour[] componentsInChildren = target.GetComponentsInChildren<NcEffectBehaviour>(true);
		foreach (NcEffectBehaviour ncEffectBehaviour in componentsInChildren)
		{
			ncEffectBehaviour.OnUpdateEffectSpeed(fSpeedRate, true);
		}
	}

	// Token: 0x060016B0 RID: 5808 RVA: 0x000BC09C File Offset: 0x000BA29C
	public static void SetActiveRecursively(GameObject target, bool bActive)
	{
		int num = target.transform.childCount - 1;
		while (0 <= num)
		{
			if (num < target.transform.childCount)
			{
				NsEffectManager.SetActiveRecursively(target.transform.GetChild(num).gameObject, bActive);
			}
			num--;
		}
		target.SetActive(bActive);
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x0000E8EA File Offset: 0x0000CAEA
	public static bool IsActive(GameObject target)
	{
		return target.activeInHierarchy && target.activeSelf;
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x000BC0F8 File Offset: 0x000BA2F8
	protected static void SetActiveRecursivelyEffect(GameObject target, bool bActive)
	{
		NcEffectBehaviour[] componentsInChildren = target.GetComponentsInChildren<NcEffectBehaviour>(true);
		foreach (NcEffectBehaviour ncEffectBehaviour in componentsInChildren)
		{
			ncEffectBehaviour.OnSetActiveRecursively(bActive);
		}
	}

	// Token: 0x060016B3 RID: 5811 RVA: 0x000BC130 File Offset: 0x000BA330
	protected static Texture[] PreloadResource(GameObject tarObj, List<GameObject> parentPrefabList)
	{
		if (!NcEffectBehaviour.IsSafe())
		{
			return null;
		}
		Renderer[] componentsInChildren = tarObj.GetComponentsInChildren<Renderer>(true);
		List<Texture> list = new List<Texture>();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer.sharedMaterials != null && renderer.sharedMaterials.Length > 0)
			{
				foreach (Material material in renderer.sharedMaterials)
				{
					if (material != null && material.mainTexture != null)
					{
						list.Add(material.mainTexture);
					}
				}
			}
		}
		NcAttachPrefab[] componentsInChildren2 = tarObj.GetComponentsInChildren<NcAttachPrefab>(true);
		foreach (NcAttachPrefab ncAttachPrefab in componentsInChildren2)
		{
			if (ncAttachPrefab.m_AttachPrefab != null)
			{
				Texture[] array3 = NsEffectManager.PreloadPrefab(ncAttachPrefab.m_AttachPrefab, parentPrefabList, true);
				if (array3 == null)
				{
					ncAttachPrefab.m_AttachPrefab = null;
				}
				else
				{
					list.AddRange(array3);
				}
			}
		}
		NcParticleSystem[] componentsInChildren3 = tarObj.GetComponentsInChildren<NcParticleSystem>(true);
		foreach (NcParticleSystem ncParticleSystem in componentsInChildren3)
		{
			if (ncParticleSystem.m_AttachPrefab != null)
			{
				Texture[] array5 = NsEffectManager.PreloadPrefab(ncParticleSystem.m_AttachPrefab, parentPrefabList, true);
				if (array5 == null)
				{
					ncParticleSystem.m_AttachPrefab = null;
				}
				else
				{
					list.AddRange(array5);
				}
			}
		}
		NcSpriteTexture[] componentsInChildren4 = tarObj.GetComponentsInChildren<NcSpriteTexture>(true);
		foreach (NcSpriteTexture ncSpriteTexture in componentsInChildren4)
		{
			if (ncSpriteTexture.m_NcSpriteFactoryPrefab != null)
			{
				Texture[] array7 = NsEffectManager.PreloadPrefab(ncSpriteTexture.m_NcSpriteFactoryPrefab, parentPrefabList, false);
				if (array7 != null)
				{
					list.AddRange(array7);
				}
			}
		}
		NcParticleSpiral[] componentsInChildren5 = tarObj.GetComponentsInChildren<NcParticleSpiral>(true);
		foreach (NcParticleSpiral ncParticleSpiral in componentsInChildren5)
		{
			if (ncParticleSpiral.m_ParticlePrefab != null)
			{
				Texture[] array9 = NsEffectManager.PreloadPrefab(ncParticleSpiral.m_ParticlePrefab, parentPrefabList, false);
				if (array9 != null)
				{
					list.AddRange(array9);
				}
			}
		}
		NcParticleEmit[] componentsInChildren6 = tarObj.GetComponentsInChildren<NcParticleEmit>(true);
		foreach (NcParticleEmit ncParticleEmit in componentsInChildren6)
		{
			if (ncParticleEmit.m_ParticlePrefab != null)
			{
				Texture[] array11 = NsEffectManager.PreloadPrefab(ncParticleEmit.m_ParticlePrefab, parentPrefabList, false);
				if (array11 != null)
				{
					list.AddRange(array11);
				}
			}
		}
		NcAttachSound[] componentsInChildren7 = tarObj.GetComponentsInChildren<NcAttachSound>(true);
		foreach (NcAttachSound ncAttachSound in componentsInChildren7)
		{
			if (ncAttachSound.m_AudioClip != null)
			{
			}
		}
		NcSpriteFactory[] componentsInChildren8 = tarObj.GetComponentsInChildren<NcSpriteFactory>(true);
		foreach (NcSpriteFactory ncSpriteFactory in componentsInChildren8)
		{
			if (ncSpriteFactory.m_SpriteList != null)
			{
				for (int num4 = 0; num4 < ncSpriteFactory.m_SpriteList.Count; num4++)
				{
					if (ncSpriteFactory.m_SpriteList[num4].m_EffectPrefab != null)
					{
						Texture[] array14 = NsEffectManager.PreloadPrefab(ncSpriteFactory.m_SpriteList[num4].m_EffectPrefab, parentPrefabList, true);
						if (array14 == null)
						{
							ncSpriteFactory.m_SpriteList[num4].m_EffectPrefab = null;
						}
						else
						{
							list.AddRange(array14);
						}
						if (ncSpriteFactory.m_SpriteList[num4].m_AudioClip != null)
						{
						}
					}
				}
			}
		}
		return list.ToArray();
	}

	// Token: 0x060016B4 RID: 5812 RVA: 0x000BC504 File Offset: 0x000BA704
	protected static Texture[] PreloadPrefab(GameObject tarObj, List<GameObject> parentPrefabList, bool bCheckDup)
	{
		if (!parentPrefabList.Contains(tarObj))
		{
			parentPrefabList.Add(tarObj);
			Texture[] result = NsEffectManager.PreloadResource(tarObj, parentPrefabList);
			parentPrefabList.Remove(tarObj);
			return result;
		}
		if (bCheckDup)
		{
			string text = string.Empty;
			for (int i = 0; i < parentPrefabList.Count; i++)
			{
				text = text + parentPrefabList[i].name + "/";
			}
			Debug.LogWarning("LoadError : Recursive Prefab - " + text + tarObj.name);
			return null;
		}
		return null;
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x000BC58C File Offset: 0x000BA78C
	protected static Component GetComponentInChildren(GameObject tarObj, Type findType, List<GameObject> parentPrefabList)
	{
		Component[] componentsInChildren = tarObj.GetComponentsInChildren(findType, true);
		foreach (Component component in componentsInChildren)
		{
			if (component.GetComponent<NcDontActive>() == null)
			{
				return component;
			}
		}
		NcAttachPrefab[] componentsInChildren2 = tarObj.GetComponentsInChildren<NcAttachPrefab>(true);
		foreach (NcAttachPrefab ncAttachPrefab in componentsInChildren2)
		{
			if (ncAttachPrefab.m_AttachPrefab != null)
			{
				Component validComponentInChildren = NsEffectManager.GetValidComponentInChildren(ncAttachPrefab.m_AttachPrefab, findType, parentPrefabList, true);
				if (validComponentInChildren != null)
				{
					return validComponentInChildren;
				}
			}
		}
		NcParticleSystem[] componentsInChildren3 = tarObj.GetComponentsInChildren<NcParticleSystem>(true);
		foreach (NcParticleSystem ncParticleSystem in componentsInChildren3)
		{
			if (ncParticleSystem.m_AttachPrefab != null)
			{
				Component validComponentInChildren = NsEffectManager.GetValidComponentInChildren(ncParticleSystem.m_AttachPrefab, findType, parentPrefabList, true);
				if (validComponentInChildren != null)
				{
					return validComponentInChildren;
				}
			}
		}
		NcSpriteTexture[] componentsInChildren4 = tarObj.GetComponentsInChildren<NcSpriteTexture>(true);
		foreach (NcSpriteTexture ncSpriteTexture in componentsInChildren4)
		{
			if (ncSpriteTexture.m_NcSpriteFactoryPrefab != null)
			{
				Component validComponentInChildren = NsEffectManager.GetValidComponentInChildren(ncSpriteTexture.m_NcSpriteFactoryPrefab, findType, parentPrefabList, false);
				if (validComponentInChildren != null)
				{
					return validComponentInChildren;
				}
			}
		}
		NcParticleSpiral[] componentsInChildren5 = tarObj.GetComponentsInChildren<NcParticleSpiral>(true);
		foreach (NcParticleSpiral ncParticleSpiral in componentsInChildren5)
		{
			if (ncParticleSpiral.m_ParticlePrefab != null)
			{
				Component validComponentInChildren = NsEffectManager.GetValidComponentInChildren(ncParticleSpiral.m_ParticlePrefab, findType, parentPrefabList, false);
				if (validComponentInChildren != null)
				{
					return validComponentInChildren;
				}
			}
		}
		NcParticleEmit[] componentsInChildren6 = tarObj.GetComponentsInChildren<NcParticleEmit>(true);
		foreach (NcParticleEmit ncParticleEmit in componentsInChildren6)
		{
			if (ncParticleEmit.m_ParticlePrefab != null)
			{
				Component validComponentInChildren = NsEffectManager.GetValidComponentInChildren(ncParticleEmit.m_ParticlePrefab, findType, parentPrefabList, false);
				if (validComponentInChildren != null)
				{
					return validComponentInChildren;
				}
			}
		}
		NcSpriteFactory[] componentsInChildren7 = tarObj.GetComponentsInChildren<NcSpriteFactory>(true);
		foreach (NcSpriteFactory ncSpriteFactory in componentsInChildren7)
		{
			if (ncSpriteFactory.m_SpriteList != null)
			{
				for (int num2 = 0; num2 < ncSpriteFactory.m_SpriteList.Count; num2++)
				{
					if (ncSpriteFactory.m_SpriteList[num2].m_EffectPrefab != null)
					{
						Component validComponentInChildren = NsEffectManager.GetValidComponentInChildren(ncSpriteFactory.m_SpriteList[num2].m_EffectPrefab, findType, parentPrefabList, true);
						if (validComponentInChildren != null)
						{
							return validComponentInChildren;
						}
					}
				}
			}
		}
		return null;
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x000BC858 File Offset: 0x000BAA58
	protected static Component GetValidComponentInChildren(GameObject tarObj, Type findType, List<GameObject> parentPrefabList, bool bCheckDup)
	{
		if (!parentPrefabList.Contains(tarObj))
		{
			parentPrefabList.Add(tarObj);
			Component componentInChildren = NsEffectManager.GetComponentInChildren(tarObj, findType, parentPrefabList);
			parentPrefabList.Remove(tarObj);
			return componentInChildren;
		}
		if (bCheckDup)
		{
			string text = string.Empty;
			for (int i = 0; i < parentPrefabList.Count; i++)
			{
				text = text + parentPrefabList[i].name + "/";
			}
			Debug.LogWarning("LoadError : Recursive Prefab - " + text + tarObj.name);
			return null;
		}
		return null;
	}
}
