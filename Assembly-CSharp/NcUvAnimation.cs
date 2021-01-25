using System;
using UnityEngine;

// Token: 0x020003EB RID: 1003
public class NcUvAnimation : NcEffectAniBehaviour
{
	// Token: 0x060017FB RID: 6139 RVA: 0x0000FAC7 File Offset: 0x0000DCC7
	public void SetFixedTileSize(bool bFixedTileSize)
	{
		this.m_bFixedTileSize = bFixedTileSize;
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x000C5AAC File Offset: 0x000C3CAC
	public override int GetAnimationState()
	{
		if (!this.m_bRepeat)
		{
			if (!base.enabled || !NcEffectBehaviour.IsActive(base.gameObject) || !base.IsEndAnimation())
			{
			}
		}
		return -1;
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x0000FAD0 File Offset: 0x0000DCD0
	public override void ResetAnimation()
	{
		if (!base.enabled)
		{
			base.enabled = true;
		}
		this.Start();
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x000C5AF4 File Offset: 0x000C3CF4
	private void Start()
	{
		this.m_Renderer = base.renderer;
		if (this.m_Renderer == null || this.m_Renderer.sharedMaterial == null || this.m_Renderer.sharedMaterial.mainTexture == null)
		{
			base.enabled = false;
		}
		else
		{
			base.renderer.material.mainTextureScale = new Vector2(this.m_fTilingX, this.m_fTilingY);
			base.AddRuntimeMaterial(base.renderer.material);
			float num = this.m_fOffsetX + this.m_fTilingX;
			this.m_RepeatOffset.x = num - (float)((int)num);
			if (this.m_RepeatOffset.x < 0f)
			{
				this.m_RepeatOffset.x = this.m_RepeatOffset.x + 1f;
			}
			num = this.m_fOffsetY + this.m_fTilingY;
			this.m_RepeatOffset.y = num - (float)((int)num);
			if (this.m_RepeatOffset.y < 0f)
			{
				this.m_RepeatOffset.y = this.m_RepeatOffset.y + 1f;
			}
			this.m_EndOffset.x = 1f - (this.m_fTilingX - (float)((int)this.m_fTilingX) + (float)((this.m_fTilingX - (float)((int)this.m_fTilingX) >= 0f) ? 0 : 1));
			this.m_EndOffset.y = 1f - (this.m_fTilingY - (float)((int)this.m_fTilingY) + (float)((this.m_fTilingY - (float)((int)this.m_fTilingY) >= 0f) ? 0 : 1));
			base.InitAnimationTimer();
		}
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x000C5CAC File Offset: 0x000C3EAC
	private void Update()
	{
		if (this.m_Renderer == null || this.m_Renderer.sharedMaterial == null || this.m_Renderer.sharedMaterial.mainTexture == null)
		{
			return;
		}
		if (this.m_bFixedTileSize)
		{
			if (this.m_fScrollSpeedX != 0f && this.m_OriginalScale.x != 0f)
			{
				this.m_fTilingX = this.m_OriginalTiling.x * (base.transform.lossyScale.x / this.m_OriginalScale.x);
			}
			if (this.m_fScrollSpeedY != 0f && this.m_OriginalScale.y != 0f)
			{
				this.m_fTilingY = this.m_OriginalTiling.y * (base.transform.lossyScale.y / this.m_OriginalScale.y);
			}
			base.renderer.material.mainTextureScale = new Vector2(this.m_fTilingX, this.m_fTilingY);
		}
		if (this.m_bUseSmoothDeltaTime)
		{
			this.m_fOffsetX += this.m_Timer.GetSmoothDeltaTime() * this.m_fScrollSpeedX;
			this.m_fOffsetY += this.m_Timer.GetSmoothDeltaTime() * this.m_fScrollSpeedY;
		}
		else
		{
			this.m_fOffsetX += this.m_Timer.GetDeltaTime() * this.m_fScrollSpeedX;
			this.m_fOffsetY += this.m_Timer.GetDeltaTime() * this.m_fScrollSpeedY;
		}
		bool flag = false;
		if (!this.m_bRepeat)
		{
			this.m_RepeatOffset.x = this.m_RepeatOffset.x + this.m_Timer.GetDeltaTime() * this.m_fScrollSpeedX;
			if (this.m_RepeatOffset.x < 0f || 1f < this.m_RepeatOffset.x)
			{
				this.m_fOffsetX = this.m_EndOffset.x;
				base.enabled = false;
				flag = true;
			}
			this.m_RepeatOffset.y = this.m_RepeatOffset.y + this.m_Timer.GetDeltaTime() * this.m_fScrollSpeedY;
			if (this.m_RepeatOffset.y < 0f || 1f < this.m_RepeatOffset.y)
			{
				this.m_fOffsetY = this.m_EndOffset.y;
				base.enabled = false;
				flag = true;
			}
		}
		this.m_Renderer.material.mainTextureOffset = new Vector2(this.m_fOffsetX - (float)((int)this.m_fOffsetX), this.m_fOffsetY - (float)((int)this.m_fOffsetY));
		if (flag)
		{
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
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x0000FAEA File Offset: 0x0000DCEA
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fScrollSpeedX *= fSpeedRate;
		this.m_fScrollSpeedY *= fSpeedRate;
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x0000FB08 File Offset: 0x0000DD08
	public override void OnUpdateToolData()
	{
		this.m_OriginalScale = base.transform.lossyScale;
		this.m_OriginalTiling.x = this.m_fTilingX;
		this.m_OriginalTiling.y = this.m_fTilingY;
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x0000FB3D File Offset: 0x0000DD3D
	public override void OnResetReplayStage(bool bClearOldParticle)
	{
		base.OnResetReplayStage(bClearOldParticle);
		this.ResetAnimation();
	}

	// Token: 0x04001CE6 RID: 7398
	public float m_fScrollSpeedX = 1f;

	// Token: 0x04001CE7 RID: 7399
	public float m_fScrollSpeedY;

	// Token: 0x04001CE8 RID: 7400
	public float m_fTilingX = 1f;

	// Token: 0x04001CE9 RID: 7401
	public float m_fTilingY = 1f;

	// Token: 0x04001CEA RID: 7402
	public float m_fOffsetX;

	// Token: 0x04001CEB RID: 7403
	public float m_fOffsetY;

	// Token: 0x04001CEC RID: 7404
	public bool m_bUseSmoothDeltaTime;

	// Token: 0x04001CED RID: 7405
	public bool m_bFixedTileSize;

	// Token: 0x04001CEE RID: 7406
	public bool m_bRepeat = true;

	// Token: 0x04001CEF RID: 7407
	public bool m_bAutoDestruct;

	// Token: 0x04001CF0 RID: 7408
	protected Vector3 m_OriginalScale = default(Vector3);

	// Token: 0x04001CF1 RID: 7409
	protected Vector2 m_OriginalTiling = default(Vector2);

	// Token: 0x04001CF2 RID: 7410
	protected Vector2 m_EndOffset = default(Vector2);

	// Token: 0x04001CF3 RID: 7411
	protected Vector2 m_RepeatOffset = default(Vector2);

	// Token: 0x04001CF4 RID: 7412
	protected Renderer m_Renderer;
}
