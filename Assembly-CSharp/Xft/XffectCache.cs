using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	// Token: 0x02000576 RID: 1398
	[AddComponentMenu("Xffect/XffectCache")]
	public class XffectCache : MonoBehaviour
	{
		// Token: 0x060022B8 RID: 8888 RVA: 0x0001721A File Offset: 0x0001541A
		private void Awake()
		{
			this.mInited = false;
			this.Init();
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x00017229 File Offset: 0x00015429
		public bool IsInited()
		{
			return this.mInited;
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x00110944 File Offset: 0x0010EB44
		public void Init()
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				XffectComponent component = transform.GetComponent<XffectComponent>();
				if (component != null)
				{
					component.Initialize();
					if (!this.EffectDic.ContainsKey(transform.name))
					{
						this.EffectDic[transform.name] = new List<XffectComponent>();
					}
					this.EffectDic[transform.name].Add(component);
				}
				CompositeXffect component2 = transform.GetComponent<CompositeXffect>();
				if (component2 != null)
				{
					component2.Initialize();
					if (!this.CompEffectDic.ContainsKey(transform.name))
					{
						this.CompEffectDic[transform.name] = new List<CompositeXffect>();
					}
					this.CompEffectDic[transform.name].Add(component2);
				}
			}
			this.mInited = true;
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x00110A64 File Offset: 0x0010EC64
		public XffectComponent AddXffect(string name)
		{
			Transform transform = base.transform.Find(name);
			if (transform == null)
			{
				Debug.Log("object:" + name + "doesn't exist!");
				return null;
			}
			Transform transform2 = Object.Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;
			transform2.parent = base.transform;
			XffectComponent.SetActive(transform2.gameObject, false);
			transform2.gameObject.name = transform.gameObject.name;
			XffectComponent component = transform2.GetComponent<XffectComponent>();
			this.EffectDic[name].Add(component);
			if (component != null)
			{
				component.Initialize();
			}
			return component;
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x00017231 File Offset: 0x00015431
		public bool ContainsEffect(string name)
		{
			return this.EffectDic.ContainsKey(name);
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x00110B14 File Offset: 0x0010ED14
		public XffectComponent GetEffect(string name)
		{
			if (!this.EffectDic.ContainsKey(name))
			{
				Debug.LogError("there is no effect:" + name + " in effect cache!");
				return null;
			}
			List<XffectComponent> list = this.EffectDic[name];
			if (list == null)
			{
				return null;
			}
			foreach (XffectComponent xffectComponent in list)
			{
				if (!(xffectComponent == null))
				{
					if (!XffectComponent.IsActive(xffectComponent.gameObject))
					{
						return xffectComponent;
					}
				}
			}
			return this.AddXffect(name);
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x00110BD0 File Offset: 0x0010EDD0
		public CompositeXffect GetCompositeXffect(string name)
		{
			List<CompositeXffect> list = this.CompEffectDic[name];
			if (list == null)
			{
				return null;
			}
			foreach (CompositeXffect compositeXffect in list)
			{
				if (!XffectComponent.IsActive(compositeXffect.gameObject))
				{
					return compositeXffect;
				}
			}
			return null;
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x00110C50 File Offset: 0x0010EE50
		public EffectNode EmitNode(string eftName, Vector3 pos)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				Debug.LogError(base.name + ": cache doesnt exist!");
				return null;
			}
			if (list.Count > 1)
			{
				Debug.LogWarning("EmitNode() only support only-one xffect cache!");
			}
			XffectComponent xffectComponent = list[0];
			if (!XffectComponent.IsActive(xffectComponent.gameObject))
			{
				xffectComponent.Active();
			}
			EffectNode effectNode = xffectComponent.EmitByPos(pos);
			if (effectNode == null)
			{
				Debug.LogError("emit node error! may be node max count is not enough:" + eftName);
			}
			return effectNode;
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x00110CDC File Offset: 0x0010EEDC
		public XffectComponent ReleaseEffect(string name, Vector3 pos)
		{
			XffectComponent effect = this.GetEffect(name);
			if (effect == null)
			{
				Debug.LogWarning("can't find available effect in cache!:" + name);
				return null;
			}
			effect.Active();
			effect.SetClient(base.transform);
			effect.SetEmitPosition(pos);
			return effect;
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x00110D2C File Offset: 0x0010EF2C
		public XffectComponent ReleaseEffect(string name, Transform client)
		{
			XffectComponent effect = this.GetEffect(name);
			if (effect == null)
			{
				Debug.LogWarning("can't find available effect in cache!:" + name);
				return null;
			}
			effect.Active();
			effect.SetClient(client);
			return effect;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x00110D70 File Offset: 0x0010EF70
		public XffectComponent ReleaseEffect(string name)
		{
			XffectComponent effect = this.GetEffect(name);
			if (effect == null)
			{
				return null;
			}
			effect.Active();
			return effect;
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x00110D9C File Offset: 0x0010EF9C
		public void StopEffect(string eftName, float fadeTime)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				Debug.LogError(base.name + ": cache doesnt exist!");
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				XffectComponent xffectComponent = list[i];
				if (XffectComponent.IsActive(xffectComponent.gameObject))
				{
					xffectComponent.StopSmoothly(fadeTime);
				}
			}
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x00110E10 File Offset: 0x0010F010
		public void DeActiveEffect(string eftName)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				Debug.LogError(base.name + ": cache doesnt exist!");
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				XffectComponent xffectComponent = list[i];
				if (XffectComponent.IsActive(xffectComponent.gameObject))
				{
					xffectComponent.DeActive();
				}
			}
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x00110E80 File Offset: 0x0010F080
		public bool IsEffectActive(string eftName)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				Debug.LogError(base.name + ": cache doesnt exist!");
				return true;
			}
			if (list.Count > 1)
			{
				Debug.LogWarning("DeActive() only support one Xffect cache!");
			}
			XffectComponent xffectComponent = list[0];
			return XffectComponent.IsActive(xffectComponent.gameObject);
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x00110EE0 File Offset: 0x0010F0E0
		public void StopEffect(string eftName)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				Debug.LogError(base.name + ": cache doesnt exist!");
				return;
			}
			if (list.Count > 1)
			{
				Debug.LogWarning("DeActive() only support one Xffect cache!");
			}
			XffectComponent xffectComponent = list[0];
			if (!XffectComponent.IsActive(xffectComponent.gameObject))
			{
				return;
			}
			xffectComponent.StopEmit();
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x00110F4C File Offset: 0x0010F14C
		public int GetAvailableEffectCount(string eftName)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				XffectComponent xffectComponent = list[i];
				if (!(xffectComponent == null))
				{
					if (!XffectComponent.IsActive(xffectComponent.gameObject))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x04002A44 RID: 10820
		private Dictionary<string, List<XffectComponent>> EffectDic = new Dictionary<string, List<XffectComponent>>();

		// Token: 0x04002A45 RID: 10821
		private Dictionary<string, List<CompositeXffect>> CompEffectDic = new Dictionary<string, List<CompositeXffect>>();

		// Token: 0x04002A46 RID: 10822
		protected bool mInited;
	}
}
