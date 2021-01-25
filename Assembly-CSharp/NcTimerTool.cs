using System;
using UnityEngine;

// Token: 0x020003AD RID: 941
public class NcTimerTool
{
	// Token: 0x06001604 RID: 5636 RVA: 0x0000DD03 File Offset: 0x0000BF03
	public static float GetEngineTime()
	{
		if (Time.time == 0f)
		{
			return 1E-06f;
		}
		return Time.time;
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x0000679C File Offset: 0x0000499C
	public static float GetEngineDeltaTime()
	{
		return Time.deltaTime;
	}

	// Token: 0x06001606 RID: 5638 RVA: 0x000B9DDC File Offset: 0x000B7FDC
	private void InitSmoothTime()
	{
		if (this.m_fSmoothTimes == null)
		{
			this.m_fSmoothTimes = new float[this.m_nSmoothCount];
			for (int i = 0; i < this.m_nSmoothCount; i++)
			{
				this.m_fSmoothTimes[i] = Time.deltaTime;
			}
			this.m_fLastSmoothDeltaTime = Time.deltaTime;
		}
	}

	// Token: 0x06001607 RID: 5639 RVA: 0x000B9E34 File Offset: 0x000B8034
	private float UpdateSmoothTime(float fDeltaTime)
	{
		this.m_fSmoothTimes[this.m_nSmoothIndex++] = Mathf.Min(fDeltaTime, this.m_fLastSmoothDeltaTime * this.m_fSmoothRate);
		if (this.m_nSmoothCount <= this.m_nSmoothIndex)
		{
			this.m_nSmoothIndex = 0;
		}
		this.m_fLastSmoothDeltaTime = 0f;
		for (int i = 0; i < this.m_nSmoothCount; i++)
		{
			this.m_fLastSmoothDeltaTime += this.m_fSmoothTimes[i];
		}
		this.m_fLastSmoothDeltaTime /= (float)this.m_nSmoothCount;
		return this.m_fLastSmoothDeltaTime;
	}

	// Token: 0x06001608 RID: 5640 RVA: 0x0000E1BE File Offset: 0x0000C3BE
	public bool IsUpdateTimer()
	{
		return this.m_fLastEngineTime != NcTimerTool.GetEngineTime();
	}

	// Token: 0x06001609 RID: 5641 RVA: 0x000B9ED8 File Offset: 0x000B80D8
	private float UpdateTimer()
	{
		if (this.m_bEnable)
		{
			if (this.m_fLastEngineTime != NcTimerTool.GetEngineTime())
			{
				this.m_fLastTime = this.m_fCurrentTime;
				this.m_fCurrentTime += (NcTimerTool.GetEngineTime() - this.m_fLastEngineTime) * this.GetTimeScale();
				this.m_fLastEngineTime = NcTimerTool.GetEngineTime();
				if (this.m_fSmoothTimes != null)
				{
					this.UpdateSmoothTime(this.m_fCurrentTime - this.m_fLastTime);
				}
			}
		}
		else
		{
			this.m_fLastEngineTime = NcTimerTool.GetEngineTime();
		}
		return this.m_fCurrentTime;
	}

	// Token: 0x0600160A RID: 5642 RVA: 0x0000E1D0 File Offset: 0x0000C3D0
	public float GetTime()
	{
		return this.UpdateTimer();
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
	public float GetDeltaTime()
	{
		if (!this.m_bEnable)
		{
			return 0f;
		}
		if (Time.timeScale == 0f)
		{
			return 0f;
		}
		this.UpdateTimer();
		return this.m_fCurrentTime - this.m_fLastTime;
	}

	// Token: 0x0600160C RID: 5644 RVA: 0x000B9F6C File Offset: 0x000B816C
	public float GetSmoothDeltaTime()
	{
		if (!this.m_bEnable)
		{
			return 0f;
		}
		if (Time.timeScale == 0f)
		{
			return 0f;
		}
		if (this.m_fSmoothTimes == null)
		{
			this.InitSmoothTime();
		}
		this.UpdateTimer();
		return this.m_fLastSmoothDeltaTime;
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x0000E214 File Offset: 0x0000C414
	public bool IsEnable()
	{
		return this.m_bEnable;
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x0000E21C File Offset: 0x0000C41C
	public void Start()
	{
		this.m_bEnable = true;
		this.m_fCurrentTime = 0f;
		this.m_fLastEngineTime = NcTimerTool.GetEngineTime() - 1E-06f;
		this.UpdateTimer();
	}

	// Token: 0x0600160F RID: 5647 RVA: 0x0000E248 File Offset: 0x0000C448
	public void Reset(float fElapsedTime)
	{
		this.m_fCurrentTime = fElapsedTime;
		this.m_fLastEngineTime = NcTimerTool.GetEngineTime() - 1E-06f;
		this.UpdateTimer();
	}

	// Token: 0x06001610 RID: 5648 RVA: 0x0000E269 File Offset: 0x0000C469
	public void Pause()
	{
		this.UpdateTimer();
		this.m_bEnable = false;
		this.UpdateTimer();
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x0000E280 File Offset: 0x0000C480
	public void Resume()
	{
		this.UpdateTimer();
		this.m_bEnable = true;
		this.UpdateTimer();
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x0000E297 File Offset: 0x0000C497
	public void SetTimeScale(float fTimeScale)
	{
		this.m_fTimeScale = fTimeScale;
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x0000E2A0 File Offset: 0x0000C4A0
	protected virtual float GetTimeScale()
	{
		return this.m_fTimeScale;
	}

	// Token: 0x04001A6D RID: 6765
	protected bool m_bEnable;

	// Token: 0x04001A6E RID: 6766
	private float m_fLastEngineTime;

	// Token: 0x04001A6F RID: 6767
	private float m_fCurrentTime;

	// Token: 0x04001A70 RID: 6768
	private float m_fLastTime;

	// Token: 0x04001A71 RID: 6769
	private float m_fTimeScale = 1f;

	// Token: 0x04001A72 RID: 6770
	private int m_nSmoothCount = 10;

	// Token: 0x04001A73 RID: 6771
	private int m_nSmoothIndex;

	// Token: 0x04001A74 RID: 6772
	private float m_fSmoothRate = 1.3f;

	// Token: 0x04001A75 RID: 6773
	private float[] m_fSmoothTimes;

	// Token: 0x04001A76 RID: 6774
	private float m_fLastSmoothDeltaTime;
}
