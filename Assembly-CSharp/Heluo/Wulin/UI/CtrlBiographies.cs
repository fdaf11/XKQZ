using System;
using System.Collections.Generic;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002EF RID: 751
	public class CtrlBiographies
	{
		// Token: 0x06000FBE RID: 4030 RVA: 0x00085DC8 File Offset: 0x00083FC8
		public void StartBiographies(string id)
		{
			this.m_CurrentIndex = 0;
			this.m_BiographiesTypeNode = Game.Biographies.GetBiographiesTypeNode(id);
			BiographiesNode biographiesNode = this.m_BiographiesTypeNode.m_BiographiesNodeList[this.m_CurrentIndex];
			if (this.OnSetBiographiesView != null)
			{
				this.OnSetBiographiesView.Invoke(biographiesNode);
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00085E1C File Offset: 0x0008401C
		public void Next()
		{
			if (this.checkBusy())
			{
				return;
			}
			if (this.OnOverRound != null && this.OnOverRound.Invoke())
			{
				return;
			}
			this.m_CurrentIndex++;
			BiographiesNode biographiesNode;
			if (this.m_CurrentIndex >= this.m_BiographiesTypeNode.m_BiographiesNodeList.Count)
			{
				biographiesNode = this.m_BiographiesTypeNode.m_BiographiesNodeList[this.m_BiographiesTypeNode.m_BiographiesNodeList.Count - 1];
				if (this.OnSetBiographiesReward != null)
				{
					this.OnSetBiographiesReward.Invoke(biographiesNode.m_BiographiesReward);
				}
				return;
			}
			biographiesNode = this.m_BiographiesTypeNode.m_BiographiesNodeList[this.m_CurrentIndex];
			if (this.OnSetBiographiesView != null)
			{
				this.OnSetBiographiesView.Invoke(biographiesNode);
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0000A77C File Offset: 0x0000897C
		public void AddBiographiesActionCtrl(BiographiesActionCtrl ctrl)
		{
			this.m_BiographiesActionCtrlList.Add(ctrl);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0000A78A File Offset: 0x0000898A
		public void RemoveBiographiesActionCtrl(BiographiesActionCtrl ctrl)
		{
			this.m_BiographiesActionCtrlList.Remove(ctrl);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0000A799 File Offset: 0x00008999
		private bool checkBusy()
		{
			return this.m_BiographiesActionCtrlList.Count > 0 || this.m_bOverRound;
		}

		// Token: 0x040012D7 RID: 4823
		public Action<BiographiesNode> OnSetBiographiesView;

		// Token: 0x040012D8 RID: 4824
		public Func<bool> OnOverRound;

		// Token: 0x040012D9 RID: 4825
		public Action<int> OnSetBiographiesReward;

		// Token: 0x040012DA RID: 4826
		private BiographiesTypeNode m_BiographiesTypeNode;

		// Token: 0x040012DB RID: 4827
		private int m_CurrentIndex;

		// Token: 0x040012DC RID: 4828
		public bool m_bOverRound;

		// Token: 0x040012DD RID: 4829
		private List<BiographiesActionCtrl> m_BiographiesActionCtrlList = new List<BiographiesActionCtrl>();
	}
}
