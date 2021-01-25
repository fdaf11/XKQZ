using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
[ExecuteInEditMode]
public class CameraPathSpeedList : CameraPathPointList
{
	// Token: 0x060002CA RID: 714 RVA: 0x00003BF2 File Offset: 0x00001DF2
	private void OnEnable()
	{
		base.hideFlags = 2;
	}

	// Token: 0x1700004A RID: 74
	public CameraPathSpeed this[int index]
	{
		get
		{
			return (CameraPathSpeed)base[index];
		}
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00004197 File Offset: 0x00002397
	public override void Init(CameraPath _cameraPath)
	{
		if (this.initialised)
		{
			return;
		}
		this.pointTypeName = "Speed";
		base.Init(_cameraPath);
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060002CD RID: 717 RVA: 0x000041B7 File Offset: 0x000023B7
	// (set) Token: 0x060002CE RID: 718 RVA: 0x000041D0 File Offset: 0x000023D0
	public bool listEnabled
	{
		get
		{
			return this._enabled && base.realNumberOfPoints > 0;
		}
		set
		{
			this._enabled = value;
		}
	}

	// Token: 0x060002CF RID: 719 RVA: 0x000298B0 File Offset: 0x00027AB0
	public void AddSpeedPoint(CameraPathControlPoint atPoint)
	{
		CameraPathSpeed cameraPathSpeed = base.gameObject.AddComponent<CameraPathSpeed>();
		cameraPathSpeed.hideFlags = 2;
		base.AddPoint(cameraPathSpeed, atPoint);
		this.RecalculatePoints();
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x000298E0 File Offset: 0x00027AE0
	public CameraPathSpeed AddSpeedPoint(CameraPathControlPoint curvePointA, CameraPathControlPoint curvePointB, float curvePercetage)
	{
		CameraPathSpeed cameraPathSpeed = base.gameObject.AddComponent<CameraPathSpeed>();
		cameraPathSpeed.hideFlags = 2;
		base.AddPoint(cameraPathSpeed, curvePointA, curvePointB, Mathf.Clamp01(curvePercetage));
		this.RecalculatePoints();
		return cameraPathSpeed;
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x00029918 File Offset: 0x00027B18
	public float GetLowesetSpeed()
	{
		float num = float.PositiveInfinity;
		int numberOfPoints = base.numberOfPoints;
		for (int i = 0; i < numberOfPoints; i++)
		{
			if (this[i].speed < num)
			{
				num = this[i].speed;
			}
		}
		return num;
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00029964 File Offset: 0x00027B64
	public float GetSpeed(float percentage)
	{
		if (base.realNumberOfPoints < 2)
		{
			if (base.realNumberOfPoints == 1)
			{
				return this[0].speed;
			}
			Debug.Log("Not enough points to define a speed");
			return 0f;
		}
		else
		{
			if (percentage >= 1f)
			{
				return ((CameraPathSpeed)base.GetPoint(base.realNumberOfPoints - 1)).speed;
			}
			percentage = Mathf.Clamp(percentage, 0f, 0.999f);
			switch (this.interpolation)
			{
			case CameraPathSpeedList.Interpolation.None:
			{
				CameraPathSpeed cameraPathSpeed = (CameraPathSpeed)base.GetPoint(base.GetNextPointIndex(percentage));
				return cameraPathSpeed.speed;
			}
			case CameraPathSpeedList.Interpolation.Linear:
				return this.LinearInterpolation(percentage);
			case CameraPathSpeedList.Interpolation.SmoothStep:
				return this.SmoothStepInterpolation(percentage);
			default:
				return this.LinearInterpolation(percentage);
			}
		}
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x00029A2C File Offset: 0x00027C2C
	private float LinearInterpolation(float percentage)
	{
		int lastPointIndex = base.GetLastPointIndex(percentage);
		CameraPathSpeed cameraPathSpeed = (CameraPathSpeed)base.GetPoint(lastPointIndex);
		CameraPathSpeed cameraPathSpeed2 = (CameraPathSpeed)base.GetPoint(lastPointIndex + 1);
		if (percentage < cameraPathSpeed.percent)
		{
			return cameraPathSpeed.speed;
		}
		if (percentage > cameraPathSpeed2.percent)
		{
			return cameraPathSpeed2.speed;
		}
		float rawPercent = cameraPathSpeed.rawPercent;
		float num = cameraPathSpeed2.rawPercent;
		if (rawPercent > num)
		{
			num += 1f;
		}
		float num2 = num - rawPercent;
		float num3 = percentage - rawPercent;
		float num4 = num3 / num2;
		Debug.Log(string.Concat(new object[]
		{
			percentage,
			" ",
			num2,
			" ",
			num3,
			" ",
			num4
		}));
		return Mathf.Lerp(cameraPathSpeed.speed, cameraPathSpeed2.speed, num4);
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00029B1C File Offset: 0x00027D1C
	private float SmoothStepInterpolation(float percentage)
	{
		int lastPointIndex = base.GetLastPointIndex(percentage);
		CameraPathSpeed cameraPathSpeed = (CameraPathSpeed)base.GetPoint(lastPointIndex);
		CameraPathSpeed cameraPathSpeed2 = (CameraPathSpeed)base.GetPoint(lastPointIndex + 1);
		if (percentage < cameraPathSpeed.percent)
		{
			return cameraPathSpeed.speed;
		}
		if (percentage > cameraPathSpeed2.percent)
		{
			return cameraPathSpeed2.speed;
		}
		float percent = cameraPathSpeed.percent;
		float num = cameraPathSpeed2.percent;
		if (percent > num)
		{
			num += 1f;
		}
		float num2 = num - percent;
		float num3 = percentage - percent;
		float val = num3 / num2;
		return Mathf.Lerp(cameraPathSpeed.speed, cameraPathSpeed2.speed, CPMath.SmoothStep(val));
	}

	// Token: 0x0400021B RID: 539
	public CameraPathSpeedList.Interpolation interpolation = CameraPathSpeedList.Interpolation.SmoothStep;

	// Token: 0x0400021C RID: 540
	[SerializeField]
	private bool _enabled = true;

	// Token: 0x0200007E RID: 126
	public enum Interpolation
	{
		// Token: 0x0400021E RID: 542
		None,
		// Token: 0x0400021F RID: 543
		Linear,
		// Token: 0x04000220 RID: 544
		SmoothStep
	}
}
