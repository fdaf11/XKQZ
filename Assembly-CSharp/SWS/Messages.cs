using System;
using System.Collections.Generic;
using UnityEngine;

namespace SWS
{
	// Token: 0x020006AE RID: 1710
	[Serializable]
	public class Messages
	{
		// Token: 0x06002954 RID: 10580 RVA: 0x0014772C File Offset: 0x0014592C
		public void Initialize(int size)
		{
			for (int i = this.list.Count; i <= size; i++)
			{
				this.list.Add(this.AddEmptyToOption(new MessageOptions()));
			}
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x0014776C File Offset: 0x0014596C
		public MessageOptions AddEmptyToOption(MessageOptions opt)
		{
			opt.message.Add(string.Empty);
			opt.type.Add(MessageOptions.ValueType.None);
			opt.obj.Add(null);
			opt.text.Add(null);
			opt.num.Add(0f);
			opt.vect2.Add(Vector2.zero);
			opt.vect3.Add(Vector3.zero);
			return opt;
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x001477E0 File Offset: 0x001459E0
		public void FillOptionWithValues(MessageOptions opt)
		{
			int count = opt.message.Count;
			if (opt.type.Count < count)
			{
				opt.type.Add(MessageOptions.ValueType.None);
			}
			if (opt.obj.Count < count)
			{
				opt.obj.Add(null);
			}
			if (opt.text.Count < count)
			{
				opt.text.Add(null);
			}
			if (opt.num.Count < count)
			{
				opt.num.Add(0f);
			}
			if (opt.vect2.Count < count)
			{
				opt.vect2.Add(Vector2.zero);
			}
			if (opt.vect3.Count < count)
			{
				opt.vect3.Add(Vector3.zero);
			}
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x001478B4 File Offset: 0x00145AB4
		public MessageOptions GetMessageOption(int waypoint)
		{
			this.Initialize(waypoint);
			return this.list[waypoint];
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x001478D8 File Offset: 0x00145AD8
		public void Execute(MonoBehaviour mono, int index)
		{
			if (this.list == null || this.list.Count - 1 < index || this.list[index].message == null)
			{
				return;
			}
			for (int i = 0; i < this.list[index].message.Count; i++)
			{
				if (!(this.list[index].message[i] == string.Empty))
				{
					MessageOptions messageOptions = this.list[index];
					switch (messageOptions.type[i])
					{
					case MessageOptions.ValueType.None:
						mono.SendMessage(messageOptions.message[i], 1);
						break;
					case MessageOptions.ValueType.Object:
						mono.SendMessage(messageOptions.message[i], messageOptions.obj[i], 1);
						break;
					case MessageOptions.ValueType.Text:
						mono.SendMessage(messageOptions.message[i], messageOptions.text[i], 1);
						break;
					case MessageOptions.ValueType.Numeric:
						mono.SendMessage(messageOptions.message[i], messageOptions.num[i], 1);
						break;
					case MessageOptions.ValueType.Vector2:
						mono.SendMessage(messageOptions.message[i], messageOptions.vect2[i], 1);
						break;
					case MessageOptions.ValueType.Vector3:
						mono.SendMessage(messageOptions.message[i], messageOptions.vect3[i], 1);
						break;
					}
				}
			}
		}

		// Token: 0x0400344F RID: 13391
		public List<MessageOptions> list = new List<MessageOptions>();
	}
}
