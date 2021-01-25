using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000424 RID: 1060
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Examples/Item Database")]
public class InvDatabase : MonoBehaviour
{
	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x060019BF RID: 6591 RVA: 0x00010C84 File Offset: 0x0000EE84
	public static InvDatabase[] list
	{
		get
		{
			if (InvDatabase.mIsDirty)
			{
				InvDatabase.mIsDirty = false;
				InvDatabase.mList = NGUITools.FindActive<InvDatabase>();
			}
			return InvDatabase.mList;
		}
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x00010C7C File Offset: 0x0000EE7C
	private void OnEnable()
	{
		InvDatabase.mIsDirty = true;
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x00010C7C File Offset: 0x0000EE7C
	private void OnDisable()
	{
		InvDatabase.mIsDirty = true;
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x000CF530 File Offset: 0x000CD730
	private InvBaseItem GetItem(int id16)
	{
		int i = 0;
		int count = this.items.Count;
		while (i < count)
		{
			InvBaseItem invBaseItem = this.items[i];
			if (invBaseItem.id16 == id16)
			{
				return invBaseItem;
			}
			i++;
		}
		return null;
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x000CF578 File Offset: 0x000CD778
	private static InvDatabase GetDatabase(int dbID)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			if (invDatabase.databaseID == dbID)
			{
				return invDatabase;
			}
			i++;
		}
		return null;
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x000CF5B8 File Offset: 0x000CD7B8
	public static InvBaseItem FindByID(int id32)
	{
		InvDatabase database = InvDatabase.GetDatabase(id32 >> 16);
		return (!(database != null)) ? null : database.GetItem(id32 & 65535);
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x000CF5F0 File Offset: 0x000CD7F0
	public static InvBaseItem FindByName(string exact)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			int j = 0;
			int count = invDatabase.items.Count;
			while (j < count)
			{
				InvBaseItem invBaseItem = invDatabase.items[j];
				if (invBaseItem.name == exact)
				{
					return invBaseItem;
				}
				j++;
			}
			i++;
		}
		return null;
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x000CF664 File Offset: 0x000CD864
	public static int FindItemID(InvBaseItem item)
	{
		int i = 0;
		int num = InvDatabase.list.Length;
		while (i < num)
		{
			InvDatabase invDatabase = InvDatabase.list[i];
			if (invDatabase.items.Contains(item))
			{
				return invDatabase.databaseID << 16 | item.id16;
			}
			i++;
		}
		return -1;
	}

	// Token: 0x04001E7A RID: 7802
	private static InvDatabase[] mList;

	// Token: 0x04001E7B RID: 7803
	private static bool mIsDirty = true;

	// Token: 0x04001E7C RID: 7804
	public int databaseID;

	// Token: 0x04001E7D RID: 7805
	public List<InvBaseItem> items = new List<InvBaseItem>();

	// Token: 0x04001E7E RID: 7806
	public UIAtlas iconAtlas;
}
