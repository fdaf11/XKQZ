using System;
using UnityEngine;

// Token: 0x02000077 RID: 119
[ExecuteInEditMode]
public class CameraPathOrientationList : CameraPathPointList
{
	// Token: 0x06000299 RID: 665 RVA: 0x00003BF2 File Offset: 0x00001DF2
	private void OnEnable()
	{
		base.hideFlags = 2;
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00003F75 File Offset: 0x00002175
	public override void Init(CameraPath _cameraPath)
	{
		if (this.initialised)
		{
			return;
		}
		this.pointTypeName = "Orientation";
		base.Init(_cameraPath);
		this.cameraPath.PathPointAddedEvent += this.AddOrientation;
		this.initialised = true;
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00003FB3 File Offset: 0x000021B3
	public override void CleanUp()
	{
		base.CleanUp();
		this.cameraPath.PathPointAddedEvent -= this.AddOrientation;
		this.initialised = false;
	}

	// Token: 0x17000041 RID: 65
	public CameraPathOrientation this[int index]
	{
		get
		{
			return (CameraPathOrientation)base[index];
		}
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00028678 File Offset: 0x00026878
	public void AddOrientation(CameraPathControlPoint atPoint)
	{
		CameraPathOrientation cameraPathOrientation = base.gameObject.AddComponent<CameraPathOrientation>();
		if (atPoint.forwardControlPoint != Vector3.zero)
		{
			cameraPathOrientation.rotation = Quaternion.LookRotation(atPoint.forwardControlPoint);
		}
		else
		{
			cameraPathOrientation.rotation = Quaternion.LookRotation(this.cameraPath.GetPathDirection(atPoint.percentage));
		}
		cameraPathOrientation.hideFlags = 2;
		base.AddPoint(cameraPathOrientation, atPoint);
		this.RecalculatePoints();
	}

	// Token: 0x0600029E RID: 670 RVA: 0x000286F0 File Offset: 0x000268F0
	public CameraPathOrientation AddOrientation(CameraPathControlPoint curvePointA, CameraPathControlPoint curvePointB, float curvePercetage, Quaternion rotation)
	{
		CameraPathOrientation cameraPathOrientation = base.gameObject.AddComponent<CameraPathOrientation>();
		cameraPathOrientation.rotation = rotation;
		cameraPathOrientation.hideFlags = 2;
		base.AddPoint(cameraPathOrientation, curvePointA, curvePointB, curvePercetage);
		this.RecalculatePoints();
		return cameraPathOrientation;
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00003FE7 File Offset: 0x000021E7
	public void RemovePoint(CameraPathOrientation orientation)
	{
		base.RemovePoint(orientation);
		this.RecalculatePoints();
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0002872C File Offset: 0x0002692C
	public Quaternion GetOrientation(float percentage)
	{
		if (base.realNumberOfPoints < 2)
		{
			if (base.realNumberOfPoints == 1)
			{
				return this[0].rotation;
			}
			return Quaternion.identity;
		}
		else
		{
			if (float.IsNaN(percentage))
			{
				percentage = 0f;
			}
			percentage = Mathf.Clamp(percentage, 0f, 1f);
			Quaternion result = Quaternion.identity;
			switch (this.interpolation)
			{
			case CameraPathOrientationList.Interpolation.None:
			{
				CameraPathOrientation cameraPathOrientation = (CameraPathOrientation)base.GetPoint(base.GetNextPointIndex(percentage));
				result = cameraPathOrientation.rotation;
				break;
			}
			case CameraPathOrientationList.Interpolation.Linear:
				result = this.LinearInterpolation(percentage);
				break;
			case CameraPathOrientationList.Interpolation.SmoothStep:
				result = this.SmootStepInterpolation(percentage);
				break;
			case CameraPathOrientationList.Interpolation.Hermite:
				result = this.CubicInterpolation(percentage);
				break;
			case CameraPathOrientationList.Interpolation.Cubic:
				result = this.CubicInterpolation(percentage);
				break;
			default:
				result = Quaternion.LookRotation(Vector3.forward);
				break;
			}
			if (float.IsNaN(result.x))
			{
				return Quaternion.identity;
			}
			return result;
		}
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00028830 File Offset: 0x00026A30
	private Quaternion LinearInterpolation(float percentage)
	{
		int lastPointIndex = base.GetLastPointIndex(percentage);
		CameraPathOrientation cameraPathOrientation = (CameraPathOrientation)base.GetPoint(lastPointIndex);
		CameraPathOrientation cameraPathOrientation2 = (CameraPathOrientation)base.GetPoint(lastPointIndex + 1);
		float percent = cameraPathOrientation.percent;
		float num = cameraPathOrientation2.percent;
		if (percent > num)
		{
			num += 1f;
		}
		float num2 = num - percent;
		float num3 = percentage - percent;
		float num4 = num3 / num2;
		return Quaternion.Lerp(cameraPathOrientation.rotation, cameraPathOrientation2.rotation, num4);
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x000288A8 File Offset: 0x00026AA8
	private Quaternion SmootStepInterpolation(float percentage)
	{
		int lastPointIndex = base.GetLastPointIndex(percentage);
		CameraPathOrientation cameraPathOrientation = (CameraPathOrientation)base.GetPoint(lastPointIndex);
		CameraPathOrientation cameraPathOrientation2 = (CameraPathOrientation)base.GetPoint(lastPointIndex + 1);
		float percent = cameraPathOrientation.percent;
		float num = cameraPathOrientation2.percent;
		if (percent > num)
		{
			num += 1f;
		}
		float num2 = num - percent;
		float num3 = percentage - percent;
		float val = num3 / num2;
		return Quaternion.Lerp(cameraPathOrientation.rotation, cameraPathOrientation2.rotation, CPMath.SmoothStep(val));
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00028928 File Offset: 0x00026B28
	private Quaternion CubicInterpolation(float percentage)
	{
		int lastPointIndex = base.GetLastPointIndex(percentage);
		CameraPathOrientation cameraPathOrientation = (CameraPathOrientation)base.GetPoint(lastPointIndex);
		CameraPathOrientation cameraPathOrientation2 = (CameraPathOrientation)base.GetPoint(lastPointIndex + 1);
		CameraPathOrientation cameraPathOrientation3 = (CameraPathOrientation)base.GetPoint(lastPointIndex - 1);
		CameraPathOrientation cameraPathOrientation4 = (CameraPathOrientation)base.GetPoint(lastPointIndex + 2);
		float percent = cameraPathOrientation.percent;
		float num = cameraPathOrientation2.percent;
		if (percent > num)
		{
			num += 1f;
		}
		float num2 = num - percent;
		float num3 = percentage - percent;
		float t = num3 / num2;
		Quaternion result = CPMath.CalculateCubic(cameraPathOrientation.rotation, cameraPathOrientation3.rotation, cameraPathOrientation4.rotation, cameraPathOrientation2.rotation, t);
		if (float.IsNaN(result.x))
		{
			Debug.Log(string.Concat(new object[]
			{
				percentage,
				" ",
				cameraPathOrientation.fullName,
				" ",
				cameraPathOrientation2.fullName,
				" ",
				cameraPathOrientation3.fullName,
				" ",
				cameraPathOrientation4.fullName
			}));
		}
		return result;
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00028A44 File Offset: 0x00026C44
	protected override void RecalculatePoints()
	{
		base.RecalculatePoints();
		for (int i = 0; i < base.realNumberOfPoints; i++)
		{
			CameraPathOrientation cameraPathOrientation = this[i];
			if (cameraPathOrientation.lookAt != null)
			{
				cameraPathOrientation.rotation = Quaternion.LookRotation(cameraPathOrientation.lookAt.transform.position - cameraPathOrientation.worldPosition);
			}
		}
	}

	// Token: 0x040001FD RID: 509
	public CameraPathOrientationList.Interpolation interpolation = CameraPathOrientationList.Interpolation.Cubic;

	// Token: 0x02000078 RID: 120
	public enum Interpolation
	{
		// Token: 0x040001FF RID: 511
		None,
		// Token: 0x04000200 RID: 512
		Linear,
		// Token: 0x04000201 RID: 513
		SmoothStep,
		// Token: 0x04000202 RID: 514
		Hermite,
		// Token: 0x04000203 RID: 515
		Cubic
	}
}
