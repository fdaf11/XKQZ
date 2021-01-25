using System;
using UnityEngine;

// Token: 0x020002B6 RID: 694
public class MovieEventNodeJson
{
	// Token: 0x06000D69 RID: 3433 RVA: 0x0006CCC0 File Offset: 0x0006AEC0
	public MovieEventNode ToPlayMode()
	{
		return new MovieEventNode
		{
			NodeID = this.m_NodeID,
			strEventName = this.m_strEventName,
			mEventnType = this.m_mEventnType,
			strActorName = this.m_strActorName,
			strActorTag = this.m_strActorTag,
			strAnimationClipName = this.m_strAnimationClipName,
			mTextOutType = this.m_mTextOutType,
			iTextOrder = this.m_iTextOrder,
			fDelayTime = this.m_fDelayTime,
			bMovieStart = this.m_bMovieStart,
			vPos = this.m_vPos,
			wrapMode = this.m_wrapMode,
			iAniLayer = this.m_iAniLayer,
			fAniSpeed = this.m_fAniSpeed,
			bCrossFade = this.m_bCrossFade,
			fMoveSpeed = this.m_fMoveSpeed,
			NextNodeID = this.m_NextNodeID,
			TriggerNodeID = this.m_TriggerNodeID,
			bLookAtMoving = this.m_bLookAtMoving,
			strSoundName = this.m_strSoundName,
			strLookatName = this.m_strLookatName,
			strLookatTag = this.m_strLookatTag,
			vPosDest = this.m_vPosDest,
			fPos = this.m_fPos,
			fTotal = this.m_fTotal,
			iOption1 = this.m_iOption1,
			iOption2 = this.m_iOption2,
			iOption3 = this.m_iOption3,
			iOption4 = this.m_iOption4,
			strScenesName = this.m_strScenesName,
			vLinkTerrain = this.m_vLinkTerrain
		};
	}

	// Token: 0x04000F87 RID: 3975
	public int m_NodeID;

	// Token: 0x04000F88 RID: 3976
	public string m_strEventName;

	// Token: 0x04000F89 RID: 3977
	public _MovieEventNodeType m_mEventnType;

	// Token: 0x04000F8A RID: 3978
	public string m_strActorName;

	// Token: 0x04000F8B RID: 3979
	public string m_strActorTag;

	// Token: 0x04000F8C RID: 3980
	public string m_strAnimationClipName;

	// Token: 0x04000F8D RID: 3981
	public string m_acAnim;

	// Token: 0x04000F8E RID: 3982
	public _MovieTextOutType m_mTextOutType;

	// Token: 0x04000F8F RID: 3983
	public int m_iTextOrder;

	// Token: 0x04000F90 RID: 3984
	public float m_fDelayTime;

	// Token: 0x04000F91 RID: 3985
	public bool m_bMovieStart;

	// Token: 0x04000F92 RID: 3986
	public MovieEventNodeJson.Vector m_vPos;

	// Token: 0x04000F93 RID: 3987
	public WrapMode m_wrapMode;

	// Token: 0x04000F94 RID: 3988
	public int m_iAniLayer;

	// Token: 0x04000F95 RID: 3989
	public float m_fAniSpeed;

	// Token: 0x04000F96 RID: 3990
	public bool m_bCrossFade;

	// Token: 0x04000F97 RID: 3991
	public float m_fMoveSpeed;

	// Token: 0x04000F98 RID: 3992
	public int m_NextNodeID;

	// Token: 0x04000F99 RID: 3993
	public int m_TriggerNodeID;

	// Token: 0x04000F9A RID: 3994
	public bool m_bLookAtMoving;

	// Token: 0x04000F9B RID: 3995
	public string m_strSoundName;

	// Token: 0x04000F9C RID: 3996
	public string m_strLookatName;

	// Token: 0x04000F9D RID: 3997
	public string m_strLookatTag;

	// Token: 0x04000F9E RID: 3998
	public MovieEventNodeJson.Vector m_vPosDest;

	// Token: 0x04000F9F RID: 3999
	public float m_fPos;

	// Token: 0x04000FA0 RID: 4000
	public float m_fTotal;

	// Token: 0x04000FA1 RID: 4001
	public int m_iOption1 = -1;

	// Token: 0x04000FA2 RID: 4002
	public int m_iOption2 = -1;

	// Token: 0x04000FA3 RID: 4003
	public int m_iOption3 = -1;

	// Token: 0x04000FA4 RID: 4004
	public int m_iOption4 = -1;

	// Token: 0x04000FA5 RID: 4005
	public string m_strScenesName;

	// Token: 0x04000FA6 RID: 4006
	public MovieEventNodeJson.Vector m_vLinkTerrain = Vector3.down;

	// Token: 0x020002B7 RID: 695
	public class Vector
	{
		// Token: 0x06000D6B RID: 3435 RVA: 0x000096B0 File Offset: 0x000078B0
		public static implicit operator Vector3(MovieEventNodeJson.Vector v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0006CE58 File Offset: 0x0006B058
		public static implicit operator MovieEventNodeJson.Vector(Vector3 v)
		{
			return new MovieEventNodeJson.Vector
			{
				x = v.x,
				y = v.y,
				z = v.z
			};
		}

		// Token: 0x04000FA7 RID: 4007
		public float x;

		// Token: 0x04000FA8 RID: 4008
		public float y;

		// Token: 0x04000FA9 RID: 4009
		public float z;
	}
}
