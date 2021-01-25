using System;
using UnityEngine;

// Token: 0x020005C8 RID: 1480
public class MyGuiV2 : MonoBehaviour
{
	// Token: 0x060024CA RID: 9418 RVA: 0x0011F094 File Offset: 0x0011D294
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

	// Token: 0x060024CB RID: 9419 RVA: 0x0011F14C File Offset: 0x0011D34C
	private void InstanceEffect(Vector3 pos)
	{
		this.currentGo = (Object.Instantiate(this.Prefabs[this.current], pos, this.Prefabs[this.current].transform.rotation) as GameObject);
		this.effectSettings = this.currentGo.GetComponent<EffectSettings>();
		this.effectSettings.Target = this.GetTargetObject(this.GuiStats[this.current]);
		if (this.isHomingMove)
		{
			this.effectSettings.IsHomingMove = this.isHomingMove;
		}
		this.prefabSpeed = this.effectSettings.MoveSpeed;
		this.effectSettings.EffectDeactivated += new EventHandler(this.effectSettings_EffectDeactivated);
		this.currentGo.transform.parent = base.transform;
	}

	// Token: 0x060024CC RID: 9420 RVA: 0x0011F218 File Offset: 0x0011D418
	private void InstanceEffectWithoutObjectPool()
	{
		this.currentGo = (Object.Instantiate(this.Prefabs[this.current], this.GetInstancePosition(this.GuiStats[this.current]), this.Prefabs[this.current].transform.rotation) as GameObject);
		this.effectSettings = this.currentGo.GetComponent<EffectSettings>();
		this.effectSettings.Target = this.GetTargetObject(this.GuiStats[this.current]);
		if (this.isHomingMove)
		{
			this.effectSettings.IsHomingMove = this.isHomingMove;
		}
		this.prefabSpeed = this.effectSettings.MoveSpeed;
		this.effectSettings.EffectDeactivated += new EventHandler(this.effectSettings_EffectDeactivated);
		this.currentGo.transform.parent = base.transform;
	}

	// Token: 0x060024CD RID: 9421 RVA: 0x0011F2F8 File Offset: 0x0011D4F8
	private GameObject GetTargetObject(MyGuiV2.GuiStat stat)
	{
		switch (stat)
		{
		case MyGuiV2.GuiStat.Ball:
			return this.Target;
		case MyGuiV2.GuiStat.Bottom:
			return this.BottomPosition;
		case MyGuiV2.GuiStat.Middle:
			return this.MiddlePosition;
		case MyGuiV2.GuiStat.Top:
			return this.TopPosition;
		default:
			return base.gameObject;
		}
	}

