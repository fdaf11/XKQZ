using System;
using UnityEngine;

// Token: 0x02000643 RID: 1603
[ExecuteInEditMode]
[AddComponentMenu("AFS/Setup Advanced Foliage Shader")]
public class SetupAdvancedFoliageShader : MonoBehaviour
{
	// Token: 0x0600277C RID: 10108 RVA: 0x00138760 File Offset: 0x00136960
	private void Awake()
	{
		this.AfsTreeBillboardShadowShader = Shader.Find("Hidden/TerrainEngine/BillboardTree");
		this.afsCheckColorSpace();
		this.afsSetupColorSpace();
		this.afsLightingSettings();
		this.afsSetupTerrainEngine();
		this.afsSetupGrassShader();
		this.afsUpdateWind();
		this.afsUpdateRain();
		this.afsAutoSyncToTerrain();
		this.afsUpdateTreeAndBillboardShaders();
		this.afsUpdateGrassTreesBillboards();
		this.afsSetupCameraLayerCulling();
	}

	// Token: 0x0600277D RID: 10109 RVA: 0x0001A0A9 File Offset: 0x000182A9
	private void Start()
	{
		this.AfsTreeBillboardShadowShader = Shader.Find("Hidden/TerrainEngine/BillboardTree");
		this.afsSetupColorSpace();
	}

	// Token: 0x0600277E RID: 10110 RVA: 0x0001A0C1 File Offset: 0x000182C1
	public void Update()
	{
		this.afsUpdateGrassTreesBillboards();
		this.afsSetupColorSpace();
		this.afsLightingSettings();
		this.afsUpdateWind();
		this.afsUpdateRain();
		this.afsAutoSyncToTerrain();
	}

	// Token: 0x0600277F RID: 10111 RVA: 0x0000264F File Offset: 0x0000084F
	private void afsCheckColorSpace()
	{
	}

	// Token: 0x06002780 RID: 10112 RVA: 0x001387C0 File Offset: 0x001369C0
	private void afsSetupColorSpace()
	{
		if (this.isLinear)
		{
			Shader.EnableKeyword("LUX_LINEAR");
			Shader.DisableKeyword("LUX_GAMMA");
			Shader.EnableKeyword("MARMO_LINEAR");
			Shader.DisableKeyword("MARMO_GAMMA");
		}
		else
		{
			Shader.EnableKeyword("LUX_GAMMA");
			Shader.DisableKeyword("LUX_LINEAR");
			Shader.EnableKeyword("MARMO_GAMMA");
			Shader.DisableKeyword("MARMO_LINEAR");
		}
	}

