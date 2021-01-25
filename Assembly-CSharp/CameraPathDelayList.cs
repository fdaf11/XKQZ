using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
[ExecuteInEditMode]
public class CameraPathDelayList : CameraPathPointList
{
	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06000267 RID: 615 RVA: 0x00003D9B File Offset: 0x00001F9B
	// (remove) Token: 0x06000268 RID: 616 RVA: 0x00003DB4 File Offset: 0x00001FB4
	public event CameraPathDelayList.CameraPathDelayEventHandler CameraPathDelayEvent;

	// Token: 0x06000269 RID: 617 RVA: 0x00003BF2 File Offset: 0x00001DF2
	private void OnEnable()
	{
		base.hideFlags = 2;
	}

	// Token: 0x1700003A RID: 58
	public CameraPathDelay this[int index]
	{
		get
		{
			return (CameraPathDelay)base[index];
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600026B RID: 619 RVA: 0x00003DDB File Offset: 0x00001FDB
	public CameraPathDelay introPoint
	{
		get
		{
			return this._introPoint;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x0600026C RID: 620 RVA: 0x00003DE3 File Offset: 0x00001FE3
	public CameraPathDelay outroPoint
	{
		get
		{
			return this._outroPoint;
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00027DA8 File Offset: 0x00025FA8
	public override void Init(CameraPath _cameraPath)
	{
		if (this.initialised)
		{
			return;
		}
		this.pointTypeName = "Delay";
		base.Init(_cameraPath);
		if (!this.delayInitialised)
		{
			this._introPoint = base.gameObject.AddComponent<CameraPathDelay>();
			this._introPoint.customName = "Start Point";
			this._introPoint.hideFlags = 2;
			base.AddPoint(this.introPoint, 0f);
			this._outroPoint = base.gameObject.AddComponent<CameraPathDelay>();
			this._outroPoint.customName = "End Point";
			this._outroPoint.hideFlags = 2;
			base.AddPoint(this.outroPoint, 1f);
			this.RecalculatePoints();
			this.delayInitialised = true;
		}
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00027E68 File Offset: 0x00026068
	public void AddDelayPoint(CameraPathControlPoint atPoint)
	{
		CameraPathDelay cameraPathDelay = base.gameObject.AddComponent<CameraPathDelay>();
		cameraPathDelay.hideFlags = 2;
		base.AddPoint(cameraPathDelay, atPoint);
		this.RecalculatePoints();
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00027E98 File Offset: 0x00026098
	public CameraPathDelay AddDelayPoint(CameraPathControlPoint curvePointA, CameraPathControlPoint curvePointB, float curvePercetage)
	{
		CameraPathDelay cameraPathDelay = base.gameObject.AddComponent<CameraPathDelay>();
		cameraPathDelay.hideFlags = 2;
		base.AddPoint(cameraPathDelay, curvePointA, curvePointB, curvePercetage);
		this.RecalculatePoints();
		return cameraPathDelay;
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00003DEB File Offset: 0x00001FEB
	public void OnAnimationStart(float startPercentage)
	{
		this._lastPercentage = startPercentage;
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00027ECC File Offset: 0x000260CC
	public void CheckEvents(float percentage)
	{
		if (Mathf.Abs(percentage - this._lastPercentage) > 0.1f)
		{
			this._lastPercentage = percentage;
			return;
		}
		if (this._lastPercentage == percentage)
		{
			return;
		}
		for (int i = 0; i < base.realNumberOfPoints; i++)
		{
			CameraPathDelay cameraPathDelay = this[i];
			if (!(cameraPathDelay == this.outroPoint))
			{
				if (cameraPathDelay.percent >= this._lastPercentage && cameraPathDelay.percent <= percentage)
				{
					if (cameraPathDelay != this.introPoint)
					{
						this.FireDelay(cameraPathDelay);
					}
					else if (cameraPathDelay.time > 0f)
					{
						this.FireDelay(cameraPathDelay);
					}
				}
				else if (cameraPathDelay.percent >= percentage && cameraPathDelay.percent <= this._lastPercentage)
				{
					if (cameraPathDelay != this.introPoint)
					{
						this.FireDelay(cameraPathDelay);
					}
					else if (cameraPathDelay.time > 0f)
					{
						this.FireDelay(cameraPathDelay);
					}
				}
			}
		}
		this._lastPercentage = percentage;
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00027FF0 File Offset: 0x000261F0
	public float CheckEase(float percent)
	{
		float num = 1f;
		for (int i = 0; i < base.realNumberOfPoints; i++)
		{
			CameraPathDelay cameraPathDelay = this[i];
			if (cameraPathDelay != this.introPoint)
			{
				CameraPathDelay cameraPathDelay2 = (CameraPathDelay)base.GetPoint(i - 1);
				float pathPercentage = this.cameraPath.GetPathPercentage(cameraPathDelay2.percent, cameraPathDelay.percent, 1f - cameraPathDelay.introStartEasePercentage);
				if (pathPercentage < percent && cameraPathDelay.percent > percent)
				{
					float num2 = (percent - pathPercentage) / (cameraPathDelay.percent - pathPercentage);
					num = cameraPathDelay.introCurve.Evaluate(num2);
				}
			}
			if (cameraPathDelay != this.outroPoint)
			{
				CameraPathDelay cameraPathDelay3 = (CameraPathDelay)base.GetPoint(i + 1);
				float pathPercentage2 = this.cameraPath.GetPathPercentage(cameraPathDelay.percent, cameraPathDelay3.percent, cameraPathDelay.outroEndEasePercentage);
				if (cameraPathDelay.percent < percent && pathPercentage2 > percent)
				{
					float num3 = (percent - cameraPathDelay.percent) / (pathPercentage2 - cameraPathDelay.percent);
					num = cameraPathDelay.outroCurve.Evaluate(num3);
				}
			}
		}
		return Math.Max(num, this.MINIMUM_EASE_VALUE);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00003DF4 File Offset: 0x00001FF4
	public void FireDelay(CameraPathDelay eventPoint)
	{
		if (this.CameraPathDelayEvent != null)
		{
			this.CameraPathDelayEvent(eventPoint.time);
		}
	}

	// Token: 0x040001D8 RID: 472
	public float MINIMUM_EASE_VALUE = 0.01f;

	// Token: 0x040001D9 RID: 473
	private float _lastPercentage;

	// Token: 0x040001DA RID: 474
	[SerializeField]
	private CameraPathDelay _introPoint;

	// Token: 0x040001DB RID: 475
	[SerializeField]
	private CameraPathDelay _outroPoint;

	// Token: 0x040001DC RID: 476
	[SerializeField]
	private bool delayInitialised;

	// Token: 0x0200006C RID: 108
	// (Invoke) Token: 0x06000275 RID: 629
	public delegate void CameraPathDelayEventHandler(float time);
}
