using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Token: 0x02000679 RID: 1657
[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("Miscellaneous/Mesh Animator")]
public class MeshAnimator : MonoBehaviour
{
	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x06002870 RID: 10352 RVA: 0x0001AA48 File Offset: 0x00018C48
	public MeshAnimation currentAnimation
	{
		get
		{
			if (this.currentAnimIndex < this.animations.Length && this.currentAnimIndex > -1)
			{
				return this.animations[this.currentAnimIndex];
			}
			return null;
		}
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x001402D0 File Offset: 0x0013E4D0
	private void Start()
	{
		if (this.animations.Length == 0)
		{
			Debug.LogWarning("No animations for MeshAnimator on object: " + base.name + ". Disabling.", this);
			base.enabled = false;
			return;
		}
		for (int i = 0; i < this.animations.Length; i++)
		{
			if (!(this.animations[i] == null))
			{
				if (!this.animIndexes.ContainsKey(this.animations[i].name.ToLower()))
				{
					this.animIndexes.Add(this.animations[i].name.ToLower(), i);
				}
				this.animations[i].GenerateFrames(this.baseMesh);
			}
		}
		if (!this.meshFilter)
		{
			this.meshFilter = base.GetComponent<MeshFilter>();
		}
		if (!MeshAnimator.mMeshCount.ContainsKey(this.baseMesh))
		{
			MeshAnimator.mMeshCount.Add(this.baseMesh, 1);
		}
		else
		{
			Dictionary<Mesh, int> dictionary2;
			Dictionary<Mesh, int> dictionary = dictionary2 = MeshAnimator.mMeshCount;
			Mesh mesh2;
			Mesh mesh = mesh2 = this.baseMesh;
			int num = dictionary2[mesh2];
			dictionary[mesh] = num + 1;
		}
		if (this.playAutomatically)
		{
			this.Play(this.defaultAnimation.name);
		}
		else
		{
			this.isPaused = true;
		}
		if (MeshAnimator.cfThread == null)
		{
			MeshAnimator.shutDownThread = false;
			MeshAnimator.cfThread = new Thread(new ThreadStart(MeshAnimator.GenerateCrossfadeFrames));
			MeshAnimator.cfThread.Start();
		}
	}

	// Token: 0x06002872 RID: 10354 RVA: 0x0001AA78 File Offset: 0x00018C78
	private void OnBecameVisible()
	{
		this.mIsVisible = true;
		if (this.OnVisibilityChanged != null)
		{
			this.OnVisibilityChanged.Invoke(this.mIsVisible);
		}
	}

	// Token: 0x06002873 RID: 10355 RVA: 0x0001AA9D File Offset: 0x00018C9D
	private void OnBecameInvisible()
	{
		this.mIsVisible = false;
		if (this.OnVisibilityChanged != null)
		{
			this.OnVisibilityChanged.Invoke(this.mIsVisible);
		}
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x00140450 File Offset: 0x0013E650
	private void OnEnable()
	{
		this.mTransform = base.transform;
		float num = 1f / (float)this.FPS;
		base.InvokeRepeating("UpdateFrameInvoked", 0.001f, num);
		if (this.resetOnEnable && this.meshFilter)
		{
			if (this.playAutomatically)
			{
				this.Play(this.defaultAnimation.name);
			}
			else
			{
				this.isPaused = true;
			}
			if (this.currentAnimation != null)
			{
				this.currentAnimation.GenerateFrameIfNeeded(this.baseMesh, this.currentFrame);
				this.currentAnimation.DisplayFrame(this.meshFilter, this.currentFrame, -1);
			}
		}
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x0001AAC2 File Offset: 0x00018CC2
	private void OnDisable()
	{
		this.isCrossfading = false;
		this.currentAnimIndex = -1;
		this.pingPong = false;
		this.queuedAnims.Clear();
		base.CancelInvoke();
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x0014050C File Offset: 0x0013E70C
	private void OnDestroy()
	{
		if (!MeshAnimator.mMeshCount.ContainsKey(this.baseMesh))
		{
			return;
		}
		Dictionary<Mesh, int> dictionary2;
		Dictionary<Mesh, int> dictionary = dictionary2 = MeshAnimator.mMeshCount;
		Mesh mesh2;
		Mesh mesh = mesh2 = this.baseMesh;
		int num = dictionary2[mesh2];
		dictionary[mesh] = num - 1;
		this.ReturnCrossfadeToPool();
		if (MeshAnimator.mMeshCount[this.baseMesh] <= 0)
		{
			MeshAnimator.mMeshCount.Remove(this.baseMesh);
			foreach (KeyValuePair<string, Mesh[]> keyValuePair in MeshAnimation.generatedFrames[this.baseMesh])
			{
				for (int i = 0; i < keyValuePair.Value.Length; i++)
				{
					Object.DestroyImmediate(keyValuePair.Value[i]);
				}
			}
			MeshAnimation.generatedFrames.Remove(this.baseMesh);
			for (int j = 0; j < this.animations.Length; j++)
			{
				this.animations[j].Reset();
			}
			if (MeshAnimator.crossFadePool.ContainsKey(this.baseMesh))
			{
				while (MeshAnimator.crossFadePool[this.baseMesh].Count > 0)
				{
					Object.Destroy(MeshAnimator.crossFadePool[this.baseMesh].Pop());
				}
				MeshAnimator.crossFadePool.Remove(this.baseMesh);
			}
		}
		if (MeshAnimator.mMeshCount.Count == 0 && MeshAnimator.cfThread != null)
		{
			MeshAnimator.cfThread = null;
			MeshAnimator.shutDownThread = true;
			MeshAnimator.crossfadeAnimators.Clear();
		}
	}

	// Token: 0x06002877 RID: 10359 RVA: 0x001406C0 File Offset: 0x0013E8C0
	private void UpdateFrameInvoked()
	{
		if ((!this.mIsVisible && !this.updateWhenOffscreen) || this.isPaused || this.currentAnimation == null || this.speed == 0f || this.currentAnimation.playbackSpeed == 0f)
		{
			return;
		}
		MeshAnimation currentAnimation = this.currentAnimation;
		float num = (float)((this.LODLevels.Length <= this.currentLodLevel) ? this.FPS : this.LODLevels[this.currentLodLevel].fps);
		num = (float)this.FPS / num;
		float num2 = Mathf.Abs(this.speed * currentAnimation.playbackSpeed * num);
		if (num2 < 1f)
		{
			float num3 = Time.time - this.mLastFrame;
			float num4 = 1f / (float)this.FPS / num2;
			if (num3 < num4)
			{
				return;
			}
		}
		this.mLastFrame = Time.time;
		int num5 = Mathf.CeilToInt(num2);
		if (this.speed * currentAnimation.playbackSpeed < 0f)
		{
			num5 *= -1;
		}
		if (this.pingPong)
		{
			num5 *= -1;
		}
		int num6 = this.currentFrame;
		int num7 = this.currentFrame + num5;
		if (currentAnimation.wrapMode == 4 && (num7 >= currentAnimation.frames.Length || num7 < 0))
		{
			num5 *= -1;
			this.pingPong = !this.pingPong;
		}
		this.currentFrame += num5;
		if (this.currentFrame >= currentAnimation.frames.Length)
		{
			if (this.OnAnimationFinished != null)
			{
				this.OnAnimationFinished.Invoke(currentAnimation.name);
			}
			if (this.queuedAnims.Count > 0)
			{
				this.Play(this.queuedAnims.Dequeue());
				return;
			}
			if (currentAnimation.wrapMode == 2)
			{
				this.currentFrame = 0;
			}
			else if (currentAnimation.wrapMode != 4)
			{
				return;
			}
		}
		if (this.currentFrame < 0)
		{
			if (this.OnAnimationFinished != null)
			{
				this.OnAnimationFinished.Invoke(this.currentAnimation.name);
			}
			if (this.queuedAnims.Count > 0)
			{
				this.Play(this.queuedAnims.Dequeue());
				return;
			}
			if (currentAnimation.wrapMode != 2 && currentAnimation.wrapMode != 4)
			{
				return;
			}
			this.currentFrame = currentAnimation.frames.Length - 1;
		}
		currentAnimation.GenerateFrameIfNeeded(this.baseMesh, this.currentFrame);
		if (this.isCrossfading)
		{
			if (Time.time >= this.crossFadeStart + this.crossFadeSpeed)
			{
				this.isCrossfading = false;
				this.ReturnCrossfadeToPool();
				currentAnimation.DisplayFrame(this.meshFilter, this.currentFrame, num6);
			}
			else
			{
				if (this.crossFadeMesh == null)
				{
					this.crossFadeMesh = this.GetCrossfadeFromPool();
				}
				float num8 = (Time.time - this.crossFadeStart) / this.crossFadeSpeed;
				int num9 = Mathf.FloorToInt(num8 * this.crossFadeSpeed * (float)this.FPS);
				if (!this.threadedCrossfade || num9 > this.crossFadeFrameGen)
				{
					Vector3[] frame = currentAnimation.GetFrame(this.currentFrame);
					for (int i = 0; i < this.crossFadeFrame.Length; i++)
					{
						this.crossFadePositions[i] = Vector3.Lerp(this.crossFadeFrame[i], frame[i], num8);
					}
				}
				else
				{
					this.crossFadePositions = this.crossFadeFrames[num9];
				}
				this.crossFadeMesh.vertices = this.crossFadePositions;
				this.meshFilter.sharedMesh = this.crossFadeMesh;
				this.crossFadeMesh.RecalculateNormals();
			}
		}
		else
		{
			currentAnimation.DisplayFrame(this.meshFilter, this.currentFrame, num6);
		}
		if (this.OnFrameUpdated != null)
		{
			this.OnFrameUpdated.Invoke();
		}
		if (this.eventReciever)
		{
			for (int j = num6; j < this.currentFrame + 1; j++)
			{
				currentAnimation.FireEvents(this.eventReciever, j);
			}
		}
		if (this.LODLevels.Length > 0 && (this.LODCamera || Camera.main))
		{
			if (this.LODCamera == null)
			{
				this.LODCamera = Camera.main.transform;
			}
			float sqrMagnitude = (this.LODCamera.position - this.mTransform.position).sqrMagnitude;
			int num10 = 0;
			for (int k = 0; k < this.LODLevels.Length; k++)
			{
				if (sqrMagnitude > this.LODLevels[k].distance * this.LODLevels[k].distance)
				{
					num10 = k;
				}
			}
			if (this.currentLodLevel != num10)
			{
				this.currentLodLevel = num10;
				base.CancelInvoke("UpdateFrameInvoked");
				float num11 = 1f / (float)this.LODLevels[this.currentLodLevel].fps;
				base.InvokeRepeating("UpdateFrameInvoked", num11, num11);
			}
		}
	}

	// Token: 0x06002878 RID: 10360 RVA: 0x00140C14 File Offset: 0x0013EE14
	private Mesh GetCrossfadeFromPool()
	{
		if (MeshAnimator.crossFadePool.ContainsKey(this.baseMesh) && MeshAnimator.crossFadePool[this.baseMesh].Count > 0)
		{
			return MeshAnimator.crossFadePool[this.baseMesh].Pop();
		}
		return (Mesh)Object.Instantiate(this.baseMesh);
	}

	// Token: 0x06002879 RID: 10361 RVA: 0x00140C78 File Offset: 0x0013EE78
	private void ReturnCrossfadeToPool()
	{
		if (this.crossFadeMesh)
		{
			if (!MeshAnimator.crossFadePool.ContainsKey(this.baseMesh))
			{
				MeshAnimator.crossFadePool.Add(this.baseMesh, new Stack<Mesh>());
			}
			MeshAnimator.crossFadePool[this.baseMesh].Push(this.crossFadeMesh);
			this.crossFadeMesh = null;
		}
	}

	// Token: 0x0600287A RID: 10362 RVA: 0x00140CE4 File Offset: 0x0013EEE4
	private static void GenerateCrossfadeFrames()
	{
		while (!MeshAnimator.shutDownThread)
		{
			try
			{
				if (MeshAnimator.crossfadeAnimators.Count > 0)
				{
					MeshAnimator meshAnimator = MeshAnimator.crossfadeAnimators.Pop();
					if (!meshAnimator.threadedCrossfade)
					{
						continue;
					}
					int num = (int)(meshAnimator.crossFadeSpeed * (float)meshAnimator.FPS);
					meshAnimator.crossFadeFrameGen = -1;
					meshAnimator.crossFadeFrames = new Vector3[num][];
					for (int i = 0; i < meshAnimator.crossFadeFrames.Length; i++)
					{
						meshAnimator.crossFadeFrames[i] = new Vector3[meshAnimator.crossFadePositions.Length];
					}
					Vector3[] frame = meshAnimator.animations[meshAnimator.currentAnimIndex].GetFrame(0);
					for (int j = 0; j < meshAnimator.crossFadeFrames.Length; j++)
					{
						for (int k = 0; k < meshAnimator.crossFadeFrames[j].Length; k++)
						{
							meshAnimator.crossFadeFrames[j][k] = Vector3.Lerp(meshAnimator.crossFadeFrame[k], frame[k], (float)j / (float)num);
						}
					}
					meshAnimator.crossFadeFrameGen++;
				}
				MeshAnimator.waitHandle.WaitOne();
			}
			catch
			{
			}
		}
	}

	// Token: 0x0600287B RID: 10363 RVA: 0x0001AAEA File Offset: 0x00018CEA
	public void Crossfade(int index)
	{
		this.Crossfade(index, 0.1f);
	}

	// Token: 0x0600287C RID: 10364 RVA: 0x0001AAF8 File Offset: 0x00018CF8
	public void Crossfade(string anim)
	{
		this.Crossfade(anim, 0.1f);
	}

	// Token: 0x0600287D RID: 10365 RVA: 0x00140E4C File Offset: 0x0013F04C
	public void Crossfade(int index, float speed)
	{
		if (this.currentAnimation == null)
		{
			this.crossFadeFrame = this.meshFilter.sharedMesh.vertices;
		}
		else
		{
			this.crossFadeFrame = this.currentAnimation.GetFrame(this.currentFrame);
		}
		this.crossFadePositions = new Vector3[this.crossFadeFrame.Length];
		Array.Copy(this.crossFadeFrame, this.crossFadePositions, this.crossFadeFrame.Length);
		this.isCrossfading = true;
		this.crossFadeSpeed = speed;
		this.crossFadeStart = Time.time;
		this.crossFadeFrameGen = -1;
		this.Play(index);
		if (this.threadedCrossfade)
		{
			if (!MeshAnimator.crossfadeAnimators.Contains(this))
			{
				MeshAnimator.crossfadeAnimators.Push(this);
			}
			MeshAnimator.waitHandle.Set();
		}
	}

	// Token: 0x0600287E RID: 10366 RVA: 0x0001AB06 File Offset: 0x00018D06
	public void Crossfade(string anim, float speed)
	{
		if (string.IsNullOrEmpty(anim))
		{
			return;
		}
		anim = anim.ToLower();
		if (!this.animIndexes.ContainsKey(anim))
		{
			return;
		}
		this.Crossfade(this.animIndexes[anim], speed);
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x0001AB41 File Offset: 0x00018D41
	public void Play()
	{
		this.isPaused = false;
	}

	// Token: 0x06002880 RID: 10368 RVA: 0x0001AB4A File Offset: 0x00018D4A
	public void Play(string anim)
	{
		if (string.IsNullOrEmpty(anim))
		{
			return;
		}
		anim = anim.ToLower();
		if (!this.animIndexes.ContainsKey(anim))
		{
			return;
		}
		this.Play(this.animIndexes[anim]);
	}

	// Token: 0x06002881 RID: 10369 RVA: 0x00140F20 File Offset: 0x0013F120
	public void Play(int index)
	{
		if (this.animations.Length <= index || this.currentAnimIndex == index)
		{
			return;
		}
		this.queuedAnims.Clear();
		this.currentAnimIndex = index;
		this.currentFrame = 0;
		this.pingPong = false;
		this.isPaused = false;
	}

	// Token: 0x06002882 RID: 10370 RVA: 0x00140F70 File Offset: 0x0013F170
	public void PlayRandom(params string[] anim)
	{
		int num = Random.Range(0, anim.Length);
		string text = anim[num].ToLower();
		if (!this.animIndexes.ContainsKey(text))
		{
			return;
		}
		this.Play(this.animIndexes[text]);
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x0001AB84 File Offset: 0x00018D84
	public void PlayQueued(string anim)
	{
		this.queuedAnims.Enqueue(anim);
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x0001AB92 File Offset: 0x00018D92
	public void Pause()
	{
		this.isPaused = true;
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x0001AB9B File Offset: 0x00018D9B
	public void RestartAnim()
	{
		this.currentFrame = 0;
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x0001ABA4 File Offset: 0x00018DA4
	public MeshAnimation GetClip(string clipname)
	{
		if (this.animIndexes.ContainsKey(clipname))
		{
			return this.animations[this.animIndexes[clipname]];
		}
		return null;
	}

	// Token: 0x040032CC RID: 13004
	public Mesh baseMesh;

	// Token: 0x040032CD RID: 13005
	public MeshAnimation defaultAnimation;

	// Token: 0x040032CE RID: 13006
	public MeshAnimation[] animations;

	// Token: 0x040032CF RID: 13007
	public float speed = 1f;

	// Token: 0x040032D0 RID: 13008
	public bool updateWhenOffscreen;

	// Token: 0x040032D1 RID: 13009
	public bool playAutomatically = true;

	// Token: 0x040032D2 RID: 13010
	public bool resetOnEnable = true;

	// Token: 0x040032D3 RID: 13011
	public GameObject eventReciever;

	// Token: 0x040032D4 RID: 13012
	public int FPS = 30;

	// Token: 0x040032D5 RID: 13013
	public bool threadedCrossfade = true;

	// Token: 0x040032D6 RID: 13014
	[HideInInspector]
	public MeshFilter meshFilter;

	// Token: 0x040032D7 RID: 13015
	public Action<string> OnAnimationFinished;

	// Token: 0x040032D8 RID: 13016
	public Action OnFrameUpdated;

	// Token: 0x040032D9 RID: 13017
	public Action<bool> OnVisibilityChanged;

	// Token: 0x040032DA RID: 13018
	public int currentFrame;

	// Token: 0x040032DB RID: 13019
	public Transform LODCamera;

	// Token: 0x040032DC RID: 13020
	public MeshAnimator.MeshAnimatorLODLevel[] LODLevels = new MeshAnimator.MeshAnimatorLODLevel[0];

	// Token: 0x040032DD RID: 13021
	private Dictionary<string, int> animIndexes = new Dictionary<string, int>();

	// Token: 0x040032DE RID: 13022
	private int currentAnimIndex;

	// Token: 0x040032DF RID: 13023
	private bool mIsVisible = true;

	// Token: 0x040032E0 RID: 13024
	private bool pingPong;

	// Token: 0x040032E1 RID: 13025
	private bool isPaused;

	// Token: 0x040032E2 RID: 13026
	private bool isCrossfading;

	// Token: 0x040032E3 RID: 13027
	private float crossFadeSpeed = 0.1f;

	// Token: 0x040032E4 RID: 13028
	private float crossFadeStart;

	// Token: 0x040032E5 RID: 13029
	private Vector3[] crossFadeFrame;

	// Token: 0x040032E6 RID: 13030
	private Vector3[] crossFadePositions;

	// Token: 0x040032E7 RID: 13031
	private Mesh crossFadeMesh;

	// Token: 0x040032E8 RID: 13032
	private Queue<string> queuedAnims = new Queue<string>();

	// Token: 0x040032E9 RID: 13033
	private float mLastFrame;

	// Token: 0x040032EA RID: 13034
	private Vector3[][] crossFadeFrames;

	// Token: 0x040032EB RID: 13035
	private int crossFadeFrameGen;

	// Token: 0x040032EC RID: 13036
	private int currentLodLevel;

	// Token: 0x040032ED RID: 13037
	private Transform mTransform;

	// Token: 0x040032EE RID: 13038
	private static Thread cfThread;

	// Token: 0x040032EF RID: 13039
	private static AutoResetEvent waitHandle = new AutoResetEvent(false);

	// Token: 0x040032F0 RID: 13040
	private static Stack<MeshAnimator> crossfadeAnimators = new Stack<MeshAnimator>();

	// Token: 0x040032F1 RID: 13041
	private static bool shutDownThread = false;

	// Token: 0x040032F2 RID: 13042
	private static Dictionary<Mesh, int> mMeshCount = new Dictionary<Mesh, int>();

	// Token: 0x040032F3 RID: 13043
	private static Dictionary<Mesh, Stack<Mesh>> crossFadePool = new Dictionary<Mesh, Stack<Mesh>>();

	// Token: 0x0200067A RID: 1658
	[Serializable]
	public class MeshAnimatorLODLevel
	{
		// Token: 0x040032F4 RID: 13044
		public int fps;

		// Token: 0x040032F5 RID: 13045
		public float distance;
	}
}
