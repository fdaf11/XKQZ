using System;
using UnityEngine;

// Token: 0x02000070 RID: 112
[ExecuteInEditMode]
public class CameraPathEventList : CameraPathPointList
{
	// Token: 0x14000011 RID: 17
	// (add) Token: 0x0600027A RID: 634 RVA: 0x00003E2D File Offset: 0x0000202D
	// (remove) Token: 0x0600027B RID: 635 RVA: 0x00003E46 File Offset: 0x00002046
	public event CameraPathEventList.CameraPathEventPointHandler CameraPathEventPoint;

	// Token: 0x0600027C RID: 636 RVA: 0x00003BF2 File Offset: 0x00001DF2
	private void OnEnable()
	{
		base.hideFlags = 2;
	}

	// Token: 0x1700003D RID: 61
	public CameraPathEvent this[int index]
	{
		get
		{
			return (CameraPathEvent)base[index];
		}
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00003E6D File Offset: 0x0000206D
	public override void Init(CameraPath _cameraPath)
	{
		if (this.initialised)
		{
			return;
		}
		this.pointTypeName = "Event";
		base.Init(_cameraPath);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0002811C File Offset: 0x0002631C
	public void AddEvent(CameraPathControlPoint atPoint)
	{
		CameraPathEvent cameraPathEvent = base.gameObject.AddComponent<CameraPathEvent>();
		cameraPathEvent.hideFlags = 2;
		base.AddPoint(cameraPathEvent, atPoint);
		this.RecalculatePoints();
	}

	// Token: 0x06000280 RID: 640 RVA: 0x0002814C File Offset: 0x0002634C
	public CameraPathEvent AddEvent(CameraPathControlPoint curvePointA, CameraPathControlPoint curvePointB, float curvePercetage)
	{
		CameraPathEvent cameraPathEvent = base.gameObject.AddComponent<CameraPathEvent>();
		cameraPathEvent.hideFlags = 2;
		base.AddPoint(cameraPathEvent, curvePointA, curvePointB, curvePercetage);
		this.RecalculatePoints();
		return cameraPathEvent;
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00003E8D File Offset: 0x0000208D
	public void OnAnimationStart(float startPercentage)
	{
		this._lastPercentage = startPercentage;
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00028180 File Offset: 0x00026380
	public void CheckEvents(float percentage)
	{
		if (Mathf.Abs(percentage - this._lastPercentage) > 0.5f)
		{
			this._lastPercentage = percentage;
			return;
		}
		for (int i = 0; i < base.realNumberOfPoints; i++)
		{
			CameraPathEvent cameraPathEvent = this[i];
			bool flag = (cameraPathEvent.percent >= this._lastPercentage && cameraPathEvent.percent <= percentage) || (cameraPathEvent.percent >= percentage && cameraPathEvent.percent <= this._lastPercentage);
			if (flag)
			{
				CameraPathEvent.Types type = cameraPathEvent.type;
				if (type != CameraPathEvent.Types.Broadcast)
				{
					if (type == CameraPathEvent.Types.Call)
					{
						this.Call(cameraPathEvent);
					}
				}
				else
				{
					this.BroadCast(cameraPathEvent);
				}
			}
		}
		this._lastPercentage = percentage;
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00003E96 File Offset: 0x00002096
	public void BroadCast(CameraPathEvent eventPoint)
	{
		if (this.CameraPathEventPoint != null)
		{
			this.CameraPathEventPoint(eventPoint.eventName);
		}
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0002824C File Offset: 0x0002644C
	public void Call(CameraPathEvent eventPoint)
	{
		if (eventPoint.target == null)
		{
			Debug.LogError("Camera Path Event Error: There is an event call without a specified target. Please check " + eventPoint.displayName, this.cameraPath);
			return;
		}
		switch (eventPoint.argumentType)
		{
		case CameraPathEvent.ArgumentTypes.None:
			eventPoint.target.SendMessage(eventPoint.methodName, 1);
			break;
		case CameraPathEvent.ArgumentTypes.Float:
		{
			float num = float.Parse(eventPoint.methodArgument);
			if (float.IsNaN(num))
			{
				Debug.LogError("Camera Path Aniamtor: The argument specified is not a float");
			}
			eventPoint.target.SendMessage(eventPoint.methodName, num, 1);
			break;
		}
		case CameraPathEvent.ArgumentTypes.Int:
		{
			int num2;
			if (int.TryParse(eventPoint.methodArgument, ref num2))
			{
				eventPoint.target.SendMessage(eventPoint.methodName, num2, 1);
			}
			else
			{
				Debug.LogError("Camera Path Aniamtor: The argument specified is not an integer");
			}
			break;
		}
		case CameraPathEvent.ArgumentTypes.String:
		{
			string methodArgument = eventPoint.methodArgument;
			eventPoint.target.SendMessage(eventPoint.methodName, methodArgument, 1);
			break;
		}
		}
	}

	// Token: 0x040001EC RID: 492
	private float _lastPercentage;

	// Token: 0x02000071 RID: 113
	// (Invoke) Token: 0x06000286 RID: 646
	public delegate void CameraPathEventPointHandler(string name);
}
