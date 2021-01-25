using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Token: 0x02000672 RID: 1650
[Serializable]
public class MeshAnimation : ScriptableObject
{
	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x06002856 RID: 10326 RVA: 0x0001A8D2 File Offset: 0x00018AD2
	[HideInInspector]
	public MeshFrameData[] frameData
	{
		get
		{
			if (this.decompressedFrameData == null || this.decompressedFrameData.Length == 0)
			{
				this.decompressedFrameData = this.compressedFrameData;
			}
			return this.decompressedFrameData;
		}
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x0013F55C File Offset: 0x0013D75C
	public void GenerateFrames(Mesh baseMesh)
	{
		if (MeshAnimation.generatedFrames.ContainsKey(baseMesh) && MeshAnimation.generatedFrames[baseMesh].ContainsKey(base.name))
		{
			this.frames = MeshAnimation.generatedFrames[baseMesh][base.name];
			return;
		}
		int num = (!this.preGenerateFrames) ? 1 : (this.frameData.Length * this.frameSkip);
		for (int i = 0; i < num; i++)
		{
			this.GenerateFrame(baseMesh, i);
		}
		if (!MeshAnimation.generatedFrames.ContainsKey(baseMesh))
		{
			MeshAnimation.generatedFrames.Add(baseMesh, new Dictionary<string, Mesh[]>());
		}
		MeshAnimation.generatedFrames[baseMesh].Add(base.name, this.frames);
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x0013F62C File Offset: 0x0013D82C
	public void GenerateFrameIfNeeded(Mesh baseMesh, int frame)
	{
		if (this.mainThreadActions.Count > 0)
		{
			try
			{
				Action action = this.mainThreadActions.Dequeue();
				if (action != null)
				{
					action.Invoke();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex);
			}
		}
		if (this.completelyGenerated)
		{
			return;
		}
		if (this.frames.Length > frame && this.frames[frame])
		{
			return;
		}
		if (MeshAnimation.generatedFrames.ContainsKey(baseMesh) && MeshAnimation.generatedFrames[baseMesh].ContainsKey(base.name) && MeshAnimation.generatedFrames[baseMesh][base.name].Length > frame && MeshAnimation.generatedFrames[baseMesh][base.name][frame])
		{
			this.frames[frame] = MeshAnimation.generatedFrames[baseMesh][base.name][frame];
			if (!this.completelyGenerated)
			{
				this.generatedMeshes.Add(frame);
				if (this.generatedMeshes.Count == this.frames.Length)
				{
					this.completelyGenerated = true;
					this.generatedMeshes = null;
				}
			}
			return;
		}
		this.GenerateFrame(baseMesh, frame);
	}

	// Token: 0x06002859 RID: 10329 RVA: 0x0013F784 File Offset: 0x0013D984
	public Vector3[] GetFrame(int frame)
	{
		bool flag = frame % this.frameSkip != 0;
		if (flag)
		{
			float num = (float)frame / (float)(this.frameSkip * this.frameData.Length);
			int num2 = (int)(num * (float)this.frameData.Length);
			float num3 = (float)num2 / (float)this.frameData.Length;
			float num4 = Mathf.Clamp01((float)(num2 + 1) / (float)this.frameData.Length);
			if (num2 >= this.frameData.Length)
			{
				num2 = this.frameData.Length - 1;
			}
			if (num2 <= 0)
			{
				num2 = 0;
			}
			Vector3[] verts = this.frameData[num2].verts;
			Vector3[] verts2;
			if (num2 + 1 >= this.frameData.Length)
			{
				verts2 = this.frameData[0].verts;
			}
			else
			{
				verts2 = this.frameData[num2 + 1].verts;
			}
			float num5 = Mathf.Lerp(0f, 1f, (num - num3) / (num4 - num3));
			Vector3[] array = new Vector3[verts.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Vector3.Slerp(verts[i], verts2[i], num5);
			}
			return array;
		}
		int num6 = frame / this.frameSkip;
		if (num6 >= 0 && num6 < this.frameData.Length)
		{
			return this.frameData[frame / this.frameSkip].verts;
		}
		return this.frameData[0].verts;
	}

	// Token: 0x0600285A RID: 10330 RVA: 0x0013F904 File Offset: 0x0013DB04
	public void DisplayFrame(MeshFilter meshFilter, int frame, int previousFrame)
	{
		if (frame != previousFrame)
		{
			meshFilter.mesh = this.frames[frame];
			if (this.recalculateNormalsOnRotation)
			{
				Quaternion rotation = meshFilter.transform.rotation;
				if (this.lastRotation != rotation)
				{
					this.lastRotation = rotation;
					this.RecalculateNormals(this.frames[frame], (float)this.smoothNormalsAngle, null, null, this.instantNormalCalculation);
				}
			}
		}
		previousFrame = frame;
	}

	// Token: 0x0600285B RID: 10331 RVA: 0x0013F978 File Offset: 0x0013DB78
	public void FireEvents(GameObject eventReciever, int frame)
	{
		for (int i = 0; i < this.events.Length; i++)
		{
			if (this.events[i].frame == frame)
			{
				this.events[i].FireEvent(eventReciever);
			}
		}
	}

	// Token: 0x0600285C RID: 10332 RVA: 0x0013F9C0 File Offset: 0x0013DBC0
	public void Reset()
	{
		this.completelyGenerated = false;
		this.generatedMeshes = new HashSet<int>();
		foreach (Mesh mesh in this.frames)
		{
			if (mesh)
			{
				Object.Destroy(mesh);
			}
		}
		this.frames = new Mesh[0];
	}

	// Token: 0x0600285D RID: 10333 RVA: 0x0013FA1C File Offset: 0x0013DC1C
	private Mesh GenerateFrame(Mesh baseMesh, int frame)
	{
		if (this.frames.Length == 0)
		{
			this.frames = new Mesh[this.frameData.Length * this.frameSkip];
		}
		bool flag = frame % this.frameSkip != 0;
		if (flag)
		{
			float num = (float)frame / (float)(this.frameSkip * this.frameData.Length);
			int num2 = (int)(num * (float)this.frameData.Length);
			float num3 = (float)num2 / (float)this.frameData.Length;
			float num4 = Mathf.Clamp01((float)(num2 + 1) / (float)this.frameData.Length);
			Vector3[] verts = this.frameData[num2].verts;
			Vector3[] verts2;
			if (num2 + 1 >= this.frameData.Length)
			{
				verts2 = this.frameData[0].verts;
			}
			else
			{
				verts2 = this.frameData[num2 + 1].verts;
			}
			float num5 = Mathf.Lerp(0f, 1f, (num - num3) / (num4 - num3));
			Vector3[] array = new Vector3[verts.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Vector3.Slerp(verts[i], verts2[i], num5);
			}
			Mesh mesh = (Mesh)Object.Instantiate(baseMesh);
			mesh.name = base.name + "_" + frame;
			mesh.vertices = array;
			this.RecalculateNormals(mesh, (float)this.smoothNormalsAngle);
			this.frames[frame] = mesh;
		}
		else
		{
			Mesh mesh2 = (Mesh)Object.Instantiate(baseMesh);
			mesh2.name = base.name + "_" + frame;
			mesh2.vertices = this.frameData[frame / this.frameSkip].verts;
			this.RecalculateNormals(mesh2, (float)this.smoothNormalsAngle);
			this.frames[frame] = mesh2;
		}
		if (!this.completelyGenerated)
		{
			this.generatedMeshes.Add(frame);
			if (this.generatedMeshes.Count == this.frames.Length)
			{
				this.completelyGenerated = true;
				this.generatedMeshes = null;
			}
		}
		return this.frames[frame];
	}

	// Token: 0x0600285E RID: 10334 RVA: 0x0013FC50 File Offset: 0x0013DE50
	private void RecalculateNormals(Mesh mesh, float angle)
	{
		if (angle == -1f)
		{
			mesh.RecalculateNormals();
		}
		else
		{
			int[] triangles = mesh.GetTriangles(0);
			Vector3[] vertices = mesh.vertices;
			if (!this.meshInfoCache.ContainsKey(mesh))
			{
				this.meshInfoCache.Add(mesh, new KeyValuePair<int[], Vector3[]>(triangles, vertices));
			}
			if (this.instantNormalCalculation)
			{
				this.RecalculateNormals(mesh, angle, triangles, vertices, this.instantNormalCalculation);
			}
			else
			{
				ThreadPool.QueueUserWorkItem(delegate(object A_1)
				{
					this.RecalculateNormals(mesh, angle, triangles, vertices, this.instantNormalCalculation);
				});
			}
		}
	}

	// Token: 0x0600285F RID: 10335 RVA: 0x0013FD50 File Offset: 0x0013DF50
	private void RecalculateNormals(Mesh mesh, float angle, int[] triangles, Vector3[] vertices, bool instant = false)
	{
		if (triangles == null)
		{
			if (this.meshInfoCache.ContainsKey(mesh))
			{
				triangles = this.meshInfoCache[mesh].Key;
				vertices = this.meshInfoCache[mesh].Value;
			}
			else
			{
				triangles = mesh.GetTriangles(0);
				vertices = mesh.vertices;
				this.meshInfoCache.Add(mesh, new KeyValuePair<int[], Vector3[]>(triangles, vertices));
			}
		}
		Vector3[] array = AllocatedArray<Vector3>.Get(triangles.Length / 3);
		Vector3[] normals = AllocatedArray<Vector3>.Get(vertices.Length);
		angle *= 0.017453292f;
		PooledDictionary<Vector3, MeshAnimation.VertexEntry> pooledDictionary = PooledDictionary<Vector3, MeshAnimation.VertexEntry>.Get(vertices.Length, MeshAnimation.VectorComparer);
		for (int i = 0; i < triangles.Length; i += 3)
		{
			int num = triangles[i];
			int num2 = triangles[i + 1];
			int num3 = triangles[i + 2];
			Vector3 vector = vertices[num2] - vertices[num];
			Vector3 vector2 = vertices[num3] - vertices[num];
			Vector3 normalized = Vector3.Cross(vector, vector2).normalized;
			int num4 = i / 3;
			array[num4] = normalized;
			MeshAnimation.VertexEntry vertexEntry;
			if (!pooledDictionary.TryGetValue(vertices[num], ref vertexEntry))
			{
				vertexEntry = GenericObjectPool<MeshAnimation.VertexEntry>.Get();
				vertexEntry.PopulateArrays();
				pooledDictionary.Add(vertices[num], vertexEntry);
			}
			vertexEntry.Add(num, num4);
			if (!pooledDictionary.TryGetValue(vertices[num2], ref vertexEntry))
			{
				vertexEntry = GenericObjectPool<MeshAnimation.VertexEntry>.Get();
				vertexEntry.PopulateArrays();
				pooledDictionary.Add(vertices[num2], vertexEntry);
			}
			vertexEntry.Add(num2, num4);
			if (!pooledDictionary.TryGetValue(vertices[num3], ref vertexEntry))
			{
				vertexEntry = GenericObjectPool<MeshAnimation.VertexEntry>.Get();
				vertexEntry.PopulateArrays();
				pooledDictionary.Add(vertices[num3], vertexEntry);
			}
			vertexEntry.Add(num3, num4);
		}
		foreach (KeyValuePair<Vector3, MeshAnimation.VertexEntry> keyValuePair in pooledDictionary)
		{
			MeshAnimation.VertexEntry value = keyValuePair.Value;
			for (int j = 0; j < value.Count; j++)
			{
				Vector3 vector3 = default(Vector3);
				for (int k = 0; k < value.Count; k++)
				{
					if (value.VertexIndex[j] == value.VertexIndex[k])
					{
						vector3 += array[value.TriangleIndex[k]];
					}
					else
					{
						float num5 = Vector3.Dot(array[value.TriangleIndex[j]], array[value.TriangleIndex[k]]);
						num5 = Mathf.Clamp(num5, -0.99999f, 0.99999f);
						float num6 = Mathf.Acos(num5);
						if (num6 <= angle)
						{
							vector3 += array[value.TriangleIndex[k]];
						}
					}
				}
				normals[value.VertexIndex[j]] = vector3.normalized;
			}
			value.Clear();
			GenericObjectPool<MeshAnimation.VertexEntry>.Return(value);
		}
		pooledDictionary.ReturnToPool(false);
		if (!instant)
		{
			this.mainThreadActions.Enqueue(delegate()
			{
				if (mesh)
				{
					mesh.normals = normals;
				}
				AllocatedArray<Vector3>.Return(normals);
			});
		}
		else
		{
			mesh.normals = normals;
			AllocatedArray<Vector3>.Return(normals);
		}
	}

	// Token: 0x040032A0 RID: 12960
	internal static Dictionary<Mesh, Dictionary<string, Mesh[]>> generatedFrames = new Dictionary<Mesh, Dictionary<string, Mesh[]>>();

	// Token: 0x040032A1 RID: 12961
	public bool preGenerateFrames;

	// Token: 0x040032A2 RID: 12962
	public float playbackSpeed = 1f;

	// Token: 0x040032A3 RID: 12963
	public int smoothNormalsAngle = -1;

	// Token: 0x040032A4 RID: 12964
	public bool instantNormalCalculation;

	// Token: 0x040032A5 RID: 12965
	public bool recalculateNormalsOnRotation;

	// Token: 0x040032A6 RID: 12966
	public WrapMode wrapMode;

	// Token: 0x040032A7 RID: 12967
	[HideInInspector]
	public DeltaCompressedFrameData compressedFrameData;

	// Token: 0x040032A8 RID: 12968
	public MeshAnimationEvent[] events;

	// Token: 0x040032A9 RID: 12969
	public float length;

	// Token: 0x040032AA RID: 12970
	[HideInInspector]
	public int frameSkip = 1;

	// Token: 0x040032AB RID: 12971
	[NonSerialized]
	public Mesh[] frames = new Mesh[0];

	// Token: 0x040032AC RID: 12972
	[NonSerialized]
	private bool completelyGenerated;

	// Token: 0x040032AD RID: 12973
	[NonSerialized]
	private HashSet<int> generatedMeshes = new HashSet<int>();

	// Token: 0x040032AE RID: 12974
	[NonSerialized]
	private MeshFrameData[] decompressedFrameData;

	// Token: 0x040032AF RID: 12975
	[NonSerialized]
	private Quaternion lastRotation;

	// Token: 0x040032B0 RID: 12976
	private Queue<Action> mainThreadActions = new Queue<Action>();

	// Token: 0x040032B1 RID: 12977
	private Dictionary<Mesh, KeyValuePair<int[], Vector3[]>> meshInfoCache = new Dictionary<Mesh, KeyValuePair<int[], Vector3[]>>();

	// Token: 0x040032B2 RID: 12978
	public static IEqualityComparer<Vector3> VectorComparer = Comparers.Create<Vector3>((Vector3 x, Vector3 y) => x.x == y.x && x.y == y.y && x.z == y.z, (Vector3 x) => x.GetHashCode());

	// Token: 0x02000673 RID: 1651
	private sealed class VertexEntry
	{
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06002863 RID: 10339 RVA: 0x0001A956 File Offset: 0x00018B56
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x00140164 File Offset: 0x0013E364
		public void Add(int vertIndex, int triIndex)
		{
			if (this._reserved == this._count)
			{
				this._reserved *= 2;
				Array.Resize<int>(ref this.TriangleIndex, this._reserved);
				Array.Resize<int>(ref this.VertexIndex, this._reserved);
			}
			this.TriangleIndex[this._count] = triIndex;
			this.VertexIndex[this._count] = vertIndex;
			this._count++;
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x0001A95E File Offset: 0x00018B5E
		public void PopulateArrays()
		{
			this.TriangleIndex = AllocatedArray<int>.Get(this._reserved);
			this.VertexIndex = AllocatedArray<int>.Get(this._reserved);
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x0001A982 File Offset: 0x00018B82
		public void Clear()
		{
			this._count = 0;
			AllocatedArray<int>.Return(this.TriangleIndex);
			AllocatedArray<int>.Return(this.VertexIndex);
			this.TriangleIndex = null;
			this.VertexIndex = null;
		}

		// Token: 0x040032B5 RID: 12981
		private int _reserved = 8;

		// Token: 0x040032B6 RID: 12982
		public int[] TriangleIndex;

		// Token: 0x040032B7 RID: 12983
		public int[] VertexIndex;

		// Token: 0x040032B8 RID: 12984
		private int _count;
	}
}