	// Token: 0x06002781 RID: 10113 RVA: 0x00138830 File Offset: 0x00136A30
	private void afsLightingSettings()
	{
		if (this.UseLinearLightingFixTrees)
		{
			Shader.EnableKeyword("AFS_LLFIX_MESHTREES_ON");
			Shader.EnableKeyword("AFS_LLFIX_BILLBOARDS_ON");
			Shader.DisableKeyword("AFS_LLFIX_MESHTREES_OFF");
			Shader.DisableKeyword("AFS_LLFIX_BILLBOARDS_OFF");
		}
		else
		{
			Shader.EnableKeyword("AFS_LLFIX_MESHTREES_OFF");
			Shader.EnableKeyword("AFS_LLFIX_BILLBOARDS_OFF");
			Shader.DisableKeyword("AFS_LLFIX_MESHTREES_ON");
			Shader.DisableKeyword("AFS_LLFIX_BILLBOARDS_ON");
		}
		if (this.useIBL)
		{
			Shader.DisableKeyword("AFS_SH");
			Shader.EnableKeyword("AFS_IBL");
		}
		else
		{
			Shader.EnableKeyword("AFS_SH");
			Shader.DisableKeyword("AFS_IBL");
			if (this.isLinear)
			{
				Shader.SetGlobalColor("_AfsAmbientColor", RenderSettings.ambientLight.linear);
			}
			else
			{
				Shader.SetGlobalColor("_AfsAmbientColor", RenderSettings.ambientLight);
			}
		}
		if (this.controlIBL)
		{
			if (this.isLinear)
			{
				this.DiffuseExposure = this.AFS_IBL_DiffuseExposure * this.AFS_IBL_MasterExposure;
				if (this.diffuseIsHDR)
				{
					this.DiffuseExposure *= Mathf.Pow(this.AFS_HDR_Scale, 2.2333333f) * this.AFS_IBL_MasterExposure;
				}
				this.SpecularExposure = this.AFS_IBL_SpecularExposure * this.AFS_IBL_MasterExposure;
				if (this.specularIsHDR)
				{
					this.SpecularExposure *= Mathf.Pow(this.AFS_HDR_Scale, 2.2333333f) * this.AFS_IBL_MasterExposure;
				}
				this.CameraExposure = this.AFS_CameraExposure;
			}
			else
			{
				this.DiffuseExposure = Mathf.Pow(this.AFS_IBL_DiffuseExposure * this.AFS_IBL_MasterExposure, 0.44776118f);
				if (this.diffuseIsHDR)
				{
					this.DiffuseExposure *= this.AFS_HDR_Scale;
				}
				this.SpecularExposure = Mathf.Pow(this.AFS_IBL_SpecularExposure * this.AFS_IBL_MasterExposure, 0.44776118f);
				if (this.specularIsHDR)
				{
					this.SpecularExposure *= this.AFS_HDR_Scale;
				}
				this.CameraExposure = Mathf.Pow(this.AFS_CameraExposure, 0.44776118f);
			}
			Shader.SetGlobalVector("ExposureIBL", new Vector4(this.DiffuseExposure, this.SpecularExposure, 1f, this.CameraExposure));
			if (this.diffuseCube)
			{
				Shader.SetGlobalTexture("_DiffCubeIBL", this.diffuseCube);
			}
			if (this.specularCube)
			{
				Shader.SetGlobalTexture("_SpecCubeIBL", this.specularCube);
			}
			else
			{
				this.createPlaceHolderCube();
				Shader.SetGlobalTexture("_SpecCubeIBL", this.PlaceHolderCube);
			}
		}
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x00138AC4 File Offset: 0x00136CC4
	private void createPlaceHolderCube()
	{
		if (this.PlaceHolderCube == null)
		{
			this.PlaceHolderCube = new Cubemap(16, 5, true);
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					for (int k = 0; k < 16; k++)
					{
						this.PlaceHolderCube.SetPixel(i, j, k, Color.black);
					}
				}
			}
			this.PlaceHolderCube.Apply(true);
		}
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x00138B48 File Offset: 0x00136D48
	private void afsSetupGrassShader()
	{
		if (Application.isPlaying || this.AllGrassObjectsCombined)
		{
			Shader.DisableKeyword("IN_EDITMODE");
			Shader.EnableKeyword("IN_PLAYMODE");
		}
		else
		{
			Shader.DisableKeyword("IN_PLAYMODE");
			Shader.EnableKeyword("IN_EDITMODE");
		}
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x00138B98 File Offset: 0x00136D98
	private void afsSetupTerrainEngine()
	{
		Shader.SetGlobalFloat("_AfsAlphaCutOff", this.VertexLitAlphaCutOff);
		Shader.SetGlobalColor("_AfsTranslucencyColor", this.VertexLitTranslucencyColor);
		Shader.SetGlobalFloat("_AfsTranslucencyViewDependency", this.VertexLitTranslucencyViewDependency);
		Shader.SetGlobalFloat("_AfsShadowStrength", this.VertexLitShadowStrength);
		Shader.SetGlobalFloat("_AfsShininess", this.VertexLitShininess);
		Shader.SetGlobalColor("_TranslucencyColor", this.VertexLitTranslucencyColor);
		Shader.SetGlobalFloat("_TranslucencyViewDependency", this.VertexLitTranslucencyViewDependency);
		Shader.SetGlobalFloat("_ShadowStrength", this.VertexLitShadowStrength);
		if (this.TerrainFoliageNrmSpecMap != null)
		{
			Shader.SetGlobalTexture("_TerrianBumpTransSpecMap", this.TerrainFoliageNrmSpecMap);
		}
		Shader.SetGlobalVector("_AfsSpecFade", new Vector4(this.AfsSpecFade.x, this.AfsSpecFade.y, 1f, 1f));
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x00138C78 File Offset: 0x00136E78
	private void afsUpdateWind()
	{
		this.TempWind = this.Wind;
		this.TempWindForce = this.Wind.w;
		this.TempWind.x = this.TempWind.x * ((1.25f + Mathf.Sin(Time.time * this.WindFrequency) * Mathf.Sin(Time.time * 0.375f)) * 0.5f);
		this.TempWind.z = this.TempWind.z * ((1.25f + Mathf.Sin(Time.time * this.WindFrequency) * Mathf.Sin(Time.time * 0.193f)) * 0.5f);
		this.TempWind.w = this.TempWindForce;
		Shader.SetGlobalVector("_Wind", this.TempWind);
		this.GrassWind = (this.TempWind.x + this.TempWind.z) / 2f * this.Wind.w;
		Shader.SetGlobalFloat("_AfsWaveSize", 0.5f / this.WaveSizeFoliageShader);
		Shader.SetGlobalVector("_AfsWaveAndDistance", new Vector4(Time.time * (this.WindFrequency * 0.05f), 0.5f / this.WaveSizeForGrassshader, this.GrassWind * this.WindMultiplierForGrassshader, this.DetailDistanceForGrassShader * this.DetailDistanceForGrassShader));
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x0001A0E7 File Offset: 0x000182E7
	private void afsUpdateRain()
	{
		Shader.SetGlobalFloat("_AfsRainamount", this.RainAmount);
	}

	// Token: 0x06002787 RID: 10119 RVA: 0x00138DD0 File Offset: 0x00136FD0
	private void afsAutoSyncToTerrain()
	{
		if (this.AutoSyncToTerrain && this.SyncedTerrain != null)
		{
			this.DetailDistanceForGrassShader = this.SyncedTerrain.detailObjectDistance;
			this.BillboardStart = this.SyncedTerrain.treeBillboardDistance;
			if (!this.TreeBillboardShadows)
			{
				this.BillboardFadeLenght = this.SyncedTerrain.treeCrossFadeLength;
			}
			this.GrassWavingTint = this.SyncedTerrain.terrainData.wavingGrassTint;
		}
	}

	// Token: 0x06002788 RID: 10120 RVA: 0x00138E50 File Offset: 0x00137050
	private void afsUpdateTreeAndBillboardShaders()
	{
		if (this.TreeShadowDissolve)
		{
			Shader.EnableKeyword("TREE_SHADOW_DISSOLVE");
			Shader.DisableKeyword("TREE_SHADOW_NO_DISSOLVE");
		}
		else
		{
			Shader.EnableKeyword("TREE_SHADOW_NO_DISSOLVE");
			Shader.DisableKeyword("TREE_SHADOW_DISSOLVE");
		}
		if (this.TreeBillboardShadows)
		{
			Shader.EnableKeyword("BILLBOARD_SHADOWS");
			Shader.DisableKeyword("BILLBOARD_NO_SHADOWS");
		}
		else
		{
			Shader.EnableKeyword("BILLBOARD_NO_SHADOWS");
			Shader.DisableKeyword("BILLBOARD_SHADOWS");
		}
		if (this.TreeShadowEdgeFade)
		{
			Shader.EnableKeyword("TREESHADOW_EDGEFADE");
			Shader.DisableKeyword("TREESHADOW_NO_EDGEFADE");
		}
		else
		{
			Shader.EnableKeyword("TREESHADOW_NO_EDGEFADE");
			Shader.DisableKeyword("TREESHADOW_EDGEFADE");
		}
		if (this.BillboardShadowEdgeFade)
		{
			Shader.EnableKeyword("BILLBOARDSHADOW_EDGEFADE");
			Shader.DisableKeyword("BILLBOARDSHADOW_NO_EDGEFADE");
		}
		else
		{
			Shader.EnableKeyword("BILLBOARDSHADOW_NO_EDGEFADE");
			Shader.DisableKeyword("BILLBOARDSHADOW_EDGEFADE");
		}
		if (this.GrassAnimateNormal)
		{
			Shader.EnableKeyword("GRASS_ANIMATE_NORMAL");
			Shader.DisableKeyword("GRASS_ANIMATE_COLOR");
		}
		else
		{
			Shader.EnableKeyword("GRASS_ANIMATE_COLOR");
			Shader.DisableKeyword("GRASS_ANIMATE_NORMAL");
		}
		if (this.TreeBillboardShadows && this.TreeBillboardLOD != 300 && this.AfsTreeBillboardShadowShader != null)
		{
			this.AfsTreeBillboardShadowShader.maximumLOD = 300;
			this.TreeBillboardLOD = 300;
		}
		if (!this.TreeBillboardShadows && this.TreeBillboardLOD != 200 && this.AfsTreeBillboardShadowShader != null)
		{
			this.AfsTreeBillboardShadowShader.maximumLOD = 200;
			this.TreeBillboardLOD = 200;
		}
		Shader.SetGlobalFloat("_AfsBillboardFog", 1f);
	}

	// Token: 0x06002789 RID: 10121 RVA: 0x00139014 File Offset: 0x00137214
	private void afsUpdateGrassTreesBillboards()
	{
		Shader.SetGlobalColor("_AfsWavingTint", this.GrassWavingTint);
		Shader.SetGlobalVector("_AfsTerrainTrees", new Vector4(this.BillboardStart, this.BillboardFadeLenght, this.BillboardFadeOutLength, 0f));
		if (this.BillboardAdjustToCamera)
		{
			if (Camera.main)
			{
				this.CameraForward = Camera.main.transform.forward;
				this.ShadowCameraForward = this.CameraForward;
				this.rollingX = Camera.main.transform.eulerAngles.x;
			}
			else
			{
				Debug.Log("You have to tag your Camera as MainCamera");
			}
			if (this.rollingX >= 270f)
			{
				this.rollingX -= 270f;
				this.rollingX = 90f - this.rollingX;
				this.rollingXShadow = this.rollingX;
			}
			else
			{
				this.rollingXShadow = -this.rollingX;
				if (this.rollingX > this.BillboardAngleLimit)
				{
					this.rollingX = Mathf.Lerp(this.rollingX, 0f, this.rollingX / this.BillboardAngleLimit - 1f);
				}
				this.rollingX *= -1f;
			}
		}
		else
		{
			this.rollingX = 0f;
			this.rollingXShadow = 0f;
		}
		this.CameraForward *= this.rollingX / 90f;
		this.ShadowCameraForward *= this.rollingXShadow / 90f;
		Shader.SetGlobalVector("_AfsBillboardCameraForward", new Vector4(this.CameraForward.x, this.CameraForward.y, this.CameraForward.z, 1f));
		Shader.SetGlobalVector("_AfsBillboardShadowCameraForward", new Vector4(this.ShadowCameraForward.x, this.ShadowCameraForward.y, this.ShadowCameraForward.z, 1f));
		if (this.TreeBillboardShadows)
		{
			if (this.BillboardLightReference != null)
			{
				this.lightDir = this.BillboardLightReference.transform.forward;
				this.templightDir = this.lightDir;
				this.lightDir = Vector3.Cross(this.lightDir, Vector3.up);
				if (Vector3.Dot(this.templightDir, Camera.main.transform.forward) > 0f)
				{
					this.lightDir = Quaternion.AngleAxis(180f, Vector3.up) * this.lightDir;
				}
				Shader.SetGlobalVector("_AfsSunDirection", new Vector4(this.lightDir.x, this.lightDir.y, this.lightDir.z, 1f));
				this.CameraForwardVec = Camera.main.transform.forward;
				this.allTerrains = (Object.FindObjectsOfType(typeof(Terrain)) as Terrain[]);
				for (int i = 0; i < this.allTerrains.Length; i++)
				{
					this.allTerrains[i].treeCrossFadeLength = 0.0001f;
				}
				this.allTerrains = null;
				this.CameraAngle = Camera.main.fieldOfView;
				this.CameraAngle -= this.CameraAngle * this.BillboardShadowEdgeFadeThreshold;
				this.CameraAngle = Mathf.Cos(this.CameraAngle * 0.017453292f);
				Shader.SetGlobalVector("_CameraForwardVec", new Vector4(this.CameraForwardVec.x, this.CameraForwardVec.y, this.CameraForwardVec.z, this.CameraAngle));
			}
			else
			{
				Debug.LogWarning("You have to specify a Light Reference!");
			}
		}
		if (this.AutosyncShadowColor)
		{
			this.BillboardShadowColor = RenderSettings.ambientLight;
			this.BillboardShadowColor = this.Desaturate(this.BillboardShadowColor.r * this.BillboardAmbientLightFactor, this.BillboardShadowColor.g * this.BillboardAmbientLightFactor, this.BillboardShadowColor.b * this.BillboardAmbientLightFactor);
			if (this.BillboardLightReference)
			{
				this.BillboardShadowColor += 0.5f * (this.BillboardShadowColor * (1f - this.BillboardLightReference.light.shadowStrength));
			}
		}
		Shader.SetGlobalColor("_AfsAmbientBillboardLight", this.BillboardShadowColor);
	}

	// Token: 0x0600278A RID: 10122 RVA: 0x00139484 File Offset: 0x00137684
	private void afsSetupCameraLayerCulling()
	{
		if (this.EnableCameraLayerCulling)
		{
			for (int i = 0; i < Camera.allCameras.Length; i++)
			{
				float[] array = new float[32];
				array = Camera.allCameras[i].layerCullDistances;
				array[8] = (float)this.SmallDetailsDistance;
				array[9] = (float)this.MediumDetailsDistance;
				Camera.allCameras[i].layerCullDistances = array;
			}
		}
	}

	// Token: 0x0600278B RID: 10123 RVA: 0x001394F0 File Offset: 0x001376F0
	private Color Desaturate(float r, float g, float b)
	{
		this.grey = 0.3f * r + 0.59f * g + 0.11f * b;
		r = this.grey * this.BillboardAmbientLightDesaturationFactor + r * (1f - this.BillboardAmbientLightDesaturationFactor);
		g = this.grey * this.BillboardAmbientLightDesaturationFactor + g * (1f - this.BillboardAmbientLightDesaturationFactor);
		b = this.grey * this.BillboardAmbientLightDesaturationFactor + b * (1f - this.BillboardAmbientLightDesaturationFactor);
		return new Color(r, g, b, 1f);
	}

	// Token: 0x0400310B RID: 12555
	public bool isLinear;

	// Token: 0x0400310C RID: 12556
	public bool useIBL;

	// Token: 0x0400310D RID: 12557
	public bool controlIBL;

	// Token: 0x0400310E RID: 12558
	public float AFS_HDR_Scale = 6f;

	// Token: 0x0400310F RID: 12559
	public float AFS_IBL_MasterExposure = 1f;

	// Token: 0x04003110 RID: 12560
	public float AFS_IBL_DiffuseExposure = 1f;

	// Token: 0x04003111 RID: 12561
	private float DiffuseExposure;

	// Token: 0x04003112 RID: 12562
	public float AFS_IBL_SpecularExposure = 1f;

	// Token: 0x04003113 RID: 12563
	private float SpecularExposure;

	// Token: 0x04003114 RID: 12564
	private Color Unity_Ambient_Lighting;

	// Token: 0x04003115 RID: 12565
	public Cubemap diffuseCube;

	// Token: 0x04003116 RID: 12566
	public bool diffuseIsHDR;

	// Token: 0x04003117 RID: 12567
	public Cubemap specularCube;

	// Token: 0x04003118 RID: 12568
	public bool specularIsHDR;

	// Token: 0x04003119 RID: 12569
	private Cubemap PlaceHolderCube;

	// Token: 0x0400311A RID: 12570
	public float AFS_CameraExposure = 1f;

	// Token: 0x0400311B RID: 12571
	public float CameraExposure;

	// Token: 0x0400311C RID: 12572
	public int AFSFog_Mode = 2;

	// Token: 0x0400311D RID: 12573
	public string AFSShader_Folder = "Assets/Advanced Foliage Shader v3/Shaders/";

	// Token: 0x0400311E RID: 12574
	public Vector4 Wind = new Vector4(0.85f, 0.075f, 0.4f, 0.5f);

	// Token: 0x0400311F RID: 12575
	public float WindFrequency = 0.75f;

	// Token: 0x04003120 RID: 12576
	public float WaveSizeFoliageShader = 10f;

	// Token: 0x04003121 RID: 12577
	public float WindMultiplierForGrassshader = 4f;

	// Token: 0x04003122 RID: 12578
	public float WaveSizeForGrassshader = 10f;

	// Token: 0x04003123 RID: 12579
	public float RainAmount;

	// Token: 0x04003124 RID: 12580
	public float VertexLitAlphaCutOff = 0.3f;

	// Token: 0x04003125 RID: 12581
	public Color VertexLitTranslucencyColor = new Color(0.73f, 0.85f, 0.4f, 1f);

	// Token: 0x04003126 RID: 12582
	public float VertexLitTranslucencyViewDependency = 0.7f;

	// Token: 0x04003127 RID: 12583
	public float VertexLitShadowStrength = 0.8f;

	// Token: 0x04003128 RID: 12584
	public float VertexLitShininess = 0.2f;

	// Token: 0x04003129 RID: 12585
	public Vector2 AfsSpecFade = new Vector2(60f, 10f);

	// Token: 0x0400312A RID: 12586
	public Texture TerrainFoliageNrmSpecMap;

	// Token: 0x0400312B RID: 12587
	public bool AutoSyncToTerrain;

	// Token: 0x0400312C RID: 12588
	public Terrain SyncedTerrain;

	// Token: 0x0400312D RID: 12589
	public bool AutoSyncInPlaymode;

	// Token: 0x0400312E RID: 12590
	public float DetailDistanceForGrassShader = 80f;

	// Token: 0x0400312F RID: 12591
	public float BillboardStart = 50f;

	// Token: 0x04003130 RID: 12592
	public float BillboardFadeLenght = 5f;

	// Token: 0x04003131 RID: 12593
	public bool GrassAnimateNormal;

	// Token: 0x04003132 RID: 12594
	public Color GrassWavingTint;

	// Token: 0x04003133 RID: 12595
	public bool UseLinearLightingFixTrees = true;

	// Token: 0x04003134 RID: 12596
	public bool TreeShadowDissolve;

	// Token: 0x04003135 RID: 12597
	public bool TreeBillboardShadows;

	// Token: 0x04003136 RID: 12598
	public int TreeBillboardLOD = 200;

	// Token: 0x04003137 RID: 12599
	public bool TreeShadowEdgeFade;

	// Token: 0x04003138 RID: 12600
	public bool BillboardShadowEdgeFade;

	// Token: 0x04003139 RID: 12601
	public float BillboardShadowEdgeFadeThreshold = 0.1f;

	// Token: 0x0400313A RID: 12602
	public float BillboardFadeOutLength = 60f;

	// Token: 0x0400313B RID: 12603
	public bool BillboardAdjustToCamera = true;

	// Token: 0x0400313C RID: 12604
	public float BillboardAngleLimit = 30f;

	// Token: 0x0400313D RID: 12605
	public Shader AfsTreeBillboardShadowShader;

	// Token: 0x0400313E RID: 12606
	public GameObject BillboardLightReference;

	// Token: 0x0400313F RID: 12607
	public Color BillboardShadowColor;

	// Token: 0x04003140 RID: 12608
	public float BillboardAmbientLightFactor = 1f;

	// Token: 0x04003141 RID: 12609
	public float BillboardAmbientLightDesaturationFactor = 0.7f;

	// Token: 0x04003142 RID: 12610
	public bool AutosyncShadowColor;

	// Token: 0x04003143 RID: 12611
	public bool EnableCameraLayerCulling = true;

	// Token: 0x04003144 RID: 12612
	public int SmallDetailsDistance = 70;

	// Token: 0x04003145 RID: 12613
	public int MediumDetailsDistance = 90;

	// Token: 0x04003146 RID: 12614
	public bool AllGrassObjectsCombined;

	// Token: 0x04003147 RID: 12615
	private Vector4 TempWind;

	// Token: 0x04003148 RID: 12616
	private float TempWindForce;

	// Token: 0x04003149 RID: 12617
	private float GrassWind;

	// Token: 0x0400314A RID: 12618
	private Vector3 CameraForward = new Vector3(0f, 0f, 0f);

	// Token: 0x0400314B RID: 12619
	private Vector3 ShadowCameraForward = new Vector3(0f, 0f, 0f);

	// Token: 0x0400314C RID: 12620
	private Vector3 CameraForwardVec;

	// Token: 0x0400314D RID: 12621
	private float rollingX;

	// Token: 0x0400314E RID: 12622
	private float rollingXShadow;

	// Token: 0x0400314F RID: 12623
	private Vector3 lightDir;

	// Token: 0x04003150 RID: 12624
	private Vector3 templightDir;

	// Token: 0x04003151 RID: 12625
	private float CameraAngle;

	// Token: 0x04003152 RID: 12626
	private Terrain[] allTerrains;

	// Token: 0x04003153 RID: 12627
	private float grey;
}
