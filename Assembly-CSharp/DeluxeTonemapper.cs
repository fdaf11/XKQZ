using System;
using UnityEngine;

// Token: 0x0200069A RID: 1690
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class DeluxeTonemapper : MonoBehaviour
{
	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x060028FE RID: 10494 RVA: 0x0001AFD7 File Offset: 0x000191D7
	public Shader TonemappingShader
	{
		get
		{
			return this.m_Shader;
		}
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x0001AF87 File Offset: 0x00019187
	private void DestroyMaterial(Material mat)
	{
		if (mat)
		{
			Object.DestroyImmediate(mat);
			mat = null;
		}
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x00145B9C File Offset: 0x00143D9C
	private void CreateMaterials()
	{
		if (this.m_Shader == null)
		{
			if (this.m_Mode == DeluxeTonemapper.Mode.Color)
			{
				this.m_Shader = Shader.Find("Hidden/Deluxe/TonemapperColor");
			}
			if (this.m_Mode == DeluxeTonemapper.Mode.Luminance)
			{
				this.m_Shader = Shader.Find("Hidden/Deluxe/TonemapperLuminosity");
			}
			if (this.m_Mode == DeluxeTonemapper.Mode.ExtendedLuminance)
			{
				this.m_Shader = Shader.Find("Hidden/Deluxe/TonemapperLuminosityExtended");
			}
		}
		if (this.m_Material == null && this.m_Shader != null && this.m_Shader.isSupported)
		{
			this.m_Material = this.CreateMaterial(this.m_Shader);
		}
	}

	// Token: 0x06002901 RID: 10497 RVA: 0x00144E10 File Offset: 0x00143010
	private Material CreateMaterial(Shader shader)
	{
		if (!shader)
		{
			return null;
		}
		return new Material(shader)
		{
			hideFlags = 13
		};
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x0001AFDF File Offset: 0x000191DF
	private void OnDisable()
	{
		this.DestroyMaterial(this.m_Material);
		this.m_Material = null;
		this.m_Shader = null;
	}

	// Token: 0x06002903 RID: 10499 RVA: 0x0001AFFB File Offset: 0x000191FB
	public void StoreK()
	{
		this.m_MainCurve.StoreK();
	}

	// Token: 0x06002904 RID: 10500 RVA: 0x00145C54 File Offset: 0x00143E54
	public void UpdateCoefficients()
	{
		this.StoreK();
		this.m_MainCurve.UpdateCoefficients();
		if (this.m_Material == null)
		{
			return;
		}
		this.m_Material.SetFloat("_K", this.m_MainCurve.m_k);
		this.m_Material.SetFloat("_Crossover", this.m_MainCurve.m_CrossOverPoint);
		this.m_Material.SetVector("_Toe", this.m_MainCurve.m_ToeCoef);
		this.m_Material.SetVector("_Shoulder", this.m_MainCurve.m_ShoulderCoef);
		this.m_Material.SetVector("_Tint", this.m_Tint);
		this.m_Material.SetFloat("_LuminosityWhite", this.m_MainCurve.m_LuminositySaturationPoint * this.m_MainCurve.m_LuminositySaturationPoint);
	}

	// Token: 0x06002905 RID: 10501 RVA: 0x0001B008 File Offset: 0x00019208
	public void ReloadShaders()
	{
		this.OnDisable();
	}

	// Token: 0x06002906 RID: 10502 RVA: 0x0001B010 File Offset: 0x00019210
	private void OnEnable()
	{
		if (this.m_MainCurve == null)
		{
			this.m_MainCurve = new FilmicCurve();
		}
		this.CreateMaterials();
		this.UpdateCoefficients();
	}

	// Token: 0x06002907 RID: 10503 RVA: 0x0001B034 File Offset: 0x00019234
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.m_Mode != this.m_LastMode)
		{
			this.ReloadShaders();
		}
		this.m_LastMode = this.m_Mode;
		this.CreateMaterials();
		this.UpdateCoefficients();
		Graphics.Blit(source, destination, this.m_Material);
	}

	// Token: 0x040033EA RID: 13290
	[SerializeField]
	public FilmicCurve m_MainCurve = new FilmicCurve();

	// Token: 0x040033EB RID: 13291
	[SerializeField]
	public Color m_Tint = new Color(1f, 1f, 1f, 1f);

	// Token: 0x040033EC RID: 13292
	[SerializeField]
	public DeluxeTonemapper.Mode m_Mode;

	// Token: 0x040033ED RID: 13293
	private DeluxeTonemapper.Mode m_LastMode;

	// Token: 0x040033EE RID: 13294
	private Material m_Material;

	// Token: 0x040033EF RID: 13295
	private Shader m_Shader;

	// Token: 0x0200069B RID: 1691
	public enum Mode
	{
		// Token: 0x040033F1 RID: 13297
		Color,
		// Token: 0x040033F2 RID: 13298
		Luminance,
		// Token: 0x040033F3 RID: 13299
		ExtendedLuminance
	}
}
