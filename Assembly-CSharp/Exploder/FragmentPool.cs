using System;
using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
	// Token: 0x020000D0 RID: 208
	public class FragmentPool : MonoBehaviour
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00035320 File Offset: 0x00033520
		public static FragmentPool Instance
		{
			get
			{
				if (FragmentPool.instance == null)
				{
					GameObject gameObject = new GameObject("FragmentRoot");
					FragmentPool.instance = gameObject.AddComponent<FragmentPool>();
				}
				return FragmentPool.instance;
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00004E71 File Offset: 0x00003071
		private void Awake()
		{
			FragmentPool.instance = this;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00004E79 File Offset: 0x00003079
		private void OnDestroy()
		{
			this.DestroyFragments();
			FragmentPool.instance = null;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x00004E87 File Offset: 0x00003087
		public int PoolSize
		{
			get
			{
				return this.pool.Length;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00004E91 File Offset: 0x00003091
		public Fragment[] Pool
		{
			get
			{
				return this.pool;
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00035358 File Offset: 0x00033558
		public List<Fragment> GetAvailableFragments(int size)
		{
			if (size > this.pool.Length)
			{
				Debug.LogError("Requesting pool size higher than allocated! Please call Allocate first! " + size);
				return null;
			}
			if (size == this.pool.Length)
			{
				return new List<Fragment>(this.pool);
			}
			List<Fragment> list = new List<Fragment>();
			int num = 0;
			foreach (Fragment fragment in this.pool)
			{
				if (!fragment.activeObj)
				{
					list.Add(fragment);
					num++;
				}
				if (num == size)
				{
					return list;
				}
			}
			foreach (Fragment fragment2 in this.pool)
			{
				if (!fragment2.visible)
				{
					list.Add(fragment2);
					num++;
				}
				if (num == size)
				{
					return list;
				}
			}
			if (num < size)
			{
				foreach (Fragment fragment3 in this.pool)
				{
					if (fragment3.IsSleeping() && fragment3.visible)
					{
						list.Add(fragment3);
						num++;
					}
					if (num == size)
					{
						return list;
					}
				}
			}
			if (num < size)
			{
				foreach (Fragment fragment4 in this.pool)
				{
					if (!fragment4.IsSleeping() && fragment4.visible)
					{
						list.Add(fragment4);
						num++;
					}
					if (num == size)
					{
						return list;
					}
				}
			}
			return null;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000354F4 File Offset: 0x000336F4
		public void Allocate(int poolSize, bool useMeshColliders, bool use2dCollision)
		{
			if (this.pool == null || this.pool.Length < poolSize || useMeshColliders != this.meshColliders)
			{
				this.DestroyFragments();
				this.pool = new Fragment[poolSize];
				this.meshColliders = useMeshColliders;
				for (int i = 0; i < poolSize; i++)
				{
					GameObject gameObject = new GameObject("fragment_" + i);
					gameObject.AddComponent<MeshFilter>();
					gameObject.AddComponent<MeshRenderer>();
					if (use2dCollision)
					{
						gameObject.AddComponent<PolygonCollider2D>();
						gameObject.AddComponent<Rigidbody2D>();
					}
					else
					{
						if (useMeshColliders)
						{
							MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
							meshCollider.convex = true;
						}
						else
						{
							gameObject.AddComponent<BoxCollider>();
						}
						gameObject.AddComponent<Rigidbody>();
					}
					gameObject.AddComponent<ExploderOption>();
					Fragment fragment = gameObject.AddComponent<Fragment>();
					gameObject.transform.parent = base.gameObject.transform;
					this.pool[i] = fragment;
					ExploderUtils.SetActiveRecursively(gameObject.gameObject, false);
					fragment.RefreshComponentsCache();
					fragment.Sleep();
				}
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000355FC File Offset: 0x000337FC
		public void WakeUp()
		{
			foreach (Fragment fragment in this.pool)
			{
				fragment.WakeUp();
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00035630 File Offset: 0x00033830
		public void Sleep()
		{
			foreach (Fragment fragment in this.pool)
			{
				fragment.Sleep();
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00035664 File Offset: 0x00033864
		public void DestroyFragments()
		{
			if (this.pool != null)
			{
				foreach (Fragment fragment in this.pool)
				{
					if (fragment)
					{
						Object.Destroy(fragment.gameObject);
					}
				}
				this.pool = null;
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000356B8 File Offset: 0x000338B8
		public void DeactivateFragments()
		{
			if (this.pool != null)
			{
				foreach (Fragment fragment in this.pool)
				{
					if (fragment)
					{
						fragment.Deactivate();
					}
				}
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00035700 File Offset: 0x00033900
		public void SetDeactivateOptions(DeactivateOptions options, FadeoutOptions fadeoutOptions, float timeout)
		{
			if (this.pool != null)
			{
				foreach (Fragment fragment in this.pool)
				{
					fragment.deactivateOptions = options;
					fragment.deactivateTimeout = timeout;
					fragment.fadeoutOptions = fadeoutOptions;
				}
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0003574C File Offset: 0x0003394C
		public void SetExplodableFragments(bool explodable, bool dontUseTag)
		{
			if (this.pool != null)
			{
				if (dontUseTag)
				{
					foreach (Fragment fragment in this.pool)
					{
						fragment.gameObject.AddComponent<Explodable>();
					}
				}
				else if (explodable)
				{
					foreach (Fragment fragment2 in this.pool)
					{
						fragment2.tag = ExploderObject.Tag;
					}
				}
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000357D4 File Offset: 0x000339D4
		public void SetFragmentPhysicsOptions(ExploderObject.FragmentOption options, bool physics2d)
		{
			if (this.pool != null)
			{
				RigidbodyConstraints rigidbodyConstraints = 0;
				if (options.FreezePositionX)
				{
					rigidbodyConstraints |= 2;
				}
				if (options.FreezePositionY)
				{
					rigidbodyConstraints |= 4;
				}
				if (options.FreezePositionZ)
				{
					rigidbodyConstraints |= 8;
				}
				if (options.FreezeRotationX)
				{
					rigidbodyConstraints |= 16;
				}
				if (options.FreezeRotationY)
				{
					rigidbodyConstraints |= 32;
				}
				if (options.FreezeRotationZ)
				{
					rigidbodyConstraints |= 64;
				}
				foreach (Fragment fragment in this.pool)
				{
					fragment.gameObject.layer = LayerMask.NameToLayer(options.Layer);
					fragment.SetConstraints(rigidbodyConstraints);
					fragment.DisableColliders(options.DisableColliders, this.meshColliders, physics2d);
				}
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0003589C File Offset: 0x00033A9C
		public void SetSFXOptions(ExploderObject.SFXOption sfx)
		{
			if (this.pool != null)
			{
				this.HitSoundTimeout = sfx.HitSoundTimeout;
				this.MaxEmitters = sfx.EmitersMax;
				for (int i = 0; i < this.pool.Length; i++)
				{
					this.pool[i].SetSFX(sfx, i < this.MaxEmitters);
				}
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000358FC File Offset: 0x00033AFC
		public List<Fragment> GetActiveFragments()
		{
			if (this.pool != null)
			{
				List<Fragment> list = new List<Fragment>(this.pool.Length);
				foreach (Fragment fragment in this.pool)
				{
					if (ExploderUtils.IsActive(fragment.gameObject))
					{
						list.Add(fragment);
					}
				}
				return list;
			}
			return null;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00004E99 File Offset: 0x00003099
		private void Update()
		{
			this.fragmentSoundTimeout -= Time.deltaTime;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00004EAD File Offset: 0x000030AD
		public void OnFragmentHit()
		{
			this.fragmentSoundTimeout = this.HitSoundTimeout;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00004EBB File Offset: 0x000030BB
		public bool CanPlayHitSound()
		{
			return this.fragmentSoundTimeout <= 0f;
		}

		// Token: 0x040003C0 RID: 960
		private static FragmentPool instance;

		// Token: 0x040003C1 RID: 961
		private Fragment[] pool;

		// Token: 0x040003C2 RID: 962
		private bool meshColliders;

		// Token: 0x040003C3 RID: 963
		private float fragmentSoundTimeout;

		// Token: 0x040003C4 RID: 964
		public float HitSoundTimeout = 1f;

		// Token: 0x040003C5 RID: 965
		public int MaxEmitters = 1000;
	}
}
