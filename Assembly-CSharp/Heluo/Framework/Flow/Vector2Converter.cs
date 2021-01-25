using System;
using Newtonsoft.Json;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000190 RID: 400
	public class Vector2Converter : JsonConverter
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x00002B59 File Offset: 0x00000D59
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00002B59 File Offset: 0x00000D59
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00006E0E File Offset: 0x0000500E
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector2);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00049F94 File Offset: 0x00048194
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Vector2 vector = (Vector2)existingValue;
			reader.Read();
			vector.x = (float)reader.ReadAsDecimal().Value;
			reader.Read();
			vector.y = (float)reader.ReadAsDecimal().Value;
			reader.Read();
			return vector;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00049FF8 File Offset: 0x000481F8
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Vector2 vector = (Vector2)value;
			writer.WriteStartObject();
			writer.WritePropertyName("x");
			writer.WriteValue(vector.x);
			writer.WritePropertyName("y");
			writer.WriteValue(vector.y);
			writer.WriteEndObject();
			writer.Flush();
		}
	}
}
