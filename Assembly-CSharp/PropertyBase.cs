using System;
using System.Collections.Generic;

// Token: 0x020001ED RID: 493
public class PropertyBase : Dictionary<CharacterData.PropertyType, int>
{
	// Token: 0x060009E9 RID: 2537 RVA: 0x00007FC3 File Offset: 0x000061C3
	public virtual int Get(CharacterData.PropertyType type)
	{
		if (this.ContainsKey(type))
		{
			return this[type];
		}
		return 0;
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00007FDA File Offset: 0x000061DA
	public virtual bool Set(CharacterData.PropertyType type, int val)
	{
		if (this.ContainsKey(type))
		{
			this[type] = val;
			return true;
		}
		return false;
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00007FF3 File Offset: 0x000061F3
	public void Remove(CharacterData.PropertyType type)
	{
		if (this.ContainsKey(type))
		{
			this[type] = 0;
		}
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00008009 File Offset: 0x00006209
	public void Add(CharacterData.PropertyType type, int val)
	{
		if (this.ContainsKey(type))
		{
			this[type] = val;
		}
		else
		{
			base.Add(type, val);
		}
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x000539F0 File Offset: 0x00051BF0
	public virtual bool SetPlus(CharacterData.PropertyType type, int val)
	{
		int num = this.Get(type);
		num += val;
		return this.Set(type, num);
	}
}
