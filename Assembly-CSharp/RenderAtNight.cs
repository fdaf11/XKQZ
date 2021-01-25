using System;
using UnityEngine;

// Token: 0x02000849 RID: 2121
[RequireComponent(typeof(Renderer))]
public class RenderAtNight : MonoBehaviour
{
	// Token: 0x0600337F RID: 13183 RVA: 0x0002064D File Offset: 0x0001E84D
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.rendererComponent = base.GetComponent<Renderer>();
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x00020676 File Offset: 0x0001E876
	protected void Update()
	{
		this.rendererComponent.enabled = this.sky.IsNight;
	}

	// Token: 0x04003FB0 RID: 16304
	public TOD_Sky sky;

	// Token: 0x04003FB1 RID: 16305
	private Renderer rendererComponent;
}
