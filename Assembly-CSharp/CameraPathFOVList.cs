using System;
using UnityEngine;

// Token: 0x02000073 RID: 115
[ExecuteInEditMode]
public class CameraPathFOVList : CameraPathPointList
{
	// Token: 0x0600028B RID: 651 RVA: 0x00003BF2 File Offset: 0x00001DF2
	private void OnEnable()
	{
		base.hideFlags = 2;
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00003EE1 File Offset: 0x000020E1
	public override void Init(CameraPath _cameraPath)
	{
		if (this.initialised)
		{
			return;
		}
		this.pointTypeName = "FOV";
		base.Init(_cameraPath);
		this.cameraPath.PathPointAddedEvent += this.AddFOV;
		this.initialised = true;
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00003F1F File Offset: 0x0000211F
	public override void CleanUp()
	{
		base.CleanUp();
		this.cameraPath.PathPointAddedEvent -= this.AddFOV;
		this.initialised = false;
	}

	// Token: 0x1700003E RID: 62
	public CameraPathFOV this[int index]
	{
		get
		{
			return (CameraPathFOV)base[index];
		}
	}

	// Token: 0x0600028F RID: 655 RVA: 0x0002835C File Offset: 0x0002655C
	public void AddFOV(CameraPathControlPoint atPoint)
	{
		CameraPathFOV cameraPathFOV = base.gameObject.AddComponent<CameraPathFOV>();
		cameraPathFOV.FOV = this.defaultFOV;
		cameraPathFOV.Size = this.defaultSize;
		cameraPathFOV.hideFlags = 2;
		base.AddPoint(cameraPathFOV, atPoint);
		this.RecalculatePoints();
	}

	// Token: 0x06000290 RID: 656 RVA: 0x000283A4 File Offset: 0x000265A4
	public CameraPathFOV AddFOV(CameraPathControlPoint curvePointA, CameraPathControlPoint curvePointB, float curvePercetage, float fov, float size)
	{
		CameraPathFOV cameraPathFOV = base.gameObject.AddComponent<CameraPathFOV>();
		cameraPathFOV.hideFlags = 2;
		cameraPathFOV.FOV = fov;
		cameraPathFOV.Size = size;
		cameraPathFOV.Size = this.defaultSize;
		base.AddPoint(cameraPathFOV, curvePointA, curvePointB, curvePercetage);
		this.RecalculatePoints();
		return cameraPathFOV;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x000283F4 File Offset: 0x000265F4
	public float GetValue(float percentage, CameraPathFOVList.ProjectionType type)
	{
		if (base.realNumberOfPoints < 2)
		{
			if (type == CameraPathFOVList.ProjectionType.FOV)
			{
				if (base.realNumberOfPoints == 1)
				{
					return this[0].FOV;
				}
				return this.defaultFOV;
			}
			else
			{
				if (base.realNumberOfPoints == 1)
				{
					return this[0].Size;
				}
				return this.defaultSize;
			}
		}
		else
		{
			percentage = Mathf.Clamp(percentage, 0f, 1f);
			CameraPathFOVList.Interpolation interpolation = this.interpolation;
			if (interpolation == CameraPathFOVList.Interpolation.Linear)
			{
				return this.LinearInterpolation(percentage, type);
			}
			if (interpolation != CameraPathFOVList.Interpolation.SmoothStep)
			{
				return this.LinearInterpolation(percentage, type);
			}
			return this.SmoothStepInterpolation(percentage, type);
		}
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0002849C File Offset: 0x0002669C
	private float LinearInterpolation(float percentage, CameraPathFOVList.ProjectionType projectionType)
	{
		int lastPointIndex = base.GetLastPointIndex(percentage);
		CameraPathFOV cameraPathFOV = (CameraPathFOV)base.GetPoint(lastPointIndex);
		CameraPathFOV cameraPathFOV2 = (CameraPathFOV)base.GetPoint(lastPointIndex + 1);
		float percent = cameraPathFOV.percent;
		float num = cameraPathFOV2.percent;
		if (percent > num)
		{
			num += 1f;
		}
		float num2 = num - percent;
		float num3 = percentage - percent;
		float num4 = num3 / num2;
		float num5 = (projectionType != CameraPathFOVList.ProjectionType.FOV) ? cameraPathFOV.Size : cameraPathFOV.FOV;
		float num6 = (projectionType != CameraPathFOVList.ProjectionType.FOV) ? cameraPathFOV2.Size : cameraPathFOV2.FOV;
		return Mathf.Lerp(num5, num6, num4);
	}

	// Token: 0x06000293 RID: 659 RVA: 0x00028540 File Offset: 0x00026740
	private float SmoothStepInterpolation(float percentage, CameraPathFOVList.ProjectionType projectionType)
	{
		int lastPointIndex = base.GetLastPointIndex(percentage);
		CameraPathFOV cameraPathFOV = (CameraPathFOV)base.GetPoint(lastPointIndex);
		CameraPathFOV cameraPathFOV2 = (CameraPathFOV)base.GetPoint(lastPointIndex + 1);
		float percent = cameraPathFOV.percent;
		float num = cameraPathFOV2.percent;
		if (percent > num)
		{
			num += 1f;
		}
		float num2 = num - percent;
		float num3 = percentage - percent;
		float val = num3 / num2;
		float num4 = (projectionType != CameraPathFOVList.ProjectionType.FOV) ? cameraPathFOV.Size : cameraPathFOV.FOV;
		float num5 = (projectionType != CameraPathFOVList.ProjectionType.FOV) ? cameraPathFOV2.Size : cameraPathFOV2.FOV;
		return Mathf.Lerp(num4, num5, CPMath.SmoothStep(val));
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000294 RID: 660 RVA: 0x000285E8 File Offset: 0x000267E8
	private float defaultFOV
	{
		get
		{
			if (Camera.current)
			{
				return Camera.current.fieldOfView;
			}
			Camera[] allCameras = Camera.allCameras;
			bool flag = allCameras.Length > 0;
			if (flag)
			{
				return allCameras[0].fieldOfView;
			}
			return 60f;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000295 RID: 661 RVA: 0x00028630 File Offset: 0x00026830
	private float defaultSize
	{
		get
		{
			if (Camera.current)
			{
				return Camera.current.orthographicSize;
			}
			Camera[] allCameras = Camera.allCameras;
			bool flag = allCameras.Length > 0;
			if (flag)
			{
				return allCameras[0].orthographicSize;
			}
			return 5f;
		}
	}

	// Token: 0x040001F0 RID: 496
	private const float DEFAULT_FOV = 60f;

	// Token: 0x040001F1 RID: 497
	private const float DEFAULT_SIZE = 5f;

	// Token: 0x040001F2 RID: 498
	public CameraPathFOVList.Interpolation interpolation = CameraPathFOVList.Interpolation.SmoothStep;

	// Token: 0x040001F3 RID: 499
	public bool listEnabled;

	// Token: 0x02000074 RID: 116
	public enum ProjectionType
	{
		// Token: 0x040001F5 RID: 501
		FOV,
		// Token: 0x040001F6 RID: 502
		Orthographic
	}

	// Token: 0x02000075 RID: 117
	public enum Interpolation
	{
		// Token: 0x040001F8 RID: 504
		None,
		// Token: 0x040001F9 RID: 505
		Linear,
		// Token: 0x040001FA RID: 506
		SmoothStep
	}
}
