using System;
using UnityEngine;

// Token: 0x0200052C RID: 1324
[ExecuteInEditMode]
public class RTPFogUpdate : MonoBehaviour
{
	// Token: 0x060021D9 RID: 8665 RVA: 0x00016AEA File Offset: 0x00014CEA
	private void Start()
	{
		RTPFogUpdate.Refresh(this.LinearColorSpace);
		base.Invoke("RefreshAll", 0.2f);
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x00016B07 File Offset: 0x00014D07
	private void Update()
	{
		if (this.UpdateOnEveryFrame)
		{
			RTPFogUpdate.Refresh(this.LinearColorSpace);
		}
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x00016B1F File Offset: 0x00014D1F
	private void OnApplicationFocus(bool focusStatus)
	{
		if (focusStatus)
		{
			this.RefreshAll();
		}
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x00100698 File Offset: 0x000FE898
	private void RefreshAll()
	{
		ReliefTerrain reliefTerrain = (ReliefTerrain)Object.FindObjectOfType(typeof(ReliefTerrain));
		if (reliefTerrain != null && reliefTerrain.globalSettingsHolder != null)
		{
			reliefTerrain.globalSettingsHolder.RefreshAll();
		}
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x001006DC File Offset: 0x000FE8DC
	public static void Refresh(bool _LinearColorSpace = false)
	{
		if (RenderSettings.fog)
		{
			Shader.SetGlobalFloat("_Fdensity", RenderSettings.fogDensity);
			if (_LinearColorSpace)
			{
				Shader.SetGlobalColor("_FColor", RenderSettings.fogColor.linear);
			}
			else
			{
				Shader.SetGlobalColor("_FColor", RenderSettings.fogColor);
			}
			Shader.SetGlobalFloat("_Fstart", RenderSettings.fogStartDistance);
			Shader.SetGlobalFloat("_Fend", RenderSettings.fogEndDistance);
		}
		else
		{
			Shader.SetGlobalFloat("_Fdensity", 0f);
			Shader.SetGlobalFloat("_Fstart", 1000000f);
			Shader.SetGlobalFloat("_Fend", 2000000f);
		}
		Shader.SetGlobalColor("RTP_ambLight", RenderSettings.ambientLight);
	}

	// Token: 0x04002546 RID: 9542
	public bool UpdateOnEveryFrame = true;

	// Token: 0x04002547 RID: 9543
	public bool LinearColorSpace;

	// Token: 0x04002548 RID: 9544
	private bool prev_LinearColorSpace;
}
