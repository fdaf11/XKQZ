using System;
using UnityEngine;

// Token: 0x02000542 RID: 1346
[Serializable]
public class ReliefTerrainGlobalSettingsHolder
{
	// Token: 0x06002219 RID: 8729 RVA: 0x00103E90 File Offset: 0x00102090
	public ReliefTerrainGlobalSettingsHolder()
	{
		this.gloss_baked = new RTPGlossBaked[12];
		this.Bumps = new Texture2D[12];
		this.Heights = new Texture2D[12];
		this.Spec = new float[12];
		this.FarSpecCorrection = new float[12];
		this.MIPmult = new float[12];
		this.MixScale = new float[12];
		this.MixBlend = new float[12];
		this.MixSaturation = new float[12];
		this.RTP_gloss2mask = new float[12];
		this.RTP_gloss_mult = new float[12];
		this.RTP_gloss_shaping = new float[12];
		this.RTP_Fresnel = new float[12];
		this.RTP_FresnelAtten = new float[12];
		this.RTP_DiffFresnel = new float[12];
		this.RTP_IBL_bump_smoothness = new float[12];
		this.RTP_IBL_DiffuseStrength = new float[12];
		this.RTP_IBL_SpecStrength = new float[12];
		this._DeferredSpecDampAddPass = new float[12];
		this.MixBrightness = new float[12];
		this.MixReplace = new float[12];
		this.LayerBrightness = new float[12];
		this.LayerBrightness2Spec = new float[12];
		this.LayerAlbedo2SpecColor = new float[12];
		this.LayerSaturation = new float[12];
		this.LayerEmission = new float[12];
		this.LayerEmissionColor = new Color[12];
		this.LayerEmissionRefractStrength = new float[12];
		this.LayerEmissionRefractHBedge = new float[12];
		this.GlobalColorPerLayer = new float[12];
		this.GlobalColorBottom = new float[12];
		this.GlobalColorTop = new float[12];
		this.GlobalColorColormapLoSat = new float[12];
		this.GlobalColorColormapHiSat = new float[12];
		this.GlobalColorLayerLoSat = new float[12];
		this.GlobalColorLayerHiSat = new float[12];
		this.GlobalColorLoBlend = new float[12];
		this.GlobalColorHiBlend = new float[12];
		this.PER_LAYER_HEIGHT_MODIFIER = new float[12];
		this._snow_strength_per_layer = new float[12];
		this.Substances = new ProceduralMaterial[12];
		this._SuperDetailStrengthMultA = new float[12];
		this._SuperDetailStrengthMultASelfMaskNear = new float[12];
		this._SuperDetailStrengthMultASelfMaskFar = new float[12];
		this._SuperDetailStrengthMultB = new float[12];
		this._SuperDetailStrengthMultBSelfMaskNear = new float[12];
		this._SuperDetailStrengthMultBSelfMaskFar = new float[12];
		this._SuperDetailStrengthNormal = new float[12];
		this._BumpMapGlobalStrength = new float[12];
		this.AO_strength = new float[12];
		this.VerticalTextureStrength = new float[12];
		this.TERRAIN_LayerWetStrength = new float[12];
		this.TERRAIN_WaterLevel = new float[12];
		this.TERRAIN_WaterLevelSlopeDamp = new float[12];
		this.TERRAIN_WaterEdge = new float[12];
		this.TERRAIN_WaterSpecularity = new float[12];
		this.TERRAIN_WaterGloss = new float[12];
		this.TERRAIN_WaterGlossDamper = new float[12];
		this.TERRAIN_WaterOpacity = new float[12];
		this.TERRAIN_Refraction = new float[12];
		this.TERRAIN_WetRefraction = new float[12];
		this.TERRAIN_Flow = new float[12];
		this.TERRAIN_WetFlow = new float[12];
		this.TERRAIN_WetSpecularity = new float[12];
		this.TERRAIN_WetGloss = new float[12];
		this.TERRAIN_WaterColor = new Color[12];
		this.TERRAIN_WaterIBL_SpecWetStrength = new float[12];
		this.TERRAIN_WaterIBL_SpecWaterStrength = new float[12];
		this.TERRAIN_WaterEmission = new float[12];
	}

