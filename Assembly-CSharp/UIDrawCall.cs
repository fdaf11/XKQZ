using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
[AddComponentMenu("NGUI/Internal/Draw Call")]
[ExecuteInEditMode]
public class UIDrawCall : MonoBehaviour
{
	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06001DAB RID: 7595 RVA: 0x00013985 File Offset: 0x00011B85
	[Obsolete("Use UIDrawCall.activeList")]
	public static BetterList<UIDrawCall> list
	{
		get
		{
			return UIDrawCall.mActiveList;
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06001DAC RID: 7596 RVA: 0x00013985 File Offset: 0x00011B85
	public static BetterList<UIDrawCall> activeList
	{
		get
		{
			return UIDrawCall.mActiveList;
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06001DAD RID: 7597 RVA: 0x0001398C File Offset: 0x00011B8C
	public static BetterList<UIDrawCall> inactiveList
	{
		get
		{
			return UIDrawCall.mInactiveList;
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06001DAE RID: 7598 RVA: 0x00013993 File Offset: 0x00011B93
	// (set) Token: 0x06001DAF RID: 7599 RVA: 0x0001399B File Offset: 0x00011B9B
	public int renderQueue
	{
		get
		{
			return this.mRenderQueue;
		}
		set
		{
			if (this.mRenderQueue != value)
			{
				this.mRenderQueue = value;
				if (this.mDynamicMat != null)
				{
					this.mDynamicMat.renderQueue = value;
				}
			}
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06001DB0 RID: 7600 RVA: 0x000139CD File Offset: 0x00011BCD
	// (set) Token: 0x06001DB1 RID: 7601 RVA: 0x000139F1 File Offset: 0x00011BF1
	public int sortingOrder
	{
		get
		{
			return (!(this.mRenderer != null)) ? 0 : this.mRenderer.sortingOrder;
		}
		set
		{
			if (this.mRenderer != null && this.mRenderer.sortingOrder != value)
			{
				this.mRenderer.sortingOrder = value;
			}
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06001DB2 RID: 7602 RVA: 0x00013A21 File Offset: 0x00011C21
	public int finalRenderQueue
	{
		get
		{
			return (!(this.mDynamicMat != null)) ? this.mRenderQueue : this.mDynamicMat.renderQueue;
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x00013A4A File Offset: 0x00011C4A
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06001DB4 RID: 7604 RVA: 0x00013A6F File Offset: 0x00011C6F
	// (set) Token: 0x06001DB5 RID: 7605 RVA: 0x00013A77 File Offset: 0x00011C77
	public Material baseMaterial
	{
		get
		{
			return this.mMaterial;
		}
		set
		{
			if (this.mMaterial != value)
			{
				this.mMaterial = value;
				this.mRebuildMat = true;
			}
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x00013A98 File Offset: 0x00011C98
	public Material dynamicMaterial
	{
		get
		{
			return this.mDynamicMat;
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x00013AA0 File Offset: 0x00011CA0
	// (set) Token: 0x06001DB8 RID: 7608 RVA: 0x00013AA8 File Offset: 0x00011CA8
	public Texture mainTexture
	{
		get
		{
			return this.mTexture;
		}
		set
		{
			this.mTexture = value;
			if (this.mDynamicMat != null)
			{
				this.mDynamicMat.mainTexture = value;
			}
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06001DB9 RID: 7609 RVA: 0x00013ACE File Offset: 0x00011CCE
	// (set) Token: 0x06001DBA RID: 7610 RVA: 0x00013AD6 File Offset: 0x00011CD6
	public Shader shader
	{
		get
		{
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				this.mShader = value;
				this.mRebuildMat = true;
			}
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06001DBB RID: 7611 RVA: 0x00013AF7 File Offset: 0x00011CF7
	public int triangles
	{
		get
		{
			return (!(this.mMesh != null)) ? 0 : this.mTriangles;
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06001DBC RID: 7612 RVA: 0x00013B16 File Offset: 0x00011D16
	public bool isClipped
	{
		get
		{
			return this.mClipCount != 0;
		}
	}

	// Token: 0x06001DBD RID: 7613 RVA: 0x000E8444 File Offset: 0x000E6644
	private void CreateMaterial()
	{
		this.mTextureClip = false;
		this.mLegacyShader = false;
		this.mClipCount = this.panel.clipCount;
		string text = (!(this.mShader != null)) ? ((!(this.mMaterial != null)) ? "Unlit/Transparent Colored" : this.mMaterial.shader.name) : this.mShader.name;
		text = text.Replace("GUI/Text Shader", "Unlit/Text");
		if (text.Length > 2 && text.get_Chars(text.Length - 2) == ' ')
		{
			int num = (int)text.get_Chars(text.Length - 1);
			if (num > 48 && num <= 57)
			{
				text = text.Substring(0, text.Length - 2);
			}
		}
		if (text.StartsWith("Hidden/"))
		{
			text = text.Substring(7);
		}
		text = text.Replace(" (SoftClip)", string.Empty);
		text = text.Replace(" (TextureClip)", string.Empty);
		if (this.panel.clipping == UIDrawCall.Clipping.TextureMask)
		{
			this.mTextureClip = true;
			this.shader = Shader.Find("Hidden/" + text + " (TextureClip)");
		}
		else if (this.mClipCount != 0)
		{
			this.shader = Shader.Find(string.Concat(new object[]
			{
				"Hidden/",
				text,
				" ",
				this.mClipCount
			}));
			if (this.shader == null)
			{
				this.shader = Shader.Find(text + " " + this.mClipCount);
			}
			if (this.shader == null && this.mClipCount == 1)
			{
				this.mLegacyShader = true;
				this.shader = Shader.Find(text + " (SoftClip)");
			}
		}
		else
		{
			this.shader = Shader.Find(text);
		}
		if (this.mMaterial != null)
		{
			this.mDynamicMat = new Material(this.mMaterial);
			this.mDynamicMat.name = "[NGUI] " + this.mMaterial.name;
			this.mDynamicMat.hideFlags = 12;
			this.mDynamicMat.CopyPropertiesFromMaterial(this.mMaterial);
			string[] shaderKeywords = this.mMaterial.shaderKeywords;
			for (int i = 0; i < shaderKeywords.Length; i++)
			{
				this.mDynamicMat.EnableKeyword(shaderKeywords[i]);
			}
			if (this.shader != null)
			{
				this.mDynamicMat.shader = this.shader;
			}
			else if (this.mClipCount != 0)
			{
				Debug.LogError(string.Concat(new object[]
				{
					text,
					" shader doesn't have a clipped shader version for ",
					this.mClipCount,
					" clip regions"
				}));
			}
		}
		else
		{
			this.mDynamicMat = new Material(this.shader);
			this.mDynamicMat.name = "[NGUI] " + this.shader.name;
			this.mDynamicMat.hideFlags = 12;
		}
	}

	// Token: 0x06001DBE RID: 7614 RVA: 0x000E8790 File Offset: 0x000E6990
	private Material RebuildMaterial()
	{
		NGUITools.DestroyImmediate(this.mDynamicMat);
		this.CreateMaterial();
		this.mDynamicMat.renderQueue = this.mRenderQueue;
		if (this.mTexture != null)
		{
			this.mDynamicMat.mainTexture = this.mTexture;
		}
		if (this.mRenderer != null)
		{
			this.mRenderer.sharedMaterials = new Material[]
			{
				this.mDynamicMat
			};
		}
		return this.mDynamicMat;
	}

	// Token: 0x06001DBF RID: 7615 RVA: 0x000E8814 File Offset: 0x000E6A14
	private void UpdateMaterials()
	{
		if (this.mRebuildMat || this.mDynamicMat == null || this.mClipCount != this.panel.clipCount || this.mTextureClip != (this.panel.clipping == UIDrawCall.Clipping.TextureMask))
		{
			this.RebuildMaterial();
			this.mRebuildMat = false;
		}
		else if (this.mRenderer.sharedMaterial != this.mDynamicMat)
		{
			this.mRenderer.sharedMaterials = new Material[]
			{
				this.mDynamicMat
			};
		}
	}

	// Token: 0x06001DC0 RID: 7616 RVA: 0x000E88B4 File Offset: 0x000E6AB4
	public void UpdateGeometry(int widgetCount)
	{
		this.widgetCount = widgetCount;
		int size = this.verts.size;
		if (size > 0 && size == this.uvs.size && size == this.cols.size && size % 4 == 0)
		{
			if (this.mFilter == null)
			{
				this.mFilter = base.gameObject.GetComponent<MeshFilter>();
			}
			if (this.mFilter == null)
			{
				this.mFilter = base.gameObject.AddComponent<MeshFilter>();
			}
			if (this.verts.size < 65000)
			{
				int num = (size >> 1) * 3;
				bool flag = this.mIndices == null || this.mIndices.Length != num;
				if (this.mMesh == null)
				{
					this.mMesh = new Mesh();
					this.mMesh.hideFlags = 4;
					this.mMesh.name = ((!(this.mMaterial != null)) ? "[NGUI] Mesh" : ("[NGUI] " + this.mMaterial.name));
					this.mMesh.MarkDynamic();
					flag = true;
				}
				bool flag2 = this.uvs.buffer.Length != this.verts.buffer.Length || this.cols.buffer.Length != this.verts.buffer.Length || (this.norms.buffer != null && this.norms.buffer.Length != this.verts.buffer.Length) || (this.tans.buffer != null && this.tans.buffer.Length != this.verts.buffer.Length);
				if (!flag2 && this.panel.renderQueue != UIPanel.RenderQueue.Automatic)
				{
					flag2 = (this.mMesh == null || this.mMesh.vertexCount != this.verts.buffer.Length);
				}
				if (!flag2 && this.verts.size << 1 < this.verts.buffer.Length)
				{
					flag2 = true;
				}
				this.mTriangles = this.verts.size >> 1;
				if (flag2 || this.verts.buffer.Length > 65000)
				{
					if (flag2 || this.mMesh.vertexCount != this.verts.size)
					{
						this.mMesh.Clear();
						flag = true;
					}
					this.mMesh.vertices = this.verts.ToArray();
					this.mMesh.uv = this.uvs.ToArray();
					this.mMesh.colors32 = this.cols.ToArray();
					if (this.norms != null)
					{
						this.mMesh.normals = this.norms.ToArray();
					}
					if (this.tans != null)
					{
						this.mMesh.tangents = this.tans.ToArray();
					}
				}
				else
				{
					if (this.mMesh.vertexCount != this.verts.buffer.Length)
					{
						this.mMesh.Clear();
						flag = true;
					}
					this.mMesh.vertices = this.verts.buffer;
					this.mMesh.uv = this.uvs.buffer;
					this.mMesh.colors32 = this.cols.buffer;
					if (this.norms != null)
					{
						this.mMesh.normals = this.norms.buffer;
					}
					if (this.tans != null)
					{
						this.mMesh.tangents = this.tans.buffer;
					}
				}
				if (flag)
				{
					this.mIndices = this.GenerateCachedIndexBuffer(size, num);
					this.mMesh.triangles = this.mIndices;
				}
				if (flag2 || !this.alwaysOnScreen)
				{
					this.mMesh.RecalculateBounds();
				}
				this.mFilter.mesh = this.mMesh;
			}
			else
			{
				this.mTriangles = 0;
				if (this.mFilter.mesh != null)
				{
					this.mFilter.mesh.Clear();
				}
				Debug.LogError("Too many vertices on one panel: " + this.verts.size);
			}
			if (this.mRenderer == null)
			{
				this.mRenderer = base.gameObject.GetComponent<MeshRenderer>();
			}
			if (this.mRenderer == null)
			{
				this.mRenderer = base.gameObject.AddComponent<MeshRenderer>();
			}
			this.UpdateMaterials();
		}
		else
		{
			if (this.mFilter.mesh != null)
			{
				this.mFilter.mesh.Clear();
			}
			Debug.LogError("UIWidgets must fill the buffer with 4 vertices per quad. Found " + size);
		}
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.norms.Clear();
		this.tans.Clear();
	}

	// Token: 0x06001DC1 RID: 7617 RVA: 0x000E8DFC File Offset: 0x000E6FFC
	private int[] GenerateCachedIndexBuffer(int vertexCount, int indexCount)
	{
		int i = 0;
		int count = UIDrawCall.mCache.Count;
		while (i < count)
		{
			int[] array = UIDrawCall.mCache[i];
			if (array != null && array.Length == indexCount)
			{
				return array;
			}
			i++;
		}
		int[] array2 = new int[indexCount];
		int num = 0;
		for (int j = 0; j < vertexCount; j += 4)
		{
			array2[num++] = j;
			array2[num++] = j + 1;
			array2[num++] = j + 2;
			array2[num++] = j + 2;
			array2[num++] = j + 3;
			array2[num++] = j;
		}
		if (UIDrawCall.mCache.Count > 10)
		{
			UIDrawCall.mCache.RemoveAt(0);
		}
		UIDrawCall.mCache.Add(array2);
		return array2;
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x000E8ED8 File Offset: 0x000E70D8
	private void OnWillRenderObject()
	{
		this.UpdateMaterials();
		if (this.onRender != null)
		{
			this.onRender(this.mDynamicMat ?? this.mMaterial);
		}
		if (this.mDynamicMat == null || this.mClipCount == 0)
		{
			return;
		}
		if (this.mTextureClip)
		{
			Vector4 drawCallClipRange = this.panel.drawCallClipRange;
			Vector2 clipSoftness = this.panel.clipSoftness;
			Vector2 vector;
			vector..ctor(1000f, 1000f);
			if (clipSoftness.x > 0f)
			{
				vector.x = drawCallClipRange.z / clipSoftness.x;
			}
			if (clipSoftness.y > 0f)
			{
				vector.y = drawCallClipRange.w / clipSoftness.y;
			}
			this.mDynamicMat.SetVector(UIDrawCall.ClipRange[0], new Vector4(-drawCallClipRange.x / drawCallClipRange.z, -drawCallClipRange.y / drawCallClipRange.w, 1f / drawCallClipRange.z, 1f / drawCallClipRange.w));
			this.mDynamicMat.SetTexture("_ClipTex", this.clipTexture);
		}
		else if (!this.mLegacyShader)
		{
			UIPanel parentPanel = this.panel;
			int num = 0;
			while (parentPanel != null)
			{
				if (parentPanel.hasClipping)
				{
					float angle = 0f;
					Vector4 drawCallClipRange2 = parentPanel.drawCallClipRange;
					if (parentPanel != this.panel)
					{
						Vector3 vector2 = parentPanel.cachedTransform.InverseTransformPoint(this.panel.cachedTransform.position);
						drawCallClipRange2.x -= vector2.x;
						drawCallClipRange2.y -= vector2.y;
						Vector3 eulerAngles = this.panel.cachedTransform.rotation.eulerAngles;
						Vector3 eulerAngles2 = parentPanel.cachedTransform.rotation.eulerAngles;
						Vector3 vector3 = eulerAngles2 - eulerAngles;
						vector3.x = NGUIMath.WrapAngle(vector3.x);
						vector3.y = NGUIMath.WrapAngle(vector3.y);
						vector3.z = NGUIMath.WrapAngle(vector3.z);
						if (Mathf.Abs(vector3.x) > 0.001f || Mathf.Abs(vector3.y) > 0.001f)
						{
							Debug.LogWarning("Panel can only be clipped properly if X and Y rotation is left at 0", this.panel);
						}
						angle = vector3.z;
					}
					this.SetClipping(num++, drawCallClipRange2, parentPanel.clipSoftness, angle);
				}
				parentPanel = parentPanel.parentPanel;
			}
		}
		else
		{
			Vector2 clipSoftness2 = this.panel.clipSoftness;
			Vector4 drawCallClipRange3 = this.panel.drawCallClipRange;
			Vector2 mainTextureOffset;
			mainTextureOffset..ctor(-drawCallClipRange3.x / drawCallClipRange3.z, -drawCallClipRange3.y / drawCallClipRange3.w);
			Vector2 mainTextureScale;
			mainTextureScale..ctor(1f / drawCallClipRange3.z, 1f / drawCallClipRange3.w);
			Vector2 vector4;
			vector4..ctor(1000f, 1000f);
			if (clipSoftness2.x > 0f)
			{
				vector4.x = drawCallClipRange3.z / clipSoftness2.x;
			}
			if (clipSoftness2.y > 0f)
			{
				vector4.y = drawCallClipRange3.w / clipSoftness2.y;
			}
			this.mDynamicMat.mainTextureOffset = mainTextureOffset;
			this.mDynamicMat.mainTextureScale = mainTextureScale;
			this.mDynamicMat.SetVector("_ClipSharpness", vector4);
		}
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x000E9288 File Offset: 0x000E7488
	private void SetClipping(int index, Vector4 cr, Vector2 soft, float angle)
	{
		angle *= -0.017453292f;
		Vector2 vector;
		vector..ctor(1000f, 1000f);
		if (soft.x > 0f)
		{
			vector.x = cr.z / soft.x;
		}
		if (soft.y > 0f)
		{
			vector.y = cr.w / soft.y;
		}
		if (index < UIDrawCall.ClipRange.Length)
		{
			this.mDynamicMat.SetVector(UIDrawCall.ClipRange[index], new Vector4(-cr.x / cr.z, -cr.y / cr.w, 1f / cr.z, 1f / cr.w));
			this.mDynamicMat.SetVector(UIDrawCall.ClipArgs[index], new Vector4(vector.x, vector.y, Mathf.Sin(angle), Mathf.Cos(angle)));
		}
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x00013B24 File Offset: 0x00011D24
	private void OnEnable()
	{
		this.mRebuildMat = true;
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x000E9390 File Offset: 0x000E7590
	private void OnDisable()
	{
		this.depthStart = int.MaxValue;
		this.depthEnd = int.MinValue;
		this.panel = null;
		this.manager = null;
		this.mMaterial = null;
		this.mTexture = null;
		this.clipTexture = null;
		if (this.mRenderer != null)
		{
			this.mRenderer.sharedMaterials = new Material[0];
		}
		NGUITools.DestroyImmediate(this.mDynamicMat);
		this.mDynamicMat = null;
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x00013B2D File Offset: 0x00011D2D
	private void OnDestroy()
	{
		NGUITools.DestroyImmediate(this.mMesh);
		this.mMesh = null;
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x00013B41 File Offset: 0x00011D41
	public static UIDrawCall Create(UIPanel panel, Material mat, Texture tex, Shader shader)
	{
		return UIDrawCall.Create(null, panel, mat, tex, shader);
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x000E940C File Offset: 0x000E760C
	private static UIDrawCall Create(string name, UIPanel pan, Material mat, Texture tex, Shader shader)
	{
		UIDrawCall uidrawCall = UIDrawCall.Create(name);
		uidrawCall.gameObject.layer = pan.cachedGameObject.layer;
		uidrawCall.baseMaterial = mat;
		uidrawCall.mainTexture = tex;
		uidrawCall.shader = shader;
		uidrawCall.renderQueue = pan.startingRenderQueue;
		uidrawCall.sortingOrder = pan.sortingOrder;
		uidrawCall.manager = pan;
		return uidrawCall;
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x000E946C File Offset: 0x000E766C
	private static UIDrawCall Create(string name)
	{
		if (UIDrawCall.mInactiveList.size > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mInactiveList.Pop();
			UIDrawCall.mActiveList.Add(uidrawCall);
			if (name != null)
			{
				uidrawCall.name = name;
			}
			NGUITools.SetActive(uidrawCall.gameObject, true);
			return uidrawCall;
		}
		GameObject gameObject = new GameObject(name);
		Object.DontDestroyOnLoad(gameObject);
		UIDrawCall uidrawCall2 = gameObject.AddComponent<UIDrawCall>();
		UIDrawCall.mActiveList.Add(uidrawCall2);
		return uidrawCall2;
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x000E94DC File Offset: 0x000E76DC
	public static void ClearAll()
	{
		bool isPlaying = Application.isPlaying;
		int i = UIDrawCall.mActiveList.size;
		while (i > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mActiveList[--i];
			if (uidrawCall)
			{
				if (isPlaying)
				{
					NGUITools.SetActive(uidrawCall.gameObject, false);
				}
				else
				{
					NGUITools.DestroyImmediate(uidrawCall.gameObject);
				}
			}
		}
		UIDrawCall.mActiveList.Clear();
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x00013B4D File Offset: 0x00011D4D
	public static void ReleaseAll()
	{
		UIDrawCall.ClearAll();
		UIDrawCall.ReleaseInactive();
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x000E9550 File Offset: 0x000E7750
	public static void ReleaseInactive()
	{
		int i = UIDrawCall.mInactiveList.size;
		while (i > 0)
		{
			UIDrawCall uidrawCall = UIDrawCall.mInactiveList[--i];
			if (uidrawCall)
			{
				NGUITools.DestroyImmediate(uidrawCall.gameObject);
			}
		}
		UIDrawCall.mInactiveList.Clear();
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x000E95A4 File Offset: 0x000E77A4
	public static int Count(UIPanel panel)
	{
		int num = 0;
		for (int i = 0; i < UIDrawCall.mActiveList.size; i++)
		{
			if (UIDrawCall.mActiveList[i].manager == panel)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x000E95F0 File Offset: 0x000E77F0
	public static void Destroy(UIDrawCall dc)
	{
		if (dc)
		{
			dc.onRender = null;
			if (Application.isPlaying)
			{
				if (UIDrawCall.mActiveList.Remove(dc))
				{
					NGUITools.SetActive(dc.gameObject, false);
					UIDrawCall.mInactiveList.Add(dc);
				}
			}
			else
			{
				UIDrawCall.mActiveList.Remove(dc);
				NGUITools.DestroyImmediate(dc.gameObject);
			}
		}
	}

	// Token: 0x040021D4 RID: 8660
	private const int maxIndexBufferCache = 10;

	// Token: 0x040021D5 RID: 8661
	private static BetterList<UIDrawCall> mActiveList = new BetterList<UIDrawCall>();

	// Token: 0x040021D6 RID: 8662
	private static BetterList<UIDrawCall> mInactiveList = new BetterList<UIDrawCall>();

	// Token: 0x040021D7 RID: 8663
	[HideInInspector]
	[NonSerialized]
	public int widgetCount;

	// Token: 0x040021D8 RID: 8664
	[HideInInspector]
	[NonSerialized]
	public int depthStart = int.MaxValue;

	// Token: 0x040021D9 RID: 8665
	[HideInInspector]
	[NonSerialized]
	public int depthEnd = int.MinValue;

	// Token: 0x040021DA RID: 8666
	[HideInInspector]
	[NonSerialized]
	public UIPanel manager;

	// Token: 0x040021DB RID: 8667
	[HideInInspector]
	[NonSerialized]
	public UIPanel panel;

	// Token: 0x040021DC RID: 8668
	[HideInInspector]
	[NonSerialized]
	public Texture2D clipTexture;

	// Token: 0x040021DD RID: 8669
	[HideInInspector]
	[NonSerialized]
	public bool alwaysOnScreen;

	// Token: 0x040021DE RID: 8670
	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector3> verts = new BetterList<Vector3>();

	// Token: 0x040021DF RID: 8671
	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector3> norms = new BetterList<Vector3>();

	// Token: 0x040021E0 RID: 8672
	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector4> tans = new BetterList<Vector4>();

	// Token: 0x040021E1 RID: 8673
	[HideInInspector]
	[NonSerialized]
	public BetterList<Vector2> uvs = new BetterList<Vector2>();

	// Token: 0x040021E2 RID: 8674
	[HideInInspector]
	[NonSerialized]
	public BetterList<Color32> cols = new BetterList<Color32>();

	// Token: 0x040021E3 RID: 8675
	private Material mMaterial;

	// Token: 0x040021E4 RID: 8676
	private Texture mTexture;

	// Token: 0x040021E5 RID: 8677
	private Shader mShader;

	// Token: 0x040021E6 RID: 8678
	private int mClipCount;

	// Token: 0x040021E7 RID: 8679
	private Transform mTrans;

	// Token: 0x040021E8 RID: 8680
	private Mesh mMesh;

	// Token: 0x040021E9 RID: 8681
	private MeshFilter mFilter;

	// Token: 0x040021EA RID: 8682
	private MeshRenderer mRenderer;

	// Token: 0x040021EB RID: 8683
	private Material mDynamicMat;

	// Token: 0x040021EC RID: 8684
	private int[] mIndices;

	// Token: 0x040021ED RID: 8685
	private bool mRebuildMat = true;

	// Token: 0x040021EE RID: 8686
	private bool mLegacyShader;

	// Token: 0x040021EF RID: 8687
	private int mRenderQueue = 3000;

	// Token: 0x040021F0 RID: 8688
	private int mTriangles;

	// Token: 0x040021F1 RID: 8689
	[NonSerialized]
	public bool isDirty;

	// Token: 0x040021F2 RID: 8690
	[NonSerialized]
	private bool mTextureClip;

	// Token: 0x040021F3 RID: 8691
	public UIDrawCall.OnRenderCallback onRender;

	// Token: 0x040021F4 RID: 8692
	private static List<int[]> mCache = new List<int[]>(10);

	// Token: 0x040021F5 RID: 8693
	private static int[] ClipRange = new int[]
	{
		Shader.PropertyToID("_ClipRange0"),
		Shader.PropertyToID("_ClipRange1"),
		Shader.PropertyToID("_ClipRange2"),
		Shader.PropertyToID("_ClipRange4")
	};

	// Token: 0x040021F6 RID: 8694
	private static int[] ClipArgs = new int[]
	{
		Shader.PropertyToID("_ClipArgs0"),
		Shader.PropertyToID("_ClipArgs1"),
		Shader.PropertyToID("_ClipArgs2"),
		Shader.PropertyToID("_ClipArgs3")
	};

	// Token: 0x020004B2 RID: 1202
	public enum Clipping
	{
		// Token: 0x040021F8 RID: 8696
		None,
		// Token: 0x040021F9 RID: 8697
		TextureMask,
		// Token: 0x040021FA RID: 8698
		SoftClip = 3,
		// Token: 0x040021FB RID: 8699
		ConstrainButDontClip
	}

	// Token: 0x020004B3 RID: 1203
	// (Invoke) Token: 0x06001DD0 RID: 7632
	public delegate void OnRenderCallback(Material mat);
}
