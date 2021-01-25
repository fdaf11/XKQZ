using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
[ExecuteInEditMode]
public class CameraPathTiltList : CameraPathPointList
{
	// Token: 0x060002D7 RID: 727 RVA: 0x00003BF2 File Offset: 0x00001DF2
	private void OnEnable()
	{
		base.hideFlags = 2;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0000420D File Offset: 0x0000240D
	public override void Init(CameraPath _cameraPath)
	{
		if (this.initialised)
		{
			return;
		}
		this.pointTypeName = "Tilt";
		base.Init(_cameraPath);
		this.cameraPath.PathPointAddedEvent += this.AddTilt;
		this.initialised = true;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0000424B File Offset: 0x0000244B
	public override void CleanUp()
	{
		base.CleanUp();
		this.cameraPath.PathPointAddedEvent -= this.AddTilt;
		this.initialised = false;
	}

	// Token: 0x1700004C RID: 76
	public CameraPathTilt this[int index]
	{
		get
		{
			return (CameraPathTilt)base[index];
		}
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00029BC0 File Offset: 0x00027DC0
	public void AddTilt(CameraPathControlPoint atPoint)
	{
		CameraPathTilt cameraPathTilt = base.gameObject.AddComponent<CameraPathTilt>();
		cameraPathTilt.tilt = 0f;
		cameraPathTilt.hideFlags = 2;
		base.AddPoint(cameraPathTilt, atPoint);
		this.RecalculatePoints();
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00029BFC File Offset: 0x00027DFC
	public CameraPathTilt AddTilt(CameraPathControlPoint curvePointA, CameraPathControlPoint curvePointB, float curvePercetage, float tilt)
	{
		CameraPathTilt cameraPathTilt = base.gameObject.AddComponent<CameraPathTilt>();
		cameraPathTilt.tilt = tilt;
		cameraPathTilt.hideFlags = 2;
		base.AddPoint(cameraPathTilt, curvePointA, curvePointB, curvePercetage);
		this.RecalculatePoints();
		return cameraPathTilt;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00029C38 File Offset: 0x00027E38
	public float GetTilt(float percentage)
	{
		if (base.realNumberOfPoints < 2)
		{
			if (base.realNumberOfPoints == 1)
			{
				return this[0].tilt;
			}
			return 0f;
		}
		else
		{
			percentage = Mathf.Clamp(percentage, 0f, 1f);
			switch (this.interpolation)
			{
			case CameraPathTiltList.Interpolation.None:
			{
				CameraPathTilt cameraPathTilt = (CameraPathTilt)base.GetPoint(base.GetNextPointIndex(percentage));
				return cameraPathTilt.tilt;
			}
			case CameraPathTiltList.Interpolation.Linear:
				return this.LinearInterpolation(percentage);
			case CameraPathTiltList.Interpolation.SmoothStep:
				return this.SmoothStepInterpolation(percentage);
			default:
				return this.LinearInterpolation(percentage);
			}
		}
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00029CD4 File Offset: 0x00027ED4
	public void AutoSetTilts()
	{
		for (int i = 0; i < base.realNumberOfPoints; i++)
		{
			this.AutoSetTilt(this[i]);
		}
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00029D08 File Offset: 0x00027F08
	public void AutoSetTilt(CameraPathTilt point)
	{
		float percent = point.percent;
		Vector3 pathPosition = this.cameraPath.GetPathPosition(percent - 0.1f);
		Vector3 pathPosition2 = this.cameraPath.GetPathPosition(percent);
		Vector3 pathPosition3 = this.cameraPath.GetPathPosition(percent + 0.1f);
		Vector3 vector = pathPosition2 - pathPosition;
		Vector3 vector2 = pathPosition3 - pathPosition2;
		Quaternion quaternion = Quaternion.LookRotation(-this.cameraPath.GetPathDirection(point.percent));
		Vector3 vector3 = quaternion * (vector2 - vector).normalized;
		float num = Vector2.Angle(Vector2.up, new Vector2(vector3.x, vector3.y));
		float num2 = Mathf.Min(Mathf.Abs(vector3.x) + Mathf.Abs(vector3.y) / Mathf.Abs(vector3.z), 1f);
		point.tilt = -num * this.autoSensitivity * num2;
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00029E00 File Offset: 0x00028000
	private float LinearInterpolation(float percentage)
	{
		int lastPointIndex = base.GetLastPointIndex(percentage);
		CameraPathTilt cameraPathTilt = (CameraPathTilt)base.GetPoint(lastPointIndex);
		CameraPathTilt cameraPathTilt2 = (CameraPathTilt)base.GetPoint(lastPointIndex + 1);
		float percent = cameraPathTilt.percent;
		float num = cameraPathTilt2.percent;
		if (percent > num)
		{
			num += 1f;
		}
		float num2 = num - percent;
		float num3 = percentage - percent;
		float num4 = num3 / num2;
		return Mathf.LerpAngle(cameraPathTilt.tilt, cameraPathTilt2.tilt, num4);
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00029E78 File Offset: 0x00028078
	private float SmoothStepInterpolation(float percentage)
	{
		int lastPointIndex = base.GetLastPointIndex(percentage);
		CameraPathTilt cameraPathTilt = (CameraPathTilt)base.GetPoint(lastPointIndex);
		CameraPathTilt cameraPathTilt2 = (CameraPathTilt)base.GetPoint(lastPointIndex + 1);
		float percent = cameraPathTilt.percent;
		float num = cameraPathTilt2.percent;
		if (percent > num)
		{
			num += 1f;
		}
		float num2 = num - percent;
		float num3 = percentage - percent;
		float val = num3 / num2;
		return Mathf.LerpAngle(cameraPathTilt.tilt, cameraPathTilt2.tilt, CPMath.SmoothStep(val));
	}

	// Token: 0x04000222 RID: 546
	public CameraPathTiltList.Interpolation interpolation = CameraPathTiltList.Interpolation.SmoothStep;

	// Token: 0x04000223 RID: 547
	public bool listEnabled = true;

	// Token: 0x04000224 RID: 548
	public float autoSensitivity = 1f;

	// Token: 0x02000081 RID: 129
	public enum Interpolation
	{
		// Token: 0x04000226 RID: 550
		None,
		// Token: 0x04000227 RID: 551
		Linear,
		// Token: 0x04000228 RID: 552
		SmoothStep
	}
}
