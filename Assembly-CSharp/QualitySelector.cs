using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002C7 RID: 711
public class QualitySelector : MonoBehaviour
{
	// Token: 0x06000E16 RID: 3606 RVA: 0x00074068 File Offset: 0x00072268
	public static void ApplyQuality(QualityLevel level)
	{
		QualitySettings.SetQualityLevel(level);
		for (int i = 0; i < QualitySelector.selector.Count; i++)
		{
			QualitySelector.selector[i].OnQualityChange();
		}
	}

	// Token: 0x06000E17 RID: 3607 RVA: 0x000099C0 File Offset: 0x00007BC0
	private void Awake()
	{
		QualitySelector.selector.Add(this);
	}

	// Token: 0x06000E18 RID: 3608 RVA: 0x000099CD File Offset: 0x00007BCD
	private void Start()
	{
		this.OnQualityChange();
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x000099D5 File Offset: 0x00007BD5
	private void OnDestroy()
	{
		QualitySelector.selector.Remove(this);
	}

	// Token: 0x06000E1A RID: 3610 RVA: 0x000740A8 File Offset: 0x000722A8
	private void OnQualityChange()
	{
		if (this.showBelowLevel)
		{
			if (QualitySettings.GetQualityLevel() < this.level)
			{
				base.gameObject.SetActive(true);
				base.gameObject.transform.ForEach(delegate(Transform t)
				{
					t.gameObject.SetActive(true);
				});
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
		else if (QualitySettings.GetQualityLevel() < this.level)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			base.gameObject.SetActive(true);
			base.gameObject.transform.ForEach(delegate(Transform t)
			{
				t.gameObject.SetActive(true);
			});
		}
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x000099E3 File Offset: 0x00007BE3
	[ContextMenu("Quality Simple")]
	private void SetQualitySimple()
	{
		QualitySelector.ApplyQuality(2);
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x000099EB File Offset: 0x00007BEB
	[ContextMenu("Quality Fast")]
	private void SetQualityFast()
	{
		QualitySelector.ApplyQuality(1);
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x000099F3 File Offset: 0x00007BF3
	[ContextMenu("Quality Good")]
	private void SetQualityGood()
	{
		QualitySelector.ApplyQuality(3);
	}

	// Token: 0x0400105F RID: 4191
	private static List<QualitySelector> selector = new List<QualitySelector>();

	// Token: 0x04001060 RID: 4192
	public QualityLevel level;

	// Token: 0x04001061 RID: 4193
	public bool showBelowLevel;
}
