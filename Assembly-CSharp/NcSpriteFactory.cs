using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003DF RID: 991
public class NcSpriteFactory : NcEffectBehaviour
{
	// Token: 0x060017B8 RID: 6072 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
	public bool IsUnused(int nNodeIndex)
	{
		return this.m_SpriteList[nNodeIndex].IsUnused() || nNodeIndex < this.m_nBuildStartIndex;
	}

	// Token: 0x060017B9 RID: 6073 RVA: 0x0000F7DC File Offset: 0x0000D9DC
	public NcSpriteFactory.NcSpriteNode GetSpriteNode(int nIndex)
	{
		if (this.m_SpriteList == null || nIndex < 0 || this.m_SpriteList.Count <= nIndex)
		{
			return null;
		}
		return this.m_SpriteList[nIndex];
	}

	// Token: 0x060017BA RID: 6074 RVA: 0x000C304C File Offset: 0x000C124C
	public NcSpriteFactory.NcSpriteNode GetSpriteNode(string spriteName)
	{
		if (this.m_SpriteList == null)
		{
			return null;
		}
		foreach (NcSpriteFactory.NcSpriteNode ncSpriteNode in this.m_SpriteList)
		{
			if (ncSpriteNode.m_SpriteName == spriteName)
			{
				return ncSpriteNode;
			}
		}
		return null;
	}

