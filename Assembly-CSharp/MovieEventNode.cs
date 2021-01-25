using System;
using UnityEngine;

// Token: 0x020002B5 RID: 693
[Serializable]
public class MovieEventNode
{
	// Token: 0x06000D67 RID: 3431 RVA: 0x0006CB28 File Offset: 0x0006AD28
	public MovieEventNodeJson ToJson()
	{
		return new MovieEventNodeJson
		{
			m_NodeID = this.NodeID,
			m_strEventName = this.strEventName,
			m_mEventnType = this.mEventnType,
			m_strActorName = this.strActorName,
			m_strActorTag = this.strActorTag,
			m_strAnimationClipName = this.strAnimationClipName,
			m_mTextOutType = this.mTextOutType,
			m_iTextOrder = this.iTextOrder,
			m_fDelayTime = this.fDelayTime,
			m_bMovieStart = this.bMovieStart,
			m_vPos = this.vPos,
			m_wrapMode = this.wrapMode,
			m_iAniLayer = this.iAniLayer,
			m_fAniSpeed = this.fAniSpeed,
			m_bCrossFade = this.bCrossFade,
			m_fMoveSpeed = this.fMoveSpeed,
			m_NextNodeID = this.NextNodeID,
			m_TriggerNodeID = this.TriggerNodeID,
			m_bLookAtMoving = this.bLookAtMoving,
			m_strSoundName = this.strSoundName,
			m_strLookatName = this.strLookatName,
			m_strLookatTag = this.strLookatTag,
			m_vPosDest = this.vPosDest,
			m_fPos = this.fPos,
			m_fTotal = this.fTotal,
			m_iOption1 = this.iOption1,
			m_iOption2 = this.iOption2,
			m_iOption3 = this.iOption3,
			m_iOption4 = this.iOption4,
			m_strScenesName = this.strScenesName,
			m_vLinkTerrain = this.vLinkTerrain
		};
	}

	// Token: 0x04000F62 RID: 3938
	public int GroupID = -1;

	// Token: 0x04000F63 RID: 3939
	public int NodeID;

	// Token: 0x04000F64 RID: 3940
	public string strEventName;

	// Token: 0x04000F65 RID: 3941
	public _MovieEventNodeType mEventnType;

	// Token: 0x04000F66 RID: 3942
	public string strActorName;

	// Token: 0x04000F67 RID: 3943
	public string strActorTag;

	// Token: 0x04000F68 RID: 3944
	public GameObject goActor;

	// Token: 0x04000F69 RID: 3945
	public string strAnimationClipName;

	// Token: 0x04000F6A RID: 3946
	public AnimationClip acAnim;

	// Token: 0x04000F6B RID: 3947
	public _MovieTextOutType mTextOutType;

	// Token: 0x04000F6C RID: 3948
	public int iTextOrder;

	// Token: 0x04000F6D RID: 3949
	public float fDelayTime;

	// Token: 0x04000F6E RID: 3950
	public bool bMovieStart;

	// Token: 0x04000F6F RID: 3951
	public Vector3 vPos;

	// Token: 0x04000F70 RID: 3952
	public WrapMode wrapMode;

	// Token: 0x04000F71 RID: 3953
	public int iAniLayer;

	// Token: 0x04000F72 RID: 3954
	public float fAniSpeed = 1f;

	// Token: 0x04000F73 RID: 3955
	public bool bCrossFade;

	// Token: 0x04000F74 RID: 3956
	public float fMoveSpeed = 1f;

	// Token: 0x04000F75 RID: 3957
	public int NextNodeID = -1;

	// Token: 0x04000F76 RID: 3958
	public int TriggerNodeID = -1;

	// Token: 0x04000F77 RID: 3959
	public bool bLookAtMoving = true;

	// Token: 0x04000F78 RID: 3960
	public AudioClip acSound;

	// Token: 0x04000F79 RID: 3961
	public string strSoundName;

	// Token: 0x04000F7A RID: 3962
	public string strLookatName;

	// Token: 0x04000F7B RID: 3963
	public string strLookatTag;

	// Token: 0x04000F7C RID: 3964
	public GameObject goLookat;

	// Token: 0x04000F7D RID: 3965
	public Vector3 vPosDest;

	// Token: 0x04000F7E RID: 3966
	public float fPos;

	// Token: 0x04000F7F RID: 3967
	public float fTotal;

	// Token: 0x04000F80 RID: 3968
	public int iOption1 = -1;

	// Token: 0x04000F81 RID: 3969
	public int iOption2 = -1;

	// Token: 0x04000F82 RID: 3970
	public int iOption3 = -1;

	// Token: 0x04000F83 RID: 3971
	public int iOption4 = -1;

	// Token: 0x04000F84 RID: 3972
	public string strScenesName;

	// Token: 0x04000F85 RID: 3973
	public Vector3 vLinkTerrain = Vector3.down;

	// Token: 0x04000F86 RID: 3974
	public Vector3 vOrigPos;
}
