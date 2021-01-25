using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E8 RID: 1000
public class NcTrailTexture : NcEffectBehaviour
{
	// Token: 0x060017F1 RID: 6129 RVA: 0x0000FA0B File Offset: 0x0000DC0B
	public void SetEmit(bool bEmit)
	{
		this.m_bEmit = bEmit;
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
		this.m_fStopTime = 0f;
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x0000FA2A File Offset: 0x0000DC2A
	public override int GetAnimationState()
	{
		if (!base.enabled || !NcEffectBehaviour.IsActive(base.gameObject))
		{
			return -1;
		}
		if (NcEffectBehaviour.GetEngineTime() < this.m_fStartTime + this.m_fDelayTime + 0.1f)
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x0000FA69 File Offset: 0x0000DC69
	private void OnDisable()
	{
		if (this.m_TrialObject != null)
		{
			NcAutoDestruct.CreateAutoDestruct(this.m_TrialObject, 0f, this.m_fLifeTime / 2f, true, true);
		}
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x000C4A08 File Offset: 0x000C2C08
	private void Start()
	{
		if (base.renderer == null || base.renderer.sharedMaterial == null)
		{
			base.enabled = false;
			return;
		}
		if (0f < this.m_fDelayTime)
		{
			this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
		}
		else
		{
			this.InitTrailObject();
		}
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x000C4A6C File Offset: 0x000C2C6C
	private void InitTrailObject()
	{
		this.m_base = base.transform;
		this.m_fStartTime = NcEffectBehaviour.GetEngineTime();
		this.m_LastPosition = base.transform.position;
		this.m_TrialObject = new GameObject("Trail");
		this.m_TrialObject.transform.position = Vector3.zero;
		this.m_TrialObject.transform.rotation = Quaternion.identity;
		this.m_TrialObject.transform.localScale = base.transform.localScale;
		this.m_TrialObject.AddComponent(typeof(MeshFilter));
		this.m_TrialObject.AddComponent(typeof(MeshRenderer));
		this.m_TrialObject.renderer.sharedMaterial = base.renderer.sharedMaterial;
		this.m_TrailMesh = this.m_TrialObject.GetComponent<MeshFilter>().mesh;
		base.CreateEditorGameObject(this.m_TrialObject);
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x000C4B60 File Offset: 0x000C2D60
	private Vector3 GetTipPoint()
	{
		switch (this.m_TipAxis)
		{
		case NcTrailTexture.AXIS_TYPE.AXIS_FORWARD:
			return this.m_base.position + this.m_base.forward;
		case NcTrailTexture.AXIS_TYPE.AXIS_BACK:
			return this.m_base.position + this.m_base.forward * -1f;
		case NcTrailTexture.AXIS_TYPE.AXIS_RIGHT:
			return this.m_base.position + this.m_base.right;
		case NcTrailTexture.AXIS_TYPE.AXIS_LEFT:
			return this.m_base.position + this.m_base.right * -1f;
		case NcTrailTexture.AXIS_TYPE.AXIS_UP:
			return this.m_base.position + this.m_base.up;
		case NcTrailTexture.AXIS_TYPE.AXIS_DOWN:
			return this.m_base.position + this.m_base.up * -1f;
		default:
			return this.m_base.position + this.m_base.forward;
		}
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x000C4C78 File Offset: 0x000C2E78
	private void Update()
	{
		if (base.renderer == null || base.renderer.sharedMaterial == null)
		{
			base.enabled = false;
			return;
		}
		if (0f < this.m_fDelayTime)
		{
			if (NcEffectBehaviour.GetEngineTime() < this.m_fStartTime + this.m_fDelayTime)
			{
				return;
			}
			this.m_fDelayTime = 0f;
			this.m_fStartTime = 0f;
			this.InitTrailObject();
		}
		if (this.m_bEmit && 0f < this.m_fEmitTime && this.m_fStopTime == 0f && this.m_fStartTime + this.m_fEmitTime < NcEffectBehaviour.GetEngineTime())
		{
			if (this.m_bSmoothHide)
			{
				this.m_fStopTime = NcEffectBehaviour.GetEngineTime();
			}
			else
			{
				this.m_bEmit = false;
			}
		}
		if (0f < this.m_fStopTime && this.m_fLifeTime < NcEffectBehaviour.GetEngineTime() - this.m_fStopTime)
		{
			this.m_bEmit = false;
		}
		if (!this.m_bEmit && this.m_Points.Count == 0 && this.m_bAutoDestruct)
		{
			Object.Destroy(this.m_TrialObject);
			Object.Destroy(base.gameObject);
		}
		float magnitude = (this.m_LastPosition - base.transform.position).magnitude;
		if (this.m_bEmit)
		{
			if (magnitude > this.m_fMinVertexDistance)
			{
				bool flag = false;
				if (this.m_Points.Count < 3)
				{
					flag = true;
				}
				else
				{
					Vector3 vector = this.m_Points[this.m_Points.Count - 2].basePosition - this.m_Points[this.m_Points.Count - 3].basePosition;
					Vector3 vector2 = this.m_Points[this.m_Points.Count - 1].basePosition - this.m_Points[this.m_Points.Count - 2].basePosition;
					if (Vector3.Angle(vector, vector2) > this.m_fMaxAngle || magnitude > this.m_fMaxVertexDistance)
					{
						flag = true;
					}
				}
				if (flag)
				{
					NcTrailTexture.Point point = new NcTrailTexture.Point();
					point.basePosition = this.m_base.position;
					point.tipPosition = this.GetTipPoint();
					if (0f < this.m_fStopTime)
					{
						point.timeCreated = NcEffectBehaviour.GetEngineTime() - (NcEffectBehaviour.GetEngineTime() - this.m_fStopTime);
					}
					else
					{
						point.timeCreated = NcEffectBehaviour.GetEngineTime();
					}
					this.m_Points.Add(point);
					this.m_LastPosition = base.transform.position;
					if (this.m_bInterpolation)
					{
						if (this.m_Points.Count == 1)
						{
							this.m_SmoothedPoints.Add(point);
						}
						else if (1 < this.m_Points.Count)
						{
							for (int i = 0; i < 1 + this.m_nSubdivisions; i++)
							{
								this.m_SmoothedPoints.Add(point);
							}
						}
						int num = 2;
						if (num <= this.m_Points.Count)
						{
							int num2 = Mathf.Min(this.m_nMaxSmoothCount, this.m_Points.Count);
							Vector3[] array = new Vector3[num2];
							for (int j = 0; j < num2; j++)
							{
								array[j] = this.m_Points[this.m_Points.Count - (num2 - j)].basePosition;
							}
							IEnumerable<Vector3> enumerable = NgInterpolate.NewCatmullRom(array, this.m_nSubdivisions, false);
							Vector3[] array2 = new Vector3[num2];
							for (int k = 0; k < num2; k++)
							{
								array2[k] = this.m_Points[this.m_Points.Count - (num2 - k)].tipPosition;
							}
							IEnumerable<Vector3> enumerable2 = NgInterpolate.NewCatmullRom(array2, this.m_nSubdivisions, false);
							List<Vector3> list = new List<Vector3>(enumerable);
							List<Vector3> list2 = new List<Vector3>(enumerable2);
							float timeCreated = this.m_Points[this.m_Points.Count - num2].timeCreated;
							float timeCreated2 = this.m_Points[this.m_Points.Count - 1].timeCreated;
							for (int l = 0; l < list.Count; l++)
							{
								int num3 = this.m_SmoothedPoints.Count - (list.Count - l);
								if (-1 < num3 && num3 < this.m_SmoothedPoints.Count)
								{
									NcTrailTexture.Point point2 = new NcTrailTexture.Point();
									point2.tipPosition = list2[l];
									point2.basePosition = list[l];
									point2.timeCreated = Mathf.Lerp(timeCreated, timeCreated2, (float)l / (float)list.Count);
									this.m_SmoothedPoints[num3] = point2;
								}
							}
						}
					}
				}
				else
				{
					this.m_Points[this.m_Points.Count - 1].tipPosition = this.GetTipPoint();
					this.m_Points[this.m_Points.Count - 1].basePosition = this.m_base.position;
					if (this.m_bInterpolation)
					{
						this.m_SmoothedPoints[this.m_SmoothedPoints.Count - 1].tipPosition = this.GetTipPoint();
						this.m_SmoothedPoints[this.m_SmoothedPoints.Count - 1].basePosition = this.m_base.position;
					}
				}
			}
			else
			{
				if (this.m_Points.Count > 0)
				{
					this.m_Points[this.m_Points.Count - 1].tipPosition = this.GetTipPoint();
					this.m_Points[this.m_Points.Count - 1].basePosition = this.m_base.position;
				}
				if (this.m_bInterpolation && this.m_SmoothedPoints.Count > 0)
				{
					this.m_SmoothedPoints[this.m_SmoothedPoints.Count - 1].tipPosition = this.GetTipPoint();
					this.m_SmoothedPoints[this.m_SmoothedPoints.Count - 1].basePosition = this.m_base.position;
				}
			}
		}
		if (!this.m_bEmit && this.m_bLastFrameEmit && this.m_Points.Count > 0)
		{
			this.m_Points[this.m_Points.Count - 1].lineBreak = true;
		}
		this.m_bLastFrameEmit = this.m_bEmit;
		List<NcTrailTexture.Point> list3 = new List<NcTrailTexture.Point>();
		foreach (NcTrailTexture.Point point3 in this.m_Points)
		{
			if (NcEffectBehaviour.GetEngineTime() - point3.timeCreated > this.m_fLifeTime)
			{
				list3.Add(point3);
			}
		}
		foreach (NcTrailTexture.Point point4 in list3)
		{
			this.m_Points.Remove(point4);
		}
		if (this.m_bInterpolation)
		{
			list3 = new List<NcTrailTexture.Point>();
			foreach (NcTrailTexture.Point point5 in this.m_SmoothedPoints)
			{
				if (NcEffectBehaviour.GetEngineTime() - point5.timeCreated > this.m_fLifeTime)
				{
					list3.Add(point5);
				}
			}
			foreach (NcTrailTexture.Point point6 in list3)
			{
				this.m_SmoothedPoints.Remove(point6);
			}
		}
		List<NcTrailTexture.Point> list4;
		if (this.m_bInterpolation)
		{
			list4 = this.m_SmoothedPoints;
		}
		else
		{
			list4 = this.m_Points;
		}
		if (list4.Count > 1)
		{
			Vector3[] array3 = new Vector3[list4.Count * 2];
			Vector2[] array4 = new Vector2[list4.Count * 2];
			int[] array5 = new int[(list4.Count - 1) * 6];
			Color[] array6 = new Color[list4.Count * 2];
			for (int m = 0; m < list4.Count; m++)
			{
				NcTrailTexture.Point point7 = list4[m];
				float num4 = (NcEffectBehaviour.GetEngineTime() - point7.timeCreated) / this.m_fLifeTime;
				Color color = Color.Lerp(Color.white, Color.clear, num4);
				if (this.m_Colors != null && this.m_Colors.Length > 0)
				{
					float num5 = num4 * (float)(this.m_Colors.Length - 1);
					float num6 = Mathf.Floor(num5);
					float num7 = Mathf.Clamp(Mathf.Ceil(num5), 1f, (float)(this.m_Colors.Length - 1));
					float num8 = Mathf.InverseLerp(num6, num7, num5);
					if (num6 >= (float)this.m_Colors.Length)
					{
						num6 = (float)(this.m_Colors.Length - 1);
					}
					if (num6 < 0f)
					{
						num6 = 0f;
					}
					if (num7 >= (float)this.m_Colors.Length)
					{
						num7 = (float)(this.m_Colors.Length - 1);
					}
					if (num7 < 0f)
					{
						num7 = 0f;
					}
					color = Color.Lerp(this.m_Colors[(int)num6], this.m_Colors[(int)num7], num8);
				}
				Vector3 vector3 = point7.basePosition - point7.tipPosition;
				float num9 = this.m_fTipSize;
				if (this.m_SizeRates != null && this.m_SizeRates.Length > 0)
				{
					float num10 = num4 * (float)(this.m_SizeRates.Length - 1);
					float num11 = Mathf.Floor(num10);
					float num12 = Mathf.Clamp(Mathf.Ceil(num10), 1f, (float)(this.m_SizeRates.Length - 1));
					float num13 = Mathf.InverseLerp(num11, num12, num10);
					if (num11 >= (float)this.m_SizeRates.Length)
					{
						num11 = (float)(this.m_SizeRates.Length - 1);
					}
					if (num11 < 0f)
					{
						num11 = 0f;
					}
					if (num12 >= (float)this.m_SizeRates.Length)
					{
						num12 = (float)(this.m_SizeRates.Length - 1);
					}
					if (num12 < 0f)
					{
						num12 = 0f;
					}
					num9 *= Mathf.Lerp(this.m_SizeRates[(int)num11], this.m_SizeRates[(int)num12], num13);
				}
				if (this.m_bCenterAlign)
				{
					array3[m * 2] = point7.basePosition - vector3 * (num9 * 0.5f);
					array3[m * 2 + 1] = point7.basePosition + vector3 * (num9 * 0.5f);
				}
				else
				{
					array3[m * 2] = point7.basePosition - vector3 * num9;
					array3[m * 2 + 1] = point7.basePosition;
				}
				int num14 = (!this.m_bInterpolation) ? this.m_nFadeTailCount : (this.m_nFadeTailCount * this.m_nSubdivisions);
				int num15 = (!this.m_bInterpolation) ? this.m_nFadeHeadCount : (this.m_nFadeHeadCount * this.m_nSubdivisions);
				if (0 < num14 && m <= num14)
				{
					color.a = color.a * (float)m / (float)num14;
				}
				if (0 < num15 && list4.Count - (m + 1) <= num15)
				{
					color.a = color.a * (float)(list4.Count - (m + 1)) / (float)num15;
				}
				array6[m * 2] = (array6[m * 2 + 1] = color);
				float num16 = (float)m / (float)list4.Count;
				array4[m * 2] = new Vector2((!this.m_UvFlipHorizontal) ? num16 : (1f - num16), (float)((!this.m_UvFlipVirtical) ? 0 : 1));
				array4[m * 2 + 1] = new Vector2((!this.m_UvFlipHorizontal) ? num16 : (1f - num16), (float)((!this.m_UvFlipVirtical) ? 1 : 0));
				if (m > 0)
				{
					array5[(m - 1) * 6] = m * 2 - 2;
					array5[(m - 1) * 6 + 1] = m * 2 - 1;
					array5[(m - 1) * 6 + 2] = m * 2;
					array5[(m - 1) * 6 + 3] = m * 2 + 1;
					array5[(m - 1) * 6 + 4] = m * 2;
					array5[(m - 1) * 6 + 5] = m * 2 - 1;
				}
			}
			this.m_TrailMesh.Clear();
			this.m_TrailMesh.vertices = array3;
			this.m_TrailMesh.colors = array6;
			this.m_TrailMesh.uv = array4;
			this.m_TrailMesh.triangles = array5;
		}
		else
		{
			this.m_TrailMesh.Clear();
		}
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x0000FA9B File Offset: 0x0000DC9B
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
		this.m_fDelayTime /= fSpeedRate;
		this.m_fEmitTime /= fSpeedRate;
		this.m_fLifeTime /= fSpeedRate;
	}

	// Token: 0x04001CBB RID: 7355
	public float m_fDelayTime;

	// Token: 0x04001CBC RID: 7356
	public float m_fEmitTime;

	// Token: 0x04001CBD RID: 7357
	public bool m_bSmoothHide = true;

	// Token: 0x04001CBE RID: 7358
	protected bool m_bEmit = true;

	// Token: 0x04001CBF RID: 7359
	protected float m_fStartTime;

	// Token: 0x04001CC0 RID: 7360
	protected float m_fStopTime;

	// Token: 0x04001CC1 RID: 7361
	public float m_fLifeTime = 0.7f;

	// Token: 0x04001CC2 RID: 7362
	public NcTrailTexture.AXIS_TYPE m_TipAxis = NcTrailTexture.AXIS_TYPE.AXIS_BACK;

	// Token: 0x04001CC3 RID: 7363
	public float m_fTipSize = 1f;

	// Token: 0x04001CC4 RID: 7364
	public bool m_bCenterAlign;

	// Token: 0x04001CC5 RID: 7365
	public bool m_UvFlipHorizontal;

	// Token: 0x04001CC6 RID: 7366
	public bool m_UvFlipVirtical;

	// Token: 0x04001CC7 RID: 7367
	public int m_nFadeHeadCount = 2;

	// Token: 0x04001CC8 RID: 7368
	public int m_nFadeTailCount = 2;

	// Token: 0x04001CC9 RID: 7369
	public Color[] m_Colors;

	// Token: 0x04001CCA RID: 7370
	public float[] m_SizeRates;

	// Token: 0x04001CCB RID: 7371
	public bool m_bInterpolation;

	// Token: 0x04001CCC RID: 7372
	public int m_nMaxSmoothCount = 10;

	// Token: 0x04001CCD RID: 7373
	public int m_nSubdivisions = 4;

	// Token: 0x04001CCE RID: 7374
	protected List<NcTrailTexture.Point> m_SmoothedPoints = new List<NcTrailTexture.Point>();

	// Token: 0x04001CCF RID: 7375
	public float m_fMinVertexDistance = 0.2f;

	// Token: 0x04001CD0 RID: 7376
	public float m_fMaxVertexDistance = 10f;

	// Token: 0x04001CD1 RID: 7377
	public float m_fMaxAngle = 3f;

	// Token: 0x04001CD2 RID: 7378
	public bool m_bAutoDestruct;

	// Token: 0x04001CD3 RID: 7379
	protected List<NcTrailTexture.Point> m_Points = new List<NcTrailTexture.Point>();

	// Token: 0x04001CD4 RID: 7380
	protected Transform m_base;

	// Token: 0x04001CD5 RID: 7381
	protected GameObject m_TrialObject;

	// Token: 0x04001CD6 RID: 7382
	protected Mesh m_TrailMesh;

	// Token: 0x04001CD7 RID: 7383
	protected Vector3 m_LastPosition;

	// Token: 0x04001CD8 RID: 7384
	protected Vector3 m_LastCameraPosition1;

	// Token: 0x04001CD9 RID: 7385
	protected Vector3 m_LastCameraPosition2;

	// Token: 0x04001CDA RID: 7386
	protected bool m_bLastFrameEmit = true;

	// Token: 0x020003E9 RID: 1001
	public enum AXIS_TYPE
	{
		// Token: 0x04001CDC RID: 7388
		AXIS_FORWARD,
		// Token: 0x04001CDD RID: 7389
		AXIS_BACK,
		// Token: 0x04001CDE RID: 7390
		AXIS_RIGHT,
		// Token: 0x04001CDF RID: 7391
		AXIS_LEFT,
		// Token: 0x04001CE0 RID: 7392
		AXIS_UP,
		// Token: 0x04001CE1 RID: 7393
		AXIS_DOWN
	}

	// Token: 0x020003EA RID: 1002
	public class Point
	{
		// Token: 0x04001CE2 RID: 7394
		public float timeCreated;

		// Token: 0x04001CE3 RID: 7395
		public Vector3 basePosition;

		// Token: 0x04001CE4 RID: 7396
		public Vector3 tipPosition;

		// Token: 0x04001CE5 RID: 7397
		public bool lineBreak;
	}
}
