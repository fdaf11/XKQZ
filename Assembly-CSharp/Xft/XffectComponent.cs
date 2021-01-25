using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000577 RID: 1399
	[ExecuteInEditMode]
	[AddComponentMenu("Xffect/XffectComponent")]
	public class XffectComponent : MonoBehaviour
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x00111050 File Offset: 0x0010F250
		// (set) Token: 0x060022CB RID: 8907 RVA: 0x0001724B File Offset: 0x0001544B
		public Camera MyCamera
		{
			get
			{
				if (this.mCamera == null || !this.mCamera.gameObject.activeInHierarchy || !this.mCamera.enabled)
				{
					this.FindMyCamera();
				}
				return this.mCamera;
			}
			set
			{
				this.mCamera = value;
			}
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x001110A0 File Offset: 0x0010F2A0
		public void FindMyCamera()
		{
			float num = float.PositiveInfinity;
			int num2 = 1 << base.gameObject.layer;
			Camera[] array = Object.FindObjectsOfType(typeof(Camera)) as Camera[];
			int i = 0;
			int num3 = array.Length;
			while (i < num3)
			{
				Camera camera = array[i];
				if (camera.enabled)
				{
					if ((camera.cullingMask & num2) != 0)
					{
						float num4 = Vector3.Distance(base.gameObject.transform.position, camera.gameObject.transform.position);
						if (num4 < num)
						{
							num = num4;
							this.mCamera = camera;
						}
					}
				}
				i++;
			}
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x00017254 File Offset: 0x00015454
		private void Awake()
		{
			this.FindMyCamera();
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x0001725C File Offset: 0x0001545C
		private void DestoryMeshObject(GameObject obj)
		{
			Object.Destroy(obj);
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x00111150 File Offset: 0x0010F350
		private MeshFilter CreateMeshObj(Material mat)
		{
			GameObject gameObject = new GameObject("xftmesh " + mat.name);
			gameObject.layer = base.gameObject.layer;
			this.MeshList.Add(gameObject);
			gameObject.AddComponent("MeshFilter");
			gameObject.AddComponent("MeshRenderer");
			XffectComponent.SetActive(gameObject, true);
			MeshFilter meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
			MeshRenderer meshRenderer = (MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer));
			meshRenderer.castShadows = false;
			meshRenderer.receiveShadows = false;
			meshRenderer.renderer.sharedMaterial = mat;
			if (this.UseWith2DSprite)
			{
				meshRenderer.sortingLayerName = this.SortingLayerName;
				meshRenderer.sortingOrder = this.SortingOrder;
			}
			meshFilter.sharedMesh = new Mesh();
			return meshFilter;
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x00111224 File Offset: 0x0010F424
		public void Initialize()
		{
			if (this.EflList.Count > 0)
			{
				return;
			}
			List<GameObject> list = new List<GameObject>();
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.name.Contains("xftmesh"))
				{
					list.Add(transform.gameObject);
				}
			}
			foreach (object obj2 in base.transform)
			{
				Transform transform2 = (Transform)obj2;
				EffectLayer effectLayer = (EffectLayer)transform2.GetComponent(typeof(EffectLayer));
				if (!(effectLayer == null))
				{
					if (effectLayer.Material == null)
					{
						Debug.LogWarning("effect layer: " + effectLayer.gameObject.name + " has no material, please assign a material first!");
					}
					else
					{
						Material material = effectLayer.Material;
						material.renderQueue = material.shader.renderQueue;
						material.renderQueue += effectLayer.Depth;
						this.EflList.Add(effectLayer);
						if (this.MergeSameMaterialMesh)
						{
							if (!this.MatDic.ContainsKey(material.name))
							{
								MeshFilter meshFilter = this.CreateMeshObj(material);
								this.MatDic[material.name] = new VertexPool(meshFilter.sharedMesh, material);
							}
						}
						else
						{
							MeshFilter meshFilter2 = this.CreateMeshObj(material);
							this.MatList.Add(new VertexPool(meshFilter2.sharedMesh, material));
						}
					}
				}
			}
			foreach (GameObject obj3 in list)
			{
				this.DestoryMeshObject(obj3);
			}
			foreach (GameObject gameObject in this.MeshList)
			{
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = Vector3.zero;
				gameObject.transform.rotation = Quaternion.identity;
				Vector3 zero = Vector3.zero;
				zero.x = 1f / gameObject.transform.parent.lossyScale.x;
				zero.y = 1f / gameObject.transform.parent.lossyScale.y;
				zero.z = 1f / gameObject.transform.parent.lossyScale.z;
				gameObject.transform.localScale = zero * this.Scale;
			}
			for (int i = 0; i < this.EflList.Count; i++)
			{
				EffectLayer effectLayer2 = this.EflList[i];
				if (this.MergeSameMaterialMesh)
				{
					effectLayer2.Vertexpool = this.MatDic[effectLayer2.Material.name];
				}
				else if (this.EflList.Count != this.MatList.Count)
				{
					Debug.LogError("something wrong with the no merge mesh mat list!");
					effectLayer2.Vertexpool = this.MatList[0];
				}
				else
				{
					effectLayer2.Vertexpool = this.MatList[i];
				}
			}
			base.transform.localScale = Vector3.one;
			foreach (EffectLayer effectLayer3 in this.EflList)
			{
				effectLayer3.StartCustom();
			}
			this.EventList.Clear();
			foreach (object obj4 in base.transform)
			{
				Transform transform3 = (Transform)obj4;
				XftEventComponent component = transform3.GetComponent<XftEventComponent>();
				if (!(component == null))
				{
					this.EventList.Add(component);
					component.Initialize(this);
				}
			}
			this.Initialized = true;
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x00017264 File Offset: 0x00015464
		private void Start()
		{
			if (!this.Initialized)
			{
				this.Initialize();
			}
			this.LastTime = (double)Time.realtimeSinceStartup;
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x00111764 File Offset: 0x0010F964
		public void Update()
		{
			this.CurTime = (double)Time.realtimeSinceStartup;
			float num = (float)(this.CurTime - this.LastTime);
			if (num > 0.1f)
			{
				num = 0.0333f;
			}
			if (this.Paused)
			{
				this.LastTime = this.CurTime;
				return;
			}
			if (!this.UpdateWhenOffScreen && !this.IsInCameraView() && !this.EditView)
			{
				this.LastTime = this.CurTime;
				return;
			}
			if (!this.IgnoreTimeScale)
			{
				num *= Time.timeScale;
			}
			this.ElapsedTime += num;
			for (int i = 0; i < this.EflList.Count; i++)
			{
				if (this.EflList[i] == null)
				{
					return;
				}
				EffectLayer effectLayer = this.EflList[i];
				if (this.ElapsedTime > effectLayer.StartTime && XffectComponent.IsActive(effectLayer.gameObject))
				{
					effectLayer.FixedUpdateCustom(num);
				}
			}
			for (int j = 0; j < this.EventList.Count; j++)
			{
				XftEventComponent xftEventComponent = this.EventList[j];
				if (XffectComponent.IsActive(xftEventComponent.gameObject))
				{
					xftEventComponent.UpdateCustom(num);
				}
			}
			this.LastTime = this.CurTime;
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x001118BC File Offset: 0x0010FABC
		public void ResetEditorEvents()
		{
			for (int i = 0; i < this.EventList.Count; i++)
			{
				XftEventComponent xftEventComponent = this.EventList[i];
				xftEventComponent.ResetCustom();
			}
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x001118F8 File Offset: 0x0010FAF8
		private void DoFinish()
		{
			this.mIsActive = false;
			for (int i = 0; i < this.EflList.Count; i++)
			{
				EffectLayer effectLayer = this.EflList[i];
				effectLayer.Reset();
			}
			this.DeActive();
			this.ElapsedTime = 0f;
			if (this.AutoDestroy)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x00111964 File Offset: 0x0010FB64
		private bool IsInCameraView()
		{
			for (int i = 0; i < this.MeshList.Count; i++)
			{
				GameObject gameObject = this.MeshList[i];
				if (gameObject != null)
				{
					MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
					if (component.isVisible)
					{
						return true;
					}
				}
			}
			for (int j = 0; j < this.EflList.Count; j++)
			{
				EffectLayer effectLayer = this.EflList[j];
				Vector3 position = effectLayer.ClientTransform.position;
				Vector3 vector = this.MyCamera.WorldToViewportPoint(position);
				if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f && vector.z > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x00111A5C File Offset: 0x0010FC5C
		private void UpdateMeshObj()
		{
			for (int i = 0; i < this.MeshList.Count; i++)
			{
				GameObject gameObject = this.MeshList[i];
				if (gameObject != null)
				{
					gameObject.transform.position = Vector3.zero;
					gameObject.transform.rotation = Quaternion.identity;
				}
			}
			if (this.MergeSameMaterialMesh)
			{
				foreach (KeyValuePair<string, VertexPool> keyValuePair in this.MatDic)
				{
					keyValuePair.Value.LateUpdate();
				}
			}
			else
			{
				for (int j = 0; j < this.MatList.Count; j++)
				{
					VertexPool vertexPool = this.MatList[j];
					vertexPool.LateUpdate();
				}
			}
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x00111B58 File Offset: 0x0010FD58
		private void LateUpdate()
		{
			if (!this.UpdateWhenOffScreen && !this.IsInCameraView() && !this.EditView)
			{
				return;
			}
			this.UpdateMeshObj();
			if (this.ElapsedTime > this.LifeTime && this.LifeTime >= 0f)
			{
				this.DoFinish();
			}
			else if (this.LifeTime < 0f && this.EflList.Count > 0)
			{
				float deltaTime = (float)(this.CurTime - this.LastTime);
				bool flag = true;
				for (int i = 0; i < this.EflList.Count; i++)
				{
					EffectLayer effectLayer = this.EflList[i];
					if (!effectLayer.EmitOver(deltaTime))
					{
						flag = false;
					}
				}
				if (flag)
				{
					this.DoFinish();
				}
			}
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x0000264F File Offset: 0x0000084F
		private void OnDrawGizmosSelected()
		{
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x00017283 File Offset: 0x00015483
		public void Active()
		{
			if (!this.Initialized)
			{
				this.Initialize();
			}
			base.gameObject.SetActive(true);
			this.Reset();
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x00111C30 File Offset: 0x0010FE30
		public void DeActive()
		{
			for (int i = 0; i < this.EventList.Count; i++)
			{
				XftEventComponent xftEventComponent = this.EventList[i];
				xftEventComponent.ResetCustom();
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x000172A8 File Offset: 0x000154A8
		public bool IsPlaying
		{
			get
			{
				return this.mIsActive;
			}
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x00111C78 File Offset: 0x0010FE78
		public void Reset()
		{
			this.mIsActive = true;
			this.ElapsedTime = 0f;
			this.Start();
			for (int i = 0; i < this.EflList.Count; i++)
			{
				EffectLayer effectLayer = this.EflList[i];
				effectLayer.Reset();
			}
			for (int j = 0; j < this.EventList.Count; j++)
			{
				XftEventComponent xftEventComponent = this.EventList[j];
				xftEventComponent.ResetCustom();
			}
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x00111CFC File Offset: 0x0010FEFC
		public void StopSmoothly(float fadeTime)
		{
			for (int i = 0; i < this.EflList.Count; i++)
			{
				EffectLayer effectLayer = this.EflList[i];
				effectLayer.StopSmoothly(fadeTime);
			}
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000172B0 File Offset: 0x000154B0
		public void ActiveNoInterrupt()
		{
			if (XffectComponent.IsActive(base.gameObject))
			{
				return;
			}
			this.Active();
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x00111D3C File Offset: 0x0010FF3C
		public void SetScale(Vector2 scale, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName)
				{
					effectLayer.SetScale(scale);
				}
			}
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x00111DAC File Offset: 0x0010FFAC
		public void SetColor(Color color, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName)
				{
					effectLayer.SetColor(color);
				}
			}
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x00111E1C File Offset: 0x0011001C
		public void SetRotation(float angle, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName)
				{
					effectLayer.SetRotation(angle);
				}
			}
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x00111E8C File Offset: 0x0011008C
		public void SetGravityGoal(Transform goal, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName && effectLayer.GravityAffectorEnable)
				{
					effectLayer.SetArractionAffectorGoal(goal);
				}
			}
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x00111F08 File Offset: 0x00110108
		public void SetCollisionGoalPos(Transform pos, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName && effectLayer.UseCollisionDetection)
				{
					effectLayer.SetCollisionGoalPos(pos);
				}
			}
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x0000264F File Offset: 0x0000084F
		public void ResetCamera(Camera cam)
		{
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x00111F84 File Offset: 0x00110184
		public void SetClient(Transform client)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				effectLayer.SetClient(client);
			}
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x00111FE0 File Offset: 0x001101E0
		public void SetDirectionAxis(Vector3 axis)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				effectLayer.OriVelocityAxis = axis;
			}
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x0011203C File Offset: 0x0011023C
		public void SetEmitPosition(Vector3 pos)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				effectLayer.EmitPoint = pos;
			}
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x00112098 File Offset: 0x00110298
		public void SetCollisionGoalPos(Transform pos)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.UseCollisionDetection)
				{
					effectLayer.SetCollisionGoalPos(pos);
				}
			}
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x00112100 File Offset: 0x00110300
		public void SetArractionAffectorGoal(Transform goal)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.GravityAffectorEnable)
				{
					effectLayer.SetArractionAffectorGoal(goal);
				}
			}
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x00111E8C File Offset: 0x0011008C
		public void SetArractionAffectorGoal(Transform goal, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName && effectLayer.GravityAffectorEnable)
				{
					effectLayer.SetArractionAffectorGoal(goal);
				}
			}
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x00112168 File Offset: 0x00110368
		public EffectNode EmitByPos(Vector3 pos)
		{
			if (this.EflList.Count > 1)
			{
				Debug.LogWarning("EmitByPos only support one effect layer!");
			}
			EffectLayer effectLayer = this.EflList[0];
			return effectLayer.EmitByPos(pos);
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x001121A4 File Offset: 0x001103A4
		public void StopEmit()
		{
			for (int i = 0; i < this.EflList.Count; i++)
			{
				EffectLayer effectLayer = this.EflList[i];
				effectLayer.StopEmit();
			}
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000172C9 File Offset: 0x000154C9
		public static bool IsActive(GameObject obj)
		{
			return obj.activeSelf;
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x0000FF26 File Offset: 0x0000E126
		public static void SetActive(GameObject obj, bool flag)
		{
			obj.SetActive(flag);
		}

		// Token: 0x04002A47 RID: 10823
		public static string CurVersion = "4.4.1";

		// Token: 0x04002A48 RID: 10824
		public string MyVersion = "4.4.1";

		// Token: 0x04002A49 RID: 10825
		private Dictionary<string, VertexPool> MatDic = new Dictionary<string, VertexPool>();

		// Token: 0x04002A4A RID: 10826
		private List<EffectLayer> EflList = new List<EffectLayer>();

		// Token: 0x04002A4B RID: 10827
		private List<XftEventComponent> EventList = new List<XftEventComponent>();

		// Token: 0x04002A4C RID: 10828
		private List<VertexPool> MatList = new List<VertexPool>();

		// Token: 0x04002A4D RID: 10829
		public float LifeTime = -1f;

		// Token: 0x04002A4E RID: 10830
		public bool IgnoreTimeScale;

		// Token: 0x04002A4F RID: 10831
		protected float ElapsedTime;

		// Token: 0x04002A50 RID: 10832
		protected bool Initialized;

		// Token: 0x04002A51 RID: 10833
		protected List<GameObject> MeshList = new List<GameObject>();

		// Token: 0x04002A52 RID: 10834
		protected double LastTime;

		// Token: 0x04002A53 RID: 10835
		protected double CurTime;

		// Token: 0x04002A54 RID: 10836
		public bool EditView;

		// Token: 0x04002A55 RID: 10837
		public float Scale = 1f;

		// Token: 0x04002A56 RID: 10838
		public int MaxFps = 60;

		// Token: 0x04002A57 RID: 10839
		public bool AutoDestroy;

		// Token: 0x04002A58 RID: 10840
		public bool MergeSameMaterialMesh = true;

		// Token: 0x04002A59 RID: 10841
		public bool Paused;

		// Token: 0x04002A5A RID: 10842
		public bool UpdateWhenOffScreen = true;

		// Token: 0x04002A5B RID: 10843
		public bool UseWith2DSprite;

		// Token: 0x04002A5C RID: 10844
		public string SortingLayerName = "Default";

		// Token: 0x04002A5D RID: 10845
		public int SortingOrder;

		// Token: 0x04002A5E RID: 10846
		public float PlaybackTime = 1f;

		// Token: 0x04002A5F RID: 10847
		protected bool mIsActive;

		// Token: 0x04002A60 RID: 10848
		protected Camera mCamera;
	}
}
