using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02000636 RID: 1590
public static class RopePersistManager
{
	// Token: 0x06002737 RID: 10039 RVA: 0x0012F440 File Offset: 0x0012D640
	public static void StorePersistentData(UltimateRope rope)
	{
		RopePersistManager.RopeData ropeData = new RopePersistManager.RopeData(rope);
		foreach (FieldInfo fieldInfo in rope.GetType().GetFields())
		{
			if (Attribute.IsDefined(fieldInfo, typeof(RopePersistAttribute)))
			{
				ropeData.m_hashFieldName2Value.Add(fieldInfo.Name, fieldInfo.GetValue(rope));
			}
		}
		if (rope.Deleted)
		{
			ropeData.m_bDeleted = true;
		}
		else
		{
			ropeData.m_aaJointsBroken = new bool[rope.RopeNodes.Count][];
			ropeData.m_aaJointsProcessed = new bool[rope.RopeNodes.Count][];
			ropeData.m_transformInfoRope = RopePersistManager.ComputeTransformInfo(rope, rope.gameObject, (!(rope.transform.parent != null)) ? null : rope.transform.parent.gameObject);
			if (rope.RopeStart != null)
			{
				ropeData.m_transformInfoStart = RopePersistManager.ComputeTransformInfo(rope, rope.RopeStart, (!(rope.RopeStart.transform.parent != null)) ? null : rope.RopeStart.transform.parent.gameObject);
			}
			int num = 0;
			for (int j = 0; j < rope.RopeNodes.Count; j++)
			{
				if (rope.RopeNodes[j].goNode != null)
				{
					ropeData.m_transformInfoSegments[j] = RopePersistManager.ComputeTransformInfo(rope, rope.RopeNodes[j].goNode, (!(rope.RopeNodes[j].goNode.transform.parent != null)) ? null : rope.RopeNodes[j].goNode.transform.parent.gameObject);
				}
				foreach (GameObject node in rope.RopeNodes[j].segmentLinks)
				{
					ropeData.m_aLinkTransformInfo[num] = RopePersistManager.ComputeTransformInfo(rope, node, (rope.RopeType != UltimateRope.ERopeType.ImportBones) ? rope.RopeNodes[j].goNode.transform.gameObject : rope.ImportedBones[num].tfNonBoneParent.gameObject);
					num++;
				}
				ropeData.m_aaJointsBroken[j] = new bool[rope.RopeNodes[j].linkJoints.Length];
				ropeData.m_aaJointsProcessed[j] = new bool[rope.RopeNodes[j].linkJointBreaksProcessed.Length];
				for (int l = 0; l < rope.RopeNodes[j].linkJoints.Length; l++)
				{
					ropeData.m_aaJointsBroken[j][l] = (rope.RopeNodes[j].linkJoints[l] == null);
				}
				for (int m = 0; m < rope.RopeNodes[j].linkJoints.Length; m++)
				{
					ropeData.m_aaJointsProcessed[j][m] = rope.RopeNodes[j].linkJointBreaksProcessed[m];
				}
			}
			ropeData.m_bDeleted = false;
		}
		RopePersistManager.s_hashInstanceID2RopeData.Add(rope.GetInstanceID(), ropeData);
	}

