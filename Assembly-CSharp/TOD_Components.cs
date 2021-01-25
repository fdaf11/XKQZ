using System;
using UnityEngine;

// Token: 0x0200081A RID: 2074
[ExecuteInEditMode]
public class TOD_Components : MonoBehaviour
{
	// Token: 0x060032D9 RID: 13017 RVA: 0x00187F9C File Offset: 0x0018619C
	public void Initialize()
	{
		this.DomeTransform = base.GetComponent<Transform>();
		if (Camera.main != null)
		{
			this.CameraTransform = Camera.main.GetComponent<Transform>();
		}
		else
		{
			Debug.LogWarning("Main camera does not exist or is not tagged 'MainCamera'.");
		}
		this.Sky = base.GetComponent<TOD_Sky>();
		this.Animation = base.GetComponent<TOD_Animation>();
		this.Time = base.GetComponent<TOD_Time>();
		this.Weather = base.GetComponent<TOD_Weather>();
		this.Resources = base.GetComponent<TOD_Resources>();
		if (!this.Space)
		{
			Debug.LogError("Space reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.SpaceTransform = this.Space.GetComponent<Transform>();
		this.SpaceRenderer = this.Space.GetComponent<Renderer>();
		this.SpaceShader = this.SpaceRenderer.sharedMaterial;
		this.SpaceMeshFilter = this.Space.GetComponent<MeshFilter>();
		if (!this.Atmosphere)
		{
			Debug.LogError("Atmosphere reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.AtmosphereRenderer = this.Atmosphere.GetComponent<Renderer>();
		this.AtmosphereShader = this.AtmosphereRenderer.sharedMaterial;
		this.AtmosphereMeshFilter = this.Atmosphere.GetComponent<MeshFilter>();
		if (!this.Clear)
		{
			Debug.LogError("Clear reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.ClearRenderer = this.Clear.GetComponent<Renderer>();
		this.ClearShader = this.ClearRenderer.sharedMaterial;
		this.ClearMeshFilter = this.Clear.GetComponent<MeshFilter>();
		if (!this.Clouds)
		{
			Debug.LogError("Clouds reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.CloudRenderer = this.Clouds.GetComponent<Renderer>();
		this.CloudShader = this.CloudRenderer.sharedMaterial;
		this.CloudMeshFilter = this.Clouds.GetComponent<MeshFilter>();
		if (!this.Projector)
		{
			Debug.LogError("Projector reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.ShadowProjector = this.Projector.GetComponent<Projector>();
		this.ShadowShader = this.ShadowProjector.material;
		if (!this.Light)
		{
			Debug.LogError("Light reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.LightTransform = this.Light.GetComponent<Transform>();
		this.LightSource = this.Light.GetComponent<Light>();
		if (!this.Sun)
		{
			Debug.LogError("Sun reference not set. Disabling TOD_Sky script.");
			this.Sky.enabled = false;
			return;
		}
		this.SunTransform = this.Sun.GetComponent<Transform>();
		this.SunRenderer = this.Sun.GetComponent<Renderer>();
		this.SunShader = this.SunRenderer.sharedMaterial;
		this.SunMeshFilter = this.Sun.GetComponent<MeshFilter>();
		if (this.Moon)
		{
			this.MoonTransform = this.Moon.GetComponent<Transform>();
			this.MoonRenderer = this.Moon.GetComponent<Renderer>();
			this.MoonShader = this.MoonRenderer.sharedMaterial;
			this.MoonMeshFilter = this.Moon.GetComponent<MeshFilter>();
			return;
		}
		Debug.LogError("Moon reference not set. Disabling TOD_Sky script.");
		this.Sky.enabled = false;
	}

	// Token: 0x04003E4B RID: 15947
	public GameObject Sun;

	// Token: 0x04003E4C RID: 15948
	public GameObject Moon;

	// Token: 0x04003E4D RID: 15949
	public GameObject Atmosphere;

	// Token: 0x04003E4E RID: 15950
	public GameObject Clear;

	// Token: 0x04003E4F RID: 15951
	public GameObject Clouds;

	// Token: 0x04003E50 RID: 15952
	public GameObject Space;

	// Token: 0x04003E51 RID: 15953
	public GameObject Light;

	// Token: 0x04003E52 RID: 15954
	public GameObject Projector;

	// Token: 0x04003E53 RID: 15955
	internal Transform DomeTransform;

	// Token: 0x04003E54 RID: 15956
	internal Transform SunTransform;

	// Token: 0x04003E55 RID: 15957
	internal Transform MoonTransform;

	// Token: 0x04003E56 RID: 15958
	internal Transform CameraTransform;

	// Token: 0x04003E57 RID: 15959
	internal Transform LightTransform;

	// Token: 0x04003E58 RID: 15960
	internal Transform SpaceTransform;

	// Token: 0x04003E59 RID: 15961
	internal Renderer SpaceRenderer;

	// Token: 0x04003E5A RID: 15962
	internal Renderer AtmosphereRenderer;

	// Token: 0x04003E5B RID: 15963
	internal Renderer ClearRenderer;

	// Token: 0x04003E5C RID: 15964
	internal Renderer CloudRenderer;

	// Token: 0x04003E5D RID: 15965
	internal Renderer SunRenderer;

	// Token: 0x04003E5E RID: 15966
	internal Renderer MoonRenderer;

	// Token: 0x04003E5F RID: 15967
	internal MeshFilter SpaceMeshFilter;

	// Token: 0x04003E60 RID: 15968
	internal MeshFilter AtmosphereMeshFilter;

	// Token: 0x04003E61 RID: 15969
	internal MeshFilter ClearMeshFilter;

	// Token: 0x04003E62 RID: 15970
	internal MeshFilter CloudMeshFilter;

	// Token: 0x04003E63 RID: 15971
	internal MeshFilter SunMeshFilter;

	// Token: 0x04003E64 RID: 15972
	internal MeshFilter MoonMeshFilter;

	// Token: 0x04003E65 RID: 15973
	internal Material SpaceShader;

	// Token: 0x04003E66 RID: 15974
	internal Material AtmosphereShader;

	// Token: 0x04003E67 RID: 15975
	internal Material ClearShader;

	// Token: 0x04003E68 RID: 15976
	internal Material CloudShader;

	// Token: 0x04003E69 RID: 15977
	internal Material SunShader;

	// Token: 0x04003E6A RID: 15978
	internal Material MoonShader;

	// Token: 0x04003E6B RID: 15979
	internal Material ShadowShader;

	// Token: 0x04003E6C RID: 15980
	internal Light LightSource;

	// Token: 0x04003E6D RID: 15981
	internal Projector ShadowProjector;

	// Token: 0x04003E6E RID: 15982
	internal TOD_Sky Sky;

	// Token: 0x04003E6F RID: 15983
	internal TOD_Animation Animation;

	// Token: 0x04003E70 RID: 15984
	internal TOD_Time Time;

	// Token: 0x04003E71 RID: 15985
	internal TOD_Weather Weather;

	// Token: 0x04003E72 RID: 15986
	internal TOD_Resources Resources;

	// Token: 0x04003E73 RID: 15987
	internal TOD_SunShafts SunShafts;
}