	// Token: 0x0600221A RID: 8730 RVA: 0x00104458 File Offset: 0x00102658
	public void ReInit(Terrain terrainComp)
	{
		if (terrainComp.terrainData.splatPrototypes.Length > this.numLayers)
		{
			Texture2D[] array = new Texture2D[terrainComp.terrainData.splatPrototypes.Length];
			for (int i = 0; i < this.splats.Length; i++)
			{
				array[i] = this.splats[i];
			}
			this.splats = array;
			this.splats[terrainComp.terrainData.splatPrototypes.Length - 1] = terrainComp.terrainData.splatPrototypes[(terrainComp.terrainData.splatPrototypes.Length - 2 < 0) ? 0 : (terrainComp.terrainData.splatPrototypes.Length - 2)].texture;
		}
		else if (terrainComp.terrainData.splatPrototypes.Length < this.numLayers)
		{
			Texture2D[] array2 = new Texture2D[terrainComp.terrainData.splatPrototypes.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = this.splats[j];
			}
			this.splats = array2;
		}
		this.numLayers = terrainComp.terrainData.splatPrototypes.Length;
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x00016C36 File Offset: 0x00014E36
	public void SetShaderParam(string name, Texture2D tex)
	{
		if (!tex)
		{
			return;
		}
		if (this.use_mat)
		{
			this.use_mat.SetTexture(name, tex);
		}
		else
		{
			Shader.SetGlobalTexture(name, tex);
		}
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x00016C36 File Offset: 0x00014E36
	public void SetShaderParam(string name, Cubemap tex)
	{
		if (!tex)
		{
			return;
		}
		if (this.use_mat)
		{
			this.use_mat.SetTexture(name, tex);
		}
		else
		{
			Shader.SetGlobalTexture(name, tex);
		}
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x00016C6D File Offset: 0x00014E6D
	public void SetShaderParam(string name, Matrix4x4 mtx)
	{
		if (this.use_mat)
		{
			this.use_mat.SetMatrix(name, mtx);
		}
		else
		{
			Shader.SetGlobalMatrix(name, mtx);
		}
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x00016C98 File Offset: 0x00014E98
	public void SetShaderParam(string name, Vector4 vec)
	{
		if (this.use_mat)
		{
			this.use_mat.SetVector(name, vec);
		}
		else
		{
			Shader.SetGlobalVector(name, vec);
		}
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x00016CC3 File Offset: 0x00014EC3
	public void SetShaderParam(string name, float val)
	{
		if (this.use_mat)
		{
			this.use_mat.SetFloat(name, val);
		}
		else
		{
			Shader.SetGlobalFloat(name, val);
		}
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x00016CEE File Offset: 0x00014EEE
	public void SetShaderParam(string name, Color col)
	{
		if (this.use_mat)
		{
			this.use_mat.SetColor(name, col);
		}
		else
		{
			Shader.SetGlobalColor(name, col);
		}
	}

	// Token: 0x06002221 RID: 8737 RVA: 0x00016D19 File Offset: 0x00014F19
	public RTP_LODmanager Get_RTP_LODmanagerScript()
	{
		return this._RTP_LODmanagerScript;
	}

	// Token: 0x06002222 RID: 8738 RVA: 0x00104578 File Offset: 0x00102778
	public void ApplyGlossBakedTexture(string shaderParamName, int i)
	{
		if (this.gloss_baked == null || this.gloss_baked.Length == 0)
		{
			this.gloss_baked = new RTPGlossBaked[12];
		}
		if (this.splats_glossBaked[i] == null)
		{
			if (this.gloss_baked[i] != null && !this.gloss_baked[i].used_in_atlas && this.gloss_baked[i].CheckSize(this.splats[i]))
			{
				this.splats_glossBaked[i] = this.gloss_baked[i].MakeTexture(this.splats[i]);
				this.SetShaderParam(shaderParamName, this.splats_glossBaked[i]);
			}
			else
			{
				this.SetShaderParam(shaderParamName, this.splats[i]);
			}
		}
		else
		{
			this.SetShaderParam(shaderParamName, this.splats_glossBaked[i]);
		}
	}

	// Token: 0x06002223 RID: 8739 RVA: 0x00104650 File Offset: 0x00102850
	public void ApplyGlossBakedAtlas(string shaderParamName, int atlasNum)
	{
		if (this.gloss_baked == null || this.gloss_baked.Length == 0)
		{
			this.gloss_baked = new RTPGlossBaked[12];
		}
		if (this.atlas_glossBaked[atlasNum] == null)
		{
			if (this.splat_atlases[atlasNum] == null)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < 4; i++)
			{
				int num = atlasNum * 4 + i;
				if (this.gloss_baked[num] != null && this.gloss_baked[num].used_in_atlas && this.gloss_baked[num].CheckSize(this.splats[num]))
				{
					flag = true;
				}
			}
			if (flag)
			{
				RTPGlossBaked[] array = new RTPGlossBaked[4];
				for (int j = 0; j < 4; j++)
				{
					int num2 = atlasNum * 4 + j;
					if (this.gloss_baked[num2] != null && this.gloss_baked[num2].used_in_atlas && this.gloss_baked[num2].CheckSize(this.splats[num2]))
					{
						array[j] = this.gloss_baked[num2];
					}
					else
					{
						array[j] = (ScriptableObject.CreateInstance(typeof(RTPGlossBaked)) as RTPGlossBaked);
						array[j].Init(this.splats[num2].width);
						array[j].GetMIPGlossMapsFromAtlas(this.splat_atlases[atlasNum], j);
						array[j].used_in_atlas = true;
					}
				}
				this.atlas_glossBaked[atlasNum] = RTPGlossBaked.MakeTexture(this.splat_atlases[atlasNum], array);
				this.SetShaderParam(shaderParamName, this.atlas_glossBaked[atlasNum]);
			}
			else
			{
				this.SetShaderParam(shaderParamName, this.splat_atlases[atlasNum]);
			}
		}
		else
		{
			this.SetShaderParam(shaderParamName, this.atlas_glossBaked[atlasNum]);
		}
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x00104820 File Offset: 0x00102A20
	private void CheckLightScriptForDefered()
	{
		Light[] array = Object.FindObjectsOfType<Light>();
		Light light = null;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].type == 1)
			{
				if (!(array[i].gameObject.GetComponent<ReliefShaders_applyLightForDeferred>() == null))
				{
					return;
				}
				light = array[i];
			}
		}
		if (light)
		{
			ReliefShaders_applyLightForDeferred reliefShaders_applyLightForDeferred = light.gameObject.AddComponent(typeof(ReliefShaders_applyLightForDeferred)) as ReliefShaders_applyLightForDeferred;
			reliefShaders_applyLightForDeferred.lightForSelfShadowing = light;
		}
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x001048A8 File Offset: 0x00102AA8
	public void RefreshAll()
	{
		this.CheckLightScriptForDefered();
		ReliefTerrain[] array = Object.FindObjectsOfType(typeof(ReliefTerrain)) as ReliefTerrain[];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].globalSettingsHolder != null)
			{
				Terrain terrain = array[i].GetComponent(typeof(Terrain)) as Terrain;
				if (terrain)
				{
					array[i].globalSettingsHolder.Refresh(terrain.materialTemplate, null);
				}
				else
				{
					array[i].globalSettingsHolder.Refresh(array[i].GetComponent<Renderer>().sharedMaterial, null);
				}
				array[i].RefreshTextures(null, false);
			}
		}
		GeometryVsTerrainBlend[] array2 = Object.FindObjectsOfType(typeof(GeometryVsTerrainBlend)) as GeometryVsTerrainBlend[];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].SetupValues();
		}
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x00104988 File Offset: 0x00102B88
	public void Refresh(Material mat = null, ReliefTerrain rt_caller = null)
	{
		if (this.splats == null)
		{
			return;
		}
		if (mat == null && rt_caller != null && rt_caller.globalSettingsHolder == this)
		{
			Terrain terrain = rt_caller.GetComponent(typeof(Terrain)) as Terrain;
			if (terrain)
			{
				rt_caller.globalSettingsHolder.Refresh(terrain.materialTemplate, null);
			}
			else if (rt_caller.GetComponent<Renderer>() != null && rt_caller.GetComponent<Renderer>().sharedMaterial != null)
			{
				rt_caller.globalSettingsHolder.Refresh(rt_caller.GetComponent<Renderer>().sharedMaterial, null);
			}
		}
		this.use_mat = mat;
		for (int i = 0; i < this.numLayers; i++)
		{
			if (i < 4)
			{
				this.ApplyGlossBakedTexture("_SplatA" + i, i);
			}
			else if (i < 8)
			{
				if (this._4LAYERS_SHADER_USED)
				{
					this.ApplyGlossBakedTexture("_SplatC" + (i - 4), i);
					this.ApplyGlossBakedTexture("_SplatB" + (i - 4), i);
				}
				else
				{
					this.ApplyGlossBakedTexture("_SplatB" + (i - 4), i);
				}
			}
			else if (i < 12)
			{
				this.ApplyGlossBakedTexture("_SplatC" + (i - 8), i);
			}
		}
		if (this.CheckAndUpdate(ref this.RTP_gloss2mask, 0.5f, this.numLayers))
		{
			for (int j = 0; j < this.numLayers; j++)
			{
				this.Spec[j] = 1f;
			}
		}
		this.CheckAndUpdate(ref this.RTP_gloss_mult, 1f, this.numLayers);
		this.CheckAndUpdate(ref this.RTP_gloss_shaping, 0.5f, this.numLayers);
		this.CheckAndUpdate(ref this.RTP_Fresnel, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.RTP_FresnelAtten, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.RTP_DiffFresnel, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.RTP_IBL_bump_smoothness, 0.7f, this.numLayers);
		this.CheckAndUpdate(ref this.RTP_IBL_DiffuseStrength, 0.5f, this.numLayers);
		this.CheckAndUpdate(ref this.RTP_IBL_SpecStrength, 0.5f, this.numLayers);
		this.CheckAndUpdate(ref this._DeferredSpecDampAddPass, 1f, this.numLayers);
		this.CheckAndUpdate(ref this.TERRAIN_WaterSpecularity, 0.5f, this.numLayers);
		this.CheckAndUpdate(ref this.TERRAIN_WaterGloss, 0.1f, this.numLayers);
		this.CheckAndUpdate(ref this.TERRAIN_WaterGlossDamper, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.TERRAIN_WetSpecularity, 0.2f, this.numLayers);
		this.CheckAndUpdate(ref this.TERRAIN_WetGloss, 0.05f, this.numLayers);
		this.CheckAndUpdate(ref this.TERRAIN_WetFlow, 0.05f, this.numLayers);
		this.CheckAndUpdate(ref this.MixBrightness, 2f, this.numLayers);
		this.CheckAndUpdate(ref this.MixReplace, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.LayerBrightness, 1f, this.numLayers);
		this.CheckAndUpdate(ref this.LayerBrightness2Spec, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.LayerAlbedo2SpecColor, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.LayerSaturation, 1f, this.numLayers);
		this.CheckAndUpdate(ref this.LayerEmission, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.FarSpecCorrection, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.LayerEmissionColor, Color.black, this.numLayers);
		this.CheckAndUpdate(ref this.LayerEmissionRefractStrength, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.LayerEmissionRefractHBedge, 0f, this.numLayers);
		this.CheckAndUpdate(ref this.TERRAIN_WaterIBL_SpecWetStrength, 0.1f, this.numLayers);
		this.CheckAndUpdate(ref this.TERRAIN_WaterIBL_SpecWaterStrength, 0.5f, this.numLayers);
		this.CheckAndUpdate(ref this.TERRAIN_WaterEmission, 0f, this.numLayers);
		if (RenderSettings.fog)
		{
			Shader.SetGlobalFloat("_Fdensity", RenderSettings.fogDensity);
			if (this.colorSpaceLinear)
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
		this.SetShaderParam("terrainTileSize", this.terrainTileSize);
		this.SetShaderParam("RTP_AOamp", this.RTP_AOamp);
		this.SetShaderParam("RTP_AOsharpness", this.RTP_AOsharpness);
		this.SetShaderParam("EmissionRefractFiltering", this.EmissionRefractFiltering);
		this.SetShaderParam("EmissionRefractAnimSpeed", this.EmissionRefractAnimSpeed);
		this.SetShaderParam("_VerticalTexture", this.VerticalTexture);
		this.SetShaderParam("_GlobalColorMapBlendValues", this.GlobalColorMapBlendValues);
		this.SetShaderParam("_GlobalColorMapSaturation", this.GlobalColorMapSaturation);
		this.SetShaderParam("_GlobalColorMapSaturationFar", this.GlobalColorMapSaturationFar);
		this.SetShaderParam("_GlobalColorMapDistortByPerlin", this.GlobalColorMapDistortByPerlin);
		this.SetShaderParam("_GlobalColorMapBrightness", this.GlobalColorMapBrightness);
		this.SetShaderParam("_GlobalColorMapBrightnessFar", this.GlobalColorMapBrightnessFar);
		this.SetShaderParam("_GlobalColorMapNearMIP", this._GlobalColorMapNearMIP);
		this.SetShaderParam("_RTP_MIP_BIAS", this.RTP_MIP_BIAS);
		this.SetShaderParam("_BumpMapGlobalScale", this.BumpMapGlobalScale);
		this.SetShaderParam("_FarNormalDamp", this._FarNormalDamp);
		this.SetShaderParam("_SpecColor", this._SpecColor);
		this.SetShaderParam("RTP_DeferredAddPassSpec", this.RTP_DeferredAddPassSpec);
		this.SetShaderParam("_blend_multiplier", this.blendMultiplier);
		this.SetShaderParam("_TERRAIN_ReliefTransform", this.ReliefTransform);
		this.SetShaderParam("_TERRAIN_ReliefTransformTriplanarZ", this.ReliefTransform.x);
		this.SetShaderParam("_TERRAIN_DIST_STEPS", this.DIST_STEPS);
		this.SetShaderParam("_TERRAIN_WAVELENGTH", this.WAVELENGTH);
		this.SetShaderParam("_TERRAIN_ExtrudeHeight", this.ExtrudeHeight);
		this.SetShaderParam("_TERRAIN_LightmapShading", this.LightmapShading);
		this.SetShaderParam("_TERRAIN_SHADOW_STEPS", this.SHADOW_STEPS);
		this.SetShaderParam("_TERRAIN_WAVELENGTH_SHADOWS", this.WAVELENGTH_SHADOWS);
		this.SetShaderParam("_TERRAIN_SelfShadowStrength", this.SelfShadowStrength);
		this.SetShaderParam("_TERRAIN_ShadowSmoothing", (1f - this.ShadowSmoothing) * 6f);
		this.SetShaderParam("_TERRAIN_ShadowSoftnessFade", this.ShadowSoftnessFade);
		this.SetShaderParam("_TERRAIN_distance_start", this.distance_start);
		this.SetShaderParam("_TERRAIN_distance_transition", this.distance_transition);
		this.SetShaderParam("_TERRAIN_distance_start_bumpglobal", this.distance_start_bumpglobal);
		this.SetShaderParam("_TERRAIN_distance_transition_bumpglobal", this.distance_transition_bumpglobal);
		this.SetShaderParam("rtp_perlin_start_val", this.rtp_perlin_start_val);
		Shader.SetGlobalVector("_TERRAIN_trees_shadow_values", new Vector4(this.trees_shadow_distance_start, this.trees_shadow_distance_transition, this.trees_shadow_value, this.global_normalMap_multiplier));
		Shader.SetGlobalVector("_TERRAIN_trees_pixel_values", new Vector4(this.trees_pixel_distance_start, this.trees_pixel_distance_transition, this.trees_pixel_blend_val, this.global_normalMap_farUsage));
		this.SetShaderParam("_Phong", this._Phong);
		this.SetShaderParam("_TessSubdivisions", this._TessSubdivisions);
		this.SetShaderParam("_TessSubdivisionsFar", this._TessSubdivisionsFar);
		this.SetShaderParam("_TessYOffset", this._TessYOffset);
		Shader.SetGlobalFloat("_AmbientEmissiveMultiplier", this._AmbientEmissiveMultiplier);
		Shader.SetGlobalFloat("_AmbientEmissiveRelief", this._AmbientEmissiveRelief);
		this.SetShaderParam("_SuperDetailTiling", this._SuperDetailTiling);
		Shader.SetGlobalFloat("rtp_snow_strength", this._snow_strength);
		Shader.SetGlobalFloat("rtp_global_color_brightness_to_snow", this._global_color_brightness_to_snow);
		Shader.SetGlobalFloat("rtp_snow_slope_factor", this._snow_slope_factor);
		Shader.SetGlobalFloat("rtp_snow_edge_definition", this._snow_edge_definition);
		Shader.SetGlobalFloat("rtp_snow_height_treshold", this._snow_height_treshold);
		Shader.SetGlobalFloat("rtp_snow_height_transition", this._snow_height_transition);
		Shader.SetGlobalColor("rtp_snow_color", this._snow_color);
		Shader.SetGlobalFloat("rtp_snow_specular", this._snow_specular);
		Shader.SetGlobalFloat("rtp_snow_gloss", this._snow_gloss);
		Shader.SetGlobalFloat("rtp_snow_reflectivness", this._snow_reflectivness);
		Shader.SetGlobalFloat("rtp_snow_deep_factor", this._snow_deep_factor);
		Shader.SetGlobalFloat("rtp_snow_fresnel", this._snow_fresnel);
		Shader.SetGlobalFloat("rtp_snow_diff_fresnel", this._snow_diff_fresnel);
		Shader.SetGlobalFloat("rtp_snow_IBL_DiffuseStrength", this._snow_IBL_DiffuseStrength);
		Shader.SetGlobalFloat("rtp_snow_IBL_SpecStrength", this._snow_IBL_SpecStrength);
		this.SetShaderParam("TERRAIN_CausticsAnimSpeed", this.TERRAIN_CausticsAnimSpeed);
		this.SetShaderParam("TERRAIN_CausticsColor", this.TERRAIN_CausticsColor);
		if (this.TERRAIN_CausticsWaterLevelRefObject)
		{
			this.TERRAIN_CausticsWaterLevel = this.TERRAIN_CausticsWaterLevelRefObject.transform.position.y;
		}
		Shader.SetGlobalFloat("TERRAIN_CausticsWaterLevel", this.TERRAIN_CausticsWaterLevel);
		Shader.SetGlobalFloat("TERRAIN_CausticsWaterLevelByAngle", this.TERRAIN_CausticsWaterLevelByAngle);
		Shader.SetGlobalFloat("TERRAIN_CausticsWaterDeepFadeLength", this.TERRAIN_CausticsWaterDeepFadeLength);
		Shader.SetGlobalFloat("TERRAIN_CausticsWaterShallowFadeLength", this.TERRAIN_CausticsWaterShallowFadeLength);
		this.SetShaderParam("TERRAIN_CausticsTilingScale", this.TERRAIN_CausticsTilingScale);
		this.SetShaderParam("TERRAIN_CausticsTex", this.TERRAIN_CausticsTex);
		if (this.numLayers > 0)
		{
			int num = 512;
			for (int k = 0; k < this.numLayers; k++)
			{
				if (this.splats[k])
				{
					num = this.splats[k].width;
					break;
				}
			}
			this.SetShaderParam("rtp_mipoffset_color", -Mathf.Log(1024f / (float)num) / Mathf.Log(2f));
			if (this.Bump01 != null)
			{
				num = this.Bump01.width;
			}
			this.SetShaderParam("rtp_mipoffset_bump", -Mathf.Log(1024f / (float)num) / Mathf.Log(2f));
			if (this.HeightMap)
			{
				num = this.HeightMap.width;
			}
			else if (this.HeightMap2)
			{
				num = this.HeightMap2.width;
			}
			else if (this.HeightMap3)
			{
				num = this.HeightMap3.width;
			}
			this.SetShaderParam("rtp_mipoffset_height", -Mathf.Log(1024f / (float)num) / Mathf.Log(2f));
			num = this.BumpGlobalCombinedSize;
			this.SetShaderParam("rtp_mipoffset_globalnorm", -Mathf.Log(1024f / ((float)num * this.BumpMapGlobalScale)) / Mathf.Log(2f) + (float)this.rtp_mipoffset_globalnorm);
			this.SetShaderParam("rtp_mipoffset_superdetail", -Mathf.Log(1024f / ((float)num * this._SuperDetailTiling)) / Mathf.Log(2f));
			this.SetShaderParam("rtp_mipoffset_flow", -Mathf.Log(1024f / ((float)num * this.TERRAIN_FlowScale)) / Mathf.Log(2f) + this.TERRAIN_FlowMipOffset);
			if (this.TERRAIN_RippleMap)
			{
				num = this.TERRAIN_RippleMap.width;
			}
			this.SetShaderParam("rtp_mipoffset_ripple", -Mathf.Log(1024f / ((float)num * this.TERRAIN_RippleScale)) / Mathf.Log(2f));
			if (this.TERRAIN_CausticsTex)
			{
				num = this.TERRAIN_CausticsTex.width;
			}
			this.SetShaderParam("rtp_mipoffset_caustics", -Mathf.Log(1024f / ((float)num * this.TERRAIN_CausticsTilingScale)) / Mathf.Log(2f));
		}
		this.SetShaderParam("TERRAIN_ReflectionMap", this.TERRAIN_ReflectionMap);
		this.SetShaderParam("TERRAIN_ReflColorA", this.TERRAIN_ReflColorA);
		this.SetShaderParam("TERRAIN_ReflColorB", this.TERRAIN_ReflColorB);
		this.SetShaderParam("TERRAIN_ReflColorC", this.TERRAIN_ReflColorC);
		this.SetShaderParam("TERRAIN_ReflColorCenter", this.TERRAIN_ReflColorCenter);
		this.SetShaderParam("TERRAIN_ReflGlossAttenuation", this.TERRAIN_ReflGlossAttenuation);
		this.SetShaderParam("TERRAIN_ReflectionRotSpeed", this.TERRAIN_ReflectionRotSpeed);
		this.SetShaderParam("TERRAIN_GlobalWetness", this.TERRAIN_GlobalWetness);
		Shader.SetGlobalFloat("TERRAIN_GlobalWetness", this.TERRAIN_GlobalWetness);
		this.SetShaderParam("TERRAIN_RippleMap", this.TERRAIN_RippleMap);
		this.SetShaderParam("TERRAIN_RippleScale", this.TERRAIN_RippleScale);
		this.SetShaderParam("TERRAIN_FlowScale", this.TERRAIN_FlowScale);
		this.SetShaderParam("TERRAIN_FlowMipOffset", this.TERRAIN_FlowMipOffset);
		this.SetShaderParam("TERRAIN_FlowSpeed", this.TERRAIN_FlowSpeed);
		this.SetShaderParam("TERRAIN_FlowCycleScale", this.TERRAIN_FlowCycleScale);
		Shader.SetGlobalFloat("TERRAIN_RainIntensity", this.TERRAIN_RainIntensity);
		this.SetShaderParam("TERRAIN_DropletsSpeed", this.TERRAIN_DropletsSpeed);
		this.SetShaderParam("TERRAIN_WetDropletsStrength", this.TERRAIN_WetDropletsStrength);
		this.SetShaderParam("TERRAIN_WetDarkening", this.TERRAIN_WetDarkening);
		this.SetShaderParam("TERRAIN_mipoffset_flowSpeed", this.TERRAIN_mipoffset_flowSpeed);
		this.SetShaderParam("TERRAIN_WetHeight_Treshold", this.TERRAIN_WetHeight_Treshold);
		this.SetShaderParam("TERRAIN_WetHeight_Transition", this.TERRAIN_WetHeight_Transition);
		Shader.SetGlobalVector("rtp_customAmbientCorrection", new Vector4(this.rtp_customAmbientCorrection.r - 0.2f, this.rtp_customAmbientCorrection.g - 0.2f, this.rtp_customAmbientCorrection.b - 0.2f, 0f) * 0.1f);
		this.SetShaderParam("_CubemapDiff", this._CubemapDiff);
		this.SetShaderParam("_CubemapSpec", this._CubemapSpec);
		Shader.SetGlobalFloat("TERRAIN_IBL_DiffAO_Damp", this.TERRAIN_IBL_DiffAO_Damp);
		Shader.SetGlobalFloat("TERRAIN_IBLRefl_SpecAO_Damp", this.TERRAIN_IBLRefl_SpecAO_Damp);
		Shader.SetGlobalVector("RTP_LightDefVector", this.RTP_LightDefVector);
		Shader.SetGlobalFloat("RTP_BackLightStrength", this.RTP_LightDefVector.x);
		Shader.SetGlobalFloat("RTP_ReflexLightDiffuseSoftness", this.RTP_LightDefVector.y);
		Shader.SetGlobalFloat("RTP_ReflexLightSpecSoftness", this.RTP_LightDefVector.z);
		Shader.SetGlobalFloat("RTP_ReflexLightSpecularity", this.RTP_LightDefVector.w);
		Shader.SetGlobalColor("RTP_ReflexLightDiffuseColor1", this.RTP_ReflexLightDiffuseColor);
		Shader.SetGlobalColor("RTP_ReflexLightDiffuseColor2", this.RTP_ReflexLightDiffuseColor2);
		Shader.SetGlobalColor("RTP_ReflexLightSpecColor", this.RTP_ReflexLightSpecColor);
		this.SetShaderParam("_VerticalTextureGlobalBumpInfluence", this.VerticalTextureGlobalBumpInfluence);
		this.SetShaderParam("_VerticalTextureTiling", this.VerticalTextureTiling);
		float[] array = new float[this.RTP_gloss_mult.Length];
		for (int l = 0; l < array.Length; l++)
		{
			if (this.gloss_baked[l] != null && this.gloss_baked[l].baked)
			{
				array[l] = 1f;
			}
			else
			{
				array[l] = this.RTP_gloss_mult[l];
			}
		}
		float[] array2 = new float[this.RTP_gloss_shaping.Length];
		for (int m = 0; m < array2.Length; m++)
		{
			if (this.gloss_baked[m] != null && this.gloss_baked[m].baked)
			{
				array2[m] = 0.5f;
			}
			else
			{
				array2[m] = this.RTP_gloss_shaping[m];
			}
		}
		this.SetShaderParam("_Spec0123", this.getVector(this.Spec, 0, 3));
		this.SetShaderParam("_FarSpecCorrection0123", this.getVector(this.FarSpecCorrection, 0, 3));
		this.SetShaderParam("_MIPmult0123", this.getVector(this.MIPmult, 0, 3));
		this.SetShaderParam("_MixScale0123", this.getVector(this.MixScale, 0, 3));
		this.SetShaderParam("_MixBlend0123", this.getVector(this.MixBlend, 0, 3));
		this.SetShaderParam("_MixSaturation0123", this.getVector(this.MixSaturation, 0, 3));
		this.SetShaderParam("RTP_gloss2mask0123", this.getVector(this.RTP_gloss2mask, 0, 3));
		this.SetShaderParam("RTP_gloss_mult0123", this.getVector(array, 0, 3));
		this.SetShaderParam("RTP_gloss_shaping0123", this.getVector(array2, 0, 3));
		this.SetShaderParam("RTP_Fresnel0123", this.getVector(this.RTP_Fresnel, 0, 3));
		this.SetShaderParam("RTP_FresnelAtten0123", this.getVector(this.RTP_FresnelAtten, 0, 3));
		this.SetShaderParam("RTP_DiffFresnel0123", this.getVector(this.RTP_DiffFresnel, 0, 3));
		this.SetShaderParam("RTP_IBL_bump_smoothness0123", this.getVector(this.RTP_IBL_bump_smoothness, 0, 3));
		this.SetShaderParam("RTP_IBL_DiffuseStrength0123", this.getVector(this.RTP_IBL_DiffuseStrength, 0, 3));
		this.SetShaderParam("RTP_IBL_SpecStrength0123", this.getVector(this.RTP_IBL_SpecStrength, 0, 3));
		this.SetShaderParam("_MixBrightness0123", this.getVector(this.MixBrightness, 0, 3));
		this.SetShaderParam("_MixReplace0123", this.getVector(this.MixReplace, 0, 3));
		this.SetShaderParam("_LayerBrightness0123", this.MasterLayerBrightness * this.getVector(this.LayerBrightness, 0, 3));
		this.SetShaderParam("_LayerSaturation0123", this.MasterLayerSaturation * this.getVector(this.LayerSaturation, 0, 3));
		this.SetShaderParam("_LayerEmission0123", this.getVector(this.LayerEmission, 0, 3));
		this.SetShaderParam("_LayerEmissionColorR0123", this.getColorVector(this.LayerEmissionColor, 0, 3, 0));
		this.SetShaderParam("_LayerEmissionColorG0123", this.getColorVector(this.LayerEmissionColor, 0, 3, 1));
		this.SetShaderParam("_LayerEmissionColorB0123", this.getColorVector(this.LayerEmissionColor, 0, 3, 2));
		this.SetShaderParam("_LayerEmissionColorA0123", this.getColorVector(this.LayerEmissionColor, 0, 3, 3));
		this.SetShaderParam("_LayerBrightness2Spec0123", this.getVector(this.LayerBrightness2Spec, 0, 3));
		this.SetShaderParam("_LayerAlbedo2SpecColor0123", this.getVector(this.LayerAlbedo2SpecColor, 0, 3));
		this.SetShaderParam("_LayerEmissionRefractStrength0123", this.getVector(this.LayerEmissionRefractStrength, 0, 3));
		this.SetShaderParam("_LayerEmissionRefractHBedge0123", this.getVector(this.LayerEmissionRefractHBedge, 0, 3));
		this.SetShaderParam("_GlobalColorPerLayer0123", this.getVector(this.GlobalColorPerLayer, 0, 3));
		this.SetShaderParam("_GlobalColorBottom0123", this.getVector(this.GlobalColorBottom, 0, 3));
		this.SetShaderParam("_GlobalColorTop0123", this.getVector(this.GlobalColorTop, 0, 3));
		this.SetShaderParam("_GlobalColorColormapLoSat0123", this.getVector(this.GlobalColorColormapLoSat, 0, 3));
		this.SetShaderParam("_GlobalColorColormapHiSat0123", this.getVector(this.GlobalColorColormapHiSat, 0, 3));
		this.SetShaderParam("_GlobalColorLayerLoSat0123", this.getVector(this.GlobalColorLayerLoSat, 0, 3));
		this.SetShaderParam("_GlobalColorLayerHiSat0123", this.getVector(this.GlobalColorLayerHiSat, 0, 3));
		this.SetShaderParam("_GlobalColorLoBlend0123", this.getVector(this.GlobalColorLoBlend, 0, 3));
		this.SetShaderParam("_GlobalColorHiBlend0123", this.getVector(this.GlobalColorHiBlend, 0, 3));
		this.SetShaderParam("PER_LAYER_HEIGHT_MODIFIER0123", this.getVector(this.PER_LAYER_HEIGHT_MODIFIER, 0, 3));
		this.SetShaderParam("rtp_snow_strength_per_layer0123", this.getVector(this._snow_strength_per_layer, 0, 3));
		this.SetShaderParam("_SuperDetailStrengthMultA0123", this.getVector(this._SuperDetailStrengthMultA, 0, 3));
		this.SetShaderParam("_SuperDetailStrengthMultB0123", this.getVector(this._SuperDetailStrengthMultB, 0, 3));
		this.SetShaderParam("_SuperDetailStrengthNormal0123", this.getVector(this._SuperDetailStrengthNormal, 0, 3));
		this.SetShaderParam("_BumpMapGlobalStrength0123", this.getVector(this._BumpMapGlobalStrength, 0, 3));
		this.SetShaderParam("_SuperDetailStrengthMultASelfMaskNear0123", this.getVector(this._SuperDetailStrengthMultASelfMaskNear, 0, 3));
		this.SetShaderParam("_SuperDetailStrengthMultASelfMaskFar0123", this.getVector(this._SuperDetailStrengthMultASelfMaskFar, 0, 3));
		this.SetShaderParam("_SuperDetailStrengthMultBSelfMaskNear0123", this.getVector(this._SuperDetailStrengthMultBSelfMaskNear, 0, 3));
		this.SetShaderParam("_SuperDetailStrengthMultBSelfMaskFar0123", this.getVector(this._SuperDetailStrengthMultBSelfMaskFar, 0, 3));
		this.SetShaderParam("TERRAIN_LayerWetStrength0123", this.getVector(this.TERRAIN_LayerWetStrength, 0, 3));
		this.SetShaderParam("TERRAIN_WaterLevel0123", this.getVector(this.TERRAIN_WaterLevel, 0, 3));
		this.SetShaderParam("TERRAIN_WaterLevelSlopeDamp0123", this.getVector(this.TERRAIN_WaterLevelSlopeDamp, 0, 3));
		this.SetShaderParam("TERRAIN_WaterEdge0123", this.getVector(this.TERRAIN_WaterEdge, 0, 3));
		this.SetShaderParam("TERRAIN_WaterSpecularity0123", this.getVector(this.TERRAIN_WaterSpecularity, 0, 3));
		this.SetShaderParam("TERRAIN_WaterGloss0123", this.getVector(this.TERRAIN_WaterGloss, 0, 3));
		this.SetShaderParam("TERRAIN_WaterGlossDamper0123", this.getVector(this.TERRAIN_WaterGlossDamper, 0, 3));
		this.SetShaderParam("TERRAIN_WaterOpacity0123", this.getVector(this.TERRAIN_WaterOpacity, 0, 3));
		this.SetShaderParam("TERRAIN_Refraction0123", this.getVector(this.TERRAIN_Refraction, 0, 3));
		this.SetShaderParam("TERRAIN_WetRefraction0123", this.getVector(this.TERRAIN_WetRefraction, 0, 3));
		this.SetShaderParam("TERRAIN_Flow0123", this.getVector(this.TERRAIN_Flow, 0, 3));
		this.SetShaderParam("TERRAIN_WetFlow0123", this.getVector(this.TERRAIN_WetFlow, 0, 3));
		this.SetShaderParam("TERRAIN_WetSpecularity0123", this.getVector(this.TERRAIN_WetSpecularity, 0, 3));
		this.SetShaderParam("TERRAIN_WetGloss0123", this.getVector(this.TERRAIN_WetGloss, 0, 3));
		this.SetShaderParam("TERRAIN_WaterColorR0123", this.getColorVector(this.TERRAIN_WaterColor, 0, 3, 0));
		this.SetShaderParam("TERRAIN_WaterColorG0123", this.getColorVector(this.TERRAIN_WaterColor, 0, 3, 1));
		this.SetShaderParam("TERRAIN_WaterColorB0123", this.getColorVector(this.TERRAIN_WaterColor, 0, 3, 2));
		this.SetShaderParam("TERRAIN_WaterColorA0123", this.getColorVector(this.TERRAIN_WaterColor, 0, 3, 3));
		this.SetShaderParam("TERRAIN_WaterIBL_SpecWetStrength0123", this.getVector(this.TERRAIN_WaterIBL_SpecWetStrength, 0, 3));
		this.SetShaderParam("TERRAIN_WaterIBL_SpecWaterStrength0123", this.getVector(this.TERRAIN_WaterIBL_SpecWaterStrength, 0, 3));
		this.SetShaderParam("TERRAIN_WaterEmission0123", this.getVector(this.TERRAIN_WaterEmission, 0, 3));
		this.SetShaderParam("RTP_AO_0123", this.getVector(this.AO_strength, 0, 3));
		this.SetShaderParam("_VerticalTexture0123", this.getVector(this.VerticalTextureStrength, 0, 3));
		if (this.numLayers > 4 && this._4LAYERS_SHADER_USED)
		{
			this.SetShaderParam("_Spec89AB", this.getVector(this.Spec, 4, 7));
			this.SetShaderParam("_FarSpecCorrection89AB", this.getVector(this.FarSpecCorrection, 4, 7));
			this.SetShaderParam("_MIPmult89AB", this.getVector(this.MIPmult, 4, 7));
			this.SetShaderParam("_MixScale89AB", this.getVector(this.MixScale, 4, 7));
			this.SetShaderParam("_MixBlend89AB", this.getVector(this.MixBlend, 4, 7));
			this.SetShaderParam("_MixSaturation89AB", this.getVector(this.MixSaturation, 4, 7));
			this.SetShaderParam("RTP_gloss2mask89AB", this.getVector(this.RTP_gloss2mask, 4, 7));
			this.SetShaderParam("RTP_gloss_mult89AB", this.getVector(array, 4, 7));
			this.SetShaderParam("RTP_gloss_shaping89AB", this.getVector(array2, 4, 7));
			this.SetShaderParam("RTP_Fresnel89AB", this.getVector(this.RTP_Fresnel, 4, 7));
			this.SetShaderParam("RTP_FresnelAtten89AB", this.getVector(this.RTP_FresnelAtten, 4, 7));
			this.SetShaderParam("RTP_DiffFresnel89AB", this.getVector(this.RTP_DiffFresnel, 4, 7));
			this.SetShaderParam("RTP_IBL_bump_smoothness89AB", this.getVector(this.RTP_IBL_bump_smoothness, 4, 7));
			this.SetShaderParam("RTP_IBL_DiffuseStrength89AB", this.getVector(this.RTP_IBL_DiffuseStrength, 4, 7));
			this.SetShaderParam("RTP_IBL_SpecStrength89AB", this.getVector(this.RTP_IBL_SpecStrength, 4, 7));
			this.SetShaderParam("_DeferredSpecDampAddPass89AB", this.getVector(this._DeferredSpecDampAddPass, 4, 7));
			this.SetShaderParam("_MixBrightness89AB", this.getVector(this.MixBrightness, 4, 7));
			this.SetShaderParam("_MixReplace89AB", this.getVector(this.MixReplace, 4, 7));
			this.SetShaderParam("_LayerBrightness89AB", this.MasterLayerBrightness * this.getVector(this.LayerBrightness, 4, 7));
			this.SetShaderParam("_LayerSaturation89AB", this.MasterLayerSaturation * this.getVector(this.LayerSaturation, 4, 7));
			this.SetShaderParam("_LayerEmission89AB", this.getVector(this.LayerEmission, 4, 7));
			this.SetShaderParam("_LayerEmissionColorR89AB", this.getColorVector(this.LayerEmissionColor, 4, 7, 0));
			this.SetShaderParam("_LayerEmissionColorG89AB", this.getColorVector(this.LayerEmissionColor, 4, 7, 1));
			this.SetShaderParam("_LayerEmissionColorB89AB", this.getColorVector(this.LayerEmissionColor, 4, 7, 2));
			this.SetShaderParam("_LayerEmissionColorA89AB", this.getColorVector(this.LayerEmissionColor, 4, 7, 3));
			this.SetShaderParam("_LayerBrightness2Spec89AB", this.getVector(this.LayerBrightness2Spec, 4, 7));
			this.SetShaderParam("_LayerAlbedo2SpecColor89AB", this.getVector(this.LayerAlbedo2SpecColor, 4, 7));
			this.SetShaderParam("_LayerEmissionRefractStrength89AB", this.getVector(this.LayerEmissionRefractStrength, 4, 7));
			this.SetShaderParam("_LayerEmissionRefractHBedge89AB", this.getVector(this.LayerEmissionRefractHBedge, 4, 7));
			this.SetShaderParam("_GlobalColorPerLayer89AB", this.getVector(this.GlobalColorPerLayer, 4, 7));
			this.SetShaderParam("_GlobalColorBottom89AB", this.getVector(this.GlobalColorBottom, 4, 7));
			this.SetShaderParam("_GlobalColorTop89AB", this.getVector(this.GlobalColorTop, 4, 7));
			this.SetShaderParam("_GlobalColorColormapLoSat89AB", this.getVector(this.GlobalColorColormapLoSat, 4, 7));
			this.SetShaderParam("_GlobalColorColormapHiSat89AB", this.getVector(this.GlobalColorColormapHiSat, 4, 7));
			this.SetShaderParam("_GlobalColorLayerLoSat89AB", this.getVector(this.GlobalColorLayerLoSat, 4, 7));
			this.SetShaderParam("_GlobalColorLayerHiSat89AB", this.getVector(this.GlobalColorLayerHiSat, 4, 7));
			this.SetShaderParam("_GlobalColorLoBlend89AB", this.getVector(this.GlobalColorLoBlend, 4, 7));
			this.SetShaderParam("_GlobalColorHiBlend89AB", this.getVector(this.GlobalColorHiBlend, 4, 7));
			this.SetShaderParam("PER_LAYER_HEIGHT_MODIFIER89AB", this.getVector(this.PER_LAYER_HEIGHT_MODIFIER, 4, 7));
			this.SetShaderParam("rtp_snow_strength_per_layer89AB", this.getVector(this._snow_strength_per_layer, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultA89AB", this.getVector(this._SuperDetailStrengthMultA, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultB89AB", this.getVector(this._SuperDetailStrengthMultB, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthNormal89AB", this.getVector(this._SuperDetailStrengthNormal, 4, 7));
			this.SetShaderParam("_BumpMapGlobalStrength89AB", this.getVector(this._BumpMapGlobalStrength, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultASelfMaskNear89AB", this.getVector(this._SuperDetailStrengthMultASelfMaskNear, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultASelfMaskFar89AB", this.getVector(this._SuperDetailStrengthMultASelfMaskFar, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultBSelfMaskNear89AB", this.getVector(this._SuperDetailStrengthMultBSelfMaskNear, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultBSelfMaskFar89AB", this.getVector(this._SuperDetailStrengthMultBSelfMaskFar, 4, 7));
			this.SetShaderParam("TERRAIN_LayerWetStrength89AB", this.getVector(this.TERRAIN_LayerWetStrength, 4, 7));
			this.SetShaderParam("TERRAIN_WaterLevel89AB", this.getVector(this.TERRAIN_WaterLevel, 4, 7));
			this.SetShaderParam("TERRAIN_WaterLevelSlopeDamp89AB", this.getVector(this.TERRAIN_WaterLevelSlopeDamp, 4, 7));
			this.SetShaderParam("TERRAIN_WaterEdge89AB", this.getVector(this.TERRAIN_WaterEdge, 4, 7));
			this.SetShaderParam("TERRAIN_WaterSpecularity89AB", this.getVector(this.TERRAIN_WaterSpecularity, 4, 7));
			this.SetShaderParam("TERRAIN_WaterGloss89AB", this.getVector(this.TERRAIN_WaterGloss, 4, 7));
			this.SetShaderParam("TERRAIN_WaterGlossDamper89AB", this.getVector(this.TERRAIN_WaterGlossDamper, 4, 7));
			this.SetShaderParam("TERRAIN_WaterOpacity89AB", this.getVector(this.TERRAIN_WaterOpacity, 4, 7));
			this.SetShaderParam("TERRAIN_Refraction89AB", this.getVector(this.TERRAIN_Refraction, 4, 7));
			this.SetShaderParam("TERRAIN_WetRefraction89AB", this.getVector(this.TERRAIN_WetRefraction, 4, 7));
			this.SetShaderParam("TERRAIN_Flow89AB", this.getVector(this.TERRAIN_Flow, 4, 7));
			this.SetShaderParam("TERRAIN_WetFlow89AB", this.getVector(this.TERRAIN_WetFlow, 4, 7));
			this.SetShaderParam("TERRAIN_WetSpecularity89AB", this.getVector(this.TERRAIN_WetSpecularity, 4, 7));
			this.SetShaderParam("TERRAIN_WetGloss89AB", this.getVector(this.TERRAIN_WetGloss, 4, 7));
			this.SetShaderParam("TERRAIN_WaterColorR89AB", this.getColorVector(this.TERRAIN_WaterColor, 4, 7, 0));
			this.SetShaderParam("TERRAIN_WaterColorG89AB", this.getColorVector(this.TERRAIN_WaterColor, 4, 7, 1));
			this.SetShaderParam("TERRAIN_WaterColorB89AB", this.getColorVector(this.TERRAIN_WaterColor, 4, 7, 2));
			this.SetShaderParam("TERRAIN_WaterColorA89AB", this.getColorVector(this.TERRAIN_WaterColor, 4, 7, 3));
			this.SetShaderParam("TERRAIN_WaterIBL_SpecWetStrength89AB", this.getVector(this.TERRAIN_WaterIBL_SpecWetStrength, 4, 7));
			this.SetShaderParam("TERRAIN_WaterIBL_SpecWaterStrength89AB", this.getVector(this.TERRAIN_WaterIBL_SpecWaterStrength, 4, 7));
			this.SetShaderParam("TERRAIN_WaterEmission89AB", this.getVector(this.TERRAIN_WaterEmission, 4, 7));
			this.SetShaderParam("RTP_AO_89AB", this.getVector(this.AO_strength, 4, 7));
			this.SetShaderParam("_VerticalTexture89AB", this.getVector(this.VerticalTextureStrength, 4, 7));
		}
		else
		{
			this.SetShaderParam("_Spec4567", this.getVector(this.Spec, 4, 7));
			this.SetShaderParam("_FarSpecCorrection4567", this.getVector(this.FarSpecCorrection, 4, 7));
			this.SetShaderParam("_MIPmult4567", this.getVector(this.MIPmult, 4, 7));
			this.SetShaderParam("_MixScale4567", this.getVector(this.MixScale, 4, 7));
			this.SetShaderParam("_MixBlend4567", this.getVector(this.MixBlend, 4, 7));
			this.SetShaderParam("_MixSaturation4567", this.getVector(this.MixSaturation, 4, 7));
			this.SetShaderParam("RTP_gloss2mask4567", this.getVector(this.RTP_gloss2mask, 4, 7));
			this.SetShaderParam("RTP_gloss_mult4567", this.getVector(array, 4, 7));
			this.SetShaderParam("RTP_gloss_shaping4567", this.getVector(array2, 4, 7));
			this.SetShaderParam("RTP_Fresnel4567", this.getVector(this.RTP_Fresnel, 4, 7));
			this.SetShaderParam("RTP_FresnelAtten4567", this.getVector(this.RTP_FresnelAtten, 4, 7));
			this.SetShaderParam("RTP_DiffFresnel4567", this.getVector(this.RTP_DiffFresnel, 4, 7));
			this.SetShaderParam("RTP_IBL_bump_smoothness4567", this.getVector(this.RTP_IBL_bump_smoothness, 4, 7));
			this.SetShaderParam("RTP_IBL_DiffuseStrength4567", this.getVector(this.RTP_IBL_DiffuseStrength, 4, 7));
			this.SetShaderParam("RTP_IBL_SpecStrength4567", this.getVector(this.RTP_IBL_SpecStrength, 4, 7));
			this.SetShaderParam("_MixBrightness4567", this.getVector(this.MixBrightness, 4, 7));
			this.SetShaderParam("_MixReplace4567", this.getVector(this.MixReplace, 4, 7));
			this.SetShaderParam("_LayerBrightness4567", this.MasterLayerBrightness * this.getVector(this.LayerBrightness, 4, 7));
			this.SetShaderParam("_LayerSaturation4567", this.MasterLayerSaturation * this.getVector(this.LayerSaturation, 4, 7));
			this.SetShaderParam("_LayerEmission4567", this.getVector(this.LayerEmission, 4, 7));
			this.SetShaderParam("_LayerEmissionColorR4567", this.getColorVector(this.LayerEmissionColor, 4, 7, 0));
			this.SetShaderParam("_LayerEmissionColorG4567", this.getColorVector(this.LayerEmissionColor, 4, 7, 1));
			this.SetShaderParam("_LayerEmissionColorB4567", this.getColorVector(this.LayerEmissionColor, 4, 7, 2));
			this.SetShaderParam("_LayerEmissionColorA4567", this.getColorVector(this.LayerEmissionColor, 4, 7, 3));
			this.SetShaderParam("_LayerBrightness2Spec4567", this.getVector(this.LayerBrightness2Spec, 4, 7));
			this.SetShaderParam("_LayerAlbedo2SpecColor4567", this.getVector(this.LayerAlbedo2SpecColor, 4, 7));
			this.SetShaderParam("_LayerEmissionRefractStrength4567", this.getVector(this.LayerEmissionRefractStrength, 4, 7));
			this.SetShaderParam("_LayerEmissionRefractHBedge4567", this.getVector(this.LayerEmissionRefractHBedge, 4, 7));
			this.SetShaderParam("_GlobalColorPerLayer4567", this.getVector(this.GlobalColorPerLayer, 4, 7));
			this.SetShaderParam("_GlobalColorBottom4567", this.getVector(this.GlobalColorBottom, 4, 7));
			this.SetShaderParam("_GlobalColorTop4567", this.getVector(this.GlobalColorTop, 4, 7));
			this.SetShaderParam("_GlobalColorColormapLoSat4567", this.getVector(this.GlobalColorColormapLoSat, 4, 7));
			this.SetShaderParam("_GlobalColorColormapHiSat4567", this.getVector(this.GlobalColorColormapHiSat, 4, 7));
			this.SetShaderParam("_GlobalColorLayerLoSat4567", this.getVector(this.GlobalColorLayerLoSat, 4, 7));
			this.SetShaderParam("_GlobalColorLayerHiSat4567", this.getVector(this.GlobalColorLayerHiSat, 4, 7));
			this.SetShaderParam("_GlobalColorLoBlend4567", this.getVector(this.GlobalColorLoBlend, 4, 7));
			this.SetShaderParam("_GlobalColorHiBlend4567", this.getVector(this.GlobalColorHiBlend, 4, 7));
			this.SetShaderParam("PER_LAYER_HEIGHT_MODIFIER4567", this.getVector(this.PER_LAYER_HEIGHT_MODIFIER, 4, 7));
			this.SetShaderParam("rtp_snow_strength_per_layer4567", this.getVector(this._snow_strength_per_layer, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultA4567", this.getVector(this._SuperDetailStrengthMultA, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultB4567", this.getVector(this._SuperDetailStrengthMultB, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthNormal4567", this.getVector(this._SuperDetailStrengthNormal, 4, 7));
			this.SetShaderParam("_BumpMapGlobalStrength4567", this.getVector(this._BumpMapGlobalStrength, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultASelfMaskNear4567", this.getVector(this._SuperDetailStrengthMultASelfMaskNear, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultASelfMaskFar4567", this.getVector(this._SuperDetailStrengthMultASelfMaskFar, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultBSelfMaskNear4567", this.getVector(this._SuperDetailStrengthMultBSelfMaskNear, 4, 7));
			this.SetShaderParam("_SuperDetailStrengthMultBSelfMaskFar4567", this.getVector(this._SuperDetailStrengthMultBSelfMaskFar, 4, 7));
			this.SetShaderParam("TERRAIN_LayerWetStrength4567", this.getVector(this.TERRAIN_LayerWetStrength, 4, 7));
			this.SetShaderParam("TERRAIN_WaterLevel4567", this.getVector(this.TERRAIN_WaterLevel, 4, 7));
			this.SetShaderParam("TERRAIN_WaterLevelSlopeDamp4567", this.getVector(this.TERRAIN_WaterLevelSlopeDamp, 4, 7));
			this.SetShaderParam("TERRAIN_WaterEdge4567", this.getVector(this.TERRAIN_WaterEdge, 4, 7));
			this.SetShaderParam("TERRAIN_WaterSpecularity4567", this.getVector(this.TERRAIN_WaterSpecularity, 4, 7));
			this.SetShaderParam("TERRAIN_WaterGloss4567", this.getVector(this.TERRAIN_WaterGloss, 4, 7));
			this.SetShaderParam("TERRAIN_WaterGlossDamper4567", this.getVector(this.TERRAIN_WaterGlossDamper, 4, 7));
			this.SetShaderParam("TERRAIN_WaterOpacity4567", this.getVector(this.TERRAIN_WaterOpacity, 4, 7));
			this.SetShaderParam("TERRAIN_Refraction4567", this.getVector(this.TERRAIN_Refraction, 4, 7));
			this.SetShaderParam("TERRAIN_WetRefraction4567", this.getVector(this.TERRAIN_WetRefraction, 4, 7));
			this.SetShaderParam("TERRAIN_Flow4567", this.getVector(this.TERRAIN_Flow, 4, 7));
			this.SetShaderParam("TERRAIN_WetFlow4567", this.getVector(this.TERRAIN_WetFlow, 4, 7));
			this.SetShaderParam("TERRAIN_WetSpecularity4567", this.getVector(this.TERRAIN_WetSpecularity, 4, 7));
			this.SetShaderParam("TERRAIN_WetGloss4567", this.getVector(this.TERRAIN_WetGloss, 4, 7));
			this.SetShaderParam("TERRAIN_WaterColorR4567", this.getColorVector(this.TERRAIN_WaterColor, 4, 7, 0));
			this.SetShaderParam("TERRAIN_WaterColorG4567", this.getColorVector(this.TERRAIN_WaterColor, 4, 7, 1));
			this.SetShaderParam("TERRAIN_WaterColorB4567", this.getColorVector(this.TERRAIN_WaterColor, 4, 7, 2));
			this.SetShaderParam("TERRAIN_WaterColorA4567", this.getColorVector(this.TERRAIN_WaterColor, 4, 7, 3));
			this.SetShaderParam("TERRAIN_WaterIBL_SpecWetStrength4567", this.getVector(this.TERRAIN_WaterIBL_SpecWetStrength, 4, 7));
			this.SetShaderParam("TERRAIN_WaterIBL_SpecWaterStrength4567", this.getVector(this.TERRAIN_WaterIBL_SpecWaterStrength, 4, 7));
			this.SetShaderParam("TERRAIN_WaterEmission4567", this.getVector(this.TERRAIN_WaterEmission, 4, 7));
			this.SetShaderParam("RTP_AO_4567", this.getVector(this.AO_strength, 4, 7));
			this.SetShaderParam("_VerticalTexture4567", this.getVector(this.VerticalTextureStrength, 4, 7));
			this.SetShaderParam("_Spec89AB", this.getVector(this.Spec, 8, 11));
			this.SetShaderParam("_FarSpecCorrection89AB", this.getVector(this.FarSpecCorrection, 8, 11));
			this.SetShaderParam("_MIPmult89AB", this.getVector(this.MIPmult, 8, 11));
			this.SetShaderParam("_MixScale89AB", this.getVector(this.MixScale, 8, 11));
			this.SetShaderParam("_MixBlend89AB", this.getVector(this.MixBlend, 8, 11));
			this.SetShaderParam("_MixSaturation89AB", this.getVector(this.MixSaturation, 8, 11));
			this.SetShaderParam("RTP_gloss2mask89AB", this.getVector(this.RTP_gloss2mask, 8, 11));
			this.SetShaderParam("RTP_gloss_mult89AB", this.getVector(array, 8, 11));
			this.SetShaderParam("RTP_gloss_shaping89AB", this.getVector(array2, 8, 11));
			this.SetShaderParam("RTP_Fresnel89AB", this.getVector(this.RTP_Fresnel, 8, 11));
			this.SetShaderParam("RTP_FresnelAtten89AB", this.getVector(this.RTP_FresnelAtten, 8, 11));
			this.SetShaderParam("RTP_DiffFresnel89AB", this.getVector(this.RTP_DiffFresnel, 8, 11));
			this.SetShaderParam("RTP_IBL_bump_smoothness89AB", this.getVector(this.RTP_IBL_bump_smoothness, 8, 11));
			this.SetShaderParam("RTP_IBL_DiffuseStrength89AB", this.getVector(this.RTP_IBL_DiffuseStrength, 8, 11));
			this.SetShaderParam("RTP_IBL_SpecStrength89AB", this.getVector(this.RTP_IBL_SpecStrength, 8, 11));
			this.SetShaderParam("_DeferredSpecDampAddPass89AB", this.getVector(this._DeferredSpecDampAddPass, 8, 11));
			this.SetShaderParam("_MixBrightness89AB", this.getVector(this.MixBrightness, 8, 11));
			this.SetShaderParam("_MixReplace89AB", this.getVector(this.MixReplace, 8, 11));
			this.SetShaderParam("_LayerBrightness89AB", this.MasterLayerBrightness * this.getVector(this.LayerBrightness, 8, 11));
			this.SetShaderParam("_LayerSaturation89AB", this.MasterLayerSaturation * this.getVector(this.LayerSaturation, 8, 11));
			this.SetShaderParam("_LayerEmission89AB", this.getVector(this.LayerEmission, 8, 11));
			this.SetShaderParam("_LayerEmissionColorR89AB", this.getColorVector(this.LayerEmissionColor, 8, 11, 0));
			this.SetShaderParam("_LayerEmissionColorG89AB", this.getColorVector(this.LayerEmissionColor, 8, 11, 1));
			this.SetShaderParam("_LayerEmissionColorB89AB", this.getColorVector(this.LayerEmissionColor, 8, 11, 2));
			this.SetShaderParam("_LayerEmissionColorA89AB", this.getColorVector(this.LayerEmissionColor, 8, 11, 3));
			this.SetShaderParam("_LayerBrightness2Spec89AB", this.getVector(this.LayerBrightness2Spec, 8, 11));
			this.SetShaderParam("_LayerAlbedo2SpecColor89AB", this.getVector(this.LayerAlbedo2SpecColor, 8, 11));
			this.SetShaderParam("_LayerEmissionRefractStrength89AB", this.getVector(this.LayerEmissionRefractStrength, 8, 11));
			this.SetShaderParam("_LayerEmissionRefractHBedge89AB", this.getVector(this.LayerEmissionRefractHBedge, 8, 11));
			this.SetShaderParam("_GlobalColorPerLayer89AB", this.getVector(this.GlobalColorPerLayer, 8, 11));
			this.SetShaderParam("_GlobalColorBottom89AB", this.getVector(this.GlobalColorBottom, 8, 11));
			this.SetShaderParam("_GlobalColorTop89AB", this.getVector(this.GlobalColorTop, 8, 11));
			this.SetShaderParam("_GlobalColorColormapLoSat89AB", this.getVector(this.GlobalColorColormapLoSat, 8, 11));
			this.SetShaderParam("_GlobalColorColormapHiSat89AB", this.getVector(this.GlobalColorColormapHiSat, 8, 11));
			this.SetShaderParam("_GlobalColorLayerLoSat89AB", this.getVector(this.GlobalColorLayerLoSat, 8, 11));
			this.SetShaderParam("_GlobalColorLayerHiSat89AB", this.getVector(this.GlobalColorLayerHiSat, 8, 11));
			this.SetShaderParam("_GlobalColorLoBlend89AB", this.getVector(this.GlobalColorLoBlend, 8, 11));
			this.SetShaderParam("_GlobalColorHiBlend89AB", this.getVector(this.GlobalColorHiBlend, 8, 11));
			this.SetShaderParam("PER_LAYER_HEIGHT_MODIFIER89AB", this.getVector(this.PER_LAYER_HEIGHT_MODIFIER, 8, 11));
			this.SetShaderParam("rtp_snow_strength_per_layer89AB", this.getVector(this._snow_strength_per_layer, 8, 11));
			this.SetShaderParam("_SuperDetailStrengthMultA89AB", this.getVector(this._SuperDetailStrengthMultA, 8, 11));
			this.SetShaderParam("_SuperDetailStrengthMultB89AB", this.getVector(this._SuperDetailStrengthMultB, 8, 11));
			this.SetShaderParam("_SuperDetailStrengthNormal89AB", this.getVector(this._SuperDetailStrengthNormal, 8, 11));
			this.SetShaderParam("_BumpMapGlobalStrength89AB", this.getVector(this._BumpMapGlobalStrength, 8, 11));
			this.SetShaderParam("_SuperDetailStrengthMultASelfMaskNear89AB", this.getVector(this._SuperDetailStrengthMultASelfMaskNear, 8, 11));
			this.SetShaderParam("_SuperDetailStrengthMultASelfMaskFar89AB", this.getVector(this._SuperDetailStrengthMultASelfMaskFar, 8, 11));
			this.SetShaderParam("_SuperDetailStrengthMultBSelfMaskNear89AB", this.getVector(this._SuperDetailStrengthMultBSelfMaskNear, 8, 11));
			this.SetShaderParam("_SuperDetailStrengthMultBSelfMaskFar89AB", this.getVector(this._SuperDetailStrengthMultBSelfMaskFar, 8, 11));
			this.SetShaderParam("TERRAIN_LayerWetStrength89AB", this.getVector(this.TERRAIN_LayerWetStrength, 8, 11));
			this.SetShaderParam("TERRAIN_WaterLevel89AB", this.getVector(this.TERRAIN_WaterLevel, 8, 11));
			this.SetShaderParam("TERRAIN_WaterLevelSlopeDamp89AB", this.getVector(this.TERRAIN_WaterLevelSlopeDamp, 8, 11));
			this.SetShaderParam("TERRAIN_WaterEdge89AB", this.getVector(this.TERRAIN_WaterEdge, 8, 11));
			this.SetShaderParam("TERRAIN_WaterSpecularity89AB", this.getVector(this.TERRAIN_WaterSpecularity, 8, 11));
			this.SetShaderParam("TERRAIN_WaterGloss89AB", this.getVector(this.TERRAIN_WaterGloss, 8, 11));
			this.SetShaderParam("TERRAIN_WaterGlossDamper89AB", this.getVector(this.TERRAIN_WaterGlossDamper, 8, 11));
			this.SetShaderParam("TERRAIN_WaterOpacity89AB", this.getVector(this.TERRAIN_WaterOpacity, 8, 11));
			this.SetShaderParam("TERRAIN_Refraction89AB", this.getVector(this.TERRAIN_Refraction, 8, 11));
			this.SetShaderParam("TERRAIN_WetRefraction89AB", this.getVector(this.TERRAIN_WetRefraction, 8, 11));
			this.SetShaderParam("TERRAIN_Flow89AB", this.getVector(this.TERRAIN_Flow, 8, 11));
			this.SetShaderParam("TERRAIN_WetFlow89AB", this.getVector(this.TERRAIN_WetFlow, 8, 11));
			this.SetShaderParam("TERRAIN_WetSpecularity89AB", this.getVector(this.TERRAIN_WetSpecularity, 8, 11));
			this.SetShaderParam("TERRAIN_WetGloss89AB", this.getVector(this.TERRAIN_WetGloss, 8, 11));
			this.SetShaderParam("TERRAIN_WaterColorR89AB", this.getColorVector(this.TERRAIN_WaterColor, 8, 11, 0));
			this.SetShaderParam("TERRAIN_WaterColorG89AB", this.getColorVector(this.TERRAIN_WaterColor, 8, 11, 1));
			this.SetShaderParam("TERRAIN_WaterColorB89AB", this.getColorVector(this.TERRAIN_WaterColor, 8, 11, 2));
			this.SetShaderParam("TERRAIN_WaterColorA89AB", this.getColorVector(this.TERRAIN_WaterColor, 8, 11, 3));
			this.SetShaderParam("TERRAIN_WaterIBL_SpecWetStrength89AB", this.getVector(this.TERRAIN_WaterIBL_SpecWetStrength, 8, 11));
			this.SetShaderParam("TERRAIN_WaterIBL_SpecWaterStrength89AB", this.getVector(this.TERRAIN_WaterIBL_SpecWaterStrength, 8, 11));
			this.SetShaderParam("TERRAIN_WaterEmission89AB", this.getVector(this.TERRAIN_WaterEmission, 8, 11));
			this.SetShaderParam("RTP_AO_89AB", this.getVector(this.AO_strength, 8, 11));
			this.SetShaderParam("_VerticalTexture89AB", this.getVector(this.VerticalTextureStrength, 8, 11));
		}
		if (this.splat_atlases.Length == 2)
		{
			Texture2D texture2D = this.splat_atlases[0];
			Texture2D texture2D2 = this.splat_atlases[1];
			this.splat_atlases = new Texture2D[3];
			this.splat_atlases[0] = texture2D;
			this.splat_atlases[1] = texture2D2;
		}
		this.ApplyGlossBakedAtlas("_SplatAtlasA", 0);
		this.SetShaderParam("_BumpMap01", this.Bump01);
		this.SetShaderParam("_BumpMap23", this.Bump23);
		this.SetShaderParam("_TERRAIN_HeightMap", this.HeightMap);
		this.SetShaderParam("_SSColorCombinedA", this.SSColorCombinedA);
		if (this.numLayers > 4)
		{
			this.ApplyGlossBakedAtlas("_SplatAtlasB", 1);
			this.ApplyGlossBakedAtlas("_SplatAtlasC", 1);
			this.SetShaderParam("_TERRAIN_HeightMap2", this.HeightMap2);
			this.SetShaderParam("_SSColorCombinedB", this.SSColorCombinedB);
		}
		if (this.numLayers > 8)
		{
			this.ApplyGlossBakedAtlas("_SplatAtlasC", 2);
		}
		if (this.numLayers > 4 && this._4LAYERS_SHADER_USED)
		{
			this.SetShaderParam("_BumpMap89", this.Bump45);
			this.SetShaderParam("_BumpMapAB", this.Bump67);
			this.SetShaderParam("_TERRAIN_HeightMap3", this.HeightMap2);
			this.SetShaderParam("_BumpMap45", this.Bump45);
			this.SetShaderParam("_BumpMap67", this.Bump67);
		}
		else
		{
			this.SetShaderParam("_BumpMap45", this.Bump45);
			this.SetShaderParam("_BumpMap67", this.Bump67);
			this.SetShaderParam("_BumpMap89", this.Bump89);
			this.SetShaderParam("_BumpMapAB", this.BumpAB);
			this.SetShaderParam("_TERRAIN_HeightMap3", this.HeightMap3);
		}
		this.use_mat = null;
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x00107768 File Offset: 0x00105968
	public Vector4 getVector(float[] vec, int idxA, int idxB)
	{
		if (vec == null)
		{
			return Vector4.zero;
		}
		Vector4 zero = Vector4.zero;
		for (int i = idxA; i <= idxB; i++)
		{
			if (i < vec.Length)
			{
				zero[i - idxA] = vec[i];
			}
		}
		return zero;
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x001077B0 File Offset: 0x001059B0
	public Vector4 getColorVector(Color[] vec, int idxA, int idxB, int channel)
	{
		if (vec == null)
		{
			return Vector4.zero;
		}
		Vector4 zero = Vector4.zero;
		for (int i = idxA; i <= idxB; i++)
		{
			if (i < vec.Length)
			{
				zero[i - idxA] = vec[i][channel];
			}
		}
		return zero;
	}

	// Token: 0x06002229 RID: 8745 RVA: 0x00107804 File Offset: 0x00105A04
	public Texture2D get_dumb_tex()
	{
		if (!this.dumb_tex)
		{
			this.dumb_tex = new Texture2D(32, 32, 3, false);
			Color[] pixels = this.dumb_tex.GetPixels();
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = Color.white;
			}
			this.dumb_tex.SetPixels(pixels);
			this.dumb_tex.Apply();
		}
		return this.dumb_tex;
	}

	// Token: 0x0600222A RID: 8746 RVA: 0x00107880 File Offset: 0x00105A80
	public void SyncGlobalPropsAcrossTerrainGroups()
	{
		ReliefTerrain[] array = (ReliefTerrain[])Object.FindObjectsOfType(typeof(ReliefTerrain));
		ReliefTerrainGlobalSettingsHolder[] array2 = new ReliefTerrainGlobalSettingsHolder[array.Length];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < num; j++)
			{
				if (array2[j] == array[i].globalSettingsHolder)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				array2[num++] = array[i].globalSettingsHolder;
			}
		}
		if (num > 1)
		{
			for (int k = 0; k < num; k++)
			{
				array2[k].useTerrainMaterial = true;
			}
		}
		for (int l = 0; l < num; l++)
		{
			if (array2[l] != this)
			{
				array2[l].trees_shadow_distance_start = this.trees_shadow_distance_start;
				array2[l].trees_shadow_distance_transition = this.trees_shadow_distance_transition;
				array2[l].trees_shadow_value = this.trees_shadow_value;
				array2[l].global_normalMap_multiplier = this.global_normalMap_multiplier;
				array2[l].trees_pixel_distance_start = this.trees_pixel_distance_start;
				array2[l].trees_pixel_distance_transition = this.trees_pixel_distance_transition;
				array2[l].trees_pixel_blend_val = this.trees_pixel_blend_val;
				array2[l].global_normalMap_farUsage = this.global_normalMap_farUsage;
				array2[l]._AmbientEmissiveMultiplier = this._AmbientEmissiveMultiplier;
				array2[l]._AmbientEmissiveRelief = this._AmbientEmissiveRelief;
				array2[l]._snow_strength = this._snow_strength;
				array2[l]._global_color_brightness_to_snow = this._global_color_brightness_to_snow;
				array2[l]._snow_slope_factor = this._snow_slope_factor;
				array2[l]._snow_edge_definition = this._snow_edge_definition;
				array2[l]._snow_height_treshold = this._snow_height_treshold;
				array2[l]._snow_height_transition = this._snow_height_transition;
				array2[l]._snow_color = this._snow_color;
				array2[l]._snow_specular = this._snow_specular;
				array2[l]._snow_gloss = this._snow_gloss;
				array2[l]._snow_reflectivness = this._snow_reflectivness;
				array2[l]._snow_deep_factor = this._snow_deep_factor;
				array2[l]._snow_fresnel = this._snow_fresnel;
				array2[l]._snow_diff_fresnel = this._snow_diff_fresnel;
				array2[l]._snow_IBL_DiffuseStrength = this._snow_IBL_DiffuseStrength;
				array2[l]._snow_IBL_SpecStrength = this._snow_IBL_SpecStrength;
				array2[l].TERRAIN_CausticsWaterLevel = this.TERRAIN_CausticsWaterLevel;
				array2[l].TERRAIN_CausticsWaterLevelByAngle = this.TERRAIN_CausticsWaterLevelByAngle;
				array2[l].TERRAIN_CausticsWaterDeepFadeLength = this.TERRAIN_CausticsWaterDeepFadeLength;
				array2[l].TERRAIN_CausticsWaterShallowFadeLength = this.TERRAIN_CausticsWaterShallowFadeLength;
				array2[l].TERRAIN_GlobalWetness = this.TERRAIN_GlobalWetness;
				array2[l].TERRAIN_RainIntensity = this.TERRAIN_RainIntensity;
				array2[l].rtp_customAmbientCorrection = this.rtp_customAmbientCorrection;
				array2[l].TERRAIN_IBL_DiffAO_Damp = this.TERRAIN_IBL_DiffAO_Damp;
				array2[l].TERRAIN_IBLRefl_SpecAO_Damp = this.TERRAIN_IBLRefl_SpecAO_Damp;
				array2[l].RTP_LightDefVector = this.RTP_LightDefVector;
				array2[l].RTP_LightDefVector = this.RTP_LightDefVector;
				array2[l].RTP_ReflexLightDiffuseColor = this.RTP_ReflexLightDiffuseColor;
				array2[l].RTP_ReflexLightDiffuseColor2 = this.RTP_ReflexLightDiffuseColor2;
				array2[l].RTP_ReflexLightSpecColor = this.RTP_ReflexLightSpecColor;
			}
		}
	}

	// Token: 0x0600222B RID: 8747 RVA: 0x00107B9C File Offset: 0x00105D9C
	public void RestorePreset(ReliefTerrainPresetHolder holder)
	{
		this.numLayers = holder.numLayers;
		this.splats = new Texture2D[holder.splats.Length];
		for (int i = 0; i < holder.splats.Length; i++)
		{
			this.splats[i] = holder.splats[i];
		}
		this.splat_atlases = new Texture2D[3];
		for (int j = 0; j < this.splat_atlases.Length; j++)
		{
			this.splat_atlases[j] = holder.splat_atlases[j];
		}
		this.gloss_baked = holder.gloss_baked;
		this.splats_glossBaked = new Texture2D[12];
		this.atlas_glossBaked = new Texture2D[3];
		this.RTP_MIP_BIAS = holder.RTP_MIP_BIAS;
		this._SpecColor = holder._SpecColor;
		this.RTP_DeferredAddPassSpec = holder.RTP_DeferredAddPassSpec;
		this.MasterLayerBrightness = holder.MasterLayerBrightness;
		this.MasterLayerSaturation = holder.MasterLayerSaturation;
		this.SuperDetailA_channel = holder.SuperDetailA_channel;
		this.SuperDetailB_channel = holder.SuperDetailB_channel;
		this.Bump01 = holder.Bump01;
		this.Bump23 = holder.Bump23;
		this.Bump45 = holder.Bump45;
		this.Bump67 = holder.Bump67;
		this.Bump89 = holder.Bump89;
		this.BumpAB = holder.BumpAB;
		this.SSColorCombinedA = holder.SSColorCombinedA;
		this.SSColorCombinedB = holder.SSColorCombinedB;
		this.BumpGlobal = holder.BumpGlobal;
		this.VerticalTexture = holder.VerticalTexture;
		this.BumpMapGlobalScale = holder.BumpMapGlobalScale;
		this.GlobalColorMapBlendValues = holder.GlobalColorMapBlendValues;
		this.GlobalColorMapSaturation = holder.GlobalColorMapSaturation;
		this.GlobalColorMapSaturationFar = holder.GlobalColorMapSaturationFar;
		this.GlobalColorMapDistortByPerlin = holder.GlobalColorMapDistortByPerlin;
		this.GlobalColorMapBrightness = holder.GlobalColorMapBrightness;
		this.GlobalColorMapBrightnessFar = holder.GlobalColorMapBrightnessFar;
		this._GlobalColorMapNearMIP = holder._GlobalColorMapNearMIP;
		this._FarNormalDamp = holder._FarNormalDamp;
		this.blendMultiplier = holder.blendMultiplier;
		this.HeightMap = holder.HeightMap;
		this.HeightMap2 = holder.HeightMap2;
		this.HeightMap3 = holder.HeightMap3;
		this.ReliefTransform = holder.ReliefTransform;
		this.DIST_STEPS = holder.DIST_STEPS;
		this.WAVELENGTH = holder.WAVELENGTH;
		this.ReliefBorderBlend = holder.ReliefBorderBlend;
		this.ExtrudeHeight = holder.ExtrudeHeight;
		this.LightmapShading = holder.LightmapShading;
		this.SHADOW_STEPS = holder.SHADOW_STEPS;
		this.WAVELENGTH_SHADOWS = holder.WAVELENGTH_SHADOWS;
		this.SelfShadowStrength = holder.SelfShadowStrength;
		this.ShadowSmoothing = holder.ShadowSmoothing;
		this.ShadowSoftnessFade = holder.ShadowSoftnessFade;
		this.distance_start = holder.distance_start;
		this.distance_transition = holder.distance_transition;
		this.distance_start_bumpglobal = holder.distance_start_bumpglobal;
		this.distance_transition_bumpglobal = holder.distance_transition_bumpglobal;
		this.rtp_perlin_start_val = holder.rtp_perlin_start_val;
		this._Phong = holder._Phong;
		this.tessHeight = holder.tessHeight;
		this._TessSubdivisions = holder._TessSubdivisions;
		this._TessSubdivisionsFar = holder._TessSubdivisionsFar;
		this._TessYOffset = holder._TessYOffset;
		this.trees_shadow_distance_start = holder.trees_shadow_distance_start;
		this.trees_shadow_distance_transition = holder.trees_shadow_distance_transition;
		this.trees_shadow_value = holder.trees_shadow_value;
		this.trees_pixel_distance_start = holder.trees_pixel_distance_start;
		this.trees_pixel_distance_transition = holder.trees_pixel_distance_transition;
		this.trees_pixel_blend_val = holder.trees_pixel_blend_val;
		this.global_normalMap_multiplier = holder.global_normalMap_multiplier;
		this.global_normalMap_farUsage = holder.global_normalMap_farUsage;
		this._AmbientEmissiveMultiplier = holder._AmbientEmissiveMultiplier;
		this._AmbientEmissiveRelief = holder._AmbientEmissiveRelief;
		this.rtp_mipoffset_globalnorm = holder.rtp_mipoffset_globalnorm;
		this._SuperDetailTiling = holder._SuperDetailTiling;
		this.SuperDetailA = holder.SuperDetailA;
		this.SuperDetailB = holder.SuperDetailB;
		this.TERRAIN_ReflectionMap = holder.TERRAIN_ReflectionMap;
		this.TERRAIN_ReflectionMap_channel = holder.TERRAIN_ReflectionMap_channel;
		this.TERRAIN_ReflColorA = holder.TERRAIN_ReflColorA;
		this.TERRAIN_ReflColorB = holder.TERRAIN_ReflColorB;
		this.TERRAIN_ReflColorC = holder.TERRAIN_ReflColorC;
		this.TERRAIN_ReflColorCenter = holder.TERRAIN_ReflColorCenter;
		this.TERRAIN_ReflGlossAttenuation = holder.TERRAIN_ReflGlossAttenuation;
		this.TERRAIN_ReflectionRotSpeed = holder.TERRAIN_ReflectionRotSpeed;
		this.TERRAIN_GlobalWetness = holder.TERRAIN_GlobalWetness;
		this.TERRAIN_RippleMap = holder.TERRAIN_RippleMap;
		this.TERRAIN_RippleScale = holder.TERRAIN_RippleScale;
		this.TERRAIN_FlowScale = holder.TERRAIN_FlowScale;
		this.TERRAIN_FlowSpeed = holder.TERRAIN_FlowSpeed;
		this.TERRAIN_FlowCycleScale = holder.TERRAIN_FlowCycleScale;
		this.TERRAIN_FlowMipOffset = holder.TERRAIN_FlowMipOffset;
		this.TERRAIN_WetDarkening = holder.TERRAIN_WetDarkening;
		this.TERRAIN_WetDropletsStrength = holder.TERRAIN_WetDropletsStrength;
		this.TERRAIN_WetHeight_Treshold = holder.TERRAIN_WetHeight_Treshold;
		this.TERRAIN_WetHeight_Transition = holder.TERRAIN_WetHeight_Transition;
		this.TERRAIN_RainIntensity = holder.TERRAIN_RainIntensity;
		this.TERRAIN_DropletsSpeed = holder.TERRAIN_DropletsSpeed;
		this.TERRAIN_mipoffset_flowSpeed = holder.TERRAIN_mipoffset_flowSpeed;
		this.TERRAIN_CausticsAnimSpeed = holder.TERRAIN_CausticsAnimSpeed;
		this.TERRAIN_CausticsColor = holder.TERRAIN_CausticsColor;
		this.TERRAIN_CausticsWaterLevel = holder.TERRAIN_CausticsWaterLevel;
		this.TERRAIN_CausticsWaterLevelByAngle = holder.TERRAIN_CausticsWaterLevelByAngle;
		this.TERRAIN_CausticsWaterDeepFadeLength = holder.TERRAIN_CausticsWaterDeepFadeLength;
		this.TERRAIN_CausticsWaterShallowFadeLength = holder.TERRAIN_CausticsWaterShallowFadeLength;
		this.TERRAIN_CausticsTilingScale = holder.TERRAIN_CausticsTilingScale;
		this.TERRAIN_CausticsTex = holder.TERRAIN_CausticsTex;
		this.rtp_customAmbientCorrection = holder.rtp_customAmbientCorrection;
		this.TERRAIN_IBL_DiffAO_Damp = holder.TERRAIN_IBL_DiffAO_Damp;
		this.TERRAIN_IBLRefl_SpecAO_Damp = holder.TERRAIN_IBLRefl_SpecAO_Damp;
		this._CubemapDiff = holder._CubemapDiff;
		this._CubemapSpec = holder._CubemapSpec;
		this.RTP_AOsharpness = holder.RTP_AOsharpness;
		this.RTP_AOamp = holder.RTP_AOamp;
		this.RTP_LightDefVector = holder.RTP_LightDefVector;
		this.RTP_ReflexLightDiffuseColor = holder.RTP_ReflexLightDiffuseColor;
		this.RTP_ReflexLightDiffuseColor2 = holder.RTP_ReflexLightDiffuseColor2;
		this.RTP_ReflexLightSpecColor = holder.RTP_ReflexLightSpecColor;
		this.EmissionRefractFiltering = holder.EmissionRefractFiltering;
		this.EmissionRefractAnimSpeed = holder.EmissionRefractAnimSpeed;
		this.VerticalTextureGlobalBumpInfluence = holder.VerticalTextureGlobalBumpInfluence;
		this.VerticalTextureTiling = holder.VerticalTextureTiling;
		this._snow_strength = holder._snow_strength;
		this._global_color_brightness_to_snow = holder._global_color_brightness_to_snow;
		this._snow_slope_factor = holder._snow_slope_factor;
		this._snow_edge_definition = holder._snow_edge_definition;
		this._snow_height_treshold = holder._snow_height_treshold;
		this._snow_height_transition = holder._snow_height_transition;
		this._snow_color = holder._snow_color;
		this._snow_specular = holder._snow_specular;
		this._snow_gloss = holder._snow_gloss;
		this._snow_reflectivness = holder._snow_reflectivness;
		this._snow_deep_factor = holder._snow_deep_factor;
		this._snow_fresnel = holder._snow_fresnel;
		this._snow_diff_fresnel = holder._snow_diff_fresnel;
		this._snow_IBL_DiffuseStrength = holder._snow_IBL_DiffuseStrength;
		this._snow_IBL_SpecStrength = holder._snow_IBL_SpecStrength;
		this.Bumps = new Texture2D[holder.Bumps.Length];
		this.Spec = new float[holder.Bumps.Length];
		this.FarSpecCorrection = new float[holder.Bumps.Length];
		this.MixScale = new float[holder.Bumps.Length];
		this.MixBlend = new float[holder.Bumps.Length];
		this.MixSaturation = new float[holder.Bumps.Length];
		this.RTP_gloss2mask = new float[holder.Bumps.Length];
		this.RTP_gloss_mult = new float[holder.Bumps.Length];
		this.RTP_gloss_shaping = new float[holder.Bumps.Length];
		this.RTP_Fresnel = new float[holder.Bumps.Length];
		this.RTP_FresnelAtten = new float[holder.Bumps.Length];
		this.RTP_DiffFresnel = new float[holder.Bumps.Length];
		this.RTP_IBL_bump_smoothness = new float[holder.Bumps.Length];
		this.RTP_IBL_DiffuseStrength = new float[holder.Bumps.Length];
		this.RTP_IBL_SpecStrength = new float[holder.Bumps.Length];
		this._DeferredSpecDampAddPass = new float[holder.Bumps.Length];
		this.MixBrightness = new float[holder.Bumps.Length];
		this.MixReplace = new float[holder.Bumps.Length];
		this.LayerBrightness = new float[holder.Bumps.Length];
		this.LayerBrightness2Spec = new float[holder.Bumps.Length];
		this.LayerAlbedo2SpecColor = new float[holder.Bumps.Length];
		this.LayerSaturation = new float[holder.Bumps.Length];
		this.LayerEmission = new float[holder.Bumps.Length];
		this.LayerEmissionColor = new Color[holder.Bumps.Length];
		this.LayerEmissionRefractStrength = new float[holder.Bumps.Length];
		this.LayerEmissionRefractHBedge = new float[holder.Bumps.Length];
		this.GlobalColorPerLayer = new float[holder.Bumps.Length];
		this.GlobalColorBottom = new float[holder.Bumps.Length];
		this.GlobalColorTop = new float[holder.Bumps.Length];
		this.GlobalColorColormapLoSat = new float[holder.Bumps.Length];
		this.GlobalColorColormapHiSat = new float[holder.Bumps.Length];
		this.GlobalColorLayerLoSat = new float[holder.Bumps.Length];
		this.GlobalColorLayerHiSat = new float[holder.Bumps.Length];
		this.GlobalColorLoBlend = new float[holder.Bumps.Length];
		this.GlobalColorHiBlend = new float[holder.Bumps.Length];
		this.PER_LAYER_HEIGHT_MODIFIER = new float[holder.Bumps.Length];
		this._SuperDetailStrengthMultA = new float[holder.Bumps.Length];
		this._SuperDetailStrengthMultASelfMaskNear = new float[holder.Bumps.Length];
		this._SuperDetailStrengthMultASelfMaskFar = new float[holder.Bumps.Length];
		this._SuperDetailStrengthMultB = new float[holder.Bumps.Length];
		this._SuperDetailStrengthMultBSelfMaskNear = new float[holder.Bumps.Length];
		this._SuperDetailStrengthMultBSelfMaskFar = new float[holder.Bumps.Length];
		this._SuperDetailStrengthNormal = new float[holder.Bumps.Length];
		this._BumpMapGlobalStrength = new float[holder.Bumps.Length];
		this.AO_strength = new float[holder.Bumps.Length];
		this.VerticalTextureStrength = new float[holder.Bumps.Length];
		this.Heights = new Texture2D[holder.Bumps.Length];
		this._snow_strength_per_layer = new float[holder.Bumps.Length];
		this.Substances = new ProceduralMaterial[holder.Bumps.Length];
		this.TERRAIN_LayerWetStrength = new float[holder.Bumps.Length];
		this.TERRAIN_WaterLevel = new float[holder.Bumps.Length];
		this.TERRAIN_WaterLevelSlopeDamp = new float[holder.Bumps.Length];
		this.TERRAIN_WaterEdge = new float[holder.Bumps.Length];
		this.TERRAIN_WaterSpecularity = new float[holder.Bumps.Length];
		this.TERRAIN_WaterGloss = new float[holder.Bumps.Length];
		this.TERRAIN_WaterGlossDamper = new float[holder.Bumps.Length];
		this.TERRAIN_WaterOpacity = new float[holder.Bumps.Length];
		this.TERRAIN_Refraction = new float[holder.Bumps.Length];
		this.TERRAIN_WetRefraction = new float[holder.Bumps.Length];
		this.TERRAIN_Flow = new float[holder.Bumps.Length];
		this.TERRAIN_WetFlow = new float[holder.Bumps.Length];
		this.TERRAIN_WetSpecularity = new float[holder.Bumps.Length];
		this.TERRAIN_WetGloss = new float[holder.Bumps.Length];
		this.TERRAIN_WaterColor = new Color[holder.Bumps.Length];
		this.TERRAIN_WaterIBL_SpecWetStrength = new float[holder.Bumps.Length];
		this.TERRAIN_WaterIBL_SpecWaterStrength = new float[holder.Bumps.Length];
		this.TERRAIN_WaterEmission = new float[holder.Bumps.Length];
		for (int k = 0; k < holder.Bumps.Length; k++)
		{
			this.Bumps[k] = holder.Bumps[k];
			this.Spec[k] = holder.Spec[k];
			this.FarSpecCorrection[k] = holder.FarSpecCorrection[k];
			this.MixScale[k] = holder.MixScale[k];
			this.MixBlend[k] = holder.MixBlend[k];
			this.MixSaturation[k] = holder.MixSaturation[k];
			if (this.CheckAndUpdate(ref holder.RTP_gloss2mask, 0.5f, holder.Bumps.Length))
			{
				for (int l = 0; l < this.numLayers; l++)
				{
					this.Spec[l] = 1f;
				}
			}
			this.CheckAndUpdate(ref holder.RTP_gloss_mult, 1f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.RTP_gloss_shaping, 0.5f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.RTP_Fresnel, 0f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.RTP_FresnelAtten, 0f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.RTP_DiffFresnel, 0f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.RTP_IBL_bump_smoothness, 0.7f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.RTP_IBL_DiffuseStrength, 0.5f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.RTP_IBL_SpecStrength, 0.5f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder._DeferredSpecDampAddPass, 1f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.TERRAIN_WaterSpecularity, 0.5f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.TERRAIN_WaterGloss, 0.1f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.TERRAIN_WaterGlossDamper, 0f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.TERRAIN_WetSpecularity, 0.2f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.TERRAIN_WetGloss, 0.05f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.TERRAIN_WetFlow, 0.05f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.MixBrightness, 2f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.MixReplace, 0f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.LayerBrightness, 1f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.LayerBrightness2Spec, 0f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.LayerAlbedo2SpecColor, 0f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.LayerSaturation, 1f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.LayerEmission, 1f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.LayerEmissionColor, Color.black, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.FarSpecCorrection, 0f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.LayerEmissionRefractStrength, 0f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.LayerEmissionRefractHBedge, 0f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.TERRAIN_WaterIBL_SpecWetStrength, 0.1f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.TERRAIN_WaterIBL_SpecWaterStrength, 0.5f, holder.Bumps.Length);
			this.CheckAndUpdate(ref holder.TERRAIN_WaterEmission, 0f, holder.Bumps.Length);
			this.RTP_gloss2mask[k] = holder.RTP_gloss2mask[k];
			this.RTP_gloss_mult[k] = holder.RTP_gloss_mult[k];
			this.RTP_gloss_shaping[k] = holder.RTP_gloss_shaping[k];
			this.RTP_Fresnel[k] = holder.RTP_Fresnel[k];
			this.RTP_FresnelAtten[k] = holder.RTP_FresnelAtten[k];
			this.RTP_DiffFresnel[k] = holder.RTP_DiffFresnel[k];
			this.RTP_IBL_bump_smoothness[k] = holder.RTP_IBL_bump_smoothness[k];
			this.RTP_IBL_DiffuseStrength[k] = holder.RTP_IBL_DiffuseStrength[k];
			this.RTP_IBL_SpecStrength[k] = holder.RTP_IBL_SpecStrength[k];
			this._DeferredSpecDampAddPass[k] = holder._DeferredSpecDampAddPass[k];
			this.MixBrightness[k] = holder.MixBrightness[k];
			this.MixReplace[k] = holder.MixReplace[k];
			this.LayerBrightness[k] = holder.LayerBrightness[k];
			this.LayerBrightness2Spec[k] = holder.LayerBrightness2Spec[k];
			this.LayerAlbedo2SpecColor[k] = holder.LayerAlbedo2SpecColor[k];
			this.LayerSaturation[k] = holder.LayerSaturation[k];
			this.LayerEmission[k] = holder.LayerEmission[k];
			this.LayerEmissionColor[k] = holder.LayerEmissionColor[k];
			this.LayerEmissionRefractStrength[k] = holder.LayerEmissionRefractStrength[k];
			this.LayerEmissionRefractHBedge[k] = holder.LayerEmissionRefractHBedge[k];
			this.GlobalColorPerLayer[k] = holder.GlobalColorPerLayer[k];
			this.GlobalColorBottom[k] = holder.GlobalColorBottom[k];
			this.GlobalColorTop[k] = holder.GlobalColorTop[k];
			this.GlobalColorColormapLoSat[k] = holder.GlobalColorColormapLoSat[k];
			this.GlobalColorColormapHiSat[k] = holder.GlobalColorColormapHiSat[k];
			this.GlobalColorLayerLoSat[k] = holder.GlobalColorLayerLoSat[k];
			this.GlobalColorLayerHiSat[k] = holder.GlobalColorLayerHiSat[k];
			this.GlobalColorLoBlend[k] = holder.GlobalColorLoBlend[k];
			this.GlobalColorHiBlend[k] = holder.GlobalColorHiBlend[k];
			this.PER_LAYER_HEIGHT_MODIFIER[k] = holder.PER_LAYER_HEIGHT_MODIFIER[k];
			this._SuperDetailStrengthMultA[k] = holder._SuperDetailStrengthMultA[k];
			this._SuperDetailStrengthMultASelfMaskNear[k] = holder._SuperDetailStrengthMultASelfMaskNear[k];
			this._SuperDetailStrengthMultASelfMaskFar[k] = holder._SuperDetailStrengthMultASelfMaskFar[k];
			this._SuperDetailStrengthMultB[k] = holder._SuperDetailStrengthMultB[k];
			this._SuperDetailStrengthMultBSelfMaskNear[k] = holder._SuperDetailStrengthMultBSelfMaskNear[k];
			this._SuperDetailStrengthMultBSelfMaskFar[k] = holder._SuperDetailStrengthMultBSelfMaskFar[k];
			this._SuperDetailStrengthNormal[k] = holder._SuperDetailStrengthNormal[k];
			this._BumpMapGlobalStrength[k] = holder._BumpMapGlobalStrength[k];
			this.VerticalTextureStrength[k] = holder.VerticalTextureStrength[k];
			this.AO_strength[k] = holder.AO_strength[k];
			this.Heights[k] = holder.Heights[k];
			this._snow_strength_per_layer[k] = holder._snow_strength_per_layer[k];
			this.Substances[k] = holder.Substances[k];
			this.TERRAIN_LayerWetStrength[k] = holder.TERRAIN_LayerWetStrength[k];
			this.TERRAIN_WaterLevel[k] = holder.TERRAIN_WaterLevel[k];
			this.TERRAIN_WaterLevelSlopeDamp[k] = holder.TERRAIN_WaterLevelSlopeDamp[k];
			this.TERRAIN_WaterEdge[k] = holder.TERRAIN_WaterEdge[k];
			this.TERRAIN_WaterSpecularity[k] = holder.TERRAIN_WaterSpecularity[k];
			this.TERRAIN_WaterGloss[k] = holder.TERRAIN_WaterGloss[k];
			this.TERRAIN_WaterGlossDamper[k] = holder.TERRAIN_WaterGlossDamper[k];
			this.TERRAIN_WaterOpacity[k] = holder.TERRAIN_WaterOpacity[k];
			this.TERRAIN_Refraction[k] = holder.TERRAIN_Refraction[k];
			this.TERRAIN_WetRefraction[k] = holder.TERRAIN_WetRefraction[k];
			this.TERRAIN_Flow[k] = holder.TERRAIN_Flow[k];
			this.TERRAIN_WetFlow[k] = holder.TERRAIN_WetFlow[k];
			this.TERRAIN_WetSpecularity[k] = holder.TERRAIN_WetSpecularity[k];
			this.TERRAIN_WetGloss[k] = holder.TERRAIN_WetGloss[k];
			this.TERRAIN_WaterColor[k] = holder.TERRAIN_WaterColor[k];
			this.TERRAIN_WaterIBL_SpecWetStrength[k] = holder.TERRAIN_WaterIBL_SpecWetStrength[k];
			this.TERRAIN_WaterIBL_SpecWaterStrength[k] = holder.TERRAIN_WaterIBL_SpecWaterStrength[k];
			this.TERRAIN_WaterEmission[k] = holder.TERRAIN_WaterEmission[k];
		}
	}

	// Token: 0x0600222C RID: 8748 RVA: 0x00108EC4 File Offset: 0x001070C4
	public void SavePreset(ref ReliefTerrainPresetHolder holder)
	{
		holder.numLayers = this.numLayers;
		holder.splats = new Texture2D[this.splats.Length];
		for (int i = 0; i < holder.splats.Length; i++)
		{
			holder.splats[i] = this.splats[i];
		}
		holder.splat_atlases = new Texture2D[3];
		for (int j = 0; j < this.splat_atlases.Length; j++)
		{
			holder.splat_atlases[j] = this.splat_atlases[j];
		}
		holder.gloss_baked = this.gloss_baked;
		holder.RTP_MIP_BIAS = this.RTP_MIP_BIAS;
		holder._SpecColor = this._SpecColor;
		holder.RTP_DeferredAddPassSpec = this.RTP_DeferredAddPassSpec;
		holder.MasterLayerBrightness = this.MasterLayerBrightness;
		holder.MasterLayerSaturation = this.MasterLayerSaturation;
		holder.SuperDetailA_channel = this.SuperDetailA_channel;
		holder.SuperDetailB_channel = this.SuperDetailB_channel;
		holder.Bump01 = this.Bump01;
		holder.Bump23 = this.Bump23;
		holder.Bump45 = this.Bump45;
		holder.Bump67 = this.Bump67;
		holder.Bump89 = this.Bump89;
		holder.BumpAB = this.BumpAB;
		holder.SSColorCombinedA = this.SSColorCombinedA;
		holder.SSColorCombinedB = this.SSColorCombinedB;
		holder.BumpGlobal = this.BumpGlobal;
		holder.VerticalTexture = this.VerticalTexture;
		holder.BumpMapGlobalScale = this.BumpMapGlobalScale;
		holder.GlobalColorMapBlendValues = this.GlobalColorMapBlendValues;
		holder.GlobalColorMapSaturation = this.GlobalColorMapSaturation;
		holder.GlobalColorMapSaturationFar = this.GlobalColorMapSaturationFar;
		holder.GlobalColorMapDistortByPerlin = this.GlobalColorMapDistortByPerlin;
		holder.GlobalColorMapBrightness = this.GlobalColorMapBrightness;
		holder.GlobalColorMapBrightnessFar = this.GlobalColorMapBrightnessFar;
		holder._GlobalColorMapNearMIP = this._GlobalColorMapNearMIP;
		holder._FarNormalDamp = this._FarNormalDamp;
		holder.blendMultiplier = this.blendMultiplier;
		holder.HeightMap = this.HeightMap;
		holder.HeightMap2 = this.HeightMap2;
		holder.HeightMap3 = this.HeightMap3;
		holder.ReliefTransform = this.ReliefTransform;
		holder.DIST_STEPS = this.DIST_STEPS;
		holder.WAVELENGTH = this.WAVELENGTH;
		holder.ReliefBorderBlend = this.ReliefBorderBlend;
		holder.ExtrudeHeight = this.ExtrudeHeight;
		holder.LightmapShading = this.LightmapShading;
		holder.SHADOW_STEPS = this.SHADOW_STEPS;
		holder.WAVELENGTH_SHADOWS = this.WAVELENGTH_SHADOWS;
		holder.SelfShadowStrength = this.SelfShadowStrength;
		holder.ShadowSmoothing = this.ShadowSmoothing;
		holder.ShadowSoftnessFade = this.ShadowSoftnessFade;
		holder.distance_start = this.distance_start;
		holder.distance_transition = this.distance_transition;
		holder.distance_start_bumpglobal = this.distance_start_bumpglobal;
		holder.distance_transition_bumpglobal = this.distance_transition_bumpglobal;
		holder.rtp_perlin_start_val = this.rtp_perlin_start_val;
		holder._Phong = this._Phong;
		holder.tessHeight = this.tessHeight;
		holder._TessSubdivisions = this._TessSubdivisions;
		holder._TessSubdivisionsFar = this._TessSubdivisionsFar;
		holder._TessYOffset = this._TessYOffset;
		holder.trees_shadow_distance_start = this.trees_shadow_distance_start;
		holder.trees_shadow_distance_transition = this.trees_shadow_distance_transition;
		holder.trees_shadow_value = this.trees_shadow_value;
		holder.trees_pixel_distance_start = this.trees_pixel_distance_start;
		holder.trees_pixel_distance_transition = this.trees_pixel_distance_transition;
		holder.trees_pixel_blend_val = this.trees_pixel_blend_val;
		holder.global_normalMap_multiplier = this.global_normalMap_multiplier;
		holder.global_normalMap_farUsage = this.global_normalMap_farUsage;
		holder._AmbientEmissiveMultiplier = this._AmbientEmissiveMultiplier;
		holder._AmbientEmissiveRelief = this._AmbientEmissiveRelief;
		holder.rtp_mipoffset_globalnorm = this.rtp_mipoffset_globalnorm;
		holder._SuperDetailTiling = this._SuperDetailTiling;
		holder.SuperDetailA = this.SuperDetailA;
		holder.SuperDetailB = this.SuperDetailB;
		holder.TERRAIN_ReflectionMap = this.TERRAIN_ReflectionMap;
		holder.TERRAIN_ReflectionMap_channel = this.TERRAIN_ReflectionMap_channel;
		holder.TERRAIN_ReflColorA = this.TERRAIN_ReflColorA;
		holder.TERRAIN_ReflColorB = this.TERRAIN_ReflColorB;
		holder.TERRAIN_ReflColorC = this.TERRAIN_ReflColorC;
		holder.TERRAIN_ReflColorCenter = this.TERRAIN_ReflColorCenter;
		holder.TERRAIN_ReflGlossAttenuation = this.TERRAIN_ReflGlossAttenuation;
		holder.TERRAIN_ReflectionRotSpeed = this.TERRAIN_ReflectionRotSpeed;
		holder.TERRAIN_GlobalWetness = this.TERRAIN_GlobalWetness;
		holder.TERRAIN_RippleMap = this.TERRAIN_RippleMap;
		holder.TERRAIN_RippleScale = this.TERRAIN_RippleScale;
		holder.TERRAIN_FlowScale = this.TERRAIN_FlowScale;
		holder.TERRAIN_FlowSpeed = this.TERRAIN_FlowSpeed;
		holder.TERRAIN_FlowCycleScale = this.TERRAIN_FlowCycleScale;
		holder.TERRAIN_FlowMipOffset = this.TERRAIN_FlowMipOffset;
		holder.TERRAIN_WetDarkening = this.TERRAIN_WetDarkening;
		holder.TERRAIN_WetDropletsStrength = this.TERRAIN_WetDropletsStrength;
		holder.TERRAIN_WetHeight_Treshold = this.TERRAIN_WetHeight_Treshold;
		holder.TERRAIN_WetHeight_Transition = this.TERRAIN_WetHeight_Transition;
		holder.TERRAIN_RainIntensity = this.TERRAIN_RainIntensity;
		holder.TERRAIN_DropletsSpeed = this.TERRAIN_DropletsSpeed;
		holder.TERRAIN_mipoffset_flowSpeed = this.TERRAIN_mipoffset_flowSpeed;
		holder.TERRAIN_CausticsAnimSpeed = this.TERRAIN_CausticsAnimSpeed;
		holder.TERRAIN_CausticsColor = this.TERRAIN_CausticsColor;
		holder.TERRAIN_CausticsWaterLevel = this.TERRAIN_CausticsWaterLevel;
		holder.TERRAIN_CausticsWaterLevelByAngle = this.TERRAIN_CausticsWaterLevelByAngle;
		holder.TERRAIN_CausticsWaterDeepFadeLength = this.TERRAIN_CausticsWaterDeepFadeLength;
		holder.TERRAIN_CausticsWaterShallowFadeLength = this.TERRAIN_CausticsWaterShallowFadeLength;
		holder.TERRAIN_CausticsTilingScale = this.TERRAIN_CausticsTilingScale;
		holder.TERRAIN_CausticsTex = this.TERRAIN_CausticsTex;
		holder.rtp_customAmbientCorrection = this.rtp_customAmbientCorrection;
		holder.TERRAIN_IBL_DiffAO_Damp = this.TERRAIN_IBL_DiffAO_Damp;
		holder.TERRAIN_IBLRefl_SpecAO_Damp = this.TERRAIN_IBLRefl_SpecAO_Damp;
		holder._CubemapDiff = this._CubemapDiff;
		holder._CubemapSpec = this._CubemapSpec;
		holder.RTP_AOsharpness = this.RTP_AOsharpness;
		holder.RTP_AOamp = this.RTP_AOamp;
		holder.RTP_LightDefVector = this.RTP_LightDefVector;
		holder.RTP_ReflexLightDiffuseColor = this.RTP_ReflexLightDiffuseColor;
		holder.RTP_ReflexLightDiffuseColor2 = this.RTP_ReflexLightDiffuseColor2;
		holder.RTP_ReflexLightSpecColor = this.RTP_ReflexLightSpecColor;
		holder.EmissionRefractFiltering = this.EmissionRefractFiltering;
		holder.EmissionRefractAnimSpeed = this.EmissionRefractAnimSpeed;
		holder.VerticalTextureGlobalBumpInfluence = this.VerticalTextureGlobalBumpInfluence;
		holder.VerticalTextureTiling = this.VerticalTextureTiling;
		holder._snow_strength = this._snow_strength;
		holder._global_color_brightness_to_snow = this._global_color_brightness_to_snow;
		holder._snow_slope_factor = this._snow_slope_factor;
		holder._snow_edge_definition = this._snow_edge_definition;
		holder._snow_height_treshold = this._snow_height_treshold;
		holder._snow_height_transition = this._snow_height_transition;
		holder._snow_color = this._snow_color;
		holder._snow_specular = this._snow_specular;
		holder._snow_gloss = this._snow_gloss;
		holder._snow_reflectivness = this._snow_reflectivness;
		holder._snow_deep_factor = this._snow_deep_factor;
		holder._snow_fresnel = this._snow_fresnel;
		holder._snow_diff_fresnel = this._snow_diff_fresnel;
		holder._snow_IBL_DiffuseStrength = this._snow_IBL_DiffuseStrength;
		holder._snow_IBL_SpecStrength = this._snow_IBL_SpecStrength;
		holder.Bumps = new Texture2D[this.numLayers];
		holder.Spec = new float[this.numLayers];
		holder.FarSpecCorrection = new float[this.numLayers];
		holder.MixScale = new float[this.numLayers];
		holder.MixBlend = new float[this.numLayers];
		holder.MixSaturation = new float[this.numLayers];
		holder.RTP_gloss2mask = new float[this.numLayers];
		holder.RTP_gloss_mult = new float[this.numLayers];
		holder.RTP_gloss_shaping = new float[this.numLayers];
		holder.RTP_Fresnel = new float[this.numLayers];
		holder.RTP_FresnelAtten = new float[this.numLayers];
		holder.RTP_DiffFresnel = new float[this.numLayers];
		holder.RTP_IBL_bump_smoothness = new float[this.numLayers];
		holder.RTP_IBL_DiffuseStrength = new float[this.numLayers];
		holder.RTP_IBL_SpecStrength = new float[this.numLayers];
		holder._DeferredSpecDampAddPass = new float[this.numLayers];
		holder.MixBrightness = new float[this.numLayers];
		holder.MixReplace = new float[this.numLayers];
		holder.LayerBrightness = new float[this.numLayers];
		holder.LayerBrightness2Spec = new float[this.numLayers];
		holder.LayerAlbedo2SpecColor = new float[this.numLayers];
		holder.LayerSaturation = new float[this.numLayers];
		holder.LayerEmission = new float[this.numLayers];
		holder.LayerEmissionColor = new Color[this.numLayers];
		holder.LayerEmissionRefractStrength = new float[this.numLayers];
		holder.LayerEmissionRefractHBedge = new float[this.numLayers];
		holder.GlobalColorPerLayer = new float[this.numLayers];
		holder.GlobalColorBottom = new float[this.numLayers];
		holder.GlobalColorTop = new float[this.numLayers];
		holder.GlobalColorColormapLoSat = new float[this.numLayers];
		holder.GlobalColorColormapHiSat = new float[this.numLayers];
		holder.GlobalColorLayerLoSat = new float[this.numLayers];
		holder.GlobalColorLayerHiSat = new float[this.numLayers];
		holder.GlobalColorLoBlend = new float[this.numLayers];
		holder.GlobalColorHiBlend = new float[this.numLayers];
		holder.PER_LAYER_HEIGHT_MODIFIER = new float[this.numLayers];
		holder._SuperDetailStrengthMultA = new float[this.numLayers];
		holder._SuperDetailStrengthMultASelfMaskNear = new float[this.numLayers];
		holder._SuperDetailStrengthMultASelfMaskFar = new float[this.numLayers];
		holder._SuperDetailStrengthMultB = new float[this.numLayers];
		holder._SuperDetailStrengthMultBSelfMaskNear = new float[this.numLayers];
		holder._SuperDetailStrengthMultBSelfMaskFar = new float[this.numLayers];
		holder._SuperDetailStrengthNormal = new float[this.numLayers];
		holder._BumpMapGlobalStrength = new float[this.numLayers];
		holder.VerticalTextureStrength = new float[this.numLayers];
		holder.AO_strength = new float[this.numLayers];
		holder.Heights = new Texture2D[this.numLayers];
		holder._snow_strength_per_layer = new float[this.numLayers];
		holder.Substances = new ProceduralMaterial[this.numLayers];
		holder.TERRAIN_LayerWetStrength = new float[this.numLayers];
		holder.TERRAIN_WaterLevel = new float[this.numLayers];
		holder.TERRAIN_WaterLevelSlopeDamp = new float[this.numLayers];
		holder.TERRAIN_WaterEdge = new float[this.numLayers];
		holder.TERRAIN_WaterSpecularity = new float[this.numLayers];
		holder.TERRAIN_WaterGloss = new float[this.numLayers];
		holder.TERRAIN_WaterGlossDamper = new float[this.numLayers];
		holder.TERRAIN_WaterOpacity = new float[this.numLayers];
		holder.TERRAIN_Refraction = new float[this.numLayers];
		holder.TERRAIN_WetRefraction = new float[this.numLayers];
		holder.TERRAIN_Flow = new float[this.numLayers];
		holder.TERRAIN_WetFlow = new float[this.numLayers];
		holder.TERRAIN_WetSpecularity = new float[this.numLayers];
		holder.TERRAIN_WetGloss = new float[this.numLayers];
		holder.TERRAIN_WaterColor = new Color[this.numLayers];
		holder.TERRAIN_WaterIBL_SpecWetStrength = new float[this.numLayers];
		holder.TERRAIN_WaterIBL_SpecWaterStrength = new float[this.numLayers];
		holder.TERRAIN_WaterEmission = new float[this.numLayers];
		for (int k = 0; k < this.numLayers; k++)
		{
			holder.Bumps[k] = this.Bumps[k];
			holder.Spec[k] = this.Spec[k];
			holder.FarSpecCorrection[k] = this.FarSpecCorrection[k];
			holder.MixScale[k] = this.MixScale[k];
			holder.MixBlend[k] = this.MixBlend[k];
			holder.MixSaturation[k] = this.MixSaturation[k];
			if (this.CheckAndUpdate(ref this.RTP_gloss2mask, 0.5f, this.numLayers))
			{
				for (int l = 0; l < this.numLayers; l++)
				{
					this.Spec[l] = 1f;
				}
			}
			this.CheckAndUpdate(ref this.RTP_gloss_mult, 1f, this.numLayers);
			this.CheckAndUpdate(ref this.RTP_gloss_shaping, 0.5f, this.numLayers);
			this.CheckAndUpdate(ref this.RTP_Fresnel, 0f, this.numLayers);
			this.CheckAndUpdate(ref this.RTP_FresnelAtten, 0f, this.numLayers);
			this.CheckAndUpdate(ref this.RTP_DiffFresnel, 0f, this.numLayers);
			this.CheckAndUpdate(ref this.RTP_IBL_bump_smoothness, 0.7f, this.numLayers);
			this.CheckAndUpdate(ref this.RTP_IBL_DiffuseStrength, 0.5f, this.numLayers);
			this.CheckAndUpdate(ref this.RTP_IBL_SpecStrength, 0.5f, this.numLayers);
			this.CheckAndUpdate(ref this._DeferredSpecDampAddPass, 1f, this.numLayers);
			this.CheckAndUpdate(ref this.TERRAIN_WaterSpecularity, 0.5f, this.numLayers);
			this.CheckAndUpdate(ref this.TERRAIN_WaterGloss, 0.1f, this.numLayers);
			this.CheckAndUpdate(ref this.TERRAIN_WaterGlossDamper, 0f, this.numLayers);
			this.CheckAndUpdate(ref this.TERRAIN_WetSpecularity, 0.2f, this.numLayers);
			this.CheckAndUpdate(ref this.TERRAIN_WetGloss, 0.05f, this.numLayers);
			this.CheckAndUpdate(ref this.TERRAIN_WetFlow, 0.05f, this.numLayers);
			this.CheckAndUpdate(ref this.MixBrightness, 2f, this.numLayers);
			this.CheckAndUpdate(ref this.MixReplace, 0f, this.numLayers);
			this.CheckAndUpdate(ref this.LayerBrightness, 1f, this.numLayers);
			this.CheckAndUpdate(ref this.LayerBrightness2Spec, 0f, this.numLayers);
			this.CheckAndUpdate(ref this.LayerAlbedo2SpecColor, 0f, this.numLayers);
			this.CheckAndUpdate(ref this.LayerSaturation, 1f, this.numLayers);
			this.CheckAndUpdate(ref this.LayerEmission, 0f, this.numLayers);
			this.CheckAndUpdate(ref this.LayerEmissionColor, Color.black, this.numLayers);
			this.CheckAndUpdate(ref this.LayerEmissionRefractStrength, 0f, this.numLayers);
			this.CheckAndUpdate(ref this.LayerEmissionRefractHBedge, 0f, this.numLayers);
			this.CheckAndUpdate(ref this.TERRAIN_WaterIBL_SpecWetStrength, 0.1f, this.numLayers);
			this.CheckAndUpdate(ref this.TERRAIN_WaterIBL_SpecWaterStrength, 0.5f, this.numLayers);
			this.CheckAndUpdate(ref this.TERRAIN_WaterEmission, 0.5f, this.numLayers);
			holder.RTP_gloss2mask[k] = this.RTP_gloss2mask[k];
			holder.RTP_gloss_mult[k] = this.RTP_gloss_mult[k];
			holder.RTP_gloss_shaping[k] = this.RTP_gloss_shaping[k];
			holder.RTP_Fresnel[k] = this.RTP_Fresnel[k];
			holder.RTP_FresnelAtten[k] = this.RTP_FresnelAtten[k];
			holder.RTP_DiffFresnel[k] = this.RTP_DiffFresnel[k];
			holder.RTP_IBL_bump_smoothness[k] = this.RTP_IBL_bump_smoothness[k];
			holder.RTP_IBL_DiffuseStrength[k] = this.RTP_IBL_DiffuseStrength[k];
			holder.RTP_IBL_SpecStrength[k] = this.RTP_IBL_SpecStrength[k];
			holder._DeferredSpecDampAddPass[k] = this._DeferredSpecDampAddPass[k];
			holder.TERRAIN_WaterIBL_SpecWetStrength[k] = this.TERRAIN_WaterIBL_SpecWetStrength[k];
			holder.TERRAIN_WaterIBL_SpecWaterStrength[k] = this.TERRAIN_WaterIBL_SpecWaterStrength[k];
			holder.TERRAIN_WaterEmission[k] = this.TERRAIN_WaterEmission[k];
			holder.MixBrightness[k] = this.MixBrightness[k];
			holder.MixReplace[k] = this.MixReplace[k];
			holder.LayerBrightness[k] = this.LayerBrightness[k];
			holder.LayerBrightness2Spec[k] = this.LayerBrightness2Spec[k];
			holder.LayerAlbedo2SpecColor[k] = this.LayerAlbedo2SpecColor[k];
			holder.LayerSaturation[k] = this.LayerSaturation[k];
			holder.LayerEmission[k] = this.LayerEmission[k];
			holder.LayerEmissionColor[k] = this.LayerEmissionColor[k];
			holder.LayerEmissionRefractStrength[k] = this.LayerEmissionRefractStrength[k];
			holder.LayerEmissionRefractHBedge[k] = this.LayerEmissionRefractHBedge[k];
			holder.GlobalColorPerLayer[k] = this.GlobalColorPerLayer[k];
			holder.GlobalColorBottom[k] = this.GlobalColorBottom[k];
			holder.GlobalColorTop[k] = this.GlobalColorTop[k];
			holder.GlobalColorColormapLoSat[k] = this.GlobalColorColormapLoSat[k];
			holder.GlobalColorColormapHiSat[k] = this.GlobalColorColormapHiSat[k];
			holder.GlobalColorLayerLoSat[k] = this.GlobalColorLayerLoSat[k];
			holder.GlobalColorLayerHiSat[k] = this.GlobalColorLayerHiSat[k];
			holder.GlobalColorLoBlend[k] = this.GlobalColorLoBlend[k];
			holder.GlobalColorHiBlend[k] = this.GlobalColorHiBlend[k];
			holder.PER_LAYER_HEIGHT_MODIFIER[k] = this.PER_LAYER_HEIGHT_MODIFIER[k];
			holder._SuperDetailStrengthMultA[k] = this._SuperDetailStrengthMultA[k];
			holder._SuperDetailStrengthMultASelfMaskNear[k] = this._SuperDetailStrengthMultASelfMaskNear[k];
			holder._SuperDetailStrengthMultASelfMaskFar[k] = this._SuperDetailStrengthMultASelfMaskFar[k];
			holder._SuperDetailStrengthMultB[k] = this._SuperDetailStrengthMultB[k];
			holder._SuperDetailStrengthMultBSelfMaskNear[k] = this._SuperDetailStrengthMultBSelfMaskNear[k];
			holder._SuperDetailStrengthMultBSelfMaskFar[k] = this._SuperDetailStrengthMultBSelfMaskFar[k];
			holder._SuperDetailStrengthNormal[k] = this._SuperDetailStrengthNormal[k];
			holder._BumpMapGlobalStrength[k] = this._BumpMapGlobalStrength[k];
			holder.VerticalTextureStrength[k] = this.VerticalTextureStrength[k];
			holder.AO_strength[k] = this.AO_strength[k];
			holder.Heights[k] = this.Heights[k];
			holder._snow_strength_per_layer[k] = this._snow_strength_per_layer[k];
			holder.Substances[k] = this.Substances[k];
			holder.TERRAIN_LayerWetStrength[k] = this.TERRAIN_LayerWetStrength[k];
			holder.TERRAIN_WaterLevel[k] = this.TERRAIN_WaterLevel[k];
			holder.TERRAIN_WaterLevelSlopeDamp[k] = this.TERRAIN_WaterLevelSlopeDamp[k];
			holder.TERRAIN_WaterEdge[k] = this.TERRAIN_WaterEdge[k];
			holder.TERRAIN_WaterSpecularity[k] = this.TERRAIN_WaterSpecularity[k];
			holder.TERRAIN_WaterGloss[k] = this.TERRAIN_WaterGloss[k];
			holder.TERRAIN_WaterGlossDamper[k] = this.TERRAIN_WaterGlossDamper[k];
			holder.TERRAIN_WaterOpacity[k] = this.TERRAIN_WaterOpacity[k];
			holder.TERRAIN_Refraction[k] = this.TERRAIN_Refraction[k];
			holder.TERRAIN_WetRefraction[k] = this.TERRAIN_WetRefraction[k];
			holder.TERRAIN_Flow[k] = this.TERRAIN_Flow[k];
			holder.TERRAIN_WetFlow[k] = this.TERRAIN_WetFlow[k];
			holder.TERRAIN_WetSpecularity[k] = this.TERRAIN_WetSpecularity[k];
			holder.TERRAIN_WetGloss[k] = this.TERRAIN_WetGloss[k];
			holder.TERRAIN_WaterColor[k] = this.TERRAIN_WaterColor[k];
		}
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x0010A200 File Offset: 0x00108400
	public void InterpolatePresets(ReliefTerrainPresetHolder holderA, ReliefTerrainPresetHolder holderB, float t)
	{
		this.RTP_MIP_BIAS = Mathf.Lerp(holderA.RTP_MIP_BIAS, holderB.RTP_MIP_BIAS, t);
		this._SpecColor = Color.Lerp(holderA._SpecColor, holderB._SpecColor, t);
		this.RTP_DeferredAddPassSpec = Mathf.Lerp(holderA.RTP_DeferredAddPassSpec, holderB.RTP_DeferredAddPassSpec, t);
		this.MasterLayerBrightness = Mathf.Lerp(holderA.MasterLayerBrightness, holderB.MasterLayerBrightness, t);
		this.MasterLayerSaturation = Mathf.Lerp(holderA.MasterLayerSaturation, holderB.MasterLayerSaturation, t);
		this.BumpMapGlobalScale = Mathf.Lerp(holderA.BumpMapGlobalScale, holderB.BumpMapGlobalScale, t);
		this.GlobalColorMapBlendValues = Vector3.Lerp(holderA.GlobalColorMapBlendValues, holderB.GlobalColorMapBlendValues, t);
		this.GlobalColorMapSaturation = Mathf.Lerp(holderA.GlobalColorMapSaturation, holderB.GlobalColorMapSaturation, t);
		this.GlobalColorMapSaturationFar = Mathf.Lerp(holderA.GlobalColorMapSaturationFar, holderB.GlobalColorMapSaturationFar, t);
		this.GlobalColorMapDistortByPerlin = Mathf.Lerp(holderA.GlobalColorMapDistortByPerlin, holderB.GlobalColorMapDistortByPerlin, t);
		this.GlobalColorMapBrightness = Mathf.Lerp(holderA.GlobalColorMapBrightness, holderB.GlobalColorMapBrightness, t);
		this.GlobalColorMapBrightnessFar = Mathf.Lerp(holderA.GlobalColorMapBrightnessFar, holderB.GlobalColorMapBrightnessFar, t);
		this._GlobalColorMapNearMIP = Mathf.Lerp(holderA._GlobalColorMapNearMIP, holderB._GlobalColorMapNearMIP, t);
		this._FarNormalDamp = Mathf.Lerp(holderA._FarNormalDamp, holderB._FarNormalDamp, t);
		this.blendMultiplier = Mathf.Lerp(holderA.blendMultiplier, holderB.blendMultiplier, t);
		this.ReliefTransform = Vector4.Lerp(holderA.ReliefTransform, holderB.ReliefTransform, t);
		this.DIST_STEPS = Mathf.Lerp(holderA.DIST_STEPS, holderB.DIST_STEPS, t);
		this.WAVELENGTH = Mathf.Lerp(holderA.WAVELENGTH, holderB.WAVELENGTH, t);
		this.ReliefBorderBlend = Mathf.Lerp(holderA.ReliefBorderBlend, holderB.ReliefBorderBlend, t);
		this.ExtrudeHeight = Mathf.Lerp(holderA.ExtrudeHeight, holderB.ExtrudeHeight, t);
		this.LightmapShading = Mathf.Lerp(holderA.LightmapShading, holderB.LightmapShading, t);
		this.SHADOW_STEPS = Mathf.Lerp(holderA.SHADOW_STEPS, holderB.SHADOW_STEPS, t);
		this.WAVELENGTH_SHADOWS = Mathf.Lerp(holderA.WAVELENGTH_SHADOWS, holderB.WAVELENGTH_SHADOWS, t);
		this.SelfShadowStrength = Mathf.Lerp(holderA.SelfShadowStrength, holderB.SelfShadowStrength, t);
		this.ShadowSmoothing = Mathf.Lerp(holderA.ShadowSmoothing, holderB.ShadowSmoothing, t);
		this.ShadowSoftnessFade = Mathf.Lerp(holderA.ShadowSoftnessFade, holderB.ShadowSoftnessFade, t);
		this.distance_start = Mathf.Lerp(holderA.distance_start, holderB.distance_start, t);
		this.distance_transition = Mathf.Lerp(holderA.distance_transition, holderB.distance_transition, t);
		this.distance_start_bumpglobal = Mathf.Lerp(holderA.distance_start_bumpglobal, holderB.distance_start_bumpglobal, t);
		this.distance_transition_bumpglobal = Mathf.Lerp(holderA.distance_transition_bumpglobal, holderB.distance_transition_bumpglobal, t);
		this.rtp_perlin_start_val = Mathf.Lerp(holderA.rtp_perlin_start_val, holderB.rtp_perlin_start_val, t);
		this.trees_shadow_distance_start = Mathf.Lerp(holderA.trees_shadow_distance_start, holderB.trees_shadow_distance_start, t);
		this.trees_shadow_distance_transition = Mathf.Lerp(holderA.trees_shadow_distance_transition, holderB.trees_shadow_distance_transition, t);
		this.trees_shadow_value = Mathf.Lerp(holderA.trees_shadow_value, holderB.trees_shadow_value, t);
		this.trees_pixel_distance_start = Mathf.Lerp(holderA.trees_pixel_distance_start, holderB.trees_pixel_distance_start, t);
		this.trees_pixel_distance_transition = Mathf.Lerp(holderA.trees_pixel_distance_transition, holderB.trees_pixel_distance_transition, t);
		this.trees_pixel_blend_val = Mathf.Lerp(holderA.trees_pixel_blend_val, holderB.trees_pixel_blend_val, t);
		this.global_normalMap_multiplier = Mathf.Lerp(holderA.global_normalMap_multiplier, holderB.global_normalMap_multiplier, t);
		this.global_normalMap_farUsage = Mathf.Lerp(holderA.global_normalMap_farUsage, holderB.global_normalMap_farUsage, t);
		this._AmbientEmissiveMultiplier = Mathf.Lerp(holderA._AmbientEmissiveMultiplier, holderB._AmbientEmissiveMultiplier, t);
		this._AmbientEmissiveRelief = Mathf.Lerp(holderA._AmbientEmissiveRelief, holderB._AmbientEmissiveRelief, t);
		this._SuperDetailTiling = Mathf.Lerp(holderA._SuperDetailTiling, holderB._SuperDetailTiling, t);
		this.TERRAIN_ReflColorA = Color.Lerp(holderA.TERRAIN_ReflColorA, holderB.TERRAIN_ReflColorA, t);
		this.TERRAIN_ReflColorB = Color.Lerp(holderA.TERRAIN_ReflColorB, holderB.TERRAIN_ReflColorB, t);
		this.TERRAIN_ReflColorC = Color.Lerp(holderA.TERRAIN_ReflColorC, holderB.TERRAIN_ReflColorC, t);
		this.TERRAIN_ReflColorCenter = Mathf.Lerp(holderA.TERRAIN_ReflColorCenter, holderB.TERRAIN_ReflColorCenter, t);
		this.TERRAIN_ReflGlossAttenuation = Mathf.Lerp(holderA.TERRAIN_ReflGlossAttenuation, holderB.TERRAIN_ReflGlossAttenuation, t);
		this.TERRAIN_ReflectionRotSpeed = Mathf.Lerp(holderA.TERRAIN_ReflectionRotSpeed, holderB.TERRAIN_ReflectionRotSpeed, t);
		this.TERRAIN_GlobalWetness = Mathf.Lerp(holderA.TERRAIN_GlobalWetness, holderB.TERRAIN_GlobalWetness, t);
		this.TERRAIN_RippleScale = Mathf.Lerp(holderA.TERRAIN_RippleScale, holderB.TERRAIN_RippleScale, t);
		this.TERRAIN_FlowScale = Mathf.Lerp(holderA.TERRAIN_FlowScale, holderB.TERRAIN_FlowScale, t);
		this.TERRAIN_FlowSpeed = Mathf.Lerp(holderA.TERRAIN_FlowSpeed, holderB.TERRAIN_FlowSpeed, t);
		this.TERRAIN_FlowCycleScale = Mathf.Lerp(holderA.TERRAIN_FlowCycleScale, holderB.TERRAIN_FlowCycleScale, t);
		this.TERRAIN_FlowMipOffset = Mathf.Lerp(holderA.TERRAIN_FlowMipOffset, holderB.TERRAIN_FlowMipOffset, t);
		this.TERRAIN_WetDarkening = Mathf.Lerp(holderA.TERRAIN_WetDarkening, holderB.TERRAIN_WetDarkening, t);
		this.TERRAIN_WetDropletsStrength = Mathf.Lerp(holderA.TERRAIN_WetDropletsStrength, holderB.TERRAIN_WetDropletsStrength, t);
		this.TERRAIN_WetHeight_Treshold = Mathf.Lerp(holderA.TERRAIN_WetHeight_Treshold, holderB.TERRAIN_WetHeight_Treshold, t);
		this.TERRAIN_WetHeight_Transition = Mathf.Lerp(holderA.TERRAIN_WetHeight_Transition, holderB.TERRAIN_WetHeight_Transition, t);
		this.TERRAIN_RainIntensity = Mathf.Lerp(holderA.TERRAIN_RainIntensity, holderB.TERRAIN_RainIntensity, t);
		this.TERRAIN_DropletsSpeed = Mathf.Lerp(holderA.TERRAIN_DropletsSpeed, holderB.TERRAIN_DropletsSpeed, t);
		this.TERRAIN_mipoffset_flowSpeed = Mathf.Lerp(holderA.TERRAIN_mipoffset_flowSpeed, holderB.TERRAIN_mipoffset_flowSpeed, t);
		this.TERRAIN_CausticsAnimSpeed = Mathf.Lerp(holderA.TERRAIN_CausticsAnimSpeed, holderB.TERRAIN_CausticsAnimSpeed, t);
		this.TERRAIN_CausticsColor = Color.Lerp(holderA.TERRAIN_CausticsColor, holderB.TERRAIN_CausticsColor, t);
		this.TERRAIN_CausticsWaterLevel = Mathf.Lerp(holderA.TERRAIN_CausticsWaterLevel, holderB.TERRAIN_CausticsWaterLevel, t);
		this.TERRAIN_CausticsWaterLevelByAngle = Mathf.Lerp(holderA.TERRAIN_CausticsWaterLevelByAngle, holderB.TERRAIN_CausticsWaterLevelByAngle, t);
		this.TERRAIN_CausticsWaterDeepFadeLength = Mathf.Lerp(holderA.TERRAIN_CausticsWaterDeepFadeLength, holderB.TERRAIN_CausticsWaterDeepFadeLength, t);
		this.TERRAIN_CausticsWaterShallowFadeLength = Mathf.Lerp(holderA.TERRAIN_CausticsWaterShallowFadeLength, holderB.TERRAIN_CausticsWaterShallowFadeLength, t);
		this.TERRAIN_CausticsTilingScale = Mathf.Lerp(holderA.TERRAIN_CausticsTilingScale, holderB.TERRAIN_CausticsTilingScale, t);
		this.rtp_customAmbientCorrection = Color.Lerp(holderA.rtp_customAmbientCorrection, holderB.rtp_customAmbientCorrection, t);
		this.TERRAIN_IBL_DiffAO_Damp = Mathf.Lerp(holderA.TERRAIN_IBL_DiffAO_Damp, holderB.TERRAIN_IBL_DiffAO_Damp, t);
		this.TERRAIN_IBLRefl_SpecAO_Damp = Mathf.Lerp(holderA.TERRAIN_IBLRefl_SpecAO_Damp, holderB.TERRAIN_IBLRefl_SpecAO_Damp, t);
		this.RTP_AOsharpness = Mathf.Lerp(holderA.RTP_AOsharpness, holderB.RTP_AOsharpness, t);
		this.RTP_AOamp = Mathf.Lerp(holderA.RTP_AOamp, holderB.RTP_AOamp, t);
		this.RTP_LightDefVector = Vector4.Lerp(holderA.RTP_LightDefVector, holderB.RTP_LightDefVector, t);
		this.RTP_ReflexLightDiffuseColor = Color.Lerp(holderA.RTP_ReflexLightDiffuseColor, holderB.RTP_ReflexLightDiffuseColor, t);
		this.RTP_ReflexLightDiffuseColor2 = Color.Lerp(holderA.RTP_ReflexLightDiffuseColor2, holderB.RTP_ReflexLightDiffuseColor2, t);
		this.RTP_ReflexLightSpecColor = Color.Lerp(holderA.RTP_ReflexLightSpecColor, holderB.RTP_ReflexLightSpecColor, t);
		this.EmissionRefractFiltering = Mathf.Lerp(holderA.EmissionRefractFiltering, holderB.EmissionRefractFiltering, t);
		this.EmissionRefractAnimSpeed = Mathf.Lerp(holderA.EmissionRefractAnimSpeed, holderB.EmissionRefractAnimSpeed, t);
		this.VerticalTextureGlobalBumpInfluence = Mathf.Lerp(holderA.VerticalTextureGlobalBumpInfluence, holderB.VerticalTextureGlobalBumpInfluence, t);
		this.VerticalTextureTiling = Mathf.Lerp(holderA.VerticalTextureTiling, holderB.VerticalTextureTiling, t);
		this._snow_strength = Mathf.Lerp(holderA._snow_strength, holderB._snow_strength, t);
		this._global_color_brightness_to_snow = Mathf.Lerp(holderA._global_color_brightness_to_snow, holderB._global_color_brightness_to_snow, t);
		this._snow_slope_factor = Mathf.Lerp(holderA._snow_slope_factor, holderB._snow_slope_factor, t);
		this._snow_edge_definition = Mathf.Lerp(holderA._snow_edge_definition, holderB._snow_edge_definition, t);
		this._snow_height_treshold = Mathf.Lerp(holderA._snow_height_treshold, holderB._snow_height_treshold, t);
		this._snow_height_transition = Mathf.Lerp(holderA._snow_height_transition, holderB._snow_height_transition, t);
		this._snow_color = Color.Lerp(holderA._snow_color, holderB._snow_color, t);
		this._snow_specular = Mathf.Lerp(holderA._snow_specular, holderB._snow_specular, t);
		this._snow_gloss = Mathf.Lerp(holderA._snow_gloss, holderB._snow_gloss, t);
		this._snow_reflectivness = Mathf.Lerp(holderA._snow_reflectivness, holderB._snow_reflectivness, t);
		this._snow_deep_factor = Mathf.Lerp(holderA._snow_deep_factor, holderB._snow_deep_factor, t);
		this._snow_fresnel = Mathf.Lerp(holderA._snow_fresnel, holderB._snow_fresnel, t);
		this._snow_diff_fresnel = Mathf.Lerp(holderA._snow_diff_fresnel, holderB._snow_diff_fresnel, t);
		this._snow_IBL_DiffuseStrength = Mathf.Lerp(holderA._snow_IBL_DiffuseStrength, holderB._snow_IBL_DiffuseStrength, t);
		this._snow_IBL_SpecStrength = Mathf.Lerp(holderA._snow_IBL_SpecStrength, holderB._snow_IBL_SpecStrength, t);
		for (int i = 0; i < holderA.Spec.Length; i++)
		{
			if (i < this.Spec.Length)
			{
				this.Spec[i] = Mathf.Lerp(holderA.Spec[i], holderB.Spec[i], t);
				this.FarSpecCorrection[i] = Mathf.Lerp(holderA.FarSpecCorrection[i], holderB.FarSpecCorrection[i], t);
				this.MixScale[i] = Mathf.Lerp(holderA.MixScale[i], holderB.MixScale[i], t);
				this.MixBlend[i] = Mathf.Lerp(holderA.MixBlend[i], holderB.MixBlend[i], t);
				this.MixSaturation[i] = Mathf.Lerp(holderA.MixSaturation[i], holderB.MixSaturation[i], t);
				this.RTP_gloss2mask[i] = Mathf.Lerp(holderA.RTP_gloss2mask[i], holderB.RTP_gloss2mask[i], t);
				this.RTP_gloss_mult[i] = Mathf.Lerp(holderA.RTP_gloss_mult[i], holderB.RTP_gloss_mult[i], t);
				this.RTP_gloss_shaping[i] = Mathf.Lerp(holderA.RTP_gloss_shaping[i], holderB.RTP_gloss_shaping[i], t);
				this.RTP_Fresnel[i] = Mathf.Lerp(holderA.RTP_Fresnel[i], holderB.RTP_Fresnel[i], t);
				this.RTP_FresnelAtten[i] = Mathf.Lerp(holderA.RTP_FresnelAtten[i], holderB.RTP_FresnelAtten[i], t);
				this.RTP_DiffFresnel[i] = Mathf.Lerp(holderA.RTP_DiffFresnel[i], holderB.RTP_DiffFresnel[i], t);
				this.RTP_IBL_bump_smoothness[i] = Mathf.Lerp(holderA.RTP_IBL_bump_smoothness[i], holderB.RTP_IBL_bump_smoothness[i], t);
				this.RTP_IBL_DiffuseStrength[i] = Mathf.Lerp(holderA.RTP_IBL_DiffuseStrength[i], holderB.RTP_IBL_DiffuseStrength[i], t);
				this.RTP_IBL_SpecStrength[i] = Mathf.Lerp(holderA.RTP_IBL_SpecStrength[i], holderB.RTP_IBL_SpecStrength[i], t);
				this._DeferredSpecDampAddPass[i] = Mathf.Lerp(holderA._DeferredSpecDampAddPass[i], holderB._DeferredSpecDampAddPass[i], t);
				this.MixBrightness[i] = Mathf.Lerp(holderA.MixBrightness[i], holderB.MixBrightness[i], t);
				this.MixReplace[i] = Mathf.Lerp(holderA.MixReplace[i], holderB.MixReplace[i], t);
				this.LayerBrightness[i] = Mathf.Lerp(holderA.LayerBrightness[i], holderB.LayerBrightness[i], t);
				this.LayerBrightness2Spec[i] = Mathf.Lerp(holderA.LayerBrightness2Spec[i], holderB.LayerBrightness2Spec[i], t);
				this.LayerAlbedo2SpecColor[i] = Mathf.Lerp(holderA.LayerAlbedo2SpecColor[i], holderB.LayerAlbedo2SpecColor[i], t);
				this.LayerSaturation[i] = Mathf.Lerp(holderA.LayerSaturation[i], holderB.LayerSaturation[i], t);
				this.LayerEmission[i] = Mathf.Lerp(holderA.LayerEmission[i], holderB.LayerEmission[i], t);
				this.LayerEmissionColor[i] = Color.Lerp(holderA.LayerEmissionColor[i], holderB.LayerEmissionColor[i], t);
				this.LayerEmissionRefractStrength[i] = Mathf.Lerp(holderA.LayerEmissionRefractStrength[i], holderB.LayerEmissionRefractStrength[i], t);
				this.LayerEmissionRefractHBedge[i] = Mathf.Lerp(holderA.LayerEmissionRefractHBedge[i], holderB.LayerEmissionRefractHBedge[i], t);
				this.GlobalColorPerLayer[i] = Mathf.Lerp(holderA.GlobalColorPerLayer[i], holderB.GlobalColorPerLayer[i], t);
				this.GlobalColorBottom[i] = Mathf.Lerp(holderA.GlobalColorBottom[i], holderB.GlobalColorBottom[i], t);
				this.GlobalColorTop[i] = Mathf.Lerp(holderA.GlobalColorTop[i], holderB.GlobalColorTop[i], t);
				this.GlobalColorColormapLoSat[i] = Mathf.Lerp(holderA.GlobalColorColormapLoSat[i], holderB.GlobalColorColormapLoSat[i], t);
				this.GlobalColorColormapHiSat[i] = Mathf.Lerp(holderA.GlobalColorColormapHiSat[i], holderB.GlobalColorColormapHiSat[i], t);
				this.GlobalColorLayerLoSat[i] = Mathf.Lerp(holderA.GlobalColorLayerLoSat[i], holderB.GlobalColorLayerLoSat[i], t);
				this.GlobalColorLayerHiSat[i] = Mathf.Lerp(holderA.GlobalColorLayerHiSat[i], holderB.GlobalColorLayerHiSat[i], t);
				this.GlobalColorLoBlend[i] = Mathf.Lerp(holderA.GlobalColorLoBlend[i], holderB.GlobalColorLoBlend[i], t);
				this.GlobalColorHiBlend[i] = Mathf.Lerp(holderA.GlobalColorHiBlend[i], holderB.GlobalColorHiBlend[i], t);
				this.PER_LAYER_HEIGHT_MODIFIER[i] = Mathf.Lerp(holderA.PER_LAYER_HEIGHT_MODIFIER[i], holderB.PER_LAYER_HEIGHT_MODIFIER[i], t);
				this._SuperDetailStrengthMultA[i] = Mathf.Lerp(holderA._SuperDetailStrengthMultA[i], holderB._SuperDetailStrengthMultA[i], t);
				this._SuperDetailStrengthMultASelfMaskNear[i] = Mathf.Lerp(holderA._SuperDetailStrengthMultASelfMaskNear[i], holderB._SuperDetailStrengthMultASelfMaskNear[i], t);
				this._SuperDetailStrengthMultASelfMaskFar[i] = Mathf.Lerp(holderA._SuperDetailStrengthMultASelfMaskFar[i], holderB._SuperDetailStrengthMultASelfMaskFar[i], t);
				this._SuperDetailStrengthMultB[i] = Mathf.Lerp(holderA._SuperDetailStrengthMultB[i], holderB._SuperDetailStrengthMultB[i], t);
				this._SuperDetailStrengthMultBSelfMaskNear[i] = Mathf.Lerp(holderA._SuperDetailStrengthMultBSelfMaskNear[i], holderB._SuperDetailStrengthMultBSelfMaskNear[i], t);
				this._SuperDetailStrengthMultBSelfMaskFar[i] = Mathf.Lerp(holderA._SuperDetailStrengthMultBSelfMaskFar[i], holderB._SuperDetailStrengthMultBSelfMaskFar[i], t);
				this._SuperDetailStrengthNormal[i] = Mathf.Lerp(holderA._SuperDetailStrengthNormal[i], holderB._SuperDetailStrengthNormal[i], t);
				this._BumpMapGlobalStrength[i] = Mathf.Lerp(holderA._BumpMapGlobalStrength[i], holderB._BumpMapGlobalStrength[i], t);
				this.AO_strength[i] = Mathf.Lerp(holderA.AO_strength[i], holderB.AO_strength[i], t);
				this.VerticalTextureStrength[i] = Mathf.Lerp(holderA.VerticalTextureStrength[i], holderB.VerticalTextureStrength[i], t);
				this._snow_strength_per_layer[i] = Mathf.Lerp(holderA._snow_strength_per_layer[i], holderB._snow_strength_per_layer[i], t);
				this.TERRAIN_LayerWetStrength[i] = Mathf.Lerp(holderA.TERRAIN_LayerWetStrength[i], holderB.TERRAIN_LayerWetStrength[i], t);
				this.TERRAIN_WaterLevel[i] = Mathf.Lerp(holderA.TERRAIN_WaterLevel[i], holderB.TERRAIN_WaterLevel[i], t);
				this.TERRAIN_WaterLevelSlopeDamp[i] = Mathf.Lerp(holderA.TERRAIN_WaterLevelSlopeDamp[i], holderB.TERRAIN_WaterLevelSlopeDamp[i], t);
				this.TERRAIN_WaterEdge[i] = Mathf.Lerp(holderA.TERRAIN_WaterEdge[i], holderB.TERRAIN_WaterEdge[i], t);
				this.TERRAIN_WaterSpecularity[i] = Mathf.Lerp(holderA.TERRAIN_WaterSpecularity[i], holderB.TERRAIN_WaterSpecularity[i], t);
				this.TERRAIN_WaterGloss[i] = Mathf.Lerp(holderA.TERRAIN_WaterGloss[i], holderB.TERRAIN_WaterGloss[i], t);
				this.TERRAIN_WaterGlossDamper[i] = Mathf.Lerp(holderA.TERRAIN_WaterGlossDamper[i], holderB.TERRAIN_WaterGlossDamper[i], t);
				this.TERRAIN_WaterOpacity[i] = Mathf.Lerp(holderA.TERRAIN_WaterOpacity[i], holderB.TERRAIN_WaterOpacity[i], t);
				this.TERRAIN_Refraction[i] = Mathf.Lerp(holderA.TERRAIN_Refraction[i], holderB.TERRAIN_Refraction[i], t);
				this.TERRAIN_WetRefraction[i] = Mathf.Lerp(holderA.TERRAIN_WetRefraction[i], holderB.TERRAIN_WetRefraction[i], t);
				this.TERRAIN_Flow[i] = Mathf.Lerp(holderA.TERRAIN_Flow[i], holderB.TERRAIN_Flow[i], t);
				this.TERRAIN_WetFlow[i] = Mathf.Lerp(holderA.TERRAIN_WetFlow[i], holderB.TERRAIN_WetFlow[i], t);
				this.TERRAIN_WetSpecularity[i] = Mathf.Lerp(holderA.TERRAIN_WetSpecularity[i], holderB.TERRAIN_WetSpecularity[i], t);
				this.TERRAIN_WetGloss[i] = Mathf.Lerp(holderA.TERRAIN_WetGloss[i], holderB.TERRAIN_WetGloss[i], t);
				this.TERRAIN_WaterColor[i] = Color.Lerp(holderA.TERRAIN_WaterColor[i], holderB.TERRAIN_WaterColor[i], t);
				this.TERRAIN_WaterIBL_SpecWetStrength[i] = Mathf.Lerp(holderA.TERRAIN_WaterIBL_SpecWetStrength[i], holderB.TERRAIN_WaterIBL_SpecWetStrength[i], t);
				this.TERRAIN_WaterIBL_SpecWaterStrength[i] = Mathf.Lerp(holderA.TERRAIN_WaterIBL_SpecWaterStrength[i], holderB.TERRAIN_WaterIBL_SpecWaterStrength[i], t);
				this.TERRAIN_WaterEmission[i] = Mathf.Lerp(holderA.TERRAIN_WaterEmission[i], holderB.TERRAIN_WaterEmission[i], t);
			}
		}
	}

	// Token: 0x0600222E RID: 8750 RVA: 0x0010B2EC File Offset: 0x001094EC
	public void ReturnToDefaults(string what = "", int layerIdx = -1)
	{
		if (what == string.Empty || what == "main")
		{
			this.ReliefTransform = new Vector4(3f, 3f, 0f, 0f);
			this.distance_start = 5f;
			this.distance_transition = 20f;
			this._SpecColor = new Color(0.78431374f, 0.78431374f, 0.78431374f, 1f);
			this.RTP_DeferredAddPassSpec = 0.5f;
			this.rtp_customAmbientCorrection = new Color(0.2f, 0.2f, 0.2f, 1f);
			this.TERRAIN_IBL_DiffAO_Damp = 0.25f;
			this.TERRAIN_IBLRefl_SpecAO_Damp = 0.5f;
			this.RTP_LightDefVector = new Vector4(0.05f, 0.5f, 0.5f, 25f);
			this.RTP_ReflexLightDiffuseColor = new Color(0.7921569f, 0.9411765f, 1f, 0.2f);
			this.RTP_ReflexLightDiffuseColor2 = new Color(0.7921569f, 0.9411765f, 1f, 0.2f);
			this.RTP_ReflexLightSpecColor = new Color(0.9411765f, 0.9607843f, 1f, 0.15f);
			this.ReliefBorderBlend = 6f;
			this.LightmapShading = 0f;
			this.RTP_MIP_BIAS = 0f;
			this.RTP_AOsharpness = 1.5f;
			this.RTP_AOamp = 0.1f;
			this.MasterLayerBrightness = 1f;
			this.MasterLayerSaturation = 1f;
			this.EmissionRefractFiltering = 4f;
			this.EmissionRefractAnimSpeed = 4f;
		}
		if (what == string.Empty || what == "perlin")
		{
			this.BumpMapGlobalScale = 0.1f;
			this._FarNormalDamp = 0.2f;
			this.distance_start_bumpglobal = 30f;
			this.distance_transition_bumpglobal = 30f;
			this.rtp_perlin_start_val = 0f;
		}
		if (what == string.Empty || what == "global_color")
		{
			this.GlobalColorMapBlendValues = new Vector3(0.2f, 0.4f, 0.5f);
			this.GlobalColorMapSaturation = 1f;
			this.GlobalColorMapSaturationFar = 1f;
			this.GlobalColorMapDistortByPerlin = 0.005f;
			this.GlobalColorMapBrightness = 1f;
			this.GlobalColorMapBrightnessFar = 1f;
			this._GlobalColorMapNearMIP = 0f;
			this.trees_shadow_distance_start = 50f;
			this.trees_shadow_distance_transition = 10f;
			this.trees_shadow_value = 0.5f;
			this.trees_pixel_distance_start = 500f;
			this.trees_pixel_distance_transition = 10f;
			this.trees_pixel_blend_val = 2f;
			this.global_normalMap_multiplier = 1f;
			this.global_normalMap_farUsage = 0f;
			this._Phong = 0f;
			this.tessHeight = 300f;
			this._TessSubdivisions = 1f;
			this._TessSubdivisionsFar = 1f;
			this._TessYOffset = 0f;
			this._AmbientEmissiveMultiplier = 1f;
			this._AmbientEmissiveRelief = 0.5f;
		}
		if (what == string.Empty || what == "uvblend")
		{
			this.blendMultiplier = 1f;
		}
		if (what == string.Empty || what == "pom/pm")
		{
			this.ExtrudeHeight = 0.05f;
			this.DIST_STEPS = 20f;
			this.WAVELENGTH = 2f;
			this.SHADOW_STEPS = 20f;
			this.WAVELENGTH_SHADOWS = 2f;
			this.SelfShadowStrength = 0.8f;
			this.ShadowSmoothing = 1f;
			this.ShadowSoftnessFade = 0.8f;
		}
		if (what == string.Empty || what == "snow")
		{
			this._global_color_brightness_to_snow = 0.5f;
			this._snow_strength = 0f;
			this._snow_slope_factor = 2f;
			this._snow_edge_definition = 5f;
			this._snow_height_treshold = -200f;
			this._snow_height_transition = 1f;
			this._snow_color = Color.white;
			this._snow_specular = 0.5f;
			this._snow_gloss = 0.7f;
			this._snow_reflectivness = 0.7f;
			this._snow_deep_factor = 1.5f;
			this._snow_fresnel = 0.5f;
			this._snow_diff_fresnel = 0.5f;
			this._snow_IBL_DiffuseStrength = 0.5f;
			this._snow_IBL_SpecStrength = 0.5f;
		}
		if (what == string.Empty || what == "superdetail")
		{
			this._SuperDetailTiling = 8f;
		}
		if (what == string.Empty || what == "vertical")
		{
			this.VerticalTextureGlobalBumpInfluence = 0f;
			this.VerticalTextureTiling = 50f;
		}
		if (what == string.Empty || what == "reflection")
		{
			this.TERRAIN_ReflectionRotSpeed = 0.3f;
			this.TERRAIN_ReflGlossAttenuation = 0.5f;
			this.TERRAIN_ReflColorA = new Color(1f, 1f, 1f, 1f);
			this.TERRAIN_ReflColorB = new Color(0.39215687f, 0.47058824f, 0.50980395f, 1f);
			this.TERRAIN_ReflColorC = new Color(0.15686275f, 0.1882353f, 0.23529412f, 1f);
			this.TERRAIN_ReflColorCenter = 0.5f;
		}
		if (what == string.Empty || what == "water")
		{
			this.TERRAIN_GlobalWetness = 1f;
			this.TERRAIN_RippleScale = 4f;
			this.TERRAIN_FlowScale = 1f;
			this.TERRAIN_FlowSpeed = 0.5f;
			this.TERRAIN_FlowCycleScale = 1f;
			this.TERRAIN_RainIntensity = 1f;
			this.TERRAIN_DropletsSpeed = 10f;
			this.TERRAIN_mipoffset_flowSpeed = 1f;
			this.TERRAIN_FlowMipOffset = 0f;
			this.TERRAIN_WetDarkening = 0.5f;
			this.TERRAIN_WetDropletsStrength = 0f;
			this.TERRAIN_WetHeight_Treshold = -200f;
			this.TERRAIN_WetHeight_Transition = 5f;
		}
		if (what == string.Empty || what == "caustics")
		{
			this.TERRAIN_CausticsAnimSpeed = 2f;
			this.TERRAIN_CausticsColor = Color.white;
			this.TERRAIN_CausticsWaterLevel = 30f;
			this.TERRAIN_CausticsWaterLevelByAngle = 2f;
			this.TERRAIN_CausticsWaterDeepFadeLength = 50f;
			this.TERRAIN_CausticsWaterShallowFadeLength = 30f;
			this.TERRAIN_CausticsTilingScale = 1f;
		}
		if (what == string.Empty || what == "layer")
		{
			int num = 0;
			int num2 = (this.numLayers >= 12) ? 12 : this.numLayers;
			if (layerIdx >= 0)
			{
				num = layerIdx;
				num2 = layerIdx + 1;
			}
			for (int i = num; i < num2; i++)
			{
				this.Spec[i] = 1f;
				this.FarSpecCorrection[i] = 0f;
				this.MIPmult[i] = 0f;
				this.MixScale[i] = 0.2f;
				this.MixBlend[i] = 0.5f;
				this.MixSaturation[i] = 0.3f;
				this.RTP_gloss2mask[i] = 0.5f;
				this.RTP_gloss_mult[i] = 1f;
				this.RTP_gloss_shaping[i] = 0.5f;
				this.RTP_Fresnel[i] = 0f;
				this.RTP_FresnelAtten[i] = 0f;
				this.RTP_DiffFresnel[i] = 0f;
				this.RTP_IBL_bump_smoothness[i] = 0.7f;
				this.RTP_IBL_DiffuseStrength[i] = 0.5f;
				this.RTP_IBL_SpecStrength[i] = 0.5f;
				this._DeferredSpecDampAddPass[i] = 1f;
				this.MixBrightness[i] = 2f;
				this.MixReplace[i] = 0f;
				this.LayerBrightness[i] = 1f;
				this.LayerBrightness2Spec[i] = 0f;
				this.LayerAlbedo2SpecColor[i] = 0f;
				this.LayerSaturation[i] = 1f;
				this.LayerEmission[i] = 0f;
				this.LayerEmissionColor[i] = Color.black;
				this.LayerEmissionRefractStrength[i] = 0f;
				this.LayerEmissionRefractHBedge[i] = 0f;
				this.GlobalColorPerLayer[i] = 1f;
				this.GlobalColorBottom[i] = 0f;
				this.GlobalColorTop[i] = 1f;
				this.GlobalColorColormapLoSat[i] = 1f;
				this.GlobalColorColormapHiSat[i] = 1f;
				this.GlobalColorLayerLoSat[i] = 1f;
				this.GlobalColorLayerHiSat[i] = 1f;
				this.GlobalColorLoBlend[i] = 1f;
				this.GlobalColorHiBlend[i] = 1f;
				this.PER_LAYER_HEIGHT_MODIFIER[i] = 0f;
				this._SuperDetailStrengthMultA[i] = 0f;
				this._SuperDetailStrengthMultASelfMaskNear[i] = 0f;
				this._SuperDetailStrengthMultASelfMaskFar[i] = 0f;
				this._SuperDetailStrengthMultB[i] = 0f;
				this._SuperDetailStrengthMultBSelfMaskNear[i] = 0f;
				this._SuperDetailStrengthMultBSelfMaskFar[i] = 0f;
				this._SuperDetailStrengthNormal[i] = 0f;
				this._BumpMapGlobalStrength[i] = 0.3f;
				this._snow_strength_per_layer[i] = 1f;
				this.VerticalTextureStrength[i] = 0.5f;
				this.AO_strength[i] = 1f;
				this.TERRAIN_LayerWetStrength[i] = 1f;
				this.TERRAIN_WaterLevel[i] = 0.5f;
				this.TERRAIN_WaterLevelSlopeDamp[i] = 0.5f;
				this.TERRAIN_WaterEdge[i] = 2f;
				this.TERRAIN_WaterSpecularity[i] = 0.5f;
				this.TERRAIN_WaterGloss[i] = 0.1f;
				this.TERRAIN_WaterGlossDamper[i] = 0f;
				this.TERRAIN_WaterOpacity[i] = 0.3f;
				this.TERRAIN_Refraction[i] = 0.01f;
				this.TERRAIN_WetRefraction[i] = 0.2f;
				this.TERRAIN_Flow[i] = 0.3f;
				this.TERRAIN_WetFlow[i] = 0.05f;
				this.TERRAIN_WetSpecularity[i] = 0.2f;
				this.TERRAIN_WetGloss[i] = 0.05f;
				this.TERRAIN_WaterColor[i] = new Color(0.9f, 0.9f, 1f, 0.5f);
				this.TERRAIN_WaterIBL_SpecWetStrength[i] = 0.5f;
				this.TERRAIN_WaterIBL_SpecWaterStrength[i] = 0.5f;
				this.TERRAIN_WaterEmission[i] = 0f;
			}
		}
	}

	// Token: 0x0600222F RID: 8751 RVA: 0x0010BD58 File Offset: 0x00109F58
	public bool CheckAndUpdate(ref float[] aLayerPropArray, float defVal, int len)
	{
		if (aLayerPropArray == null || aLayerPropArray.Length < len)
		{
			aLayerPropArray = new float[len];
			for (int i = 0; i < len; i++)
			{
				aLayerPropArray[i] = defVal;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x0010BD98 File Offset: 0x00109F98
	public bool CheckAndUpdate(ref Color[] aLayerPropArray, Color defVal, int len)
	{
		if (aLayerPropArray == null || aLayerPropArray.Length < len)
		{
			aLayerPropArray = new Color[len];
			for (int i = 0; i < len; i++)
			{
				aLayerPropArray[i] = defVal;
			}
			return true;
		}
		return false;
	}

	// Token: 0x04002675 RID: 9845
	public bool useTerrainMaterial = true;

	// Token: 0x04002676 RID: 9846
	public int numTiles;

	// Token: 0x04002677 RID: 9847
	public int numLayers;

	// Token: 0x04002678 RID: 9848
	[NonSerialized]
	public bool dont_check_weak_references;

	// Token: 0x04002679 RID: 9849
	[NonSerialized]
	public bool dont_check_for_interfering_terrain_replacement_shaders;

	// Token: 0x0400267A RID: 9850
	[NonSerialized]
	public Texture2D[] splats_glossBaked = new Texture2D[12];

	// Token: 0x0400267B RID: 9851
	[NonSerialized]
	public Texture2D[] atlas_glossBaked = new Texture2D[3];

	// Token: 0x0400267C RID: 9852
	public RTPGlossBaked[] gloss_baked = new RTPGlossBaked[12];

	// Token: 0x0400267D RID: 9853
	public Texture2D[] splats;

	// Token: 0x0400267E RID: 9854
	public Texture2D[] splat_atlases = new Texture2D[3];

	// Token: 0x0400267F RID: 9855
	public string save_path_atlasA = string.Empty;

	// Token: 0x04002680 RID: 9856
	public string save_path_atlasB = string.Empty;

	// Token: 0x04002681 RID: 9857
	public string save_path_atlasC = string.Empty;

	// Token: 0x04002682 RID: 9858
	public string save_path_terrain_steepness = string.Empty;

	// Token: 0x04002683 RID: 9859
	public string save_path_terrain_height = string.Empty;

	// Token: 0x04002684 RID: 9860
	public string save_path_terrain_direction = string.Empty;

	// Token: 0x04002685 RID: 9861
	public string save_path_Bump01 = string.Empty;

	// Token: 0x04002686 RID: 9862
	public string save_path_Bump23 = string.Empty;

	// Token: 0x04002687 RID: 9863
	public string save_path_Bump45 = string.Empty;

	// Token: 0x04002688 RID: 9864
	public string save_path_Bump67 = string.Empty;

	// Token: 0x04002689 RID: 9865
	public string save_path_Bump89 = string.Empty;

	// Token: 0x0400268A RID: 9866
	public string save_path_BumpAB = string.Empty;

	// Token: 0x0400268B RID: 9867
	public string save_path_HeightMap = string.Empty;

	// Token: 0x0400268C RID: 9868
	public string save_path_HeightMap2 = string.Empty;

	// Token: 0x0400268D RID: 9869
	public string save_path_HeightMap3 = string.Empty;

	// Token: 0x0400268E RID: 9870
	public string save_path_SSColorCombinedA = string.Empty;

	// Token: 0x0400268F RID: 9871
	public string save_path_SSColorCombinedB = string.Empty;

	// Token: 0x04002690 RID: 9872
	public string newPresetName = "a preset name...";

	// Token: 0x04002691 RID: 9873
	public Texture2D activateObject;

	// Token: 0x04002692 RID: 9874
	private GameObject _RTP_LODmanager;

	// Token: 0x04002693 RID: 9875
	public RTP_LODmanager _RTP_LODmanagerScript;

	// Token: 0x04002694 RID: 9876
	public bool super_simple_active;

	// Token: 0x04002695 RID: 9877
	public float RTP_MIP_BIAS;

	// Token: 0x04002696 RID: 9878
	public Color _SpecColor;

	// Token: 0x04002697 RID: 9879
	public float RTP_DeferredAddPassSpec = 0.5f;

	// Token: 0x04002698 RID: 9880
	public float MasterLayerBrightness = 1f;

	// Token: 0x04002699 RID: 9881
	public float MasterLayerSaturation = 1f;

	// Token: 0x0400269A RID: 9882
	public float EmissionRefractFiltering = 4f;

	// Token: 0x0400269B RID: 9883
	public float EmissionRefractAnimSpeed = 4f;

	// Token: 0x0400269C RID: 9884
	public RTPColorChannels SuperDetailA_channel;

	// Token: 0x0400269D RID: 9885
	public RTPColorChannels SuperDetailB_channel;

	// Token: 0x0400269E RID: 9886
	public Texture2D Bump01;

	// Token: 0x0400269F RID: 9887
	public Texture2D Bump23;

	// Token: 0x040026A0 RID: 9888
	public Texture2D Bump45;

	// Token: 0x040026A1 RID: 9889
	public Texture2D Bump67;

	// Token: 0x040026A2 RID: 9890
	public Texture2D Bump89;

	// Token: 0x040026A3 RID: 9891
	public Texture2D BumpAB;

	// Token: 0x040026A4 RID: 9892
	public Texture2D BumpGlobal;

	// Token: 0x040026A5 RID: 9893
	public int BumpGlobalCombinedSize = 1024;

	// Token: 0x040026A6 RID: 9894
	public Texture2D SSColorCombinedA;

	// Token: 0x040026A7 RID: 9895
	public Texture2D SSColorCombinedB;

	// Token: 0x040026A8 RID: 9896
	public Texture2D VerticalTexture;

	// Token: 0x040026A9 RID: 9897
	public float BumpMapGlobalScale;

	// Token: 0x040026AA RID: 9898
	public Vector3 GlobalColorMapBlendValues;

	// Token: 0x040026AB RID: 9899
	public float _GlobalColorMapNearMIP;

	// Token: 0x040026AC RID: 9900
	public float GlobalColorMapSaturation;

	// Token: 0x040026AD RID: 9901
	public float GlobalColorMapSaturationFar = 1f;

	// Token: 0x040026AE RID: 9902
	public float GlobalColorMapDistortByPerlin = 0.005f;

	// Token: 0x040026AF RID: 9903
	public float GlobalColorMapBrightness;

	// Token: 0x040026B0 RID: 9904
	public float GlobalColorMapBrightnessFar = 1f;

	// Token: 0x040026B1 RID: 9905
	public float _FarNormalDamp;

	// Token: 0x040026B2 RID: 9906
	public float blendMultiplier;

	// Token: 0x040026B3 RID: 9907
	public Vector3 terrainTileSize;

	// Token: 0x040026B4 RID: 9908
	public Texture2D HeightMap;

	// Token: 0x040026B5 RID: 9909
	public Vector4 ReliefTransform;

	// Token: 0x040026B6 RID: 9910
	public float DIST_STEPS;

	// Token: 0x040026B7 RID: 9911
	public float WAVELENGTH;

	// Token: 0x040026B8 RID: 9912
	public float ReliefBorderBlend;

	// Token: 0x040026B9 RID: 9913
	public float ExtrudeHeight;

	// Token: 0x040026BA RID: 9914
	public float LightmapShading;

	// Token: 0x040026BB RID: 9915
	public float RTP_AOsharpness;

	// Token: 0x040026BC RID: 9916
	public float RTP_AOamp;

	// Token: 0x040026BD RID: 9917
	public bool colorSpaceLinear;

	// Token: 0x040026BE RID: 9918
	public float SHADOW_STEPS;

	// Token: 0x040026BF RID: 9919
	public float WAVELENGTH_SHADOWS;

	// Token: 0x040026C0 RID: 9920
	public float SelfShadowStrength;

	// Token: 0x040026C1 RID: 9921
	public float ShadowSmoothing;

	// Token: 0x040026C2 RID: 9922
	public float ShadowSoftnessFade = 0.8f;

	// Token: 0x040026C3 RID: 9923
	public float distance_start;

	// Token: 0x040026C4 RID: 9924
	public float distance_transition;

	// Token: 0x040026C5 RID: 9925
	public float distance_start_bumpglobal;

	// Token: 0x040026C6 RID: 9926
	public float distance_transition_bumpglobal;

	// Token: 0x040026C7 RID: 9927
	public float rtp_perlin_start_val;

	// Token: 0x040026C8 RID: 9928
	public float _Phong;

	// Token: 0x040026C9 RID: 9929
	public float tessHeight = 300f;

	// Token: 0x040026CA RID: 9930
	public float _TessSubdivisions = 1f;

	// Token: 0x040026CB RID: 9931
	public float _TessSubdivisionsFar = 1f;

	// Token: 0x040026CC RID: 9932
	public float _TessYOffset;

	// Token: 0x040026CD RID: 9933
	public float trees_shadow_distance_start;

	// Token: 0x040026CE RID: 9934
	public float trees_shadow_distance_transition;

	// Token: 0x040026CF RID: 9935
	public float trees_shadow_value;

	// Token: 0x040026D0 RID: 9936
	public float trees_pixel_distance_start;

	// Token: 0x040026D1 RID: 9937
	public float trees_pixel_distance_transition;

	// Token: 0x040026D2 RID: 9938
	public float trees_pixel_blend_val;

	// Token: 0x040026D3 RID: 9939
	public float global_normalMap_multiplier;

	// Token: 0x040026D4 RID: 9940
	public float global_normalMap_farUsage;

	// Token: 0x040026D5 RID: 9941
	public float _AmbientEmissiveMultiplier = 1f;

	// Token: 0x040026D6 RID: 9942
	public float _AmbientEmissiveRelief = 0.5f;

	// Token: 0x040026D7 RID: 9943
	public Texture2D HeightMap2;

	// Token: 0x040026D8 RID: 9944
	public Texture2D HeightMap3;

	// Token: 0x040026D9 RID: 9945
	public int rtp_mipoffset_globalnorm;

	// Token: 0x040026DA RID: 9946
	public float _SuperDetailTiling;

	// Token: 0x040026DB RID: 9947
	public Texture2D SuperDetailA;

	// Token: 0x040026DC RID: 9948
	public Texture2D SuperDetailB;

	// Token: 0x040026DD RID: 9949
	public Texture2D TERRAIN_ReflectionMap;

	// Token: 0x040026DE RID: 9950
	public RTPColorChannels TERRAIN_ReflectionMap_channel;

	// Token: 0x040026DF RID: 9951
	public Color TERRAIN_ReflColorA;

	// Token: 0x040026E0 RID: 9952
	public Color TERRAIN_ReflColorB;

	// Token: 0x040026E1 RID: 9953
	public Color TERRAIN_ReflColorC;

	// Token: 0x040026E2 RID: 9954
	public float TERRAIN_ReflColorCenter = 0.5f;

	// Token: 0x040026E3 RID: 9955
	public float TERRAIN_ReflGlossAttenuation = 0.5f;

	// Token: 0x040026E4 RID: 9956
	public float TERRAIN_ReflectionRotSpeed;

	// Token: 0x040026E5 RID: 9957
	public float TERRAIN_GlobalWetness;

	// Token: 0x040026E6 RID: 9958
	public Texture2D TERRAIN_RippleMap;

	// Token: 0x040026E7 RID: 9959
	public float TERRAIN_RippleScale;

	// Token: 0x040026E8 RID: 9960
	public float TERRAIN_FlowScale;

	// Token: 0x040026E9 RID: 9961
	public float TERRAIN_FlowSpeed;

	// Token: 0x040026EA RID: 9962
	public float TERRAIN_FlowCycleScale;

	// Token: 0x040026EB RID: 9963
	public float TERRAIN_FlowMipOffset;

	// Token: 0x040026EC RID: 9964
	public float TERRAIN_WetDarkening;

	// Token: 0x040026ED RID: 9965
	public float TERRAIN_WetDropletsStrength;

	// Token: 0x040026EE RID: 9966
	public float TERRAIN_WetHeight_Treshold;

	// Token: 0x040026EF RID: 9967
	public float TERRAIN_WetHeight_Transition;

	// Token: 0x040026F0 RID: 9968
	public float TERRAIN_RainIntensity;

	// Token: 0x040026F1 RID: 9969
	public float TERRAIN_DropletsSpeed;

	// Token: 0x040026F2 RID: 9970
	public float TERRAIN_mipoffset_flowSpeed;

	// Token: 0x040026F3 RID: 9971
	public float TERRAIN_CausticsAnimSpeed;

	// Token: 0x040026F4 RID: 9972
	public Color TERRAIN_CausticsColor;

	// Token: 0x040026F5 RID: 9973
	public GameObject TERRAIN_CausticsWaterLevelRefObject;

	// Token: 0x040026F6 RID: 9974
	public float TERRAIN_CausticsWaterLevel;

	// Token: 0x040026F7 RID: 9975
	public float TERRAIN_CausticsWaterLevelByAngle;

	// Token: 0x040026F8 RID: 9976
	public float TERRAIN_CausticsWaterDeepFadeLength;

	// Token: 0x040026F9 RID: 9977
	public float TERRAIN_CausticsWaterShallowFadeLength;

	// Token: 0x040026FA RID: 9978
	public float TERRAIN_CausticsTilingScale;

	// Token: 0x040026FB RID: 9979
	public Texture2D TERRAIN_CausticsTex;

	// Token: 0x040026FC RID: 9980
	public Color rtp_customAmbientCorrection;

	// Token: 0x040026FD RID: 9981
	public Cubemap _CubemapDiff;

	// Token: 0x040026FE RID: 9982
	public float TERRAIN_IBL_DiffAO_Damp = 0.25f;

	// Token: 0x040026FF RID: 9983
	public Cubemap _CubemapSpec;

	// Token: 0x04002700 RID: 9984
	public float TERRAIN_IBLRefl_SpecAO_Damp = 0.5f;

	// Token: 0x04002701 RID: 9985
	public Vector4 RTP_LightDefVector;

	// Token: 0x04002702 RID: 9986
	public Color RTP_ReflexLightDiffuseColor;

	// Token: 0x04002703 RID: 9987
	public Color RTP_ReflexLightDiffuseColor2;

	// Token: 0x04002704 RID: 9988
	public Color RTP_ReflexLightSpecColor;

	// Token: 0x04002705 RID: 9989
	public Texture2D[] Bumps;

	// Token: 0x04002706 RID: 9990
	public float[] Spec;

	// Token: 0x04002707 RID: 9991
	public float[] FarSpecCorrection;

	// Token: 0x04002708 RID: 9992
	public float[] MIPmult;

	// Token: 0x04002709 RID: 9993
	public float[] MixScale;

	// Token: 0x0400270A RID: 9994
	public float[] MixBlend;

	// Token: 0x0400270B RID: 9995
	public float[] MixSaturation;

	// Token: 0x0400270C RID: 9996
	public float[] RTP_gloss2mask;

	// Token: 0x0400270D RID: 9997
	public float[] RTP_gloss_mult;

	// Token: 0x0400270E RID: 9998
	public float[] RTP_gloss_shaping;

	// Token: 0x0400270F RID: 9999
	public float[] RTP_Fresnel;

	// Token: 0x04002710 RID: 10000
	public float[] RTP_FresnelAtten;

	// Token: 0x04002711 RID: 10001
	public float[] RTP_DiffFresnel;

	// Token: 0x04002712 RID: 10002
	public float[] RTP_IBL_bump_smoothness;

	// Token: 0x04002713 RID: 10003
	public float[] RTP_IBL_DiffuseStrength;

	// Token: 0x04002714 RID: 10004
	public float[] RTP_IBL_SpecStrength;

	// Token: 0x04002715 RID: 10005
	public float[] _DeferredSpecDampAddPass;

	// Token: 0x04002716 RID: 10006
	public float[] GlobalColorBottom;

	// Token: 0x04002717 RID: 10007
	public float[] GlobalColorTop;

	// Token: 0x04002718 RID: 10008
	public float[] GlobalColorColormapLoSat;

	// Token: 0x04002719 RID: 10009
	public float[] GlobalColorColormapHiSat;

	// Token: 0x0400271A RID: 10010
	public float[] GlobalColorLayerLoSat;

	// Token: 0x0400271B RID: 10011
	public float[] GlobalColorLayerHiSat;

	// Token: 0x0400271C RID: 10012
	public float[] GlobalColorLoBlend;

	// Token: 0x0400271D RID: 10013
	public float[] GlobalColorHiBlend;

	// Token: 0x0400271E RID: 10014
	public float[] MixBrightness;

	// Token: 0x0400271F RID: 10015
	public float[] MixReplace;

	// Token: 0x04002720 RID: 10016
	public float[] LayerBrightness;

	// Token: 0x04002721 RID: 10017
	public float[] LayerBrightness2Spec;

	// Token: 0x04002722 RID: 10018
	public float[] LayerAlbedo2SpecColor;

	// Token: 0x04002723 RID: 10019
	public float[] LayerSaturation;

	// Token: 0x04002724 RID: 10020
	public float[] LayerEmission;

	// Token: 0x04002725 RID: 10021
	public Color[] LayerEmissionColor;

	// Token: 0x04002726 RID: 10022
	public float[] LayerEmissionRefractStrength;

	// Token: 0x04002727 RID: 10023
	public float[] LayerEmissionRefractHBedge;

	// Token: 0x04002728 RID: 10024
	public float[] GlobalColorPerLayer;

	// Token: 0x04002729 RID: 10025
	public float[] PER_LAYER_HEIGHT_MODIFIER;

	// Token: 0x0400272A RID: 10026
	public float[] _SuperDetailStrengthMultA;

	// Token: 0x0400272B RID: 10027
	public float[] _SuperDetailStrengthMultASelfMaskNear;

	// Token: 0x0400272C RID: 10028
	public float[] _SuperDetailStrengthMultASelfMaskFar;

	// Token: 0x0400272D RID: 10029
	public float[] _SuperDetailStrengthMultB;

	// Token: 0x0400272E RID: 10030
	public float[] _SuperDetailStrengthMultBSelfMaskNear;

	// Token: 0x0400272F RID: 10031
	public float[] _SuperDetailStrengthMultBSelfMaskFar;

	// Token: 0x04002730 RID: 10032
	public float[] _SuperDetailStrengthNormal;

	// Token: 0x04002731 RID: 10033
	public float[] _BumpMapGlobalStrength;

	// Token: 0x04002732 RID: 10034
	public float[] AO_strength = new float[]
	{
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f
	};

	// Token: 0x04002733 RID: 10035
	public float[] VerticalTextureStrength;

	// Token: 0x04002734 RID: 10036
	public float VerticalTextureGlobalBumpInfluence;

	// Token: 0x04002735 RID: 10037
	public float VerticalTextureTiling;

	// Token: 0x04002736 RID: 10038
	public Texture2D[] Heights;

	// Token: 0x04002737 RID: 10039
	public float[] _snow_strength_per_layer;

	// Token: 0x04002738 RID: 10040
	public ProceduralMaterial[] Substances;

	// Token: 0x04002739 RID: 10041
	public float[] TERRAIN_LayerWetStrength;

	// Token: 0x0400273A RID: 10042
	public float[] TERRAIN_WaterLevel;

	// Token: 0x0400273B RID: 10043
	public float[] TERRAIN_WaterLevelSlopeDamp;

	// Token: 0x0400273C RID: 10044
	public float[] TERRAIN_WaterEdge;

	// Token: 0x0400273D RID: 10045
	public float[] TERRAIN_WaterSpecularity;

	// Token: 0x0400273E RID: 10046
	public float[] TERRAIN_WaterGloss;

	// Token: 0x0400273F RID: 10047
	public float[] TERRAIN_WaterGlossDamper;

	// Token: 0x04002740 RID: 10048
	public float[] TERRAIN_WaterOpacity;

	// Token: 0x04002741 RID: 10049
	public float[] TERRAIN_Refraction;

	// Token: 0x04002742 RID: 10050
	public float[] TERRAIN_WetRefraction;

	// Token: 0x04002743 RID: 10051
	public float[] TERRAIN_Flow;

	// Token: 0x04002744 RID: 10052
	public float[] TERRAIN_WetFlow;

	// Token: 0x04002745 RID: 10053
	public float[] TERRAIN_WetSpecularity;

	// Token: 0x04002746 RID: 10054
	public float[] TERRAIN_WetGloss;

	// Token: 0x04002747 RID: 10055
	public Color[] TERRAIN_WaterColor;

	// Token: 0x04002748 RID: 10056
	public float[] TERRAIN_WaterIBL_SpecWetStrength;

	// Token: 0x04002749 RID: 10057
	public float[] TERRAIN_WaterIBL_SpecWaterStrength;

	// Token: 0x0400274A RID: 10058
	public float[] TERRAIN_WaterEmission;

	// Token: 0x0400274B RID: 10059
	public float _snow_strength;

	// Token: 0x0400274C RID: 10060
	public float _global_color_brightness_to_snow;

	// Token: 0x0400274D RID: 10061
	public float _snow_slope_factor;

	// Token: 0x0400274E RID: 10062
	public float _snow_edge_definition;

	// Token: 0x0400274F RID: 10063
	public float _snow_height_treshold;

	// Token: 0x04002750 RID: 10064
	public float _snow_height_transition;

	// Token: 0x04002751 RID: 10065
	public Color _snow_color;

	// Token: 0x04002752 RID: 10066
	public float _snow_specular;

	// Token: 0x04002753 RID: 10067
	public float _snow_gloss;

	// Token: 0x04002754 RID: 10068
	public float _snow_reflectivness;

	// Token: 0x04002755 RID: 10069
	public float _snow_deep_factor;

	// Token: 0x04002756 RID: 10070
	public float _snow_fresnel;

	// Token: 0x04002757 RID: 10071
	public float _snow_diff_fresnel;

	// Token: 0x04002758 RID: 10072
	public float _snow_IBL_DiffuseStrength;

	// Token: 0x04002759 RID: 10073
	public float _snow_IBL_SpecStrength;

	// Token: 0x0400275A RID: 10074
	public bool _4LAYERS_SHADER_USED;

	// Token: 0x0400275B RID: 10075
	public bool flat_dir_ref = true;

	// Token: 0x0400275C RID: 10076
	public bool flip_dir_ref = true;

	// Token: 0x0400275D RID: 10077
	public GameObject direction_object;

	// Token: 0x0400275E RID: 10078
	public bool show_details;

	// Token: 0x0400275F RID: 10079
	public bool show_details_main;

	// Token: 0x04002760 RID: 10080
	public bool show_details_atlasing;

	// Token: 0x04002761 RID: 10081
	public bool show_details_layers;

	// Token: 0x04002762 RID: 10082
	public bool show_details_uv_blend;

	// Token: 0x04002763 RID: 10083
	public bool show_controlmaps;

	// Token: 0x04002764 RID: 10084
	public bool show_controlmaps_build;

	// Token: 0x04002765 RID: 10085
	public bool show_controlmaps_helpers;

	// Token: 0x04002766 RID: 10086
	public bool show_controlmaps_highcost;

	// Token: 0x04002767 RID: 10087
	public bool show_controlmaps_splats;

	// Token: 0x04002768 RID: 10088
	public bool show_vert_texture;

	// Token: 0x04002769 RID: 10089
	public bool show_global_color;

	// Token: 0x0400276A RID: 10090
	public bool show_snow;

	// Token: 0x0400276B RID: 10091
	public bool show_global_bump;

	// Token: 0x0400276C RID: 10092
	public bool show_global_bump_normals;

	// Token: 0x0400276D RID: 10093
	public bool show_global_bump_superdetail;

	// Token: 0x0400276E RID: 10094
	public ReliefTerrainMenuItems submenu;

	// Token: 0x0400276F RID: 10095
	public ReliefTerrainSettingsItems submenu_settings;

	// Token: 0x04002770 RID: 10096
	public ReliefTerrainDerivedTexturesItems submenu_derived_textures;

	// Token: 0x04002771 RID: 10097
	public ReliefTerrainControlTexturesItems submenu_control_textures;

	// Token: 0x04002772 RID: 10098
	public bool show_global_wet_settings;

	// Token: 0x04002773 RID: 10099
	public bool show_global_reflection_settings;

	// Token: 0x04002774 RID: 10100
	public int show_active_layer;

	// Token: 0x04002775 RID: 10101
	public bool show_derivedmaps;

	// Token: 0x04002776 RID: 10102
	public bool show_settings;

	// Token: 0x04002777 RID: 10103
	public bool undo_flag;

	// Token: 0x04002778 RID: 10104
	public bool paint_flag;

	// Token: 0x04002779 RID: 10105
	public float paint_size = 0.5f;

	// Token: 0x0400277A RID: 10106
	public float paint_smoothness;

	// Token: 0x0400277B RID: 10107
	public float paint_opacity = 1f;

	// Token: 0x0400277C RID: 10108
	public Color paintColor = new Color(0.5f, 0.3f, 0f, 0f);

	// Token: 0x0400277D RID: 10109
	public bool preserveBrightness = true;

	// Token: 0x0400277E RID: 10110
	public bool paint_alpha_flag;

	// Token: 0x0400277F RID: 10111
	public bool paint_wetmask;

	// Token: 0x04002780 RID: 10112
	public RaycastHit paintHitInfo;

	// Token: 0x04002781 RID: 10113
	public bool paintHitInfo_flag;

	// Token: 0x04002782 RID: 10114
	public bool cut_holes;

	// Token: 0x04002783 RID: 10115
	private Texture2D dumb_tex;

	// Token: 0x04002784 RID: 10116
	public Color[] paintColorSwatches;

	// Token: 0x04002785 RID: 10117
	public Material use_mat;
}
