using System;
using UnityEngine;

// Token: 0x020003A9 RID: 937
public class NcEffectInitBackup
{
	// Token: 0x060015D3 RID: 5587 RVA: 0x0000DE54 File Offset: 0x0000C054
	public void BackupTransform(Transform targetTrans)
	{
		this.m_Transform = targetTrans;
		this.m_NcTansform = new NcTransformTool(this.m_Transform);
	}

	// Token: 0x060015D4 RID: 5588 RVA: 0x0000DE6E File Offset: 0x0000C06E
	public void RestoreTransform()
	{
		this.m_NcTansform.CopyToLocalTransform(this.m_Transform);
	}

	// Token: 0x060015D5 RID: 5589 RVA: 0x000B958C File Offset: 0x000B778C
	public void BackupMaterialColor(GameObject targetObj, bool bRecursively)
	{
		if (targetObj == null)
		{
			return;
		}
		if (bRecursively)
		{
			this.m_SavedMaterialColor = NcEffectInitBackup.SAVE_TYPE.RECURSIVELY;
		}
		else
		{
			this.m_SavedMaterialColor = NcEffectInitBackup.SAVE_TYPE.ONE;
		}
		Transform transform = targetObj.transform;
		if (this.m_SavedMaterialColor == NcEffectInitBackup.SAVE_TYPE.RECURSIVELY)
		{
			this.m_MaterialColorRenderers = transform.GetComponentsInChildren<Renderer>(true);
			this.m_MaterialColorColorNames = new string[this.m_MaterialColorRenderers.Length];
			this.m_MaterialColorSaveValues = new Vector4[this.m_MaterialColorRenderers.Length];
			for (int i = 0; i < this.m_MaterialColorRenderers.Length; i++)
			{
				Renderer renderer = this.m_MaterialColorRenderers[i];
				this.m_MaterialColorColorNames[i] = NcEffectInitBackup.GetMaterialColorName(renderer.sharedMaterial);
				if (this.m_MaterialColorColorNames[i] != null)
				{
					this.m_MaterialColorSaveValues[i] = renderer.material.GetColor(this.m_MaterialColorColorNames[i]);
				}
			}
		}
		else
		{
			this.m_MaterialColorRenderer = transform.GetComponent<Renderer>();
			if (this.m_MaterialColorRenderer != null)
			{
				this.m_MaterialColorColorName = NcEffectInitBackup.GetMaterialColorName(this.m_MaterialColorRenderer.sharedMaterial);
				if (this.m_MaterialColorColorName != null)
				{
					this.m_MaterialColorSaveValue = this.m_MaterialColorRenderer.material.GetColor(this.m_MaterialColorColorName);
				}
			}
		}
	}

