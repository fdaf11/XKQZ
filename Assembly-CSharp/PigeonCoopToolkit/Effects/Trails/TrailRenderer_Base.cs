using System;
using System.Collections.Generic;
using System.Linq;
using Assets.PigeonCoopUtil;
using UnityEngine;

namespace PigeonCoopToolkit.Effects.Trails
{
	// Token: 0x020005B4 RID: 1460
	public abstract class TrailRenderer_Base : MonoBehaviour
	{
		// Token: 0x06002466 RID: 9318 RVA: 0x000183A9 File Offset: 0x000165A9
		[ContextMenu("CLEARER")]
		public void NewClear()
		{
			if (Application.isPlaying)
			{
				this.ClearSystem(true);
			}
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0011CB28 File Offset: 0x0011AD28
		protected virtual void Awake()
		{
			TrailRenderer_Base.GlobalTrailRendererCount++;
			if (TrailRenderer_Base.GlobalTrailRendererCount == 1)
			{
				TrailRenderer_Base._matToTrailList = new Dictionary<Material, List<PCTrail>>();
				TrailRenderer_Base._toClean = new List<Mesh>();
			}
			this._activeTrail = new PCTrail(this.MaxNumberOfPoints);
			this._fadingTrails = new List<PCTrail>();
			this._t = base.transform;
			this._emit = this.Emit;
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void Start()
		{
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x0011CB94 File Offset: 0x0011AD94
		protected virtual void LateUpdate()
		{
			if (TrailRenderer_Base._hasRenderer)
			{
				return;
			}
			TrailRenderer_Base._hasRenderer = true;
			foreach (KeyValuePair<Material, List<PCTrail>> keyValuePair in TrailRenderer_Base._matToTrailList)
			{
				CombineInstance[] array = new CombineInstance[keyValuePair.Value.Count];
				for (int i = 0; i < keyValuePair.Value.Count; i++)
				{
					CombineInstance[] array2 = array;
					int num = i;
					CombineInstance combineInstance = default(CombineInstance);
					combineInstance.mesh = keyValuePair.Value[i].Mesh;
					combineInstance.subMeshIndex = 0;
					combineInstance.transform = Matrix4x4.identity;
					array2[num] = combineInstance;
				}
				Mesh mesh = new Mesh();
				mesh.CombineMeshes(array, true, false);
				TrailRenderer_Base._toClean.Add(mesh);
				this.DrawMesh(mesh, keyValuePair.Key);
				keyValuePair.Value.Clear();
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0011CCA4 File Offset: 0x0011AEA4
		protected virtual void Update()
		{
			if (TrailRenderer_Base._hasRenderer)
			{
				TrailRenderer_Base._hasRenderer = false;
				if (TrailRenderer_Base._toClean.Count > 0)
				{
					foreach (Mesh mesh in TrailRenderer_Base._toClean)
					{
						if (Application.isEditor)
						{
							Object.DestroyImmediate(mesh, true);
						}
						else
						{
							Object.Destroy(mesh);
						}
					}
				}
				TrailRenderer_Base._toClean.Clear();
			}
			if (!TrailRenderer_Base._matToTrailList.ContainsKey(this.TrailData.TrailMaterial))
			{
				TrailRenderer_Base._matToTrailList.Add(this.TrailData.TrailMaterial, new List<PCTrail>());
			}
			this.CheckEmitChange();
			if (this._activeTrail != null)
			{
				this.UpdatePoints(Time.deltaTime, this._activeTrail);
				this.GenerateMesh(this._activeTrail);
				TrailRenderer_Base._matToTrailList[this.TrailData.TrailMaterial].Add(this._activeTrail);
			}
			for (int i = this._fadingTrails.Count - 1; i >= 0; i--)
			{
				if (this._fadingTrails[i] == null || !Enumerable.Any<PCTrailPoint>(this._fadingTrails[i].Points, (PCTrailPoint a) => a.TimeActive() < this.TrailData.Lifetime))
				{
					if (this._fadingTrails[i] != null)
					{
						this._fadingTrails[i].Dispose();
					}
					this._fadingTrails.RemoveAt(i);
				}
				else
				{
					this.UpdatePoints(Time.deltaTime, this._fadingTrails[i]);
					this.GenerateMesh(this._fadingTrails[i]);
					TrailRenderer_Base._matToTrailList[this.TrailData.TrailMaterial].Add(this._fadingTrails[i]);
				}
			}
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0011CE98 File Offset: 0x0011B098
		protected virtual void OnDestroy()
		{
			TrailRenderer_Base.GlobalTrailRendererCount--;
			if (TrailRenderer_Base.GlobalTrailRendererCount == 0)
			{
				if (TrailRenderer_Base._toClean != null && TrailRenderer_Base._toClean.Count > 0)
				{
					foreach (Mesh mesh in TrailRenderer_Base._toClean)
					{
						if (Application.isEditor)
						{
							Object.DestroyImmediate(mesh, true);
						}
						else
						{
							Object.Destroy(mesh);
						}
					}
				}
				TrailRenderer_Base._toClean = null;
				TrailRenderer_Base._matToTrailList.Clear();
				TrailRenderer_Base._matToTrailList = null;
			}
			if (this._activeTrail != null)
			{
				this._activeTrail.Dispose();
				this._activeTrail = null;
			}
			if (this._fadingTrails != null)
			{
				foreach (PCTrail pctrail in this._fadingTrails)
				{
					if (pctrail != null)
					{
						pctrail.Dispose();
					}
				}
				this._fadingTrails.Clear();
			}
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void OnStopEmit()
		{
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void OnStartEmit()
		{
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0011CFD0 File Offset: 0x0011B1D0
		protected virtual void Reset()
		{
			if (this.TrailData == null)
			{
				this.TrailData = new PCTrailRendererData();
			}
			this.TrailData.ColorOverLife = new Gradient();
			this.TrailData.Lifetime = 1f;
			this.TrailData.SizeOverLife = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 1f),
				new Keyframe(1f, 0f)
			});
			this.MaxNumberOfPoints = 50;
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void InitialiseNewPoint(PCTrailPoint newPoint)
		{
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x0000264F File Offset: 0x0000084F
		protected virtual void UpdatePoint(PCTrailPoint point, float deltaTime)
		{
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x0011D068 File Offset: 0x0011B268
		protected void AddPoint(PCTrailPoint newPoint, Vector3 pos)
		{
			if (this._activeTrail == null)
			{
				return;
			}
			newPoint.Position = pos;
			newPoint.PointNumber = ((this._activeTrail.Points.Count != 0) ? (this._activeTrail.Points[this._activeTrail.Points.Count - 1].PointNumber + 1) : 0);
			this.InitialiseNewPoint(newPoint);
			newPoint.SetDistanceFromStart((this._activeTrail.Points.Count != 0) ? (this._activeTrail.Points[this._activeTrail.Points.Count - 1].GetDistanceFromStart() + Vector3.Distance(this._activeTrail.Points[this._activeTrail.Points.Count - 1].Position, pos)) : 0f);
			if (this.TrailData.UseForwardOverride)
			{
				newPoint.Forward = ((!this.TrailData.ForwardOverrideRelative) ? this.TrailData.ForwardOverride.normalized : this._t.TransformDirection(this.TrailData.ForwardOverride.normalized));
			}
			this._activeTrail.Points.Add(newPoint);
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x0011D1BC File Offset: 0x0011B3BC
		private void GenerateMesh(PCTrail trail)
		{
			trail.Mesh.Clear(false);
			Vector3 vector = (!(Camera.main != null)) ? Vector3.forward : Camera.main.transform.forward;
			if (this.TrailData.UseForwardOverride)
			{
				vector = this.TrailData.ForwardOverride.normalized;
			}
			trail.activePointCount = this.NumberOfActivePoints(trail);
			if (trail.activePointCount < 2)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < trail.Points.Count; i++)
			{
				PCTrailPoint pctrailPoint = trail.Points[i];
				float num2 = pctrailPoint.TimeActive() / this.TrailData.Lifetime;
				if (pctrailPoint.TimeActive() <= this.TrailData.Lifetime)
				{
					if (this.TrailData.UseForwardOverride && this.TrailData.ForwardOverrideRelative)
					{
						vector = pctrailPoint.Forward;
					}
					Vector3 vector2 = Vector3.zero;
					if (i < trail.Points.Count - 1)
					{
						vector2 = Vector3.Cross((trail.Points[i + 1].Position - pctrailPoint.Position).normalized, vector).normalized;
					}
					else
					{
						vector2 = Vector3.Cross((pctrailPoint.Position - trail.Points[i - 1].Position).normalized, vector).normalized;
					}
					Color color = (!this.TrailData.StretchColorToFit) ? this.TrailData.ColorOverLife.Evaluate(num2) : this.TrailData.ColorOverLife.Evaluate(1f - (float)num / (float)trail.activePointCount / 2f);
					float num3 = (!this.TrailData.StretchSizeToFit) ? this.TrailData.SizeOverLife.Evaluate(num2) : this.TrailData.SizeOverLife.Evaluate(1f - (float)num / (float)trail.activePointCount / 2f);
					trail.verticies[num] = pctrailPoint.Position + vector2 * num3;
					if (this.TrailData.MaterialTileLength <= 0f)
					{
						trail.uvs[num] = new Vector2((float)num / (float)trail.activePointCount / 2f, 0f);
					}
					else
					{
						trail.uvs[num] = new Vector2(pctrailPoint.GetDistanceFromStart() / this.TrailData.MaterialTileLength, 0f);
					}
					trail.normals[num] = vector;
					trail.colors[num] = color;
					num++;
					trail.verticies[num] = pctrailPoint.Position - vector2 * num3;
					if (this.TrailData.MaterialTileLength <= 0f)
					{
						trail.uvs[num] = new Vector2((float)num / (float)trail.activePointCount / 2f, 1f);
					}
					else
					{
						trail.uvs[num] = new Vector2(pctrailPoint.GetDistanceFromStart() / this.TrailData.MaterialTileLength, 1f);
					}
					trail.normals[num] = vector;
					trail.colors[num] = color;
					num++;
				}
			}
			Vector2 vector3 = trail.verticies[num - 1];
			for (int j = num; j < trail.verticies.Length; j++)
			{
				trail.verticies[j] = vector3;
			}
			int num4 = 0;
			for (int k = 0; k < 2 * (trail.activePointCount - 1); k++)
			{
				if (k % 2 == 0)
				{
					trail.indicies[num4] = k;
					num4++;
					trail.indicies[num4] = k + 1;
					num4++;
					trail.indicies[num4] = k + 2;
				}
				else
				{
					trail.indicies[num4] = k + 2;
					num4++;
					trail.indicies[num4] = k + 1;
					num4++;
					trail.indicies[num4] = k;
				}
				num4++;
			}
			int num5 = trail.indicies[num4 - 1];
			for (int l = num4; l < trail.indicies.Length; l++)
			{
				trail.indicies[l] = num5;
			}
			trail.Mesh.vertices = trail.verticies;
			trail.Mesh.SetIndices(trail.indicies, 0, 0);
			trail.Mesh.uv = trail.uvs;
			trail.Mesh.normals = trail.normals;
			trail.Mesh.colors = trail.colors;
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000183BC File Offset: 0x000165BC
		private void DrawMesh(Mesh trailMesh, Material trailMaterial)
		{
			Graphics.DrawMesh(trailMesh, Matrix4x4.identity, trailMaterial, base.gameObject.layer);
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x0011D6E0 File Offset: 0x0011B8E0
		private void UpdatePoints(float deltaTime, PCTrail line)
		{
			for (int i = 0; i < line.Points.Count; i++)
			{
				line.Points[i].Update(deltaTime);
				this.UpdatePoint(line.Points[i], deltaTime);
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x0011D730 File Offset: 0x0011B930
		private void CheckEmitChange()
		{
			if (this._emit != this.Emit)
			{
				this._emit = this.Emit;
				if (this._emit)
				{
					this.OnStartEmit();
					this._activeTrail = new PCTrail(this.MaxNumberOfPoints);
				}
				else
				{
					this.OnStopEmit();
					this._fadingTrails.Add(this._activeTrail);
					this._activeTrail = null;
				}
			}
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0011D7A0 File Offset: 0x0011B9A0
		private int NumberOfActivePoints(PCTrail line)
		{
			int num = 0;
			for (int i = 0; i < line.Points.Count; i++)
			{
				if (line.Points[i].TimeActive() < this.TrailData.Lifetime)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x0011D7F4 File Offset: 0x0011B9F4
		public void CreateTrail(Vector3 from, Vector3 to, float distanceBetweenPoints)
		{
			float num = Vector3.Distance(from, to);
			Vector3 normalized = (to - from).normalized;
			float num2 = 0f;
			CircularBuffer<PCTrailPoint> circularBuffer = new CircularBuffer<PCTrailPoint>(this.MaxNumberOfPoints);
			int num3 = 0;
			while (num2 < num)
			{
				PCTrailPoint pctrailPoint = new PCTrailPoint();
				pctrailPoint.PointNumber = num3;
				pctrailPoint.Position = from + normalized * num2;
				circularBuffer.Add(pctrailPoint);
				this.InitialiseNewPoint(pctrailPoint);
				num3++;
				if (distanceBetweenPoints <= 0f)
				{
					break;
				}
				num2 += distanceBetweenPoints;
			}
			PCTrailPoint pctrailPoint2 = new PCTrailPoint();
			pctrailPoint2.PointNumber = num3;
			pctrailPoint2.Position = to;
			circularBuffer.Add(pctrailPoint2);
			this.InitialiseNewPoint(pctrailPoint2);
			PCTrail pctrail = new PCTrail(this.MaxNumberOfPoints);
			pctrail.Points = circularBuffer;
			this._fadingTrails.Add(pctrail);
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x0011D8D4 File Offset: 0x0011BAD4
		public void ClearSystem(bool emitState)
		{
			if (this._activeTrail != null)
			{
				this._activeTrail.Dispose();
				this._activeTrail = null;
			}
			if (this._fadingTrails != null)
			{
				foreach (PCTrail pctrail in this._fadingTrails)
				{
					if (pctrail != null)
					{
						pctrail.Dispose();
					}
				}
				this._fadingTrails.Clear();
			}
			this.Emit = emitState;
			this._emit = !emitState;
			this.CheckEmitChange();
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x0011D980 File Offset: 0x0011BB80
		public int NumSegments()
		{
			int num = 0;
			if (this._activeTrail != null && this.NumberOfActivePoints(this._activeTrail) != 0)
			{
				num++;
			}
			return num + this._fadingTrails.Count;
		}

		// Token: 0x04002C34 RID: 11316
		public PCTrailRendererData TrailData;

		// Token: 0x04002C35 RID: 11317
		public bool Emit;

		// Token: 0x04002C36 RID: 11318
		public int MaxNumberOfPoints = 50;

		// Token: 0x04002C37 RID: 11319
		protected bool _emit;

		// Token: 0x04002C38 RID: 11320
		private PCTrail _activeTrail;

		// Token: 0x04002C39 RID: 11321
		private List<PCTrail> _fadingTrails;

		// Token: 0x04002C3A RID: 11322
		protected Transform _t;

		// Token: 0x04002C3B RID: 11323
		private static Dictionary<Material, List<PCTrail>> _matToTrailList;

		// Token: 0x04002C3C RID: 11324
		private static List<Mesh> _toClean;

		// Token: 0x04002C3D RID: 11325
		private static bool _hasRenderer;

		// Token: 0x04002C3E RID: 11326
		private static int GlobalTrailRendererCount;
	}
}
