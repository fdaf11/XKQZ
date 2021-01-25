using System;

namespace FSC
{
	// Token: 0x020006E3 RID: 1763
	public class DirectCallChannel : Channel
	{
		// Token: 0x06002A6C RID: 10860 RVA: 0x0001BA92 File Offset: 0x00019C92
		public override void Send(Message message)
		{
			this.messageList.Enqueue(message);
			this.rxEndpoint.OnMessage(this.m_bIsHandshakeChannel);
		}
	}
}
