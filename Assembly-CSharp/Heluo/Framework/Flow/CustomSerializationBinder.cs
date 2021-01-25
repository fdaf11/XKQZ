using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200018B RID: 395
	public class CustomSerializationBinder : SerializationBinder
	{
		// Token: 0x060007EB RID: 2027 RVA: 0x00049E4C File Offset: 0x0004804C
		public override Type BindToType(string assemblyName, string typeName)
		{
			return Type.GetType(typeName) ?? typeof(Vector3).Assembly.GetType(typeName);
		}
	}
}