	// Token: 0x06002738 RID: 10040 RVA: 0x0012F7B0 File Offset: 0x0012D9B0
	public static void RetrievePersistentData(UltimateRope rope)
	{
		RopePersistManager.RopeData ropeData = RopePersistManager.s_hashInstanceID2RopeData[rope.GetInstanceID()];
		foreach (FieldInfo fieldInfo in rope.GetType().GetFields())
		{
			fieldInfo.SetValue(rope, ropeData.m_hashFieldName2Value[fieldInfo.Name]);
		}
		if (ropeData.m_bDeleted)
		{
			rope.DeleteRope(false);
		}
		else
		{
			RopePersistManager.SetTransformInfo(ropeData.m_transformInfoRope, rope.gameObject);
			if (rope.RopeStart != null)
			{
				if (ropeData.m_transformInfoStart.goObject == null)
				{
					rope.RopeStart = new GameObject(ropeData.m_transformInfoStart.strObjectName);
				}
				RopePersistManager.SetTransformInfo(ropeData.m_transformInfoStart, rope.RopeStart);
			}
			if (rope.RopeType != UltimateRope.ERopeType.ImportBones)
			{
				rope.DeleteRopeLinks();
			}
			int num = 0;
			for (int j = 0; j < rope.RopeNodes.Count; j++)
			{
				if (rope.RopeType != UltimateRope.ERopeType.ImportBones)
				{
					for (int k = 0; k < rope.RopeNodes[j].linkJoints.Length; k++)
					{
						rope.RopeNodes[j].linkJointBreaksProcessed[k] = ropeData.m_aaJointsProcessed[j][k];
					}
					if (rope.RopeNodes[j].goNode != null)
					{
						if (ropeData.m_transformInfoSegments[j].goObject == null)
						{
							rope.RopeNodes[j].goNode = new GameObject(ropeData.m_transformInfoSegments[j].strObjectName);
						}
						RopePersistManager.SetTransformInfo(ropeData.m_transformInfoSegments[j], rope.RopeNodes[j].goNode);
					}
				}
				if (rope.RopeType != UltimateRope.ERopeType.ImportBones)
				{
					rope.RopeNodes[j].segmentLinks = new GameObject[rope.RopeNodes[j].nTotalLinks];
				}
				for (int l = 0; l < rope.RopeNodes[j].segmentLinks.Length; l++)
				{
					if (rope.RopeType != UltimateRope.ERopeType.ImportBones)
					{
						if (rope.RopeType == UltimateRope.ERopeType.Procedural)
						{
							rope.RopeNodes[j].segmentLinks[l] = new GameObject(ropeData.m_aLinkTransformInfo[num].strObjectName);
						}
						else if (rope.RopeType == UltimateRope.ERopeType.LinkedObjects)
						{
							rope.RopeNodes[j].segmentLinks[l] = (Object.Instantiate(rope.LinkObject) as GameObject);
							rope.RopeNodes[j].segmentLinks[l].name = ropeData.m_aLinkTransformInfo[num].strObjectName;
						}
						rope.RopeNodes[j].segmentLinks[l].AddComponent<UltimateRopeLink>();
						rope.RopeNodes[j].segmentLinks[l].transform.parent = ((!rope.FirstNodeIsCoil() || j != 0) ? rope.gameObject.transform : rope.CoilObject.transform);
						if (!rope.RopeNodes[j].bIsCoil)
						{
							rope.RopeNodes[j].segmentLinks[l].AddComponent<Rigidbody>();
							rope.RopeNodes[j].segmentLinks[l].rigidbody.isKinematic = (ropeData.m_aLinkTransformInfo[num].bExtensibleKinematic || ropeData.m_aLinkTransformInfo[num].bLinkMarkedKinematic);
						}
					}
					RopePersistManager.SetTransformInfo(ropeData.m_aLinkTransformInfo[num], rope.RopeNodes[j].segmentLinks[l]);
					if (rope.RopeType == UltimateRope.ERopeType.ImportBones)
					{
						rope.RopeNodes[j].segmentLinks[l].transform.parent = ((!rope.ImportedBones[num].bIsStatic) ? rope.transform : rope.ImportedBones[num].tfNonBoneParent);
					}
					if (ropeData.m_aLinkTransformInfo[num].bExtensibleKinematic)
					{
						UltimateRopeLink component = rope.RopeNodes[j].segmentLinks[l].GetComponent<UltimateRopeLink>();
						if (component != null)
						{
							component.ExtensibleKinematic = true;
						}
						rope.RopeNodes[j].segmentLinks[l].transform.parent = ((j <= rope.m_nFirstNonCoilNode) ? rope.RopeStart.transform : rope.RopeNodes[j - 1].goNode.transform);
						rope.RopeNodes[j].segmentLinks[l].transform.position = rope.RopeNodes[j].segmentLinks[l].transform.parent.position;
						Vector3 vector = rope.RopeNodes[j].segmentLinks[l].transform.parent.TransformDirection(rope.RopeNodes[j].m_v3LocalDirectionForward);
						Vector3 vector2 = rope.RopeNodes[j].segmentLinks[l].transform.parent.TransformDirection(rope.RopeNodes[j].m_v3LocalDirectionUp);
						rope.RopeNodes[j].segmentLinks[l].transform.rotation = Quaternion.LookRotation(vector, vector2);
					}
					num++;
				}
			}
			rope.SetupRopeLinks();
			SkinnedMeshRenderer skinnedMeshRenderer = rope.GetComponent<SkinnedMeshRenderer>();
			if (!ropeData.m_bSkin)
			{
				if (skinnedMeshRenderer != null)
				{
					Object.DestroyImmediate(skinnedMeshRenderer);
				}
			}
			else
			{
				if (skinnedMeshRenderer == null)
				{
					skinnedMeshRenderer = rope.gameObject.AddComponent<SkinnedMeshRenderer>();
				}
				int num2 = ropeData.m_av3SkinVertices.Length;
				int num3 = ropeData.m_anSkinTrianglesRope.Length;
				int num4 = ropeData.m_anSkinTrianglesSections.Length;
				Vector3[] array = new Vector3[num2];
				Vector2[] array2 = new Vector2[num2];
				Vector4[] array3 = (ropeData.m_av4SkinTangents == null) ? null : new Vector4[ropeData.m_av4SkinTangents.Length];
				BoneWeight[] array4 = new BoneWeight[num2];
				int[] array5 = new int[num3];
				int[] array6 = new int[num4];
				Matrix4x4[] array7 = new Matrix4x4[ropeData.m_amtxSkinBindPoses.Length];
				Mesh mesh = new Mesh();
				RopePersistManager.RopeData.MakeSkinDeepCopy(ropeData.m_av3SkinVertices, ropeData.m_av2SkinMapping, ropeData.m_av4SkinTangents, ropeData.m_aSkinBoneWeights, ropeData.m_anSkinTrianglesRope, ropeData.m_anSkinTrianglesSections, ropeData.m_amtxSkinBindPoses, array, array2, array3, array4, array5, array6, array7);
				Transform[] array8 = new Transform[rope.TotalLinks];
				num = 0;
				for (int m = 0; m < rope.RopeNodes.Count; m++)
				{
					for (int n = 0; n < rope.RopeNodes[m].segmentLinks.Length; n++)
					{
						array8[num++] = rope.RopeNodes[m].segmentLinks[n].transform;
					}
				}
				mesh.vertices = array;
				mesh.uv = array2;
				mesh.boneWeights = array4;
				mesh.bindposes = array7;
				mesh.subMeshCount = 2;
				mesh.SetTriangles(array5, 0);
				mesh.SetTriangles(array6, 1);
				mesh.RecalculateNormals();
				if (array3 != null && array3.Length == num2)
				{
					mesh.tangents = array3;
				}
				skinnedMeshRenderer.bones = array8;
				skinnedMeshRenderer.sharedMesh = mesh;
				skinnedMeshRenderer.materials = new Material[]
				{
					rope.RopeMaterial,
					rope.RopeSectionMaterial
				};
			}
		}
	}

