using System;
using UnityEngine;

// Token: 0x020003C7 RID: 967
public class NcBillboard : NcEffectBehaviour
{
	// Token: 0x060016FE RID: 5886 RVA: 0x0000264F File Offset: 0x0000084F
	private void Awake()
	{
	}

	// Token: 0x060016FF RID: 5887 RVA: 0x0000EDA9 File Offset: 0x0000CFA9
	private void OnEnable()
	{
		this.UpdateBillboard();
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x0000EDB1 File Offset: 0x0000CFB1
	public void UpdateBillboard()
	{
		this.m_fRndValue = Random.Range(0f, 360f);
		if (base.enabled)
		{
			this.Update();
		}
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x0000EDD9 File Offset: 0x0000CFD9
	private void Start()
	{
		this.m_qOiginal = base.transform.rotation;
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x000BDD9C File Offset: 0x000BBF9C
	private void Update()
	{
		if (Camera.main == null)
		{
			return;
		}
		Vector3 vector;
		if (this.m_bFixedObjectUp)
		{
			vector = base.transform.up;
		}
		else
		{
			vector = Camera.main.transform.rotation * Vector3.up;
		}
		if (this.m_bCameraLookAt)
		{
			base.transform.LookAt(Camera.main.transform, vector);
		}
		else
		{
			base.transform.LookAt(base.transform.position + Camera.main.transform.rotation * Vector3.back, vector);
		}
		switch (this.m_FrontAxis)
		{
		case NcBillboard.AXIS_TYPE.AXIS_BACK:
			base.transform.Rotate(base.transform.up, 180f, 0);
			break;
		case NcBillboard.AXIS_TYPE.AXIS_RIGHT:
			base.transform.Rotate(base.transform.up, 270f, 0);
			break;
		case NcBillboard.AXIS_TYPE.AXIS_LEFT:
			base.transform.Rotate(base.transform.up, 90f, 0);
			break;
		case NcBillboard.AXIS_TYPE.AXIS_UP:
			base.transform.Rotate(base.transform.right, 90f, 0);
			break;
		case NcBillboard.AXIS_TYPE.AXIS_DOWN:
			base.transform.Rotate(base.transform.right, 270f, 0);
			break;
		}
		if (this.m_bFixedStand)
		{
			base.transform.rotation = Quaternion.Euler(new Vector3(0f, base.transform.rotation.eulerAngles.y, base.transform.rotation.eulerAngles.z));
		}
		if (this.m_RatationMode == NcBillboard.ROTATION.RND)
		{
			base.transform.localRotation *= Quaternion.Euler((this.m_RatationAxis != NcBillboard.AXIS.X) ? 0f : this.m_fRndValue, (this.m_RatationAxis != NcBillboard.AXIS.Y) ? 0f : this.m_fRndValue, (this.m_RatationAxis != NcBillboard.AXIS.Z) ? 0f : this.m_fRndValue);
		}
		if (this.m_RatationMode == NcBillboard.ROTATION.ROTATE)
		{
			float num = NcEffectBehaviour.GetEngineDeltaTime() * this.m_fRotationValue;
			base.transform.Rotate((this.m_RatationAxis != NcBillboard.AXIS.X) ? 0f : num, (this.m_RatationAxis != NcBillboard.AXIS.Y) ? 0f : num, (this.m_RatationAxis != NcBillboard.AXIS.Z) ? 0f : num, 1);
		}
	}

	// Token: 0x04001B48 RID: 6984
	public bool m_bCameraLookAt;

	// Token: 0x04001B49 RID: 6985
	public bool m_bFixedObjectUp;

	// Token: 0x04001B4A RID: 6986
	public bool m_bFixedStand;

	// Token: 0x04001B4B RID: 6987
	public NcBillboard.AXIS_TYPE m_FrontAxis;

	// Token: 0x04001B4C RID: 6988
	public NcBillboard.ROTATION m_RatationMode;

	// Token: 0x04001B4D RID: 6989
	public NcBillboard.AXIS m_RatationAxis = NcBillboard.AXIS.Z;

	// Token: 0x04001B4E RID: 6990
	public float m_fRotationValue = 180f;

	// Token: 0x04001B4F RID: 6991
	protected float m_fRndValue;

	// Token: 0x04001B50 RID: 6992
	protected float m_fTotalRotationValue;

	// Token: 0x04001B51 RID: 6993
	protected Quaternion m_qOiginal;

	// Token: 0x020003C8 RID: 968
	public enum AXIS_TYPE
	{
		// Token: 0x04001B53 RID: 6995
		AXIS_FORWARD,
		// Token: 0x04001B54 RID: 6996
		AXIS_BACK,
		// Token: 0x04001B55 RID: 6997
		AXIS_RIGHT,
		// Token: 0x04001B56 RID: 6998
		AXIS_LEFT,
		// Token: 0x04001B57 RID: 6999
		AXIS_UP,
		// Token: 0x04001B58 RID: 7000
		AXIS_DOWN
	}

	// Token: 0x020003C9 RID: 969
	public enum ROTATION
	{
		// Token: 0x04001B5A RID: 7002
		NONE,
		// Token: 0x04001B5B RID: 7003
		RND,
		// Token: 0x04001B5C RID: 7004
		ROTATE
	}

	// Token: 0x020003CA RID: 970
	public enum AXIS
	{
		// Token: 0x04001B5E RID: 7006
		X,
		// Token: 0x04001B5F RID: 7007
		Y,
		// Token: 0x04001B60 RID: 7008
		Z
	}
}
