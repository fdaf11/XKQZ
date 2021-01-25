﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000639 RID: 1593
[ExecuteInEditMode]
public class UltimateRope : MonoBehaviour
{
	// Token: 0x170003AD RID: 941
	// (get) Token: 0x06002741 RID: 10049 RVA: 0x00019F30 File Offset: 0x00018130
	// (set) Token: 0x06002742 RID: 10050 RVA: 0x00019F38 File Offset: 0x00018138
	[HideInInspector]
	public string Status
	{
		get
		{
			return this.m_strStatus;
		}
		set
		{
			this.m_strStatus = value;
		}
	}

	// Token: 0x06002743 RID: 10051 RVA: 0x00019F41 File Offset: 0x00018141
	private void Awake()
	{
		if (Application.isPlaying)
		{
			this.CreateRopeJoints(true);
			this.SetupRopeLinks();
			if (this.FirstNodeIsCoil())
			{
				this.RecomputeCoil();
			}
		}
		else
		{
			this.CheckLoadPersistentData();
		}
	}

	// Token: 0x06002744 RID: 10052 RVA: 0x00019F76 File Offset: 0x00018176
	private void OnApplicationQuit()
	{
		this.CheckSavePersistentData();
	}

	// Token: 0x06002745 RID: 10053 RVA: 0x00019F7E File Offset: 0x0001817E
	private void Start()
	{
		this.m_fCurrentExtensionInput = this.m_fCurrentExtension;
	}

	// Token: 0x06002746 RID: 10054 RVA: 0x0000264F File Offset: 0x0000084F
	private void OnGUI()
	{
	}

	// Token: 0x06002747 RID: 10055 RVA: 0x0000264F File Offset: 0x0000084F
	private void Update()
	{
	}