	// Token: 0x06002739 RID: 10041 RVA: 0x00019F0B File Offset: 0x0001810B
	public static bool PersistentDataExists(UltimateRope rope)
	{
		return RopePersistManager.s_hashInstanceID2RopeData.ContainsKey(rope.GetInstanceID());
	}

	// Token: 0x0600273A RID: 10042 RVA: 0x00019F1D File Offset: 0x0001811D
	public static void RemovePersistentData(UltimateRope rope)
	{
		RopePersistManager.s_hashInstanceID2RopeData.Remove(rope.GetInstanceID());
	}

	// Token: 0x0600273B RID: 10043 RVA: 0x0012FF74 File Offset: 0x0012E174
	private static RopePersistManager.RopeData.TransformInfo ComputeTransformInfo(UltimateRope rope, GameObject node, GameObject parent)
	{
		RopePersistManager.RopeData.TransformInfo transformInfo = new RopePersistManager.RopeData.TransformInfo();
		transformInfo.goObject = node;
		transformInfo.strObjectName = node.name;
		transformInfo.tfParent = ((!(parent == null)) ? parent.transform : null);
		if (transformInfo.tfParent != null)
		{
			transformInfo.v3LocalPosition = transformInfo.tfParent.InverseTransformPoint(node.transform.position);
			transformInfo.qLocalOrientation = Quaternion.Inverse(transformInfo.tfParent.rotation) * node.transform.rotation;
		}
		else
		{
			transformInfo.v3LocalPosition = node.transform.position;
			transformInfo.qLocalOrientation = node.transform.rotation;
		}
		transformInfo.v3LocalScale = node.transform.localScale;
		UltimateRopeLink component = node.GetComponent<UltimateRopeLink>();
		if (component != null)
		{
			transformInfo.bExtensibleKinematic = component.ExtensibleKinematic;
			transformInfo.bLinkMarkedKinematic = (node.rigidbody != null && node.rigidbody.isKinematic);
		}
		else
		{
			transformInfo.bExtensibleKinematic = false;
			transformInfo.bLinkMarkedKinematic = false;
		}
		return transformInfo;
	}

