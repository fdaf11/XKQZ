using System;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace HeLuo.Framework.Flow
{
	// Token: 0x0200017C RID: 380
	public abstract class ObjectMemberNode<T> : OutputNode<T>
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x000067F3 File Offset: 0x000049F3
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x000067FB File Offset: 0x000049FB
		[JsonIgnore]
		public virtual MemberInfo memberInfo
		{
			get
			{
				return this._memberInfo;
			}
			set
			{
				this._memberInfo = value;
				this.typeName = value.DeclaringType.FullName;
				this.memberName = value.Name;
			}
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00006821 File Offset: 0x00004A21
		internal override void OnBeforeSerialize(NodeGraph graph)
		{
			this.typeName = this.memberInfo.DeclaringType.Name;
			this.memberName = this.memberInfo.Name;
			base.OnBeforeSerialize(graph);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00048EA4 File Offset: 0x000470A4
		internal override void OnAfterDeserialize(NodeGraph graph)
		{
			base.OnAfterDeserialize(graph);
			Type type = Type.GetType(this.typeName) ?? typeof(Vector3).Assembly.GetType(this.typeName);
			if (type != null)
			{
				MemberInfo[] member = type.GetMember(this.memberName);
				if (member.Length > 0)
				{
					this._memberInfo = member[0];
				}
			}
		}

		// Token: 0x040007CB RID: 1995
		[Argument("輸入物件")]
		public IOutput objectNode;

		// Token: 0x040007CC RID: 1996
		public string typeName;

		// Token: 0x040007CD RID: 1997
		public string memberName;

		// Token: 0x040007CE RID: 1998
		[JsonIgnore]
		protected MemberInfo _memberInfo;
	}
}