	// Token: 0x06002748 RID: 10056 RVA: 0x001304D8 File Offset: 0x0012E6D8
	private void FixedUpdate()
	{
		if (this.RopeNodes == null)
		{
			return;
		}
		if (this.RopeNodes.Count == 0)
		{
			return;
		}
		if (this.RopeType == UltimateRope.ERopeType.Procedural && (this.LinkJointBreakForce != float.PositiveInfinity || this.LinkJointBreakTorque != float.PositiveInfinity))
		{
			SkinnedMeshRenderer component = base.gameObject.GetComponent<SkinnedMeshRenderer>();
			if (component == null)
			{
				return;
			}
			Mesh sharedMesh = component.sharedMesh;
			int[] triangles = sharedMesh.GetTriangles(0);
			int[] triangles2 = sharedMesh.GetTriangles(1);
			int num = 0;
			bool flag = false;
			for (int i = 0; i < this.RopeNodes.Count; i++)
			{
				UltimateRope.RopeNode ropeNode = this.RopeNodes[i];
				if (ropeNode.bIsCoil)
				{
					num += ropeNode.segmentLinks.Length;
				}
				else
				{
					for (int j = 0; j < ropeNode.linkJoints.Length; j++)
					{
						if (ropeNode.linkJoints[j] == null && !ropeNode.linkJointBreaksProcessed[j])
						{
							ropeNode.linkJointBreaksProcessed[j] = true;
							bool flag2 = i == 0 && j == 0 && !this.FirstNodeIsCoil();
							bool flag3 = i == this.RopeNodes.Count - 1 && j == ropeNode.linkJoints.Length - 1;
							if (!flag2 && !flag3)
							{
								this.FillLinkMeshIndicesRope(num - 1, this.TotalLinks, ref triangles, true, true);
								this.FillLinkMeshIndicesSections(num - 1, this.TotalLinks, ref triangles2, true, true);
								flag = true;
							}
							if (this.SendEvents && this.EventsObjectReceiver != null && this.OnBreakMethodName.Length > 0)
							{
								UltimateRope.RopeBreakEventInfo ropeBreakEventInfo = new UltimateRope.RopeBreakEventInfo();
								ropeBreakEventInfo.rope = this;
								ropeBreakEventInfo.worldPos = ((j != ropeNode.linkJoints.Length - 1) ? ropeNode.segmentLinks[j].transform.position : ropeNode.goNode.transform.position);
								ropeBreakEventInfo.link2 = ((j != ropeNode.linkJoints.Length - 1) ? ropeNode.segmentLinks[j] : ropeNode.goNode);
								ropeBreakEventInfo.localLink2Pos = Vector3.zero;
								if (flag2)
								{
									ropeBreakEventInfo.link1 = this.RopeStart.gameObject;
									ropeBreakEventInfo.localLink1Pos = Vector3.zero;
								}
								else
								{
									if (j > 0)
									{
										ropeBreakEventInfo.link1 = ropeNode.segmentLinks[j - 1];
									}
									else
									{
										ropeBreakEventInfo.link1 = this.RopeNodes[i - 1].goNode;
									}
									ropeBreakEventInfo.localLink1Pos = this.GetLinkAxisOffset(this.LinkLengths[num - 1]);
								}
								this.EventsObjectReceiver.SendMessage(this.OnBreakMethodName, ropeBreakEventInfo);
							}
						}
						if (j < ropeNode.segmentLinks.Length)
						{
							num++;
						}
					}
				}
			}
			if (flag)
			{
				sharedMesh.SetTriangles(triangles, 0);
				sharedMesh.SetTriangles(triangles2, 1);
				Vector4[] array = null;
				if (sharedMesh.tangents != null && sharedMesh.tangents.Length == sharedMesh.vertexCount)
				{
					array = sharedMesh.tangents;
				}
				sharedMesh.RecalculateNormals();
				if (array != null)
				{
					sharedMesh.tangents = array;
				}
			}
		}
		else if (this.RopeType == UltimateRope.ERopeType.LinkedObjects && (this.LinkJointBreakForce != float.PositiveInfinity || this.LinkJointBreakTorque != float.PositiveInfinity) && this.SendEvents)
		{
			int num2 = 0;
			for (int k = 0; k < this.RopeNodes.Count; k++)
			{
				UltimateRope.RopeNode ropeNode2 = this.RopeNodes[k];
				if (ropeNode2.bIsCoil)
				{
					num2 += ropeNode2.segmentLinks.Length;
				}
				else
				{
					for (int l = 0; l < ropeNode2.linkJoints.Length; l++)
					{
						if (ropeNode2.linkJoints[l] == null && !ropeNode2.linkJointBreaksProcessed[l])
						{
							ropeNode2.linkJointBreaksProcessed[l] = true;
							bool flag4 = k == 0 && l == 0 && !this.FirstNodeIsCoil();
							if (this.SendEvents && this.EventsObjectReceiver != null && this.OnBreakMethodName.Length > 0)
							{
								UltimateRope.RopeBreakEventInfo ropeBreakEventInfo2 = new UltimateRope.RopeBreakEventInfo();
								ropeBreakEventInfo2.rope = this;
								ropeBreakEventInfo2.worldPos = ((l != ropeNode2.linkJoints.Length - 1) ? ropeNode2.segmentLinks[l].transform.position : ropeNode2.goNode.transform.position);
								ropeBreakEventInfo2.link2 = ((l != ropeNode2.linkJoints.Length - 1) ? ropeNode2.segmentLinks[l] : ropeNode2.goNode);
								ropeBreakEventInfo2.localLink2Pos = Vector3.zero;
								if (flag4)
								{
									ropeBreakEventInfo2.link1 = this.RopeStart.gameObject;
									ropeBreakEventInfo2.localLink1Pos = Vector3.zero;
								}
								else
								{
									if (l > 0)
									{
										ropeBreakEventInfo2.link1 = ropeNode2.segmentLinks[l - 1];
									}
									else
									{
										ropeBreakEventInfo2.link1 = this.RopeNodes[k - 1].goNode;
									}
									ropeBreakEventInfo2.localLink1Pos = this.GetLinkAxisOffset(this.LinkLengths[num2 - 1]);
								}
								this.EventsObjectReceiver.SendMessage(this.OnBreakMethodName, ropeBreakEventInfo2);
							}
						}
						if (l < ropeNode2.segmentLinks.Length)
						{
							num2++;
						}
					}
				}
			}
		}
		if (this.LinkJointBreakForce != float.PositiveInfinity || this.LinkJointBreakTorque != float.PositiveInfinity)
		{
			for (int m = 0; m < this.RopeNodes.Count; m++)
			{
				UltimateRope.RopeNode ropeNode3 = this.RopeNodes[m];
				if (!ropeNode3.bIsCoil)
				{
					bool flag5 = false;
					if (!ropeNode3.bSegmentBroken)
					{
						for (int n = 0; n < ropeNode3.linkJoints.Length; n++)
						{
							if (ropeNode3.linkJoints[n] == null)
							{
								flag5 = true;
								ropeNode3.bSegmentBroken = true;
								break;
							}
						}
					}
					if (flag5)
					{
						for (int num3 = 0; num3 < ropeNode3.linkJoints.Length; num3++)
						{
							if (ropeNode3.linkJoints[num3] != null)
							{
								ropeNode3.linkJoints[num3].breakForce = float.PositiveInfinity;
								ropeNode3.linkJoints[num3].breakTorque = float.PositiveInfinity;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06002749 RID: 10057 RVA: 0x00130B9C File Offset: 0x0012ED9C
	public void DeleteRope(bool bResetNodePositions = false)
	{
		this.DeleteRopeLinks();
		foreach (UltimateRope.RopeNode ropeNode in this.RopeNodes)
		{
			ropeNode.bSegmentBroken = false;
			if (ropeNode.bInitialOrientationInitialized && bResetNodePositions)
			{
				ropeNode.goNode.transform.localPosition = ropeNode.v3InitialLocalPos;
				ropeNode.goNode.transform.localRotation = ropeNode.qInitialLocalRot;
				ropeNode.goNode.transform.localScale = ropeNode.v3InitialLocalScale;
			}
			ropeNode.bInitialOrientationInitialized = false;
			for (int i = 0; i < ropeNode.linkJoints.Length; i++)
			{
				if (ropeNode.linkJoints[i] != null)
				{
					Object.DestroyImmediate(ropeNode.linkJoints[i]);
				}
			}
		}
		if (this.RopeStart != null && this.m_bRopeStartInitialOrientationInitialized && bResetNodePositions)
		{
			this.RopeStart.transform.localPosition = this.m_v3InitialRopeStartLocalPos;
			this.RopeStart.transform.localRotation = this.m_qInitialRopeStartLocalRot;
			this.RopeStart.transform.localScale = this.m_v3InitialRopeStartLocalScale;
		}
		this.m_bRopeStartInitialOrientationInitialized = false;
		if (this.ImportedBones != null)
		{
			foreach (UltimateRope.RopeBone ropeBone in this.ImportedBones)
			{
				if (ropeBone.goBone != null)
				{
					ropeBone.goBone.layer = ropeBone.nOriginalLayer;
					if (ropeBone.bCreatedCollider && ropeBone.goBone.collider != null)
					{
						Object.DestroyImmediate(ropeBone.goBone.collider);
					}
					if (ropeBone.bCreatedRigidbody && ropeBone.goBone.rigidbody != null)
					{
						Object.DestroyImmediate(ropeBone.goBone.rigidbody);
					}
				}
			}
			foreach (UltimateRope.RopeBone ropeBone2 in this.ImportedBones)
			{
				if (ropeBone2.goBone != null)
				{
					if (ropeBone2.tfNonBoneParent != null)
					{
						ropeBone2.goBone.transform.parent = ropeBone2.tfNonBoneParent;
						ropeBone2.goBone.transform.localPosition = ropeBone2.v3OriginalLocalPos;
						ropeBone2.goBone.transform.localRotation = ropeBone2.qOriginalLocalRot;
					}
					ropeBone2.goBone.transform.parent = ropeBone2.tfParent;
					ropeBone2.goBone.transform.localScale = ropeBone2.v3OriginalLocalScale;
				}
			}
		}
		if (!Application.isEditor || !Application.isPlaying)
		{
			this.ImportedBones = null;
		}
		SkinnedMeshRenderer component = base.GetComponent<SkinnedMeshRenderer>();
		if (component)
		{
			Object.DestroyImmediate(component.sharedMesh);
			Object.DestroyImmediate(component);
		}
		this.CheckDelCoilNode();
		this.Deleted = true;
	}

	// Token: 0x0600274A RID: 10058 RVA: 0x00130EC8 File Offset: 0x0012F0C8
	public void DeleteRopeLinks()
	{
		if (!this.m_bBonesAreImported)
		{
			if (this.CoilObject != null)
			{
				for (int i = 0; i < this.CoilObject.transform.GetChildCount(); i++)
				{
					Transform child = this.CoilObject.transform.GetChild(i);
					if (child.gameObject.GetComponent<UltimateRopeLink>() != null)
					{
						Object.DestroyImmediate(child.gameObject);
						i--;
					}
				}
			}
			if (this.RopeStart != null)
			{
				for (int j = 0; j < this.RopeStart.transform.GetChildCount(); j++)
				{
					Transform child2 = this.RopeStart.transform.GetChild(j);
					if (child2.gameObject.GetComponent<UltimateRopeLink>() != null)
					{
						Object.DestroyImmediate(child2.gameObject);
						j--;
					}
				}
			}
			for (int k = 0; k < base.transform.GetChildCount(); k++)
			{
				Transform child3 = base.transform.GetChild(k);
				if (child3.gameObject.GetComponent<UltimateRopeLink>() != null)
				{
					Object.DestroyImmediate(child3.gameObject);
					k--;
				}
			}
			foreach (UltimateRope.RopeNode ropeNode in this.RopeNodes)
			{
				if (ropeNode.goNode)
				{
					for (int l = 0; l < ropeNode.goNode.transform.GetChildCount(); l++)
					{
						Transform child4 = ropeNode.goNode.transform.GetChild(l);
						if (child4.gameObject.GetComponent<UltimateRopeLink>() != null)
						{
							Object.DestroyImmediate(child4.gameObject);
							l--;
						}
					}
				}
				ropeNode.segmentLinks = null;
			}
		}
	}

	// Token: 0x0600274B RID: 10059 RVA: 0x001310D0 File Offset: 0x0012F2D0
	public bool Regenerate(bool bResetNodePositions = false)
	{
		this.m_bLastStatusIsError = true;
		this.DeleteRope(bResetNodePositions);
		if (this.RopeType == UltimateRope.ERopeType.Procedural || this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			if (this.RopeStart == null)
			{
				this.Status = "A rope start GameObject needs to be specified";
				return false;
			}
			if (this.RopeNodes == null)
			{
				this.Status = "At least a rope node needs to be added";
				return false;
			}
			if (this.RopeNodes.Count == 0)
			{
				this.Status = "At least a rope node needs to be added";
				return false;
			}
			if (this.RopeType == UltimateRope.ERopeType.Procedural && this.IsExtensible && this.HasACoil && this.CoilObject == null)
			{
				this.Status = "A coil object needs to be specified";
				return false;
			}
			if (this.RopeType == UltimateRope.ERopeType.LinkedObjects && this.LinkObject == null)
			{
				this.Status = "A link object needs to be specified";
				return false;
			}
			for (int i = 0; i < this.RopeNodes.Count; i++)
			{
				if (this.RopeNodes[i].goNode == null)
				{
					this.Status = string.Format("Rope segment {0} has unassigned Segment End property", i);
					return false;
				}
			}
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		List<UltimateRope.RopeBone> list = null;
		if (this.RopeType == UltimateRope.ERopeType.ImportBones)
		{
			this.Status = string.Empty;
			if (this.BoneFirst == null)
			{
				this.Status = "The first bone needs to be specified";
				return false;
			}
			if (this.BoneLast == null)
			{
				this.Status = "The last bone needs to be specified";
				return false;
			}
			List<int> listImportBonesStatic;
			string text;
			if (!this.ParseBoneIndices(this.BoneListNamesStatic, out listImportBonesStatic, out text))
			{
				this.Status = "Error parsing static bone list:\n" + text;
				return false;
			}
			List<int> listImportBonesNoCollider;
			if (!this.ParseBoneIndices(this.BoneListNamesNoColliders, out listImportBonesNoCollider, out text))
			{
				this.Status = "Error parsing collider bone list:\n" + text;
				return false;
			}
			if (!this.BuildImportedBoneList(this.BoneFirst, this.BoneLast, listImportBonesStatic, listImportBonesNoCollider, out list, out text))
			{
				this.Status = "Error building bone list:\n" + text;
				return false;
			}
		}
		base.gameObject.layer = this.RopeLayer;
		this.CheckAddCoilNode();
		if (!this.m_bRopeStartInitialOrientationInitialized && this.RopeStart != null)
		{
			this.m_v3InitialRopeStartLocalPos = this.RopeStart.transform.localPosition;
			this.m_qInitialRopeStartLocalRot = this.RopeStart.transform.localRotation;
			this.m_v3InitialRopeStartLocalScale = this.RopeStart.transform.localScale;
			this.m_bRopeStartInitialOrientationInitialized = true;
		}
		if (this.RopeType == UltimateRope.ERopeType.Procedural || this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			this.TotalLinks = 0;
			this.TotalRopeLength = 0f;
			for (int j = 0; j < this.RopeNodes.Count; j++)
			{
				UltimateRope.RopeNode ropeNode = this.RopeNodes[j];
				if (!ropeNode.bInitialOrientationInitialized)
				{
					ropeNode.v3InitialLocalPos = ropeNode.goNode.transform.localPosition;
					ropeNode.qInitialLocalRot = ropeNode.goNode.transform.localRotation;
					ropeNode.v3InitialLocalScale = ropeNode.goNode.transform.localScale;
					ropeNode.bInitialOrientationInitialized = true;
				}
				if (ropeNode.nNumLinks < 1)
				{
					ropeNode.nNumLinks = 1;
				}
				if (ropeNode.fLength < 0f)
				{
					ropeNode.fLength = 0.001f;
				}
				ropeNode.nTotalLinks = ropeNode.nNumLinks;
				ropeNode.fTotalLength = ropeNode.fLength;
				GameObject gameObject;
				GameObject gameObject2;
				if (this.FirstNodeIsCoil() && j == 0)
				{
					gameObject = this.CoilObject;
					gameObject2 = this.RopeStart;
				}
				else
				{
					gameObject = ((j != this.m_nFirstNonCoilNode) ? this.RopeNodes[j - 1].goNode : this.RopeStart);
					gameObject2 = this.RopeNodes[j].goNode;
				}
				ropeNode.m_v3LocalDirectionForward = gameObject.transform.InverseTransformDirection((gameObject2.transform.position - gameObject.transform.position).normalized);
				if (j == this.RopeNodes.Count - 1 && this.IsExtensible && this.ExtensibleLength > 0f)
				{
					ropeNode.nTotalLinks += (int)(this.ExtensibleLength / (ropeNode.fLength / (float)ropeNode.nNumLinks)) + 1;
					ropeNode.fTotalLength += this.ExtensibleLength;
					ropeNode.m_bExtensionInitialized = false;
					ropeNode.m_nExtensionLinkIn = ropeNode.nTotalLinks - ropeNode.nNumLinks;
					ropeNode.m_nExtensionLinkOut = ropeNode.m_nExtensionLinkIn - 1;
					ropeNode.m_fExtensionRemainingLength = this.ExtensibleLength;
					ropeNode.m_fExtensionRemainderIn = 0f;
					ropeNode.m_fExtensionRemainderOut = 0f;
					this.m_fCurrentExtension = 0f;
				}
				ropeNode.linkJoints = new ConfigurableJoint[ropeNode.nTotalLinks + 1];
				ropeNode.linkJointBreaksProcessed = new bool[ropeNode.nTotalLinks + 1];
				ropeNode.segmentLinks = new GameObject[ropeNode.nTotalLinks];
				if (this.FirstNodeIsCoil() && j == 0)
				{
					for (int k = 0; k < ropeNode.segmentLinks.Length; k++)
					{
						string text2 = "Coil Link " + k;
						ropeNode.segmentLinks[k] = new GameObject(text2);
						ropeNode.segmentLinks[k].AddComponent<UltimateRopeLink>();
						ropeNode.segmentLinks[k].transform.parent = this.CoilObject.transform;
						ropeNode.segmentLinks[k].layer = this.RopeLayer;
					}
					if (this.CoilDiameter < 0f)
					{
						this.CoilDiameter = 0f;
					}
					if (this.CoilWidth < 0f)
					{
						this.CoilWidth = 0f;
					}
					this.SetupCoilBones(this.ExtensibleLength);
				}
				else
				{
					float num = ropeNode.fLength / (float)ropeNode.nNumLinks;
					float num2 = ((gameObject2.transform.position - gameObject.transform.position).magnitude - num) / (gameObject2.transform.position - gameObject.transform.position).magnitude;
					float num3 = (this.RopeType != UltimateRope.ERopeType.LinkedObjects) ? 1f : this.GetLinkedObjectScale(ropeNode.fLength, ropeNode.nNumLinks);
					for (int l = 0; l < ropeNode.segmentLinks.Length; l++)
					{
						float num4 = (float)l / ((ropeNode.segmentLinks.Length != 1) ? ((float)ropeNode.segmentLinks.Length - 1f) : 1f);
						string text3 = string.Concat(new object[]
						{
							"Node ",
							j,
							" Link ",
							l
						});
						if (ropeNode.nTotalLinks > ropeNode.nNumLinks && l < ropeNode.nTotalLinks - ropeNode.nNumLinks)
						{
							text3 += " (extension)";
						}
						if (this.RopeType == UltimateRope.ERopeType.Procedural)
						{
							ropeNode.segmentLinks[l] = new GameObject(text3);
						}
						else if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
						{
							ropeNode.segmentLinks[l] = (Object.Instantiate(this.LinkObject) as GameObject);
							ropeNode.segmentLinks[l].name = text3;
						}
						ropeNode.segmentLinks[l].AddComponent<UltimateRopeLink>();
						if (Vector3.Distance(gameObject.transform.position, gameObject2.transform.position) < 0.001f)
						{
							ropeNode.segmentLinks[l].transform.position = gameObject.transform.position;
							ropeNode.segmentLinks[l].transform.rotation = gameObject.transform.rotation;
						}
						else
						{
							ropeNode.segmentLinks[l].transform.position = Vector3.Lerp(gameObject.transform.position, gameObject2.transform.position, num4 * num2);
							ropeNode.segmentLinks[l].transform.rotation = Quaternion.LookRotation((gameObject2.transform.position - gameObject.transform.position).normalized);
						}
						if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
						{
							ropeNode.segmentLinks[l].transform.rotation *= this.GetLinkedObjectLocalRotation(this.LinkTwistAngleStart + this.LinkTwistAngleIncrement * (float)l);
							ropeNode.segmentLinks[l].transform.localScale = new Vector3(num3, num3, num3);
						}
						if (ropeNode.segmentLinks[l].rigidbody == null)
						{
							ropeNode.segmentLinks[l].AddComponent<Rigidbody>();
						}
						ropeNode.segmentLinks[l].transform.parent = base.transform;
						ropeNode.segmentLinks[l].layer = this.RopeLayer;
					}
				}
				this.TotalLinks += ropeNode.segmentLinks.Length;
				this.TotalRopeLength += ropeNode.fTotalLength;
			}
			this.m_bBonesAreImported = false;
		}
		else if (this.RopeType == UltimateRope.ERopeType.ImportBones)
		{
			this.TotalLinks = 0;
			this.TotalRopeLength = 0f;
			this.ImportedBones = list.ToArray();
			bool flag = false;
			if (this.RopeNodes != null && this.RopeNodes.Count != 0)
			{
				flag = true;
			}
			if (!flag)
			{
				this.RopeNodes = new List<UltimateRope.RopeNode>();
				this.RopeNodes.Add(new UltimateRope.RopeNode());
			}
			UltimateRope.RopeNode ropeNode2 = this.RopeNodes[0];
			ropeNode2.nNumLinks = this.ImportedBones.Length;
			ropeNode2.nTotalLinks = ropeNode2.nNumLinks;
			ropeNode2.linkJoints = new ConfigurableJoint[this.ImportedBones.Length];
			ropeNode2.linkJointBreaksProcessed = new bool[this.ImportedBones.Length];
			ropeNode2.segmentLinks = new GameObject[ropeNode2.nTotalLinks];
			int num5 = 0;
			for (int m = 0; m < this.ImportedBones.Length; m++)
			{
				ropeNode2.segmentLinks[num5] = this.ImportedBones[m].goBone;
				if (this.ImportedBones[m].goBone.rigidbody == null)
				{
					this.ImportedBones[m].goBone.AddComponent<Rigidbody>();
					this.ImportedBones[m].bCreatedRigidbody = true;
				}
				else
				{
					this.ImportedBones[m].bCreatedRigidbody = false;
				}
				this.ImportedBones[m].goBone.layer = this.RopeLayer;
				float num6;
				if (num5 < this.ImportedBones.Length - 1)
				{
					num6 = Vector3.Distance(this.ImportedBones[m].goBone.transform.position, this.ImportedBones[m + 1].goBone.transform.position);
				}
				else
				{
					num6 = 0f;
				}
				this.TotalLinks += ropeNode2.segmentLinks.Length;
				this.TotalRopeLength += num6;
				this.ImportedBones[m].fLength = num6;
				num5++;
			}
			ropeNode2.fLength = this.TotalRopeLength;
			ropeNode2.fTotalLength = ropeNode2.fLength;
			ropeNode2.eColliderType = this.BoneColliderType;
			ropeNode2.nColliderSkip = this.BoneColliderSkip;
			this.m_bBonesAreImported = true;
		}
		if (this.RopeType == UltimateRope.ERopeType.Procedural)
		{
			Transform[] array = new Transform[this.TotalLinks];
			Matrix4x4[] array2 = new Matrix4x4[this.TotalLinks];
			this.LinkLengths = new float[this.TotalLinks];
			int num7 = 0;
			for (int n = 0; n < this.RopeNodes.Count; n++)
			{
				UltimateRope.RopeNode ropeNode3 = this.RopeNodes[n];
				for (int num8 = 0; num8 < ropeNode3.segmentLinks.Length; num8++)
				{
					array[num7] = ropeNode3.segmentLinks[num8].transform;
					array2[num7] = ropeNode3.segmentLinks[num8].transform.worldToLocalMatrix;
					if (ropeNode3.segmentLinks[num8].transform.parent != null)
					{
						array2[num7] *= base.transform.localToWorldMatrix;
					}
					this.LinkLengths[num7] = ropeNode3.fLength / (float)ropeNode3.nNumLinks;
					num7++;
				}
			}
			if (this.RopeDiameter < 0.01f)
			{
				this.RopeDiameter = 0.01f;
			}
			bool flag2 = this.LinkJointBreakForce != float.PositiveInfinity || this.LinkJointBreakTorque != float.PositiveInfinity;
			Mesh mesh = new Mesh();
			int num9 = (!flag2) ? ((this.TotalLinks + 1) * (this.RopeSegmentSides + 1) + (this.RopeSegmentSides + 1) * 2) : (this.TotalLinks * (this.RopeSegmentSides + 1) * 4);
			int num10 = this.TotalLinks * this.RopeSegmentSides * 2;
			int num11 = (!flag2) ? (2 * (this.RopeSegmentSides - 2)) : (this.TotalLinks * 2 * (this.RopeSegmentSides - 2));
			Vector3[] array3 = new Vector3[num9];
			Vector2[] array4 = new Vector2[num9];
			Vector4[] array5 = new Vector4[num9];
			BoneWeight[] array6 = new BoneWeight[num9];
			int[] array7 = new int[num10 * 3];
			int[] array8 = new int[num11 * 3];
			if (flag2)
			{
				int num12 = 0;
				for (int num13 = 0; num13 < this.TotalLinks; num13++)
				{
					int num14 = num13;
					int num15 = num14;
					float num16 = 1f;
					float num17 = 1f - num16;
					this.FillLinkMeshIndicesRope(num13, this.TotalLinks, ref array7, flag2, false);
					this.FillLinkMeshIndicesSections(num13, this.TotalLinks, ref array8, flag2, false);
					for (int num18 = 0; num18 < 4; num18++)
					{
						for (int num19 = 0; num19 < this.RopeSegmentSides + 1; num19++)
						{
							int num20 = (num18 >= 2) ? 1 : 0;
							float num21 = (float)(num13 + num20) / (float)this.TotalLinks;
							float num22 = Mathf.Cos((float)num19 / (float)this.RopeSegmentSides * 3.1415927f * 2f);
							float num23 = Mathf.Sin((float)num19 / (float)this.RopeSegmentSides * 3.1415927f * 2f);
							array3[num12] = new Vector3(num22 * this.RopeDiameter * 0.5f, num23 * this.RopeDiameter * 0.5f, this.LinkLengths[num13] * (float)num20);
							array3[num12] = array[num14].TransformPoint(array3[num12]) * num16 + array[num15].TransformPoint(array3[num12]) * num17;
							array3[num12] = base.transform.InverseTransformPoint(array3[num12]);
							if (num18 == 0 || num18 == 3)
							{
								array4[num12] = new Vector2(Mathf.Clamp01((num22 + 1f) * 0.5f), Mathf.Clamp01((num23 + 1f) * 0.5f));
								array5[num12] = new Vector4(1f, 0f, 0f, 1f);
							}
							else
							{
								array4[num12] = new Vector2(num21 * this.TotalRopeLength * this.RopeTextureTileMeters, (float)num19 / (float)this.RopeSegmentSides);
								array5[num12] = new Vector4(0f, 0f, 1f, 1f);
							}
							array6[num12].boneIndex0 = num14;
							array6[num12].boneIndex1 = num15;
							array6[num12].weight0 = num16;
							array6[num12].weight1 = num17;
							num12++;
						}
					}
				}
			}
			else
			{
				int num24 = 0;
				this.FillLinkMeshIndicesSections(0, this.TotalLinks, ref array8, flag2, false);
				for (int num25 = 0; num25 < this.TotalLinks + 1; num25++)
				{
					int num26 = (num25 >= this.TotalLinks) ? (this.TotalLinks - 1) : num25;
					int num27 = num26;
					float num28 = 1f;
					float num29 = 1f - num28;
					if (num25 < this.TotalLinks)
					{
						this.FillLinkMeshIndicesRope(num25, this.TotalLinks, ref array7, flag2, false);
					}
					bool flag3 = false;
					bool flag4 = false;
					int num30 = 1;
					if (num25 == 0)
					{
						num30++;
						flag3 = true;
					}
					if (num25 == this.TotalLinks)
					{
						num30++;
						flag4 = true;
					}
					for (int num31 = 0; num31 < num30; num31++)
					{
						for (int num32 = 0; num32 < this.RopeSegmentSides + 1; num32++)
						{
							float num33 = (float)num25 / (float)this.TotalLinks;
							float num34 = Mathf.Cos((float)num32 / (float)this.RopeSegmentSides * 3.1415927f * 2f);
							float num35 = Mathf.Sin((float)num32 / (float)this.RopeSegmentSides * 3.1415927f * 2f);
							array3[num24] = new Vector3(num34 * this.RopeDiameter * 0.5f, num35 * this.RopeDiameter * 0.5f, flag4 ? this.LinkLengths[this.TotalLinks - 1] : 0f);
							array3[num24] = array[num26].TransformPoint(array3[num24]) * num28 + array[num27].TransformPoint(array3[num24]) * num29;
							array3[num24] = base.transform.InverseTransformPoint(array3[num24]);
							if ((flag3 && num31 == 0) || (flag4 && num31 == num30 - 1))
							{
								array4[num24] = new Vector2(Mathf.Clamp01((num34 + 1f) * 0.5f), Mathf.Clamp01((num35 + 1f) * 0.5f));
								array5[num24] = new Vector4(1f, 0f, 0f, 1f);
							}
							else
							{
								array4[num24] = new Vector2(num33 * this.TotalRopeLength * this.RopeTextureTileMeters, (float)num32 / (float)this.RopeSegmentSides);
								array5[num24] = new Vector4(0f, 0f, 1f, 1f);
							}
							array6[num24].boneIndex0 = num26;
							array6[num24].boneIndex1 = num27;
							array6[num24].weight0 = num28;
							array6[num24].weight1 = num29;
							num24++;
						}
					}
				}
			}
			mesh.vertices = array3;
			mesh.uv = array4;
			mesh.boneWeights = array6;
			mesh.bindposes = array2;
			mesh.subMeshCount = 2;
			mesh.SetTriangles(array7, 0);
			mesh.SetTriangles(array8, 1);
			mesh.RecalculateNormals();
			mesh.tangents = array5;
			SkinnedMeshRenderer skinnedMeshRenderer = (!(base.gameObject.GetComponent<SkinnedMeshRenderer>() != null)) ? base.gameObject.AddComponent<SkinnedMeshRenderer>() : base.gameObject.GetComponent<SkinnedMeshRenderer>();
			skinnedMeshRenderer.materials = new Material[]
			{
				this.RopeMaterial,
				this.RopeSectionMaterial
			};
			skinnedMeshRenderer.bones = array;
			skinnedMeshRenderer.sharedMesh = mesh;
			skinnedMeshRenderer.updateWhenOffscreen = true;
		}
		this.Deleted = false;
		if (Application.isPlaying)
		{
			this.CreateRopeJoints(false);
		}
		this.SetupRopeLinks();
		float realtimeSinceStartup2 = Time.realtimeSinceStartup;
		this.Status = string.Format("Rope generated in {0} seconds", realtimeSinceStartup2 - realtimeSinceStartup);
		this.m_bLastStatusIsError = false;
		return true;
	}

	// Token: 0x0600274C RID: 10060 RVA: 0x00019F8C File Offset: 0x0001818C
	public bool IsLastStatusError()
	{
		return this.m_bLastStatusIsError;
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x001325B8 File Offset: 0x001307B8
	public bool ChangeRopeDiameter(float fNewDiameter)
	{
		if (this.RopeType != UltimateRope.ERopeType.Procedural)
		{
			return false;
		}
		SkinnedMeshRenderer component = base.gameObject.GetComponent<SkinnedMeshRenderer>();
		if (component == null)
		{
			return false;
		}
		this.RopeDiameter = fNewDiameter;
		if (this.RopeDiameter < 0.01f)
		{
			this.RopeDiameter = 0.01f;
		}
		bool flag = this.LinkJointBreakForce != float.PositiveInfinity || this.LinkJointBreakTorque != float.PositiveInfinity;
		Vector3[] vertices = component.sharedMesh.vertices;
		Matrix4x4[] bindposes = component.sharedMesh.bindposes;
		Vector2[] array = new Vector2[this.RopeSegmentSides + 1];
		for (int i = 0; i < this.RopeSegmentSides + 1; i++)
		{
			float num = Mathf.Cos((float)i / (float)this.RopeSegmentSides * 3.1415927f * 2f);
			float num2 = Mathf.Sin((float)i / (float)this.RopeSegmentSides * 3.1415927f * 2f);
			array[i] = new Vector2(num * this.RopeDiameter * 0.5f, num2 * this.RopeDiameter * 0.5f);
		}
		if (flag)
		{
			int num3 = 0;
			for (int j = 0; j < this.TotalLinks; j++)
			{
				int num4 = j;
				int num5 = num4;
				float num6 = 1f;
				float num7 = 1f - num6;
				bindposes[j] = component.bones[j].transform.worldToLocalMatrix;
				if (component.bones[j].transform.parent != null)
				{
					bindposes[j] *= base.transform.localToWorldMatrix;
				}
				for (int k = 0; k < 4; k++)
				{
					for (int l = 0; l < this.RopeSegmentSides + 1; l++)
					{
						int num8 = (k >= 2) ? 1 : 0;
						vertices[num3] = new Vector3(array[l].x, array[l].y, this.LinkLengths[j] * (float)num8);
						vertices[num3] = component.bones[num4].TransformPoint(vertices[num3]) * num6 + component.bones[num5].TransformPoint(vertices[num3]) * num7;
						vertices[num3] = base.transform.InverseTransformPoint(vertices[num3]);
						num3++;
					}
				}
			}
		}
		else
		{
			int num9 = 0;
			for (int m = 0; m < this.TotalLinks + 1; m++)
			{
				int num10 = (m >= this.TotalLinks) ? (this.TotalLinks - 1) : m;
				int num11 = num10;
				float num12 = 1f;
				float num13 = 1f - num12;
				bool flag2 = false;
				int num14 = 1;
				if (m == 0)
				{
					num14++;
				}
				if (m == this.TotalLinks)
				{
					num14++;
					flag2 = true;
				}
				if (m < this.TotalLinks)
				{
					bindposes[m] = component.bones[m].transform.worldToLocalMatrix;
					if (component.bones[m].transform.parent != null)
					{
						bindposes[m] *= base.transform.localToWorldMatrix;
					}
				}
				for (int n = 0; n < num14; n++)
				{
					for (int num15 = 0; num15 < this.RopeSegmentSides + 1; num15++)
					{
						vertices[num9] = new Vector3(array[num15].x, array[num15].y, flag2 ? this.LinkLengths[this.TotalLinks - 1] : 0f);
						vertices[num9] = component.bones[num10].TransformPoint(vertices[num9]) * num12 + component.bones[num11].TransformPoint(vertices[num9]) * num13;
						vertices[num9] = base.transform.InverseTransformPoint(vertices[num9]);
						num9++;
					}
				}
			}
		}
		component.sharedMesh.vertices = vertices;
		component.sharedMesh.bindposes = bindposes;
		this.SetupRopeLinks();
		return true;
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x00132A94 File Offset: 0x00130C94
	public bool ChangeRopeSegmentSides(int nNewSegmentSides)
	{
		if (this.RopeType != UltimateRope.ERopeType.Procedural)
		{
			return false;
		}
		SkinnedMeshRenderer component = base.gameObject.GetComponent<SkinnedMeshRenderer>();
		if (component == null)
		{
			return false;
		}
		this.RopeSegmentSides = nNewSegmentSides;
		if (this.RopeSegmentSides < 3)
		{
			this.RopeSegmentSides = 3;
		}
		bool flag = this.LinkJointBreakForce != float.PositiveInfinity || this.LinkJointBreakTorque != float.PositiveInfinity;
		Mesh mesh = new Mesh();
		int num = (!flag) ? ((this.TotalLinks + 1) * (this.RopeSegmentSides + 1) + (this.RopeSegmentSides + 1) * 2) : (this.TotalLinks * (this.RopeSegmentSides + 1) * 4);
		int num2 = this.TotalLinks * this.RopeSegmentSides * 2;
		int num3 = (!flag) ? (2 * (this.RopeSegmentSides - 2)) : (this.TotalLinks * 2 * (this.RopeSegmentSides - 2));
		Vector3[] array = new Vector3[num];
		Vector2[] array2 = new Vector2[num];
		Vector4[] array3 = new Vector4[num];
		BoneWeight[] array4 = new BoneWeight[num];
		int[] array5 = new int[num2 * 3];
		int[] array6 = new int[num3 * 3];
		Matrix4x4[] bindposes = component.sharedMesh.bindposes;
		if (flag)
		{
			int num4 = 0;
			for (int i = 0; i < this.TotalLinks; i++)
			{
				int num5 = i;
				int num6 = num5;
				float num7 = 1f;
				float num8 = 1f - num7;
				bindposes[i] = component.bones[i].transform.worldToLocalMatrix;
				if (component.bones[i].transform.parent != null)
				{
					bindposes[i] *= base.transform.localToWorldMatrix;
				}
				this.FillLinkMeshIndicesRope(i, this.TotalLinks, ref array5, flag, false);
				this.FillLinkMeshIndicesSections(i, this.TotalLinks, ref array6, flag, false);
				for (int j = 0; j < 4; j++)
				{
					for (int k = 0; k < this.RopeSegmentSides + 1; k++)
					{
						int num9 = (j >= 2) ? 1 : 0;
						float num10 = (float)(i + num9) / (float)this.TotalLinks;
						float num11 = Mathf.Cos((float)k / (float)this.RopeSegmentSides * 3.1415927f * 2f);
						float num12 = Mathf.Sin((float)k / (float)this.RopeSegmentSides * 3.1415927f * 2f);
						array[num4] = new Vector3(num11 * this.RopeDiameter * 0.5f, num12 * this.RopeDiameter * 0.5f, this.LinkLengths[i] * (float)num9);
						array[num4] = component.bones[num5].TransformPoint(array[num4]) * num7 + component.bones[num6].TransformPoint(array[num4]) * num8;
						array[num4] = base.transform.InverseTransformPoint(array[num4]);
						if (j == 0 || j == 3)
						{
							array2[num4] = new Vector2(Mathf.Clamp01((num11 + 1f) * 0.5f), Mathf.Clamp01((num12 + 1f) * 0.5f));
							array3[num4] = new Vector4(1f, 0f, 0f, 1f);
						}
						else
						{
							array2[num4] = new Vector2(num10 * this.TotalRopeLength * this.RopeTextureTileMeters, (float)k / (float)this.RopeSegmentSides);
							array3[num4] = new Vector4(0f, 0f, 1f, 1f);
						}
						array4[num4].boneIndex0 = num5;
						array4[num4].boneIndex1 = num6;
						array4[num4].weight0 = num7;
						array4[num4].weight1 = num8;
						num4++;
					}
				}
			}
		}
		else
		{
			int num13 = 0;
			this.FillLinkMeshIndicesSections(0, this.TotalLinks, ref array6, flag, false);
			for (int l = 0; l < this.TotalLinks + 1; l++)
			{
				int num14 = (l >= this.TotalLinks) ? (this.TotalLinks - 1) : l;
				int num15 = num14;
				float num16 = 1f;
				float num17 = 1f - num16;
				if (l < this.TotalLinks)
				{
					this.FillLinkMeshIndicesRope(l, this.TotalLinks, ref array5, flag, false);
				}
				bool flag2 = false;
				bool flag3 = false;
				int num18 = 1;
				if (l == 0)
				{
					num18++;
					flag2 = true;
				}
				if (l == this.TotalLinks)
				{
					num18++;
					flag3 = true;
				}
				if (l < this.TotalLinks)
				{
					bindposes[l] = component.bones[l].transform.worldToLocalMatrix;
					if (component.bones[l].transform.parent != null)
					{
						bindposes[l] *= base.transform.localToWorldMatrix;
					}
				}
				for (int m = 0; m < num18; m++)
				{
					for (int n = 0; n < this.RopeSegmentSides + 1; n++)
					{
						float num19 = (float)l / (float)this.TotalLinks;
						float num20 = Mathf.Cos((float)n / (float)this.RopeSegmentSides * 3.1415927f * 2f);
						float num21 = Mathf.Sin((float)n / (float)this.RopeSegmentSides * 3.1415927f * 2f);
						array[num13] = new Vector3(num20 * this.RopeDiameter * 0.5f, num21 * this.RopeDiameter * 0.5f, flag3 ? this.LinkLengths[this.TotalLinks - 1] : 0f);
						array[num13] = component.bones[num14].TransformPoint(array[num13]) * num16 + component.bones[num15].TransformPoint(array[num13]) * num17;
						array[num13] = base.transform.InverseTransformPoint(array[num13]);
						if ((flag2 && m == 0) || (flag3 && m == num18 - 1))
						{
							array2[num13] = new Vector2(Mathf.Clamp01((num20 + 1f) * 0.5f), Mathf.Clamp01((num21 + 1f) * 0.5f));
							array3[num13] = new Vector4(1f, 0f, 0f, 1f);
						}
						else
						{
							array2[num13] = new Vector2(num19 * this.TotalRopeLength * this.RopeTextureTileMeters, (float)n / (float)this.RopeSegmentSides);
							array3[num13] = new Vector4(0f, 0f, 1f, 1f);
						}
						array4[num13].boneIndex0 = num14;
						array4[num13].boneIndex1 = num15;
						array4[num13].weight0 = num16;
						array4[num13].weight1 = num17;
						num13++;
					}
				}
			}
		}
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.boneWeights = array4;
		mesh.bindposes = bindposes;
		mesh.subMeshCount = 2;
		mesh.SetTriangles(array5, 0);
		mesh.SetTriangles(array6, 1);
		mesh.RecalculateNormals();
		mesh.tangents = array3;
		if (Application.isEditor && !Application.isPlaying)
		{
			Object.DestroyImmediate(component.sharedMesh);
		}
		else
		{
			Object.Destroy(component.sharedMesh);
		}
		component.sharedMesh = mesh;
		this.SetupRopeLinks();
		return true;
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x001332FC File Offset: 0x001314FC
	public void SetupRopeMaterials()
	{
		if (this.RopeType != UltimateRope.ERopeType.Procedural)
		{
			return;
		}
		SkinnedMeshRenderer component = base.gameObject.GetComponent<SkinnedMeshRenderer>();
		if (component != null)
		{
			component.materials = new Material[]
			{
				this.RopeMaterial,
				this.RopeSectionMaterial
			};
		}
	}

	// Token: 0x06002750 RID: 10064 RVA: 0x00133350 File Offset: 0x00131550
	public void SetupRopeLinks()
	{
		if (this.RopeNodes == null)
		{
			return;
		}
		if (this.RopeNodes.Count == 0)
		{
			return;
		}
		if (this.Deleted)
		{
			return;
		}
		if (this.RopeType == UltimateRope.ERopeType.ImportBones && this.ImportedBones == null)
		{
			return;
		}
		base.gameObject.layer = this.RopeLayer;
		if (this.RopeDiameter < 0.01f)
		{
			this.RopeDiameter = 0.01f;
		}
		for (int i = 0; i < this.RopeNodes.Count; i++)
		{
			UltimateRope.RopeNode ropeNode = this.RopeNodes[i];
			if (!ropeNode.bIsCoil)
			{
				if (this.RopeType == UltimateRope.ERopeType.ImportBones)
				{
					ropeNode.eColliderType = this.BoneColliderType;
					ropeNode.nColliderSkip = this.BoneColliderSkip;
				}
				float num = ropeNode.fLength / (float)ropeNode.nNumLinks;
				float linkDiameter = this.GetLinkDiameter();
				int nColliderSkip = ropeNode.nColliderSkip;
				float fValue = (this.RopeType != UltimateRope.ERopeType.Procedural) ? 0f : (num * 0.5f);
				int num2 = 0;
				foreach (GameObject gameObject in ropeNode.segmentLinks)
				{
					if (gameObject)
					{
						if (gameObject.collider)
						{
							Object.DestroyImmediate(gameObject.collider);
						}
						bool flag = num2 % (nColliderSkip + 1) == 0;
						bool isKinematic = gameObject.rigidbody != null && gameObject.rigidbody.isKinematic;
						if (this.RopeType == UltimateRope.ERopeType.ImportBones)
						{
							if (Mathf.Approximately(this.ImportedBones[num2].fLength, 0f))
							{
								flag = false;
							}
							else if (flag)
							{
								flag = this.ImportedBones[num2].bCreatedCollider;
							}
							num = this.ImportedBones[num2].fLength * this.BoneColliderLength;
							fValue = num * this.BoneColliderOffset;
							isKinematic = this.ImportedBones[num2].bIsStatic;
						}
						if (flag)
						{
							UltimateRope.EColliderType eColliderType = ropeNode.eColliderType;
							if (eColliderType != UltimateRope.EColliderType.Capsule)
							{
								if (eColliderType == UltimateRope.EColliderType.Box)
								{
									BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
									Vector3 linkAxisOffset = this.GetLinkAxisOffset(fValue);
									Vector3 zero = Vector3.zero;
									boxCollider.material = this.RopePhysicsMaterial;
									if (this.GetLinkBoxColliderCenterAndSize(num, linkDiameter, ref linkAxisOffset, ref zero))
									{
										boxCollider.center = linkAxisOffset;
										boxCollider.size = zero;
										boxCollider.enabled = flag;
									}
									else
									{
										boxCollider.enabled = false;
									}
								}
							}
							else
							{
								CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
								capsuleCollider.material = this.RopePhysicsMaterial;
								capsuleCollider.center = this.GetLinkAxisOffset(fValue);
								capsuleCollider.radius = linkDiameter * 0.5f;
								capsuleCollider.height = num;
								capsuleCollider.direction = this.GetLinkAxisIndex();
								capsuleCollider.material = this.RopePhysicsMaterial;
								capsuleCollider.enabled = flag;
							}
						}
						Rigidbody rigidbody = (!(gameObject.rigidbody != null)) ? gameObject.AddComponent<Rigidbody>() : gameObject.rigidbody;
						rigidbody.mass = this.LinkMass;
						rigidbody.solverIterationCount = this.LinkSolverIterationCount;
						rigidbody.isKinematic = isKinematic;
						gameObject.layer = this.RopeLayer;
						num2++;
					}
				}
			}
		}
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x001336B4 File Offset: 0x001318B4
	public void SetupRopeJoints()
	{
		if (this.RopeNodes == null)
		{
			return;
		}
		if (this.RopeNodes.Count == 0)
		{
			return;
		}
		if (this.Deleted)
		{
			return;
		}
		if (this.RopeType == UltimateRope.ERopeType.ImportBones && this.ImportedBones == null)
		{
			return;
		}
		foreach (UltimateRope.RopeNode ropeNode in this.RopeNodes)
		{
			if (ropeNode.segmentLinks == null)
			{
				return;
			}
		}
		int num = 0;
		Vector3[] array = new Vector3[this.TotalLinks];
		Quaternion[] array2 = new Quaternion[this.TotalLinks];
		Vector3 localPosition = (!(this.RopeStart != null)) ? Vector3.zero : this.RopeStart.transform.localPosition;
		Quaternion localRotation = (!(this.RopeStart != null)) ? Quaternion.identity : this.RopeStart.transform.localRotation;
		Vector3[] array3 = new Vector3[this.RopeNodes.Count];
		Quaternion[] array4 = new Quaternion[this.RopeNodes.Count];
		if (this.m_bRopeStartInitialOrientationInitialized && this.RopeStart != null)
		{
			this.RopeStart.transform.localPosition = this.m_v3InitialRopeStartLocalPos;
			this.RopeStart.transform.localRotation = this.m_qInitialRopeStartLocalRot;
		}
		for (int i = 0; i < this.RopeNodes.Count; i++)
		{
			UltimateRope.RopeNode ropeNode2 = this.RopeNodes[i];
			if (ropeNode2.bInitialOrientationInitialized && ropeNode2.goNode != null)
			{
				array3[i] = ropeNode2.goNode.transform.localPosition;
				array4[i] = ropeNode2.goNode.transform.localRotation;
				ropeNode2.goNode.transform.localPosition = ropeNode2.v3InitialLocalPos;
				ropeNode2.goNode.transform.localRotation = ropeNode2.qInitialLocalRot;
			}
		}
		if (this.RopeType == UltimateRope.ERopeType.Procedural || this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			for (int j = 0; j < this.RopeNodes.Count; j++)
			{
				UltimateRope.RopeNode ropeNode3 = this.RopeNodes[j];
				float num2 = ropeNode3.fLength / (float)ropeNode3.nNumLinks;
				float num3 = num2 * (float)(ropeNode3.segmentLinks.Length - 1);
				for (int k = 0; k < ropeNode3.segmentLinks.Length; k++)
				{
					float num4 = (float)k / ((ropeNode3.segmentLinks.Length != 1) ? ((float)ropeNode3.segmentLinks.Length - 1f) : 1f);
					array[num] = ropeNode3.segmentLinks[k].transform.position;
					array2[num] = ropeNode3.segmentLinks[k].transform.rotation;
					if (!ropeNode3.bIsCoil)
					{
						ropeNode3.segmentLinks[k].transform.position = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, num3), num4);
						ropeNode3.segmentLinks[k].transform.rotation = Quaternion.identity;
						if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
						{
							ropeNode3.segmentLinks[k].transform.rotation *= this.GetLinkedObjectLocalRotation(this.LinkTwistAngleStart + this.LinkTwistAngleIncrement * (float)k);
						}
					}
					num++;
				}
			}
		}
		else if (this.RopeType == UltimateRope.ERopeType.ImportBones)
		{
			for (int l = 0; l < this.ImportedBones.Length; l++)
			{
				array[l] = this.ImportedBones[l].goBone.transform.position;
				array2[l] = this.ImportedBones[l].goBone.transform.rotation;
				if (this.ImportedBones[l].tfNonBoneParent != null)
				{
					Transform parent = this.ImportedBones[l].goBone.transform.parent;
					this.ImportedBones[l].goBone.transform.parent = this.ImportedBones[l].tfNonBoneParent;
					this.ImportedBones[l].goBone.transform.localPosition = this.ImportedBones[l].v3OriginalLocalPos;
					this.ImportedBones[l].goBone.transform.localRotation = this.ImportedBones[l].qOriginalLocalRot;
					this.ImportedBones[l].goBone.transform.parent = parent;
					this.ImportedBones[l].goBone.transform.localScale = this.ImportedBones[l].v3OriginalLocalScale;
				}
			}
		}
		for (int m = 0; m < this.RopeNodes.Count; m++)
		{
			UltimateRope.RopeNode ropeNode4 = this.RopeNodes[m];
			if (!ropeNode4.bIsCoil)
			{
				foreach (ConfigurableJoint configurableJoint in ropeNode4.linkJoints)
				{
					if (configurableJoint)
					{
						this.SetupJoint(configurableJoint);
					}
				}
				if ((this.RopeType == UltimateRope.ERopeType.Procedural || this.RopeType == UltimateRope.ERopeType.LinkedObjects) && ropeNode4.bInitialOrientationInitialized)
				{
					GameObject gameObject = (m != this.m_nFirstNonCoilNode) ? this.RopeNodes[m - 1].goNode : this.RopeStart;
					GameObject goNode = this.RopeNodes[m].goNode;
					Vector3 vector = gameObject.transform.TransformDirection(ropeNode4.m_v3LocalDirectionForward);
					Vector3 vector2 = gameObject.transform.TransformDirection(ropeNode4.m_v3LocalDirectionUp);
					ropeNode4.segmentLinks[0].transform.position = gameObject.transform.position;
					ropeNode4.segmentLinks[0].transform.rotation = Quaternion.LookRotation(vector, vector2);
					ropeNode4.segmentLinks[ropeNode4.segmentLinks.Length - 1].transform.position = goNode.transform.position - vector * (ropeNode4.fLength / (float)ropeNode4.nNumLinks);
					ropeNode4.segmentLinks[ropeNode4.segmentLinks.Length - 1].transform.rotation = Quaternion.LookRotation(vector, vector2);
					if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
					{
						ropeNode4.segmentLinks[0].transform.rotation *= this.GetLinkedObjectLocalRotation(this.LinkTwistAngleStart);
						ropeNode4.segmentLinks[ropeNode4.segmentLinks.Length - 1].transform.rotation *= this.GetLinkedObjectLocalRotation(this.LinkTwistAngleStart + this.LinkTwistAngleIncrement * (float)(ropeNode4.segmentLinks.Length - 1));
					}
					if (ropeNode4.linkJoints[0] != null)
					{
						this.SetupJoint(ropeNode4.linkJoints[0]);
					}
					if (ropeNode4.linkJoints[ropeNode4.linkJoints.Length - 1] != null)
					{
						this.SetupJoint(ropeNode4.linkJoints[ropeNode4.linkJoints.Length - 1]);
					}
				}
			}
		}
		num = 0;
		if (this.m_bRopeStartInitialOrientationInitialized && this.RopeStart != null)
		{
			this.RopeStart.transform.localPosition = localPosition;
			this.RopeStart.transform.localRotation = localRotation;
		}
		for (int num5 = 0; num5 < this.RopeNodes.Count; num5++)
		{
			UltimateRope.RopeNode ropeNode5 = this.RopeNodes[num5];
			if (ropeNode5.bInitialOrientationInitialized && ropeNode5.goNode != null)
			{
				ropeNode5.goNode.transform.localPosition = array3[num5];
				ropeNode5.goNode.transform.localRotation = array4[num5];
			}
		}
		if (this.RopeType == UltimateRope.ERopeType.Procedural || this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			for (int num6 = 0; num6 < this.RopeNodes.Count; num6++)
			{
				UltimateRope.RopeNode ropeNode6 = this.RopeNodes[num6];
				for (int num7 = 0; num7 < ropeNode6.segmentLinks.Length; num7++)
				{
					ropeNode6.segmentLinks[num7].transform.position = array[num];
					ropeNode6.segmentLinks[num7].transform.rotation = array2[num];
					num++;
				}
			}
		}
		else if (this.RopeType == UltimateRope.ERopeType.ImportBones)
		{
			for (int num8 = 0; num8 < this.ImportedBones.Length; num8++)
			{
				this.ImportedBones[num8].goBone.transform.position = array[num8];
				this.ImportedBones[num8].goBone.transform.rotation = array2[num8];
			}
		}
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x00134068 File Offset: 0x00132268
	public void CheckNeedsStartExitLockZ()
	{
		if (this.RopeType == UltimateRope.ERopeType.Procedural)
		{
			int num = 0;
			for (int i = 0; i < this.RopeNodes.Count; i++)
			{
				UltimateRope.RopeNode ropeNode = this.RopeNodes[i];
				for (int j = 0; j < ropeNode.segmentLinks.Length; j++)
				{
					Transform transform = null;
					Transform transform2 = null;
					if (!this.FirstNodeIsCoil())
					{
						transform = ((i != this.m_nFirstNonCoilNode) ? this.RopeNodes[i - 1].goNode.transform : this.RopeStart.transform);
						transform2 = this.RopeNodes[i].goNode.transform;
					}
					if (transform != null && transform2 != null)
					{
						if (j == 0)
						{
							ropeNode.segmentLinks[j].transform.rotation = ((!this.LockStartEndInZAxis) ? Quaternion.LookRotation((transform2.position - transform.position).normalized) : transform.rotation);
							ropeNode.segmentLinks[j].transform.parent = ((!this.LockStartEndInZAxis) ? base.transform : transform);
							ropeNode.segmentLinks[j].rigidbody.isKinematic = this.LockStartEndInZAxis;
						}
						else if (j == ropeNode.segmentLinks.Length - 1)
						{
							ropeNode.segmentLinks[j].transform.position = ((!this.LockStartEndInZAxis) ? (transform2.position - (transform2.position - transform.position).normalized * this.LinkLengths[num]) : (transform2.position - transform2.forward * this.LinkLengths[num]));
							ropeNode.segmentLinks[j].transform.rotation = ((!this.LockStartEndInZAxis) ? Quaternion.LookRotation((transform2.position - transform.position).normalized) : transform2.rotation);
							ropeNode.segmentLinks[j].transform.parent = ((!this.LockStartEndInZAxis) ? base.transform : transform2);
							ropeNode.segmentLinks[j].rigidbody.isKinematic = this.LockStartEndInZAxis;
						}
					}
					num++;
				}
			}
		}
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x001342F0 File Offset: 0x001324F0
	public void FillLinkMeshIndicesRope(int nLinearLinkIndex, int nTotalLinks, ref int[] indices, bool bBreakable, bool bBrokenLink = false)
	{
		if (bBreakable)
		{
			int num = nLinearLinkIndex * this.RopeSegmentSides * 2;
			int num2 = nLinearLinkIndex * (this.RopeSegmentSides + 1) * 4 + (this.RopeSegmentSides + 1);
			int num3 = (this.RopeSegmentSides + 1) * 3;
			int num4 = (bBrokenLink || nLinearLinkIndex >= nTotalLinks - 1) ? 0 : num3;
			for (int i = 0; i < this.RopeSegmentSides + 1; i++)
			{
				if (i < this.RopeSegmentSides)
				{
					int num5 = num2 + i;
					indices[num * 3 + 2] = num5;
					indices[num * 3 + 1] = num5 + num4 + (this.RopeSegmentSides + 1);
					indices[num * 3] = num5 + 1;
					indices[num * 3 + 5] = num5 + 1;
					indices[num * 3 + 4] = num5 + num4 + (this.RopeSegmentSides + 1);
					indices[num * 3 + 3] = num5 + num4 + (this.RopeSegmentSides + 1) + 1;
					num += 2;
				}
			}
		}
		else
		{
			int num6 = nLinearLinkIndex * this.RopeSegmentSides * 2;
			int num7 = nLinearLinkIndex * (this.RopeSegmentSides + 1) + (this.RopeSegmentSides + 1);
			for (int j = 0; j < this.RopeSegmentSides + 1; j++)
			{
				if (j < this.RopeSegmentSides)
				{
					int num8 = num7 + j;
					indices[num6 * 3 + 2] = num8;
					indices[num6 * 3 + 1] = num8 + this.RopeSegmentSides + 1;
					indices[num6 * 3] = num8 + 1;
					indices[num6 * 3 + 5] = num8 + 1;
					indices[num6 * 3 + 4] = num8 + this.RopeSegmentSides + 1;
					indices[num6 * 3 + 3] = num8 + 1 + this.RopeSegmentSides + 1;
					num6 += 2;
				}
			}
		}
	}

	// Token: 0x06002754 RID: 10068 RVA: 0x0013449C File Offset: 0x0013269C
	public void FillLinkMeshIndicesSections(int nLinearLinkIndex, int nTotalLinks, ref int[] indices, bool bBreakable, bool bBrokenLink = false)
	{
		if (bBreakable)
		{
			int num = nLinearLinkIndex * 2 * (this.RopeSegmentSides - 2);
			int num2 = nLinearLinkIndex * (this.RopeSegmentSides + 1) * 4;
			int num3 = (this.RopeSegmentSides + 1) * 2;
			for (int i = 0; i < this.RopeSegmentSides - 2; i++)
			{
				indices[num * 3] = num2;
				indices[num * 3 + 1] = num2 + (i + 2);
				indices[num * 3 + 2] = num2 + (i + 1);
				num++;
			}
			int num4 = (bBrokenLink || nLinearLinkIndex >= nTotalLinks - 1) ? 0 : num3;
			for (int j = 0; j < this.RopeSegmentSides - 2; j++)
			{
				indices[num * 3 + 2] = num2 + (this.RopeSegmentSides + 1) * 3 + num4;
				indices[num * 3 + 1] = num2 + (this.RopeSegmentSides + 1) * 3 + num4 + (j + 2);
				indices[num * 3] = num2 + (this.RopeSegmentSides + 1) * 3 + num4 + (j + 1);
				num++;
			}
		}
		else
		{
			int num5 = 0;
			int num6 = 0;
			for (int k = 0; k < this.RopeSegmentSides - 2; k++)
			{
				indices[num5 * 3] = num6;
				indices[num5 * 3 + 1] = num6 + (k + 2);
				indices[num5 * 3 + 2] = num6 + (k + 1);
				num5++;
			}
			num6 = (this.TotalLinks + 1) * (this.RopeSegmentSides + 1) + (this.RopeSegmentSides + 1);
			for (int l = 0; l < this.RopeSegmentSides - 2; l++)
			{
				indices[num5 * 3 + 2] = num6;
				indices[num5 * 3 + 1] = num6 + (l + 2);
				indices[num5 * 3] = num6 + (l + 1);
				num5++;
			}
		}
	}

	// Token: 0x06002755 RID: 10069 RVA: 0x0013465C File Offset: 0x0013285C
	public bool HasDynamicSegmentNodes()
	{
		if (this.RopeNodes == null)
		{
			return false;
		}
		if (this.RopeNodes.Count == 0)
		{
			return false;
		}
		foreach (UltimateRope.RopeNode ropeNode in this.RopeNodes)
		{
			if (ropeNode.goNode && ropeNode.goNode.rigidbody && !ropeNode.goNode.rigidbody.isKinematic)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x00134714 File Offset: 0x00132914
	public void BeforeImportedBonesObjectRespawn()
	{
		if (this.ImportedBones != null)
		{
			foreach (UltimateRope.RopeBone ropeBone in this.ImportedBones)
			{
				if (ropeBone.goBone != null)
				{
					ropeBone.goBone.transform.parent = ropeBone.tfParent;
				}
			}
		}
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x00134774 File Offset: 0x00132974
	public void AfterImportedBonesObjectRespawn()
	{
		if (this.ImportedBones != null)
		{
			foreach (UltimateRope.RopeBone ropeBone in this.ImportedBones)
			{
				if (ropeBone.goBone != null)
				{
					ropeBone.goBone.transform.parent = ((!ropeBone.bIsStatic) ? base.transform : ropeBone.tfNonBoneParent);
				}
			}
		}
	}

	// Token: 0x06002758 RID: 10072 RVA: 0x001347E8 File Offset: 0x001329E8
	public void ExtendRope(UltimateRope.ERopeExtensionMode eRopeExtensionMode, float fIncrement)
	{
		if (!this.IsExtensible)
		{
			Debug.LogError("Rope can not be extended since the IsExtensible property has been marked as false");
			return;
		}
		if (eRopeExtensionMode == UltimateRope.ERopeExtensionMode.CoilRotationIncrement && !this.FirstNodeIsCoil())
		{
			Debug.LogError("Rope can not be extended through coil rotation since no coil is present");
			return;
		}
		float fLinearIncrement = (eRopeExtensionMode != UltimateRope.ERopeExtensionMode.LinearExtensionIncrement) ? 0f : fIncrement;
		float fCurrentCoilRopeRadius = this.m_fCurrentCoilRopeRadius;
		if (eRopeExtensionMode == UltimateRope.ERopeExtensionMode.CoilRotationIncrement)
		{
			fLinearIncrement = this.m_fCurrentCoilRopeRadius * (fIncrement / 360f) * 2f * 3.1415927f;
		}
		float num = this.ExtendRopeLinear(fLinearIncrement);
		float num2 = num * 360f / (6.2831855f * fCurrentCoilRopeRadius);
		if (!Mathf.Approximately(num, 0f))
		{
			this.CoilObject.transform.Rotate(this.GetAxisVector(this.CoilAxisRight, 1f) * num2);
			this.SetupCoilBones(this.m_fCurrentCoilLength - num);
		}
	}

	// Token: 0x06002759 RID: 10073 RVA: 0x00019F94 File Offset: 0x00018194
	public void RecomputeCoil()
	{
		this.SetupCoilBones(this.m_fCurrentCoilLength);
	}

	// Token: 0x0600275A RID: 10074 RVA: 0x001348C0 File Offset: 0x00132AC0
	public GameObject BuildStaticMeshObject(out string strStatusMessage)
	{
		if (Application.isEditor && Application.isPlaying)
		{
			strStatusMessage = "Error: Rope can't be made static from the editor in play mode";
			return null;
		}
		if (this.RopeType == UltimateRope.ERopeType.Procedural)
		{
			SkinnedMeshRenderer component = base.GetComponent<SkinnedMeshRenderer>();
			if (component == null)
			{
				strStatusMessage = "Error: Procedural rope has no skinned mesh renderer";
				return null;
			}
			Mesh sharedMesh = component.sharedMesh;
			Mesh mesh = new Mesh();
			int vertexCount = component.sharedMesh.vertexCount;
			int num = component.sharedMesh.GetTriangles(0).Length;
			int num2 = component.sharedMesh.GetTriangles(1).Length;
			Vector3[] vertices = sharedMesh.vertices;
			Vector2[] uv = sharedMesh.uv;
			Vector4[] tangents = sharedMesh.tangents;
			int[] triangles = sharedMesh.GetTriangles(0);
			int[] triangles2 = sharedMesh.GetTriangles(1);
			Vector3[] array = new Vector3[vertexCount];
			Vector2[] array2 = new Vector2[vertexCount];
			Vector4[] array3 = (sharedMesh.tangents == null) ? null : new Vector4[sharedMesh.tangents.Length];
			int[] array4 = new int[num];
			int[] array5 = new int[num2];
			BoneWeight[] boneWeights = sharedMesh.boneWeights;
			Matrix4x4[] bindposes = sharedMesh.bindposes;
			Transform[] bones = component.bones;
			Vector3 vector;
			vector..ctor(0f, 0f, 0f);
			for (int i = 0; i < vertexCount; i++)
			{
				BoneWeight boneWeight = boneWeights[i];
				array[i] = new Vector3(0f, 0f, 0f);
				if (Math.Abs(boneWeight.weight0) > 1E-05f)
				{
					Vector3 vector2 = bindposes[boneWeight.boneIndex0].MultiplyPoint3x4(vertices[i]);
					array[i] += bones[boneWeight.boneIndex0].transform.localToWorldMatrix.MultiplyPoint3x4(vector2) * boneWeight.weight0;
				}
				if (Math.Abs(boneWeight.weight1) > 1E-05f)
				{
					Vector3 vector2 = bindposes[boneWeight.boneIndex1].MultiplyPoint3x4(vertices[i]);
					array[i] += bones[boneWeight.boneIndex1].transform.localToWorldMatrix.MultiplyPoint3x4(vector2) * boneWeight.weight1;
				}
				if (Math.Abs(boneWeight.weight2) > 1E-05f)
				{
					Vector3 vector2 = bindposes[boneWeight.boneIndex2].MultiplyPoint3x4(vertices[i]);
					array[i] += bones[boneWeight.boneIndex2].transform.localToWorldMatrix.MultiplyPoint3x4(vector2) * boneWeight.weight2;
				}
				if (Math.Abs(boneWeight.weight3) > 1E-05f)
				{
					Vector3 vector2 = bindposes[boneWeight.boneIndex3].MultiplyPoint3x4(vertices[i]);
					array[i] += bones[boneWeight.boneIndex3].transform.localToWorldMatrix.MultiplyPoint3x4(vector2) * boneWeight.weight3;
				}
				vector += array[i];
				array2[i] = uv[i];
				if (array3 != null && array3.Length == vertexCount)
				{
					array3[i] = tangents[i];
				}
			}
			if (vertexCount > 0)
			{
				vector /= (float)vertexCount;
			}
			Vector3 position = base.transform.position;
			base.transform.position = vector;
			for (int j = 0; j < vertexCount; j++)
			{
				array[j] = base.transform.InverseTransformPoint(array[j]);
			}
			base.transform.position = position;
			for (int k = 0; k < num; k++)
			{
				array4[k] = triangles[k];
			}
			for (int l = 0; l < num2; l++)
			{
				array5[l] = triangles2[l];
			}
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.subMeshCount = 2;
			mesh.SetTriangles(array4, 0);
			mesh.SetTriangles(array5, 1);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			if (array3 != null && array3.Length == vertexCount)
			{
				mesh.tangents = array3;
			}
			GameObject gameObject = new GameObject(base.gameObject.name + " (static)");
			gameObject.transform.position = vector;
			gameObject.transform.rotation = base.transform.rotation;
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshFilter.sharedMesh = mesh;
			meshRenderer.sharedMaterials = new Material[]
			{
				this.RopeMaterial,
				this.RopeSectionMaterial
			};
			gameObject.isStatic = true;
			MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = mesh;
			meshCollider.convex = false;
			meshCollider.material = this.RopePhysicsMaterial;
			base.gameObject.SetActive(false);
			strStatusMessage = "Rope converted succesfully";
			return gameObject;
		}
		else if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			if (this.LinkObject == null)
			{
				strStatusMessage = "Error: LinkObject not specified. Can't continue.";
				return null;
			}
			Renderer renderer = this.LinkObject.renderer;
			MeshFilter component2 = this.LinkObject.GetComponent<MeshFilter>();
			if (renderer == null)
			{
				strStatusMessage = "Error: LinkObject has no Renderer. Can't continue.";
				return null;
			}
			if (component2 == null)
			{
				strStatusMessage = "Error: LinkObject has no Mesh Filter. Can't continue.";
				return null;
			}
			if (component2.sharedMesh == null)
			{
				strStatusMessage = "Error: LinkObject has no mesh. Can't continue.";
				return null;
			}
			Material[] array6 = new Material[renderer.sharedMaterials.Length];
			for (int m = 0; m < renderer.sharedMaterials.Length; m++)
			{
				array6[m] = renderer.sharedMaterials[m];
			}
			List<CombineInstance> list = new List<CombineInstance>();
			for (int n = 0; n < this.RopeNodes.Count; n++)
			{
				UltimateRope.RopeNode ropeNode = this.RopeNodes[n];
				for (int num3 = 0; num3 < ropeNode.segmentLinks.Length; num3++)
				{
					CombineInstance combineInstance = default(CombineInstance);
					combineInstance.mesh = component2.sharedMesh;
					combineInstance.transform = ropeNode.segmentLinks[num3].transform.localToWorldMatrix;
					list.Add(combineInstance);
				}
			}
			GameObject gameObject2 = new GameObject(base.gameObject.name + " (static)");
			MeshFilter meshFilter2 = gameObject2.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer2 = gameObject2.AddComponent<MeshRenderer>();
			meshFilter2.sharedMesh = new Mesh();
			meshFilter2.sharedMesh.CombineMeshes(list.ToArray());
			meshRenderer2.sharedMaterials = array6;
			gameObject2.isStatic = true;
			Vector3[] vertices2 = meshFilter2.sharedMesh.vertices;
			Vector3 vector3 = Vector3.zero;
			for (int num4 = 0; num4 < meshFilter2.sharedMesh.vertexCount; num4++)
			{
				vertices2[num4] = base.transform.TransformPoint(vertices2[num4]);
				vector3 += vertices2[num4];
			}
			if (meshFilter2.sharedMesh.vertexCount > 1)
			{
				vector3 /= (float)meshFilter2.sharedMesh.vertexCount;
			}
			gameObject2.transform.position = vector3;
			gameObject2.transform.rotation = base.transform.rotation;
			for (int num5 = 0; num5 < meshFilter2.sharedMesh.vertexCount; num5++)
			{
				vertices2[num5] = gameObject2.transform.InverseTransformPoint(vertices2[num5]);
			}
			meshFilter2.sharedMesh.vertices = vertices2;
			meshFilter2.sharedMesh.RecalculateBounds();
			MeshCollider meshCollider2 = gameObject2.AddComponent<MeshCollider>();
			meshCollider2.sharedMesh = meshFilter2.sharedMesh;
			meshCollider2.convex = false;
			meshCollider2.material = this.RopePhysicsMaterial;
			base.gameObject.SetActive(false);
			strStatusMessage = "Rope converted succesfully";
			return gameObject2;
		}
		else
		{
			if (this.RopeType == UltimateRope.ERopeType.ImportBones)
			{
				strStatusMessage = "Error: ImportBones rope type not supported";
				return null;
			}
			strStatusMessage = "Error: Unknown rope type not supported";
			return null;
		}
	}

	// Token: 0x0600275B RID: 10075 RVA: 0x00135180 File Offset: 0x00133380
	public void MoveNodeUp(int nNode)
	{
		if (this.RopeNodes != null && nNode > 0 && nNode < this.RopeNodes.Count)
		{
			UltimateRope.RopeNode ropeNode = this.RopeNodes[nNode];
			this.RopeNodes[nNode] = this.RopeNodes[nNode - 1];
			this.RopeNodes[nNode - 1] = ropeNode;
		}
	}

	// Token: 0x0600275C RID: 10076 RVA: 0x001351E8 File Offset: 0x001333E8
	public void MoveNodeDown(int nNode)
	{
		if (this.RopeNodes != null && nNode >= 0 && nNode < this.RopeNodes.Count - 1)
		{
			UltimateRope.RopeNode ropeNode = this.RopeNodes[nNode];
			this.RopeNodes[nNode] = this.RopeNodes[nNode + 1];
			this.RopeNodes[nNode + 1] = ropeNode;
		}
	}

	// Token: 0x0600275D RID: 10077 RVA: 0x00019FA2 File Offset: 0x000181A2
	public void CreateNewNode(int nNode)
	{
		if (this.RopeNodes == null)
		{
			this.RopeNodes = new List<UltimateRope.RopeNode>();
		}
		this.RopeNodes.Insert(nNode + 1, new UltimateRope.RopeNode());
	}

	// Token: 0x0600275E RID: 10078 RVA: 0x00019FCD File Offset: 0x000181CD
	public void RemoveNode(int nNode)
	{
		if (this.RopeNodes == null)
		{
			return;
		}
		this.RopeNodes.RemoveAt(nNode);
	}

	// Token: 0x0600275F RID: 10079 RVA: 0x00019FE7 File Offset: 0x000181E7
	public bool FirstNodeIsCoil()
	{
		return this.RopeNodes != null && this.RopeNodes.Count > 0 && this.RopeNodes[0].bIsCoil;
	}

	// Token: 0x06002760 RID: 10080 RVA: 0x00135250 File Offset: 0x00133450
	private void CheckAddCoilNode()
	{
		if (this.RopeType == UltimateRope.ERopeType.Procedural && this.IsExtensible && this.HasACoil && this.CoilObject != null && this.RopeStart)
		{
			if (!this.RopeNodes[0].bIsCoil)
			{
				this.RopeNodes.Insert(0, new UltimateRope.RopeNode());
				if (this.CoilNumBones < 1)
				{
					this.CoilNumBones = 1;
				}
				this.RopeNodes[0].goNode = this.CoilObject;
				this.RopeNodes[0].fLength = this.ExtensibleLength;
				this.RopeNodes[0].fTotalLength = this.RopeNodes[0].fLength;
				this.RopeNodes[0].nNumLinks = this.CoilNumBones;
				this.RopeNodes[0].nTotalLinks = this.RopeNodes[0].nNumLinks;
				this.RopeNodes[0].eColliderType = UltimateRope.EColliderType.None;
				this.RopeNodes[0].nColliderSkip = 0;
				this.RopeNodes[0].bFold = true;
				this.RopeNodes[0].bIsCoil = true;
				this.m_afCoilBoneRadiuses = new float[this.RopeNodes[0].nTotalLinks];
				this.m_afCoilBoneAngles = new float[this.RopeNodes[0].nTotalLinks];
				this.m_afCoilBoneX = new float[this.RopeNodes[0].nTotalLinks];
			}
			this.m_nFirstNonCoilNode = 1;
		}
	}

	// Token: 0x06002761 RID: 10081 RVA: 0x0001A01E File Offset: 0x0001821E
	private void CheckDelCoilNode()
	{
		if (this.RopeNodes[0].bIsCoil)
		{
			this.RopeNodes.RemoveAt(0);
			this.m_afCoilBoneRadiuses = null;
			this.m_afCoilBoneAngles = null;
			this.m_afCoilBoneX = null;
		}
		this.m_nFirstNonCoilNode = 0;
	}

	// Token: 0x06002762 RID: 10082 RVA: 0x00135408 File Offset: 0x00133608
	private void CreateRopeJoints(bool bCheckIfBroken = false)
	{
		if (this.RopeNodes == null)
		{
			return;
		}
		if (this.RopeNodes.Count == 0)
		{
			return;
		}
		if (this.Deleted)
		{
			return;
		}
		if (this.RopeType == UltimateRope.ERopeType.ImportBones)
		{
			if (this.ImportedBones == null)
			{
				return;
			}
			if (this.ImportedBones.Length == 0)
			{
				return;
			}
		}
		foreach (UltimateRope.RopeNode ropeNode in this.RopeNodes)
		{
			if (ropeNode.segmentLinks == null)
			{
				return;
			}
		}
		if (this.RopeStart != null && this.RopeStart.rigidbody == null)
		{
			this.RopeStart.AddComponent<Rigidbody>();
			this.RopeStart.rigidbody.isKinematic = true;
		}
		for (int i = 0; i < this.RopeNodes.Count; i++)
		{
			UltimateRope.RopeNode ropeNode2 = this.RopeNodes[i];
			if (ropeNode2.goNode != null && ropeNode2.goNode.rigidbody == null)
			{
				ropeNode2.goNode.AddComponent<Rigidbody>();
				ropeNode2.goNode.rigidbody.isKinematic = true;
			}
		}
		int num = 0;
		int num2 = 0;
		Vector3[] array = new Vector3[this.TotalLinks];
		Quaternion[] array2 = new Quaternion[this.TotalLinks];
		Vector3 localPosition = (!(this.RopeStart != null)) ? Vector3.zero : this.RopeStart.transform.localPosition;
		Quaternion localRotation = (!(this.RopeStart != null)) ? Quaternion.identity : this.RopeStart.transform.localRotation;
		Vector3[] array3 = new Vector3[this.RopeNodes.Count];
		Quaternion[] array4 = new Quaternion[this.RopeNodes.Count];
		if (this.m_bRopeStartInitialOrientationInitialized && this.RopeStart != null)
		{
			this.RopeStart.transform.localPosition = this.m_v3InitialRopeStartLocalPos;
			this.RopeStart.transform.localRotation = this.m_qInitialRopeStartLocalRot;
		}
		for (int j = 0; j < this.RopeNodes.Count; j++)
		{
			UltimateRope.RopeNode ropeNode3 = this.RopeNodes[j];
			if (ropeNode3.bInitialOrientationInitialized && ropeNode3.goNode != null)
			{
				array3[j] = ropeNode3.goNode.transform.localPosition;
				array4[j] = ropeNode3.goNode.transform.localRotation;
				ropeNode3.goNode.transform.localPosition = ropeNode3.v3InitialLocalPos;
				ropeNode3.goNode.transform.localRotation = ropeNode3.qInitialLocalRot;
			}
		}
		for (int k = 0; k < this.RopeNodes.Count; k++)
		{
			UltimateRope.RopeNode ropeNode4 = this.RopeNodes[k];
			GameObject gameObject;
			GameObject gameObject2;
			if (this.FirstNodeIsCoil() && k == 0)
			{
				gameObject = this.CoilObject;
				gameObject2 = this.RopeStart;
			}
			else
			{
				gameObject = ((k != this.m_nFirstNonCoilNode) ? this.RopeNodes[k - 1].goNode : this.RopeStart);
				gameObject2 = this.RopeNodes[k].goNode;
			}
			float num3 = ropeNode4.fLength / (float)ropeNode4.nNumLinks;
			float num4 = num3 * (float)(ropeNode4.segmentLinks.Length - 1);
			for (int l = 0; l < ropeNode4.segmentLinks.Length; l++)
			{
				if (this.RopeType == UltimateRope.ERopeType.Procedural || this.RopeType == UltimateRope.ERopeType.LinkedObjects)
				{
					float num5 = (float)l / ((ropeNode4.segmentLinks.Length != 1) ? ((float)ropeNode4.segmentLinks.Length - 1f) : 1f);
					if (l == 0)
					{
						ropeNode4.m_v3LocalDirectionUp = gameObject.transform.InverseTransformDirection(ropeNode4.segmentLinks[l].transform.up);
					}
					Vector3 normalized = (gameObject2.transform.position - gameObject.transform.position).normalized;
					if (ropeNode4.nTotalLinks > ropeNode4.nNumLinks && !ropeNode4.m_bExtensionInitialized)
					{
						ropeNode4.segmentLinks[l].transform.rotation = Quaternion.LookRotation((gameObject2.transform.position - gameObject.transform.position).normalized);
						if (l < ropeNode4.m_nExtensionLinkIn)
						{
							ropeNode4.segmentLinks[l].transform.position = gameObject.transform.position;
							ropeNode4.segmentLinks[l].transform.parent = ((k <= this.m_nFirstNonCoilNode) ? this.RopeStart.transform : this.RopeNodes[k - 1].goNode.transform);
							ropeNode4.segmentLinks[l].rigidbody.isKinematic = true;
							UltimateRopeLink component = ropeNode4.segmentLinks[l].GetComponent<UltimateRopeLink>();
							if (component != null)
							{
								component.ExtensibleKinematic = true;
							}
						}
						else
						{
							float num6 = (float)(l - ropeNode4.m_nExtensionLinkIn) / ((ropeNode4.nNumLinks <= 1) ? 1f : ((float)(ropeNode4.nNumLinks - 1)));
							ropeNode4.segmentLinks[l].transform.position = Vector3.Lerp(gameObject.transform.position + normalized * num3, gameObject2.transform.position - normalized * num3, num6);
							ropeNode4.segmentLinks[l].rigidbody.isKinematic = false;
							UltimateRopeLink component2 = ropeNode4.segmentLinks[l].GetComponent<UltimateRopeLink>();
							if (component2 != null)
							{
								component2.ExtensibleKinematic = false;
							}
						}
					}
					array[num2] = ropeNode4.segmentLinks[l].transform.position;
					array2[num2] = ropeNode4.segmentLinks[l].transform.rotation;
					ropeNode4.segmentLinks[l].transform.position = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, num4), num5);
					ropeNode4.segmentLinks[l].transform.rotation = Quaternion.identity;
					if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
					{
						ropeNode4.segmentLinks[l].transform.rotation *= this.GetLinkedObjectLocalRotation(this.LinkTwistAngleStart + this.LinkTwistAngleIncrement * (float)l);
					}
					num2++;
				}
				else if (this.RopeType == UltimateRope.ERopeType.ImportBones)
				{
					array[l] = this.ImportedBones[l].goBone.transform.position;
					array2[l] = this.ImportedBones[l].goBone.transform.rotation;
					if (this.ImportedBones[l].tfNonBoneParent != null)
					{
						Transform parent = this.ImportedBones[l].goBone.transform.parent;
						this.ImportedBones[l].goBone.transform.parent = this.ImportedBones[l].tfNonBoneParent;
						this.ImportedBones[l].goBone.transform.localPosition = this.ImportedBones[l].v3OriginalLocalPos;
						this.ImportedBones[l].goBone.transform.localRotation = this.ImportedBones[l].qOriginalLocalRot;
						this.ImportedBones[l].goBone.transform.parent = parent;
						this.ImportedBones[l].goBone.transform.localScale = this.ImportedBones[l].v3OriginalLocalScale;
					}
				}
				bool flag = !bCheckIfBroken || !ropeNode4.linkJointBreaksProcessed[l];
				if (this.RopeType == UltimateRope.ERopeType.ImportBones)
				{
					bool flag2 = true;
					if (l > 0)
					{
						flag2 = !this.ImportedBones[l - 1].goBone.rigidbody.isKinematic;
					}
					if (!flag2 && this.ImportedBones[l].goBone.rigidbody.isKinematic)
					{
						flag = false;
					}
				}
				if (flag && l > 0 && !ropeNode4.bIsCoil)
				{
					ropeNode4.linkJoints[l] = this.CreateJoint(ropeNode4.segmentLinks[l], ropeNode4.segmentLinks[l - 1], ropeNode4.segmentLinks[l].transform.position);
					ropeNode4.linkJointBreaksProcessed[l] = false;
				}
				else
				{
					ropeNode4.linkJoints[l] = null;
				}
			}
			float num7 = (this.RopeType != UltimateRope.ERopeType.ImportBones) ? (((gameObject2.transform.position - gameObject.transform.position).magnitude - num3) / (gameObject2.transform.position - gameObject.transform.position).magnitude) : 0f;
			if (num7 < 0f)
			{
				num7 = 0f;
			}
			for (int m = 0; m < ropeNode4.segmentLinks.Length; m++)
			{
				if (this.RopeType == UltimateRope.ERopeType.Procedural || this.RopeType == UltimateRope.ERopeType.LinkedObjects)
				{
					float num8 = (float)m / ((ropeNode4.segmentLinks.Length != 1) ? ((float)ropeNode4.segmentLinks.Length - 1f) : 1f);
					if (Vector3.Distance(gameObject.transform.position, gameObject2.transform.position) < 0.001f)
					{
						ropeNode4.segmentLinks[m].transform.position = gameObject.transform.position;
						ropeNode4.segmentLinks[m].transform.rotation = gameObject.transform.rotation;
					}
					else
					{
						ropeNode4.segmentLinks[m].transform.position = Vector3.Lerp(gameObject.transform.position, gameObject2.transform.position, num8 * num7);
						ropeNode4.segmentLinks[m].transform.rotation = Quaternion.LookRotation((gameObject2.transform.position - gameObject.transform.position).normalized);
					}
					if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
					{
						ropeNode4.segmentLinks[m].transform.rotation *= this.GetLinkedObjectLocalRotation(this.LinkTwistAngleStart + this.LinkTwistAngleIncrement * (float)m);
					}
				}
				num++;
			}
			if (this.RopeType == UltimateRope.ERopeType.Procedural || this.RopeType == UltimateRope.ERopeType.LinkedObjects)
			{
				if (!ropeNode4.bIsCoil)
				{
					if (!bCheckIfBroken || !ropeNode4.linkJointBreaksProcessed[0])
					{
						if (ropeNode4.nTotalLinks == ropeNode4.nNumLinks)
						{
							ropeNode4.linkJoints[0] = this.CreateJoint(ropeNode4.segmentLinks[0], gameObject, gameObject.transform.position);
							ropeNode4.linkJointBreaksProcessed[0] = false;
						}
						else
						{
							ropeNode4.linkJoints[0] = null;
							ropeNode4.linkJointBreaksProcessed[0] = true;
						}
					}
					else
					{
						ropeNode4.linkJoints[0] = null;
					}
					if (!bCheckIfBroken || !ropeNode4.linkJointBreaksProcessed[ropeNode4.segmentLinks.Length])
					{
						ropeNode4.linkJoints[ropeNode4.segmentLinks.Length] = this.CreateJoint(ropeNode4.segmentLinks[ropeNode4.segmentLinks.Length - 1], gameObject2, gameObject2.transform.position);
						ropeNode4.linkJointBreaksProcessed[ropeNode4.segmentLinks.Length] = false;
					}
					else
					{
						ropeNode4.linkJoints[ropeNode4.segmentLinks.Length] = null;
					}
				}
			}
			else if (this.RopeType == UltimateRope.ERopeType.ImportBones)
			{
				ropeNode4.linkJointBreaksProcessed[0] = true;
			}
			if (ropeNode4.nTotalLinks > ropeNode4.nNumLinks && !ropeNode4.m_bExtensionInitialized)
			{
				ropeNode4.m_bExtensionInitialized = true;
			}
		}
		if (this.m_bRopeStartInitialOrientationInitialized && this.RopeStart != null)
		{
			this.RopeStart.transform.localPosition = localPosition;
			this.RopeStart.transform.localRotation = localRotation;
		}
		for (int n = 0; n < this.RopeNodes.Count; n++)
		{
			UltimateRope.RopeNode ropeNode5 = this.RopeNodes[n];
			if (ropeNode5.bInitialOrientationInitialized && ropeNode5.goNode != null)
			{
				ropeNode5.goNode.transform.localPosition = array3[n];
				ropeNode5.goNode.transform.localRotation = array4[n];
			}
		}
		num2 = 0;
		if (this.RopeType == UltimateRope.ERopeType.Procedural || this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			for (int num9 = 0; num9 < this.RopeNodes.Count; num9++)
			{
				UltimateRope.RopeNode ropeNode6 = this.RopeNodes[num9];
				for (int num10 = 0; num10 < ropeNode6.segmentLinks.Length; num10++)
				{
					ropeNode6.segmentLinks[num10].transform.position = array[num2];
					ropeNode6.segmentLinks[num10].transform.rotation = array2[num2];
					num2++;
				}
			}
		}
		else if (this.RopeType == UltimateRope.ERopeType.ImportBones)
		{
			for (int num11 = 0; num11 < this.ImportedBones.Length; num11++)
			{
				this.ImportedBones[num11].goBone.transform.position = array[num11];
				this.ImportedBones[num11].goBone.transform.rotation = array2[num11];
			}
		}
		this.CheckNeedsStartExitLockZ();
	}

	// Token: 0x06002763 RID: 10083 RVA: 0x001362CC File Offset: 0x001344CC
	private ConfigurableJoint CreateJoint(GameObject goObject, GameObject goConnectedTo, Vector3 v3Pivot)
	{
		ConfigurableJoint configurableJoint = goObject.AddComponent<ConfigurableJoint>();
		this.SetupJoint(configurableJoint);
		configurableJoint.connectedBody = goConnectedTo.rigidbody;
		configurableJoint.anchor = goObject.transform.InverseTransformPoint(v3Pivot);
		return configurableJoint;
	}

	// Token: 0x06002764 RID: 10084 RVA: 0x00136308 File Offset: 0x00134508
	private void SetupJoint(ConfigurableJoint joint)
	{
		SoftJointLimit softJointLimit = default(SoftJointLimit);
		softJointLimit.spring = 0f;
		softJointLimit.damper = 0f;
		softJointLimit.bounciness = 0f;
		JointDrive jointDrive = default(JointDrive);
		jointDrive.mode = 1;
		jointDrive.positionSpring = this.LinkJointSpringValue;
		jointDrive.positionDamper = this.LinkJointDamperValue;
		jointDrive.maximumForce = this.LinkJointMaxForceValue;
		joint.axis = Vector3.right;
		joint.secondaryAxis = Vector3.up;
		joint.breakForce = this.LinkJointBreakForce;
		joint.breakTorque = this.LinkJointBreakTorque;
		joint.xMotion = 0;
		joint.yMotion = 0;
		joint.zMotion = 0;
		joint.angularXMotion = (Mathf.Approximately(this.LinkJointAngularXLimit, 0f) ? 0 : 1);
		joint.angularYMotion = (Mathf.Approximately(this.LinkJointAngularYLimit, 0f) ? 0 : 1);
		joint.angularZMotion = (Mathf.Approximately(this.LinkJointAngularZLimit, 0f) ? 0 : 1);
		softJointLimit.limit = -this.LinkJointAngularXLimit;
		joint.lowAngularXLimit = softJointLimit;
		softJointLimit.limit = this.LinkJointAngularXLimit;
		joint.highAngularXLimit = softJointLimit;
		softJointLimit.limit = this.LinkJointAngularYLimit;
		joint.angularYLimit = softJointLimit;
		softJointLimit.limit = this.LinkJointAngularZLimit;
		joint.angularZLimit = softJointLimit;
		joint.angularXDrive = jointDrive;
		joint.angularYZDrive = jointDrive;
	}

	// Token: 0x06002765 RID: 10085 RVA: 0x00136480 File Offset: 0x00134680
	private Vector3 GetAxisVector(UltimateRope.EAxis eAxis, float fLength)
	{
		if (eAxis == UltimateRope.EAxis.X)
		{
			return new Vector3(fLength, 0f, 0f);
		}
		if (eAxis == UltimateRope.EAxis.Y)
		{
			return new Vector3(0f, fLength, 0f);
		}
		if (eAxis == UltimateRope.EAxis.Z)
		{
			return new Vector3(0f, 0f, fLength);
		}
		if (eAxis == UltimateRope.EAxis.MinusX)
		{
			return new Vector3(-fLength, 0f, 0f);
		}
		if (eAxis == UltimateRope.EAxis.MinusY)
		{
			return new Vector3(0f, -fLength, 0f);
		}
		if (eAxis == UltimateRope.EAxis.MinusZ)
		{
			return new Vector3(0f, 0f, -fLength);
		}
		return Vector3.zero;
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x00136524 File Offset: 0x00134724
	private float ExtendRopeLinear(float fLinearIncrement)
	{
		if (fLinearIncrement > 0f && Mathf.Approximately(this.m_fCurrentExtension, this.ExtensibleLength))
		{
			return 0f;
		}
		if (fLinearIncrement < 0f && Mathf.Approximately(this.m_fCurrentExtension, 0f))
		{
			return 0f;
		}
		UltimateRope.RopeNode ropeNode = this.RopeNodes[this.RopeNodes.Count - 1];
		bool flag = false;
		float fCurrentExtension = this.m_fCurrentExtension;
		float num = ropeNode.fLength / (float)ropeNode.nNumLinks;
		Transform transform = (this.RopeNodes.Count - 1 <= this.m_nFirstNonCoilNode) ? this.RopeStart.transform : this.RopeNodes[this.RopeNodes.Count - 2].goNode.transform;
		Vector3 vector = transform.TransformDirection(ropeNode.m_v3LocalDirectionForward);
		if (fLinearIncrement < 0f)
		{
			while (fLinearIncrement < 0f && ropeNode.m_nExtensionLinkIn > 0 && ropeNode.m_nExtensionLinkIn < ropeNode.segmentLinks.Length - 1 && !flag)
			{
				float num2 = Mathf.Max(-num * 0.5f, fLinearIncrement);
				if (Mathf.Abs(num2) > this.m_fCurrentExtension)
				{
					num2 = -this.m_fCurrentExtension;
					flag = true;
				}
				ropeNode.m_fExtensionRemainderIn += num2;
				if (ropeNode.m_fExtensionRemainderIn < -num)
				{
					num2 += Mathf.Abs(ropeNode.m_fExtensionRemainderIn - -num);
					ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].transform.position = ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn - 1].transform.position;
					ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].transform.rotation = ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn - 1].transform.rotation;
					if (!ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].rigidbody.isKinematic)
					{
						ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].rigidbody.isKinematic = true;
						UltimateRopeLink component = ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].GetComponent<UltimateRopeLink>();
						if (component != null)
						{
							component.ExtensibleKinematic = true;
						}
					}
					ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].transform.parent = transform;
					ropeNode.m_nExtensionLinkIn++;
					ropeNode.m_nExtensionLinkOut = ropeNode.m_nExtensionLinkIn - 1;
					ropeNode.m_fExtensionRemainderIn = 0f;
					ropeNode.m_fExtensionRemainderOut = 0f;
				}
				else
				{
					float num3 = -ropeNode.m_fExtensionRemainderIn / num;
					ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].transform.position = transform.position + vector * (num + ropeNode.m_fExtensionRemainderIn);
					ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].transform.rotation = Quaternion.Slerp(ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].transform.rotation, ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn - 1].transform.rotation, num3);
					if (!ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].rigidbody.isKinematic)
					{
						ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].rigidbody.isKinematic = true;
						UltimateRopeLink component2 = ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].GetComponent<UltimateRopeLink>();
						if (component2 != null)
						{
							component2.ExtensibleKinematic = true;
						}
					}
					ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].transform.parent = transform;
					ropeNode.m_nExtensionLinkOut = ropeNode.m_nExtensionLinkIn;
					ropeNode.m_fExtensionRemainderOut = num + ropeNode.m_fExtensionRemainderIn;
				}
				fLinearIncrement -= num2;
				this.m_fCurrentExtension += num2;
			}
		}
		else if (fLinearIncrement > 0f)
		{
			while (fLinearIncrement > 0f && ropeNode.m_nExtensionLinkOut > 0 && ropeNode.m_nExtensionLinkOut < ropeNode.segmentLinks.Length - 1 && !flag)
			{
				float num4 = Mathf.Min(num * 0.5f, fLinearIncrement);
				if (this.m_fCurrentExtension + num4 > this.ExtensibleLength)
				{
					num4 = this.ExtensibleLength - this.m_fCurrentExtension;
					flag = true;
				}
				ropeNode.m_fExtensionRemainderOut += num4;
				if (ropeNode.m_fExtensionRemainderOut > num)
				{
					num4 -= ropeNode.m_fExtensionRemainderOut - num;
					if (ropeNode.segmentLinks[ropeNode.m_nExtensionLinkOut].rigidbody.isKinematic)
					{
						ropeNode.segmentLinks[ropeNode.m_nExtensionLinkOut].rigidbody.isKinematic = false;
						UltimateRopeLink component3 = ropeNode.segmentLinks[ropeNode.m_nExtensionLinkIn].GetComponent<UltimateRopeLink>();
						if (component3 != null)
						{
							component3.ExtensibleKinematic = false;
						}
					}
					ropeNode.segmentLinks[ropeNode.m_nExtensionLinkOut].transform.parent = base.transform;
					ropeNode.m_nExtensionLinkOut--;
					ropeNode.m_nExtensionLinkIn = ropeNode.m_nExtensionLinkOut + 1;
					ropeNode.m_fExtensionRemainderIn = 0f;
					ropeNode.m_fExtensionRemainderOut = 0f;
				}
				else
				{
					ropeNode.segmentLinks[ropeNode.m_nExtensionLinkOut].transform.position = transform.position + vector * ropeNode.m_fExtensionRemainderOut;
					ropeNode.m_nExtensionLinkIn = ropeNode.m_nExtensionLinkOut;
					ropeNode.m_fExtensionRemainderIn = -num + ropeNode.m_fExtensionRemainderOut;
				}
				fLinearIncrement -= num4;
				this.m_fCurrentExtension += num4;
			}
		}
		return this.m_fCurrentExtension - fCurrentExtension;
	}

	// Token: 0x06002767 RID: 10087 RVA: 0x00136AA4 File Offset: 0x00134CA4
	private void SetupCoilBones(float fCoilLength)
	{
		float num = 0f;
		float num2 = this.CoilWidth * -0.5f + this.RopeDiameter * 0.5f;
		float num3 = this.CoilDiameter * 0.5f + this.RopeDiameter * 0.5f;
		float num4 = 0f;
		float num5 = 1f;
		float num6 = -1f;
		float num7 = Vector3.Distance(this.CoilObject.transform.position, this.RopeStart.transform.position) + this.CoilDiameter;
		float num8 = fCoilLength + num7;
		float num9 = 0f;
		int num10 = 0;
		UltimateRope.RopeNode ropeNode = this.RopeNodes[0];
		Vector3 localPosition = ropeNode.goNode.transform.localPosition;
		Quaternion localRotation = ropeNode.goNode.transform.localRotation;
		Vector3 localScale = ropeNode.goNode.transform.localScale;
		if (ropeNode.bInitialOrientationInitialized)
		{
			ropeNode.goNode.transform.localPosition = ropeNode.v3InitialLocalPos;
			ropeNode.goNode.transform.localRotation = ropeNode.qInitialLocalRot;
			ropeNode.goNode.transform.localScale = ropeNode.v3InitialLocalScale;
		}
		Vector3 vector = -this.CoilObject.transform.TransformDirection(this.GetAxisVector(this.CoilAxisRight, 1f));
		Vector3 vector2 = this.CoilObject.transform.TransformDirection(this.GetAxisVector(this.CoilAxisUp, 1f));
		Vector3 vector3 = Vector3.Cross(vector2, vector);
		Quaternion rotation = Quaternion.LookRotation(vector3, vector2);
		ropeNode.goNode.transform.localPosition = localPosition;
		ropeNode.goNode.transform.localRotation = localRotation;
		ropeNode.goNode.transform.localScale = localScale;
		float num11 = (this.RopeNodes[0].fLength + num7) / (float)this.RopeNodes[0].nNumLinks;
		for (int i = 0; i < this.RopeNodes[0].segmentLinks.Length; i++)
		{
			this.m_afCoilBoneRadiuses[i] = num3;
			this.m_afCoilBoneAngles[i] = num4;
			this.m_afCoilBoneX[i] = num2;
			Vector3 vector4 = this.CoilObject.transform.position + vector2 * num3 + vector * num2;
			float magnitude = (vector4 - this.RopeStart.transform.position).magnitude;
			num += num11;
			num10++;
			float num12 = num8 - magnitude;
			if (num > num12)
			{
				float num13 = num - num12;
				num9 = num4 - num13 / (num3 * 3.1415927f * 2f) * 360f;
				this.m_fCurrentCoilRopeRadius = num3;
				this.m_fCurrentCoilTurnsLeft = num4 / 360f;
				break;
			}
			float num14 = num11 / (num3 * 3.1415927f * 2f) * 360f;
			float num15 = num3 * 3.1415927f * 2f / num11;
			num4 += num14;
			if (num6 > 0f)
			{
				num3 += this.RopeDiameter / num15;
				num6 -= num14;
			}
			else
			{
				num2 += this.RopeDiameter * num5 / num15;
			}
			if (num5 > 0f && num2 > this.CoilWidth * 0.5f - this.RopeDiameter * 0.5f)
			{
				num2 = this.CoilWidth * 0.5f - this.RopeDiameter * 0.5f;
				num6 = 360f;
				num5 = -1f;
			}
			if (num5 < 0f && num2 < this.CoilWidth * -0.5f + this.RopeDiameter * 0.5f)
			{
				num2 = this.CoilWidth * -0.5f + this.RopeDiameter * 0.5f;
				num6 = 360f;
				num5 = 1f;
			}
		}
		for (int j = 0; j < num10; j++)
		{
			this.m_afCoilBoneAngles[j] -= num9;
			this.RopeNodes[0].segmentLinks[j].transform.position = this.CoilObject.transform.position + vector2 * this.m_afCoilBoneRadiuses[j];
			this.RopeNodes[0].segmentLinks[j].transform.rotation = rotation;
			this.RopeNodes[0].segmentLinks[j].transform.RotateAround(this.CoilObject.transform.position, -vector, this.m_afCoilBoneAngles[j]);
			this.RopeNodes[0].segmentLinks[j].transform.position += vector * this.m_afCoilBoneX[j];
		}
		Vector3 vector5 = this.CoilObject.transform.position + vector2 * num3 + vector * num2;
		Vector3 normalized = (this.RopeStart.transform.position - vector5).normalized;
		num = (this.RopeNodes[0].segmentLinks[num10 - 1].transform.position - vector5).magnitude;
		float magnitude2 = (this.RopeStart.transform.position - vector5).magnitude;
		Quaternion rotation2 = Quaternion.LookRotation((this.RopeStart.transform.position - this.CoilObject.transform.position).normalized, vector2);
		for (int k = num10; k < this.RopeNodes[0].segmentLinks.Length; k++)
		{
			num += num11;
			if (num < magnitude2)
			{
				this.RopeNodes[0].segmentLinks[k].transform.position = vector5 + normalized * num;
				this.RopeNodes[0].segmentLinks[k].transform.rotation = rotation2;
			}
			else
			{
				this.RopeNodes[0].segmentLinks[k].transform.position = this.RopeStart.transform.position;
				this.RopeNodes[0].segmentLinks[k].transform.rotation = rotation2;
			}
		}
		this.m_fCurrentCoilLength = fCoilLength;
	}

	// Token: 0x06002768 RID: 10088 RVA: 0x00137154 File Offset: 0x00135354
	private Quaternion GetLinkedObjectLocalRotation(float fTwistAngle = 0f)
	{
		if (this.LinkAxis == UltimateRope.EAxis.X)
		{
			return Quaternion.LookRotation(Vector3.right) * Quaternion.AngleAxis(fTwistAngle, Vector3.right);
		}
		if (this.LinkAxis == UltimateRope.EAxis.Y)
		{
			return Quaternion.LookRotation(Vector3.up) * Quaternion.AngleAxis(fTwistAngle, Vector3.up);
		}
		if (this.LinkAxis == UltimateRope.EAxis.Z)
		{
			return Quaternion.LookRotation(Vector3.forward) * Quaternion.AngleAxis(fTwistAngle, Vector3.forward);
		}
		if (this.LinkAxis == UltimateRope.EAxis.MinusX)
		{
			return Quaternion.LookRotation(-Vector3.right) * Quaternion.AngleAxis(fTwistAngle, -Vector3.right);
		}
		if (this.LinkAxis == UltimateRope.EAxis.MinusY)
		{
			return Quaternion.LookRotation(-Vector3.up) * Quaternion.AngleAxis(fTwistAngle, -Vector3.up);
		}
		if (this.LinkAxis == UltimateRope.EAxis.MinusZ)
		{
			return Quaternion.LookRotation(-Vector3.forward) * Quaternion.AngleAxis(fTwistAngle, -Vector3.forward);
		}
		return Quaternion.identity;
	}

	// Token: 0x06002769 RID: 10089 RVA: 0x00137270 File Offset: 0x00135470
	private float GetLinkedObjectScale(float fSegmentLength, int nNumLinks)
	{
		if (this.LinkObject == null)
		{
			return 0f;
		}
		MeshFilter component = this.LinkObject.GetComponent<MeshFilter>();
		if (component == null)
		{
			return 0f;
		}
		float num = 0f;
		if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			if (this.LinkAxis == UltimateRope.EAxis.X || this.LinkAxis == UltimateRope.EAxis.MinusX)
			{
				num = component.sharedMesh.bounds.size.x;
			}
			if (this.LinkAxis == UltimateRope.EAxis.Y || this.LinkAxis == UltimateRope.EAxis.MinusY)
			{
				num = component.sharedMesh.bounds.size.y;
			}
			if (this.LinkAxis == UltimateRope.EAxis.Z || this.LinkAxis == UltimateRope.EAxis.MinusZ)
			{
				num = component.sharedMesh.bounds.size.z;
			}
		}
		float num2 = fSegmentLength / (float)nNumLinks - this.LinkOffsetObject * (fSegmentLength / (float)(nNumLinks - 1));
		return num2 / num;
	}

	// Token: 0x0600276A RID: 10090 RVA: 0x0013737C File Offset: 0x0013557C
	private float GetLinkDiameter()
	{
		if (this.RopeType == UltimateRope.ERopeType.Procedural)
		{
			return this.RopeDiameter;
		}
		if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			if (this.LinkObject == null)
			{
				return 0f;
			}
			MeshFilter component = this.LinkObject.GetComponent<MeshFilter>();
			if (component == null)
			{
				return 0f;
			}
			float result = 0f;
			if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
			{
				if (this.LinkAxis == UltimateRope.EAxis.X || this.LinkAxis == UltimateRope.EAxis.MinusX)
				{
					result = Mathf.Max(component.sharedMesh.bounds.size.y, component.sharedMesh.bounds.size.z);
				}
				if (this.LinkAxis == UltimateRope.EAxis.Y || this.LinkAxis == UltimateRope.EAxis.MinusY)
				{
					result = Mathf.Max(component.sharedMesh.bounds.size.x, component.sharedMesh.bounds.size.z);
				}
				if (this.LinkAxis == UltimateRope.EAxis.Z || this.LinkAxis == UltimateRope.EAxis.MinusZ)
				{
					result = Mathf.Max(component.sharedMesh.bounds.size.x, component.sharedMesh.bounds.size.y);
				}
			}
			return result;
		}
		else
		{
			if (this.RopeType == UltimateRope.ERopeType.ImportBones)
			{
				return this.BoneColliderDiameter;
			}
			return 0f;
		}
	}

	// Token: 0x0600276B RID: 10091 RVA: 0x00137510 File Offset: 0x00135710
	private Vector3 GetLinkAxisOffset(float fValue)
	{
		UltimateRope.EAxis eaxis = UltimateRope.EAxis.Z;
		if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			eaxis = this.LinkAxis;
		}
		if (this.RopeType == UltimateRope.ERopeType.ImportBones)
		{
			eaxis = this.BoneAxis;
		}
		if (eaxis == UltimateRope.EAxis.X)
		{
			return new Vector3(fValue, 0f, 0f);
		}
		if (eaxis == UltimateRope.EAxis.Y)
		{
			return new Vector3(0f, fValue, 0f);
		}
		if (eaxis == UltimateRope.EAxis.Z)
		{
			return new Vector3(0f, 0f, fValue);
		}
		if (eaxis == UltimateRope.EAxis.MinusX)
		{
			return new Vector3(-fValue, 0f, 0f);
		}
		if (eaxis == UltimateRope.EAxis.MinusY)
		{
			return new Vector3(0f, -fValue, 0f);
		}
		if (eaxis == UltimateRope.EAxis.MinusZ)
		{
			return new Vector3(0f, 0f, -fValue);
		}
		return new Vector3(0f, 0f, fValue);
	}

	// Token: 0x0600276C RID: 10092 RVA: 0x001375E8 File Offset: 0x001357E8
	private int GetLinkAxisIndex()
	{
		UltimateRope.EAxis eaxis = UltimateRope.EAxis.Z;
		if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			eaxis = this.LinkAxis;
		}
		if (this.RopeType == UltimateRope.ERopeType.ImportBones)
		{
			eaxis = this.BoneAxis;
		}
		if (eaxis == UltimateRope.EAxis.X)
		{
			return 0;
		}
		if (eaxis == UltimateRope.EAxis.Y)
		{
			return 1;
		}
		if (eaxis == UltimateRope.EAxis.Z)
		{
			return 2;
		}
		if (eaxis == UltimateRope.EAxis.MinusX)
		{
			return 0;
		}
		if (eaxis == UltimateRope.EAxis.MinusY)
		{
			return 1;
		}
		if (eaxis == UltimateRope.EAxis.MinusZ)
		{
			return 2;
		}
		return 2;
	}

	// Token: 0x0600276D RID: 10093 RVA: 0x00137654 File Offset: 0x00135854
	private bool GetLinkBoxColliderCenterAndSize(float fLinkLength, float fRopeDiameter, ref Vector3 v3CenterInOut, ref Vector3 v3SizeInOut)
	{
		if (this.RopeType == UltimateRope.ERopeType.Procedural)
		{
			v3CenterInOut = Vector3.zero;
			v3SizeInOut..ctor(fRopeDiameter, fRopeDiameter, fLinkLength);
			return true;
		}
		if (this.RopeType == UltimateRope.ERopeType.LinkedObjects)
		{
			MeshFilter component = this.LinkObject.GetComponent<MeshFilter>();
			if (component == null)
			{
				return false;
			}
			v3CenterInOut = component.sharedMesh.bounds.center;
			v3SizeInOut = component.sharedMesh.bounds.size;
			return true;
		}
		else
		{
			if (this.RopeType == UltimateRope.ERopeType.ImportBones)
			{
				if (this.BoneAxis == UltimateRope.EAxis.X)
				{
					v3SizeInOut..ctor(fLinkLength, fRopeDiameter, fRopeDiameter);
				}
				if (this.BoneAxis == UltimateRope.EAxis.Y)
				{
					v3SizeInOut..ctor(fRopeDiameter, fLinkLength, fRopeDiameter);
				}
				if (this.BoneAxis == UltimateRope.EAxis.Z)
				{
					v3SizeInOut..ctor(fRopeDiameter, fRopeDiameter, fLinkLength);
				}
				if (this.BoneAxis == UltimateRope.EAxis.MinusX)
				{
					v3SizeInOut..ctor(fLinkLength, fRopeDiameter, fRopeDiameter);
				}
				if (this.BoneAxis == UltimateRope.EAxis.MinusY)
				{
					v3SizeInOut..ctor(fRopeDiameter, fLinkLength, fRopeDiameter);
				}
				if (this.BoneAxis == UltimateRope.EAxis.MinusZ)
				{
					v3SizeInOut..ctor(fRopeDiameter, fRopeDiameter, fLinkLength);
				}
				return true;
			}
			v3CenterInOut = Vector3.zero;
			v3SizeInOut..ctor(fRopeDiameter, fRopeDiameter, fLinkLength);
			return true;
		}
	}

	// Token: 0x0600276E RID: 10094 RVA: 0x00137788 File Offset: 0x00135988
	private bool BuildImportedBoneList(GameObject goBoneFirst, GameObject goBoneLast, List<int> ListImportBonesStatic, List<int> ListImportBonesNoCollider, out List<UltimateRope.RopeBone> outListImportedBones, out string strErrorMessage)
	{
		strErrorMessage = string.Empty;
		outListImportedBones = new List<UltimateRope.RopeBone>();
		int num = 0;
		int num2 = 0;
		for (int i = goBoneFirst.name.Length - 1; i >= 0; i--)
		{
			if (!char.IsDigit(goBoneFirst.name.get_Chars(i)))
			{
				break;
			}
			num++;
		}
		if (num == 0)
		{
			strErrorMessage = "First bone name needs to end with digits in order to infer bone sequence";
			return false;
		}
		int nIndexFirst = int.Parse(goBoneFirst.name.Substring(goBoneFirst.name.Length - num));
		for (int j = goBoneLast.name.Length - 1; j >= 0; j--)
		{
			if (!char.IsDigit(goBoneLast.name.get_Chars(j)))
			{
				break;
			}
			num2++;
		}
		if (num2 == 0)
		{
			strErrorMessage = "Last bone name needs to end with digits in order to infer bone sequence";
			return false;
		}
		int nIndexLast = int.Parse(goBoneLast.name.Substring(goBoneLast.name.Length - num2));
		string text = goBoneFirst.name.Substring(0, goBoneFirst.name.Length - num);
		string text2 = goBoneLast.name.Substring(0, goBoneLast.name.Length - num2);
		if (text != text2)
		{
			strErrorMessage = string.Format("First bone name prefix ({0}) and last bone name prefix ({1}) don't match", text, text2);
			return false;
		}
		if (this.BoneFirst.transform.parent == null || this.BoneLast.transform.parent == null)
		{
			strErrorMessage = string.Format("First and last bones need to share a common parent object", new object[0]);
			return false;
		}
		GameObject gameObject = (!this.BoneLast.transform.IsChildOf(this.BoneFirst.transform)) ? this.BoneLast.transform.parent.gameObject : this.BoneFirst.transform.parent.gameObject;
		if (this.BuildImportedBoneListTry(gameObject, text, nIndexFirst, nIndexLast, num, num2, ListImportBonesStatic, ListImportBonesNoCollider, out outListImportedBones, ref strErrorMessage))
		{
			return true;
		}
		gameObject = gameObject.transform.root.gameObject;
		string text3 = string.Format("Try1: {0}\nTry2: ", strErrorMessage);
		if (this.BuildImportedBoneListTry(gameObject, text, nIndexFirst, nIndexLast, num, num2, ListImportBonesStatic, ListImportBonesNoCollider, out outListImportedBones, ref strErrorMessage))
		{
			return true;
		}
		strErrorMessage = text3 + strErrorMessage;
		return false;
	}

	// Token: 0x0600276F RID: 10095 RVA: 0x001379F4 File Offset: 0x00135BF4
	private bool BuildImportedBoneListTry(GameObject goRoot, string strPrefix, int nIndexFirst, int nIndexLast, int nDigitsFirst, int nDigitsLast, List<int> ListImportBonesStatic, List<int> ListImportBonesNoCollider, out List<UltimateRope.RopeBone> outListImportedBones, ref string strErrorMessage)
	{
		outListImportedBones = new List<UltimateRope.RopeBone>();
		Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
		if (!this.BuildBoneHashString2GameObject(goRoot, goRoot, ref dictionary, ref strErrorMessage))
		{
			return false;
		}
		Dictionary<GameObject, Transform> dictionary2 = new Dictionary<GameObject, Transform>();
		int num = (nIndexFirst > nIndexLast) ? -1 : 1;
		int num2 = nIndexFirst;
		while ((num != 1) ? (num2 >= nIndexLast) : (num2 <= nIndexLast))
		{
			bool flag = false;
			for (int i = nDigitsFirst; i <= nDigitsLast; i++)
			{
				string text = strPrefix + num2.ToString("D" + i);
				if (dictionary.ContainsKey(text))
				{
					UltimateRope.RopeBone ropeBone = new UltimateRope.RopeBone();
					ropeBone.goBone = dictionary[text];
					ropeBone.tfParent = ropeBone.goBone.transform.parent;
					ropeBone.bCreatedCollider = !ListImportBonesNoCollider.Contains(num2);
					ropeBone.bIsStatic = ListImportBonesStatic.Contains(num2);
					ropeBone.nOriginalLayer = ropeBone.goBone.layer;
					outListImportedBones.Add(ropeBone);
					dictionary2.Add(ropeBone.goBone, ropeBone.goBone.transform);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				strErrorMessage = string.Format("Bone not found (bone number suffix {0}, trying to find below node {1}'s hierarchy)", num2, goRoot.name);
				return false;
			}
			num2 += num;
		}
		foreach (UltimateRope.RopeBone ropeBone2 in outListImportedBones)
		{
			Transform transform = ropeBone2.goBone.transform.parent;
			while (transform != null)
			{
				if (!dictionary2.ContainsKey(transform.gameObject))
				{
					break;
				}
				transform = transform.parent;
			}
			if (transform == null)
			{
				transform = goRoot.transform;
			}
			dictionary2[ropeBone2.goBone] = transform;
		}
		foreach (UltimateRope.RopeBone ropeBone3 in outListImportedBones)
		{
			Transform transform2 = dictionary2[ropeBone3.goBone];
			GameObject gameObject = new GameObject();
			ropeBone3.v3OriginalLocalScale = ropeBone3.goBone.transform.localScale;
			gameObject.transform.position = ropeBone3.goBone.transform.position;
			gameObject.transform.rotation = ropeBone3.goBone.transform.rotation;
			gameObject.transform.parent = transform2.transform;
			ropeBone3.v3OriginalLocalPos = gameObject.transform.localPosition;
			ropeBone3.qOriginalLocalRot = gameObject.transform.localRotation;
			ropeBone3.tfNonBoneParent = transform2;
			Object.DestroyImmediate(gameObject);
			if (ropeBone3.bIsStatic)
			{
				ropeBone3.goBone.transform.parent = transform2;
			}
			else
			{
				ropeBone3.goBone.transform.parent = base.transform;
			}
		}
		return true;
	}

	// Token: 0x06002770 RID: 10096 RVA: 0x00137D58 File Offset: 0x00135F58
	private bool BuildBoneHashString2GameObject(GameObject goRoot, GameObject goCurrent, ref Dictionary<string, GameObject> outHashString2GameObjects, ref string strErrorMessage)
	{
		for (int i = 0; i < goCurrent.transform.GetChildCount(); i++)
		{
			GameObject gameObject = goCurrent.transform.GetChild(i).gameObject;
			if (!this.BuildBoneHashString2GameObject(goRoot, gameObject, ref outHashString2GameObjects, ref strErrorMessage))
			{
				return false;
			}
		}
		if (outHashString2GameObjects.ContainsKey(goCurrent.name))
		{
			strErrorMessage = string.Format("Bone name {0} is found more than once in GameObject {1}'s hierarchy. The name must be unique.", goCurrent.name, goRoot.name);
			return false;
		}
		outHashString2GameObjects.Add(goCurrent.name, goCurrent);
		return true;
	}

	// Token: 0x06002771 RID: 10097 RVA: 0x00137DE4 File Offset: 0x00135FE4
	private bool ParseBoneIndices(string strBoneList, out List<int> outListBoneIndices, out string strErrorMessage)
	{
		outListBoneIndices = new List<int>();
		strErrorMessage = string.Empty;
		if (strBoneList.Length == 0)
		{
			return true;
		}
		string[] array = strBoneList.Split(new char[]
		{
			','
		});
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[]
			{
				'-'
			});
			if (array2.Length == 1)
			{
				int num = 0;
				try
				{
					num = int.Parse(array2[0]);
				}
				catch
				{
					strErrorMessage = string.Concat(new object[]
					{
						"Field ",
						i + 1,
						" is invalid (error parsing number: ",
						array2[0],
						")"
					});
					return false;
				}
				outListBoneIndices.Add(num);
			}
			else
			{
				if (array2.Length != 2)
				{
					strErrorMessage = string.Concat(new object[]
					{
						"Field ",
						i + 1,
						" has invalid range (field content: ",
						array[i],
						")"
					});
					return false;
				}
				int num2 = 0;
				int num3 = 0;
				try
				{
					num2 = int.Parse(array2[0]);
				}
				catch
				{
					strErrorMessage = string.Concat(new object[]
					{
						"Field ",
						i + 1,
						" is invalid (error parsing range start: ",
						array2[0],
						")"
					});
					return false;
				}
				try
				{
					num3 = int.Parse(array2[1]);
				}
				catch
				{
					strErrorMessage = string.Concat(new object[]
					{
						"Field ",
						i + 1,
						" is invalid (error parsing range end: ",
						array2[1],
						")"
					});
					return false;
				}
				if (num3 < num2)
				{
					strErrorMessage = string.Concat(new object[]
					{
						"Field ",
						i + 1,
						" has invalid range (",
						num2,
						" is greater than ",
						num3,
						")"
					});
					return false;
				}
				for (int j = num2; j <= num3; j++)
				{
					outListBoneIndices.Add(j);
				}
			}
		}
		outListBoneIndices.Sort();
		List<int> list = new List<int>();
		int num4 = -1;
		foreach (int num5 in outListBoneIndices)
		{
			if (num5 != num4)
			{
				num4 = num5;
				list.Add(num5);
			}
		}
		outListBoneIndices = list;
		return true;
	}

	// Token: 0x06002772 RID: 10098 RVA: 0x0001A05E File Offset: 0x0001825E
	private void CheckLoadPersistentData()
	{
		if (Application.isEditor && RopePersistManager.PersistentDataExists(this))
		{
			RopePersistManager.RetrievePersistentData(this);
			RopePersistManager.RemovePersistentData(this);
		}
	}

	// Token: 0x06002773 RID: 10099 RVA: 0x0001A081 File Offset: 0x00018281
	private void CheckSavePersistentData()
	{
		if (Application.isEditor && this.PersistAfterPlayMode && !this.m_bLastStatusIsError)
		{
			RopePersistManager.StorePersistentData(this);
		}
	}

	// Token: 0x04003081 RID: 12417
	[RopePersist]
	public UltimateRope.ERopeType RopeType;

	// Token: 0x04003082 RID: 12418
	[RopePersist]
	public GameObject RopeStart;

	// Token: 0x04003083 RID: 12419
	[RopePersist]
	public List<UltimateRope.RopeNode> RopeNodes;

	// Token: 0x04003084 RID: 12420
	[RopePersist]
	public int RopeLayer;

	// Token: 0x04003085 RID: 12421
	[RopePersist]
	public PhysicMaterial RopePhysicsMaterial;

	// Token: 0x04003086 RID: 12422
	[RopePersist]
	public float RopeDiameter = 0.1f;

	// Token: 0x04003087 RID: 12423
	[RopePersist]
	public int RopeSegmentSides = 8;

	// Token: 0x04003088 RID: 12424
	[RopePersist]
	public Material RopeMaterial;

	// Token: 0x04003089 RID: 12425
	[RopePersist]
	public float RopeTextureTileMeters = 1f;

	// Token: 0x0400308A RID: 12426
	[RopePersist]
	public Material RopeSectionMaterial;

	// Token: 0x0400308B RID: 12427
	[RopePersist]
	public float RopeTextureSectionTileMeters = 1f;

	// Token: 0x0400308C RID: 12428
	[RopePersist]
	public bool IsExtensible;

	// Token: 0x0400308D RID: 12429
	[RopePersist]
	public float ExtensibleLength = 10f;

	// Token: 0x0400308E RID: 12430
	[RopePersist]
	public bool HasACoil;

	// Token: 0x0400308F RID: 12431
	[RopePersist]
	public GameObject CoilObject;

	// Token: 0x04003090 RID: 12432
	[RopePersist]
	public UltimateRope.EAxis CoilAxisRight = UltimateRope.EAxis.X;

	// Token: 0x04003091 RID: 12433
	[RopePersist]
	public UltimateRope.EAxis CoilAxisUp = UltimateRope.EAxis.Y;

	// Token: 0x04003092 RID: 12434
	[RopePersist]
	public float CoilWidth = 0.5f;

	// Token: 0x04003093 RID: 12435
	[RopePersist]
	public float CoilDiameter = 0.5f;

	// Token: 0x04003094 RID: 12436
	[RopePersist]
	public int CoilNumBones = 50;

	// Token: 0x04003095 RID: 12437
	[RopePersist]
	public GameObject LinkObject;

	// Token: 0x04003096 RID: 12438
	[RopePersist]
	public UltimateRope.EAxis LinkAxis = UltimateRope.EAxis.Z;

	// Token: 0x04003097 RID: 12439
	[RopePersist]
	public float LinkOffsetObject;

	// Token: 0x04003098 RID: 12440
	[RopePersist]
	public float LinkTwistAngleStart;

	// Token: 0x04003099 RID: 12441
	[RopePersist]
	public float LinkTwistAngleIncrement;

	// Token: 0x0400309A RID: 12442
	[RopePersist]
	public GameObject BoneFirst;

	// Token: 0x0400309B RID: 12443
	[RopePersist]
	public GameObject BoneLast;

	// Token: 0x0400309C RID: 12444
	[RopePersist]
	public string BoneListNamesStatic;

	// Token: 0x0400309D RID: 12445
	[RopePersist]
	public string BoneListNamesNoColliders;

	// Token: 0x0400309E RID: 12446
	[RopePersist]
	public UltimateRope.EAxis BoneAxis = UltimateRope.EAxis.Z;

	// Token: 0x0400309F RID: 12447
	[RopePersist]
	public UltimateRope.EColliderType BoneColliderType = UltimateRope.EColliderType.Capsule;

	// Token: 0x040030A0 RID: 12448
	[RopePersist]
	public float BoneColliderDiameter = 0.1f;

	// Token: 0x040030A1 RID: 12449
	[RopePersist]
	public int BoneColliderSkip;

	// Token: 0x040030A2 RID: 12450
	[RopePersist]
	public float BoneColliderLength = 1f;

	// Token: 0x040030A3 RID: 12451
	[RopePersist]
	public float BoneColliderOffset;

	// Token: 0x040030A4 RID: 12452
	[RopePersist]
	public float LinkMass = 1f;

	// Token: 0x040030A5 RID: 12453
	[RopePersist]
	public int LinkSolverIterationCount = 100;

	// Token: 0x040030A6 RID: 12454
	[RopePersist]
	public float LinkJointAngularXLimit = 30f;

	// Token: 0x040030A7 RID: 12455
	[RopePersist]
	public float LinkJointAngularYLimit = 30f;

	// Token: 0x040030A8 RID: 12456
	[RopePersist]
	public float LinkJointAngularZLimit = 30f;

	// Token: 0x040030A9 RID: 12457
	[RopePersist]
	public float LinkJointSpringValue = 1f;

	// Token: 0x040030AA RID: 12458
	[RopePersist]
	public float LinkJointDamperValue;

	// Token: 0x040030AB RID: 12459
	[RopePersist]
	public float LinkJointMaxForceValue = 1f;

	// Token: 0x040030AC RID: 12460
	[RopePersist]
	public float LinkJointBreakForce = float.PositiveInfinity;

	// Token: 0x040030AD RID: 12461
	[RopePersist]
	public float LinkJointBreakTorque = float.PositiveInfinity;

	// Token: 0x040030AE RID: 12462
	[RopePersist]
	public bool LockStartEndInZAxis;

	// Token: 0x040030AF RID: 12463
	[RopePersist]
	public bool SendEvents;

	// Token: 0x040030B0 RID: 12464
	[RopePersist]
	public GameObject EventsObjectReceiver;

	// Token: 0x040030B1 RID: 12465
	[RopePersist]
	public string OnBreakMethodName;

	// Token: 0x040030B2 RID: 12466
	[RopePersist]
	public bool PersistAfterPlayMode;

	// Token: 0x040030B3 RID: 12467
	[RopePersist]
	public bool AutoRegenerate = true;

	// Token: 0x040030B4 RID: 12468
	[HideInInspector]
	[RopePersist]
	public bool Deleted = true;

	// Token: 0x040030B5 RID: 12469
	[RopePersist]
	[HideInInspector]
	public float[] LinkLengths;

	// Token: 0x040030B6 RID: 12470
	[RopePersist]
	[HideInInspector]
	public int TotalLinks;

	// Token: 0x040030B7 RID: 12471
	[RopePersist]
	[HideInInspector]
	public float TotalRopeLength;

	// Token: 0x040030B8 RID: 12472
	[RopePersist]
	[HideInInspector]
	public bool m_bRopeStartInitialOrientationInitialized;

	// Token: 0x040030B9 RID: 12473
	[HideInInspector]
	[RopePersist]
	public Vector3 m_v3InitialRopeStartLocalPos;

	// Token: 0x040030BA RID: 12474
	[RopePersist]
	[HideInInspector]
	public Quaternion m_qInitialRopeStartLocalRot;

	// Token: 0x040030BB RID: 12475
	[RopePersist]
	[HideInInspector]
	public Vector3 m_v3InitialRopeStartLocalScale;

	// Token: 0x040030BC RID: 12476
	[RopePersist]
	[HideInInspector]
	public int m_nFirstNonCoilNode;

	// Token: 0x040030BD RID: 12477
	[RopePersist]
	[HideInInspector]
	public float[] m_afCoilBoneRadiuses;

	// Token: 0x040030BE RID: 12478
	[HideInInspector]
	[RopePersist]
	public float[] m_afCoilBoneAngles;

	// Token: 0x040030BF RID: 12479
	[HideInInspector]
	[RopePersist]
	public float[] m_afCoilBoneX;

	// Token: 0x040030C0 RID: 12480
	[HideInInspector]
	[RopePersist]
	public float m_fCurrentCoilRopeRadius;

	// Token: 0x040030C1 RID: 12481
	[HideInInspector]
	[RopePersist]
	public float m_fCurrentCoilTurnsLeft;

	// Token: 0x040030C2 RID: 12482
	[RopePersist]
	[HideInInspector]
	public float m_fCurrentCoilLength;

	// Token: 0x040030C3 RID: 12483
	[RopePersist]
	[HideInInspector]
	public float m_fCurrentExtension;

	// Token: 0x040030C4 RID: 12484
	[RopePersist]
	[HideInInspector]
	public float m_fCurrentExtensionInput;

	// Token: 0x040030C5 RID: 12485
	[HideInInspector]
	[RopePersist]
	public UltimateRope.RopeBone[] ImportedBones;

	// Token: 0x040030C6 RID: 12486
	[RopePersist]
	[HideInInspector]
	public bool m_bBonesAreImported;

	// Token: 0x040030C7 RID: 12487
	[RopePersist]
	[HideInInspector]
	public string m_strStatus;

	// Token: 0x040030C8 RID: 12488
	[HideInInspector]
	[RopePersist]
	public bool m_bLastStatusIsError = true;

	// Token: 0x0200063A RID: 1594
	public enum ERopeType
	{
		// Token: 0x040030CA RID: 12490
		Procedural,
		// Token: 0x040030CB RID: 12491
		LinkedObjects,
		// Token: 0x040030CC RID: 12492
		ImportBones
	}

	// Token: 0x0200063B RID: 1595
	public enum EAxis
	{
		// Token: 0x040030CE RID: 12494
		MinusX,
		// Token: 0x040030CF RID: 12495
		MinusY,
		// Token: 0x040030D0 RID: 12496
		MinusZ,
		// Token: 0x040030D1 RID: 12497
		X,
		// Token: 0x040030D2 RID: 12498
		Y,
		// Token: 0x040030D3 RID: 12499
		Z
	}

	// Token: 0x0200063C RID: 1596
	public enum EColliderType
	{
		// Token: 0x040030D5 RID: 12501
		None,
		// Token: 0x040030D6 RID: 12502
		Capsule,
		// Token: 0x040030D7 RID: 12503
		Box
	}

	// Token: 0x0200063D RID: 1597
	public enum ERopeExtensionMode
	{
		// Token: 0x040030D9 RID: 12505
		CoilRotationIncrement,
		// Token: 0x040030DA RID: 12506
		LinearExtensionIncrement
	}

	// Token: 0x0200063E RID: 1598
	[Serializable]
	public class RopeNode
	{
		// Token: 0x06002774 RID: 10100 RVA: 0x001380B4 File Offset: 0x001362B4
		public RopeNode()
		{
			this.goNode = null;
			this.fLength = 5f;
			this.fTotalLength = this.fLength;
			this.nNumLinks = 20;
			this.nTotalLinks = this.nNumLinks;
			this.eColliderType = UltimateRope.EColliderType.Capsule;
			this.nColliderSkip = 1;
			this.bFold = true;
			this.bIsCoil = false;
			this.bInitialOrientationInitialized = false;
			this.linkJoints = new ConfigurableJoint[0];
			this.linkJointBreaksProcessed = new bool[0];
			this.bSegmentBroken = false;
		}

		// Token: 0x040030DB RID: 12507
		public GameObject goNode;

		// Token: 0x040030DC RID: 12508
		public float fLength;

		// Token: 0x040030DD RID: 12509
		public float fTotalLength;

		// Token: 0x040030DE RID: 12510
		public int nNumLinks;

		// Token: 0x040030DF RID: 12511
		public int nTotalLinks;

		// Token: 0x040030E0 RID: 12512
		public UltimateRope.EColliderType eColliderType;

		// Token: 0x040030E1 RID: 12513
		public int nColliderSkip;

		// Token: 0x040030E2 RID: 12514
		public bool bFold;

		// Token: 0x040030E3 RID: 12515
		public bool bIsCoil;

		// Token: 0x040030E4 RID: 12516
		public bool bInitialOrientationInitialized;

		// Token: 0x040030E5 RID: 12517
		public Vector3 v3InitialLocalPos;

		// Token: 0x040030E6 RID: 12518
		public Quaternion qInitialLocalRot;

		// Token: 0x040030E7 RID: 12519
		public Vector3 v3InitialLocalScale;

		// Token: 0x040030E8 RID: 12520
		public bool m_bExtensionInitialized;

		// Token: 0x040030E9 RID: 12521
		public int m_nExtensionLinkIn;

		// Token: 0x040030EA RID: 12522
		public int m_nExtensionLinkOut;

		// Token: 0x040030EB RID: 12523
		public float m_fExtensionRemainingLength;

		// Token: 0x040030EC RID: 12524
		public float m_fExtensionRemainderIn;

		// Token: 0x040030ED RID: 12525
		public float m_fExtensionRemainderOut;

		// Token: 0x040030EE RID: 12526
		public Vector3 m_v3LocalDirectionForward;

		// Token: 0x040030EF RID: 12527
		public Vector3 m_v3LocalDirectionUp;

		// Token: 0x040030F0 RID: 12528
		public GameObject[] segmentLinks;

		// Token: 0x040030F1 RID: 12529
		public ConfigurableJoint[] linkJoints;

		// Token: 0x040030F2 RID: 12530
		public bool[] linkJointBreaksProcessed;

		// Token: 0x040030F3 RID: 12531
		public bool bSegmentBroken;
	}

	// Token: 0x0200063F RID: 1599
	[Serializable]
	public class RopeBone
	{
		// Token: 0x06002775 RID: 10101 RVA: 0x0013813C File Offset: 0x0013633C
		public RopeBone()
		{
			this.goBone = null;
			this.tfParent = null;
			this.tfNonBoneParent = null;
			this.bCreatedCollider = false;
			this.bIsStatic = false;
			this.fLength = 0f;
			this.bCreatedRigidbody = false;
			this.nOriginalLayer = 0;
		}

		// Token: 0x040030F4 RID: 12532
		public GameObject goBone;

		// Token: 0x040030F5 RID: 12533
		public Transform tfParent;

		// Token: 0x040030F6 RID: 12534
		public Transform tfNonBoneParent;

		// Token: 0x040030F7 RID: 12535
		public bool bCreatedCollider;

		// Token: 0x040030F8 RID: 12536
		public bool bIsStatic;

		// Token: 0x040030F9 RID: 12537
		public float fLength;

		// Token: 0x040030FA RID: 12538
		public bool bCreatedRigidbody;

		// Token: 0x040030FB RID: 12539
		public int nOriginalLayer;

		// Token: 0x040030FC RID: 12540
		public Vector3 v3OriginalLocalScale;

		// Token: 0x040030FD RID: 12541
		public Vector3 v3OriginalLocalPos;

		// Token: 0x040030FE RID: 12542
		public Quaternion qOriginalLocalRot;
	}

	// Token: 0x02000640 RID: 1600
	public class RopeBreakEventInfo
	{
		// Token: 0x040030FF RID: 12543
		public UltimateRope rope;

		// Token: 0x04003100 RID: 12544
		public GameObject link1;

		// Token: 0x04003101 RID: 12545
		public GameObject link2;

		// Token: 0x04003102 RID: 12546
		public Vector3 worldPos;

		// Token: 0x04003103 RID: 12547
		public Vector3 localLink1Pos;

		// Token: 0x04003104 RID: 12548
		public Vector3 localLink2Pos;
	}
}
