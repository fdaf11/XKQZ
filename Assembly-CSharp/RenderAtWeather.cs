using System;
using UnityEngine;

// Token: 0x0200084A RID: 2122
[RequireComponent(typeof(Renderer))]
public class RenderAtWeather : MonoBehaviour
{
	// Token: 0x06003382 RID: 13186 RVA: 0x0002068E File Offset: 0x0001E88E
	protected void OnEnable()
	{
		if (!this.sky)
		{
			this.sky = TOD_Sky.Instance;
		}
		this.rendererComponent = base.GetComponent<Renderer>();
	}

	// Token: 0x06003383 RID: 13187 RVA: 0x000206B7 File Offset: 0x0001E8B7
	protected void Update()
	{
		this.rendererComponent.enabled = (this.sky.Components.Weather.Weather == this.type);
	}

	// Token: 0x04003FB2 RID: 16306
	public TOD_Sky sky;

	// Token: 0x04003FB3 RID: 16307
	public TOD_Weather.WeatherType type;

	// Token: 0x04003FB4 RID: 16308
	private Renderer rendererComponent;
}
