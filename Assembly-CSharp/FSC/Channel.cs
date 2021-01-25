using System;
using System.Collections.Generic;

namespace FSC
{
	// Token: 0x020006E2 RID: 1762
	public abstract class Channel
	{
		// Token: 0x06002A63 RID: 10851 RVA: 0x0001BA16 File Offset: 0x00019C16
		public Channel()
		{
			this.m_bIsHandshakeChannel = false;
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x0000264F File Offset: 0x0000084F
		public static void Init(IntPtr pointerCallbackFuncPtr)
		{
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x0001BA30 File Offset: 0x00019C30
		public void SetRxEndpoint(Endpoint endpoint, bool useAsHandshakeChannel = false)
		{
			this.rxEndpoint = endpoint;
			this.m_bIsHandshakeChannel = useAsHandshakeChannel;
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x0001BA40 File Offset: 0x00019C40
		public void SetTxEndpoint(Endpoint endpoint, bool useAsHandshakeChannel = false)
		{
			this.txEndpoint = endpoint;
			this.m_bIsHandshakeChannel = useAsHandshakeChannel;
		}

		// Token: 0x06002A67 RID: 10855
		public abstract void Send(Message message);

		// Token: 0x06002A68 RID: 10856 RVA: 0x0001BA50 File Offset: 0x00019C50
		public int Push(Message message)
		{
			if (message == null)
			{
				return 1;
			}
			this.messageList.Enqueue(message);
			return 0;
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x0001BA67 File Offset: 0x00019C67
		public int Pop(Message message)
		{
			if (this.messageList.Count < 1)
			{
				return 1;
			}
			message = this.messageList.Dequeue();
			return 0;
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x0000264F File Offset: 0x0000084F
		public void Terminate()
		{
		}

		// Token: 0x04003585 RID: 13701
		protected Endpoint txEndpoint;

		// Token: 0x04003586 RID: 13702
		protected Endpoint rxEndpoint;

		// Token: 0x04003587 RID: 13703
		protected bool m_bIsHandshakeChannel;

		// Token: 0x04003588 RID: 13704
		protected Queue<Message> messageList = new Queue<Message>();
	}
}
