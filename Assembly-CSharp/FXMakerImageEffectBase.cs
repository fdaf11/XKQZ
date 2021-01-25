using System;
using UnityEngine;

// Token: 0x02000402 RID: 1026
[AddComponentMenu("")]
[RequireComponent(typeof(Camera))]
public class FXMakerImageEffectBase : MonoBehaviour
{
	// Token: 0x060018C5 RID: 6341 RVA: 0x00010124 File Offset: 0x0000E324
	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			base.enabled = false;
			return;
		}
		if (!this.shader || !this.shader.isSupported)
		{
			base.enabled = false;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x060018C6 RID: 6342 RVA: 0x0001015F File Offset: 0x0000E35F
	protected Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.shader);
				this.m_Material.hideFlags = 13;
			}
			return this.m_Material;
		}
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x00010196 File Offset: 0x0000E396
	protected void OnDisable()
	{
		if (this.m_Material)
		{
			Object.DestroyImmediate(this.m_Material);
		}
	}

	// Token: 0x04001D26 RID: 7462
	public Shader shader;

	// Token: 0x04001D27 RID: 7463
	private Material m_Material;
}
