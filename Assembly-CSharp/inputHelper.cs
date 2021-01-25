using System;
using UnityEngine;

// Token: 0x020005AF RID: 1455
public class inputHelper : MonoBehaviour
{
	// Token: 0x0600244A RID: 9290 RVA: 0x00018150 File Offset: 0x00016350
	private void Start()
	{
		this.window = new Rect(0f, (float)(Screen.height - 150), (float)Screen.width, 150f);
		this.hayate = this.HayateHolder.GetComponent<Hayate>();
	}

	// Token: 0x0600244B RID: 9291 RVA: 0x0001818A File Offset: 0x0001638A
	private void OnGUI()
	{
		this.window = GUI.Window(0, this.window, new GUI.WindowFunction(this.functionKeys), "Turbulence settings");
	}

	// Token: 0x0600244C RID: 9292 RVA: 0x0011BEA0 File Offset: 0x0011A0A0
	private void functionKeys(int windowID)
	{
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Emitter: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		if (GUILayout.Button("Plane", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.HayateHolder.SetActive(true);
			this.HayateHolder3.SetActive(false);
			this.HayateHolder2.SetActive(false);
			this.hayate = this.HayateHolder.GetComponent<Hayate>();
		}
		if (GUILayout.Button("Sphere", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.HayateHolder.SetActive(false);
			this.HayateHolder3.SetActive(false);
			this.HayateHolder2.SetActive(true);
			this.hayate = this.HayateHolder2.GetComponent<Hayate>();
		}
		if (GUILayout.Button("Line", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.HayateHolder.SetActive(false);
			this.HayateHolder3.SetActive(true);
			this.HayateHolder2.SetActive(false);
			this.hayate = this.HayateHolder3.GetComponent<Hayate>();
		}
		if (GUILayout.Button("Velocity relative", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(120f)
		}))
		{
			this.hayate.UseRelativeOrAbsoluteValues = 0;
			this.hayate.AssignTurbulenceTo = 0;
		}
		if (GUILayout.Button("Velocity absolute", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(120f)
		}))
		{
			this.hayate.UseRelativeOrAbsoluteValues = 1;
			this.hayate.AssignTurbulenceTo = 0;
		}
		if (GUILayout.Button("Position relative", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(120f)
		}))
		{
			this.hayate.UseRelativeOrAbsoluteValues = 0;
			this.hayate.AssignTurbulenceTo = 1;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("X-Axis: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		if (GUILayout.Button("None", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodX = 0;
		}
		if (GUILayout.Button("Sine", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodX = 1;
		}
		if (GUILayout.Button("Cosine", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodX = 2;
		}
		if (GUILayout.Button("Perlin", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodX = 3;
		}
		if (GUILayout.Button("Texture", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodX = 4;
		}
		if (GUILayout.Button("Audio", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodX = 5;
		}
		GUILayout.Label("Amplitude: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.Amplitude.x = float.Parse(GUILayout.TextField(this.hayate.Amplitude.x.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.Label("Frequency: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.Frequency.x = float.Parse(GUILayout.TextField(this.hayate.Frequency.x.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.Label("Offset: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.Offset.x = float.Parse(GUILayout.TextField(this.hayate.Offset.x.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.Label("OffsetSpeed: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.OffsetSpeed.x = float.Parse(GUILayout.TextField(this.hayate.OffsetSpeed.x.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Y-Axis: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		if (GUILayout.Button("None", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodY = 0;
		}
		if (GUILayout.Button("Sine", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodY = 1;
		}
		if (GUILayout.Button("Cosine", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodY = 2;
		}
		if (GUILayout.Button("Perlin", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodY = 3;
		}
		if (GUILayout.Button("Texture", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodY = 4;
		}
		if (GUILayout.Button("Audio", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodY = 5;
		}
		GUILayout.Label("Amplitude: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.Amplitude.y = float.Parse(GUILayout.TextField(this.hayate.Amplitude.y.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.Label("Frequency: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.Frequency.y = float.Parse(GUILayout.TextField(this.hayate.Frequency.y.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.Label("Offset: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.Offset.y = float.Parse(GUILayout.TextField(this.hayate.Offset.y.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.Label("OffsetSpeed: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.OffsetSpeed.y = float.Parse(GUILayout.TextField(this.hayate.OffsetSpeed.y.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Z-Axis: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		if (GUILayout.Button("None", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodZ = 0;
		}
		if (GUILayout.Button("Sine", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodZ = 1;
		}
		if (GUILayout.Button("Cosine", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodZ = 2;
		}
		if (GUILayout.Button("Perlin", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodZ = 3;
		}
		if (GUILayout.Button("Texture", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodZ = 4;
		}
		if (GUILayout.Button("Audio", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		}))
		{
			this.hayate.UseCalculationMethodZ = 5;
		}
		GUILayout.Label("Amplitude: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.Amplitude.z = float.Parse(GUILayout.TextField(this.hayate.Amplitude.z.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.Label("Frequency: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.Frequency.z = float.Parse(GUILayout.TextField(this.hayate.Frequency.z.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.Label("Offset: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.Offset.z = float.Parse(GUILayout.TextField(this.hayate.Offset.z.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.Label("OffsetSpeed: ", new GUILayoutOption[]
		{
			GUILayout.MaxWidth(80f)
		});
		this.hayate.OffsetSpeed.z = float.Parse(GUILayout.TextField(this.hayate.OffsetSpeed.z.ToString(), new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		}));
		GUILayout.EndHorizontal();
	}

	// Token: 0x04002C22 RID: 11298
	public GameObject HayateHolder;

	// Token: 0x04002C23 RID: 11299
	public GameObject HayateHolder2;

	// Token: 0x04002C24 RID: 11300
	public GameObject HayateHolder3;

	// Token: 0x04002C25 RID: 11301
	private Hayate hayate;

	// Token: 0x04002C26 RID: 11302
	private Rect window;
}
