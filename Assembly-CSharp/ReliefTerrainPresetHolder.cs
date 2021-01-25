using System;
using UnityEngine;

// Token: 0x02000543 RID: 1347
[Serializable]
public class ReliefTerrainPresetHolder : ScriptableObject
{
	// Token: 0x06002232 RID: 8754 RVA: 0x00016D21 File Offset: 0x00014F21
	public void Init(string name)
	{
		this.PresetID = string.Empty + Random.value + Time.realtimeSinceStartup;
		this.PresetName = name;
	}

	// Token: 0x04002786 RID: 10118
	public string PresetID;

	// Token: 0x04002787 RID: 10119
	public string PresetName;

	// Token: 0x04002788 RID: 10120
	public string type;

	// Token: 0x04002789 RID: 10121
	public int numLayers;

	// Token: 0x0400278A RID: 10122
	public Texture2D[] splats;

	// Token: 0x0400278B RID: 10123
	public RTPGlossBaked[] gloss_baked = new RTPGlossBaked[12];

	// Token: 0x0400278C RID: 10124
	public Texture2D[] splat_atlases;

	// Token: 0x0400278D RID: 10125
	public Texture2D controlA;

	// Token: 0x0400278E RID: 10126
	public Texture2D controlB;

	// Token: 0x0400278F RID: 10127
	public Texture2D controlC;

	// Token: 0x04002790 RID: 10128
	public float RTP_MIP_BIAS;

	// Token: 0x04002791 RID: 10129
	public Color _SpecColor;

	// Token: 0x04002792 RID: 10130
	public float RTP_DeferredAddPassSpec = 0.5f;

	// Token: 0x04002793 RID: 10131
	public float MasterLayerBrightness = 1f;

	// Token: 0x04002794 RID: 10132
	public float MasterLayerSaturation = 1f;

	// Token: 0x04002795 RID: 10133
	public float EmissionRefractFiltering = 4f;

	// Token: 0x04002796 RID: 10134
	public float EmissionRefractAnimSpeed = 4f;

	// Token: 0x04002797 RID: 10135
	public RTPColorChannels SuperDetailA_channel;

	// Token: 0x04002798 RID: 10136
	public RTPColorChannels SuperDetailB_channel;

	// Token: 0x04002799 RID: 10137
	public Texture2D Bump01;

	// Token: 0x0400279A RID: 10138
	public Texture2D Bump23;

	// Token: 0x0400279B RID: 10139
	public Texture2D Bump45;

	// Token: 0x0400279C RID: 10140
	public Texture2D Bump67;

	// Token: 0x0400279D RID: 10141
	public Texture2D Bump89;

	// Token: 0x0400279E RID: 10142
	public Texture2D BumpAB;

	// Token: 0x0400279F RID: 10143
	public Texture2D ColorGlobal;

	// Token: 0x040027A0 RID: 10144
	public Texture2D NormalGlobal;

	// Token: 0x040027A1 RID: 10145
	public Texture2D TreesGlobal;

	// Token: 0x040027A2 RID: 10146
	public Texture2D AmbientEmissiveMap;

	// Token: 0x040027A3 RID: 10147
	public Texture2D SSColorCombinedA;

	// Token: 0x040027A4 RID: 10148
	public Texture2D SSColorCombinedB;

	// Token: 0x040027A5 RID: 10149
	public bool globalColorModifed_flag;

	// Token: 0x040027A6 RID: 10150
	public bool globalCombinedModifed_flag;

	// Token: 0x040027A7 RID: 10151
	public bool globalWaterModifed_flag;

	// Token: 0x040027A8 RID: 10152
	public Texture2D BumpGlobal;

	// Token: 0x040027A9 RID: 10153
	public Texture2D BumpGlobalCombined;

	// Token: 0x040027AA RID: 10154
	public Texture2D VerticalTexture;

	// Token: 0x040027AB RID: 10155
	public float BumpMapGlobalScale;

	// Token: 0x040027AC RID: 10156
	public Vector3 GlobalColorMapBlendValues;

