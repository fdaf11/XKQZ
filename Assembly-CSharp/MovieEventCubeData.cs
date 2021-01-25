using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000282 RID: 642
public class MovieEventCubeData
{
	// Token: 0x06000BFD RID: 3069 RVA: 0x00062F6C File Offset: 0x0006116C
	public bool CopyFromComponent(MovieEventCube mec)
	{
		this.mecObjName = mec.gameObject.name;
		this.mecObjPos = mec.gameObject.transform.localPosition;
		this.mecObjEulerAngles = mec.gameObject.transform.localEulerAngles;
		this.mecObjScale = mec.gameObject.transform.localScale;
		this.mecObjActive = mec.gameObject.activeSelf;
		if (mec.gameObject.collider != null)
		{
			this.mecCliIsTrigger = mec.gameObject.collider.isTrigger;
			if (mec.gameObject.collider is BoxCollider)
			{
				this.mecCliMode = _EventCubeColliderMode.Box;
				BoxCollider boxCollider = mec.gameObject.collider as BoxCollider;
				this.mecCliSize = boxCollider.size;
			}
			if (mec.gameObject.collider is SphereCollider)
			{
				this.mecCliMode = _EventCubeColliderMode.Sphere;
				SphereCollider sphereCollider = mec.gameObject.collider as SphereCollider;
				this.mecCliRedius = sphereCollider.radius;
			}
			this.bInverse = mec.bInverse;
			this.strCollectionID = mec.strCollectionID;
			this.strQuestID = mec.strQuestID;
			this.iBattleID = mec.iBattleID;
			this.iEventID = mec.iEventID;
			this.strBGMusicName = mec.strBGMusicName;
			this.bCheckTime = mec.bCheckTime;
			this.fStartTime = mec.fStartTime;
			this.fEndTime = mec.fEndTime;
			this.bDontRemoveEventCube = mec.bDontRemoveEventCube;
			this.iItemID = mec.iItemID;
			this.iItemAmount = mec.iItemAmount;
			this.iDevelopQuestID = mec.iDevelopQuestID;
			this.CheckConvert = mec.CheckConvert;
			this.strTransferSceneName = mec.strTransferSceneName;
			this.vPos = mec.vPos;
			this.vLocalEulerAngles = mec.vLocalEulerAngles;
			this.fDir = mec.fDir;
			if (mec.goTarget != null)
			{
				GameObject gameObject = mec.goTarget;
				string text = "/" + gameObject.name;
				while (gameObject.transform.parent != null)
				{
					gameObject = mec.goTarget.transform.parent.gameObject;
					text = "/" + gameObject.name + text;
				}
				this.strTarget = text;
			}
			this.eventMode = mec.eventMode;
			for (int i = 0; i < mec.mustHaveList.Count; i++)
			{
				EventCubeTriggleNode eventCubeTriggleNode = mec.mustHaveList[i];
				this.mustHaveList.Add(eventCubeTriggleNode.Copy());
			}
			for (int i = 0; i < mec.oneHaveList.Count; i++)
			{
				EventCubeTriggleNode eventCubeTriggleNode2 = mec.oneHaveList[i];
				this.oneHaveList.Add(eventCubeTriggleNode2.Copy());
			}
			return true;
		}
		Debug.LogError("This obj has not collider");
		return false;
	}

	// Token: 0x04000DBD RID: 3517
	public string mecObjName;

	// Token: 0x04000DBE RID: 3518
	public MovieEventCubeData.Vector mecObjPos;

	// Token: 0x04000DBF RID: 3519
	public MovieEventCubeData.Vector mecObjEulerAngles;

	// Token: 0x04000DC0 RID: 3520
	public MovieEventCubeData.Vector mecObjScale;

	// Token: 0x04000DC1 RID: 3521
	public bool mecObjActive;

	// Token: 0x04000DC2 RID: 3522
	public _EventCubeColliderMode mecCliMode;

	// Token: 0x04000DC3 RID: 3523
	public bool mecCliIsTrigger;

	// Token: 0x04000DC4 RID: 3524
	public MovieEventCubeData.Vector mecCliSize;

	// Token: 0x04000DC5 RID: 3525
	public float mecCliRedius;

	// Token: 0x04000DC6 RID: 3526
	public bool bInverse;

	// Token: 0x04000DC7 RID: 3527
	public string strCollectionID;

	// Token: 0x04000DC8 RID: 3528
	public string strQuestID;

	// Token: 0x04000DC9 RID: 3529
	public int iBattleID;

	// Token: 0x04000DCA RID: 3530
	public int iEventID;

	// Token: 0x04000DCB RID: 3531
	public string strBGMusicName;

	// Token: 0x04000DCC RID: 3532
	public bool bCheckTime;

	// Token: 0x04000DCD RID: 3533
	public float fStartTime;

	// Token: 0x04000DCE RID: 3534
	public float fEndTime;

	// Token: 0x04000DCF RID: 3535
	public bool bDontRemoveEventCube;

	// Token: 0x04000DD0 RID: 3536
	public int iItemID;

	// Token: 0x04000DD1 RID: 3537
	public int iItemAmount;

	// Token: 0x04000DD2 RID: 3538
	public int iDevelopQuestID;

	// Token: 0x04000DD3 RID: 3539
	public bool CheckConvert;

	// Token: 0x04000DD4 RID: 3540
	public string strTransferSceneName;

	// Token: 0x04000DD5 RID: 3541
	public MovieEventCubeData.Vector vPos;

	// Token: 0x04000DD6 RID: 3542
	public MovieEventCubeData.Vector vLocalEulerAngles;

	// Token: 0x04000DD7 RID: 3543
	public float fDir;

	// Token: 0x04000DD8 RID: 3544
	public string strTarget;

	// Token: 0x04000DD9 RID: 3545
	public _EventCubeTriggleMode eventMode;

	// Token: 0x04000DDA RID: 3546
	public List<EventCubeTriggleNode> mustHaveList = new List<EventCubeTriggleNode>();

	// Token: 0x04000DDB RID: 3547
	public List<EventCubeTriggleNode> oneHaveList = new List<EventCubeTriggleNode>();

	// Token: 0x02000283 RID: 643
	public class Vector
	{
		// Token: 0x06000BFF RID: 3071 RVA: 0x00009257 File Offset: 0x00007457
		public static implicit operator Vector3(MovieEventCubeData.Vector v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00063288 File Offset: 0x00061488
		public static implicit operator MovieEventCubeData.Vector(Vector3 v)
		{
			return new MovieEventCubeData.Vector
			{
				x = v.x,
				y = v.y,
				z = v.z
			};
		}

		// Token: 0x04000DDC RID: 3548
		public float x;

		// Token: 0x04000DDD RID: 3549
		public float y;

		// Token: 0x04000DDE RID: 3550
		public float z;
	}
}
