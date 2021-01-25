using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace FSC
{
	// Token: 0x020006DA RID: 1754
	public class Endpoint
	{
		// Token: 0x06002A3A RID: 10810 RVA: 0x0001B964 File Offset: 0x00019B64
		public Endpoint(int activateDefaultKeyBinding = 0, uint updateInterval = 1U, Channel channelTx = null, Channel channelRx = null)
		{
			this.Init(activateDefaultKeyBinding, updateInterval, channelTx, channelRx);
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x0014BE70 File Offset: 0x0014A070
		protected static int createBaseId()
		{
			uint seed = Helper.TimeNull();
			FSCCSharp.PrngCore.srand(seed);
			return (int)FSCCSharp.PrngCore.rand();
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x0014BE98 File Offset: 0x0014A098
		protected int Pop(Message message, bool isHandshakeMessage = false)
		{
			if (isHandshakeMessage)
			{
				if (this.m_HandshakeChannelRx == null)
				{
					return 2;
				}
				if (this.m_HandshakeChannelRx.Pop(message) != 0 && this.FilterMessage(message) != 0)
				{
					message = null;
					return 3;
				}
			}
			else
			{
				if (this.m_ChannelRx == null)
				{
					return 2;
				}
				if (this.m_ChannelRx.Pop(message) != 0 && this.FilterMessage(message) != 0)
				{
					message = null;
					return 3;
				}
			}
			return 0;
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x0001B991 File Offset: 0x00019B91
		protected int EncryptMessage(Message message)
		{
			return this.EncDecMessage(message, true);
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x0014BF10 File Offset: 0x0014A110
		protected unsafe int EncDecMessage(Message message, bool encryption)
		{
			if (message == null)
			{
				return -1;
			}
			int num = 0;
			bool flag = !encryption;
			if (encryption)
			{
				fixed (uint* result = &message.payloadIntegrityValue)
				{
					Endpoint.calculateIntegrityValue(message.payload, (uint)message.payloadSize, result);
				}
			}
			if (flag)
			{
				if (this.updateCounter == 0U)
				{
					this.UpdateDefaultKey();
				}
				fixed (byte* key = ref (this.defaultKey != null && this.defaultKey.Length != 0) ? ref this.defaultKey[0] : ref *null)
				{
					num = FSCCSharp.CipherCore.Decrypt(key, (uint)this.defaultKey.Length, message.payload, (uint)message.payloadSize);
				}
				if (this.updateCounter != 0U)
				{
					this.UpdateDefaultKey();
				}
				this.updateCounter += 1U;
				if (num != 0)
				{
					return num;
				}
			}
			int type = message.type;
			switch (type + 3)
			{
			case 0:
				goto IL_171;
			case 1:
			case 2:
			case 5:
				goto IL_171;
			}
			if ((message.attributes & 1) == 0)
			{
				byte* key2 = Helper.ByteArrToBytePtr(this.sessionKey);
				if (encryption)
				{
					num = FSCCSharp.CipherCore.Encrypt(key2, (uint)this.sessionKey.Length, message.payload, (uint)message.payloadSize);
				}
				if (flag)
				{
					num = FSCCSharp.CipherCore.Decrypt(key2, (uint)this.sessionKey.Length, message.payload, (uint)message.payloadSize);
				}
				if (num != 0)
				{
					return num;
				}
			}
			IL_171:
			if (encryption)
			{
				if (this.updateCounter == 0U)
				{
					this.UpdateDefaultKey();
				}
				byte* key3 = Helper.ByteArrToBytePtr(this.defaultKey);
				num = FSCCSharp.CipherCore.Encrypt(key3, (uint)this.defaultKey.Length, message.payload, (uint)message.payloadSize);
				if (this.updateCounter != 0U)
				{
					this.UpdateDefaultKey();
				}
				this.updateCounter += 1U;
				if (num != 0)
				{
					return num;
				}
			}
			if (flag)
			{
				uint num2;
				Endpoint.calculateIntegrityValue(message.payload, (uint)message.payloadSize, &num2);
				if (message.type != 0 && num2 != message.payloadIntegrityValue)
				{
					return 1;
				}
			}
			return num;
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x0014C130 File Offset: 0x0014A330
		protected unsafe void UpdateDefaultKey()
		{
			if (this.bindGen != null && this.updateCounter % this.m_UpdateInterval == 0U)
			{
				ulong num = 9223372036854775807UL;
				this.bindGen((uint*)(&num), (uint)Marshal.SizeOf(num));
				byte* key = Helper.ByteArrToBytePtr(this.defaultKey);
				uint keySize = (uint)this.defaultKey.Length;
				this.deriveKeyFromId(key, keySize, num);
				FSCCSharp.CipherCore.SetUpKey(key, keySize);
			}
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x0014C1B0 File Offset: 0x0014A3B0
		protected unsafe void deriveKeyFromId(byte* key, uint keySize, ulong id)
		{
			ulong num = 88172645463325252UL ^ id;
			for (uint num2 = 0U; num2 < keySize; num2 += 1U)
			{
				num ^= num << 13;
				num ^= num >> 7;
				key[num2] = (byte)(num ^= num << 17);
			}
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x0014C1F8 File Offset: 0x0014A3F8
		protected unsafe void deriveSessionKey(byte* key, uint keySize, uint keyContribution1, uint keyContribution2)
		{
			ulong id = (ulong)keyContribution1 + ((ulong)keyContribution2 << 16);
			this.deriveKeyFromId(key, keySize, id);
			FSCCSharp.CipherCore.SetUpKey(key, keySize);
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x0014C228 File Offset: 0x0014A428
		protected int FilterMessage(Message message)
		{
			if (message == null)
			{
				return -1;
			}
			int type = message.type;
			switch (type + 2)
			{
			case 0:
			case 1:
			case 4:
				return 0;
			}
			if (this.handshakeFailed != 0U)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x0014C280 File Offset: 0x0014A480
		public void Init(int activateDefaultKeyBinding = 0, uint updateInterval = 1U, Channel channelTx = null, Channel channelRx = null)
		{
			if (Endpoint.baseId == 0)
			{
				Endpoint.baseId = Endpoint.createBaseId();
			}
			this.m_ChannelRx = (this.m_ChannelTx = (this.m_HandshakeChannelRx = (this.m_HandshakeChannelTx = null)));
			this.m_UpdateInterval = updateInterval;
			this.updateCounter = 0U;
			this.handshakeFailed = 0U;
			this.endpointId = (uint)Interlocked.Increment(ref Endpoint.baseId);
			if (activateDefaultKeyBinding == 0)
			{
				uint[] array = new uint[]
				{
					171U,
					180U,
					69U,
					162U,
					212U,
					1U,
					165U,
					111U,
					231U,
					43U,
					177U,
					130U,
					111U,
					86U,
					35U,
					227U,
					43U,
					38U,
					249U,
					237U,
					130U,
					142U,
					96U,
					133U,
					52U,
					65U,
					63U,
					168U,
					71U,
					157U,
					233U,
					12U
				};
				for (int i = 0; i < 32; i++)
				{
					this.defaultKey[i] = (byte)array[i];
				}
			}
			else
			{
				this.ActivateEncryptionBinding(activateDefaultKeyBinding);
			}
			if (channelTx != null)
			{
				this.SetTxChannel(channelTx);
			}
			if (channelRx != null)
			{
				this.SetRxChannel(channelRx);
			}
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x0001B99B File Offset: 0x00019B9B
		public int DecryptMessage(Message message)
		{
			return this.EncDecMessage(message, false);
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x0001B9A5 File Offset: 0x00019BA5
		public void SetRxChannel(Channel rx)
		{
			this.m_ChannelRx = rx;
			this.m_ChannelRx.SetRxEndpoint(this, false);
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x0001B9BB File Offset: 0x00019BBB
		public void SetTxChannel(Channel tx)
		{
			this.m_ChannelTx = tx;
			this.m_ChannelTx.SetTxEndpoint(this, false);
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x0001B9D1 File Offset: 0x00019BD1
		public void SetHandshakeRxChannel(Channel rx)
		{
			this.m_HandshakeChannelRx = rx;
			this.m_HandshakeChannelRx.SetRxEndpoint(this, true);
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x0001B9E7 File Offset: 0x00019BE7
		public void SetHandshakeTxChannel(Channel tx)
		{
			this.m_HandshakeChannelTx = tx;
			this.m_HandshakeChannelTx.SetTxEndpoint(this, true);
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x0014C348 File Offset: 0x0014A548
		public unsafe void OnMessage(bool processHandshakeMessages = false)
		{
			Message message = null;
			if (this.Pop(message, processHandshakeMessages) != 0 || message == null || message.payload == null)
			{
				return;
			}
			int type = message.type;
			if (type != -2)
			{
				this.m_ChannelRx.Push(message);
			}
			else if (this.DecryptMessage(message) == 0)
			{
				byte[] array = new byte[32];
				byte* key = Helper.ByteArrToBytePtr(array);
				this.deriveKeyFromId(key, (uint)array.Length, (ulong)this.endpointId);
				FSCCSharp.CipherCore.SetUpKey(key, (uint)array.Length);
				CSharpHandshakePayload csharpHandshakePayload = *(CSharpHandshakePayload*)message.payload;
				if (csharpHandshakePayload.Decrypt(key, (uint)array.Length) == 0)
				{
					if (this.nonceClient != csharpHandshakePayload.nonceClient)
					{
						this.handshakeFailed = 1U;
					}
					else
					{
						this.handshakeFailed = 0U;
						this.sessionId = csharpHandshakePayload.id;
						byte* key2 = Helper.ByteArrToBytePtr(this.sessionKey);
						uint keySize = (uint)this.sessionKey.Length;
						this.deriveSessionKey(key2, keySize, this.nonceClient, csharpHandshakePayload.nonceServer);
					}
				}
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x0001B9FD File Offset: 0x00019BFD
		public void Handshake()
		{
			this.Handshake(null, false);
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x0014C464 File Offset: 0x0014A664
		public unsafe void Handshake(Channel channel, bool handshakeSend = false)
		{
			this.nonceClient = FSCCSharp.PrngCore.rand();
			byte[] array = new byte[32];
			byte* key = Helper.ByteArrToBytePtr(array);
			uint keySize = (uint)array.Length;
			this.deriveKeyFromId(key, keySize, (ulong)this.endpointId);
			FSCCSharp.CipherCore.SetUpKey(key, keySize);
			Helper.Print("EndpointID: " + this.endpointId);
			CSharpHandshakePayload csharpHandshakePayload = CSharpHandshakePayload.Step1(this.endpointId, null, this.nonceClient);
			csharpHandshakePayload.Encrypt(key, keySize);
			int type;
			if (handshakeSend)
			{
				type = 2;
			}
			else
			{
				type = -1;
			}
			Message message = new Message(type, (void*)(&csharpHandshakePayload), Marshal.SizeOf(csharpHandshakePayload));
			this.sessionId = csharpHandshakePayload.id;
			this.Send(message);
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x0014C528 File Offset: 0x0014A728
		public void Send(Message message)
		{
			if (this.FilterMessage(message) != 0)
			{
				return;
			}
			message.sessionId = this.sessionId;
			int num = this.EncryptMessage(message);
			if (num != 0)
			{
				return;
			}
			if (this.m_HandshakeChannelTx != null && (message.type == 2 || message.type == -1))
			{
				this.m_HandshakeChannelTx.Send(message);
			}
			else
			{
				if (this.m_ChannelTx == null)
				{
					return;
				}
				this.m_ChannelTx.Send(message);
			}
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x0001BA07 File Offset: 0x00019C07
		public void HandshakeSend(Message message)
		{
			this.Handshake(this.m_ChannelRx, true);
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x0014C5B0 File Offset: 0x0014A7B0
		public int Receive(Message message)
		{
			if (message.type == 3)
			{
				message.MessageDataToMessage();
				return this.DecryptMessage(message);
			}
			int num = this.Pop(message, false);
			if (num != 0)
			{
				return num;
			}
			return this.DecryptMessage(message);
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x0014C5F4 File Offset: 0x0014A7F4
		public unsafe static int calculateIntegrityValue(void* data, uint dataSize, uint* result)
		{
			uint num = 0U;
			for (uint num2 = 0U; num2 < dataSize; num2 += 1U)
			{
				num += ((uint)((byte*)data)[num2] | 256U) << 1;
			}
			*result = num;
			return 0;
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x0014C62C File Offset: 0x0014A82C
		public unsafe void Terminate()
		{
			NullPayload nullPayload = default(NullPayload);
			int payloadSize = Marshal.SizeOf(nullPayload);
			Message message = new Message(-3, (void*)(&nullPayload), payloadSize);
			this.Send(message);
			if (this.m_ChannelTx != null)
			{
				this.m_ChannelTx.Terminate();
			}
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x0014C678 File Offset: 0x0014A878
		private void ActivateEncryptionBinding(int selector = 1)
		{
			switch (selector)
			{
			case 1:
				this.bindGen = FSCCSharp.FSC_BINDINGNUMBERGENERATOR_CLIENTSERVER_1;
				break;
			case 2:
				this.bindGen = FSCCSharp.FSC_BINDINGNUMBERGENERATOR_CLIENTSERVER_1;
				break;
			case 3:
				this.bindGen = FSCCSharp.FSC_BINDINGNUMBERGENERATOR_CLIENTSERVER_1;
				break;
			default:
				this.bindGen = FSCCSharp.FSC_BINDINGNUMBERGENERATOR_CLIENTSERVER_1;
				break;
			}
		}

		// Token: 0x04003577 RID: 13687
		protected static int baseId;

		// Token: 0x04003578 RID: 13688
		protected uint endpointId;

		// Token: 0x04003579 RID: 13689
		protected Channel m_ChannelRx;

		// Token: 0x0400357A RID: 13690
		protected Channel m_ChannelTx;

		// Token: 0x0400357B RID: 13691
		protected Channel m_HandshakeChannelRx;

		// Token: 0x0400357C RID: 13692
		protected Channel m_HandshakeChannelTx;

		// Token: 0x0400357D RID: 13693
		protected uint m_UpdateInterval;

		// Token: 0x0400357E RID: 13694
		protected byte[] defaultKey = new byte[32];

		// Token: 0x0400357F RID: 13695
		protected byte[] sessionKey = new byte[32];

		// Token: 0x04003580 RID: 13696
		protected uint updateCounter;

		// Token: 0x04003581 RID: 13697
		protected uint nonceClient;

		// Token: 0x04003582 RID: 13698
		protected uint sessionId;

		// Token: 0x04003583 RID: 13699
		protected uint handshakeFailed;

		// Token: 0x04003584 RID: 13700
		private Endpoint.FunctionBindingNumberGenerator bindGen;

		// Token: 0x020006DB RID: 1755
		// (Invoke) Token: 0x06002A54 RID: 10836
		public unsafe delegate void FunctionBindingNumberGenerator(uint* data, uint datasize);
	}
}