	// Token: 0x040027AD RID: 10157
	public float GlobalColorMapSaturation;

	// Token: 0x040027AE RID: 10158
	public float GlobalColorMapSaturationFar;

	// Token: 0x040027AF RID: 10159
	public float GlobalColorMapDistortByPerlin = 0.005f;

	// Token: 0x040027B0 RID: 10160
	public float GlobalColorMapBrightness;

	// Token: 0x040027B1 RID: 10161
	public float GlobalColorMapBrightnessFar = 1f;

	// Token: 0x040027B2 RID: 10162
	public float _GlobalColorMapNearMIP;

	// Token: 0x040027B3 RID: 10163
	public float _FarNormalDamp;

	// Token: 0x040027B4 RID: 10164
	public float blendMultiplier;

	// Token: 0x040027B5 RID: 10165
	public Texture2D HeightMap;

	// Token: 0x040027B6 RID: 10166
	public Texture2D HeightMap2;

	// Token: 0x040027B7 RID: 10167
	public Texture2D HeightMap3;

	// Token: 0x040027B8 RID: 10168
	public Vector4 ReliefTransform;

	// Token: 0x040027B9 RID: 10169
	public float DIST_STEPS;

	// Token: 0x040027BA RID: 10170
	public float WAVELENGTH;

	// Token: 0x040027BB RID: 10171
	public float ReliefBorderBlend;

	// Token: 0x040027BC RID: 10172
	public float ExtrudeHeight;

	// Token: 0x040027BD RID: 10173
	public float LightmapShading;

	// Token: 0x040027BE RID: 10174
	public float SHADOW_STEPS;

	// Token: 0x040027BF RID: 10175
	public float WAVELENGTH_SHADOWS;

	// Token: 0x040027C0 RID: 10176
	public float SelfShadowStrength;

	// Token: 0x040027C1 RID: 10177
	public float ShadowSmoothing;

	// Token: 0x040027C2 RID: 10178
	public float ShadowSoftnessFade = 0.7f;

	// Token: 0x040027C3 RID: 10179
	public float distance_start;

	// Token: 0x040027C4 RID: 10180
	public float distance_transition;

	// Token: 0x040027C5 RID: 10181
	public float distance_start_bumpglobal;

	// Token: 0x040027C6 RID: 10182
	public float distance_transition_bumpglobal;

	// Token: 0x040027C7 RID: 10183
	public float rtp_perlin_start_val;

	// Token: 0x040027C8 RID: 10184
	public float _Phong;

	// Token: 0x040027C9 RID: 10185
	public float tessHeight = 300f;

	// Token: 0x040027CA RID: 10186
	public float _TessSubdivisions = 1f;

	// Token: 0x040027CB RID: 10187
	public float _TessSubdivisionsFar = 1f;

	// Token: 0x040027CC RID: 10188
	public float _TessYOffset;

	// Token: 0x040027CD RID: 10189
	public float trees_shadow_distance_start;

	// Token: 0x040027CE RID: 10190
	public float trees_shadow_distance_transition;

	// Token: 0x040027CF RID: 10191
	public float trees_shadow_value;

	// Token: 0x040027D0 RID: 10192
	public float trees_pixel_distance_start;

	// Token: 0x040027D1 RID: 10193
	public float trees_pixel_distance_transition;

	// Token: 0x040027D2 RID: 10194
	public float trees_pixel_blend_val;

	// Token: 0x040027D3 RID: 10195
	public float global_normalMap_multiplier;

	// Token: 0x040027D4 RID: 10196
	public float global_normalMap_farUsage;

	// Token: 0x040027D5 RID: 10197
	public float _AmbientEmissiveMultiplier;

	// Token: 0x040027D6 RID: 10198
	public float _AmbientEmissiveRelief;

	// Token: 0x040027D7 RID: 10199
	public int rtp_mipoffset_globalnorm;

	// Token: 0x040027D8 RID: 10200
	public float _SuperDetailTiling;

	// Token: 0x040027D9 RID: 10201
	public Texture2D SuperDetailA;

	// Token: 0x040027DA RID: 10202
	public Texture2D SuperDetailB;

