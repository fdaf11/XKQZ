using System;
using UnityEngine;

// Token: 0x020003AE RID: 942
public class NcTransformTool
{
	// Token: 0x06001614 RID: 5652 RVA: 0x000B9FC0 File Offset: 0x000B81C0
	public NcTransformTool()
	{
		this.m_vecPos = default(Vector3);
		this.m_vecRot = default(Quaternion);
		this.m_vecRotHint = default(Vector3);
		this.m_vecScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x06001615 RID: 5653 RVA: 0x0000E2A8 File Offset: 0x0000C4A8
	public NcTransformTool(Transform val)
	{
		this.SetLocalTransform(val);
	}

	// Token: 0x06001616 RID: 5654 RVA: 0x0000E2B7 File Offset: 0x0000C4B7
	public static Vector3 GetZeroVector()
	{
		return Vector3.zero;
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x0000E2BE File Offset: 0x0000C4BE
	public static Vector3 GetUnitVector()
	{
		return new Vector3(1f, 1f, 1f);
	}

	// Token: 0x06001618 RID: 5656 RVA: 0x0000E2D4 File Offset: 0x0000C4D4
	public static Quaternion GetIdenQuaternion()
	{
		return Quaternion.identity;
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x0000E2DB File Offset: 0x0000C4DB
	public static void InitLocalTransform(Transform dst)
	{
		dst.localPosition = NcTransformTool.GetZeroVector();
		dst.localRotation = NcTransformTool.GetIdenQuaternion();
		dst.localScale = NcTransformTool.GetUnitVector();
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x0000E2FE File Offset: 0x0000C4FE
	public static void InitWorldTransform(Transform dst)
	{
		dst.position = NcTransformTool.GetZeroVector();
		dst.rotation = NcTransformTool.GetIdenQuaternion();
		NcTransformTool.InitWorldScale(dst);
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x000BA01C File Offset: 0x000B821C
	public static void InitWorldScale(Transform dst)
	{
		dst.localScale = NcTransformTool.GetUnitVector();
		dst.localScale = new Vector3((dst.lossyScale.x != 0f) ? (1f / dst.lossyScale.x) : 1f, (dst.lossyScale.y != 0f) ? (1f / dst.lossyScale.y) : 1f, (dst.lossyScale.z != 0f) ? (1f / dst.lossyScale.z) : 1f);
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x0000E31C File Offset: 0x0000C51C
	public static void CopyLocalTransform(Transform src, Transform dst)
	{
		dst.localPosition = src.localPosition;
		dst.localRotation = src.localRotation;
		dst.localScale = src.localScale;
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x000BA0E4 File Offset: 0x000B82E4
	public static void CopyLossyToLocalScale(Vector3 srcLossyScale, Transform dst)
	{
		dst.localScale = NcTransformTool.GetUnitVector();
		dst.localScale = new Vector3((dst.lossyScale.x != 0f) ? (srcLossyScale.x / dst.lossyScale.x) : srcLossyScale.x, (dst.lossyScale.y != 0f) ? (srcLossyScale.y / dst.lossyScale.y) : srcLossyScale.y, (dst.lossyScale.z != 0f) ? (srcLossyScale.z / dst.lossyScale.z) : srcLossyScale.z);
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x0000E342 File Offset: 0x0000C542
	public void CopyToLocalTransform(Transform dst)
	{
		dst.localPosition = this.m_vecPos;
		dst.localRotation = this.m_vecRot;
		dst.localScale = this.m_vecScale;
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x0000E368 File Offset: 0x0000C568
	public void CopyToTransform(Transform dst)
	{
		dst.position = this.m_vecPos;
		dst.rotation = this.m_vecRot;
		NcTransformTool.CopyLossyToLocalScale(this.m_vecScale, dst);
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x000BA1B8 File Offset: 0x000B83B8
	public void AddLocalTransform(Transform val)
	{
		this.m_vecPos += val.localPosition;
		this.m_vecRot = Quaternion.Euler(this.m_vecRot.eulerAngles + val.localRotation.eulerAngles);
		this.m_vecScale = Vector3.Scale(this.m_vecScale, val.localScale);
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x0000E38E File Offset: 0x0000C58E
	public void SetLocalTransform(Transform val)
	{
		this.m_vecPos = val.localPosition;
		this.m_vecRot = val.localRotation;
		this.m_vecScale = val.localScale;
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x000BA21C File Offset: 0x000B841C
	public bool IsLocalEquals(Transform val)
	{
		return !(this.m_vecPos != val.localPosition) && !(this.m_vecRot != val.localRotation) && !(this.m_vecScale != val.localScale);
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x000BA274 File Offset: 0x000B8474
	public void AddTransform(Transform val)
	{
		this.m_vecPos += val.position;
		this.m_vecRot = Quaternion.Euler(this.m_vecRot.eulerAngles + val.rotation.eulerAngles);
		this.m_vecScale = Vector3.Scale(this.m_vecScale, val.lossyScale);
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x0000E3B4 File Offset: 0x0000C5B4
	public void SetTransform(Transform val)
	{
		this.m_vecPos = val.position;
		this.m_vecRot = val.rotation;
		this.m_vecScale = val.lossyScale;
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x000BA2D8 File Offset: 0x000B84D8
	public bool IsEquals(Transform val)
	{
		return !(this.m_vecPos != val.position) && !(this.m_vecRot != val.rotation) && !(this.m_vecScale != val.lossyScale);
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x0000E3DA File Offset: 0x0000C5DA
	public void SetTransform(NcTransformTool val)
	{
		this.m_vecPos = val.m_vecPos;
		this.m_vecRot = val.m_vecRot;
		this.m_vecScale = val.m_vecScale;
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x000BA330 File Offset: 0x000B8530
	public static float GetTransformScaleMeanValue(Transform srcTrans)
	{
		return (srcTrans.lossyScale.x + srcTrans.lossyScale.y + srcTrans.lossyScale.z) / 3f;
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x000BA370 File Offset: 0x000B8570
	public static Vector3 GetTransformScaleMeanVector(Transform srcTrans)
	{
		float transformScaleMeanValue = NcTransformTool.GetTransformScaleMeanValue(srcTrans);
		return new Vector3(transformScaleMeanValue, transformScaleMeanValue, transformScaleMeanValue);
	}

	// Token: 0x04001A77 RID: 6775
	public Vector3 m_vecPos;

	// Token: 0x04001A78 RID: 6776
	public Quaternion m_vecRot;

	// Token: 0x04001A79 RID: 6777
	public Vector3 m_vecRotHint;

	// Token: 0x04001A7A RID: 6778
	public Vector3 m_vecScale;
}
