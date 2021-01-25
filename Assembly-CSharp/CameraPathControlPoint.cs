using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
[ExecuteInEditMode]
public class CameraPathControlPoint : MonoBehaviour
{
	// Token: 0x0600024E RID: 590 RVA: 0x00003BF2 File Offset: 0x00001DF2
	private void OnEnable()
	{
		base.hideFlags = 2;
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600024F RID: 591 RVA: 0x00003BFB File Offset: 0x00001DFB
	// (set) Token: 0x06000250 RID: 592 RVA: 0x00003C13 File Offset: 0x00001E13
	public Vector3 localPosition
	{
		get
		{
			return base.transform.rotation * this._position;
		}
		set
		{
			this._position = Quaternion.Inverse(base.transform.rotation) * value;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000251 RID: 593 RVA: 0x00003C31 File Offset: 0x00001E31
	// (set) Token: 0x06000252 RID: 594 RVA: 0x00003C3F File Offset: 0x00001E3F
	public Vector3 worldPosition
	{
		get
		{
			return this.LocalToWorldPosition(this._position);
		}
		set
		{
			this._position = this.WorldToLocalPosition(value);
		}
	}

	// Token: 0x06000253 RID: 595 RVA: 0x00027B80 File Offset: 0x00025D80
	public Vector3 WorldToLocalPosition(Vector3 _worldPosition)
	{
		Vector3 vector = _worldPosition - base.transform.position;
		return Quaternion.Inverse(base.transform.rotation) * vector;
	}

	// Token: 0x06000254 RID: 596 RVA: 0x00003C4E File Offset: 0x00001E4E
	public Vector3 LocalToWorldPosition(Vector3 _localPosition)
	{
		return base.transform.rotation * _localPosition + base.transform.position;
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000256 RID: 598 RVA: 0x00003C8A File Offset: 0x00001E8A
	// (set) Token: 0x06000255 RID: 597 RVA: 0x00003C71 File Offset: 0x00001E71
	public Vector3 forwardControlPointWorld
	{
		get
		{
			return this.forwardControlPoint + base.transform.position;
		}
		set
		{
			this.forwardControlPoint = value - base.transform.position;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x06000257 RID: 599 RVA: 0x00003CA2 File Offset: 0x00001EA2
	// (set) Token: 0x06000258 RID: 600 RVA: 0x00027BB8 File Offset: 0x00025DB8
	public Vector3 forwardControlPoint
	{
		get
		{
			return base.transform.rotation * (this._forwardControlPoint + this._position);
		}
		set
		{
			Vector3 vector = Quaternion.Inverse(base.transform.rotation) * value;
			vector += -this._position;
			this._forwardControlPoint = vector;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x06000259 RID: 601 RVA: 0x00003CC5 File Offset: 0x00001EC5
	// (set) Token: 0x0600025A RID: 602 RVA: 0x00027BF8 File Offset: 0x00025DF8
	public Vector3 forwardControlPointLocal
	{
		get
		{
			return base.transform.rotation * this._forwardControlPoint;
		}
		set
		{
			Vector3 forwardControlPoint = Quaternion.Inverse(base.transform.rotation) * value;
			this._forwardControlPoint = forwardControlPoint;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x0600025C RID: 604 RVA: 0x00003CF6 File Offset: 0x00001EF6
	// (set) Token: 0x0600025B RID: 603 RVA: 0x00003CDD File Offset: 0x00001EDD
	public Vector3 backwardControlPointWorld
	{
		get
		{
			return this.backwardControlPoint + base.transform.position;
		}
		set
		{
			this.backwardControlPoint = value - base.transform.position;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x0600025D RID: 605 RVA: 0x00027C28 File Offset: 0x00025E28
	// (set) Token: 0x0600025E RID: 606 RVA: 0x00027C74 File Offset: 0x00025E74
	public Vector3 backwardControlPoint
	{
		get
		{
			Vector3 vector = (!this._splitControlPoints) ? (-this._forwardControlPoint) : this._backwardControlPoint;
			return base.transform.rotation * (vector + this._position);
		}
		set
		{
			Vector3 vector = Quaternion.Inverse(base.transform.rotation) * value;
			vector += -this._position;
			if (this._splitControlPoints)
			{
				this._backwardControlPoint = vector;
			}
			else
			{
				this._forwardControlPoint = -vector;
			}
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x0600025F RID: 607 RVA: 0x00003D0E File Offset: 0x00001F0E
	// (set) Token: 0x06000260 RID: 608 RVA: 0x00003D16 File Offset: 0x00001F16
	public bool splitControlPoints
	{
		get
		{
			return this._splitControlPoints;
		}
		set
		{
			if (value != this._splitControlPoints)
			{
				this._backwardControlPoint = -this._forwardControlPoint;
			}
			this._splitControlPoints = value;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000261 RID: 609 RVA: 0x00003D3C File Offset: 0x00001F3C
	// (set) Token: 0x06000262 RID: 610 RVA: 0x00003D44 File Offset: 0x00001F44
	public Vector3 trackDirection
	{
		get
		{
			return this._pathDirection;
		}
		set
		{
			if (value == Vector3.zero)
			{
				return;
			}
			this._pathDirection = value.normalized;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000263 RID: 611 RVA: 0x00003D64 File Offset: 0x00001F64
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

	// Token: 0x06000264 RID: 612 RVA: 0x00027CD0 File Offset: 0x00025ED0
	public void CopyData(CameraPathControlPoint to)
	{
		to.customName = this.customName;
		to.index = this.index;
		to.percentage = this.percentage;
		to.normalisedPercentage = this.normalisedPercentage;
		to.worldPosition = this.worldPosition;
		to.splitControlPoints = this._splitControlPoints;
		to.forwardControlPoint = this._forwardControlPoint;
		to.backwardControlPoint = this._backwardControlPoint;
	}

	// Token: 0x040001C8 RID: 456
	public string givenName = string.Empty;

	// Token: 0x040001C9 RID: 457
	public string customName = string.Empty;

	// Token: 0x040001CA RID: 458
	public string fullName = string.Empty;

	// Token: 0x040001CB RID: 459
	[SerializeField]
	private Vector3 _position;

	// Token: 0x040001CC RID: 460
	[SerializeField]
	private bool _splitControlPoints;

	// Token: 0x040001CD RID: 461
	[SerializeField]
	private Vector3 _forwardControlPoint;

	// Token: 0x040001CE RID: 462
	[SerializeField]
	private Vector3 _backwardControlPoint;

	// Token: 0x040001CF RID: 463
	[SerializeField]
	private Vector3 _pathDirection = Vector3.forward;

	// Token: 0x040001D0 RID: 464
	public int index;

	// Token: 0x040001D1 RID: 465
	public float percentage;

	// Token: 0x040001D2 RID: 466
	public float normalisedPercentage;
}
