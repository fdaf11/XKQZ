using System;
using UnityEngine;

// Token: 0x02000848 RID: 2120
[RequireComponent(typeof(Renderer))]
public class RenderAtDay : MonoBehaviour
{
	// Token: 0x0600337C RID: 13180 RVA: 0x0002060C File Offset: 0x0001E80C
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.rendererComponent = base.GetComponent<Renderer>();
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x00020635 File Offset: 0x0001E835
	protected void Update()
	{
		this.rendererComponent.enabled = this.sky.IsDay;
	}

	// Token: 0x04003FAE RID: 16302
	public TOD_Sky sky;

	// Token: 0x04003FAF RID: 16303
	private Renderer rendererComponent;
}
