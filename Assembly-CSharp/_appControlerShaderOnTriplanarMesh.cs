using System;
using UnityEngine;

// Token: 0x02000525 RID: 1317
public class _appControlerShaderOnTriplanarMesh : MonoBehaviour
{
	// Token: 0x060021BA RID: 8634 RVA: 0x000169D1 File Offset: 0x00014BD1
	private void Awake()
	{
		this.panel_enabled = true;
	}

	// Token: 0x060021BB RID: 8635 RVA: 0x000FED78 File Offset: 0x000FCF78
	private void Update()
	{
		if (Input.GetKeyDown(112))
		{
			this.panel_enabled = !this.panel_enabled;
		}
		if (Input.GetKey(46))
		{
			MouseOrbitCS mouseOrbitCS = base.GetComponent(typeof(MouseOrbitCS)) as MouseOrbitCS;
			mouseOrbitCS.distance += 1f;
			if (mouseOrbitCS.distance > 150f)
			{
				mouseOrbitCS.distance = 150f;
			}
		}
		if (Input.GetKey(44))
		{
			MouseOrbitCS mouseOrbitCS2 = base.GetComponent(typeof(MouseOrbitCS)) as MouseOrbitCS;
			mouseOrbitCS2.distance -= 1f;
			if (mouseOrbitCS2.distance < 30f)
			{
				mouseOrbitCS2.distance = 30f;
			}
		}
	}

	// Token: 0x060021BC RID: 8636 RVA: 0x000FEE40 File Offset: 0x000FD040
	private void OnGUI()
	{
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
			GUILayout.Label("Light", new GUILayoutOption[]
			{
				GUILayout.MaxWidth(40f)
			});
			this.light_dir = GUILayout.HorizontalSlider(this.light_dir, 0f, 360f, new GUILayoutOption[0]);
			component.transform.rotation = Quaternion.Euler(this.light_dir, this.light_dir, this.light_dir);
			Light component3 = GameObject.Find("Directional light2").GetComponent<Light>();
			component3.transform.rotation = Quaternion.Euler(-this.light_dir, -this.light_dir, -this.light_dir);
			GUILayout.Label("Model orientation (snow)", new GUILayoutOption[]
			{
				GUILayout.MaxWidth(170f)
			});
			Transform transform = GameObject.Find("WeirdOne").transform;
			this.model_dir = GUILayout.HorizontalSlider(this.model_dir, 0f, 180f, new GUILayoutOption[0]);
			transform.rotation = Quaternion.Euler(this.model_dir, this.model_dir * 0.7f, -this.model_dir * 0.1f);
			if (!Application.isWebPlayer && GUILayout.Button("QUIT", new GUILayoutOption[0]))
			{
				Application.Quit();
			}
			GUILayout.Label("  F (hold) - freeze camera", new GUILayoutOption[0]);
			GUILayout.Label("  ,/. - zoom camera", new GUILayoutOption[0]);
		}
		else if (!Application.isWebPlayer && GUILayout.Button("QUIT", new GUILayoutOption[0]))
		{
			Application.Quit();
		}
		GUILayout.Label("  P - toggle panel", new GUILayoutOption[0]);
		GUILayout.EndVertical();
	}

	// Token: 0x04002516 RID: 9494
	public bool shadows;

	// Token: 0x04002517 RID: 9495
	public bool forward_path = true;

	// Token: 0x04002518 RID: 9496
	private bool panel_enabled;

	// Token: 0x04002519 RID: 9497
	public float light_dir;

	// Token: 0x0400251A RID: 9498
	public float model_dir;
}
