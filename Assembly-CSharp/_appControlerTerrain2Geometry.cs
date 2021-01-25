﻿using System;
using UnityEngine;

// Token: 0x02000527 RID: 1319
public class _appControlerTerrain2Geometry : MonoBehaviour
{
	// Token: 0x060021C3 RID: 8643 RVA: 0x00016A2B File Offset: 0x00014C2B
	private void Awake()
	{
		this.GetLODManager();
		this.panel_enabled = true;
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x000FF7DC File Offset: 0x000FD9DC
	private void Update()
	{
		if (Input.GetKeyDown(112))
		{
			this.panel_enabled = !this.panel_enabled;
		}
		if (Input.GetKey(46))
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Min(base.transform.localPosition.y + 0.5f, 50f), base.transform.localPosition.z);
		}
		if (Input.GetKey(44))
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Max(base.transform.localPosition.y - 0.5f, 0.9f), base.transform.localPosition.z);
		}
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x000FF8D0 File Offset: 0x000FDAD0
	private void GetLODManager()
	{
		GameObject gameObject = GameObject.Find("_RTP_LODmanager");
		if (gameObject == null)
		{
			return;
		}
		this.LODmanager = (RTP_LODmanager)gameObject.GetComponent(typeof(RTP_LODmanager));
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x000FF910 File Offset: 0x000FDB10
	private void OnGUI()
	{
		if (!this.LODmanager)
		{
			this.GetLODManager();
			return;
		}
		GUILayout.Space(10f);
		GUILayout.BeginVertical("box", new GUILayoutOption[0]);
		GUILayout.Label(string.Empty + FPSmeter.fps, new GUILayoutOption[0]);
		if (this.panel_enabled)
		{
			this.shadows = GUILayout.Toggle(this.shadows, "disable Unity's shadows", new GUILayoutOption[0]);
			Light component = GameObject.Find("Directional light").GetComponent<Light>();
			component.shadows = ((!this.shadows) ? 2 : 0);
			this.forward_path = GUILayout.Toggle(this.forward_path, "forward rendering", new GUILayoutOption[0]);
			Camera component2 = GameObject.Find("Main Camera").GetComponent<Camera>();
			component2.renderingPath = ((!this.forward_path) ? 2 : 1);
			if (this.forward_path)
			{
				RenderSettings.ambientLight = new Color32(25, 25, 25, 0);
			}
			else
			{
				RenderSettings.ambientLight = new Color32(93, 103, 122, 0);
			}
			TerrainShaderLod rtp_LODlevel = this.LODmanager.RTP_LODlevel;
			TerrainShaderLod terrainShaderLod = rtp_LODlevel;
			switch (rtp_LODlevel)
			{
			case TerrainShaderLod.POM:
				if (GUILayout.Button("POM shading", new GUILayoutOption[0]))
				{
					terrainShaderLod = TerrainShaderLod.PM;
				}
				break;
			case TerrainShaderLod.PM:
				if (GUILayout.Button("PM shading", new GUILayoutOption[0]))
				{
					terrainShaderLod = TerrainShaderLod.SIMPLE;
				}
				break;
			case TerrainShaderLod.SIMPLE:
				if (GUILayout.Button("SIMPLE shading", new GUILayoutOption[0]))
				{
					terrainShaderLod = TerrainShaderLod.CLASSIC;
				}
				break;
			case TerrainShaderLod.CLASSIC:
				if (GUILayout.Button("CLASSIC shading", new GUILayoutOption[0]))
				{
					terrainShaderLod = TerrainShaderLod.POM;
				}
				break;
			}
			switch (terrainShaderLod)
			{
			case TerrainShaderLod.POM:
				if (terrainShaderLod != rtp_LODlevel)
				{
					GameObject gameObject = GameObject.Find("terrainMesh");
					ReliefTerrain reliefTerrain = gameObject.GetComponent(typeof(ReliefTerrain)) as ReliefTerrain;
					reliefTerrain.globalSettingsHolder.Refresh(null, null);
					this.LODmanager.RTP_LODlevel = TerrainShaderLod.POM;
					this.LODmanager.RefreshLODlevel();
				}
				break;
			case TerrainShaderLod.PM:
				if (terrainShaderLod != rtp_LODlevel)
				{
					GameObject gameObject2 = GameObject.Find("terrainMesh");
					ReliefTerrain reliefTerrain2 = gameObject2.GetComponent(typeof(ReliefTerrain)) as ReliefTerrain;
					reliefTerrain2.globalSettingsHolder.Refresh(null, null);
					this.LODmanager.RTP_LODlevel = TerrainShaderLod.PM;
					this.LODmanager.RefreshLODlevel();
				}
				break;
			case TerrainShaderLod.SIMPLE:
				if (terrainShaderLod != rtp_LODlevel)
				{
					GameObject gameObject3 = GameObject.Find("terrainMesh");
					ReliefTerrain reliefTerrain3 = gameObject3.GetComponent(typeof(ReliefTerrain)) as ReliefTerrain;
					reliefTerrain3.globalSettingsHolder.Refresh(null, null);
					this.LODmanager.RTP_LODlevel = TerrainShaderLod.SIMPLE;
					this.LODmanager.RefreshLODlevel();
				}
				break;
			case TerrainShaderLod.CLASSIC:
				if (terrainShaderLod != rtp_LODlevel)
				{
					GameObject gameObject4 = GameObject.Find("terrainMesh");
					ReliefTerrain reliefTerrain4 = gameObject4.GetComponent(typeof(ReliefTerrain)) as ReliefTerrain;
					reliefTerrain4.globalSettingsHolder.Refresh(null, null);
					this.LODmanager.RTP_LODlevel = TerrainShaderLod.CLASSIC;
					this.LODmanager.RefreshLODlevel();
				}
				break;
			}
			if (terrainShaderLod == TerrainShaderLod.POM)
			{
				this.terrain_self_shadow = this.LODmanager.RTP_SHADOWS;
				bool flag = GUILayout.Toggle(this.terrain_self_shadow, "self shadowing", new GUILayoutOption[0]);
				if (flag != this.terrain_self_shadow)
				{
					this.LODmanager.RTP_SHADOWS = flag;
					this.LODmanager.RefreshLODlevel();
				}
				this.terrain_self_shadow = flag;
				if (this.terrain_self_shadow)
				{
					this.terrain_smooth_shadows = this.LODmanager.RTP_SOFT_SHADOWS;
					bool flag2 = GUILayout.Toggle(this.terrain_smooth_shadows, "smooth shadows", new GUILayoutOption[0]);
					if (flag2 != this.terrain_smooth_shadows)
					{
						this.LODmanager.RTP_SOFT_SHADOWS = flag2;
						this.LODmanager.RefreshLODlevel();
					}
					this.terrain_smooth_shadows = flag2;
				}
			}
			if (this.LODmanager.RTP_SNOW_FIRST)
			{
				GameObject gameObject5 = GameObject.Find("terrainMesh");
				ReliefTerrain reliefTerrain5 = gameObject5.GetComponent(typeof(ReliefTerrain)) as ReliefTerrain;
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Label("Snow", new GUILayoutOption[]
				{
					GUILayout.MaxWidth(40f)
				});
				float num = GUILayout.HorizontalSlider(reliefTerrain5.globalSettingsHolder._snow_strength, 0f, 1f, new GUILayoutOption[0]);
				if (num != reliefTerrain5.globalSettingsHolder._snow_strength)
				{
					reliefTerrain5.globalSettingsHolder._snow_strength = num;
					reliefTerrain5.globalSettingsHolder.Refresh(null, null);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.Label("Light", new GUILayoutOption[]
			{
				GUILayout.MaxWidth(40f)
			});
			this.light_dir = GUILayout.HorizontalSlider(this.light_dir, 0f, 360f, new GUILayoutOption[0]);
			component.transform.rotation = Quaternion.Euler(40f, this.light_dir, 0f);
			if (!Application.isWebPlayer && GUILayout.Button("QUIT", new GUILayoutOption[0]))
			{
				Application.Quit();
			}
			GUILayout.Label("  F (hold) - freeze camera", new GUILayoutOption[0]);
			GUILayout.Label("  ,/. - change cam position", new GUILayoutOption[0]);
		}
		else if (!Application.isWebPlayer && GUILayout.Button("QUIT", new GUILayoutOption[0]))
		{
			Application.Quit();
		}
		GUILayout.Label("  P - toggle panel", new GUILayoutOption[0]);
		GUILayout.EndVertical();
	}

	// Token: 0x04002523 RID: 9507
	public bool shadows;

	// Token: 0x04002524 RID: 9508
	public bool forward_path = true;

	// Token: 0x04002525 RID: 9509
	public bool terrain_self_shadow;

	// Token: 0x04002526 RID: 9510
	public bool terrain_smooth_shadows = true;

	// Token: 0x04002527 RID: 9511
	private bool panel_enabled;

	// Token: 0x04002528 RID: 9512
	public float light_dir = 285f;

	// Token: 0x04002529 RID: 9513
	public float preset_param_interp;

	// Token: 0x0400252A RID: 9514
	private RTP_LODmanager LODmanager;
}