	// Token: 0x060015D6 RID: 5590 RVA: 0x000B96D4 File Offset: 0x000B78D4
	public void RestoreMaterialColor()
	{
		if (this.m_SavedMaterialColor == NcEffectInitBackup.SAVE_TYPE.NONE)
		{
			return;
		}
		if (this.m_SavedMaterialColor == NcEffectInitBackup.SAVE_TYPE.RECURSIVELY)
		{
			for (int i = 0; i < this.m_MaterialColorRenderers.Length; i++)
			{
				if (this.m_MaterialColorRenderers[i] != null && this.m_MaterialColorColorNames[i] != null)
				{
					this.m_MaterialColorRenderers[i].material.SetColor(this.m_MaterialColorColorNames[i], this.m_MaterialColorSaveValues[i]);
				}
			}
		}
		else if (this.m_MaterialColorRenderers != null)
		{
			this.m_MaterialColorColorName = NcEffectInitBackup.GetMaterialColorName(this.m_MaterialColorRenderer.sharedMaterial);
			if (this.m_MaterialColorColorName != null)
			{
				this.m_MaterialColorRenderer.material.SetColor(this.m_MaterialColorColorName, this.m_MaterialColorSaveValue);
			}
		}
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x0000DE81 File Offset: 0x0000C081
	public void BackupUvAnimation(NcUvAnimation uvAniCom)
	{
		if (uvAniCom == null)
		{
			return;
		}
		this.m_NcUvAnimation = uvAniCom;
		this.m_UvAniSaveValue = new Vector2(this.m_NcUvAnimation.m_fScrollSpeedX, this.m_NcUvAnimation.m_fScrollSpeedY);
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x0000DEB8 File Offset: 0x0000C0B8
	public void RestoreUvAnimation()
	{
		if (this.m_NcUvAnimation == null)
		{
			return;
		}
		this.m_NcUvAnimation.m_fScrollSpeedX = this.m_UvAniSaveValue.x;
		this.m_NcUvAnimation.m_fScrollSpeedY = this.m_UvAniSaveValue.y;
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x000B97B4 File Offset: 0x000B79B4
	public void BackupMeshColor(GameObject targetObj, bool bRecursively)
	{
		if (targetObj == null)
		{
			return;
		}
		if (bRecursively)
		{
			this.m_SavedMeshColor = NcEffectInitBackup.SAVE_TYPE.RECURSIVELY;
		}
		else
		{
			this.m_SavedMeshColor = NcEffectInitBackup.SAVE_TYPE.ONE;
		}
		if (this.m_SavedMeshColor == NcEffectInitBackup.SAVE_TYPE.RECURSIVELY)
		{
			this.m_MeshColorMeshFilters = targetObj.GetComponentsInChildren<MeshFilter>(true);
			this.m_MeshColorSaveValues = new Vector4[this.m_MeshColorMeshFilters.Length];
			if (this.m_MeshColorMeshFilters == null || this.m_MeshColorMeshFilters.Length < 0)
			{
				return;
			}
			for (int i = 0; i < this.m_MeshColorMeshFilters.Length; i++)
			{
				this.m_MeshColorSaveValues[i] = this.GetMeshColor(this.m_MeshColorMeshFilters[i]);
			}
		}
		else
		{
			this.m_MeshColorMeshFilter = targetObj.GetComponent<MeshFilter>();
			this.m_MeshColorSaveValue = this.GetMeshColor(this.m_MeshColorMeshFilter);
		}
	}

	// Token: 0x060015DA RID: 5594 RVA: 0x000B9894 File Offset: 0x000B7A94
	public void RestoreMeshColor()
	{
		if (this.m_SavedMeshColor == NcEffectInitBackup.SAVE_TYPE.NONE)
		{
			return;
		}
		if (this.m_SavedMeshColor == NcEffectInitBackup.SAVE_TYPE.RECURSIVELY)
		{
			if (this.m_MeshColorMeshFilters == null || this.m_MeshColorMeshFilters.Length < 0)
			{
				return;
			}
			for (int i = 0; i < this.m_MeshColorMeshFilters.Length; i++)
			{
				this.SetMeshColor(this.m_MeshColorMeshFilters[i], this.m_MeshColorSaveValues[i]);
			}
		}
		else
		{
			this.SetMeshColor(this.m_MeshColorMeshFilter, this.m_MeshColorSaveValue);
		}
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x000B992C File Offset: 0x000B7B2C
	protected Color GetMeshColor(MeshFilter mFilter)
	{
		if (mFilter == null || mFilter.mesh == null)
		{
			return Color.white;
		}
		Color[] array = mFilter.mesh.colors;
		if (array.Length == 0)
		{
			array = new Color[mFilter.mesh.vertices.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Color.white;
			}
			mFilter.mesh.colors = array;
			return Color.white;
		}
		return array[0];
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x000B99C8 File Offset: 0x000B7BC8
	protected void SetMeshColor(MeshFilter mFilter, Color tarColor)
	{
		if (mFilter == null || mFilter.mesh == null)
		{
			return;
		}
		Color[] array = mFilter.mesh.colors;
		if (array.Length == 0)
		{
			array = new Color[mFilter.mesh.vertices.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Color.white;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = tarColor;
		}
		mFilter.mesh.colors = array;
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x000B91A0 File Offset: 0x000B73A0
	protected static string GetMaterialColorName(Material mat)
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

	// Token: 0x04001A4E RID: 6734
	protected NcEffectInitBackup.SAVE_TYPE m_SavedMaterialColor;

	// Token: 0x04001A4F RID: 6735
	protected Renderer m_MaterialColorRenderer;

	// Token: 0x04001A50 RID: 6736
	protected string m_MaterialColorColorName;

	// Token: 0x04001A51 RID: 6737
	protected Vector4 m_MaterialColorSaveValue;

	// Token: 0x04001A52 RID: 6738
	protected Renderer[] m_MaterialColorRenderers;

	// Token: 0x04001A53 RID: 6739
	protected string[] m_MaterialColorColorNames;

	// Token: 0x04001A54 RID: 6740
	protected Vector4[] m_MaterialColorSaveValues;

	// Token: 0x04001A55 RID: 6741
	protected NcEffectInitBackup.SAVE_TYPE m_SavedMeshColor;

	// Token: 0x04001A56 RID: 6742
	protected MeshFilter m_MeshColorMeshFilter;

	// Token: 0x04001A57 RID: 6743
	protected Vector4 m_MeshColorSaveValue;

	// Token: 0x04001A58 RID: 6744
	protected MeshFilter[] m_MeshColorMeshFilters;

	// Token: 0x04001A59 RID: 6745
	protected Vector4[] m_MeshColorSaveValues;

	// Token: 0x04001A5A RID: 6746
	protected NcUvAnimation m_NcUvAnimation;

	// Token: 0x04001A5B RID: 6747
	protected Vector2 m_UvAniSaveValue;

	// Token: 0x04001A5C RID: 6748
	protected Transform m_Transform;

	// Token: 0x04001A5D RID: 6749
	protected NcTransformTool m_NcTansform;

	// Token: 0x020003AA RID: 938
	protected enum SAVE_TYPE
	{
		// Token: 0x04001A5F RID: 6751
		NONE,
		// Token: 0x04001A60 RID: 6752
		ONE,
		// Token: 0x04001A61 RID: 6753
		RECURSIVELY
	}
}
