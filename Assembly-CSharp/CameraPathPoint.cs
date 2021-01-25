using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
[ExecuteInEditMode]
public class CameraPathPoint : MonoBehaviour
{
	// Token: 0x060002A6 RID: 678 RVA: 0x00003BF2 File Offset: 0x00001DF2
	private void OnEnable()
	{
		base.hideFlags = 2;
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060002A7 RID: 679 RVA: 0x00028AB0 File Offset: 0x00026CB0
	// (set) Token: 0x060002A8 RID: 680 RVA: 0x0000401F File Offset: 0x0000221F
	public float percent
	{
		get
		{
			switch (this.positionModes)
			{
			case CameraPathPoint.PositionModes.Free:
				return this._percent;
			case CameraPathPoint.PositionModes.FixedToPoint:
				return this.point.percentage;
			case CameraPathPoint.PositionModes.FixedToPercent:
				return this._percent;
			default:
				return this._percent;
			}
		}
		set
		{
			this._percent = value;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060002A9 RID: 681 RVA: 0x00004028 File Offset: 0x00002228
	public float rawPercent
	{
		get
		{
			return this._percent;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060002AA RID: 682 RVA: 0x00028AFC File Offset: 0x00026CFC
	// (set) Token: 0x060002AB RID: 683 RVA: 0x00004030 File Offset: 0x00002230
	public float animationPercentage
	{
		get
		{
			switch (this.positionModes)
			{
			case CameraPathPoint.PositionModes.Free:
				return this._animationPercentage;
			case CameraPathPoint.PositionModes.FixedToPoint:
				return this.point.normalisedPercentage;
			case CameraPathPoint.PositionModes.FixedToPercent:
				return this._animationPercentage;
			default:
				return this._percent;
			}
		}
		set
		{
			this._animationPercentage = value;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x060002AC RID: 684 RVA: 0x00004039 File Offset: 0x00002239
	public string displayName
	{
		get
		{
			if (this.customName != string.Empty)
			{
				return this.customName;
			}
			return this.givenName;
		}
	}

	// Token: 0x04000204 RID: 516
	public CameraPathPoint.PositionModes positionModes;

	// Token: 0x04000205 RID: 517
	public string givenName = string.Empty;

	// Token: 0x04000206 RID: 518
	public string customName = string.Empty;

	// Token: 0x04000207 RID: 519
	public string fullName = string.Empty;

	// Token: 0x04000208 RID: 520
	[SerializeField]
	protected float _percent;

	// Token: 0x04000209 RID: 521
	[SerializeField]
	protected float _animationPercentage;

	// Token: 0x0400020A RID: 522
	public CameraPathControlPoint point;

	// Token: 0x0400020B RID: 523
	public int index;

	// Token: 0x0400020C RID: 524
	public CameraPathControlPoint cpointA;

	// Token: 0x0400020D RID: 525
	public CameraPathControlPoint cpointB;

	// Token: 0x0400020E RID: 526
	public float curvePercentage;

	// Token: 0x0400020F RID: 527
	public Vector3 worldPosition;

	// Token: 0x04000210 RID: 528
	public bool lockPoint;

	// Token: 0x0200007A RID: 122
	public enum PositionModes
	{
		// Token: 0x04000212 RID: 530
		Free,
		// Token: 0x04000213 RID: 531
		FixedToPoint,
		// Token: 0x04000214 RID: 532
		FixedToPercent
	}
}
