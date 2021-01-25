using System;
using UnityEngine;

// Token: 0x020005C6 RID: 1478
public class MyGui : MonoBehaviour
{
	// Token: 0x060024BF RID: 9407 RVA: 0x0011E864 File Offset: 0x0011CA64
	private void Start()
	{
		this.oldAmbientColor = RenderSettings.ambientLight;
		this.oldLightIntensity = this.DirLight.intensity;
		this.anim = this.Target.GetComponent<Animator>();
		this.guiStyleHeader.fontSize = 14;
		this.guiStyleHeader.normal.textColor = new Color(1f, 1f, 1f);
		EffectSettings component = this.Prefabs[this.current].GetComponent<EffectSettings>();
		if (component != null)
		{
			this.prefabSpeed = component.MoveSpeed;
		}
		this.current = this.CurrentPrefabNomber;
		this.InstanceCurrent(this.GuiStats[this.CurrentPrefabNomber]);
	}

	// Token: 0x060024C0 RID: 9408 RVA: 0x0011E91C File Offset: 0x0011CB1C
	private void InstanceEffect(Vector3 pos)
	{
		this.currentGo = (Object.Instantiate(this.Prefabs[this.current], pos, this.Prefabs[this.current].transform.rotation) as GameObject);
		this.effectSettings = this.currentGo.GetComponent<EffectSettings>();
		this.effectSettings.Target = this.Target;
		if (this.isHomingMove)
		{
			this.effectSettings.IsHomingMove = this.isHomingMove;
		}
		this.prefabSpeed = this.effectSettings.MoveSpeed;
		this.effectSettings.EffectDeactivated += new EventHandler(this.effectSettings_EffectDeactivated);
		this.currentGo.transform.parent = base.transform;
	}

	// Token: 0x060024C1 RID: 9409 RVA: 0x0011E9DC File Offset: 0x0011CBDC
	private void InstanceDefaulBall()
	{
		this.defaultBall = (Object.Instantiate(this.Prefabs[1], base.transform.position, this.Prefabs[1].transform.rotation) as GameObject);
		this.defaultBallEffectSettings = this.defaultBall.GetComponent<EffectSettings>();
		this.defaultBallEffectSettings.Target = this.Target;
		this.defaultBallEffectSettings.EffectDeactivated += new EventHandler(this.defaultBall_EffectDeactivated);
		this.defaultBall.transform.parent = base.transform;
	}

	// Token: 0x060024C2 RID: 9410 RVA: 0x00018604 File Offset: 0x00016804
	private void defaultBall_EffectDeactivated(object sender, EventArgs e)
	{
		this.defaultBall.transform.position = base.transform.position;
		this.isReadyDefaulBall = true;
	}

