using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000832 RID: 2098
[RequireComponent(typeof(TOD_Resources))]
[ExecuteInEditMode]
[RequireComponent(typeof(TOD_Components))]
public class TOD_Sky : MonoBehaviour
{
	// Token: 0x06003309 RID: 13065 RVA: 0x00189668 File Offset: 0x00187868
	private void SetupQualitySettings()
	{
		TOD_Resources resources = this.Components.Resources;
		Material material = null;
		Material material2 = null;
		switch (this.CloudQuality)
		{
		case TOD_Sky.CloudQualityType.Fastest:
			material = resources.CloudMaterialFastest;
			material2 = resources.ShadowMaterialFastest;
			break;
		case TOD_Sky.CloudQualityType.Density:
			material = resources.CloudMaterialDensity;
			material2 = resources.ShadowMaterialDensity;
			break;
		case TOD_Sky.CloudQualityType.Bumped:
			material = resources.CloudMaterialBumped;
			material2 = resources.ShadowMaterialBumped;
			break;
		}
		Mesh mesh = null;
		Mesh mesh2 = null;
		Mesh mesh3 = null;
		Mesh mesh4 = null;
		Mesh mesh5 = null;
		Mesh mesh6 = null;
		switch (this.MeshQuality)
		{
		case TOD_Sky.MeshQualityType.Low:
			mesh = resources.IcosphereLow;
			mesh2 = resources.IcosphereLow;
			mesh3 = resources.IcosphereLow;
			mesh4 = resources.HalfIcosphereLow;
			mesh5 = resources.Quad;
			mesh6 = resources.SphereLow;
			break;
		case TOD_Sky.MeshQualityType.Medium:
			mesh = resources.IcosphereMedium;
			mesh2 = resources.IcosphereMedium;
			mesh3 = resources.IcosphereLow;
			mesh4 = resources.HalfIcosphereMedium;
			mesh5 = resources.Quad;
			mesh6 = resources.SphereMedium;
			break;
		case TOD_Sky.MeshQualityType.High:
			mesh = resources.IcosphereHigh;
			mesh2 = resources.IcosphereHigh;
			mesh3 = resources.IcosphereLow;
			mesh4 = resources.HalfIcosphereHigh;
			mesh5 = resources.Quad;
			mesh6 = resources.SphereHigh;
			break;
		}
		if (!this.Components.SpaceShader || this.Components.SpaceShader.name != resources.SpaceMaterial.name)
		{
			TOD_Components components = this.Components;
			Material material3 = resources.SpaceMaterial;
			this.Components.SpaceRenderer.sharedMaterial = material3;
			components.SpaceShader = material3;
		}
		if (!this.Components.AtmosphereShader || this.Components.AtmosphereShader.name != resources.AtmosphereMaterial.name)
		{
			TOD_Components components2 = this.Components;
			Material material3 = resources.AtmosphereMaterial;
			this.Components.AtmosphereRenderer.sharedMaterial = material3;
			components2.AtmosphereShader = material3;
		}
		if (!this.Components.ClearShader || this.Components.ClearShader.name != resources.ClearMaterial.name)
		{
			TOD_Components components3 = this.Components;
			Material material3 = resources.ClearMaterial;
			this.Components.ClearRenderer.sharedMaterial = material3;
			components3.ClearShader = material3;
		}
		if (!this.Components.CloudShader || this.Components.CloudShader.name != material.name)
		{
			TOD_Components components4 = this.Components;
			Material material3 = material;
			this.Components.CloudRenderer.sharedMaterial = material3;
			components4.CloudShader = material3;
		}
		if (!this.Components.ShadowShader || this.Components.ShadowShader.name != material2.name)
		{
			TOD_Components components5 = this.Components;
			Material material3 = material2;
			this.Components.ShadowProjector.material = material3;
			components5.ShadowShader = material3;
		}
		if (!this.Components.SunShader || this.Components.SunShader.name != resources.SunMaterial.name)
		{
			TOD_Components components6 = this.Components;
			Material material3 = resources.SunMaterial;
			this.Components.SunRenderer.sharedMaterial = material3;
			components6.SunShader = material3;
		}
		if (!this.Components.MoonShader || this.Components.MoonShader.name != resources.MoonMaterial.name)
		{
			TOD_Components components7 = this.Components;
			Material material3 = resources.MoonMaterial;
			this.Components.MoonRenderer.sharedMaterial = material3;
			components7.MoonShader = material3;
		}
		if (this.Components.SpaceMeshFilter.sharedMesh != mesh)
		{
			this.Components.SpaceMeshFilter.mesh = mesh;
		}
		if (this.Components.AtmosphereMeshFilter.sharedMesh != mesh2)
		{
			this.Components.AtmosphereMeshFilter.mesh = mesh2;
		}
		if (this.Components.ClearMeshFilter.sharedMesh != mesh3)
		{
			this.Components.ClearMeshFilter.mesh = mesh3;
		}
		if (this.Components.CloudMeshFilter.sharedMesh != mesh4)
		{
			this.Components.CloudMeshFilter.mesh = mesh4;
		}
		if (this.Components.SunMeshFilter.sharedMesh != mesh5)
		{
			this.Components.SunMeshFilter.mesh = mesh5;
		}
		if (this.Components.MoonMeshFilter.sharedMesh != mesh6)
		{
			this.Components.MoonMeshFilter.mesh = mesh6;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x0600330A RID: 13066 RVA: 0x0001FFD9 File Offset: 0x0001E1D9
	public static List<TOD_Sky> Instances
	{
		get
		{
			return TOD_Sky.instances;
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x0600330B RID: 13067 RVA: 0x0001FFE0 File Offset: 0x0001E1E0
	public static TOD_Sky Instance
	{
		get
		{
			return (TOD_Sky.instances.Count != 0) ? TOD_Sky.instances[TOD_Sky.instances.Count - 1] : null;
		}
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x00189B50 File Offset: 0x00187D50
	protected void OnEnable()
	{
		this.Components = base.GetComponent<TOD_Components>();
		if (!this.Components)
		{
			Debug.LogError("TOD_Components not found. Disabling script.");
			base.enabled = false;
			return;
		}
		this.Components.Initialize();
		this.LateUpdate();
		TOD_Sky.instances.Add(this);
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x0002000D File Offset: 0x0001E20D
	protected void OnDisable()
	{
		TOD_Sky.instances.Remove(this);
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x00189BA8 File Offset: 0x00187DA8
	protected void LateUpdate()
	{
		if (this.Components.SunShafts != null && this.Components.SunShafts.enabled)
		{
			if (!this.Components.ClearRenderer.enabled)
			{
				this.Components.ClearRenderer.enabled = true;
			}
		}
		else if (this.Components.ClearRenderer.enabled)
		{
			this.Components.ClearRenderer.enabled = false;
		}
		this.SetupQualitySettings();
		this.SetupSunAndMoon();
		this.SetupScattering();
		switch (Time.frameCount % 3)
		{
		case 0:
			if (!Application.isPlaying || this.timeSinceFogUpdate >= this.Fog.UpdateInterval)
			{
				this.timeSinceFogUpdate = 0f;
				TOD_FogType mode = this.Fog.Mode;
				if (mode != TOD_FogType.None)
				{
					if (mode == TOD_FogType.Color)
					{
						Color fogColor = this.SampleFogColor(this.Fog.Directional);
						RenderSettings.fogColor = fogColor;
					}
				}
			}
			else
			{
				this.timeSinceFogUpdate += Time.deltaTime;
			}
			break;
		case 1:
			if (!Application.isPlaying || this.timeSinceAmbientUpdate >= this.Ambient.UpdateInterval || this.Ambient.bSkipCheckIntervalOnce)
			{
				this.Ambient.bSkipCheckIntervalOnce = false;
				this.timeSinceAmbientUpdate = 0f;
				switch (this.Ambient.Mode)
				{
				case TOD_AmbientType.Flat:
					if (!this.m_bDontUpdateAmbient)
					{
						RenderSettings.ambientLight = this.AmbientColor;
					}
					break;
				case TOD_AmbientType.Hemisphere:
					Debug.LogWarning("Ambient.Mode." + this.Ambient.Mode.ToString() + " is only supported in Unity 5 or later.");
					break;
				case TOD_AmbientType.Trilight:
					Debug.LogWarning("Ambient.Mode." + this.Ambient.Mode.ToString() + " is only supported in Unity 5 or later.");
					break;
				case TOD_AmbientType.Spherical:
					Debug.LogWarning("Ambient.Mode." + this.Ambient.Mode.ToString() + " is only supported in Unity 5 or later.");
					break;
				}
			}
			else
			{
				this.timeSinceAmbientUpdate += Time.deltaTime;
			}
			break;
		case 2:
			if (!Application.isPlaying || this.timeSinceReflectionUpdate >= this.Reflection.UpdateInterval)
			{
				this.timeSinceReflectionUpdate = 0f;
				TOD_ReflectionType mode2 = this.Reflection.Mode;
				if (mode2 != TOD_ReflectionType.None)
				{
					if (mode2 == TOD_ReflectionType.Cubemap)
					{
						Debug.LogWarning("Reflection.Mode." + this.Reflection.Mode.ToString() + " is only supported in Unity 5 or later.");
					}
				}
			}
			else
			{
				this.timeSinceReflectionUpdate += Time.deltaTime;
			}
			break;
		}
		Vector4 vector = this.Components.Animation.CloudUV + this.Components.Animation.OffsetUV;
		Shader.SetGlobalFloat("TOD_Gamma", this.Gamma);
		Shader.SetGlobalFloat("TOD_OneOverGamma", this.OneOverGamma);
		Shader.SetGlobalColor("TOD_LightColor", this.LightColor);
		Shader.SetGlobalColor("TOD_CloudColor", this.CloudColor);
		Shader.SetGlobalColor("TOD_SunColor", this.SunColor);
		Shader.SetGlobalColor("TOD_MoonColor", this.MoonColor);
		Shader.SetGlobalColor("TOD_AdditiveColor", this.AdditiveColor);
		Shader.SetGlobalColor("TOD_MoonHaloColor", this.MoonHaloColor);
		Shader.SetGlobalVector("TOD_SunDirection", this.SunDirection);
		Shader.SetGlobalVector("TOD_MoonDirection", this.MoonDirection);
		Shader.SetGlobalVector("TOD_LightDirection", this.LightDirection);
		Shader.SetGlobalVector("TOD_LocalSunDirection", this.LocalSunDirection);
		Shader.SetGlobalVector("TOD_LocalMoonDirection", this.LocalMoonDirection);
		Shader.SetGlobalVector("TOD_LocalLightDirection", this.LocalLightDirection);
		Shader.SetGlobalFloat("TOD_Contrast", this.Atmosphere.Contrast * this.OneOverGamma);
		Shader.SetGlobalFloat("TOD_Haziness", this.Atmosphere.Haziness);
		Shader.SetGlobalFloat("TOD_Fogginess", this.Atmosphere.Fogginess);
		Shader.SetGlobalFloat("TOD_Horizon", this.World.HorizonOffset);
		Shader.SetGlobalVector("TOD_OpticalDepth", this.opticalDepth);
		Shader.SetGlobalVector("TOD_OneOverBeta", this.oneOverBeta);
		Shader.SetGlobalVector("TOD_BetaRayleigh", this.betaRayleigh);
		Shader.SetGlobalVector("TOD_BetaRayleighTheta", this.betaRayleighTheta);
		Shader.SetGlobalVector("TOD_BetaMie", this.betaMie);
		Shader.SetGlobalVector("TOD_BetaMieTheta", this.betaMieTheta);
		Shader.SetGlobalVector("TOD_BetaMiePhase", this.betaMiePhase);
		Shader.SetGlobalVector("TOD_BetaNight", this.betaNight);
		Shader.SetGlobalMatrix("TOD_World2Sky", this.Components.DomeTransform.worldToLocalMatrix);
		Shader.SetGlobalMatrix("TOD_Sky2World", this.Components.DomeTransform.localToWorldMatrix);
		if (this.Components.CloudShader != null)
		{
			float num = (1f - this.Atmosphere.Fogginess) * this.Clouds.Glow * this.Clouds.Brightness;
			float num2 = num * this.Day.CloudMultiplier * this.LerpValue * 2f;
			float num3 = num * this.Night.CloudMultiplier * (1f - Mathf.Abs(this.Moon.Phase)) * 4f;
			this.Components.CloudShader.SetFloat("_SunGlow", num2);
			this.Components.CloudShader.SetFloat("_MoonGlow", num3);
			this.Components.CloudShader.SetFloat("_CloudDensity", this.Clouds.Density);
			this.Components.CloudShader.SetFloat("_CloudSharpness", this.Clouds.Sharpness);
			this.Components.CloudShader.SetVector("_CloudScale1", this.Clouds.Scale1);
			this.Components.CloudShader.SetVector("_CloudScale2", this.Clouds.Scale2);
			this.Components.CloudShader.SetVector("_CloudUV", vector);
		}
		if (this.Components.SpaceShader != null)
		{
			this.Components.SpaceShader.SetFloat("_Tiling", this.Stars.Tiling);
			this.Components.SpaceShader.SetFloat("_Brightness", this.Stars.Brightness);
		}
		if (this.Components.SunShader != null)
		{
			this.Components.SunShader.SetColor("_Color", this.Sun.MeshColor * this.LerpValue * (1f - this.Atmosphere.Fogginess));
			this.Components.SunShader.SetFloat("_Contrast", this.Sun.MeshContrast);
			this.Components.SunShader.SetFloat("_Brightness", this.Sun.MeshBrightness);
		}
		if (this.Components.MoonShader != null)
		{
			this.Components.MoonShader.SetColor("_Color", this.Moon.MeshColor);
			this.Components.MoonShader.SetFloat("_Phase", this.Moon.Phase);
			this.Components.MoonShader.SetFloat("_Contrast", this.Moon.MeshContrast);
			this.Components.MoonShader.SetFloat("_Brightness", this.Moon.MeshBrightness * 2f);
		}
		if (this.Components.ShadowShader != null)
		{
			float num4 = this.Clouds.ShadowStrength * Mathf.Clamp01(1f - this.LightZenith / 90f);
			this.Components.ShadowShader.SetFloat("_Alpha", num4);
			this.Components.ShadowShader.SetFloat("_CloudDensity", this.Clouds.Density);
			this.Components.ShadowShader.SetFloat("_CloudSharpness", this.Clouds.Sharpness);
			this.Components.ShadowShader.SetVector("_CloudScale1", this.Clouds.Scale1);
			this.Components.ShadowShader.SetVector("_CloudScale2", this.Clouds.Scale2);
			this.Components.ShadowShader.SetVector("_CloudUV", vector);
		}
		if (this.Components.ShadowProjector != null)
		{
			bool enabled = this.Clouds.ShadowStrength != 0f && this.Components.ShadowShader != null;
			float farClipPlane = this.Radius * 2f;
			float radius = this.Radius;
			this.Components.ShadowProjector.enabled = enabled;
			this.Components.ShadowProjector.farClipPlane = farClipPlane;
			this.Components.ShadowProjector.orthographicSize = radius;
		}
	}

	// Token: 0x17000477 RID: 1143
	// (get) Token: 0x0600330F RID: 13071 RVA: 0x0002001B File Offset: 0x0001E21B
	// (set) Token: 0x06003310 RID: 13072 RVA: 0x00020023 File Offset: 0x0001E223
	internal TOD_Components Components { get; private set; }

	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06003311 RID: 13073 RVA: 0x0002002C File Offset: 0x0001E22C
	// (set) Token: 0x06003312 RID: 13074 RVA: 0x00020034 File Offset: 0x0001E234
	internal bool IsDay { get; private set; }

	// Token: 0x17000479 RID: 1145
	// (get) Token: 0x06003313 RID: 13075 RVA: 0x0002003D File Offset: 0x0001E23D
	// (set) Token: 0x06003314 RID: 13076 RVA: 0x00020045 File Offset: 0x0001E245
	internal bool IsNight { get; private set; }

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x06003315 RID: 13077 RVA: 0x0018A57C File Offset: 0x0018877C
	internal float Radius
	{
		get
		{
			return this.Components.DomeTransform.localScale.x;
		}
	}

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x06003316 RID: 13078 RVA: 0x0002004E File Offset: 0x0001E24E
	internal float Gamma
	{
		get
		{
			return ((this.UnityColorSpace != TOD_Sky.ColorSpaceDetection.Auto || QualitySettings.activeColorSpace != 1) && this.UnityColorSpace != TOD_Sky.ColorSpaceDetection.Linear) ? 2.2f : 1f;
		}
	}

	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x06003317 RID: 13079 RVA: 0x00020081 File Offset: 0x0001E281
	internal float OneOverGamma
	{
		get
		{
			return ((this.UnityColorSpace != TOD_Sky.ColorSpaceDetection.Auto || QualitySettings.activeColorSpace != 1) && this.UnityColorSpace != TOD_Sky.ColorSpaceDetection.Linear) ? 0.45454544f : 1f;
		}
	}

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x06003318 RID: 13080 RVA: 0x000200B4 File Offset: 0x0001E2B4
	// (set) Token: 0x06003319 RID: 13081 RVA: 0x000200BC File Offset: 0x0001E2BC
	internal float LerpValue { get; private set; }

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x0600331A RID: 13082 RVA: 0x000200C5 File Offset: 0x0001E2C5
	// (set) Token: 0x0600331B RID: 13083 RVA: 0x000200CD File Offset: 0x0001E2CD
	internal float SunZenith { get; private set; }

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x0600331C RID: 13084 RVA: 0x000200D6 File Offset: 0x0001E2D6
	// (set) Token: 0x0600331D RID: 13085 RVA: 0x000200DE File Offset: 0x0001E2DE
	internal float MoonZenith { get; private set; }

	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x0600331E RID: 13086 RVA: 0x000200E7 File Offset: 0x0001E2E7
	internal float LightZenith
	{
		get
		{
			return Mathf.Min(this.SunZenith, this.MoonZenith);
		}
	}

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x0600331F RID: 13087 RVA: 0x000200FA File Offset: 0x0001E2FA
	internal float LightIntensity
	{
		get
		{
			return this.Components.LightSource.intensity;
		}
	}

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x06003320 RID: 13088 RVA: 0x0002010C File Offset: 0x0001E30C
	// (set) Token: 0x06003321 RID: 13089 RVA: 0x00020114 File Offset: 0x0001E314
	internal Vector3 MoonDirection { get; private set; }

	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06003322 RID: 13090 RVA: 0x0002011D File Offset: 0x0001E31D
	// (set) Token: 0x06003323 RID: 13091 RVA: 0x00020125 File Offset: 0x0001E325
	internal Vector3 SunDirection { get; private set; }

	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06003324 RID: 13092 RVA: 0x0002012E File Offset: 0x0001E32E
	// (set) Token: 0x06003325 RID: 13093 RVA: 0x00020136 File Offset: 0x0001E336
	internal Vector3 LightDirection { get; private set; }

	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06003326 RID: 13094 RVA: 0x0002013F File Offset: 0x0001E33F
	// (set) Token: 0x06003327 RID: 13095 RVA: 0x00020147 File Offset: 0x0001E347
	internal Vector3 LocalMoonDirection { get; private set; }

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06003328 RID: 13096 RVA: 0x00020150 File Offset: 0x0001E350
	// (set) Token: 0x06003329 RID: 13097 RVA: 0x00020158 File Offset: 0x0001E358
	internal Vector3 LocalSunDirection { get; private set; }

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x0600332A RID: 13098 RVA: 0x00020161 File Offset: 0x0001E361
	// (set) Token: 0x0600332B RID: 13099 RVA: 0x00020169 File Offset: 0x0001E369
	internal Vector3 LocalLightDirection { get; private set; }

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x0600332C RID: 13100 RVA: 0x00020172 File Offset: 0x0001E372
	internal Color LightColor
	{
		get
		{
			return this.Components.LightSource.color;
		}
	}

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x0600332D RID: 13101 RVA: 0x00020184 File Offset: 0x0001E384
	// (set) Token: 0x0600332E RID: 13102 RVA: 0x0002018C File Offset: 0x0001E38C
	internal Color SunShaftColor { get; private set; }

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x0600332F RID: 13103 RVA: 0x00020195 File Offset: 0x0001E395
	// (set) Token: 0x06003330 RID: 13104 RVA: 0x0002019D File Offset: 0x0001E39D
	internal Color SunColor { get; private set; }

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x06003331 RID: 13105 RVA: 0x000201A6 File Offset: 0x0001E3A6
	// (set) Token: 0x06003332 RID: 13106 RVA: 0x000201AE File Offset: 0x0001E3AE
	internal Color MoonColor { get; private set; }

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06003333 RID: 13107 RVA: 0x000201B7 File Offset: 0x0001E3B7
	// (set) Token: 0x06003334 RID: 13108 RVA: 0x000201BF File Offset: 0x0001E3BF
	internal Color MoonHaloColor { get; private set; }

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06003335 RID: 13109 RVA: 0x000201C8 File Offset: 0x0001E3C8
	// (set) Token: 0x06003336 RID: 13110 RVA: 0x000201D0 File Offset: 0x0001E3D0
	internal Color CloudColor { get; private set; }

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x06003337 RID: 13111 RVA: 0x000201D9 File Offset: 0x0001E3D9
	// (set) Token: 0x06003338 RID: 13112 RVA: 0x000201E1 File Offset: 0x0001E3E1
	internal Color AdditiveColor { get; private set; }

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06003339 RID: 13113 RVA: 0x000201EA File Offset: 0x0001E3EA
	// (set) Token: 0x0600333A RID: 13114 RVA: 0x000201F2 File Offset: 0x0001E3F2
	internal Color AmbientColor { get; private set; }

	// Token: 0x0600333B RID: 13115 RVA: 0x0018A5A4 File Offset: 0x001887A4
	internal Vector3 OrbitalToUnity(float radius, float theta, float phi)
	{
		float num = Mathf.Sin(theta);
		float num2 = Mathf.Cos(theta);
		float num3 = Mathf.Sin(phi);
		float num4 = Mathf.Cos(phi);
		Vector3 result;
		result.z = radius * num * num4;
		result.y = radius * num2;
		result.x = radius * num * num3;
		return result;
	}

	// Token: 0x0600333C RID: 13116 RVA: 0x0018A5F4 File Offset: 0x001887F4
	internal Vector3 OrbitalToLocal(float theta, float phi)
	{
		float num = Mathf.Sin(theta);
		float y = Mathf.Cos(theta);
		float num2 = Mathf.Sin(phi);
		float num3 = Mathf.Cos(phi);
		Vector3 result;
		result.z = num * num3;
		result.y = y;
		result.x = num * num2;
		return result;
	}

	// Token: 0x0600333D RID: 13117 RVA: 0x0018A63C File Offset: 0x0018883C
	internal Color SampleAtmosphere(Vector3 direction, bool clampAlpha = true, bool directLight = true)
	{
		direction = this.Components.DomeTransform.InverseTransformDirection(direction);
		float horizonOffset = this.World.HorizonOffset;
		float power = this.Atmosphere.Contrast * 0.45454544f;
		float haziness = this.Atmosphere.Haziness;
		float fogginess = this.Atmosphere.Fogginess;
		Color sunColor = this.SunColor;
		Color moonColor = this.MoonColor;
		Color moonHaloColor = this.MoonHaloColor;
		Color cloudColor = this.CloudColor;
		Color additiveColor = this.AdditiveColor;
		Vector3 localSunDirection = this.LocalSunDirection;
		Vector3 localMoonDirection = this.LocalMoonDirection;
		Vector3 vector = this.opticalDepth;
		Vector3 vector2 = this.oneOverBeta;
		Vector3 vector3 = this.betaRayleigh;
		Vector3 vector4 = this.betaRayleighTheta;
		Vector3 vector5 = this.betaMie;
		Vector3 vector6 = this.betaMieTheta;
		Vector3 vector7 = this.betaMiePhase;
		Vector3 vector8 = this.betaNight;
		Color color = Color.black;
		float num = (!directLight) ? 0f : Mathf.Max(0f, Vector3.Dot(-direction, localSunDirection));
		float num2 = Mathf.Clamp(direction.y + horizonOffset, 0.001f, 1f);
		float num3 = Mathf.Pow(num2, haziness);
		float num4 = (1f - num3) * 190000f;
		float num5 = num4 + num3 * (vector.x - num4);
		float num6 = num4 + num3 * (vector.y - num4);
		float num7 = 1f + num * num;
		Vector3 vector9 = vector3 * num5 + vector5 * num6;
		Vector3 vector10 = vector4 + vector6 / Mathf.Pow(vector7.x - vector7.y * num, 1.5f);
		float r = sunColor.r;
		float g = sunColor.g;
		float b = sunColor.b;
		float r2 = moonColor.r;
		float g2 = moonColor.g;
		float b2 = moonColor.b;
		float num8 = Mathf.Exp(-vector9.x);
		float num9 = Mathf.Exp(-vector9.y);
		float num10 = Mathf.Exp(-vector9.z);
		float num11 = num7 * vector10.x * vector2.x;
		float num12 = num7 * vector10.y * vector2.y;
		float num13 = num7 * vector10.z * vector2.z;
		float x = vector8.x;
		float y = vector8.y;
		float z = vector8.z;
		color.r = (1f - num8) * (r * num11 + r2 * x);
		color.g = (1f - num9) * (g * num12 + g2 * y);
		color.b = (1f - num10) * (b * num13 + b2 * z);
		color.a = color.grayscale * 50f;
		color += moonHaloColor * Mathf.Pow(Mathf.Max(0f, Vector3.Dot(localMoonDirection, -direction)), 10f);
		color += additiveColor;
		color.r = Mathf.Lerp(color.r, cloudColor.r, fogginess);
		color.g = Mathf.Lerp(color.g, cloudColor.g, fogginess);
		color.b = Mathf.Lerp(color.b, cloudColor.b, fogginess);
		color.a += fogginess;
		if (clampAlpha)
		{
			color.a = Mathf.Clamp01(color.a);
		}
		color = TOD_Util.PowRGB(color, power);
		return color;
	}

	// Token: 0x0600333E RID: 13118 RVA: 0x0018A9D8 File Offset: 0x00188BD8
	internal Cubemap RenderToCubemap(int size, int faceMask = 63, float exposure = 1f, bool directLight = true)
	{
		Cubemap cubemap = new Cubemap(size, 5, true);
		this.RenderToCubemap(cubemap, faceMask, exposure, directLight);
		return cubemap;
	}

	// Token: 0x0600333F RID: 13119 RVA: 0x0018A9FC File Offset: 0x00188BFC
	internal void RenderToCubemap(Cubemap cubemap, int faceMask = 63, float exposure = 1f, bool directLight = true)
	{
		if (faceMask == 0)
		{
			return;
		}
		int width = cubemap.width;
		float num = (float)(width / 2);
		float num2 = num - 0.5f;
		Color[] array = new Color[width * width];
		if ((faceMask & 1) != 0)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < width; j++)
				{
					Vector3 vector;
					vector..ctor(num, num2 - (float)i, num2 - (float)j);
					Vector3 normalized = vector.normalized;
					Color color = Color.Lerp(this.SampleAtmosphere(normalized, true, directLight), this.AmbientColor, -normalized.y);
					array[i * width + j] = TOD_Util.Linear(TOD_Util.ExpRGB(color, exposure));
				}
			}
			cubemap.SetPixels(array, 0);
		}
		if ((faceMask & 2) != 0)
		{
			for (int k = 0; k < width; k++)
			{
				for (int l = 0; l < width; l++)
				{
					Vector3 vector2;
					vector2..ctor(-num, num2 - (float)k, (float)l - num2);
					Vector3 normalized2 = vector2.normalized;
					Color color2 = Color.Lerp(this.SampleAtmosphere(normalized2, true, directLight), this.AmbientColor, -normalized2.y);
					array[k * width + l] = TOD_Util.Linear(TOD_Util.ExpRGB(color2, exposure));
				}
			}
			cubemap.SetPixels(array, 1);
		}
		if ((faceMask & 4) != 0)
		{
			for (int m = 0; m < width; m++)
			{
				for (int n = 0; n < width; n++)
				{
					Vector3 vector3;
					vector3..ctor((float)n - num2, num, (float)m - num2);
					Vector3 normalized3 = vector3.normalized;
					Color color3 = Color.Lerp(this.SampleAtmosphere(normalized3, true, directLight), this.AmbientColor, -normalized3.y);
					array[m * width + n] = TOD_Util.Linear(TOD_Util.ExpRGB(color3, exposure));
				}
			}
			cubemap.SetPixels(array, 2);
		}
		if ((faceMask & 8) != 0)
		{
			for (int num3 = 0; num3 < width; num3++)
			{
				for (int num4 = 0; num4 < width; num4++)
				{
					Vector3 vector4;
					vector4..ctor((float)num4 - num2, -num, num2 - (float)num3);
					Vector3 normalized4 = vector4.normalized;
					Color color4 = Color.Lerp(this.SampleAtmosphere(normalized4, true, directLight), this.AmbientColor, -normalized4.y);
					array[num3 * width + num4] = TOD_Util.Linear(TOD_Util.ExpRGB(color4, exposure));
				}
			}
			cubemap.SetPixels(array, 3);
		}
		if ((faceMask & 16) != 0)
		{
			for (int num5 = 0; num5 < width; num5++)
			{
				for (int num6 = 0; num6 < width; num6++)
				{
					Vector3 vector5;
					vector5..ctor((float)num6 - num2, num2 - (float)num5, num);
					Vector3 normalized5 = vector5.normalized;
					Color color5 = Color.Lerp(this.SampleAtmosphere(normalized5, true, directLight), this.AmbientColor, -normalized5.y);
					array[num5 * width + num6] = TOD_Util.Linear(TOD_Util.ExpRGB(color5, exposure));
				}
			}
			cubemap.SetPixels(array, 4);
		}
		if ((faceMask & 32) != 0)
		{
			for (int num7 = 0; num7 < width; num7++)
			{
				for (int num8 = 0; num8 < width; num8++)
				{
					Vector3 vector6;
					vector6..ctor(num2 - (float)num8, num2 - (float)num7, -num);
					Vector3 normalized6 = vector6.normalized;
					Color color6 = Color.Lerp(this.SampleAtmosphere(normalized6, true, directLight), this.AmbientColor, -normalized6.y);
					array[num7 * width + num8] = TOD_Util.Linear(TOD_Util.ExpRGB(color6, exposure));
				}
			}
			cubemap.SetPixels(array, 5);
		}
		cubemap.Apply();
	}

	// Token: 0x06003340 RID: 13120 RVA: 0x0018ADBC File Offset: 0x00188FBC
	internal Color SampleFogColor(bool directLight = true)
	{
		Vector3 vector = (!(this.Components.CameraTransform != null)) ? Vector3.forward : this.Components.CameraTransform.forward;
		Color color = this.SampleAtmosphere(Vector3.Lerp(new Vector3(vector.x, 0f, vector.z), Vector3.up, this.Fog.HeightBias).normalized, true, directLight);
		float a = color.a;
		return new Color(a * color.r, a * color.g, a * color.b, 1f);
	}

	// Token: 0x06003341 RID: 13121 RVA: 0x0018AE68 File Offset: 0x00189068
	internal Color SampleSkyColor()
	{
		Vector3 sunDirection = this.SunDirection;
		sunDirection.y = Mathf.Abs(sunDirection.y);
		Color color = this.SampleAtmosphere(sunDirection.normalized, true, false);
		float num = color.a * Mathf.Lerp(this.Night.AmbientColor.a, this.Day.AmbientColor.a, this.LerpValue);
		return new Color(num * color.r, num * color.g, num * color.b, 1f);
	}

	// Token: 0x06003342 RID: 13122 RVA: 0x0018AEF8 File Offset: 0x001890F8
	internal Color SampleEquatorColor()
	{
		Vector3 sunDirection = this.SunDirection;
		sunDirection.y = 0f;
		Color color = this.SampleAtmosphere(sunDirection.normalized, true, false);
		float num = color.a * Mathf.Lerp(this.Night.AmbientColor.a, this.Day.AmbientColor.a, this.LerpValue);
		return new Color(num * color.r, num * color.g, num * color.b, 1f);
	}

	// Token: 0x06003343 RID: 13123 RVA: 0x0018AF84 File Offset: 0x00189184
	private void SetupScattering()
	{
		float num = 0.001f + this.Atmosphere.RayleighMultiplier * this.Atmosphere.ScatteringColor.r;
		float num2 = 0.001f + this.Atmosphere.RayleighMultiplier * this.Atmosphere.ScatteringColor.g;
		float num3 = 0.001f + this.Atmosphere.RayleighMultiplier * this.Atmosphere.ScatteringColor.b;
		this.betaRayleigh.x = 5.8E-06f * num;
		this.betaRayleigh.y = 1.35E-05f * num2;
		this.betaRayleigh.z = 3.31E-05f * num3;
		this.betaRayleighTheta.x = 0.000116f * num * 0.059683103f;
		this.betaRayleighTheta.y = 0.00027f * num2 * 0.059683103f;
		this.betaRayleighTheta.z = 0.00066200003f * num3 * 0.059683103f;
		this.opticalDepth.x = 8000f * Mathf.Exp(-this.World.ViewerHeight * 50000f / 8000f);
		float num4 = 0.001f + this.Atmosphere.MieMultiplier * this.Atmosphere.ScatteringColor.r;
		float num5 = 0.001f + this.Atmosphere.MieMultiplier * this.Atmosphere.ScatteringColor.g;
		float num6 = 0.001f + this.Atmosphere.MieMultiplier * this.Atmosphere.ScatteringColor.b;
		float directionality = this.Atmosphere.Directionality;
		float num7 = 0.23873241f * (1f - directionality * directionality) / (2f + directionality * directionality);
		this.betaMie.x = 2E-06f * num4;
		this.betaMie.y = 2E-06f * num5;
		this.betaMie.z = 2E-06f * num6;
		this.betaMieTheta.x = 4E-05f * num4 * num7;
		this.betaMieTheta.y = 4E-05f * num5 * num7;
		this.betaMieTheta.z = 4E-05f * num6 * num7;
		this.betaMiePhase.x = 1f + directionality * directionality;
		this.betaMiePhase.y = 2f * directionality;
		this.opticalDepth.y = 1200f * Mathf.Exp(-this.World.ViewerHeight * 50000f / 1200f);
		this.oneOverBeta = TOD_Util.Inverse(this.betaMie + this.betaRayleigh);
		this.betaNight = Vector3.Scale(this.betaRayleighTheta + this.betaMieTheta / Mathf.Pow(this.betaMiePhase.x, 1.5f), this.oneOverBeta);
		this.oneOverBeta *= Mathf.Lerp(1f, Mathf.Lerp(1f, 0.1f, Mathf.Sqrt(this.SunZenith / 90f) - 0.25f), this.Atmosphere.FakeHDR);
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x0018B2C0 File Offset: 0x001894C0
	private void SetupSunAndMoon()
	{
		float num = 0.017453292f * this.Cycle.Latitude;
		float num2 = Mathf.Sin(num);
		float num3 = Mathf.Cos(num);
		float longitude = this.Cycle.Longitude;
		float num4 = (float)(367 * this.Cycle.Year - 7 * (this.Cycle.Year + (this.Cycle.Month + 9) / 12) / 4 + 275 * this.Cycle.Month / 9 + this.Cycle.Day - 730530);
		float num5 = this.Cycle.Hour - this.Cycle.UTC;
		float num6 = 23.4393f - 3.563E-07f * num4;
		float num7 = 0.017453292f * num6;
		float num8 = Mathf.Sin(num7);
		float num9 = Mathf.Cos(num7);
		float num10 = 282.9404f + 4.70935E-05f * num4;
		float num11 = 0.016709f - 1.151E-09f * num4;
		float num12 = 356.047f + 0.98560023f * num4;
		float num13 = 0.017453292f * num12;
		float num14 = Mathf.Sin(num13);
		float num15 = Mathf.Cos(num13);
		float num16 = num12 + num11 * 57.29578f * num14 * (1f + num11 * num15);
		float num17 = 0.017453292f * num16;
		float num18 = Mathf.Sin(num17);
		float num19 = Mathf.Cos(num17);
		float num20 = num19 - num11;
		float num21 = num18 * Mathf.Sqrt(1f - num11 * num11);
		float num22 = 57.29578f * Mathf.Atan2(num21, num20);
		float num23 = Mathf.Sqrt(num20 * num20 + num21 * num21);
		float num24 = num22 + num10;
		float num25 = 0.017453292f * num24;
		float num26 = Mathf.Sin(num25);
		float num27 = Mathf.Cos(num25);
		float num28 = num23 * num27;
		float num29 = num23 * num26;
		float num30 = num28;
		float num31 = num29 * num9;
		float num32 = num29 * num8;
		float num33 = Mathf.Atan2(num31, num30);
		float num34 = 57.29578f * num33;
		float num35 = Mathf.Atan2(num32, Mathf.Sqrt(num30 * num30 + num31 * num31));
		float num36 = Mathf.Sin(num35);
		float num37 = Mathf.Cos(num35);
		float num38 = num22 + num10 + 180f;
		float num39 = num38 + num5 * 15f;
		float num40 = num39 + longitude;
		float num41 = num40 - num34;
		float num42 = 0.017453292f * num41;
		float num43 = Mathf.Sin(num42);
		float num44 = Mathf.Cos(num42);
		float num45 = num44 * num37;
		float num46 = num43 * num37;
		float num47 = num36;
		float num48 = num45 * num2 - num47 * num3;
		float num49 = num46;
		float num50 = num45 * num3 + num47 * num2;
		float num51 = Mathf.Atan2(num49, num48) + 3.1415927f;
		float num52 = Mathf.Atan2(num50, Mathf.Sqrt(num48 * num48 + num49 * num49));
		float num53 = 1.5707964f - num52;
		float num54 = num51;
		float num108;
		float num109;
		if (this.Moon.Position == TOD_MoonPositionType.Realistic)
		{
			float num55 = 125.1228f - 0.05295381f * num4;
			float num56 = 5.1454f;
			float num57 = 318.0634f + 0.16435732f * num4;
			float num58 = 60.2666f;
			float num59 = 0.0549f;
			float num60 = 115.3654f + 13.064993f * num4;
			float num61 = 0.017453292f * num55;
			float num62 = Mathf.Sin(num61);
			float num63 = Mathf.Cos(num61);
			float num64 = 0.017453292f * num56;
			float num65 = Mathf.Sin(num64);
			float num66 = Mathf.Cos(num64);
			float num67 = 0.017453292f * num60;
			float num68 = Mathf.Sin(num67);
			float num69 = Mathf.Cos(num67);
			float num70 = num60 + num59 * 57.29578f * num68 * (1f + num59 * num69);
			float num71 = 0.017453292f * num70;
			float num72 = Mathf.Sin(num71);
			float num73 = Mathf.Cos(num71);
			float num74 = num58 * (num73 - num59);
			float num75 = num58 * (num72 * Mathf.Sqrt(1f - num59 * num59));
			float num76 = 57.29578f * Mathf.Atan2(num75, num74);
			float num77 = Mathf.Sqrt(num74 * num74 + num75 * num75);
			float num78 = num76 + num57;
			float num79 = 0.017453292f * num78;
			float num80 = Mathf.Sin(num79);
			float num81 = Mathf.Cos(num79);
			float num82 = num77 * (num63 * num81 - num62 * num80 * num66);
			float num83 = num77 * (num62 * num81 + num63 * num80 * num66);
			float num84 = num77 * (num80 * num65);
			float num85 = num82;
			float num86 = num83 * num9 - num84 * num8;
			float num87 = num84 * num8 + num84 * num9;
			float num88 = Mathf.Atan2(num86, num85);
			float num89 = 57.29578f * num88;
			float num90 = Mathf.Atan2(num87, Mathf.Sqrt(num85 * num85 + num86 * num86));
			float num91 = Mathf.Sin(num90);
			float num92 = Mathf.Cos(num90);
			float num93 = num76 + num57 + 180f;
			float num94 = num93 + num5 * 15f;
			float num95 = num94 + longitude;
			float num96 = num95 - num89;
			float num97 = 0.017453292f * num96;
			float num98 = Mathf.Sin(num97);
			float num99 = Mathf.Cos(num97);
			float num100 = num99 * num92;
			float num101 = num98 * num92;
			float num102 = num91;
			float num103 = num100 * num2 - num102 * num3;
			float num104 = num101;
			float num105 = num100 * num3 + num102 * num2;
			float num106 = Mathf.Atan2(num104, num103) + 3.1415927f;
			float num107 = Mathf.Atan2(num105, Mathf.Sqrt(num103 * num103 + num104 * num104));
			num108 = 1.5707964f - num107;
			num109 = num106;
		}
		else
		{
			num108 = num53 - 3.1415927f;
			num109 = num54;
		}
		Quaternion localRotation = Quaternion.identity;
		if (this.Stars.Position == TOD_StarsPositionType.Rotating)
		{
			localRotation = Quaternion.Euler(90f - this.Cycle.Latitude, 0f, 0f) * Quaternion.Euler(0f, this.Cycle.Longitude, 0f) * Quaternion.Euler(0f, this.Cycle.Hour / 24f * 360f, 0f);
		}
		this.Components.SpaceTransform.localRotation = localRotation;
		Vector3 localPosition = this.OrbitalToLocal(num53, num54);
		this.Components.SunTransform.localPosition = localPosition;
		this.Components.SunTransform.LookAt(this.Components.DomeTransform.position, this.Components.SunTransform.up);
		if (this.Components.CameraTransform != null)
		{
			Vector3 eulerAngles = this.Components.CameraTransform.rotation.eulerAngles;
			Vector3 localEulerAngles = this.Components.SunTransform.localEulerAngles;
			localEulerAngles.z = 2f * Time.time + Mathf.Abs(eulerAngles.x) + Mathf.Abs(eulerAngles.y) + Mathf.Abs(eulerAngles.z);
			this.Components.SunTransform.localEulerAngles = localEulerAngles;
		}
		Vector3 localPosition2 = this.OrbitalToLocal(num108, num109);
		this.Components.MoonTransform.localPosition = localPosition2;
		this.Components.MoonTransform.LookAt(this.Components.DomeTransform.position, this.Components.MoonTransform.up);
		float num110 = 4f * Mathf.Tan(0.008726646f * this.Sun.MeshSize);
		float num111 = 2f * num110;
		Vector3 localScale;
		localScale..ctor(num111, num111, num111);
		this.Components.SunTransform.localScale = localScale;
		float num112 = 2f * Mathf.Tan(0.008726646f * this.Moon.MeshSize);
		float num113 = 2f * num112;
		Vector3 localScale2;
		localScale2..ctor(num113, num113, num113);
		this.Components.MoonTransform.localScale = localScale2;
		this.SunZenith = 57.29578f * num53;
		this.MoonZenith = 57.29578f * num108;
		bool enabled = this.LerpValue > 0f;
		bool enabled2 = this.Components.MoonTransform.localPosition.y > -num113;
		bool enabled3 = this.SampleAtmosphere(Vector3.up, false, true).a < 2f;
		bool enabled4 = this.Clouds.Density > 0f;
		this.Components.SunRenderer.enabled = enabled;
		this.Components.MoonRenderer.enabled = enabled2;
		this.Components.SpaceRenderer.enabled = enabled3;
		this.Components.CloudRenderer.enabled = enabled4;
		this.SetupLightSource(num53, num54, num108, num109);
	}

	// Token: 0x06003345 RID: 13125 RVA: 0x0018BB3C File Offset: 0x00189D3C
	private void SetupLightSource(float sun_theta, float sun_phi, float moon_theta, float moon_phi)
	{
		float num = Mathf.Cos(Mathf.Pow(sun_theta / 6.2831855f, 2f - this.Light.Falloff) * 2f * 3.1415927f);
		float num2 = Mathf.Sqrt(501264f * num * num + 1416f + 1f) - 708f * num;
		float num3 = this.Sun.LightColor.r;
		float num4 = this.Sun.LightColor.g;
		float num5 = this.Sun.LightColor.b;
		num3 *= Mathf.Exp(-0.008735f * Mathf.Pow(0.68f, -4.08f * num2));
		num4 *= Mathf.Exp(-0.008735f * Mathf.Pow(0.55f, -4.08f * num2));
		num5 *= Mathf.Exp(-0.008735f * Mathf.Pow(0.44f, -4.08f * num2));
		float num6 = 1.1f;
		Color color;
		color..ctor(num3, num4, num5);
		this.LerpValue = Mathf.Clamp01(num6 * color.grayscale);
		float num7 = this.Moon.LightColor.r * this.Moon.LightColor.a;
		float num8 = this.Moon.LightColor.g * this.Moon.LightColor.a;
		float num9 = this.Moon.LightColor.b * this.Moon.LightColor.a;
		float num10 = this.Sun.LightColor.r * this.Sun.LightColor.a * Mathf.Lerp(1f, num3, this.Light.Coloring);
		float num11 = this.Sun.LightColor.g * this.Sun.LightColor.a * Mathf.Lerp(1f, num4, this.Light.Coloring);
		float num12 = this.Sun.LightColor.b * this.Sun.LightColor.a * Mathf.Lerp(1f, num5, this.Light.Coloring);
		float num13 = this.Moon.LightColor.r * this.Moon.LightColor.a;
		float num14 = this.Moon.LightColor.g * this.Moon.LightColor.a;
		float num15 = this.Moon.LightColor.b * this.Moon.LightColor.a;
		float num16 = this.Sun.LightColor.r * this.Sun.LightColor.a * Mathf.Lerp(1f, num3, this.Light.SkyColoring);
		float num17 = this.Sun.LightColor.g * this.Sun.LightColor.a * Mathf.Lerp(1f, num4, this.Light.SkyColoring);
		float num18 = this.Sun.LightColor.b * this.Sun.LightColor.a * Mathf.Lerp(1f, num5, this.Light.SkyColoring);
		float num19 = this.Sun.ShaftColor.r * this.Sun.ShaftColor.a * Mathf.Lerp(1f, num3, this.Light.ShaftColoring);
		float num20 = this.Sun.ShaftColor.g * this.Sun.ShaftColor.a * Mathf.Lerp(1f, num4, this.Light.ShaftColoring);
		float num21 = this.Sun.ShaftColor.b * this.Sun.ShaftColor.a * Mathf.Lerp(1f, num5, this.Light.ShaftColoring);
		float num22 = this.Night.AmbientColor.r * this.Night.AmbientColor.a;
		float num23 = this.Night.AmbientColor.g * this.Night.AmbientColor.a;
		float num24 = this.Night.AmbientColor.b * this.Night.AmbientColor.a;
		float num25 = this.Day.AmbientColor.r * this.Day.AmbientColor.a * Mathf.Lerp(1f, num3, this.Light.AmbientColoring);
		float num26 = this.Day.AmbientColor.g * this.Day.AmbientColor.a * Mathf.Lerp(1f, num4, this.Light.AmbientColoring);
		float num27 = this.Day.AmbientColor.b * this.Day.AmbientColor.a * Mathf.Lerp(1f, num5, this.Light.AmbientColoring);
		float num28 = this.Night.CloudColor.r * this.Night.CloudColor.a;
		float num29 = this.Night.CloudColor.g * this.Night.CloudColor.a;
		float num30 = this.Night.CloudColor.b * this.Night.CloudColor.a;
		float num31 = this.Day.CloudColor.r * this.Day.CloudColor.a * Mathf.Lerp(1f, num3, this.Light.CloudColoring);
		float num32 = this.Day.CloudColor.g * this.Day.CloudColor.a * Mathf.Lerp(1f, num4, this.Light.CloudColoring);
		float num33 = this.Day.CloudColor.b * this.Day.CloudColor.a * Mathf.Lerp(1f, num5, this.Light.CloudColoring);
		Color color2;
		color2..ctor(num7, num8, num9, 1f);
		Color color3;
		color3..ctor(num10, num11, num12, 1f);
		float lerpValue = this.LerpValue;
		this.SunShaftColor = new Color(num19 * lerpValue, num20 * lerpValue, num21 * lerpValue, this.LerpValue);
		float num34 = this.Atmosphere.Brightness * this.Night.SkyMultiplier * (1f - this.LerpValue);
		float num35 = this.Atmosphere.Brightness * this.Day.SkyMultiplier * this.LerpValue;
		this.MoonColor = new Color(num13 * num34, num14 * num34, num15 * num34, 1f - this.LerpValue);
		this.SunColor = new Color(num16 * num35, num17 * num35, num18 * num35, this.LerpValue);
		float num36 = this.Clouds.Brightness * this.Night.CloudMultiplier * (1f - this.LerpValue);
		float num37 = this.Clouds.Brightness * this.Day.CloudMultiplier * this.LerpValue;
		Color color4;
		color4..ctor(num28 * num36, num29 * num36, num30 * num36, 1f);
		Color color5;
		color5..ctor(num31 * num37, num32 * num37, num33 * num37, 1f);
		this.CloudColor = Color.Lerp(color4, color5, this.LerpValue);
		Color moonHaloColor = (1f - this.LerpValue) * (1f - Mathf.Abs(this.Moon.Phase)) * this.Atmosphere.Brightness * this.Moon.HaloColor;
		moonHaloColor.r *= moonHaloColor.a;
		moonHaloColor.g *= moonHaloColor.a;
		moonHaloColor.b *= moonHaloColor.a;
		moonHaloColor.a = moonHaloColor.grayscale;
		this.MoonHaloColor = moonHaloColor;
		Color additiveColor = Color.Lerp(this.Night.AdditiveColor, this.Day.AdditiveColor, this.LerpValue);
		additiveColor.r *= additiveColor.a;
		additiveColor.g *= additiveColor.a;
		additiveColor.b *= additiveColor.a;
		additiveColor.a = additiveColor.grayscale;
		this.AdditiveColor = additiveColor;
		Color color6;
		color6..ctor(num22, num23, num24, 1f);
		Color color7;
		color7..ctor(num25, num26, num27, 1f);
		this.AmbientColor = Color.Lerp(color6, color7, this.LerpValue);
		float intensity;
		float shadowStrength;
		Vector3 localPosition;
		if (this.LerpValue > 0.25f)
		{
			this.IsDay = true;
			this.IsNight = false;
			float num38 = (this.LerpValue - 0.25f) / 0.75f;
			intensity = Mathf.Lerp(0f, this.Sun.LightIntensity, num38);
			shadowStrength = this.Sun.ShadowStrength;
			localPosition = this.OrbitalToLocal(Mathf.Min(sun_theta, (1f - this.Light.MinimumHeight) * 3.1415927f / 2f), sun_phi);
			this.Components.LightSource.color = color3;
		}
		else
		{
			this.IsDay = false;
			this.IsNight = true;
			float num39 = (0.25f - this.LerpValue) / 0.25f;
			float num40 = 1f - Mathf.Abs(this.Moon.Phase);
			intensity = Mathf.Lerp(0f, this.Moon.LightIntensity * num40, num39);
			shadowStrength = this.Moon.ShadowStrength;
			localPosition = this.OrbitalToLocal(Mathf.Min(moon_theta, (1f - this.Light.MinimumHeight) * 3.1415927f / 2f), moon_phi);
			this.Components.LightSource.color = color2;
		}
		this.Components.LightSource.intensity = intensity;
		this.Components.LightSource.shadowStrength = shadowStrength;
		if (!Application.isPlaying || this.timeSinceLightUpdate >= this.Light.UpdateInterval)
		{
			this.timeSinceLightUpdate = 0f;
			this.Components.LightTransform.localPosition = localPosition;
			this.Components.LightTransform.LookAt(this.Components.DomeTransform.position);
		}
		else
		{
			this.timeSinceLightUpdate += Time.deltaTime;
		}
		this.SunDirection = this.Components.SunTransform.forward;
		this.LocalSunDirection = this.Components.DomeTransform.InverseTransformDirection(this.SunDirection);
		this.MoonDirection = this.Components.MoonTransform.forward;
		this.LocalMoonDirection = this.Components.DomeTransform.InverseTransformDirection(this.MoonDirection);
		this.LightDirection = Vector3.Lerp(this.MoonDirection, this.SunDirection, this.LerpValue * this.LerpValue);
		this.LocalLightDirection = this.Components.DomeTransform.InverseTransformDirection(this.LightDirection);
	}

	// Token: 0x04003EFF RID: 16127
	private const float pi = 3.1415927f;

	// Token: 0x04003F00 RID: 16128
	private const float pi2 = 9.869605f;

	// Token: 0x04003F01 RID: 16129
	private const float pi3 = 31.006279f;

	// Token: 0x04003F02 RID: 16130
	private const float pi4 = 97.4091f;

	// Token: 0x04003F03 RID: 16131
	private static List<TOD_Sky> instances = new List<TOD_Sky>();

	// Token: 0x04003F04 RID: 16132
	private Vector2 opticalDepth;

	// Token: 0x04003F05 RID: 16133
	private Vector3 oneOverBeta;

	// Token: 0x04003F06 RID: 16134
	private Vector3 betaRayleigh;

	// Token: 0x04003F07 RID: 16135
	private Vector3 betaRayleighTheta;

	// Token: 0x04003F08 RID: 16136
	private Vector3 betaMie;

	// Token: 0x04003F09 RID: 16137
	private Vector3 betaMieTheta;

	// Token: 0x04003F0A RID: 16138
	private Vector2 betaMiePhase;

	// Token: 0x04003F0B RID: 16139
	private Vector3 betaNight;

	// Token: 0x04003F0C RID: 16140
	private float timeSinceLightUpdate = float.MaxValue;

	// Token: 0x04003F0D RID: 16141
	private float timeSinceFogUpdate = float.MaxValue;

	// Token: 0x04003F0E RID: 16142
	private float timeSinceAmbientUpdate = float.MaxValue;

	// Token: 0x04003F0F RID: 16143
	private float timeSinceReflectionUpdate = float.MaxValue;

	// Token: 0x04003F10 RID: 16144
	public bool m_bDontUpdateAmbient;

	// Token: 0x04003F11 RID: 16145
	public TOD_Sky.ColorSpaceDetection UnityColorSpace;

	// Token: 0x04003F12 RID: 16146
	public TOD_Sky.CloudQualityType CloudQuality = TOD_Sky.CloudQualityType.Bumped;

	// Token: 0x04003F13 RID: 16147
	public TOD_Sky.MeshQualityType MeshQuality = TOD_Sky.MeshQualityType.High;

	// Token: 0x04003F14 RID: 16148
	public TOD_CycleParameters Cycle;

	// Token: 0x04003F15 RID: 16149
	public TOD_AtmosphereParameters Atmosphere;

	// Token: 0x04003F16 RID: 16150
	public TOD_DayParameters Day;

	// Token: 0x04003F17 RID: 16151
	public TOD_NightParameters Night;

	// Token: 0x04003F18 RID: 16152
	public TOD_SunParameters Sun;

	// Token: 0x04003F19 RID: 16153
	public TOD_MoonParameters Moon;

	// Token: 0x04003F1A RID: 16154
	public TOD_LightParameters Light;

	// Token: 0x04003F1B RID: 16155
	public TOD_StarParameters Stars;

	// Token: 0x04003F1C RID: 16156
	public TOD_CloudParameters Clouds;

	// Token: 0x04003F1D RID: 16157
	public TOD_WorldParameters World;

	// Token: 0x04003F1E RID: 16158
	public TOD_FogParameters Fog;

	// Token: 0x04003F1F RID: 16159
	public TOD_AmbientParameters Ambient;

	// Token: 0x04003F20 RID: 16160
	public TOD_ReflectionParameters Reflection;

	// Token: 0x02000833 RID: 2099
	public enum ColorSpaceDetection
	{
		// Token: 0x04003F35 RID: 16181
		Auto,
		// Token: 0x04003F36 RID: 16182
		Linear,
		// Token: 0x04003F37 RID: 16183
		Gamma
	}

	// Token: 0x02000834 RID: 2100
	public enum CloudQualityType
	{
		// Token: 0x04003F39 RID: 16185
		Fastest,
		// Token: 0x04003F3A RID: 16186
		Density,
		// Token: 0x04003F3B RID: 16187
		Bumped
	}

	// Token: 0x02000835 RID: 2101
	public enum MeshQualityType
	{
		// Token: 0x04003F3D RID: 16189
		Low,
		// Token: 0x04003F3E RID: 16190
		Medium,
		// Token: 0x04003F3F RID: 16191
		High
	}
}
