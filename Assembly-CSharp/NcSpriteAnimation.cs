using System;
using UnityEngine;

// Token: 0x020003DC RID: 988
public class NcSpriteAnimation : NcEffectAniBehaviour
{
	// Token: 0x06001798 RID: 6040 RVA: 0x0000F5F3 File Offset: 0x0000D7F3
	public override int GetAnimationState()
	{
		if (!base.enabled || !NcEffectBehaviour.IsActive(base.gameObject))
		{
			return -1;
		}
		if (this.m_fStartTime == 0f || !base.IsEndAnimation())
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06001799 RID: 6041 RVA: 0x0000F630 File Offset: 0x0000D830
	public float GetDurationTime()
	{
		return (float)((this.m_PlayMode != NcSpriteAnimation.PLAYMODE.PINGPONG) ? this.m_nFrameCount : (this.m_nFrameCount * 2 - 1)) / this.m_fFps;
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x0000F65B File Offset: 0x0000D85B
	public int GetShowIndex()
	{
		return this.m_nLastIndex + this.m_nStartFrame;
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x0000F66A File Offset: 0x0000D86A
	public void SetBreakLoop()
	{
		this.m_bBreakLoop = true;
	}

	// Token: 0x0600179C RID: 6044 RVA: 0x0000F673 File Offset: 0x0000D873
	public bool IsInPartLoop()
	{
		return this.m_bInPartLoop;
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x0000F67B File Offset: 0x0000D87B
	public override void ResetAnimation()
	{
		this.m_nLastIndex = -1;
		this.m_nLastSeqIndex = -1;
		if (!base.enabled)
		{
			base.enabled = true;
		}
		this.Start();
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x0000F6A3 File Offset: 0x0000D8A3
	public void SetSelectFrame(int nSelFrame)
	{
		this.m_nSelectFrame = nSelFrame;
		this.SetIndex(this.m_nSelectFrame);
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x0000F6B8 File Offset: 0x0000D8B8
	public bool IsEmptyFrame()
	{
		return this.m_NcSpriteFrameInfos[this.m_nSelectFrame].m_bEmptyFrame;
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x0000F6CC File Offset: 0x0000D8CC
	public int GetMaxFrameCount()
	{
		if (this.m_TextureType == NcSpriteAnimation.TEXTURE_TYPE.TileTexture)
		{
			return this.m_nTilingX * this.m_nTilingY;
		}
		return this.m_NcSpriteFrameInfos.Length;
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x0000F6EF File Offset: 0x0000D8EF
	public int GetValidFrameCount()
	{
		if (this.m_TextureType == NcSpriteAnimation.TEXTURE_TYPE.TileTexture)
		{
			return this.m_nTilingX * this.m_nTilingY - this.m_nStartFrame;
		}
		return this.m_NcSpriteFrameInfos.Length - this.m_nStartFrame;
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x0000F720 File Offset: 0x0000D920
	public void SetCallBackChangeFrame(Component changeFrameComponent, string changeFrameFunction)
	{
		this.m_OnChangeFrameComponent = changeFrameComponent;
		this.m_OnChangeFrameFunction = changeFrameFunction;
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x000C2124 File Offset: 0x000C0324
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
		if (this.m_nLoopFrameCount == 0)
		{
			this.m_nLoopFrameCount = this.m_nFrameCount - this.m_nLoopStartFrame;
		}
		this.m_Renderer = base.renderer;
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x000C21E0 File Offset: 0x000C03E0
	private void ResetLocalValue()
	{
		this.m_size = new Vector2(1f / (float)this.m_nTilingX, 1f / (float)this.m_nTilingY);
		this.m_Renderer = base.renderer;
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
		this.m_nFrameCount = ((this.m_nFrameCount > 0) ? this.m_nFrameCount : (this.m_nTilingX * this.m_nTilingY));
		this.m_nLastIndex = -999;
		this.m_nLastSeqIndex = -1;
		this.m_bInPartLoop = false;
		this.m_bBreakLoop = false;
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x000C2274 File Offset: 0x000C0474
	private void Start()
	{
		this.ResetLocalValue();
		if (this.m_Renderer == null)
		{
			base.enabled = false;
			return;
		}
		if (this.m_PlayMode == NcSpriteAnimation.PLAYMODE.SELECT)
		{
			this.SetIndex(this.m_nSelectFrame);
		}
		else
		{
			if (0f < this.m_fDelayTime)
			{
				this.m_Renderer.enabled = false;
				return;
			}
			if (this.m_PlayMode == NcSpriteAnimation.PLAYMODE.RANDOM)
			{
				this.SetIndex(Random.Range(0, this.m_nFrameCount - 1));
			}
			else
			{
				base.InitAnimationTimer();
				if (this.m_bLoopRandom)
				{
					this.m_Timer.Reset(Random.value);
				}
				this.SetIndex(0);
			}
		}
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x000C2328 File Offset: 0x000C0528
	private void Update()
	{
		if (this.m_PlayMode == NcSpriteAnimation.PLAYMODE.SELECT)
		{
			return;
		}
		if (this.m_Renderer == null || this.m_nTilingX * this.m_nTilingY == 0)
		{
			return;
		}
		if (this.m_fDelayTime != 0f)
		{
			if (NcEffectBehaviour.GetEngineTime() < this.m_fStartTime + this.m_fDelayTime)
			{
				return;
			}
			this.m_fDelayTime = 0f;
			base.InitAnimationTimer();
			this.m_Renderer.enabled = true;
		}
		if (this.m_PlayMode != NcSpriteAnimation.PLAYMODE.RANDOM)
		{
			int num = (int)(this.m_Timer.GetTime() * this.m_fFps);
			if (num == 0 && this.m_NcSpriteFactoryCom != null)
			{
				this.m_NcSpriteFactoryCom.OnAnimationStartFrame(this);
			}
			if (this.m_NcSpriteFactoryCom != null && this.m_nFrameCount <= 0)
			{
				this.m_NcSpriteFactoryCom.OnAnimationLastFrame(this, 0);
			}
			else
			{
				if (((this.m_PlayMode != NcSpriteAnimation.PLAYMODE.PINGPONG) ? this.m_nFrameCount : (this.m_nFrameCount * 2 - 1)) <= num)
				{
					if (!this.m_bLoop)
					{
						if (this.m_NcSpriteFactoryCom != null && this.m_NcSpriteFactoryCom.OnAnimationLastFrame(this, 1))
						{
							return;
						}
						this.UpdateEndAnimation();
						return;
					}
					else if (this.m_PlayMode == NcSpriteAnimation.PLAYMODE.PINGPONG)
					{
						if (this.m_NcSpriteFactoryCom != null && num % (this.m_nFrameCount * 2 - 2) == 1 && this.m_NcSpriteFactoryCom.OnAnimationLastFrame(this, num / (this.m_nFrameCount * 2 - 1)))
						{
							return;
						}
					}
					else if (this.m_NcSpriteFactoryCom != null && num % this.m_nFrameCount == 0 && this.m_NcSpriteFactoryCom.OnAnimationLastFrame(this, num / this.m_nFrameCount))
					{
						return;
					}
				}
				this.SetIndex(num);
			}
		}
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x0000F730 File Offset: 0x0000D930
	public void SetSpriteFactoryIndex(int nSpriteFactoryIndex, bool bRunImmediate)
	{
		this.UpdateFactoryInfo(nSpriteFactoryIndex);
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x0000F73A File Offset: 0x0000D93A
	public void SetShowRate(float fShowRate)
	{
		this.m_fShowRate = fShowRate;
		this.UpdateSpriteTexture(this.m_nLastIndex, true);
	}

	// Token: 0x060017A9 RID: 6057 RVA: 0x000C250C File Offset: 0x000C070C
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
		NcSpriteFactory.NcSpriteNode spriteNode = this.m_NcSpriteFactoryCom.GetSpriteNode(this.m_nSpriteFactoryIndex);
		this.m_bBuildSpriteObj = false;
		this.m_bAutoDestruct = false;
		this.m_fUvScale = this.m_NcSpriteFactoryCom.m_fUvScale;
		this.m_nStartFrame = 0;
		if (spriteNode != null)
		{
			this.m_nFrameCount = spriteNode.m_nFrameCount;
			this.m_fFps = spriteNode.m_fFps;
			this.m_bLoop = spriteNode.m_bLoop;
			this.m_nLoopStartFrame = spriteNode.m_nLoopStartFrame;
			this.m_nLoopFrameCount = spriteNode.m_nLoopFrameCount;
			this.m_nLoopingCount = spriteNode.m_nLoopingCount;
			this.m_NcSpriteFrameInfos = spriteNode.m_FrameInfos;
		}
		this.ResetLocalValue();
		return true;
	}

	// Token: 0x060017AA RID: 6058 RVA: 0x000C2608 File Offset: 0x000C0808
	private int GetPartLoopFrameIndex(int nSeqIndex)
	{
		if (nSeqIndex < 0)
		{
			return -1;
		}
		int num = nSeqIndex - this.m_nLoopStartFrame;
		if (num < 0)
		{
			return nSeqIndex;
		}
		int num2 = num / this.m_nLoopFrameCount;
		if (this.m_nLoopingCount == 0 || num2 < this.m_nLoopingCount)
		{
			return num % this.m_nLoopFrameCount + this.m_nLoopStartFrame;
		}
		return num - this.m_nLoopFrameCount * (this.m_nLoopingCount - 1) + this.m_nLoopStartFrame;
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x000C2678 File Offset: 0x000C0878
	private int GetPartLoopCount(int nSeqIndex)
	{
		if (nSeqIndex < 0)
		{
			return -1;
		}
		int num = nSeqIndex - this.m_nLoopStartFrame;
		if (num < 0)
		{
			return -1;
		}
		int num2 = num / this.m_nLoopFrameCount;
		if (this.m_nLoopingCount == 0 || num2 < this.m_nLoopingCount)
		{
			return num2;
		}
		return this.m_nLoopingCount;
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x000C26C8 File Offset: 0x000C08C8
	private int CalcPartLoopInfo(int nSeqIndex, ref int nLoopCount)
	{
		if (this.m_nLoopFrameCount <= 0)
		{
			return 0;
		}
		if (this.m_bBreakLoop)
		{
			nLoopCount = this.GetPartLoopCount(nSeqIndex);
			this.UpdateEndAnimation();
			this.m_bBreakLoop = false;
			return this.m_nLoopStartFrame + this.m_nLoopFrameCount;
		}
		if (nSeqIndex < this.m_nLoopStartFrame)
		{
			return nSeqIndex;
		}
		this.m_bInPartLoop = true;
		int partLoopCount = this.GetPartLoopCount(this.m_nLastSeqIndex);
		int num = this.GetPartLoopFrameIndex(nSeqIndex);
		nLoopCount = this.GetPartLoopCount(nSeqIndex);
		int num2 = 0;
		int i = partLoopCount;
		while (i < Mathf.Min(nLoopCount, this.m_nLoopFrameCount - 1))
		{
			if (base.transform.parent != null)
			{
				base.transform.parent.SendMessage("OnSpriteAnimationLoopStart", nLoopCount, 1);
			}
			i++;
			num2++;
		}
		if (0 < this.m_nLoopingCount && this.m_nLoopingCount <= nLoopCount)
		{
			this.m_bInPartLoop = false;
			if (base.transform.parent != null)
			{
				base.transform.parent.SendMessage("OnSpriteAnimationLoopEnd", nLoopCount, 1);
			}
			if (this.m_nFrameCount <= num)
			{
				num = this.m_nFrameCount - 1;
				this.UpdateEndAnimation();
			}
		}
		return num;
	}

	// Token: 0x060017AD RID: 6061 RVA: 0x000C2810 File Offset: 0x000C0A10
	private void UpdateEndAnimation()
	{
		base.enabled = false;
		base.OnEndAnimation();
		if (this.m_bAutoDestruct)
		{
			if (this.m_bReplayState)
			{
				NcEffectBehaviour.SetActiveRecursively(base.gameObject, false);
			}
			else
			{
				Object.DestroyObject(base.gameObject);
			}
		}
	}

	// Token: 0x060017AE RID: 6062 RVA: 0x000C285C File Offset: 0x000C0A5C
	private void SetIndex(int nSeqIndex)
	{
		if (this.m_Renderer != null)
		{
			this.m_nLastSeqIndex = nSeqIndex;
			int num = nSeqIndex;
			int nLoopCount = nSeqIndex / this.m_nFrameCount;
			switch (this.m_PlayMode)
			{
			case NcSpriteAnimation.PLAYMODE.DEFAULT:
				if (this.m_bLoop)
				{
					num = this.CalcPartLoopInfo(nSeqIndex, ref nLoopCount) % this.m_nFrameCount;
				}
				else
				{
					num = nSeqIndex % this.m_nFrameCount;
				}
				break;
			case NcSpriteAnimation.PLAYMODE.INVERSE:
				num = this.m_nFrameCount - num % this.m_nFrameCount - 1;
				break;
			case NcSpriteAnimation.PLAYMODE.PINGPONG:
				nLoopCount = num / (this.m_nFrameCount * 2 - ((num != 0) ? 2 : 1));
				num %= this.m_nFrameCount * 2 - ((num != 0) ? 2 : 1);
				if (this.m_nFrameCount <= num)
				{
					num = this.m_nFrameCount - num % this.m_nFrameCount - 2;
				}
				break;
			case NcSpriteAnimation.PLAYMODE.SELECT:
				num = nSeqIndex % this.m_nFrameCount;
				break;
			}
			if (num == this.m_nLastIndex)
			{
				return;
			}
			if (this.m_TextureType == NcSpriteAnimation.TEXTURE_TYPE.TileTexture)
			{
				int num2 = (num + this.m_nStartFrame) % this.m_nTilingX;
				int num3 = (num + this.m_nStartFrame) / this.m_nTilingX;
				Vector2 vector;
				vector..ctor((float)num2 * this.m_size.x, 1f - this.m_size.y - (float)num3 * this.m_size.y);
				if (!this.UpdateMeshUVsByTileTexture(new Rect(vector.x, vector.y, this.m_size.x, this.m_size.y)))
				{
					this.m_Renderer.material.mainTextureOffset = new Vector2(vector.x - (float)((int)vector.x), vector.y - (float)((int)vector.y));
					this.m_Renderer.material.mainTextureScale = this.m_size;
					base.AddRuntimeMaterial(this.m_Renderer.material);
				}
			}
			else if (this.m_TextureType == NcSpriteAnimation.TEXTURE_TYPE.TrimTexture)
			{
				this.UpdateSpriteTexture(num, true);
			}
			else if (this.m_TextureType == NcSpriteAnimation.TEXTURE_TYPE.SpriteFactory)
			{
				this.UpdateFactoryTexture(num, true);
			}
			if (this.m_NcSpriteFactoryCom != null)
			{
				this.m_NcSpriteFactoryCom.OnAnimationChangingFrame(this, this.m_nLastIndex, num, nLoopCount);
			}
			if (this.m_OnChangeFrameComponent != null)
			{
				this.m_OnChangeFrameComponent.SendMessage(this.m_OnChangeFrameFunction, num, 1);
			}
			this.m_nLastIndex = num;
		}
	}

	// Token: 0x060017AF RID: 6063 RVA: 0x0000F750 File Offset: 0x0000D950
	private void UpdateSpriteTexture(int nSelIndex, bool bShowEffect)
	{
		nSelIndex += this.m_nStartFrame;
		if (this.m_NcSpriteFrameInfos == null || nSelIndex < 0 || this.m_NcSpriteFrameInfos.Length <= nSelIndex)
		{
			return;
		}
		this.CreateBuiltInPlane(nSelIndex);
		this.UpdateBuiltInPlane(nSelIndex);
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x000C2AEC File Offset: 0x000C0CEC
	private void UpdateFactoryTexture(int nSelIndex, bool bShowEffect)
	{
		nSelIndex += this.m_nStartFrame;
		if (this.m_NcSpriteFrameInfos == null || nSelIndex < 0 || this.m_NcSpriteFrameInfos.Length <= nSelIndex)
		{
			return;
		}
		if (!this.UpdateFactoryMaterial())
		{
			return;
		}
		if (!this.m_NcSpriteFactoryCom.IsValidFactory())
		{
			return;
		}
		this.CreateBuiltInPlane(nSelIndex);
		this.UpdateBuiltInPlane(nSelIndex);
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x000C2B50 File Offset: 0x000C0D50
	public bool UpdateFactoryMaterial()
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
		if (this.m_NcSpriteFactoryCom.m_SpriteType != NcSpriteFactory.SPRITE_TYPE.NcSpriteAnimation && this.m_NcSpriteFactoryCom.m_SpriteType != NcSpriteFactory.SPRITE_TYPE.Auto)
		{
			return false;
		}
		base.renderer.sharedMaterial = this.m_NcSpriteFactoryPrefab.renderer.sharedMaterial;
		return true;
	}

	// Token: 0x060017B2 RID: 6066 RVA: 0x000C2C50 File Offset: 0x000C0E50
	private void CreateBuiltInPlane(int nSelIndex)
	{
		if (this.m_bCreateBuiltInPlane)
		{
			return;
		}
		this.m_bCreateBuiltInPlane = true;
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
		NcSpriteFactory.CreatePlane(this.m_MeshFilter, this.m_fUvScale, this.m_NcSpriteFrameInfos[nSelIndex], this.m_TextureType != NcSpriteAnimation.TEXTURE_TYPE.TileTexture && this.m_bTrimCenterAlign, this.m_AlignType, this.m_MeshType, this.m_fShowRate);
	}

	// Token: 0x060017B3 RID: 6067 RVA: 0x000C2D00 File Offset: 0x000C0F00
	private void UpdateBuiltInPlane(int nSelIndex)
	{
		NcSpriteFactory.UpdatePlane(this.m_MeshFilter, this.m_fUvScale, this.m_NcSpriteFrameInfos[nSelIndex], this.m_TextureType != NcSpriteAnimation.TEXTURE_TYPE.TileTexture && this.m_bTrimCenterAlign, this.m_AlignType, this.m_fShowRate);
		NcSpriteFactory.UpdateMeshUVs(this.m_MeshFilter, this.m_NcSpriteFrameInfos[nSelIndex].m_TextureUvOffset, this.m_AlignType, this.m_fShowRate);
	}

	// Token: 0x060017B4 RID: 6068 RVA: 0x000C2D70 File Offset: 0x000C0F70
	private bool UpdateMeshUVsByTileTexture(Rect uv)
	{
		if (this.m_MeshFilter != null && this.m_MeshUVsByTileTexture == null)
		{
			return false;
		}
		if (this.m_MeshFilter == null)
		{
			this.m_MeshFilter = (MeshFilter)base.gameObject.GetComponent(typeof(MeshFilter));
		}
		if (this.m_MeshFilter == null || this.m_MeshFilter.sharedMesh == null)
		{
			return false;
		}
		if (this.m_MeshUVsByTileTexture == null)
		{
			for (int i = 0; i < this.m_MeshFilter.sharedMesh.uv.Length; i++)
			{
				if (this.m_MeshFilter.sharedMesh.uv[i].x != 0f && this.m_MeshFilter.sharedMesh.uv[i].x != 1f)
				{
					return false;
				}
				if (this.m_MeshFilter.sharedMesh.uv[i].y != 0f && this.m_MeshFilter.sharedMesh.uv[i].y != 1f)
				{
					return false;
				}
			}
			this.m_MeshUVsByTileTexture = this.m_MeshFilter.sharedMesh.uv;
		}
		Vector2[] array = new Vector2[this.m_MeshUVsByTileTexture.Length];
		for (int j = 0; j < this.m_MeshUVsByTileTexture.Length; j++)
		{
			if (this.m_MeshUVsByTileTexture[j].x == 0f)
			{
				array[j].x = uv.x;
			}
			if (this.m_MeshUVsByTileTexture[j].y == 0f)
			{
				array[j].y = uv.y;
			}
			if (this.m_MeshUVsByTileTexture[j].x == 1f)
			{
				array[j].x = uv.x + uv.width;
			}
			if (this.m_MeshUVsByTileTexture[j].y == 1f)
			{
				array[j].y = uv.y + uv.height;
			}
		}
		this.m_MeshFilter.mesh.uv = array;
		return true;
	}

	// Token: 0x060017B5 RID: 6069 RVA: 0x0000F78B File Offset: 0x0000D98B
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fDelayTime *= fSpeedRate;
		this.m_fFps *= fSpeedRate;
	}

	// Token: 0x060017B6 RID: 6070 RVA: 0x0000F7A9 File Offset: 0x0000D9A9
	public override void OnResetReplayStage(bool bClearOldParticle)
	{
		base.OnResetReplayStage(bClearOldParticle);
		this.ResetAnimation();
	}

	// Token: 0x04001C28 RID: 7208
	public NcSpriteAnimation.TEXTURE_TYPE m_TextureType;

	// Token: 0x04001C29 RID: 7209
	public NcSpriteAnimation.PLAYMODE m_PlayMode;

	// Token: 0x04001C2A RID: 7210
	public float m_fDelayTime;

	// Token: 0x04001C2B RID: 7211
	public int m_nStartFrame;

	// Token: 0x04001C2C RID: 7212
	public int m_nFrameCount;

	// Token: 0x04001C2D RID: 7213
	public int m_nSelectFrame;

	// Token: 0x04001C2E RID: 7214
	public bool m_bLoop = true;

	// Token: 0x04001C2F RID: 7215
	public int m_nLoopStartFrame;

	// Token: 0x04001C30 RID: 7216
	public int m_nLoopFrameCount;

	// Token: 0x04001C31 RID: 7217
	public int m_nLoopingCount;

	// Token: 0x04001C32 RID: 7218
	public bool m_bLoopRandom;

	// Token: 0x04001C33 RID: 7219
	public bool m_bAutoDestruct;

	// Token: 0x04001C34 RID: 7220
	public float m_fFps = 10f;

	// Token: 0x04001C35 RID: 7221
	public int m_nTilingX = 2;

	// Token: 0x04001C36 RID: 7222
	public int m_nTilingY = 2;

	// Token: 0x04001C37 RID: 7223
	public GameObject m_NcSpriteFactoryPrefab;

	// Token: 0x04001C38 RID: 7224
	protected NcSpriteFactory m_NcSpriteFactoryCom;

	// Token: 0x04001C39 RID: 7225
	public NcSpriteFactory.NcFrameInfo[] m_NcSpriteFrameInfos;

	// Token: 0x04001C3A RID: 7226
	public float m_fUvScale = 1f;

	// Token: 0x04001C3B RID: 7227
	public int m_nSpriteFactoryIndex;

	// Token: 0x04001C3C RID: 7228
	public NcSpriteFactory.MESH_TYPE m_MeshType;

	// Token: 0x04001C3D RID: 7229
	public NcSpriteFactory.ALIGN_TYPE m_AlignType = NcSpriteFactory.ALIGN_TYPE.CENTER;

	// Token: 0x04001C3E RID: 7230
	public float m_fShowRate = 1f;

	// Token: 0x04001C3F RID: 7231
	public bool m_bTrimCenterAlign;

	// Token: 0x04001C40 RID: 7232
	protected Component m_OnChangeFrameComponent;

	// Token: 0x04001C41 RID: 7233
	protected string m_OnChangeFrameFunction;

	// Token: 0x04001C42 RID: 7234
	protected bool m_bCreateBuiltInPlane;

	// Token: 0x04001C43 RID: 7235
	[HideInInspector]
	public bool m_bBuildSpriteObj;

	// Token: 0x04001C44 RID: 7236
	[HideInInspector]
	public bool m_bNeedRebuildAlphaChannel;

	// Token: 0x04001C45 RID: 7237
	[HideInInspector]
	public AnimationCurve m_curveAlphaWeight;

	// Token: 0x04001C46 RID: 7238
	protected Vector2 m_size;

	// Token: 0x04001C47 RID: 7239
	protected Renderer m_Renderer;

	// Token: 0x04001C48 RID: 7240
	protected float m_fStartTime;

	// Token: 0x04001C49 RID: 7241
	protected int m_nLastIndex = -999;

	// Token: 0x04001C4A RID: 7242
	protected int m_nLastSeqIndex = -1;

	// Token: 0x04001C4B RID: 7243
	protected bool m_bInPartLoop;

	// Token: 0x04001C4C RID: 7244
	protected bool m_bBreakLoop;

	// Token: 0x04001C4D RID: 7245
	protected Vector2[] m_MeshUVsByTileTexture;

	// Token: 0x020003DD RID: 989
	public enum TEXTURE_TYPE
	{
		// Token: 0x04001C4F RID: 7247
		TileTexture,
		// Token: 0x04001C50 RID: 7248
		TrimTexture,
		// Token: 0x04001C51 RID: 7249
		SpriteFactory
	}

	// Token: 0x020003DE RID: 990
	public enum PLAYMODE
	{
		// Token: 0x04001C53 RID: 7251
		DEFAULT,
		// Token: 0x04001C54 RID: 7252
		INVERSE,
		// Token: 0x04001C55 RID: 7253
		PINGPONG,
		// Token: 0x04001C56 RID: 7254
		RANDOM,
		// Token: 0x04001C57 RID: 7255
		SELECT
	}
}
