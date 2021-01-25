using System;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class ProjectorTransTexture : MonoBehaviour
{
	// Token: 0x0600099C RID: 2460 RVA: 0x00051B14 File Offset: 0x0004FD14
	private void Awake()
	{
		this.m_Projector = base.gameObject.GetComponent<Projector>();
		this.m_ParentObj = base.transform.parent.gameObject;
		this.m_bStart = true;
		this.m_iCurIndex = 0;
		this.m_TimeCount = 0f;
		this.m_Projector.material.SetTexture("_ShadowTex", this.TexList[this.m_iCurIndex]);
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00007D0A File Offset: 0x00005F0A
	private void OnDisable()
	{
		this.m_bStart = true;
		this.m_iCurIndex = 0;
		this.m_TimeCount = 0f;
		this.m_Projector.material.SetTexture("_ShadowTex", this.TexList[this.m_iCurIndex]);
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x00051B84 File Offset: 0x0004FD84
	private void Update()
	{
		if (!this.m_ParentObj.activeSelf)
		{
			this.m_bStart = true;
			this.m_iCurIndex = 0;
			this.m_TimeCount = 0f;
			this.m_Projector.material.SetTexture("_ShadowTex", this.TexList[this.m_iCurIndex]);
			return;
		}
		if (this.m_bStart)
		{
			this.m_TimeCount += Time.deltaTime;
			if (this.m_TimeCount >= this.ChangeGap)
			{
				this.m_iCurIndex++;
				this.m_TimeCount -= this.ChangeGap;
				if (this.m_iCurIndex >= this.TexList.Length)
				{
					this.m_bStart = false;
					return;
				}
				this.m_Projector.material.SetTexture("_ShadowTex", this.TexList[this.m_iCurIndex]);
			}
		}
	}

	// Token: 0x0400098C RID: 2444
	public Texture[] TexList;

	// Token: 0x0400098D RID: 2445
	public float ChangeGap = 0.2f;

	// Token: 0x0400098E RID: 2446
	private Projector m_Projector;

	// Token: 0x0400098F RID: 2447
	private GameObject m_ParentObj;

	// Token: 0x04000990 RID: 2448
	private bool m_bStart;

	// Token: 0x04000991 RID: 2449
	private int m_iCurIndex;

	// Token: 0x04000992 RID: 2450
	private float m_TimeCount;
}
