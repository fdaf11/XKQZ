using System;
using UnityEngine;

// Token: 0x02000547 RID: 1351
public class visibility : MonoBehaviour
{
	// Token: 0x06002244 RID: 8772 RVA: 0x00016E14 File Offset: 0x00015014
	private void Start()
	{
		visibility.rate = base.particleSystem.emissionRate;
	}

	// Token: 0x06002245 RID: 8773 RVA: 0x0010C5D8 File Offset: 0x0010A7D8
	private void OnGUI()
	{
		if (base.name == "cloud1" && GUI.Button(new Rect(30f, 30f, 20f, 20f), visibility.act1))
		{
			base.renderer.enabled = true;
			visibility.act1 = "x";
			visibility.act2 = "2";
			visibility.act3 = "3";
			visibility.act4 = "4";
			visibility.act5 = "5";
			visibility.act6 = "6";
		}
		if (base.name == "cloud2" && GUI.Button(new Rect(60f, 30f, 20f, 20f), visibility.act2))
		{
			base.renderer.enabled = true;
			visibility.act1 = "1";
			visibility.act2 = "x";
			visibility.act3 = "3";
			visibility.act4 = "4";
			visibility.act5 = "5";
			visibility.act6 = "6";
		}
		if (base.name == "cloud3" && GUI.Button(new Rect(90f, 30f, 20f, 20f), visibility.act3))
		{
			base.renderer.enabled = true;
			visibility.act1 = "1";
			visibility.act2 = "2";
			visibility.act3 = "x";
			visibility.act4 = "4";
			visibility.act5 = "5";
			visibility.act6 = "6";
		}
		if (base.name == "cloud4" && GUI.Button(new Rect(120f, 30f, 20f, 20f), visibility.act4))
		{
			base.renderer.enabled = true;
			visibility.act1 = "1";
			visibility.act2 = "2";
			visibility.act3 = "3";
			visibility.act4 = "x";
			visibility.act5 = "5";
			visibility.act6 = "6";
		}
		if (base.name == "cloud5" && GUI.Button(new Rect(150f, 30f, 20f, 20f), visibility.act5))
		{
			base.renderer.enabled = true;
			visibility.act1 = "1";
			visibility.act2 = "2";
			visibility.act3 = "3";
			visibility.act4 = "4";
			visibility.act5 = "x";
			visibility.act6 = "6";
		}
		if (base.name == "cloud6" && GUI.Button(new Rect(180f, 30f, 20f, 20f), visibility.act6))
		{
			base.renderer.enabled = true;
			visibility.act1 = "1";
			visibility.act2 = "2";
			visibility.act3 = "3";
			visibility.act4 = "4";
			visibility.act5 = "5";
			visibility.act6 = "x";
		}
	}

	// Token: 0x06002246 RID: 8774 RVA: 0x0010C904 File Offset: 0x0010AB04
	private void Update()
	{
		if (this.tr != transparency.density)
		{
			this.tr = transparency.density;
			base.particleSystem.emissionRate = visibility.rate * this.tr;
			MonoBehaviour.print(this.tr);
		}
		if (this.drk != transparency.darkness)
		{
			this.drk = transparency.darkness;
			base.renderer.material.SetColor("_TintColor", new Color(1f - this.drk, 1f - this.drk, 1f - this.drk, base.renderer.material.GetColor("_TintColor").a));
			MonoBehaviour.print(this.tr);
		}
		if (base.name == "cloud1" && base.renderer.enabled && visibility.act1 == "1")
		{
			base.renderer.enabled = false;
		}
		if (base.name == "cloud2" && base.renderer.enabled && visibility.act2 == "2")
		{
			base.renderer.enabled = false;
		}
		if (base.name == "cloud3" && base.renderer.enabled && visibility.act3 == "3")
		{
			base.renderer.enabled = false;
		}
		if (base.name == "cloud4" && base.renderer.enabled && visibility.act4 == "4")
		{
			base.renderer.enabled = false;
		}
		if (base.name == "cloud5" && base.renderer.enabled && visibility.act5 == "5")
		{
			base.renderer.enabled = false;
		}
		if (base.name == "cloud6" && base.renderer.enabled && visibility.act6 == "6")
		{
			base.renderer.enabled = false;
		}
	}

	// Token: 0x0400286D RID: 10349
	public static string act1 = "1";

	// Token: 0x0400286E RID: 10350
	public static string act2 = "2";

	// Token: 0x0400286F RID: 10351
	public static string act3 = "3";

	// Token: 0x04002870 RID: 10352
	public static string act4 = "4";

	// Token: 0x04002871 RID: 10353
	public static string act5 = "x";

	// Token: 0x04002872 RID: 10354
	public static string act6 = "6";

	// Token: 0x04002873 RID: 10355
	public static float rate;

	// Token: 0x04002874 RID: 10356
	private float drk;

	// Token: 0x04002875 RID: 10357
	private float tr = 1f;
}
