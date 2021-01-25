using System;
using UnityEngine;

// Token: 0x020003AB RID: 939
public class NcRandomTimerTool : NcTimerTool
{
	// Token: 0x060015DF RID: 5599 RVA: 0x0000DF00 File Offset: 0x0000C100
	public bool UpdateRandomTimer(bool bReset)
	{
		if (this.UpdateRandomTimer())
		{
			this.ResetUpdateTime();
			return true;
		}
		return false;
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x000B9A6C File Offset: 0x000B7C6C
	public bool UpdateRandomTimer()
	{
		if (!this.m_bEnable)
		{
			return false;
		}
		bool flag = this.m_fUpdateTime <= base.GetTime();
		if (flag)
		{
			this.m_fUpdateTime += this.m_fMinIntervalTime + ((0f >= this.m_fRandomTime) ? 0f : (Random.value % this.m_fRandomTime));
			this.m_nCallCount++;
			if (this.m_nRepeatCount != 0 && this.m_nRepeatCount <= this.m_nCallCount)
			{
				this.m_bEnable = false;
			}
		}
		return flag;
	}

	// Token: 0x060015E1 RID: 5601 RVA: 0x0000DF16 File Offset: 0x0000C116
	public void ResetUpdateTime()
	{
		this.m_fUpdateTime = base.GetTime() + this.m_fMinIntervalTime + ((0f >= this.m_fRandomTime) ? 0f : (Random.value % this.m_fRandomTime));
	}

	// Token: 0x060015E2 RID: 5602 RVA: 0x0000DF52 File Offset: 0x0000C152
	public int GetCallCount()
	{
		return this.m_nCallCount;
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x0000DF5A File Offset: 0x0000C15A
	public object GetArgObject()
	{
		return this.m_ArgObject;
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x0000DF62 File Offset: 0x0000C162
	public float GetElapsedRate()
	{
		if (this.m_fUpdateTime == 0f)
		{
			return 1f;
		}
		return base.GetTime() / this.m_fUpdateTime;
	}

	// Token: 0x060015E5 RID: 5605 RVA: 0x0000DF87 File Offset: 0x0000C187
	public void SetTimer(float fStartTime, float fRandomTime)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRandomTime);
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x0000DF97 File Offset: 0x0000C197
	public void SetTimer(float fStartTime, float fRandomTime, float fMinIntervalTime)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRandomTime, fMinIntervalTime);
	}

	// Token: 0x060015E7 RID: 5607 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
	public void SetTimer(float fStartTime, float fRandomTime, float fMinIntervalTime, int nRepeatCount)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRandomTime, fMinIntervalTime, nRepeatCount);
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x0000DFBB File Offset: 0x0000C1BB
	public void SetTimer(float fStartTime, float fRandomTime, object arg)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRandomTime, arg);
	}

	// Token: 0x060015E9 RID: 5609 RVA: 0x0000DFCC File Offset: 0x0000C1CC
	public void SetTimer(float fStartTime, float fRandomTime, float fMinIntervalTime, object arg)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRandomTime, fMinIntervalTime, arg);
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x0000DFDF File Offset: 0x0000C1DF
	public void SetTimer(float fStartTime, float fRandomTime, float fMinIntervalTime, int nRepeatCount, object arg)
	{
		this.SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRandomTime, fMinIntervalTime, nRepeatCount, arg);
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x000B9B0C File Offset: 0x000B7D0C
	public void SetRelTimer(float fStartRelTime, float fRandomTime)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fRandomTime = fRandomTime;
		this.m_fUpdateTime = ((0f >= this.m_fRandomTime) ? 0f : (Random.value % this.m_fRandomTime));
		this.m_fMinIntervalTime = 0f;
		this.m_nRepeatCount = 0;
	}

	// Token: 0x060015EC RID: 5612 RVA: 0x000B9B6C File Offset: 0x000B7D6C
	public void SetRelTimer(float fStartRelTime, float fRandomTime, float fMinIntervalTime)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fRandomTime = fRandomTime;
		this.m_fUpdateTime = ((0f >= this.m_fRandomTime) ? 0f : (Random.value % this.m_fRandomTime));
		this.m_fMinIntervalTime = fMinIntervalTime;
		this.m_nRepeatCount = 0;
	}

	// Token: 0x060015ED RID: 5613 RVA: 0x000B9BC8 File Offset: 0x000B7DC8
	public void SetRelTimer(float fStartRelTime, float fRandomTime, float fMinIntervalTime, int nRepeatCount)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fRandomTime = fRandomTime;
		this.m_fUpdateTime = ((0f >= this.m_fRandomTime) ? 0f : (Random.value % this.m_fRandomTime));
		this.m_fMinIntervalTime = fMinIntervalTime;
		this.m_nRepeatCount = nRepeatCount;
	}

	// Token: 0x060015EE RID: 5614 RVA: 0x000B9C24 File Offset: 0x000B7E24
	public void SetRelTimer(float fStartRelTime, float fRandomTime, object arg)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fRandomTime = fRandomTime;
		this.m_fUpdateTime = ((0f >= this.m_fRandomTime) ? 0f : (Random.value % this.m_fRandomTime));
		this.m_fMinIntervalTime = 0f;
		this.m_nRepeatCount = 0;
		this.m_ArgObject = arg;
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x000B9C8C File Offset: 0x000B7E8C
	public void SetRelTimer(float fStartRelTime, float fRandomTime, float fMinIntervalTime, object arg)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fRandomTime = fRandomTime;
		this.m_fUpdateTime = ((0f >= this.m_fRandomTime) ? 0f : (Random.value % this.m_fRandomTime));
		this.m_fMinIntervalTime = fMinIntervalTime;
		this.m_nRepeatCount = 0;
		this.m_ArgObject = arg;
	}

	// Token: 0x060015F0 RID: 5616 RVA: 0x000B9CF0 File Offset: 0x000B7EF0
	public void SetRelTimer(float fStartRelTime, float fRandomTime, float fMinIntervalTime, int nRepeatCount, object arg)
	{
		base.Start();
		this.m_nCallCount = 0;
		this.m_fRandomTime = fRandomTime;
		this.m_fUpdateTime = ((0f >= this.m_fRandomTime) ? 0f : (Random.value % this.m_fRandomTime));
		this.m_fMinIntervalTime = fMinIntervalTime;
		this.m_nRepeatCount = nRepeatCount;
		this.m_ArgObject = arg;
	}

	// Token: 0x04001A62 RID: 6754
	protected float m_fRandomTime;

	// Token: 0x04001A63 RID: 6755
	protected float m_fUpdateTime;

	// Token: 0x04001A64 RID: 6756
	protected float m_fMinIntervalTime;

	// Token: 0x04001A65 RID: 6757
	protected int m_nRepeatCount;

	// Token: 0x04001A66 RID: 6758
	protected int m_nCallCount;

	// Token: 0x04001A67 RID: 6759
	protected object m_ArgObject;
}
