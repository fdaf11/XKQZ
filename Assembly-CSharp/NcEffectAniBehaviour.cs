using System;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public class NcEffectAniBehaviour : NcEffectBehaviour
{
	// Token: 0x060015A7 RID: 5543 RVA: 0x0000DBF4 File Offset: 0x0000BDF4
	public void SetCallBackEndAnimation(GameObject callBackGameObj)
	{
		this.m_OnEndAniGameObject = callBackGameObj;
	}

	// Token: 0x060015A8 RID: 5544 RVA: 0x0000DBFD File Offset: 0x0000BDFD
	public void SetCallBackEndAnimation(GameObject callBackGameObj, string nameFunction)
	{
		this.m_OnEndAniGameObject = callBackGameObj;
		this.m_OnEndAniFunction = nameFunction;
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x0000DC0D File Offset: 0x0000BE0D
	public bool IsStartAnimation()
	{
		return this.m_Timer != null && this.m_Timer.IsEnable();
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x0000DC28 File Offset: 0x0000BE28
	public bool IsEndAnimation()
	{
		return this.m_bEndAnimation;
	}

	// Token: 0x060015AB RID: 5547 RVA: 0x0000DC30 File Offset: 0x0000BE30
	protected void InitAnimationTimer()
	{
		if (this.m_Timer == null)
		{
			this.m_Timer = new NcTimerTool();
		}
		this.m_bEndAnimation = false;
		this.m_Timer.Start();
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x0000DC5A File Offset: 0x0000BE5A
	public virtual void ResetAnimation()
	{
		this.m_bEndAnimation = false;
		if (this.m_Timer != null)
		{
			this.m_Timer.Reset(0f);
		}
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x0000DC7E File Offset: 0x0000BE7E
	public virtual void PauseAnimation()
	{
		if (this.m_Timer != null)
		{
			this.m_Timer.Pause();
		}
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x0000DC96 File Offset: 0x0000BE96
	public virtual void ResumeAnimation()
	{
		if (this.m_Timer != null)
		{
			this.m_Timer.Resume();
		}
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x0000DCAE File Offset: 0x0000BEAE
	public virtual void MoveAnimation(float fElapsedTime)
	{
		if (this.m_Timer != null)
		{
			this.m_Timer.Reset(fElapsedTime);
		}
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x0000DCC7 File Offset: 0x0000BEC7
	protected void OnEndAnimation()
	{
		this.m_bEndAnimation = true;
		if (this.m_OnEndAniGameObject != null)
		{
			this.m_OnEndAniGameObject.SendMessage(this.m_OnEndAniFunction, this, 1);
		}
	}

	// Token: 0x04001A41 RID: 6721
	protected NcTimerTool m_Timer;

	// Token: 0x04001A42 RID: 6722
	protected GameObject m_OnEndAniGameObject;

	// Token: 0x04001A43 RID: 6723
	protected bool m_bEndAnimation;

	// Token: 0x04001A44 RID: 6724
	public string m_OnEndAniFunction = "OnEndAnimation";
}
