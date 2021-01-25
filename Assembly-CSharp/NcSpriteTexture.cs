using System;
using UnityEngine;

// Token: 0x020003E6 RID: 998
public class NcSpriteTexture : NcEffectBehaviour
{
	// Token: 0x060017E1 RID: 6113 RVA: 0x000C4384 File Offset: 0x000C2584
	private void Awake()
	{
		if (this.m_MeshFilter == null)
		{
			this.m_MeshFilter = base.gameObject.GetComponent<MeshFilter>();
			if (this.m_MeshFilter == null)
			{
				this.m_MeshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
		}
		if (this.m_NcSpriteFactoryPrefab == null && base.gameObject.GetComponent<NcSpriteFactory>() != null)
		{
			this.m_NcSpriteFactoryPrefab = base.gameObject;
		}
		this.UpdateFactoryInfo(this.m_nSpriteFactoryIndex);
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x0000F99A File Offset: 0x0000DB9A
	private void Start()
	{
		this.UpdateSpriteTexture(true);
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x000C4418 File Offset: 0x000C2618
	public void SetSpriteFactoryIndex(string spriteName, int nFrameIndex, bool bRunImmediate)
	{
		if (this.m_NcSpriteFactoryCom == null)
		{
			if (!this.m_NcSpriteFactoryPrefab || !(this.m_NcSpriteFactoryPrefab.GetComponent<NcSpriteFactory>() != null))
			{
				return;
			}
			this.m_NcSpriteFactoryCom = this.m_NcSpriteFactoryPrefab.GetComponent<NcSpriteFactory>();
		}
		this.m_nSpriteFactoryIndex = this.m_NcSpriteFactoryCom.GetSpriteNodeIndex(spriteName);
		this.SetSpriteFactoryIndex(this.m_nSpriteFactoryIndex, nFrameIndex, bRunImmediate);
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x0000F9A3 File Offset: 0x0000DBA3
	public void SetSpriteFactoryIndex(int nSpriteFactoryIndex, int nFrameIndex, bool bRunImmediate)
	{
		if (!this.UpdateFactoryInfo(nSpriteFactoryIndex))
		{
			return;
		}
		this.SetFrameIndex(nFrameIndex);
		if (bRunImmediate)
		{
			this.UpdateSpriteTexture(bRunImmediate);
		}
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x000C4494 File Offset: 0x000C2694
	public void SetFrameIndex(int nFrameIndex)
	{
		this.m_nFrameIndex = ((0 > nFrameIndex) ? this.m_nFrameIndex : nFrameIndex);
		if (this.m_NcSpriteFrameInfos == null)
		{
			return;
		}
		this.m_nFrameIndex = ((this.m_NcSpriteFrameInfos.Length != 0) ? ((this.m_NcSpriteFrameInfos.Length > this.m_nFrameIndex) ? this.m_nFrameIndex : (this.m_NcSpriteFrameInfos.Length - 1)) : 0);
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x0000F9C6 File Offset: 0x0000DBC6
	public void SetShowRate(float fShowRate)
	{
		this.m_fShowRate = fShowRate;
		this.UpdateSpriteTexture(true);
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x000C4508 File Offset: 0x000C2708
	private bool UpdateFactoryInfo(int nSpriteFactoryIndex)
	{
		this.m_nSpriteFactoryIndex = nSpriteFactoryIndex;
		if (this.m_NcSpriteFactoryCom == null)
		{
			if (!this.m_NcSpriteFactoryPrefab || !(this.m_NcSpriteFactoryPrefab.GetComponent<NcSpriteFactory>() != null))
			{
				return false;
			}
			this.m_NcSpriteFactoryCom = this.m_NcSpriteFactoryPrefab.GetComponent<NcSpriteFactory>();
		}
		if (!this.m_NcSpriteFactoryCom.IsValidFactory())
		{
			return false;
		}
		this.m_NcSpriteFrameInfos = this.m_NcSpriteFactoryCom.GetSpriteNode(this.m_nSpriteFactoryIndex).m_FrameInfos;
		this.m_fUvScale = this.m_NcSpriteFactoryCom.m_fUvScale;
		return true;
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x000C45AC File Offset: 0x000C27AC
	private void UpdateSpriteTexture(bool bShowEffect)
	{
		if (!this.UpdateSpriteMaterial())
		{
			return;
		}
		if (!this.m_NcSpriteFactoryCom.IsValidFactory())
		{
			return;
		}
		if (this.m_NcSpriteFrameInfos.Length == 0)
		{
			this.SetSpriteFactoryIndex(this.m_nSpriteFactoryIndex, this.m_nFrameIndex, false);
		}
		if (this.m_MeshFilter == null)
		{
			if (base.gameObject.GetComponent<MeshFilter>() != null)
			{
				this.m_MeshFilter = base.gameObject.GetComponent<MeshFilter>();
			}
			else
			{
				this.m_MeshFilter = base.gameObject.AddComponent<MeshFilter>();
			}
		}
		NcSpriteFactory.CreatePlane(this.m_MeshFilter, this.m_fUvScale, this.m_NcSpriteFrameInfos[this.m_nFrameIndex], false, this.m_AlignType, this.m_MeshType, this.m_fShowRate);
		NcSpriteFactory.UpdateMeshUVs(this.m_MeshFilter, this.m_NcSpriteFrameInfos[this.m_nFrameIndex].m_TextureUvOffset, this.m_AlignType, this.m_fShowRate);
		if (bShowEffect)
		{
			this.m_EffectObject = this.m_NcSpriteFactoryCom.CreateSpriteEffect(this.m_nSpriteFactoryIndex, base.transform);
		}
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x000C46C4 File Offset: 0x000C28C4
	public bool UpdateSpriteMaterial()
	{
		if (this.m_NcSpriteFactoryPrefab == null)
		{
			return false;
		}
		if (this.m_NcSpriteFactoryPrefab.renderer == null || this.m_NcSpriteFactoryPrefab.renderer.sharedMaterial == null || this.m_NcSpriteFactoryPrefab.renderer.sharedMaterial.mainTexture == null)
		{
			return false;
		}
		if (base.renderer == null)
		{
			return false;
		}
		if (this.m_NcSpriteFactoryCom == null)
		{
			return false;
		}
		if (this.m_nSpriteFactoryIndex < 0 || this.m_NcSpriteFactoryCom.GetSpriteNodeCount() <= this.m_nSpriteFactoryIndex)
		{
			return false;
		}
		if (this.m_NcSpriteFactoryCom.m_SpriteType != NcSpriteFactory.SPRITE_TYPE.NcSpriteTexture && this.m_NcSpriteFactoryCom.m_SpriteType != NcSpriteFactory.SPRITE_TYPE.Auto)
		{
			return false;
		}
		base.renderer.sharedMaterial = this.m_NcSpriteFactoryPrefab.renderer.sharedMaterial;
		return true;
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x0000264F File Offset: 0x0000084F
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x0000264F File Offset: 0x0000084F
	public override void OnUpdateToolData()
	{
	}

	// Token: 0x04001CAA RID: 7338
	public GameObject m_NcSpriteFactoryPrefab;

	// Token: 0x04001CAB RID: 7339
	protected NcSpriteFactory m_NcSpriteFactoryCom;

	// Token: 0x04001CAC RID: 7340
	public NcSpriteFactory.NcFrameInfo[] m_NcSpriteFrameInfos;

	// Token: 0x04001CAD RID: 7341
	public float m_fUvScale = 1f;

	// Token: 0x04001CAE RID: 7342
	public int m_nSpriteFactoryIndex;

	// Token: 0x04001CAF RID: 7343
	public int m_nFrameIndex;

	// Token: 0x04001CB0 RID: 7344
	public NcSpriteFactory.MESH_TYPE m_MeshType;

	// Token: 0x04001CB1 RID: 7345
	public NcSpriteFactory.ALIGN_TYPE m_AlignType = NcSpriteFactory.ALIGN_TYPE.CENTER;

	// Token: 0x04001CB2 RID: 7346
	public float m_fShowRate = 1f;

	// Token: 0x04001CB3 RID: 7347
	protected GameObject m_EffectObject;
}
