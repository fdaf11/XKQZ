using System;
using UnityEngine;

// Token: 0x020003E7 RID: 999
public class NcTilingTexture : NcEffectBehaviour
{
	// Token: 0x060017ED RID: 6125 RVA: 0x000C4808 File Offset: 0x000C2A08
	private void Start()
	{
		if (base.renderer != null && base.renderer.material != null)
		{
			base.renderer.material.mainTextureScale = new Vector2(this.m_fTilingX, this.m_fTilingY);
			base.renderer.material.mainTextureOffset = new Vector2(this.m_fOffsetX - (float)((int)this.m_fOffsetX), this.m_fOffsetY - (float)((int)this.m_fOffsetY));
			base.AddRuntimeMaterial(base.renderer.material);
		}
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x000C48A4 File Offset: 0x000C2AA4
	private void Update()
	{
		if (this.m_bFixedTileSize)
		{
			if (this.m_OriginalScale.x != 0f)
			{
				this.m_fTilingX = this.m_OriginalTiling.x * (base.transform.lossyScale.x / this.m_OriginalScale.x);
			}
			if (this.m_OriginalScale.y != 0f)
			{
				this.m_fTilingY = this.m_OriginalTiling.y * (base.transform.lossyScale.y / this.m_OriginalScale.y);
			}
			base.renderer.material.mainTextureScale = new Vector2(this.m_fTilingX, this.m_fTilingY);
		}
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x0000F9D6 File Offset: 0x0000DBD6
	public override void OnUpdateToolData()
	{
		this.m_OriginalScale = base.transform.lossyScale;
		this.m_OriginalTiling.x = this.m_fTilingX;
		this.m_OriginalTiling.y = this.m_fTilingY;
	}

	// Token: 0x04001CB4 RID: 7348
	public float m_fTilingX = 2f;

	// Token: 0x04001CB5 RID: 7349
	public float m_fTilingY = 2f;

	// Token: 0x04001CB6 RID: 7350
	public float m_fOffsetX;

	// Token: 0x04001CB7 RID: 7351
	public float m_fOffsetY;

	// Token: 0x04001CB8 RID: 7352
	public bool m_bFixedTileSize;

	// Token: 0x04001CB9 RID: 7353
	protected Vector3 m_OriginalScale = default(Vector3);

	// Token: 0x04001CBA RID: 7354
	protected Vector2 m_OriginalTiling = default(Vector2);
}
