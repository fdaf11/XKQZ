using System;
using UnityEngine;

namespace Heluo.Wulin.HeadLook
{
	// Token: 0x020001AA RID: 426
	public class BoneInfo
	{
		// Token: 0x060008FB RID: 2299 RVA: 0x0004E180 File Offset: 0x0004C380
		public BoneInfo(Transform t)
		{
			this.m_BoneList = new Transform[7];
			this.SetBoneTransform("Bip001/Bip001 Pelvis/Bip001 Spine", BoneInfo.eBone.Bip001Spine, t);
			this.SetBoneTransform("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1", BoneInfo.eBone.Bip001Spine1, t);
			this.SetBoneTransform("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck", BoneInfo.eBone.Bip001Neck, t);
			this.SetBoneTransform("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 Head", BoneInfo.eBone.Bip001Head, t);
			this.SetBoneTransform("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm", BoneInfo.eBone.Bip001LUpperArm, t);
			this.SetBoneTransform("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm", BoneInfo.eBone.Bip001RUpperArm, t);
			this.SetBoneTransform("head", BoneInfo.eBone.MeshHead, t);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0004E1FC File Offset: 0x0004C3FC
		private void SetBoneTransform(string name, BoneInfo.eBone eb, Transform t)
		{
			Transform transform = t.Find(name);
			this.m_BoneList[(int)eb] = transform;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0004E21C File Offset: 0x0004C41C
		public BendingSegment[] GetBendingSegment()
		{
			BendingSegment[] array = new BendingSegment[2];
			array[0] = new BendingSegment();
			array[0].firstTransform = this.m_BoneList[1];
			array[0].lastTransform = this.m_BoneList[2];
			array[0].thresholdAngleDifference = 30f;
			array[0].bendingMultiplier = 0.6f;
			array[0].maxAngleDifference = 90f;
			array[0].maxBendingAngle = 20f;
			array[0].responsiveness = 2.5f;
			array[1] = new BendingSegment();
			array[1].firstTransform = this.m_BoneList[2];
			array[1].lastTransform = this.m_BoneList[3];
			array[1].thresholdAngleDifference = 30f;
			array[1].bendingMultiplier = 0.7f;
			array[1].maxAngleDifference = 30f;
			array[1].maxBendingAngle = 80f;
			array[1].responsiveness = 4f;
			return array;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0004E304 File Offset: 0x0004C504
		public NonAffectedJoints[] GetNonAffectedJoints()
		{
			NonAffectedJoints[] array = new NonAffectedJoints[2];
			array[0] = new NonAffectedJoints();
			array[0].joint = this.m_BoneList[4];
			array[0].effect = 0.3f;
			array[1] = new NonAffectedJoints();
			array[1].joint = this.m_BoneList[5];
			array[1].effect = 0.3f;
			return array;
		}

		// Token: 0x0400087E RID: 2174
		public Transform[] m_BoneList;

		// Token: 0x020001AB RID: 427
		public enum eBone
		{
			// Token: 0x04000880 RID: 2176
			Bip001Spine,
			// Token: 0x04000881 RID: 2177
			Bip001Spine1,
			// Token: 0x04000882 RID: 2178
			Bip001Neck,
			// Token: 0x04000883 RID: 2179
			Bip001Head,
			// Token: 0x04000884 RID: 2180
			Bip001LUpperArm,
			// Token: 0x04000885 RID: 2181
			Bip001RUpperArm,
			// Token: 0x04000886 RID: 2182
			MeshHead,
			// Token: 0x04000887 RID: 2183
			Count
		}
	}
}