	// Token: 0x060024CE RID: 9422 RVA: 0x0011F344 File Offset: 0x0011D544
	private void InstanceDefaulBall()
	{
		this.defaultBall = (Object.Instantiate(this.Prefabs[1], base.transform.position, this.Prefabs[1].transform.rotation) as GameObject);
		this.defaultBallEffectSettings = this.defaultBall.GetComponent<EffectSettings>();
		this.defaultBallEffectSettings.Target = this.Target;
		this.defaultBallEffectSettings.EffectDeactivated += new EventHandler(this.defaultBall_EffectDeactivated);
		this.defaultBall.transform.parent = base.transform;
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x0001867D File Offset: 0x0001687D
	private void defaultBall_EffectDeactivated(object sender, EventArgs e)
	{
		this.defaultBall.transform.position = base.transform.position;
		this.isReadyDefaulBall = true;
	}

	// Token: 0x060024D0 RID: 9424 RVA: 0x0011F3D8 File Offset: 0x0011D5D8
	private void effectSettings_EffectDeactivated(object sender, EventArgs e)
	{
		if (this.current == 15 || this.current == 16)
		{
			Object.Destroy(this.effectSettings.gameObject);
			this.InstanceEffect(this.GetInstancePosition(this.GuiStats[this.current]));
		}
		else
		{
			this.currentGo.transform.position = this.GetInstancePosition(this.GuiStats[this.current]);
			this.isReadyEffect = true;
		}
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x0011F458 File Offset: 0x0011D658
	private void OnGUI()
	{
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
		if (this.current <= 40)
		{
			GUI.Label(new Rect(10f, 152f, 225f, 30f), "Ball Speed " + (int)this.prefabSpeed + "m", this.guiStyleHeader);
			this.prefabSpeed = GUI.HorizontalSlider(new Rect(115f, 155f, 120f, 30f), this.prefabSpeed, 1f, 30f);
			this.isHomingMove = GUI.Toggle(new Rect(10f, 190f, 150f, 30f), this.isHomingMove, " Is Homing Move");
			this.effectSettings.MoveSpeed = this.prefabSpeed;
		}
		GUI.Label(new Rect(1f, 1f, 30f, 30f), string.Empty + (int)this.fps, this.guiStyleHeader);
	}

	// Token: 0x060024D2 RID: 9426 RVA: 0x0011F750 File Offset: 0x0011D950
	private void Update()
	{
		this.anim.enabled = this.isHomingMove;
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

	// Token: 0x060024D3 RID: 9427 RVA: 0x0011F82C File Offset: 0x0011DA2C
	private void InstanceCurrent(MyGuiV2.GuiStat stat)
	{
		switch (stat)
		{
		case MyGuiV2.GuiStat.Ball:
			this.InstanceEffect(base.transform.position);
			break;
		case MyGuiV2.GuiStat.Bottom:
			this.InstanceEffect(this.BottomPosition.transform.position);
			break;
		case MyGuiV2.GuiStat.Middle:
			this.MiddlePosition.SetActive(true);
			this.InstanceEffect(this.MiddlePosition.transform.position);
			break;
		case MyGuiV2.GuiStat.Top:
			this.InstanceEffect(this.TopPosition.transform.position);
			break;
		}
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x0011F8CC File Offset: 0x0011DACC
	private Vector3 GetInstancePosition(MyGuiV2.GuiStat stat)
	{
		switch (stat)
		{
		case MyGuiV2.GuiStat.Ball:
			return base.transform.position;
		case MyGuiV2.GuiStat.Bottom:
			return this.BottomPosition.transform.position;
		case MyGuiV2.GuiStat.Middle:
			return this.MiddlePosition.transform.position;
		case MyGuiV2.GuiStat.Top:
			return this.TopPosition.transform.position;
		default:
			return base.transform.position;
		}
	}

	// Token: 0x060024D5 RID: 9429 RVA: 0x0011F940 File Offset: 0x0011DB40
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
		this.MiddlePosition.SetActive(this.GuiStats[this.current] == MyGuiV2.GuiStat.Middle);
		this.InstanceEffect(this.GetInstancePosition(this.GuiStats[this.current]));
	}

	// Token: 0x04002CB7 RID: 11447
	public int CurrentPrefabNomber;

	// Token: 0x04002CB8 RID: 11448
	public float UpdateInterval = 0.5f;

	// Token: 0x04002CB9 RID: 11449
	public Light DirLight;

	// Token: 0x04002CBA RID: 11450
	public GameObject Target;

	// Token: 0x04002CBB RID: 11451
	public GameObject TopPosition;

	// Token: 0x04002CBC RID: 11452
	public GameObject MiddlePosition;

	// Token: 0x04002CBD RID: 11453
	public GameObject BottomPosition;

	// Token: 0x04002CBE RID: 11454
	public GameObject Plane1;

	// Token: 0x04002CBF RID: 11455
	public GameObject Plane2;

	// Token: 0x04002CC0 RID: 11456
	public Material[] PlaneMaterials;

	// Token: 0x04002CC1 RID: 11457
	public MyGuiV2.GuiStat[] GuiStats;

	// Token: 0x04002CC2 RID: 11458
	public GameObject[] Prefabs;

	// Token: 0x04002CC3 RID: 11459
	private float oldLightIntensity;

	// Token: 0x04002CC4 RID: 11460
	private Color oldAmbientColor;

	// Token: 0x04002CC5 RID: 11461
	private GameObject currentGo;

	// Token: 0x04002CC6 RID: 11462
	private GameObject defaultBall;

	// Token: 0x04002CC7 RID: 11463
	private bool isDay;

	// Token: 0x04002CC8 RID: 11464
	private bool isHomingMove;

	// Token: 0x04002CC9 RID: 11465
	private bool isDefaultPlaneTexture;

	// Token: 0x04002CCA RID: 11466
	private int current;

	// Token: 0x04002CCB RID: 11467
	private Animator anim;

	// Token: 0x04002CCC RID: 11468
	private float prefabSpeed = 4f;

	// Token: 0x04002CCD RID: 11469
	private EffectSettings effectSettings;

	// Token: 0x04002CCE RID: 11470
	private EffectSettings defaultBallEffectSettings;

	// Token: 0x04002CCF RID: 11471
	private bool isReadyEffect;

	// Token: 0x04002CD0 RID: 11472
	private bool isReadyDefaulBall;

	// Token: 0x04002CD1 RID: 11473
	private float accum;

	// Token: 0x04002CD2 RID: 11474
	private int frames;

	// Token: 0x04002CD3 RID: 11475
	private float timeleft;

	// Token: 0x04002CD4 RID: 11476
	private float fps;

	// Token: 0x04002CD5 RID: 11477
	private GUIStyle guiStyleHeader = new GUIStyle();

	// Token: 0x020005C9 RID: 1481
	public enum GuiStat
	{
		// Token: 0x04002CD7 RID: 11479
		Ball,
		// Token: 0x04002CD8 RID: 11480
		Bottom,
		// Token: 0x04002CD9 RID: 11481
		Middle,
		// Token: 0x04002CDA RID: 11482
		Top
	}
}
