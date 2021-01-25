using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200068B RID: 1675
[ExecuteInEditMode]
public class ProFlare : MonoBehaviour
{
	// Token: 0x060028BF RID: 10431 RVA: 0x0001AD7C File Offset: 0x00018F7C
	private void Awake()
	{
		this.DisabledPlayMode = false;
		this.Initialised = false;
		this.thisTransform = base.transform;
	}

	// Token: 0x060028C0 RID: 10432 RVA: 0x00141FBC File Offset: 0x001401BC
	private void Start()
	{
		this.thisTransform = base.transform;
		if (this._Atlas != null && this.FlareBatches.Length == 0)
		{
			this.PopulateFlareBatches();
		}
		if (!this.Initialised)
		{
			this.Init();
		}
		for (int i = 0; i < this.Elements.Count; i++)
		{
			this.Elements[i].flareAtlas = this._Atlas;
			this.Elements[i].flare = this;
		}
	}

	// Token: 0x060028C1 RID: 10433 RVA: 0x00142050 File Offset: 0x00140250
	private void PopulateFlareBatches()
	{
		ProFlareBatch[] array = Object.FindObjectsOfType(typeof(ProFlareBatch)) as ProFlareBatch[];
		int num = 0;
		foreach (ProFlareBatch proFlareBatch in array)
		{
			if (proFlareBatch._atlas == this._Atlas)
			{
				num++;
			}
		}
		this.FlareBatches = new ProFlareBatch[num];
		int num2 = 0;
		foreach (ProFlareBatch proFlareBatch2 in array)
		{
			if (proFlareBatch2._atlas == this._Atlas)
			{
				this.FlareBatches[num2] = proFlareBatch2;
				num2++;
			}
		}
	}

