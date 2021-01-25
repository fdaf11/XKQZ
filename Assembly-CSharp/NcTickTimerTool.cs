using System;
using UnityEngine;

// Token: 0x02000400 RID: 1024
public class NcTickTimerTool
{
	// Token: 0x060018BC RID: 6332 RVA: 0x00010093 File Offset: 0x0000E293
	public NcTickTimerTool()
	{
		this.StartTickCount();
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x000100A1 File Offset: 0x0000E2A1
	public static NcTickTimerTool GetTickTimer()
	{
		return new NcTickTimerTool();
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x000100A8 File Offset: 0x0000E2A8
	public void StartTickCount()
	{
		this.m_nStartTickCount = Environment.TickCount;
		this.m_nCheckTickCount = this.m_nStartTickCount;
	}

	// Token: 0x060018BF RID: 6335 RVA: 0x000100C1 File Offset: 0x0000E2C1
	public int GetStartedTickCount()
	{
		return Environment.TickCount - this.m_nStartTickCount;
	}

	// Token: 0x060018C0 RID: 6336 RVA: 0x000C9314 File Offset: 0x000C7514
	public int GetElapsedTickCount()
	{
		int result = Environment.TickCount - this.m_nCheckTickCount;
		this.m_nCheckTickCount = Environment.TickCount;
		return result;
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x000100CF File Offset: 0x0000E2CF
	public void LogElapsedTickCount()
	{
		Debug.Log(this.GetElapsedTickCount());
	}

	// Token: 0x04001D22 RID: 7458
	protected int m_nStartTickCount;

	// Token: 0x04001D23 RID: 7459
	protected int m_nCheckTickCount;
}
