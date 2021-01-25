using System;

namespace FSC
{
	// Token: 0x020006D8 RID: 1752
	public class Message
	{
		// Token: 0x06002A32 RID: 10802 RVA: 0x0014BA94 File Offset: 0x00149C94
		public unsafe Message(int _type, void* _payload, int _payloadSize)
		{
			this.type = _type;
			this.attributes = 0;
			this.payload = _payload;
			this.payloadSize = _payloadSize;
			this.sessionId = 0U;
			this._msgData = default(MessageData);
			this._msgData.Init(_type, _payload, _payloadSize);
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x0014BAF4 File Offset: 0x00149CF4
		public unsafe Message(int _type, int _attributes, void* _payload, int _payloadSize)
		{
			this.type = _type;
			this.attributes = _attributes;
			this.payload = _payload;
			this.payloadSize = _payloadSize;
			this.sessionId = 0U;
			this._msgData = default(MessageData);
			this._msgData.Init(_type, _attributes, _payload, _payloadSize);
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x0014BB58 File Offset: 0x00149D58
		public Message(MessageData msg)
		{
			this.type = msg.type;
			this.attributes = msg.attributes;
			this.payload = msg.payload;
			this.payloadSize = msg.payloadSize;
			this.payloadIntegrityValue = msg.payloadIntegrityValue;
			this.sessionId = msg.sessionId;
			this.reserved[0] = msg.reserved0;
			this.reserved[1] = msg.reserved1;
			this.reserved[2] = msg.reserved2;
			this.reserved[3] = msg.reserved3;
			this.reserved[4] = msg.reserved4;
			this.reserved[5] = msg.reserved5;
			this.reserved[6] = msg.reserved6;
			this.reserved[7] = msg.reserved7;
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06002A35 RID: 10805 RVA: 0x0014BC40 File Offset: 0x00149E40
		public unsafe MessageData* MsgData
		{
			get
			{
				return &this._msgData;
			}
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x0014BC58 File Offset: 0x00149E58
		public void MessageToMessageData()
		{
			this._msgData.type = this.type;
			this._msgData.attributes = this.attributes;
			this._msgData.payload = this.payload;
			this._msgData.payloadSize = this.payloadSize;
			this._msgData.payloadIntegrityValue = this.payloadIntegrityValue;
			this._msgData.sessionId = this.sessionId;
			this._msgData.reserved0 = this.reserved[0];
			this._msgData.reserved1 = this.reserved[1];
			this._msgData.reserved2 = this.reserved[2];
			this._msgData.reserved3 = this.reserved[3];
			this._msgData.reserved4 = this.reserved[4];
			this._msgData.reserved5 = this.reserved[5];
			this._msgData.reserved6 = this.reserved[6];
			this._msgData.reserved7 = this.reserved[7];
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x0014BD64 File Offset: 0x00149F64
		public void MessageDataToMessage()
		{
			this.type = this._msgData.type;
			this.attributes = this._msgData.attributes;
			this.payload = this._msgData.payload;
			this.payloadSize = this._msgData.payloadSize;
			this.payloadIntegrityValue = this._msgData.payloadIntegrityValue;
			this.sessionId = this._msgData.sessionId;
			this.reserved[0] = this._msgData.reserved0;
			this.reserved[1] = this._msgData.reserved1;
			this.reserved[2] = this._msgData.reserved2;
			this.reserved[3] = this._msgData.reserved3;
			this.reserved[4] = this._msgData.reserved4;
			this.reserved[5] = this._msgData.reserved5;
			this.reserved[6] = this._msgData.reserved6;
			this.reserved[7] = this._msgData.reserved7;
		}

		// Token: 0x04003557 RID: 13655
		public const int TYPE_HANDSHAKE_STEP_1 = -1;

		// Token: 0x04003558 RID: 13656
		public const int TYPE_HANDSHAKE_STEP_2 = -2;

		// Token: 0x04003559 RID: 13657
		public const int TYPE_TERMINATE_CONNECTION = -3;

		// Token: 0x0400355A RID: 13658
		public const int TYPE_NULL = 0;

		// Token: 0x0400355B RID: 13659
		public const int TYPE_SMS = 1;

		// Token: 0x0400355C RID: 13660
		public const int TYPE_HANDSHAKESEND_STEP_1 = 2;

		// Token: 0x0400355D RID: 13661
		public const int TYPE_CSHARP = 3;

		// Token: 0x0400355E RID: 13662
		public const int TYPE_CSHARP_HANDSHAKE_STEP_1 = -4;

		// Token: 0x0400355F RID: 13663
		public const int TYPE_CSHARP_HANDSHAKE_STEP_2 = -5;

		// Token: 0x04003560 RID: 13664
		public const int ATT_NO_HANDSHAKE = 1;

		// Token: 0x04003561 RID: 13665
		public uint[] reserved = new uint[8];

		// Token: 0x04003562 RID: 13666
		public int type;

		// Token: 0x04003563 RID: 13667
		public int attributes;

		// Token: 0x04003564 RID: 13668
		public unsafe void* payload;

		// Token: 0x04003565 RID: 13669
		public int payloadSize;

		// Token: 0x04003566 RID: 13670
		public uint payloadIntegrityValue;

		// Token: 0x04003567 RID: 13671
		public uint sessionId;

		// Token: 0x04003568 RID: 13672
		private MessageData _msgData;
	}
}
