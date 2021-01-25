using System;
using UnityEngine;

namespace Heluo.Wulin.UI
{
	// Token: 0x02000396 RID: 918
	public class Store : MonoBehaviour
	{
		// Token: 0x06001544 RID: 5444 RVA: 0x0000264F File Offset: 0x0000084F
		private void Awake()
		{
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0000264F File Offset: 0x0000084F
		private void Start()
		{
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0000D7FB File Offset: 0x0000B9FB
		public void SetStoreItemList(int iStoreID)
		{
			this.m_StoreDataNode = Game.StoreData.GetStoreDataNode(iStoreID);
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0000D80E File Offset: 0x0000BA0E
		public void SetDataToForm()
		{
			Game.UI.Show<UIShop>();
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0000264F File Offset: 0x0000084F
		public void SetMoney(int iMoney)
		{
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0000264F File Offset: 0x0000084F
		public void LessStoreItem(int ItemID)
		{
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0000264F File Offset: 0x0000084F
		public void AddStoreItem(int ItemID)
		{
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0000D81A File Offset: 0x0000BA1A
		public StoreDataNode GetStoreItemNode()
		{
			return this.m_StoreDataNode;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0000264F File Offset: 0x0000084F
		private void Update()
		{
		}

		// Token: 0x040019F9 RID: 6649
		private StoreDataNode m_StoreDataNode;
	}
}
