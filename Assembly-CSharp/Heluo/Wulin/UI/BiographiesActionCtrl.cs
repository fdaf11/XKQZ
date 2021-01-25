using System;
using System.Collections.Generic;

namespace Heluo.Wulin.UI
{
	// Token: 0x020002EE RID: 750
	public class BiographiesActionCtrl
	{
		// Token: 0x06000FB8 RID: 4024 RVA: 0x00002672 File Offset: 0x00000872
		public BiographiesActionCtrl()
		{
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0000A72D File Offset: 0x0000892D
		public BiographiesActionCtrl(Control Obj, List<BiographiesActionNode> list)
		{
			this.m_ActionList = list;
			this.m_Action = Obj;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0000A743 File Offset: 0x00008943
		public void StartAction()
		{
			this.m_CurrentIndex = 0;
			if (this.OnSetAction != null)
			{
				this.OnSetAction.Invoke(this.m_Action, this);
			}
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00085D0C File Offset: 0x00083F0C
		public BiographiesActionNode GetCurrentNode()
		{
			return this.m_ActionList[this.m_CurrentIndex];
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00085D30 File Offset: 0x00083F30
		public void NextAction()
		{
			if (this.m_bFade || this.m_bDisplacement)
			{
				return;
			}
			if (this.m_ActionList == null)
			{
				this.OnActionFinish.Invoke(this);
			}
			this.m_CurrentIndex++;
			if (this.m_CurrentIndex >= this.m_ActionList.Count)
			{
				if (this.OnActionFinish != null)
				{
					this.OnActionFinish.Invoke(this);
				}
			}
			else if (this.OnSetAction != null)
			{
				this.OnSetAction.Invoke(this.m_Action, this);
			}
		}

		// Token: 0x040012D0 RID: 4816
		public Action<Control, BiographiesActionCtrl> OnSetAction;

		// Token: 0x040012D1 RID: 4817
		public Action<BiographiesActionCtrl> OnActionFinish;

		// Token: 0x040012D2 RID: 4818
		public Control m_Action;

		// Token: 0x040012D3 RID: 4819
		private List<BiographiesActionNode> m_ActionList;

		// Token: 0x040012D4 RID: 4820
		public bool m_bFade;

		// Token: 0x040012D5 RID: 4821
		public bool m_bDisplacement;

		// Token: 0x040012D6 RID: 4822
		private int m_CurrentIndex;
	}
}
