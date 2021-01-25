using System;
using UnityEngine;

// Token: 0x02000687 RID: 1671
public class NPCcolorChange : MonoBehaviour
{
	// Token: 0x060028BB RID: 10427 RVA: 0x0014186C File Offset: 0x0013FA6C
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		int num = Random.Range(0, 9);
		Color color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		switch (num)
		{
		case 0:
			color = new Color32(139, 79, 148, byte.MaxValue);
			component.material.SetColor("_ColorChange", color);
			if (this.headColor)
			{
				this.headColor.renderer.material.SetColor("_ColorChange", color);
			}
			break;
		case 1:
			color = new Color32(148, 79, 79, byte.MaxValue);
			component.material.SetColor("_ColorChange", color);
			if (this.headColor)
			{
				this.headColor.renderer.material.SetColor("_ColorChange", color);
			}
			break;
		case 2:
			color = new Color32(80, 72, 72, byte.MaxValue);
			component.material.SetColor("_ColorChange", color);
			if (this.headColor)
			{
				this.headColor.renderer.material.SetColor("_ColorChange", color);
			}
			break;
		case 3:
			color = new Color32(84, 88, 155, byte.MaxValue);
			component.material.SetColor("_ColorChange", color);
			if (this.headColor)
			{
				this.headColor.renderer.material.SetColor("_ColorChange", color);
			}
			break;
		case 4:
			color = new Color32(199, 181, 111, byte.MaxValue);
			component.material.SetColor("_ColorChange", color);
			if (this.headColor)
			{
				this.headColor.renderer.material.SetColor("_ColorChange", color);
			}
			break;
		case 5:
			color = new Color32(95, 156, 82, byte.MaxValue);
			component.material.SetColor("_ColorChange", color);
			if (this.headColor)
			{
				this.headColor.renderer.material.SetColor("_ColorChange", color);
			}
			break;
		case 6:
			component.material.SetColor("_ColorChange", color);
			if (this.headColor)
			{
				this.headColor.renderer.material.SetColor("_ColorChange", color);
			}
			break;
		case 7:
			color = new Color32(82, 156, 155, byte.MaxValue);
			component.material.SetColor("_ColorChange", color);
			if (this.headColor)
			{
				this.headColor.renderer.material.SetColor("_ColorChange", color);
			}
			break;
		case 8:
			color = new Color32(210, 32, 32, byte.MaxValue);
			component.material.SetColor("_ColorChange", color);
			if (this.headColor)
			{
				this.headColor.renderer.material.SetColor("_ColorChange", color);
			}
			break;
		default:
			component.material.SetColor("_ColorChange", color);
			if (this.headColor)
			{
				this.headColor.renderer.material.SetColor("_ColorChange", color);
			}
			break;
		}
	}

	// Token: 0x0400330C RID: 13068
	public Transform headColor;
}
