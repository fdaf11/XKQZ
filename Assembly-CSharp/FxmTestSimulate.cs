using System;
using UnityEngine;

// Token: 0x02000408 RID: 1032
public class FxmTestSimulate : MonoBehaviour
{
	// Token: 0x0600190A RID: 6410 RVA: 0x000104CA File Offset: 0x0000E6CA
	public void Init(Component fxmEffectControls, int nMultiShotCount)
	{
		this.m_FXMakerControls = fxmEffectControls;
		this.m_nMultiShotCount = nMultiShotCount;
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x000CB3B0 File Offset: 0x000C95B0
	public void SimulateMove(FxmTestControls.AXIS nTransAxis, float fHalfDist, float fSpeed, bool bRotFront)
	{
		Vector3 position = base.transform.position;
		this.m_nAxis = nTransAxis;
		this.m_StartPos = position;
		this.m_EndPos = position;
		ref Vector3 ptr = ref this.m_StartPos;
		int nAxis;
		int num = nAxis = (int)this.m_nAxis;
		float num2 = ptr[nAxis];
		this.m_StartPos[num] = num2 - fHalfDist;
		ref Vector3 ptr2 = ref this.m_EndPos;
		int num3 = nAxis = (int)this.m_nAxis;
		num2 = ptr2[nAxis];
		this.m_EndPos[num3] = num2 + fHalfDist;
		this.m_fDist = Vector3.Distance(this.m_StartPos, this.m_EndPos);
		this.m_Mode = FxmTestSimulate.MODE_TYPE.MOVE;
		this.SimulateStart(this.m_StartPos, fSpeed, bRotFront);
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x000CB450 File Offset: 0x000C9650
	public void SimulateArc(float fHalfDist, float fSpeed, bool bRotFront)
	{
		this.m_Curve = FxmTestMain.inst.m_SimulateArcCurve;
		if (this.m_Curve == null)
		{
			Debug.LogError("FXMakerOption.m_SimulateArcCurve is null !!!!");
			return;
		}
		Vector3 position = base.transform.position;
		this.m_StartPos = new Vector3(position.x - fHalfDist, position.y, position.z);
		this.m_EndPos = new Vector3(position.x + fHalfDist, position.y, position.z);
		this.m_fDist = Vector3.Distance(this.m_StartPos, this.m_EndPos);
		this.m_Mode = FxmTestSimulate.MODE_TYPE.ARC;
		this.SimulateStart(this.m_StartPos, fSpeed, bRotFront);
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x000CB500 File Offset: 0x000C9700
	public void SimulateFall(float fHeight, float fSpeed, bool bRotFront)
	{
		Vector3 position = base.transform.position;
		this.m_StartPos = new Vector3(position.x, position.y + fHeight, position.z);
		this.m_EndPos = new Vector3(position.x, position.y, position.z);
		this.m_fDist = Vector3.Distance(this.m_StartPos, this.m_EndPos);
		this.m_Mode = FxmTestSimulate.MODE_TYPE.MOVE;
		this.SimulateStart(this.m_StartPos, fSpeed, bRotFront);
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x000CB588 File Offset: 0x000C9788
	public void SimulateRaise(float fHeight, float fSpeed, bool bRotFront)
	{
		Vector3 position = base.transform.position;
		this.m_StartPos = new Vector3(position.x, position.y, position.z);
		this.m_EndPos = new Vector3(position.x, position.y + fHeight, position.z);
		this.m_fDist = Vector3.Distance(this.m_StartPos, this.m_EndPos);
		this.m_Mode = FxmTestSimulate.MODE_TYPE.MOVE;
		this.SimulateStart(this.m_StartPos, fSpeed, bRotFront);
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x000CB610 File Offset: 0x000C9810
	public void SimulateCircle(float fRadius, float fSpeed, bool bRotFront)
	{
		Vector3 position = base.transform.position;
		this.m_fRadius = fRadius;
		this.m_Mode = FxmTestSimulate.MODE_TYPE.ROTATE;
		this.m_fDist = 1f;
		this.SimulateStart(new Vector3(position.x - fRadius, position.y, position.z), fSpeed, bRotFront);
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x000CB668 File Offset: 0x000C9868
	public void SimulateTornado(float fRadius, float fHeight, float fSpeed, bool bRotFront)
	{
		Vector3 position = base.transform.position;
		this.m_fRadius = fRadius;
		this.m_Mode = FxmTestSimulate.MODE_TYPE.TORNADO;
		this.m_StartPos = new Vector3(position.x - fRadius, position.y, position.z);
		this.m_EndPos = new Vector3(position.x - fRadius, position.y + fHeight, position.z);
		this.m_fDist = Vector3.Distance(this.m_StartPos, this.m_EndPos);
		this.SimulateStart(this.m_StartPos, fSpeed, bRotFront);
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x000CB6FC File Offset: 0x000C98FC
	public void SimulateScale(FxmTestControls.AXIS nTransAxis, float fHalfDist, float fStartPosition, float fSpeed, bool bRotFront)
	{
		Vector3 position = base.transform.position;
		this.m_nAxis = nTransAxis;
		this.m_StartPos = position;
		this.m_EndPos = position;
		ref Vector3 ptr = ref this.m_StartPos;
		int nAxis;
		int num = nAxis = (int)this.m_nAxis;
		float num2 = ptr[nAxis];
		this.m_StartPos[num] = num2 + fHalfDist * fStartPosition;
		ref Vector3 ptr2 = ref this.m_EndPos;
		int num3 = nAxis = (int)this.m_nAxis;
		num2 = ptr2[nAxis];
		this.m_EndPos[num3] = num2 + (fHalfDist * 2f + fHalfDist * fStartPosition);
		this.m_fDist = Vector3.Distance(this.m_StartPos, this.m_EndPos);
		this.m_Mode = FxmTestSimulate.MODE_TYPE.SCALE;
		this.SimulateStart(this.m_StartPos, fSpeed, bRotFront);
	}

	// Token: 0x06001912 RID: 6418 RVA: 0x000104DA File Offset: 0x0000E6DA
	public void Stop()
	{
		this.m_fSpeed = 0f;
	}

	// Token: 0x06001913 RID: 6419 RVA: 0x000CB7A8 File Offset: 0x000C99A8
	private void SimulateStart(Vector3 startPos, float fSpeed, bool bRotFront)
	{
		base.transform.position = startPos;
		this.m_fSpeed = fSpeed;
		this.m_bRotFront = bRotFront;
		this.m_nCircleCount = 0;
		this.m_PrevPosition = Vector3.zero;
		if (bRotFront && this.m_Mode == FxmTestSimulate.MODE_TYPE.MOVE)
		{
			base.transform.LookAt(this.m_EndPos);
		}
		if (this.m_Mode != FxmTestSimulate.MODE_TYPE.SCALE && 1 < this.m_nMultiShotCount)
		{
			NcDuplicator ncDuplicator = base.gameObject.AddComponent<NcDuplicator>();
			ncDuplicator.m_fDuplicateTime = 0.2f;
			ncDuplicator.m_nDuplicateCount = this.m_nMultiShotCount;
			ncDuplicator.m_fDuplicateLifeTime = 0f;
			FxmTestSimulate.m_nMultiShotCreate = 0;
			this.m_nMultiShotIndex = 0;
		}
		this.m_fStartTime = Time.time;
		this.Update();
	}

	// Token: 0x06001914 RID: 6420 RVA: 0x000CB868 File Offset: 0x000C9A68
	private Vector3 GetArcPos(float fTimeRate)
	{
		Vector3 vector = Vector3.Lerp(this.m_StartPos, this.m_EndPos, fTimeRate);
		return new Vector3(vector.x, this.m_Curve.Evaluate(fTimeRate) * this.m_fDist, vector.z);
	}

	// Token: 0x06001915 RID: 6421 RVA: 0x000104E7 File Offset: 0x0000E6E7
	private void Awake()
	{
		this.m_nMultiShotIndex = FxmTestSimulate.m_nMultiShotCreate;
		FxmTestSimulate.m_nMultiShotCreate++;
	}

	// Token: 0x06001916 RID: 6422 RVA: 0x00010500 File Offset: 0x0000E700
	private void Start()
	{
		this.m_fStartTime = Time.time;
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x000CB8B0 File Offset: 0x000C9AB0
	private void Update()
	{
		if (0f < this.m_fDist && 0f < this.m_fSpeed)
		{
			switch (this.m_Mode)
			{
			case FxmTestSimulate.MODE_TYPE.MOVE:
			{
				float num = this.m_fDist / this.m_fSpeed;
				float num2 = Time.time - this.m_fStartTime;
				base.transform.position = Vector3.Lerp(this.m_StartPos, this.m_EndPos, num2 / num);
				if (1f < num2 / num)
				{
					this.OnMoveEnd();
				}
				break;
			}
			case FxmTestSimulate.MODE_TYPE.ARC:
			{
				float num3 = this.m_fDist / this.m_fSpeed;
				float num4 = Time.time - this.m_fStartTime;
				Vector3 arcPos = this.GetArcPos(num4 / num3 + num4 / num3 * 0.01f);
				base.transform.position = this.GetArcPos(num4 / num3);
				if (this.m_bRotFront)
				{
					base.transform.LookAt(arcPos);
				}
				if (1f < num4 / num3)
				{
					this.OnMoveEnd();
				}
				break;
			}
			case FxmTestSimulate.MODE_TYPE.ROTATE:
			{
				float num5 = this.m_fSpeed / 3.14f * 360f;
				base.transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * ((this.m_fRadius != 0f) ? (num5 / (this.m_fRadius * 2f)) : 0f));
				if (this.m_PrevPosition.z < 0f && 0f < base.transform.position.z)
				{
					if (1 <= this.m_nCircleCount)
					{
						this.OnMoveEnd();
					}
					this.m_nCircleCount++;
				}
				break;
			}
			case FxmTestSimulate.MODE_TYPE.TORNADO:
			{
				float num6 = this.m_fDist / (this.m_fSpeed / 20f);
				float num7 = Time.time - this.m_fStartTime;
				Vector3 vector = Vector3.Lerp(this.m_StartPos, this.m_EndPos, num7 / num6);
				base.transform.position = new Vector3(base.transform.position.x, vector.y, base.transform.position.z);
				float num8 = this.m_fSpeed / 3.14f * 360f;
				base.transform.RotateAround(new Vector3(0f, vector.y, 0f), Vector3.up, Time.deltaTime * ((this.m_fRadius != 0f) ? (num8 / (this.m_fRadius * 2f)) : 0f));
				if (1f < num7 / num6)
				{
					this.OnMoveEnd();
				}
				break;
			}
			case FxmTestSimulate.MODE_TYPE.SCALE:
			{
				float num9 = this.m_fDist / this.m_fSpeed;
				float num10 = Time.time - this.m_fStartTime;
				Vector3 localScale;
				localScale..ctor(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z);
				localScale[(int)this.m_nAxis] = this.m_fDist * num10 / num9;
				if (localScale[(int)this.m_nAxis] == 0f)
				{
					localScale[(int)this.m_nAxis] = 0.001f;
				}
				base.transform.localScale = localScale;
				if (1f < num10 / num9)
				{
					this.OnMoveEnd();
				}
				break;
			}
			}
		}
		this.m_PrevPosition = base.transform.position;
	}

	// Token: 0x06001918 RID: 6424 RVA: 0x0000264F File Offset: 0x0000084F
	private void FixedUpdate()
	{
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x0000264F File Offset: 0x0000084F
	public void LateUpdate()
	{
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x000CBC54 File Offset: 0x000C9E54
	private void OnMoveEnd()
	{
		this.m_fSpeed = 0f;
		NgObject.SetActiveRecursively(base.gameObject, false);
		if (1 < FxmTestSimulate.m_nMultiShotCreate && this.m_nMultiShotIndex < FxmTestSimulate.m_nMultiShotCreate - 1)
		{
			return;
		}
		if (this.m_FXMakerControls != null)
		{
			this.m_FXMakerControls.SendMessage("OnActionTransEnd");
		}
	}

	// Token: 0x04001D6F RID: 7535
	public FxmTestSimulate.MODE_TYPE m_Mode;

	// Token: 0x04001D70 RID: 7536
	public FxmTestControls.AXIS m_nAxis;

	// Token: 0x04001D71 RID: 7537
	public float m_fStartTime;

	// Token: 0x04001D72 RID: 7538
	public Vector3 m_StartPos;

	// Token: 0x04001D73 RID: 7539
	public Vector3 m_EndPos;

	// Token: 0x04001D74 RID: 7540
	public float m_fSpeed;

	// Token: 0x04001D75 RID: 7541
	public bool m_bRotFront;

	// Token: 0x04001D76 RID: 7542
	public float m_fDist;

	// Token: 0x04001D77 RID: 7543
	public float m_fRadius;

	// Token: 0x04001D78 RID: 7544
	public float m_fArcLenRate;

	// Token: 0x04001D79 RID: 7545
	public AnimationCurve m_Curve;

	// Token: 0x04001D7A RID: 7546
	public Component m_FXMakerControls;

	// Token: 0x04001D7B RID: 7547
	public int m_nMultiShotIndex;

	// Token: 0x04001D7C RID: 7548
	public int m_nMultiShotCount;

	// Token: 0x04001D7D RID: 7549
	public int m_nCircleCount;

	// Token: 0x04001D7E RID: 7550
	public Vector3 m_PrevPosition = Vector3.zero;

	// Token: 0x04001D7F RID: 7551
	protected static int m_nMultiShotCreate;

	// Token: 0x02000409 RID: 1033
	public enum MODE_TYPE
	{
		// Token: 0x04001D81 RID: 7553
		NONE,
		// Token: 0x04001D82 RID: 7554
		MOVE,
		// Token: 0x04001D83 RID: 7555
		ARC,
		// Token: 0x04001D84 RID: 7556
		ROTATE,
		// Token: 0x04001D85 RID: 7557
		TORNADO,
		// Token: 0x04001D86 RID: 7558
		SCALE
	}
}
