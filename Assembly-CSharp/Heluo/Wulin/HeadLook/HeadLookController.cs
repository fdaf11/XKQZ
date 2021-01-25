using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin.HeadLook
{
	// Token: 0x020001AC RID: 428
	public class HeadLookController : MonoBehaviour
	{
		// Token: 0x06000901 RID: 2305 RVA: 0x0000767E File Offset: 0x0000587E
		public static void AddLookTarget(HeadLookTarget target)
		{
			if (!HeadLookController.lookList.Contains(target))
			{
				HeadLookController.lookList.Add(target);
			}
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0000769B File Offset: 0x0000589B
		public static void RemoveLookTarget(HeadLookTarget target)
		{
			if (HeadLookController.lookList.Contains(target))
			{
				HeadLookController.lookList.Remove(target);
			}
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x000076B9 File Offset: 0x000058B9
		private void Start()
		{
			this.InitHeadLookController();
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0004E3C4 File Offset: 0x0004C5C4
		public void InitHeadLookController()
		{
			if (this.rootNode == null)
			{
				this.rootNode = base.transform;
			}
			this.m_BoneInfo = new BoneInfo(this.rootNode);
			if (this.m_BoneInfo.m_BoneList.Length == 0)
			{
				return;
			}
			if (this.segments == null || this.segments.Length == 0)
			{
				this.segments = this.m_BoneInfo.GetBendingSegment();
			}
			if (this.nonAffectedJoints == null || this.nonAffectedJoints.Length == 0)
			{
				this.nonAffectedJoints = this.m_BoneInfo.GetNonAffectedJoints();
			}
			foreach (BendingSegment bendingSegment in this.segments)
			{
				Quaternion rotation = bendingSegment.firstTransform.parent.rotation;
				Quaternion quaternion = Quaternion.Inverse(rotation);
				bendingSegment.referenceLookDir = quaternion * this.rootNode.rotation * this.headLookVector.normalized;
				bendingSegment.referenceUpDir = quaternion * this.rootNode.rotation * this.headUpVector.normalized;
				bendingSegment.angleH = 0f;
				bendingSegment.angleV = 0f;
				bendingSegment.dirUp = bendingSegment.referenceUpDir;
				bendingSegment.chainLength = 1;
				Transform transform = bendingSegment.lastTransform;
				while (transform != bendingSegment.firstTransform && transform != transform.root)
				{
					bendingSegment.chainLength++;
					transform = transform.parent;
				}
				bendingSegment.origRotations = new Quaternion[bendingSegment.chainLength];
				transform = bendingSegment.lastTransform;
				for (int j = bendingSegment.chainLength - 1; j >= 0; j--)
				{
					bendingSegment.origRotations[j] = transform.localRotation;
					transform = transform.parent;
				}
			}
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0004E5B8 File Offset: 0x0004C7B8
		private void Update()
		{
			if (this.MovieTarget != null)
			{
				this.SetTraget(this.MovieTarget.transform.position);
				return;
			}
			if (GameGlobal.m_bMovie)
			{
				return;
			}
			this.searchTargetList();
			this.updateLookTime();
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0004E604 File Offset: 0x0004C804
		private void searchTargetList()
		{
			if (this.isLooking)
			{
				return;
			}
			this.lookCoolDownCount += Time.deltaTime;
			if (this.lookCoolDownCount < this.lookCoolDown)
			{
				return;
			}
			this.lookObjectList.Clear();
			float num = this.DetectionRrange * this.DetectionRrange;
			float x = base.transform.position.x;
			float z = base.transform.position.z;
			for (int i = 0; i < HeadLookController.lookList.Count; i++)
			{
				HeadLookTarget headLookTarget = HeadLookController.lookList[i];
				if (headLookTarget.gameObject.activeSelf)
				{
					float x2 = headLookTarget.Origin.x;
					float z2 = headLookTarget.Origin.z;
					float num2 = (x - x2) * (x - x2) + (z - z2) * (z - z2);
					if (num2 <= num)
					{
						if (num2 <= headLookTarget.Dis * headLookTarget.Dis)
						{
							this.lookObjectList.Add(headLookTarget);
						}
					}
				}
			}
			if (this.lookObjectList != null && this.lookObjectList.Count > 0)
			{
				this.lookTime = (float)SimpleRandom.Range(3, 8);
				this.isLooking = true;
				int num3 = SimpleRandom.Range(0, this.lookObjectList.Count);
				this.lookObject = this.lookObjectList[num3];
				this.lookTimeCount = 0f;
				GameDebugTool.Log(string.Concat(new object[]
				{
					"肥宅 ",
					base.gameObject.name,
					" 偷偷看正妹 : ",
					this.lookObject.gameObject.name,
					" 秒數 : ",
					this.lookTime
				}));
			}
			else
			{
				this.lookObject = null;
				this.isLooking = false;
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0004E7FC File Offset: 0x0004C9FC
		private void updateLookTime()
		{
			if (!this.isLooking)
			{
				this.ResetTarget();
				return;
			}
			this.lookTimeCount += Time.deltaTime;
			if (this.lookTimeCount >= this.lookTime)
			{
				this.isLooking = false;
				this.lookCoolDown = (float)SimpleRandom.Range(8, 15);
				this.lookCoolDownCount = 0f;
				GameDebugTool.Log(string.Concat(new object[]
				{
					"肥宅 ",
					base.gameObject.name,
					" 假裝看前方 秒數 : ",
					this.lookCoolDown
				}));
			}
			if (this.lookObject != null)
			{
				float x = base.transform.position.x;
				float z = base.transform.position.z;
				float x2 = this.lookObject.Origin.x;
				float z2 = this.lookObject.Origin.z;
				float num = (x - x2) * (x - x2) + (z - z2) * (z - z2);
				if (num > this.lookObject.Dis * this.lookObject.Dis)
				{
					this.lookObject = null;
					this.isLooking = false;
				}
				else
				{
					this.SetTraget(this.lookObject.Target);
				}
			}
			else
			{
				this.ResetTarget();
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0004E960 File Offset: 0x0004CB60
		public void ResetTarget()
		{
			if (!base.animation.IsPlaying("stand01") || this.lookObjectList.Count != 0)
			{
				this.isUsuallyLook = false;
			}
			else
			{
				this.usuallyLookTimeCount += Time.deltaTime;
				if (this.usuallyLookTimeCount >= this.usuallyLookTime)
				{
					this.usuallyLookTimeCount = 0f;
					this.isUsuallyLook = !this.isUsuallyLook;
					if (this.isUsuallyLook)
					{
						this.usuallyLookTime = (float)SimpleRandom.Range(3, 8);
						float num = (float)SimpleRandom.PositiveOrNegative(60, 90);
						Vector3 vector = Quaternion.Euler(0f, num, 0f) * this.rootNode.forward;
						this.usuallyLook = this.m_BoneInfo.m_BoneList[3].position + vector * 1f;
						GameDebugTool.Log(string.Concat(new object[]
						{
							"肥宅沒事亂看中 : ",
							this.usuallyLookTime,
							" 角度 : ",
							num
						}));
					}
					else
					{
						this.usuallyLookTime = (float)SimpleRandom.Range(10, 20);
						GameDebugTool.Log("肥宅看正前方 : " + this.usuallyLookTime);
					}
				}
			}
			if (this.isUsuallyLook)
			{
				this.target = this.usuallyLook;
			}
			else
			{
				this.target = this.m_BoneInfo.m_BoneList[3].position + this.rootNode.forward * 100f;
			}
			GameDebugTool.DrawLine(this.m_BoneInfo.m_BoneList[3].position, this.target, Color.red);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x000076C1 File Offset: 0x000058C1
		public void SetTraget(Vector3 pos)
		{
			this.target = pos;
			GameDebugTool.DrawLine(this.m_BoneInfo.m_BoneList[3].position, this.target, Color.red);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0004EB20 File Offset: 0x0004CD20
		private void LateUpdate()
		{
			if (GameGlobal.m_bMovie)
			{
				return;
			}
			if (!base.animation.IsPlaying("run") && !base.animation.IsPlaying("stand01"))
			{
				return;
			}
			if (Time.deltaTime == 0f)
			{
				return;
			}
			if (this.segments == null || this.nonAffectedJoints == null)
			{
				return;
			}
			Vector3[] array = new Vector3[this.nonAffectedJoints.Length];
			for (int i = 0; i < this.nonAffectedJoints.Length; i++)
			{
				using (IEnumerator enumerator = this.nonAffectedJoints[i].joint.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Transform transform = (Transform)enumerator.Current;
						array[i] = transform.position - this.nonAffectedJoints[i].joint.position;
					}
				}
			}
			foreach (BendingSegment bendingSegment in this.segments)
			{
				Transform transform2 = bendingSegment.lastTransform;
				if (this.overrideAnimation)
				{
					for (int k = bendingSegment.chainLength - 1; k >= 0; k--)
					{
						transform2.localRotation = bendingSegment.origRotations[k];
						transform2 = transform2.parent;
					}
				}
				Quaternion rotation = bendingSegment.firstTransform.parent.rotation;
				Quaternion quaternion = Quaternion.Inverse(rotation);
				Vector3 normalized = (this.target - bendingSegment.lastTransform.position).normalized;
				Vector3 vector = quaternion * normalized;
				float num = HeadLookController.AngleAroundAxis(bendingSegment.referenceLookDir, vector, bendingSegment.referenceUpDir);
				Vector3 axis = Vector3.Cross(bendingSegment.referenceUpDir, vector);
				Vector3 dirA = vector - Vector3.Project(vector, bendingSegment.referenceUpDir);
				float num2 = HeadLookController.AngleAroundAxis(dirA, vector, axis);
				float num3 = Mathf.Max(0f, Mathf.Abs(num) - bendingSegment.thresholdAngleDifference) * Mathf.Sign(num);
				float num4 = Mathf.Max(0f, Mathf.Abs(num2) - bendingSegment.thresholdAngleDifference) * Mathf.Sign(num2);
				num = Mathf.Max(Mathf.Abs(num3) * Mathf.Abs(bendingSegment.bendingMultiplier), Mathf.Abs(num) - bendingSegment.maxAngleDifference) * Mathf.Sign(num) * Mathf.Sign(bendingSegment.bendingMultiplier);
				num2 = Mathf.Max(Mathf.Abs(num4) * Mathf.Abs(bendingSegment.bendingMultiplier), Mathf.Abs(num2) - bendingSegment.maxAngleDifference) * Mathf.Sign(num2) * Mathf.Sign(bendingSegment.bendingMultiplier);
				num = Mathf.Clamp(num, -bendingSegment.maxBendingAngle, bendingSegment.maxBendingAngle);
				num2 = Mathf.Clamp(num2, -bendingSegment.maxBendingAngle, bendingSegment.maxBendingAngle);
				Vector3 vector2 = Vector3.Cross(bendingSegment.referenceUpDir, bendingSegment.referenceLookDir);
				bendingSegment.angleH = Mathf.Lerp(bendingSegment.angleH, num, Time.deltaTime * bendingSegment.responsiveness);
				bendingSegment.angleV = Mathf.Lerp(bendingSegment.angleV, num2, Time.deltaTime * bendingSegment.responsiveness);
				vector = Quaternion.AngleAxis(bendingSegment.angleH, bendingSegment.referenceUpDir) * Quaternion.AngleAxis(bendingSegment.angleV, vector2) * bendingSegment.referenceLookDir;
				Vector3 referenceUpDir = bendingSegment.referenceUpDir;
				Vector3.OrthoNormalize(ref vector, ref referenceUpDir);
				Vector3 vector3 = vector;
				bendingSegment.dirUp = Vector3.Slerp(bendingSegment.dirUp, referenceUpDir, Time.deltaTime * 5f);
				Vector3.OrthoNormalize(ref vector3, ref bendingSegment.dirUp);
				Quaternion quaternion2 = rotation * Quaternion.LookRotation(vector3, bendingSegment.dirUp) * Quaternion.Inverse(rotation * Quaternion.LookRotation(bendingSegment.referenceLookDir, bendingSegment.referenceUpDir));
				Quaternion quaternion3 = Quaternion.Slerp(Quaternion.identity, quaternion2, this.effect / (float)bendingSegment.chainLength);
				transform2 = bendingSegment.lastTransform;
				for (int l = 0; l < bendingSegment.chainLength; l++)
				{
					transform2.rotation = quaternion3 * transform2.rotation;
					transform2 = transform2.parent;
				}
			}
			for (int m = 0; m < this.nonAffectedJoints.Length; m++)
			{
				Vector3 vector4 = Vector3.zero;
				using (IEnumerator enumerator2 = this.nonAffectedJoints[m].joint.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						Transform transform3 = (Transform)enumerator2.Current;
						vector4 = transform3.position - this.nonAffectedJoints[m].joint.position;
					}
				}
				Vector3 vector5 = Vector3.Slerp(array[m], vector4, this.nonAffectedJoints[m].effect);
				this.nonAffectedJoints[m].joint.rotation = Quaternion.FromToRotation(vector4, vector5) * this.nonAffectedJoints[m].joint.rotation;
			}
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0004F0A8 File Offset: 0x0004D2A8
		public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
		{
			dirA -= Vector3.Project(dirA, axis);
			dirB -= Vector3.Project(dirB, axis);
			float num = Vector3.Angle(dirA, dirB);
			return num * (float)((Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) >= 0f) ? 1 : -1);
		}

		// Token: 0x04000888 RID: 2184
		private static List<HeadLookTarget> lookList = new List<HeadLookTarget>();

		// Token: 0x04000889 RID: 2185
		public Transform rootNode;

		// Token: 0x0400088A RID: 2186
		public BendingSegment[] segments;

		// Token: 0x0400088B RID: 2187
		public NonAffectedJoints[] nonAffectedJoints;

		// Token: 0x0400088C RID: 2188
		public Vector3 headLookVector = Vector3.forward;

		// Token: 0x0400088D RID: 2189
		public Vector3 headUpVector = Vector3.up;

		// Token: 0x0400088E RID: 2190
		public Vector3 target = Vector3.zero;

		// Token: 0x0400088F RID: 2191
		public float effect = 1f;

		// Token: 0x04000890 RID: 2192
		public bool overrideAnimation;

		// Token: 0x04000891 RID: 2193
		private BoneInfo m_BoneInfo;

		// Token: 0x04000892 RID: 2194
		public GameObject MovieTarget;

		// Token: 0x04000893 RID: 2195
		private HeadLookTarget lookObject;

		// Token: 0x04000894 RID: 2196
		private float lookTime;

		// Token: 0x04000895 RID: 2197
		private float lookTimeCount;

		// Token: 0x04000896 RID: 2198
		private bool isLooking;

		// Token: 0x04000897 RID: 2199
		private float lookCoolDown;

		// Token: 0x04000898 RID: 2200
		private float lookCoolDownCount;

		// Token: 0x04000899 RID: 2201
		private Vector3 usuallyLook;

		// Token: 0x0400089A RID: 2202
		private float usuallyLookTime;

		// Token: 0x0400089B RID: 2203
		private float usuallyLookTimeCount;

		// Token: 0x0400089C RID: 2204
		private bool isUsuallyLook;

		// Token: 0x0400089D RID: 2205
		private List<HeadLookTarget> lookObjectList = new List<HeadLookTarget>();

		// Token: 0x0400089E RID: 2206
		private float smoothSpeed = 5f;

		// Token: 0x0400089F RID: 2207
		public float DetectionRrange = 10f;
	}
}
