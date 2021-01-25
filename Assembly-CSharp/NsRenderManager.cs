using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003BC RID: 956
public class NsRenderManager : MonoBehaviour
{
	// Token: 0x060016B8 RID: 5816 RVA: 0x0000264F File Offset: 0x0000084F
	private void Awake()
	{
	}

	// Token: 0x060016B9 RID: 5817 RVA: 0x0000264F File Offset: 0x0000084F
	private void OnEnable()
	{
	}

	// Token: 0x060016BA RID: 5818 RVA: 0x0000264F File Offset: 0x0000084F
	private void OnDisable()
	{
	}

	// Token: 0x060016BB RID: 5819 RVA: 0x0000264F File Offset: 0x0000084F
	private void Start()
	{
	}

	// Token: 0x060016BC RID: 5820 RVA: 0x000BC8E0 File Offset: 0x000BAAE0
	private void OnPreRender()
	{
		if (this.m_RenderEventCalls != null)
		{
			int num = this.m_RenderEventCalls.Count - 1;
			while (0 <= num)
			{
				if (this.m_RenderEventCalls[num] == null)
				{
					this.m_RenderEventCalls.RemoveAt(num);
				}
				else
				{
					this.m_RenderEventCalls[num].SendMessage("OnPreRender", 1);
				}
				num--;
			}
		}
	}

	// Token: 0x060016BD RID: 5821 RVA: 0x0000264F File Offset: 0x0000084F
	private void OnRenderObject()
	{
	}

	// Token: 0x060016BE RID: 5822 RVA: 0x000BC958 File Offset: 0x000BAB58
	private void OnPostRender()
	{
		if (this.m_RenderEventCalls != null)
		{
			foreach (Component component in this.m_RenderEventCalls)
			{
				if (component != null)
				{
					component.SendMessage("OnPostRender", 1);
				}
			}
		}
	}

	// Token: 0x060016BF RID: 5823 RVA: 0x0000E900 File Offset: 0x0000CB00
	public void AddRenderEventCall(Component tarCom)
	{
		if (this.m_RenderEventCalls == null)
		{
			this.m_RenderEventCalls = new List<Component>();
		}
		if (!this.m_RenderEventCalls.Contains(tarCom))
		{
			this.m_RenderEventCalls.Add(tarCom);
		}
	}

	// Token: 0x060016C0 RID: 5824 RVA: 0x0000E935 File Offset: 0x0000CB35
	public void RemoveRenderEventCall(Component tarCom)
	{
		if (this.m_RenderEventCalls == null)
		{
			this.m_RenderEventCalls = new List<Component>();
		}
		if (this.m_RenderEventCalls.Contains(tarCom))
		{
			this.m_RenderEventCalls.Remove(tarCom);
		}
	}

	// Token: 0x04001AF4 RID: 6900
	public List<Component> m_RenderEventCalls;
}