	// Token: 0x060017BB RID: 6075 RVA: 0x000C30C8 File Offset: 0x000C12C8
	public int GetSpriteNodeIndex(string spriteName)
	{
		if (this.m_SpriteList == null)
		{
			return -1;
		}
		for (int i = 0; i < this.m_SpriteList.Count; i++)
		{
			if (this.m_SpriteList[i].m_SpriteName == spriteName)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x000C3120 File Offset: 0x000C1320
	public NcSpriteFactory.NcSpriteNode SetSpriteNode(int nIndex, NcSpriteFactory.NcSpriteNode newInfo)
	{
		if (this.m_SpriteList == null || nIndex < 0 || this.m_SpriteList.Count <= nIndex)
		{
			return null;
		}
		NcSpriteFactory.NcSpriteNode result = this.m_SpriteList[nIndex];
		this.m_SpriteList[nIndex] = newInfo;
		return result;
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x000C3170 File Offset: 0x000C1370
	public int AddSpriteNode()
	{
		NcSpriteFactory.NcSpriteNode ncSpriteNode = new NcSpriteFactory.NcSpriteNode();
		if (this.m_SpriteList == null)
		{
			this.m_SpriteList = new List<NcSpriteFactory.NcSpriteNode>();
		}
		this.m_SpriteList.Add(ncSpriteNode);
		return this.m_SpriteList.Count - 1;
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x0000F80F File Offset: 0x0000DA0F
	public int AddSpriteNode(NcSpriteFactory.NcSpriteNode addSpriteNode)
	{
		if (this.m_SpriteList == null)
		{
			this.m_SpriteList = new List<NcSpriteFactory.NcSpriteNode>();
		}
		this.m_SpriteList.Add(addSpriteNode.GetClone());
		this.m_bNeedRebuild = true;
		return this.m_SpriteList.Count - 1;
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x000C31B4 File Offset: 0x000C13B4
	public void DeleteSpriteNode(int nIndex)
	{
		if (this.m_SpriteList == null || nIndex < 0 || this.m_SpriteList.Count <= nIndex)
		{
			return;
		}
		this.m_bNeedRebuild = true;
		this.m_SpriteList.Remove(this.m_SpriteList[nIndex]);
	}

	// Token: 0x060017C0 RID: 6080 RVA: 0x000C3204 File Offset: 0x000C1404
	public void MoveSpriteNode(int nSrcIndex, int nTarIndex)
	{
		NcSpriteFactory.NcSpriteNode ncSpriteNode = this.m_SpriteList[nSrcIndex];
		this.m_SpriteList.Remove(ncSpriteNode);
		this.m_SpriteList.Insert(nTarIndex, ncSpriteNode);
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x0000F84C File Offset: 0x0000DA4C
	public void ClearAllSpriteNode()
	{
		if (this.m_SpriteList == null)
		{
			return;
		}
		this.m_bNeedRebuild = true;
		this.m_SpriteList.Clear();
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x0000F86C File Offset: 0x0000DA6C
	public int GetSpriteNodeCount()
	{
		if (this.m_SpriteList == null)
		{
			return 0;
		}
		return this.m_SpriteList.Count;
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x0000F886 File Offset: 0x0000DA86
	public NcSpriteFactory.NcSpriteNode GetCurrentSpriteNode()
	{
		if (this.m_SpriteList == null || this.m_SpriteList.Count <= this.m_nCurrentIndex)
		{
			return null;
		}
		return this.m_SpriteList[this.m_nCurrentIndex];
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x000C3238 File Offset: 0x000C1438
	public Rect GetSpriteUvRect(int nStriteIndex, int nFrameIndex)
	{
		if (this.m_SpriteList.Count <= nStriteIndex || this.m_SpriteList[nStriteIndex].m_FrameInfos == null || this.m_SpriteList[nStriteIndex].m_FrameInfos.Length <= nFrameIndex)
		{
			return new Rect(0f, 0f, 0f, 0f);
		}
		return this.m_SpriteList[nStriteIndex].m_FrameInfos[nFrameIndex].m_TextureUvOffset;
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x0000F8BC File Offset: 0x0000DABC
	public bool IsValidFactory()
	{
		return !this.m_bNeedRebuild;
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x0000F8CC File Offset: 0x0000DACC
	private void Awake()
	{
		this.m_bbInstance = true;
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x0000F8D5 File Offset: 0x0000DAD5
	public NcEffectBehaviour SetSprite(int nNodeIndex)
	{
		return this.SetSprite(nNodeIndex, true);
	}

	// Token: 0x060017C8 RID: 6088 RVA: 0x000C32B8 File Offset: 0x000C14B8
	public NcEffectBehaviour SetSprite(string spriteName)
	{
		if (this.m_SpriteList == null)
		{
			return null;
		}
		int num = 0;
		foreach (NcSpriteFactory.NcSpriteNode ncSpriteNode in this.m_SpriteList)
		{
			if (ncSpriteNode.m_SpriteName == spriteName)
			{
				return this.SetSprite(num, true);
			}
			num++;
		}
		return null;
	}

	// Token: 0x060017C9 RID: 6089 RVA: 0x000C3340 File Offset: 0x000C1540
	public NcEffectBehaviour SetSprite(int nNodeIndex, bool bRunImmediate)
	{
		if (this.m_SpriteList == null || nNodeIndex < 0 || this.m_SpriteList.Count <= nNodeIndex)
		{
			return null;
		}
		if (bRunImmediate)
		{
			this.OnChangingSprite(this.m_nCurrentIndex, nNodeIndex);
		}
		this.m_nCurrentIndex = nNodeIndex;
		NcSpriteAnimation component = base.GetComponent<NcSpriteAnimation>();
		if (component != null)
		{
			component.SetSpriteFactoryIndex(nNodeIndex, false);
			if (bRunImmediate)
			{
				component.ResetAnimation();
			}
		}
		NcSpriteTexture component2 = base.GetComponent<NcSpriteTexture>();
		if (component2 != null)
		{
			component2.SetSpriteFactoryIndex(nNodeIndex, -1, false);
			if (bRunImmediate)
			{
				this.CreateEffectObject();
			}
		}
		if (component != null)
		{
			return component;
		}
		if (component != null)
		{
			return component2;
		}
		return null;
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x0000F8DF File Offset: 0x0000DADF
	public int GetCurrentSpriteIndex()
	{
		return this.m_nCurrentIndex;
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x000C33F8 File Offset: 0x000C15F8
	public bool IsEndSprite()
	{
		return this.m_SpriteList == null || this.m_nCurrentIndex < 0 || this.m_SpriteList.Count <= this.m_nCurrentIndex || (this.IsUnused(this.m_nCurrentIndex) || this.m_SpriteList[this.m_nCurrentIndex].IsEmptyTexture()) || this.m_bEndSprite;
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x000C3468 File Offset: 0x000C1668
	private void CreateEffectObject()
	{
		if (!this.m_bbInstance)
		{
			return;
		}
		if (!this.m_bShowEffect)
		{
			return;
		}
		this.DestroyEffectObject();
		if (base.transform.parent != null)
		{
			base.transform.parent.SendMessage("OnSpriteListEffectFrame", this.m_SpriteList[this.m_nCurrentIndex], 1);
		}
		if (this.m_SpriteList[this.m_nCurrentIndex].m_bEffectInstantiate)
		{
			this.m_CurrentEffect = this.CreateSpriteEffect(this.m_nCurrentIndex, base.transform);
			if (base.transform.parent != null)
			{
				base.transform.parent.SendMessage("OnSpriteListEffectInstance", this.m_CurrentEffect, 1);
			}
		}
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x000C3538 File Offset: 0x000C1738
	public GameObject CreateSpriteEffect(int nSrcSpriteIndex, Transform parentTrans)
	{
		GameObject gameObject = null;
		if (this.m_SpriteList[nSrcSpriteIndex].m_EffectPrefab != null)
		{
			gameObject = base.CreateGameObject("Effect_" + this.m_SpriteList[nSrcSpriteIndex].m_EffectPrefab.name);
			if (gameObject == null)
			{
				return null;
			}
			base.ChangeParent(parentTrans, gameObject.transform, true, null);
			NcAttachPrefab ncAttachPrefab = gameObject.AddComponent<NcAttachPrefab>();
			ncAttachPrefab.m_AttachPrefab = this.m_SpriteList[nSrcSpriteIndex].m_EffectPrefab;
			ncAttachPrefab.m_fPrefabSpeed = this.m_SpriteList[nSrcSpriteIndex].m_fEffectSpeed;
			ncAttachPrefab.m_bDetachParent = this.m_SpriteList[nSrcSpriteIndex].m_bEffectDetach;
			ncAttachPrefab.m_nSpriteFactoryIndex = this.m_SpriteList[nSrcSpriteIndex].m_nSpriteFactoryIndex;
			ncAttachPrefab.UpdateImmediately();
			gameObject.transform.localScale *= this.m_SpriteList[nSrcSpriteIndex].m_fEffectScale;
			gameObject.transform.localPosition += this.m_SpriteList[nSrcSpriteIndex].m_EffectPos;
			gameObject.transform.localRotation *= Quaternion.Euler(this.m_SpriteList[nSrcSpriteIndex].m_EffectRot);
		}
		return gameObject;
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x0000F8E7 File Offset: 0x0000DAE7
	private void DestroyEffectObject()
	{
		if (this.m_CurrentEffect != null)
		{
			Object.Destroy(this.m_CurrentEffect);
		}
		this.m_CurrentEffect = null;
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x000C368C File Offset: 0x000C188C
	private void CreateSoundObject(NcSpriteFactory.NcSpriteNode ncSpriteNode)
	{
		if (!this.m_bShowEffect)
		{
			return;
		}
		if (ncSpriteNode.m_AudioClip != null)
		{
			if (this.m_CurrentSound == null)
			{
				this.m_CurrentSound = base.gameObject.AddComponent<NcAttachSound>();
			}
			this.m_CurrentSound.m_AudioClip = ncSpriteNode.m_AudioClip;
			this.m_CurrentSound.m_bLoop = ncSpriteNode.m_bSoundLoop;
			this.m_CurrentSound.m_fVolume = ncSpriteNode.m_fSoundVolume;
			this.m_CurrentSound.m_fPitch = ncSpriteNode.m_fSoundPitch;
			this.m_CurrentSound.enabled = true;
			this.m_CurrentSound.Replay();
		}
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x0000F90C File Offset: 0x0000DB0C
	public void OnChangingSprite(int nOldNodeIndex, int nNewNodeIndex)
	{
		this.m_bEndSprite = false;
		this.DestroyEffectObject();
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x0000264F File Offset: 0x0000084F
	public void OnAnimationStartFrame(NcSpriteAnimation spriteCom)
	{
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x000C3734 File Offset: 0x000C1934
	public void OnAnimationChangingFrame(NcSpriteAnimation spriteCom, int nOldIndex, int nNewIndex, int nLoopCount)
	{
		if (this.m_SpriteList.Count <= this.m_nCurrentIndex)
		{
			return;
		}
		if (this.m_SpriteList[this.m_nCurrentIndex].m_EffectPrefab != null && (nOldIndex < this.m_SpriteList[this.m_nCurrentIndex].m_nEffectFrame || nNewIndex <= nOldIndex) && this.m_SpriteList[this.m_nCurrentIndex].m_nEffectFrame <= nNewIndex && (nLoopCount == 0 || !this.m_SpriteList[this.m_nCurrentIndex].m_bEffectOnlyFirst))
		{
			this.CreateEffectObject();
		}
		if (this.m_SpriteList[this.m_nCurrentIndex].m_AudioClip != null && (nOldIndex < this.m_SpriteList[this.m_nCurrentIndex].m_nSoundFrame || nNewIndex <= nOldIndex) && this.m_SpriteList[this.m_nCurrentIndex].m_nSoundFrame <= nNewIndex && (nLoopCount == 0 || !this.m_SpriteList[this.m_nCurrentIndex].m_bSoundOnlyFirst))
		{
			this.CreateSoundObject(this.m_SpriteList[this.m_nCurrentIndex]);
		}
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x000C387C File Offset: 0x000C1A7C
	public bool OnAnimationLastFrame(NcSpriteAnimation spriteCom, int nLoopCount)
	{
		if (this.m_SpriteList.Count <= this.m_nCurrentIndex)
		{
			return false;
		}
		this.m_bEndSprite = true;
		if (this.m_bSequenceMode)
		{
			if (this.m_nCurrentIndex < this.GetSpriteNodeCount() - 1)
			{
				if (((!this.m_SpriteList[this.m_nCurrentIndex].m_bLoop) ? 1 : 3) == nLoopCount)
				{
					this.SetSprite(this.m_nCurrentIndex + 1);
					return true;
				}
			}
			else
			{
				this.SetSprite(0);
			}
		}
		else
		{
			NcSpriteAnimation ncSpriteAnimation = this.SetSprite(this.m_SpriteList[this.m_nCurrentIndex].m_nNextSpriteIndex) as NcSpriteAnimation;
			if (ncSpriteAnimation != null)
			{
				ncSpriteAnimation.ResetAnimation();
				return true;
			}
		}
		return false;
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x0000264F File Offset: 0x0000084F
	public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
	{
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x000C3948 File Offset: 0x000C1B48
	public static void CreatePlane(MeshFilter meshFilter, float fUvScale, NcSpriteFactory.NcFrameInfo ncSpriteFrameInfo, bool bTrimCenterAlign, NcSpriteFactory.ALIGN_TYPE alignType, NcSpriteFactory.MESH_TYPE m_MeshType, float fHShowRate)
	{
		Vector2 vector;
		vector..ctor(fUvScale * ncSpriteFrameInfo.m_FrameScale.x, fUvScale * ncSpriteFrameInfo.m_FrameScale.y);
		float num = (alignType != NcSpriteFactory.ALIGN_TYPE.BOTTOM) ? ((alignType != NcSpriteFactory.ALIGN_TYPE.TOP) ? 0f : (-1f * vector.y)) : (1f * vector.y);
		float num2 = (alignType != NcSpriteFactory.ALIGN_TYPE.LEFTCENTER) ? ((alignType != NcSpriteFactory.ALIGN_TYPE.RIGHTCENTER) ? 0f : (-1f * vector.x)) : (1f * vector.x);
		Rect frameUvOffset = ncSpriteFrameInfo.m_FrameUvOffset;
		if (bTrimCenterAlign)
		{
			frameUvOffset.center = Vector2.zero;
		}
		Vector3[] array = new Vector3[4];
		if (alignType == NcSpriteFactory.ALIGN_TYPE.LEFTCENTER && 0f < fHShowRate)
		{
			array[0] = new Vector3(frameUvOffset.xMax * vector.x * fHShowRate + num2 * fHShowRate, frameUvOffset.yMax * vector.y + num);
			array[1] = new Vector3(frameUvOffset.xMax * vector.x * fHShowRate + num2 * fHShowRate, frameUvOffset.yMin * vector.y + num);
		}
		else
		{
			array[0] = new Vector3(frameUvOffset.xMax * vector.x + num2, frameUvOffset.yMax * vector.y + num);
			array[1] = new Vector3(frameUvOffset.xMax * vector.x + num2, frameUvOffset.yMin * vector.y + num);
		}
		array[2] = new Vector3(frameUvOffset.xMin * vector.x + num2, frameUvOffset.yMin * vector.y + num);
		array[3] = new Vector3(frameUvOffset.xMin * vector.x + num2, frameUvOffset.yMax * vector.y + num);
		Color color = Color.white;
		if (meshFilter.mesh.colors != null && 0 < meshFilter.mesh.colors.Length)
		{
			color = meshFilter.mesh.colors[0];
		}
		Color[] colors = new Color[]
		{
			color,
			color,
			color,
			color
		};
		Vector3[] normals = new Vector3[]
		{
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(0f, 0f, -1f)
		};
		Vector4[] tangents = new Vector4[]
		{
			new Vector4(1f, 0f, 0f, -1f),
			new Vector4(1f, 0f, 0f, -1f),
			new Vector4(1f, 0f, 0f, -1f),
			new Vector4(1f, 0f, 0f, -1f)
		};
		int[] triangles;
		if (m_MeshType == NcSpriteFactory.MESH_TYPE.BuiltIn_Plane)
		{
			triangles = new int[]
			{
				1,
				2,
				0,
				0,
				2,
				3
			};
		}
		else
		{
			triangles = new int[]
			{
				1,
				2,
				0,
				0,
				2,
				3,
				1,
				0,
				3,
				3,
				2,
				1
			};
		}
		Vector2[] array2 = new Vector2[4];
		float num3 = 1f;
		if (alignType == NcSpriteFactory.ALIGN_TYPE.LEFTCENTER && 0f < fHShowRate)
		{
			num3 = fHShowRate;
		}
		array2[0] = new Vector2(num3, 1f);
		array2[1] = new Vector2(num3, 0f);
		array2[2] = new Vector2(0f, 0f);
		array2[3] = new Vector2(0f, 1f);
		meshFilter.mesh.Clear();
		meshFilter.mesh.vertices = array;
		meshFilter.mesh.colors = colors;
		meshFilter.mesh.normals = normals;
		meshFilter.mesh.tangents = tangents;
		meshFilter.mesh.triangles = triangles;
		meshFilter.mesh.uv = array2;
		meshFilter.mesh.RecalculateBounds();
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x000C3E84 File Offset: 0x000C2084
	public static void CreateEmptyMesh(MeshFilter meshFilter)
	{
		int num = 3;
		Vector3[] array = new Vector3[num];
		Color[] array2 = new Color[num];
		Vector3[] array3 = new Vector3[num];
		Vector4[] array4 = new Vector4[num];
		int[] array5 = new int[num];
		Vector2[] array6 = new Vector2[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = Vector3.zero;
			array2[i] = Color.white;
			array3[i] = Vector3.zero;
			array4[i] = Vector4.zero;
			array5[i] = 0;
			array6[i] = Vector2.zero;
		}
		meshFilter.mesh.Clear();
		meshFilter.mesh.vertices = array;
		meshFilter.mesh.colors = array2;
		meshFilter.mesh.normals = array3;
		meshFilter.mesh.tangents = array4;
		meshFilter.mesh.triangles = array5;
		meshFilter.mesh.uv = array6;
		meshFilter.mesh.RecalculateBounds();
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x000C3F9C File Offset: 0x000C219C
	public static void UpdatePlane(MeshFilter meshFilter, float fUvScale, NcSpriteFactory.NcFrameInfo ncSpriteFrameInfo, bool bTrimCenterAlign, NcSpriteFactory.ALIGN_TYPE alignType, float fHShowRate)
	{
		Vector2 vector;
		vector..ctor(fUvScale * ncSpriteFrameInfo.m_FrameScale.x, fUvScale * ncSpriteFrameInfo.m_FrameScale.y);
		float num = (alignType != NcSpriteFactory.ALIGN_TYPE.BOTTOM) ? ((alignType != NcSpriteFactory.ALIGN_TYPE.TOP) ? 0f : (-1f * vector.y)) : (1f * vector.y);
		float num2 = (alignType != NcSpriteFactory.ALIGN_TYPE.LEFTCENTER) ? ((alignType != NcSpriteFactory.ALIGN_TYPE.RIGHTCENTER) ? 0f : (-1f * vector.x)) : (1f * vector.x);
		Rect frameUvOffset = ncSpriteFrameInfo.m_FrameUvOffset;
		if (bTrimCenterAlign)
		{
			frameUvOffset.center = Vector2.zero;
		}
		Vector3[] array = new Vector3[4];
		if (alignType == NcSpriteFactory.ALIGN_TYPE.LEFTCENTER && 0f < fHShowRate)
		{
			array[0] = new Vector3(frameUvOffset.xMax * vector.x * fHShowRate + num2 * fHShowRate, frameUvOffset.yMax * vector.y + num);
			array[1] = new Vector3(frameUvOffset.xMax * vector.x * fHShowRate + num2 * fHShowRate, frameUvOffset.yMin * vector.y + num);
		}
		else
		{
			array[0] = new Vector3(frameUvOffset.xMax * vector.x + num2, frameUvOffset.yMax * vector.y + num);
			array[1] = new Vector3(frameUvOffset.xMax * vector.x + num2, frameUvOffset.yMin * vector.y + num);
		}
		array[2] = new Vector3(frameUvOffset.xMin * vector.x, frameUvOffset.yMin * vector.y + num);
		array[3] = new Vector3(frameUvOffset.xMin * vector.x, frameUvOffset.yMax * vector.y + num);
		meshFilter.mesh.vertices = array;
		meshFilter.mesh.RecalculateBounds();
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x000C41C8 File Offset: 0x000C23C8
	public static void UpdateMeshUVs(MeshFilter meshFilter, Rect uv, NcSpriteFactory.ALIGN_TYPE alignType, float fHShowRate)
	{
		Vector2[] array = new Vector2[4];
		float num = 1f;
		if (alignType == NcSpriteFactory.ALIGN_TYPE.LEFTCENTER && 0f < fHShowRate)
		{
			num = fHShowRate;
		}
		array[0] = new Vector2(uv.x + uv.width * num, uv.y + uv.height);
		array[1] = new Vector2(uv.x + uv.width * num, uv.y);
		array[2] = new Vector2(uv.x, uv.y);
		array[3] = new Vector2(uv.x, uv.y + uv.height);
		meshFilter.mesh.uv = array;
	}

	// Token: 0x04001C58 RID: 7256
	public NcSpriteFactory.SPRITE_TYPE m_SpriteType = NcSpriteFactory.SPRITE_TYPE.Auto;

	// Token: 0x04001C59 RID: 7257
	public List<NcSpriteFactory.NcSpriteNode> m_SpriteList;

	// Token: 0x04001C5A RID: 7258
	public int m_nCurrentIndex;

	// Token: 0x04001C5B RID: 7259
	public int m_nMaxAtlasTextureSize = 2048;

	// Token: 0x04001C5C RID: 7260
	public bool m_bNeedRebuild = true;

	// Token: 0x04001C5D RID: 7261
	public int m_nBuildStartIndex;

	// Token: 0x04001C5E RID: 7262
	public float m_fSpriteResizeRate = 1f;

	// Token: 0x04001C5F RID: 7263
	public bool m_bTrimBlack = true;

	// Token: 0x04001C60 RID: 7264
	public bool m_bTrimAlpha = true;

	// Token: 0x04001C61 RID: 7265
	public float m_fUvScale = 1f;

	// Token: 0x04001C62 RID: 7266
	public float m_fTextureRatio = 1f;

	// Token: 0x04001C63 RID: 7267
	public GameObject m_CurrentEffect;

	// Token: 0x04001C64 RID: 7268
	public NcAttachSound m_CurrentSound;

	// Token: 0x04001C65 RID: 7269
	protected bool m_bEndSprite = true;

	// Token: 0x04001C66 RID: 7270
	public NcSpriteFactory.SHOW_TYPE m_ShowType = NcSpriteFactory.SHOW_TYPE.SPRITE;

	// Token: 0x04001C67 RID: 7271
	public bool m_bShowEffect = true;

	// Token: 0x04001C68 RID: 7272
	public bool m_bTestMode = true;

	// Token: 0x04001C69 RID: 7273
	public bool m_bSequenceMode;

	// Token: 0x04001C6A RID: 7274
	protected bool m_bbInstance;

	// Token: 0x020003E0 RID: 992
	[Serializable]
	public class NcFrameInfo
	{
		// Token: 0x04001C6B RID: 7275
		public int m_nFrameIndex;

		// Token: 0x04001C6C RID: 7276
		public bool m_bEmptyFrame;

		// Token: 0x04001C6D RID: 7277
		public int m_nTexWidth;

		// Token: 0x04001C6E RID: 7278
		public int m_nTexHeight;

		// Token: 0x04001C6F RID: 7279
		public Rect m_TextureUvOffset;

		// Token: 0x04001C70 RID: 7280
		public Rect m_FrameUvOffset;

		// Token: 0x04001C71 RID: 7281
		public Vector2 m_FrameScale;

		// Token: 0x04001C72 RID: 7282
		public Vector2 m_scaleFactor;
	}

	// Token: 0x020003E1 RID: 993
	[SerializeField]
	[Serializable]
	public class NcSpriteNode
	{
		// Token: 0x060017DB RID: 6107 RVA: 0x00008E8C File Offset: 0x0000708C
		public NcSpriteFactory.NcSpriteNode GetClone()
		{
			return null;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x0000F91B File Offset: 0x0000DB1B
		public int GetStartFrame()
		{
			if (this.m_FrameInfos == null || this.m_FrameInfos.Length == 0)
			{
				return 0;
			}
			return this.m_FrameInfos[0].m_nFrameIndex;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x0000F944 File Offset: 0x0000DB44
		public void SetEmpty()
		{
			this.m_FrameInfos = null;
			this.m_TextureGUID = string.Empty;
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0000F958 File Offset: 0x0000DB58
		public bool IsEmptyTexture()
		{
			return this.m_TextureGUID == string.Empty;
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0000F96A File Offset: 0x0000DB6A
		public bool IsUnused()
		{
			return !this.m_bIncludedAtlas;
		}

		// Token: 0x04001C73 RID: 7283
		public bool m_bIncludedAtlas = true;

		// Token: 0x04001C74 RID: 7284
		public string m_TextureGUID = string.Empty;

		// Token: 0x04001C75 RID: 7285
		public string m_TextureName = string.Empty;

		// Token: 0x04001C76 RID: 7286
		public float m_fMaxTextureAlpha = 1f;

		// Token: 0x04001C77 RID: 7287
		public string m_SpriteName = string.Empty;

		// Token: 0x04001C78 RID: 7288
		public int m_nSkipFrame;

		// Token: 0x04001C79 RID: 7289
		public NcSpriteFactory.NcFrameInfo[] m_FrameInfos;

		// Token: 0x04001C7A RID: 7290
		public int m_nTilingX = 1;

		// Token: 0x04001C7B RID: 7291
		public int m_nTilingY = 1;

		// Token: 0x04001C7C RID: 7292
		public int m_nStartFrame;

		// Token: 0x04001C7D RID: 7293
		public int m_nFrameCount = 1;

		// Token: 0x04001C7E RID: 7294
		public bool m_bLoop;

		// Token: 0x04001C7F RID: 7295
		public int m_nLoopStartFrame;

		// Token: 0x04001C80 RID: 7296
		public int m_nLoopFrameCount;

		// Token: 0x04001C81 RID: 7297
		public int m_nLoopingCount;

		// Token: 0x04001C82 RID: 7298
		public float m_fFps = 20f;

		// Token: 0x04001C83 RID: 7299
		public float m_fTime;

		// Token: 0x04001C84 RID: 7300
		public int m_nNextSpriteIndex = -1;

		// Token: 0x04001C85 RID: 7301
		public int m_nTestMode;

		// Token: 0x04001C86 RID: 7302
		public float m_fTestSpeed = 1f;

		// Token: 0x04001C87 RID: 7303
		public bool m_bEffectInstantiate = true;

		// Token: 0x04001C88 RID: 7304
		public GameObject m_EffectPrefab;

		// Token: 0x04001C89 RID: 7305
		public int m_nSpriteFactoryIndex = -1;

		// Token: 0x04001C8A RID: 7306
		public int m_nEffectFrame;

		// Token: 0x04001C8B RID: 7307
		public bool m_bEffectOnlyFirst = true;

		// Token: 0x04001C8C RID: 7308
		public bool m_bEffectDetach = true;

		// Token: 0x04001C8D RID: 7309
		public float m_fEffectSpeed = 1f;

		// Token: 0x04001C8E RID: 7310
		public float m_fEffectScale = 1f;

		// Token: 0x04001C8F RID: 7311
		public Vector3 m_EffectPos = Vector3.zero;

		// Token: 0x04001C90 RID: 7312
		public Vector3 m_EffectRot = Vector3.zero;

		// Token: 0x04001C91 RID: 7313
		public AudioClip m_AudioClip;

		// Token: 0x04001C92 RID: 7314
		public int m_nSoundFrame;

		// Token: 0x04001C93 RID: 7315
		public bool m_bSoundOnlyFirst = true;

		// Token: 0x04001C94 RID: 7316
		public bool m_bSoundLoop;

		// Token: 0x04001C95 RID: 7317
		public float m_fSoundVolume = 1f;

		// Token: 0x04001C96 RID: 7318
		public float m_fSoundPitch = 1f;
	}

	// Token: 0x020003E2 RID: 994
	[SerializeField]
	public enum MESH_TYPE
	{
		// Token: 0x04001C98 RID: 7320
		BuiltIn_Plane,
		// Token: 0x04001C99 RID: 7321
		BuiltIn_TwosidePlane
	}

	// Token: 0x020003E3 RID: 995
	public enum ALIGN_TYPE
	{
		// Token: 0x04001C9B RID: 7323
		TOP,
		// Token: 0x04001C9C RID: 7324
		CENTER,
		// Token: 0x04001C9D RID: 7325
		BOTTOM,
		// Token: 0x04001C9E RID: 7326
		LEFTCENTER,
		// Token: 0x04001C9F RID: 7327
		RIGHTCENTER
	}

	// Token: 0x020003E4 RID: 996
	public enum SPRITE_TYPE
	{
		// Token: 0x04001CA1 RID: 7329
		NcSpriteTexture,
		// Token: 0x04001CA2 RID: 7330
		NcSpriteAnimation,
		// Token: 0x04001CA3 RID: 7331
		Auto
	}

	// Token: 0x020003E5 RID: 997
	public enum SHOW_TYPE
	{
		// Token: 0x04001CA5 RID: 7333
		NONE,
		// Token: 0x04001CA6 RID: 7334
		ALL,
		// Token: 0x04001CA7 RID: 7335
		SPRITE,
		// Token: 0x04001CA8 RID: 7336
		ANIMATION,
		// Token: 0x04001CA9 RID: 7337
		EFFECT
	}
}
