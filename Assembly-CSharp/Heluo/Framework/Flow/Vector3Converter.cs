using System;
using Newtonsoft.Json;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000191 RID: 401
	public class Vector3Converter : JsonConverter
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00002B59 File Offset: 0x00000D59
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x00002B59 File Offset: 0x00000D59
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00006E1D File Offset: 0x0000501D
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector3);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0004A050 File Offset: 0x00048250
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Vector3 vector = (Vector3)existingValue;
			reader.Read();
			vector.x = (float)reader.ReadAsDecimal().Value;
			reader.Read();
			vector.y = (float)reader.ReadAsDecimal().Value;
			reader.Read();
			vector.z = (float)reader.ReadAsDecimal().Value;
			reader.Read();
			return vector;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0004A0D4 File Offset: 0x000482D4
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Vector3 vector = (Vector3)value;
			writer.WriteStartObject();
			writer.WritePropertyName("x");
			writer.WriteValue(vector.x);
			writer.WritePropertyName("y");
			writer.WriteValue(vector.y);
			writer.WritePropertyName("z");
			writer.WriteValue(vector.z);
			writer.WriteEndObject();
			writer.Flush();
		}
	}
}