	// Token: 0x060024C3 RID: 9411 RVA: 0x00018628 File Offset: 0x00016828
	private void effectSettings_EffectDeactivated(object sender, EventArgs e)
	{
		this.currentGo.transform.position = this.GetInstancePosition(this.GuiStats[this.current]);
		this.isReadyEffect = true;
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x0011EA70 File Offset: 0x0011CC70
	private void OnGUI()
	{
		if (!this.UseGui)
		{
			return;
		}
		if (GUI.Button(new Rect(10f, 15f, 105f, 30f), "Previous Effect"))
		{
			this.ChangeCurrent(-1);
		}
		if (GUI.Button(new Rect(130f, 15f, 105f, 30f), "Next Effect"))
		{
			this.ChangeCurrent(1);
		}
		if (this.Prefabs[this.current] != null)
		{
			GUI.Label(new Rect(300f, 15f, 100f, 20f), "Prefab name is \"" + this.Prefabs[this.current].name + "\"  \r\nHold any mouse button that would move the camera", this.guiStyleHeader);
		}
		if (GUI.Button(new Rect(10f, 60f, 225f, 30f), "Day/Night"))
		{
			this.DirLight.intensity = (this.isDay ? this.oldLightIntensity : 0f);
			RenderSettings.ambientLight = (this.isDay ? this.oldAmbientColor : new Color(0.1f, 0.1f, 0.1f));
			this.isDay = !this.isDay;
		}
		if (GUI.Button(new Rect(10f, 105f, 225f, 30f), "Change environment"))
		{
			if (this.isDefaultPlaneTexture)
			{
				this.Plane1.renderer.material = this.PlaneMaterials[0];
				this.Plane2.renderer.material = this.PlaneMaterials[0];
			}
			else
			{
				this.Plane1.renderer.material = this.PlaneMaterials[1];
				this.Plane2.renderer.material = this.PlaneMaterials[2];
			}
			this.isDefaultPlaneTexture = !this.isDefaultPlaneTexture;
		}
		if (this.current <= 15)
		{
			GUI.Label(new Rect(10f, 152f, 225f, 30f), "Ball Speed " + (int)this.prefabSpeed + "m", this.guiStyleHeader);
			this.prefabSpeed = GUI.HorizontalSlider(new Rect(115f, 155f, 120f, 30f), this.prefabSpeed, 1f, 30f);
			this.isHomingMove = GUI.Toggle(new Rect(10f, 190f, 150f, 30f), this.isHomingMove, " Is Homing Move");
			this.effectSettings.MoveSpeed = this.prefabSpeed;
		}
		GUI.Label(new Rect(1f, 1f, 30f, 30f), string.Empty + (int)this.fps, this.guiStyleHeader);
	}

	// Token: 0x060024C5 RID: 9413 RVA: 0x0011ED74 File Offset: 0x0011CF74
	private void Update()
	{
		this.anim.enabled = this.isHomingMove;
		this.effectSettings.IsHomingMove = this.isHomingMove;
		this.timeleft -= Time.deltaTime;
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			this.fps = this.accum / (float)this.frames;
			this.timeleft = this.UpdateInterval;
			this.accum = 0f;
			this.frames = 0;
		}
		if (this.isReadyEffect)
		{
			this.isReadyEffect = false;
			this.currentGo.SetActive(true);
		}
		if (this.isReadyDefaulBall)
		{
			this.isReadyDefaulBall = false;
			this.defaultBall.SetActive(true);
		}
	}

	// Token: 0x060024C6 RID: 9414 RVA: 0x0011EE60 File Offset: 0x0011D060
	private void InstanceCurrent(MyGui.GuiStat stat)
	{
		switch (stat)
		{
		case MyGui.GuiStat.Ball:
			this.InstanceEffect(base.transform.position);
			break;
		case MyGui.GuiStat.Bottom:
			this.InstanceEffect(this.BottomPosition.transform.position);
			break;
		case MyGui.GuiStat.Middle:
			this.MiddlePosition.SetActive(true);
			this.InstanceEffect(this.MiddlePosition.transform.position);
			break;
		case MyGui.GuiStat.Top:
			this.InstanceEffect(this.TopPosition.transform.position);
			break;
		}
	}

	// Token: 0x060024C7 RID: 9415 RVA: 0x0011EF00 File Offset: 0x0011D100
	private Vector3 GetInstancePosition(MyGui.GuiStat stat)
	{
		switch (stat)
		{
		case MyGui.GuiStat.Ball:
			return base.transform.position;
		case MyGui.GuiStat.Bottom:
			return this.BottomPosition.transform.position;
		case MyGui.GuiStat.Middle:
			return this.MiddlePosition.transform.position;
		case MyGui.GuiStat.Top:
			return this.TopPosition.transform.position;
		default:
			return base.transform.position;
		}
	}

