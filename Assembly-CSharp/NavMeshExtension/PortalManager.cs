using System;
using System.Collections.Generic;
using UnityEngine;

namespace NavMeshExtension
{
	// Token: 0x0200051B RID: 1307
	public class PortalManager : MonoBehaviour
	{
		// Token: 0x06002189 RID: 8585 RVA: 0x00016869 File Offset: 0x00014A69
		private void Awake()
		{
			PortalManager.instance = this;
			PortalManager.Init();
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x00016876 File Offset: 0x00014A76
		private void OnDestroy()
		{
			PortalManager.portals.Clear();
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000FC654 File Offset: 0x000FA854
		public static void Init()
		{
			foreach (object obj in PortalManager.instance.transform)
			{
				Transform transform = (Transform)obj;
				if (!PortalManager.portals.ContainsKey(transform))
				{
					PortalManager.portals.Add(transform, new List<Transform>());
					foreach (object obj2 in transform)
					{
						Transform transform2 = (Transform)obj2;
						PortalManager.portals[transform].Add(transform2);
					}
				}
			}
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000FC734 File Offset: 0x000FA934
		private static Vector3 GetComplement(Transform parent, Vector3 portal)
		{
			Vector3 position;
			if (PortalManager.portals[parent][0].position == portal)
			{
				position = PortalManager.portals[parent][1].position;
			}
			else
			{
				position = PortalManager.portals[parent][0].position;
			}
			return position;
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000FC798 File Offset: 0x000FA998
		private static Vector3? FindNearest(Transform parent, Vector3 pos)
		{
			NavMeshPath navMeshPath = new NavMeshPath();
			Vector3? result = default(Vector3?);
			float num = float.PositiveInfinity;
			if (!PortalManager.portals.ContainsKey(parent))
			{
				return default(Vector3?);
			}
			for (int i = 0; i < PortalManager.portals[parent].Count; i++)
			{
				Vector3 position = PortalManager.portals[parent][i].position;
				float num2 = float.PositiveInfinity;
				if (NavMesh.CalculatePath(pos, position, -1, navMeshPath) && navMeshPath.status == null)
				{
					num2 = PortalManager.PathLength(navMeshPath);
				}
				if (num2 < num)
				{
					num = num2;
					result = new Vector3?(position);
				}
			}
			return result;
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000FC850 File Offset: 0x000FAA50
		private static float PathLength(NavMeshPath path)
		{
			if (path.corners.Length < 2)
			{
				return 0f;
			}
			Vector3 vector = path.corners[0];
			float num = 0f;
			for (int i = 1; i < path.corners.Length; i++)
			{
				Vector3 vector2 = path.corners[i];
				num += Vector3.Distance(vector, vector2);
				vector = vector2;
			}
			return num;
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000FC8C4 File Offset: 0x000FAAC4
		public static Vector3[] GetPath(Vector3 start, Vector3 target)
		{
			return new PortalManager.PortalPath
			{
				pos = start,
				final = target
			}.GetShortestPath();
		}

		// Token: 0x040024B5 RID: 9397
		private static PortalManager instance;

		// Token: 0x040024B6 RID: 9398
		public static readonly Dictionary<Transform, List<Transform>> portals = new Dictionary<Transform, List<Transform>>();

		// Token: 0x0200051C RID: 1308
		[Serializable]
		public class PortalPath
		{
			// Token: 0x06002191 RID: 8593 RVA: 0x000FC8EC File Offset: 0x000FAAEC
			public Vector3[] GetShortestPath()
			{
				List<Transform> list = new List<Transform>();
				foreach (Transform transform in PortalManager.portals.Keys)
				{
					list.Add(transform);
				}
				List<PortalManager.PortalPath.PortalNode> list2 = new List<PortalManager.PortalPath.PortalNode>();
				list2.Add(new PortalManager.PortalPath.PortalNode(this, this.pos));
				for (int i = 0; i < list.Count; i++)
				{
					list2.Add(new PortalManager.PortalPath.PortalNode(this.pos, list, i));
				}
				for (int j = 0; j < list2.Count; j++)
				{
					list2[j].CalculateLength(0f);
				}
				float num = float.PositiveInfinity;
				int num2 = 0;
				for (int k = 0; k < list2.Count; k++)
				{
					float shortestLength = list2[k].GetShortestLength();
					if (shortestLength < num)
					{
						num = shortestLength;
						num2 = k;
					}
				}
				return list2[num2].GetShortestPath().ToArray();
			}

			// Token: 0x040024B7 RID: 9399
			public Vector3 pos;

			// Token: 0x040024B8 RID: 9400
			public Vector3 final;

			// Token: 0x0200051D RID: 1309
			[Serializable]
			public class PortalNode
			{
				// Token: 0x06002192 RID: 8594 RVA: 0x000FCA1C File Offset: 0x000FAC1C
				public PortalNode(PortalManager.PortalPath instance, Vector3 start)
				{
					PortalManager.PortalPath.PortalNode.path = instance;
					this.start = start;
					NavMeshPath navMeshPath = new NavMeshPath();
					if (NavMesh.CalculatePath(start, instance.final, -1, navMeshPath) && navMeshPath.status == null)
					{
						this.target = new Vector3?(instance.final);
					}
				}

				// Token: 0x06002193 RID: 8595 RVA: 0x000FCA7C File Offset: 0x000FAC7C
				public PortalNode(Vector3 tar, List<Transform> nodes, int index)
				{
					this.start = tar;
					List<Transform> list = new List<Transform>(nodes);
					Transform transform = list[index];
					this.target = PortalManager.FindNearest(transform, this.start);
					if (this.target == null)
					{
						return;
					}
					Vector3 complement = PortalManager.GetComplement(transform, this.target.Value);
					list.Remove(transform);
					this.childs.Add(new PortalManager.PortalPath.PortalNode(PortalManager.PortalPath.PortalNode.path, complement));
					for (int i = 0; i < list.Count; i++)
					{
						this.childs.Add(new PortalManager.PortalPath.PortalNode(complement, list, i));
					}
				}

				// Token: 0x06002194 RID: 8596 RVA: 0x000FCB30 File Offset: 0x000FAD30
				public string PrintPath()
				{
					string text = string.Concat(new object[]
					{
						" ",
						this.start,
						"->",
						this.target
					});
					for (int i = 0; i < this.childs.Count; i++)
					{
						text += this.childs[i].PrintPath();
					}
					return text;
				}

				// Token: 0x06002195 RID: 8597 RVA: 0x000FCBAC File Offset: 0x000FADAC
				public void CalculateLength(float parLength)
				{
					NavMeshPath navMeshPath = new NavMeshPath();
					float num = float.PositiveInfinity;
					if (this.target != null && NavMesh.CalculatePath(this.start, this.target.Value, -1, navMeshPath))
					{
						num = parLength + PortalManager.PathLength(navMeshPath);
					}
					this.length = num;
					for (int i = 0; i < this.childs.Count; i++)
					{
						this.childs[i].CalculateLength(this.length);
					}
				}

				// Token: 0x06002196 RID: 8598 RVA: 0x000FCC38 File Offset: 0x000FAE38
				public float GetShortestLength()
				{
					if (this.childs.Count == 0)
					{
						return this.length;
					}
					float num = float.PositiveInfinity;
					for (int i = 0; i < this.childs.Count; i++)
					{
						float shortestLength = this.childs[i].GetShortestLength();
						if (shortestLength < num)
						{
							num = shortestLength;
						}
					}
					return num;
				}

				// Token: 0x06002197 RID: 8599 RVA: 0x000FCC9C File Offset: 0x000FAE9C
				public List<Vector3> GetShortestPath()
				{
					List<Vector3> list = new List<Vector3>();
					if (this.target != null)
					{
						list.Add(this.start);
						list.Add(this.target.Value);
					}
					if (this.target == null || this.childs.Count == 0)
					{
						return list;
					}
					float num = float.PositiveInfinity;
					int num2 = 0;
					for (int i = 0; i < this.childs.Count; i++)
					{
						float shortestLength = this.childs[i].GetShortestLength();
						if (shortestLength < num)
						{
							num = shortestLength;
							num2 = i;
						}
					}
					list.AddRange(this.childs[num2].GetShortestPath());
					return list;
				}

				// Token: 0x040024B9 RID: 9401
				private static PortalManager.PortalPath path;

				// Token: 0x040024BA RID: 9402
				public Vector3 start;

				// Token: 0x040024BB RID: 9403
				public Vector3? target;

				// Token: 0x040024BC RID: 9404
				public float length;

				// Token: 0x040024BD RID: 9405
				public List<PortalManager.PortalPath.PortalNode> childs = new List<PortalManager.PortalPath.PortalNode>();
			}
		}
	}
}
