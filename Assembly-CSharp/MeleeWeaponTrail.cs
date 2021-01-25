using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000627 RID: 1575
public class MeleeWeaponTrail : MonoBehaviour
{
	// Token: 0x170003AB RID: 939
	// (set) Token: 0x06002702 RID: 9986 RVA: 0x00019C5E File Offset: 0x00017E5E
	public bool Emit
	{
		set
		{
			this._emit = value;
		}
	}

	// Token: 0x170003AC RID: 940
	// (set) Token: 0x06002703 RID: 9987 RVA: 0x00019C67 File Offset: 0x00017E67
	public bool Use
	{
		set
		{
			this._use = value;
		}
	}

	// Token: 0x06002704 RID: 9988 RVA: 0x0012D4A8 File Offset: 0x0012B6A8
	private void Start()
	{
		this._lastPosition = base.transform.position;
		this._trailObject = new GameObject("Trail");
		this._trailObject.transform.parent = null;
		this._trailObject.transform.position = Vector3.zero;
		this._trailObject.transform.rotation = Quaternion.identity;
		this._trailObject.transform.localScale = Vector3.one;
		this._trailObject.AddComponent(typeof(MeshFilter));
		this._trailObject.AddComponent(typeof(MeshRenderer));
		this._trailObject.renderer.material = this._material;
		this._trailMesh = new Mesh();
		this._trailMesh.name = base.name + "TrailMesh";
		this._trailObject.GetComponent<MeshFilter>().mesh = this._trailMesh;
		this._minVertexDistanceSqr = this._minVertexDistance * this._minVertexDistance;
		this._maxVertexDistanceSqr = this._maxVertexDistance * this._maxVertexDistance;
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x00019C70 File Offset: 0x00017E70
	private void OnDisable()
	{
		Object.Destroy(this._trailObject);
	}

	// Token: 0x06002706 RID: 9990 RVA: 0x0012D5CC File Offset: 0x0012B7CC
	private void Update()
	{
		if (!this._use)
		{
			return;
		}
		if (this._emit && this._emitTime != 0f)
		{
			this._emitTime -= Time.deltaTime;
			if (this._emitTime == 0f)
			{
				this._emitTime = -1f;
			}
			if (this._emitTime < 0f)
			{
				this._emit = false;
			}
		}
		if (!this._emit && this._points.Count == 0 && this._autoDestruct)
		{
			Object.Destroy(this._trailObject);
			Object.Destroy(base.gameObject);
		}
		float sqrMagnitude = (this._lastPosition - base.transform.position).sqrMagnitude;
		if (this._emit)
		{
			if (sqrMagnitude > this._minVertexDistanceSqr)
			{
				bool flag = false;
				if (this._points.Count < 3)
				{
					flag = true;
				}
				else
				{
					Vector3 vector = this._points[this._points.Count - 2].tipPosition - this._points[this._points.Count - 3].tipPosition;
					Vector3 vector2 = this._points[this._points.Count - 1].tipPosition - this._points[this._points.Count - 2].tipPosition;
					if (Vector3.Angle(vector, vector2) > this._maxAngle || sqrMagnitude > this._maxVertexDistanceSqr)
					{
						flag = true;
					}
				}
				if (flag)
				{
					MeleeWeaponTrail.Point point = new MeleeWeaponTrail.Point();
					point.basePosition = this._base.position;
					point.tipPosition = this._tip.position;
					point.timeCreated = Time.time;
					this._points.Add(point);
					this._lastPosition = base.transform.position;
					if (this._points.Count == 1)
					{
						this._smoothedPoints.Add(point);
					}
					else if (this._points.Count > 1)
					{
						for (int i = 0; i < 1 + this.subdivisions; i++)
						{
							this._smoothedPoints.Add(point);
						}
					}
					if (this._points.Count >= 4)
					{
						IEnumerable<Vector3> enumerable = Interpolate.NewCatmullRom(new Vector3[]
						{
							this._points[this._points.Count - 4].tipPosition,
							this._points[this._points.Count - 3].tipPosition,
							this._points[this._points.Count - 2].tipPosition,
							this._points[this._points.Count - 1].tipPosition
						}, this.subdivisions, false);
						IEnumerable<Vector3> enumerable2 = Interpolate.NewCatmullRom(new Vector3[]
						{
							this._points[this._points.Count - 4].basePosition,
							this._points[this._points.Count - 3].basePosition,
							this._points[this._points.Count - 2].basePosition,
							this._points[this._points.Count - 1].basePosition
						}, this.subdivisions, false);
						List<Vector3> list = new List<Vector3>(enumerable);
						List<Vector3> list2 = new List<Vector3>(enumerable2);
						float timeCreated = this._points[this._points.Count - 4].timeCreated;
						float timeCreated2 = this._points[this._points.Count - 1].timeCreated;
						for (int j = 0; j < list.Count; j++)
						{
							int num = this._smoothedPoints.Count - (list.Count - j);
							if (num > -1 && num < this._smoothedPoints.Count)
							{
								MeleeWeaponTrail.Point point2 = new MeleeWeaponTrail.Point();
								point2.basePosition = list2[j];
								point2.tipPosition = list[j];
								point2.timeCreated = Mathf.Lerp(timeCreated, timeCreated2, (float)j / (float)list.Count);
								this._smoothedPoints[num] = point2;
							}
						}
					}
				}
				else
				{
					this._points[this._points.Count - 1].basePosition = this._base.position;
					this._points[this._points.Count - 1].tipPosition = this._tip.position;
					this._smoothedPoints[this._smoothedPoints.Count - 1].basePosition = this._base.position;
					this._smoothedPoints[this._smoothedPoints.Count - 1].tipPosition = this._tip.position;
				}
			}
			else
			{
				if (this._points.Count > 0)
				{
					this._points[this._points.Count - 1].basePosition = this._base.position;
					this._points[this._points.Count - 1].tipPosition = this._tip.position;
				}
				if (this._smoothedPoints.Count > 0)
				{
					this._smoothedPoints[this._smoothedPoints.Count - 1].basePosition = this._base.position;
					this._smoothedPoints[this._smoothedPoints.Count - 1].tipPosition = this._tip.position;
				}
			}
		}
		this.RemoveOldPoints(this._points);
		if (this._points.Count == 0)
		{
			this._trailMesh.Clear();
		}
		this.RemoveOldPoints(this._smoothedPoints);
		if (this._smoothedPoints.Count == 0)
		{
			this._trailMesh.Clear();
		}
		List<MeleeWeaponTrail.Point> smoothedPoints = this._smoothedPoints;
		if (smoothedPoints.Count > 1)
		{
			Vector3[] array = new Vector3[smoothedPoints.Count * 2];
			Vector2[] array2 = new Vector2[smoothedPoints.Count * 2];
			int[] array3 = new int[(smoothedPoints.Count - 1) * 6];
			Color[] array4 = new Color[smoothedPoints.Count * 2];
			for (int k = 0; k < smoothedPoints.Count; k++)
			{
				MeleeWeaponTrail.Point point3 = smoothedPoints[k];
				float num2 = (Time.time - point3.timeCreated) / this._lifeTime;
				Color color = Color.Lerp(Color.white, Color.clear, num2);
				if (this._colors != null && this._colors.Length > 0)
				{
					float num3 = num2 * (float)(this._colors.Length - 1);
					float num4 = Mathf.Floor(num3);
					float num5 = Mathf.Clamp(Mathf.Ceil(num3), 1f, (float)(this._colors.Length - 1));
					float num6 = Mathf.InverseLerp(num4, num5, num3);
					if (num4 >= (float)this._colors.Length)
					{
						num4 = (float)(this._colors.Length - 1);
					}
					if (num4 < 0f)
					{
						num4 = 0f;
					}
					if (num5 >= (float)this._colors.Length)
					{
						num5 = (float)(this._colors.Length - 1);
					}
					if (num5 < 0f)
					{
						num5 = 0f;
					}
					color = Color.Lerp(this._colors[(int)num4], this._colors[(int)num5], num6);
				}
				float num7 = 0f;
				if (this._sizes != null && this._sizes.Length > 0)
				{
					float num8 = num2 * (float)(this._sizes.Length - 1);
					float num9 = Mathf.Floor(num8);
					float num10 = Mathf.Clamp(Mathf.Ceil(num8), 1f, (float)(this._sizes.Length - 1));
					float num11 = Mathf.InverseLerp(num9, num10, num8);
					if (num9 >= (float)this._sizes.Length)
					{
						num9 = (float)(this._sizes.Length - 1);
					}
					if (num9 < 0f)
					{
						num9 = 0f;
					}
					if (num10 >= (float)this._sizes.Length)
					{
						num10 = (float)(this._sizes.Length - 1);
					}
					if (num10 < 0f)
					{
						num10 = 0f;
					}
					num7 = Mathf.Lerp(this._sizes[(int)num9], this._sizes[(int)num10], num11);
				}
				Vector3 vector3 = point3.tipPosition - point3.basePosition;
				array[k * 2] = point3.basePosition - vector3 * (num7 * 0.5f);
				array[k * 2 + 1] = point3.tipPosition + vector3 * (num7 * 0.5f);
				array4[k * 2] = (array4[k * 2 + 1] = color);
				float num12 = (float)k / (float)smoothedPoints.Count;
				array2[k * 2] = new Vector2(num12, 0f);
				array2[k * 2 + 1] = new Vector2(num12, 1f);
				if (k > 0)
				{
					array3[(k - 1) * 6] = k * 2 - 2;
					array3[(k - 1) * 6 + 1] = k * 2 - 1;
					array3[(k - 1) * 6 + 2] = k * 2;
					array3[(k - 1) * 6 + 3] = k * 2 + 1;
					array3[(k - 1) * 6 + 4] = k * 2;
					array3[(k - 1) * 6 + 5] = k * 2 - 1;
				}
			}
			this._trailMesh.Clear();
			this._trailMesh.vertices = array;
			this._trailMesh.colors = array4;
			this._trailMesh.uv = array2;
			this._trailMesh.triangles = array3;
		}
	}

	// Token: 0x06002707 RID: 9991 RVA: 0x0012E060 File Offset: 0x0012C260
	private void RemoveOldPoints(List<MeleeWeaponTrail.Point> pointList)
	{
		List<MeleeWeaponTrail.Point> list = new List<MeleeWeaponTrail.Point>();
		foreach (MeleeWeaponTrail.Point point in pointList)
		{
			if (Time.time - point.timeCreated > this._lifeTime)
			{
				list.Add(point);
			}
		}
		foreach (MeleeWeaponTrail.Point point2 in list)
		{
			pointList.Remove(point2);
		}
	}

	// Token: 0x0400301D RID: 12317
	[SerializeField]
	private bool _emit = true;

	// Token: 0x0400301E RID: 12318
	private bool _use = true;

	// Token: 0x0400301F RID: 12319
	[SerializeField]
	private float _emitTime;

	// Token: 0x04003020 RID: 12320
	[SerializeField]
	private Material _material;

	// Token: 0x04003021 RID: 12321
	[SerializeField]
	private float _lifeTime = 1f;

	// Token: 0x04003022 RID: 12322
	[SerializeField]
	private Color[] _colors;

	// Token: 0x04003023 RID: 12323
	[SerializeField]
	private float[] _sizes;

	// Token: 0x04003024 RID: 12324
	[SerializeField]
	private float _minVertexDistance = 0.1f;

	// Token: 0x04003025 RID: 12325
	[SerializeField]
	private float _maxVertexDistance = 10f;

	// Token: 0x04003026 RID: 12326
	private float _minVertexDistanceSqr;

	// Token: 0x04003027 RID: 12327
	private float _maxVertexDistanceSqr;

	// Token: 0x04003028 RID: 12328
	[SerializeField]
	private float _maxAngle = 3f;

	// Token: 0x04003029 RID: 12329
	[SerializeField]
	private bool _autoDestruct;

	// Token: 0x0400302A RID: 12330
	[SerializeField]
	private int subdivisions = 4;

	// Token: 0x0400302B RID: 12331
	[SerializeField]
	private Transform _base;

	// Token: 0x0400302C RID: 12332
	[SerializeField]
	private Transform _tip;

	// Token: 0x0400302D RID: 12333
	private List<MeleeWeaponTrail.Point> _points = new List<MeleeWeaponTrail.Point>();

	// Token: 0x0400302E RID: 12334
	private List<MeleeWeaponTrail.Point> _smoothedPoints = new List<MeleeWeaponTrail.Point>();

	// Token: 0x0400302F RID: 12335
	private GameObject _trailObject;

	// Token: 0x04003030 RID: 12336
	private Mesh _trailMesh;

	// Token: 0x04003031 RID: 12337
	private Vector3 _lastPosition;

	// Token: 0x02000628 RID: 1576
	[Serializable]
	public class Point
	{
		// Token: 0x04003032 RID: 12338
		public float timeCreated;

		// Token: 0x04003033 RID: 12339
		public Vector3 basePosition;

		// Token: 0x04003034 RID: 12340
		public Vector3 tipPosition;
	}
}
