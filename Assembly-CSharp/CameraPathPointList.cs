using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007B RID: 123
[ExecuteInEditMode]
public class CameraPathPointList : MonoBehaviour
{
	// Token: 0x060002AE RID: 686 RVA: 0x00003BF2 File Offset: 0x00001DF2
	private void OnEnable()
	{
		base.hideFlags = 2;
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00028B48 File Offset: 0x00026D48
	public virtual void Init(CameraPath _cameraPath)
	{
		if (this.initialised)
		{
			return;
		}
		base.hideFlags = 2;
		this.CheckListIsNull();
		this.cameraPath = _cameraPath;
		this.cameraPath.CleanUpListsEvent += this.CleanUp;
		this.cameraPath.RecalculateCurvesEvent += this.RecalculatePoints;
		this.cameraPath.PathPointRemovedEvent += this.PathPointRemovedEvent;
		this.cameraPath.CheckStartPointCullEvent += this.CheckPointCullEventFromStart;
		this.cameraPath.CheckEndPointCullEvent += this.CheckPointCullEventFromEnd;
		this.initialised = true;
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00028BF4 File Offset: 0x00026DF4
	public virtual void CleanUp()
	{
		this.cameraPath.CleanUpListsEvent -= this.CleanUp;
		this.cameraPath.RecalculateCurvesEvent -= this.RecalculatePoints;
		this.cameraPath.PathPointRemovedEvent -= this.PathPointRemovedEvent;
		this.cameraPath.CheckStartPointCullEvent -= this.CheckPointCullEventFromStart;
		this.cameraPath.CheckEndPointCullEvent -= this.CheckPointCullEventFromEnd;
		this.initialised = false;
	}

	// Token: 0x17000046 RID: 70
	public CameraPathPoint this[int index]
	{
		get
		{
			if (this.cameraPath.loop && index > this._points.Count - 1)
			{
				index %= this._points.Count;
			}
			if (index < 0)
			{
				Debug.LogError("Index can't be minus");
			}
			if (index >= this._points.Count)
			{
				Debug.LogError("Index out of range");
			}
			return this._points[index];
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060002B2 RID: 690 RVA: 0x00028CF8 File Offset: 0x00026EF8
	public int numberOfPoints
	{
		get
		{
			if (this._points.Count == 0)
			{
				return 0;
			}
			return (!this.cameraPath.loop) ? this._points.Count : (this._points.Count + 1);
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000407B File Offset: 0x0000227B
	public int realNumberOfPoints
	{
		get
		{
			return this._points.Count;
		}
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00004088 File Offset: 0x00002288
	public int IndexOf(CameraPathPoint point)
	{
		return this._points.IndexOf(point);
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00004096 File Offset: 0x00002296
	public void AddPoint(CameraPathPoint newPoint, CameraPathControlPoint curvePointA, CameraPathControlPoint curvePointB, float curvePercetage)
	{
		newPoint.positionModes = CameraPathPoint.PositionModes.Free;
		newPoint.cpointA = curvePointA;
		newPoint.cpointB = curvePointB;
		newPoint.curvePercentage = curvePercetage;
		this._points.Add(newPoint);
		this.RecalculatePoints();
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x000040C7 File Offset: 0x000022C7
	public void AddPoint(CameraPathPoint newPoint, float fixPercent)
	{
		newPoint.positionModes = CameraPathPoint.PositionModes.FixedToPercent;
		newPoint.percent = fixPercent;
		this._points.Add(newPoint);
		this.RecalculatePoints();
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x000040E9 File Offset: 0x000022E9
	public void AddPoint(CameraPathPoint newPoint, CameraPathControlPoint atPoint)
	{
		newPoint.positionModes = CameraPathPoint.PositionModes.FixedToPoint;
		newPoint.point = atPoint;
		this._points.Add(newPoint);
		this.RecalculatePoints();
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0000410B File Offset: 0x0000230B
	public void RemovePoint(CameraPathPoint newPoint)
	{
		this._points.Remove(newPoint);
		this.RecalculatePoints();
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00028D44 File Offset: 0x00026F44
	public void PathPointAddedEvent(CameraPathControlPoint addedPoint)
	{
		float percentage = addedPoint.percentage;
		for (int i = 0; i < this.realNumberOfPoints; i++)
		{
			CameraPathPoint cameraPathPoint = this._points[i];
			if (cameraPathPoint.positionModes == CameraPathPoint.PositionModes.Free)
			{
				float percentage2 = cameraPathPoint.cpointA.percentage;
				float percentage3 = cameraPathPoint.cpointB.percentage;
				if (percentage > percentage2 && percentage < percentage3)
				{
					if (percentage < cameraPathPoint.percent)
					{
						cameraPathPoint.cpointA = addedPoint;
					}
					else
					{
						cameraPathPoint.cpointB = addedPoint;
					}
					this.cameraPath.GetCurvePercentage(cameraPathPoint);
				}
			}
		}
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00028DDC File Offset: 0x00026FDC
	public void PathPointRemovedEvent(CameraPathControlPoint removedPathPoint)
	{
		for (int i = 0; i < this.realNumberOfPoints; i++)
		{
			CameraPathPoint cameraPathPoint = this._points[i];
			switch (cameraPathPoint.positionModes)
			{
			case CameraPathPoint.PositionModes.Free:
				if (cameraPathPoint.cpointA == removedPathPoint)
				{
					CameraPathControlPoint point = this.cameraPath.GetPoint(removedPathPoint.index - 1);
					cameraPathPoint.cpointA = point;
					this.cameraPath.GetCurvePercentage(cameraPathPoint);
				}
				if (cameraPathPoint.cpointB == removedPathPoint)
				{
					CameraPathControlPoint point2 = this.cameraPath.GetPoint(removedPathPoint.index + 1);
					cameraPathPoint.cpointB = point2;
					this.cameraPath.GetCurvePercentage(cameraPathPoint);
				}
				break;
			case CameraPathPoint.PositionModes.FixedToPoint:
				if (cameraPathPoint.point == removedPathPoint)
				{
					this._points.Remove(cameraPathPoint);
					i--;
				}
				break;
			}
		}
		this.RecalculatePoints();
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00028ED8 File Offset: 0x000270D8
	public void CheckPointCullEventFromStart(float percent)
	{
		int num = this._points.Count;
		for (int i = 0; i < num; i++)
		{
			CameraPathPoint cameraPathPoint = this._points[i];
			if (cameraPathPoint.positionModes != CameraPathPoint.PositionModes.FixedToPercent)
			{
				if (cameraPathPoint.percent < percent)
				{
					this._points.Remove(cameraPathPoint);
					i--;
					num--;
				}
			}
		}
		this.RecalculatePoints();
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00028F48 File Offset: 0x00027148
	public void CheckPointCullEventFromEnd(float percent)
	{
		int num = this._points.Count;
		for (int i = 0; i < num; i++)
		{
			CameraPathPoint cameraPathPoint = this._points[i];
			if (cameraPathPoint.positionModes != CameraPathPoint.PositionModes.FixedToPercent)
			{
				if (cameraPathPoint.percent > percent)
				{
					this._points.Remove(cameraPathPoint);
					i--;
					num--;
				}
			}
		}
		this.RecalculatePoints();
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00028FB8 File Offset: 0x000271B8
	protected int GetNextPointIndex(float percent)
	{
		if (this.realNumberOfPoints == 0)
		{
			Debug.LogError("No points to draw from");
		}
		if (percent == 0f)
		{
			return 1;
		}
		if (percent == 1f)
		{
			return this._points.Count - 1;
		}
		int count = this._points.Count;
		int num = 0;
		for (int i = 1; i < count; i++)
		{
			if (this._points[i].percent > percent)
			{
				return num + 1;
			}
			num = i;
		}
		return num;
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00029040 File Offset: 0x00027240
	protected int GetLastPointIndex(float percent)
	{
		if (this.realNumberOfPoints == 0)
		{
			Debug.LogError("No points to draw from");
		}
		if (percent == 0f)
		{
			return 0;
		}
		if (percent == 1f)
		{
			return (!this.cameraPath.loop && !this.cameraPath.shouldInterpolateNextPath) ? (this._points.Count - 2) : (this._points.Count - 1);
		}
		int count = this._points.Count;
		int result = 0;
		for (int i = 1; i < count; i++)
		{
			if (this._points[i].percent > percent)
			{
				return result;
			}
			result = i;
		}
		return result;
	}

	// Token: 0x060002BF RID: 703 RVA: 0x000290F8 File Offset: 0x000272F8
	public CameraPathPoint GetPoint(int index)
	{
		int count = this._points.Count;
		if (count == 0)
		{
			return null;
		}
		CameraPathPointList cameraPathPointList = this;
		if (this.cameraPath.shouldInterpolateNextPath)
		{
			string text = this.pointTypeName;
			if (text != null)
			{
				if (CameraPathPointList.<>f__switch$map0 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
					dictionary.Add("Orientation", 0);
					dictionary.Add("FOV", 1);
					dictionary.Add("Tilt", 2);
					CameraPathPointList.<>f__switch$map0 = dictionary;
				}
				int num;
				if (CameraPathPointList.<>f__switch$map0.TryGetValue(text, ref num))
				{
					switch (num)
					{
					case 0:
						cameraPathPointList = this.cameraPath.nextPath.orientationList;
						break;
					case 1:
						cameraPathPointList = this.cameraPath.nextPath.fovList;
						break;
					case 2:
						cameraPathPointList = this.cameraPath.nextPath.tiltList;
						break;
					}
				}
			}
		}
		if (cameraPathPointList == this)
		{
			if (!this.cameraPath.loop)
			{
				return this._points[Mathf.Clamp(index, 0, count - 1)];
			}
			if (index >= count)
			{
				index -= count;
			}
			if (index < 0)
			{
				index += count;
			}
		}
		else if (this.cameraPath.loop)
		{
			if (index == count)
			{
				index = 0;
				cameraPathPointList = null;
			}
			else if (index > count)
			{
				index = Mathf.Clamp(index, 0, cameraPathPointList.realNumberOfPoints - 1);
			}
			else if (index < 0)
			{
				index += count;
				cameraPathPointList = null;
			}
			else
			{
				cameraPathPointList = null;
			}
		}
		else if (index > count - 1)
		{
			index = Mathf.Clamp(index - count, 0, cameraPathPointList.realNumberOfPoints - 1);
		}
		else if (index < 0)
		{
			index = 0;
			cameraPathPointList = null;
		}
		else
		{
			index = Mathf.Clamp(index, 0, count - 1);
			cameraPathPointList = null;
		}
		if (cameraPathPointList != null)
		{
			return cameraPathPointList[index];
		}
		return this._points[index];
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x000292E8 File Offset: 0x000274E8
	public CameraPathPoint GetPoint(CameraPathControlPoint atPoint)
	{
		if (this._points.Count == 0)
		{
			return null;
		}
		foreach (CameraPathPoint cameraPathPoint in this._points)
		{
			if (cameraPathPoint.positionModes == CameraPathPoint.PositionModes.FixedToPoint && cameraPathPoint.point == atPoint)
			{
				return cameraPathPoint;
			}
		}
		return null;
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x00004120 File Offset: 0x00002320
	public void Clear()
	{
		this._points.Clear();
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x00029378 File Offset: 0x00027578
	public CameraPathPoint DuplicatePointCheck()
	{
		foreach (CameraPathPoint cameraPathPoint in this._points)
		{
			foreach (CameraPathPoint cameraPathPoint2 in this._points)
			{
				if (cameraPathPoint != cameraPathPoint2 && cameraPathPoint.percent == cameraPathPoint2.percent)
				{
					return cameraPathPoint;
				}
			}
		}
		return null;
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00029438 File Offset: 0x00027638
	protected virtual void RecalculatePoints()
	{
		if (this.cameraPath == null)
		{
			Debug.LogError("Camera Path Point List was not initialised - run Init();");
			return;
		}
		int count = this._points.Count;
		if (count == 0)
		{
			return;
		}
		List<CameraPathPoint> list = new List<CameraPathPoint>();
		for (int i = 0; i < count; i++)
		{
			if (!(this._points[i] == null))
			{
				CameraPathPoint cameraPathPoint = this._points[i];
				if (i == 0)
				{
					list.Add(cameraPathPoint);
				}
				else
				{
					bool flag = false;
					foreach (CameraPathPoint cameraPathPoint2 in list)
					{
						if (cameraPathPoint2.percent > cameraPathPoint.percent)
						{
							list.Insert(list.IndexOf(cameraPathPoint2), cameraPathPoint);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						list.Add(cameraPathPoint);
					}
				}
			}
		}
		count = list.Count;
		this._points = list;
		for (int j = 0; j < count; j++)
		{
			CameraPathPoint cameraPathPoint3 = this._points[j];
			cameraPathPoint3.givenName = this.pointTypeName + " Point " + j;
			cameraPathPoint3.fullName = string.Concat(new object[]
			{
				this.cameraPath.name,
				" ",
				this.pointTypeName,
				" Point ",
				j
			});
			cameraPathPoint3.index = j;
			if (this.cameraPath.realNumberOfPoints >= 2)
			{
				switch (cameraPathPoint3.positionModes)
				{
				case CameraPathPoint.PositionModes.Free:
					if (cameraPathPoint3.cpointA == cameraPathPoint3.cpointB)
					{
						cameraPathPoint3.positionModes = CameraPathPoint.PositionModes.FixedToPoint;
						cameraPathPoint3.point = cameraPathPoint3.cpointA;
						cameraPathPoint3.cpointA = null;
						cameraPathPoint3.cpointB = null;
						cameraPathPoint3.percent = cameraPathPoint3.point.percentage;
						cameraPathPoint3.animationPercentage = ((!this.cameraPath.normalised) ? cameraPathPoint3.point.percentage : cameraPathPoint3.point.normalisedPercentage);
						cameraPathPoint3.worldPosition = cameraPathPoint3.point.worldPosition;
						return;
					}
					cameraPathPoint3.percent = this.cameraPath.GetPathPercentage(cameraPathPoint3.cpointA, cameraPathPoint3.cpointB, cameraPathPoint3.curvePercentage);
					cameraPathPoint3.animationPercentage = ((!this.cameraPath.normalised) ? cameraPathPoint3.percent : this.cameraPath.CalculateNormalisedPercentage(cameraPathPoint3.percent));
					cameraPathPoint3.worldPosition = this.cameraPath.GetPathPosition(cameraPathPoint3.percent, true);
					break;
				case CameraPathPoint.PositionModes.FixedToPoint:
					if (cameraPathPoint3.point == null)
					{
						cameraPathPoint3.point = this.cameraPath[this.cameraPath.GetNearestPointIndex(cameraPathPoint3.rawPercent)];
					}
					cameraPathPoint3.percent = cameraPathPoint3.point.percentage;
					cameraPathPoint3.animationPercentage = ((!this.cameraPath.normalised) ? cameraPathPoint3.point.percentage : cameraPathPoint3.point.normalisedPercentage);
					cameraPathPoint3.worldPosition = cameraPathPoint3.point.worldPosition;
					break;
				case CameraPathPoint.PositionModes.FixedToPercent:
					cameraPathPoint3.worldPosition = this.cameraPath.GetPathPosition(cameraPathPoint3.percent, true);
					cameraPathPoint3.animationPercentage = ((!this.cameraPath.normalised) ? cameraPathPoint3.percent : this.cameraPath.CalculateNormalisedPercentage(cameraPathPoint3.percent));
					break;
				}
			}
		}
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00029814 File Offset: 0x00027A14
	public void ReassignCP(CameraPathControlPoint from, CameraPathControlPoint to)
	{
		foreach (CameraPathPoint cameraPathPoint in this._points)
		{
			if (cameraPathPoint.point == from)
			{
				cameraPathPoint.point = to;
			}
			if (cameraPathPoint.cpointA == from)
			{
				cameraPathPoint.cpointA = to;
			}
			if (cameraPathPoint.cpointB == from)
			{
				cameraPathPoint.cpointB = to;
			}
		}
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0000412D File Offset: 0x0000232D
	protected void CheckListIsNull()
	{
		if (this._points == null)
		{
			this._points = new List<CameraPathPoint>();
		}
	}

	// Token: 0x04000215 RID: 533
	[SerializeField]
	private List<CameraPathPoint> _points = new List<CameraPathPoint>();

	// Token: 0x04000216 RID: 534
	[SerializeField]
	protected CameraPath cameraPath;

	// Token: 0x04000217 RID: 535
	protected string pointTypeName = "point";

	// Token: 0x04000218 RID: 536
	[NonSerialized]
	protected bool initialised;
}