	// Token: 0x040027DB RID: 10203
	public Texture2D TERRAIN_ReflectionMap;

	// Token: 0x040027DC RID: 10204
	public RTPColorChannels TERRAIN_ReflectionMap_channel;

	// Token: 0x040027DD RID: 10205
	public Color TERRAIN_ReflColorA;

	// Token: 0x040027DE RID: 10206
	public Color TERRAIN_ReflColorB;

	// Token: 0x040027DF RID: 10207
	public Color TERRAIN_ReflColorC;

	// Token: 0x040027E0 RID: 10208
	public float TERRAIN_ReflColorCenter;

	// Token: 0x040027E1 RID: 10209
	public float TERRAIN_ReflGlossAttenuation;

	// Token: 0x040027E2 RID: 10210
	public float TERRAIN_ReflectionRotSpeed;

	// Token: 0x040027E3 RID: 10211
	public float TERRAIN_GlobalWetness;

	// Token: 0x040027E4 RID: 10212
	public Texture2D TERRAIN_RippleMap;

	// Token: 0x040027E5 RID: 10213
	public Texture2D TERRAIN_WetMask;

	// Token: 0x040027E6 RID: 10214
	public float TERRAIN_RippleScale;

	// Token: 0x040027E7 RID: 10215
	public float TERRAIN_FlowScale;

	// Token: 0x040027E8 RID: 10216
	public float TERRAIN_FlowSpeed;

	// Token: 0x040027E9 RID: 10217
	public float TERRAIN_FlowCycleScale;

	// Token: 0x040027EA RID: 10218
	public float TERRAIN_FlowMipOffset;

	// Token: 0x040027EB RID: 10219
	public float TERRAIN_WetDarkening;

	// Token: 0x040027EC RID: 10220
	public float TERRAIN_WetDropletsStrength;

	// Token: 0x040027ED RID: 10221
	public float TERRAIN_WetHeight_Treshold;

	// Token: 0x040027EE RID: 10222
	public float TERRAIN_WetHeight_Transition;

	// Token: 0x040027EF RID: 10223
	public float TERRAIN_RainIntensity;

	// Token: 0x040027F0 RID: 10224
	public float TERRAIN_DropletsSpeed;

	// Token: 0x040027F1 RID: 10225
	public float TERRAIN_mipoffset_flowSpeed;

	// Token: 0x040027F2 RID: 10226
	public float TERRAIN_CausticsAnimSpeed;

	// Token: 0x040027F3 RID: 10227
	public Color TERRAIN_CausticsColor;

	// Token: 0x040027F4 RID: 10228
	public float TERRAIN_CausticsWaterLevel;

	// Token: 0x040027F5 RID: 10229
	public float TERRAIN_CausticsWaterLevelByAngle;

	// Token: 0x040027F6 RID: 10230
	public float TERRAIN_CausticsWaterDeepFadeLength;

	// Token: 0x040027F7 RID: 10231
	public float TERRAIN_CausticsWaterShallowFadeLength;

	// Token: 0x040027F8 RID: 10232
	public float TERRAIN_CausticsTilingScale;

	// Token: 0x040027F9 RID: 10233
	public Texture2D TERRAIN_CausticsTex;

	// Token: 0x040027FA RID: 10234
	public Color rtp_customAmbientCorrection;

	// Token: 0x040027FB RID: 10235
	public float TERRAIN_IBL_DiffAO_Damp;

	// Token: 0x040027FC RID: 10236
	public float TERRAIN_IBLRefl_SpecAO_Damp;

	// Token: 0x040027FD RID: 10237
	public Cubemap _CubemapDiff;

	// Token: 0x040027FE RID: 10238
	public Cubemap _CubemapSpec;

	// Token: 0x040027FF RID: 10239
	public Vector4 RTP_LightDefVector;

	// Token: 0x04002800 RID: 10240
	public Color RTP_ReflexLightDiffuseColor;

	// Token: 0x04002801 RID: 10241
	public Color RTP_ReflexLightDiffuseColor2;

