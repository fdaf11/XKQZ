using System;

// Token: 0x020003AC RID: 940
public class NcRepeatTimerTool : NcTimerTool
{
	// Token: 0x060015F2 RID: 5618 RVA: 0x000B9D54 File Offset: 0x000B7F54
	public bool UpdateTimer()
	{
		if (!this.m_bEnable)
		{
			return false;
		}
		bool flag = this.m_fUpdateTime <= base.GetTime();
		if (flag)
		{
			this.m_fUpdateTime += this.m_fIntervalTime;
			this.m_nCallCount++;
			if (this.m_fIntervalTime <= 0f || (this.m_nRepeatCount != 0 && this.m_nRepeatCount <= this.m_nCallCount))
			{
				this.m_bEnable = false;
			}
		}
		return flag;
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x0000DFF4 File Offset: 0x0000C1F4
	public void ResetUpdateTime()
	{
		this.m_fUpdateTime = base.GetTime() + this.m_fIntervalTime;
	}

	// Token: 0x060015F4 RID: 5620 RVA: 0x0000E009 File Offset: 0x0000C209
	public int GetCallCount()
	{
		return this.m_nCallCount;
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x0000E011 File Offset: 0x0000C211
	public object GetArgObject()
	{
		return this.m_ArgObject;
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x0000E019 File Offset: 0x0000C219
	public float GetElapsedRate()
	{
		if (this.m_fUpdateTime == 0f)
		{
			return 1f;
		}
		return base.GetTime() / this.m_fUpdateTime;
	}

	// Token: 0x060015F7 RID: 5623 RVA: 0x0000E03E File Offset: 0x0000C23E
	public void SetTimer(float fStartTime)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime());
	}

	// Token: 0x060015F8 RID: 5624 RVA: 0x0000E04D File Offset: 0x0000C24D
	public void SetTimer(float fStartTime, float fRepeatTime)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRepeatTime);
	}

	// Token: 0x060015F9 RID: 5625 RVA: 0x0000E05D File Offset: 0x0000C25D
	public void SetTimer(float fStartTime, float fRepeatTime, int nRepeatCount)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRepeatTime, nRepeatCount);
	}

	// Token: 0x060015FA RID: 5626 RVA: 0x0000E06E File Offset: 0x0000C26E
	public void SetTimer(float fStartTime, object arg)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), arg);
	}

	// Token: 0x060015FB RID: 5627 RVA: 0x0000E07E File Offset: 0x0000C27E
	public void SetTimer(float fStartTime, float fRepeatTime, object arg)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRepeatTime, arg);
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x0000E08F File Offset: 0x0000C28F
	public void SetTimer(float fStartTime, float fRepeatTime, int nRepeatCount, object arg)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRepeatTime, nRepeatCount, arg);
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x0000E0A2 File Offset: 0x0000C2A2
	public void SetRelTimer(float fStartRelTime)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fUpdateTime = fStartRelTime;
		this.m_fIntervalTime = 0f;
		this.m_nRepeatCount = 0;
	}

	// Token: 0x060015FE RID: 5630 RVA: 0x0000E0CA File Offset: 0x0000C2CA
	public void SetRelTimer(float fStartRelTime, float fRepeatTime)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fUpdateTime = fStartRelTime;
		this.m_fIntervalTime = fRepeatTime;
		this.m_nRepeatCount = 0;
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x0000E0EE File Offset: 0x0000C2EE
	public void SetRelTimer(float fStartRelTime, float fRepeatTime, int nRepeatCount)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fUpdateTime = fStartRelTime;
		this.m_fIntervalTime = fRepeatTime;
		this.m_nRepeatCount = nRepeatCount;
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x0000E112 File Offset: 0x0000C312
	public void SetRelTimer(float fStartRelTime, object arg)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fUpdateTime = fStartRelTime;
		this.m_fIntervalTime = 0f;
		this.m_nRepeatCount = 0;
		this.m_ArgObject = arg;
	}

	// Token: 0x06001601 RID: 5633 RVA: 0x0000E141 File Offset: 0x0000C341
	public void SetRelTimer(float fStartRelTime, float fRepeatTime, object arg)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fUpdateTime = fStartRelTime;
		this.m_fIntervalTime = fRepeatTime;
		this.m_nRepeatCount = 0;
		this.m_ArgObject = arg;
	}

	// Token: 0x06001602 RID: 5634 RVA: 0x0000E16C File Offset: 0x0000C36C
	public void SetRelTimer(float fStartRelTime, float fRepeatTime, int nRepeatCount, object arg)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fUpdateTime = fStartRelTime;
		this.m_fIntervalTime = fRepeatTime;
		this.m_nRepeatCount = nRepeatCount;
		this.m_ArgObject = arg;
	}

	// Token: 0x04001A68 RID: 6760
	protected float m_fUpdateTime;

	// Token: 0x04001A69 RID: 6761
	protected float m_fIntervalTime;

	// Token: 0x04001A6A RID: 6762
	protected int m_nRepeatCount;

	// Token: 0x04001A6B RID: 6763
	protected int m_nCallCount;

	// Token: 0x04001A6C RID: 6764
	protected object m_ArgObject;
}