	// Token: 0x060028C2 RID: 10434 RVA: 0x00142108 File Offset: 0x00140308
	private void Init()
	{
		if (this.thisTransform == null)
		{
			this.thisTransform = base.transform;
		}
		if (this._Atlas == null)
		{
			return;
		}
		this.PopulateFlareBatches();
		for (int i = 0; i < this.Elements.Count; i++)
		{
			this.Elements[i].flareAtlas = this._Atlas;
		}
		for (int j = 0; j < this.FlareBatches.Length; j++)
		{
			if (this.FlareBatches[j] != null && this.FlareBatches[j]._atlas == this._Atlas)
			{
				this.FlareBatches[j].AddFlare(this);
			}
		}
		this.Initialised = true;
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x0001AD98 File Offset: 0x00018F98
	public void ReInitialise()
	{
		this.Initialised = false;
		this.Init();
	}

	// Token: 0x060028C4 RID: 10436 RVA: 0x001421DC File Offset: 0x001403DC
	private void OnEnable()
	{
		if (Application.isPlaying && this.DisabledPlayMode)
		{
			this.DisabledPlayMode = false;
		}
		else
		{
			if (this._Atlas)
			{
				for (int i = 0; i < this.FlareBatches.Length; i++)
				{
					if (this.FlareBatches[i] != null)
					{
						this.FlareBatches[i].dirty = true;
					}
					else
					{
						this.Initialised = false;
					}
				}
			}
			this.Init();
		}
	}

	// Token: 0x060028C5 RID: 10437 RVA: 0x00142268 File Offset: 0x00140468
	private void OnDisable()
	{
		if (Application.isPlaying)
		{
			this.DisabledPlayMode = true;
		}
		else
		{
			for (int i = 0; i < this.FlareBatches.Length; i++)
			{
				if (this.FlareBatches[i] != null)
				{
					this.FlareBatches[i].RemoveFlare(this);
					this.FlareBatches[i].dirty = true;
				}
				else
				{
					this.Initialised = false;
				}
			}
		}
	}

	// Token: 0x060028C6 RID: 10438 RVA: 0x001422E0 File Offset: 0x001404E0
	private void OnDestroy()
	{
		for (int i = 0; i < this.FlareBatches.Length; i++)
		{
			if (this.FlareBatches[i] != null)
			{
				this.FlareBatches[i].RemoveFlare(this);
				this.FlareBatches[i].dirty = true;
			}
			else
			{
				this.Initialised = false;
			}
		}
	}

	// Token: 0x060028C7 RID: 10439 RVA: 0x0001ADA7 File Offset: 0x00018FA7
	private void Update()
	{
		if (!this.Initialised)
		{
			this.Init();
		}
	}

	// Token: 0x060028C8 RID: 10440 RVA: 0x0001ADBA File Offset: 0x00018FBA
	private void OnDrawGizmos()
	{
		Gizmos.color = this.GlobalTintColor;
		Gizmos.DrawSphere(base.transform.position, 0.1f);
	}

	// Token: 0x0400334D RID: 13133
	public ProFlareAtlas _Atlas;

	// Token: 0x0400334E RID: 13134
	public ProFlareBatch[] FlareBatches = new ProFlareBatch[0];

	// Token: 0x0400334F RID: 13135
	public bool EditingAtlas;

	// Token: 0x04003350 RID: 13136
	public bool isVisible = true;

	// Token: 0x04003351 RID: 13137
	public List<ProFlareElement> Elements = new List<ProFlareElement>();

	// Token: 0x04003352 RID: 13138
	public Transform thisTransform;

	// Token: 0x04003353 RID: 13139
	public Vector3 LensPosition;

	// Token: 0x04003354 RID: 13140
	public bool EditGlobals;

	// Token: 0x04003355 RID: 13141
	public float GlobalScale = 100f;

	// Token: 0x04003356 RID: 13142
	public bool MultiplyScaleByTransformScale;

	// Token: 0x04003357 RID: 13143
	public float GlobalBrightness = 1f;

	// Token: 0x04003358 RID: 13144
	public Color GlobalTintColor = Color.white;

	// Token: 0x04003359 RID: 13145
	public bool useMaxDistance = true;

	// Token: 0x0400335A RID: 13146
	public bool useDistanceScale = true;

	// Token: 0x0400335B RID: 13147
	public bool useDistanceFade = true;

	// Token: 0x0400335C RID: 13148
	public float GlobalMaxDistance = 150f;

	// Token: 0x0400335D RID: 13149
	public bool UseAngleLimit;

	// Token: 0x0400335E RID: 13150
	public float maxAngle = 90f;

	// Token: 0x0400335F RID: 13151
	public bool UseAngleScale;

	// Token: 0x04003360 RID: 13152
	public bool UseAngleBrightness = true;

	// Token: 0x04003361 RID: 13153
	public bool UseAngleCurve;

	// Token: 0x04003362 RID: 13154
	public AnimationCurve AngleCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x04003363 RID: 13155
	public LayerMask mask = 1;

	// Token: 0x04003364 RID: 13156
	public bool RaycastPhysics;

	// Token: 0x04003365 RID: 13157
	public bool Occluded;

	// Token: 0x04003366 RID: 13158
	public float OccludeScale = 1f;

	// Token: 0x04003367 RID: 13159
	public float OffScreenFadeDist = 0.2f;

	// Token: 0x04003368 RID: 13160
	public bool useDynamicEdgeBoost;

	// Token: 0x04003369 RID: 13161
	public float DynamicEdgeBoost = 1f;

	// Token: 0x0400336A RID: 13162
	public float DynamicEdgeBrightness = 0.1f;

	// Token: 0x0400336B RID: 13163
	public float DynamicEdgeRange = 0.3f;

	// Token: 0x0400336C RID: 13164
	public float DynamicEdgeBias = -0.1f;

	// Token: 0x0400336D RID: 13165
	public AnimationCurve DynamicEdgeCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.5f, 1f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x0400336E RID: 13166
	public bool useDynamicCenterBoost;

	// Token: 0x0400336F RID: 13167
	public float DynamicCenterBoost = 1f;

	// Token: 0x04003370 RID: 13168
	public float DynamicCenterBrightness = 0.2f;

	// Token: 0x04003371 RID: 13169
	public float DynamicCenterRange = 0.3f;

	// Token: 0x04003372 RID: 13170
	public float DynamicCenterBias;

	// Token: 0x04003373 RID: 13171
	public bool neverCull;

	// Token: 0x04003374 RID: 13172
	private bool Initialised;

	// Token: 0x04003375 RID: 13173
	public bool DisabledPlayMode;
}
