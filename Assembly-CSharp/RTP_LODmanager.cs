using System;
using UnityEngine;

// Token: 0x02000533 RID: 1331
public class RTP_LODmanager : MonoBehaviour
{
	// Token: 0x060021EF RID: 8687 RVA: 0x00016B89 File Offset: 0x00014D89
	private void Awake()
	{
		this.RefreshLODlevel();
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x00101EDC File Offset: 0x001000DC
	public void RefreshLODlevel()
	{
		ReliefTerrain[] array = (ReliefTerrain[])Object.FindObjectsOfType(typeof(ReliefTerrain));
		ReliefTerrain reliefTerrain = null;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].GetComponent(typeof(Terrain)))
			{
				reliefTerrain = array[i];
				break;
			}
		}
		if (reliefTerrain != null && reliefTerrain.globalSettingsHolder != null && reliefTerrain.globalSettingsHolder.useTerrainMaterial)
		{
			if (this.terrain_shader == null)
			{
				this.terrain_shader = Shader.Find("Relief Pack/ReliefTerrain-FirstPass");
			}
			if (this.terrain_shader_add == null)
			{
				this.terrain_shader_add = Shader.Find("Relief Pack/ReliefTerrain-AddPass");
			}
		}
		else
		{
			if (this.terrain_shader == null)
			{
				this.terrain_shader = Shader.Find("Hidden/TerrainEngine/Splatmap/Lightmap-FirstPass");
			}
			if (this.terrain_shader_add == null)
			{
				this.terrain_shader_add = Shader.Find("Hidden/TerrainEngine/Splatmap/Lightmap-AddPass");
			}
		}
		if (this.terrain_shader_far == null)
		{
			this.terrain_shader_far = Shader.Find("Relief Pack/ReliefTerrain-FarOnly");
		}
		if (this.terrain2geom_shader == null)
		{
			this.terrain2geom_shader = Shader.Find("Relief Pack/Terrain2Geometry");
		}
		if (this.terrain_geomBlend_shader == null)
		{
			this.terrain_geomBlend_shader = Shader.Find("Relief Pack/ReliefTerrainGeometryBlendBase");
		}
		if (this.terrain2geom_geomBlend_shader == null)
		{
			this.terrain2geom_geomBlend_shader = Shader.Find("Relief Pack/ReliefTerrain2GeometryBlendBase");
		}
		if (this.terrain_geomBlend_GeometryBlend_BumpedDetailSnow == null)
		{
			this.terrain_geomBlend_GeometryBlend_BumpedDetailSnow = Shader.Find("Relief Pack/GeometryBlend_BumpedDetailSnow");
		}
		if (this.geomblend_GeometryBlend_WaterShader_2VertexPaint_HB == null)
		{
			this.geomblend_GeometryBlend_WaterShader_2VertexPaint_HB = Shader.Find("Relief Pack/GeometryBlend_WaterShader_2VertexPaint_HB");
		}
		if (this.geomBlend_GeometryBlend_WaterShader_FlowMap_HB == null)
		{
			this.geomBlend_GeometryBlend_WaterShader_FlowMap_HB = Shader.Find("Relief Pack/GeometryBlend_WaterShader_FlowMap_HB");
		}
		int maximumLOD;
		if (this.RTP_LODlevel == TerrainShaderLod.CLASSIC)
		{
			maximumLOD = 100;
		}
		else
		{
			maximumLOD = 700;
			if (this.RTP_LODlevel == TerrainShaderLod.POM)
			{
				if (this.RTP_SHADOWS)
				{
					if (this.RTP_SOFT_SHADOWS)
					{
						Shader.EnableKeyword("RTP_POM_SHADING_HI");
						Shader.DisableKeyword("RTP_POM_SHADING_MED");
						Shader.DisableKeyword("RTP_POM_SHADING_LO");
					}
					else
					{
						Shader.EnableKeyword("RTP_POM_SHADING_MED");
						Shader.DisableKeyword("RTP_POM_SHADING_HI");
						Shader.DisableKeyword("RTP_POM_SHADING_LO");
					}
				}
				else
				{
					Shader.EnableKeyword("RTP_POM_SHADING_LO");
					Shader.DisableKeyword("RTP_POM_SHADING_MED");
					Shader.DisableKeyword("RTP_POM_SHADING_HI");
				}
				Shader.DisableKeyword("RTP_PM_SHADING");
				Shader.DisableKeyword("RTP_SIMPLE_SHADING");
			}
			else if (this.RTP_LODlevel == TerrainShaderLod.PM)
			{
				Shader.DisableKeyword("RTP_POM_SHADING_HI");
				Shader.DisableKeyword("RTP_POM_SHADING_MED");
				Shader.DisableKeyword("RTP_POM_SHADING_LO");
				Shader.EnableKeyword("RTP_PM_SHADING");
				Shader.DisableKeyword("RTP_SIMPLE_SHADING");
			}
			else
			{
				Shader.DisableKeyword("RTP_POM_SHADING_HI");
				Shader.DisableKeyword("RTP_POM_SHADING_MED");
				Shader.DisableKeyword("RTP_POM_SHADING_LO");
				Shader.DisableKeyword("RTP_PM_SHADING");
				Shader.EnableKeyword("RTP_SIMPLE_SHADING");
			}
		}
		if (this.terrain_shader != null)
		{
			this.terrain_shader.maximumLOD = maximumLOD;
		}
		if (this.terrain_shader_far != null)
		{
			this.terrain_shader_far.maximumLOD = maximumLOD;
		}
		if (this.terrain_shader_add != null)
		{
			this.terrain_shader_add.maximumLOD = maximumLOD;
		}
		if (this.terrain2geom_shader != null)
		{
			this.terrain2geom_shader.maximumLOD = maximumLOD;
		}
		if (this.terrain_geomBlend_shader != null)
		{
			this.terrain_geomBlend_shader.maximumLOD = maximumLOD;
		}
		if (this.terrain2geom_geomBlend_shader != null)
		{
			this.terrain2geom_geomBlend_shader.maximumLOD = maximumLOD;
		}
		if (this.terrain_geomBlend_GeometryBlend_BumpedDetailSnow != null)
		{
			this.terrain_geomBlend_GeometryBlend_BumpedDetailSnow.maximumLOD = maximumLOD;
		}
		if (this.geomblend_GeometryBlend_WaterShader_2VertexPaint_HB != null)
		{
			this.geomblend_GeometryBlend_WaterShader_2VertexPaint_HB.maximumLOD = maximumLOD;
		}
		if (this.geomBlend_GeometryBlend_WaterShader_FlowMap_HB != null)
		{
			this.geomBlend_GeometryBlend_WaterShader_FlowMap_HB.maximumLOD = maximumLOD;
		}
	}

	// Token: 0x0400256A RID: 9578
	public TerrainShaderLod RTP_LODlevel;

	// Token: 0x0400256B RID: 9579
	public bool RTP_SHADOWS = true;

	// Token: 0x0400256C RID: 9580
	public bool RTP_SOFT_SHADOWS = true;

	// Token: 0x0400256D RID: 9581
	public bool show_first_features;

	// Token: 0x0400256E RID: 9582
	public bool show_add_features;

	// Token: 0x0400256F RID: 9583
	public bool RTP_NOFORWARDADD;

	// Token: 0x04002570 RID: 9584
	public bool RTP_FULLFORWARDSHADOWS;

	// Token: 0x04002571 RID: 9585
	public bool RTP_NOLIGHTMAP;

	// Token: 0x04002572 RID: 9586
	public bool RTP_NODIRLIGHTMAP;

	// Token: 0x04002573 RID: 9587
	public bool RTP_NODYNLIGHTMAP;

	// Token: 0x04002574 RID: 9588
	public bool RTP_NOAMBIENT;

	// Token: 0x04002575 RID: 9589
	public bool NO_SPECULARITY;

	// Token: 0x04002576 RID: 9590
	public bool RTP_ADDSHADOW;

	// Token: 0x04002577 RID: 9591
	public bool RTP_INDEPENDENT_TILING;

	// Token: 0x04002578 RID: 9592
	public bool RTP_CUT_HOLES;

	// Token: 0x04002579 RID: 9593
	public bool FIX_REFRESHING_ISSUE = true;

	// Token: 0x0400257A RID: 9594
	public bool RTP_USE_COLOR_ATLAS_FIRST;

	// Token: 0x0400257B RID: 9595
	public bool RTP_USE_COLOR_ATLAS_ADD;

	// Token: 0x0400257C RID: 9596
	public RTPFogType RTP_FOGTYPE;

	// Token: 0x0400257D RID: 9597
	public bool ADV_COLOR_MAP_BLENDING_FIRST;

	// Token: 0x0400257E RID: 9598
	public bool ADV_COLOR_MAP_BLENDING_ADD;

	// Token: 0x0400257F RID: 9599
	public bool RTP_UV_BLEND_FIRST = true;

	// Token: 0x04002580 RID: 9600
	public bool RTP_UV_BLEND_ADD = true;

	// Token: 0x04002581 RID: 9601
	public bool RTP_DISTANCE_ONLY_UV_BLEND_FIRST = true;

	// Token: 0x04002582 RID: 9602
	public bool RTP_DISTANCE_ONLY_UV_BLEND_ADD = true;

	// Token: 0x04002583 RID: 9603
	public bool RTP_NORMALS_FOR_REPLACE_UV_BLEND_FIRST = true;

	// Token: 0x04002584 RID: 9604
	public bool RTP_NORMALS_FOR_REPLACE_UV_BLEND_ADD = true;

	// Token: 0x04002585 RID: 9605
	public bool RTP_SUPER_DETAIL_FIRST = true;

	// Token: 0x04002586 RID: 9606
	public bool RTP_SUPER_DETAIL_ADD = true;

	// Token: 0x04002587 RID: 9607
	public bool RTP_SUPER_DETAIL_MULTS_FIRST;

	// Token: 0x04002588 RID: 9608
	public bool RTP_SUPER_DETAIL_MULTS_ADD;

	// Token: 0x04002589 RID: 9609
	public bool RTP_SNOW_FIRST;

	// Token: 0x0400258A RID: 9610
	public bool RTP_SNOW_ADD;

	// Token: 0x0400258B RID: 9611
	public bool RTP_SNW_CHOOSEN_LAYER_COLOR_FIRST;

	// Token: 0x0400258C RID: 9612
	public bool RTP_SNW_CHOOSEN_LAYER_COLOR_ADD;

	// Token: 0x0400258D RID: 9613
	public int RTP_SNW_CHOOSEN_LAYER_COLOR_NUM_FIRST = 7;

	// Token: 0x0400258E RID: 9614
	public int RTP_SNW_CHOOSEN_LAYER_COLOR_NUM_ADD = 7;

	// Token: 0x0400258F RID: 9615
	public bool RTP_SNW_CHOOSEN_LAYER_NORMAL_FIRST;

	// Token: 0x04002590 RID: 9616
	public bool RTP_SNW_CHOOSEN_LAYER_NORMAL_ADD;

	// Token: 0x04002591 RID: 9617
	public int RTP_SNW_CHOOSEN_LAYER_NORMAL_NUM_FIRST = 7;

	// Token: 0x04002592 RID: 9618
	public int RTP_SNW_CHOOSEN_LAYER_NORMAL_NUM_ADD = 7;

	// Token: 0x04002593 RID: 9619
	public RTPLodLevel MAX_LOD_FIRST = RTPLodLevel.PM;

	// Token: 0x04002594 RID: 9620
	public RTPLodLevel MAX_LOD_FIRST_PLUS4 = RTPLodLevel.SIMPLE;

	// Token: 0x04002595 RID: 9621
	public RTPLodLevel MAX_LOD_ADD = RTPLodLevel.PM;

	// Token: 0x04002596 RID: 9622
	public bool RTP_SHARPEN_HEIGHTBLEND_EDGES_PASS1_FIRST = true;

	// Token: 0x04002597 RID: 9623
	public bool RTP_SHARPEN_HEIGHTBLEND_EDGES_PASS2_FIRST;

	// Token: 0x04002598 RID: 9624
	public bool RTP_SHARPEN_HEIGHTBLEND_EDGES_PASS1_ADD = true;

	// Token: 0x04002599 RID: 9625
	public bool RTP_SHARPEN_HEIGHTBLEND_EDGES_PASS2_ADD;

	// Token: 0x0400259A RID: 9626
	public bool RTP_HEIGHTBLEND_AO_FIRST;

	// Token: 0x0400259B RID: 9627
	public bool RTP_HEIGHTBLEND_AO_ADD;

	// Token: 0x0400259C RID: 9628
	public bool RTP_EMISSION_FIRST;

	// Token: 0x0400259D RID: 9629
	public bool RTP_EMISSION_ADD;

	// Token: 0x0400259E RID: 9630
	public bool RTP_FUILD_EMISSION_WRAP_FIRST;

	// Token: 0x0400259F RID: 9631
	public bool RTP_FUILD_EMISSION_WRAP_ADD;

	// Token: 0x040025A0 RID: 9632
	public bool RTP_HOTAIR_EMISSION_FIRST;

	// Token: 0x040025A1 RID: 9633
	public bool RTP_HOTAIR_EMISSION_ADD;

	// Token: 0x040025A2 RID: 9634
	public bool RTP_COMPLEMENTARY_LIGHTS;

	// Token: 0x040025A3 RID: 9635
	public bool RTP_SPEC_COMPLEMENTARY_LIGHTS;

	// Token: 0x040025A4 RID: 9636
	public bool RTP_PBL_FRESNEL;

	// Token: 0x040025A5 RID: 9637
	public bool RTP_PBL_VISIBILITY_FUNCTION;

	// Token: 0x040025A6 RID: 9638
	public bool RTP_DEFERRED_PBL_NORMALISATION;

	// Token: 0x040025A7 RID: 9639
	public bool RTP_COLORSPACE_LINEAR;

	// Token: 0x040025A8 RID: 9640
	public bool RTP_SKYSHOP_SYNC;

	// Token: 0x040025A9 RID: 9641
	public bool RTP_SKYSHOP_SKY_ROTATION;

	// Token: 0x040025AA RID: 9642
	public bool RTP_IBL_DIFFUSE_FIRST;

	// Token: 0x040025AB RID: 9643
	public bool RTP_IBL_DIFFUSE_ADD;

	// Token: 0x040025AC RID: 9644
	public bool RTP_IBL_SPEC_FIRST;

	// Token: 0x040025AD RID: 9645
	public bool RTP_IBL_SPEC_ADD;

	// Token: 0x040025AE RID: 9646
	public bool RTP_SHOW_OVERLAPPED;

	// Token: 0x040025AF RID: 9647
	public bool RTP_AMBIENT_EMISSIVE_MAP;

	// Token: 0x040025B0 RID: 9648
	public bool RTP_TRIPLANAR_FIRST;

	// Token: 0x040025B1 RID: 9649
	public bool RTP_TRIPLANAR_ADD;

	// Token: 0x040025B2 RID: 9650
	public bool RTP_NORMALGLOBAL;

	// Token: 0x040025B3 RID: 9651
	public bool RTP_TESSELLATION;

	// Token: 0x040025B4 RID: 9652
	public bool RTP_TESSELLATION_SAMPLE_TEXTURE;

	// Token: 0x040025B5 RID: 9653
	public bool RTP_HEIGHTMAP_SAMPLE_BICUBIC = true;

	// Token: 0x040025B6 RID: 9654
	public bool RTP_DETAIL_HEIGHTMAP_SAMPLE;

	// Token: 0x040025B7 RID: 9655
	public bool RTP_TREESGLOBAL;

	// Token: 0x040025B8 RID: 9656
	public bool RTP_SUPER_SIMPLE;

	// Token: 0x040025B9 RID: 9657
	public bool RTP_SS_GRAYSCALE_DETAIL_COLORS_FIRST;

	// Token: 0x040025BA RID: 9658
	public bool RTP_SS_GRAYSCALE_DETAIL_COLORS_ADD;

	// Token: 0x040025BB RID: 9659
	public bool RTP_USE_BUMPMAPS_FIRST = true;

	// Token: 0x040025BC RID: 9660
	public bool RTP_USE_BUMPMAPS_ADD = true;

	// Token: 0x040025BD RID: 9661
	public bool RTP_USE_PERLIN_FIRST;

	// Token: 0x040025BE RID: 9662
	public bool RTP_USE_PERLIN_ADD;

	// Token: 0x040025BF RID: 9663
	public bool RTP_COLOR_MAP_BLEND_MULTIPLY_FIRST = true;

	// Token: 0x040025C0 RID: 9664
	public bool RTP_COLOR_MAP_BLEND_MULTIPLY_ADD = true;

	// Token: 0x040025C1 RID: 9665
	public bool RTP_SIMPLE_FAR_FIRST = true;

	// Token: 0x040025C2 RID: 9666
	public bool RTP_SIMPLE_FAR_ADD = true;

	// Token: 0x040025C3 RID: 9667
	public bool RTP_CROSSPASS_HEIGHTBLEND;

	// Token: 0x040025C4 RID: 9668
	public int[] UV_BLEND_ROUTE_NUM_FIRST = new int[]
	{
		0,
		1,
		2,
		3,
		4,
		5,
		6,
		7
	};

	// Token: 0x040025C5 RID: 9669
	public int[] UV_BLEND_ROUTE_NUM_ADD = new int[]
	{
		0,
		1,
		2,
		3,
		4,
		5,
		6,
		7
	};

	// Token: 0x040025C6 RID: 9670
	public bool RTP_HARD_CROSSPASS = true;

	// Token: 0x040025C7 RID: 9671
	public bool RTP_MAPPED_SHADOWS_FIRST;

	// Token: 0x040025C8 RID: 9672
	public bool RTP_MAPPED_SHADOWS_ADD;

	// Token: 0x040025C9 RID: 9673
	public bool RTP_VERTICAL_TEXTURE_FIRST;

	// Token: 0x040025CA RID: 9674
	public bool RTP_VERTICAL_TEXTURE_ADD;

	// Token: 0x040025CB RID: 9675
	public bool RTP_ADDITIONAL_FEATURES_IN_FALLBACKS = true;

	// Token: 0x040025CC RID: 9676
	public bool PLATFORM_D3D9 = true;

	// Token: 0x040025CD RID: 9677
	public bool PLATFORM_D3D11 = true;

	// Token: 0x040025CE RID: 9678
	public bool PLATFORM_OPENGL = true;

	// Token: 0x040025CF RID: 9679
	public bool PLATFORM_GLES = true;

	// Token: 0x040025D0 RID: 9680
	public bool PLATFORM_FLASH = true;

	// Token: 0x040025D1 RID: 9681
	public bool RTP_4LAYERS_MODE;

	// Token: 0x040025D2 RID: 9682
	public int numLayers;

	// Token: 0x040025D3 RID: 9683
	public int numLayersProcessedByFarShader = 8;

	// Token: 0x040025D4 RID: 9684
	public bool ADDPASS_IN_BLENDBASE;

	// Token: 0x040025D5 RID: 9685
	public bool RTP_REFLECTION_FIRST;

	// Token: 0x040025D6 RID: 9686
	public bool RTP_REFLECTION_ADD;

	// Token: 0x040025D7 RID: 9687
	public bool RTP_ROTATE_REFLECTION;

	// Token: 0x040025D8 RID: 9688
	public bool RTP_WETNESS_FIRST;

	// Token: 0x040025D9 RID: 9689
	public bool RTP_WETNESS_ADD;

	// Token: 0x040025DA RID: 9690
	public bool RTP_WET_RIPPLE_TEXTURE_FIRST;

	// Token: 0x040025DB RID: 9691
	public bool RTP_WET_RIPPLE_TEXTURE_ADD;

	// Token: 0x040025DC RID: 9692
	public bool RTP_CAUSTICS_FIRST;

	// Token: 0x040025DD RID: 9693
	public bool RTP_CAUSTICS_ADD;

	// Token: 0x040025DE RID: 9694
	public bool RTP_VERTALPHA_CAUSTICS;

	// Token: 0x040025DF RID: 9695
	public bool SIMPLE_WATER_FIRST;

	// Token: 0x040025E0 RID: 9696
	public bool SIMPLE_WATER_ADD;

	// Token: 0x040025E1 RID: 9697
	public bool RTP_USE_EXTRUDE_REDUCTION_FIRST;

	// Token: 0x040025E2 RID: 9698
	public bool RTP_USE_EXTRUDE_REDUCTION_ADD;

	// Token: 0x040025E3 RID: 9699
	public bool SHADER_USAGE_FirstPass;

	// Token: 0x040025E4 RID: 9700
	public bool SHADER_USAGE_AddPass;

	// Token: 0x040025E5 RID: 9701
	public bool SHADER_USAGE_AddPassGeom;

	// Token: 0x040025E6 RID: 9702
	public bool SHADER_USAGE_TerrainFarOnly;

	// Token: 0x040025E7 RID: 9703
	public bool SHADER_USAGE_BlendBase;

	// Token: 0x040025E8 RID: 9704
	public bool SHADER_USAGE_Terrain2Geometry;

	// Token: 0x040025E9 RID: 9705
	public bool SHADER_USAGE_Terrain2GeometryBlendBase;

	// Token: 0x040025EA RID: 9706
	private Shader terrain_shader;

	// Token: 0x040025EB RID: 9707
	private Shader terrain_shader_far;

	// Token: 0x040025EC RID: 9708
	private Shader terrain_shader_add;

	// Token: 0x040025ED RID: 9709
	private Shader terrain2geom_shader;

	// Token: 0x040025EE RID: 9710
	private Shader terrain_geomBlend_shader;

	// Token: 0x040025EF RID: 9711
	private Shader terrain2geom_geomBlend_shader;

	// Token: 0x040025F0 RID: 9712
	private Shader terrain_geomBlend_GeometryBlend_BumpedDetailSnow;

	// Token: 0x040025F1 RID: 9713
	private Shader geomblend_GeometryBlend_WaterShader_2VertexPaint_HB;

	// Token: 0x040025F2 RID: 9714
	private Shader geomBlend_GeometryBlend_WaterShader_FlowMap_HB;

	// Token: 0x040025F3 RID: 9715
	private Shader terrain_geomBlendActual_shader;

	// Token: 0x040025F4 RID: 9716
	public bool dont_sync;
}