	// Token: 0x04002802 RID: 10242
	public Color RTP_ReflexLightSpecColor;

	// Token: 0x04002803 RID: 10243
	public float RTP_AOamp;

	// Token: 0x04002804 RID: 10244
	public float RTP_AOsharpness;

	// Token: 0x04002805 RID: 10245
	public Texture2D[] Bumps;

	// Token: 0x04002806 RID: 10246
	public float[] Spec;

	// Token: 0x04002807 RID: 10247
	public float[] FarSpecCorrection;

	// Token: 0x04002808 RID: 10248
	public float[] MixScale;

	// Token: 0x04002809 RID: 10249
	public float[] MixBlend;

	// Token: 0x0400280A RID: 10250
	public float[] MixSaturation;

	// Token: 0x0400280B RID: 10251
	public float[] RTP_gloss2mask;

	// Token: 0x0400280C RID: 10252
	public float[] RTP_gloss_mult;

	// Token: 0x0400280D RID: 10253
	public float[] RTP_gloss_shaping;

	// Token: 0x0400280E RID: 10254
	public float[] RTP_Fresnel;

	// Token: 0x0400280F RID: 10255
	public float[] RTP_FresnelAtten;

	// Token: 0x04002810 RID: 10256
	public float[] RTP_DiffFresnel;

	// Token: 0x04002811 RID: 10257
	public float[] RTP_IBL_bump_smoothness;

	// Token: 0x04002812 RID: 10258
	public float[] RTP_IBL_DiffuseStrength;

	// Token: 0x04002813 RID: 10259
	public float[] RTP_IBL_SpecStrength;

	// Token: 0x04002814 RID: 10260
	public float[] _DeferredSpecDampAddPass;

	// Token: 0x04002815 RID: 10261
	public float[] MixBrightness;

	// Token: 0x04002816 RID: 10262
	public float[] MixReplace;

	// Token: 0x04002817 RID: 10263
	public float[] LayerBrightness;

	// Token: 0x04002818 RID: 10264
	public float[] LayerBrightness2Spec;

	// Token: 0x04002819 RID: 10265
	public float[] LayerAlbedo2SpecColor;

	// Token: 0x0400281A RID: 10266
	public float[] LayerSaturation;

	// Token: 0x0400281B RID: 10267
	public float[] LayerEmission;

	// Token: 0x0400281C RID: 10268
	public Color[] LayerEmissionColor;

	// Token: 0x0400281D RID: 10269
	public float[] LayerEmissionRefractStrength;

	// Token: 0x0400281E RID: 10270
	public float[] LayerEmissionRefractHBedge;

	// Token: 0x0400281F RID: 10271
	public float[] GlobalColorPerLayer;

	// Token: 0x04002820 RID: 10272
	public float[] GlobalColorBottom;

	// Token: 0x04002821 RID: 10273
	public float[] GlobalColorTop;

	// Token: 0x04002822 RID: 10274
	public float[] GlobalColorColormapLoSat;

	// Token: 0x04002823 RID: 10275
	public float[] GlobalColorColormapHiSat;

	// Token: 0x04002824 RID: 10276
	public float[] GlobalColorLayerLoSat;

	// Token: 0x04002825 RID: 10277
	public float[] GlobalColorLayerHiSat;

	// Token: 0x04002826 RID: 10278
	public float[] GlobalColorLoBlend;

	// Token: 0x04002827 RID: 10279
	public float[] GlobalColorHiBlend;

	// Token: 0x04002828 RID: 10280
	public float[] PER_LAYER_HEIGHT_MODIFIER;

	// Token: 0x04002829 RID: 10281
	public float[] _SuperDetailStrengthMultA;

	// Token: 0x0400282A RID: 10282
	public float[] _SuperDetailStrengthMultASelfMaskNear;

	// Token: 0x0400282B RID: 10283
	public float[] _SuperDetailStrengthMultASelfMaskFar;

	// Token: 0x0400282C RID: 10284
	public float[] _SuperDetailStrengthMultB;

	// Token: 0x0400282D RID: 10285
	public float[] _SuperDetailStrengthMultBSelfMaskNear;