	// Token: 0x0600273C RID: 10044 RVA: 0x001300A0 File Offset: 0x0012E2A0
	private static void SetTransformInfo(RopePersistManager.RopeData.TransformInfo transformInfo, GameObject node)
	{
		if (transformInfo.tfParent != null)
		{
			node.transform.position = transformInfo.tfParent.TransformPoint(transformInfo.v3LocalPosition);
			node.transform.rotation = transformInfo.tfParent.rotation * transformInfo.qLocalOrientation;
		}
		else
		{
			node.transform.position = transformInfo.v3LocalPosition;
			node.transform.rotation = transformInfo.qLocalOrientation;
		}
		node.transform.localScale = transformInfo.v3LocalScale;
	}

	// Token: 0x04003067 RID: 12391
	private static Dictionary<int, RopePersistManager.RopeData> s_hashInstanceID2RopeData = new Dictionary<int, RopePersistManager.RopeData>();

	// Token: 0x02000637 RID: 1591
	private class RopeData
	{
		// Token: 0x0600273D RID: 10045 RVA: 0x00130134 File Offset: 0x0012E334
		public RopeData(UltimateRope rope)
		{
			this.m_rope = rope;
			this.m_hashFieldName2Value = new Dictionary<string, object>();
			this.m_aLinkTransformInfo = new RopePersistManager.RopeData.TransformInfo[rope.TotalLinks];
			this.m_transformInfoSegments = new RopePersistManager.RopeData.TransformInfo[rope.RopeNodes.Count];
			this.m_bSkin = (rope.GetComponent<SkinnedMeshRenderer>() != null);
			if (this.m_bSkin)
			{
				SkinnedMeshRenderer component = rope.GetComponent<SkinnedMeshRenderer>();
				Mesh sharedMesh = component.sharedMesh;
				int vertexCount = component.sharedMesh.vertexCount;
				int num = component.sharedMesh.GetTriangles(0).Length;
				int num2 = component.sharedMesh.GetTriangles(1).Length;
				this.m_av3SkinVertices = new Vector3[vertexCount];
				this.m_av2SkinMapping = new Vector2[vertexCount];
				this.m_av4SkinTangents = ((sharedMesh.tangents == null) ? null : new Vector4[sharedMesh.tangents.Length]);
				this.m_aSkinBoneWeights = new BoneWeight[vertexCount];
				this.m_anSkinTrianglesRope = new int[num];
				this.m_anSkinTrianglesSections = new int[num2];
				this.m_amtxSkinBindPoses = new Matrix4x4[component.sharedMesh.bindposes.Length];
				RopePersistManager.RopeData.MakeSkinDeepCopy(sharedMesh.vertices, sharedMesh.uv, sharedMesh.tangents, sharedMesh.boneWeights, sharedMesh.GetTriangles(0), sharedMesh.GetTriangles(1), sharedMesh.bindposes, this.m_av3SkinVertices, this.m_av2SkinMapping, this.m_av4SkinTangents, this.m_aSkinBoneWeights, this.m_anSkinTrianglesRope, this.m_anSkinTrianglesSections, this.m_amtxSkinBindPoses);
			}
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x001302AC File Offset: 0x0012E4AC
		public static void MakeSkinDeepCopy(Vector3[] av3VerticesSource, Vector2[] av2MappingSource, Vector4[] av4TangentsSource, BoneWeight[] aBoneWeightsSource, int[] anTrianglesRopeSource, int[] anTrianglesSectionsSource, Matrix4x4[] aBindPosesSource, Vector3[] av3VerticesDestiny, Vector2[] av2MappingDestiny, Vector4[] av4TangentsDestiny, BoneWeight[] aBoneWeightsDestiny, int[] anTrianglesRopeDestiny, int[] anTrianglesSectionsDestiny, Matrix4x4[] aBindPosesDestiny)
		{
			int num = av3VerticesSource.Length;
			for (int i = 0; i < num; i++)
			{
				av3VerticesDestiny[i] = av3VerticesSource[i];
				av2MappingDestiny[i] = av2MappingSource[i];
				if (av4TangentsDestiny != null && av4TangentsSource != null && av4TangentsDestiny.Length == num && av4TangentsSource.Length == num)
				{
					av4TangentsDestiny[i] = av4TangentsSource[i];
				}
				aBoneWeightsDestiny[i] = aBoneWeightsSource[i];
			}
			for (int j = 0; j < anTrianglesRopeDestiny.Length; j++)
			{
				anTrianglesRopeDestiny[j] = anTrianglesRopeSource[j];
			}
			for (int k = 0; k < anTrianglesSectionsDestiny.Length; k++)
			{
				anTrianglesSectionsDestiny[k] = anTrianglesSectionsSource[k];
			}
			for (int l = 0; l < aBindPosesSource.Length; l++)
			{
				aBindPosesDestiny[l] = aBindPosesSource[l];
			}
		}

		// Token: 0x04003068 RID: 12392
		public UltimateRope m_rope;

		// Token: 0x04003069 RID: 12393
		public bool m_bDeleted;

		// Token: 0x0400306A RID: 12394
		public Dictionary<string, object> m_hashFieldName2Value;

		// Token: 0x0400306B RID: 12395
		public bool m_bSkin;

		// Token: 0x0400306C RID: 12396
		public Vector3[] m_av3SkinVertices;

		// Token: 0x0400306D RID: 12397
		public Vector2[] m_av2SkinMapping;

		// Token: 0x0400306E RID: 12398
		public Vector4[] m_av4SkinTangents;

		// Token: 0x0400306F RID: 12399
		public BoneWeight[] m_aSkinBoneWeights;

		// Token: 0x04003070 RID: 12400
		public int[] m_anSkinTrianglesRope;

		// Token: 0x04003071 RID: 12401
		public int[] m_anSkinTrianglesSections;

		// Token: 0x04003072 RID: 12402
		public Matrix4x4[] m_amtxSkinBindPoses;

		// Token: 0x04003073 RID: 12403
		public RopePersistManager.RopeData.TransformInfo m_transformInfoRope;

		// Token: 0x04003074 RID: 12404
		public RopePersistManager.RopeData.TransformInfo[] m_aLinkTransformInfo;

		// Token: 0x04003075 RID: 12405
		public RopePersistManager.RopeData.TransformInfo m_transformInfoStart;

		// Token: 0x04003076 RID: 12406
		public RopePersistManager.RopeData.TransformInfo[] m_transformInfoSegments;

		// Token: 0x04003077 RID: 12407
		public bool[][] m_aaJointsProcessed;

		// Token: 0x04003078 RID: 12408
		public bool[][] m_aaJointsBroken;

		// Token: 0x02000638 RID: 1592
		public class TransformInfo
		{
			// Token: 0x04003079 RID: 12409
			public GameObject goObject;

			// Token: 0x0400307A RID: 12410
			public string strObjectName;

			// Token: 0x0400307B RID: 12411
			public Transform tfParent;

			// Token: 0x0400307C RID: 12412
			public Vector3 v3LocalPosition;

			// Token: 0x0400307D RID: 12413
			public Quaternion qLocalOrientation;

			// Token: 0x0400307E RID: 12414
			public Vector3 v3LocalScale;

			// Token: 0x0400307F RID: 12415
			public bool bLinkMarkedKinematic;

			// Token: 0x04003080 RID: 12416
			public bool bExtensibleKinematic;
		}
	}
}
