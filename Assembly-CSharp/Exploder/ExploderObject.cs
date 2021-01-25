using System;
using System.Collections.Generic;
using System.Diagnostics;
using Exploder.Core;
using Exploder.Core.Math;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000C1 RID: 193
	public class ExploderObject : MonoBehaviour
	{
		// Token: 0x06000405 RID: 1029 RVA: 0x00004C46 File Offset: 0x00002E46
		public void Explode()
		{
			this.Explode(null);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00004C4F File Offset: 0x00002E4F
		public void Explode(ExploderObject.OnExplosion callback)
		{
			this.queue.Explode(callback);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00004C5D File Offset: 0x00002E5D
		public void StartExplosionFromQueue(Vector3 pos, int id, ExploderObject.OnExplosion callback)
		{
			this.mainCentroid = pos;
			this.explosionID = id;
			this.state = ExploderObject.State.Preprocess;
			this.ExplosionCallback = callback;
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00004C7B File Offset: 0x00002E7B
		public void Crack()
		{
			this.Crack(null);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00004C84 File Offset: 0x00002E84
		public void Crack(ExploderObject.OnCracked callback)
		{
			if (!this.crack)
			{
				this.CrackedCallback = callback;
				this.crack = true;
				this.cracked = false;
				this.Explode(null);
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00004CAD File Offset: 0x00002EAD
		public void ExplodeCracked(ExploderObject.OnExplosion callback)
		{
			if (this.cracked)
			{
				this.PostCrackExplode(callback);
				this.crack = false;
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public void ExplodeCracked()
		{
			this.ExplodeCracked(null);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00032A8C File Offset: 0x00030C8C
		private void Awake()
		{
			this.cutter = new MeshCutter();
			this.cutter.Init(512, 512);
			Random.seed = DateTime.Now.Millisecond;
			bool use2DCollision = this.Use2DCollision;
			FragmentPool.Instance.Allocate(this.FragmentPoolSize, this.MeshColliders, use2DCollision);
			FragmentPool.Instance.SetDeactivateOptions(this.DeactivateOptions, this.FadeoutOptions, this.DeactivateTimeout);
			FragmentPool.Instance.SetExplodableFragments(this.ExplodeFragments, this.DontUseTag);
			FragmentPool.Instance.SetFragmentPhysicsOptions(this.FragmentOptions, use2DCollision);
			FragmentPool.Instance.SetSFXOptions(this.SFXOptions);
			this.timer = new Stopwatch();
			this.queue = new ExploderQueue(this);
			if (this.DontUseTag)
			{
				base.gameObject.AddComponent<Explodable>();
			}
			else
			{
				base.gameObject.tag = "Exploder";
			}
			this.state = ExploderObject.State.DryRun;
			this.PreAllocateBuffers();
			this.state = ExploderObject.State.None;
			if (this.SFXOptions.ExplosionSoundClip)
			{
				this.audioSource = base.gameObject.GetComponent<AudioSource>();
				if (!this.audioSource)
				{
					this.audioSource = base.gameObject.AddComponent<AudioSource>();
				}
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00032BDC File Offset: 0x00030DDC
		private void PreAllocateBuffers()
		{
			this.newFragments = new HashSet<ExploderObject.CutMesh>();
			this.meshToRemove = new HashSet<ExploderObject.CutMesh>();
			this.meshSet = new HashSet<ExploderObject.CutMesh>();
			for (int i = 0; i < 64; i++)
			{
				this.meshSet.Add(default(ExploderObject.CutMesh));
			}
			this.levelCount = new int[64];
			this.Preprocess();
			long num;
			this.ProcessCutter(out num);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00032C50 File Offset: 0x00030E50
		private void OnDrawGizmos()
		{
			if (base.enabled && (!this.ExplodeSelf || !this.DisableRadiusScan))
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(ExploderUtils.GetCentroid(base.gameObject), this.Radius);
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00004CD1 File Offset: 0x00002ED1
		private int GetLevelFragments(int level, int fragmentsMax)
		{
			return fragmentsMax * 2 / (level * level + level) + 1;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00032CA0 File Offset: 0x00030EA0
		private int GetLevel(float distance, float radius)
		{
			float num = distance / radius * 6f;
			int num2 = (int)num / 2 + 1;
			return Mathf.Clamp(num2, 0, 10);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00032CC8 File Offset: 0x00030EC8
		private List<ExploderObject.MeshData> GetMeshData(GameObject obj)
		{
			MeshRenderer[] componentsInChildren = obj.GetComponentsInChildren<MeshRenderer>();
			MeshFilter[] componentsInChildren2 = obj.GetComponentsInChildren<MeshFilter>();
			if (componentsInChildren.Length != componentsInChildren2.Length)
			{
				return new List<ExploderObject.MeshData>();
			}
			List<ExploderObject.MeshData> list = new List<ExploderObject.MeshData>(componentsInChildren.Length);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!(componentsInChildren2[i].sharedMesh == null))
				{
					if (!componentsInChildren2[i].sharedMesh || !componentsInChildren2[i].sharedMesh.isReadable)
					{
						Debug.LogWarning("Mesh is not readable: " + componentsInChildren2[i].name);
					}
					else
					{
						list.Add(new ExploderObject.MeshData
						{
							sharedMesh = componentsInChildren2[i].sharedMesh,
							sharedMaterial = componentsInChildren[i].sharedMaterial,
							gameObject = componentsInChildren[i].gameObject,
							centroid = componentsInChildren[i].bounds.center,
							parentObject = obj
						});
					}
				}
			}
			SkinnedMeshRenderer[] componentsInChildren3 = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int j = 0; j < componentsInChildren3.Length; j++)
			{
				Mesh mesh = new Mesh();
				componentsInChildren3[j].BakeMesh(mesh);
				GameObject gameObject = new GameObject("BakeSkin");
				MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
				meshFilter.sharedMesh = mesh;
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
				meshRenderer.sharedMaterial = componentsInChildren3[j].material;
				gameObject.transform.position = obj.transform.position;
				gameObject.transform.rotation = obj.transform.rotation;
				ExploderUtils.SetVisible(gameObject, false);
				list.Add(new ExploderObject.MeshData
				{
					sharedMesh = mesh,
					sharedMaterial = meshRenderer.sharedMaterial,
					gameObject = gameObject,
					centroid = meshRenderer.bounds.center,
					parentObject = gameObject,
					skinnedBakeOriginal = obj
				});
			}
			return list;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00004CDE File Offset: 0x00002EDE
		private bool IsExplodable(GameObject obj)
		{
			if (this.DontUseTag)
			{
				return obj.GetComponent<Explodable>() != null;
			}
			return obj.CompareTag(ExploderObject.Tag);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00032EC4 File Offset: 0x000310C4
		private List<ExploderObject.CutMesh> GetMeshList()
		{
			GameObject[] array3;
			if (this.DontUseTag)
			{
				Object[] array = Object.FindObjectsOfType(typeof(Explodable));
				List<GameObject> list = new List<GameObject>(array.Length);
				foreach (Object @object in array)
				{
					Explodable explodable = (Explodable)@object;
					if (explodable)
					{
						list.Add(explodable.gameObject);
					}
				}
				array3 = list.ToArray();
			}
			else
			{
				array3 = GameObject.FindGameObjectsWithTag("Exploder");
			}
			List<ExploderObject.CutMesh> list2 = new List<ExploderObject.CutMesh>(array3.Length);
			foreach (GameObject gameObject in array3)
			{
				if (this.ExplodeSelf || !(gameObject == base.gameObject))
				{
					if (!(gameObject != base.gameObject) || !this.ExplodeSelf || !this.DisableRadiusScan)
					{
						float sqrMagnitude = (ExploderUtils.GetCentroid(gameObject) - this.mainCentroid).sqrMagnitude;
						if (sqrMagnitude < this.Radius * this.Radius)
						{
							List<ExploderObject.MeshData> meshData = this.GetMeshData(gameObject);
							int count = meshData.Count;
							for (int k = 0; k < count; k++)
							{
								Vector3 centroid = meshData[k].centroid;
								float magnitude = (centroid - this.mainCentroid).magnitude;
								list2.Add(new ExploderObject.CutMesh
								{
									mesh = meshData[k].sharedMesh,
									material = meshData[k].sharedMaterial,
									centroid = meshData[k].gameObject.transform.InverseTransformPoint(centroid),
									vertices = meshData[k].sharedMesh.vertexCount,
									transform = meshData[k].gameObject.transform,
									parent = meshData[k].gameObject.transform.parent,
									position = meshData[k].gameObject.transform.position,
									rotation = meshData[k].gameObject.transform.rotation,
									localScale = meshData[k].gameObject.transform.localScale,
									distance = magnitude,
									level = this.GetLevel(magnitude, this.Radius),
									original = meshData[k].parentObject,
									skinnedOriginal = meshData[k].skinnedBakeOriginal,
									option = gameObject.GetComponent<ExploderOption>()
								});
							}
						}
					}
				}
			}
			if (list2.Count == 0)
			{
				return list2;
			}
			list2.Sort((ExploderObject.CutMesh m0, ExploderObject.CutMesh m1) => m0.level.CompareTo(m1.level));
			if (list2.Count > this.TargetFragments)
			{
				list2.RemoveRange(this.TargetFragments - 1, list2.Count - this.TargetFragments);
			}
			int level = list2[list2.Count - 1].level;
			int levelFragments = this.GetLevelFragments(level, this.TargetFragments);
			int num = 0;
			int count2 = list2.Count;
			int[] array5 = new int[level + 1];
			foreach (ExploderObject.CutMesh cutMesh in list2)
			{
				array5[cutMesh.level]++;
			}
			for (int l = 0; l < count2; l++)
			{
				ExploderObject.CutMesh cutMesh2 = list2[l];
				int num2 = level + 1 - cutMesh2.level;
				int num3 = num2 * levelFragments / array5[cutMesh2.level];
				cutMesh2.fragments = num3;
				num += num3;
				list2[l] = cutMesh2;
				if (num >= this.TargetFragments)
				{
					cutMesh2.fragments -= num - this.TargetFragments;
					num -= num - this.TargetFragments;
					list2[l] = cutMesh2;
					break;
				}
			}
			return list2;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00033380 File Offset: 0x00031580
		private void Update()
		{
			long timeOffset = 0L;
			switch (this.state)
			{
			case ExploderObject.State.None:
				return;
			case ExploderObject.State.Preprocess:
				this.timer.Reset();
				this.timer.Start();
				if (!this.Preprocess())
				{
					this.OnExplosionFinished(false);
					return;
				}
				this.state = ExploderObject.State.ProcessCutter;
				break;
			case ExploderObject.State.ProcessCutter:
				break;
			case ExploderObject.State.IsolateMeshIslands:
				goto IL_D9;
			case ExploderObject.State.PostprocessInit:
				goto IL_F9;
			case ExploderObject.State.Postprocess:
				goto IL_10B;
			default:
				return;
			}
			bool flag = this.ProcessCutter(out timeOffset);
			if (!flag)
			{
				return;
			}
			this.poolIdx = 0;
			this.postList = new List<ExploderObject.CutMesh>(this.meshSet);
			if (!this.splitMeshIslands)
			{
				this.state = ExploderObject.State.PostprocessInit;
				goto IL_F9;
			}
			this.islands = new List<ExploderObject.CutMesh>(this.meshSet.Count);
			this.state = ExploderObject.State.IsolateMeshIslands;
			IL_D9:
			bool flag2 = this.IsolateMeshIslands(ref timeOffset);
			if (!flag2)
			{
				return;
			}
			this.state = ExploderObject.State.PostprocessInit;
			IL_F9:
			this.InitPostprocess();
			this.state = ExploderObject.State.Postprocess;
			IL_10B:
			if (this.Postprocess(timeOffset))
			{
				this.timer.Stop();
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000334B4 File Offset: 0x000316B4
		private bool Preprocess()
		{
			List<ExploderObject.CutMesh> meshList = this.GetMeshList();
			if (meshList.Count == 0)
			{
				return false;
			}
			this.newFragments.Clear();
			this.meshToRemove.Clear();
			this.meshSet = new HashSet<ExploderObject.CutMesh>(meshList);
			this.splitMeshIslands = this.SplitMeshIslands;
			int level = meshList[meshList.Count - 1].level;
			this.levelCount = new int[level + 1];
			foreach (ExploderObject.CutMesh cutMesh in this.meshSet)
			{
				this.levelCount[cutMesh.level] += cutMesh.fragments;
			}
			if (this.UniformFragmentDistribution)
			{
				int[] array = new int[64];
				foreach (ExploderObject.CutMesh cutMesh2 in this.meshSet)
				{
					array[cutMesh2.level]++;
				}
				int num = this.TargetFragments / this.meshSet.Count;
				foreach (ExploderObject.CutMesh cutMesh3 in this.meshSet)
				{
					this.levelCount[cutMesh3.level] = num * array[cutMesh3.level];
				}
			}
			return true;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00033670 File Offset: 0x00031870
		private bool ProcessCutter(out long cuttingTime)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			bool flag = true;
			bool flag2 = false;
			int num = 0;
			while (flag)
			{
				num++;
				if (num > this.TargetFragments)
				{
					break;
				}
				int count = this.meshSet.Count;
				this.newFragments.Clear();
				this.meshToRemove.Clear();
				flag = false;
				foreach (ExploderObject.CutMesh cutMesh in this.meshSet)
				{
					if (this.levelCount[cutMesh.level] > 0)
					{
						Vector3 insideUnitSphere = Random.insideUnitSphere;
						if (cutMesh.transform)
						{
							Plane plane = new Plane(insideUnitSphere, cutMesh.transform.TransformPoint(cutMesh.centroid));
							bool triangulateHoles = true;
							Color crossSectionVertexColor = Color.white;
							Vector4 crossSectionUV;
							crossSectionUV..ctor(0f, 0f, 1f, 1f);
							if (cutMesh.option)
							{
								triangulateHoles = !cutMesh.option.Plane2D;
								crossSectionVertexColor = cutMesh.option.CrossSectionVertexColor;
								crossSectionUV = cutMesh.option.CrossSectionUV;
								this.splitMeshIslands |= cutMesh.option.SplitMeshIslands;
							}
							if (this.Use2DCollision)
							{
								triangulateHoles = false;
							}
							List<CutterMesh> list = null;
							this.cutter.Cut(cutMesh.mesh, cutMesh.transform, plane, triangulateHoles, this.AllowOpenMeshCutting, ref list, crossSectionVertexColor, crossSectionUV);
							flag = true;
							if (list != null)
							{
								foreach (CutterMesh cutterMesh in list)
								{
									this.newFragments.Add(new ExploderObject.CutMesh
									{
										mesh = cutterMesh.mesh,
										centroid = cutterMesh.centroid,
										material = cutMesh.material,
										vertices = cutMesh.vertices,
										transform = cutMesh.transform,
										distance = cutMesh.distance,
										level = cutMesh.level,
										fragments = cutMesh.fragments,
										original = cutMesh.original,
										skinnedOriginal = cutMesh.skinnedOriginal,
										parent = cutMesh.transform.parent,
										position = cutMesh.transform.position,
										rotation = cutMesh.transform.rotation,
										localScale = cutMesh.transform.localScale,
										option = cutMesh.option
									});
								}
								this.meshToRemove.Add(cutMesh);
								this.levelCount[cutMesh.level]--;
								if (count + this.newFragments.Count - this.meshToRemove.Count >= this.TargetFragments)
								{
									cuttingTime = stopwatch.ElapsedMilliseconds;
									return true;
								}
								if ((float)stopwatch.ElapsedMilliseconds > this.FrameBudget)
								{
									flag2 = true;
									break;
								}
							}
						}
					}
				}
				this.meshSet.ExceptWith(this.meshToRemove);
				this.meshSet.UnionWith(this.newFragments);
				if (flag2)
				{
					break;
				}
			}
			cuttingTime = stopwatch.ElapsedMilliseconds;
			return !flag2;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00033A34 File Offset: 0x00031C34
		private bool IsolateMeshIslands(ref long timeOffset)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int count = this.postList.Count;
			while (this.poolIdx < count)
			{
				ExploderObject.CutMesh cutMesh = this.postList[this.poolIdx];
				this.poolIdx++;
				bool flag = false;
				if (this.SplitMeshIslands || (cutMesh.option && cutMesh.option.SplitMeshIslands))
				{
					List<CutterMesh> list = MeshUtils.IsolateMeshIslands(cutMesh.mesh);
					if (list != null)
					{
						flag = true;
						foreach (CutterMesh cutterMesh in list)
						{
							this.islands.Add(new ExploderObject.CutMesh
							{
								mesh = cutterMesh.mesh,
								centroid = cutterMesh.centroid,
								material = cutMesh.material,
								vertices = cutMesh.vertices,
								transform = cutMesh.transform,
								distance = cutMesh.distance,
								level = cutMesh.level,
								fragments = cutMesh.fragments,
								original = cutMesh.original,
								skinnedOriginal = cutMesh.skinnedOriginal,
								parent = cutMesh.transform.parent,
								position = cutMesh.transform.position,
								rotation = cutMesh.transform.rotation,
								localScale = cutMesh.transform.localScale,
								option = cutMesh.option
							});
						}
					}
				}
				if (!flag)
				{
					this.islands.Add(cutMesh);
				}
				if ((float)(stopwatch.ElapsedMilliseconds + timeOffset) > this.FrameBudget)
				{
					return false;
				}
			}
			this.postList = this.islands;
			return true;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00033C54 File Offset: 0x00031E54
		private void InitPostprocess()
		{
			int count = this.postList.Count;
			bool use2DCollision = this.Use2DCollision;
			FragmentPool.Instance.Allocate(count, this.MeshColliders, use2DCollision);
			FragmentPool.Instance.SetDeactivateOptions(this.DeactivateOptions, this.FadeoutOptions, this.DeactivateTimeout);
			FragmentPool.Instance.SetExplodableFragments(this.ExplodeFragments, this.DontUseTag);
			FragmentPool.Instance.SetFragmentPhysicsOptions(this.FragmentOptions, use2DCollision);
			FragmentPool.Instance.SetSFXOptions(this.SFXOptions);
			this.poolIdx = 0;
			this.pool = FragmentPool.Instance.GetAvailableFragments(count);
			if (this.ExplosionCallback != null)
			{
				this.ExplosionCallback((float)this.timer.ElapsedMilliseconds, ExploderObject.ExplosionState.ExplosionStarted);
			}
			if (this.SFXOptions.ExplosionSoundClip)
			{
				if (!this.audioSource)
				{
					this.audioSource = base.gameObject.AddComponent<AudioSource>();
				}
				this.audioSource.PlayOneShot(this.SFXOptions.ExplosionSoundClip);
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00033D60 File Offset: 0x00031F60
		private void PostCrackExplode(ExploderObject.OnExplosion callback)
		{
			if (callback != null)
			{
				callback(0f, ExploderObject.ExplosionState.ExplosionStarted);
			}
			int count = this.postList.Count;
			this.poolIdx = 0;
			while (this.poolIdx < count)
			{
				Fragment fragment = this.pool[this.poolIdx];
				ExploderObject.CutMesh cutMesh = this.postList[this.poolIdx];
				this.poolIdx++;
				if (cutMesh.original != base.gameObject)
				{
					ExploderUtils.SetActiveRecursively(cutMesh.original, false);
				}
				else
				{
					ExploderUtils.EnableCollider(cutMesh.original, false);
					ExploderUtils.SetVisible(cutMesh.original, false);
				}
				if (cutMesh.skinnedOriginal && cutMesh.skinnedOriginal != base.gameObject)
				{
					ExploderUtils.SetActiveRecursively(cutMesh.skinnedOriginal, false);
				}
				else
				{
					ExploderUtils.EnableCollider(cutMesh.skinnedOriginal, false);
					ExploderUtils.SetVisible(cutMesh.skinnedOriginal, false);
				}
				fragment.Explode();
			}
			if (this.DestroyOriginalObject)
			{
				foreach (ExploderObject.CutMesh cutMesh2 in this.postList)
				{
					if (cutMesh2.original && !cutMesh2.original.GetComponent<Fragment>())
					{
						Object.Destroy(cutMesh2.original);
					}
					if (cutMesh2.skinnedOriginal)
					{
						Object.Destroy(cutMesh2.skinnedOriginal);
					}
				}
			}
			if (this.ExplodeSelf && !this.DestroyOriginalObject)
			{
				ExploderUtils.SetActiveRecursively(base.gameObject, false);
			}
			if (this.HideSelf)
			{
				ExploderUtils.SetActiveRecursively(base.gameObject, false);
			}
			this.ExplosionCallback = callback;
			this.OnExplosionFinished(true);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00033F5C File Offset: 0x0003215C
		private bool Postprocess(long timeOffset)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int count = this.postList.Count;
			while (this.poolIdx < count)
			{
				Fragment fragment = this.pool[this.poolIdx];
				ExploderObject.CutMesh cutMesh = this.postList[this.poolIdx];
				this.poolIdx++;
				if (cutMesh.original)
				{
					if (this.crack)
					{
						ExploderUtils.SetActiveRecursively(fragment.gameObject, false);
					}
					fragment.meshFilter.sharedMesh = cutMesh.mesh;
					if (this.FragmentOptions.FragmentMaterial != null)
					{
						fragment.meshRenderer.sharedMaterial = this.FragmentOptions.FragmentMaterial;
					}
					else
					{
						fragment.meshRenderer.sharedMaterial = cutMesh.material;
					}
					cutMesh.mesh.RecalculateBounds();
					Transform parent = fragment.transform.parent;
					fragment.transform.parent = cutMesh.parent;
					fragment.transform.position = cutMesh.position;
					fragment.transform.rotation = cutMesh.rotation;
					fragment.transform.localScale = cutMesh.localScale;
					fragment.transform.parent = null;
					fragment.transform.parent = parent;
					if (!this.crack)
					{
						if (cutMesh.original != base.gameObject)
						{
							ExploderUtils.SetActiveRecursively(cutMesh.original, false);
						}
						else
						{
							ExploderUtils.EnableCollider(cutMesh.original, false);
							ExploderUtils.SetVisible(cutMesh.original, false);
						}
						if (cutMesh.skinnedOriginal && cutMesh.skinnedOriginal != base.gameObject)
						{
							ExploderUtils.SetActiveRecursively(cutMesh.skinnedOriginal, false);
						}
						else
						{
							ExploderUtils.EnableCollider(cutMesh.skinnedOriginal, false);
							ExploderUtils.SetVisible(cutMesh.skinnedOriginal, false);
						}
					}
					bool flag = cutMesh.option && cutMesh.option.Plane2D;
					bool use2DCollision = this.Use2DCollision;
					if (!this.FragmentOptions.DisableColliders)
					{
						if (this.MeshColliders && !use2DCollision)
						{
							if (!flag)
							{
								fragment.meshCollider.sharedMesh = cutMesh.mesh;
							}
						}
						else if (this.Use2DCollision)
						{
							MeshUtils.GeneratePolygonCollider(fragment.polygonCollider2D, cutMesh.mesh);
						}
						else
						{
							fragment.boxCollider.center = cutMesh.mesh.bounds.center;
							fragment.boxCollider.size = cutMesh.mesh.bounds.extents;
						}
					}
					if (cutMesh.option)
					{
						cutMesh.option.DuplicateSettings(fragment.options);
					}
					if (!this.crack)
					{
						fragment.Explode();
					}
					float force = this.Force;
					if (cutMesh.option && cutMesh.option.UseLocalForce)
					{
						force = cutMesh.option.Force;
					}
					fragment.ApplyExplosion(cutMesh.transform, cutMesh.centroid, this.mainCentroid, this.FragmentOptions, this.UseForceVector, this.ForceVector, force, cutMesh.original, this.TargetFragments);
					if ((float)(stopwatch.ElapsedMilliseconds + timeOffset) > this.FrameBudget)
					{
						return false;
					}
				}
			}
			if (!this.crack)
			{
				if (this.DestroyOriginalObject)
				{
					foreach (ExploderObject.CutMesh cutMesh2 in this.postList)
					{
						if (cutMesh2.original && !cutMesh2.original.GetComponent<Fragment>())
						{
							Object.Destroy(cutMesh2.original);
						}
						if (cutMesh2.skinnedOriginal)
						{
							Object.Destroy(cutMesh2.skinnedOriginal);
						}
					}
				}
				if (this.ExplodeSelf && !this.DestroyOriginalObject)
				{
					ExploderUtils.SetActiveRecursively(base.gameObject, false);
				}
				if (this.HideSelf)
				{
					ExploderUtils.SetActiveRecursively(base.gameObject, false);
				}
				this.OnExplosionFinished(true);
			}
			else
			{
				this.cracked = true;
				if (this.CrackedCallback != null)
				{
					this.CrackedCallback();
				}
			}
			return true;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00034400 File Offset: 0x00032600
		private void OnExplosionFinished(bool success)
		{
			if (this.ExplosionCallback != null)
			{
				if (!success)
				{
					this.ExplosionCallback((float)this.timer.ElapsedMilliseconds, ExploderObject.ExplosionState.ExplosionStarted);
					this.OnExplosionStarted();
				}
				this.ExplosionCallback((float)this.timer.ElapsedMilliseconds, ExploderObject.ExplosionState.ExplosionFinished);
			}
			this.state = ExploderObject.State.None;
			this.queue.OnExplosionFinished(this.explosionID);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnExplosionStarted()
		{
		}

		// Token: 0x0400031F RID: 799
		public static string Tag = "Exploder";

		// Token: 0x04000320 RID: 800
		public bool DontUseTag;

		// Token: 0x04000321 RID: 801
		public float Radius = 10f;

		// Token: 0x04000322 RID: 802
		public Vector3 ForceVector = Vector3.up;

		// Token: 0x04000323 RID: 803
		public bool UseForceVector;

		// Token: 0x04000324 RID: 804
		public float Force = 30f;

		// Token: 0x04000325 RID: 805
		public float FrameBudget = 15f;

		// Token: 0x04000326 RID: 806
		public int TargetFragments = 30;

		// Token: 0x04000327 RID: 807
		public DeactivateOptions DeactivateOptions;

		// Token: 0x04000328 RID: 808
		public float DeactivateTimeout = 10f;

		// Token: 0x04000329 RID: 809
		public FadeoutOptions FadeoutOptions;

		// Token: 0x0400032A RID: 810
		public bool MeshColliders;

		// Token: 0x0400032B RID: 811
		public bool ExplodeSelf = true;

		// Token: 0x0400032C RID: 812
		public bool DisableRadiusScan;

		// Token: 0x0400032D RID: 813
		public bool HideSelf = true;

		// Token: 0x0400032E RID: 814
		public bool DestroyOriginalObject;

		// Token: 0x0400032F RID: 815
		public bool ExplodeFragments = true;

		// Token: 0x04000330 RID: 816
		public bool UniformFragmentDistribution;

		// Token: 0x04000331 RID: 817
		public bool SplitMeshIslands;

		// Token: 0x04000332 RID: 818
		public bool AllowOpenMeshCutting;

		// Token: 0x04000333 RID: 819
		public int FragmentPoolSize = 200;

		// Token: 0x04000334 RID: 820
		public bool Use2DCollision;

		// Token: 0x04000335 RID: 821
		public ExploderObject.SFXOption SFXOptions = new ExploderObject.SFXOption
		{
			HitSoundTimeout = 0.3f,
			EmitersMax = 1000
		};

		// Token: 0x04000336 RID: 822
		public ExploderObject.FragmentOption FragmentOptions = new ExploderObject.FragmentOption
		{
			Layer = "Default",
			Mass = 20f,
			MaxVelocity = 1000f,
			UseGravity = true,
			InheritParentPhysicsProperty = true,
			AngularVelocity = 1f,
			AngularVelocityVector = Vector3.up,
			MaxAngularVelocity = 7f,
			RandomAngularVelocityVector = true
		};

		// Token: 0x04000337 RID: 823
		private ExploderObject.OnExplosion ExplosionCallback;

		// Token: 0x04000338 RID: 824
		private ExploderObject.OnCracked CrackedCallback;

		// Token: 0x04000339 RID: 825
		private bool crack;

		// Token: 0x0400033A RID: 826
		private bool cracked;

		// Token: 0x0400033B RID: 827
		private ExploderObject.State state;

		// Token: 0x0400033C RID: 828
		private ExploderQueue queue;

		// Token: 0x0400033D RID: 829
		private MeshCutter cutter;

		// Token: 0x0400033E RID: 830
		private Stopwatch timer;

		// Token: 0x0400033F RID: 831
		private HashSet<ExploderObject.CutMesh> newFragments;

		// Token: 0x04000340 RID: 832
		private HashSet<ExploderObject.CutMesh> meshToRemove;

		// Token: 0x04000341 RID: 833
		private HashSet<ExploderObject.CutMesh> meshSet;

		// Token: 0x04000342 RID: 834
		private int[] levelCount;

		// Token: 0x04000343 RID: 835
		private int poolIdx;

		// Token: 0x04000344 RID: 836
		private List<ExploderObject.CutMesh> postList;

		// Token: 0x04000345 RID: 837
		private List<Fragment> pool;

		// Token: 0x04000346 RID: 838
		private Vector3 mainCentroid;

		// Token: 0x04000347 RID: 839
		private bool splitMeshIslands;

		// Token: 0x04000348 RID: 840
		private List<ExploderObject.CutMesh> islands;

		// Token: 0x04000349 RID: 841
		private int explosionID;

		// Token: 0x0400034A RID: 842
		private AudioSource audioSource;

		// Token: 0x020000C2 RID: 194
		[Serializable]
		public class SFXOption
		{
			// Token: 0x0600041F RID: 1055 RVA: 0x0003446C File Offset: 0x0003266C
			public ExploderObject.SFXOption Clone()
			{
				return new ExploderObject.SFXOption
				{
					ExplosionSoundClip = this.ExplosionSoundClip,
					FragmentSoundClip = this.FragmentSoundClip,
					FragmentEmitter = this.FragmentEmitter,
					HitSoundTimeout = this.HitSoundTimeout,
					EmitersMax = this.EmitersMax
				};
			}

			// Token: 0x0400034C RID: 844
			public AudioClip ExplosionSoundClip;

			// Token: 0x0400034D RID: 845
			public AudioClip FragmentSoundClip;

			// Token: 0x0400034E RID: 846
			public GameObject FragmentEmitter;

			// Token: 0x0400034F RID: 847
			public float HitSoundTimeout;

			// Token: 0x04000350 RID: 848
			public int EmitersMax;
		}

		// Token: 0x020000C3 RID: 195
		[Serializable]
		public class FragmentOption
		{
			// Token: 0x06000421 RID: 1057 RVA: 0x000344BC File Offset: 0x000326BC
			public ExploderObject.FragmentOption Clone()
			{
				return new ExploderObject.FragmentOption
				{
					FreezePositionX = this.FreezePositionX,
					FreezePositionY = this.FreezePositionY,
					FreezePositionZ = this.FreezePositionZ,
					FreezeRotationX = this.FreezeRotationX,
					FreezeRotationY = this.FreezeRotationY,
					FreezeRotationZ = this.FreezeRotationZ,
					Layer = this.Layer,
					Mass = this.Mass,
					DisableColliders = this.DisableColliders,
					UseGravity = this.UseGravity,
					MaxVelocity = this.MaxVelocity,
					MaxAngularVelocity = this.MaxAngularVelocity,
					InheritParentPhysicsProperty = this.InheritParentPhysicsProperty,
					AngularVelocity = this.AngularVelocity,
					AngularVelocityVector = this.AngularVelocityVector,
					RandomAngularVelocityVector = this.RandomAngularVelocityVector,
					FragmentMaterial = this.FragmentMaterial
				};
			}

			// Token: 0x04000351 RID: 849
			public bool FreezePositionX;

			// Token: 0x04000352 RID: 850
			public bool FreezePositionY;

			// Token: 0x04000353 RID: 851
			public bool FreezePositionZ;

			// Token: 0x04000354 RID: 852
			public bool FreezeRotationX;

			// Token: 0x04000355 RID: 853
			public bool FreezeRotationY;

			// Token: 0x04000356 RID: 854
			public bool FreezeRotationZ;

			// Token: 0x04000357 RID: 855
			public string Layer;

			// Token: 0x04000358 RID: 856
			public float MaxVelocity;

			// Token: 0x04000359 RID: 857
			public bool InheritParentPhysicsProperty;

			// Token: 0x0400035A RID: 858
			public float Mass;

			// Token: 0x0400035B RID: 859
			public bool UseGravity;

			// Token: 0x0400035C RID: 860
			public bool DisableColliders;

			// Token: 0x0400035D RID: 861
			public float AngularVelocity;

			// Token: 0x0400035E RID: 862
			public float MaxAngularVelocity;

			// Token: 0x0400035F RID: 863
			public Vector3 AngularVelocityVector;

			// Token: 0x04000360 RID: 864
			public bool RandomAngularVelocityVector;

			// Token: 0x04000361 RID: 865
			public Material FragmentMaterial;
		}

		// Token: 0x020000C4 RID: 196
		public enum ExplosionState
		{
			// Token: 0x04000363 RID: 867
			ExplosionStarted,
			// Token: 0x04000364 RID: 868
			ExplosionFinished
		}

		// Token: 0x020000C5 RID: 197
		private enum State
		{
			// Token: 0x04000366 RID: 870
			None,
			// Token: 0x04000367 RID: 871
			Preprocess,
			// Token: 0x04000368 RID: 872
			ProcessCutter,
			// Token: 0x04000369 RID: 873
			IsolateMeshIslands,
			// Token: 0x0400036A RID: 874
			PostprocessInit,
			// Token: 0x0400036B RID: 875
			Postprocess,
			// Token: 0x0400036C RID: 876
			DryRun
		}

		// Token: 0x020000C6 RID: 198
		private struct CutMesh
		{
			// Token: 0x0400036D RID: 877
			public Mesh mesh;

			// Token: 0x0400036E RID: 878
			public Material material;

			// Token: 0x0400036F RID: 879
			public Transform transform;

			// Token: 0x04000370 RID: 880
			public Transform parent;

			// Token: 0x04000371 RID: 881
			public Vector3 position;

			// Token: 0x04000372 RID: 882
			public Quaternion rotation;

			// Token: 0x04000373 RID: 883
			public Vector3 localScale;

			// Token: 0x04000374 RID: 884
			public GameObject original;

			// Token: 0x04000375 RID: 885
			public Vector3 centroid;

			// Token: 0x04000376 RID: 886
			public float distance;

			// Token: 0x04000377 RID: 887
			public int vertices;

			// Token: 0x04000378 RID: 888
			public int level;

			// Token: 0x04000379 RID: 889
			public int fragments;

			// Token: 0x0400037A RID: 890
			public ExploderOption option;

			// Token: 0x0400037B RID: 891
			public GameObject skinnedOriginal;
		}

		// Token: 0x020000C7 RID: 199
		private struct MeshData
		{
			// Token: 0x0400037C RID: 892
			public Mesh sharedMesh;

			// Token: 0x0400037D RID: 893
			public Material sharedMaterial;

			// Token: 0x0400037E RID: 894
			public GameObject gameObject;

			// Token: 0x0400037F RID: 895
			public GameObject parentObject;

			// Token: 0x04000380 RID: 896
			public GameObject skinnedBakeOriginal;

			// Token: 0x04000381 RID: 897
			public Vector3 centroid;
		}

		// Token: 0x020000C8 RID: 200
		// (Invoke) Token: 0x06000423 RID: 1059
		public delegate void OnExplosion(float timeMS, ExploderObject.ExplosionState state);

		// Token: 0x020000C9 RID: 201
		// (Invoke) Token: 0x06000427 RID: 1063
		public delegate void OnCracked();
	}
}
