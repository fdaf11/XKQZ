using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x02000180 RID: 384
	[Description("內部使用/MethodNode")]
	public class ObjectMethodNode : ObjectMemberNode<bool>
	{
		// Token: 0x06000793 RID: 1939 RVA: 0x00006896 File Offset: 0x00004A96
		public ObjectMethodNode()
		{
			this.getter = typeof(IOutput).GetMethod("GetValue");
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x000068CE File Offset: 0x00004ACE
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x00049294 File Offset: 0x00047494
		[JsonIgnore]
		public override MemberInfo memberInfo
		{
			get
			{
				return base.memberInfo;
			}
			set
			{
				base.memberInfo = value;
				MethodInfo methodInfo = value as MethodInfo;
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (parameters.Length > 0)
				{
					this.paramsTypeName.Clear();
					for (int i = 0; i < parameters.Length; i++)
					{
						this.paramsTypeName.Add(parameters[i].ParameterType.FullName);
					}
				}
				else
				{
					this.paramsTypeName = null;
				}
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00049304 File Offset: 0x00047504
		internal override void OnAfterDeserialize(NodeGraph graph)
		{
			base.OnAfterDeserialize(graph);
			Type type = Type.GetType(this.typeName) ?? typeof(Vector3).Assembly.GetType(this.typeName);
			if (type != null)
			{
				if (this.paramsTypeName != null && this.paramsTypeName.Count != 0)
				{
					List<Type> list = new List<Type>();
					for (int i = 0; i < this.paramsTypeName.Count; i++)
					{
						Type type2 = Type.GetType(this.paramsTypeName[i]) ?? typeof(Vector3).Assembly.GetType(this.paramsTypeName[i]);
						if (type2 == null)
						{
							break;
						}
						list.Add(type2);
					}
					this._memberInfo = type.GetMethod(this.memberName, list.ToArray());
				}
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x000493F0 File Offset: 0x000475F0
		public override bool GetValue()
		{
			object value = this.objectNode.GetValue();
			MethodInfo methodInfo = this.memberInfo as MethodInfo;
			ParameterInfo[] parameters = methodInfo.GetParameters();
			List<object> list = this.objectListNode.ConvertAll<object>((OutputNode item) => this.getter.Invoke(item, null));
			object obj = methodInfo.Invoke(value, list.ToArray());
			if (obj != null)
			{
				if (this.outputNode != null)
				{
					FieldInfo field = this.outputNode.GetType().GetField("value");
					field.SetValue(this.outputNode, obj);
				}
				return true;
			}
			return false;
		}

		// Token: 0x040007D3 RID: 2003
		[Argument("參數")]
		public List<OutputNode> objectListNode = new List<OutputNode>();

		// Token: 0x040007D4 RID: 2004
		[Argument("<回傳值")]
		public OutputNode outputNode;

		// Token: 0x040007D5 RID: 2005
		public List<string> paramsTypeName = new List<string>();

		// Token: 0x040007D6 RID: 2006
		[JsonIgnore]
		private MethodInfo getter;
	}
}
