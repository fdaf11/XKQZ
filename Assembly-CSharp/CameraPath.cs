using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000054 RID: 84
[ExecuteInEditMode]
public class CameraPath : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600017A RID: 378 RVA: 0x000033A2 File Offset: 0x000015A2
	// (remove) Token: 0x0600017B RID: 379 RVA: 0x000033BB File Offset: 0x000015BB
	public event CameraPath.RecalculateCurvesHandler RecalculateCurvesEvent;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x0600017C RID: 380 RVA: 0x000033D4 File Offset: 0x000015D4
	// (remove) Token: 0x0600017D RID: 381 RVA: 0x000033ED File Offset: 0x000015ED
	public event CameraPath.PathPointAddedHandler PathPointAddedEvent;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600017E RID: 382 RVA: 0x00003406 File Offset: 0x00001606
	// (remove) Token: 0x0600017F RID: 383 RVA: 0x0000341F File Offset: 0x0000161F
	public event CameraPath.PathPointRemovedHandler PathPointRemovedEvent;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000180 RID: 384 RVA: 0x00003438 File Offset: 0x00001638
	// (remove) Token: 0x06000181 RID: 385 RVA: 0x00003451 File Offset: 0x00001651
	public event CameraPath.CheckStartPointCullHandler CheckStartPointCullEvent;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06000182 RID: 386 RVA: 0x0000346A File Offset: 0x0000166A
	// (remove) Token: 0x06000183 RID: 387 RVA: 0x00003483 File Offset: 0x00001683
	public event CameraPath.CheckEndPointCullHandler CheckEndPointCullEvent;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000184 RID: 388 RVA: 0x0000349C File Offset: 0x0000169C
	// (remove) Token: 0x06000185 RID: 389 RVA: 0x000034B5 File Offset: 0x000016B5
	public event CameraPath.CleanUpListsHandler CleanUpListsEvent;

	// Token: 0x1700000E RID: 14
	public CameraPathControlPoint this[int index]
	{
		get
		{
			int count = this._points.Count;
			if (this._looped)
			{
				if (this.shouldInterpolateNextPath)
				{
					if (index == count)
					{
						index = 0;
					}
					else
					{
						if (index > count)
						{
							return this._nextPath[index % count];
						}
						if (index < 0)
						{
							Debug.LogError("Index out of range");
						}
					}
				}
				else
				{
					index %= count;
				}
			}
			else
			{
				if (index < 0)
				{
					Debug.LogError("Index can't be minus");
				}
				if (index >= this._points.Count)
				{
					if (index >= this._points.Count && this.shouldInterpolateNextPath)
					{
						return this.nextPath[index % count];
					}
					Debug.LogError("Index out of range");
				}
			}
			return this._points[index];
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000187 RID: 391 RVA: 0x000258C8 File Offset: 0x00023AC8
	public int numberOfPoints
	{
		get
		{
			if (this._points.Count == 0)
			{
				return 0;
			}
			int num = (!this._looped) ? this._points.Count : (this._points.Count + 1);
			if (this.shouldInterpolateNextPath)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000188 RID: 392 RVA: 0x000034CE File Offset: 0x000016CE
	public int realNumberOfPoints
	{
		get
		{
			return this._points.Count;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000189 RID: 393 RVA: 0x000034DB File Offset: 0x000016DB
	public int numberOfCurves
	{
		get
		{
			if (this._points.Count < 2)
			{
				return 0;
			}
			return this.numberOfPoints - 1;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x0600018A RID: 394 RVA: 0x000034F8 File Offset: 0x000016F8
	// (set) Token: 0x0600018B RID: 395 RVA: 0x00003500 File Offset: 0x00001700
	public bool loop
	{
		get
		{
			return this._looped;
		}
		set
		{
			if (this._looped != value)
			{
				this._looped = value;
				this.RecalculateStoredValues();
			}
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600018C RID: 396 RVA: 0x0000351B File Offset: 0x0000171B
	public float pathLength
	{
		get
		{
			return this._storedTotalArcLength;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x0600018D RID: 397 RVA: 0x00003523 File Offset: 0x00001723
	public CameraPathOrientationList orientationList
	{
		get
		{
			return this._orientationList;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600018E RID: 398 RVA: 0x0000352B File Offset: 0x0000172B
	public CameraPathFOVList fovList
	{
		get
		{
			return this._fovList;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600018F RID: 399 RVA: 0x00003533 File Offset: 0x00001733
	public CameraPathTiltList tiltList
	{
		get
		{
			return this._tiltList;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000190 RID: 400 RVA: 0x0000353B File Offset: 0x0000173B
	public CameraPathSpeedList speedList
	{
		get
		{
			return this._speedList;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000191 RID: 401 RVA: 0x00003543 File Offset: 0x00001743
	public CameraPathEventList eventList
	{
		get
		{
			return this._eventList;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000192 RID: 402 RVA: 0x0000354B File Offset: 0x0000174B
	public CameraPathDelayList delayList
	{
		get
		{
			return this._delayList;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000193 RID: 403 RVA: 0x00003553 File Offset: 0x00001753
	public Bounds bounds
	{
		get
		{
			return this._pathBounds;
		}
	}

	// Token: 0x06000194 RID: 404 RVA: 0x00025920 File Offset: 0x00023B20
	public float StoredArcLength(int curve)
	{
		if (this._looped)
		{
			curve %= this.numberOfCurves - 1;
		}
		else
		{
			curve = Mathf.Clamp(curve, 0, this.numberOfCurves - 1);
		}
		curve = Mathf.Clamp(curve, 0, this._storedArcLengths.Length - 1);
		return this._storedArcLengths[curve];
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000195 RID: 405 RVA: 0x0000355B File Offset: 0x0000175B
	public int storedValueArraySize
	{
		get
		{
			return this._storedValueArraySize;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000196 RID: 406 RVA: 0x00003563 File Offset: 0x00001763
	public CameraPathControlPoint[] pointALink
	{
		get
		{
			return this._pointALink;
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000197 RID: 407 RVA: 0x0000356B File Offset: 0x0000176B
	public CameraPathControlPoint[] pointBLink
	{
		get
		{
			return this._pointBLink;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000198 RID: 408 RVA: 0x00003573 File Offset: 0x00001773
	public Vector3[] storedPoints
	{
		get
		{
			return this._storedPoints;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000199 RID: 409 RVA: 0x0000357B File Offset: 0x0000177B
	// (set) Token: 0x0600019A RID: 410 RVA: 0x00003583 File Offset: 0x00001783
	public bool normalised
	{
		get
		{
			return this._normalised;
		}
		set
		{
			this._normalised = value;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600019B RID: 411 RVA: 0x0000358C File Offset: 0x0000178C
	// (set) Token: 0x0600019C RID: 412 RVA: 0x00003594 File Offset: 0x00001794
	public CameraPath.Interpolation interpolation
	{
		get
		{
			return this._interpolation;
		}
		set
		{
			if (value != this._interpolation)
			{
				this._interpolation = value;
				this.RecalculateStoredValues();
			}
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600019D RID: 413 RVA: 0x000035AF File Offset: 0x000017AF
	// (set) Token: 0x0600019E RID: 414 RVA: 0x00025978 File Offset: 0x00023B78
	public CameraPath nextPath
	{
		get
		{
			return this._nextPath;
		}
		set
		{
			if (value != this._nextPath)
			{
				if (value == this)
				{
					Debug.LogError("Do not link a path to itself! The Universe would crumble and it would be your fault!! If you want to loop a path, just toggle the loop option...");
					return;
				}
				this._nextPath = value;
				this._nextPath.GetComponent<CameraPathAnimator>().playOnStart = false;
				this.RecalculateStoredValues();
			}
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x0600019F RID: 415 RVA: 0x000035B7 File Offset: 0x000017B7
	// (set) Token: 0x060001A0 RID: 416 RVA: 0x000035BF File Offset: 0x000017BF
	public bool interpolateNextPath
	{
		get
		{
			return this._interpolateNextPath;
		}
		set
		{
			if (this._interpolateNextPath != value)
			{
				this._interpolateNextPath = value;
				this.RecalculateStoredValues();
			}
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060001A1 RID: 417 RVA: 0x000035DA File Offset: 0x000017DA
	public bool shouldInterpolateNextPath
	{
		get
		{
			return this.nextPath != null && this.interpolateNextPath;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060001A2 RID: 418 RVA: 0x000035F6 File Offset: 0x000017F6
	// (set) Token: 0x060001A3 RID: 419 RVA: 0x000035FE File Offset: 0x000017FE
	public float storedPointResolution
	{
		get
		{
			return this._storedPointResolution;
		}
		set
		{
			this._storedPointResolution = Mathf.Clamp(value, this._storedTotalArcLength / 10000f, 10f);
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000361D File Offset: 0x0000181D
	// (set) Token: 0x060001A5 RID: 421 RVA: 0x00003625 File Offset: 0x00001825
	public float directionWidth
	{
		get
		{
			return this._directionWidth;
		}
		set
		{
			this._directionWidth = value;
		}
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x000259CC File Offset: 0x00023BCC
	public int StoredValueIndex(float percentage)
	{
		int num = this.storedValueArraySize - 1;
		return Mathf.Clamp(Mathf.RoundToInt((float)num * percentage), 0, num);
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x000259F4 File Offset: 0x00023BF4
	public CameraPathControlPoint AddPoint(Vector3 position)
	{
		CameraPathControlPoint cameraPathControlPoint = base.gameObject.AddComponent<CameraPathControlPoint>();
		cameraPathControlPoint.hideFlags = 2;
		cameraPathControlPoint.localPosition = position;
		this._points.Add(cameraPathControlPoint);
		if (this._addOrientationsWithPoints)
		{
			this.orientationList.AddOrientation(cameraPathControlPoint);
		}
		this.RecalculateStoredValues();
		this.PathPointAddedEvent(cameraPathControlPoint);
		return cameraPathControlPoint;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000362E File Offset: 0x0000182E
	public void AddPoint(CameraPathControlPoint point)
	{
		this._points.Add(point);
		this.RecalculateStoredValues();
		this.PathPointAddedEvent(point);
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000364E File Offset: 0x0000184E
	public void InsertPoint(CameraPathControlPoint point, int index)
	{
		this._points.Insert(index, point);
		this.RecalculateStoredValues();
		this.PathPointAddedEvent(point);
	}

	// Token: 0x060001AA RID: 426 RVA: 0x00025A54 File Offset: 0x00023C54
	public CameraPathControlPoint InsertPoint(int index)
	{
		CameraPathControlPoint cameraPathControlPoint = base.gameObject.AddComponent<CameraPathControlPoint>();
		cameraPathControlPoint.hideFlags = 2;
		this._points.Insert(index, cameraPathControlPoint);
		this.RecalculateStoredValues();
		this.PathPointAddedEvent(cameraPathControlPoint);
		return cameraPathControlPoint;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0000366F File Offset: 0x0000186F
	public void RemovePoint(int index)
	{
		this.RemovePoint(this[index]);
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00025A94 File Offset: 0x00023C94
	public bool RemovePoint(string pointName)
	{
		foreach (CameraPathControlPoint cameraPathControlPoint in this._points)
		{
			if (cameraPathControlPoint.displayName == pointName)
			{
				this.RemovePoint(cameraPathControlPoint);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060001AD RID: 429 RVA: 0x00025B0C File Offset: 0x00023D0C
	public void RemovePoint(Vector3 pointPosition)
	{
		foreach (CameraPathControlPoint cameraPathControlPoint in this._points)
		{
			if (cameraPathControlPoint.worldPosition == pointPosition)
			{
				this.RemovePoint(cameraPathControlPoint);
			}
		}
		float nearestPoint = this.GetNearestPoint(pointPosition, true);
		this.RemovePoint(this.GetNearestPointIndex(nearestPoint));
	}

	// Token: 0x060001AE RID: 430 RVA: 0x00025B90 File Offset: 0x00023D90
	public void RemovePoint(CameraPathControlPoint point)
	{
		if (this._points.Count < 3)
		{
			Debug.Log("We can't see any point in allowing you to delete any more points so we're not going to do it.");
			return;
		}
		this.PathPointRemovedEvent(point);
		int num = this._points.IndexOf(point);
		if (num == 0)
		{
			float pathPercentage = this.GetPathPercentage(1);
			this.CheckStartPointCullEvent(pathPercentage);
		}
		if (num == this.realNumberOfPoints - 1)
		{
			float pathPercentage2 = this.GetPathPercentage(this.realNumberOfPoints - 2);
			this.CheckEndPointCullEvent(pathPercentage2);
		}
		this._points.Remove(point);
		this.RecalculateStoredValues();
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00025C28 File Offset: 0x00023E28
	private float ParsePercentage(float percentage)
	{
		if (percentage == 0f)
		{
			return 0f;
		}
		if (percentage == 1f)
		{
			return 1f;
		}
		if (this._looped)
		{
			percentage %= 1f;
		}
		else
		{
			percentage = Mathf.Clamp01(percentage);
		}
		if (this._normalised)
		{
			int num = this.storedValueArraySize - 1;
			float num2 = 1f / (float)this.storedValueArraySize;
			int num3 = Mathf.Clamp(Mathf.FloorToInt((float)this.storedValueArraySize * percentage), 0, num);
			int num4 = Mathf.Clamp(num3 + 1, 0, num);
			float num5 = (float)num3 * num2;
			float num6 = (float)num4 * num2;
			float num7 = this._normalisedPercentages[num3];
			float num8 = this._normalisedPercentages[num4];
			if (num7 == num8)
			{
				return num7;
			}
			float num9 = (percentage - num5) / (num6 - num5);
			percentage = Mathf.Lerp(num7, num8, num9);
		}
		return percentage;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00025D04 File Offset: 0x00023F04
	public float CalculateNormalisedPercentage(float percentage)
	{
		if (this.realNumberOfPoints < 2)
		{
			return percentage;
		}
		if (percentage <= 0f)
		{
			return 0f;
		}
		if (percentage >= 1f)
		{
			return 1f;
		}
		if (this._storedTotalArcLength == 0f)
		{
			return percentage;
		}
		float num = percentage * this._storedTotalArcLength;
		int i = 0;
		int num2 = this.storedValueArraySize - 1;
		int num3 = 0;
		while (i < num2)
		{
			num3 = i + (num2 - i) / 2;
			if (this._storedArcLengthsFull[num3] < num)
			{
				i = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		if (this._storedArcLengthsFull[num3] > num && num3 > 0)
		{
			num3--;
		}
		float num4 = this._storedArcLengthsFull[num3];
		float result = (float)num3 / (float)(this.storedValueArraySize - 1);
		if (num4 == num)
		{
			return result;
		}
		return ((float)num3 + (num - num4) / (this._storedArcLengthsFull[num3 + 1] - num4)) / (float)this.storedValueArraySize;
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00025DF0 File Offset: 0x00023FF0
	public float DeNormalisePercentage(float normalisedPercent)
	{
		int num = this._normalisedPercentages.Length;
		int i = 0;
		while (i < num)
		{
			if (this._normalisedPercentages[i] > normalisedPercent)
			{
				if (i == 0)
				{
					return 0f;
				}
				float num2 = (float)(i - 1) / (float)num;
				float num3 = (float)i / (float)num;
				float num4 = this._normalisedPercentages[i - 1];
				float num5 = this._normalisedPercentages[i];
				float num6 = (normalisedPercent - num4) / (num5 - num4);
				return Mathf.Lerp(num2, num3, num6);
			}
			else
			{
				i++;
			}
		}
		return 1f;
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00025E74 File Offset: 0x00024074
	public int GetPointNumber(float percentage)
	{
		percentage = this.ParsePercentage(percentage);
		float num = 1f / (float)this.numberOfCurves;
		return Mathf.Clamp(Mathf.FloorToInt(percentage / num), 0, this._points.Count - 1);
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0000367E File Offset: 0x0000187E
	public Vector3 GetPathPosition(float percentage)
	{
		return this.GetPathPosition(percentage, false);
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00025EB4 File Offset: 0x000240B4
	public Vector3 GetPathPosition(float percentage, bool ignoreNormalisation)
	{
		if (this.realNumberOfPoints < 2)
		{
			Debug.LogError("Not enough points to define a curve");
			if (this.realNumberOfPoints == 1)
			{
				return this._points[0].worldPosition;
			}
			return Vector3.zero;
		}
		else
		{
			if (!ignoreNormalisation)
			{
				percentage = this.ParsePercentage(percentage);
			}
			float num = 1f / (float)this.numberOfCurves;
			int num2 = Mathf.FloorToInt(percentage / num);
			float num3 = Mathf.Clamp01((percentage - (float)num2 * num) * (float)this.numberOfCurves);
			CameraPathControlPoint point = this.GetPoint(num2);
			CameraPathControlPoint point2 = this.GetPoint(num2 + 1);
			if (point == null || point2 == null)
			{
				return Vector3.zero;
			}
			switch (this.interpolation)
			{
			case CameraPath.Interpolation.Linear:
				return Vector3.Lerp(point.worldPosition, point2.worldPosition, num3);
			case CameraPath.Interpolation.SmoothStep:
				return Vector3.Lerp(point.worldPosition, point2.worldPosition, CPMath.SmoothStep(num3));
			case CameraPath.Interpolation.CatmullRom:
			{
				CameraPathControlPoint point3 = this.GetPoint(num2 - 1);
				CameraPathControlPoint point4 = this.GetPoint(num2 + 2);
				return CPMath.CalculateCatmullRom(point3.worldPosition, point.worldPosition, point2.worldPosition, point4.worldPosition, num3);
			}
			case CameraPath.Interpolation.Hermite:
			{
				CameraPathControlPoint point3 = this.GetPoint(num2 - 1);
				CameraPathControlPoint point4 = this.GetPoint(num2 + 2);
				return CPMath.CalculateHermite(point3.worldPosition, point.worldPosition, point2.worldPosition, point4.worldPosition, num3, this.hermiteTension, this.hermiteBias);
			}
			case CameraPath.Interpolation.Bezier:
				return CPMath.CalculateBezier(num3, point.worldPosition, point.forwardControlPointWorld, point2.backwardControlPointWorld, point2.worldPosition);
			default:
				return Vector3.zero;
			}
		}
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00003688 File Offset: 0x00001888
	public Quaternion GetPathRotation(float percentage, bool ignoreNormalisation)
	{
		if (!ignoreNormalisation)
		{
			percentage = this.ParsePercentage(percentage);
		}
		return this.orientationList.GetOrientation(percentage);
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x000036A5 File Offset: 0x000018A5
	public Vector3 GetPathDirection(float percentage)
	{
		return this.GetPathDirection(percentage, true);
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00026060 File Offset: 0x00024260
	public Vector3 GetPathDirection(float percentage, bool normalisePercent)
	{
		int num = this.storedValueArraySize - 1;
		int num2 = Mathf.Clamp(Mathf.FloorToInt((float)num * percentage), 0, num);
		int num3 = Mathf.Clamp(Mathf.CeilToInt((float)num * percentage), 0, num);
		if (num2 == num3)
		{
			return this._storedPathDirections[num2];
		}
		float num4 = (float)num2 / (float)this.storedValueArraySize;
		float num5 = (float)num3 / (float)this.storedValueArraySize;
		float num6 = (percentage - num4) / (num5 - num4);
		Vector3 vector = this._storedPathDirections[num2];
		Vector3 vector2 = this._storedPathDirections[num3];
		return Vector3.Lerp(vector, vector2, num6);
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x000036AF File Offset: 0x000018AF
	public float GetPathTilt(float percentage)
	{
		percentage = this.ParsePercentage(percentage);
		return this._tiltList.GetTilt(percentage);
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x000036C6 File Offset: 0x000018C6
	public float GetPathFOV(float percentage)
	{
		percentage = this.ParsePercentage(percentage);
		return this._fovList.GetValue(percentage, CameraPathFOVList.ProjectionType.FOV);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x000036DE File Offset: 0x000018DE
	public float GetPathOrthographicSize(float percentage)
	{
		percentage = this.ParsePercentage(percentage);
		return this._fovList.GetValue(percentage, CameraPathFOVList.ProjectionType.Orthographic);
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00026104 File Offset: 0x00024304
	public float GetPathSpeed(float percentage)
	{
		float speed = this._speedList.GetSpeed(percentage);
		return speed * this._delayList.CheckEase(percentage);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00026130 File Offset: 0x00024330
	public float GetPathEase(float percentage)
	{
		percentage = this.ParsePercentage(percentage);
		return this._delayList.CheckEase(percentage);
	}

	// Token: 0x060001BD RID: 445 RVA: 0x000036F6 File Offset: 0x000018F6
	public void CheckEvents(float percentage)
	{
		percentage = this.ParsePercentage(percentage);
		this._eventList.CheckEvents(percentage);
		this._delayList.CheckEvents(percentage);
	}

	// Token: 0x060001BE RID: 446 RVA: 0x00026154 File Offset: 0x00024354
	public float GetPathPercentage(CameraPathControlPoint point)
	{
		int num = this._points.IndexOf(point);
		return (float)num / (float)this.numberOfCurves;
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00003719 File Offset: 0x00001919
	public float GetPathPercentage(int pointIndex)
	{
		return (float)pointIndex / (float)this.numberOfCurves;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00003725 File Offset: 0x00001925
	public int GetNearestPointIndex(float percentage)
	{
		percentage = this.ParsePercentage(percentage);
		return Mathf.RoundToInt((float)this.numberOfCurves * percentage);
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000373E File Offset: 0x0000193E
	public int GetLastPointIndex(float percentage, bool isNormalised)
	{
		if (isNormalised)
		{
			percentage = this.ParsePercentage(percentage);
		}
		return Mathf.FloorToInt((float)this.numberOfCurves * percentage);
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000375D File Offset: 0x0000195D
	public int GetNextPointIndex(float percentage, bool isNormalised)
	{
		if (isNormalised)
		{
			percentage = this.ParsePercentage(percentage);
		}
		return Mathf.CeilToInt((float)this.numberOfCurves * percentage);
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00026178 File Offset: 0x00024378
	public float GetCurvePercentage(CameraPathControlPoint pointA, CameraPathControlPoint pointB, float percentage)
	{
		float num = this.GetPathPercentage(pointA);
		float num2 = this.GetPathPercentage(pointB);
		if (num == num2)
		{
			return num;
		}
		if (num > num2)
		{
			float num3 = num2;
			num2 = num;
			num = num3;
		}
		return Mathf.Clamp01((percentage - num) / (num2 - num));
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x000261B8 File Offset: 0x000243B8
	public float GetCurvePercentage(CameraPathPoint pointA, CameraPathPoint pointB, float percentage)
	{
		float num = pointA.percent;
		float num2 = pointB.percent;
		if (num > num2)
		{
			float num3 = num2;
			num2 = num;
			num = num3;
		}
		return Mathf.Clamp01((percentage - num) / (num2 - num));
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x000261EC File Offset: 0x000243EC
	public float GetCurvePercentage(CameraPathPoint point)
	{
		float num = this.GetPathPercentage(point.cpointA);
		float num2 = this.GetPathPercentage(point.cpointB);
		if (num > num2)
		{
			float num3 = num2;
			num2 = num;
			num = num3;
		}
		point.curvePercentage = Mathf.Clamp01((point.percent - num) / (num2 - num));
		return point.curvePercentage;
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00026240 File Offset: 0x00024440
	public float GetOutroEasePercentage(CameraPathDelay point)
	{
		float num = point.percent;
		float num2 = this._delayList.GetPoint(point.index + 1).percent;
		if (num > num2)
		{
			float num3 = num2;
			num2 = num;
			num = num3;
		}
		return Mathf.Lerp(num, num2, point.outroEndEasePercentage);
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00026288 File Offset: 0x00024488
	public float GetIntroEasePercentage(CameraPathDelay point)
	{
		float percent = this._delayList.GetPoint(point.index - 1).percent;
		float percent2 = point.percent;
		return Mathf.Lerp(percent, percent2, 1f - point.introStartEasePercentage);
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x000262C8 File Offset: 0x000244C8
	public float GetPathPercentage(CameraPathControlPoint pointA, CameraPathControlPoint pointB, float curvePercentage)
	{
		float pathPercentage = this.GetPathPercentage(pointA);
		float pathPercentage2 = this.GetPathPercentage(pointB);
		return Mathf.Lerp(pathPercentage, pathPercentage2, curvePercentage);
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000377C File Offset: 0x0000197C
	public float GetPathPercentage(float pointA, float pointB, float curvePercentage)
	{
		return Mathf.Lerp(pointA, pointB, curvePercentage);
	}

	// Token: 0x060001CA RID: 458 RVA: 0x000262F0 File Offset: 0x000244F0
	public int GetStoredPoint(float percentage)
	{
		percentage = this.ParsePercentage(percentage);
		return Mathf.Clamp(Mathf.FloorToInt((float)this.storedValueArraySize * percentage), 0, this.storedValueArraySize - 1);
	}

	// Token: 0x060001CB RID: 459 RVA: 0x00003786 File Offset: 0x00001986
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x060001CC RID: 460 RVA: 0x00026324 File Offset: 0x00024524
	private void Start()
	{
		if (!Application.isPlaying)
		{
			if (this.version == CameraPath.CURRENT_VERSION_NUMBER)
			{
				return;
			}
			if (this.version > CameraPath.CURRENT_VERSION_NUMBER)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Camera Path v.",
					this.version,
					": Great scot! This data is from the future! (version:",
					CameraPath.CURRENT_VERSION_NUMBER,
					") - need to avoid contact to ensure the survival of the universe..."
				}));
				return;
			}
			Debug.Log(string.Concat(new object[]
			{
				"Camera Path v.",
				this.version,
				" Upgrading to version ",
				CameraPath.CURRENT_VERSION_NUMBER,
				"\nRemember to backup your data!"
			}));
			this.version = CameraPath.CURRENT_VERSION_NUMBER;
		}
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000378E File Offset: 0x0000198E
	private void OnValidate()
	{
		this.InitialiseLists();
		if (!Application.isPlaying)
		{
			this.RecalculateStoredValues();
		}
	}

	// Token: 0x060001CE RID: 462 RVA: 0x000037A6 File Offset: 0x000019A6
	private void OnDestroy()
	{
		this.Clear();
		if (this.CleanUpListsEvent != null)
		{
			this.CleanUpListsEvent();
		}
	}

	// Token: 0x060001CF RID: 463 RVA: 0x000263F0 File Offset: 0x000245F0
	public void RecalculateStoredValues()
	{
		if (this.autoSetStoedPointRes && this._storedTotalArcLength > 0f)
		{
			this._storedPointResolution = this._storedTotalArcLength / 1000f;
		}
		for (int i = 0; i < this.realNumberOfPoints; i++)
		{
			CameraPathControlPoint cameraPathControlPoint = this._points[i];
			cameraPathControlPoint.percentage = this.GetPathPercentage(i);
			cameraPathControlPoint.normalisedPercentage = this.CalculateNormalisedPercentage(this._points[i].percentage);
			cameraPathControlPoint.givenName = "Point " + i;
			cameraPathControlPoint.fullName = base.name + " Point " + i;
			cameraPathControlPoint.index = i;
			cameraPathControlPoint.hideFlags = 2;
		}
		if (this._points.Count < 2)
		{
			return;
		}
		this._storedTotalArcLength = 0f;
		for (int j = 0; j < this.numberOfCurves; j++)
		{
			CameraPathControlPoint point = this.GetPoint(j);
			CameraPathControlPoint point2 = this.GetPoint(j + 1);
			float num = 0f;
			num += Vector3.Distance(point.worldPosition, point.forwardControlPointWorld);
			num += Vector3.Distance(point.forwardControlPointWorld, point2.backwardControlPointWorld);
			num += Vector3.Distance(point2.backwardControlPointWorld, point2.worldPosition);
			this._storedTotalArcLength += num;
		}
		this._storedValueArraySize = Mathf.Max(Mathf.RoundToInt(this._storedTotalArcLength / this._storedPointResolution), 1);
		float num2 = 1f / (float)(this._storedValueArraySize * 10);
		float num3 = 0f;
		float num4 = this._storedTotalArcLength / (float)(this._storedValueArraySize - 1);
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		List<float> list3 = new List<float>();
		List<float> list4 = new List<float>();
		List<float> list5 = new List<float>();
		float num5 = 0f;
		float num6 = num4;
		float num7 = 0f;
		Vector3 vector = this.GetPathPosition(0f, true);
		list.Add(vector);
		list2.Add((this.GetPathPosition(num2, true) - vector).normalized);
		list3.Add(0f);
		while (num3 < 1f)
		{
			Vector3 pathPosition = this.GetPathPosition(num3, true);
			float num8 = Vector3.Distance(vector, pathPosition);
			if (num5 + num8 >= num6)
			{
				float num9 = Mathf.Clamp01((num6 - num5) / num8);
				float num10 = Mathf.Lerp(num3, num3 + num2, num9);
				list3.Add(num10);
				list.Add(pathPosition);
				float percentage = Mathf.Clamp(num3 - this._directionWidth, 0f, 1f);
				Vector3 pathPosition2 = this.GetPathPosition(percentage, true);
				float percentage2 = Mathf.Clamp(num3 + this._directionWidth, 0f, 1f);
				Vector3 pathPosition3 = this.GetPathPosition(percentage2, true);
				Vector3 normalized = (vector - pathPosition2 + (pathPosition3 - vector)).normalized;
				list2.Add(normalized);
				list4.Add(num5);
				list5.Add(num7);
				num5 = num6;
				num6 += num4;
			}
			num5 += num8;
			num7 += num8;
			vector = pathPosition;
			num3 += num2;
		}
		list3.Add(1f);
		list.Add(this.GetPathPosition(1f, true));
		Vector3 pathPosition4 = this.GetPathPosition(1f, true);
		Vector3 pathPosition5 = this.GetPathPosition(1f - num2, true);
		Vector3 normalized2 = (pathPosition4 - pathPosition5).normalized;
		list2.Add(normalized2);
		this._storedValueArraySize = list3.Count;
		this._normalisedPercentages = list3.ToArray();
		this._storedTotalArcLength = num7;
		this._storedPoints = list.ToArray();
		this._storedPathDirections = list2.ToArray();
		this._storedArcLengths = list4.ToArray();
		this._storedArcLengthsFull = list5.ToArray();
		if (this.RecalculateCurvesEvent != null)
		{
			this.RecalculateCurvesEvent();
		}
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x000037C4 File Offset: 0x000019C4
	public float GetNearestPoint(Vector3 fromPostition)
	{
		return this.GetNearestPoint(fromPostition, false, 4);
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x000037CF File Offset: 0x000019CF
	public float GetNearestPoint(Vector3 fromPostition, bool ignoreNormalisation)
	{
		return this.GetNearestPoint(fromPostition, ignoreNormalisation, 4);
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x000267F4 File Offset: 0x000249F4
	public float GetNearestPoint(Vector3 fromPostition, bool ignoreNormalisation, int refinments)
	{
		int num = 10;
		float num2 = 1f / (float)num;
		float num3 = 0f;
		float num4 = float.PositiveInfinity;
		for (float num5 = 0f; num5 < 1f; num5 += num2)
		{
			Vector3 pathPosition = this.GetPathPosition(num5, ignoreNormalisation);
			Vector3 vector = pathPosition - fromPostition;
			float num6 = Vector3.SqrMagnitude(vector);
			if (num4 > num6)
			{
				num3 = num5;
				num4 = num6;
			}
		}
		float num7 = num3;
		float num8 = num4;
		for (int i = 0; i < refinments; i++)
		{
			float num9 = num2 / 1.8f;
			float num10 = num3 - num9;
			float num11 = num3 + num9;
			float num12 = num2 / (float)num;
			for (float num13 = num10; num13 < num11; num13 += num12)
			{
				float num14 = num13 % 1f;
				if (num14 < 0f)
				{
					num14 += 1f;
				}
				Vector3 pathPosition2 = this.GetPathPosition(num14, ignoreNormalisation);
				Vector3 vector2 = pathPosition2 - fromPostition;
				float num15 = Vector3.SqrMagnitude(vector2);
				if (num4 > num15)
				{
					num7 = num3;
					num8 = num4;
					num3 = num14;
					num4 = num15;
				}
				else if (num8 > num15)
				{
					num7 = num14;
					num8 = num15;
				}
			}
			num2 = num12;
		}
		float num16 = num4 / (num4 + num8);
		return Mathf.Clamp01(Mathf.Lerp(num3, num7, num16));
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0002694C File Offset: 0x00024B4C
	public float GetNearestPointNear(Vector3 fromPostition, float prevPercentage, Vector3 prevPosition, bool ignoreNormalisation, int refinments)
	{
		int num = 10;
		float num2 = 1f / (float)num;
		float num3 = prevPercentage;
		float num4 = num3;
		float num5 = Vector3.SqrMagnitude(prevPosition - fromPostition);
		float num6 = num5;
		for (int i = 0; i < refinments; i++)
		{
			float num7 = num2 / 1.8f;
			float num8 = num3 - num7;
			float num9 = num3 + num7;
			float num10 = num2 / (float)num;
			for (float num11 = num8; num11 < num9; num11 += num10)
			{
				float num12 = num11 % 1f;
				if (num12 < 0f)
				{
					num12 += 1f;
				}
				Vector3 pathPosition = this.GetPathPosition(num12, ignoreNormalisation);
				Vector3 vector = pathPosition - fromPostition;
				float num13 = Vector3.SqrMagnitude(vector);
				if (num5 > num13)
				{
					num4 = num3;
					num6 = num5;
					num3 = num12;
					num5 = num13;
				}
				else if (num6 > num13)
				{
					num4 = num12;
					num6 = num13;
				}
			}
			num2 = num10;
		}
		float num14 = num5 / (num5 + num6);
		return Mathf.Clamp01(Mathf.Lerp(num3, num4, num14));
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x000037DA File Offset: 0x000019DA
	public void Clear()
	{
		this._points.Clear();
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x000037E7 File Offset: 0x000019E7
	public CameraPathControlPoint GetPoint(int index)
	{
		return this[this.GetPointIndex(index)];
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00026A50 File Offset: 0x00024C50
	public int GetPointIndex(int index)
	{
		if (this._points.Count == 0)
		{
			return -1;
		}
		if (!this._looped)
		{
			return Mathf.Clamp(index, 0, this.numberOfCurves);
		}
		if (index >= this.numberOfCurves)
		{
			index -= this.numberOfCurves;
		}
		if (index < 0)
		{
			index += this.numberOfCurves;
		}
		return index;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00026AB0 File Offset: 0x00024CB0
	public int GetCurveIndex(int startPointIndex)
	{
		if (this._points.Count == 0)
		{
			return -1;
		}
		if (!this._looped)
		{
			return Mathf.Clamp(startPointIndex, 0, this.numberOfCurves - 1);
		}
		if (startPointIndex >= this.numberOfCurves - 1)
		{
			startPointIndex = startPointIndex - this.numberOfCurves - 1;
		}
		if (startPointIndex < 0)
		{
			startPointIndex = startPointIndex + this.numberOfCurves - 1;
		}
		return startPointIndex;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00026B18 File Offset: 0x00024D18
	private void Init()
	{
		this.InitialiseLists();
		if (this.initialised)
		{
			return;
		}
		CameraPathControlPoint cameraPathControlPoint = base.gameObject.AddComponent<CameraPathControlPoint>();
		CameraPathControlPoint cameraPathControlPoint2 = base.gameObject.AddComponent<CameraPathControlPoint>();
		CameraPathControlPoint cameraPathControlPoint3 = base.gameObject.AddComponent<CameraPathControlPoint>();
		CameraPathControlPoint cameraPathControlPoint4 = base.gameObject.AddComponent<CameraPathControlPoint>();
		cameraPathControlPoint.hideFlags = 2;
		cameraPathControlPoint2.hideFlags = 2;
		cameraPathControlPoint3.hideFlags = 2;
		cameraPathControlPoint4.hideFlags = 2;
		cameraPathControlPoint.localPosition = new Vector3(-20f, 0f, -20f);
		cameraPathControlPoint2.localPosition = new Vector3(20f, 0f, -20f);
		cameraPathControlPoint3.localPosition = new Vector3(20f, 0f, 20f);
		cameraPathControlPoint4.localPosition = new Vector3(-20f, 0f, 20f);
		cameraPathControlPoint.forwardControlPoint = new Vector3(0f, 0f, -20f);
		cameraPathControlPoint2.forwardControlPoint = new Vector3(40f, 0f, -20f);
		cameraPathControlPoint3.forwardControlPoint = new Vector3(0f, 0f, 20f);
		cameraPathControlPoint4.forwardControlPoint = new Vector3(-40f, 0f, 20f);
		this.AddPoint(cameraPathControlPoint);
		this.AddPoint(cameraPathControlPoint2);
		this.AddPoint(cameraPathControlPoint3);
		this.AddPoint(cameraPathControlPoint4);
		this.initialised = true;
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x00026C78 File Offset: 0x00024E78
	private void InitialiseLists()
	{
		if (this._orientationList == null)
		{
			this._orientationList = base.gameObject.AddComponent<CameraPathOrientationList>();
		}
		if (this._fovList == null)
		{
			this._fovList = base.gameObject.AddComponent<CameraPathFOVList>();
		}
		if (this._tiltList == null)
		{
			this._tiltList = base.gameObject.AddComponent<CameraPathTiltList>();
		}
		if (this._speedList == null)
		{
			this._speedList = base.gameObject.AddComponent<CameraPathSpeedList>();
		}
		if (this._eventList == null)
		{
			this._eventList = base.gameObject.AddComponent<CameraPathEventList>();
		}
		if (this._delayList == null)
		{
			this._delayList = base.gameObject.AddComponent<CameraPathDelayList>();
		}
		this._orientationList.Init(this);
		this._fovList.Init(this);
		this._tiltList.Init(this);
		this._speedList.Init(this);
		this._eventList.Init(this);
		this._delayList.Init(this);
	}

	// Token: 0x04000138 RID: 312
	private const float CLIP_THREASHOLD = 0.5f;

	// Token: 0x04000139 RID: 313
	public static float CURRENT_VERSION_NUMBER = 3.43f;

	// Token: 0x0400013A RID: 314
	public float version = CameraPath.CURRENT_VERSION_NUMBER;

	// Token: 0x0400013B RID: 315
	[SerializeField]
	private List<CameraPathControlPoint> _points = new List<CameraPathControlPoint>();

	// Token: 0x0400013C RID: 316
	[SerializeField]
	private CameraPath.Interpolation _interpolation = CameraPath.Interpolation.Bezier;

	// Token: 0x0400013D RID: 317
	[SerializeField]
	private bool initialised;

	// Token: 0x0400013E RID: 318
	[SerializeField]
	private float _storedTotalArcLength;

	// Token: 0x0400013F RID: 319
	[SerializeField]
	private float[] _storedArcLengths;

	// Token: 0x04000140 RID: 320
	[SerializeField]
	private float[] _storedArcLengthsFull;

	// Token: 0x04000141 RID: 321
	[SerializeField]
	private Vector3[] _storedPoints;

	// Token: 0x04000142 RID: 322
	[SerializeField]
	private float[] _normalisedPercentages;

	// Token: 0x04000143 RID: 323
	[SerializeField]
	private float _storedPointResolution = 0.1f;

	// Token: 0x04000144 RID: 324
	[SerializeField]
	private int _storedValueArraySize;

	// Token: 0x04000145 RID: 325
	[SerializeField]
	private Vector3[] _storedPathDirections;

	// Token: 0x04000146 RID: 326
	[SerializeField]
	private float _directionWidth = 0.05f;

	// Token: 0x04000147 RID: 327
	[SerializeField]
	private CameraPathControlPoint[] _pointALink;

	// Token: 0x04000148 RID: 328
	[SerializeField]
	private CameraPathControlPoint[] _pointBLink;

	// Token: 0x04000149 RID: 329
	[SerializeField]
	private CameraPathOrientationList _orientationList;

	// Token: 0x0400014A RID: 330
	[SerializeField]
	private CameraPathFOVList _fovList;

	// Token: 0x0400014B RID: 331
	[SerializeField]
	private CameraPathTiltList _tiltList;

	// Token: 0x0400014C RID: 332
	[SerializeField]
	private CameraPathSpeedList _speedList;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	private CameraPathEventList _eventList;

	// Token: 0x0400014E RID: 334
	[SerializeField]
	private CameraPathDelayList _delayList;

	// Token: 0x0400014F RID: 335
	[SerializeField]
	private bool _addOrientationsWithPoints = true;

	// Token: 0x04000150 RID: 336
	[SerializeField]
	private bool _looped;

	// Token: 0x04000151 RID: 337
	[SerializeField]
	private bool _normalised = true;

	// Token: 0x04000152 RID: 338
	[SerializeField]
	private Bounds _pathBounds = default(Bounds);

	// Token: 0x04000153 RID: 339
	public float hermiteTension;

	// Token: 0x04000154 RID: 340
	public float hermiteBias;

	// Token: 0x04000155 RID: 341
	public GameObject editorPreview;

	// Token: 0x04000156 RID: 342
	public int selectedPoint;

	// Token: 0x04000157 RID: 343
	public CameraPath.PointModes pointMode;

	// Token: 0x04000158 RID: 344
	public float addPointAtPercent;

	// Token: 0x04000159 RID: 345
	[SerializeField]
	private CameraPath _nextPath;

	// Token: 0x0400015A RID: 346
	[SerializeField]
	private bool _interpolateNextPath;

	// Token: 0x0400015B RID: 347
	public bool showGizmos = true;

	// Token: 0x0400015C RID: 348
	public Color selectedPathColour = CameraPathColours.GREEN;

	// Token: 0x0400015D RID: 349
	public Color unselectedPathColour = CameraPathColours.GREY;

	// Token: 0x0400015E RID: 350
	public Color selectedPointColour = CameraPathColours.RED;

	// Token: 0x0400015F RID: 351
	public Color unselectedPointColour = CameraPathColours.GREEN;

	// Token: 0x04000160 RID: 352
	public bool showOrientationIndicators;

	// Token: 0x04000161 RID: 353
	public float orientationIndicatorUnitLength = 2.5f;

	// Token: 0x04000162 RID: 354
	public Color orientationIndicatorColours = CameraPathColours.PURPLE;

	// Token: 0x04000163 RID: 355
	public bool autoSetStoedPointRes = true;

	// Token: 0x04000164 RID: 356
	public bool enableUndo = true;

	// Token: 0x04000165 RID: 357
	public bool showPreview = true;

	// Token: 0x04000166 RID: 358
	public bool enablePreviews = true;

	// Token: 0x02000055 RID: 85
	public enum PointModes
	{
		// Token: 0x0400016E RID: 366
		Transform,
		// Token: 0x0400016F RID: 367
		ControlPoints,
		// Token: 0x04000170 RID: 368
		FOV,
		// Token: 0x04000171 RID: 369
		Events,
		// Token: 0x04000172 RID: 370
		Speed,
		// Token: 0x04000173 RID: 371
		Delay,
		// Token: 0x04000174 RID: 372
		Ease,
		// Token: 0x04000175 RID: 373
		Orientations,
		// Token: 0x04000176 RID: 374
		Tilt,
		// Token: 0x04000177 RID: 375
		AddPathPoints,
		// Token: 0x04000178 RID: 376
		RemovePathPoints,
		// Token: 0x04000179 RID: 377
		AddOrientations,
		// Token: 0x0400017A RID: 378
		RemoveOrientations,
		// Token: 0x0400017B RID: 379
		TargetOrientation,
		// Token: 0x0400017C RID: 380
		AddFovs,
		// Token: 0x0400017D RID: 381
		RemoveFovs,
		// Token: 0x0400017E RID: 382
		AddTilts,
		// Token: 0x0400017F RID: 383
		RemoveTilts,
		// Token: 0x04000180 RID: 384
		AddEvents,
		// Token: 0x04000181 RID: 385
		RemoveEvents,
		// Token: 0x04000182 RID: 386
		AddSpeeds,
		// Token: 0x04000183 RID: 387
		RemoveSpeeds,
		// Token: 0x04000184 RID: 388
		AddDelays,
		// Token: 0x04000185 RID: 389
		RemoveDelays,
		// Token: 0x04000186 RID: 390
		Options
	}

	// Token: 0x02000056 RID: 86
	public enum Interpolation
	{
		// Token: 0x04000188 RID: 392
		Linear,
		// Token: 0x04000189 RID: 393
		SmoothStep,
		// Token: 0x0400018A RID: 394
		CatmullRom,
		// Token: 0x0400018B RID: 395
		Hermite,
		// Token: 0x0400018C RID: 396
		Bezier
	}

	// Token: 0x02000057 RID: 87
	// (Invoke) Token: 0x060001DB RID: 475
	public delegate void RecalculateCurvesHandler();

	// Token: 0x02000058 RID: 88
	// (Invoke) Token: 0x060001DF RID: 479
	public delegate void PathPointAddedHandler(CameraPathControlPoint point);

	// Token: 0x02000059 RID: 89
	// (Invoke) Token: 0x060001E3 RID: 483
	public delegate void PathPointRemovedHandler(CameraPathControlPoint point);

	// Token: 0x0200005A RID: 90
	// (Invoke) Token: 0x060001E7 RID: 487
	public delegate void CheckStartPointCullHandler(float percentage);

	// Token: 0x0200005B RID: 91
	// (Invoke) Token: 0x060001EB RID: 491
	public delegate void CheckEndPointCullHandler(float percentage);

	// Token: 0x0200005C RID: 92
	// (Invoke) Token: 0x060001EF RID: 495
	public delegate void CleanUpListsHandler();
}