	// Token: 0x060024C8 RID: 9416 RVA: 0x0011EF74 File Offset: 0x0011D174
	private void ChangeCurrent(int delta)
	{
		Object.Destroy(this.currentGo);
		Object.Destroy(this.defaultBall);
		base.CancelInvoke("InstanceDefaulBall");
		this.current += delta;
		if (this.current > this.Prefabs.Length - 1)
		{
			this.current = 0;
		}
		else if (this.current < 0)
		{
			this.current = this.Prefabs.Length - 1;
		}
		if (this.effectSettings != null)
		{
			this.effectSettings.EffectDeactivated -= new EventHandler(this.effectSettings_EffectDeactivated);
		}
		if (this.defaultBallEffectSettings != null)
		{
			this.defaultBallEffectSettings.EffectDeactivated -= new EventHandler(this.effectSettings_EffectDeactivated);
		}
		this.MiddlePosition.SetActive(this.GuiStats[this.current] == MyGui.GuiStat.Middle);
		if (this.GuiStats[this.current] == MyGui.GuiStat.Middle)
		{
			base.Invoke("InstanceDefaulBall", 2f);
		}
		this.InstanceEffect(this.GetInstancePosition(this.GuiStats[this.current]));
	}

	// Token: 0x04002C91 RID: 11409
	public bool UseGui = true;

	// Token: 0x04002C92 RID: 11410
	public int CurrentPrefabNomber;

	// Token: 0x04002C93 RID: 11411
	public float UpdateInterval = 0.5f;

	// Token: 0x04002C94 RID: 11412
	public Light DirLight;

	// Token: 0x04002C95 RID: 11413
	public GameObject Target;

	// Token: 0x04002C96 RID: 11414
	public GameObject TopPosition;

	// Token: 0x04002C97 RID: 11415
	public GameObject MiddlePosition;

	// Token: 0x04002C98 RID: 11416
	public GameObject BottomPosition;

	// Token: 0x04002C99 RID: 11417
	public GameObject Plane1;

	// Token: 0x04002C9A RID: 11418
	public GameObject Plane2;

	// Token: 0x04002C9B RID: 11419
	public Material[] PlaneMaterials;

	// Token: 0x04002C9C RID: 11420
	public MyGui.GuiStat[] GuiStats;

	// Token: 0x04002C9D RID: 11421
	public float[] Times;

	// Token: 0x04002C9E RID: 11422
	public GameObject[] Prefabs;

	// Token: 0x04002C9F RID: 11423
	private float oldLightIntensity;

	// Token: 0x04002CA0 RID: 11424
	private Color oldAmbientColor;

	// Token: 0x04002CA1 RID: 11425
	private GameObject currentGo;

	// Token: 0x04002CA2 RID: 11426
	private GameObject defaultBall;

	// Token: 0x04002CA3 RID: 11427
	private bool isDay;

	// Token: 0x04002CA4 RID: 11428
	private bool isHomingMove;

	// Token: 0x04002CA5 RID: 11429
	private bool isDefaultPlaneTexture;

	// Token: 0x04002CA6 RID: 11430
	private int current;

	// Token: 0x04002CA7 RID: 11431
	private Animator anim;

	// Token: 0x04002CA8 RID: 11432
	private float prefabSpeed = 4f;

	// Token: 0x04002CA9 RID: 11433
	private EffectSettings effectSettings;

	// Token: 0x04002CAA RID: 11434
	private EffectSettings defaultBallEffectSettings;

	// Token: 0x04002CAB RID: 11435
	private bool isReadyEffect;

	// Token: 0x04002CAC RID: 11436
	private bool isReadyDefaulBall;

	// Token: 0x04002CAD RID: 11437
	private float accum;

	// Token: 0x04002CAE RID: 11438
	private int frames;

	// Token: 0x04002CAF RID: 11439
	private float timeleft;

	// Token: 0x04002CB0 RID: 11440
	private float fps;

	// Token: 0x04002CB1 RID: 11441
	private GUIStyle guiStyleHeader = new GUIStyle();

	// Token: 0x020005C7 RID: 1479
	public enum GuiStat
	{
		// Token: 0x04002CB3 RID: 11443
		Ball,
		// Token: 0x04002CB4 RID: 11444
		Bottom,
		// Token: 0x04002CB5 RID: 11445
		Middle,
		// Token: 0x04002CB6 RID: 11446
		Top
	}
}