	// Token: 0x0400282E RID: 10286
	public float[] _SuperDetailStrengthMultBSelfMaskFar;

	// Token: 0x0400282F RID: 10287
	public float[] _SuperDetailStrengthNormal;

	// Token: 0x04002830 RID: 10288
	public float[] _BumpMapGlobalStrength;

	// Token: 0x04002831 RID: 10289
	public float[] VerticalTextureStrength;

	// Token: 0x04002832 RID: 10290
	public float[] AO_strength;

	// Token: 0x04002833 RID: 10291
	public float VerticalTextureGlobalBumpInfluence;

	// Token: 0x04002834 RID: 10292
	public float VerticalTextureTiling;

	// Token: 0x04002835 RID: 10293
	public Texture2D[] Heights;

	// Token: 0x04002836 RID: 10294
	public float[] _snow_strength_per_layer;

	// Token: 0x04002837 RID: 10295
	public ProceduralMaterial[] Substances;

	// Token: 0x04002838 RID: 10296
	public float[] TERRAIN_LayerWetStrength;

	// Token: 0x04002839 RID: 10297
	public float[] TERRAIN_WaterLevel;

	// Token: 0x0400283A RID: 10298
	public float[] TERRAIN_WaterLevelSlopeDamp;

	// Token: 0x0400283B RID: 10299
	public float[] TERRAIN_WaterEdge;

	// Token: 0x0400283C RID: 10300
	public float[] TERRAIN_WaterSpecularity;

	// Token: 0x0400283D RID: 10301
	public float[] TERRAIN_WaterGloss;

	// Token: 0x0400283E RID: 10302
	public float[] TERRAIN_WaterGlossDamper;

	// Token: 0x0400283F RID: 10303
	public float[] TERRAIN_WaterOpacity;

	// Token: 0x04002840 RID: 10304
	public float[] TERRAIN_Refraction;

	// Token: 0x04002841 RID: 10305
	public float[] TERRAIN_WetRefraction;

	// Token: 0x04002842 RID: 10306
	public float[] TERRAIN_Flow;

	// Token: 0x04002843 RID: 10307
	public float[] TERRAIN_WetFlow;

	// Token: 0x04002844 RID: 10308
	public float[] TERRAIN_WetSpecularity;

	// Token: 0x04002845 RID: 10309
	public float[] TERRAIN_WetGloss;

	// Token: 0x04002846 RID: 10310
	public Color[] TERRAIN_WaterColor;

	// Token: 0x04002847 RID: 10311
	public float[] TERRAIN_WaterIBL_SpecWetStrength;

	// Token: 0x04002848 RID: 10312
	public float[] TERRAIN_WaterIBL_SpecWaterStrength;

	// Token: 0x04002849 RID: 10313
	public float[] TERRAIN_WaterEmission;

	// Token: 0x0400284A RID: 10314
	public float _snow_strength;

	// Token: 0x0400284B RID: 10315
	public float _global_color_brightness_to_snow;

	// Token: 0x0400284C RID: 10316
	public float _snow_slope_factor;

	// Token: 0x0400284D RID: 10317
	public float _snow_edge_definition;

	// Token: 0x0400284E RID: 10318
	public float _snow_height_treshold;

	// Token: 0x0400284F RID: 10319
	public float _snow_height_transition;

	// Token: 0x04002850 RID: 10320
	public Color _snow_color;

	// Token: 0x04002851 RID: 10321
	public float _snow_specular;

	// Token: 0x04002852 RID: 10322
	public float _snow_gloss;

	// Token: 0x04002853 RID: 10323
	public float _snow_reflectivness;

	// Token: 0x04002854 RID: 10324
	public float _snow_deep_factor;

	// Token: 0x04002855 RID: 10325
	public float _snow_fresnel;

	// Token: 0x04002856 RID: 10326
	public float _snow_diff_fresnel;

	// Token: 0x04002857 RID: 10327
	public float _snow_IBL_DiffuseStrength;

	// Token: 0x04002858 RID: 10328
	public float _snow_IBL_SpecStrength;
}
