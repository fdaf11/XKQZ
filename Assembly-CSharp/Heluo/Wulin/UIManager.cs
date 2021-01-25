using System;
using System.Collections.Generic;
using UnityEngine;

namespace Heluo.Wulin
{
	// Token: 0x02000270 RID: 624
	public class UIManager : Dictionary<Type, UILayer>
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00008F5D File Offset: 0x0000715D
		public static UIManager Singleton
		{
			get
			{
				return UIManager.instance;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00008F64 File Offset: 0x00007164
		public UIRoot Root
		{
			get
			{
				if (this.root == null)
				{
					this.root = (Object.FindObjectOfType(typeof(UIRoot)) as UIRoot);
				}
				return this.root;
			}
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0005DC18 File Offset: 0x0005BE18
		public T Get<T>() where T : UILayer
		{
			Type typeFromHandle = typeof(T);
			if (this.ContainsKey(typeFromHandle))
			{
				return this[typeFromHandle] as T;
			}
			return (T)((object)null);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0005DC54 File Offset: 0x0005BE54
		public T GetOrCreat<T>() where T : UILayer
		{
			Type typeFromHandle = typeof(T);
			T t = (T)((object)null);
			if (this.ContainsKey(typeFromHandle))
			{
				t = (this[typeFromHandle] as T);
			}
			if (t == null)
			{
				string strForm = typeFromHandle.Name.Replace("UI", "cForm");
				GameObject gameObject = this.CreateSamllGameUI(strForm);
				return gameObject.GetComponentInChildren<T>();
			}
			return t;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0005DCC8 File Offset: 0x0005BEC8
		public UILayer GetLayer(UILayer layer)
		{
			Type type = layer.GetType();
			if (this.ContainsKey(type))
			{
				return this[type];
			}
			return null;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0005DCF4 File Offset: 0x0005BEF4
		public void Remove(UILayer layer)
		{
			Type type = layer.GetType();
			if (this.ContainsKey(type))
			{
				this[type] = null;
			}
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0005DD1C File Offset: 0x0005BF1C
		public void Add(UILayer layer)
		{
			Type type = layer.GetType();
			if (this.ContainsKey(type))
			{
				Object.Destroy(this[type]);
				this[type] = layer;
			}
			else
			{
				this.Add(type, layer);
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0005DD60 File Offset: 0x0005BF60
		public void Show<T>() where T : UILayer
		{
			T t = this.Get<T>();
			if (t == null)
			{
				Debug.LogError("no create ui : " + typeof(T).ToString());
				return;
			}
			t.Show();
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0005DDB4 File Offset: 0x0005BFB4
		public void Hide<T>() where T : UILayer
		{
			T t = this.Get<T>();
			if (t == null)
			{
				Debug.LogError("no create ui : " + typeof(T).ToString());
				return;
			}
			t.Hide();
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0005DE08 File Offset: 0x0005C008
		public GameObject CreateUI(string strForm)
		{
			if (Game.g_cForm.Contains(strForm))
			{
				GameObject gameObject = Game.g_cForm.Load(strForm) as GameObject;
				GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
				gameObject2.name = gameObject.name;
				if (this.Root != null)
				{
					gameObject2.transform.parent = this.Root.transform;
				}
				gameObject2.transform.localPosition = gameObject.transform.localPosition;
				gameObject2.transform.localRotation = gameObject.transform.localRotation;
				gameObject2.transform.localScale = gameObject.transform.localScale;
				return gameObject2;
			}
			Debug.LogError(strForm + " no found");
			return null;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0005DECC File Offset: 0x0005C0CC
		public GameObject CreateSamllGameUI(string strForm)
		{
			if (Game.g_SmallGame.Contains(strForm))
			{
				GameObject gameObject = Game.g_SmallGame.Load(strForm) as GameObject;
				GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
				gameObject2.name = gameObject.name;
				if (this.Root != null)
				{
					gameObject2.transform.parent = this.Root.transform;
				}
				gameObject2.transform.localPosition = gameObject.transform.localPosition;
				gameObject2.transform.localRotation = gameObject.transform.localRotation;
				gameObject2.transform.localScale = gameObject.transform.localScale;
				return gameObject2;
			}
			Debug.LogError(strForm + " no found SmallGame");
			return null;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0005DF90 File Offset: 0x0005C190
		public void ClearAllUIData()
		{
			foreach (KeyValuePair<Type, UILayer> keyValuePair in UIManager.Singleton)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.ClearUIData();
				}
			}
		}

		// Token: 0x04000D45 RID: 3397
		private static readonly UIManager instance = new UIManager();

		// Token: 0x04000D46 RID: 3398
		private UIRoot root;

		// Token: 0x04000D47 RID: 3399
		private UILayer m_CurrentUI;
	}
}
