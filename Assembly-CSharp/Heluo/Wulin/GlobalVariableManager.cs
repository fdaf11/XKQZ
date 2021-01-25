using System;
using System.Collections.Generic;
using System.Linq;

namespace Heluo.Wulin
{
	// Token: 0x020001A8 RID: 424
	public class GlobalVariableManager
	{
		// Token: 0x060008EC RID: 2284 RVA: 0x000075A2 File Offset: 0x000057A2
		public GlobalVariableManager()
		{
			this.Data = new Dictionary<string, int>();
			this.mod_EquipDic = new Dictionary<int, List<ItmeEffectNode>>();
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x000075CC File Offset: 0x000057CC
		public static GlobalVariableManager Singleton
		{
			get
			{
				return GlobalVariableManager.instance;
			}
		}

		// Token: 0x17000105 RID: 261
		public int this[string key]
		{
			get
			{
				if (this.Data.ContainsKey(key))
				{
					return this.Data[key];
				}
				return -1;
			}
			set
			{
				if (!this.Data.ContainsKey(key))
				{
					this.Data.Add(key, value);
					return;
				}
				this.Data[key] = value;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x0000761C File Offset: 0x0000581C
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x00007624 File Offset: 0x00005824
		public Dictionary<string, int> Data { get; set; }

		// Token: 0x060008F3 RID: 2291 RVA: 0x0004DF80 File Offset: 0x0004C180
		public void CopyDataTo(Dictionary<string, int> res)
		{
			if (res == null)
			{
				res = new Dictionary<string, int>();
			}
			res.Clear();
			foreach (KeyValuePair<string, int> keyValuePair in this.Data)
			{
				res.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0004DFF0 File Offset: 0x0004C1F0
		public void CopyDataFrom(Dictionary<string, int> org)
		{
			if (org == null)
			{
				return;
			}
			this.Data.Clear();
			foreach (KeyValuePair<string, int> keyValuePair in org)
			{
				this.Data.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0000762D File Offset: 0x0000582D
		// (set) Token: 0x060008F6 RID: 2294 RVA: 0x00007635 File Offset: 0x00005835
		public Dictionary<int, List<ItmeEffectNode>> mod_EquipDic { get; set; }

		// Token: 0x060008F7 RID: 2295 RVA: 0x0004E060 File Offset: 0x0004C260
		public void mod_CopyEquipFrom(Dictionary<int, List<ItmeEffectNode>> org)
		{
			if (org == null)
			{
				return;
			}
			this.mod_EquipDic.Clear();
			foreach (KeyValuePair<int, List<ItmeEffectNode>> keyValuePair in org)
			{
				this.mod_EquipDic.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0004E0D0 File Offset: 0x0004C2D0
		public void mod_CopyEquipTo(Dictionary<int, List<ItmeEffectNode>> res)
		{
			if (res == null)
			{
				res = new Dictionary<int, List<ItmeEffectNode>>();
			}
			res.Clear();
			foreach (KeyValuePair<int, List<ItmeEffectNode>> keyValuePair in this.mod_EquipDic)
			{
				res.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0004E140 File Offset: 0x0004C340
		public int mod_GetGuid()
		{
			List<int> list = Enumerable.ToList<int>(this.mod_EquipDic.Keys);
			if (list.Count <= 0)
			{
				return 1000;
			}
			return list[list.Count - 1] + 1;
		}

		// Token: 0x0400086D RID: 2157
		private static readonly GlobalVariableManager instance = new GlobalVariableManager();
	}
}
