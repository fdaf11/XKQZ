using System;
using Heluo.Wulin.HeadLook;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000389 RID: 905
	public class MouseEventCubeData
	{
		// Token: 0x06001511 RID: 5393 RVA: 0x000B5288 File Offset: 0x000B3488
		public MouseEventCube CreateToGameObject()
		{
			if (!this.msecObjName.Contains("MEC"))
			{
				this.msecObjName += "_MEC_Game";
			}
			GameObject gameObject = new GameObject(this.msecObjName);
			gameObject.transform.position = this.msecObjPos;
			gameObject.transform.eulerAngles = this.msecObjEulerAngles;
			gameObject.transform.localScale = this.msecObjScale;
			gameObject.SetActive(this.msecObjActive);
			gameObject.tag = "Chest";
			gameObject.layer = LayerMask.NameToLayer("Item");
			if (this.mecCliMode == _EventCubeColliderMode.Box)
			{
				BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
				boxCollider.size = this.msecCliSize;
				boxCollider.center = this.msecCliCenter;
			}
			if (this.mecCliMode == _EventCubeColliderMode.Sphere)
			{
				SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
				sphereCollider.center = this.msecCliCenter;
				sphereCollider.radius = this.msecCliRedius;
			}
			MouseEventCube mouseEventCube = gameObject.AddComponent<MouseEventCube>();
			mouseEventCube.m_QuestID = this.m_QuestID;
			mouseEventCube.m_GroundItemID = this.m_GroundItemID;
			mouseEventCube.m_ParentName = this.m_ParentName;
			mouseEventCube.m_ChildIndexList = this.m_ChildIndexList;
			mouseEventCube.m_bSearch = this.m_bSearch;
			mouseEventCube.m_bHide = this.m_bHide;
			mouseEventCube.FindAttachObj();
			if (this.HasLook)
			{
				HeadLookTarget headLookTarget = mouseEventCube.gameObject.AddComponent<HeadLookTarget>();
				headLookTarget.Offset = this.Offset;
				headLookTarget.Dis = this.Dis;
			}
			return mouseEventCube;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x000B5428 File Offset: 0x000B3628
		public bool CopyFromComponent(MouseEventCube msec)
		{
			this.msecObjName = msec.gameObject.name;
			this.msecObjPos = msec.gameObject.transform.position;
			this.msecObjEulerAngles = msec.gameObject.transform.eulerAngles;
			this.msecObjScale = msec.gameObject.transform.lossyScale;
			this.msecObjActive = msec.gameObject.activeSelf;
			if (!(msec.gameObject.collider != null))
			{
				Debug.LogError("This obj has not collider");
				return false;
			}
			this.mecCliIsTrigger = msec.gameObject.collider.isTrigger;
			if (msec.gameObject.collider is BoxCollider)
			{
				this.mecCliMode = _EventCubeColliderMode.Box;
				BoxCollider boxCollider = msec.gameObject.collider as BoxCollider;
				this.msecCliSize = boxCollider.size;
				this.msecCliCenter = boxCollider.center;
			}
			if (msec.gameObject.collider is SphereCollider)
			{
				this.mecCliMode = _EventCubeColliderMode.Sphere;
				SphereCollider sphereCollider = msec.gameObject.collider as SphereCollider;
				this.msecCliRedius = sphereCollider.radius;
				this.msecCliCenter = sphereCollider.center;
			}
			this.m_QuestID = msec.m_QuestID;
			this.m_GroundItemID = msec.m_GroundItemID;
			msec.GetPath();
			if (msec.AttachObj == null)
			{
				return false;
			}
			this.m_ObjName = msec.AttachObj.name;
			this.m_ParentName = msec.m_ParentName;
			this.m_ChildIndexList = msec.m_ChildIndexList;
			this.m_bSearch = msec.m_bSearch;
			this.m_bHide = msec.m_bHide;
			HeadLookTarget component = msec.gameObject.GetComponent<HeadLookTarget>();
			if (component != null)
			{
				this.HasLook = true;
				this.Offset = component.Offset;
				this.Dis = component.Dis;
			}
			else
			{
				this.HasLook = false;
			}
			return true;
		}

		// Token: 0x040019A9 RID: 6569
		public string m_QuestID;

		// Token: 0x040019AA RID: 6570
		public string m_GroundItemID;

		// Token: 0x040019AB RID: 6571
		public string m_ObjName;

		// Token: 0x040019AC RID: 6572
		public string m_ParentName;

		// Token: 0x040019AD RID: 6573
		public bool m_bHide = true;

		// Token: 0x040019AE RID: 6574
		public bool m_bSearch;

		// Token: 0x040019AF RID: 6575
		public int[] m_ChildIndexList;

		// Token: 0x040019B0 RID: 6576
		public string m_strBGMusicName;

		// Token: 0x040019B1 RID: 6577
		public string msecObjName;

		// Token: 0x040019B2 RID: 6578
		public MouseEventCubeData.Vector msecObjPos;

		// Token: 0x040019B3 RID: 6579
		public MouseEventCubeData.Vector msecObjEulerAngles;

		// Token: 0x040019B4 RID: 6580
		public MouseEventCubeData.Vector msecObjScale;

		// Token: 0x040019B5 RID: 6581
		public bool msecObjActive;

		// Token: 0x040019B6 RID: 6582
		public _EventCubeColliderMode mecCliMode;

		// Token: 0x040019B7 RID: 6583
		public bool mecCliIsTrigger;

		// Token: 0x040019B8 RID: 6584
		public MouseEventCubeData.Vector msecCliCenter;

		// Token: 0x040019B9 RID: 6585
		public MouseEventCubeData.Vector msecCliSize;

		// Token: 0x040019BA RID: 6586
		public float msecCliRedius;

		// Token: 0x040019BB RID: 6587
		public bool HasLook;

		// Token: 0x040019BC RID: 6588
		public MouseEventCubeData.Vector Offset;

		// Token: 0x040019BD RID: 6589
		public float Dis;

		// Token: 0x0200038A RID: 906
		public class Vector
		{
			// Token: 0x06001514 RID: 5396 RVA: 0x0000D6AA File Offset: 0x0000B8AA
			public static implicit operator Vector3(MouseEventCubeData.Vector v)
			{
				return new Vector3(v.x, v.y, v.z);
			}

			// Token: 0x06001515 RID: 5397 RVA: 0x000B563C File Offset: 0x000B383C
			public static implicit operator MouseEventCubeData.Vector(Vector3 v)
			{
				return new MouseEventCubeData.Vector
				{
					x = v.x,
					y = v.y,
					z = v.z
				};
			}

			// Token: 0x040019BE RID: 6590
			public float x;

			// Token: 0x040019BF RID: 6591
			public float y;

			// Token: 0x040019C0 RID: 6592
			public float z;
		}
	}
}
